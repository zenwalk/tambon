using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using De.AHoerstemeier.Geo;
using De.AHoerstemeier.Tambon;

namespace De.AHoerstemeier.Tambon
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            GlobalSettings.LoadSettings();
        }

        private void btnGazetteMirror_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(TambonHelper.GlobalGazetteList.MirrorAllToCache);
            t.Name = "Worker Thread Gazette Mirror";
            t.Start();
        }

        private void PopulationDataReadyShow(Object sender, PopulationDataEventArgs e)
        {
            if ( e.PopulationData.Data != null )
            {
                Invoke(new Action(() => ShowPopulationDialog(e.PopulationData)));
            }
        }

        private void PopulationDataReadyCheck(Object sender, PopulationDataEventArgs e)
        {
            if ( e.PopulationData.Data != null )
            {
                e.PopulationData.SaveXml();
                Invoke(new Action(() => CheckPopulationData(e.PopulationData)));
            }
        }

        private void ShowPopulationDialog(PopulationData iData)
        {
            var lDataForm = new PopulationDataView();
            lDataForm.ShowGazette += ShowGazetteDialog;
            lDataForm.Data = iData;
            lDataForm.Show();
        }

        private void CheckPopulationData(PopulationData data)
        {
            var entitiesWithoutGeocode = data.EntitiesWithoutGeocode();
            if ( entitiesWithoutGeocode.Count > 0 )
            {
                StringBuilder builder = new StringBuilder();

                foreach ( Tuple<PopulationDataEntry, Int32> entry in entitiesWithoutGeocode )
                {
                    builder.AppendLine(String.Format("{0} ({1}, Parent {2}, {3} people in {4} households)", entry.Item1.Name, entry.Item1.Type, entry.Item2, entry.Item1.Total, entry.Item1.Households));
                }

                MessageBox.Show(entitiesWithoutGeocode.Count.ToString() + " entities without geocode in " + data.Year.ToString() + Environment.NewLine + builder.ToString());
            }
            var entitiesWithInvalidGeocode = data.EntitiesWithInvalidGeocode();
            if ( entitiesWithInvalidGeocode.Count > 0 )
            {
                String message = "";

                foreach ( PopulationDataEntry lEntry in entitiesWithInvalidGeocode )
                {
                    message = message + lEntry.Geocode.ToString() + ' ' + lEntry.Name + Environment.NewLine;
                }

                MessageBox.Show(entitiesWithInvalidGeocode.Count.ToString() + " entities with invalid geocode in " + data.Year.ToString() + Environment.NewLine + message);
            }
        }

        private void btnPopulation_Click(object sender, EventArgs e)
        {
            PopulationData downloader = new PopulationData(Convert.ToInt32(edtYear.Value), GetCurrentChangwat().Geocode);
            Thread downloadThread = new Thread(downloader.Process);

            downloader.ProcessingFinished += PopulationDataReadyCheck;
            downloader.ProcessingFinished += PopulationDataReadyShow;

            downloadThread.Name = "Worker Thread Population " + GetCurrentChangwat().English + " " + downloader.Year.ToString();
            downloadThread.Start();
        }

        private void FillChangwatCombobox(ComboBox cbx_changwat)
        {
            TambonHelper.LoadGeocodeList();

            cbx_changwat.Items.Clear();
            foreach ( PopulationDataEntry lEntry in TambonHelper.ProvinceGeocodes )
            {
                cbx_changwat.Items.Add(lEntry);
                if ( lEntry.Geocode == TambonHelper.PreferredProvinceGeocode )
                {
                    cbx_changwat.SelectedItem = lEntry;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FillChangwatCombobox(cbx_changwat);
            edtYear.Maximum = TambonHelper.PopulationStatisticMaxYear;
            edtYear.Minimum = TambonHelper.PopulationStatisticMinYear;
            edtYear.Value = edtYear.Maximum;
        }

        private void btnPopulationDownloadAll_click(object sender, EventArgs e)
        {
            var lChangwat = (PopulationDataEntry)cbx_changwat.SelectedItem;
            Int32 lGeocode = lChangwat.Geocode;
            for ( int lYear = TambonHelper.PopulationStatisticMinYear ; lYear <= TambonHelper.PopulationStatisticMaxYear ; lYear++ )
            {
                PopulationData downloader = new PopulationData(lYear, lGeocode);
                downloader.ProcessingFinished += PopulationDataReadyCheck;
                Thread t = new Thread(downloader.Process);
                t.Name = "Worker Thread Population " + lChangwat.English + " " + downloader.Year.ToString();
                t.Start();
            }
        }

        private PopulationDataEntry GetCurrentChangwat()
        {
            var lChangwat = (PopulationDataEntry)cbx_changwat.SelectedItem;
            return lChangwat;
        }

        private void btn_GazetteShow_Click(object sender, EventArgs e)
        {
            RoyalGazetteList lGazetteList = TambonHelper.GlobalGazetteList.AllAboutEntity(GetCurrentChangwat().Geocode, true);
            RoyalGazetteViewer.ShowGazetteDialog(lGazetteList, false);
        }

        private void btn_GazetteLoad_Click(object sender, EventArgs e)
        {
            // DirectoryInfo lDirInfo = new DirectoryInfo("e:\\thailand\\dopa\\tambon\\gazette\\");

            string lDir = Path.Combine(Application.StartupPath, "gazette");
            if ( Directory.Exists(lDir) )
            {
                foreach ( string lFilename in Directory.GetFiles(lDir, "Gazette*.XML") )
                {
                    RoyalGazetteList lCurrentGazetteList = RoyalGazetteList.Load(lFilename);
                    TambonHelper.GlobalGazetteList.AddRange(lCurrentGazetteList);
                }
                Boolean lHasEntries = (TambonHelper.GlobalGazetteList.Count > 0);
                btn_GazetteShow.Enabled = lHasEntries;
                btn_GazetteShowAll.Enabled = lHasEntries;
            }
            else
            {
                MessageBox.Show(this, "Fatal error: Directory " + lDir + " is not existing." + Environment.NewLine + "Application will be terminated.", "Directory error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private void cbx_changwat_SelectedValueChanged(object sender, EventArgs e)
        {
            Int32 lGeocode = GetCurrentChangwat().Geocode;
            btn_Population.Enabled = lGeocode != 0;
            btn_PopulationAll.Enabled = lGeocode != 0;
        }

        private void GazetteNewsReady(Object sender, RoyalGazetteEventArgs e)
        {
            GazetteNewsReady(e.Data);
        }

        private void GazetteNewsReady(RoyalGazetteList data)
        {
            Invoke(new Action(() => RoyalGazetteViewer.ShowGazetteNewsDialog(data)));
        }

        private void ShowGazetteDialog(Object sender, RoyalGazetteEventArgs e)
        {
            ShowGazetteDialog(e.Data);
        }

        private void ShowGazetteDialog(RoyalGazetteList data)
        {
            Invoke(new Action(() => RoyalGazetteViewer.ShowGazetteDialog(data, true)));
        }

        private void btn_CheckForNews_Click(Object sender, EventArgs e)
        {
            RoyalGazetteOnlineSearch searcher = new RoyalGazetteOnlineSearch();
            searcher.ProcessingFinished += GazetteNewsReady;
            Thread t = new Thread(searcher.SearchNewsNow);
            t.Name = "Worker Thread Gazette News";
            t.Start();
        }

        private void btn_GazetteShowAll_Click(Object sender, EventArgs e)
        {
            ShowGazetteDialog(TambonHelper.GlobalGazetteList);
        }

        private void btn_GazetteSearchYear_Click(Object sender, EventArgs e)
        {
            RoyalGazetteOnlineSearch searcher = new RoyalGazetteOnlineSearch();
            var gazetteList = searcher.SearchNews(new DateTime((Int32)edtYear.Value, 1, 1));
            GazetteNewsReady(gazetteList);
        }

        private void btn_LoadCcaatt_Click(Object sender, EventArgs e)
        {
            // ToDo: Konfigurierbares Verzeichnis für ccaatt files
            openFileDialogCCAATT.InitialDirectory = @"e:\Thailand\geocode\";
            openFileDialogCCAATT.FileName = "ccaatt.txt";
            openFileDialogCCAATT.ShowDialog();
        }

        private void ccaattPostProcessing(DopaGeocodeList data)
        {
            data.RemoveAllKnownGeocodes();
            data.ExportToXML(Path.Combine(GlobalSettings.XMLOutputDir, "unknowngeocodes.xml"));
            var form = new StringDisplayForm("New Geocodes", data.ToString());
            form.Show();
        }

        private void openFileDialogCCAATT_FileOk(Object sender, CancelEventArgs e)
        {
            this.Activate();
            String[] files = openFileDialogCCAATT.FileNames;
            foreach ( String fileName in files )
            {
                var data = new DopaGeocodeList(fileName);
                ccaattPostProcessing(data);
            }
        }

        private void openFileDialogXML_FileOk(Object sender, CancelEventArgs e)
        {
            this.Activate();
            String[] files = openFileDialogXML.FileNames;
            RoyalGazetteList currentGazetteList = new RoyalGazetteList();
            foreach ( String fileName in files )
            {
                RoyalGazetteList loadedGazetteList = RoyalGazetteList.Load(fileName);
                currentGazetteList.AddRange(loadedGazetteList);
            }
            ShowGazetteDialog(currentGazetteList);
        }

        private void btn_DownloadCcaatt_Click(Object sender, EventArgs e)
        {
            var data = DopaGeocodeList.CreateFromOnline();
            ccaattPostProcessing(data);
        }

        private void btn_GazetteSearch_Click(Object sender, EventArgs e)
        {
            RoyalGazetteSearch searchWindow = new RoyalGazetteSearch();
            searchWindow.SearchFinished += ShowGazetteDialog;
            searchWindow.Show();
        }

        private void button1_Click(Object sender, EventArgs e)
        {
            AmphoeComDownloader downloader = new AmphoeComDownloader();
            Int32 geocode = GetCurrentChangwat().Geocode;
            var data = downloader.DoItAll(geocode);
            XmlDocument xmlDocument = new XmlDocument();
            var element = (XmlElement)xmlDocument.CreateNode("element", "mirror", "");
            element.SetAttribute("date", DateTime.Now.ToString("yyyy-MM-dd"));
            xmlDocument.AppendChild(element);
            foreach ( var dataEntry in data )
            {
                dataEntry.ExportToXML(element);
            }
            Directory.CreateDirectory(OutputDir());
            xmlDocument.Save(Path.Combine(OutputDir(), "amphoe" + geocode.ToString() + ".xml"));
        }

        private String OutputDir()
        {
            String retval = Path.Combine(GlobalSettings.XMLOutputDir, "amphoe");
            return retval;
        }

        private void btnSetup_Click(Object sender, EventArgs e)
        {
            Setup setupForm = new Setup();
            setupForm.ShowDialog();
        }

        private void btnNumerals_Click(Object sender, EventArgs e)
        {
            NumeralsTambonHelper numeralsTambonHelperForm = new NumeralsTambonHelper();
            numeralsTambonHelperForm.Show();
        }

        private void btnTambonFrequency_Click(Object sender, EventArgs e)
        {
            List<EntityType> types = new List<EntityType>()
            {
                EntityType.Tambon
            };
            EntityCounter counter = new EntityCounter(types);
            var lChangwat = (PopulationDataEntry)cbx_changwat.SelectedItem;
            // lCounter.BaseGeocode = lChangwat.Geocode;
            counter.Calculate();

            var form = new StringDisplayForm("Tambon", counter.CommonNames(20));
            form.Show();
        }

        private void btnTambonCreation_Click(object sender, EventArgs e)
        {
            CreationStatisticForm lForm = new CreationStatisticForm();
            lForm.Show();
        }

        private void btn_LoadGazetteXML_Click(object sender, EventArgs e)
        {
            openFileDialogXML.InitialDirectory = Path.Combine(Application.StartupPath, "gazette");
            openFileDialogXML.ShowDialog();
        }

        private void btnAreaUnits_Click(object sender, EventArgs e)
        {
            UnitConverterForm lForm = new UnitConverterForm();
            lForm.Show();
        }

        private void btnGovernor_Click(object sender, EventArgs e)
        {
            ProvinceGovernorParser lParser = new ProvinceGovernorParser();
            lParser.ParseUrl("http://www.moi.go.th/portal/page?_pageid=33,76197,33_76230&_dad=portal&_schema=PORTAL");
            // lParser.ParseFile("C:\\Users\\Andy\\Dropbox\\My Dropbox\\Misc\\governor list 2008.htm");

            XmlDocument lXmlDocument = new XmlDocument();
            lParser.ExportToXML(lXmlDocument);
            Directory.CreateDirectory(GlobalSettings.XMLOutputDir);
            lXmlDocument.Save(Path.Combine(GlobalSettings.XMLOutputDir, "governor.xml"));

            var lNewGovernors = lParser.NewGovernorsList();
            lXmlDocument = new XmlDocument();
            lParser.ExportToXML(lXmlDocument);
            lXmlDocument.Save(Path.Combine(GlobalSettings.XMLOutputDir, "newgovernor.xml"));

            var lForm = new StringDisplayForm("New governors", lParser.NewGovernorsText());
            lForm.Show();
        }

        private void btnBoard_Click(object sender, EventArgs e)
        {
            String lFilename = Path.GetDirectoryName(Application.ExecutablePath) + "\\misc\\board.xml";
            BoardMeetingList lBoardMeetingList = BoardMeetingList.Load(lFilename);
            if ( lBoardMeetingList != null )
            {
                FrequencyCounter lCounter = lBoardMeetingList.EffectiveDateTillPublication();
                lCounter = lBoardMeetingList.MeetingDateTillPublication();

                String lResult = "Mean time " + lCounter.MeanValue.ToString("##0.0") + Environment.NewLine;
                lResult = lResult + "Max. time " + lCounter.MaxValue.ToString() + Environment.NewLine;
                lResult = lResult + "Min. time " + lCounter.MinValue.ToString() + Environment.NewLine;
                FrequencyCounter lCounterMissing = lBoardMeetingList.MissingConstituencyAnnouncements();
                lResult = lResult + "No constituency: " + lCounterMissing.NumberOfValues.ToString() + Environment.NewLine + Environment.NewLine;

                var lSorted = new List<Int32>();
                foreach ( Int32 lKey in lCounter.Data.Keys )
                {
                    lSorted.Add(lKey);
                }
                lSorted.Sort(delegate(Int32 p1, Int32 p2)
                {
                    return (p2.CompareTo(p1));
                });

                foreach ( int lEntry in lSorted )
                {
                    String lLine = lEntry.ToString() + ": ";
                    foreach ( int lGeocode in lCounter.Data[lEntry] )
                    {
                        lLine = lLine + lGeocode.ToString() + ',';
                    }
                    lLine = lLine.Remove(lLine.Length - 1);
                    lResult = lResult + lLine + Environment.NewLine;
                }

                var lForm = new StringDisplayForm("Board meeting to constituency", lResult);
                lForm.Show();
            }
        }

        private void btnMuban_Click(object sender, EventArgs e)
        {
            Int32 geocode = GetCurrentChangwat().Geocode;
            var mubanCsvReader = new MubanCSVReader();
            var data = mubanCsvReader.Parse(geocode);

            if ( data != null )
            {
                var form = new StringDisplayForm("Muban", mubanCsvReader.Information(data));

                String outFilename = Path.Combine(GlobalSettings.XMLOutputDir, "Muban" + geocode.ToString() + ".kml");
                data.ExportToKml(outFilename);
                form.Show();
            }
        }

        private void btnMubanNames_Click(object sender, EventArgs e)
        {
            List<EntityType> types = new List<EntityType>()
            {
                EntityType.Muban
            };
            EntityCounter namesCounter = new EntityCounter(types);
            if ( chkUseCsv.Checked )
            {
                var entityList = new List<PopulationDataEntry>();
                var counter = new FrequencyCounter();
                foreach ( PopulationDataEntry entity in TambonHelper.ProvinceGeocodes )
                {
                    if ( entity.Geocode != 10 )
                    {
                        var reader = new MubanCSVReader();
                        var data = reader.Parse(entity.Geocode);
                        if ( data != null )
                        {
                            MubanCSVReader.Statistics(data, counter);
                            var flatData = data.FlatList(types);
                            entityList.AddRange(flatData);
                        }
                    }
                }
                var formStatistics = new StringDisplayForm("Muban", MubanCSVReader.StatisticsText(counter));
                formStatistics.Show();
                namesCounter.Calculate(entityList);
            }
            else
            {
                namesCounter.Calculate();
            }
            var formNames = new StringDisplayForm("Muban", namesCounter.CommonNames(20));
            formNames.Show();
        }

        private void btnCreateKml_Click(object sender, EventArgs e)
        {
            var allEntities = new List<PopulationDataEntry>();
            foreach ( PopulationDataEntry provinceGeocode in TambonHelper.ProvinceGeocodes )
            {
                PopulationData provinceEntities = TambonHelper.GetGeocodeList(provinceGeocode.Geocode);
                allEntities.Add(provinceEntities.Data);
            }
            PopulationDataEntry master = new PopulationDataEntry();
            master.SubEntities.AddRange(allEntities);
            String outFilename = Path.Combine(GlobalSettings.XMLOutputDir, "offices.kml");
            master.ExportToKml(outFilename);

            var geotaggedOffices = new Dictionary<OfficeType, Int32>();
            var anyOffices = new Dictionary<OfficeType, Int32>();
            var flatList = master.FlatList(EntityTypeHelper.AllEntityTypes);
            foreach ( var entity in flatList )
            {
                foreach ( var office in entity.Offices )
                {
                    if ( office.Location != null )
                    {
                        if ( !geotaggedOffices.ContainsKey(office.Type) )
                        {
                            geotaggedOffices.Add(office.Type, 0);
                        }
                        geotaggedOffices[office.Type]++;
                    }
                    if ( !anyOffices.ContainsKey(office.Type) )
                    {
                        anyOffices.Add(office.Type, 0);
                    }
                    anyOffices[office.Type]++;
                }
            }
            String officeTypeInfo = String.Empty;
            foreach ( OfficeType officeType in System.Enum.GetValues(typeof(OfficeType)) )
            {
                if ( geotaggedOffices.ContainsKey(officeType) )
                {
                    officeTypeInfo =
                        officeTypeInfo +
                        String.Format("{0} {1} (of {2})", officeType, geotaggedOffices[officeType], anyOffices[officeType])
                        + Environment.NewLine;
                }
            }

            var provincesWithoutPAOLocation = flatList.FindAll(x => x.Offices.Any(y => (y.Type == OfficeType.PAOOffice) && (y.Location == null)));
            String provincesWithoutPAOLocationInfo = String.Format("No PAO location ({0}): ", provincesWithoutPAOLocation.Count());
            foreach ( var entity in provincesWithoutPAOLocation )
            {
                provincesWithoutPAOLocationInfo = provincesWithoutPAOLocationInfo + String.Format("{0} ({1}),", entity.English, entity.Geocode);
            }
            var districtsWithoutOffice = flatList.FindAll(x => x.Offices.Any(y => (y.Type == OfficeType.DistrictOffice) && (y.Location == null)));
            String districtsWithoutOfficeInfo = String.Format("No district office location ({0}): ", districtsWithoutOffice.Count());
            foreach ( var entity in districtsWithoutOffice )
            {
                districtsWithoutOfficeInfo = districtsWithoutOfficeInfo + String.Format("{0} ({1}),", entity.English, entity.Geocode);
            }

            var info = officeTypeInfo + Environment.NewLine + provincesWithoutPAOLocationInfo + Environment.NewLine + districtsWithoutOfficeInfo;

            var form = new StringDisplayForm("Office types", info);
            form.Show();
        }

        private void btnGeo_Click(object sender, EventArgs e)
        {
            GeoCoordinateForm lGeoForm = new GeoCoordinateForm();
            lGeoForm.Show();
        }

        private void btnThesaban_Click(object sender, EventArgs e)
        {
            Int32 lGeocode = GetCurrentChangwat().Geocode;
            ConstituencyChecker lChecker = new ConstituencyChecker(lGeocode);
            String lResult = String.Empty;
            foreach ( PopulationDataEntry lEntry in lChecker.ThesabanWithoutConstituencies() )
            {
                lResult = lResult + lEntry.Geocode.ToString() + " " + lEntry.English + Environment.NewLine;
            }
            var lForm = new StringDisplayForm("Thesaban without constituency announcement", lResult);
            lForm.Show();
        }

        private void btnMgrsGrid_Click(object sender, EventArgs e)
        {
            List<String> aachenHectards = new List<String>() {
                "31UGS05", "31UGS15", "31UGS06", "31UGS16",
                "32UKB86", "32UKB85", "32UKB95", "32UKB94", "32ULB04",
                "32ULB03"
            };
            List<String> thailandMyriads = new List<string>() {
                "47PLK", "47PLL",
                "47PMK", "47PML", "47PMM",                            "47PMR", "47PMS", "47PMT",
                "47PNK", "47PNL", "47PNM", "47PNN", "47PNP", "47PNQ", "47PNR", "47PNS", "47PNT",
                "47PPK", "47PPL",                   "47PPP", "47PPQ", "47PPR", "47PPS", "47PPT",
                                                    "47PQP", "47PQQ", "47PQR", "47PQS", "47PQT",
                                                    "47PRP", "47PRQ", "47PRR", "47PRS", "47PRT",

                                  "47NMJ",
                         "47NNH", "47NNJ",
                         "47NPH", "47NPJ",
                "47NQG", "47NQH",
                "47NRG", "47NRH",
                                  "47QLV", "47QLA", "47QLB",
                "47QMU", "47QMV", "47QMA", "47QMB",
                "47QNU", "47QNV", "47QNA", "47QNB", "47QNC",
                "47QPU", "47QPV", "47QPA", "47QPB", "47QPC",
                "47QQU", "47QQV", "47QQA", "47QQB",
                "47QRU", "47QRV", "47QRA",

                "48NSM",
                         "48PSU", "48PSV", "48PSA", "48PSB", "48PSC", "48QSD", "48QSE", "48QSF",
                "48PTT", "48PTU", "48PTV", "48PTA", "48PTB", "48PTC", "48QTD", "48QTE", "48QTF",
                                           "48PUA", "48PUB", "48PUC", "48QUD", "48QUE", "48QUF",
                                           "48PVA", "48PVB", "48PVC", "48QVD", "48QVE", "48QVF",
                                           "48PWA", "48PWB", "48PWC"
            };

            String outFilename = Path.Combine(GlobalSettings.XMLOutputDir, "mgrs.kml");
            KmlHelper kmlWriter = MgrsGridElement.StartKmlWriting();
            // foreach ( String lEntry in thailandMyriads )
            foreach ( String mgrsGridElement in aachenHectards )
            {
                MgrsGridElement mainGrid = new MgrsGridElement(mgrsGridElement);
                // lGrid.Datum = GeoDatum.DatumIndian1975();
                XmlNode kmlNode = kmlWriter.AddFolder(kmlWriter.DocumentNode, mainGrid.Name, false);
                //lGrid.WriteToKml(lKmlWriter, lNode);
                foreach ( var subGrid in mainGrid.SubGrids() )
                {
                    subGrid.WriteToKml(kmlWriter, kmlNode, subGrid.Name);
                }
            }

            //MgrsGridElement lElement = new MgrsGridElement("47PNL31");
            //lElement.Datum = GeoDatum.DatumIndian1975();
            //List<MgrsGridElement> lList = lElement.SubGrids();
            //foreach (MgrsGridElement lEntry in lList)
            //{
            //    XmlNode lNode = lKmlWriter.AddFolder(lKmlWriter.DocumentNode, lEntry.Name, false);
            //    lEntry.WriteToKml(lKmlWriter, lNode);
            //}
            kmlWriter.SaveToFile(outFilename);
        }

        private void btnConstituency_Click(object sender, EventArgs e)
        {
            ConstituencyForm lForm = new ConstituencyForm();
            lForm.Show();
        }

        private void btn_PopulationAllProvinces_Click(object sender, EventArgs e)
        {
            Int32 year = Convert.ToInt32(edtYear.Value);
            PopulationDataEntry country = TambonHelper.GetCountryPopulationData(year);
            // CheckPopulationData(downloader);
            var dataForm = new PopulationByEntityTypeViewer();
            dataForm.Country = country;
            dataForm.Show();
        }

        private void btn_dopaamphoe_Click(object sender, EventArgs e)
        {
            // Load http://amphoe.dopa.go.th/Shop/frontshowroom/###/aboutus (1..999)
            // in returned document find everything inside these div tag (cut away line breaks and spaces)
            // <div class="aname">อำเภอยางชุมน้อย</div>
            // <div class="adistinct">จังหวัดศรีสะเกษ</div>
            // <div class="blue">Name of Nai Amphoe</div>
            // <div class="adef dm-260 p-20 fl al-r">District slogan</div>
        }

        private void btnCheckNames_Click(object sender, EventArgs e)
        {
            PopulationDataEntry masterDataEntry = new PopulationDataEntry();
            Dictionary<String, String> romanizations = new Dictionary<String, String>();
            var romanizationMistakes = new List<Tuple<Int32, String, String, String>>();
            var romanizationSuggestions = new List<Tuple<Int32, String, String>>();
            var romanizationMissing = new List<Tuple<Int32, String>>();

            var provinceList = new List<PopulationDataEntry>();
            foreach ( PopulationDataEntry province in TambonHelper.ProvinceGeocodes )
            {
                PopulationData entities = TambonHelper.GetGeocodeList(province.Geocode);
                provinceList.Add(entities.Data);
            }
            masterDataEntry.SubEntities.AddRange(provinceList);
            foreach ( var entityToCheck in masterDataEntry.FlatList(EntityTypeHelper.AllEntityTypes) )
            {
                if ( !String.IsNullOrEmpty(entityToCheck.English) )
                {
                    String name = entityToCheck.Name;
                    String english = entityToCheck.English;
                    if ( (entityToCheck.Type == EntityType.Muban) | (entityToCheck.Type == EntityType.Chumchon) )
                    {
                        name = TambonHelper.StripBanOrChumchon(name);

                        if ( english.StartsWith("Ban ") )
                        {
                            english = english.Remove(0, "Ban ".Length).Trim();
                        }
                        if ( english.StartsWith("Chumchon ") )
                        {
                            english = english.Remove(0, "Chumchon ".Length).Trim();
                        }

                        if ( entityToCheck.Type == EntityType.Chumchon )
                        {
                            // Chumchon may have the name "Chumchon Ban ..."
                            name = TambonHelper.StripBanOrChumchon(name);

                            if ( english.StartsWith("Ban ") )
                            {
                                english = english.Remove(0, "Ban ".Length).Trim();
                            }
                        }
                    }
                    if ( romanizations.Keys.Contains(name) )
                    {
                        if ( romanizations[name] != english )
                        {
                            romanizationMistakes.Add(Tuple.Create(entityToCheck.Geocode, name, english, romanizations[name]));
                        }
                    }
                    else
                    {
                        romanizations[name] = english;
                    }
                }
            }

            foreach ( var entityToCheck in masterDataEntry.FlatList(EntityTypeHelper.AllEntityTypes) )
            {
                if ( String.IsNullOrEmpty(entityToCheck.English) )
                {
                    String foundEnglishName = String.Empty;
                    if ( romanizations.Keys.Contains(entityToCheck.Name) )
                    {
                        foundEnglishName = entityToCheck.Name;
                    }
                    else
                    {
                        var searchName = TambonHelper.StripBanOrChumchon(entityToCheck.Name);

                        if ( romanizations.Keys.Contains(searchName) )
                        {
                            foundEnglishName = searchName;
                        }
                        else
                        {
                            // Chumchon may have the name "Chumchon Ban ..."
                            searchName = TambonHelper.StripBanOrChumchon(searchName);
                            if ( romanizations.Keys.Contains(searchName) )
                            {
                                foundEnglishName = searchName;
                            }
                        }
                    }

                    if ( !String.IsNullOrEmpty(foundEnglishName) )
                    {
                        romanizationSuggestions.Add(Tuple.Create(entityToCheck.Geocode, entityToCheck.Name, romanizations[foundEnglishName]));
                    }
                    else
                    {
                        Boolean found = false;
                        String name = TambonHelper.StripBanOrChumchon(entityToCheck.Name);
                        name = TambonHelper.ReplaceThaiNumerals(name);
                        List<Char> numerals = new List<Char>() { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
                        foreach ( Char c in numerals )
                        {
                            name = name.Replace(c, ' ');
                        }
                        name = name.Trim();
                        foreach ( var keyValuePair in TambonHelper.NameSuffixRomanizations )
                        {
                            if ( entityToCheck.Name.EndsWith(keyValuePair.Key) )
                            {
                                String searchString = TambonHelper.StripBanOrChumchon(name.Substring(0, name.Length - keyValuePair.Key.Length));
                                if ( String.IsNullOrEmpty(searchString) )
                                {
                                    romanizationSuggestions.Add(Tuple.Create(entityToCheck.Geocode, entityToCheck.Name, keyValuePair.Value));
                                    found = true;
                                }
                                else if ( romanizations.Keys.Contains(searchString) )
                                {
                                    romanizationSuggestions.Add(Tuple.Create(entityToCheck.Geocode, entityToCheck.Name, romanizations[searchString] + " " + keyValuePair.Value));
                                    found = true;
                                }
                            }
                        }
                        if ( !found )
                        {
                            var prefixes = TambonHelper.NamePrefixRomanizations.Union(TambonHelper.NameSuffixRomanizations);
                            foreach ( var keyValuePair in prefixes )
                            {
                                if ( name.StartsWith(keyValuePair.Key) )
                                {
                                    String searchString = name.Substring(keyValuePair.Key.Length);
                                    if ( String.IsNullOrEmpty(searchString) )
                                    {
                                        romanizationSuggestions.Add(Tuple.Create(entityToCheck.Geocode, entityToCheck.Name, keyValuePair.Value));
                                        found = true;
                                    }
                                    else if ( romanizations.Keys.Contains(searchString) )
                                    {
                                        romanizationSuggestions.Add(Tuple.Create(entityToCheck.Geocode, entityToCheck.Name, keyValuePair.Value + " " + romanizations[searchString]));
                                        found = true;
                                    }
                                }
                            }
                        }
                        if ( !found )
                        {
                            romanizationMissing.Add(Tuple.Create(entityToCheck.Geocode, entityToCheck.Name));
                        }
                    }
                }
            }

            Int32 provinceFilter = 0;
            if ( cbxCheckNamesFiltered.Checked )
            {
                provinceFilter = GetCurrentChangwat().Geocode;
            }

            StringBuilder romanizationMistakesBuilder = new StringBuilder();
            Int32 romanizationMistakeCount = 0;
            foreach ( var entry in romanizationMistakes )
            {
                if ( TambonHelper.IsBaseGeocode(provinceFilter, entry.Item1) )
                {
                    romanizationMistakesBuilder.AppendLine(String.Format("{0} {1}: {2} vs. {3}", entry.Item1, entry.Item2, entry.Item3, entry.Item4));
                    romanizationMistakeCount++;
                }
            }

            if ( romanizationMistakeCount > 0 )
            {
                var lForm = new StringDisplayForm("Romanization problems (" + romanizationMistakeCount.ToString() + ")", romanizationMistakesBuilder.ToString());
                lForm.Show();
            }

            StringBuilder romanizationSuggestionBuilder = new StringBuilder();
            Int32 romanizationSuggestionCount = 0;
            foreach ( var entry in romanizationSuggestions )
            {
                if ( TambonHelper.IsBaseGeocode(provinceFilter, entry.Item1) )
                {
                    romanizationSuggestionBuilder.AppendLine(String.Format("{0} {1}: {2}", entry.Item1, entry.Item2, entry.Item3));
                    romanizationSuggestionCount++;
                }
            }
            if ( romanizationSuggestionCount > 0 )
            {
                var lForm = new StringDisplayForm("Romanization suggestions (" + romanizationSuggestionCount.ToString() + ")", romanizationSuggestionBuilder.ToString());
                lForm.Show();

                List<Tuple<String, String, Int32>> counter = new List<Tuple<String, String, Int32>>();
                foreach ( var entry in romanizationSuggestions )
                {
                    var found = counter.FirstOrDefault(x => x.Item1 == entry.Item2);
                    if ( found == null )
                    {
                        counter.Add(Tuple.Create(entry.Item2, entry.Item3, 1));
                    }
                    else
                    {
                        counter.Remove(found);
                        counter.Add(Tuple.Create(entry.Item2, entry.Item3, found.Item3 + 1));
                    }
                }
                counter.RemoveAll(x => x.Item3 < 2);
                if ( counter.Any() )
                {
                    counter.Sort(delegate(Tuple<String, String, Int32> p1, Tuple<String, String, Int32> p2)
                    {
                        return (p2.Item3.CompareTo(p1.Item3));
                    });

                    Int32 suggestionCounter = 0;
                    StringBuilder sortedBuilder = new StringBuilder();
                    foreach ( var entry in counter )
                    {
                        sortedBuilder.AppendLine(String.Format("{0}: {1} ({2})", entry.Item1, entry.Item2, entry.Item3));
                        suggestionCounter += entry.Item3;
                    }
                    var lForm2 = new StringDisplayForm(
                        String.Format("Romanization suggestions ({0} of {1})", suggestionCounter, romanizationSuggestionCount),
                        sortedBuilder.ToString());
                    lForm2.Show();
                }
            }

            if ( romanizationMissing.Any() )
            {
                List<Tuple<String, Int32>> counter = new List<Tuple<String, Int32>>();
                foreach ( var entry in romanizationMissing )
                {
                    var found = counter.FirstOrDefault(x => x.Item1 == entry.Item2);
                    if ( found == null )
                    {
                        counter.Add(Tuple.Create(entry.Item2, 1));
                    }
                    else
                    {
                        counter.Remove(found);
                        counter.Add(Tuple.Create(entry.Item2, found.Item2 + 1));
                    }
                }
                // counter.RemoveAll(x => x.Item2 < 2);
                if ( counter.Any() )
                {
                    counter.Sort(delegate(Tuple<String, Int32> p1, Tuple<String, Int32> p2)
                    {
                        var result = p2.Item2.CompareTo(p1.Item2);
                        if ( result == 0 )
                        {
                            result = p2.Item1.CompareTo(p1.Item1);
                        }
                        return result;
                    });

                    Int32 missingCounter = 0;
                    StringBuilder sortedBuilder = new StringBuilder();
                    foreach ( var entry in counter )
                    {
                        sortedBuilder.AppendLine(String.Format("{0}: {1}", entry.Item1, entry.Item2));
                        missingCounter += entry.Item2;
                    }
                    var lForm2 = new StringDisplayForm(
                        String.Format("Romanization missing ({0} of {1})", missingCounter, romanizationMissing.Count),
                        sortedBuilder.ToString());
                    lForm2.Show();
                }
            }
        }

        private void btn_L7018_Click(object sender, EventArgs e)
        {
            String outFilename = Path.Combine(GlobalSettings.XMLOutputDir, "l7018.kml");
            KmlHelper kmlWriter = MgrsGridElement.StartKmlWriting();
            XmlNode kmlNode = kmlWriter.AddFolder(kmlWriter.DocumentNode, "L7018", false);
            RtsdMapIndex.CalcIndexList();
            foreach ( var entry in RtsdMapIndex.MapIndexL7018 )
            {
                entry.WriteToKml(kmlWriter, kmlNode, entry.Name);
            }
            kmlWriter.SaveToFile(outFilename);

            String nonStandardSheetInfo = String.Empty;
            foreach ( var index in RtsdMapIndex.NonStandardSheets )
            {
                var sheet = RtsdMapIndex.MapIndexL7018.FirstOrDefault(x => x.Index.Equals(index));
                if ( sheet != null )
                {
                    nonStandardSheetInfo += sheet.Name + " " +
                        sheet.NorthWestCorner.ToString("%D° %M' %S'' %H") + " - " +
                        sheet.SouthEastCorner.ToString("%D° %M' %S'' %H") + Environment.NewLine;
                }
            }
            var stringDisplayForm = new StringDisplayForm("L7018 Non-Standard sheets", nonStandardSheetInfo);
            stringDisplayForm.Show();
        }

        private void cbxCheckNamesFiltered_CheckedChanged(object sender, EventArgs e)
        {
        }
    }
}
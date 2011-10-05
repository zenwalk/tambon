using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using De.AHoerstemeier.Tambon;
using De.AHoerstemeier.Geo;

namespace De.Ahoerstemeier.Tambon
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
        private void PopulationDataReadyShow(PopulationData iData)
        {
            if ( iData.Data != null )
            {
                Invoke(new PopulationData.ProcessingFinished(ShowPopulationDialog), new object[] { iData });
            }
        }
        private void PopulationDataReadyCheck(PopulationData iData)
        {
            if ( iData.Data != null )
            {
                Invoke(new PopulationData.ProcessingFinished(CheckPopulationData), new object[] { iData });
            }
        }
        private void ShowPopulationDialog(PopulationData iData)
        {
            var lDataForm = new PopulationDataView();
            lDataForm.OnShowGazette += ShowGazetteDialog;
            lDataForm.Data = iData;
            lDataForm.Show();
        }
        private void CheckPopulationData(PopulationData iData)
        {
            iData.SaveXML();
            var lEntitiesWithoutGeocode = iData.EntitiesWithoutGeocode();
            if ( lEntitiesWithoutGeocode.Count > 0 )
            {
                String lMessage = "";

                foreach ( PopulationDataEntry lEntry in lEntitiesWithoutGeocode )
                {
                    lMessage = lMessage + lEntry.Name + Environment.NewLine;
                }

                MessageBox.Show(lEntitiesWithoutGeocode.Count.ToString() + " entities without geocode in " + iData.year.ToString() + Environment.NewLine + lMessage);
            }
            var lEntitiesWithInvalidGeocode = iData.EntitiesWithInvalidGeocode();
            if ( lEntitiesWithInvalidGeocode.Count > 0 )
            {
                String lMessage = "";

                foreach ( PopulationDataEntry lEntry in lEntitiesWithInvalidGeocode )
                {
                    lMessage = lMessage + lEntry.Geocode.ToString() + ' ' + lEntry.Name + Environment.NewLine;
                }

                MessageBox.Show(lEntitiesWithInvalidGeocode.Count.ToString() + " entities with invalid geocode in " + iData.year.ToString() + Environment.NewLine + lMessage);
            }

        }
        private void btnPopulation_Click(object sender, EventArgs e)
        {
            PopulationData lDownloader = new PopulationData(Convert.ToInt32(edt_year.Value), GetCurrentChangwat().Geocode);
            Thread lThread = new Thread(lDownloader.Process);

            lDownloader.OnProcessingFinished += PopulationDataReadyCheck;
            lDownloader.OnProcessingFinished += PopulationDataReadyShow;

            lThread.Name = "Worker Thread Population " + GetCurrentChangwat().English + " " + lDownloader.year.ToString();
            lThread.Start();
        }

        private void FillChangwatCombobox(ComboBox cbx_changwat)
        {
            TambonHelper.LoadGeocodeList();

            cbx_changwat.Items.Clear();
            foreach ( PopulationDataEntry lEntry in TambonHelper.Geocodes )
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
            edt_year.Maximum = TambonHelper.PopulationStatisticMaxYear;
            edt_year.Value = edt_year.Maximum;
        }

        private void btnPopulationDownloadAll_click(object sender, EventArgs e)
        {
            var lChangwat = (PopulationDataEntry)cbx_changwat.SelectedItem;
            Int32 lGeocode = lChangwat.Geocode;
            for ( int lYear = 1993 ; lYear <= TambonHelper.PopulationStatisticMaxYear ; lYear++ )
            {

                PopulationData lDownloader = new PopulationData(lYear, lGeocode);
                lDownloader.OnProcessingFinished += PopulationDataReadyCheck;
                Thread t = new Thread(lDownloader.Process);
                t.Name = "Worker Thread Population " + lChangwat.English + " " + lDownloader.year.ToString();
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

        private void GazetteNewsReady(RoyalGazetteList data)
        {
            Invoke(new RoyalGazetteList.ProcessingFinished(RoyalGazetteViewer.ShowGazetteNewsDialog), new object[] { data });
        }
        private void ShowGazetteDialog(RoyalGazetteList data)
        {
            Invoke(new RoyalGazetteList.ProcessingFinishedFiltered(RoyalGazetteViewer.ShowGazetteDialog), new object[] { data, true });
        }
        private void btn_CheckForNews_Click(object sender, EventArgs e)
        {
            RoyalGazetteOnlineSearch lSearcher = new RoyalGazetteOnlineSearch();
            lSearcher.OnProcessingFinished += GazetteNewsReady;
            Thread t = new Thread(lSearcher.SearchNewsNow);
            t.Name = "Worker Thread Gazette News";
            t.Start();
        }

        private void btn_GazetteShowAll_Click(object sender, EventArgs e)
        {
            ShowGazetteDialog(TambonHelper.GlobalGazetteList);
        }

        private void btn_GazetteSearchYear_Click(object sender, EventArgs e)
        {
            RoyalGazetteOnlineSearch lSearcher = new RoyalGazetteOnlineSearch();
            var lGazetteList = lSearcher.SearchNews(new DateTime((Int32)edt_year.Value, 1, 1));
            GazetteNewsReady(lGazetteList);
        }

        private void btn_LoadCcaatt_Click(object sender, EventArgs e)
        {
            // ToDo: Konfigurierbares Verzeichnis für ccaatt files
            openFileDialogCCAATT.InitialDirectory = @"e:\Thailand\geocode\";
            openFileDialogCCAATT.FileName = "ccaatt.txt";
            openFileDialogCCAATT.ShowDialog();
        }

        private void ccaatt_postprocessing(DopaGeocodeList iData)
        {
            iData.RemoveAllKnownGeocodes();
            iData.ExportToXML(Path.Combine(GlobalSettings.XMLOutputDir, "unknowngeocodes.xml"));
            var lForm = new StringDisplayForm("New Geocodes", iData.ToString());
            lForm.Show();
        }
        private void openFileDialogCCAATT_FileOk(object sender,
                System.ComponentModel.CancelEventArgs e)
        {
            this.Activate();
            String[] lFiles = openFileDialogCCAATT.FileNames;
            foreach ( String lFileName in lFiles )
            {
                var lData = new DopaGeocodeList(lFileName);
                ccaatt_postprocessing(lData);
            }
        }
        private void openFileDialogXML_FileOk(object sender,
                System.ComponentModel.CancelEventArgs e)
        {
            this.Activate();
            String[] lFiles = openFileDialogXML.FileNames;
            RoyalGazetteList lCurrentGazetteList = new RoyalGazetteList();
            foreach ( String lFileName in lFiles )
            {
                RoyalGazetteList lLoadedGazetteList = RoyalGazetteList.Load(lFileName);
                lCurrentGazetteList.AddRange(lLoadedGazetteList);
            }
            ShowGazetteDialog(lCurrentGazetteList);
        }

        private void btn_DownloadCcaatt_Click(object sender, EventArgs e)
        {
            var lData = DopaGeocodeList.CreateFromOnline();
            ccaatt_postprocessing(lData);
        }

        private void btn_GazetteSearch_Click(object sender, EventArgs e)
        {
            RoyalGazetteSearch lSearchWindow = new RoyalGazetteSearch();
            lSearchWindow.OnSearchFinished += ShowGazetteDialog;
            lSearchWindow.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AmphoeComDownloader lDownloader = new AmphoeComDownloader();
            Int32 lGeocode = GetCurrentChangwat().Geocode;
            var lData = lDownloader.DoItAll(lGeocode);
            XmlDocument lXmlDocument = new XmlDocument();
            var lElement = (XmlElement)lXmlDocument.CreateNode("element", "mirror", "");
            lElement.SetAttribute("date", DateTime.Now.ToString("yyyy-MM-dd"));
            lXmlDocument.AppendChild(lElement);
            foreach ( var lDataEntry in lData )
            {
                lDataEntry.ExportToXML(lElement);
            }
            Directory.CreateDirectory(OutputDir());
            lXmlDocument.Save(Path.Combine(OutputDir(), "amphoe" + lGeocode.ToString() + ".xml"));

        }
        private String OutputDir()
        {
            String retval = Path.Combine(GlobalSettings.XMLOutputDir, "amphoe");
            return retval;
        }

        private void ButtonThesaban_Click(object sender, EventArgs e)
        {
            var lDownloader = new KontessabanDownloader();
            var lData = lDownloader.DoIt();

            XmlDocument lXmlDocumentAll = new XmlDocument();
            var lNodeAll = (XmlElement)lXmlDocumentAll.CreateNode("element", "country", "");
            lXmlDocumentAll.AppendChild(lNodeAll);
            XmlDocument lXmlDocumentNew = new XmlDocument();
            var lNodeNew = (XmlElement)lXmlDocumentNew.CreateNode("element", "country", "");
            lXmlDocumentNew.AppendChild(lNodeNew);
            foreach ( KontessabanDataEntry entity in lData )
            {
                entity.ExportToXML(lNodeAll);
                if ( entity.Geocode == 0 )
                {
                    entity.ExportToXML(lNodeNew);
                }
            }
            lXmlDocumentAll.Save(Path.Combine(GlobalSettings.XMLOutputDir, "Tessaban.xml"));
            lXmlDocumentNew.Save(Path.Combine(GlobalSettings.XMLOutputDir, "NewTessaban.xml"));
        }

        private void btnSetup_Click(object sender, EventArgs e)
        {
            Setup lSetupForm = new Setup();
            lSetupForm.ShowDialog();
        }

        private void btnNumerals_Click(object sender, EventArgs e)
        {
            NumeralsTambonHelper lNumeralsTambonHelperForm = new NumeralsTambonHelper();
            lNumeralsTambonHelperForm.Show();
        }

        private void btnTambonFrequency_Click(object sender, EventArgs e)
        {
            List<EntityType> lTypes = new List<EntityType>()
            {
                EntityType.Tambon
            };
            EntityCounter lCounter = new EntityCounter(lTypes);
            var lChangwat = (PopulationDataEntry)cbx_changwat.SelectedItem;
            // lCounter.BaseGeocode = lChangwat.Geocode;
            lCounter.Calculate();

            var lForm = new StringDisplayForm("Tambon", lCounter.CommonNames(20));
            lForm.Show();
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
                lSorted.Sort(delegate(Int32 p1, Int32 p2) { return (p2.CompareTo(p1)); });

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
            Int32 lGeocode = GetCurrentChangwat().Geocode;
            var lMuban = new MubanCSVReader();
            var lData = lMuban.Parse(lGeocode);

            var lForm = new StringDisplayForm("Muban", lMuban.Information(lData));

            String lOutFilename = Path.Combine(GlobalSettings.XMLOutputDir, "Muban" + lGeocode.ToString() + ".kml");
            lData.ExportToKml(lOutFilename);
            lForm.Show();
        }

        private void btnMubanNames_Click(object sender, EventArgs e)
        {
            List<EntityType> lTypes = new List<EntityType>()
            {
                EntityType.Muban
            };
            EntityCounter lNamesCounter = new EntityCounter(lTypes);
            if ( chkUseCsv.Checked )
            {
                var lList = new List<PopulationDataEntry>();
                var lCounter = new FrequencyCounter();
                foreach ( PopulationDataEntry lEntry in TambonHelper.Geocodes )
                {
                    if ( lEntry.Geocode != 10 )
                    {
                        var lReader = new MubanCSVReader();
                        var lData = lReader.Parse(lEntry.Geocode);
                        MubanCSVReader.Statistics(lData, lCounter);
                        var lFlatData = lData.FlatList(lTypes);
                        lList.AddRange(lFlatData);
                    }
                }
                var lFormStatistics = new StringDisplayForm("Muban", MubanCSVReader.StatisticsText(lCounter));
                lFormStatistics.Show();
                lNamesCounter.Calculate(lList);
            }
            else
            {
                lNamesCounter.Calculate();
            }
            var lFormNames = new StringDisplayForm("Muban", lNamesCounter.CommonNames(20));
            lFormNames.Show();

        }

        private void btnCreateKml_Click(object sender, EventArgs e)
        {
            var lList = new List<PopulationDataEntry>();
            foreach ( PopulationDataEntry lEntry in TambonHelper.Geocodes )
            {
                PopulationData lEntities = TambonHelper.GetGeocodeList(lEntry.Geocode);
                lList.Add(lEntities.Data);
            }
            PopulationDataEntry lMaster = new PopulationDataEntry();
            lMaster.SubEntities.AddRange(lList);
            String lOutFilename = Path.Combine(GlobalSettings.XMLOutputDir, "offices.kml");
            lMaster.ExportToKml(lOutFilename);

            var lGeotaggedOffices = new Dictionary<OfficeType, Int32>();
            var lAnyOffices = new Dictionary<OfficeType, Int32>();
            var lFlatList = lMaster.FlatList(EntityTypeHelper.AllEntityTypes);
            foreach ( var lEntity in lFlatList )
            {
                foreach ( var lOffice in lEntity.Offices )
                {
                    if ( lOffice.Location != null )
                    {
                        if ( !lGeotaggedOffices.ContainsKey(lOffice.Type) )
                        {
                            lGeotaggedOffices.Add(lOffice.Type, 0);
                        }
                        lGeotaggedOffices[lOffice.Type]++;
                    }
                    if ( !lAnyOffices.ContainsKey(lOffice.Type) )
                    {
                        lAnyOffices.Add(lOffice.Type, 0);
                    }
                    lAnyOffices[lOffice.Type]++;
                }
            }
            String lOfficeType = String.Empty;
            foreach ( OfficeType i in System.Enum.GetValues(typeof(OfficeType)) )
            {
                if ( lGeotaggedOffices.ContainsKey(i) )
                {
                    lOfficeType = lOfficeType +
                        i.ToString() + ' ' +
                        lGeotaggedOffices[i].ToString() +
                        " (of " + lAnyOffices[i].ToString() + ')' + Environment.NewLine;
                }
            }
            var lForm = new StringDisplayForm("Office types", lOfficeType);
            lForm.Show();
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
            List<String> lAachenHectards = new List<String>() {
                "31UGS05", "31UGS15", "31UGS06", "31UGS16", 
                "32UKB86", "32UKB85", "32UKB95", "32UKB94", "32ULB04",
                "32ULB03"
            };
            List<String> lThailandMyriads = new List<string>() { 
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

            String lOutFilename = Path.Combine(GlobalSettings.XMLOutputDir, "mgrs.kml");
            KmlHelper lKmlWriter = MgrsGridElement.StartKmlWriting();
            // foreach ( String lEntry in lThailandMyriads )
            foreach ( String lEntry in lAachenHectards )
            {
                MgrsGridElement lGrid = new MgrsGridElement(lEntry);
                // lGrid.Datum = GeoDatum.DatumIndian1975();
                XmlNode lNode = lKmlWriter.AddFolder(lKmlWriter.DocumentNode, lGrid.Name, false);
                //lGrid.WriteToKml(lKmlWriter, lNode);
                foreach ( var lSubGrid in lGrid.SubGrids() )
                {
                    lSubGrid.WriteToKml(lKmlWriter, lNode);
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
            lKmlWriter.SaveToFile(lOutFilename);
        }

        private void btnConstituency_Click(object sender, EventArgs e)
        {
            ConstituencyForm lForm = new ConstituencyForm();
            lForm.Show();
        }

        private void btn_PopulationAllProvinces_Click(object sender, EventArgs e)
        {
            PopulationData lDownloader = new PopulationData(Convert.ToInt32(edt_year.Value), 0);
            lDownloader.Process();
            lDownloader.ReOrderThesaban();
            PopulationDataEntry lCountry = lDownloader.Data;

            var lForm = new PopulationByEntityTypeViewer();
            lForm.BaseEntry = lCountry;
            lForm.Show();

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
            var lList = new List<PopulationDataEntry>();
            foreach ( PopulationDataEntry lEntry in TambonHelper.Geocodes )
            {
                PopulationData lEntities = TambonHelper.GetGeocodeList(lEntry.Geocode);
                lList.Add(lEntities.Data);
            }
            PopulationDataEntry lMaster = new PopulationDataEntry();
            Dictionary<String, String> lRomanization = new Dictionary<String, String>();
            StringBuilder lResultMistakes = new StringBuilder();
            Int32 lResultMistakesCount = 0;
            lMaster.SubEntities.AddRange(lList);
            foreach ( var lEntity in lMaster.FlatList(EntityTypeHelper.AllEntityTypes) )
            {
                if ( !String.IsNullOrEmpty(lEntity.English) )
                {
                    String lName = lEntity.Name;
                    String lEnglish = lEntity.English;
                    if ( lEntity.Type == EntityType.Muban )
                    {
                        lName = TambonHelper.StripBan(lName);
                        if ( lEnglish.StartsWith("Ban ") )
                        {
                            lEnglish = lEnglish.Remove(0, "Ban ".Length).Trim();
                        }
                    }
                    if ( lEntity.Type == EntityType.Chumchon )
                    {
                        if ( lName.StartsWith("ชุมชน") )
                        {
                            lName = lName.Remove(0, "ชุมชน".Length).Trim();
                        }
                        if ( lEnglish.StartsWith("Chumchon ") )
                        {
                            lEnglish = lEnglish.Remove(0, "Chumchon ".Length).Trim();
                        }
                    }
                    if ( lRomanization.Keys.Contains(lName) )
                    {
                        if ( lRomanization[lName] != lEnglish )
                        {
                            lResultMistakes.AppendLine(lEntity.Geocode.ToString() + ' ' + lName + ": " + lEnglish + " vs. " + lRomanization[lName]);
                            lResultMistakesCount++;
                        }
                    }
                    else
                    {
                        lRomanization[lName] = lEnglish;
                    }
                }
            }
            StringBuilder lResultSuggestions = new StringBuilder();
            Int32 lResultSuggestionsCount = 0;
            foreach ( var lEntity in lMaster.FlatList(EntityTypeHelper.AllEntityTypes) )
            {
                if ( String.IsNullOrEmpty(lEntity.English) )
                {
                    String lFound = String.Empty;
                    if ( lRomanization.Keys.Contains(lEntity.Name) )
                    {
                        lFound = lEntity.Name;
                    }
                    else if ( lRomanization.Keys.Contains(TambonHelper.StripBan(lEntity.Name)) )
                    {
                        lFound = TambonHelper.StripBan(lEntity.Name);
                    }
                    if ( !String.IsNullOrEmpty(lFound) )
                    {
                        lResultSuggestions.AppendLine(lEntity.Geocode.ToString() + ' ' + lEntity.Name + ": " + lRomanization[lFound]);
                        lResultSuggestionsCount++;
                    }
                    else
                    {
                        Boolean found = false;
                        foreach ( var lKeyValuePair in TambonHelper.NameSuffixRomanizations )
                        {
                            if ( lEntity.Name.EndsWith(lKeyValuePair.Key) )
                            {
                                String lSearch = TambonHelper.StripBan(lEntity.Name.Substring(0, lEntity.Name.Length - lKeyValuePair.Key.Length));
                                if ( String.IsNullOrEmpty(lSearch) )
                                {
                                    lResultSuggestions.AppendLine(lEntity.Geocode.ToString() + ' ' + lEntity.Name + ": " + lKeyValuePair.Value);
                                    lResultSuggestionsCount++;
                                    found = true;
                                }
                                else if ( lRomanization.Keys.Contains(lSearch) )
                                {
                                    lResultSuggestions.AppendLine(lEntity.Geocode.ToString() + ' ' + lEntity.Name + ": " + lRomanization[lSearch] + " " + lKeyValuePair.Value);
                                    lResultSuggestionsCount++;
                                    found = true;
                                }
                            }
                        }
                        if ( !found )
                        {
                            String lName = TambonHelper.StripBan(lEntity.Name);
                            foreach ( var lKeyValuePair in TambonHelper.NamePrefixRomanizations )
                            {
                                if ( lName.StartsWith(lKeyValuePair.Key) )
                                {
                                    String lSearch = lName.Substring(lKeyValuePair.Key.Length);
                                    if ( String.IsNullOrEmpty(lSearch) )
                                    {
                                        lResultSuggestions.AppendLine(lEntity.Geocode.ToString() + ' ' + lEntity.Name + ": " + lKeyValuePair.Value);
                                        lResultSuggestionsCount++;
                                        found = true;
                                    }
                                    else if ( lRomanization.Keys.Contains(lSearch) )
                                    {
                                        lResultSuggestions.AppendLine(lEntity.Geocode.ToString() + ' ' + lEntity.Name + ": " + lKeyValuePair.Value + " " + lRomanization[lSearch]);
                                        lResultSuggestionsCount++;
                                        found = true;
                                    }
                                }
                            }

                        }
                    }

                }
            }
            if ( lResultMistakesCount > 0 )
            {
                var lForm = new StringDisplayForm("Romanization problems (" + lResultMistakesCount.ToString() + ")", lResultMistakes.ToString());
                lForm.Show();
            }
            if ( lResultSuggestionsCount > 0 )
            {
                var lForm = new StringDisplayForm("Romanization suggestions (" + lResultSuggestionsCount.ToString() + ")", lResultSuggestions.ToString());
                lForm.Show();
            }
        }
    }
}
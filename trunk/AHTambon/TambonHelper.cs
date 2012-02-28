using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.IO;
using System.Globalization;
using System.Windows.Forms;

namespace De.AHoerstemeier.Tambon
{
    public class TambonHelper
    {

        #region constants
        private const string PHOSO = "พ.ศ.";
        public const Int32 PopulationStatisticMaxYear = 2010;
        public static XNamespace TambonNameSpace = "http://hoerstemeier.com/tambon/";
        #endregion
        static public RoyalGazetteList GlobalGazetteList = new RoyalGazetteList();
        /// <summary>
        /// List with all the provinces which have a TIS 1099 code assigned.
        /// </summary>
        static public List<PopulationDataEntry> ProvinceGeocodes = new List<PopulationDataEntry>();
        static public Int32 PreferredProvinceGeocode = 84;  // Surat Thani
        static public Encoding ThaiEncoding = Encoding.GetEncoding(874);
        static public CultureInfo CultureInfoUS = new CultureInfo("en-us");
        public static Dictionary<EntityLeaderType, String> EntityLeaderName = new Dictionary<EntityLeaderType, String>()
        {
            {EntityLeaderType.Governor, "ผู้ว่าราชการจังหวัด"},
            {EntityLeaderType.ViceGovernor, "รองผู้ว่าราชการจังหวัด"},
            {EntityLeaderType.DistrictOfficer,"นายอำเภอ"},
            {EntityLeaderType.MinorDistrictOfficer,"หัวหน้ากิ่งอำเภอ"},
            {EntityLeaderType.Kamnan,"กำนัน"},
            {EntityLeaderType.PhuYaiBan,"ผู้ใหญ่บ้าน"},
            {EntityLeaderType.Mayor,"นายกเทศมนตรี"},
            {EntityLeaderType.TAOMayor,"นายกองค์การบริหารส่วนตำบล"},
            {EntityLeaderType.TAOChairman,"ประธานกรรมการบริหารองค์การบริหารส่วนตำบล"},
            {EntityLeaderType.PAOChairman,"นายกองค์การบริหารส่วนจังหวัด"},
            {EntityLeaderType.SanitaryDistrictChairman,"ประธานกรรมการสุขาภิบาล"},
            {EntityLeaderType.ChumchonChairman,"ประธานชุมชน"}
        };
        public static Dictionary<GazetteSignPosition, String> GazetteSignPositionThai = new Dictionary<GazetteSignPosition, string>()
        {
            {GazetteSignPosition.PrimeMinister, "นายกรัฐมนตรี" },
            {GazetteSignPosition.DeputyPrimeMinister, "รองนายกรัฐมนตรี" },
            {GazetteSignPosition.MinisterOfInterior, "รัฐมนตรีว่าการกระทรวงมหาดไทย" },
            {GazetteSignPosition.DeputyMinisterOfInterior, "รัฐมนตรีช่วยว่าการกระทรวงมหาดไทย" },
            {GazetteSignPosition.MinistryOfInteriorPermanentSecretary, "ปลัดกระทรวงมหาดไทย" },
            {GazetteSignPosition.ProvinceGovernor, "ผู้ว่าราชการจังหวัด" },
            {GazetteSignPosition.ViceProvinceGovernor, "รองผู้ว่าราชการจังหวัด" },
            // {GazetteSignPosition.BangkokGovernor, ""},
            {GazetteSignPosition.BangkokPermanentSecretary, "ปลัดกรุงเทพ" },
            {GazetteSignPosition.DeputyBangkokPermanentSecretary, "รองปลัดกรุงเทพ" },
            {GazetteSignPosition.MinisterOfInformationAndCommunicationTechnology, "รัฐมนตรีว่าการกระทรวงเทคโนโลยีสารสนเทศและการสื่อสาร" },
            {GazetteSignPosition.ElectionCommissionPresident, "ประธานกรรมการการเลือกตั้ง" },
            {GazetteSignPosition.RoyalInstitutePresident, "นายกราชบัณฑิตยสถาน" } ,
            {GazetteSignPosition.RoyalInstituteActingPresident, "รักษาการตำแหน่งนายกราชบัณฑิตยสถาน" },
            {GazetteSignPosition.DepartmentOfTransportDirectorGeneral,"อธิบดีกรมการขนส่งทางบก" },
            {GazetteSignPosition.DistrictOfficerBangkok,"ผู้อำนวยการเขต"},
            {GazetteSignPosition.DistrictOfficer,"นายอำเภอ"},
            {GazetteSignPosition.SpeakerOfParliament,"ประธานสภาผู้แทนราษฎร"},
            {GazetteSignPosition.Mayor,"นายกเทศมนตรี"},
            {GazetteSignPosition.TAOMayor,"นายกองค์การบริหารส่วนตําบล"},
            {GazetteSignPosition.PAOChairman,"นายกองค์การบริหารส่วนจังหวัด"},
            {GazetteSignPosition.MunicipalPermanentSecretary,"ปลัดเทศบาล"},
            {GazetteSignPosition.MinistryOfInteriorDeputyPermanentSecretary,"รองปลัดกระทรวงมหาดไทย"},
            {GazetteSignPosition.FineArtsDepartmentDirectorGeneral,"อธิบดีกรมศิลปากร"}
        };

        public static Dictionary<String, String> NameSuffixRomanizations = new Dictionary<String, String>()
        {
            {"เหนือ", "Nuea"},  //North
            {"ใต้", "Tai"}, // South
            {"พัฒนา", "Phatthana"}, // Development
            {"ใหม่", "Mai"},  // New
            {"ทอง", "Thong"}, // Gold
            {"น้อย","Noi"}, // Small
            {"ใน", "Nai"},  // within
            // less common ones
            { "สามัคคี", "Samakkhi" },  // Harmonious
            { "ใหม่พัฒนา", "Mai Phatthana"}, // New Development
            {"ตะวันออก", "Tawan Ok"}, // East
            {"ตะวันตก", "Tawan Tok"}, // West
            {"สอง", "Song"},  // second
            {"กลาง", "Klang"}, // Middle
            {"คำ", "Kham"},  // Word
            {"ใหญ่", "Yai"}, // Large
            {"เล็ก","Lek"},  // small
            {"เก่า", "Kao"}, // Old
            {"สันติสุข", "Santi Suk"},  // peace
            {"เจริญ", "Charoen"},  // growth
            {"ศรีเจริญ", "Si Charoen"},
            {"เจริญสุข","Charoen Suk"},
            {"บูรพา", "Burapha"},  // East
            {"สวรรค์", "Sawan"}, // Heaven
            {"หลวง", "Luang"},  // Big
            {"งาม", "Ngam"},     // Beautiful
            {"สมบูรณ์", "Sombun"}, // Complete
            {"สะอาด", "Sa-at"},  // clean
            {"นอก","Nok"},  // outside
            {"แดง","Daeng"},  // red
            {"ดง","Dong"},  
            {"ไร","Rai"},  // Gain
            {"ราษฏร์","Rat"}, // people
            {"อรุณ","Arun"},  // dawn
            {"เรือ", "Ruea"},  // boat
            {"เฒ่า", "Thao"},  // old
            {"ยืน", "Yuen"},  // durable
            {"ยาง","Yang"},  // Rubber
            {"บน", "Bon"},  // upon
            {"อุดม", "Udom"},  // rich
            {"เดิม","Doem"},  // old
            {"บำรุง","Bamrung"}, // administrate
            {"เตียน","Tian"},
            {"เหลี่ยม","Liam"},
            {"คีรี","Khiri"},
            {"เด่น","Den"},  // notable
            {"สำนัก","Samnak"},  // office
            {"มงคล","Mongkhon"},  // dragon
            {"ศิริ","Siri"},
            {"ถาวร","Thawon"},  // permanent
            {"นิคม","Nikhom"}  // plantation
        };
        public static Dictionary<String, String> NamePrefixRomanizations = new Dictionary<String, String>()
        {
            {"ปากคลอง", "Pak Khlong"},  // Mouth of Canal
            {"คลอง", "Khlong"},  // Canal
            {"น้ำ","Nam"},  // Water (river)
            {"ปากน้ำ","Pak Nam"},  // River mouth
            {"แม่","Mae"},  // Mother
            {"วัง","Wang"},  // Palace
            {"หนอง","Nong"},  // Swamp
            {"หัว","Hua"},  // Head
            {"ตลาด","Talat"},  // Market
            {"ห้วย", "Huai"},  // Creek
            {"ดอน","Don"},  // Hill
            {"แหลม","Laem"},  // Cape
            {"ท่า","Tha"},  // position
            {"โคก","Khok"},  // mound
            {"บาง","Bang"},  // village
            {"นา","Na"},  // field
            {"ลาด","Lat"},  // slope
            {"ไผ่","Phai"},  // Bamboo
            {"วัด","Wat"},  // Temple
            {"พระ","Phra"}, // holy
            {"ศรี","Si"},
            {"โนน","Non"},
            {"โพธิ์","Pho"},
            {"บึง","Bueng"},  // swamp
            {"หลัก","Lak"},  // pillar
            {"ปาก","Pak"},  // mouth            
            {"เกาะ","Ko"},  // Island
            {"ป่า","Pa"},  // forest
            {"มาบ","Map"},  // marshland
            {"อ่าง","Ang"},  // Basin
            {"หาด","Hat"},  // Beach
            {"สวน","Suan"},  // Garden
            {"อ่าว","Ao"},  // Bay
            {"ถ้ำ","Tham"},  // cave
            {"ดอย","Doi"},  // mountain
            {"ซับ","Sap"},  // absorb
            {"สัน","San"},  // crest
            {"นคร", "Nakhon"},  // city
            {"โป่ง","Pong"},   // large
            {"ต้น","Ton"},   // Beginning
            {"เชียง","Chiang"}, //
            {"เหล่า","Lao"}, 
            {"ชัย","Chai"}, 
            {"โพรง","Phrong"},
            {"ตรอก","Trok"},  // lane
            {"ดวง","Duang"}  // disc
        };

        public static Dictionary<String, String> ChangwatMisspellings = new Dictionary<String, String>()
        {
            {"สุราษฏร์ธานี","สุราษฎร์ธานี"}
        };
        public static Dictionary<String, PersonTitle> PersonTitleStrings = new Dictionary<String, PersonTitle>()
        {
            {"นาย",PersonTitle.Mister},
            {"นาง",PersonTitle.Mistress},
            {"น.ส.",PersonTitle.Miss},
            {"พล.อ.",PersonTitle.General},
            {"พล.ท.",PersonTitle.LieutenantGeneral},
            {"พล.ต.",PersonTitle.MajorGeneral},
            {"พลตรี",PersonTitle.MajorGeneral},
            {"พ.อ.",PersonTitle.Colonel},
            {"พ.ท.",PersonTitle.LieutenantColonel},
            {"พ.ต.",PersonTitle.Major},
            {"ร.อ.",PersonTitle.Captain},
            {"ร้อยเอก",PersonTitle.Captain},
            {"ร.ท.",PersonTitle.FirstLieutenant},
            {"ร.ต.",PersonTitle.SecondLieutenant},
            {"ร้อยตรี",PersonTitle.SecondLieutenant},
            {"ว่าที่ ร.ต.",PersonTitle.ActingSecondLieutenant},
            {"ว่าที่ร.ต.",PersonTitle.ActingSecondLieutenant},
            {"ว่าที่ร้อยตรี",PersonTitle.ActingSecondLieutenant},
            {"เรือตรี",PersonTitle.SubLieutenant}

            // พล.ต.อ. Police General
            // พล.ต.ท. Police Lieutenant General
            // พล.ต.ต. = พลตำรวจตรี Police Major General
            // พ.ต.อ. Police Colonel
            // พ.ต.ท. Police Lieutenant Colonel
            // พ.ต.ต. Police Major
            // ร.ต.อ. Police Captain
            // ร.ต.ท. Police Lieutenant
            // ร.ต.ต. Police Sub-Lieutenant
            // นายกองเอก ?
            // อำมาตย์โท
            // อำมาตย์เอก
            // เรืออากาศตรี
        };
        internal static Dictionary<Char, Byte> ThaiNumerals = new Dictionary<char, byte> 
        {
            {'๐',0}, 
            {'๑',1}, 
            {'๒',2}, 
            {'๓',3}, 
            {'๔',4},
            {'๕',5}, 
            {'๖',6},
            {'๗',7},
            {'๘',8},
            {'๙',9}
        };
        public static Dictionary<String, Byte> ThaiMonthNames = new Dictionary<string, byte>
        {
            {"มกราคม",1},
            {"กุมภาพันธ์",2},
            {"มีนาคม",3},
            {"เมษายน",4},
            {"พฤษภาคม",5},
            {"มิถุนายน",6},
            {"กรกฎาคม",7},
            {"สิงหาคม",8},
            {"กันยายน",9},
            {"ตุลาคม",10},
            {"พฤศจิกายน",11},
            {"ธันวาคม",12}
        };
        public static Dictionary<String, Byte> ThaiMonthAbbreviations = new Dictionary<string, byte>
        {
            {"ม.ค.",1},
            {"ก.พ.",2},
            {"มี.ค.",3},
            {"เม.ย.",4},
            {"พ.ค.",5},
            {"มิ.ย.",6},
            {"ก.ค.",7},
            {"สิ.ค.",8},
            {"ส.ค.",8},
            {"ก.ย.",9},
            {"ต.ค.",10},
            {"พ.ย.",11},
            {"ธ.ค.",12}
        };

        // XML utilities
        public static XmlDocument XmlDocumentFromNode(XmlNode iNode)
        {
            XmlDocument retval = null;

            if ( iNode is XmlDocument )
            {
                retval = (XmlDocument)iNode;
            }
            else
            {
                retval = iNode.OwnerDocument;
            }

            return retval;
        }
        public static XmlNode XNodeToXmlNode(XNode iNode)
        {
            String lXml = iNode.ToString();
            XmlDocument lXmlDoc = new XmlDocument();
            lXmlDoc.LoadXml(lXml);
            return lXmlDoc.FirstChild;
        }
        public static Boolean HasAttribute(XmlNode iNode, String iAttributeName)
        {
            Boolean retval = false;
            if ( iNode != null && iNode.Attributes != null )
            {
                foreach ( XmlAttribute i in iNode.Attributes )
                {
                    retval = retval | (i.Name == iAttributeName);
                }
            }
            return retval;
        }

        public static String GetAttribute(XmlNode iNode, String iAttributeName)
        {
            String RetVal = String.Empty;
            if ( iNode != null && iNode.Attributes != null && (iNode.Attributes.Count > 0) && !String.IsNullOrEmpty(iAttributeName) )
            {
                RetVal = iNode.Attributes.GetNamedItem(iAttributeName).Value;
            }
            return RetVal;
        }
        public static String GetAttributeOptionalString(XmlNode iNode, String iAttributeName)
        {
            String RetVal = String.Empty;
            if ( HasAttribute(iNode, iAttributeName) )
            {
                RetVal = iNode.Attributes.GetNamedItem(iAttributeName).Value;
            }
            return RetVal;
        }
        public static Int32 GetAttributeOptionalInt(XmlNode iNode, String iAttributeName, Int32 iReplace)
        {
            Int32 RetVal = iReplace;
            if ( HasAttribute(iNode, iAttributeName) )
            {
                string s = iNode.Attributes.GetNamedItem(iAttributeName).Value;
                if ( !String.IsNullOrEmpty(s) )
                {
                    try
                    {
                        RetVal = Convert.ToInt32(s);
                    }
                    catch
                    {
                    }
                }
            }
            return RetVal;
        }
        public static Boolean GetAttributeOptionalBool(XmlNode iNode, String iAttributeName, Boolean iReplace)
        {
            String lValue = GetAttributeOptionalString(iNode, iAttributeName);
            switch ( lValue )
            {
                case "":
                    return iReplace;
                case "0":
                case "false":
                    return false;
                case "1":
                case "true":
                    return true;
                default:
                    throw new ArgumentOutOfRangeException("Invalid boolean value " + lValue);
            }
        }

        public static DateTime GetAttributeOptionalDateTime(XmlNode iNode, String iAttributeName)
        {
            DateTime RetVal = new DateTime();
            if ( HasAttribute(iNode, iAttributeName) )
            {
                RetVal = Convert.ToDateTime(iNode.Attributes.GetNamedItem(iAttributeName).Value);
            }
            return RetVal;
        }
        internal static DateTime GetAttributeDateTime(XmlNode iNode, String iAttributeName)
        {
            DateTime RetVal = Convert.ToDateTime(iNode.Attributes.GetNamedItem(iAttributeName).Value);
            return RetVal;
        }

        public static void StreamCopy(Stream iInput, Stream ioOutput)
        {
            byte[] lBuffer = new byte[2048];
            int lRead = 0;

            do
            {
                lRead = iInput.Read(lBuffer, 0, lBuffer.Length);
                ioOutput.Write(lBuffer, 0, lRead);
            } while ( lRead > 0 );
        }
        public static bool IsNumeric(String iValue)
        {
            for ( int i = 0 ; i < iValue.Length ; i++ )
            {
                if ( !(Convert.ToInt32(iValue[i]) >= 48 && Convert.ToInt32(iValue[i]) <= 57) )
                {
                    return false;
                }
            }
            return !String.IsNullOrEmpty(iValue);
        }

        /// <summary>
        /// Replaces Thai numberals with their corresponding Arabian numeral.
        /// </summary>
        /// <param name="value">String to check.</param>
        /// <returns>String with numerals exchanged.</returns>
        public static String ReplaceThaiNumerals(String value)
        {
            string RetVal = String.Empty;

            if ( !String.IsNullOrEmpty(value) )
            {
                foreach ( char c in value )
                {
                    if ( ThaiNumerals.ContainsKey(c) )
                    {
                        RetVal = RetVal + ThaiNumerals[c].ToString();
                    }
                    else
                    {
                        RetVal = RetVal + c;
                    }
                }
            }
            return RetVal;
        }

        /// <summary>
        /// Replaces any Arabian numerals with the corresponding Thai numerals.
        /// </summary>
        /// <param name="value">String to be checked.</param>
        /// <returns>String with Thai numerals.</returns>
        internal static string UseThaiNumerals(string value)
        {
            string RetVal = String.Empty;

            if ( !String.IsNullOrEmpty(value) )
            {
                foreach ( Char c in value )
                {
                    if ( (c >= '0') | (c <= '9') )
                    {
                        Int32 numericValue = Convert.ToInt32(c) - Convert.ToInt32('0');
                        foreach ( KeyValuePair<Char, Byte> keyValuePair in ThaiNumerals )
                        {
                            if ( keyValuePair.Value == numericValue )
                            {
                                RetVal = RetVal + keyValuePair.Key;
                            }
                        }
                    }
                    else
                    {
                        RetVal = RetVal + c;
                    }
                }
            }
            return RetVal;
        }

        internal static DateTime ParseThaiDate(string iValue)
        {
            String lMonthString = String.Empty;
            Int32 lMonth = 0;
            String lYearString = String.Empty;
            Int32 lYear = 0;
            Int32 lDay = 0;
            Int32 lPosition = 0;

            String lDate = ReplaceThaiNumerals(iValue);

            lPosition = lDate.IndexOf(' ');
            lDay = Convert.ToInt32(lDate.Substring(0, lPosition));
            lDate = lDate.Substring(lPosition + 1, lDate.Length - lPosition - 1);
            lPosition = lDate.IndexOf(' ');
            lMonthString = lDate.Substring(0, lPosition).Trim();
            lMonth = ThaiMonthNames[lMonthString];
            // TODO: Kamen da nicht auch welche mit KhoSo vor?
            lPosition = lDate.IndexOf(PHOSO) + PHOSO.Length;
            lYearString = lDate.Substring(lPosition, lDate.Length - lPosition);
            lYear = Convert.ToInt32(lYearString);
            if ( lYear < 2100 )
            {
                lYear = lYear + 543;  // there are entries in KhoSo but with "พ.ศ." in the returned info
            }

            if ( (lYear < 2484) & (lMonth < 4) )
            {
                lYear = lYear - 542;
            }
            else
            {
                lYear = lYear - 543;
            }
            return new DateTime(lYear, lMonth, lDay);
            ;
        }

        /// <summary>
        /// Checks whether two geocodes are identical, or in case the sub entities are included
        /// if the code to check is below the code to find.
        /// </summary>
        /// <param name="geocodeToFind">Code to find.</param>
        /// <param name="geocodeToCheck">Code to check.</param>
        /// <param name="includeSubEntities">Toggles whether codes under the main code are considered fitting as well.</param>
        /// <returns>True if both codes fit together, false otherwise.</returns>
        internal static Boolean IsSameGeocode(Int32 geocodeToFind, Int32 geocodeToCheck, Boolean includeSubEntities)
        {
            Boolean result = false;
            if ( includeSubEntities )
            {
                result = TambonHelper.IsBaseGeocode(geocodeToFind, geocodeToCheck);
            }
            else
            {
                result = (geocodeToFind == geocodeToCheck);
            }
            return result;
        }

        /// <summary>
        /// Checks whether a geocode is a code under another base code.
        /// A base code of zero means no check is done and true is returned.
        /// </summary>
        /// <param name="baseGeocode">Base code.</param>
        /// <param name="geocodeToCheck">Code to be checked to be under the base code.</param>
        /// <returns>True if code is under base code, or base code is zero; false otherwise.</returns>
        public static Boolean IsBaseGeocode(Int32 baseGeocode, Int32 geocodeToCheck)
        {
            Boolean result = false;
            if ( baseGeocode == 0 )
            {
                result = true;
            }
            else if ( geocodeToCheck == 0 )
            {
                result = false;
            }
            else
            {
                Int32 level = 1;
                while ( baseGeocode < 1000000 )
                {
                    baseGeocode = baseGeocode * 100;
                    level = level * 100;
                }
                while ( geocodeToCheck < 1000000 )
                {
                    geocodeToCheck = geocodeToCheck * 100;
                }
                Int32 difference = geocodeToCheck - baseGeocode;

                result = (!(difference < 0)) & (difference < level);
            }
            return result;
        }

        internal static Int64 GetDateJavaScript(DateTime iValue)
        {
            // milliseconds since January 1, 1970
            TimeSpan lDifference = iValue.ToUniversalTime() - new DateTime(1970, 1, 1);
            Int64 retval = Convert.ToInt64(lDifference.TotalMilliseconds);
            return retval;
        }

        internal static String OnlyNumbers(String iValue)
        {
            String retval = String.Empty;
            foreach ( Char c in iValue )
            {
                if ( (c >= '0') && (c <= '9') )
                {
                    retval = retval + c;
                }
            }
            return retval;
        }

        /// <summary>
        /// Gets the geocode from the province name.
        /// </summary>
        /// <param name="changwatName">Name of the province in Thai.</param>
        /// <returns>The geocode of the province; zero if no province was found.</returns>
        internal static Int32 GetGeocode(String changwatName)
        {
            XElement geocodeXml = XElement.Load(BasicGeocodeFileName());
            Int32 provinceId = 0;
            var query = from c in geocodeXml.Descendants(TambonHelper.TambonNameSpace + "entity")
                        where (String)c.Attribute("name") == changwatName
                        select (Int32)c.Attribute("geocode");

            foreach ( Int32 entry in query )
            {
                provinceId = entry;
            }
            return provinceId;
        }

        /// <summary>
        /// Gets the geocode of a entity by its Thai name, administrative type and the province.
        /// </summary>
        /// <param name="changwatName">Name of the province in Thai.</param>
        /// <param name="subdivisionName">Thai name of administrative entity to find.</param>
        /// <param name="subdivisionType">Type of administrative entity to find.</param>
        /// <returns>Code of the entity found; zero if none was found.</returns>
        internal static Int32 GetGeocode(String changwatName, String subdivisionName, EntityType subdivisionType)
        {
            Int32 provinceId = GetGeocode(changwatName);
            Int32 geocode = 0;
            if ( provinceId != 0 )
            {
                String searchName = subdivisionName;
                if ( searchName.Contains(" ") )
                {
                    searchName = searchName.Substring(0, searchName.IndexOf(" "));
                }
                XElement changwatXml = XElement.Load(GeocodeSourceFile(provinceId));
                var subdivisionQuery = from c in changwatXml.Descendants(TambonHelper.TambonNameSpace + "entity")
                                       where (
                                         ((String)c.Attribute("name") == searchName) &&
                                         EntityTypeHelper.IsCompatibleEntityType(
                                           (EntityType)Enum.Parse(typeof(EntityType), (String)c.Attribute("type"))
                                           , subdivisionType))
                                       select (Int32)c.Attribute("geocode");

                geocode = subdivisionQuery.FirstOrDefault();
            }
            return geocode;
        }

        internal static String _BaseXMLDirectory = Path.GetDirectoryName(Application.ExecutablePath);
        public static String BaseXMLDirectory
        {
            get { return _BaseXMLDirectory; }
            set { _BaseXMLDirectory = value; }
        }

        internal static String GeocodeXmlSourceDir()
        {
            String retval = BaseXMLDirectory + "\\geocode\\";
            return retval;
        }
        static private String GeocodeSourceFile(Int32 provinceGeocode)
        {
            String filename = GeocodeXmlSourceDir() + "geocode" + provinceGeocode.ToString("D2") + ".XML";
            return filename;
        }

        static private Dictionary<Int32, PopulationDataEntry> _GeocodeCache = new Dictionary<Int32, PopulationDataEntry>();
        /// <summary>
        /// Returns the tree of administrative subdivisions for a given province.
        /// </summary>
        /// <param name="provinceCode">TIS1099 code of the province.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <cref>provinceCode</cref> does not refer to a valid province.</exception>
        /// <returns>Tree of subdivisions.</returns>
        /// <remarks>Internally caches a clone of the returned value, to load the file from disc only once.</remarks>
        static public PopulationData GetGeocodeList(Int32 provinceCode)
        {
            PopulationData result = null;
            if ( !ProvinceGeocodes.Any(entry => entry.Geocode == provinceCode) )
            {
                throw new ArgumentOutOfRangeException("provinceCode");
            }
            if ( _GeocodeCache.Keys.Contains(provinceCode) )
            {
                result = new PopulationData((PopulationDataEntry)(_GeocodeCache[provinceCode].Clone()));
            }
            else
            {
                String fileName = TambonHelper.GeocodeSourceFile(provinceCode);
                if ( File.Exists(fileName) )
                {
                    result = PopulationData.Load(fileName);
                    _GeocodeCache.Add(provinceCode, (PopulationDataEntry)(result.Data.Clone()));
                }
            }
            return result;
        }

        static public String BasicGeocodeFileName()
        {
            String lFilename = BaseXMLDirectory + "\\geocode.xml";
            return lFilename;
        }
        static public String BasicRegionFileName()
        {
            String lFilename = BaseXMLDirectory + "\\regions.xml";
            return lFilename;
        }
        static public void LoadGeocodeList()
        {
            if ( !ProvinceGeocodes.Any() )
            {
                XElement lGeocodeXML = XElement.Load(BasicGeocodeFileName());

                var lQuery = from c in lGeocodeXML.Descendants(TambonHelper.TambonNameSpace + "entity")
                             orderby (string)c.Attribute("english")
                             select new PopulationDataEntry
                             {
                                 Name = (string)c.Attribute("name"),
                                 English = (string)c.Attribute("english"),
                                 Type = (EntityType)Enum.Parse(typeof(EntityType), (string)c.Attribute("type")),
                                 Geocode = (Int32)c.Attribute("geocode")
                             };

                ProvinceGeocodes.Clear();
                ProvinceGeocodes.AddRange(lQuery);
            }
        }
        static public List<String> RegionSchemes()
        {
            XElement lRegionsXML = XElement.Load(BasicRegionFileName());

            var lQuery = from c in lRegionsXML.Descendants(TambonHelper.TambonNameSpace + "regions")
                         orderby (string)c.Attribute("english")
                         select (String)c.Attribute("english");
            List<String> lResult = new List<String>();
            foreach ( var lEntry in lQuery )
            {
                lResult.Add(lEntry);
            }
            return lResult;
        }
        static public List<PopulationDataEntry> GetRegionBySchemeName(String iSchemeName)
        {
            XElement lRegionsXML = XElement.Load(BasicRegionFileName());
            List<PopulationDataEntry> lResult = new List<PopulationDataEntry>();

            var lQuery = from c in lRegionsXML.Descendants(TambonHelper.TambonNameSpace + "regions")
                         where (string)c.Attribute("english") == iSchemeName
                         select c.Elements();
            foreach ( var lEntry in lQuery )
            {
                foreach ( XElement lNode in lEntry )
                {
                    PopulationDataEntry lData = PopulationDataEntry.Load(XNodeToXmlNode(lNode));
                    lResult.Add(lData);
                }
            }
            return lResult;

        }

        /// <summary>
        /// Checks whether two Thai names can be considered identical as name of a Muban.
        /// </summary>
        /// <param name="name1">Name of first Muban.</param>
        /// <param name="name2">Name of second Muban.</param>
        /// <returns>True if identical, false otherwise.</returns>
        public static Boolean IsSameMubanName(String name1, String name2)
        {
            Boolean RetVal = (StripBanOrChumchon(name1) == StripBanOrChumchon(name2));
            return RetVal;
        }
        /// <summary>
        /// Removes the word Ban (บ้าน) preceeding the name.
        /// </summary>
        /// <param name="name">Name of a Muban.</param>
        /// <returns>Name without Ban.</returns>
        public static String StripBanOrChumchon(String name)
        {
            const String ThaiStringBan = "บ้าน";
            const String ThaiStringChumchon = "ชุมชน";
            String retval = String.Empty;
            if ( name.StartsWith(ThaiStringBan) )
            {
                retval = name.Remove(0, ThaiStringBan.Length).Trim();
            }
            else if ( name.StartsWith(ThaiStringChumchon) )
            {
                retval = name.Remove(0, ThaiStringChumchon.Length).Trim();
            }
            else
            {
                retval = name;
            }
            return retval;
        }
    }
}

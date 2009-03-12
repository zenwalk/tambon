﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace De.AHoerstemeier.Tambon
{
    class EntityLeader: ICloneable
    {
        #region properties
        private String mName = String.Empty;
        public String Name { get { return mName; } set { SetName(value); } }
        public String English { get; set; }
        public PersonTitle Title { get; set; }
        public String Telephone { get; set; }
        public String CellPhone { get; set; }
        public String Comment { get; set; }
        public EntityLeaderType Position { get; set; }
        public Int32 Index { get; set; }
        public DateTime BeginOfTerm { get; set; }
        public Int32 BeginOfTermYear { get; set; }
        public DateTime EndOfTerm { get; set; }
        public Int32 EndOfTermYear { get; set; }
        #endregion
        #region constructor
        public EntityLeader()
        {
        }
        public EntityLeader(EntityLeader iValue)
        {
            Name = iValue.Name;
            English = iValue.English;
            Title = iValue.Title;
            Telephone = iValue.Telephone;
            CellPhone = iValue.CellPhone;
            Position = iValue.Position;
            Index = iValue.Index;
            BeginOfTerm = iValue.BeginOfTerm;
            BeginOfTermYear = iValue.BeginOfTermYear;
            EndOfTerm = iValue.EndOfTerm;
            EndOfTermYear = iValue.EndOfTermYear;
        }
        #endregion
        #region methods
        private void SetName(String iName)
        {
            mName = iName;
            foreach (KeyValuePair<String,PersonTitle> lEntry in Helper.PersonTitleStrings)
            {
                String lSearch = lEntry.Key;
                if (iName.StartsWith(lSearch))
                {
                    Title = lEntry.Value;
                    mName = iName.Remove(0, lSearch.Length).Trim();
                }
            }
            // TODO Strip persontitle and store it separately in Title property
        }
        public void ExportToXML(XmlElement iNode)
        {
            XmlDocument lXmlDocument = Helper.XmlDocumentFromNode(iNode);
            var lNewElement = (XmlElement)lXmlDocument.CreateNode("element", "official", "");
            lNewElement.SetAttribute("title", Position.ToString());
            if (Index > 0)
            {
                lNewElement.SetAttribute("index", Index.ToString());
            }
            lNewElement.SetAttribute("name", Name);
            if (Title != PersonTitle.Unknown)
            {
                lNewElement.SetAttribute("nametitle", Title.ToString());
            }
            if (!String.IsNullOrEmpty(English))
            {
                    lNewElement.SetAttribute("english", English);
            }
            if (!String.IsNullOrEmpty(Telephone))
            {
                lNewElement.SetAttribute("telephone", Telephone);
            }
            if (!String.IsNullOrEmpty(CellPhone))
            {
                lNewElement.SetAttribute("cellphone", CellPhone);
            }
            if ((BeginOfTerm != null)&&(BeginOfTerm.Year>1))
            {
                lNewElement.SetAttribute("begin", BeginOfTerm.ToString("yyyy-MM-dd", Helper.CultureInfoUS));
            }
            if (BeginOfTermYear>0)
            {
                lNewElement.SetAttribute("beginyear", BeginOfTermYear.ToString());
            }
            if ((EndOfTerm != null)&&(EndOfTerm.Year>1))
            {
                lNewElement.SetAttribute("begin", EndOfTerm.ToString("yyyy-MM-dd", Helper.CultureInfoUS));
            }
            if (EndOfTermYear > 0)
            {
                lNewElement.SetAttribute("endyear", EndOfTermYear.ToString());
            }
            if (!String.IsNullOrEmpty(Comment))
            {
                lNewElement.SetAttribute("comment", Comment);
            }
            iNode.AppendChild(lNewElement);
        }
        internal static EntityLeader Load(XmlNode iNode)
        {
            EntityLeader RetVal = null;

            if (iNode != null && iNode.Name.Equals("official"))
            {
                RetVal = new EntityLeader();
                RetVal.Name = Helper.GetAttribute(iNode, "name");
                RetVal.English = Helper.GetAttributeOptionalString(iNode, "english");
                RetVal.Telephone = Helper.GetAttributeOptionalString(iNode, "telephone");
                RetVal.CellPhone = Helper.GetAttributeOptionalString(iNode, "cellphone");
                RetVal.Comment = Helper.GetAttributeOptionalString(iNode, "comment");

                RetVal.BeginOfTermYear = Helper.GetAttributeOptionalInt(iNode, "beginyear", 0);
                RetVal.EndOfTermYear = Helper.GetAttributeOptionalInt(iNode, "endyear", 0);
                RetVal.Index = Helper.GetAttributeOptionalInt(iNode, "index", 0);
                RetVal.BeginOfTerm = Helper.GetAttributeOptionalDateTime(iNode, "begin");
                RetVal.EndOfTerm = Helper.GetAttributeOptionalDateTime(iNode, "end");

                String lPosition = Helper.GetAttribute(iNode, "title");
                RetVal.Position = (EntityLeaderType)Enum.Parse(typeof(EntityLeaderType), lPosition);

                String lPersonTitle = Helper.GetAttributeOptionalString(iNode, "nametitle");
                if (!String.IsNullOrEmpty(lPersonTitle))
                {
                    RetVal.Title = (PersonTitle)Enum.Parse(typeof(PersonTitle), lPosition);
                }
            }
            return RetVal;
        }
        #endregion
        #region ICloneable Members

        public object Clone()
        {
            return new EntityLeader(this);
        }

        #endregion
    }
}

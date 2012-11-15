// ------------------------------------------------------------------------------
//  <auto-generated>
//    Generated by Xsd2Code. Version 3.4.3.29394
//    <NameSpace>De.AHoerstemeier.Tambon</NameSpace><Collection>List</Collection><codeType>CSharp</codeType><EnableDataBinding>False</EnableDataBinding><EnableLazyLoading>False</EnableLazyLoading><TrackingChangesEnable>False</TrackingChangesEnable><GenTrackingClasses>False</GenTrackingClasses><HidePrivateFieldInIDE>False</HidePrivateFieldInIDE><EnableSummaryComment>True</EnableSummaryComment><VirtualProp>False</VirtualProp><IncludeSerializeMethod>False</IncludeSerializeMethod><UseBaseClass>False</UseBaseClass><GenBaseClass>False</GenBaseClass><GenerateCloneMethod>False</GenerateCloneMethod><GenerateDataContracts>True</GenerateDataContracts><CodeBaseTag>Net40</CodeBaseTag><SerializeMethodName>Serialize</SerializeMethodName><DeserializeMethodName>Deserialize</DeserializeMethodName><SaveToFileMethodName>SaveToFile</SaveToFileMethodName><LoadFromFileMethodName>LoadFromFile</LoadFromFileMethodName><GenerateXMLAttributes>True</GenerateXMLAttributes><EnableEncoding>False</EnableEncoding><AutomaticProperties>False</AutomaticProperties><GenerateShouldSerialize>False</GenerateShouldSerialize><DisableDebug>False</DisableDebug><PropNameSpecified>Default</PropNameSpecified><Encoder>UTF8</Encoder><CustomUsings></CustomUsings><ExcludeIncludedTypes>True</ExcludeIncludedTypes><EnableInitializeFields>True</EnableInitializeFields>
//  </auto-generated>
// ------------------------------------------------------------------------------
namespace De.AHoerstemeier.Tambon {
    using System;
    using System.Diagnostics;
    using System.Xml.Serialization;
    using System.Collections;
    using System.Xml.Schema;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    
    
    /// <summary>
    /// Reference to a board meeting.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://hoerstemeier.com/tambon/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://hoerstemeier.com/tambon/", IsNullable=true)]
    [System.Runtime.Serialization.DataContractAttribute(Name="MeetingReference", Namespace="http://hoerstemeier.com/tambon/", IsReference=true)]
    public partial class MeetingReference {
        
        private byte numberField;
        
        private BoardNumber boardField;
        
        private System.DateTime dateField;
        
        /// <summary>
        /// Number of the meeting in the given year.
        /// </summary>
        /// <value>
        /// The number.
        /// </value>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.Runtime.Serialization.DataMemberAttribute()]
        public byte number {
            get {
                return this.numberField;
            }
            set {
                this.numberField = value;
            }
        }
        
        /// <summary>
        /// Type of board.
        /// </summary>
        /// <value>
        /// The board.
        /// </value>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.Runtime.Serialization.DataMemberAttribute()]
        public BoardNumber board {
            get {
                return this.boardField;
            }
            set {
                this.boardField = value;
            }
        }
        
        /// <summary>
        /// Date of the board meeting.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType="date")]
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime date {
            get {
                return this.dateField;
            }
            set {
                this.dateField = value;
            }
        }
    }
    
    /// <summary>
    /// Type of board.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://hoerstemeier.com/tambon/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://hoerstemeier.com/tambon/", IsNullable=false)]
    public enum BoardNumber {
        
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Item1,
        
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        Item2,
        
        [System.Xml.Serialization.XmlEnumAttribute("1,2")]
        Item12,
        
        /// <summary>
        /// Board to consider name changes (คณะกรรมการพิจารณาเรื่องการขอเปลี่ยนแปลงชื่อจังหวัด อำเภอ และตำบล หมู่บ้าน หรือสถานที่ราชการอื่น ๆ)
        /// </summary>
        rename,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://hoerstemeier.com/tambon/")]
    [System.Xml.Serialization.XmlRootAttribute("boardmeetings", Namespace="http://hoerstemeier.com/tambon/", IsNullable=false)]
    [System.Runtime.Serialization.DataContractAttribute(Name="MeetingEntryContainerTop", Namespace="http://hoerstemeier.com/tambon/", IsReference=true)]
    public partial class MeetingEntryContainerTop {
        
        private List<object> itemsField;
        
        /// <summary>
        /// Creates a new instance of MeetingEntryContainerTop.
        /// </summary>
        public MeetingEntryContainerTop() {
            this.itemsField = new List<object>();
        }
        
        /// <summary>
        /// Auto generated comment tag to suppress XML code documentation warning.
        /// </summary>
        /// <value>
        /// Auto generated value tag to suppress XML code documentation warning.
        /// </value>
        [System.Xml.Serialization.XmlElementAttribute("boardmeeting", typeof(MeetingEntry), Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("year", typeof(MeetingEntryContainerYear), Order=0)]
        [System.Runtime.Serialization.DataMemberAttribute()]
        public List<object> Items {
            get {
                return this.itemsField;
            }
            set {
                this.itemsField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://hoerstemeier.com/tambon/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://hoerstemeier.com/tambon/", IsNullable=true)]
    [System.Runtime.Serialization.DataContractAttribute(Name="MeetingEntry", Namespace="http://hoerstemeier.com/tambon/", IsReference=true)]
    public partial class MeetingEntry {
        
        private List<object> itemsField;
        
        private byte numberField;
        
        private bool numberFieldSpecified;
        
        private BoardNumber boardField;
        
        private System.DateTime dateField;
        
        private System.DateTime timeField;
        
        private bool timeFieldSpecified;
        
        private string urlField;
        
        /// <summary>
        /// Creates a new instance of MeetingEntry.
        /// </summary>
        public MeetingEntry() {
            this.itemsField = new List<object>();
        }
        
        /// <summary>
        /// Auto generated comment tag to suppress XML code documentation warning.
        /// </summary>
        /// <value>
        /// Auto generated value tag to suppress XML code documentation warning.
        /// </value>
        [System.Xml.Serialization.XmlElementAttribute("abolish", typeof(AbolishOperation), Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("areachange", typeof(AreaOperation), Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("create", typeof(CreateOperation), Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("related", typeof(GazetteRelated), Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("rename", typeof(RenameOperation), Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("renamewat", typeof(RenameWatOperation), Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("status", typeof(StatusOperation), Order=0)]
        [System.Runtime.Serialization.DataMemberAttribute()]
        public List<object> Items {
            get {
                return this.itemsField;
            }
            set {
                this.itemsField = value;
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.Runtime.Serialization.DataMemberAttribute()]
        public byte number {
            get {
                return this.numberField;
            }
            set {
                this.numberField = value;
            }
        }
        
        /// <summary>
        /// Auto generated comment tag to suppress XML code documentation warning.
        /// </summary>
        /// <value>
        /// Auto generated value tag to suppress XML code documentation warning.
        /// </value>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool numberSpecified {
            get {
                return this.numberFieldSpecified;
            }
            set {
                this.numberFieldSpecified = value;
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.Runtime.Serialization.DataMemberAttribute()]
        public BoardNumber board {
            get {
                return this.boardField;
            }
            set {
                this.boardField = value;
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute(DataType="date")]
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime date {
            get {
                return this.dateField;
            }
            set {
                this.dateField = value;
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute(DataType="time")]
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime time {
            get {
                return this.timeField;
            }
            set {
                this.timeField = value;
            }
        }
        
        /// <summary>
        /// Auto generated comment tag to suppress XML code documentation warning.
        /// </summary>
        /// <value>
        /// Auto generated value tag to suppress XML code documentation warning.
        /// </value>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool timeSpecified {
            get {
                return this.timeFieldSpecified;
            }
            set {
                this.timeFieldSpecified = value;
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute(DataType="anyURI")]
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string url {
            get {
                return this.urlField;
            }
            set {
                this.urlField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://hoerstemeier.com/tambon/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://hoerstemeier.com/tambon/", IsNullable=true)]
    [System.Runtime.Serialization.DataContractAttribute(Name="AbolishOperation", Namespace="http://hoerstemeier.com/tambon/", IsReference=true)]
    public partial class AbolishOperation : BasicOperation {
        
        private EntityType typeField;
        
        /// <summary>
        /// Auto generated comment tag to suppress XML code documentation warning.
        /// </summary>
        /// <value>
        /// Auto generated value tag to suppress XML code documentation warning.
        /// </value>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.Runtime.Serialization.DataMemberAttribute()]
        public EntityType type {
            get {
                return this.typeField;
            }
            set {
                this.typeField = value;
            }
        }
    }
    
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StatusOperation))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CreateOperation))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AbolishOperation))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AreaOperation))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RenameWatOperation))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RenameOperation))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://hoerstemeier.com/tambon/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://hoerstemeier.com/tambon/", IsNullable=true)]
    [System.Runtime.Serialization.DataContractAttribute(Name="BasicOperation", Namespace="http://hoerstemeier.com/tambon/", IsReference=true)]
    public partial class BasicOperation {
        
        private List<object> itemsField;
        
        private uint geocodeField;
        
        private bool geocodeFieldSpecified;
        
        private string nameField;
        
        private string englishField;
        
        private uint tambonField;
        
        private bool tambonFieldSpecified;
        
        private System.DateTime effectiveField;
        
        private bool effectiveFieldSpecified;
        
        private string commentField;
        
        private string indexField;
        
        /// <summary>
        /// Creates a new instance of BasicOperation.
        /// </summary>
        public BasicOperation() {
            this.itemsField = new List<object>();
        }
        
        /// <summary>
        /// Auto generated comment tag to suppress XML code documentation warning.
        /// </summary>
        /// <value>
        /// Auto generated value tag to suppress XML code documentation warning.
        /// </value>
        [System.Xml.Serialization.XmlElementAttribute("abolish", typeof(GazetteAbolish), Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("abolishpark", typeof(GazetteParkAbolish), Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("areachange", typeof(GazetteAreaChange), Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("areachangepark", typeof(GazetteParkAreaChange), Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("areadefinition", typeof(GazetteAreaDefinition), Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("capital", typeof(GazetteCapital), Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("constituency", typeof(GazetteConstituency), Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("correction", typeof(GazetteCorrection), Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("councilsize", typeof(GazetteCouncilSize), Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("create", typeof(GazetteCreate), Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("createpark", typeof(GazetteParkCreate), Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("electionresult", typeof(GazetteElectionResult), Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("interpellation", typeof(GazetteInterpellation), Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("meetingreference", typeof(MeetingReference), Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("mention", typeof(GazetteMention), Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("ministerialorder", typeof(MinisterialOrder), Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("official", typeof(GazetteOfficial), Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("reassign", typeof(GazetteReassign), Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("related", typeof(GazetteRelated), Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("rename", typeof(GazetteRename), Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("renumber", typeof(GazetteRenumber), Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("status", typeof(GazetteStatusChange), Order=0)]
        [System.Runtime.Serialization.DataMemberAttribute()]
        public List<object> Items {
            get {
                return this.itemsField;
            }
            set {
                this.itemsField = value;
            }
        }
        
        /// <summary>
        /// Geocode of the entity.
        /// </summary>
        /// <value>
        /// The geocode.
        /// </value>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.Runtime.Serialization.DataMemberAttribute()]
        public uint geocode {
            get {
                return this.geocodeField;
            }
            set {
                this.geocodeField = value;
            }
        }
        
        /// <summary>
        /// Auto generated comment tag to suppress XML code documentation warning.
        /// </summary>
        /// <value>
        /// Auto generated value tag to suppress XML code documentation warning.
        /// </value>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool geocodeSpecified {
            get {
                return this.geocodeFieldSpecified;
            }
            set {
                this.geocodeFieldSpecified = value;
            }
        }
        
        /// <summary>
        /// Auto generated comment tag to suppress XML code documentation warning.
        /// </summary>
        /// <value>
        /// Auto generated value tag to suppress XML code documentation warning.
        /// </value>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <summary>
        /// Auto generated comment tag to suppress XML code documentation warning.
        /// </summary>
        /// <value>
        /// Auto generated value tag to suppress XML code documentation warning.
        /// </value>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string english {
            get {
                return this.englishField;
            }
            set {
                this.englishField = value;
            }
        }
        
        /// <summary>
        /// Geocode of the corresponding tambon, used for TAO which have no geocode by themself.
        /// </summary>
        /// <value>
        /// The tambon.
        /// </value>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.Runtime.Serialization.DataMemberAttribute()]
        public uint tambon {
            get {
                return this.tambonField;
            }
            set {
                this.tambonField = value;
            }
        }
        
        /// <summary>
        /// Auto generated comment tag to suppress XML code documentation warning.
        /// </summary>
        /// <value>
        /// Auto generated value tag to suppress XML code documentation warning.
        /// </value>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool tambonSpecified {
            get {
                return this.tambonFieldSpecified;
            }
            set {
                this.tambonFieldSpecified = value;
            }
        }
        
        /// <summary>
        /// Date at which the change becomes effective.
        /// </summary>
        /// <value>
        /// The effective.
        /// </value>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType="date")]
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime effective {
            get {
                return this.effectiveField;
            }
            set {
                this.effectiveField = value;
            }
        }
        
        /// <summary>
        /// Auto generated comment tag to suppress XML code documentation warning.
        /// </summary>
        /// <value>
        /// Auto generated value tag to suppress XML code documentation warning.
        /// </value>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool effectiveSpecified {
            get {
                return this.effectiveFieldSpecified;
            }
            set {
                this.effectiveFieldSpecified = value;
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string comment {
            get {
                return this.commentField;
            }
            set {
                this.commentField = value;
            }
        }
        
        /// <summary>
        /// Number of the issue on the agenda of the meeting.
        /// </summary>
        /// <value>
        /// The index.
        /// </value>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType="nonNegativeInteger")]
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string index {
            get {
                return this.indexField;
            }
            set {
                this.indexField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://hoerstemeier.com/tambon/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://hoerstemeier.com/tambon/", IsNullable=true)]
    [System.Runtime.Serialization.DataContractAttribute(Name="StatusOperation", Namespace="http://hoerstemeier.com/tambon/", IsReference=true)]
    public partial class StatusOperation : BasicOperation {
        
        private EntityType oldField;
        
        private EntityType newField;
        
        /// <summary>
        /// Auto generated comment tag to suppress XML code documentation warning.
        /// </summary>
        /// <value>
        /// Auto generated value tag to suppress XML code documentation warning.
        /// </value>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.Runtime.Serialization.DataMemberAttribute()]
        public EntityType old {
            get {
                return this.oldField;
            }
            set {
                this.oldField = value;
            }
        }
        
        /// <summary>
        /// Auto generated comment tag to suppress XML code documentation warning.
        /// </summary>
        /// <value>
        /// Auto generated value tag to suppress XML code documentation warning.
        /// </value>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.Runtime.Serialization.DataMemberAttribute()]
        public EntityType @new {
            get {
                return this.newField;
            }
            set {
                this.newField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://hoerstemeier.com/tambon/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://hoerstemeier.com/tambon/", IsNullable=true)]
    [System.Runtime.Serialization.DataContractAttribute(Name="CreateOperation", Namespace="http://hoerstemeier.com/tambon/", IsReference=true)]
    public partial class CreateOperation : BasicOperation {
        
        private EntityType typeField;
        
        private uint ownerField;
        
        private bool ownerFieldSpecified;
        
        /// <summary>
        /// Auto generated comment tag to suppress XML code documentation warning.
        /// </summary>
        /// <value>
        /// Auto generated value tag to suppress XML code documentation warning.
        /// </value>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.Runtime.Serialization.DataMemberAttribute()]
        public EntityType type {
            get {
                return this.typeField;
            }
            set {
                this.typeField = value;
            }
        }
        
        /// <summary>
        /// Auto generated comment tag to suppress XML code documentation warning.
        /// </summary>
        /// <value>
        /// Auto generated value tag to suppress XML code documentation warning.
        /// </value>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.Runtime.Serialization.DataMemberAttribute()]
        public uint owner {
            get {
                return this.ownerField;
            }
            set {
                this.ownerField = value;
            }
        }
        
        /// <summary>
        /// Auto generated comment tag to suppress XML code documentation warning.
        /// </summary>
        /// <value>
        /// Auto generated value tag to suppress XML code documentation warning.
        /// </value>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool ownerSpecified {
            get {
                return this.ownerFieldSpecified;
            }
            set {
                this.ownerFieldSpecified = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://hoerstemeier.com/tambon/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://hoerstemeier.com/tambon/", IsNullable=true)]
    [System.Runtime.Serialization.DataContractAttribute(Name="AreaOperation", Namespace="http://hoerstemeier.com/tambon/", IsReference=true)]
    public partial class AreaOperation : BasicOperation {
        
        private EntityType typeField;
        
        /// <summary>
        /// Auto generated comment tag to suppress XML code documentation warning.
        /// </summary>
        /// <value>
        /// Auto generated value tag to suppress XML code documentation warning.
        /// </value>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.Runtime.Serialization.DataMemberAttribute()]
        public EntityType type {
            get {
                return this.typeField;
            }
            set {
                this.typeField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://hoerstemeier.com/tambon/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://hoerstemeier.com/tambon/", IsNullable=true)]
    [System.Runtime.Serialization.DataContractAttribute(Name="RenameWatOperation", Namespace="http://hoerstemeier.com/tambon/", IsReference=true)]
    public partial class RenameWatOperation : BasicOperation {
        
        private string oldnameField;
        
        private string oldenglishField;
        
        /// <summary>
        /// Auto generated comment tag to suppress XML code documentation warning.
        /// </summary>
        /// <value>
        /// Auto generated value tag to suppress XML code documentation warning.
        /// </value>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string oldname {
            get {
                return this.oldnameField;
            }
            set {
                this.oldnameField = value;
            }
        }
        
        /// <summary>
        /// Auto generated comment tag to suppress XML code documentation warning.
        /// </summary>
        /// <value>
        /// Auto generated value tag to suppress XML code documentation warning.
        /// </value>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string oldenglish {
            get {
                return this.oldenglishField;
            }
            set {
                this.oldenglishField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://hoerstemeier.com/tambon/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://hoerstemeier.com/tambon/", IsNullable=true)]
    [System.Runtime.Serialization.DataContractAttribute(Name="RenameOperation", Namespace="http://hoerstemeier.com/tambon/", IsReference=true)]
    public partial class RenameOperation : BasicOperation {
        
        private EntityType typeField;
        
        private string oldnameField;
        
        private string oldenglishField;
        
        /// <summary>
        /// Auto generated comment tag to suppress XML code documentation warning.
        /// </summary>
        /// <value>
        /// Auto generated value tag to suppress XML code documentation warning.
        /// </value>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.Runtime.Serialization.DataMemberAttribute()]
        public EntityType type {
            get {
                return this.typeField;
            }
            set {
                this.typeField = value;
            }
        }
        
        /// <summary>
        /// Auto generated comment tag to suppress XML code documentation warning.
        /// </summary>
        /// <value>
        /// Auto generated value tag to suppress XML code documentation warning.
        /// </value>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string oldname {
            get {
                return this.oldnameField;
            }
            set {
                this.oldnameField = value;
            }
        }
        
        /// <summary>
        /// Auto generated comment tag to suppress XML code documentation warning.
        /// </summary>
        /// <value>
        /// Auto generated value tag to suppress XML code documentation warning.
        /// </value>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string oldenglish {
            get {
                return this.oldenglishField;
            }
            set {
                this.oldenglishField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://hoerstemeier.com/tambon/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://hoerstemeier.com/tambon/", IsNullable=true)]
    [System.Runtime.Serialization.DataContractAttribute(Name="MeetingEntryContainerYear", Namespace="http://hoerstemeier.com/tambon/", IsReference=true)]
    public partial class MeetingEntryContainerYear {
        
        private List<object> itemsField;
        
        private string valueField;
        
        /// <summary>
        /// Creates a new instance of MeetingEntryContainerYear.
        /// </summary>
        public MeetingEntryContainerYear() {
            this.itemsField = new List<object>();
        }
        
        /// <summary>
        /// Auto generated comment tag to suppress XML code documentation warning.
        /// </summary>
        /// <value>
        /// Auto generated value tag to suppress XML code documentation warning.
        /// </value>
        [System.Xml.Serialization.XmlElementAttribute("boardmeeting", typeof(MeetingEntry), Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("month", typeof(MeetingEntryContainerMonth), Order=0)]
        [System.Runtime.Serialization.DataMemberAttribute()]
        public List<object> Items {
            get {
                return this.itemsField;
            }
            set {
                this.itemsField = value;
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute(DataType="gYear")]
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string value {
            get {
                return this.valueField;
            }
            set {
                this.valueField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://hoerstemeier.com/tambon/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://hoerstemeier.com/tambon/", IsNullable=true)]
    [System.Runtime.Serialization.DataContractAttribute(Name="MeetingEntryContainerMonth", Namespace="http://hoerstemeier.com/tambon/", IsReference=true)]
    public partial class MeetingEntryContainerMonth {
        
        private List<MeetingEntry> boardmeetingField;
        
        private string valueField;
        
        /// <summary>
        /// Creates a new instance of MeetingEntryContainerMonth.
        /// </summary>
        public MeetingEntryContainerMonth() {
            this.boardmeetingField = new List<MeetingEntry>();
        }
        
        [System.Xml.Serialization.XmlElementAttribute("boardmeeting", Order=0)]
        [System.Runtime.Serialization.DataMemberAttribute()]
        public List<MeetingEntry> boardmeeting {
            get {
                return this.boardmeetingField;
            }
            set {
                this.boardmeetingField = value;
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute(DataType="gYearMonth")]
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string value {
            get {
                return this.valueField;
            }
            set {
                this.valueField = value;
            }
        }
    }
}

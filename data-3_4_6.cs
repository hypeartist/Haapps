﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Serialization;

// 
// This source code was auto-generated by xsd, Version=4.8.3928.0.
// 


/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
public partial class intrinsics_list {
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("intrinsic", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public intrinsics_listIntrinsic[] intrinsic;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string version;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string date;
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class intrinsics_listIntrinsic {
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string description;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string header;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("type", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true)]
    public intrinsics_listIntrinsicType[] type;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("CPUID", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true)]
    public intrinsics_listIntrinsicCPUID[] CPUID;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("category", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true)]
    public intrinsics_listIntrinsicCategory[] category;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("parameter", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public intrinsics_listIntrinsicParameter[] parameter;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("operation", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true)]
    public intrinsics_listIntrinsicOperation[] operation;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("instruction", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public intrinsics_listIntrinsicInstruction[] instruction;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("perfdata", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public intrinsics_listIntrinsicPerfdata[] perfdata;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string tech;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string rettype;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string name;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string sequence;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string vexEq;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string dontShowZeroUnmodMsg;
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class intrinsics_listIntrinsicType {
    
    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string Value;
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class intrinsics_listIntrinsicCPUID {
    
    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string Value;
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class intrinsics_listIntrinsicCategory {
    
    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string Value;
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class intrinsics_listIntrinsicParameter {
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string varname;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string type;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string hint;
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class intrinsics_listIntrinsicOperation {
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string validate;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string Value;
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class intrinsics_listIntrinsicInstruction {
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string name;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string form;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string xed;
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class intrinsics_listIntrinsicPerfdata {
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string arch;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string lat;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string tpt;
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
public partial class NewDataSet {
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("intrinsics_list")]
    public intrinsics_list[] Items;
}

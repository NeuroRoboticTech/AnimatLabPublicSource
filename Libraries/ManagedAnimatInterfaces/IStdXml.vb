

Public Interface IStdXml

    Sub SetLogger(ByVal oLog As ILogger)

    Function Serialize() As String
    Sub Deserialize(ByVal strXml As String)

    Function IntoElem() As Boolean
    Function OutOfElem() As Boolean
    Function FullTagPath() As String
    Function FullTagPath(ByVal bAddChildName As Boolean) As String
    Function TagName() As String
    Function ChildTagName() As String

    Function NumberOfChildren() As Integer
    Function FindElement(ByVal strElementName As String) As Boolean
    Function FindElement(ByVal strElementName As String, ByVal bThrowError As Boolean) As Boolean
    Function FindChildByIndex(ByVal iIndex As Integer) As Boolean
    Function FindChildByIndex(ByVal iIndex As Integer, ByVal bThrowError As Boolean) As Boolean
    Function FindChildElement(ByVal strElementName As String) As Boolean
    Function FindChildElement(ByVal strElementName As String, ByVal bThrowError As Boolean) As Boolean

    Function IntoChildElement(ByVal strElementName As String) As Boolean
    Function IntoChildElement(ByVal strElementName As String, ByVal bThrowError As Boolean) As Boolean

    Function GetChildString(ByVal strElementName As String) As String
    Function GetChildString(ByVal strElementName As String, ByVal strDefault As String) As String
    Function GetChildString() As String
    Function GetChildLong(ByVal strElementName As String) As Long
    Function GetChildLong(ByVal strElementName As String, ByVal lDefault As Long) As Long
    Function GetChildLong() As Long
    Function GetChildInt(ByVal strElementName As String) As Integer
    Function GetChildInt(ByVal strElementName As String, ByVal iDefault As Integer) As Integer
    Function GetChildInt() As Integer
    Function GetChildDouble(ByVal strElementName As String) As Double
    Function GetChildDouble(ByVal strElementName As String, ByVal dblDefault As Double) As Double
    Function GetChildDouble() As Double
    Function GetChildFloat(ByVal strElementName As String) As Single
    Function GetChildFloat(ByVal strElementName As String, ByVal fltDefault As Single) As Single
    Function GetChildFloat() As Single
    Function GetChildBool(ByVal strElementName As String) As Boolean
    Function GetChildBool(ByVal strElementName As String, ByVal bDefault As Boolean) As Boolean
    Function GetChildBool() As Boolean
    Function GetChildByteArray(ByVal strElementName As String) As Byte()

    Sub AddElement(ByVal strElementName As String)
    Sub AddElement(ByVal strElementName As String, ByVal strData As String)

    Sub AddChildElement(ByVal strElementName As String)
    Sub AddChildElement(ByVal strElementName As String, ByVal strVal As String)
    Sub AddChildElement(ByVal strElementName As String, ByVal lVal As Long)
    Sub AddChildElement(ByVal strElementName As String, ByVal iVal As Integer)
    Sub AddChildElement(ByVal strElementName As String, ByVal dblVal As Double)
    Sub AddChildElement(ByVal strElementName As String, ByVal fltVal As Single)
    Sub AddChildElement(ByVal strElementName As String, ByVal bVal As Boolean)
    Sub AddChildElement(ByVal strElementName As String, ByVal aryData() As Byte)

    Sub AddChildCData(ByVal strElementName As String, ByVal strCData As String)

    Function GetAttribString(ByVal strAttribName As String) As String
    Function GetAttribString(ByVal strAttribName As String, ByVal bCanBeBlank As Boolean) As String
    Function GetAttribString(ByVal strAttribName As String, ByVal bCanBeBlank As Boolean, ByVal bThrowError As Boolean) As String
    Function GetAttribString(ByVal strAttribName As String, ByVal bCanBeBlank As Boolean, ByVal bThrowError As Boolean, ByVal strDefault As String) As String

    Function GetAttribLong(ByVal strAttribName As String) As Long
    Function GetAttribLong(ByVal strAttribName As String, ByVal bThrowError As Boolean) As Long
    Function GetAttribLong(ByVal strAttribName As String, ByVal bThrowError As Boolean, ByVal lDefault As Long) As Long

    Function GetAttribInt(ByVal strAttribName As String) As Integer
    Function GetAttribInt(ByVal strAttribName As String, ByVal bThrowError As Boolean) As Integer
    Function GetAttribInt(ByVal strAttribName As String, ByVal bThrowError As Boolean, ByVal iDefault As Integer) As Integer

    Function GetAttribDouble(ByVal strAttribName As String) As Double
    Function GetAttribDouble(ByVal strAttribName As String, ByVal bThrowError As Boolean) As Double
    Function GetAttribDouble(ByVal strAttribName As String, ByVal bThrowError As Boolean, ByVal dblDefault As Double) As Double

    Function GetAttribFloat(ByVal strAttribName As String) As Single
    Function GetAttribFloat(ByVal strAttribName As String, ByVal bThrowError As Boolean) As Single
    Function GetAttribFloat(ByVal strAttribName As String, ByVal bThrowError As Boolean, ByVal fltDefault As Single) As Single

    Function GetAttribBool(ByVal strAttribName As String) As Boolean
    Function GetAttribBool(ByVal strAttribName As String, ByVal bThrowError As Boolean) As Boolean
    Function GetAttribBool(ByVal strAttribName As String, ByVal bThrowError As Boolean, ByVal bDefault As Boolean) As Boolean

    Sub SetAttrib(ByVal strAttribName As String, ByVal strVal As String)
    Sub SetAttrib(ByVal strAttribName As String, ByVal lVal As Long)
    Sub SetAttrib(ByVal strAttribName As String, ByVal iVal As Integer)
    Sub SetAttrib(ByVal strAttribName As String, ByVal dblVal As Double)
    Sub SetAttrib(ByVal strAttribName As String, ByVal fltVal As Single)
    Sub SetAttrib(ByVal strAttribName As String, ByVal bVal As Boolean)

    Function GetChildAttribString(ByVal strAttribName As String) As String
    Function GetChildAttribString(ByVal strAttribName As String, ByVal bCanBeBlank As Boolean) As String
    Function GetChildAttribString(ByVal strAttribName As String, ByVal bCanBeBlank As Boolean, ByVal bThrowError As Boolean) As String
    Function GetChildAttribString(ByVal strAttribName As String, ByVal bCanBeBlank As Boolean, ByVal bThrowError As Boolean, ByVal strDefault As String) As String

    Function GetChildAttribLong(ByVal strAttribName As String) As Long
    Function GetChildAttribLong(ByVal strAttribName As String, ByVal bThrowError As Boolean) As Long
    Function GetChildAttribLong(ByVal strAttribName As String, ByVal bThrowError As Boolean, ByVal lDefault As Long) As Long

    Function GetChildAttribInt(ByVal strAttribName As String) As Integer
    Function GetChildAttribInt(ByVal strAttribName As String, ByVal bThrowError As Boolean) As Integer
    Function GetChildAttribInt(ByVal strAttribName As String, ByVal bThrowError As Boolean, ByVal iDefault As Integer) As Integer

    Function GetChildAttribDouble(ByVal strAttribName As String) As Double
    Function GetChildAttribDouble(ByVal strAttribName As String, ByVal bThrowError As Boolean) As Double
    Function GetChildAttribDouble(ByVal strAttribName As String, ByVal bThrowError As Boolean, ByVal dblDefault As Double) As Double

    Function GetChildAttribFloat(ByVal strAttribName As String) As Single
    Function GetChildAttribFloat(ByVal strAttribName As String, ByVal bThrowError As Boolean) As Single
    Function GetChildAttribFloat(ByVal strAttribName As String, ByVal bThrowError As Boolean, ByVal fltDefault As Single) As Single

    Function GetChildAttribBool(ByVal strAttribName As String) As Boolean
    Function GetChildAttribBool(ByVal strAttribName As String, ByVal bThrowError As Boolean) As Boolean
    Function GetChildAttribBool(ByVal strAttribName As String, ByVal bThrowError As Boolean, ByVal bDefault As Boolean) As Boolean

    Sub SetChildAttrib(ByVal strAttribName As String, ByVal strVal As String)
    Sub SetChildAttrib(ByVal strAttribName As String, ByVal lVal As Long)
    Sub SetChildAttrib(ByVal strAttribName As String, ByVal iVal As Integer)
    Sub SetChildAttrib(ByVal strAttribName As String, ByVal dblVal As Double)
    Sub SetChildAttrib(ByVal strAttribName As String, ByVal fltVal As Single)
    Sub SetChildAttrib(ByVal strAttribName As String, ByVal bVal As Boolean)

    Sub AddChildDoc(ByVal Doc As String)
    Function GetChildDoc() As String
    Function GetParentTagName() As String

    Sub Load(ByVal strFilename As String)
    Sub Save(ByVal strFilename As String)

End Interface

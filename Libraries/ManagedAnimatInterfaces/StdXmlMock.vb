
Public Class StdXmlMock
    Implements IStdXml


    Public Sub AddChildCData(strElementName As String, strCData As String) Implements IStdXml.AddChildCData

    End Sub

    Public Sub AddChildDoc(Doc As String) Implements IStdXml.AddChildDoc

    End Sub

    Public Sub AddChildElement(strElementName As String) Implements IStdXml.AddChildElement

    End Sub

    Public Sub AddChildElement(strElementName As String, bVal As Boolean) Implements IStdXml.AddChildElement

    End Sub

    Public Sub AddChildElement(strElementName As String, aryData() As Byte) Implements IStdXml.AddChildElement

    End Sub

    Public Sub AddChildElement(strElementName As String, dblVal As Double) Implements IStdXml.AddChildElement

    End Sub

    Public Sub AddChildElement(strElementName As String, iVal As Integer) Implements IStdXml.AddChildElement

    End Sub

    Public Sub AddChildElement(strElementName As String, lVal As Long) Implements IStdXml.AddChildElement

    End Sub

    Public Sub AddChildElement(strElementName As String, fltVal As Single) Implements IStdXml.AddChildElement

    End Sub

    Public Sub AddChildElement(strElementName As String, strVal As String) Implements IStdXml.AddChildElement

    End Sub

    Public Sub AddElement(strElementName As String) Implements IStdXml.AddElement

    End Sub

    Public Sub AddElement(strElementName As String, strData As String) Implements IStdXml.AddElement

    End Sub

    Public Function ChildTagName() As String Implements IStdXml.ChildTagName
        Return ""
    End Function

    Public Sub Deserialize(strXml As String) Implements IStdXml.Deserialize

    End Sub

    Public Function FindChildByIndex(iIndex As Integer) As Boolean Implements IStdXml.FindChildByIndex
        Return False
    End Function

    Public Function FindChildByIndex(iIndex As Integer, bThrowError As Boolean) As Boolean Implements IStdXml.FindChildByIndex
        Return False
    End Function

    Public Function FindChildElement(strElementName As String) As Boolean Implements IStdXml.FindChildElement
        Return False
    End Function

    Public Function FindChildElement(strElementName As String, bThrowError As Boolean) As Boolean Implements IStdXml.FindChildElement
        Return False
    End Function

    Public Function FindElement(strElementName As String) As Boolean Implements IStdXml.FindElement
        Return False
    End Function

    Public Function FindElement(strElementName As String, bThrowError As Boolean) As Boolean Implements IStdXml.FindElement
        Return False
    End Function

    Public Function FullTagPath() As String Implements IStdXml.FullTagPath
        Return ""
    End Function

    Public Function FullTagPath(bAddChildName As Boolean) As String Implements IStdXml.FullTagPath
        Return ""
    End Function

    Public Function GetAttribBool(strAttribName As String) As Boolean Implements IStdXml.GetAttribBool
        Return False
    End Function

    Public Function GetAttribBool(strAttribName As String, bThrowError As Boolean) As Boolean Implements IStdXml.GetAttribBool
        Return False
    End Function

    Public Function GetAttribBool(strAttribName As String, bThrowError As Boolean, bDefault As Boolean) As Boolean Implements IStdXml.GetAttribBool
        Return False
    End Function

    Public Function GetAttribDouble(strAttribName As String) As Double Implements IStdXml.GetAttribDouble
        Return 0
    End Function

    Public Function GetAttribDouble(strAttribName As String, bThrowError As Boolean) As Double Implements IStdXml.GetAttribDouble
        Return 0
    End Function

    Public Function GetAttribDouble(strAttribName As String, bThrowError As Boolean, dblDefault As Double) As Double Implements IStdXml.GetAttribDouble
        Return 0
    End Function

    Public Function GetAttribFloat(strAttribName As String) As Single Implements IStdXml.GetAttribFloat
        Return 0
    End Function

    Public Function GetAttribFloat(strAttribName As String, bThrowError As Boolean) As Single Implements IStdXml.GetAttribFloat
        Return 0
    End Function

    Public Function GetAttribFloat(strAttribName As String, bThrowError As Boolean, fltDefault As Single) As Single Implements IStdXml.GetAttribFloat
        Return 0
    End Function

    Public Function GetAttribInt(strAttribName As String) As Integer Implements IStdXml.GetAttribInt
        Return 0
    End Function

    Public Function GetAttribInt(strAttribName As String, bThrowError As Boolean) As Integer Implements IStdXml.GetAttribInt
        Return 0
    End Function

    Public Function GetAttribInt(strAttribName As String, bThrowError As Boolean, iDefault As Integer) As Integer Implements IStdXml.GetAttribInt
        Return 0
    End Function

    Public Function GetAttribLong(strAttribName As String) As Long Implements IStdXml.GetAttribLong
        Return 0
    End Function

    Public Function GetAttribLong(strAttribName As String, bThrowError As Boolean) As Long Implements IStdXml.GetAttribLong
        Return 0
    End Function

    Public Function GetAttribLong(strAttribName As String, bThrowError As Boolean, lDefault As Long) As Long Implements IStdXml.GetAttribLong
        Return 0
    End Function

    Public Function GetAttribString(strAttribName As String) As String Implements IStdXml.GetAttribString
        Return ""
    End Function

    Public Function GetAttribString(strAttribName As String, bCanBeBlank As Boolean) As String Implements IStdXml.GetAttribString
        Return ""
    End Function

    Public Function GetAttribString(strAttribName As String, bCanBeBlank As Boolean, bThrowError As Boolean) As String Implements IStdXml.GetAttribString
        Return ""
    End Function

    Public Function GetAttribString(strAttribName As String, bCanBeBlank As Boolean, bThrowError As Boolean, strDefault As String) As String Implements IStdXml.GetAttribString
        Return ""
    End Function

    Public Function GetChildAttribBool(strAttribName As String) As Boolean Implements IStdXml.GetChildAttribBool
        Return False
    End Function

    Public Function GetChildAttribBool(strAttribName As String, bThrowError As Boolean) As Boolean Implements IStdXml.GetChildAttribBool
        Return False
    End Function

    Public Function GetChildAttribBool(strAttribName As String, bThrowError As Boolean, bDefault As Boolean) As Boolean Implements IStdXml.GetChildAttribBool
        Return False
    End Function

    Public Function GetChildAttribDouble(strAttribName As String) As Double Implements IStdXml.GetChildAttribDouble
        Return 0
    End Function

    Public Function GetChildAttribDouble(strAttribName As String, bThrowError As Boolean) As Double Implements IStdXml.GetChildAttribDouble
        Return 0
    End Function

    Public Function GetChildAttribDouble(strAttribName As String, bThrowError As Boolean, dblDefault As Double) As Double Implements IStdXml.GetChildAttribDouble
        Return 0
    End Function

    Public Function GetChildAttribFloat(strAttribName As String) As Single Implements IStdXml.GetChildAttribFloat
        Return 0
    End Function

    Public Function GetChildAttribFloat(strAttribName As String, bThrowError As Boolean) As Single Implements IStdXml.GetChildAttribFloat
        Return 0
    End Function

    Public Function GetChildAttribFloat(strAttribName As String, bThrowError As Boolean, fltDefault As Single) As Single Implements IStdXml.GetChildAttribFloat
        Return 0
    End Function

    Public Function GetChildAttribInt(strAttribName As String) As Integer Implements IStdXml.GetChildAttribInt
        Return 0
    End Function

    Public Function GetChildAttribInt(strAttribName As String, bThrowError As Boolean) As Integer Implements IStdXml.GetChildAttribInt
        Return 0
    End Function

    Public Function GetChildAttribInt(strAttribName As String, bThrowError As Boolean, iDefault As Integer) As Integer Implements IStdXml.GetChildAttribInt
        Return 0
    End Function

    Public Function GetChildAttribLong(strAttribName As String) As Long Implements IStdXml.GetChildAttribLong
        Return 0
    End Function

    Public Function GetChildAttribLong(strAttribName As String, bThrowError As Boolean) As Long Implements IStdXml.GetChildAttribLong
        Return 0
    End Function

    Public Function GetChildAttribLong(strAttribName As String, bThrowError As Boolean, lDefault As Long) As Long Implements IStdXml.GetChildAttribLong
        Return 0
    End Function

    Public Function GetChildAttribString(strAttribName As String) As String Implements IStdXml.GetChildAttribString
        Return ""
    End Function

    Public Function GetChildAttribString(strAttribName As String, bCanBeBlank As Boolean) As String Implements IStdXml.GetChildAttribString
        Return ""
    End Function

    Public Function GetChildAttribString(strAttribName As String, bCanBeBlank As Boolean, bThrowError As Boolean) As String Implements IStdXml.GetChildAttribString
        Return ""
    End Function

    Public Function GetChildAttribString(strAttribName As String, bCanBeBlank As Boolean, bThrowError As Boolean, strDefault As String) As String Implements IStdXml.GetChildAttribString
        Return ""
    End Function

    Public Function GetChildBool() As Boolean Implements IStdXml.GetChildBool
        Return False
    End Function

    Public Function GetChildBool(strElementName As String) As Boolean Implements IStdXml.GetChildBool
        Return False
    End Function

    Public Function GetChildBool(strElementName As String, bDefault As Boolean) As Boolean Implements IStdXml.GetChildBool
        Return False
    End Function

    Public Function GetChildByteArray(strElementName As String) As Byte() Implements IStdXml.GetChildByteArray
        Return Nothing
    End Function

    Public Function GetChildDoc() As String Implements IStdXml.GetChildDoc
        Return ""
    End Function

    Public Function GetChildDouble() As Double Implements IStdXml.GetChildDouble
        Return 0
    End Function

    Public Function GetChildDouble(strElementName As String) As Double Implements IStdXml.GetChildDouble
        Return 0
    End Function

    Public Function GetChildDouble(strElementName As String, dblDefault As Double) As Double Implements IStdXml.GetChildDouble
        Return 0
    End Function

    Public Function GetChildFloat() As Single Implements IStdXml.GetChildFloat
        Return 0
    End Function

    Public Function GetChildFloat(strElementName As String) As Single Implements IStdXml.GetChildFloat
        Return 0
    End Function

    Public Function GetChildFloat(strElementName As String, fltDefault As Single) As Single Implements IStdXml.GetChildFloat
        Return 0
    End Function

    Public Function GetChildInt() As Integer Implements IStdXml.GetChildInt
        Return 0
    End Function

    Public Function GetChildInt(strElementName As String) As Integer Implements IStdXml.GetChildInt
        Return 0
    End Function

    Public Function GetChildInt(strElementName As String, iDefault As Integer) As Integer Implements IStdXml.GetChildInt
        Return 0
    End Function

    Public Function GetChildLong() As Long Implements IStdXml.GetChildLong
        Return 0
    End Function

    Public Function GetChildLong(strElementName As String) As Long Implements IStdXml.GetChildLong
        Return 0
    End Function

    Public Function GetChildLong(strElementName As String, lDefault As Long) As Long Implements IStdXml.GetChildLong
        Return 0
    End Function

    Public Function GetChildString() As String Implements IStdXml.GetChildString
        Return ""
    End Function

    Public Function GetChildString(strElementName As String) As String Implements IStdXml.GetChildString
        Return ""
    End Function

    Public Function GetChildString(strElementName As String, strDefault As String) As String Implements IStdXml.GetChildString
        Return ""
    End Function

    Public Function GetParentTagName() As String Implements IStdXml.GetParentTagName
        Return ""
    End Function

    Public Function IntoChildElement(strElementName As String) As Boolean Implements IStdXml.IntoChildElement
        Return False
    End Function

    Public Function IntoChildElement(strElementName As String, bThrowError As Boolean) As Boolean Implements IStdXml.IntoChildElement
        Return False
    End Function

    Public Function IntoElem() As Boolean Implements IStdXml.IntoElem
        Return False
    End Function

    Public Sub Load(strFilename As String) Implements IStdXml.Load

    End Sub

    Public Function NumberOfChildren() As Integer Implements IStdXml.NumberOfChildren
        Return 0
    End Function

    Public Function OutOfElem() As Boolean Implements IStdXml.OutOfElem
        Return False
    End Function

    Public Sub Save(strFilename As String) Implements IStdXml.Save

    End Sub

    Public Function Serialize() As String Implements IStdXml.Serialize
        Return ""
    End Function

    Public Sub SetAttrib(strAttribName As String, bVal As Boolean) Implements IStdXml.SetAttrib

    End Sub

    Public Sub SetAttrib(strAttribName As String, dblVal As Double) Implements IStdXml.SetAttrib

    End Sub

    Public Sub SetAttrib(strAttribName As String, iVal As Integer) Implements IStdXml.SetAttrib

    End Sub

    Public Sub SetAttrib(strAttribName As String, lVal As Long) Implements IStdXml.SetAttrib

    End Sub

    Public Sub SetAttrib(strAttribName As String, fltVal As Single) Implements IStdXml.SetAttrib

    End Sub

    Public Sub SetAttrib(strAttribName As String, strVal As String) Implements IStdXml.SetAttrib

    End Sub

    Public Sub SetChildAttrib(strAttribName As String, bVal As Boolean) Implements IStdXml.SetChildAttrib

    End Sub

    Public Sub SetChildAttrib(strAttribName As String, dblVal As Double) Implements IStdXml.SetChildAttrib

    End Sub

    Public Sub SetChildAttrib(strAttribName As String, iVal As Integer) Implements IStdXml.SetChildAttrib

    End Sub

    Public Sub SetChildAttrib(strAttribName As String, lVal As Long) Implements IStdXml.SetChildAttrib

    End Sub

    Public Sub SetChildAttrib(strAttribName As String, fltVal As Single) Implements IStdXml.SetChildAttrib

    End Sub

    Public Sub SetChildAttrib(strAttribName As String, strVal As String) Implements IStdXml.SetChildAttrib

    End Sub

    Public Sub SetLogger(oLog As ILogger) Implements IStdXml.SetLogger

    End Sub

    Public Function TagName() As String Implements IStdXml.TagName
        Return ""
    End Function

End Class

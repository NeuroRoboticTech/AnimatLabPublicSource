Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO

Namespace Framework

    <TypeConverter(GetType(ScaledNumber.ScaledNumericPropBagConverter))> _
    Public Class ScaledNumber
        Inherits DataObject

#Region " Enums "

        Public Enum enumNumericScale
            Tera = 12
            Giga = 9
            Mega = 6
            Kilo = 3
            None = 0
            centi = -2
            milli = -3
            micro = -6
            nano = -9
            pico = -12
            femto = -15
        End Enum

#End Region

#Region " Attributes "

        Protected m_strPropertyName As String = ""
        Protected m_fltValue As Double
        Protected m_eScale As enumNumericScale
        Protected m_strUnits As String = ""
        Protected m_strUnitsAbbrev As String = ""

        Protected m_eDefaultScale As enumNumericScale
        Protected m_bPropertiesReadOnly As Boolean = False
        Protected m_bIgnoreChangeValueEvents As Boolean = False


#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Property PropertyName() As String
            Get
                Return m_strPropertyName
            End Get
            Set(ByVal Value As String)
                m_strPropertyName = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Property ActualValue() As Double
            Get
                Return m_fltValue * 10 ^ m_eScale
            End Get
            Set(ByVal Value As Double)
                SetFromValue(Value)
            End Set
        End Property

        Public Property ValueManual() As Double
            Get
                Return m_fltValue
            End Get
            Set(ByVal Value As Double)

                m_fltValue = Value
                If Not m_bIgnoreChangeValueEvents Then RaiseEvent ValueChanged()
            End Set
        End Property

        <Browsable(False)> _
        Public Property Value() As Double
            Get
                Return m_fltValue
            End Get
            Set(ByVal Value As Double)

                ''NEED TO FIX
                'I do not remember the purpose behind this bit of code. Not sure what I was trying to do.
                'If I do not remember before testing phase then fix this
                'If Not m_doParent Is Nothing AndAlso m_strPropertyName.Trim.Length > 0 Then
                '    Dim snOld As ScaledNumber = DirectCast(Me.Clone(Me.Parent, False, Nothing), ScaledNumber)
                '    snOld.m_fltValue = Value
                '    If Not UpdateParent(snOld) Then
                '        m_fltValue = Value
                '    End If
                'Else
                '    m_fltValue = Value
                'End If

                m_fltValue = Value
                If Not m_bIgnoreChangeValueEvents Then RaiseEvent ValueChanged()

                'Dim snNew As ScaledNumber = DirectCast(Me.Clone, ScaledNumber)
                'Me.ManualAddPropertyHistory(Me.Parent, m_strPropertyName, snOld, snNew, True)
            End Set
        End Property

        Public Property ScaleManual() As enumNumericScale
            Get
                Return m_eScale
            End Get
            Set(ByVal Value As enumNumericScale)
                m_eScale = Value
            End Set
        End Property

        Public Property Scale() As enumNumericScale
            Get
                Return m_eScale
            End Get
            Set(ByVal Value As enumNumericScale)

                If Not m_doParent Is Nothing AndAlso m_strPropertyName.Trim.Length > 0 Then
                    Dim snOld As ScaledNumber = DirectCast(Me.Clone(Me.Parent, False, Nothing), ScaledNumber)
                    snOld.m_eScale = Value
                    If Not UpdateParent(snOld) Then
                        m_eScale = Value
                    End If
                Else
                    m_eScale = Value
                End If

                'Dim snOld As ScaledNumber = DirectCast(Me.Clone(), ScaledNumber)

                'm_eScale = Value

                'Dim snNew As ScaledNumber = DirectCast(Me.Clone, ScaledNumber)
                'Me.ManualAddPropertyHistory(Me.Parent, m_strPropertyName, snOld, snNew, True)
            End Set
        End Property

        Public ReadOnly Property Units() As String
            Get
                Return m_strUnits
            End Get
        End Property

        Public ReadOnly Property UnitsAbbreviation() As String
            Get
                Return m_strUnitsAbbrev
            End Get
        End Property

        <Browsable(False)> _
        Public ReadOnly Property Text() As String
            Get
                Return m_fltValue.ToString("0.###") & " " & ScaledNumber.ScaleAbbreviation(m_eScale) & m_strUnitsAbbrev
            End Get
        End Property

        <Browsable(False)> _
        Public ReadOnly Property ValueFromDefaultScale() As Double
            Get
                Return ActualValue / (10 ^ m_eDefaultScale)
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property RootForm() As System.Windows.Forms.Form
            Get
                If Not m_doParent Is Nothing Then
                    Return m_doParent.RootForm
                Else
                    Return Util.Application
                End If
            End Get
        End Property

        Public Overridable Property PropertiesReadOnly() As Boolean
            Get
                Return m_bPropertiesReadOnly
            End Get
            Set(ByVal value As Boolean)
                m_bPropertiesReadOnly = value
            End Set
        End Property

        Public Overridable Property IgnoreChangeValueEvents() As Boolean
            Get
                Return m_bIgnoreChangeValueEvents
            End Get
            Set(ByVal value As Boolean)
                m_bIgnoreChangeValueEvents = value
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            m_eScale = enumNumericScale.None
            m_eDefaultScale = enumNumericScale.None
        End Sub

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal dblActualVal As Double)
            MyBase.New(doParent)

            Me.ActualValue = dblActualVal
        End Sub

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal dblActualVal As Double, ByVal eScale As enumNumericScale)
            MyBase.New(doParent)

            Me.SetFromValue(dblActualVal, eScale)
        End Sub

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal strPropertyName As String, ByVal fltValue As Double, ByVal eScale As enumNumericScale)
            MyBase.New(doParent)

            m_strPropertyName = strPropertyName
            m_fltValue = fltValue
            m_eScale = eScale
            m_eDefaultScale = eScale
        End Sub

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal strPropertyName As String, ByVal fltValue As Double, ByVal eScale As enumNumericScale, ByVal strUnits As String, ByVal strUnitsAbbrev As String)
            MyBase.New(doParent)

            m_strPropertyName = strPropertyName
            m_fltValue = fltValue
            m_eScale = eScale
            m_strUnits = strUnits
            m_strUnitsAbbrev = strUnitsAbbrev
            m_eDefaultScale = eScale
        End Sub

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal strPropertyName As String, ByVal strUnits As String, ByVal strUnitsAbbrev As String)
            MyBase.New(doParent)

            m_strPropertyName = strPropertyName
            m_eScale = enumNumericScale.None
            m_strUnits = strUnits
            m_strUnitsAbbrev = strUnitsAbbrev
            m_eDefaultScale = enumNumericScale.None
        End Sub

        Public Sub SetScaleUnits(ByVal strUnits As String, ByVal strUnitsAbbrev As String)
            m_strUnits = strUnits
            m_strUnitsAbbrev = strUnitsAbbrev
        End Sub

        Public Sub SetFromValue(ByVal fltActualValue As Single)
            SetFromValue(CDbl(fltActualValue))
        End Sub

        Public Sub SetFromValue(ByVal dblActualValue As Double)

            Dim dblNewVal As Double

            If dblActualValue <> 0 Then
                Dim iMinUnit As Integer = -1000
                Dim dblMinVal As Double

                For Each iUnit As Integer In [Enum].GetValues(m_eScale.GetType)

                    dblNewVal = Math.Abs(dblActualValue / (10 ^ iUnit))

                    If Math.Log10(dblNewVal) >= 0 And Math.Log10(dblNewVal) < 2 Then
                        m_fltValue = Math.Round(dblActualValue / (10 ^ iUnit), 3)
                        m_eScale = CType(iUnit, enumNumericScale)
                        If Not m_bIgnoreChangeValueEvents Then RaiseEvent ValueChanged()
                        Return
                    End If

                    If iMinUnit = -1000 OrElse Math.Abs(Math.Log10(dblNewVal)) < dblMinVal Then
                        iMinUnit = iUnit
                        dblMinVal = Math.Abs(Math.Log10(dblNewVal))
                    End If
                Next

                SetFromValue(dblActualValue, iMinUnit)
            Else
                m_fltValue = 0
                m_eScale = enumNumericScale.None
                If Not m_bIgnoreChangeValueEvents Then RaiseEvent ValueChanged()
            End If

        End Sub

        Public Sub SetFromValue(ByVal fltActualValue As Single, ByVal eUsingScale As ScaledNumber.enumNumericScale)
            SetFromValue(CDbl(fltActualValue), eUsingScale)
        End Sub

        Public Class myPointSorter
            Implements IComparer

            ' Calls CaseInsensitiveComparer.Compare with the parameters reversed.
            Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
                Dim ptX As Point = DirectCast(x, Point)
                Dim ptY As Point = DirectCast(y, Point)

                Return ptX.X - ptY.X
            End Function 'IComparer.Compare

        End Class 'myReverserClass

        Protected Function FindClosestUnit(ByVal iTest As Integer) As ScaledNumber.enumNumericScale

            Dim aryValues As System.Array = [Enum].GetValues(m_eScale.GetType)
            Dim aryDiff As New ArrayList

            Dim iIndex As Integer
            For Each iVal As Integer In aryValues
                aryDiff.Add(New Point(iVal - iTest, iVal))

                'If we find one that matches exactly then send it back.
                If iVal - iTest = 0 Then
                    Return CType(iVal, ScaledNumber.enumNumericScale)
                End If

                iIndex = iIndex + 1
            Next

            aryDiff.Sort(New myPointSorter)

            Dim ptPrev As Point = DirectCast(aryDiff(0), Point)
            Dim ptVal As Point
            For iIndex = 1 To aryDiff.Count - 1
                ptVal = DirectCast(aryDiff(iIndex), Point)
                If ptVal.X > 0 Then
                    Return CType(ptPrev.Y, ScaledNumber.enumNumericScale)
                End If
                ptPrev = ptVal
            Next

            Return enumNumericScale.None
        End Function

        Public Sub SetFromValue(ByVal dblActualValue As Double, ByVal eUsingScale As ScaledNumber.enumNumericScale)

            Dim dblNewVal As Double

            If dblActualValue <> 0 Then
                Dim iUnit As Integer = FindClosestUnit(CInt(eUsingScale))

                dblNewVal = Math.Abs(dblActualValue / (10 ^ iUnit))
                m_fltValue = Math.Round(dblActualValue / (10 ^ iUnit), 3)
                m_eScale = CType(iUnit, enumNumericScale)
                If Not m_bIgnoreChangeValueEvents Then RaiseEvent ValueChanged()
                Return
            Else
                m_fltValue = 0
                m_eScale = FindClosestUnit(CInt(eUsingScale))
                If Not m_bIgnoreChangeValueEvents Then RaiseEvent ValueChanged()
            End If

        End Sub

        Public Sub SetFromValue(ByVal dblActualValue As Double, ByVal iUsingScale As Integer)
            SetFromValue(dblActualValue, CType(iUsingScale, ScaledNumber.enumNumericScale))
        End Sub

        Public Sub SetFromValue(ByVal fltActualValue As Single, ByVal iUsingScale As Integer)
            SetFromValue(CDbl(fltActualValue), CType(iUsingScale, ScaledNumber.enumNumericScale))
        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Value", m_fltValue.GetType(), "Value", _
                                        "Link Properties", "Sets the base value of this parameter.", m_fltValue, m_bPropertiesReadOnly))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Scale", m_eScale.GetType(), "Scale", _
                                        "Link Properties", "Sets the numeric scale for this parameter. (Kilo, Micro, etc)", m_eScale, m_bPropertiesReadOnly))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Units", m_strUnits.GetType(), "Units", _
                                        "Link Properties", "The units for this parameter", m_strUnits, True))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Units Abbrev", m_strUnitsAbbrev.GetType(), "UnitsAbbreviation", _
                                        "Link Properties", "The units abbreviation for this parameter", m_strUnitsAbbrev, True))

        End Sub

        Public Shared Function ScaleAbbreviation(ByVal eScale As enumNumericScale) As String

            Select Case eScale
                Case enumNumericScale.Tera
                    Return "T"
                Case enumNumericScale.Giga
                    Return "G"
                Case enumNumericScale.Mega
                    Return "M"
                Case enumNumericScale.Kilo
                    Return "K"
                Case enumNumericScale.None
                    Return ""
                Case enumNumericScale.centi
                    Return "c"
                Case enumNumericScale.milli
                    Return "m"
                Case enumNumericScale.micro
                    Return "u"
                Case enumNumericScale.nano
                    Return "n"
                Case enumNumericScale.pico
                    Return "p"
                Case enumNumericScale.femto
                    Return "f"
            End Select

            Return ""
        End Function

        Public Shared Function ScaleFromAbbreviation(ByVal strAbbrev As String) As enumNumericScale

            Select Case strAbbrev
                Case "T", "t"
                    Return enumNumericScale.Tera
                Case "G", "g"
                    Return enumNumericScale.Giga
                Case "M"
                    Return enumNumericScale.Mega
                Case "K", "k"
                    Return enumNumericScale.Kilo
                Case "C", "c"
                    Return enumNumericScale.centi
                Case "m"
                    Return enumNumericScale.milli
                Case "u", "U"
                    Return enumNumericScale.micro
                Case "n", "N"
                    Return enumNumericScale.nano
                Case "p", "P"
                    Return enumNumericScale.pico
                Case "f", "F"
                    Return enumNumericScale.femto
            End Select

        End Function

        Public Sub CopyData(ByRef snValue As ScaledNumber, Optional ByVal bIgnoreEvents As Boolean = False, Optional ByVal bSetIsDirty As Boolean = True)
            Me.m_fltValue = snValue.m_fltValue
            Me.m_eScale = snValue.m_eScale
            If bSetIsDirty Then
                Me.IsDirty = True
            End If
            If Not bIgnoreEvents AndAlso Not m_bIgnoreChangeValueEvents Then RaiseEvent ValueChanged()
        End Sub

        Public Sub CopyData(ByVal fltValue As Single, ByVal eScale As AnimatGUI.Framework.ScaledNumber.enumNumericScale, Optional ByVal bIgnoreEvents As Boolean = False, Optional ByVal bSetIsDirty As Boolean = True)
            Me.m_fltValue = fltValue
            Me.m_eScale = eScale
            If bSetIsDirty Then
                Me.IsDirty = True
            End If
            If Not bIgnoreEvents AndAlso Not m_bIgnoreChangeValueEvents Then RaiseEvent ValueChanged()
        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNew As New ScaledNumber(doParent)

            oNew.m_strPropertyName = Me.m_strPropertyName
            oNew.m_fltValue = Me.m_fltValue
            oNew.m_eScale = Me.m_eScale
            oNew.m_strUnits = Me.m_strUnits
            oNew.m_strUnitsAbbrev = Me.m_strUnitsAbbrev
            oNew.m_eDefaultScale = Me.m_eDefaultScale

            Return oNew
        End Function

        Public Overloads Sub LoadData(ByRef oXml As AnimatGUI.Interfaces.StdXml, ByVal strName As String, Optional ByVal bThrowError As Boolean = True)
            If oXml.FindChildElement(strName, False) Then
                oXml.IntoChildElement(strName)
                m_fltValue = oXml.GetAttribFloat("Value")
                m_strPropertyName = strName

                Dim strScale As String = oXml.GetAttribString("Scale")

                If [Enum].IsDefined(GetType(enumNumericScale), strScale) Then
                    m_eScale = DirectCast([Enum].Parse(GetType(enumNumericScale), strScale, True), enumNumericScale)
                Else
                    Dim fltVal As Double = oXml.GetAttribDouble("Actual", False, m_fltValue)
                    Me.ActualValue = fltVal
                End If

                oXml.OutOfElem()
            ElseIf bThrowError Then
                Throw New System.Exception("No xml tag with the name '" & strName & "' was found.")
            End If
        End Sub

        Public Overloads Sub SaveData(ByRef oXml As AnimatGUI.Interfaces.StdXml, ByVal strName As String)
            oXml.AddChildElement(strName)
            oXml.IntoElem()
            oXml.SetAttrib("Value", m_fltValue)
            oXml.SetAttrib("Scale", m_eScale.ToString())
            oXml.SetAttrib("Actual", Me.ActualValue)
            oXml.OutOfElem()
        End Sub

        Public Overrides Sub SaveSimulationXml(ByRef oXml As Interfaces.StdXml, Optional ByRef nmParentControl As DataObject = Nothing, Optional ByVal strName As String = "")
            oXml.AddChildElement(strName, Me.ActualValue)
        End Sub

        Public Shared Function IsValidXml(ByVal oXml As AnimatGUI.Interfaces.StdXml, ByVal strName As String) As Boolean
            Dim bRetVal As Boolean = False

            If oXml.FindChildElement(strName, False) Then
                oXml.IntoElem()

                Try
                    Dim fltVal As Single = oXml.GetAttribFloat("Value")
                    Dim strScale As String = oXml.GetAttribString("Scale")

                    bRetVal = True

                Catch ex As System.Exception
                End Try

                oXml.OutOfElem()
            End If

            Return bRetVal
        End Function

        Protected Overridable Function UpdateParent(ByVal snNewValue As ScaledNumber) As Boolean
            Dim propInfo As System.Reflection.PropertyInfo
            Dim doHelper As AnimatGUI.DataObjects.FormHelper

            If Not TypeOf m_doParent Is AnimatGUI.DataObjects.FormHelper Then
                propInfo = m_doParent.GetType().GetProperty(m_strPropertyName)
            Else
                doHelper = DirectCast(m_doParent, AnimatGUI.DataObjects.FormHelper)
                propInfo = doHelper.AnimatParent.GetType().GetProperty(m_strPropertyName)
            End If

            If Not propInfo Is Nothing Then
                Dim doOrig As ScaledNumber = DirectCast(Me.Clone(Me.Parent, False, Nothing), ScaledNumber)

                If Not TypeOf m_doParent Is AnimatGUI.DataObjects.FormHelper Then
                    propInfo.SetValue(m_doParent, snNewValue, Nothing)
                    Me.ManualAddPropertyHistory(m_doParent, m_strPropertyName, doOrig, snNewValue, True)
                Else
                    Dim frmParent As AnimatGUI.Forms.AnimatForm = doHelper.AnimatParent
                    propInfo.SetValue(frmParent, snNewValue, Nothing)
                    Me.ManualAddPropertyHistory(m_doParent, m_strPropertyName, doOrig, snNewValue, True)
                End If

                Dim frmRoot As System.Windows.Forms.Form = Me.RootForm
                If Not frmRoot Is Nothing AndAlso TypeOf frmRoot Is AnimatGUI.Forms.MdiChild Then
                    Dim mdiRoot As AnimatGUI.Forms.MdiChild = DirectCast(frmRoot, AnimatGUI.Forms.MdiChild)
                    mdiRoot.RefreshProperties()
                End If

                Return True
            End If

            Return False
        End Function

        Public Shared Function Parse(ByVal value As String, ByVal strOrigUnitsAbbrev As String, ByVal eOrigScale As ScaledNumber.enumNumericScale) As ScaledNumber
            Dim strValue As String = DirectCast(value, String)
            Dim snValue As ScaledNumber
            Dim strOrigUnits As String = ""

            If strValue.Trim.Length = 0 Then
                Throw New System.Exception("Cannot parse an empty string")
            Else
                Dim aryParts() As String = Split(strValue, " ")
                Dim iVal As Integer = UBound(aryParts)

                If UBound(aryParts) >= 0 Then
                    If IsNumeric(aryParts(0)) Then
                        If UBound(aryParts) >= 1 Then
                            Dim strScale As String = aryParts(1)

                            strOrigUnits = strOrigUnitsAbbrev

                            If Right(strScale, strOrigUnits.Length).ToUpper() = strOrigUnits.ToUpper() Then
                                strScale = Left(strScale, strScale.Length - strOrigUnits.Length)
                            End If

                            'If there is more than one char then it can not be a scale.
                            If strScale.Length > 1 Then
                                strScale = ""
                            End If

                            snValue = New ScaledNumber(Nothing, "", Convert.ToDouble(aryParts(0)), _
                                                        ScaledNumber.ScaleFromAbbreviation(strScale))
                        Else
                            Dim eScale As ScaledNumber.enumNumericScale
                            eScale = eOrigScale

                            snValue = New ScaledNumber(Nothing, "", Convert.ToDouble(aryParts(0)), eScale)
                        End If
                    Else
                        Throw New System.Exception("You must specify a numeric value for this property.")
                    End If
                End If
            End If

            Return snValue
        End Function

        Public Overrides Function ToString() As String
            Return Me.ActualValue.ToString()
        End Function ' ToString

#End Region

#Region " Events "

        Public Event ValueChanged()

#End Region


#Region " ScaledNumericPropBagConverter "

        Public Class ScaledNumericPropBagConverter
            Inherits ExpandableObjectConverter

            Public Overloads Overrides Function CanConvertFrom(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal sourceType As System.Type) As Boolean

                If sourceType Is GetType(String) Then
                    Return True
                End If

                Return MyBase.CanConvertFrom(context, sourceType)
            End Function

            Public Overloads Overrides Function CanConvertTo(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal destinationType As System.Type) As Boolean

                If destinationType Is GetType(AnimatGuiCtrls.Controls.PropertyBag) Then
                    Return True
                End If
                Return MyBase.CanConvertTo(context, destinationType)

            End Function

            Protected Function GetUnitsFromPropTable(ByVal context As System.ComponentModel.ITypeDescriptorContext) As String

                Dim pbOuter As AnimatGuiCtrls.Controls.PropertyTable = DirectCast(context.Instance, AnimatGuiCtrls.Controls.PropertyTable)

                For Each propSpec As AnimatGuiCtrls.Controls.PropertySpec In pbOuter.Properties
                    If (propSpec.Category.Replace(" ", "").ToUpper() = context.PropertyDescriptor.Category.Replace(" ", "").ToUpper()) AndAlso _
                       (propSpec.Name.Replace(" ", "").ToUpper() = context.PropertyDescriptor.Name.Replace(" ", "").ToUpper()) Then
                        If TypeOf propSpec.DefaultValue Is AnimatGuiCtrls.Controls.PropertyTable Then
                            Dim pbNumber As AnimatGuiCtrls.Controls.PropertyTable = DirectCast(propSpec.DefaultValue, AnimatGuiCtrls.Controls.PropertyTable)

                            Dim psUnits As AnimatGuiCtrls.Controls.PropertySpec = DirectCast(pbNumber.Properties(3), AnimatGuiCtrls.Controls.PropertySpec)

                            If TypeOf psUnits.DefaultValue Is String Then
                                Return DirectCast(psUnits.DefaultValue, String)
                            Else
                                Return ""
                            End If
                        End If
                    End If
                Next

                Return ""
            End Function

            Protected Function GetUnitsFromDataObject(ByVal context As System.ComponentModel.ITypeDescriptorContext) As String

                Dim doData As AnimatGUI.Framework.DataObject = DirectCast(context.Instance, AnimatGUI.Framework.DataObject)
                Dim propInfo As System.Reflection.PropertyInfo = doData.GetType().GetProperty(context.PropertyDescriptor.Name)
                Dim oVal As Object
                Dim strUnits As String = ""

                If Not propInfo Is Nothing Then
                    If propInfo.CanRead Then
                        oVal = propInfo.GetValue(doData, Nothing)

                        If TypeOf oVal Is ScaledNumber Then
                            Dim snNumber As ScaledNumber = DirectCast(oVal, ScaledNumber)
                            strUnits = snNumber.UnitsAbbreviation
                        End If
                    End If
                End If

                Return strUnits
            End Function

            Protected Function GetScaleFromPropTable(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal oObject As Object) As ScaledNumber.enumNumericScale

                Dim pbOuter As AnimatGuiCtrls.Controls.PropertyTable = DirectCast(oObject, AnimatGuiCtrls.Controls.PropertyTable)

                For Each propSpec As AnimatGuiCtrls.Controls.PropertySpec In pbOuter.Properties
                    If (propSpec.Category.Replace(" ", "").ToUpper() = context.PropertyDescriptor.Category.Replace(" ", "").ToUpper()) AndAlso _
                       (propSpec.Name.Replace(" ", "").ToUpper() = context.PropertyDescriptor.Name.Replace(" ", "").ToUpper()) Then
                        If TypeOf propSpec.DefaultValue Is AnimatGuiCtrls.Controls.PropertyTable Then
                            Dim pbNumber As AnimatGuiCtrls.Controls.PropertyTable = DirectCast(propSpec.DefaultValue, AnimatGuiCtrls.Controls.PropertyTable)

                            If Not pbNumber.Tag Is Nothing AndAlso TypeOf pbNumber.Tag Is ScaledNumber Then
                                Dim snTag As ScaledNumber = DirectCast(pbNumber.Tag, ScaledNumber)
                                Return snTag.Scale
                            Else
                                Return enumNumericScale.None
                            End If
                        End If
                    End If
                Next

                Return enumNumericScale.None
            End Function

            Protected Function GetScaleFromDataObject(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal oObject As Object) As ScaledNumber.enumNumericScale

                Dim doData As AnimatGUI.Framework.DataObject = DirectCast(oObject, AnimatGUI.Framework.DataObject)
                Dim propInfo As System.Reflection.PropertyInfo = doData.GetType().GetProperty(context.PropertyDescriptor.Name)
                Dim oVal As Object
                Dim eScale As ScaledNumber.enumNumericScale = enumNumericScale.None

                If Not propInfo Is Nothing Then
                    If propInfo.CanRead Then
                        oVal = propInfo.GetValue(doData, Nothing)

                        If TypeOf oVal Is ScaledNumber Then
                            Dim snNumber As ScaledNumber = DirectCast(oVal, ScaledNumber)
                            eScale = snNumber.Scale
                        End If
                    End If
                End If

                Return eScale
            End Function

            Public Overloads Overrides Function ConvertFrom(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal culture As System.Globalization.CultureInfo, ByVal value As Object) As Object
                If TypeOf (value) Is String Then
                    Dim strValue As String = DirectCast(value, String)
                    Dim snValue As ScaledNumber
                    Dim strOrigUnits As String = ""

                    If strValue.Trim.Length = 0 Then
                        snValue = New ScaledNumber(Nothing, "", 0, enumNumericScale.None)
                    Else
                        Dim aryParts() As String = Split(strValue, " ")
                        Dim iVal As Integer = UBound(aryParts)

                        If UBound(aryParts) >= 0 Then
                            If IsNumeric(aryParts(0)) Then
                                If UBound(aryParts) >= 1 Then
                                    Dim strScale As String = aryParts(1)

                                    If Not context Is Nothing AndAlso Not context.Instance Is Nothing Then
                                        If TypeOf context.Instance Is AnimatGuiCtrls.Controls.PropertyTable Then
                                            strOrigUnits = GetUnitsFromPropTable(context)
                                        ElseIf TypeOf context.Instance Is DataObject Then
                                            strOrigUnits = GetUnitsFromDataObject(context)
                                        End If
                                    End If

                                    If Right(strScale, strOrigUnits.Length).ToUpper() = strOrigUnits.ToUpper() Then
                                        strScale = Left(strScale, strScale.Length - strOrigUnits.Length)
                                    End If

                                    'If there is more than one char then it can not be a scale.
                                    If strScale.Length > 1 Then
                                        strScale = ""
                                    End If

                                    snValue = New ScaledNumber(Nothing, "", Convert.ToDouble(aryParts(0)), _
                                                              ScaledNumber.ScaleFromAbbreviation(strScale))
                                Else
                                    Dim eScale As ScaledNumber.enumNumericScale

                                    If Not context Is Nothing AndAlso Not context.Instance Is Nothing Then
                                        If TypeOf context.Instance Is AnimatGuiCtrls.Controls.PropertyTable Then
                                            eScale = GetScaleFromPropTable(context, context.Instance)
                                        ElseIf TypeOf context.Instance Is DataObject Then
                                            eScale = GetScaleFromDataObject(context, context.Instance)
                                        ElseIf TypeOf context.Instance Is System.Array Then
                                            Dim aryData As System.Array = DirectCast(context.Instance, System.Array)
                                            If aryData.Length > 0 Then
                                                Dim oVal As Object = aryData.GetValue(0)
                                                If TypeOf oVal Is AnimatGuiCtrls.Controls.PropertyTable Then
                                                    eScale = GetScaleFromPropTable(context, oVal)
                                                ElseIf TypeOf oVal Is DataObject Then
                                                    eScale = GetScaleFromDataObject(context, oVal)
                                                End If
                                            End If
                                        End If
                                    Else
                                        eScale = enumNumericScale.None
                                    End If

                                    snValue = New ScaledNumber(Nothing, "", Convert.ToDouble(aryParts(0)), eScale)
                                End If
                            Else
                                Throw New System.Exception("You must specify a numeric value for this property.")
                            End If
                        End If

                    End If

                    Return snValue
                End If

                Return MyBase.ConvertFrom(context, culture, value)
            End Function

            Public Overloads Overrides Function ConvertTo(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal culture As System.Globalization.CultureInfo, ByVal value As Object, ByVal destinationType As System.Type) As Object
                If destinationType Is GetType(String) AndAlso TypeOf (value) Is AnimatGuiCtrls.Controls.PropertyTable Then
                    Dim pbValue As AnimatGuiCtrls.Controls.PropertyTable = DirectCast(value, AnimatGuiCtrls.Controls.PropertyTable)

                    Dim psValue As AnimatGuiCtrls.Controls.PropertySpec = DirectCast(pbValue.Properties(0), AnimatGuiCtrls.Controls.PropertySpec)
                    Dim psScale As AnimatGuiCtrls.Controls.PropertySpec = DirectCast(pbValue.Properties(1), AnimatGuiCtrls.Controls.PropertySpec)
                    Dim psUnits As AnimatGuiCtrls.Controls.PropertySpec = DirectCast(pbValue.Properties(2), AnimatGuiCtrls.Controls.PropertySpec)
                    Dim psUnitsAbbrev As AnimatGuiCtrls.Controls.PropertySpec = DirectCast(pbValue.Properties(3), AnimatGuiCtrls.Controls.PropertySpec)

                    Dim eScale As enumNumericScale = DirectCast(psScale.DefaultValue, enumNumericScale)
                    Dim strUnits As String = DirectCast(psUnitsAbbrev.DefaultValue, String)

                    Dim strValue As String = Convert.ToDouble(psValue.DefaultValue).ToString("0.###") & " " & _
                                             ScaledNumber.ScaleAbbreviation(eScale) & strUnits

                    Return strValue
                ElseIf destinationType Is GetType(String) AndAlso TypeOf (value) Is AnimatGUI.Framework.ScaledNumber Then
                    Dim snValue As ScaledNumber = DirectCast(value, ScaledNumber)

                    Dim strValue As String = snValue.Value.ToString("0.###") & " " & _
                                             ScaledNumber.ScaleAbbreviation(snValue.Scale) & snValue.UnitsAbbreviation

                    Return strValue
                End If

                Return MyBase.ConvertTo(context, culture, value, destinationType)
            End Function

        End Class

#End Region

    End Class

End Namespace

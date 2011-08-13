Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO

Namespace Framework

    Public Class ScaledVector3
        Inherits DataObject

#Region " Attributes "

        Protected m_strPropName As String = ""
        Protected m_strDescription As String = ""
        Protected m_snX As ScaledNumber
        Protected m_snY As ScaledNumber
        Protected m_snZ As ScaledNumber
        Protected m_bPropertiesReadOnly As Boolean = False
        Protected m_bInsideCopyData As Boolean = False
        Protected m_bIgnoreChangeValueEvents As Boolean = False

#End Region

#Region " Properties "

        Public Overridable Property X() As ScaledNumber
            Get
                Return m_snX
            End Get
            Set(ByVal value As ScaledNumber)
                'SetSimData("TimeStep", value.ActualValue.ToString, True)
                m_snX.CopyData(value)
            End Set
        End Property

        Public Overridable Property Y() As ScaledNumber
            Get
                Return m_snY
            End Get
            Set(ByVal value As ScaledNumber)
                'SetSimData("TimeStep", value.ActualValue.ToString, True)
                m_snY.CopyData(value)
            End Set
        End Property

        Public Overridable Property Z() As ScaledNumber
            Get
                Return m_snZ
            End Get
            Set(ByVal value As ScaledNumber)
                'SetSimData("TimeStep", value.ActualValue.ToString, True)
                m_snZ.CopyData(value)
            End Set
        End Property

        Public Overridable Property PropertiesReadOnly() As Boolean
            Get
                Return m_bPropertiesReadOnly
            End Get
            Set(ByVal value As Boolean)
                m_bPropertiesReadOnly = value
                m_snX.PropertiesReadOnly = value
                m_snY.PropertiesReadOnly = value
                m_snZ.PropertiesReadOnly = value
            End Set
        End Property

        Public Overridable Property IgnoreChangeValueEvents() As Boolean
            Get
                Return m_bIgnoreChangeValueEvents
            End Get
            Set(ByVal value As Boolean)
                m_bIgnoreChangeValueEvents = value
                m_snX.IgnoreChangeValueEvents = value
                m_snY.IgnoreChangeValueEvents = value
                m_snZ.IgnoreChangeValueEvents = value
            End Set
        End Property

#End Region

#Region " Methods "

#End Region

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal strPropName As String, ByVal strDescription As String, ByVal strUnits As String, ByVal strUnitAbbrev As String)
            MyBase.New(doParent)

            m_strPropName = strPropName
            m_strDescription = strDescription
            m_snX = New ScaledNumber(Me, "X", 0, ScaledNumber.enumNumericScale.None, strUnits, strUnitAbbrev)
            m_snY = New ScaledNumber(Me, "Y", 0, ScaledNumber.enumNumericScale.None, strUnits, strUnitAbbrev)
            m_snZ = New ScaledNumber(Me, "Z", 0, ScaledNumber.enumNumericScale.None, strUnits, strUnitAbbrev)

            AddHandler m_snX.ValueChanged, AddressOf Me.OnValueChanged
            AddHandler m_snY.ValueChanged, AddressOf Me.OnValueChanged
            AddHandler m_snZ.ValueChanged, AddressOf Me.OnValueChanged

            AddHandler m_snX.ValueChanging, AddressOf Me.OnXValueChanging
            AddHandler m_snY.ValueChanging, AddressOf Me.OnYValueChanging
            AddHandler m_snZ.ValueChanging, AddressOf Me.OnZValueChanging

        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

            m_snX.PropertiesReadOnly = m_bPropertiesReadOnly
            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = m_snX.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("X", pbNumberBag.GetType(), "X", _
                                        m_strPropName, m_strDescription, pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter), m_bPropertiesReadOnly))

            m_snY.PropertiesReadOnly = m_bPropertiesReadOnly
            pbNumberBag = m_snY.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Y", pbNumberBag.GetType(), "Y", _
                                        m_strPropName, m_strDescription, pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter), m_bPropertiesReadOnly))

            m_snZ.PropertiesReadOnly = m_bPropertiesReadOnly
            pbNumberBag = m_snZ.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Z", pbNumberBag.GetType(), "Z", _
                                        m_strPropName, m_strDescription, pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter), m_bPropertiesReadOnly))
        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNew As New Framework.ScaledVector3(doParent, m_strPropName, m_strDescription, m_snX.Units, m_snX.UnitsAbbreviation)
            oNew.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNew.AfterClone(Me, bCutData, doRoot, oNew)
            Return oNew
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As DataObject, ByVal bCutData As Boolean, ByVal doRoot As DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim svOrig As Framework.ScaledVector3 = DirectCast(doOriginal, Framework.ScaledVector3)
            m_snX = DirectCast(svOrig.m_snX.Clone(Me, bCutData, doRoot), Framework.ScaledNumber)
            m_snY = DirectCast(svOrig.m_snY.Clone(Me, bCutData, doRoot), Framework.ScaledNumber)
            m_snZ = DirectCast(svOrig.m_snZ.Clone(Me, bCutData, doRoot), Framework.ScaledNumber)

            AddHandler m_snX.ValueChanged, AddressOf Me.OnValueChanged
            AddHandler m_snY.ValueChanged, AddressOf Me.OnValueChanged
            AddHandler m_snZ.ValueChanged, AddressOf Me.OnValueChanged

            AddHandler m_snX.ValueChanging, AddressOf Me.OnXValueChanging
            AddHandler m_snY.ValueChanging, AddressOf Me.OnYValueChanging
            AddHandler m_snZ.ValueChanging, AddressOf Me.OnZValueChanging

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()
            m_snX.ClearIsDirty()
            m_snY.ClearIsDirty()
            m_snZ.ClearIsDirty()
        End Sub

        Public Overridable Overloads Sub CopyData(ByVal dblX As Double, ByVal dblY As Double, ByVal dblZ As Double, Optional ByVal bIgnoreEvents As Boolean = True, Optional ByVal bSetIsDirty As Boolean = True)
            CopyData(CSng(dblX), CSng(dblY), CSng(dblZ), bIgnoreEvents, bSetIsDirty)
        End Sub

        Public Overridable Overloads Sub CopyData(ByVal fltX As Single, ByVal fltY As Single, ByVal fltZ As Single, Optional ByVal bIgnoreEvents As Boolean = True, Optional ByVal bSetIsDirty As Boolean = True)
            Try
                m_bInsideCopyData = True

                m_snX.ActualValue = fltX
                m_snY.ActualValue = fltY
                m_snZ.ActualValue = fltZ

                If Not bIgnoreEvents AndAlso Not m_bIgnoreChangeValueEvents Then RaiseEvent ValueChanged()
                If bSetIsDirty Then
                    Me.IsDirty = True
                End If

            Catch ex As Exception
                Throw ex
            Finally
                m_bInsideCopyData = False
            End Try
        End Sub

        Public Overridable Overloads Sub CopyData(ByRef svVec3 As ScaledVector3, Optional ByVal bIgnoreEvents As Boolean = False, Optional ByVal bSetIsDirty As Boolean = True)
            Try
                m_bInsideCopyData = True

                m_snX.CopyData(svVec3.m_snX)
                m_snY.CopyData(svVec3.m_snY)
                m_snZ.CopyData(svVec3.m_snZ)

                If Not bIgnoreEvents AndAlso Not m_bIgnoreChangeValueEvents Then RaiseEvent ValueChanged()
                If bSetIsDirty Then
                    Me.IsDirty = True
                End If

            Catch ex As Exception
                Throw ex
            Finally
                m_bInsideCopyData = False
            End Try
        End Sub

        Public Overrides Function ToString() As String
            Return "(" & m_snX.Text() & ", " & m_snY.Text() & ", " & m_snZ.Text() & ")"
        End Function

        Public Overloads Sub LoadData(ByRef oXml As Interfaces.StdXml, ByVal strName As String, Optional ByVal bThrowError As Boolean = True)
            If oXml.FindChildElement(strName, False) Then
                oXml.IntoChildElement(strName)

                m_snX.LoadData(oXml, "X")
                m_snY.LoadData(oXml, "Y")
                m_snZ.LoadData(oXml, "Z")

                oXml.OutOfElem()
            ElseIf bThrowError Then
                Throw New System.Exception("No xml tag with the name '" & strName & "' was found.")
            End If
        End Sub

        Public Overloads Sub SaveData(ByRef oXml As Interfaces.StdXml, ByVal strName As String)
            oXml.AddChildElement(strName)
            oXml.IntoElem()

            m_snX.SaveData(oXml, "X")
            m_snY.SaveData(oXml, "Y")
            m_snZ.SaveData(oXml, "Z")

            oXml.OutOfElem()
        End Sub

        Public Overrides Sub SaveSimulationXml(ByRef oXml As Interfaces.StdXml, Optional ByRef nmParentControl As DataObject = Nothing, Optional ByVal strName As String = "")
            oXml.AddChildElement(strName)
            oXml.IntoElem()
            oXml.SetAttrib("x", m_snX.ActualValue)
            oXml.SetAttrib("y", m_snY.ActualValue)
            oXml.SetAttrib("z", m_snZ.ActualValue)
            oXml.OutOfElem()
        End Sub

#Region " Events "

        Public Event ValueChanging(ByVal snParam As ScaledNumber, ByVal dblNewVal As Double, ByVal eNewScale As ScaledNumber.enumNumericScale)
        Public Event ValueChanged()

        'If one of the scaled numbers value changed then raise our value changed event unless we 
        'are inside a copydata call. Then we want to suppress the events from the scaled numbers
        'and only raise the event once within our copydata
        Protected Overridable Sub OnValueChanged()
            Try
                If m_bInsideCopyData Then
                    Return
                End If

                If Not m_bIgnoreChangeValueEvents Then RaiseEvent ValueChanged()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub OnXValueChanging(ByVal dblNewVal As Double, ByVal eNewScale As ScaledNumber.enumNumericScale)
            If Not m_bIgnoreChangeValueEvents Then RaiseEvent ValueChanging(m_snX, dblNewVal, eNewScale)
        End Sub

        Protected Overridable Sub OnYValueChanging(ByVal dblNewVal As Double, ByVal eNewScale As ScaledNumber.enumNumericScale)
            If Not m_bIgnoreChangeValueEvents Then RaiseEvent ValueChanging(m_snY, dblNewVal, eNewScale)
        End Sub

        Protected Overridable Sub OnZValueChanging(ByVal dblNewVal As Double, ByVal eNewScale As ScaledNumber.enumNumericScale)
            If Not m_bIgnoreChangeValueEvents Then RaiseEvent ValueChanging(m_snZ, dblNewVal, eNewScale)
        End Sub

#End Region

#Region " ScaledVector3PropBagConverter "

        Public Class ScaledVector3PropBagConverter
            Inherits ExpandableObjectConverter

            Public Overloads Overrides Function CanConvertFrom(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal sourceType As System.Type) As Boolean

                Return MyBase.CanConvertFrom(context, sourceType)
            End Function

            Public Overloads Overrides Function CanConvertTo(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal destinationType As System.Type) As Boolean

                If destinationType Is GetType(AnimatGuiCtrls.Controls.PropertyBag) Then
                    Return True
                End If
                Return MyBase.CanConvertTo(context, destinationType)

            End Function

            Public Overloads Overrides Function ConvertTo(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal culture As System.Globalization.CultureInfo, ByVal value As Object, ByVal destinationType As System.Type) As Object
                If destinationType Is GetType(String) AndAlso TypeOf (value) Is AnimatGuiCtrls.Controls.PropertyTable Then
                    Dim pbValue As AnimatGuiCtrls.Controls.PropertyTable = DirectCast(value, AnimatGuiCtrls.Controls.PropertyTable)

                    If Not pbValue Is Nothing AndAlso Not pbValue.Tag Is Nothing AndAlso TypeOf (pbValue.Tag) Is ScaledVector3 Then
                        Dim svValue As ScaledVector3 = DirectCast(pbValue.Tag, ScaledVector3)
                        Return svValue.ToString
                    End If

                    Return ""
                ElseIf destinationType Is GetType(String) AndAlso TypeOf (value) Is AnimatGUI.Framework.ScaledVector3 Then
                    Dim svValue As ScaledVector3 = DirectCast(value, ScaledVector3)

                    Dim strValue As String = svValue.ToString
                    Return strValue
                End If

                Return MyBase.ConvertTo(context, culture, value, destinationType)
            End Function

        End Class

#End Region

    End Class

End Namespace

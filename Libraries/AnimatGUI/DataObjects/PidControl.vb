Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports AnimatGuiCtrls.Controls
Imports AnimatGUI.Framework

Namespace DataObjects

    Public Class PidControl
        Inherits Framework.DataObject

#Region " Attributes "

        Protected m_bComplexError As Boolean = True
        Protected m_bAntiResetWindup As Boolean = False
        Protected m_bRampLimit As Boolean = False

        Protected m_snKp As AnimatGUI.Framework.ScaledNumber
        Protected m_snKi As AnimatGUI.Framework.ScaledNumber
        Protected m_snKd As AnimatGUI.Framework.ScaledNumber

        Protected m_snRangeMax As AnimatGUI.Framework.ScaledNumber
        Protected m_snRangeMin As AnimatGUI.Framework.ScaledNumber

        Protected m_snARWBound As AnimatGUI.Framework.ScaledNumber
        Protected m_snRampGradient As AnimatGUI.Framework.ScaledNumber

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Overridable Property ComplexError() As Boolean
            Get
                Return m_bComplexError
            End Get
            Set(ByVal Value As Boolean)
                SetSimData("ComplexError", Value.ToString, True)
                m_bComplexError = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property AntiResetWindup() As Boolean
            Get
                Return m_bAntiResetWindup
            End Get
            Set(ByVal Value As Boolean)
                SetSimData("AntiResetWindup", Value.ToString, True)
                m_bAntiResetWindup = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property RampLimit() As Boolean
            Get
                Return m_bRampLimit
            End Get
            Set(ByVal Value As Boolean)
                SetSimData("RampLimit", Value.ToString, True)
                m_bRampLimit = Value
            End Set
        End Property

        '<Category("Gain Limits"), _
        ' Description("Sets the lower limit of the x variable."), _
        ' TypeConverter(GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter))> _
        <Browsable(False)> _
        Public Overridable Property Kp() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snKp
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Not Value Is Nothing Then
                    SetSimData("Kp", Value.ActualValue.ToString, True)
                    If Not Value Is Nothing Then m_snKp.CopyData(Value)
                End If
            End Set
        End Property

        '<Category("Gain Limits"), _
        ' Description("Sets the upper limit of the x variable."), _
        ' TypeConverter(GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter))> _
        <Browsable(False)> _
        Public Overridable Property Ki() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snKi
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Not Value Is Nothing Then
                    SetSimData("Ki", Value.ActualValue.ToString, True)
                    If Not Value Is Nothing Then m_snKi.CopyData(Value)
                End If
            End Set
        End Property

        '<Category("Gain Limits"), _
        ' Description("Sets the output value to use when the x value is less than the lower limit."), _
        ' TypeConverter(GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter))> _
        <Browsable(False)> _
        Public Overridable Property Kd() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snKd
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Not Value Is Nothing Then
                    SetSimData("Kd", Value.ActualValue.ToString, True)
                    If Not Value Is Nothing Then m_snKd.CopyData(Value)
                End If
            End Set
        End Property

        '<Category("Gain Limits"), _
        ' Description("Sets the output value to use when the x value is more than the upper limit."), _
        ' TypeConverter(GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter))> _
        <Browsable(False)> _
        Public Overridable Property RangeMin() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snRangeMin
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Not Value Is Nothing Then
                    SetSimData("RangeMin", Value.ActualValue.ToString, True)
                    If Not Value Is Nothing Then m_snRangeMin.CopyData(Value)
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property RangeMax() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snRangeMax
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Not Value Is Nothing Then
                    SetSimData("RangeMax", Value.ActualValue.ToString, True)
                    If Not Value Is Nothing Then m_snRangeMax.CopyData(Value)
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property ARWBound() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snARWBound
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Not Value Is Nothing Then
                    SetSimData("ARWBound", Value.ActualValue.ToString, True)
                    If Not Value Is Nothing Then m_snARWBound.CopyData(Value)
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property RampGradient() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snRampGradient
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Not Value Is Nothing Then
                    If Value.ActualValue < 0 OrElse Value.ActualValue > 90 Then
                        Throw New System.Exception("The ramp gradient must be between 0 and 90 degrees.")
                    End If

                    SetSimData("RampGradient", Value.ActualValue.ToString, True)
                    If Not Value Is Nothing Then m_snRampGradient.CopyData(Value)
                End If
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            m_strName = "PidControl"
            m_snKp = New AnimatGUI.Framework.ScaledNumber(Me, "Kp", 10, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")
            m_snKi = New AnimatGUI.Framework.ScaledNumber(Me, "Ki", 0.2, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")
            m_snKd = New AnimatGUI.Framework.ScaledNumber(Me, "Kd", 10, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")
            m_snRangeMax = New AnimatGUI.Framework.ScaledNumber(Me, "RangeMax", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")
            m_snRangeMin = New AnimatGUI.Framework.ScaledNumber(Me, "RangeMin", -1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")
            m_snARWBound = New AnimatGUI.Framework.ScaledNumber(Me, "ARWBound", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")
            m_snRampGradient = New AnimatGUI.Framework.ScaledNumber(Me, "RampGradient", 45, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Degrees", "Deg")
        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()
            m_snKp.ClearIsDirty()
            m_snKi.ClearIsDirty()
            m_snKd.ClearIsDirty()
            m_snRangeMax.ClearIsDirty()
            m_snRangeMin.ClearIsDirty()
            m_snARWBound.ClearIsDirty()
            m_snRampGradient.ClearIsDirty()
        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim doAxis As New PidControl(doParent)
            doAxis.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then doAxis.AfterClone(Me, bCutData, doRoot, doAxis)
            Return doAxis
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim OrigNode As DataObjects.PidControl = DirectCast(doOriginal, DataObjects.PidControl)

            'Protected m_bComplexError As Boolean = True
            'Protected m_bAntiResetWindup As Boolean = False
            'Protected m_bRampLimit As Boolean = False

            'Protected m_snKp As AnimatGUI.Framework.ScaledNumber
            'Protected m_snKi As AnimatGUI.Framework.ScaledNumber
            'Protected m_snKd As AnimatGUI.Framework.ScaledNumber

            'Protected m_snRangeMax As AnimatGUI.Framework.ScaledNumber
            'Protected m_snRangeMin As AnimatGUI.Framework.ScaledNumber

            'Protected m_snARWBound As AnimatGUI.Framework.ScaledNumber
            'Protected m_snRampGradient As AnimatGUI.Framework.ScaledNumber

            m_bComplexError = OrigNode.m_bComplexError
            m_bAntiResetWindup = OrigNode.m_bAntiResetWindup
            m_bRampLimit = OrigNode.m_bRampLimit

            m_snKp = DirectCast(OrigNode.m_snKp.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snKi = DirectCast(OrigNode.m_snKi.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snKd = DirectCast(OrigNode.m_snKd.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snRangeMax = DirectCast(OrigNode.m_snRangeMax.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snRangeMin = DirectCast(OrigNode.m_snRangeMin.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snARWBound = DirectCast(OrigNode.m_snARWBound.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snRampGradient = DirectCast(OrigNode.m_snRampGradient.Clone(Me, bCutData, doRoot), ScaledNumber)
        End Sub

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("ID", Me.ID.GetType(), "ID", _
                                        "PID Properties", "ID", Me.ID, True))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Enabled", GetType(Boolean), "Enabled", _
                                        "PID Properties", "Determines if this controller is enabled or not.", m_bEnabled))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Complex Error", m_bComplexError.GetType(), "ComplexError", _
                                        "PID Properties", "Sets whether the PID controller uses complex error calculations or simple ones.", m_bComplexError))

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = m_snKp.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Kp", pbNumberBag.GetType(), "Kp", _
                                        "Gains", "Proportional gain.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snKi.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Ki", pbNumberBag.GetType(), "Ki", _
                                        "Gains", "Integral gain.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snKd.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Kd", pbNumberBag.GetType(), "Kd", _
                                        "Gains", "Derivative gain.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Anti-Reset Windup", m_bAntiResetWindup.GetType(), "AntiResetWindup", _
                                        "ARW Properties", "Sets the whether to use anti-reset windup.", m_bAntiResetWindup))

            pbNumberBag = m_snRangeMax.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Range Max", pbNumberBag.GetType(), "RangeMax", _
                                        "ARW Properties", "Range maximum for ARW.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snRangeMin.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Range Min", pbNumberBag.GetType(), "RangeMin", _
                                        "ARW Properties", "Range minimum for ARW.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snARWBound.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("ARW Bound", pbNumberBag.GetType(), "ARWBound", _
                                        "ARW Properties", "Boundary for ARW.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Ramp Limit", m_bRampLimit.GetType(), "RampLimit", _
                                        "Ramp Properties", "Sets the whether to use ramp limit.", m_bRampLimit))

            pbNumberBag = m_snRampGradient.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Ramp Gradient", pbNumberBag.GetType(), "RampGradient", _
                                        "Ramp Properties", "Ramp Gradient.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

        End Sub

        Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            oXml.IntoElem()  'Into PID Element

            m_strID = oXml.GetChildString("ID", Me.ID)
            m_bEnabled = oXml.GetChildBool("Enabled", m_bEnabled)
            m_bComplexError = oXml.GetChildBool("ComplexError", m_bComplexError)
            m_bAntiResetWindup = oXml.GetChildBool("AntiResetWindup", m_bAntiResetWindup)
            m_bRampLimit = oXml.GetChildBool("RampLimit", m_bRampLimit)

            m_snKp.LoadData(oXml, "Kp")
            m_snKi.LoadData(oXml, "Ki")
            m_snKd.LoadData(oXml, "Kd")

            m_snRangeMax.LoadData(oXml, "RangeMax")
            m_snRangeMin.LoadData(oXml, "RangeMin")
            m_snARWBound.LoadData(oXml, "ARWBound")
            m_snRampGradient.LoadData(oXml, "RampGradient")

            oXml.OutOfElem()

        End Sub


        Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            oXml.AddChildElement("PID")
            oXml.IntoElem()

            oXml.AddChildElement("ID", Me.ID)
            oXml.AddChildElement("AssemblyFile", Me.AssemblyFile)
            oXml.AddChildElement("ClassName", Me.ClassName)

            oXml.AddChildElement("Enabled", m_bEnabled)
            oXml.AddChildElement("ComplexError", m_bComplexError)
            oXml.AddChildElement("AntiResetWindup", m_bAntiResetWindup)
            oXml.AddChildElement("RampLimit", m_bRampLimit)

            m_snKp.SaveData(oXml, "Kp")
            m_snKi.SaveData(oXml, "Ki")
            m_snKd.SaveData(oXml, "Kd")

            m_snRangeMax.SaveData(oXml, "RangeMax")
            m_snRangeMin.SaveData(oXml, "RangeMin")
            m_snARWBound.SaveData(oXml, "ARWBound")
            m_snRampGradient.SaveData(oXml, "RampGradient")

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            oXml.AddChildElement("PID")
            oXml.IntoElem()

            oXml.AddChildElement("ID", Me.ID)
            oXml.AddChildElement("Type", "PidControl")
            oXml.AddChildElement("AssemblyFile", Me.AssemblyFile)
            oXml.AddChildElement("ClassName", Me.ClassName)

            oXml.AddChildElement("Enabled", m_bEnabled)
            oXml.AddChildElement("ComplexError", m_bComplexError)
            oXml.AddChildElement("AntiResetWindup", m_bAntiResetWindup)
            oXml.AddChildElement("RampLimit", m_bRampLimit)

            m_snKp.SaveSimulationXml(oXml, Me, "Kp")
            m_snKi.SaveSimulationXml(oXml, Me, "Ki")
            m_snKd.SaveSimulationXml(oXml, Me, "Kd")

            m_snRangeMax.SaveSimulationXml(oXml, Me, "RangeMax")
            m_snRangeMin.SaveSimulationXml(oXml, Me, "RangeMin")
            m_snARWBound.SaveSimulationXml(oXml, Me, "ARWBound")
            m_snRampGradient.SaveSimulationXml(oXml, Me, "RampGradient")

            oXml.OutOfElem()

        End Sub

#End Region

#End Region

    End Class

#Region " PidControlPropBagConverter "

    Public Class PidControlPropBagConverter
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

                If Not pbValue Is Nothing AndAlso Not pbValue.Tag Is Nothing AndAlso TypeOf (pbValue.Tag) Is PidControl Then
                    Dim svValue As PidControl = DirectCast(pbValue.Tag, PidControl)

                    If svValue.Enabled Then
                        Return "PID Params"
                    Else
                        Return "PID Params"
                    End If
                End If

                Return ""
            ElseIf destinationType Is GetType(String) AndAlso TypeOf (value) Is PidControl Then
                Dim svValue As PidControl = DirectCast(value, PidControl)

                If svValue.Enabled Then
                    Return "PID Params"
                Else
                    Return "PID Params"
                End If
            End If

            Return MyBase.ConvertTo(context, culture, value, destinationType)
        End Function

    End Class

#End Region

End Namespace

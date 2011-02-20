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

Namespace DataObjects.Gains.MuscleGains

    Public Class StimulusTension
        Inherits DataObjects.Gain

#Region " Attributes "

        Protected m_Muscle As DataObjects.Physical.IMuscle

        Protected m_snA1 As AnimatGUI.Framework.ScaledNumber
        Protected m_snA2 As AnimatGUI.Framework.ScaledNumber
        Protected m_snA3 As AnimatGUI.Framework.ScaledNumber
        Protected m_snA4 As AnimatGUI.Framework.ScaledNumber

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Overridable Property Muscle() As DataObjects.Physical.IMuscle
            Get
                Return m_Muscle
            End Get
            Set(ByVal Value As DataObjects.Physical.IMuscle)
                m_Muscle = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property GainType() As String
            Get
                Return "Stimulus-Tension Curve"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property Type() As String
            Get
                Return "StimulusTension"
            End Get
        End Property

        Public Overrides ReadOnly Property GainEquation() As String
            Get
                Return "Y = A2/(1 + e^(A3*(A1-X))) + A4"
            End Get
        End Property

        <Category("Equation Parameters"), _
         Description("Sets the X offset for the sigmoid for the tension-stimulus curve."), _
         TypeConverter(GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter))> _
        Public Overridable Property XOffset() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snA1
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                Dim snOrig As ScaledNumber = DirectCast(m_snA1.Clone(m_snA1.Parent, False, Nothing), ScaledNumber)
                If Not Value Is Nothing Then m_snA1.CopyData(Value)

                Dim snNew As ScaledNumber = DirectCast(m_snA1.Clone(m_snA1.Parent, False, Nothing), ScaledNumber)
                Me.ManualAddPropertyHistory("A1", snOrig, snNew, True)
                'RecalculuateLimits()
            End Set
        End Property

        <Category("Equation Parameters"), _
         Description("Sets the height for the sigmoid for the tension-stimulus curve."), _
         TypeConverter(GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter))> _
        Public Overridable Property Amplitude() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snA2
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                Dim snOrig As ScaledNumber = DirectCast(m_snA2.Clone(m_snA2.Parent, False, Nothing), ScaledNumber)
                If Not Value Is Nothing Then m_snA2.CopyData(Value)

                Dim snNew As ScaledNumber = DirectCast(m_snA2.Clone(m_snA2.Parent, False, Nothing), ScaledNumber)
                Me.ManualAddPropertyHistory("A2", snOrig, snNew, True)
                'RecalculuateLimits()
            End Set
        End Property

        <Category("Equation Parameters"), _
         Description("Sets how fast the sigmoid rises for the tension-stimulus curve."), _
         TypeConverter(GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter))> _
        Public Overridable Property Steepness() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snA3
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                Dim snOrig As ScaledNumber = DirectCast(m_snA3.Clone(m_snA3.Parent, False, Nothing), ScaledNumber)
                If Not Value Is Nothing Then m_snA3.CopyData(Value)

                Dim snNew As ScaledNumber = DirectCast(m_snA3.Clone(m_snA3.Parent, False, Nothing), ScaledNumber)
                Me.ManualAddPropertyHistory("A3", snOrig, snNew, True)
                'RecalculuateLimits()
            End Set
        End Property

        '<Category("Equation Parameters"), _
        ' Description("Sets the Y offset for the sigmoid for the tension-stimulus curve."), _
        ' TypeConverter(GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter))> _
        <Browsable(False)> _
        Public Overridable Property YOffset() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snA4
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                Dim snOrig As ScaledNumber = DirectCast(m_snA4.Clone(m_snA4.Parent, False, Nothing), ScaledNumber)
                If Not Value Is Nothing Then m_snA4.CopyData(Value)

                Dim snNew As ScaledNumber = DirectCast(m_snA4.Clone(m_snA4.Parent, False, Nothing), ScaledNumber)
                Me.ManualAddPropertyHistory("A4", snOrig, snNew, True)
                'RecalculuateLimits()
            End Set
        End Property

        '<Category("Gain Limits"), _
        ' Description("The tension value at the minimum limit."), _
        ' TypeConverter(GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter))> _
        Public Overridable ReadOnly Property ValAtMin() As AnimatGUI.Framework.ScaledNumber
            Get
                Dim snVal As New AnimatGUI.Framework.ScaledNumber(Me, "ValAtMin", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Newtons", "N")

                Dim dblGain As Double = CalculateGain(m_snLowerLimit.ActualValue)
                snVal.SetFromValue(dblGain)
                Return snVal
            End Get
        End Property

        '<Category("Gain Limits"), _
        ' Description("The tension value at the maximum limit."), _
        ' TypeConverter(GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter))> _
        Public Overridable ReadOnly Property ValAtMax() As AnimatGUI.Framework.ScaledNumber
            Get
                Dim snVal As New AnimatGUI.Framework.ScaledNumber(Me, "ValAtMax", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Newtons", "N")

                Dim dblGain As Double = CalculateGain(m_snUpperLimit.ActualValue)
                snVal.SetFromValue(dblGain)
                Return snVal
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property SelectableGain() As Boolean
            Get
                Return False
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property BarAssemblyFile() As String
            Get
                Return "AnimatGUI.dll"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property BarClassName() As String
            Get
                Return "AnimatGUI.Forms.Gain.ConfigureGain"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.StimulusTension.gif"
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            m_Muscle = DirectCast(doParent, DataObjects.Physical.IMuscle)

            m_snA1 = New AnimatGUI.Framework.ScaledNumber(Me, "XOffset", -40, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "Volts", "V")
            m_snA2 = New AnimatGUI.Framework.ScaledNumber(Me, "Amplitude", 5, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Newtons", "N")
            m_snA3 = New AnimatGUI.Framework.ScaledNumber(Me, "Steepness", 100, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")
            m_snA4 = New AnimatGUI.Framework.ScaledNumber(Me, "YOffset", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Newtons", "N")

            Me.UseLimits = False
            m_snLowerLimit = New AnimatGUI.Framework.ScaledNumber(Me, "LowerLimit", -100, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "Volts", "V")
            m_snUpperLimit = New AnimatGUI.Framework.ScaledNumber(Me, "UpperLimit", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "Volts", "V")
            m_snLowerOutput = New AnimatGUI.Framework.ScaledNumber(Me, "LowerOutput", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Newtons", "N")
            m_snUpperOutput = New AnimatGUI.Framework.ScaledNumber(Me, "UpperOutput", 6, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Newtons", "N")

            'RecalculuateLimits()

            m_strGainPropertyName = "StimTension"
            m_strIndependentUnits = "Membrane Voltage (V)"
            m_strDependentUnits = "Contractile Tension (N)"

        End Sub

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal strIndependentUnits As String, ByVal strDependentUnits As String, _
                       Optional ByVal bLimitsReadOnly As Boolean = False, Optional ByVal bLimitOutputsReadOnly As Boolean = False)
            MyBase.New(doParent)

            m_Muscle = DirectCast(doParent, DataObjects.Physical.IMuscle)

            m_snA1 = New AnimatGUI.Framework.ScaledNumber(Me, "XOffset", -40, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "Volts", "V")
            m_snA2 = New AnimatGUI.Framework.ScaledNumber(Me, "Amplitude", 5, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Newtons", "N")
            m_snA3 = New AnimatGUI.Framework.ScaledNumber(Me, "Steepness", 100, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")
            m_snA4 = New AnimatGUI.Framework.ScaledNumber(Me, "YOffset", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Newtons", "N")

            Me.UseLimits = False
            m_snLowerLimit = New AnimatGUI.Framework.ScaledNumber(Me, "LowerLimit", -100, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "Volts", "V")
            m_snUpperLimit = New AnimatGUI.Framework.ScaledNumber(Me, "UpperLimit", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "Volts", "V")
            m_snLowerOutput = New AnimatGUI.Framework.ScaledNumber(Me, "LowerOutput", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Newtons", "N")
            m_snUpperOutput = New AnimatGUI.Framework.ScaledNumber(Me, "UpperOutput", 6, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Newtons", "N")

            'RecalculuateLimits()

            m_strIndependentUnits = strIndependentUnits
            m_strDependentUnits = strDependentUnits
            m_bLimitsReadOnly = bLimitsReadOnly
            m_bLimitOutputsReadOnly = bLimitOutputsReadOnly
        End Sub

        Public Overrides Function CalculateGain(ByVal dblInput As Double) As Double

            If (InLimits(dblInput)) Then
                Return ((m_snA2.ActualValue / (1 + Math.Exp(m_snA3.ActualValue * (m_snA1.ActualValue - dblInput)))) + m_snA4.ActualValue)
            Else
                Return CalculateLimitOutput(dblInput)
            End If

        End Function

        Public Overrides Sub RecalculuateLimits()

            If Not m_bUseLimits Then
                Dim dblExtent As Double = (15.88 * Math.Pow(m_snA3.ActualValue, -0.8471)) - 1.475
                If dblExtent <= 0 Then
                    dblExtent = (55.44 * Math.Pow(m_snA3.ActualValue, -1.387)) + 0.1134
                    If dblExtent < 0 Then dblExtent = 0.1
                End If

                dblExtent = (dblExtent + (dblExtent * 0.3)) / 2

                m_snLowerLimit.ActualValue = m_snA1.ActualValue - dblExtent
                m_snUpperLimit.ActualValue = m_snA1.ActualValue + dblExtent
            End If

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNew As New DataObjects.Gains.MuscleGains.StimulusTension(doParent)
            oNew.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNew.AfterClone(Me, bCutData, doRoot, oNew)
            Return oNew
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim gnOrig As DataObjects.Gains.MuscleGains.StimulusTension = DirectCast(doOriginal, DataObjects.Gains.MuscleGains.StimulusTension)

            m_Muscle = gnOrig.Muscle
            m_snA1 = DirectCast(gnOrig.m_snA1.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snA2 = DirectCast(gnOrig.m_snA2.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snA3 = DirectCast(gnOrig.m_snA3.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snA4 = DirectCast(gnOrig.m_snA4.Clone(Me, bCutData, doRoot), ScaledNumber)

        End Sub

        Public Overrides Sub UnitsChanged(ByVal ePrevMass As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits, _
                                          ByVal eNewMass As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits, _
                                          ByVal fltMassChange As Single, _
                                          ByVal ePrevDistance As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits, _
                                          ByVal eNewDistance As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits, _
                                          ByVal fltDistanceChange As Single)

            m_snA2.ActualValue = m_snA2.ActualValue * fltDistanceChange
            m_snA4.ActualValue = m_snA4.ActualValue * fltDistanceChange
            m_snLowerOutput.ActualValue = m_snLowerOutput.ActualValue * fltDistanceChange
            m_snUpperOutput.ActualValue = m_snUpperOutput.ActualValue * fltDistanceChange

        End Sub

        Public Overloads Overrides Sub LoadData(ByRef oXml As AnimatGUI.Interfaces.StdXml, ByVal strName As String, ByVal strGainPropertyName As String)
            MyBase.LoadData(oXml, strName, strGainPropertyName)

            oXml.IntoElem()

            If oXml.FindChildElement("A1", False) Then
                m_snA1.LoadData(oXml, "A1")
                m_snA2.LoadData(oXml, "A2")
                m_snA3.LoadData(oXml, "A3")
                m_snA4.LoadData(oXml, "A4")
            End If

            'RecalculuateLimits()

            oXml.OutOfElem()

            m_strIndependentUnits = "Membrane Voltage (V)"
            m_strDependentUnits = "Contractile Tension (N)"

        End Sub

        Public Overloads Overrides Sub SaveData(ByRef oXml As AnimatGUI.Interfaces.StdXml, ByVal strName As String)
            MyBase.SaveData(oXml, strName)

            oXml.IntoElem()

            m_snA1.SaveData(oXml, "A1")
            m_snA2.SaveData(oXml, "A2")
            m_snA3.SaveData(oXml, "A3")
            m_snA4.SaveData(oXml, "A4")

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveSimulationXml(ByRef oXml As AnimatGUI.Interfaces.StdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

            oXml.IntoElem()

            m_snA1.SaveSimulationXml(oXml, Me, "A1")
            m_snA2.SaveSimulationXml(oXml, Me, "A2")
            m_snA3.SaveSimulationXml(oXml, Me, "A3")
            m_snA4.SaveSimulationXml(oXml, Me, "A4")

            oXml.OutOfElem()

        End Sub

        Public Overrides Function ToString() As String
            Return ""
        End Function

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = m_snA1.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("X Offset", pbNumberBag.GetType(), "XOffset", _
                                        "Equation Parameters", "Sets the X offset for the sigmoid for the tension-stimulus curve.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snA2.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Amplitude", pbNumberBag.GetType(), "Amplitude", _
                                        "Equation Parameters", "Sets the height for the sigmoid for the tension-stimulus curve.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snA3.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Steepness", pbNumberBag.GetType(), "Steepness", _
                                        "Equation Parameters", "Sets how fast the sigmoid rises for the tension-stimulus curve.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = Me.ValAtMin.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("ValAtMin", pbNumberBag.GetType(), "ValAtMin", _
                                        "Gain Limits", "The tension value at the minimum limit.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = Me.ValAtMax.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("ValAtMax", pbNumberBag.GetType(), "ValAtMax", _
                                        "Gain Limits", "The tension value at the maximum limit.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snA4.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Y Offset", pbNumberBag.GetType(), "YOffset", _
                                        "Equation Parameters", "Sets the Y offset for the sigmoid for the tension-stimulus curve.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            If Not m_snA1 Is Nothing Then m_snA1.ClearIsDirty()
            If Not m_snA2 Is Nothing Then m_snA2.ClearIsDirty()
            If Not m_snA3 Is Nothing Then m_snA3.ClearIsDirty()
            If Not m_snA4 Is Nothing Then m_snA4.ClearIsDirty()
        End Sub

#End Region

#End Region

    End Class
End Namespace


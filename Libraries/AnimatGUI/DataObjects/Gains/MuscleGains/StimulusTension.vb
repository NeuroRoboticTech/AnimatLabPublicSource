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
        Inherits DataObjects.Gains.Sigmoid

#Region " Attributes "

        Protected m_Muscle As DataObjects.Physical.IMuscle

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

            m_snA = New AnimatGUI.Framework.ScaledNumber(Me, "XOffset", -40, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "Volts", "V")
            m_snB = New AnimatGUI.Framework.ScaledNumber(Me, "Amplitude", 5, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Newtons", "N")
            m_snC = New AnimatGUI.Framework.ScaledNumber(Me, "Steepness", 100, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")
            m_snD = New AnimatGUI.Framework.ScaledNumber(Me, "YOffset", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Newtons", "N")

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

            m_snA = New AnimatGUI.Framework.ScaledNumber(Me, "XOffset", -40, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "Volts", "V")
            m_snB = New AnimatGUI.Framework.ScaledNumber(Me, "Amplitude", 5, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Newtons", "N")
            m_snC = New AnimatGUI.Framework.ScaledNumber(Me, "Steepness", 100, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")
            m_snD = New AnimatGUI.Framework.ScaledNumber(Me, "YOffset", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Newtons", "N")

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

        End Sub

        Public Overrides Sub UnitsChanged(ByVal ePrevMass As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits, _
                                          ByVal eNewMass As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits, _
                                          ByVal fltMassChange As Single, _
                                          ByVal ePrevDistance As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits, _
                                          ByVal eNewDistance As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits, _
                                          ByVal fltDistanceChange As Single)

            m_snB.ActualValue = m_snB.ActualValue * fltDistanceChange
            m_snD.ActualValue = m_snD.ActualValue * fltDistanceChange
            m_snLowerOutput.ActualValue = m_snLowerOutput.ActualValue * fltDistanceChange
            m_snUpperOutput.ActualValue = m_snUpperOutput.ActualValue * fltDistanceChange

        End Sub

        Public Overloads Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml, ByVal strName As String, ByVal strGainPropertyName As String)
            MyBase.LoadData(oXml, strName, strGainPropertyName)

            m_strIndependentUnits = "Membrane Voltage (V)"
            m_strDependentUnits = "Contractile Tension (N)"

        End Sub

        Public Overrides Function ToString() As String
            Return ""
        End Function

#Region " DataObject Methods "

#End Region

#End Region

    End Class
End Namespace


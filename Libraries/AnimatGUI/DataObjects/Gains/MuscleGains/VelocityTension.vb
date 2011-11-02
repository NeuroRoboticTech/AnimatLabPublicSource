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

    Public Class VelocityTension
        Inherits DataObjects.Gain

#Region " Attributes "

        Protected m_Muscle As DataObjects.Physical.IMuscle

        Protected m_snAf As AnimatGUI.Framework.ScaledNumber
        Protected m_snVmax As AnimatGUI.Framework.ScaledNumber

        Protected m_dblB As Double = 0
        Protected m_dblA As Double = 0

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
                Return "Velocity-Tension Curve"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property Type() As String
            Get
                Return "VelocityTension"
            End Get
        End Property

        Public Overrides ReadOnly Property GainEquation() As String
            Get
                Return "Y = (1 - (L-Lrest)^2/Lwidth^2) * 100"
            End Get
        End Property

        <Category("Equation Parameters"), _
         Description("Determines the depth of the concavity of the curve ."), _
         TypeConverter(GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter))> _
        Public Overridable Property Af() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snAf
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)

                If Not Value Is Nothing Then
                    If Value.ActualValue <= 0 Then
                        Throw New System.Exception("Af can not be less than zero.")
                    End If

                    Dim snOrig As ScaledNumber = DirectCast(m_snAf.Clone(m_snAf.Parent, False, Nothing), ScaledNumber)
                    If Not Value Is Nothing Then m_snAf.CopyData(Value)

                    Dim snNew As ScaledNumber = DirectCast(m_snAf.Clone(m_snAf.Parent, False, Nothing), ScaledNumber)
                    Me.ManualAddPropertyHistory("Af", snOrig, snNew, True)
                    'RecalculuateLimits()
                End If
            End Set
        End Property

        <Category("Equation Parameters"), _
         Description("Sets the maximum velocity of shortening for the muscle."), _
         TypeConverter(GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter))> _
        Public Overridable Property Vmax() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snVmax
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)

                If Not Value Is Nothing Then
                    If Value.ActualValue <= 0 Then
                        Throw New System.Exception("Vmax can not be less than zero.")
                    End If

                    Dim snOrig As ScaledNumber = DirectCast(m_snVmax.Clone(m_snVmax.Parent, False, Nothing), ScaledNumber)
                    If Not Value Is Nothing Then m_snVmax.CopyData(Value)

                    Dim snNew As ScaledNumber = DirectCast(m_snVmax.Clone(m_snVmax.Parent, False, Nothing), ScaledNumber)
                    Me.ManualAddPropertyHistory("Vmax", snOrig, snNew, True)
                    'RecalculuateLimits()
                End If
            End Set
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
                Return "AnimatGUI.VelocityTension.gif"
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            m_Muscle = DirectCast(doParent, DataObjects.Physical.IMuscle)

            m_snAf = New AnimatGUI.Framework.ScaledNumber(Me, "Af", 0.15, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")
            m_snVmax = New AnimatGUI.Framework.ScaledNumber(Me, "Vmax", 6, AnimatGUI.Framework.ScaledNumber.enumNumericScale.centi, "m/s", "m/s")

            m_snLowerLimit = New AnimatGUI.Framework.ScaledNumber(Me, "LowerLimit", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "m/s", "m/s")
            m_snUpperLimit = New AnimatGUI.Framework.ScaledNumber(Me, "UpperLimit", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "m/s", "m/s")
            m_snLowerOutput = New AnimatGUI.Framework.ScaledNumber(Me, "LowerOutput", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Newtons", "N")
            m_snUpperOutput = New AnimatGUI.Framework.ScaledNumber(Me, "UpperOutput", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Newtons", "N")

            If Not Util.Environment Is Nothing Then
                m_snVmax.SetFromValue(m_snVmax.ActualValue, CInt(Util.Environment.DisplayDistanceUnits))
                m_snLowerLimit.SetFromValue(m_snLowerLimit.ActualValue, CInt(Util.Environment.DisplayDistanceUnits))
                m_snUpperLimit.SetFromValue(m_snUpperLimit.ActualValue, CInt(Util.Environment.DisplayDistanceUnits))
            End If

            'RecalculuateLimits()

            m_strGainPropertyName = "VelocityTension"
            m_strIndependentUnits = "Velocity of Shortening (m/s)"
            m_strDependentUnits = "Tension (N)"

        End Sub

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal strIndependentUnits As String, ByVal strDependentUnits As String, _
                       Optional ByVal bLimitsReadOnly As Boolean = False, Optional ByVal bLimitOutputsReadOnly As Boolean = False)
            MyBase.New(doParent)

            m_Muscle = DirectCast(doParent, DataObjects.Physical.IMuscle)

            m_snAf = New AnimatGUI.Framework.ScaledNumber(Me, "Af", 0.15, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")
            m_snVmax = New AnimatGUI.Framework.ScaledNumber(Me, "Vmax", 6, AnimatGUI.Framework.ScaledNumber.enumNumericScale.centi, "m/s", "m/s")

            m_snLowerLimit = New AnimatGUI.Framework.ScaledNumber(Me, "LowerLimit", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "m/s", "m/s")
            m_snUpperLimit = New AnimatGUI.Framework.ScaledNumber(Me, "UpperLimit", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "m/s", "m/s")
            m_snLowerOutput = New AnimatGUI.Framework.ScaledNumber(Me, "LowerOutput", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Newtons", "N")
            m_snUpperOutput = New AnimatGUI.Framework.ScaledNumber(Me, "UpperOutput", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Newtons", "N")

            If Not Util.Environment Is Nothing Then
                m_snVmax.SetFromValue(m_snVmax.ActualValue, CInt(Util.Environment.DisplayDistanceUnits))
                m_snLowerLimit.SetFromValue(m_snLowerLimit.ActualValue, CInt(Util.Environment.DisplayDistanceUnits))
                m_snUpperLimit.SetFromValue(m_snUpperLimit.ActualValue, CInt(Util.Environment.DisplayDistanceUnits))
            End If

            'RecalculuateLimits()

            m_strIndependentUnits = strIndependentUnits
            m_strDependentUnits = strDependentUnits
            m_bLimitsReadOnly = bLimitsReadOnly
            m_bLimitOutputsReadOnly = bLimitOutputsReadOnly

            m_strIndependentUnits = "Velocity of Shortening (m/s)"
            m_strDependentUnits = "Tension (N)"
        End Sub

        Public Overrides Function CalculateGain(ByVal dblInput As Double) As Double

            If (InLimits(dblInput)) Then
                If Not m_Muscle Is Nothing AndAlso Not m_Muscle.StimulusTension Is Nothing Then
                    Return (m_dblB * m_Muscle.StimulusTension.Amplitude.ActualValue - dblInput * m_dblA) / (dblInput + m_dblB)
                Else
                    Return 0
                End If
            Else
                Return CalculateLimitOutput(dblInput)
            End If

        End Function

        Public Overrides Sub RecalculuateLimits()

            If Not m_Muscle Is Nothing AndAlso Not m_Muscle.StimulusTension Is Nothing Then
                m_dblA = m_snAf.ActualValue * m_Muscle.StimulusTension.Amplitude.ActualValue
            End If
            m_dblB = m_snAf.ActualValue * m_snVmax.ActualValue

            If Not m_bUseLimits Then
                m_snLowerLimit.ActualValue = 0
                m_snUpperLimit.ActualValue = m_snVmax.ActualValue
            End If

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNew As New DataObjects.Gains.MuscleGains.VelocityTension(doParent)
            oNew.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNew.AfterClone(Me, bCutData, doRoot, oNew)
            Return oNew
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim gnOrig As DataObjects.Gains.MuscleGains.VelocityTension = DirectCast(doOriginal, DataObjects.Gains.MuscleGains.VelocityTension)

            m_Muscle = gnOrig.Muscle
            m_snAf = DirectCast(gnOrig.m_snAf.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snVmax = DirectCast(gnOrig.m_snVmax.Clone(Me, bCutData, doRoot), ScaledNumber)

            'RecalculuateLimits()

        End Sub

        Public Overrides Sub UnitsChanged(ByVal ePrevMass As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits, _
                                          ByVal eNewMass As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits, _
                                          ByVal fltMassChange As Single, _
                                          ByVal ePrevDistance As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits, _
                                          ByVal eNewDistance As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits, _
                                          ByVal fltDistanceChange As Single)

            m_snVmax.SetFromValue(m_snVmax.ActualValue * fltDistanceChange, CInt(Util.Environment.DisplayDistanceUnits))
            m_snLowerLimit.SetFromValue(m_snLowerLimit.ActualValue * fltDistanceChange, CInt(Util.Environment.DisplayDistanceUnits))
            m_snUpperOutput.SetFromValue(m_snUpperOutput.ActualValue * fltDistanceChange, CInt(Util.Environment.DisplayDistanceUnits))

        End Sub

        Public Overloads Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml, ByVal strName As String, ByVal strGainPropertyName As String)
            MyBase.LoadData(oXml, strName, strGainPropertyName)

            oXml.IntoElem()

            If oXml.FindChildElement("Af", False) Then
                m_snAf.LoadData(oXml, "Af")
                m_snVmax.LoadData(oXml, "Vmax")
            End If

            oXml.OutOfElem()

            'RecalculuateLimits()

            m_strIndependentUnits = "Velocity of Shortening (m/s)"
            m_strDependentUnits = "Tension (N)"

        End Sub

        Public Overloads Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml, ByVal strName As String)
            MyBase.SaveData(oXml, strName)

            oXml.IntoElem()

            m_snAf.SaveData(oXml, "Af")
            m_snVmax.SaveData(oXml, "Vmax")

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

            oXml.IntoElem()

            m_snAf.SaveSimulationXml(oXml, Me, "Af")
            m_snVmax.SaveSimulationXml(oXml, Me, "Vmax")

            oXml.OutOfElem()

        End Sub

        Public Overrides Function ToString() As String
            Return ""
        End Function

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = m_snAf.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Af", pbNumberBag.GetType(), "Af", _
                                        "Equation Parameters", "Determines the depth of the concavity of the curve .", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snVmax.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Vmax", pbNumberBag.GetType(), "Vmax", _
                                        "Equation Parameters", "Sets the maximum velocity of shortening for the muscle.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            If Not m_snAf Is Nothing Then m_snAf.ClearIsDirty()
            If Not m_snVmax Is Nothing Then m_snVmax.ClearIsDirty()
        End Sub

#End Region

#End Region

    End Class
End Namespace



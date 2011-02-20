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

    Public Class LengthTension
        Inherits DataObjects.Gain

#Region " Attributes "

        Protected m_Muscle As DataObjects.Physical.IMuscle

        Protected m_snRestingLength As AnimatGUI.Framework.ScaledNumber
        Protected m_snLwidth As AnimatGUI.Framework.ScaledNumber
        Protected m_snLcePercentage As AnimatGUI.Framework.ScaledNumber

        Protected m_bShowLcePercentage As Boolean = True

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
                Return "Length-Tension Curve"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property Type() As String
            Get
                Return "LengthTension"
            End Get
        End Property

        Public Overrides ReadOnly Property GainEquation() As String
            Get
                Return "Y = (1 - (L-Lrest)^2/Lwidth^2) * 100"
            End Get
        End Property

        <Category("Equation Parameters"), _
         Description("Sets the X offset for the sigmoid for the tension-stimulus curve."), _
         TypeConverter(GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter))> _
        Public Overridable Property RestingLength() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snRestingLength
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)

                If Not Value Is Nothing Then
                    If Value.ActualValue <= 0 Then
                        Throw New System.Exception("The muscle resting length can not be less than zero.")
                    End If

                    Dim snOrig As ScaledNumber = DirectCast(m_snRestingLength.Clone(m_snRestingLength.Parent, False, Nothing), ScaledNumber)
                    If Not Value Is Nothing Then m_snRestingLength.CopyData(Value)

                    Dim snNew As ScaledNumber = DirectCast(m_snRestingLength.Clone(m_snRestingLength.Parent, False, Nothing), ScaledNumber)
                    Me.ManualAddPropertyHistory("RestingLength", snOrig, snNew, True)
                    'RecalculuateLimits()
                End If
            End Set
        End Property

        '<Category("Equation Parameters"), _
        ' Description("Sets the height for the sigmoid for the tension-stimulus curve."), _
        ' TypeConverter(GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter))> _
        <Browsable(False)> _
        Public Overridable Property LcePercentage() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snLcePercentage
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)

                If Not Value Is Nothing Then
                    If Value.ActualValue <= 0 Then
                        Throw New System.Exception("The percentage of the muscle length for the contractile element can not be less than zero.")
                    End If

                    If Value.ActualValue > 90 Then
                        Throw New System.Exception("The percentage of the muscle length for the contractile element can not be greater than 90.")
                    End If

                    Dim snOrig As ScaledNumber = DirectCast(m_snLcePercentage.Clone(m_snLcePercentage.Parent, False, Nothing), ScaledNumber)
                    If Not Value Is Nothing Then m_snLcePercentage.CopyData(Value)

                    Dim snNew As ScaledNumber = DirectCast(m_snLcePercentage.Clone(m_snLcePercentage.Parent, False, Nothing), ScaledNumber)
                    Me.ManualAddPropertyHistory("LcePercentage", snOrig, snNew, True)
                End If
            End Set
        End Property

        <Category("Equation Parameters"), _
         Description("Sets the how much of the resting muscle length is taken up by the contractile portion."), _
         TypeConverter(GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter))> _
        Public Overridable Property Lwidth() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snLwidth
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)

                If Not Value Is Nothing Then
                    If Value.ActualValue <= 0 Then
                        Throw New System.Exception("The width of the tension-length curve can not be less than zero.")
                    End If

                    Dim snOrig As ScaledNumber = DirectCast(m_snLwidth.Clone(m_snLwidth.Parent, False, Nothing), ScaledNumber)
                    If Not Value Is Nothing Then m_snLwidth.CopyData(Value)

                    Dim snNew As ScaledNumber = DirectCast(m_snLwidth.Clone(m_snLwidth.Parent, False, Nothing), ScaledNumber)
                    Me.ManualAddPropertyHistory("Lwidth", snOrig, snNew, True)
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
                Return "AnimatGUI.LengthTension.gif"
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property ShowLcePercentage() As Boolean
            Get
                Return m_bShowLcePercentage
            End Get
            Set(ByVal Value As Boolean)
                m_bShowLcePercentage = False
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            m_Muscle = DirectCast(doParent, DataObjects.Physical.IMuscle)

            m_snRestingLength = New AnimatGUI.Framework.ScaledNumber(Me, "RestingLength", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Meters", "m")
            m_snLwidth = New AnimatGUI.Framework.ScaledNumber(Me, "Lwidth", 50, AnimatGUI.Framework.ScaledNumber.enumNumericScale.centi, "Meters", "m")
            m_snLcePercentage = New AnimatGUI.Framework.ScaledNumber(Me, "LcePercentage", 50, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")

            m_snLowerLimit = New AnimatGUI.Framework.ScaledNumber(Me, "LowerLimit", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Meters", "m")
            m_snUpperLimit = New AnimatGUI.Framework.ScaledNumber(Me, "UpperLimit", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Meters", "m")
            m_snLowerOutput = New AnimatGUI.Framework.ScaledNumber(Me, "LowerOutput", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")
            m_snUpperOutput = New AnimatGUI.Framework.ScaledNumber(Me, "UpperOutput", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")

            If Not Util.Environment Is Nothing Then
                m_snRestingLength.SetFromValue(1, CInt(Util.Environment.DisplayDistanceUnits))
                m_snLwidth.SetFromValue(0.5, CInt(Util.Environment.DisplayDistanceUnits))
                m_snLowerLimit.SetFromValue(0, CInt(Util.Environment.DisplayDistanceUnits))
                m_snUpperLimit.SetFromValue(0, CInt(Util.Environment.DisplayDistanceUnits))
            End If

            'RecalculuateLimits()

            m_strGainPropertyName = "LengthTension"
            m_strIndependentUnits = "Meters"
            m_strDependentUnits = "% Isometric Tension Used"

        End Sub

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal strIndependentUnits As String, ByVal strDependentUnits As String, _
                       Optional ByVal bLimitsReadOnly As Boolean = False, Optional ByVal bLimitOutputsReadOnly As Boolean = False)
            MyBase.New(doParent)

            m_Muscle = DirectCast(doParent, DataObjects.Physical.IMuscle)

            m_snRestingLength = New AnimatGUI.Framework.ScaledNumber(Me, "RestingLength", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Meters", "m")
            m_snLwidth = New AnimatGUI.Framework.ScaledNumber(Me, "Lwidth", 50, AnimatGUI.Framework.ScaledNumber.enumNumericScale.centi, "Meters", "m")
            m_snLcePercentage = New AnimatGUI.Framework.ScaledNumber(Me, "LcePercentage", 50, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")

            m_snLowerLimit = New AnimatGUI.Framework.ScaledNumber(Me, "LowerLimit", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Meters", "m")
            m_snUpperLimit = New AnimatGUI.Framework.ScaledNumber(Me, "UpperLimit", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Meters", "m")
            m_snLowerOutput = New AnimatGUI.Framework.ScaledNumber(Me, "LowerOutput", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")
            m_snUpperOutput = New AnimatGUI.Framework.ScaledNumber(Me, "UpperOutput", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")

            If Not Util.Environment Is Nothing Then
                m_snRestingLength.SetFromValue(1, CInt(Util.Environment.DisplayDistanceUnits))
                m_snLwidth.SetFromValue(0.5, CInt(Util.Environment.DisplayDistanceUnits))
                m_snLowerLimit.SetFromValue(0, CInt(Util.Environment.DisplayDistanceUnits))
                m_snUpperLimit.SetFromValue(0, CInt(Util.Environment.DisplayDistanceUnits))
            End If

            'RecalculuateLimits()

            m_strIndependentUnits = strIndependentUnits
            m_strDependentUnits = strDependentUnits
            m_bLimitsReadOnly = bLimitsReadOnly
            m_bLimitOutputsReadOnly = bLimitOutputsReadOnly

            m_strIndependentUnits = "Muscle Length (Meters)"
            m_strDependentUnits = "% Isometric Tension Used"
        End Sub

        Public Overrides Function CalculateGain(ByVal dblInput As Double) As Double

            If (InLimits(dblInput)) Then
                Return (1 - (Math.Pow((dblInput - m_snRestingLength.ActualValue), 2) / Math.Pow(m_snLwidth.ActualValue, 2))) * 100
            Else
                Return CalculateLimitOutput(dblInput)
            End If

        End Function

        Public Overrides Sub RecalculuateLimits()

            If Not m_bUseLimits Then
                m_snLowerLimit.ActualValue = m_snRestingLength.ActualValue - m_snLwidth.ActualValue
                m_snUpperLimit.ActualValue = m_snRestingLength.ActualValue + m_snLwidth.ActualValue
            End If

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNew As New DataObjects.Gains.MuscleGains.LengthTension(doParent)
            oNew.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNew.AfterClone(Me, bCutData, doRoot, oNew)
            Return oNew
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim gnOrig As DataObjects.Gains.MuscleGains.LengthTension = DirectCast(doOriginal, DataObjects.Gains.MuscleGains.LengthTension)

            m_Muscle = gnOrig.Muscle
            m_snRestingLength = DirectCast(gnOrig.m_snRestingLength.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snLwidth = DirectCast(gnOrig.m_snLwidth.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snLcePercentage = DirectCast(gnOrig.m_snLcePercentage.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_bShowLcePercentage = gnOrig.m_bShowLcePercentage

        End Sub

        Public Overrides Sub UnitsChanged(ByVal ePrevMass As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits, _
                                          ByVal eNewMass As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits, _
                                          ByVal fltMassChange As Single, _
                                          ByVal ePrevDistance As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits, _
                                          ByVal eNewDistance As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits, _
                                          ByVal fltDistanceChange As Single)

            m_snRestingLength.SetFromValue(m_snRestingLength.ActualValue * fltDistanceChange, CInt(Util.Environment.DisplayDistanceUnits))
            m_snLwidth.SetFromValue(m_snLwidth.ActualValue * fltDistanceChange, CInt(Util.Environment.DisplayDistanceUnits))
            m_snLowerLimit.SetFromValue(m_snLowerLimit.ActualValue * fltDistanceChange, CInt(Util.Environment.DisplayDistanceUnits))
            m_snUpperLimit.SetFromValue(m_snUpperLimit.ActualValue * fltDistanceChange, CInt(Util.Environment.DisplayDistanceUnits))

        End Sub

        Public Overloads Overrides Sub LoadData(ByRef oXml As AnimatGUI.Interfaces.StdXml, ByVal strName As String, ByVal strGainPropertyName As String)
            MyBase.LoadData(oXml, strName, strGainPropertyName)

            oXml.IntoElem()

            If oXml.FindChildElement("RestingLength", False) Then
                m_snRestingLength.LoadData(oXml, "RestingLength")
                m_snLwidth.LoadData(oXml, "Lwidth")
            End If

            If oXml.FindChildElement("LcePercentage", False) Then
                m_snLcePercentage.LoadData(oXml, "LcePercentage")
            End If

            oXml.OutOfElem()

            'RecalculuateLimits()

            m_strIndependentUnits = "Meters"
            m_strDependentUnits = "% Isometric Tension Used"

        End Sub

        Public Overloads Overrides Sub SaveData(ByRef oXml As AnimatGUI.Interfaces.StdXml, ByVal strName As String)
            MyBase.SaveData(oXml, strName)

            oXml.IntoElem()

            m_snRestingLength.SaveData(oXml, "RestingLength")
            m_snLwidth.SaveData(oXml, "Lwidth")
            m_snLcePercentage.SaveData(oXml, "LcePercentage")

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveSimulationXml(ByRef oXml As AnimatGUI.Interfaces.StdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

            oXml.IntoElem()

            m_snRestingLength.SaveSimulationXml(oXml, Me, "RestingLength")
            m_snLwidth.SaveSimulationXml(oXml, Me, "Lwidth")
            m_snLcePercentage.SaveSimulationXml(oXml, Me, "LcePercentage")

            oXml.OutOfElem()

        End Sub

        Public Overrides Function ToString() As String
            Return ""
        End Function

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = m_snRestingLength.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("RestingLength", pbNumberBag.GetType(), "RestingLength", _
                                        "Equation Parameters", "Sets the resting length of the muscle.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snLwidth.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Lwidth", pbNumberBag.GetType(), "Lwidth", _
                                        "Equation Parameters", "Sets the width of the inverted parabola used for tension-length curve.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            If m_bShowLcePercentage Then
                pbNumberBag = m_snLcePercentage.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Lce Percentage", pbNumberBag.GetType(), "LcePercentage", _
                                        "Equation Parameters", "Sets the how much of the resting muscle length is taken up by the contractile portion.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))
            End If

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            If Not m_snRestingLength Is Nothing Then m_snRestingLength.ClearIsDirty()
            If Not m_snLwidth Is Nothing Then m_snLwidth.ClearIsDirty()
            If Not m_snLcePercentage Is Nothing Then m_snLcePercentage.ClearIsDirty()
        End Sub

#End Region

#End Region

    End Class
End Namespace



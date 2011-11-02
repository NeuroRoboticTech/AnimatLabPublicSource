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

        Protected m_snPeLengthPercentage As ScaledNumber
        Protected m_snMinPeLengthPercentage As ScaledNumber

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

        <Category("Muscle Properties"), _
         Description("Sets the resting length of the muscle."), _
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

                    SetSimData("RestingLength", Value.ActualValue.ToString, True)

                    Dim snOrig As ScaledNumber = DirectCast(m_snRestingLength.Clone(m_snRestingLength.Parent, False, Nothing), ScaledNumber)
                    m_snRestingLength.CopyData(Value)

                    Dim snNew As ScaledNumber = DirectCast(m_snRestingLength.Clone(m_snRestingLength.Parent, False, Nothing), ScaledNumber)
                    Me.ManualAddPropertyHistory("RestingLength", snOrig, snNew, True)
                    'RecalculuateLimits()
                End If
            End Set
        End Property

        <Category("Muscle Properties"), _
         Description("Sets the width of the inverted parabola used for tension-length curve."), _
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

                    SetSimData("Lwidth", Value.ActualValue.ToString, True)

                    Dim snOrig As ScaledNumber = DirectCast(m_snLwidth.Clone(m_snLwidth.Parent, False, Nothing), ScaledNumber)
                    m_snLwidth.CopyData(Value)

                    Dim snNew As ScaledNumber = DirectCast(m_snLwidth.Clone(m_snLwidth.Parent, False, Nothing), ScaledNumber)
                    Me.ManualAddPropertyHistory("Lwidth", snOrig, snNew, True)
                    'RecalculuateLimits()
                End If
            End Set
        End Property

        <Category("Muscle Properties"), _
         Description("The percentage of the resting length of the muscle that Pe section takes up."), _
         TypeConverter(GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter))> _
        Public Overridable Property PeLengthPercentage() As ScaledNumber
            Get
                Return m_snPeLengthPercentage
            End Get
            Set(ByVal value As ScaledNumber)
                If Not value Is Nothing Then
                    If value.ActualValue <= 0 OrElse value.ActualValue >= 100 Then
                        Throw New System.Exception("The Pe Length percentage must be between 0 and 100.")
                    End If

                    If m_snMinPeLengthPercentage.ActualValue > value.ActualValue Then
                        Throw New System.Exception("The Pe Length percentage can not be made lower than the minimum pe length percentage.")
                    End If

                    SetSimData("PeLengthPercentage", value.ActualValue.ToString, True)

                    Dim snOrig As ScaledNumber = DirectCast(m_snPeLengthPercentage.Clone(m_snLwidth.Parent, False, Nothing), ScaledNumber)
                    m_snPeLengthPercentage.CopyData(value)

                    Dim snNew As ScaledNumber = DirectCast(m_snPeLengthPercentage.Clone(m_snLwidth.Parent, False, Nothing), ScaledNumber)
                    Me.ManualAddPropertyHistory("PeLengthPercentage", snOrig, snNew, True)
                End If
            End Set
        End Property

        <Category("Muscle Properties"), _
         Description("The minimum length, as a percentage of resting length, that the Pe section can go attain."), _
         TypeConverter(GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter))> _
        Public Overridable Property MinPeLengthPercentage() As ScaledNumber
            Get
                Return m_snMinPeLengthPercentage
            End Get
            Set(ByVal value As ScaledNumber)
                If Not value Is Nothing Then
                    If value.ActualValue <= 0 OrElse value.ActualValue >= 100 Then
                        Throw New System.Exception("The Pe Length percentage must be between 0 and 100.")
                    End If

                    If m_snPeLengthPercentage.ActualValue < value.ActualValue Then
                        Throw New System.Exception("The Min Pe Length percentage can not be made greater than the pe length percentage.")
                    End If

                    SetSimData("MinPeLengthPercentage", value.ActualValue.ToString, True)

                    Dim snOrig As ScaledNumber = DirectCast(m_snMinPeLengthPercentage.Clone(m_snLwidth.Parent, False, Nothing), ScaledNumber)
                    m_snMinPeLengthPercentage.CopyData(value)

                    Dim snNew As ScaledNumber = DirectCast(m_snMinPeLengthPercentage.Clone(m_snLwidth.Parent, False, Nothing), ScaledNumber)
                    Me.ManualAddPropertyHistory("MinPeLengthPercentage", snOrig, snNew, True)

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

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            m_Muscle = DirectCast(doParent, DataObjects.Physical.IMuscle)

            m_snRestingLength = New AnimatGUI.Framework.ScaledNumber(Me, "RestingLength", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Meters", "m")
            m_snLwidth = New AnimatGUI.Framework.ScaledNumber(Me, "Lwidth", 50, AnimatGUI.Framework.ScaledNumber.enumNumericScale.centi, "Meters", "m")

            m_snPeLengthPercentage = New AnimatGUI.Framework.ScaledNumber(Me, "PeLengthPercentage", 90, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "%", "%")
            m_snMinPeLengthPercentage = New AnimatGUI.Framework.ScaledNumber(Me, "MinPeLengthPercentage", 5, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "%", "%")

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

            m_snPeLengthPercentage = New AnimatGUI.Framework.ScaledNumber(Me, "PeLengthPercentage", 90, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "%", "%")
            m_snMinPeLengthPercentage = New AnimatGUI.Framework.ScaledNumber(Me, "MinPeLengthPercentage", 5, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "%", "%")

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
            m_snPeLengthPercentage = DirectCast(gnOrig.m_snPeLengthPercentage.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snMinPeLengthPercentage = DirectCast(gnOrig.m_snMinPeLengthPercentage.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)

        End Sub

        Protected Overrides Sub SetAllSimData(ByVal doInterface As ManagedAnimatInterfaces.IDataObjectInterface)
            MyBase.SetAllSimData(doInterface)

            SetSimData("RestingLength", m_snRestingLength.ActualValue.ToString, True)
            SetSimData("Lwidth", m_snLwidth.ActualValue.ToString, True)
            SetSimData("PeLengthPercentage", m_snPeLengthPercentage.ActualValue.ToString, True)
            SetSimData("MinPeLengthPercentage", m_snMinPeLengthPercentage.ActualValue.ToString, True)
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

        Public Overloads Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml, ByVal strName As String, ByVal strGainPropertyName As String)
            MyBase.LoadData(oXml, strName, strGainPropertyName)

            oXml.IntoElem()

            m_snRestingLength.LoadData(oXml, "RestingLength")
            m_snLwidth.LoadData(oXml, "Lwidth")
            m_snPeLengthPercentage.LoadData(oXml, "PeLength")
            m_snMinPeLengthPercentage.LoadData(oXml, "MinPeLength")

            oXml.OutOfElem()

            m_strIndependentUnits = "Meters"
            m_strDependentUnits = "% Isometric Tension Used"

        End Sub

        Public Overloads Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml, ByVal strName As String)
            MyBase.SaveData(oXml, strName)

            oXml.IntoElem()

            m_snRestingLength.SaveData(oXml, "RestingLength")
            m_snLwidth.SaveData(oXml, "Lwidth")
            m_snPeLengthPercentage.SaveData(oXml, "PeLength")
            m_snMinPeLengthPercentage.SaveData(oXml, "MinPeLength")

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

            oXml.IntoElem()

            m_snRestingLength.SaveSimulationXml(oXml, Me, "RestingLength")
            m_snLwidth.SaveSimulationXml(oXml, Me, "Lwidth")
            m_snPeLengthPercentage.SaveSimulationXml(oXml, Me, "PeLength")
            m_snMinPeLengthPercentage.SaveSimulationXml(oXml, Me, "MinPeLength")

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
                                        "Muscle Properties", "Sets the resting length of the muscle.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snLwidth.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Lwidth", pbNumberBag.GetType(), "Lwidth", _
                                        "Muscle Properties", "Sets the width of the inverted parabola used for tension-length curve.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snPeLengthPercentage.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Pe Length Percentage", pbNumberBag.GetType(), "PeLengthPercentage", _
                          "Muscle Properties", "The percentage of the resting length of the muscle that Pe section takes up.", pbNumberBag, _
                          "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snMinPeLengthPercentage.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Min Pe Length Percentage", pbNumberBag.GetType(), "MinPeLengthPercentage", _
                          "Muscle Properties", "The minimum length, as a percentage of resting length, that the Pe section can go attain.", pbNumberBag, _
                          "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            If Not m_snRestingLength Is Nothing Then m_snRestingLength.ClearIsDirty()
            If Not m_snLwidth Is Nothing Then m_snLwidth.ClearIsDirty()
            If Not m_snPeLengthPercentage Is Nothing Then m_snPeLengthPercentage.ClearIsDirty()
            If Not m_snMinPeLengthPercentage Is Nothing Then m_snMinPeLengthPercentage.ClearIsDirty()
        End Sub

#End Region

#End Region

    End Class
End Namespace



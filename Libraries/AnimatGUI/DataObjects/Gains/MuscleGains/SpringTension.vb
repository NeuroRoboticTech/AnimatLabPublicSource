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

    Public Class SpringTension
        Inherits DataObjects.Gain

#Region " Attributes "

        Protected m_Muscle As DataObjects.Physical.IMuscle

        Protected m_snStiffness As AnimatGUI.Framework.ScaledNumber
        Protected m_snQuadraticLimit As AnimatGUI.Framework.ScaledNumber

        Protected m_dblKl As Double
        Protected m_dblB As Double

        Protected m_strImageName As String = ""

        Protected m_bStiffnessRequired As Boolean = True

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
                Return "Spring-Tension Curve"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property Type() As String
            Get
                Return "SpringTension"
            End Get
        End Property

        Public Overrides ReadOnly Property GainEquation() As String
            Get
                Return "Y = Stiffness * Extension^2"
            End Get
        End Property

        <Category("Equation Parameters"), _
         Description("Sets the X offset for the sigmoid for the tension-stimulus curve."), _
         TypeConverter(GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter))> _
        Public Overridable Property Stiffness() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snStiffness
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Not Value Is Nothing Then
                    If Value.ActualValue < 0 Then
                        Throw New System.Exception("The Stiffness can not be less than zero.")
                    End If

                    If Value.ActualValue = 0 AndAlso m_bStiffnessRequired Then
                        Throw New System.Exception("The Stiffness can not be less than or equal to zero.")
                    End If

                    SetSimData("Stiffness", Value.ActualValue.ToString, True)

                    Dim snOrig As ScaledNumber = DirectCast(m_snStiffness.Clone(m_snStiffness.Parent, False, Nothing), ScaledNumber)
                    If Not Value Is Nothing Then m_snStiffness.CopyData(Value)

                    Dim snNew As ScaledNumber = DirectCast(m_snStiffness.Clone(m_snStiffness.Parent, False, Nothing), ScaledNumber)
                    Me.ManualAddPropertyHistory("Stiffness", snOrig, snNew, True)
                    'RecalculuateLimits()
                End If
            End Set
        End Property

        <Category("Equation Parameters"), _
         Description("Sets the height for the sigmoid for the tension-stimulus curve."), _
         TypeConverter(GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter))> _
        Public Overridable Property QuadraticLimit() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snQuadraticLimit
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)

                If Not Value Is Nothing Then
                    If Value.ActualValue <= 0 Then
                        Throw New System.Exception("The quadratic limit can not be less than zero.")
                    End If

                    SetSimData("QuadraticLimit", Value.ActualValue.ToString, True)

                    Dim snOrig As ScaledNumber = DirectCast(m_snQuadraticLimit.Clone(m_snQuadraticLimit.Parent, False, Nothing), ScaledNumber)
                    If Not Value Is Nothing Then m_snQuadraticLimit.CopyData(Value)

                    Dim snNew As ScaledNumber = DirectCast(m_snQuadraticLimit.Clone(m_snQuadraticLimit.Parent, False, Nothing), ScaledNumber)
                    Me.ManualAddPropertyHistory("QuadraticLimit", snOrig, snNew, True)
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
                Return m_strImageName
            End Get
        End Property

        <Browsable(False)> _
        Public Property SpringImageName() As String
            Get
                Return m_strImageName
            End Get
            Set(ByVal Value As String)
                m_strImageName = Value
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            m_Muscle = DirectCast(doParent, DataObjects.Physical.IMuscle)

            Dim strUnit As String = "N/m^2"
            If Not Util.Environment Is Nothing Then
                strUnit = "N/" & Util.Environment.DistanceUnitAbbreviation(Util.Environment.DisplayDistanceUnits) & "^2"
            End If

            m_snStiffness = New AnimatGUI.Framework.ScaledNumber(Me, "Stiffness", 5, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, strUnit, strUnit)
            m_snQuadraticLimit = New AnimatGUI.Framework.ScaledNumber(Me, "QuadraticLimit", 0.5, AnimatGUI.Framework.ScaledNumber.enumNumericScale.centi, "Meters", "m")

            m_snLowerLimit = New AnimatGUI.Framework.ScaledNumber(Me, "LowerLimit", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Meters", "m")
            m_snUpperLimit = New AnimatGUI.Framework.ScaledNumber(Me, "UpperLimit", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Meters", "m")
            m_snLowerOutput = New AnimatGUI.Framework.ScaledNumber(Me, "LowerOutput", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Newtons", "N")
            m_snUpperOutput = New AnimatGUI.Framework.ScaledNumber(Me, "UpperOutput", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Newtons", "N")

            'RecalculuateLimits()

            m_strIndependentUnits = "Extension from resting length (Meters)"
            m_strDependentUnits = "Spring Tension (Newtons)"

        End Sub

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal strGainPropertyName As String, ByVal strImageName As String, ByVal bStiffnessRequired As Boolean)
            MyBase.New(doParent)

            m_strImageName = strImageName

            m_Muscle = DirectCast(doParent, DataObjects.Physical.IMuscle)

            Dim strUnit As String = "N/m^2"
            If Not Util.Environment Is Nothing Then
                strUnit = "N/" & Util.Environment.DistanceUnitAbbreviation(Util.Environment.DisplayDistanceUnits) & "^2"
            End If

            m_snStiffness = New AnimatGUI.Framework.ScaledNumber(Me, "Stiffness", 5, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, strUnit, strUnit)
            m_snQuadraticLimit = New AnimatGUI.Framework.ScaledNumber(Me, "QuadraticLimit", 0.5, AnimatGUI.Framework.ScaledNumber.enumNumericScale.centi, "Meters", "m")

            m_snLowerLimit = New AnimatGUI.Framework.ScaledNumber(Me, "LowerLimit", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Meters", "m")
            m_snUpperLimit = New AnimatGUI.Framework.ScaledNumber(Me, "UpperLimit", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Meters", "m")
            m_snLowerOutput = New AnimatGUI.Framework.ScaledNumber(Me, "LowerOutput", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Newtons", "N")
            m_snUpperOutput = New AnimatGUI.Framework.ScaledNumber(Me, "UpperOutput", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Newtons", "N")

            'RecalculuateLimits()

            m_strGainPropertyName = strGainPropertyName
            m_strIndependentUnits = "Extension from resting length (Meters)"
            m_strDependentUnits = "Spring Tension (Newtons)"
            m_bStiffnessRequired = bStiffnessRequired

        End Sub

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal strGainPropertyName As String, ByVal strIndependentUnits As String, ByVal strDependentUnits As String, _
                       ByVal bLimitsReadOnly As Boolean, ByVal bLimitOutputsReadOnly As Boolean, ByVal strImageName As String, ByVal bStiffnessRequired As Boolean)
            MyBase.New(doParent)

            m_Muscle = DirectCast(doParent, DataObjects.Physical.IMuscle)

            Dim strUnit As String = "N/m^2"
            If Not Util.Environment Is Nothing Then
                strUnit = "N/" & Util.Environment.DistanceUnitAbbreviation(Util.Environment.DisplayDistanceUnits) & "^2"
            End If

            m_snStiffness = New AnimatGUI.Framework.ScaledNumber(Me, "Stiffness", 5, AnimatGUI.Framework.ScaledNumber.enumNumericScale.Kilo, strUnit, strUnit)
            m_snQuadraticLimit = New AnimatGUI.Framework.ScaledNumber(Me, "QuadraticLimit", 10, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "Meters", "m")

            m_snLowerLimit = New AnimatGUI.Framework.ScaledNumber(Me, "LowerLimit", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Meters", "m")
            m_snUpperLimit = New AnimatGUI.Framework.ScaledNumber(Me, "UpperLimit", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Meters", "m")
            m_snLowerOutput = New AnimatGUI.Framework.ScaledNumber(Me, "LowerOutput", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Newtons", "N")
            m_snUpperOutput = New AnimatGUI.Framework.ScaledNumber(Me, "UpperOutput", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Newtons", "N")

            m_strImageName = strImageName

            'RecalculuateLimits()

            m_strIndependentUnits = strIndependentUnits
            m_strDependentUnits = strDependentUnits
            m_bLimitsReadOnly = bLimitsReadOnly
            m_bLimitOutputsReadOnly = bLimitOutputsReadOnly

            m_strIndependentUnits = "Extension from resting length (Meters)"
            m_strDependentUnits = "Spring Tension (Newtons)"
            m_bStiffnessRequired = bStiffnessRequired

        End Sub

        Public Overrides Function CalculateGain(ByVal dblInput As Double) As Double

            If (InLimits(dblInput)) Then
                Dim dblT As Double

                If dblInput > 0 AndAlso dblInput <= m_snQuadraticLimit.ActualValue Then
                    dblT = m_snStiffness.ActualValue * Math.Pow(dblInput, 2)
                ElseIf dblInput > 0 AndAlso dblInput > m_snQuadraticLimit.ActualValue Then
                    dblT = m_dblKl * dblInput + m_dblB
                Else
                    dblT = 0
                End If

                Return dblT
            Else
                Return CalculateLimitOutput(dblInput)
            End If

        End Function

        Public Overrides Sub RecalculuateLimits()


            m_dblKl = 2 * m_snStiffness.ActualValue * m_snQuadraticLimit.ActualValue
            m_dblB = -m_snStiffness.ActualValue * Math.Pow(m_snQuadraticLimit.ActualValue, 2)

            If Not m_bUseLimits Then
                m_snLowerLimit.ActualValue = 0
                m_snUpperLimit.ActualValue = m_snQuadraticLimit.ActualValue * 3
            End If

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNew As New DataObjects.Gains.MuscleGains.SpringTension(doParent)
            oNew.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNew.AfterClone(Me, bCutData, doRoot, oNew)
            Return oNew
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim gnOrig As DataObjects.Gains.MuscleGains.SpringTension = DirectCast(doOriginal, DataObjects.Gains.MuscleGains.SpringTension)

            m_Muscle = gnOrig.Muscle
            m_snStiffness = DirectCast(gnOrig.m_snStiffness.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snQuadraticLimit = DirectCast(gnOrig.m_snQuadraticLimit.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_strImageName = gnOrig.m_strImageName

            'RecalculuateLimits()
        End Sub

        Protected Overrides Sub SetAllSimData(ByVal doInterface As ManagedAnimatInterfaces.IDataObjectInterface)
            MyBase.SetAllSimData(doInterface)

            SetSimData("Stiffness", m_snStiffness.ActualValue.ToString, True)
            SetSimData("QuadraticLimit", m_snQuadraticLimit.ActualValue.ToString, True)
        End Sub

        Public Overrides Sub UnitsChanged(ByVal ePrevMass As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits, _
                                          ByVal eNewMass As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits, _
                                          ByVal fltMassChange As Single, _
                                          ByVal ePrevDistance As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits, _
                                          ByVal eNewDistance As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits, _
                                          ByVal fltDistanceChange As Single)

            Dim iDistDiff As Integer = CInt(eNewDistance) - CInt(Util.Environment.DisplayDistanceUnits(ePrevDistance))
            Dim fltDistChange As Single = CSng(10 ^ iDistDiff)

            m_snQuadraticLimit.ActualValue = m_snQuadraticLimit.ActualValue * fltDistChange

            Dim strUnit As String = "N/" & Util.Environment.DistanceUnitAbbreviation(Util.Environment.DisplayDistanceUnits) & "^2"
            Dim dblStiffness As Double = m_snStiffness.ActualValue * Math.Pow(fltDistChange, 2)
            m_snStiffness = New AnimatGUI.Framework.ScaledNumber(Me, "Stiffness", strUnit, strUnit)
            m_snStiffness.ActualValue = dblStiffness

            m_snLowerLimit.ActualValue = m_snLowerLimit.ActualValue * fltDistChange
            m_snUpperLimit.ActualValue = m_snUpperLimit.ActualValue * fltDistChange
            m_snLowerOutput.ActualValue = m_snLowerOutput.ActualValue * fltDistChange
            m_snUpperOutput.ActualValue = m_snUpperOutput.ActualValue * fltDistChange
        End Sub

        Public Overloads Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml, ByVal strName As String, ByVal strGainPropertyName As String)
            MyBase.LoadData(oXml, strName, strGainPropertyName)

            oXml.IntoElem()

            If oXml.FindChildElement("Stiffness", False) Then
                m_snStiffness.LoadData(oXml, "Stiffness")
                m_snQuadraticLimit.LoadData(oXml, "QuadraticLimit")
            Else
                m_snStiffness.LoadData(oXml, "Compliance")
                m_snQuadraticLimit.LoadData(oXml, "QuadraticLimit")
            End If

            oXml.OutOfElem()

            'RecalculuateLimits()

            m_strIndependentUnits = "Extension from resting length (Meters)"
            m_strDependentUnits = "Spring Tension (Newtons)"

        End Sub

        Public Overloads Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml, ByVal strName As String)
            MyBase.SaveData(oXml, strName)

            oXml.IntoElem()

            m_snStiffness.SaveData(oXml, "Stiffness")
            m_snQuadraticLimit.SaveData(oXml, "QuadraticLimit")

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

            oXml.IntoElem()

            m_snStiffness.SaveSimulationXml(oXml, Me, "Stiffness")
            m_snQuadraticLimit.SaveSimulationXml(oXml, Me, "QuadraticLimit")

            oXml.OutOfElem()

        End Sub

        Public Overrides Function ToString() As String
            Return ""
        End Function

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = m_snStiffness.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Stiffness", pbNumberBag.GetType(), "Stiffness", _
                                        "Equation Parameters", "Sets the resting length of the muscle.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snQuadraticLimit.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Quadratic Limit", pbNumberBag.GetType(), "QuadraticLimit", _
                                        "Equation Parameters", "Sets the width of the inverted parabola used for tension-length curve.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            If Not m_snStiffness Is Nothing Then m_snStiffness.ClearIsDirty()
            If Not m_snQuadraticLimit Is Nothing Then m_snQuadraticLimit.ClearIsDirty()
        End Sub

#End Region

#End Region

    End Class
End Namespace



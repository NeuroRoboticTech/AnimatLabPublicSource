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

    Public MustInherit Class Gain
        Inherits Framework.DataObject

#Region " Attributes "

        Protected m_bUseLimits As Boolean
        Protected m_bLimitsReadOnly As Boolean
        Protected m_bLimitOutputsReadOnly As Boolean
        Protected m_bUseParentIncomingDataType As Boolean

        Protected m_snLowerLimit As AnimatGUI.Framework.ScaledNumber
        Protected m_snUpperLimit As AnimatGUI.Framework.ScaledNumber
        Protected m_snLowerOutput As AnimatGUI.Framework.ScaledNumber
        Protected m_snUpperOutput As AnimatGUI.Framework.ScaledNumber

        Protected m_strIndependentUnits As String = ""
        Protected m_strDependentUnits As String = ""

        Protected m_bdParentData As Behavior.Data
        Protected m_strGainPropertyName As String = ""

        Protected m_imgGain As Image

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public MustOverride ReadOnly Property GainType() As String

        <Browsable(False)> _
        Public MustOverride ReadOnly Property Type() As String

        <Browsable(False)> _
        Public MustOverride ReadOnly Property GainEquation() As String

        <Browsable(False)> _
        Public Overridable Property GainPropertyName() As String
            Get
                Return m_strGainPropertyName
            End Get
            Set(ByVal Value As String)
                m_strGainPropertyName = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property UseLimits() As Boolean
            Get
                Return m_bUseLimits
            End Get
            Set(ByVal Value As Boolean)
                SetSimData("UseLimits", Value.ToString, True)

                'If we are activating this we may need to set the rest of the limit values. So call this here.
                SetAllSimData(m_doInterface)

                Me.ManualAddPropertyHistory("UseLimits", m_bUseLimits, Value, True)

                m_bUseLimits = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property LimitsReadOnly() As Boolean
            Get
                Return m_bLimitsReadOnly
            End Get
            Set(ByVal Value As Boolean)
                Me.ManualAddPropertyHistory("LimitsReadOnly", m_bLimitsReadOnly, Value, True)

                m_bLimitsReadOnly = Value
                If m_bLimitsReadOnly Then m_bUseLimits = True
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property LimitOutputsReadOnly() As Boolean
            Get
                Return m_bLimitOutputsReadOnly
            End Get
            Set(ByVal Value As Boolean)
                Me.ManualAddPropertyHistory("LimitOutputsReadOnly", m_bLimitOutputsReadOnly, Value, True)

                m_bLimitOutputsReadOnly = Value
                If m_bLimitsReadOnly Then m_bUseLimits = True
            End Set
        End Property

        '<Category("Gain Limits"), _
        ' Description("Sets the lower limit of the x variable."), _
        ' TypeConverter(GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter))> _
        <Browsable(False)> _
        Public Overridable Property LowerLimit() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snLowerLimit
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Not Value Is Nothing Then
                    SetSimData("LowerLimit", Value.ActualValue.ToString, True)

                    Dim snOrig As ScaledNumber = DirectCast(m_snLowerLimit.Clone(m_snLowerLimit.Parent, False, Nothing), ScaledNumber)
                    If Not Value Is Nothing Then m_snLowerLimit.CopyData(Value)

                    Dim snNew As ScaledNumber = DirectCast(m_snLowerLimit.Clone(m_snLowerLimit.Parent, False, Nothing), ScaledNumber)
                    Me.ManualAddPropertyHistory("LowerLimit", snOrig, snNew, True)
                End If
            End Set
        End Property

        '<Category("Gain Limits"), _
        ' Description("Sets the upper limit of the x variable."), _
        ' TypeConverter(GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter))> _
        <Browsable(False)> _
        Public Overridable Property UpperLimit() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snUpperLimit
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Not Value Is Nothing Then
                    SetSimData("UpperLimit", Value.ActualValue.ToString, True)

                    Dim snOrig As ScaledNumber = DirectCast(m_snUpperLimit.Clone(m_snUpperLimit.Parent, False, Nothing), ScaledNumber)
                    If Not Value Is Nothing Then m_snUpperLimit.CopyData(Value)

                    Dim snNew As ScaledNumber = DirectCast(m_snUpperLimit.Clone(m_snUpperLimit.Parent, False, Nothing), ScaledNumber)
                    Me.ManualAddPropertyHistory("UpperLimit", snOrig, snNew, True)
                End If
            End Set
        End Property

        '<Category("Gain Limits"), _
        ' Description("Sets the output value to use when the x value is less than the lower limit."), _
        ' TypeConverter(GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter))> _
        <Browsable(False)> _
        Public Overridable Property LowerOutput() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snLowerOutput
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Not Value Is Nothing Then
                    SetSimData("LowerOutput", Value.ActualValue.ToString, True)

                    Dim snOrig As ScaledNumber = DirectCast(m_snLowerOutput.Clone(m_snLowerOutput.Parent, False, Nothing), ScaledNumber)
                    If Not Value Is Nothing Then m_snLowerOutput.CopyData(Value)

                    Dim snNew As ScaledNumber = DirectCast(m_snLowerOutput.Clone(m_snLowerOutput.Parent, False, Nothing), ScaledNumber)
                    Me.ManualAddPropertyHistory("LowerOutput", snOrig, snNew, True)
                End If
            End Set
        End Property

        '<Category("Gain Limits"), _
        ' Description("Sets the output value to use when the x value is more than the upper limit."), _
        ' TypeConverter(GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter))> _
        <Browsable(False)> _
        Public Overridable Property UpperOutput() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snUpperOutput
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Not Value Is Nothing Then
                    SetSimData("UpperOutput", Value.ActualValue.ToString, True)

                    Dim snOrig As ScaledNumber = DirectCast(m_snUpperOutput.Clone(m_snUpperOutput.Parent, False, Nothing), ScaledNumber)
                    If Not Value Is Nothing Then m_snUpperOutput.CopyData(Value)

                    Dim snNew As ScaledNumber = DirectCast(m_snUpperOutput.Clone(m_snUpperOutput.Parent, False, Nothing), ScaledNumber)
                    Me.ManualAddPropertyHistory("UpperOutput", snOrig, snNew, True)
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property UseParentIncomingDataType() As Boolean
            Get
                Return m_bUseParentIncomingDataType
            End Get
            Set(ByVal Value As Boolean)
                Me.ManualAddPropertyHistory("UseParentIncomingDataType", m_bUseParentIncomingDataType, Value, True)

                m_bUseParentIncomingDataType = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides Property ViewSubProperties() As Boolean
            Get
                Return False
            End Get
            Set(ByVal Value As Boolean)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property IndependentUnits() As String
            Get
                Return m_strIndependentUnits
            End Get
            Set(ByVal Value As String)
                m_strIndependentUnits = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property DependentUnits() As String
            Get
                Return m_strDependentUnits
            End Get
            Set(ByVal Value As String)
                m_strDependentUnits = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Property ParentData() As AnimatGUI.DataObjects.Behavior.Data
            Get
                Return m_bdParentData
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Behavior.Data)
                m_bdParentData = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property SelectableGain() As Boolean
            Get
                Return True
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property BarAssemblyFile() As String
            Get
                Return "AnimatGUI.dll"
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property BarClassName() As String
            Get
                Return "AnimatGUI.Forms.Gain.SelectGainType"
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property DraggableParent() As AnimatGUI.DataObjects.DragObject
            Get
                If Not m_doParent Is Nothing AndAlso TypeOf m_doParent Is AnimatGUI.DataObjects.DragObject Then
                    Return DirectCast(m_doParent, AnimatGUI.DataObjects.DragObject)
                Else
                    Return Nothing
                End If
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property GainImage() As Image
            Get
                If m_imgGain Is Nothing AndAlso Me.GainImageName <> "" Then
                    m_imgGain = ImageManager.LoadImage(Me.GainImageName)
                End If

                Return m_imgGain
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property GainImageName() As String
            Get
                Return ""
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            m_strName = "Gain"
            m_snLowerLimit = New AnimatGUI.Framework.ScaledNumber(Me, "LowerLimit", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")
            m_snUpperLimit = New AnimatGUI.Framework.ScaledNumber(Me, "UpperLimit", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")
            m_snLowerOutput = New AnimatGUI.Framework.ScaledNumber(Me, "LowerOutput", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")
            m_snUpperOutput = New AnimatGUI.Framework.ScaledNumber(Me, "UpperOutput", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")
            m_bUseParentIncomingDataType = True
        End Sub

        Public Sub New(ByVal doParent As Framework.DataObject, ByVal strIndependentUnits As String, ByVal strDependentUnits As String, _
                       ByVal bLimitsReadOnly As Boolean, ByVal bLimitOutputsReadOnly As Boolean, ByVal bUseParentIncomingDataType As Boolean)
            MyBase.New(doParent)

            m_snLowerLimit = New AnimatGUI.Framework.ScaledNumber(Me, "LowerLimit", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")
            m_snUpperLimit = New AnimatGUI.Framework.ScaledNumber(Me, "UpperLimit", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")
            m_snLowerOutput = New AnimatGUI.Framework.ScaledNumber(Me, "LowerOutput", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")
            m_snUpperOutput = New AnimatGUI.Framework.ScaledNumber(Me, "UpperOutput", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")

            m_bUseParentIncomingDataType = bUseParentIncomingDataType
            m_strIndependentUnits = strIndependentUnits
            m_strDependentUnits = strDependentUnits
            m_bLimitsReadOnly = bLimitsReadOnly
            m_bLimitOutputsReadOnly = bLimitOutputsReadOnly
        End Sub

        Public Overridable Function InLimits(ByVal dblInput As Double) As Boolean
            If m_bUseLimits AndAlso ((dblInput < m_snLowerLimit.ActualValue) OrElse (dblInput > m_snUpperLimit.ActualValue)) Then
                Return False
            Else
                Return True
            End If
        End Function

        Public Overridable Function CalculateLimitOutput(ByVal dblInput As Double) As Double
            If dblInput < m_snLowerLimit.ActualValue Then Return m_snLowerOutput.ActualValue

            If dblInput > m_snUpperLimit.ActualValue Then Return m_snUpperOutput.ActualValue

            Return 0
        End Function

        Public MustOverride Function CalculateGain(ByVal dblInput As Double) As Double

        Public Overridable Sub RecalculuateLimits()
        End Sub

        Protected Overridable Sub SetAllSimData(ByVal doInterface As ManagedAnimatInterfaces.IDataObjectInterface)
            m_doInterface = doInterface
            SetSimData("UseLimits", m_bUseLimits.ToString, True)
            SetSimData("LowerLimit", m_snLowerLimit.ActualValue.ToString, True)
            SetSimData("UpperLimit", m_snUpperLimit.ActualValue.ToString, True)
            SetSimData("LowerOutput", m_snLowerOutput.ActualValue.ToString, True)
            SetSimData("UpperOutput", m_snUpperOutput.ActualValue.ToString, True)
        End Sub

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim OrigNode As DataObjects.Gain = DirectCast(doOriginal, DataObjects.Gain)

            m_strGainPropertyName = OrigNode.m_strGainPropertyName
            m_bUseLimits = OrigNode.m_bUseLimits
            m_bLimitsReadOnly = OrigNode.m_bLimitsReadOnly
            m_bLimitOutputsReadOnly = OrigNode.m_bLimitOutputsReadOnly
            m_bUseParentIncomingDataType = OrigNode.m_bUseParentIncomingDataType

            m_snLowerLimit = DirectCast(OrigNode.m_snLowerLimit.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snUpperLimit = DirectCast(OrigNode.m_snUpperLimit.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snLowerOutput = DirectCast(OrigNode.m_snLowerOutput.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snUpperOutput = DirectCast(OrigNode.m_snUpperOutput.Clone(Me, bCutData, doRoot), ScaledNumber)

            m_strIndependentUnits = OrigNode.m_strIndependentUnits
            m_strDependentUnits = OrigNode.m_strDependentUnits
        End Sub

        Public Overrides Function ToString() As String
            Return Me.Type
        End Function

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("ID", Me.ID.GetType(), "ID", _
                                        "Gain Limits", "ID", Me.ID, True))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Use Limits", m_bUseLimits.GetType(), "UseLimits", _
                                        "Gain Limits", "Sets the whether to use the upper and lower limits on the x variable.", m_bUseLimits, _
                                        (m_bLimitsReadOnly Or m_bLimitOutputsReadOnly)))

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = m_snLowerLimit.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Lower Limit", pbNumberBag.GetType(), "LowerLimit", _
                                        "Gain Limits", "Sets the lower limit of the x variable.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter), m_bLimitsReadOnly))

            pbNumberBag = m_snUpperLimit.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Upper Limit", pbNumberBag.GetType(), "UpperLimit", _
                                        "Gain Limits", "Sets the upper limit of the x variable.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter), m_bLimitsReadOnly))

            pbNumberBag = m_snLowerOutput.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Value @ Lower Limit", pbNumberBag.GetType(), "LowerOutput", _
                                        "Gain Limits", "Sets the output value to use when the x value is less than the lower limit.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter), m_bLimitOutputsReadOnly))

            pbNumberBag = m_snUpperOutput.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Value @ Upper Limit", pbNumberBag.GetType(), "UpperOutput", _
                                        "Gain Limits", "Sets the output value to use when the x value is more than the upper limit.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter), m_bLimitOutputsReadOnly))

        End Sub

        Public Overridable Overloads Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml, ByVal strName As String, ByVal strGainPropertyName As String)

            m_strGainPropertyName = strGainPropertyName

            oXml.IntoChildElement(strName)

            m_strID = oXml.GetChildString("ID", Me.ID)
            m_bUseLimits = oXml.GetChildBool("UseLimits")

            m_bLimitsReadOnly = oXml.GetChildBool("LimitsReadOnly", m_bLimitsReadOnly)
            m_bLimitOutputsReadOnly = oXml.GetChildBool("LimitOutputsReadOnly", m_bLimitOutputsReadOnly)
            m_bUseParentIncomingDataType = oXml.GetChildBool("UseParentIncomingDataType", m_bUseParentIncomingDataType)

            If oXml.FindChildElement("LowerLimitScale", False) Then m_snLowerLimit.LoadData(oXml, "LowerLimitScale")
            If oXml.FindChildElement("UpperLimitScale", False) Then m_snUpperLimit.LoadData(oXml, "UpperLimitScale")
            If oXml.FindChildElement("LowerOutputScale", False) Then m_snLowerOutput.LoadData(oXml, "LowerOutputScale")
            If oXml.FindChildElement("UpperOutputScale", False) Then m_snUpperOutput.LoadData(oXml, "UpperOutputScale")

            Dim strIndependentUnits As String = oXml.GetChildString("IndependentUnits", "")
            Dim strDependentUnits As String = oXml.GetChildString("DependentUnits", "")

            If m_strIndependentUnits.Trim.Length > 0 Then m_strIndependentUnits = strIndependentUnits
            If strDependentUnits.Trim.Length > 0 Then m_strDependentUnits = strDependentUnits

            oXml.OutOfElem()

        End Sub


        Public Overridable Overloads Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml, ByVal strName As String)

            oXml.AddChildElement(strName)
            oXml.IntoElem()

            oXml.AddChildElement("ID", Me.ID)
            oXml.AddChildElement("Type", Me.Type())
            oXml.AddChildElement("AssemblyFile", Me.AssemblyFile)
            oXml.AddChildElement("ClassName", Me.ClassName)

            oXml.AddChildElement("UseLimits", m_bUseLimits)

            oXml.AddChildElement("LimitsReadOnly", m_bLimitsReadOnly)
            oXml.AddChildElement("LimitOutputsReadOnly", m_bLimitOutputsReadOnly)
            oXml.AddChildElement("UseParentIncomingDataType", m_bUseParentIncomingDataType)

            m_snLowerLimit.SaveData(oXml, "LowerLimitScale")
            m_snUpperLimit.SaveData(oXml, "UpperLimitScale")
            m_snLowerOutput.SaveData(oXml, "LowerOutputScale")
            m_snUpperOutput.SaveData(oXml, "UpperOutputScale")

            oXml.AddChildElement("IndependentUnits", m_strIndependentUnits)
            oXml.AddChildElement("DependentUnits", m_strDependentUnits)

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            oXml.AddChildElement(strName)
            oXml.IntoElem()

            oXml.AddChildElement("ID", Me.ID)
            If Me.ModuleName.Trim.Length > 0 Then
                oXml.AddChildElement("ModuleName", Me.ModuleName())
            End If

            oXml.AddChildElement("Type", Me.Type())
            oXml.AddChildElement("UseLimits", m_bUseLimits)
            If m_bUseLimits Then
                m_snLowerLimit.SaveSimulationXml(oXml, Me, "LowerLimit")
                m_snUpperLimit.SaveSimulationXml(oXml, Me, "UpperLimit")
                m_snLowerOutput.SaveSimulationXml(oXml, Me, "LowerOutput")
                m_snUpperOutput.SaveSimulationXml(oXml, Me, "UpperOutput")
            End If

            oXml.OutOfElem()

        End Sub

#End Region

#End Region

    End Class

End Namespace

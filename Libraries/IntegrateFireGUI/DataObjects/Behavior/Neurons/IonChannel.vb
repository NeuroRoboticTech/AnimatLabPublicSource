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

Namespace DataObjects.Behavior.Neurons

    Public Class IonChannel
        Inherits AnimatGUI.DataObjects.DragObject

#Region " Attributes "

        Protected m_strSymbol As String = ""

        'Params for I = Gmax*m^mpower*h^hpower*(Vm-EqPot)
        Protected m_snGmax As ScaledNumber
        Protected m_iMPower As Integer = 1
        Protected m_iHPower As Integer = 1
        Protected m_snEquilibriumPotential As ScaledNumber

        'Params for dm/dt = (Minf(V)-m)/n1*Tm(V)
        Protected m_snMinit As ScaledNumber
        Protected m_snNm As ScaledNumber
        Protected m_gnMinf As AnimatGUI.DataObjects.Gain
        Protected m_gnTm As AnimatGUI.DataObjects.Gain

        'Params for dh/dt = (Hinf(V)-h)/n2*Th(V)
        Protected m_snHinit As ScaledNumber
        Protected m_snNh As ScaledNumber
        Protected m_gnHinf As AnimatGUI.DataObjects.Gain
        Protected m_gnTh As AnimatGUI.DataObjects.Gain

        Protected m_liListItem As ListViewItem

#End Region

#Region " Properties "

#Region " Drag ObjectProperties "

        <Browsable(False)> _
        Public Overrides Property ItemName() As String
            Get
                Return Me.Name
            End Get
            Set(ByVal Value As String)
                Me.Name = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "IntegrateFireGUI.IonChannel.gif"
            End Get
        End Property


        <Browsable(False)> _
        Public Overrides ReadOnly Property DragImageName() As String
            Get
                Return Me.WorkspaceImageName
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property StructureID() As String
            Get
                If Not Me.Parent Is Nothing AndAlso TypeOf Me.Parent Is DataObjects.Behavior.Neurons.Spiking Then
                    Dim doNeuron As DataObjects.Behavior.Neurons.Spiking = DirectCast(Me.Parent, DataObjects.Behavior.Neurons.Spiking)
                    If Not doNeuron.Organism Is Nothing Then
                        Return doNeuron.Organism.ID
                    Else
                        Return ""
                    End If
                Else
                    Return ""
                End If
            End Get
        End Property

        'Tells if this data item can be added to a chart object. certain items like graphical node can not.
        <Browsable(False)> _
        Public Overrides ReadOnly Property CanBeCharted() As Boolean
            Get
                Return True
            End Get
        End Property

#End Region

        <Browsable(False)> _
        Public Overrides Property Name() As String
            Get
                Return m_strName
            End Get
            Set(ByVal Value As String)
                m_strName = Value
                If Not m_liListItem Is Nothing Then
                    m_liListItem.Text = m_strName
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property Symbol() As String
            Get
                Return m_strSymbol
            End Get
            Set(ByVal Value As String)
                m_strSymbol = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property Gmax() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snGmax
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue < 0 OrElse Value.ActualValue > 0.1 Then
                    Throw New System.Exception("The maximum conductance must be greater than zero and less than 100 mS.")
                End If

                SetSimData("Gmax", Value.ActualValue.ToString, True)
                m_snGmax.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property MPower() As Integer
            Get
                Return m_iMPower
            End Get
            Set(ByVal Value As Integer)
                SetSimData("MPower", Value.ToString, True)
                m_iMPower = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property HPower() As Integer
            Get
                Return m_iHPower
            End Get
            Set(ByVal Value As Integer)
                SetSimData("HPower", Value.ToString, True)
                m_iHPower = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property EquilibriumPotential() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snEquilibriumPotential
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue < -0.2 OrElse Value.ActualValue > 0.2 Then
                    Throw New System.Exception("The maximum conductance must be between -200 mV and 200 mV.")
                End If

                SetSimData("EquilibriumPotential", Value.ActualValue.ToString, True)
                m_snEquilibriumPotential.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property Minit() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snMinit
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue < 0 OrElse Value.ActualValue > 1 Then
                    Throw New System.Exception("The initial activation value (m) must be greater than zero and less than one.")
                End If

                SetSimData("Minit", Value.ActualValue.ToString, True)
                m_snMinit.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property Nm() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snNm
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                SetSimData("Nm", Value.ActualValue.ToString, True)
                m_snNm.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property Minf() As AnimatGUI.DataObjects.Gain
            Get
                Return m_gnMinf
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Gain)
                If Not Value Is Nothing Then
                    SetSimData("Minf", Value.GetSimulationXml("Gain", Me), True)
                    Value.InitializeSimulationReferences()
                End If

                m_gnMinf = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property Tm() As AnimatGUI.DataObjects.Gain
            Get
                Return m_gnTm
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Gain)
                If Not Value Is Nothing Then
                    SetSimData("Tm", Value.GetSimulationXml("Gain", Me), True)
                    Value.InitializeSimulationReferences()
                End If

                m_gnTm = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property Hinit() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snHinit
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue < 0 OrElse Value.ActualValue > 1 Then
                    Throw New System.Exception("The initial inactivation value (h) must be greater than zero and less than one.")
                End If

                SetSimData("Hinit", Value.ActualValue.ToString, True)
                m_snHinit.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property Nh() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snNh
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                SetSimData("Nh", Value.ActualValue.ToString, True)
                m_snNh.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property Hinf() As AnimatGUI.DataObjects.Gain
            Get
                Return m_gnHinf
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Gain)
                If Not Value Is Nothing Then
                    SetSimData("Hinf", Value.GetSimulationXml("Gain", Me), True)
                    Value.InitializeSimulationReferences()
                End If

                m_gnHinf = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property Th() As AnimatGUI.DataObjects.Gain
            Get
                Return m_gnTh
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Gain)
                If Not Value Is Nothing Then
                    SetSimData("Th", Value.GetSimulationXml("Gain", Me), True)
                    Value.InitializeSimulationReferences()
                End If

                m_gnTh = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property ListItem() As ListViewItem
            Get
                Return m_liListItem
            End Get
            Set(ByVal Value As ListViewItem)
                m_liListItem = Value
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            m_snGmax = New AnimatGUI.Framework.ScaledNumber(Me, "Gmax", 5, AnimatGUI.Framework.ScaledNumber.enumNumericScale.nano, "Siemens", "S")
            m_snEquilibriumPotential = New AnimatGUI.Framework.ScaledNumber(Me, "EquilibriumPotential", -30, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "Volts", "V")

            m_snMinit = New AnimatGUI.Framework.ScaledNumber(Me, "Minit", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")
            m_snNm = New AnimatGUI.Framework.ScaledNumber(Me, "Nm", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")

            m_snHinit = New AnimatGUI.Framework.ScaledNumber(Me, "Hinit", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")
            m_snNh = New AnimatGUI.Framework.ScaledNumber(Me, "Nh", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")

            m_gnMinf = New DataObjects.Gains.IonChannelSigmoid(Me, "Minf", "Volts", "", False, False, False)
            m_gnTm = New DataObjects.Gains.IonChannelSigmoid(Me, "Tm", "Volts", "", False, False, False)
            m_gnHinf = New DataObjects.Gains.IonChannelSigmoid(Me, "Hinf", "Volts", "", False, False, False)
            m_gnTh = New DataObjects.Gains.IonChannelSigmoid(Me, "Th", "Volts", "", False, False, False)

            'Lets add the data types that this node understands.
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("G", "Conductance", "Siemens", "S", -100, 100, ScaledNumber.enumNumericScale.nano, ScaledNumber.enumNumericScale.nano))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("M", "Activation", "", "", 0, 1, ScaledNumber.enumNumericScale.None, ScaledNumber.enumNumericScale.None))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("H", "Inactivation", "", "", 0, 1, ScaledNumber.enumNumericScale.None, ScaledNumber.enumNumericScale.None))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("I", "Current", "Amps", "A", -100, 100, ScaledNumber.enumNumericScale.nano, ScaledNumber.enumNumericScale.nano))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("Act", "Total Activation", "", "", 0, 1, ScaledNumber.enumNumericScale.None, ScaledNumber.enumNumericScale.None))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("Tm", "Activation Time Constant", "Seconds", "s", 0, 1, ScaledNumber.enumNumericScale.None, ScaledNumber.enumNumericScale.None))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("Th", "Inactivation Time Constant", "Seconds", "s", 0, 1, ScaledNumber.enumNumericScale.None, ScaledNumber.enumNumericScale.None))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("Minf", "Minf", "", "", 0, 1, ScaledNumber.enumNumericScale.None, ScaledNumber.enumNumericScale.None))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("Hinf", "Hinf", "", "", 0, 1, ScaledNumber.enumNumericScale.None, ScaledNumber.enumNumericScale.None))
            m_thDataTypes.ID = "I"

        End Sub

        Public Overrides Function FindDragObject(ByVal strStructureName As String, ByVal strDataItemID As String, Optional ByVal bThrowError As Boolean = True) As AnimatGUI.DataObjects.DragObject

            Dim oOrg As Object = Util.Environment.FindOrganism(strStructureName, bThrowError)
            If oOrg Is Nothing Then Return Nothing

            Dim doOrganism As AnimatGUI.DataObjects.Physical.Organism = DirectCast(oOrg, AnimatGUI.DataObjects.Physical.Organism)
            Dim doNode As AnimatGUI.DataObjects.Behavior.Node

            If Not doOrganism Is Nothing Then

                Dim aryNodes As AnimatGUI.Collections.DataObjects = New AnimatGUI.Collections.DataObjects(Nothing)
                doOrganism.RootSubSystem.FindChildrenOfType(GetType(DataObjects.Behavior.Neurons.Spiking), aryNodes)

                For Each doNeuron As DataObjects.Behavior.Neurons.Spiking In aryNodes
                    If doNeuron.IonChannels.Contains(strDataItemID) Then
                        Return doNeuron.IonChannels(strDataItemID)
                    End If
                Next

            End If

            If bThrowError AndAlso Not doNode Is Nothing Then
                Throw New System.Exception("The drag object with id '" & strDataItemID & "' was not found.")
            End If

            Return doNode

        End Function

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("ID", m_strID.GetType, "ID", _
                                        "Channel Properties", "The ID for this channel.", m_strID, True))

            'Channel properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Name", m_strName.GetType, "Name", _
                                        "Channel Properties", "The name for this channel.", m_strName))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Symbol", m_strSymbol.GetType, "Symbol", _
                                        "Channel Properties", "A symbol for this channel.", m_strSymbol))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Enabled", m_bEnabled.GetType, "Enabled", _
                                        "Channel Properties", "Determines whether this channel is active and generates currents in the neuron.", m_bEnabled))

            'Ion Current properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("M Power", m_iMPower.GetType, "MPower", _
                                        "Ion Current", "The power to which the activation variable is raised.", m_iMPower))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("H Power", m_iHPower.GetType, "HPower", _
                                        "Ion Current", "The power to which the inactivation variable is raised.", m_iHPower))

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = m_snGmax.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Gmax", pbNumberBag.GetType(), "Gmax", _
                                        "Ion Current", "The maximum conductance for this ion channel. " & _
                                        "Acceptable values are in the range 0 to 100 mS.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snEquilibriumPotential.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Equilibrium Potential", pbNumberBag.GetType(), "EquilibriumPotential", _
                                        "Ion Current", "The equilibrium potential for this ion channel. Acceptable values are in the range -200 to 200 mV.", _
                                        pbNumberBag, "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            'Activation properties
            pbNumberBag = m_snMinit.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("M Initial", pbNumberBag.GetType(), "Minit", _
                                        "Activation", "The initial value of the activation variable. Acceptable values are in the range 0 to 1.", _
                                        pbNumberBag, "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snNm.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("N", pbNumberBag.GetType(), "Nm", _
                                        "Activation", "The value of a scaling variable multiplied to the denominator of the activation derivative.", _
                                        pbNumberBag, "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_gnMinf.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Minf", pbNumberBag.GetType(), "Minf", _
                                        "Activation", "A gain function that describes the equation used in the numerator of the activation derivative.", _
                                        pbNumberBag, GetType(AnimatGUI.TypeHelpers.GainTypeEditor), GetType(AnimatGuiCtrls.Controls.ExpandablePropBagConverter)))

            pbNumberBag = m_gnTm.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Tm", pbNumberBag.GetType(), "Tm", _
                                        "Activation", "A gain function that describes the equation used in the denominator of the activation derivative.", _
                                        pbNumberBag, GetType(AnimatGUI.TypeHelpers.GainTypeEditor), GetType(AnimatGuiCtrls.Controls.ExpandablePropBagConverter)))

            'Inactivation properties
            pbNumberBag = m_snHinit.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("H Initial", pbNumberBag.GetType(), "Hinit", _
                                        "Inactivation", "The initial value of the inactivation variable. Acceptable values are in the range 0 to 1.", _
                                        pbNumberBag, "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snNh.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("N", pbNumberBag.GetType(), "Nh", _
                                        "Inactivation", "The value of a scaling variable multiplied to the denominator of the inactivation derivative.", _
                                        pbNumberBag, "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_gnHinf.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Hinf", pbNumberBag.GetType(), "Hinf", _
                                        "Inactivation", "A gain function that describes the equation used in the numerator of the inactivation derivative.", _
                                        pbNumberBag, GetType(AnimatGUI.TypeHelpers.GainTypeEditor), GetType(AnimatGuiCtrls.Controls.ExpandablePropBagConverter)))

            pbNumberBag = m_gnTh.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Th", pbNumberBag.GetType(), "Th", _
                                        "Inactivation", "A gain function that describes the equation used in the denominator of the inactivation derivative.", _
                                        pbNumberBag, GetType(AnimatGUI.TypeHelpers.GainTypeEditor), GetType(AnimatGuiCtrls.Controls.ExpandablePropBagConverter)))

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNewNode As New DataObjects.Behavior.Neurons.IonChannel(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim bnOrig As DataObjects.Behavior.Neurons.IonChannel = DirectCast(doOriginal, DataObjects.Behavior.Neurons.IonChannel)

            m_strSymbol = bnOrig.m_strSymbol
            m_bEnabled = bnOrig.m_bEnabled
            m_snGmax = DirectCast(bnOrig.m_snGmax.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_iMPower = bnOrig.m_iMPower
            m_iHPower = bnOrig.m_iHPower
            m_snEquilibriumPotential = DirectCast(bnOrig.m_snEquilibriumPotential.Clone(Me, bCutData, doRoot), ScaledNumber)

            m_snMinit = DirectCast(bnOrig.m_snMinit.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snNm = DirectCast(bnOrig.m_snNm.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_gnMinf = DirectCast(bnOrig.m_gnMinf.Clone(Me, bCutData, doRoot), AnimatGUI.DataObjects.Gain)
            m_gnTm = DirectCast(bnOrig.m_gnTm.Clone(Me, bCutData, doRoot), AnimatGUI.DataObjects.Gain)

            m_snHinit = DirectCast(bnOrig.m_snHinit.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snNh = DirectCast(bnOrig.m_snNh.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_gnHinf = DirectCast(bnOrig.m_gnHinf.Clone(Me, bCutData, doRoot), AnimatGUI.DataObjects.Gain)
            m_gnTh = DirectCast(bnOrig.m_gnTh.Clone(Me, bCutData, doRoot), AnimatGUI.DataObjects.Gain)

            If Not m_WorkspaceImage Is Nothing Then m_WorkspaceImage = DirectCast(bnOrig.m_WorkspaceImage.Clone, System.Drawing.Image)
            If Not m_DragImage Is Nothing Then m_DragImage = DirectCast(bnOrig.m_DragImage.Clone, System.Drawing.Image)

        End Sub

        Public Overrides Sub AddToReplaceIDList(ByVal aryReplaceIDList As ArrayList)
            MyBase.AddToReplaceIDList(aryReplaceIDList)

            m_gnMinf.AddToReplaceIDList(aryReplaceIDList)
            m_gnTm.AddToReplaceIDList(aryReplaceIDList)
            m_gnHinf.AddToReplaceIDList(aryReplaceIDList)
            m_gnTh.AddToReplaceIDList(aryReplaceIDList)
        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            If Not m_snGmax Is Nothing Then m_snGmax.ClearIsDirty()
            If Not m_snEquilibriumPotential Is Nothing Then m_snEquilibriumPotential.ClearIsDirty()
            If Not m_snMinit Is Nothing Then m_snMinit.ClearIsDirty()
            If Not m_snNm Is Nothing Then m_snNm.ClearIsDirty()
            If Not m_gnMinf Is Nothing Then m_gnMinf.ClearIsDirty()
            If Not m_gnTm Is Nothing Then m_gnTm.ClearIsDirty()
            If Not m_snHinit Is Nothing Then m_snHinit.ClearIsDirty()
            If Not m_snNh Is Nothing Then m_snNh.ClearIsDirty()
            If Not m_gnHinf Is Nothing Then m_gnHinf.ClearIsDirty()
            If Not m_gnTh Is Nothing Then m_gnTh.ClearIsDirty()

        End Sub

#Region " Add-Remove to List Methods "

        Public Overrides Sub AddToSim(ByVal bThrowError As Boolean)
            If Not Me.Parent Is Nothing Then
                Util.Application.SimulationInterface.AddItem(Me.Parent.ID, "IonChannel", Me.ID, Me.GetSimulationXml("IonChannel"), bThrowError)
                InitializeSimulationReferences()
            End If
        End Sub

        Public Overrides Sub RemoveFromSim(ByVal bThrowError As Boolean)
            If Not Me.Parent Is Nothing AndAlso Not m_doInterface Is Nothing Then
                Util.Application.SimulationInterface.RemoveItem(Me.Parent.ID, "IonChannel", Me.ID, bThrowError)
            End If
            m_doInterface = Nothing
        End Sub

#End Region

        Public Overrides Sub SaveSimulationXml(ByRef oXml As AnimatGUI.Interfaces.StdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            oXml.AddChildElement("IonChannel")
            oXml.IntoElem()

            oXml.AddChildElement("Name", Me.Name)
            oXml.AddChildElement("ID", Me.ID)
            oXml.AddChildElement("Enabled", m_bEnabled)

            oXml.AddChildElement("Gmax", m_snGmax.ActualValue)
            oXml.AddChildElement("MPower", m_iMPower)
            oXml.AddChildElement("HPower", m_iHPower)
            oXml.AddChildElement("EqPot", m_snEquilibriumPotential.ActualValue)

            oXml.AddChildElement("Minit", m_snMinit.ActualValue)
            oXml.AddChildElement("Nm", m_snNm.ActualValue)
            m_gnMinf.SaveSimulationXml(oXml, Nothing, "Minf")
            m_gnTm.SaveSimulationXml(oXml, Nothing, "Tm")

            oXml.AddChildElement("Hinit", m_snHinit.ActualValue)
            oXml.AddChildElement("Nh", m_snNh.ActualValue)
            m_gnHinf.SaveSimulationXml(oXml, Nothing, "Hinf")
            m_gnTh.SaveSimulationXml(oXml, Nothing, "Th")

            oXml.OutOfElem() 'Outof IonChannel

        End Sub

        'Public Overrides Sub SaveDataColumnToXml(ByRef oXml As AnimatGUI.Interfaces.StdXml)

        '    If Not Me.Parent Is Nothing AndAlso TypeOf Me.Parent Is DataObjects.Behavior.Neurons.Spiking Then
        '        Dim doNeuron As DataObjects.Behavior.Neurons.Spiking = DirectCast(Me.Parent, DataObjects.Behavior.Neurons.Spiking)

        '        If Not doNeuron.Organism Is Nothing Then
        '            oXml.IntoElem()

        '            oXml.AddChildElement("OrganismID", doNeuron.Organism.ID)
        '            oXml.AddChildElement("NeuronIndex", doNeuron.NodeIndex)
        '            oXml.AddChildElement("ChannelID", Me.ID)
        '            oXml.AddChildElement("Enabled", m_bEnabled)

        '            oXml.OutOfElem() 'Outof ChannelData
        '        End If
        '    End If

        'End Sub

        Public Overrides Sub LoadData(ByRef oXml As AnimatGUI.Interfaces.StdXml)

            oXml.IntoElem()

            m_strID = oXml.GetChildString("ID", m_strID)
            m_strName = oXml.GetChildString("Name", m_strName)
            m_strSymbol = oXml.GetChildString("Symbol", m_strSymbol)
            m_bEnabled = oXml.GetChildBool("Enabled", m_bEnabled)

            m_snGmax.LoadData(oXml, "Gmax")
            m_iMPower = oXml.GetChildInt("MPower", m_iMPower)
            m_iHPower = oXml.GetChildInt("HPower", m_iHPower)
            m_snEquilibriumPotential.LoadData(oXml, "EqPot")

            m_snMinit.LoadData(oXml, "Minit")
            m_snNm.LoadData(oXml, "Nm")
            m_gnMinf = Util.LoadGain("Minf", "Minf", Me, oXml)
            m_gnTm = Util.LoadGain("Tm", "Tm", Me, oXml)

            m_snHinit.LoadData(oXml, "Hinit")
            m_snNh.LoadData(oXml, "Nh")
            m_gnHinf = Util.LoadGain("Hinf", "Hinf", Me, oXml)
            m_gnTh = Util.LoadGain("Th", "Th", Me, oXml)

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveData(ByRef oXml As AnimatGUI.Interfaces.StdXml)

            oXml.AddChildElement("IonChannel")
            oXml.IntoElem()  'Into IonChannel Element

            oXml.AddChildElement("Name", Me.Name)
            oXml.AddChildElement("ID", Me.ID)
            oXml.AddChildElement("Symbol", m_strSymbol)
            oXml.AddChildElement("Enabled", m_bEnabled)

            m_snGmax.SaveData(oXml, "Gmax")
            oXml.AddChildElement("MPower", m_iMPower)
            oXml.AddChildElement("HPower", m_iHPower)
            m_snEquilibriumPotential.SaveData(oXml, "EqPot")

            m_snMinit.SaveData(oXml, "Minit")
            m_snNm.SaveData(oXml, "Nm")
            m_gnMinf.SaveData(oXml, "Minf")
            m_gnTm.SaveData(oXml, "Tm")

            m_snHinit.SaveData(oXml, "Hinit")
            m_snNh.SaveData(oXml, "Nh")
            m_gnHinf.SaveData(oXml, "Hinf")
            m_gnTh.SaveData(oXml, "Th")

            oXml.OutOfElem() ' Outof IonChannel Element

        End Sub

#End Region

#End Region

    End Class

End Namespace

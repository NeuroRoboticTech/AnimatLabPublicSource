Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports AnimatGuiCtrls.Controls

Namespace DataObjects.Behavior.Neurons

    Public Class FiringRateAdapter
        Inherits AnimatGUI.DataObjects.Behavior.Nodes.NodeToNodeAdapter

#Region " Attributes "

        'Protected m_bUseConductances As Boolean = True
        'Protected m_snEquilibriumPotential As AnimatGUI.Framework.ScaledNumber
        'Protected m_snSynapticConductance As AnimatGUI.Framework.ScaledNumber

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Firing Rate-IF Adapter"
            End Get
        End Property

        Public Overrides ReadOnly Property AdapterType() As String
            Get
                Return "NodeToNode"
            End Get
        End Property

        '<Browsable(False)> _
        'Public Overridable Property EquilibriumPotential() As AnimatGUI.Framework.ScaledNumber
        '    Get
        '        Return m_snEquilibriumPotential
        '    End Get
        '    Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
        '        If Value.ActualValue < -0.1 OrElse Value.ActualValue > 0.3 Then
        '            Throw New System.Exception("The equilibrium potential must be between the range -100 to 300 mV.")
        '        End If

        '        m_snEquilibriumPotential.CopyData(Value)
        '    End Set
        'End Property

        '<Browsable(False)> _
        'Public Overridable Property SynapticConductance() As AnimatGUI.Framework.ScaledNumber
        '    Get
        '        Return m_snSynapticConductance
        '    End Get
        '    Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
        '        If Value.ActualValue < 0 OrElse Value.ActualValue > 0.0001 Then
        '            Throw New System.Exception("The synaptic conductance must be between the range 0 to 100 uS/size.")
        '        End If

        '        m_snSynapticConductance.CopyData(Value)
        '    End Set
        'End Property

        '<Browsable(False)> _
        'Public Overridable Property UseConductances() As Boolean
        '    Get
        '        Return m_bUseConductances
        '    End Get
        '    Set(ByVal Value As Boolean)
        '        m_bUseConductances = Value
        '    End Set
        'End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            Me.Name = "Firing Rate-IF Adapter"
            Me.Description = "Provides an interface adapter between a CIF and IF neuron."
            'm_snEquilibriumPotential = New AnimatGUI.Framework.ScaledNumber(Me, "EquilibriumPotential", -10, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "Volts", "V")
            'm_snSynapticConductance = New AnimatGUI.Framework.ScaledNumber(Me, "SynapticConductance", 0.5, AnimatGUI.Framework.ScaledNumber.enumNumericScale.micro, "Siemens", "S")
        End Sub

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim bnOrig As FiringRateAdapter = DirectCast(doOriginal, FiringRateAdapter)

            ''Basic Synaptic Properties
            'm_snEquilibriumPotential = DirectCast(bnOrig.m_snEquilibriumPotential.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            'm_snSynapticConductance = DirectCast(bnOrig.m_snSynapticConductance.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNewNode As New Behavior.Neurons.FiringRateAdapter(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        'Public Overrides Sub SaveNetwork(ByRef oXml As AnimatGUI.Interfaces.StdXml, ByRef nmModule As AnimatGUI.DataObjects.Behavior.NeuralModule)

        '    If Not m_bEnabled Then
        '        Return
        '    End If

        '    If m_bnOrigin Is Nothing Then
        '        Throw New System.Exception("The origin node for adapter '" & Me.Name & "' is not defined!")
        '    End If

        '    If m_bnDestination Is Nothing Then
        '        Throw New System.Exception("The destination node for adapter '" & Me.Name & "' is not defined!")
        '    End If

        '    'If the origin or destination is an offpage then take care of that.
        '    Dim bnOrigin As AnimatGUI.DataObjects.Behavior.Node
        '    Dim bnDestination As AnimatGUI.DataObjects.Behavior.Node

        '    If TypeOf m_bnOrigin Is AnimatGUI.DataObjects.Behavior.Nodes.OffPage Then
        '        Dim bnOffpage As AnimatGUI.DataObjects.Behavior.Nodes.OffPage = DirectCast(m_bnOrigin, AnimatGUI.DataObjects.Behavior.Nodes.OffPage)
        '        bnOrigin = bnOffpage.LinkedNode.Node
        '    Else
        '        bnOrigin = m_bnOrigin
        '    End If

        '    If TypeOf m_bnDestination Is AnimatGUI.DataObjects.Behavior.Nodes.OffPage Then
        '        Dim bnOffpage As AnimatGUI.DataObjects.Behavior.Nodes.OffPage = DirectCast(m_bnDestination, AnimatGUI.DataObjects.Behavior.Nodes.OffPage)
        '        bnDestination = bnOffpage.LinkedNode.Node
        '    Else
        '        bnDestination = m_bnDestination
        '    End If

        '    'Do not attempt to save this adapter if there is no source data type specified.
        '    If m_thDataTypes.ID.Trim.Length = 0 Then
        '        Return
        '    End If

        '    Dim nmSource As NeuralModule = DirectCast(m_doOrganism.NeuralModules(bnOrigin.NeuralModuleType.FullName), NeuralModule)
        '    Dim nmTarget As NeuralModule = DirectCast(m_doOrganism.NeuralModules(bnDestination.NeuralModuleType.FullName), NeuralModule)

        '    If Not m_thDataTypes Is Nothing AndAlso m_thDataTypes.ID.Trim.Length > 0 AndAlso _
        '       Not nmTarget Is Nothing AndAlso Not bnDestination Is Nothing AndAlso _
        '       Not m_gnGain Is Nothing Then

        '        oXml.AddChildElement("Adapter")
        '        oXml.IntoElem()

        '        oXml.AddChildElement("Type", Me.AdapterType)
        '        oXml.AddChildElement("SourceModule", nmSource.ModuleName)
        '        oXml.AddChildElement("SourceNodeID", bnOrigin.NodeIndex)
        '        oXml.AddChildElement("SourceDataType", m_thDataTypes.ID)
        '        oXml.AddChildElement("TargetModule", nmTarget.ModuleName)
        '        oXml.AddChildElement("TargetNodeID", bnDestination.NodeIndex)
        '        oXml.AddChildElement("Equil", m_snEquilibriumPotential.ActualValue)
        '        oXml.AddChildElement("SynAmp", m_snSynapticConductance.ActualValue)

        '        m_gnGain.SaveNetwork(oXml, "Gain")

        '        oXml.OutOfElem() 'Outof Neuron

        '    End If


        'End Sub

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            'propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Use Conductances", m_bUseConductances.GetType(), "UseConductances", _
            '                            "Adapter Properties", "If true then inputs to post-synaptic current is conductance based, " & _
            '                            "otherwise it is pure current injection.", m_bUseConductances))

            'If m_bUseConductances Then
            '    Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = m_snEquilibriumPotential.Properties
            '    propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Equilibrium Potential", pbNumberBag.GetType(), "EquilibriumPotential", _
            '                                "Adapter Properties", "Sets the equilibrium (reversal) potential for this synaptic type. " & _
            '                                "Acceptable values are in the range -100 to 300 mV", pbNumberBag, _
            '                                "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            '    pbNumberBag = m_snSynapticConductance.Properties
            '    propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Synaptic Conductance", pbNumberBag.GetType(), "SynapticConductance", _
            '                                "Adapter Properties", "Sets the amplitude of the post-synaptic conductance change which this synapse mediates. " & _
            '                                "Acceptable values are in the range 0 to 100 uS/size.", _
            '                                pbNumberBag, "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))
            'Else
            '    propTable.Properties.Remove("Gain")
            '    propTable.Properties.Remove("Source Data Type ID")
            'End If

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            'If Not m_snEquilibriumPotential Is Nothing Then m_snEquilibriumPotential.ClearIsDirty()
            'If Not m_snSynapticConductance Is Nothing Then m_snSynapticConductance.ClearIsDirty()
        End Sub

        Public Overrides Sub LoadData(ByRef oXml As AnimatGUI.Interfaces.StdXml)
            MyBase.LoadData(oXml)

            'oXml.IntoElem()

            'm_snEquilibriumPotential.LoadData(oXml, "EquilibriumPotential")
            'm_snSynapticConductance.LoadData(oXml, "SynapticConductance")

            'oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveData(ByRef oXml As AnimatGUI.Interfaces.StdXml)
            MyBase.SaveData(oXml)

            'oXml.IntoElem() 'Into Node Element

            'm_snEquilibriumPotential.SaveData(oXml, "EquilibriumPotential")
            'm_snSynapticConductance.SaveData(oXml, "SynapticConductance")

            'oXml.OutOfElem() ' Outof Node Element

        End Sub

#End Region

#End Region

    End Class

End Namespace

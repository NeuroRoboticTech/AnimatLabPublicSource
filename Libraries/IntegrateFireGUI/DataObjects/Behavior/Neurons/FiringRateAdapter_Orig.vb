Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports Crownwood.Magic.Controls

Namespace DataObjects.Behavior.Neurons

    Public Class FiringRateAdapter
        Inherits AnimatTools.DataObjects.Behavior.Nodes.Adapter

#Region " Attributes "

        Protected m_bUseConductances As Boolean = True
        Protected m_snEquilibriumPotential As AnimatTools.Framework.ScaledNumber
        Protected m_snSynapticConductance As AnimatTools.Framework.ScaledNumber

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

        <Browsable(False)> _
        Public Overridable Property EquilibriumPotential() As AnimatTools.Framework.ScaledNumber
            Get
                Return m_snEquilibriumPotential
            End Get
            Set(ByVal Value As AnimatTools.Framework.ScaledNumber)
                If Value.ActualValue < -0.1 OrElse Value.ActualValue > 0.3 Then
                    Throw New System.Exception("The equilibrium potential must be between the range -100 to 300 mV.")
                End If

                m_snEquilibriumPotential.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property SynapticConductance() As AnimatTools.Framework.ScaledNumber
            Get
                Return m_snSynapticConductance
            End Get
            Set(ByVal Value As AnimatTools.Framework.ScaledNumber)
                If Value.ActualValue < 0 OrElse Value.ActualValue > 0.0001 Then
                    Throw New System.Exception("The synaptic conductance must be between the range 0 to 100 uS/size.")
                End If

                m_snSynapticConductance.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property UseConductances() As Boolean
            Get
                Return m_bUseConductances
            End Get
            Set(ByVal Value As Boolean)
                m_bUseConductances = Value
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatTools.Framework.DataObject)
            MyBase.New(doParent)

            Me.Name = "Firing Rate-IF Adapter"
            Me.Description = "Provides an interface adapter between a CIF and IF neuron."
            m_snEquilibriumPotential = New AnimatTools.Framework.ScaledNumber(Me, "EquilibriumPotential", -10, AnimatTools.Framework.ScaledNumber.enumNumericScale.milli, "Volts", "V")
            m_snSynapticConductance = New AnimatTools.Framework.ScaledNumber(Me, "SynapticConductance", 0.5, AnimatTools.Framework.ScaledNumber.enumNumericScale.micro, "Siemens", "S")
        End Sub

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatTools.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatTools.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim bnOrig As FiringRateAdapter = DirectCast(doOriginal, FiringRateAdapter)

            'Basic Synaptic Properties
            m_snEquilibriumPotential = DirectCast(bnOrig.m_snEquilibriumPotential.Clone(Me, bCutData, doRoot), AnimatTools.Framework.ScaledNumber)
            m_snSynapticConductance = DirectCast(bnOrig.m_snSynapticConductance.Clone(Me, bCutData, doRoot), AnimatTools.Framework.ScaledNumber)
        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatTools.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatTools.Framework.DataObject) As AnimatTools.Framework.DataObject
            Dim oNewNode As New Behavior.Neurons.FiringRateAdapter(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Public Overrides Sub SaveNetwork(ByRef oXml As AnimatTools.Interfaces.StdXml, ByRef nmModule As AnimatTools.DataObjects.Behavior.NeuralModule)

            If Not m_bEnabled Then
                Return
            End If

            If m_bnOrigin Is Nothing Then
                Throw New System.Exception("The origin node for adapter '" & Me.Name & "' is not defined!")
            End If

            If m_bnDestination Is Nothing Then
                Throw New System.Exception("The destination node for adapter '" & Me.Name & "' is not defined!")
            End If

            'If the origin or destination is an offpage then take care of that.
            Dim bnOrigin As AnimatTools.DataObjects.Behavior.Node
            Dim bnDestination As AnimatTools.DataObjects.Behavior.Node

            If TypeOf m_bnOrigin Is AnimatTools.DataObjects.Behavior.Nodes.OffPage Then
                Dim bnOffpage As AnimatTools.DataObjects.Behavior.Nodes.OffPage = DirectCast(m_bnOrigin, AnimatTools.DataObjects.Behavior.Nodes.OffPage)
                bnOrigin = bnOffpage.LinkedNode.Node
            Else
                bnOrigin = m_bnOrigin
            End If

            If TypeOf m_bnDestination Is AnimatTools.DataObjects.Behavior.Nodes.OffPage Then
                Dim bnOffpage As AnimatTools.DataObjects.Behavior.Nodes.OffPage = DirectCast(m_bnDestination, AnimatTools.DataObjects.Behavior.Nodes.OffPage)
                bnDestination = bnOffpage.LinkedNode.Node
            Else
                bnDestination = m_bnDestination
            End If

            'Do not attempt to save this adapter if there is no source data type specified.
            If m_thDataTypes.ID.Trim.Length = 0 Then
                Return
            End If

            Dim nmSource As NeuralModule = DirectCast(m_doOrganism.NeuralModules(bnOrigin.NeuralModuleType.FullName), NeuralModule)
            Dim nmTarget As NeuralModule = DirectCast(m_doOrganism.NeuralModules(bnDestination.NeuralModuleType.FullName), NeuralModule)

            If Not m_thDataTypes Is Nothing AndAlso m_thDataTypes.ID.Trim.Length > 0 AndAlso _
               Not nmTarget Is Nothing AndAlso Not bnDestination Is Nothing AndAlso _
               Not m_gnGain Is Nothing Then

                oXml.AddChildElement("Adapter")
                oXml.IntoElem()

                oXml.AddChildElement("Type", Me.AdapterType)
                oXml.AddChildElement("SourceModule", nmSource.ModuleName)
                oXml.AddChildElement("SourceNodeID", bnOrigin.NodeIndex)
                oXml.AddChildElement("SourceDataType", m_thDataTypes.ID)
                oXml.AddChildElement("TargetModule", nmTarget.ModuleName)
                oXml.AddChildElement("TargetNodeID", bnDestination.NodeIndex)
                oXml.AddChildElement("Equil", m_snEquilibriumPotential.ActualValue)
                oXml.AddChildElement("SynAmp", m_snSynapticConductance.ActualValue)

                m_gnGain.SaveNetwork(oXml, "Gain")

                oXml.OutOfElem() 'Outof Neuron

            End If


        End Sub

#Region " DataObject Methods "

        Protected Overrides Sub BuildProperties()
            MyBase.BuildProperties()

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Use Conductances", m_bUseConductances.GetType(), "UseConductances", _
                                        "Adapter Properties", "If true then inputs to post-synaptic current is conductance based, " & _
                                        "otherwise it is pure current injection.", m_bUseConductances))

            If m_bUseConductances Then
                Dim pbNumberBag As Crownwood.Magic.Controls.PropertyBag = m_snEquilibriumPotential.Properties
                m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Equilibrium Potential", pbNumberBag.GetType(), "EquilibriumPotential", _
                                            "Adapter Properties", "Sets the equilibrium (reversal) potential for this synaptic type. " & _
                                            "Acceptable values are in the range -100 to 300 mV", pbNumberBag, _
                                            "", GetType(AnimatTools.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

                pbNumberBag = m_snSynapticConductance.Properties
                m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Synaptic Conductance", pbNumberBag.GetType(), "SynapticConductance", _
                                            "Adapter Properties", "Sets the amplitude of the post-synaptic conductance change which this synapse mediates. " & _
                                            "Acceptable values are in the range 0 to 100 uS/size.", _
                                            pbNumberBag, "", GetType(AnimatTools.Framework.ScaledNumber.ScaledNumericPropBagConverter)))
            Else
                m_Properties.Properties.Remove("Gain")
                m_Properties.Properties.Remove("Source Data Type ID")
            End If

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            If Not m_snEquilibriumPotential Is Nothing Then m_snEquilibriumPotential.ClearIsDirty()
            If Not m_snSynapticConductance Is Nothing Then m_snSynapticConductance.ClearIsDirty()
        End Sub

        Public Overrides Sub LoadData(ByRef oXml As AnimatTools.Interfaces.StdXml)
            MyBase.LoadData(oXml)

            oXml.IntoElem()

            m_snEquilibriumPotential.LoadData(oXml, "EquilibriumPotential")
            m_snSynapticConductance.LoadData(oXml, "SynapticConductance")

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveData(ByRef oXml As AnimatTools.Interfaces.StdXml)
            MyBase.SaveData(oXml)

            oXml.IntoElem() 'Into Node Element

            m_snEquilibriumPotential.SaveData(oXml, "EquilibriumPotential")
            m_snSynapticConductance.SaveData(oXml, "SynapticConductance")

            oXml.OutOfElem() ' Outof Node Element

        End Sub

#End Region

#End Region

    End Class

End Namespace

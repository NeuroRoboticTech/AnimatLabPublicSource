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
Imports AnimatCarlGUI.DataObjects.Behavior.NodeTypes
Imports AnimatCarlGUI.DataObjects.Behavior.SynapseTypes

Namespace DataObjects.Behavior

    Public Class NeuralModule
        Inherits AnimatGUI.DataObjects.Behavior.NeuralModule

#Region " Attributes "

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property NetworkFilename() As String
            Get
                If Not m_doOrganism Is Nothing Then
                    Return m_doOrganism.Name & ".afnn"
                Else
                    Return ""
                End If
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            m_strModuleName = "AnimatCarlSimCUDA"
            m_strModuleType = "CarlSimNeuralModule"

            m_snTimeStep = New AnimatGUI.Framework.ScaledNumber(Me, "TimeStep", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "seconds", "s")

            Util.Application.AddAdapterPair(GetType(AnimatCarlGUI.DataObjects.Behavior.NodeTypes.NeuronGroup).ToString(), "IntegrateFireGUI.DataObjects.Behavior.Neurons.Spiking", GetType(AnimatGUI.DataObjects.Behavior.Nodes.NodeToNodeAdapter).ToString())

            Util.Application.AddLinkPair(GetType(AnimatCarlGUI.DataObjects.Behavior.NodeTypes.NeuronGroup).ToString, "IntegrateFireGUI.DataObjects.Behavior.Neurons.Spiking", GetType(AnimatCarlGUI.DataObjects.Behavior.SynapseTypes.SpikingCurrentSynapse).ToString())
        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNewModule As New DataObjects.Behavior.NeuralModule(doParent)
            oNewModule.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewModule.AfterClone(Me, bCutData, doRoot, oNewModule)
            Return oNewModule
        End Function

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

            oXml.IntoElem()  'neuralmodule xml

            Dim bnNeuronGroup As NeuronGroup
            oXml.AddChildElement("NeuronGroups")
            oXml.IntoElem()
            For Each deEntry As DictionaryEntry In m_aryNodes
                If Util.IsTypeOf(deEntry.Value.GetType, GetType(NeuronGroup), False) Then
                    bnNeuronGroup = DirectCast(deEntry.Value, NeuronGroup)
                    bnNeuronGroup.SaveSimulationXml(oXml, Me)
                End If
            Next
            oXml.OutOfElem()

            Dim blSynpase As SynapseGroup
            oXml.AddChildElement("Synapses")
            oXml.IntoElem()
            For Each deEntry As DictionaryEntry In m_aryLinks
                blSynpase = DirectCast(deEntry.Value, SynapseGroup)
                blSynpase.SaveSimulationXml(oXml, Me)
            Next
            oXml.OutOfElem()

            oXml.OutOfElem()  'neuralmodule xml

        End Sub

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            If propTable.Properties.Contains("Time Step") Then propTable.Properties.Remove("Time Step")

            'Make sure time step is readonly.
            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = m_snTimeStep.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Time Step", pbNumberBag.GetType(), "TimeStep", _
                                        "Module Properties", "Sets integration time step determines the speed and accuracy of the calculation. " & _
                                        "The smaller the value, the more accurate but the slower the calculation.  " & _
                                        "If the value is too large, the calculation may become unstable. " & _
                                        "Acceptable values are in the range 0.0001 to 50 ms.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter), True))

        End Sub

#End Region

#End Region

    End Class

End Namespace

Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports Crownwood.Magic.Controls
Imports AnimatTools.Framework

Namespace DataObjects.Behavior

    Public Class NeuralModule
        Inherits AnimatTools.DataObjects.Behavior.NeuralModule

#Region " Attributes "

#End Region

#Region " Properties "

        Public Overrides Property Organism() As AnimatTools.DataObjects.Physical.Organism
            Get
                Return m_doOrganism
            End Get
            Set(ByVal Value As AnimatTools.DataObjects.Physical.Organism)
                m_doOrganism = Value
            End Set
        End Property

        Public Overrides ReadOnly Property NetworkFilename() As String
            Get
                If Not m_doOrganism Is Nothing Then
                    Return m_doOrganism.Name & "[*NEURAL_FILE_TYPE*]"
                Else
                    Return ""
                End If
            End Get
        End Property

        Public Overrides ReadOnly Property ModuleFilename() As String
            Get
                If Util.Simulation.UseReleaseLibraries Then
                    Return "[*PROJECT_NAME*]_vc7.dll"
                Else
                    Return "[*PROJECT_NAME*]_vc7D.dll"
                End If

            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatTools.Framework.DataObject)
            MyBase.New(doParent)

            m_strModuleName = "[*PROJECT_NAME*]"
            m_strModuleType = "[*PROJECT_NAME*]NeuralModule"
        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatTools.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatTools.Framework.DataObject) As AnimatTools.Framework.DataObject
            Dim oNewModule As New DataObjects.Behavior.NeuralModule(doParent)
            oNewModule.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewModule.AfterClone(Me, bCutData, doRoot, oNewModule)
            Return oNewModule
        End Function

        Protected Overloads Overrides Sub SaveNetworkFile(ByRef oXml As AnimatTools.Interfaces.StdXml)

            oXml.AddChildElement("TimeStep", Me.TimeStep.ActualValue)
            Util.SaveVector(oXml, "NetworkSize", New AnimatTools.Framework.Vec3i(Me, m_aryNodes.Count, 1, 1))

            'First we need to go through and set the neuron indexes for all of the neurons in this module.
            Dim bnNode As DataObjects.Behavior.Neurons.[*NEURON_NAME*]
            Dim iNeuronIndex As Integer = 0
            For Each deEntry As DictionaryEntry In m_aryNodes
                If Util.IsTypeOf(deEntry.Value.GetType(), GetType(DataObjects.Behavior.Neurons.[*NEURON_NAME*]), False) Then
                    bnNode = DirectCast(deEntry.Value, DataObjects.Behavior.Neurons.[*NEURON_NAME*])
                    bnNode.NodeIndex = iNeuronIndex
                    iNeuronIndex = iNeuronIndex + 1
                Else
                    Throw New System.Exception("There was a node in the fast neural module that was not derived from a [*NEURON_NAME*]?")
                End If
            Next

            'Now we can save the neurons
            oXml.AddChildElement("Neurons")
            oXml.IntoElem()
            For Each deEntry As DictionaryEntry In m_aryNodes
                bnNode = DirectCast(deEntry.Value, DataObjects.Behavior.Neurons.[*NEURON_NAME*])
                bnNode.SaveNetwork(oXml, Me)
            Next
            oXml.OutOfElem()

        End Sub

#Region " DataObject Methods "

        Protected Overrides Sub BuildProperties()
            MyBase.BuildProperties()

        End Sub

        Public Overrides Sub LoadData(ByRef oXml As AnimatTools.Interfaces.StdXml)
            MyBase.LoadData(oXml)

            oXml.IntoElem() 'Into RigidBody Element

            oXml.OutOfElem() 'Outof RigidBody Element	
        End Sub

        Public Overrides Sub SaveData(ByRef oXml As AnimatTools.Interfaces.StdXml)
            MyBase.SaveData(oXml)

            oXml.IntoElem()

            oXml.OutOfElem()
        End Sub

#End Region

#End Region

    End Class

End Namespace
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

Namespace DataObjects.Behavior.Nodes

    'This is primarily used to differentiate neuron types from other node types. At the moment I have no base code in here 
    'for this neuron type.
    Public MustInherit Class Neuron
        Inherits DataObjects.Behavior.Node

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
        End Sub

#Region " Add-Remove to List Methods "

        Public Overrides Sub BeforeAddToList(Optional ByVal bThrowError As Boolean = True)
            MyBase.BeforeAddToList(bThrowError)

            If Not NeuralModule Is Nothing Then
                NeuralModule.VerifyExistsInSim()
                If Not Util.Application.SimulationInterface.FindItem(Me.ID, False) Then
                    'If we just created this neuralmodule in the sim then this object might already exist now. We should only add it if it does not exist.
                    Util.Application.SimulationInterface.AddItem(NeuralModule.ID(), "Neuron", Me.GetSimulationXml("Neuron"), bThrowError)
                End If
            End If
            InitializeSimulationReferences()
        End Sub

        Public Overrides Sub BeforeRemoveFromList(Optional ByVal bThrowError As Boolean = True)
            MyBase.BeforeRemoveFromList(bThrowError)

            If Not NeuralModule Is Nothing AndAlso Not m_doInterface Is Nothing Then
                Util.Application.SimulationInterface.RemoveItem(NeuralModule.ID(), "Neuron", Me.ID, bThrowError)
            End If
            m_doInterface = Nothing
        End Sub

        Public Overrides Sub AfterAddToList(Optional ByVal bThrowError As Boolean = True)
            MyBase.AfterAddToList(bThrowError)

            If Not NeuralModule Is Nothing Then
                NeuralModule.Nodes.Add(Me.ID, Me)
            End If
        End Sub

        Public Overrides Sub AfterRemoveFromList(Optional ByVal bThrowError As Boolean = True)
            MyBase.AfterRemoveFromList(bThrowError)

            If Not NeuralModule Is Nothing AndAlso NeuralModule.Nodes.Contains(Me.ID) Then
                NeuralModule.Nodes.Remove(Me.ID)
            End If
        End Sub

#End Region

    End Class

End Namespace


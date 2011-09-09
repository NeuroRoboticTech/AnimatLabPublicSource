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

Namespace DataObjects.Behavior.Links

    Public MustInherit Class Synapse
        Inherits AnimatGUI.DataObjects.Behavior.Link

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)
        End Sub


#Region " Add-Remove to List Methods "

        Public Overrides Sub BeforeAddToList(Optional ByVal bThrowError As Boolean = True)
            MyBase.BeforeAddToList(bThrowError)

            'Synpases are stored in the destination neuron object.
            If Not Me.Destination Is Nothing Then
                Util.Application.SimulationInterface.AddItem(Me.Destination.ID, "Synapse", Me.GetSimulationXml("Synapse"), bThrowError)
                InitializeSimulationReferences()
            End If
        End Sub

        Public Overrides Sub BeforeRemoveFromList(Optional ByVal bThrowError As Boolean = True)
            MyBase.BeforeRemoveFromList(bThrowError)

            'Synpases are stored in the destination neuron object.
            If Not Me.Destination Is Nothing AndAlso Not m_doInterface Is Nothing Then
                Util.Application.SimulationInterface.RemoveItem(Me.Destination.ID, "Synapse", Me.ID, bThrowError)
                m_doInterface = Nothing
            End If
        End Sub

        Public Overrides Sub AfterAddToList(Optional ByVal bThrowError As Boolean = True)
            MyBase.AfterAddToList(bThrowError)

            If Not NeuralModule Is Nothing Then
                NeuralModule.Links.Add(Me.ID, Me)
            End If
        End Sub

        Public Overrides Sub AfterRemoveFromList(Optional ByVal bThrowError As Boolean = True)
            MyBase.AfterRemoveFromList(bThrowError)

            If Not NeuralModule Is Nothing AndAlso NeuralModule.Links.Contains(Me.ID) Then
                NeuralModule.Links.Remove(Me.ID)
            End If
        End Sub

#End Region

    End Class

End Namespace

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

Namespace DataObjects.Behavior

    Public MustInherit Class FastSynapse
        Inherits AnimatGUI.DataObjects.Behavior.Links.Synapse

        Public MustOverride ReadOnly Property SynapseType() As String

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)
        End Sub

    End Class

End Namespace


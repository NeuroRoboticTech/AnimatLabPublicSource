Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports AnimatGuiCtrls.Controls

Namespace DataObjects.Behavior.Nodes

    'This is primarily used to differentiate neuron types from other node types. At the moment I have no base code in here 
    'for this neuron type.
    Public MustInherit Class Neuron
        Inherits DataObjects.Behavior.Node

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
        End Sub

    End Class

End Namespace


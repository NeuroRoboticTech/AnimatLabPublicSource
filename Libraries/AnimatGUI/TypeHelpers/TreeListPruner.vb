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

Namespace TypeHelpers

    Public MustInherit Class TreeListPruner

        Public MustOverride Function PruneTree(ByVal tnNodes As Crownwood.DotNetMagic.Controls.NodeCollection) As Integer
        Public MustOverride Sub PruneList(ByVal lbList As ListBox)
    End Class

End Namespace


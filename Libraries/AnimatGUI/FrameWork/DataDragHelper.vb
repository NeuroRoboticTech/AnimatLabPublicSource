Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports AnimatGuiCtrls.Controls

Namespace Framework

    'The sole purpose of this class is that the stupid GetDataPresent method used when doing drag-drop is too stupid
    'to realize that I am trying to get a base class of a polymorphic derived class that is being dropped. So I have to
    'create this wrapper to hold the data that way I can always get a copy of this one using strong typing
    Public Class DataDragHelper
        Public m_doData As Framework.DataObject
    End Class

End Namespace


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

Namespace DataObjects

    Public MustInherit Class ProgramModule
        Inherits Framework.DataObject

#Region " Attributes "

        Public Overridable ReadOnly Property Description() As String
            Get
                Return ""
            End Get
        End Property

#End Region

#Region " Properties "

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Public MustOverride Sub ShowDialog()

#Region " DataObject Methods "

#End Region

#End Region

    End Class

End Namespace

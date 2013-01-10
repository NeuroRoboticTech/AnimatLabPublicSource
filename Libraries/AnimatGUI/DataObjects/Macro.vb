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

    Public MustInherit Class Macro
        Inherits Framework.DataObject

#Region " Attributes "

        Public Overridable ReadOnly Property Description() As String
            Get
                Return ""
            End Get
        End Property

#End Region

#Region " Properties "

        Public Overridable ReadOnly Property AllowUserSelection() As Boolean
            Get
                Return True
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Public MustOverride Sub Execute()

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            'This should never really be used by a macro
        End Sub

        Public Overrides Function Clone(doParent As Framework.DataObject, bCutData As Boolean, doRoot As Framework.DataObject) As Framework.DataObject
            'This should never really be used by a macro
            Return Nothing
        End Function

#End Region

#End Region

    End Class

End Namespace

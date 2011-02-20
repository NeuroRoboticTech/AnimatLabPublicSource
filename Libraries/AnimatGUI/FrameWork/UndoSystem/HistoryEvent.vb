Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports AnimatGuiCtrls.Controls

Namespace Framework.UndoSystem

    Public MustInherit Class HistoryEvent

#Region " Attributes "

        Protected m_mdiParent As AnimatGUI.Forms.MdiChild
        Protected m_frmParent As AnimatGUI.Forms.AnimatForm

#End Region

#Region " Properties "

        Public Overridable Property MdiParent() As AnimatGUI.Forms.MdiChild
            Get
                Return m_mdiParent
            End Get
            Set(ByVal Value As AnimatGUI.Forms.MdiChild)
                m_mdiParent = Value
            End Set
        End Property

        Public Overridable Property AnimatParent() As AnimatGUI.Forms.AnimatForm
            Get
                Return m_frmParent
            End Get
            Set(ByVal Value As AnimatGUI.Forms.AnimatForm)
                m_frmParent = Value
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal frmParent As System.Windows.Forms.Form)

            If TypeOf frmParent Is AnimatGUI.Forms.MdiChild Then
                m_mdiParent = DirectCast(frmParent, AnimatGUI.Forms.MdiChild)
            ElseIf TypeOf frmParent Is AnimatGUI.Forms.AnimatForm Then
                m_frmParent = DirectCast(frmParent, AnimatGUI.Forms.AnimatForm)
            End If

        End Sub

        Public MustOverride Sub Undo()
        Public MustOverride Sub Redo()

#End Region

    End Class

End Namespace


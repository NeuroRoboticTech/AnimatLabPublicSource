Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Text
Imports System.Runtime.Remoting
Imports AnimatGuiCtrls.Controls
Imports AnimatGUI.Framework

Namespace Forms

    Public Class SelectMacro
        Inherits AnimatGUI.Forms.AnimatDialog

#Region " Windows Form Designer generated code "

        Public Sub New()
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call

        End Sub

        'Form overrides dispose to clean up the component list.
        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
                If Not (components Is Nothing) Then
                    components.Dispose()
                End If
            End If
            MyBase.Dispose(disposing)
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        Friend WithEvents lvMacros As System.Windows.Forms.ListView
        Friend WithEvents txtDescription As System.Windows.Forms.TextBox
        Friend WithEvents btnOk As System.Windows.Forms.Button
        Friend WithEvents btnCancel As System.Windows.Forms.Button
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.lvMacros = New System.Windows.Forms.ListView
            Me.txtDescription = New System.Windows.Forms.TextBox
            Me.btnOk = New System.Windows.Forms.Button
            Me.btnCancel = New System.Windows.Forms.Button
            Me.SuspendLayout()
            '
            'lvMacros
            '
            Me.lvMacros.HideSelection = False
            Me.lvMacros.Location = New System.Drawing.Point(8, 8)
            Me.lvMacros.MultiSelect = False
            Me.lvMacros.Name = "lvMacros"
            Me.lvMacros.Size = New System.Drawing.Size(184, 312)
            Me.lvMacros.Sorting = System.Windows.Forms.SortOrder.Ascending
            Me.lvMacros.TabIndex = 0
            Me.lvMacros.View = System.Windows.Forms.View.List
            '
            'txtDescription
            '
            Me.txtDescription.Location = New System.Drawing.Point(200, 16)
            Me.txtDescription.Multiline = True
            Me.txtDescription.Name = "txtDescription"
            Me.txtDescription.ReadOnly = True
            Me.txtDescription.Size = New System.Drawing.Size(192, 280)
            Me.txtDescription.TabIndex = 1
            Me.txtDescription.Text = ""
            '
            'btnOk
            '
            Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnOk.Location = New System.Drawing.Point(240, 304)
            Me.btnOk.Name = "btnOk"
            Me.btnOk.Size = New System.Drawing.Size(72, 24)
            Me.btnOk.TabIndex = 2
            Me.btnOk.Text = "Ok"
            '
            'btnCancel
            '
            Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(320, 304)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(72, 24)
            Me.btnCancel.TabIndex = 3
            Me.btnCancel.Text = "Cancel"
            '
            'SelectMacro
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(400, 334)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnOk)
            Me.Controls.Add(Me.txtDescription)
            Me.Controls.Add(Me.lvMacros)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.Name = "SelectMacro"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Select Macro"
            Me.ResumeLayout(False)

        End Sub

#End Region

        Protected m_doSelModule As DataObjects.Macro

        Public Property SelectedModule() As DataObjects.Macro
            Get
                Return m_doSelModule
            End Get
            Set(ByVal Value As DataObjects.Macro)
                m_doSelModule = Value
            End Set
        End Property

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            MyBase.OnLoad(e)

            Try
                m_btnOk = Me.btnOk
                m_btnCancel = Me.btnCancel

                For Each doModule As DataObjects.Macro In Util.Application.Macros
                    If doModule.AllowUserSelection Then
                        Dim liItem As New ListViewItem(doModule.Name)
                        liItem.Tag = doModule
                        lvMacros.Items.Add(liItem)
                    End If
                Next

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
            Try
                If lvMacros.SelectedItems.Count <> 1 Then
                    Throw New System.Exception("You must select a program module to run.")
                Else
                    Dim liItem As ListViewItem = lvMacros.SelectedItems.Item(0)

                    If Not liItem.Tag Is Nothing AndAlso TypeOf liItem.Tag Is DataObjects.Macro Then
                        m_doSelModule = DirectCast(liItem.Tag, DataObjects.Macro)
                    End If

                    Me.DialogResult = DialogResult.OK
                    Me.Close()
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub lvMacros_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvMacros.SelectedIndexChanged
            Try
                If lvMacros.SelectedItems.Count = 1 Then
                    Dim liItem As ListViewItem = lvMacros.SelectedItems.Item(0)

                    If Not liItem.Tag Is Nothing AndAlso TypeOf liItem.Tag Is DataObjects.Macro Then
                        Dim doModule As DataObjects.Macro = DirectCast(liItem.Tag, DataObjects.Macro)
                        txtDescription.Text = doModule.Description
                    End If
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

    End Class

End Namespace

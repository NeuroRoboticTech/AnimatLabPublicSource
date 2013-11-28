Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports AnimatGuiCtrls.Controls
Imports AnimatGUI
Imports AnimatGUI.Framework
Imports AnimatGUI.DataObjects

Namespace Forms

    Public Class NewProject
        Inherits Forms.AnimatDialog

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
        Friend WithEvents lblProjectName As System.Windows.Forms.Label
        Friend WithEvents txtProjectName As System.Windows.Forms.TextBox
        Friend WithEvents btnCancel As System.Windows.Forms.Button
        Friend WithEvents btnOk As System.Windows.Forms.Button
        Friend WithEvents lblLocation As System.Windows.Forms.Label
        Friend WithEvents txtLocation As System.Windows.Forms.TextBox
        Friend WithEvents cboPhysicsEngine As System.Windows.Forms.ComboBox
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents btnBrowseLocation As System.Windows.Forms.Button
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.lblProjectName = New System.Windows.Forms.Label()
            Me.lblLocation = New System.Windows.Forms.Label()
            Me.txtProjectName = New System.Windows.Forms.TextBox()
            Me.txtLocation = New System.Windows.Forms.TextBox()
            Me.btnBrowseLocation = New System.Windows.Forms.Button()
            Me.btnCancel = New System.Windows.Forms.Button()
            Me.btnOk = New System.Windows.Forms.Button()
            Me.cboPhysicsEngine = New System.Windows.Forms.ComboBox()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.SuspendLayout()
            '
            'lblProjectName
            '
            Me.lblProjectName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblProjectName.Location = New System.Drawing.Point(8, 16)
            Me.lblProjectName.Name = "lblProjectName"
            Me.lblProjectName.Size = New System.Drawing.Size(280, 16)
            Me.lblProjectName.TabIndex = 0
            Me.lblProjectName.Text = "Project Name"
            Me.lblProjectName.TextAlign = System.Drawing.ContentAlignment.TopCenter
            '
            'lblLocation
            '
            Me.lblLocation.Location = New System.Drawing.Point(8, 56)
            Me.lblLocation.Name = "lblLocation"
            Me.lblLocation.Size = New System.Drawing.Size(280, 16)
            Me.lblLocation.TabIndex = 1
            Me.lblLocation.Text = "Location"
            Me.lblLocation.TextAlign = System.Drawing.ContentAlignment.TopCenter
            '
            'txtProjectName
            '
            Me.txtProjectName.Location = New System.Drawing.Point(8, 32)
            Me.txtProjectName.Name = "txtProjectName"
            Me.txtProjectName.Size = New System.Drawing.Size(288, 20)
            Me.txtProjectName.TabIndex = 2
            '
            'txtLocation
            '
            Me.txtLocation.Location = New System.Drawing.Point(8, 72)
            Me.txtLocation.Name = "txtLocation"
            Me.txtLocation.Size = New System.Drawing.Size(264, 20)
            Me.txtLocation.TabIndex = 3
            '
            'btnBrowseLocation
            '
            Me.btnBrowseLocation.Location = New System.Drawing.Point(272, 72)
            Me.btnBrowseLocation.Name = "btnBrowseLocation"
            Me.btnBrowseLocation.Size = New System.Drawing.Size(24, 20)
            Me.btnBrowseLocation.TabIndex = 4
            Me.btnBrowseLocation.Text = "..."
            '
            'btnCancel
            '
            Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(160, 154)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(64, 24)
            Me.btnCancel.TabIndex = 13
            Me.btnCancel.Text = "Cancel"
            '
            'btnOk
            '
            Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnOk.Location = New System.Drawing.Point(88, 154)
            Me.btnOk.Name = "btnOk"
            Me.btnOk.Size = New System.Drawing.Size(64, 24)
            Me.btnOk.TabIndex = 12
            Me.btnOk.Text = "Ok"
            '
            'cboPhysicsEngine
            '
            Me.cboPhysicsEngine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboPhysicsEngine.FormattingEnabled = True
            Me.cboPhysicsEngine.Items.AddRange(New Object() {"Bullet", "Vortex"})
            Me.cboPhysicsEngine.Location = New System.Drawing.Point(8, 116)
            Me.cboPhysicsEngine.Name = "cboPhysicsEngine"
            Me.cboPhysicsEngine.Size = New System.Drawing.Size(288, 21)
            Me.cboPhysicsEngine.TabIndex = 14
            '
            'Label1
            '
            Me.Label1.Location = New System.Drawing.Point(8, 99)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(280, 16)
            Me.Label1.TabIndex = 15
            Me.Label1.Text = "Physics Engine"
            Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopCenter
            '
            'NewProject
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(312, 186)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.cboPhysicsEngine)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnOk)
            Me.Controls.Add(Me.btnBrowseLocation)
            Me.Controls.Add(Me.txtLocation)
            Me.Controls.Add(Me.txtProjectName)
            Me.Controls.Add(Me.lblLocation)
            Me.Controls.Add(Me.lblProjectName)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.Name = "NewProject"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "New Project"
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region

#Region " Attributes "

        Protected m_bAllowUserToChoosePhysicsSystem As Boolean = True

#End Region

#Region " Properties "

        Public Property AllowUserToChoosePhysicsSystem() As Boolean
            Get
                Return m_bAllowUserToChoosePhysicsSystem
            End Get
            Set(ByVal value As Boolean)
                m_bAllowUserToChoosePhysicsSystem = value
            End Set
        End Property

#End Region

#Region " Methods "

#End Region

#Region " Events "

        Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click

            Try
                If txtProjectName.Text.Trim.Length = 0 Then
                    Throw New System.Exception("You must specify a project name.")
                End If

                If txtLocation.Text.Trim.Length = 0 Then
                    Throw New System.Exception("You must specify a location for the new project.")
                End If

                'Now lets make sure there is not already a directory with that name at the specified location.
                Dim strProjectDir As String = txtLocation.Text & "\" & txtProjectName.Text
                If System.IO.Directory.Exists(strProjectDir) Then
                    Throw New System.Exception("The directory '" & strProjectDir & "' already exists. Please choose a different name or location for the project.")
                End If

                Me.DialogResult = DialogResult.OK
                Me.Close()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            Me.DialogResult = DialogResult.Cancel
            Me.Close()
        End Sub

        Private Sub btnBrowseDirectory_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseLocation.Click
            Try
                Dim openFolderDialog As New System.Windows.Forms.FolderBrowserDialog
                openFolderDialog.Description = "Specify the drive location where the new project directory will be created."
                openFolderDialog.ShowNewFolderButton = True
                openFolderDialog.SelectedPath = txtLocation.Text

                If openFolderDialog.ShowDialog() = DialogResult.OK Then
                    txtLocation.Text = openFolderDialog.SelectedPath
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub NewProject_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
            Try
                If e.KeyCode = Keys.Enter Then
                    btnOk_Click(sender, e)
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub txtProjectName_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtProjectName.KeyDown
            Try
                If e.KeyCode = Keys.Enter Then
                    btnOk_Click(sender, e)
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub txtLocation_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtLocation.KeyDown
            Try
                If e.KeyCode = Keys.Enter Then
                    btnOk_Click(sender, e)
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            MyBase.OnLoad(e)

            Try
                m_btnOk = Me.btnOk
                m_btnCancel = Me.btnCancel

                txtLocation.Text = Util.Application.DefaultNewFolder

                If Not m_bAllowUserToChoosePhysicsSystem Then
                    cboPhysicsEngine.Enabled = False
                    cboPhysicsEngine.SelectedText = Util.Application.SimPhysicsSystem
                Else
                    cboPhysicsEngine.SelectedIndex = 1  '0
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Public Overridable Sub SetProjectParams(ByVal strName As String, ByVal strPath As String)
            Me.txtProjectName.Text = strName
            Me.txtLocation.Text = strPath
        End Sub

#End Region

    End Class

End Namespace

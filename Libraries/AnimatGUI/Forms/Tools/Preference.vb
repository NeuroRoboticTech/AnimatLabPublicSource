Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports AnimatGuiCtrls.Controls
Imports AnimatGUI.Framework

Namespace Forms.Tools

    Public Class Preference
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
        Friend WithEvents bt_OKButton As System.Windows.Forms.Button
        Friend WithEvents bt_CancelButton As System.Windows.Forms.Button
        Friend WithEvents tcTabs As Crownwood.DotNetMagic.Controls.TabControl
        Friend WithEvents tbUpdates As Crownwood.DotNetMagic.Controls.TabPage
        Friend WithEvents btnCheckForUpdates As System.Windows.Forms.Button
        Friend WithEvents cbUpdateInterval As System.Windows.Forms.ComboBox
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents lblDefNewFolder As System.Windows.Forms.Label
        Friend WithEvents txtDefNewFolder As System.Windows.Forms.TextBox
        Friend WithEvents btnDefNewFolder As System.Windows.Forms.Button
        Friend WithEvents tbSettings As Crownwood.DotNetMagic.Controls.TabPage
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.bt_OKButton = New System.Windows.Forms.Button
            Me.bt_CancelButton = New System.Windows.Forms.Button
            Me.tcTabs = New Crownwood.DotNetMagic.Controls.TabControl
            Me.tbSettings = New Crownwood.DotNetMagic.Controls.TabPage
            Me.lblDefNewFolder = New System.Windows.Forms.Label
            Me.txtDefNewFolder = New System.Windows.Forms.TextBox
            Me.tbUpdates = New Crownwood.DotNetMagic.Controls.TabPage
            Me.btnCheckForUpdates = New System.Windows.Forms.Button
            Me.cbUpdateInterval = New System.Windows.Forms.ComboBox
            Me.Label1 = New System.Windows.Forms.Label
            Me.btnDefNewFolder = New System.Windows.Forms.Button
            Me.tcTabs.SuspendLayout()
            Me.tbSettings.SuspendLayout()
            Me.tbUpdates.SuspendLayout()
            Me.SuspendLayout()
            '
            'bt_OKButton
            '
            Me.bt_OKButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.bt_OKButton.Location = New System.Drawing.Point(8, 204)
            Me.bt_OKButton.Name = "bt_OKButton"
            Me.bt_OKButton.Size = New System.Drawing.Size(48, 24)
            Me.bt_OKButton.TabIndex = 1
            Me.bt_OKButton.Text = "OK"
            '
            'bt_CancelButton
            '
            Me.bt_CancelButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.bt_CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.bt_CancelButton.Location = New System.Drawing.Point(72, 204)
            Me.bt_CancelButton.Name = "bt_CancelButton"
            Me.bt_CancelButton.Size = New System.Drawing.Size(48, 24)
            Me.bt_CancelButton.TabIndex = 3
            Me.bt_CancelButton.Text = "Cancel"
            '
            'tcTabs
            '
            Me.tcTabs.AllowDrop = False
            Me.tcTabs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.tcTabs.Location = New System.Drawing.Point(-1, -1)
            Me.tcTabs.MediaPlayerDockSides = False
            Me.tcTabs.Name = "tcTabs"
            Me.tcTabs.OfficeDockSides = False
            Me.tcTabs.PositionTop = True
            Me.tcTabs.SelectedIndex = 0
            Me.tcTabs.ShowDropSelect = False
            Me.tcTabs.Size = New System.Drawing.Size(471, 193)
            Me.tcTabs.TabIndex = 4
            Me.tcTabs.TabPages.AddRange(New Crownwood.DotNetMagic.Controls.TabPage() {Me.tbSettings, Me.tbUpdates})
            Me.tcTabs.TextTips = True
            '
            'tbSettings
            '
            Me.tbSettings.Controls.Add(Me.btnDefNewFolder)
            Me.tbSettings.Controls.Add(Me.lblDefNewFolder)
            Me.tbSettings.Controls.Add(Me.txtDefNewFolder)
            Me.tbSettings.InactiveBackColor = System.Drawing.Color.Empty
            Me.tbSettings.InactiveTextBackColor = System.Drawing.Color.Empty
            Me.tbSettings.InactiveTextColor = System.Drawing.Color.Empty
            Me.tbSettings.Location = New System.Drawing.Point(1, 24)
            Me.tbSettings.Name = "tbSettings"
            Me.tbSettings.SelectBackColor = System.Drawing.Color.Empty
            Me.tbSettings.SelectTextBackColor = System.Drawing.Color.Empty
            Me.tbSettings.SelectTextColor = System.Drawing.Color.Empty
            Me.tbSettings.Size = New System.Drawing.Size(469, 168)
            Me.tbSettings.TabIndex = 5
            Me.tbSettings.Title = "Settings"
            Me.tbSettings.ToolTip = "Default Settings"
            '
            'lblDefNewFolder
            '
            Me.lblDefNewFolder.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblDefNewFolder.Location = New System.Drawing.Point(4, 4)
            Me.lblDefNewFolder.Name = "lblDefNewFolder"
            Me.lblDefNewFolder.Size = New System.Drawing.Size(429, 18)
            Me.lblDefNewFolder.TabIndex = 1
            Me.lblDefNewFolder.Text = "Default New Folder Location"
            Me.lblDefNewFolder.TextAlign = System.Drawing.ContentAlignment.TopCenter
            '
            'txtDefNewFolder
            '
            Me.txtDefNewFolder.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtDefNewFolder.Location = New System.Drawing.Point(4, 25)
            Me.txtDefNewFolder.Name = "txtDefNewFolder"
            Me.txtDefNewFolder.Size = New System.Drawing.Size(429, 23)
            Me.txtDefNewFolder.TabIndex = 0
            '
            'tbUpdates
            '
            Me.tbUpdates.Controls.Add(Me.btnCheckForUpdates)
            Me.tbUpdates.Controls.Add(Me.cbUpdateInterval)
            Me.tbUpdates.Controls.Add(Me.Label1)
            Me.tbUpdates.InactiveBackColor = System.Drawing.Color.Empty
            Me.tbUpdates.InactiveTextBackColor = System.Drawing.Color.Empty
            Me.tbUpdates.InactiveTextColor = System.Drawing.Color.Empty
            Me.tbUpdates.Location = New System.Drawing.Point(1, 24)
            Me.tbUpdates.Name = "tbUpdates"
            Me.tbUpdates.SelectBackColor = System.Drawing.Color.Empty
            Me.tbUpdates.Selected = False
            Me.tbUpdates.SelectTextBackColor = System.Drawing.Color.Empty
            Me.tbUpdates.SelectTextColor = System.Drawing.Color.Empty
            Me.tbUpdates.Size = New System.Drawing.Size(469, 168)
            Me.tbUpdates.TabIndex = 4
            Me.tbUpdates.Title = "Updates"
            Me.tbUpdates.ToolTip = "Params for auto-updating the program."
            '
            'btnCheckForUpdates
            '
            Me.btnCheckForUpdates.Location = New System.Drawing.Point(8, 66)
            Me.btnCheckForUpdates.Name = "btnCheckForUpdates"
            Me.btnCheckForUpdates.Size = New System.Drawing.Size(149, 24)
            Me.btnCheckForUpdates.TabIndex = 5
            Me.btnCheckForUpdates.Text = "Check for Updates now"
            '
            'cbUpdateInterval
            '
            Me.cbUpdateInterval.Items.AddRange(New Object() {"Never", "Daily", "Weekly", "Monthly"})
            Me.cbUpdateInterval.Location = New System.Drawing.Point(8, 34)
            Me.cbUpdateInterval.Name = "cbUpdateInterval"
            Me.cbUpdateInterval.Size = New System.Drawing.Size(208, 23)
            Me.cbUpdateInterval.TabIndex = 4
            '
            'Label1
            '
            Me.Label1.Location = New System.Drawing.Point(8, 10)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(240, 16)
            Me.Label1.TabIndex = 3
            Me.Label1.Text = "How often would you like to run the update?"
            '
            'btnDefNewFolder
            '
            Me.btnDefNewFolder.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnDefNewFolder.Location = New System.Drawing.Point(440, 25)
            Me.btnDefNewFolder.Name = "btnDefNewFolder"
            Me.btnDefNewFolder.Size = New System.Drawing.Size(26, 23)
            Me.btnDefNewFolder.TabIndex = 2
            Me.btnDefNewFolder.Text = "..."
            Me.btnDefNewFolder.UseVisualStyleBackColor = True
            '
            'Preference
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(470, 237)
            Me.Controls.Add(Me.tcTabs)
            Me.Controls.Add(Me.bt_CancelButton)
            Me.Controls.Add(Me.bt_OKButton)
            Me.Name = "Preference"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Preferences"
            Me.tcTabs.ResumeLayout(False)
            Me.tbSettings.ResumeLayout(False)
            Me.tbSettings.PerformLayout()
            Me.tbUpdates.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub

#End Region

        Private Sub bt_OKButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_OKButton.Click

            Try
                If txtDefNewFolder.Text.Trim.Length = 0 Then
                    Throw New System.Exception("You must specify a default new folder location.")
                End If

                Util.Application.AutoUpdateInterval = CType(cbUpdateInterval.SelectedIndex, AnimatGUI.Forms.AnimatApplication.enumAutoUpdateInterval)
                Util.Application.DefaultNewFolder = txtDefNewFolder.Text
                Util.SaveUserConfigData()
                Me.Close()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub tbAutoUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        End Sub

        Private Sub cbUpdateInterval_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
            'cbUpdateInterval.SelectedIndex()
        End Sub

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            MyBase.OnLoad(e)

            Try
                cbUpdateInterval.SelectedIndex = Util.Application.AutoUpdateInterval
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub btnCheckForUpdates_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Try
                Util.Application.CheckForUpdates(True)
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
                Util.DisplayError(ex)
            End Try

        End Sub

        'This should be done in a load event, but for some reason that event is not working for this class.
        'I have no idea why.
        Public Sub LoadPreferences()
            Try
                If Util.Application.DefaultNewFolder.Length = 0 Then
                    Me.txtDefNewFolder.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\AnimatLab"
                Else
                    Me.txtDefNewFolder.Text = Util.Application.DefaultNewFolder
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub btnDefNewFolder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDefNewFolder.Click
            Try
                Dim openFolderDialog As New System.Windows.Forms.FolderBrowserDialog
                openFolderDialog.Description = "Specify the drive location where the new project directory will be created."
                openFolderDialog.ShowNewFolderButton = True

                If openFolderDialog.ShowDialog() = DialogResult.OK Then
                    txtDefNewFolder.Text = openFolderDialog.SelectedPath
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

    End Class

End Namespace

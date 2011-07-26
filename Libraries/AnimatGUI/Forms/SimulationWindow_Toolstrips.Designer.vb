<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SimulationWindow_Toolstrips
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SimulationWindow_Toolstrips))
        Me.SimWindowToolStrip = New System.Windows.Forms.ToolStrip()
        Me.TrackCameraToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.lbStucture = New System.Windows.Forms.ToolStripLabel()
        Me.cboStructure = New System.Windows.Forms.ToolStripComboBox()
        Me.lblBodyPart = New System.Windows.Forms.ToolStripLabel()
        Me.cboBodyPart = New System.Windows.Forms.ToolStripComboBox()
        Me.SimWindowMenuStrip = New System.Windows.Forms.MenuStrip()
        Me.SimWindowToolStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'SimWindowToolStrip
        '
        Me.SimWindowToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TrackCameraToolStripButton, Me.lbStucture, Me.cboStructure, Me.lblBodyPart, Me.cboBodyPart})
        Me.SimWindowToolStrip.Location = New System.Drawing.Point(0, 0)
        Me.SimWindowToolStrip.Name = "SimWindowToolStrip"
        Me.SimWindowToolStrip.Size = New System.Drawing.Size(774, 25)
        Me.SimWindowToolStrip.TabIndex = 0
        Me.SimWindowToolStrip.Text = "ToolStrip1"
        Me.SimWindowToolStrip.Visible = False
        '
        'TrackCameraToolStripButton
        '
        Me.TrackCameraToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TrackCameraToolStripButton.Image = CType(resources.GetObject("TrackCameraToolStripButton.Image"), System.Drawing.Image)
        Me.TrackCameraToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TrackCameraToolStripButton.Name = "TrackCameraToolStripButton"
        Me.TrackCameraToolStripButton.Size = New System.Drawing.Size(23, 22)
        Me.TrackCameraToolStripButton.Text = "TrackCameraToolStripButton"
        Me.TrackCameraToolStripButton.ToolTipText = "Determines if the camera tracking is on or not."
        '
        'lbStucture
        '
        Me.lbStucture.Name = "lbStucture"
        Me.lbStucture.Size = New System.Drawing.Size(58, 22)
        Me.lbStucture.Text = "Structure:"
        '
        'cboStructure
        '
        Me.cboStructure.Name = "cboStructure"
        Me.cboStructure.Size = New System.Drawing.Size(121, 25)
        Me.cboStructure.ToolTipText = "The structure to track with the camera"
        '
        'lblBodyPart
        '
        Me.lblBodyPart.Name = "lblBodyPart"
        Me.lblBodyPart.Size = New System.Drawing.Size(61, 22)
        Me.lblBodyPart.Text = "Body Part:"
        '
        'cboBodyPart
        '
        Me.cboBodyPart.Name = "cboBodyPart"
        Me.cboBodyPart.Size = New System.Drawing.Size(121, 25)
        Me.cboBodyPart.ToolTipText = "A body part of the selected strucutre to track with the camera"
        '
        'SimWindowMenuStrip
        '
        Me.SimWindowMenuStrip.Location = New System.Drawing.Point(0, 0)
        Me.SimWindowMenuStrip.Name = "SimWindowMenuStrip"
        Me.SimWindowMenuStrip.Size = New System.Drawing.Size(284, 24)
        Me.SimWindowMenuStrip.TabIndex = 1
        Me.SimWindowMenuStrip.Text = "MenuStrip1"
        Me.SimWindowMenuStrip.Visible = False
        '
        'SimulationWindow_Toolstrips
        '
        Me.ClientSize = New System.Drawing.Size(774, 262)
        Me.Controls.Add(Me.SimWindowToolStrip)
        Me.Controls.Add(Me.SimWindowMenuStrip)
        Me.MainMenuStrip = Me.SimWindowMenuStrip
        Me.Name = "SimulationWindow_Toolstrips"
        Me.Text = "SimulationWindow"
        Me.SimWindowToolStrip.ResumeLayout(False)
        Me.SimWindowToolStrip.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents SimWindowToolStrip As System.Windows.Forms.ToolStrip
    Friend WithEvents lbStucture As System.Windows.Forms.ToolStripLabel
    Friend WithEvents cboStructure As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents SimWindowMenuStrip As System.Windows.Forms.MenuStrip
    Friend WithEvents TrackCameraToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents lblBodyPart As System.Windows.Forms.ToolStripLabel
    Friend WithEvents cboBodyPart As System.Windows.Forms.ToolStripComboBox
End Class

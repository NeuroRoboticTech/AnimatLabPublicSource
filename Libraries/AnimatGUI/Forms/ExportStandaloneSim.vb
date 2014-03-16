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

    Public Class ExportStandaloneSim
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
        Friend WithEvents cboPhysicsEngine As System.Windows.Forms.ComboBox
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents lblMassUnits As System.Windows.Forms.Label
        Friend WithEvents cboLibraryVersion As System.Windows.Forms.ComboBox
        Friend WithEvents lblBinaryType As System.Windows.Forms.Label
        Friend WithEvents chkShowGraphics As System.Windows.Forms.CheckBox
        Friend WithEvents cboBinaryType As System.Windows.Forms.ComboBox
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.lblProjectName = New System.Windows.Forms.Label()
            Me.txtProjectName = New System.Windows.Forms.TextBox()
            Me.btnCancel = New System.Windows.Forms.Button()
            Me.btnOk = New System.Windows.Forms.Button()
            Me.cboPhysicsEngine = New System.Windows.Forms.ComboBox()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.lblMassUnits = New System.Windows.Forms.Label()
            Me.cboLibraryVersion = New System.Windows.Forms.ComboBox()
            Me.lblBinaryType = New System.Windows.Forms.Label()
            Me.cboBinaryType = New System.Windows.Forms.ComboBox()
            Me.chkShowGraphics = New System.Windows.Forms.CheckBox()
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
            'txtProjectName
            '
            Me.txtProjectName.Location = New System.Drawing.Point(8, 32)
            Me.txtProjectName.Name = "txtProjectName"
            Me.txtProjectName.Size = New System.Drawing.Size(288, 20)
            Me.txtProjectName.TabIndex = 2
            '
            'btnCancel
            '
            Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(160, 228)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(64, 24)
            Me.btnCancel.TabIndex = 13
            Me.btnCancel.Text = "Cancel"
            '
            'btnOk
            '
            Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnOk.Location = New System.Drawing.Point(88, 228)
            Me.btnOk.Name = "btnOk"
            Me.btnOk.Size = New System.Drawing.Size(64, 24)
            Me.btnOk.TabIndex = 12
            Me.btnOk.Text = "Ok"
            '
            'cboPhysicsEngine
            '
            Me.cboPhysicsEngine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboPhysicsEngine.FormattingEnabled = True
            Me.cboPhysicsEngine.Location = New System.Drawing.Point(8, 73)
            Me.cboPhysicsEngine.Name = "cboPhysicsEngine"
            Me.cboPhysicsEngine.Size = New System.Drawing.Size(288, 21)
            Me.cboPhysicsEngine.TabIndex = 14
            '
            'Label1
            '
            Me.Label1.Location = New System.Drawing.Point(8, 56)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(280, 16)
            Me.Label1.TabIndex = 15
            Me.Label1.Text = "Physics Engine"
            Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopCenter
            '
            'lblMassUnits
            '
            Me.lblMassUnits.Location = New System.Drawing.Point(8, 105)
            Me.lblMassUnits.Name = "lblMassUnits"
            Me.lblMassUnits.Size = New System.Drawing.Size(280, 16)
            Me.lblMassUnits.TabIndex = 17
            Me.lblMassUnits.Text = "Library Version"
            Me.lblMassUnits.TextAlign = System.Drawing.ContentAlignment.TopCenter
            '
            'cboLibraryVersion
            '
            Me.cboLibraryVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboLibraryVersion.FormattingEnabled = True
            Me.cboLibraryVersion.Location = New System.Drawing.Point(8, 122)
            Me.cboLibraryVersion.Name = "cboLibraryVersion"
            Me.cboLibraryVersion.Size = New System.Drawing.Size(288, 21)
            Me.cboLibraryVersion.TabIndex = 16
            '
            'lblBinaryType
            '
            Me.lblBinaryType.Location = New System.Drawing.Point(8, 154)
            Me.lblBinaryType.Name = "lblBinaryType"
            Me.lblBinaryType.Size = New System.Drawing.Size(280, 16)
            Me.lblBinaryType.TabIndex = 19
            Me.lblBinaryType.Text = "Binary Type"
            Me.lblBinaryType.TextAlign = System.Drawing.ContentAlignment.TopCenter
            '
            'cboBinaryType
            '
            Me.cboBinaryType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboBinaryType.FormattingEnabled = True
            Me.cboBinaryType.Location = New System.Drawing.Point(8, 171)
            Me.cboBinaryType.Name = "cboBinaryType"
            Me.cboBinaryType.Size = New System.Drawing.Size(288, 21)
            Me.cboBinaryType.TabIndex = 18
            '
            'chkShowGraphics
            '
            Me.chkShowGraphics.AutoSize = True
            Me.chkShowGraphics.Checked = True
            Me.chkShowGraphics.CheckState = System.Windows.Forms.CheckState.Checked
            Me.chkShowGraphics.Location = New System.Drawing.Point(89, 205)
            Me.chkShowGraphics.Name = "chkShowGraphics"
            Me.chkShowGraphics.Size = New System.Drawing.Size(135, 17)
            Me.chkShowGraphics.TabIndex = 20
            Me.chkShowGraphics.Text = "Show graphics window"
            Me.chkShowGraphics.UseVisualStyleBackColor = True
            '
            'ExportStandaloneSim
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(312, 260)
            Me.Controls.Add(Me.chkShowGraphics)
            Me.Controls.Add(Me.lblBinaryType)
            Me.Controls.Add(Me.cboBinaryType)
            Me.Controls.Add(Me.lblMassUnits)
            Me.Controls.Add(Me.cboLibraryVersion)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.cboPhysicsEngine)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnOk)
            Me.Controls.Add(Me.txtProjectName)
            Me.Controls.Add(Me.lblProjectName)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.Name = "ExportStandaloneSim"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Export Standalone Simulation"
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region

#Region " Attributes "

        Protected m_doPhysics As Physical.PhysicsEngine

#End Region

#Region " Properties "

        Public Property Physics() As Physical.PhysicsEngine
            Get
                Return m_doPhysics
            End Get
            Set(ByVal value As Physical.PhysicsEngine)
                m_doPhysics = value
            End Set
        End Property

#End Region

#Region " Methods "

#Region "Automation Methods"

        Public Sub SetPhysics(ByVal strPhysics As String)

            Dim iIdx As Integer = 0
            For Each doEngine As DataObjects.Physical.PhysicsEngine In cboPhysicsEngine.Items
                If doEngine.Name = strPhysics Then
                    cboPhysicsEngine.SelectedIndex = iIdx
                    Return
                End If

                iIdx = iIdx + 1
            Next

        End Sub

        Public Sub SetLibraryVersion(ByVal strVersion As String)

            Dim iIdx As Integer = 0
            For Each strUnit As String In cboLibraryVersion.Items
                If strUnit = strVersion Then
                    cboLibraryVersion.SelectedIndex = iIdx
                    Return
                End If

                iIdx = iIdx + 1
            Next

        End Sub

        Public Sub SetBinaryTye(ByVal strType As String)

            Dim iIdx As Integer = 0
            For Each strUnit As String In cboBinaryType.Items
                If strUnit = strType Then
                    cboBinaryType.SelectedIndex = iIdx
                    Return
                End If

                iIdx = iIdx + 1
            Next

        End Sub

        Protected Sub SetupLibraryVersions(ByVal doPhysics As Physical.PhysicsEngine)

            cboLibraryVersion.Items.Clear()

            Dim iSelIdx As Integer = -1
            Dim iIdx As Integer = 0
            For Each deEntry As DictionaryEntry In doPhysics.AvailableLibraryVersions
                Dim dtData As DataType = DirectCast(deEntry.Value, DataType)
                cboLibraryVersion.Items.Add(dtData)

                If dtData.ID = m_doPhysics.LibraryVersion.ID Then
                    iSelIdx = iIdx
                End If

                iIdx = iIdx + 1
            Next

            If iSelIdx >= 0 Then
                cboLibraryVersion.SelectedIndex = iSelIdx
            End If

        End Sub

        Protected Sub SetupBinaryTypes(ByVal doPhysics As Physical.PhysicsEngine)

            cboBinaryType.Items.Clear()

            Dim iSelIdx As Integer = -1
            Dim iIdx As Integer = 0
            For Each eVal As Physical.PhysicsEngine.enumBinaryMode In doPhysics.AvailableBinaryModeTypes
                cboBinaryType.Items.Add(eVal)

                If eVal = m_doPhysics.BinaryMode Then
                    iSelIdx = iIdx
                End If

                iIdx = iIdx + 1
            Next

            If iSelIdx >= 0 Then
                cboBinaryType.SelectedIndex = iSelIdx
            End If

        End Sub

#End Region

#End Region

#Region " Events "

        Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click

            Try
                If txtProjectName.Text.Trim.Length = 0 Then
                    Throw New System.Exception("You must specify a file name.")
                End If

                If Not txtProjectName.Text.EndsWith(".asim") Then
                    Throw New System.Exception("Simulation files must end with a .asim extension")
                End If

                Dim doPhysics As Physical.PhysicsEngine = DirectCast(cboPhysicsEngine.SelectedItem, Physical.PhysicsEngine)
                m_doPhysics = DirectCast(doPhysics.Clone(Nothing, False, Nothing), Physical.PhysicsEngine)
                m_doPhysics.BinaryMode = DirectCast([Enum].Parse(GetType(Physical.PhysicsEngine.enumBinaryMode), cboBinaryType.SelectedItem.ToString(), True), Physical.PhysicsEngine.enumBinaryMode)
                Dim dtData As DataType = DirectCast(cboLibraryVersion.SelectedItem, DataType)
                m_doPhysics.SetLibraryVersion(dtData.ID, True)

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

        Private Sub txtLocation_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs)
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

                Dim doPhysics As Physical.PhysicsEngine
                cboPhysicsEngine.Items.Clear()
                Dim iIdx As Integer = 0
                Dim iSelIdx As Integer = 0
                For Each doEngine As DataObjects.Physical.PhysicsEngine In Util.Application.PhysicsEngines
                    cboPhysicsEngine.Items.Add(doEngine)
                    If doEngine.Name = m_doPhysics.Name Then
                        iSelIdx = iIdx
                        doPhysics = doEngine
                    End If
                    iIdx = iIdx + 1
                Next

                cboPhysicsEngine.SelectedIndex = iSelIdx

                txtProjectName.Text = Util.Application.ProjectName & "_Standalone.asim"

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub cboPhysicsEngine_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cboPhysicsEngine.SelectedIndexChanged

            Try
                Dim doPhysics As Physical.PhysicsEngine = DirectCast(cboPhysicsEngine.SelectedItem, Physical.PhysicsEngine)
                SetupLibraryVersions(doPhysics)
                SetupBinaryTypes(doPhysics)
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Public Overridable Sub SetProjectParams(ByVal strName As String)
            Me.txtProjectName.Text = strName
        End Sub

#End Region

    End Class

End Namespace

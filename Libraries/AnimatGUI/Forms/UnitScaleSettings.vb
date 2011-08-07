Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports Crownwood.Magic.Common
Imports AnimatGuiCtrls.Controls
Imports Crownwood.Magic.Docking
Imports Crownwood.Magic.Menus
Imports AnimatGUI.Framework
Imports AnimatGUI.DataObjects.Physical

Namespace Forms

    Public Class UnitScaleSettings
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
        Friend WithEvents cboDistanceUnits As System.Windows.Forms.ComboBox
        Friend WithEvents cboMassUnits As System.Windows.Forms.ComboBox
        Friend WithEvents lblDistanceUnits As System.Windows.Forms.Label
        Friend WithEvents lblMassUnits As System.Windows.Forms.Label
        Friend WithEvents btnCancel As System.Windows.Forms.Button
        Friend WithEvents btnOk As System.Windows.Forms.Button
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.cboDistanceUnits = New System.Windows.Forms.ComboBox
            Me.cboMassUnits = New System.Windows.Forms.ComboBox
            Me.lblDistanceUnits = New System.Windows.Forms.Label
            Me.lblMassUnits = New System.Windows.Forms.Label
            Me.btnCancel = New System.Windows.Forms.Button
            Me.btnOk = New System.Windows.Forms.Button
            Me.SuspendLayout()
            '
            'cboDistanceUnits
            '
            Me.cboDistanceUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboDistanceUnits.Items.AddRange(New Object() {"Kilometers", "Centameters", "Decameters", "Meters", "Decimeters", "Centimeters", "Millimeters"})
            Me.cboDistanceUnits.Location = New System.Drawing.Point(8, 24)
            Me.cboDistanceUnits.Name = "cboDistanceUnits"
            Me.cboDistanceUnits.Size = New System.Drawing.Size(136, 21)
            Me.cboDistanceUnits.TabIndex = 0
            '
            'cboMassUnits
            '
            Me.cboMassUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboMassUnits.Items.AddRange(New Object() {"Kilograms", "Centagrams", "Decagrams", "Grams", "Decigrams", "Centigrams", "Milligrams"})
            Me.cboMassUnits.Location = New System.Drawing.Point(8, 72)
            Me.cboMassUnits.Name = "cboMassUnits"
            Me.cboMassUnits.Size = New System.Drawing.Size(136, 21)
            Me.cboMassUnits.TabIndex = 1
            '
            'lblDistanceUnits
            '
            Me.lblDistanceUnits.Location = New System.Drawing.Point(8, 8)
            Me.lblDistanceUnits.Name = "lblDistanceUnits"
            Me.lblDistanceUnits.Size = New System.Drawing.Size(136, 16)
            Me.lblDistanceUnits.TabIndex = 2
            Me.lblDistanceUnits.Text = "Distance Units"
            Me.lblDistanceUnits.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'lblMassUnits
            '
            Me.lblMassUnits.Location = New System.Drawing.Point(8, 56)
            Me.lblMassUnits.Name = "lblMassUnits"
            Me.lblMassUnits.Size = New System.Drawing.Size(136, 16)
            Me.lblMassUnits.TabIndex = 3
            Me.lblMassUnits.Text = "Mass Units"
            Me.lblMassUnits.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'btnCancel
            '
            Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(80, 104)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(64, 24)
            Me.btnCancel.TabIndex = 15
            Me.btnCancel.Text = "Cancel"
            '
            'btnOk
            '
            Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnOk.Location = New System.Drawing.Point(8, 104)
            Me.btnOk.Name = "btnOk"
            Me.btnOk.Size = New System.Drawing.Size(64, 24)
            Me.btnOk.TabIndex = 14
            Me.btnOk.Text = "Ok"
            '
            'UnitScaleSettings
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(152, 134)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnOk)
            Me.Controls.Add(Me.lblMassUnits)
            Me.Controls.Add(Me.lblDistanceUnits)
            Me.Controls.Add(Me.cboMassUnits)
            Me.Controls.Add(Me.cboDistanceUnits)
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "UnitScaleSettings"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Units"
            Me.ResumeLayout(False)

        End Sub

#End Region

#Region " Attributes "

        Protected m_eDistanceUnits As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits
        Protected m_eMassUnits As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits

#End Region

#Region " Properties "

        Public Property DistanceUnits() As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits
            Get
                Return m_eDistanceUnits
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits)
                m_eDistanceUnits = Value
            End Set
        End Property

        Public Property MassUnits() As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits
            Get
                Return m_eMassUnits
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits)
                m_eMassUnits = Value
            End Set
        End Property

#End Region

#Region " Methods "

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            MyBase.OnLoad(e)

            Try
                m_btnOk = Me.btnOk
                m_btnCancel = Me.btnCancel

                SelectItem(cboDistanceUnits, m_eDistanceUnits.ToString())
                SelectItem(cboMassUnits, m_eMassUnits.ToString())

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Sub SelectItem(ByVal cboBox As ComboBox, ByVal strSelected As String)

            Dim iIndex As Integer = 0
            For Each strItem As String In cboBox.Items
                If strItem = strSelected Then
                    cboBox.SelectedIndex = iIndex
                    Return
                End If
                iIndex = iIndex + 1
            Next
        End Sub

        Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
            Try

                m_eDistanceUnits = DirectCast([Enum].Parse(GetType(AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits), DirectCast(cboDistanceUnits.SelectedItem, String), True), AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits)
                m_eMassUnits = DirectCast([Enum].Parse(GetType(AnimatGUI.DataObjects.Physical.Environment.enumMassUnits), DirectCast(cboMassUnits.SelectedItem, String), True), AnimatGUI.DataObjects.Physical.Environment.enumMassUnits)
                Me.DialogResult = DialogResult.OK
                Me.Close()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace

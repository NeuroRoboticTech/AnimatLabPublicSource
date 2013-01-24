Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports AnimatGuiCtrls.Controls
Imports AnimatGUI.Framework

Namespace Forms.Behavior

    Public Class Relabel
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
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents txtMatch As System.Windows.Forms.TextBox
        Friend WithEvents txtReplace As System.Windows.Forms.TextBox
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents btnReplace As System.Windows.Forms.Button
        Friend WithEvents btnCancel As System.Windows.Forms.Button
        Friend WithEvents grpApplyTo As System.Windows.Forms.GroupBox
        Friend WithEvents rbThisDiagram As System.Windows.Forms.RadioButton
        Friend WithEvents rbAllDiagrams As System.Windows.Forms.RadioButton
        Friend WithEvents rbSelected As System.Windows.Forms.RadioButton
        Friend WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents rbThisSubsystem As System.Windows.Forms.RadioButton
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.txtMatch = New System.Windows.Forms.TextBox()
            Me.txtReplace = New System.Windows.Forms.TextBox()
            Me.Label2 = New System.Windows.Forms.Label()
            Me.btnReplace = New System.Windows.Forms.Button()
            Me.btnCancel = New System.Windows.Forms.Button()
            Me.grpApplyTo = New System.Windows.Forms.GroupBox()
            Me.rbSelected = New System.Windows.Forms.RadioButton()
            Me.rbThisSubsystem = New System.Windows.Forms.RadioButton()
            Me.rbAllDiagrams = New System.Windows.Forms.RadioButton()
            Me.rbThisDiagram = New System.Windows.Forms.RadioButton()
            Me.Label3 = New System.Windows.Forms.Label()
            Me.grpApplyTo.SuspendLayout()
            Me.SuspendLayout()
            '
            'Label1
            '
            Me.Label1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.Label1.Location = New System.Drawing.Point(10, 8)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(365, 16)
            Me.Label1.TabIndex = 0
            Me.Label1.Text = "Match Expression"
            Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'txtMatch
            '
            Me.txtMatch.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtMatch.Location = New System.Drawing.Point(10, 32)
            Me.txtMatch.Name = "txtMatch"
            Me.txtMatch.Size = New System.Drawing.Size(365, 20)
            Me.txtMatch.TabIndex = 1
            '
            'txtReplace
            '
            Me.txtReplace.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtReplace.Location = New System.Drawing.Point(10, 88)
            Me.txtReplace.Name = "txtReplace"
            Me.txtReplace.Size = New System.Drawing.Size(365, 20)
            Me.txtReplace.TabIndex = 3
            '
            'Label2
            '
            Me.Label2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.Label2.Location = New System.Drawing.Point(10, 64)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(365, 16)
            Me.Label2.TabIndex = 2
            Me.Label2.Text = "Replace Expression"
            Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'btnReplace
            '
            Me.btnReplace.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnReplace.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.btnReplace.Location = New System.Drawing.Point(175, 238)
            Me.btnReplace.Name = "btnReplace"
            Me.btnReplace.Size = New System.Drawing.Size(96, 24)
            Me.btnReplace.TabIndex = 4
            Me.btnReplace.Text = "Replace"
            '
            'btnCancel
            '
            Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(279, 238)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(96, 24)
            Me.btnCancel.TabIndex = 5
            Me.btnCancel.Text = "Cancel"
            '
            'grpApplyTo
            '
            Me.grpApplyTo.Controls.Add(Me.rbSelected)
            Me.grpApplyTo.Controls.Add(Me.rbThisSubsystem)
            Me.grpApplyTo.Controls.Add(Me.rbAllDiagrams)
            Me.grpApplyTo.Controls.Add(Me.rbThisDiagram)
            Me.grpApplyTo.Location = New System.Drawing.Point(10, 120)
            Me.grpApplyTo.Name = "grpApplyTo"
            Me.grpApplyTo.Size = New System.Drawing.Size(365, 48)
            Me.grpApplyTo.TabIndex = 6
            Me.grpApplyTo.TabStop = False
            Me.grpApplyTo.Text = "Apply To"
            '
            'rbSelected
            '
            Me.rbSelected.Location = New System.Drawing.Point(289, 24)
            Me.rbSelected.Name = "rbSelected"
            Me.rbSelected.Size = New System.Drawing.Size(70, 16)
            Me.rbSelected.TabIndex = 3
            Me.rbSelected.Text = "Selected"
            '
            'rbThisSubsystem
            '
            Me.rbThisSubsystem.Location = New System.Drawing.Point(102, 24)
            Me.rbThisSubsystem.Name = "rbThisSubsystem"
            Me.rbThisSubsystem.Size = New System.Drawing.Size(104, 16)
            Me.rbThisSubsystem.TabIndex = 2
            Me.rbThisSubsystem.Text = "This Subsystem"
            '
            'rbAllDiagrams
            '
            Me.rbAllDiagrams.Location = New System.Drawing.Point(207, 24)
            Me.rbAllDiagrams.Name = "rbAllDiagrams"
            Me.rbAllDiagrams.Size = New System.Drawing.Size(88, 16)
            Me.rbAllDiagrams.TabIndex = 1
            Me.rbAllDiagrams.Text = "All Diagrams"
            '
            'rbThisDiagram
            '
            Me.rbThisDiagram.Checked = True
            Me.rbThisDiagram.Location = New System.Drawing.Point(16, 24)
            Me.rbThisDiagram.Name = "rbThisDiagram"
            Me.rbThisDiagram.Size = New System.Drawing.Size(96, 16)
            Me.rbThisDiagram.TabIndex = 0
            Me.rbThisDiagram.TabStop = True
            Me.rbThisDiagram.Text = "This Diagram"
            '
            'Label3
            '
            Me.Label3.Location = New System.Drawing.Point(7, 182)
            Me.Label3.Name = "Label3"
            Me.Label3.Size = New System.Drawing.Size(368, 53)
            Me.Label3.TabIndex = 7
            Me.Label3.Text = "use a regulare expression to match text fields to change. " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "For example, Matcch E" & _
        "xpression = LU_*" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Replace Expression = RU_ "
            '
            'Relabel
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(385, 270)
            Me.Controls.Add(Me.Label3)
            Me.Controls.Add(Me.grpApplyTo)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnReplace)
            Me.Controls.Add(Me.txtReplace)
            Me.Controls.Add(Me.Label2)
            Me.Controls.Add(Me.txtMatch)
            Me.Controls.Add(Me.Label1)
            Me.Name = "Relabel"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Relabel"
            Me.grpApplyTo.ResumeLayout(False)
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region

#Region " Attributes "

        Protected m_aryItems As New ArrayList
        Protected m_doDiagram As AnimatGUI.Forms.Behavior.Diagram

#End Region

#Region " Properties "

        Public Overridable ReadOnly Property Items() As ArrayList
            Get
                Return m_aryItems
            End Get
        End Property

        Public Overridable Property Diagram() As AnimatGUI.Forms.Behavior.Diagram
            Get
                Return m_doDiagram
            End Get
            Set(ByVal Value As AnimatGUI.Forms.Behavior.Diagram)
                m_doDiagram = Value
            End Set
        End Property

        Public Overridable ReadOnly Property MatchText() As String
            Get
                Return txtMatch.Text
            End Get
        End Property

        Public Overridable ReadOnly Property ReplaceText() As String
            Get
                Return txtReplace.Text
            End Get
        End Property
#End Region

#Region " Methods "

#End Region

#Region " Events "

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            MyBase.OnLoad(e)

            Try
                m_btnOk = Me.btnReplace
                m_btnCancel = Me.btnCancel
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub btnReplace_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReplace.Click

            Try
                If m_doDiagram Is Nothing Then
                    Throw New System.Exception("No diagram was selected")
                End If

                If Me.txtMatch.Text.Trim().Length = 0 Then
                    Throw New System.Exception("The match expression string cannot be blank.")
                End If

                If Me.txtReplace.Text.Trim().Length = 0 Then
                    Throw New System.Exception("The replace expression string cannot be blank.")
                End If

                m_aryItems = New ArrayList
                If rbThisDiagram.Checked Then
                    m_doDiagram.Subsystem.RetrieveChildren(True, m_aryItems)
                ElseIf rbThisSubsystem.Checked Then
                    m_doDiagram.Subsystem.RetrieveChildren(False, m_aryItems)
                ElseIf rbSelected.Checked Then
                    Dim arySelected As Collections.DataObjects = Util.ProjectWorkspace.SelectedDataObjects

                    For Each dobj As Framework.DataObject In arySelected
                        m_aryItems.Add(dobj)
                    Next
                Else
                    Dim aryNodes As New Collections.DataObjects(Nothing)
                    m_doDiagram.Subsystem.Organism.FindChildrenOfType(GetType(DataObjects.Behavior.Node), aryNodes)

                    Dim aryLinks As New Collections.DataObjects(Nothing)
                    m_doDiagram.Subsystem.Organism.FindChildrenOfType(GetType(DataObjects.Behavior.Link), aryLinks)

                    For Each dobj As DataObjects.Behavior.Node In aryNodes
                        m_aryItems.Add(dobj)
                    Next

                    For Each dobj As DataObjects.Behavior.Link In aryLinks
                        m_aryItems.Add(dobj)
                    Next
                End If

                Dim frmConfirm As New AnimatGUI.Forms.ConfirmRelabel
                frmConfirm.Items = m_aryItems
                frmConfirm.Match = Me.txtMatch.Text
                frmConfirm.Replace = Me.txtReplace.Text

                If frmConfirm.ShowDialog() = DialogResult.OK Then
                    Util.RegExRelable(m_aryItems, Me.txtMatch.Text, Me.txtReplace.Text)
                    Me.Close()
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region


    End Class

End Namespace


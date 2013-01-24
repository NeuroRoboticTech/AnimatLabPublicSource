Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports AnimatGuiCtrls.Controls
Imports AnimatGUI.Framework

Namespace Forms.BodyPlan

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
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.Label1 = New System.Windows.Forms.Label
            Me.txtMatch = New System.Windows.Forms.TextBox
            Me.txtReplace = New System.Windows.Forms.TextBox
            Me.Label2 = New System.Windows.Forms.Label
            Me.btnReplace = New System.Windows.Forms.Button
            Me.btnCancel = New System.Windows.Forms.Button
            Me.SuspendLayout()
            '
            'Label1
            '
            Me.Label1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.Label1.Location = New System.Drawing.Point(10, 8)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(272, 16)
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
            Me.txtMatch.Size = New System.Drawing.Size(272, 20)
            Me.txtMatch.TabIndex = 1
            Me.txtMatch.Text = ""
            '
            'txtReplace
            '
            Me.txtReplace.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtReplace.Location = New System.Drawing.Point(10, 88)
            Me.txtReplace.Name = "txtReplace"
            Me.txtReplace.Size = New System.Drawing.Size(272, 20)
            Me.txtReplace.TabIndex = 3
            Me.txtReplace.Text = ""
            '
            'Label2
            '
            Me.Label2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.Label2.Location = New System.Drawing.Point(10, 64)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(272, 16)
            Me.Label2.TabIndex = 2
            Me.Label2.Text = "Replace Expression"
            Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'btnReplace
            '
            Me.btnReplace.Location = New System.Drawing.Point(40, 120)
            Me.btnReplace.Name = "btnReplace"
            Me.btnReplace.Size = New System.Drawing.Size(96, 24)
            Me.btnReplace.TabIndex = 4
            Me.btnReplace.Text = "Replace"
            '
            'btnCancel
            '
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(144, 120)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(96, 24)
            Me.btnCancel.TabIndex = 5
            Me.btnCancel.Text = "Cancel"
            '
            'Relabel
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(292, 150)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnReplace)
            Me.Controls.Add(Me.txtReplace)
            Me.Controls.Add(Me.txtMatch)
            Me.Controls.Add(Me.Label2)
            Me.Controls.Add(Me.Label1)
            Me.Name = "Relabel"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Relabel"
            Me.ResumeLayout(False)

        End Sub

#End Region

#Region " Attributes "

        Protected m_aryItems As New ArrayList
        Protected m_doSelectedItem As AnimatGUI.Framework.DataObject
        Protected m_doRootNode As AnimatGUI.DataObjects.Physical.BodyPart

#End Region

#Region " Properties "

        Public Overridable ReadOnly Property Items() As ArrayList
            Get
                Return m_aryItems
            End Get
        End Property

        Public Overridable Property SelectedItem() As AnimatGUI.Framework.DataObject
            Get
                Return m_doSelectedItem
            End Get
            Set(ByVal Value As AnimatGUI.Framework.DataObject)
                m_doSelectedItem = Value
            End Set
        End Property

        Public Overridable Property RootNode() As AnimatGUI.DataObjects.Physical.BodyPart
            Get
                Return m_doRootNode
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Physical.BodyPart)
                m_doRootNode = Value
            End Set
        End Property

#End Region

#Region " Methods "

#End Region

#Region " Events "

        Private Sub btnReplace_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReplace.Click

            Try
                If m_doSelectedItem Is Nothing Then
                    Throw New System.Exception("No item was selected")
                End If

                If Me.txtMatch.Text.Trim().Length = 0 Then
                    Throw New System.Exception("The match expression string cannot be blank.")
                End If

                If Me.txtReplace.Text.Trim().Length = 0 Then
                    Throw New System.Exception("The replace expression string cannot be blank.")
                End If

                Dim aryColObjs As New Collections.DataObjects(Nothing)
                If m_doRootNode Is Nothing Then
                    m_doSelectedItem.FindChildrenOfType(GetType(AnimatGUI.DataObjects.Physical.BodyPart), aryColObjs)
                Else

                    'If this is a joint then you really want to move from its parent down.
                    If TypeOf m_doRootNode Is AnimatGUI.DataObjects.Physical.Joint Then
                        Dim doJoint As AnimatGUI.DataObjects.Physical.Joint = DirectCast(m_doRootNode, AnimatGUI.DataObjects.Physical.Joint)

                        If Not doJoint.Parent Is Nothing Then
                            m_doRootNode = DirectCast(doJoint.Parent, AnimatGUI.DataObjects.Physical.BodyPart)
                        End If
                    End If

                    m_doRootNode.FindChildrenOfType(GetType(AnimatGUI.DataObjects.Physical.BodyPart), aryColObjs)
                End If

                m_aryItems = New ArrayList
                For Each doObj As Framework.DataObject In aryColObjs
                    m_aryItems.Add(doObj)
                Next

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

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            MyBase.OnLoad(e)

            Try
                m_btnOk = Me.btnReplace
                m_btnCancel = Me.btnCancel
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region


    End Class

End Namespace


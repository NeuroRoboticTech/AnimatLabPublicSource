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

Namespace Forms

    Public Class ReceptiveFieldCurrent
        'Inherits Windows.Forms.Form
        Inherits AnimatForm

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
        Friend WithEvents lvFieldPairs As System.Windows.Forms.ListView
        Friend WithEvents btnAdd As System.Windows.Forms.Button
        Friend WithEvents btnRemove As System.Windows.Forms.Button
        Friend WithEvents btnClear As System.Windows.Forms.Button
        Friend WithEvents cboNeurons As System.Windows.Forms.ComboBox
        Friend WithEvents txtSelVertex As System.Windows.Forms.TextBox
        Friend WithEvents lblReceptivePairs As System.Windows.Forms.Label
        Friend WithEvents lblSelectedVertex As System.Windows.Forms.Label
        Friend WithEvents lblNeurons As System.Windows.Forms.Label

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.lvFieldPairs = New System.Windows.Forms.ListView
            Me.btnAdd = New System.Windows.Forms.Button
            Me.btnRemove = New System.Windows.Forms.Button
            Me.btnClear = New System.Windows.Forms.Button
            Me.cboNeurons = New System.Windows.Forms.ComboBox
            Me.txtSelVertex = New System.Windows.Forms.TextBox
            Me.lblReceptivePairs = New System.Windows.Forms.Label
            Me.lblSelectedVertex = New System.Windows.Forms.Label
            Me.lblNeurons = New System.Windows.Forms.Label
            Me.SuspendLayout()
            '
            'lvFieldPairs
            '
            Me.lvFieldPairs.Location = New System.Drawing.Point(12, 150)
            Me.lvFieldPairs.Name = "lvFieldPairs"
            Me.lvFieldPairs.Size = New System.Drawing.Size(268, 164)
            Me.lvFieldPairs.TabIndex = 0
            Me.lvFieldPairs.UseCompatibleStateImageBehavior = False
            '
            'btnAdd
            '
            Me.btnAdd.Location = New System.Drawing.Point(12, 13)
            Me.btnAdd.Name = "btnAdd"
            Me.btnAdd.Size = New System.Drawing.Size(55, 25)
            Me.btnAdd.TabIndex = 2
            Me.btnAdd.Text = "Add"
            Me.btnAdd.UseVisualStyleBackColor = True
            '
            'btnRemove
            '
            Me.btnRemove.Location = New System.Drawing.Point(73, 12)
            Me.btnRemove.Name = "btnRemove"
            Me.btnRemove.Size = New System.Drawing.Size(55, 25)
            Me.btnRemove.TabIndex = 3
            Me.btnRemove.Text = "Remove"
            Me.btnRemove.UseVisualStyleBackColor = True
            '
            'btnClear
            '
            Me.btnClear.Location = New System.Drawing.Point(134, 12)
            Me.btnClear.Name = "btnClear"
            Me.btnClear.Size = New System.Drawing.Size(55, 25)
            Me.btnClear.TabIndex = 4
            Me.btnClear.Text = "Clear"
            Me.btnClear.UseVisualStyleBackColor = True
            '
            'cboNeurons
            '
            Me.cboNeurons.FormattingEnabled = True
            Me.cboNeurons.Location = New System.Drawing.Point(12, 57)
            Me.cboNeurons.Name = "cboNeurons"
            Me.cboNeurons.Size = New System.Drawing.Size(268, 21)
            Me.cboNeurons.TabIndex = 9
            '
            'txtSelVertex
            '
            Me.txtSelVertex.Location = New System.Drawing.Point(12, 99)
            Me.txtSelVertex.Name = "txtSelVertex"
            Me.txtSelVertex.ReadOnly = True
            Me.txtSelVertex.Size = New System.Drawing.Size(268, 20)
            Me.txtSelVertex.TabIndex = 10
            '
            'lblReceptivePairs
            '
            Me.lblReceptivePairs.Location = New System.Drawing.Point(12, 134)
            Me.lblReceptivePairs.Name = "lblReceptivePairs"
            Me.lblReceptivePairs.Size = New System.Drawing.Size(268, 13)
            Me.lblReceptivePairs.TabIndex = 11
            Me.lblReceptivePairs.Text = "Neuron\Vertex Receptive Field Pairs"
            Me.lblReceptivePairs.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'lblSelectedVertex
            '
            Me.lblSelectedVertex.Location = New System.Drawing.Point(12, 81)
            Me.lblSelectedVertex.Name = "lblSelectedVertex"
            Me.lblSelectedVertex.Size = New System.Drawing.Size(268, 13)
            Me.lblSelectedVertex.TabIndex = 12
            Me.lblSelectedVertex.Text = "Selected Vertex"
            Me.lblSelectedVertex.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'lblNeurons
            '
            Me.lblNeurons.Location = New System.Drawing.Point(12, 41)
            Me.lblNeurons.Name = "lblNeurons"
            Me.lblNeurons.Size = New System.Drawing.Size(268, 13)
            Me.lblNeurons.TabIndex = 13
            Me.lblNeurons.Text = "Neurons"
            Me.lblNeurons.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'ReceptiveFieldPairs
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(292, 326)
            Me.Controls.Add(Me.lblNeurons)
            Me.Controls.Add(Me.lblSelectedVertex)
            Me.Controls.Add(Me.lblReceptivePairs)
            Me.Controls.Add(Me.txtSelVertex)
            Me.Controls.Add(Me.cboNeurons)
            Me.Controls.Add(Me.btnClear)
            Me.Controls.Add(Me.btnRemove)
            Me.Controls.Add(Me.btnAdd)
            Me.Controls.Add(Me.lvFieldPairs)
            Me.Name = "ReceptiveFieldPairs"
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region

#Region " Attributes "

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property IconName() As String
            Get
                Return "AnimatGUI.ReceptiveField.gif"
            End Get
        End Property

#End Region

#Region " Methods "

        Public Overrides Sub Initialize(Optional ByVal frmParent As AnimatForm = Nothing)

            Try
                MyBase.Initialize(frmParent)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

#Region " Events "

        Private Sub ReceptiveFieldPairs_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Resize

            Try
                lblNeurons.Width = Me.Width - 30
                lblReceptivePairs.Width = Me.Width - 30
                lblSelectedVertex.Width = Me.Width - 30
                cboNeurons.Width = Me.Width - 30
                txtSelVertex.Width = Me.Width - 30
                lvFieldPairs.Width = Me.Width - 30
                lvFieldPairs.Height = Me.Height - lvFieldPairs.Location.Y - 30
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

    End Class

End Namespace
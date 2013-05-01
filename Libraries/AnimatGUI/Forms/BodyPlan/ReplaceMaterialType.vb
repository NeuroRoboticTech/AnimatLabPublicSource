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
Imports System.Drawing.Imaging

Namespace Forms.BodyPlan

    Public Class ReplaceMaterialType
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
        Friend WithEvents btnCancel As System.Windows.Forms.Button
        Friend WithEvents cboMaterialTypes As System.Windows.Forms.ComboBox
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents TextBox1 As System.Windows.Forms.TextBox

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        Friend WithEvents btnOk As System.Windows.Forms.Button
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ReplaceMaterialType))
            Me.btnOk = New System.Windows.Forms.Button()
            Me.btnCancel = New System.Windows.Forms.Button()
            Me.cboMaterialTypes = New System.Windows.Forms.ComboBox()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.TextBox1 = New System.Windows.Forms.TextBox()
            Me.SuspendLayout()
            '
            'btnOk
            '
            Me.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.btnOk.Location = New System.Drawing.Point(52, 147)
            Me.btnOk.Name = "btnOk"
            Me.btnOk.Size = New System.Drawing.Size(71, 32)
            Me.btnOk.TabIndex = 9
            Me.btnOk.Text = "Ok"
            '
            'btnCancel
            '
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(129, 147)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(71, 32)
            Me.btnCancel.TabIndex = 10
            Me.btnCancel.Text = "Cancel"
            '
            'cboMaterialTypes
            '
            Me.cboMaterialTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboMaterialTypes.FormattingEnabled = True
            Me.cboMaterialTypes.Location = New System.Drawing.Point(11, 120)
            Me.cboMaterialTypes.Name = "cboMaterialTypes"
            Me.cboMaterialTypes.Size = New System.Drawing.Size(230, 21)
            Me.cboMaterialTypes.TabIndex = 11
            '
            'Label1
            '
            Me.Label1.Location = New System.Drawing.Point(11, 101)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(230, 16)
            Me.Label1.TabIndex = 12
            Me.Label1.Text = "Replacement Material Type"
            Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'TextBox1
            '
            Me.TextBox1.Location = New System.Drawing.Point(11, 13)
            Me.TextBox1.Multiline = True
            Me.TextBox1.Name = "TextBox1"
            Me.TextBox1.ReadOnly = True
            Me.TextBox1.Size = New System.Drawing.Size(230, 81)
            Me.TextBox1.TabIndex = 13
            Me.TextBox1.Text = resources.GetString("TextBox1.Text")
            '
            'ReplaceMaterialType
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(252, 187)
            Me.Controls.Add(Me.TextBox1)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.cboMaterialTypes)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnOk)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "ReplaceMaterialType"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Edit Material Types"
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region

#Region " Attributes "

        Protected m_doTypeToReplace As DataObjects.Physical.MaterialType
 
#End Region

#Region " Properties "

        Public Property TypeToReplace() As DataObjects.Physical.MaterialType
            Get
                Return m_doTypeToReplace
            End Get
            Set(ByVal Value As DataObjects.Physical.MaterialType)
                m_doTypeToReplace = Value
            End Set
        End Property

#End Region

#Region " Methods "

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            MyBase.OnLoad(e)

            Try
                m_btnOk = Me.btnOk
                m_btnCancel = Me.btnCancel

                Dim doType As DataObjects.Physical.MaterialType
                For Each deEntry As DictionaryEntry In Util.Environment.MaterialTypes
                    doType = DirectCast(deEntry.Value, DataObjects.Physical.MaterialType)
                    If Not doType Is m_doTypeToReplace Then
                        cboMaterialTypes.Items.Add(doType)
                    End If
                Next

                cboMaterialTypes.SelectedIndex = 0

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

#Region " Events "


#End Region

#Region "Automation"

        Public Sub Automation_SelectMaterialType(ByVal strName As String)
            Dim doItem As Framework.DataObject = Util.FindComboItemByName(strName, cboMaterialTypes.Items)

            cboMaterialTypes.SelectedItem = doItem
        End Sub

#End Region

    End Class

End Namespace

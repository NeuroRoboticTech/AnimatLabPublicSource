Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports AnimatGuiCtrls.Controls
Imports AnimatGUI.Framework

Namespace Forms

    Public Class RelabelSelected
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
        Friend WithEvents txtNewLabel As System.Windows.Forms.TextBox
        Friend WithEvents txtStartWith As System.Windows.Forms.TextBox
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents btnOk As System.Windows.Forms.Button
        Friend WithEvents btnCancel As System.Windows.Forms.Button
        Friend WithEvents txtIncrementBy As System.Windows.Forms.TextBox
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.Label1 = New System.Windows.Forms.Label
            Me.txtNewLabel = New System.Windows.Forms.TextBox
            Me.txtStartWith = New System.Windows.Forms.TextBox
            Me.Label2 = New System.Windows.Forms.Label
            Me.Label3 = New System.Windows.Forms.Label
            Me.txtIncrementBy = New System.Windows.Forms.TextBox
            Me.btnOk = New System.Windows.Forms.Button
            Me.btnCancel = New System.Windows.Forms.Button
            Me.SuspendLayout()
            '
            'Label1
            '
            Me.Label1.Location = New System.Drawing.Point(10, 16)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(272, 16)
            Me.Label1.TabIndex = 0
            Me.Label1.Text = "New Label String"
            Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'txtNewLabel
            '
            Me.txtNewLabel.Location = New System.Drawing.Point(10, 32)
            Me.txtNewLabel.Name = "txtNewLabel"
            Me.txtNewLabel.Size = New System.Drawing.Size(272, 20)
            Me.txtNewLabel.TabIndex = 1
            Me.txtNewLabel.Text = ""
            '
            'txtStartWith
            '
            Me.txtStartWith.Location = New System.Drawing.Point(176, 104)
            Me.txtStartWith.Name = "txtStartWith"
            Me.txtStartWith.Size = New System.Drawing.Size(56, 20)
            Me.txtStartWith.TabIndex = 2
            Me.txtStartWith.Text = "1"
            '
            'Label2
            '
            Me.Label2.Location = New System.Drawing.Point(56, 104)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(120, 16)
            Me.Label2.TabIndex = 3
            Me.Label2.Text = "Start Numbering With:"
            '
            'Label3
            '
            Me.Label3.Location = New System.Drawing.Point(56, 128)
            Me.Label3.Name = "Label3"
            Me.Label3.Size = New System.Drawing.Size(120, 16)
            Me.Label3.TabIndex = 5
            Me.Label3.Text = "Increment By:"
            '
            'txtIncrementBy
            '
            Me.txtIncrementBy.Location = New System.Drawing.Point(176, 128)
            Me.txtIncrementBy.Name = "txtIncrementBy"
            Me.txtIncrementBy.Size = New System.Drawing.Size(56, 20)
            Me.txtIncrementBy.TabIndex = 4
            Me.txtIncrementBy.Text = "1"
            '
            'btnOk
            '
            Me.btnOk.Location = New System.Drawing.Point(40, 160)
            Me.btnOk.Name = "btnOk"
            Me.btnOk.Size = New System.Drawing.Size(96, 32)
            Me.btnOk.TabIndex = 6
            Me.btnOk.Text = "Relabel"
            '
            'btnCancel
            '
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(152, 160)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(96, 32)
            Me.btnCancel.TabIndex = 7
            Me.btnCancel.Text = "Cancel"
            '
            'RelabelSelected
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(292, 196)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnOk)
            Me.Controls.Add(Me.Label3)
            Me.Controls.Add(Me.txtIncrementBy)
            Me.Controls.Add(Me.Label2)
            Me.Controls.Add(Me.txtStartWith)
            Me.Controls.Add(Me.txtNewLabel)
            Me.Controls.Add(Me.Label1)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
            Me.Name = "RelabelSelected"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Relabel Selected Items"
            Me.ResumeLayout(False)

        End Sub

#End Region

#Region " Attributes "

        Protected m_strNewLabel As String = ""
        Protected m_iStartWith As Integer = 1
        Protected m_iIncrementBy As Integer = 1

#End Region

#Region " Properties "

        Public Property NewLabel() As String
            Get
                Return m_strNewLabel
            End Get
            Set(ByVal Value As String)
                m_strNewLabel = Value
            End Set
        End Property

        Public Property StartWith() As Integer
            Get
                Return m_iStartWith
            End Get
            Set(ByVal Value As Integer)
                m_iStartWith = Value
            End Set
        End Property

        Public Property IncrementBy() As Integer
            Get
                Return m_iIncrementBy
            End Get
            Set(ByVal Value As Integer)
                m_iIncrementBy = Value
            End Set
        End Property

#End Region

#Region " Methods "

#End Region

#Region " Events "

#End Region

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            MyBase.OnLoad(e)

            Try
                m_btnOk = Me.btnOk
                m_btnCancel = Me.btnCancel

                Me.txtNewLabel.Text = m_strNewLabel
                Me.txtStartWith.Text = m_iStartWith.ToString()
                Me.txtIncrementBy.Text = m_iIncrementBy.ToString()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
            Try
                If Me.txtNewLabel.Text.Trim().Length = 0 Then
                    Throw New System.Exception("The new label can not be empty.")
                End If

                If InStr(Me.txtNewLabel.Text, "%NUM%", CompareMethod.Text) = 0 Then
                    Throw New System.Exception("You must specify where you want the numbers placed in the new label. Place the code %NUM% in the string where you want the numbers placed.")
                End If

                If Me.txtStartWith.Text.Trim().Length = 0 Then
                    Throw New System.Exception("You must specify a number to begin with.")
                End If

                If Me.txtIncrementBy.Text.Trim().Length = 0 Then
                    Throw New System.Exception("You must specify a number to increment by.")
                End If

                m_iStartWith = Integer.Parse(txtStartWith.Text)
                m_iIncrementBy = Integer.Parse(txtIncrementBy.Text)
                Me.DialogResult = DialogResult.OK
                Me.Close()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

    End Class

End Namespace


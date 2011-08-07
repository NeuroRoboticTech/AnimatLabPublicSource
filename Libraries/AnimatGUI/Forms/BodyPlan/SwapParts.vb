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
Imports AnimatGUI
Imports AnimatGUI.Framework
Imports AnimatGUI.DataObjects
Imports System.Drawing.Imaging

Namespace Forms.BodyPlan

    Public Class SwapParts
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
        Friend WithEvents btnOk As System.Windows.Forms.Button
        Friend WithEvents btnCancel As System.Windows.Forms.Button
        Friend WithEvents cboPartTypes As System.Windows.Forms.ComboBox
        Friend WithEvents Label1 As System.Windows.Forms.Label
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.btnOk = New System.Windows.Forms.Button
            Me.btnCancel = New System.Windows.Forms.Button
            Me.cboPartTypes = New System.Windows.Forms.ComboBox
            Me.Label1 = New System.Windows.Forms.Label
            Me.SuspendLayout()
            '
            'btnOk
            '
            Me.btnOk.Location = New System.Drawing.Point(56, 80)
            Me.btnOk.Name = "btnOk"
            Me.btnOk.Size = New System.Drawing.Size(88, 40)
            Me.btnOk.TabIndex = 0
            Me.btnOk.Text = "Ok"
            '
            'btnCancel
            '
            Me.btnCancel.Location = New System.Drawing.Point(152, 80)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(88, 40)
            Me.btnCancel.TabIndex = 1
            Me.btnCancel.Text = "Cancel"
            '
            'cboPartTypes
            '
            Me.cboPartTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboPartTypes.Location = New System.Drawing.Point(8, 40)
            Me.cboPartTypes.Name = "cboPartTypes"
            Me.cboPartTypes.Size = New System.Drawing.Size(312, 21)
            Me.cboPartTypes.Sorted = True
            Me.cboPartTypes.TabIndex = 2
            '
            'Label1
            '
            Me.Label1.Location = New System.Drawing.Point(8, 16)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(312, 24)
            Me.Label1.TabIndex = 3
            Me.Label1.Text = "New Body Part Type"
            Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'SwapParts
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(328, 134)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.cboPartTypes)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnOk)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "SwapParts"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Swap Body Parts"
            Me.ResumeLayout(False)

        End Sub

#End Region

        Protected m_doExistingPart As AnimatGUI.DataObjects.Physical.BodyPart
        Protected m_doNewPart As AnimatGUI.DataObjects.Physical.BodyPart
        Protected m_aryPartList As AnimatGUI.Collections.BodyParts

        Public Overridable Property ExistingPart() As AnimatGUI.DataObjects.Physical.BodyPart
            Get
                Return m_doExistingPart
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Physical.BodyPart)
                m_doExistingPart = Value
            End Set
        End Property

        Public Overridable Property NewPart() As AnimatGUI.DataObjects.Physical.BodyPart
            Get
                Return m_doNewPart
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Physical.BodyPart)
                m_doNewPart = Value
            End Set
        End Property

        Public Overridable Property PartList() As AnimatGUI.Collections.BodyParts
            Get
                Return m_aryPartList
            End Get
            Set(ByVal Value As AnimatGUI.Collections.BodyParts)
                m_aryPartList = Value
            End Set
        End Property

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            MyBase.OnLoad(e)

            Try
                m_btnOk = Me.btnOk
                m_btnCancel = Me.btnCancel

                'Load up the combo box with the available part types
                If Not m_aryPartList Is Nothing AndAlso m_aryPartList.Count > 0 Then

                    Dim ciItem As ImageComboItem
                    cboPartTypes.Items.Clear()
                    For Each doItem As Framework.DataObject In m_aryPartList
                        Dim doPart As DataObjects.Physical.BodyPart = DirectCast(doItem, DataObjects.Physical.BodyPart)
                        ciItem = New ImageComboItem(doPart.BodyPartName)
                        ciItem.Tag = doPart

                        If Not doItem.GetType() Is m_doExistingPart.GetType Then
                            cboPartTypes.Items.Add(ciItem)

                            If cboPartTypes.SelectedItem Is Nothing Then
                                cboPartTypes.SelectedItem = ciItem
                            End If
                        End If
                    Next
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
            Try
                Me.NewPart = Nothing
                Me.DialogResult = DialogResult.Cancel

                If Not cboPartTypes.SelectedItem Is Nothing Then
                    Dim ciItem As ImageComboItem = DirectCast(cboPartTypes.SelectedItem, ImageComboItem)

                    If Not ciItem Is Nothing AndAlso Not ciItem.Tag Is Nothing Then
                        Me.NewPart = DirectCast(ciItem.Tag, DataObjects.Physical.BodyPart)
                        Me.DialogResult = DialogResult.OK
                    End If
                End If

                Me.Close()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            Try
                Me.NewPart = Nothing
                Me.DialogResult = DialogResult.Cancel
                Me.Close()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub


    End Class

End Namespace

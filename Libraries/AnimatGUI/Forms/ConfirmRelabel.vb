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
Imports System.Text.RegularExpressions

Namespace Forms

    Public Class ConfirmRelabel
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
        Friend WithEvents lvChanges As System.Windows.Forms.ListView
        Friend WithEvents chType As System.Windows.Forms.ColumnHeader
        Friend WithEvents chName As System.Windows.Forms.ColumnHeader
        Friend WithEvents chNewName As System.Windows.Forms.ColumnHeader
        Friend WithEvents btnConfirm As System.Windows.Forms.Button
        Friend WithEvents btnCancel As System.Windows.Forms.Button
        Friend WithEvents Label1 As System.Windows.Forms.Label
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.lvChanges = New System.Windows.Forms.ListView
            Me.chType = New System.Windows.Forms.ColumnHeader
            Me.chName = New System.Windows.Forms.ColumnHeader
            Me.chNewName = New System.Windows.Forms.ColumnHeader
            Me.btnConfirm = New System.Windows.Forms.Button
            Me.btnCancel = New System.Windows.Forms.Button
            Me.Label1 = New System.Windows.Forms.Label
            Me.SuspendLayout()
            '
            'lvChanges
            '
            Me.lvChanges.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lvChanges.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chType, Me.chName, Me.chNewName})
            Me.lvChanges.FullRowSelect = True
            Me.lvChanges.GridLines = True
            Me.lvChanges.Location = New System.Drawing.Point(10, 24)
            Me.lvChanges.MultiSelect = False
            Me.lvChanges.Name = "lvChanges"
            Me.lvChanges.Size = New System.Drawing.Size(316, 192)
            Me.lvChanges.Sorting = System.Windows.Forms.SortOrder.Ascending
            Me.lvChanges.TabIndex = 0
            Me.lvChanges.View = System.Windows.Forms.View.Details
            '
            'chType
            '
            Me.chType.Text = "Object Type"
            Me.chType.Width = 100
            '
            'chName
            '
            Me.chName.Text = "Name"
            Me.chName.Width = 100
            '
            'chNewName
            '
            Me.chNewName.Text = "New Name"
            Me.chNewName.Width = 100
            '
            'btnConfirm
            '
            Me.btnConfirm.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnConfirm.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.btnConfirm.Location = New System.Drawing.Point(92, 224)
            Me.btnConfirm.Name = "btnConfirm"
            Me.btnConfirm.Size = New System.Drawing.Size(72, 24)
            Me.btnConfirm.TabIndex = 1
            Me.btnConfirm.Text = "Confirm"
            '
            'btnCancel
            '
            Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(172, 224)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(72, 24)
            Me.btnCancel.TabIndex = 2
            Me.btnCancel.Text = "Cancel"
            '
            'Label1
            '
            Me.Label1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.Label1.Location = New System.Drawing.Point(8, 8)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(324, 16)
            Me.Label1.TabIndex = 3
            Me.Label1.Text = "List of Changes"
            Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'ConfirmRelabel
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(336, 254)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnConfirm)
            Me.Controls.Add(Me.lvChanges)
            Me.Name = "ConfirmRelabel"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Confirm Relabel"
            Me.ResumeLayout(False)

        End Sub

#End Region

#Region " Attributes "

        Protected m_aryItems As New ArrayList
        Protected m_strMatch As String
        Protected m_strReplace As String

#End Region

#Region " Properties "

        Public Property Items() As ArrayList
            Get
                Return m_aryItems
            End Get
            Set(ByVal Value As ArrayList)
                m_aryItems = Value
            End Set
        End Property

        Public Property Match() As String
            Get
                Return m_strMatch
            End Get
            Set(ByVal Value As String)
                m_strMatch = Value
            End Set
        End Property

        Public Property Replace() As String
            Get
                Return m_strReplace
            End Get
            Set(ByVal Value As String)
                m_strReplace = Value
            End Set
        End Property

#End Region

#Region " Methods "

#End Region

#Region " Events "

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            MyBase.OnLoad(e)

            Try
                m_btnOk = Me.btnConfirm
                m_btnCancel = Me.btnCancel

                Dim bFound As Boolean = False

                If Not Me.Items Is Nothing AndAlso Me.Items.Count > 0 AndAlso m_strMatch.Trim().Length > 0 AndAlso m_strReplace.Trim().Length > 0 Then
                    Dim doData As Framework.DataObject
                    Dim liItem As ListViewItem
                    For Each oObj As Object In m_aryItems
                        If TypeOf oObj Is Framework.DataObject Then
                            doData = DirectCast(oObj, Framework.DataObject)

                            If Regex.IsMatch(doData.Name, m_strMatch) Then
                                bFound = True
                                liItem = New ListViewItem(doData.GetType().Name)
                                liItem.SubItems.Add(doData.Name)
                                liItem.SubItems.Add(Regex.Replace(doData.Name, m_strMatch, m_strReplace))

                                lvChanges.Items.Add(liItem)
                            End If
                        End If
                    Next

                End If

                If Not bFound Then
                    Util.ShowMessage("No matches were found.")
                    Me.DialogResult = DialogResult.Cancel
                    Me.Close()
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

    End Class

End Namespace


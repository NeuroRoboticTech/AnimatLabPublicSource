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

Namespace Forms.Tools

    Public Class CompareItems
        Inherits Crownwood.DotNetMagic.Forms.DotNetMagicForm

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
        Friend WithEvents pnlGrids As System.Windows.Forms.Panel
        Friend WithEvents lvSelectedItems As System.Windows.Forms.ListView
        Friend WithEvents chName As System.Windows.Forms.ColumnHeader
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.pnlGrids = New System.Windows.Forms.Panel
            Me.lvSelectedItems = New System.Windows.Forms.ListView
            Me.chName = New System.Windows.Forms.ColumnHeader
            Me.SuspendLayout()
            '
            'pnlGrids
            '
            Me.pnlGrids.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.pnlGrids.AutoScroll = True
            Me.pnlGrids.BackColor = System.Drawing.SystemColors.Control
            Me.pnlGrids.Location = New System.Drawing.Point(160, 32)
            Me.pnlGrids.Name = "pnlGrids"
            Me.pnlGrids.Size = New System.Drawing.Size(480, 392)
            Me.pnlGrids.TabIndex = 0
            '
            'lvSelectedItems
            '
            Me.lvSelectedItems.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chName})
            Me.lvSelectedItems.FullRowSelect = True
            Me.lvSelectedItems.HideSelection = False
            Me.lvSelectedItems.Location = New System.Drawing.Point(8, 40)
            Me.lvSelectedItems.Name = "lvSelectedItems"
            Me.lvSelectedItems.Size = New System.Drawing.Size(136, 320)
            Me.lvSelectedItems.TabIndex = 1
            Me.lvSelectedItems.View = System.Windows.Forms.View.Details
            '
            'chName
            '
            Me.chName.Text = "Selected Items"
            Me.chName.Width = 150
            '
            'CompareItems
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(640, 437)
            Me.Controls.Add(Me.lvSelectedItems)
            Me.Controls.Add(Me.pnlGrids)
            Me.Name = "CompareItems"
            Me.Text = "CompareItems"
            Me.ResumeLayout(False)

        End Sub

#End Region

#Region " Attributes "

        Protected m_doPhysicalStructure As AnimatGUI.DataObjects.Physical.PhysicalStructure
        Protected m_arySelectedItems As New AnimatGUI.Collections.DataObjects(Nothing)

#End Region

#Region " Properties "

        Public Property PhysicalStructure() As AnimatGUI.DataObjects.Physical.PhysicalStructure
            Get
                Return m_doPhysicalStructure
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Physical.PhysicalStructure)
                m_doPhysicalStructure = Value
            End Set
        End Property

        Public Property SelectedItems() As AnimatGUI.Collections.DataObjects
            Get
                Return m_arySelectedItems
            End Get
            Set(ByVal Value As AnimatGUI.Collections.DataObjects)
                m_arySelectedItems = Value
            End Set
        End Property

#End Region

#Region " Methods "
        Public Sub VerifyItemType()
            Dim tpTemplate As Type
            For Each doItem As AnimatGUI.Framework.DataObject In m_arySelectedItems
                If tpTemplate Is Nothing Then
                    tpTemplate = doItem.GetType()
                Else
                    If Not Util.IsTypeOf(doItem.GetType, tpTemplate, False) Then
                        Throw New System.Exception("The object types for the selected items are not all the same. You can only compare similar items. Item '" & doItem.Name & "' is different than the other items.")
                    End If
                End If
            Next
        End Sub



#End Region

#Region " Events "

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)

            Try
                Dim iX As Integer = 0
                For Each doItem As AnimatGUI.Framework.DataObject In m_arySelectedItems
                    Dim ctrlGrid As New PropertyGrid
                    ctrlGrid.Location = New Point(iX, 0)
                    ctrlGrid.Size = New Size(300, Me.Height - 10)
                    ctrlGrid.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Bottom
                    ctrlGrid.SelectedObject = doItem.Properties
                    Me.pnlGrids.Controls.Add(ctrlGrid)

                    iX = iX + 300
                Next


                For Each doItem As AnimatGUI.Framework.DataObject In m_arySelectedItems
                    Dim liItem As New ListViewItem
                    liItem.Text = doItem.Name

                    liItem.Tag = doItem
                    lvSelectedItems.Items.Add(liItem)
                Next
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region



    End Class

End Namespace

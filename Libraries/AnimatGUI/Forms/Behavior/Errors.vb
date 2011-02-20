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

Namespace Forms.Behavior

    Public Class Errors
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

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        Friend WithEvents lvErrors As System.Windows.Forms.ListView
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.lvErrors = New System.Windows.Forms.ListView
            Me.SuspendLayout()
            '
            'lvErrors
            '
            'Me.lvErrors.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            '            Or System.Windows.Forms.AnchorStyles.Left) _
            '            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lvErrors.Location = New System.Drawing.Point(2, 2)
            Me.lvErrors.Name = "lvErrors"
            Me.lvErrors.Size = New System.Drawing.Size(616, 192)
            Me.lvErrors.TabIndex = 0
            '
            'Errors
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(632, 214)
            Me.Controls.Add(Me.lvErrors)
            Me.Name = "Errors"
            Me.Text = "Errors"
            Me.ResumeLayout(False)

        End Sub

#End Region

#Region " Attributes "

        Protected m_beEditor As AnimatGUI.Forms.Behavior.Editor
        Protected m_mgrToolStripImages As AnimatGUI.Framework.ImageManager
        Protected m_aryErrors As Collections.DiagramErrors

#End Region

#Region " Properties "

        Public Overridable Property Editor() As Forms.Behavior.Editor
            Get
                Return m_beEditor
            End Get
            Set(ByVal Value As Forms.Behavior.Editor)
                m_beEditor = Value
            End Set
        End Property

        Public Overridable ReadOnly Property Errors() As Collections.DiagramErrors
            Get
                Return m_aryErrors
            End Get
        End Property

        Public Overridable ReadOnly Property ToolStripImages() As AnimatGUI.Framework.ImageManager
            Get
                Return m_mgrToolStripImages
            End Get
        End Property

#End Region

#Region " Methods "

        Public Overrides Sub Initialize(Optional ByVal frmParent As AnimatForm = Nothing)
            Try

                MyBase.Initialize(frmParent)

                'm_beEditor = DirectCast(frmMdiParent, AnimatGUI.Forms.Behavior.Editor)
                m_aryErrors = New Collections.DiagramErrors(Me.FormHelper, Me)

                lvErrors.HideSelection = False
                lvErrors.MultiSelect = False
                lvErrors.FullRowSelect = True
                lvErrors.GridLines = True
                lvErrors.View = View.Details
                lvErrors.AllowColumnReorder = True
                lvErrors.LabelEdit = False
                lvErrors.Sorting = SortOrder.Ascending

                lvErrors.Columns.Add("Level", 80, HorizontalAlignment.Left)
                lvErrors.Columns.Add("Item Type", 80, HorizontalAlignment.Left)
                lvErrors.Columns.Add("Item Name", 80, HorizontalAlignment.Left)
                lvErrors.Columns.Add("Error Type", 80, HorizontalAlignment.Left)
                lvErrors.Columns.Add("Error Message", 400, HorizontalAlignment.Left)

                Dim myAssembly As System.Reflection.Assembly
                myAssembly = System.Reflection.Assembly.Load("AnimatGUI")

                m_mgrToolStripImages = New AnimatGUI.Framework.ImageManager
                m_mgrToolStripImages.ImageList.ImageSize = New Size(16, 16)
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.ErrorSmall.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.WarningSmall.gif")

                lvErrors.SmallImageList = m_mgrToolStripImages.ImageList

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overridable Sub AddItem(ByRef lvItem As ListViewItem)
            If Not lvItem Is Nothing Then
                lvErrors.Items.Add(lvItem)
            End If
        End Sub

        Public Overridable Sub RemoveItem(ByRef lvItem As ListViewItem)
            If Not lvItem Is Nothing Then
                lvErrors.Items.Remove(lvItem)
            End If
        End Sub

#End Region

#Region " Events "

        Private Sub Errors_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize

            Try

                Me.lvErrors.Width = Me.Width - 5
                Me.lvErrors.Height = Me.Height - 5

            Catch ex As System.Exception

            End Try
        End Sub

        Private Sub lvErrors_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvErrors.DoubleClick

            Try
                If lvErrors.SelectedItems.Count > 0 Then
                    Dim lvItem As ListViewItem = lvErrors.SelectedItems(0)

                    If Not lvItem.Tag Is Nothing Then
                        Dim deError As DataObjects.Behavior.DiagramError = DirectCast(lvItem.Tag, DataObjects.Behavior.DiagramError)
                        deError.DoubleClicked(m_beEditor)
                    End If
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub lvErrors_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles lvErrors.ColumnClick
            Try
                ' Set the ListViewItemSorter property to a new ListViewItemComparer object.
                Me.lvErrors.ListViewItemSorter = New ListViewItemComparer(e.Column)
                ' Call the sort method to manually sort the column based on the ListViewItemComparer implementation.
                lvErrors.Sort()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

#Region " ListViewItemComparer "

        Class ListViewItemComparer
            Implements IComparer

            Private col As Integer

            Public Sub New()
                col = 0
            End Sub

            Public Sub New(ByVal column As Integer)
                col = column
            End Sub

            Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer _
               Implements IComparer.Compare
                Return [String].Compare(CType(x, ListViewItem).SubItems(col).Text, CType(y, ListViewItem).SubItems(col).Text)
            End Function
        End Class

#End Region

    End Class

End Namespace

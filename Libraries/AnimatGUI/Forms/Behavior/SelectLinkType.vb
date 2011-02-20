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

Namespace Forms.Behavior

    Public Class SelectLinkType
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
        Friend WithEvents ctrlLinkTypes As System.Windows.Forms.ListView
        Friend WithEvents chLinkType As System.Windows.Forms.ColumnHeader
        Friend WithEvents chLinkDescription As System.Windows.Forms.ColumnHeader
        Friend WithEvents btnOk As System.Windows.Forms.Button
        Friend WithEvents btnCancel As System.Windows.Forms.Button
        Friend WithEvents lblDescription As System.Windows.Forms.RichTextBox
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.ctrlLinkTypes = New System.Windows.Forms.ListView
            Me.chLinkType = New System.Windows.Forms.ColumnHeader
            Me.chLinkDescription = New System.Windows.Forms.ColumnHeader
            Me.btnOk = New System.Windows.Forms.Button
            Me.btnCancel = New System.Windows.Forms.Button
            Me.lblDescription = New System.Windows.Forms.RichTextBox
            Me.SuspendLayout()
            '
            'ctrlLinkTypes
            '
            Me.ctrlLinkTypes.Activation = System.Windows.Forms.ItemActivation.OneClick
            Me.ctrlLinkTypes.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.ctrlLinkTypes.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chLinkType, Me.chLinkDescription})
            Me.ctrlLinkTypes.Location = New System.Drawing.Point(8, 8)
            Me.ctrlLinkTypes.Name = "ctrlLinkTypes"
            Me.ctrlLinkTypes.Size = New System.Drawing.Size(496, 112)
            Me.ctrlLinkTypes.TabIndex = 0
            '
            'chLinkType
            '
            Me.chLinkType.Text = "Link Type"
            '
            'chLinkDescription
            '
            Me.chLinkDescription.Text = "Description"
            '
            'btnOk
            '
            Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnOk.Location = New System.Drawing.Point(188, 224)
            Me.btnOk.Name = "btnOk"
            Me.btnOk.Size = New System.Drawing.Size(64, 24)
            Me.btnOk.TabIndex = 1
            Me.btnOk.Text = "Ok"
            '
            'btnCancel
            '
            Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(260, 224)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(64, 24)
            Me.btnCancel.TabIndex = 2
            Me.btnCancel.Text = "Cancel"
            '
            'lblDescription
            '
            Me.lblDescription.BackColor = System.Drawing.SystemColors.Control
            Me.lblDescription.Location = New System.Drawing.Point(8, 128)
            Me.lblDescription.Name = "lblDescription"
            Me.lblDescription.ReadOnly = True
            Me.lblDescription.Size = New System.Drawing.Size(496, 88)
            Me.lblDescription.TabIndex = 4
            Me.lblDescription.Text = ""
            '
            'SelectLinkType
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(512, 254)
            Me.Controls.Add(Me.lblDescription)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnOk)
            Me.Controls.Add(Me.ctrlLinkTypes)
            Me.Name = "SelectLinkType"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Select Link Type"
            Me.ResumeLayout(False)

        End Sub

#End Region

#Region " Attributes "

        Protected m_blSelectedLink As DataObjects.Behavior.Link
        Protected m_bnOrigin As DataObjects.Behavior.Node
        Protected m_bnDestination As DataObjects.Behavior.Node
        Protected m_aryCompatibleLinks As Collections.Links
        Protected m_mgrIconImages As AnimatGUI.Framework.ImageManager

#End Region

#Region " Properties "

        Public Property SelectedLink() As DataObjects.Behavior.Link
            Get
                Return m_blSelectedLink
            End Get
            Set(ByVal Value As DataObjects.Behavior.Link)
                m_blSelectedLink = Value
            End Set
        End Property

        Public Property Origin() As DataObjects.Behavior.Node
            Get
                Return m_bnOrigin
            End Get
            Set(ByVal Value As DataObjects.Behavior.Node)
                m_bnOrigin = Value
            End Set
        End Property

        Public Property Destination() As DataObjects.Behavior.Node
            Get
                Return m_bnDestination
            End Get
            Set(ByVal Value As DataObjects.Behavior.Node)
                m_bnDestination = Value
            End Set
        End Property

        Public Property CompatibleLinks() As Collections.Links
            Get
                Return m_aryCompatibleLinks
            End Get
            Set(ByVal Value As Collections.Links)
                m_aryCompatibleLinks = Value
            End Set
        End Property

#End Region

#Region " Methods "

#End Region

#Region " Events "

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            MyBase.OnLoad(e)

            Try
                If m_aryCompatibleLinks Is Nothing Then
                    Throw New System.Exception("The list of compatible links is not defined.")
                End If

                If m_aryCompatibleLinks.Count = 0 Then
                    Throw New System.Exception("There are no compatible links defined in the list.")
                End If

                Dim myAssembly As System.Reflection.Assembly
                myAssembly = System.Reflection.Assembly.Load("AnimatGUI")

                Dim imgDefault As Image = ImageManager.LoadImage(myAssembly, "AnimatGUI.DefaultLink.gif")

                'Find the maximum image size of the compatible icons
                Dim iMaxWidth As Integer = imgDefault.Width
                Dim iMaxHeight As Integer = imgDefault.Height
                m_mgrIconImages = New AnimatGUI.Framework.ImageManager
                For Each blLink As DataObjects.Behavior.Link In m_aryCompatibleLinks
                    If Not blLink.WorkspaceImage Is Nothing AndAlso Not TypeOf (blLink) Is AnimatGUI.DataObjects.Behavior.Links.Adapter Then
                        If blLink.WorkspaceImage.Width > iMaxWidth Then iMaxWidth = blLink.WorkspaceImage.Width
                        If blLink.WorkspaceImage.Height > iMaxHeight Then iMaxHeight = blLink.WorkspaceImage.Height
                    End If
                Next

                m_mgrIconImages.ImageList.ImageSize = New Size(iMaxWidth, iMaxHeight)

                m_mgrIconImages.AddImage("DefaultLink", imgDefault)

                'Now lets go through and add create the image list
                For Each blLink As DataObjects.Behavior.Link In m_aryCompatibleLinks
                    If Not blLink.WorkspaceImage Is Nothing AndAlso Not TypeOf (blLink) Is AnimatGUI.DataObjects.Behavior.Links.Adapter Then
                        m_mgrIconImages.AddImage(blLink.Name, blLink.WorkspaceImage)
                    End If
                Next

                ctrlLinkTypes.LargeImageList = m_mgrIconImages.ImageList

                'Now lets go through and add the link types to the list
                For Each blLink As DataObjects.Behavior.Link In m_aryCompatibleLinks
                    If Not TypeOf (blLink) Is AnimatGUI.DataObjects.Behavior.Links.Adapter Then
                        Dim liItem As New ListViewItem
                        liItem.Text = blLink.Name

                        If Not blLink.WorkspaceImage Is Nothing Then
                            liItem.ImageIndex = m_mgrIconImages.GetImageIndex(blLink.Name)
                        Else
                            liItem.ImageIndex = m_mgrIconImages.GetImageIndex("DefaultLink")
                        End If

                        liItem.Tag = blLink
                        ctrlLinkTypes.Items.Add(liItem)
                    End If
                Next

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
            MyBase.OnResize(e)

            Try
                lblDescription.Left = 8
                lblDescription.Top = (ctrlLinkTypes.Top + ctrlLinkTypes.Height + 5)
                lblDescription.Width = ctrlLinkTypes.Width

                btnOk.Left = CInt((Me.Width / 2) - btnOk.Width - 2)
                btnCancel.Left = CInt((Me.Width / 2) + 2)

            Catch ex As System.Exception

            End Try
        End Sub

        Private Sub ctrlLinkTypes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ctrlLinkTypes.Click

            Try

                If ctrlLinkTypes.SelectedItems.Count = 1 Then
                    Dim liItem As ListViewItem = ctrlLinkTypes.SelectedItems(0)

                    lblDescription.Clear()
                    lblDescription.SelectionFont = New Font("Arial", 10, FontStyle.Bold)

                    Dim blLink As DataObjects.Behavior.Link = DirectCast(liItem.Tag, DataObjects.Behavior.Link)
                    lblDescription.AppendText(blLink.Name & vbCrLf)

                    lblDescription.SelectionFont = New Font("Arial", 10)
                    lblDescription.AppendText(blLink.Description)

                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub ctrlLinkTypes_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ctrlLinkTypes.DoubleClick
            btnOk_Click(sender, e)

            If Me.DialogResult = DialogResult.OK Then
                Me.Close()
            End If
        End Sub

        Private Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
            Try
                If ctrlLinkTypes.SelectedItems.Count <> 1 Then
                    Throw New System.Exception("You must select a link type or hit cancel.")
                End If

                Dim liItem As ListViewItem = ctrlLinkTypes.SelectedItems(0)
                Dim blLink As DataObjects.Behavior.Link = DirectCast(liItem.Tag, DataObjects.Behavior.Link)

                m_blSelectedLink = DirectCast(blLink.Clone(m_bnOrigin.Parent, False, Nothing), DataObjects.Behavior.Link)
                m_blSelectedLink.Origin = m_bnOrigin
                m_blSelectedLink.Destination = m_bnDestination

                Me.DialogResult = DialogResult.OK
                Me.Close()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace


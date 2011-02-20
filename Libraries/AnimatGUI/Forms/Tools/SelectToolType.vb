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

Namespace Forms.Tools

    Public Class SelectToolType
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
        Friend WithEvents lblDescription As System.Windows.Forms.RichTextBox
        Friend WithEvents btnCancel As System.Windows.Forms.Button
        Friend WithEvents btnOk As System.Windows.Forms.Button
        Friend WithEvents ctrlToolTypes As System.Windows.Forms.ListView
        Friend WithEvents chStimulusType As System.Windows.Forms.ColumnHeader
        Friend WithEvents chStimulusDescription As System.Windows.Forms.ColumnHeader
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.lblDescription = New System.Windows.Forms.RichTextBox
            Me.btnCancel = New System.Windows.Forms.Button
            Me.btnOk = New System.Windows.Forms.Button
            Me.ctrlToolTypes = New System.Windows.Forms.ListView
            Me.chStimulusType = New System.Windows.Forms.ColumnHeader
            Me.chStimulusDescription = New System.Windows.Forms.ColumnHeader
            Me.SuspendLayout()
            '
            'lblDescription
            '
            Me.lblDescription.BackColor = System.Drawing.SystemColors.Control
            Me.lblDescription.Location = New System.Drawing.Point(8, 127)
            Me.lblDescription.Name = "lblDescription"
            Me.lblDescription.ReadOnly = True
            Me.lblDescription.Size = New System.Drawing.Size(496, 88)
            Me.lblDescription.TabIndex = 12
            Me.lblDescription.Text = ""
            '
            'btnCancel
            '
            Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(260, 223)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(64, 24)
            Me.btnCancel.TabIndex = 11
            Me.btnCancel.Text = "Cancel"
            '
            'btnOk
            '
            Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnOk.Location = New System.Drawing.Point(188, 223)
            Me.btnOk.Name = "btnOk"
            Me.btnOk.Size = New System.Drawing.Size(64, 24)
            Me.btnOk.TabIndex = 10
            Me.btnOk.Text = "Ok"
            '
            'ctrlToolTypes
            '
            Me.ctrlToolTypes.Activation = System.Windows.Forms.ItemActivation.OneClick
            Me.ctrlToolTypes.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.ctrlToolTypes.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chStimulusType, Me.chStimulusDescription})
            Me.ctrlToolTypes.Location = New System.Drawing.Point(8, 7)
            Me.ctrlToolTypes.Name = "ctrlToolTypes"
            Me.ctrlToolTypes.Size = New System.Drawing.Size(496, 112)
            Me.ctrlToolTypes.TabIndex = 9
            '
            'chStimulusType
            '
            Me.chStimulusType.Text = "Stimulus Type"
            '
            'chStimulusDescription
            '
            Me.chStimulusDescription.Text = "Description"
            '
            'SelectToolType
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(512, 254)
            Me.Controls.Add(Me.lblDescription)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnOk)
            Me.Controls.Add(Me.ctrlToolTypes)
            Me.Name = "SelectToolType"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Select Data Tool Type"
            Me.ResumeLayout(False)

        End Sub

#End Region


#Region " Attributes "

        Protected m_doSelectedTool As Forms.Tools.ToolForm
        Protected m_mgrIconImages As AnimatGUI.Framework.ImageManager

#End Region

#Region " Properties "

        Public Property SelectedToolType() As Forms.Tools.ToolForm
            Get
                Return m_doSelectedTool
            End Get
            Set(ByVal Value As Forms.Tools.ToolForm)
                m_doSelectedTool = Value
            End Set
        End Property

#End Region

#Region " Methods "

#End Region

#Region " Events "

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            MyBase.OnLoad(e)

            Try

                Dim myAssembly As System.Reflection.Assembly
                myAssembly = System.Reflection.Assembly.Load("AnimatGUI")

                Dim imgDefault As Image = ImageManager.LoadImage(myAssembly, "AnimatGUI.Wrench.gif")

                'Find the maximum image size of the compatible icons
                Dim iMaxWidth As Integer = imgDefault.Width
                Dim iMaxHeight As Integer = imgDefault.Height
                m_mgrIconImages = New AnimatGUI.Framework.ImageManager
                For Each doTool As Forms.Tools.ToolForm In Util.Application.ToolPlugins
                    If Not doTool.TabImage Is Nothing Then
                        If doTool.TabImage.Width > iMaxWidth Then iMaxWidth = doTool.TabImage.Width
                        If doTool.TabImage.Height > iMaxHeight Then iMaxHeight = doTool.TabImage.Height
                    End If
                Next

                m_mgrIconImages.ImageList.ImageSize = New Size(iMaxWidth, iMaxHeight)

                m_mgrIconImages.AddImage("DefaultTool", imgDefault)

                For Each doTool As Forms.Tools.ToolForm In Util.Application.ToolPlugins
                    If Not doTool.TabImage Is Nothing Then
                        m_mgrIconImages.AddImage(doTool.TabImageName, doTool.TabImage)
                    End If
                Next

                ctrlToolTypes.LargeImageList = m_mgrIconImages.ImageList

                'Now lets go through and add the link types to the list
                For Each doTool As Forms.Tools.ToolForm In Util.Application.ToolPlugins
                    Dim liItem As New ListViewItem
                    liItem.Text = doTool.Name

                    If Not doTool.TabImage Is Nothing Then
                        liItem.ImageIndex = m_mgrIconImages.GetImageIndex(doTool.TabImageName)
                    Else
                        liItem.ImageIndex = m_mgrIconImages.GetImageIndex("DefaultTool")
                    End If

                    liItem.Tag = doTool
                    ctrlToolTypes.Items.Add(liItem)
                Next

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
            MyBase.OnResize(e)

            Try
                lblDescription.Left = 8
                lblDescription.Top = (ctrlToolTypes.Top + ctrlToolTypes.Height + 5)
                lblDescription.Width = ctrlToolTypes.Width

                btnOk.Left = CInt((Me.Width / 2) - btnOk.Width - 2)
                btnCancel.Left = CInt((Me.Width / 2) + 2)

            Catch ex As System.Exception

            End Try
        End Sub

        Private Sub ctrlLinkTypes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ctrlToolTypes.Click

            Try

                If ctrlToolTypes.SelectedItems.Count = 1 Then
                    Dim liItem As ListViewItem = ctrlToolTypes.SelectedItems(0)

                    lblDescription.Clear()
                    lblDescription.SelectionFont = New Font("Arial", 10, FontStyle.Bold)

                    Dim doTool As Forms.Tools.ToolForm = DirectCast(liItem.Tag, Forms.Tools.ToolForm)
                    lblDescription.AppendText(doTool.Name & vbCrLf)

                    lblDescription.SelectionFont = New Font("Arial", 10)
                    lblDescription.AppendText(doTool.Description)

                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub ctrlLinkTypes_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ctrlToolTypes.DoubleClick
            btnOk_Click(sender, e)

            If Me.DialogResult = DialogResult.OK Then
                Me.Close()
            End If
        End Sub

        Private Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
            Try
                If ctrlToolTypes.SelectedItems.Count <> 1 Then
                    Throw New System.Exception("You must select a stimulus type or hit cancel.")
                End If

                Dim liItem As ListViewItem = ctrlToolTypes.SelectedItems(0)
                m_doSelectedTool = DirectCast(liItem.Tag, Forms.Tools.ToolForm)

                Me.DialogResult = DialogResult.OK
                Me.Close()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace


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

Namespace Forms.ExternalStimuli

    Public Class SelectStimulusType
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
        Friend WithEvents ctrlStimulusTypes As System.Windows.Forms.ListView
        Friend WithEvents chStimulusType As System.Windows.Forms.ColumnHeader
        Friend WithEvents chStimulusDescription As System.Windows.Forms.ColumnHeader
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.lblDescription = New System.Windows.Forms.RichTextBox
            Me.btnCancel = New System.Windows.Forms.Button
            Me.btnOk = New System.Windows.Forms.Button
            Me.ctrlStimulusTypes = New System.Windows.Forms.ListView
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
            Me.lblDescription.TabIndex = 8
            Me.lblDescription.Text = ""
            '
            'btnCancel
            '
            Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(260, 223)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(64, 24)
            Me.btnCancel.TabIndex = 7
            Me.btnCancel.Text = "Cancel"
            '
            'btnOk
            '
            Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnOk.Location = New System.Drawing.Point(188, 223)
            Me.btnOk.Name = "btnOk"
            Me.btnOk.Size = New System.Drawing.Size(64, 24)
            Me.btnOk.TabIndex = 6
            Me.btnOk.Text = "Ok"
            '
            'ctrlStimulusTypes
            '
            Me.ctrlStimulusTypes.Activation = System.Windows.Forms.ItemActivation.OneClick
            Me.ctrlStimulusTypes.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.ctrlStimulusTypes.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chStimulusType, Me.chStimulusDescription})
            Me.ctrlStimulusTypes.Location = New System.Drawing.Point(8, 7)
            Me.ctrlStimulusTypes.Name = "ctrlStimulusTypes"
            Me.ctrlStimulusTypes.Size = New System.Drawing.Size(496, 112)
            Me.ctrlStimulusTypes.TabIndex = 5
            '
            'chStimulusType
            '
            Me.chStimulusType.Text = "Stimulus Type"
            '
            'chStimulusDescription
            '
            Me.chStimulusDescription.Text = "Description"
            '
            'SelectStimulusType
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(512, 254)
            Me.Controls.Add(Me.lblDescription)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnOk)
            Me.Controls.Add(Me.ctrlStimulusTypes)
            Me.Name = "SelectStimulusType"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Select Stimulus Type"
            Me.ResumeLayout(False)

        End Sub

#End Region

#Region " Attributes "

        Protected m_doSelectedStimulus As DataObjects.ExternalStimuli.Stimulus
        Protected m_aryCompatibleStimuli As Collections.Stimuli
        Protected m_mgrIconImages As AnimatGUI.Framework.ImageManager

#End Region

#Region " Properties "

        Public Property SelectedStimulus() As DataObjects.ExternalStimuli.Stimulus
            Get
                Return m_doSelectedStimulus
            End Get
            Set(ByVal Value As DataObjects.ExternalStimuli.Stimulus)
                m_doSelectedStimulus = Value
            End Set
        End Property

        Public Property CompatibleStimuli() As Collections.Stimuli
            Get
                Return m_aryCompatibleStimuli
            End Get
            Set(ByVal Value As Collections.Stimuli)
                m_aryCompatibleStimuli = Value
            End Set
        End Property

#End Region

#Region " Methods "

#End Region

#Region " Events "

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            MyBase.OnLoad(e)

            Try
                If m_aryCompatibleStimuli Is Nothing Then
                    Throw New System.Exception("The list of compatible stimulus is not defined.")
                End If

                If m_aryCompatibleStimuli.Count = 0 Then
                    Throw New System.Exception("There are no compatible stimulus types defined in the list.")
                End If

                Dim myAssembly As System.Reflection.Assembly
                myAssembly = System.Reflection.Assembly.Load("AnimatGUI")

                Dim imgDefault As Image = ImageManager.LoadImage(myAssembly, "AnimatGUI.ExternalStimulus.gif")

                'Find the maximum image size of the compatible icons
                Dim iMaxWidth As Integer = imgDefault.Width
                Dim iMaxHeight As Integer = imgDefault.Height
                m_mgrIconImages = New AnimatGUI.Framework.ImageManager
                For Each doStimulus As DataObjects.ExternalStimuli.Stimulus In m_aryCompatibleStimuli
                    If Not doStimulus.WorkspaceImage Is Nothing Then
                        If doStimulus.WorkspaceImage.Width > iMaxWidth Then iMaxWidth = doStimulus.WorkspaceImage.Width
                        If doStimulus.WorkspaceImage.Height > iMaxHeight Then iMaxHeight = doStimulus.WorkspaceImage.Height
                    End If
                Next

                m_mgrIconImages.ImageList.ImageSize = New Size(iMaxWidth, iMaxHeight)

                m_mgrIconImages.AddImage("DefaultStimulus", imgDefault)

                'Now lets go through and add create the image list
                For Each doStimulus As DataObjects.ExternalStimuli.Stimulus In m_aryCompatibleStimuli
                    If Not doStimulus.WorkspaceImage Is Nothing Then
                        m_mgrIconImages.AddImage(doStimulus.WorkspaceImageName, doStimulus.WorkspaceImage)
                    End If
                Next

                ctrlStimulusTypes.LargeImageList = m_mgrIconImages.ImageList

                'Now lets go through and add the link types to the list
                For Each doStimulus As DataObjects.ExternalStimuli.Stimulus In m_aryCompatibleStimuli
                    Dim liItem As New ListViewItem
                    liItem.Text = doStimulus.TypeName

                    If Not doStimulus.WorkspaceImage Is Nothing Then
                        liItem.ImageIndex = m_mgrIconImages.GetImageIndex(doStimulus.WorkspaceImageName)
                    Else
                        liItem.ImageIndex = m_mgrIconImages.GetImageIndex("DefaultStimulus")
                    End If

                    liItem.Tag = doStimulus
                    ctrlStimulusTypes.Items.Add(liItem)
                Next

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
            MyBase.OnResize(e)

            Try
                lblDescription.Left = 8
                lblDescription.Top = (ctrlStimulusTypes.Top + ctrlStimulusTypes.Height + 5)
                lblDescription.Width = ctrlStimulusTypes.Width

                btnOk.Left = CInt((Me.Width / 2) - btnOk.Width - 2)
                btnCancel.Left = CInt((Me.Width / 2) + 2)

            Catch ex As System.Exception

            End Try
        End Sub

        Private Sub ctrlLinkTypes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ctrlStimulusTypes.Click

            Try

                If ctrlStimulusTypes.SelectedItems.Count = 1 Then
                    Dim liItem As ListViewItem = ctrlStimulusTypes.SelectedItems(0)

                    lblDescription.Clear()
                    lblDescription.SelectionFont = New Font("Arial", 10, FontStyle.Bold)

                    Dim doStimulus As DataObjects.ExternalStimuli.Stimulus = DirectCast(liItem.Tag, DataObjects.ExternalStimuli.Stimulus)
                    lblDescription.AppendText(doStimulus.TypeName & vbCrLf)

                    lblDescription.SelectionFont = New Font("Arial", 10)
                    lblDescription.AppendText(doStimulus.Description)

                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub ctrlLinkTypes_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ctrlStimulusTypes.DoubleClick
            btnOk_Click(sender, e)

            If Me.DialogResult = DialogResult.OK Then
                Me.Close()
            End If
        End Sub

        Private Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
            Try
                If ctrlStimulusTypes.SelectedItems.Count <> 1 Then
                    Throw New System.Exception("You must select a stimulus type or hit cancel.")
                End If

                Dim liItem As ListViewItem = ctrlStimulusTypes.SelectedItems(0)
                m_doSelectedStimulus = DirectCast(liItem.Tag, DataObjects.ExternalStimuli.Stimulus)

                Me.DialogResult = DialogResult.OK
                Me.Close()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace


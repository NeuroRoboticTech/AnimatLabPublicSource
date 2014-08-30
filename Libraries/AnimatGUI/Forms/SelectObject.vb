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
Imports AnimatGUI.Collections

Namespace Forms

    Public Class SelectObject
        Inherits Forms.AnimatDialog

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
        Friend WithEvents ctrlObjectTypes As System.Windows.Forms.ListView
        Friend WithEvents chStimulusType As System.Windows.Forms.ColumnHeader
        Friend WithEvents btnOk As System.Windows.Forms.Button
        Friend WithEvents btnCancel As System.Windows.Forms.Button
        Friend WithEvents lblDescription As System.Windows.Forms.Label
        Friend WithEvents chStimulusDescription As System.Windows.Forms.ColumnHeader
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.ctrlObjectTypes = New System.Windows.Forms.ListView()
            Me.chStimulusType = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.chStimulusDescription = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.btnOk = New System.Windows.Forms.Button()
            Me.btnCancel = New System.Windows.Forms.Button()
            Me.lblDescription = New System.Windows.Forms.Label()
            Me.SuspendLayout()
            '
            'ctrlObjectTypes
            '
            Me.ctrlObjectTypes.Activation = System.Windows.Forms.ItemActivation.OneClick
            Me.ctrlObjectTypes.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.ctrlObjectTypes.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chStimulusType, Me.chStimulusDescription})
            Me.ctrlObjectTypes.Location = New System.Drawing.Point(8, 7)
            Me.ctrlObjectTypes.MultiSelect = False
            Me.ctrlObjectTypes.Name = "ctrlObjectTypes"
            Me.ctrlObjectTypes.Size = New System.Drawing.Size(418, 166)
            Me.ctrlObjectTypes.Sorting = System.Windows.Forms.SortOrder.Ascending
            Me.ctrlObjectTypes.TabIndex = 9
            Me.ctrlObjectTypes.UseCompatibleStateImageBehavior = False
            '
            'chStimulusType
            '
            Me.chStimulusType.Text = "Stimulus Type"
            '
            'chStimulusDescription
            '
            Me.chStimulusDescription.Text = "Description"
            '
            'btnOk
            '
            Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnOk.Location = New System.Drawing.Point(323, 179)
            Me.btnOk.Name = "btnOk"
            Me.btnOk.Size = New System.Drawing.Size(73, 24)
            Me.btnOk.TabIndex = 14
            Me.btnOk.Text = "Ok"
            Me.btnOk.UseVisualStyleBackColor = True
            '
            'btnCancel
            '
            Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(323, 209)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(75, 23)
            Me.btnCancel.TabIndex = 15
            Me.btnCancel.Text = "Cancel"
            Me.btnCancel.UseVisualStyleBackColor = True
            '
            'lblDescription
            '
            Me.lblDescription.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblDescription.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.lblDescription.Location = New System.Drawing.Point(8, 179)
            Me.lblDescription.Name = "lblDescription"
            Me.lblDescription.Size = New System.Drawing.Size(309, 48)
            Me.lblDescription.TabIndex = 16
            '
            'SelectObject
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(434, 236)
            Me.Controls.Add(Me.lblDescription)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnOk)
            Me.Controls.Add(Me.ctrlObjectTypes)
            Me.MinimumSize = New System.Drawing.Size(450, 250)
            Me.Name = "SelectObject"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Select Object"
            Me.ResumeLayout(False)

        End Sub

#End Region


#Region " Attributes "

        Protected m_doSelected As Framework.DataObject
        Protected m_mgrIconImages As AnimatGUI.Framework.ImageManager
        Protected m_aryObjects As AnimatCollectionBase
        Protected m_strPartTypeName As String = ""

#End Region

#Region " Properties "

        Public Property Selected() As Framework.DataObject
            Get
                Return m_doSelected
            End Get
            Set(ByVal Value As Framework.DataObject)
                m_doSelected = Value
            End Set
        End Property

        Public Property PartTypeName() As String
            Get
                Return m_strPartTypeName
            End Get
            Set(ByVal Value As String)
                m_strPartTypeName = Value
            End Set
        End Property

        Public Property Objects() As AnimatCollectionBase
            Get
                Return m_aryObjects
            End Get
            Set(ByVal Value As AnimatCollectionBase)
                m_aryObjects = Value
            End Set
        End Property


#End Region

#Region " Methods "

#End Region

#Region " Events "

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            MyBase.OnLoad(e)

            Try
                m_btnOk = Me.btnOk
                m_btnCancel = Me.btnCancel
                Me.m_lvItems = Me.ctrlObjectTypes

                Dim myAssembly As System.Reflection.Assembly
                myAssembly = System.Reflection.Assembly.Load("AnimatGUI")

                Dim imgDefault As Image = ImageManager.LoadImage(myAssembly, "AnimatGUI.DefaultObject.gif")
                m_mgrIconImages = New AnimatGUI.Framework.ImageManager

                m_mgrIconImages.AddImage("Default", imgDefault)

                Me.Text = "Select " & m_strPartTypeName

                Dim iMaxWidth As Integer = imgDefault.Width
                Dim iMaxHeight As Integer = imgDefault.Height
                Dim aryList As New ArrayList
                For Each doObject As Framework.DataObject In m_aryObjects
                    If doObject.AllowUserAdd Then
                        If Not doObject.ButtonImage Is Nothing Then
                            m_mgrIconImages.AddImage(doObject.ButtonImageName, doObject.ButtonImage)
                            If doObject.ButtonImage.Width > iMaxWidth Then iMaxWidth = doObject.ButtonImage.Width
                            If doObject.ButtonImage.Height > iMaxHeight Then iMaxHeight = doObject.ButtonImage.Height
                        End If

                        Dim liItem As New ListViewItem
                        liItem.Text = doObject.Name

                        If Not doObject.ButtonImage Is Nothing Then
                            liItem.ImageIndex = m_mgrIconImages.GetImageIndex(doObject.ButtonImageName)
                        Else
                            liItem.ImageIndex = m_mgrIconImages.GetImageIndex("Default")
                        End If

                        liItem.Tag = doObject
                        aryList.Add(liItem)
                    End If
                Next

                'I have to sort the list like this because when I set the large imag list next it resorts it badly. So I 
                'to reset the ListViewItemSorter to nothing before adding the list items.
                aryList.Sort(New ListViewItemComparer)

                m_mgrIconImages.ImageList.ImageSize = New Size(iMaxWidth, iMaxHeight)
                ctrlObjectTypes.LargeImageList = m_mgrIconImages.ImageList

                ctrlObjectTypes.ListViewItemSorter = Nothing
                For Each liItem As ListViewItem In aryList
                    ctrlObjectTypes.Items.Add(liItem)
                Next

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub ctrlLinkTypes_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ctrlObjectTypes.DoubleClick
            btnOk_Click(sender, e)

            If Me.DialogResult = DialogResult.OK Then
                Me.Close()
            End If
        End Sub


        Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
            Try
                If ctrlObjectTypes.SelectedItems.Count <> 1 Then
                    Throw New System.Exception("You must select an object type or hit cancel.")
                End If

                Dim liItem As ListViewItem = ctrlObjectTypes.SelectedItems(0)
                m_doSelected = DirectCast(liItem.Tag, Framework.DataObject)

                Me.DialogResult = DialogResult.OK
                Me.Close()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub


        Private Sub ctrlObjectTypes_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ctrlObjectTypes.SelectedIndexChanged
            Try
                If ctrlObjectTypes.SelectedItems.Count = 1 Then
                    Dim doItem As Framework.DataObject = DirectCast(ctrlObjectTypes.SelectedItems(0).Tag, Framework.DataObject)
                    lblDescription.Text = doItem.Description
                Else
                    lblDescription.Text = ""
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

#Region " Comparers "

        Class ListViewItemComparer
            Implements IComparer

            Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
                Dim iRetVal As Integer = [String].Compare(CType(x, ListViewItem).Text, CType(y, ListViewItem).Text)
                Return iRetVal
            End Function
        End Class

#End Region

#Region "Automation Methods"

#End Region

    End Class

End Namespace


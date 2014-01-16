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

Namespace Forms.BodyPlan

    Public Class SelectPartType
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
        Friend WithEvents ctrlPartTypes As System.Windows.Forms.ListView
        Friend WithEvents chStimulusType As System.Windows.Forms.ColumnHeader
        Friend WithEvents chkIsSensor As System.Windows.Forms.CheckBox
        Friend WithEvents chkAddGraphics As System.Windows.Forms.CheckBox
        Friend WithEvents btnOk As System.Windows.Forms.Button
        Friend WithEvents btnCancel As System.Windows.Forms.Button
        Friend WithEvents rdCollision As System.Windows.Forms.RadioButton
        Friend WithEvents rdGraphics As System.Windows.Forms.RadioButton
        Friend WithEvents chStimulusDescription As System.Windows.Forms.ColumnHeader
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.ctrlPartTypes = New System.Windows.Forms.ListView()
            Me.chStimulusType = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.chStimulusDescription = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.chkIsSensor = New System.Windows.Forms.CheckBox()
            Me.chkAddGraphics = New System.Windows.Forms.CheckBox()
            Me.btnOk = New System.Windows.Forms.Button()
            Me.btnCancel = New System.Windows.Forms.Button()
            Me.rdCollision = New System.Windows.Forms.RadioButton()
            Me.rdGraphics = New System.Windows.Forms.RadioButton()
            Me.SuspendLayout()
            '
            'ctrlPartTypes
            '
            Me.ctrlPartTypes.Activation = System.Windows.Forms.ItemActivation.OneClick
            Me.ctrlPartTypes.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.ctrlPartTypes.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chStimulusType, Me.chStimulusDescription})
            Me.ctrlPartTypes.Location = New System.Drawing.Point(8, 7)
            Me.ctrlPartTypes.MultiSelect = False
            Me.ctrlPartTypes.Name = "ctrlPartTypes"
            Me.ctrlPartTypes.Size = New System.Drawing.Size(418, 166)
            Me.ctrlPartTypes.Sorting = System.Windows.Forms.SortOrder.Ascending
            Me.ctrlPartTypes.TabIndex = 9
            Me.ctrlPartTypes.UseCompatibleStateImageBehavior = False
            '
            'chStimulusType
            '
            Me.chStimulusType.Text = "Stimulus Type"
            '
            'chStimulusDescription
            '
            Me.chStimulusDescription.Text = "Description"
            '
            'chkIsSensor
            '
            Me.chkIsSensor.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.chkIsSensor.AutoSize = True
            Me.chkIsSensor.Location = New System.Drawing.Point(142, 207)
            Me.chkIsSensor.Name = "chkIsSensor"
            Me.chkIsSensor.Size = New System.Drawing.Size(89, 17)
            Me.chkIsSensor.TabIndex = 12
            Me.chkIsSensor.Text = "Make Sensor"
            Me.chkIsSensor.UseVisualStyleBackColor = True
            '
            'chkAddGraphics
            '
            Me.chkAddGraphics.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.chkAddGraphics.AutoSize = True
            Me.chkAddGraphics.Location = New System.Drawing.Point(8, 207)
            Me.chkAddGraphics.Name = "chkAddGraphics"
            Me.chkAddGraphics.Size = New System.Drawing.Size(127, 17)
            Me.chkAddGraphics.TabIndex = 13
            Me.chkAddGraphics.Text = "Add Default Graphics"
            Me.chkAddGraphics.UseVisualStyleBackColor = True
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
            'rdCollision
            '
            Me.rdCollision.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.rdCollision.AutoSize = True
            Me.rdCollision.Location = New System.Drawing.Point(8, 179)
            Me.rdCollision.Name = "rdCollision"
            Me.rdCollision.Size = New System.Drawing.Size(97, 17)
            Me.rdCollision.TabIndex = 16
            Me.rdCollision.Text = "Collision Object"
            Me.rdCollision.UseVisualStyleBackColor = True
            '
            'rdGraphics
            '
            Me.rdGraphics.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.rdGraphics.AutoSize = True
            Me.rdGraphics.Location = New System.Drawing.Point(142, 179)
            Me.rdGraphics.Name = "rdGraphics"
            Me.rdGraphics.Size = New System.Drawing.Size(101, 17)
            Me.rdGraphics.TabIndex = 17
            Me.rdGraphics.Text = "Graphics Object"
            Me.rdGraphics.UseVisualStyleBackColor = True
            '
            'SelectPartType
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(434, 236)
            Me.Controls.Add(Me.rdGraphics)
            Me.Controls.Add(Me.rdCollision)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnOk)
            Me.Controls.Add(Me.chkAddGraphics)
            Me.Controls.Add(Me.chkIsSensor)
            Me.Controls.Add(Me.ctrlPartTypes)
            Me.MinimumSize = New System.Drawing.Size(450, 250)
            Me.Name = "SelectPartType"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Select Part Type"
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region


#Region " Attributes "

        Protected m_doSelectedPart As DataObjects.Physical.BodyPart
        Protected m_mgrIconImages As AnimatGUI.Framework.ImageManager
        Protected m_tpPartType As System.Type
        Protected m_bIsRoot As Boolean = False
        Protected m_bIsRigidBody As Boolean = False
        Protected m_bHasDynamics As Boolean = True
        Protected m_bDefaultAddGraphics As Boolean = True
        Protected m_rbParentBody As DataObjects.Physical.RigidBody
        Protected m_rbChildBody As DataObjects.Physical.RigidBody

#End Region

#Region " Properties "

        Public Property SelectedPart() As DataObjects.Physical.BodyPart
            Get
                Return m_doSelectedPart
            End Get
            Set(ByVal Value As DataObjects.Physical.BodyPart)
                m_doSelectedPart = Value
            End Set
        End Property

        Public Property PartType() As System.Type
            Get
                Return m_tpPartType
            End Get
            Set(ByVal value As System.Type)
                m_tpPartType = value
            End Set
        End Property

        Public Property IsRoot() As Boolean
            Get
                Return m_bIsRoot
            End Get
            Set(ByVal value As Boolean)
                m_bIsRoot = value
            End Set
        End Property

        Public Property ParentBody() As DataObjects.Physical.RigidBody
            Get
                Return m_rbParentBody
            End Get
            Set(ByVal value As DataObjects.Physical.RigidBody)
                m_rbParentBody = value
            End Set
        End Property

        Public Property ChildBody() As DataObjects.Physical.RigidBody
            Get
                Return m_rbChildBody
            End Get
            Set(ByVal value As DataObjects.Physical.RigidBody)
                m_rbChildBody = value
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
                Me.m_lvItems = Me.ctrlPartTypes

                Dim myAssembly As System.Reflection.Assembly
                myAssembly = System.Reflection.Assembly.Load("AnimatGUI")

                Dim imgDefault As Image = ImageManager.LoadImage(myAssembly, "AnimatGUI.DefaultObject.gif")
                m_mgrIconImages = New AnimatGUI.Framework.ImageManager

                m_mgrIconImages.AddImage("Default", imgDefault)

                m_bIsRigidBody = Util.IsTypeOf(m_tpPartType, GetType(Physical.RigidBody), False)

                If m_bIsRigidBody Then
                    Me.Text = "Select Rigid Body Type"
                Else
                    Me.Text = "Select Joint Type"
                End If

                Dim iMaxWidth As Integer = imgDefault.Width
                Dim iMaxHeight As Integer = imgDefault.Height
                Dim aryList As New ArrayList
                For Each doPart As Physical.BodyPart In Util.Application.BodyPartTypes
                    If Util.IsTypeOf(doPart.GetType(), m_tpPartType, False) AndAlso Not doPart.GetType().IsAbstract AndAlso doPart.AllowUserAdd Then
                        If Not m_bIsRigidBody OrElse (m_bIsRigidBody AndAlso Not m_bIsRoot) OrElse (m_bIsRigidBody AndAlso m_bIsRoot AndAlso doPart.HasDynamics) Then
                            If m_rbParentBody Is Nothing OrElse Util.Application.CanAddPartAsChild(m_rbParentBody.GetType, doPart.GetType) Then
                                If Util.IsTypeOf(m_tpPartType, GetType(Physical.RigidBody)) OrElse (Util.IsTypeOf(m_tpPartType, GetType(Physical.Joint)) AndAlso _
                                                                                                    Not m_rbChildBody Is Nothing AndAlso _
                                                                                                    Util.Application.CanAddPartAsChild(m_rbChildBody.GetType, doPart.GetType)) Then
                                    If Not doPart.ButtonImage Is Nothing Then
                                        m_mgrIconImages.AddImage(doPart.ButtonImageName, doPart.ButtonImage)
                                        If doPart.ButtonImage.Width > iMaxWidth Then iMaxWidth = doPart.ButtonImage.Width
                                        If doPart.ButtonImage.Height > iMaxHeight Then iMaxHeight = doPart.ButtonImage.Height
                                    End If

                                    Dim liItem As New ListViewItem
                                    liItem.Text = doPart.BodyPartName

                                    If Not doPart.ButtonImage Is Nothing Then
                                        liItem.ImageIndex = m_mgrIconImages.GetImageIndex(doPart.ButtonImageName)
                                    Else
                                        liItem.ImageIndex = m_mgrIconImages.GetImageIndex("Default")
                                    End If

                                    liItem.Tag = doPart
                                    aryList.Add(liItem)
                                End If
                            End If
                        End If
                    End If
                Next

                'I have to sort the list like this because when I set the large imag list next it resorts it badly. So I 
                'to reset the ListViewItemSorter to nothing before adding the list items.
                aryList.Sort(New ListViewItemComparer)

                m_mgrIconImages.ImageList.ImageSize = New Size(iMaxWidth, iMaxHeight)
                ctrlPartTypes.LargeImageList = m_mgrIconImages.ImageList

                ctrlPartTypes.ListViewItemSorter = Nothing
                For Each liItem As ListViewItem In aryList
                    ctrlPartTypes.Items.Add(liItem)
                Next

                SetOptions(True)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub SetJointOptions(ByVal bInit As Boolean)
            'If it is a joint then do this.
            rdCollision.Enabled = False
            rdCollision.Visible = False
            rdGraphics.Enabled = False
            rdGraphics.Visible = False
            chkAddGraphics.Enabled = False
            chkAddGraphics.Visible = False
            chkIsSensor.Enabled = False
            chkIsSensor.Visible = False
        End Sub

        Protected Sub SetRigidBodyOptions(ByVal bInit As Boolean)

            rdCollision.Visible = True
            rdGraphics.Visible = True
            chkAddGraphics.Visible = True
            chkIsSensor.Visible = True
            chkAddGraphics.Checked = m_bDefaultAddGraphics
            rdCollision.Checked = m_bHasDynamics
            rdGraphics.Checked = Not m_bHasDynamics

            If m_bIsRoot Then
                rdCollision.Enabled = False
                rdGraphics.Enabled = False
                chkAddGraphics.Enabled = True
                chkIsSensor.Enabled = False
                If bInit Then chkIsSensor.Checked = False
            Else
                rdCollision.Enabled = m_bHasDynamics
                rdGraphics.Enabled = m_bHasDynamics
                chkAddGraphics.Enabled = m_bHasDynamics
                chkIsSensor.Enabled = rdCollision.Checked
                If bInit AndAlso m_bHasDynamics Then chkIsSensor.Checked = False
            End If

        End Sub

        Protected Sub SetRigidBodyOptions(ByVal bInit As Boolean, ByVal bpBody As Physical.RigidBody)
            If Not bpBody Is Nothing Then
                m_bHasDynamics = bpBody.HasDynamics
                m_bDefaultAddGraphics = bpBody.DefaultAddGraphics
            End If

            SetRigidBodyOptions(bInit)
        End Sub

        Protected Sub SetOptions(ByVal bInit As Boolean, Optional ByVal bpBody As Physical.RigidBody = Nothing)

            If Not m_bIsRigidBody Then
                SetJointOptions(bInit)
            Else
                SetRigidBodyOptions(bInit, bpBody)
            End If

        End Sub

        Private Sub ctrlLinkTypes_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ctrlPartTypes.DoubleClick
            btnOk_Click(sender, e)

            If Me.DialogResult = DialogResult.OK Then
                Me.Close()
            End If
        End Sub


        Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
            Try
                If ctrlPartTypes.SelectedItems.Count <> 1 Then
                    Throw New System.Exception("You must select a stimulus type or hit cancel.")
                End If

                Dim liItem As ListViewItem = ctrlPartTypes.SelectedItems(0)
                m_doSelectedPart = DirectCast(liItem.Tag, Physical.BodyPart)

                Me.DialogResult = DialogResult.OK
                Me.Close()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub rdCollision_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdCollision.CheckedChanged
            Try
                If rdCollision.Checked Then
                    chkIsSensor.Checked = False
                    chkIsSensor.Enabled = True
                    chkAddGraphics.Enabled = True
                Else
                    chkAddGraphics.Checked = False
                    chkAddGraphics.Enabled = False
                    chkIsSensor.Enabled = False
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub ctrlPartTypes_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ctrlPartTypes.SelectedIndexChanged
            Try
                If ctrlPartTypes.SelectedItems.Count > 0 Then
                    Dim liItem As ListViewItem = ctrlPartTypes.SelectedItems(0)

                    If Not liItem.Tag Is Nothing AndAlso Util.IsTypeOf(liItem.Tag.GetType(), GetType(Physical.RigidBody), False) Then
                        Dim bpBody As Physical.RigidBody = DirectCast(liItem.Tag, Physical.RigidBody)
                        SetOptions(False, bpBody)
                    End If
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub chkIsSensor_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkIsSensor.CheckedChanged
            Try
                If chkIsSensor.Checked Then
                    rdCollision.Checked = True
                    rdCollision.Enabled = False
                    rdGraphics.Enabled = False
                    chkAddGraphics.Enabled = True
                Else
                    rdGraphics.Enabled = True
                    rdCollision.Enabled = True
                    chkAddGraphics.Enabled = True
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

        Public Sub Click_GraphicsCheckBox(ByVal bCheckedState As Boolean)
            chkAddGraphics.Checked = bCheckedState
        End Sub

#End Region

    End Class

End Namespace


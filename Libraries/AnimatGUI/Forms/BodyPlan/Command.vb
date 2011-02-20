Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports Crownwood.Magic.Common
Imports Crownwood.Magic.Controls
Imports Crownwood.Magic.Docking
Imports Crownwood.Magic.Menus
Imports AnimatTools
Imports AnimatTools.Framework
Imports AnimatTools.DataObjects
Imports System.Drawing.Imaging

Namespace Forms.BodyPlan

    Public Class Command
        Inherits AnimatForm

#Region " Windows Form Designer generated code "

        Public Sub New()
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            Me.ttSelectBodies.SetToolTip(Me.btnSelectBodies, "Select Bodies Mode")
            Me.ttSelectJoints.SetToolTip(Me.btnSelectJoints, "Select Joints Mode")
            Me.ttAddBodies.SetToolTip(Me.btnAddBody, "Add Bodies Mode")
            Me.ttReceptiveFields.SetToolTip(Me.btnSelectReceptiveFields, "Select Receptive Fields")
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
        Friend WithEvents ttSelectBodies As System.Windows.Forms.ToolTip
        Friend WithEvents ttSelectJoints As System.Windows.Forms.ToolTip
        Friend WithEvents ttReceptiveFields As System.Windows.Forms.ToolTip
        Friend WithEvents ttAddBodies As System.Windows.Forms.ToolTip
        Friend WithEvents txtSensitivity As System.Windows.Forms.TextBox
        Friend WithEvents lblSensitivity As System.Windows.Forms.Label
        Friend WithEvents btnAddBody As System.Windows.Forms.Button
        Friend WithEvents btnSelectJoints As System.Windows.Forms.Button
        Friend WithEvents btnSelectBodies As System.Windows.Forms.Button
        Friend WithEvents cbJointType As System.Windows.Forms.ImageCombo
        Friend WithEvents cbPartType As System.Windows.Forms.ImageCombo
        Friend WithEvents lblDefaultPart As System.Windows.Forms.Label
        Friend WithEvents lblDefaultJoint As System.Windows.Forms.Label
        Friend WithEvents btnSelectReceptiveFields As System.Windows.Forms.Button
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(Command))
            Me.ttSelectBodies = New System.Windows.Forms.ToolTip(Me.components)
            Me.ttSelectJoints = New System.Windows.Forms.ToolTip(Me.components)
            Me.ttAddBodies = New System.Windows.Forms.ToolTip(Me.components)
            Me.ttReceptiveFields = New System.Windows.Forms.ToolTip(Me.components)
            Me.txtSensitivity = New System.Windows.Forms.TextBox
            Me.lblSensitivity = New System.Windows.Forms.Label
            Me.btnAddBody = New System.Windows.Forms.Button
            Me.btnSelectJoints = New System.Windows.Forms.Button
            Me.btnSelectBodies = New System.Windows.Forms.Button
            Me.cbJointType = New System.Windows.Forms.ImageCombo
            Me.cbPartType = New System.Windows.Forms.ImageCombo
            Me.lblDefaultPart = New System.Windows.Forms.Label
            Me.lblDefaultJoint = New System.Windows.Forms.Label
            Me.btnSelectReceptiveFields = New System.Windows.Forms.Button
            Me.SuspendLayout()
            '
            'txtSensitivity
            '
            Me.txtSensitivity.Location = New System.Drawing.Point(192, 24)
            Me.txtSensitivity.Name = "txtSensitivity"
            Me.txtSensitivity.Size = New System.Drawing.Size(56, 20)
            Me.txtSensitivity.TabIndex = 10
            Me.txtSensitivity.Text = ""
            '
            'lblSensitivity
            '
            Me.lblSensitivity.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblSensitivity.Location = New System.Drawing.Point(192, 0)
            Me.lblSensitivity.Name = "lblSensitivity"
            Me.lblSensitivity.Size = New System.Drawing.Size(56, 24)
            Me.lblSensitivity.TabIndex = 9
            Me.lblSensitivity.Text = "Mouse Sensitivity"
            Me.lblSensitivity.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'btnAddBody
            '
            Me.btnAddBody.Image = CType(resources.GetObject("btnAddBody.WorkspaceImage"), System.Drawing.Image)
            Me.btnAddBody.Location = New System.Drawing.Point(96, 0)
            Me.btnAddBody.Name = "btnAddBody"
            Me.btnAddBody.Size = New System.Drawing.Size(45, 45)
            Me.btnAddBody.TabIndex = 8
            '
            'btnSelectJoints
            '
            Me.btnSelectJoints.Image = CType(resources.GetObject("btnSelectJoints.WorkspaceImage"), System.Drawing.Image)
            Me.btnSelectJoints.Location = New System.Drawing.Point(48, 0)
            Me.btnSelectJoints.Name = "btnSelectJoints"
            Me.btnSelectJoints.Size = New System.Drawing.Size(45, 45)
            Me.btnSelectJoints.TabIndex = 7
            '
            'btnSelectBodies
            '
            Me.btnSelectBodies.Image = CType(resources.GetObject("btnSelectBodies.WorkspaceImage"), System.Drawing.Image)
            Me.btnSelectBodies.Location = New System.Drawing.Point(0, 0)
            Me.btnSelectBodies.Name = "btnSelectBodies"
            Me.btnSelectBodies.Size = New System.Drawing.Size(45, 45)
            Me.btnSelectBodies.TabIndex = 6
            '
            'cbJointType
            '
            Me.cbJointType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
            Me.cbJointType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cbJointType.ItemHeight = 25
            Me.cbJointType.Location = New System.Drawing.Point(416, 16)
            Me.cbJointType.Name = "cbJointType"
            Me.cbJointType.Size = New System.Drawing.Size(250, 31)
            Me.cbJointType.Sorted = True
            Me.cbJointType.TabIndex = 12
            '
            'cbPartType
            '
            Me.cbPartType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
            Me.cbPartType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cbPartType.ItemHeight = 25
            Me.cbPartType.Location = New System.Drawing.Point(256, 16)
            Me.cbPartType.Name = "cbPartType"
            Me.cbPartType.Size = New System.Drawing.Size(250, 31)
            Me.cbPartType.Sorted = True
            Me.cbPartType.TabIndex = 11
            '
            'lblDefaultPart
            '
            Me.lblDefaultPart.Location = New System.Drawing.Point(256, 0)
            Me.lblDefaultPart.Name = "lblDefaultPart"
            Me.lblDefaultPart.Size = New System.Drawing.Size(250, 16)
            Me.lblDefaultPart.TabIndex = 13
            Me.lblDefaultPart.Text = "Default Body Part Type"
            Me.lblDefaultPart.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'lblDefaultJoint
            '
            Me.lblDefaultJoint.Location = New System.Drawing.Point(416, 0)
            Me.lblDefaultJoint.Name = "lblDefaultJoint"
            Me.lblDefaultJoint.Size = New System.Drawing.Size(250, 16)
            Me.lblDefaultJoint.TabIndex = 14
            Me.lblDefaultJoint.Text = "Default Joint Type"
            Me.lblDefaultJoint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'btnSelectReceptiveFields
            '
            Me.btnSelectReceptiveFields.Image = CType(resources.GetObject("btnSelectReceptiveFields.WorkspaceImage"), System.Drawing.Image)
            Me.btnSelectReceptiveFields.Location = New System.Drawing.Point(144, 0)
            Me.btnSelectReceptiveFields.Name = "btnSelectReceptiveFields"
            Me.btnSelectReceptiveFields.Size = New System.Drawing.Size(45, 45)
            Me.btnSelectReceptiveFields.TabIndex = 15
            Me.btnSelectReceptiveFields.Visible = False
            '
            'Command
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.AutoScroll = True
            Me.ClientSize = New System.Drawing.Size(656, 54)
            Me.Controls.Add(Me.btnSelectReceptiveFields)
            Me.Controls.Add(Me.lblDefaultJoint)
            Me.Controls.Add(Me.lblDefaultPart)
            Me.Controls.Add(Me.cbJointType)
            Me.Controls.Add(Me.cbPartType)
            Me.Controls.Add(Me.txtSensitivity)
            Me.Controls.Add(Me.lblSensitivity)
            Me.Controls.Add(Me.btnAddBody)
            Me.Controls.Add(Me.btnSelectJoints)
            Me.Controls.Add(Me.btnSelectBodies)
            Me.Name = "Command"
            Me.Text = "Command"
            Me.ResumeLayout(False)

        End Sub

#End Region

#Region " Enums "

        Public Enum enumCommandMode
            SelectBodies
            SelectJoints
            SelectReceptiveFields
            AddBodies
            PasteBodies
        End Enum

#End Region

#Region " Attributes "

        Protected m_beEditor As AnimatTools.Forms.BodyPlan.Editor

        Protected m_doSelectedBodyType As DataObjects.Physical.RigidBody
        Protected m_doSelectedJointType As DataObjects.Physical.Joint

        Protected m_eCommandMode As enumCommandMode
        Protected m_btnSelectedMode As System.Windows.Forms.Button

        Protected m_imgButtonManager As AnimatTools.Framework.ImageManager

#End Region

#Region " Properties "

        Public Overridable ReadOnly Property ButtonImageManager() As AnimatTools.Framework.ImageManager
            Get
                Return m_imgButtonManager
            End Get
        End Property

        Public Overridable Property Editor() As Forms.BodyPlan.Editor
            Get
                Return m_beEditor
            End Get
            Set(ByVal Value As Forms.BodyPlan.Editor)
                m_beEditor = Value
            End Set
        End Property

        Public Overridable Property SelectedBodyPartType() As AnimatTools.DataObjects.Physical.RigidBody
            Get
                Return m_doSelectedBodyType
            End Get
            Set(ByVal Value As AnimatTools.DataObjects.Physical.RigidBody)
                If Not Value Is Nothing Then
                    m_doSelectedBodyType = Value
                End If
            End Set
        End Property

        Public Overridable Property SelectedJointType() As AnimatTools.DataObjects.Physical.Joint
            Get
                Return m_doSelectedJointType
            End Get
            Set(ByVal Value As AnimatTools.DataObjects.Physical.Joint)
                m_doSelectedJointType = Value
            End Set
        End Property

        Public Overridable Property CommandMode() As enumCommandMode
            Get
                Return m_eCommandMode
            End Get
            Set(ByVal Value As enumCommandMode)

                If Not m_btnSelectedMode Is Nothing Then
                    m_btnSelectedMode.BackColor = System.Drawing.Color.FromArgb(236, 233, 216)
                End If

                m_eCommandMode = Value

                If m_eCommandMode = enumCommandMode.SelectBodies Then
                    m_btnSelectedMode = btnSelectBodies
                ElseIf m_eCommandMode = enumCommandMode.SelectJoints Then
                    m_btnSelectedMode = btnSelectJoints
                ElseIf m_eCommandMode = enumCommandMode.SelectReceptiveFields Then
                    m_btnSelectedMode = btnSelectReceptiveFields
                Else
                    m_btnSelectedMode = btnAddBody
                End If

                If Not m_btnSelectedMode Is Nothing Then
                    m_btnSelectedMode.BackColor = System.Drawing.Color.CornflowerBlue
                End If

                If Not m_beEditor Is Nothing AndAlso Not m_beEditor.SelectBodiesMenuItem Is Nothing AndAlso _
                   Not m_beEditor.SelectJointsMenuItem Is Nothing AndAlso Not m_beEditor.AddBodyMenuItem Is Nothing Then
                    m_beEditor.SelectBodiesMenuItem.Checked = False
                    m_beEditor.SelectJointsMenuItem.Checked = False
                    m_beEditor.AddBodyMenuItem.Checked = False
                    m_beEditor.SelectReceptiveFieldsMenuItem.Checked = False

                    If m_eCommandMode = enumCommandMode.SelectBodies Then
                        m_beEditor.SelectBodiesMenuItem.Checked = True
                    ElseIf m_eCommandMode = enumCommandMode.SelectJoints Then
                        m_beEditor.SelectJointsMenuItem.Checked = True
                    ElseIf m_eCommandMode = enumCommandMode.SelectReceptiveFields Then
                        m_beEditor.SelectReceptiveFieldsMenuItem.Checked = True
                    Else
                        m_beEditor.AddBodyMenuItem.Checked = True
                    End If
                End If

                OnCommandModeChanged()
            End Set
        End Property

        Public Overridable ReadOnly Property InAddBodyMode() As Boolean
            Get
                If Me.CommandMode = enumCommandMode.AddBodies OrElse Me.CommandMode = enumCommandMode.PasteBodies Then
                    Return True
                Else
                    Return False
                End If
            End Get
        End Property

#End Region

#Region " Command Events "

        Public Event CommandModeChanged(ByVal eNewMode As enumCommandMode)

#End Region

#Region " Methods "

        Public Overrides Sub Initialize(Optional ByVal frmParent As AnimatForm = Nothing)
            Try

                MyBase.Initialize(frmParent)

                'm_beEditor = DirectCast(frmMdiParent, AnimatTools.Forms.BodyPlan.Editor)

                If Not Me.Editor Is Nothing Then
                    m_imgButtonManager = New ImageManager
                    m_imgButtonManager.ImageList.ImageSize = New Size(25, 25)
                    cbPartType.ImageList = m_imgButtonManager.ImageList
                    cbJointType.ImageList = m_imgButtonManager.ImageList

                    AddBodyParts(cbPartType, Util.Application.RigidBodyTypes, "Box")
                    AddBodyParts(cbJointType, Util.Application.JointTypes, "Hinge")
                End If

                Me.txtSensitivity.Text = m_beEditor.MouseSensitivity.ToString()

                Me.CommandMode = AnimatTools.Forms.BodyPlan.Command.enumCommandMode.SelectBodies

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Overridable Sub AddBodyParts(ByVal cbDropDown As System.Windows.Forms.ImageCombo, _
                                               ByVal colParts As AnimatTools.Collections.BodyParts, _
                                               ByVal strSelectPart As String)
            cbDropDown.Items.Clear()

            Dim ciItem As ImageComboItem
            For Each doBody As AnimatTools.DataObjects.Physical.BodyPart In colParts
                If doBody.AllowUserAdd Then
                    m_imgButtonManager.AddImage(doBody.ButtonImageName, doBody.ButtonImage)
                    ciItem = New ImageComboItem(doBody.BodyPartName, m_imgButtonManager.GetImageIndex(doBody.ButtonImageName))
                    ciItem.Tag = doBody
                    cbDropDown.Items.Add(ciItem)

                    If strSelectPart.Trim.ToUpper = doBody.BodyPartName.Trim.ToUpper Then
                        cbDropDown.SelectedItem = ciItem
                    End If
                End If
            Next

        End Sub

        Protected Overridable Sub OnCommandModeChanged()
            RaiseEvent CommandModeChanged(Me.CommandMode)
        End Sub

        Protected Overridable Sub HideCommandWindow()
            'If Not m_ctContent Is Nothing AndAlso Not m_ctContent.AutoHidePanel Is Nothing Then
            '    m_ctContent.AutoHidePanel.RestoreToHiddenState()
            'End If
        End Sub

#End Region

#Region " Events "

        Private Sub btnSelectBodies_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectBodies.Click
            Try
                Me.CommandMode = enumCommandMode.SelectBodies
                HideCommandWindow()
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub btnSelectJoints_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectJoints.Click
            Try
                Me.CommandMode = enumCommandMode.SelectJoints
                HideCommandWindow()
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub btnAddBody_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddBody.Click
            Try
                Me.CommandMode = enumCommandMode.AddBodies
                HideCommandWindow()
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub btnSelectReceptiveFields_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectReceptiveFields.Click
            Try
                Me.CommandMode = enumCommandMode.SelectReceptiveFields
                HideCommandWindow()
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub cbPartType_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbPartType.SelectedValueChanged

            Try
                If Not cbPartType.SelectedItem Is Nothing AndAlso TypeOf cbPartType.SelectedItem Is ImageComboItem Then
                    Dim ciItem As ImageComboItem = DirectCast(cbPartType.SelectedItem, ImageComboItem)

                    If Not ciItem.Tag Is Nothing AndAlso TypeOf ciItem.Tag Is AnimatTools.DataObjects.Physical.RigidBody Then
                        Dim doPart As AnimatTools.DataObjects.Physical.RigidBody = DirectCast(ciItem.Tag, AnimatTools.DataObjects.Physical.RigidBody)
                        Me.SelectedBodyPartType = doPart
                    End If
                End If

                HideCommandWindow()
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub cbJointType_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbJointType.SelectedValueChanged

            Try
                If Not cbJointType.SelectedItem Is Nothing AndAlso TypeOf cbJointType.SelectedItem Is ImageComboItem Then
                    Dim ciItem As ImageComboItem = DirectCast(cbJointType.SelectedItem, ImageComboItem)

                    If Not ciItem.Tag Is Nothing AndAlso TypeOf ciItem.Tag Is AnimatTools.DataObjects.Physical.Joint Then
                        Dim doPart As AnimatTools.DataObjects.Physical.Joint = DirectCast(ciItem.Tag, AnimatTools.DataObjects.Physical.Joint)
                        Me.SelectedJointType = doPart
                    End If
                End If

                HideCommandWindow()
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub txtSensitivity_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSensitivity.LostFocus

            Try
                If txtSensitivity.Text.Trim.Length = 0 Then
                    txtSensitivity.Text = m_beEditor.MouseSensitivity.ToString()
                    Return
                End If

                If Not IsNumeric(txtSensitivity.Text) Then
                    Throw New System.Exception("The mouse sensitivity setting must be a numeric value between 1e-5 and 100.")
                End If

                Dim fltVal As Single = CSng(txtSensitivity.Text)

                If fltVal < 0.00001 OrElse fltVal > 100 Then
                    Throw New System.Exception("The mouse sensitivity setting must be a numeric value between 1e-5 and 100.")
                End If

                m_beEditor.MouseSensitivity = fltVal

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
                txtSensitivity.Text = m_beEditor.MouseSensitivity.ToString()
            End Try

        End Sub

        Protected Overrides Sub OnResize(ByVal e As System.EventArgs)

            If Me.Width > Me.Height + 200 Then
                'Draw it laid out horizontally
                btnSelectBodies.Left = 1
                btnSelectBodies.Top = 1

                btnSelectJoints.Left = btnSelectBodies.Left + btnSelectBodies.Width + 1
                btnSelectJoints.Top = 1

                btnAddBody.Left = btnSelectJoints.Left + btnSelectJoints.Width + 1
                btnAddBody.Top = 1

                If Not Me.Editor Is Nothing AndAlso Not Me.Editor.PhysicalStructure Is Nothing AndAlso TypeOf Me.Editor.PhysicalStructure Is DataObjects.Physical.Organism Then
                    btnSelectReceptiveFields.Visible = True

                    btnSelectReceptiveFields.Left = btnAddBody.Left + btnAddBody.Width + 1
                    btnSelectReceptiveFields.Top = 1

                    lblSensitivity.Left = btnSelectReceptiveFields.Left + btnSelectReceptiveFields.Width + 3
                    lblSensitivity.Top = 1
                Else
                    btnSelectReceptiveFields.Visible = False

                    lblSensitivity.Left = btnAddBody.Left + btnAddBody.Width + 3
                    lblSensitivity.Top = 1
                End If

                txtSensitivity.Left = lblSensitivity.Left
                txtSensitivity.Top = lblSensitivity.Top + lblSensitivity.Height + 1

                lblDefaultPart.Left = txtSensitivity.Left + txtSensitivity.Width + 5
                lblDefaultPart.Top = 1

                cbPartType.Left = lblDefaultPart.Left
                cbPartType.Top = lblDefaultPart.Top + lblDefaultPart.Height + 1
                cbPartType.Width = 250

                lblDefaultJoint.Left = cbPartType.Left + cbPartType.Width + 1
                lblDefaultJoint.Top = 1

                cbJointType.Left = lblDefaultJoint.Left
                cbJointType.Top = lblDefaultJoint.Top + lblDefaultJoint.Height + 1
                cbJointType.Width = 250
            Else
                'Draw it laid out vertically
                btnSelectBodies.Left = 5
                btnSelectBodies.Top = 5

                btnSelectJoints.Left = btnSelectBodies.Left + btnSelectBodies.Width + 1
                btnSelectJoints.Top = 5

                btnAddBody.Left = btnSelectJoints.Left + btnSelectJoints.Width + 1
                btnAddBody.Top = 5

                If Not Me.Editor Is Nothing AndAlso Not Me.Editor.PhysicalStructure Is Nothing AndAlso TypeOf Me.Editor.PhysicalStructure Is DataObjects.Physical.Organism Then
                    btnSelectReceptiveFields.Visible = True

                    btnSelectReceptiveFields.Left = btnAddBody.Left + btnAddBody.Width + 1
                    btnSelectReceptiveFields.Top = 5

                    lblSensitivity.Left = btnSelectReceptiveFields.Left + btnSelectReceptiveFields.Width + 5
                    lblSensitivity.Top = 5
                Else
                    btnSelectReceptiveFields.Visible = False

                    lblSensitivity.Left = btnAddBody.Left + btnAddBody.Width + 5
                    lblSensitivity.Top = 5
                End If

                txtSensitivity.Left = lblSensitivity.Left
                txtSensitivity.Top = lblSensitivity.Top + lblSensitivity.Height + 1

                lblDefaultPart.Left = 5
                lblDefaultPart.Top = btnSelectBodies.Top + btnSelectBodies.Height + 5

                cbPartType.Left = 5
                cbPartType.Top = lblDefaultPart.Top + lblDefaultPart.Height + 1
                cbPartType.Width = Me.Width - 10

                lblDefaultJoint.Left = 5
                lblDefaultJoint.Top = cbPartType.Top + cbPartType.Height + 5

                cbJointType.Left = 5
                cbJointType.Top = lblDefaultJoint.Top + lblDefaultJoint.Height + 1
                cbJointType.Width = Me.Width - 10
            End If

            Me.Invalidate()
        End Sub

#End Region

    End Class

End Namespace

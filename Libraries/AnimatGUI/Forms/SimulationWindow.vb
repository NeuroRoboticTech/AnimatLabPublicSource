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
Imports System.Drawing.Printing
Imports System.Drawing.Imaging

Namespace Forms

    Public Class SimulationWindow
        Inherits AnimatForm

#Region " Windows Form Designer generated code "

        Public Sub New()
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

        End Sub

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SimulationWindow))
            Me.SimWindowToolStrip = New AnimatGuiCtrls.Controls.AnimatToolStrip()
            Me.PasteToolStripButton = New System.Windows.Forms.ToolStripButton()
            Me.CopyToolStripButton = New System.Windows.Forms.ToolStripButton()
            Me.CutToolStripButton = New System.Windows.Forms.ToolStripButton()
            Me.lbStucture = New System.Windows.Forms.ToolStripLabel()
            Me.cboStructure = New System.Windows.Forms.ToolStripComboBox()
            Me.lblBodyPart = New System.Windows.Forms.ToolStripLabel()
            Me.cboBodyPart = New System.Windows.Forms.ToolStripComboBox()
            Me.SimWindowMenuStrip = New AnimatGuiCtrls.Controls.AnimatMenuStrip()
            Me.EditToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.PasteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.CopyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.CutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.SimWindowToolStrip.SuspendLayout()
            Me.SimWindowMenuStrip.SuspendLayout()
            Me.SuspendLayout()
            '
            'SimWindowToolStrip
            '
            Me.SimWindowToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PasteToolStripButton, Me.CopyToolStripButton, Me.CutToolStripButton, Me.lbStucture, Me.cboStructure, Me.lblBodyPart, Me.cboBodyPart})
            Me.SimWindowToolStrip.Location = New System.Drawing.Point(0, 0)
            Me.SimWindowToolStrip.Name = "SimWindowToolStrip"
            Me.SimWindowToolStrip.SecurityMgr = Nothing
            Me.SimWindowToolStrip.Size = New System.Drawing.Size(774, 25)
            Me.SimWindowToolStrip.TabIndex = 0
            Me.SimWindowToolStrip.Text = "ToolStrip1"
            Me.SimWindowToolStrip.ToolName = ""
            Me.SimWindowToolStrip.Visible = False
            '
            'PasteToolStripButton
            '
            Me.PasteToolStripButton.CheckOnClick = True
            Me.PasteToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.PasteToolStripButton.Image = CType(resources.GetObject("PasteToolStripButton.Image"), System.Drawing.Image)
            Me.PasteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.PasteToolStripButton.MergeAction = System.Windows.Forms.MergeAction.Insert
            Me.PasteToolStripButton.MergeIndex = 4
            Me.PasteToolStripButton.Name = "PasteToolStripButton"
            Me.PasteToolStripButton.Size = New System.Drawing.Size(23, 22)
            Me.PasteToolStripButton.Text = "&Paste"
            '
            'CopyToolStripButton
            '
            Me.CopyToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.CopyToolStripButton.Image = CType(resources.GetObject("CopyToolStripButton.Image"), System.Drawing.Image)
            Me.CopyToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.CopyToolStripButton.MergeAction = System.Windows.Forms.MergeAction.Insert
            Me.CopyToolStripButton.MergeIndex = 4
            Me.CopyToolStripButton.Name = "CopyToolStripButton"
            Me.CopyToolStripButton.Size = New System.Drawing.Size(23, 22)
            Me.CopyToolStripButton.Text = "&Copy"
            '
            'CutToolStripButton
            '
            Me.CutToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.CutToolStripButton.Image = CType(resources.GetObject("CutToolStripButton.Image"), System.Drawing.Image)
            Me.CutToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.CutToolStripButton.MergeAction = System.Windows.Forms.MergeAction.Insert
            Me.CutToolStripButton.MergeIndex = 4
            Me.CutToolStripButton.Name = "CutToolStripButton"
            Me.CutToolStripButton.Size = New System.Drawing.Size(23, 22)
            Me.CutToolStripButton.Text = "C&ut"
            '
            'lbStucture
            '
            Me.lbStucture.Name = "lbStucture"
            Me.lbStucture.Size = New System.Drawing.Size(58, 22)
            Me.lbStucture.Text = "Structure:"
            '
            'cboStructure
            '
            Me.cboStructure.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboStructure.Name = "cboStructure"
            Me.cboStructure.Size = New System.Drawing.Size(121, 25)
            '
            'lblBodyPart
            '
            Me.lblBodyPart.Name = "lblBodyPart"
            Me.lblBodyPart.Size = New System.Drawing.Size(61, 22)
            Me.lblBodyPart.Text = "Body Part:"
            '
            'cboBodyPart
            '
            Me.cboBodyPart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboBodyPart.Name = "cboBodyPart"
            Me.cboBodyPart.Size = New System.Drawing.Size(121, 25)
            '
            'SimWindowMenuStrip
            '
            Me.SimWindowMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EditToolStripMenuItem})
            Me.SimWindowMenuStrip.Location = New System.Drawing.Point(0, 0)
            Me.SimWindowMenuStrip.Name = "SimWindowMenuStrip"
            Me.SimWindowMenuStrip.SecurityMgr = Nothing
            Me.SimWindowMenuStrip.Size = New System.Drawing.Size(284, 24)
            Me.SimWindowMenuStrip.TabIndex = 1
            Me.SimWindowMenuStrip.Text = "MenuStrip1"
            Me.SimWindowMenuStrip.ToolName = ""
            Me.SimWindowMenuStrip.Visible = False
            '
            'EditToolStripMenuItem
            '
            Me.EditToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PasteToolStripMenuItem, Me.CopyToolStripMenuItem, Me.CutToolStripMenuItem})
            Me.EditToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.MatchOnly
            Me.EditToolStripMenuItem.Name = "EditToolStripMenuItem"
            Me.EditToolStripMenuItem.Size = New System.Drawing.Size(39, 20)
            Me.EditToolStripMenuItem.Text = "&Edit"
            '
            'PasteToolStripMenuItem
            '
            Me.PasteToolStripMenuItem.CheckOnClick = True
            Me.PasteToolStripMenuItem.Image = CType(resources.GetObject("PasteToolStripMenuItem.Image"), System.Drawing.Image)
            Me.PasteToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.PasteToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert
            Me.PasteToolStripMenuItem.MergeIndex = 4
            Me.PasteToolStripMenuItem.Name = "PasteToolStripMenuItem"
            Me.PasteToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.V), System.Windows.Forms.Keys)
            Me.PasteToolStripMenuItem.Size = New System.Drawing.Size(144, 22)
            Me.PasteToolStripMenuItem.Text = "&Paste"
            '
            'CopyToolStripMenuItem
            '
            Me.CopyToolStripMenuItem.Image = CType(resources.GetObject("CopyToolStripMenuItem.Image"), System.Drawing.Image)
            Me.CopyToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.CopyToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert
            Me.CopyToolStripMenuItem.MergeIndex = 4
            Me.CopyToolStripMenuItem.Name = "CopyToolStripMenuItem"
            Me.CopyToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
            Me.CopyToolStripMenuItem.Size = New System.Drawing.Size(144, 22)
            Me.CopyToolStripMenuItem.Text = "&Copy"
            '
            'CutToolStripMenuItem
            '
            Me.CutToolStripMenuItem.Image = CType(resources.GetObject("CutToolStripMenuItem.Image"), System.Drawing.Image)
            Me.CutToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.CutToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert
            Me.CutToolStripMenuItem.MergeIndex = 4
            Me.CutToolStripMenuItem.Name = "CutToolStripMenuItem"
            Me.CutToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.X), System.Windows.Forms.Keys)
            Me.CutToolStripMenuItem.Size = New System.Drawing.Size(144, 22)
            Me.CutToolStripMenuItem.Text = "Cu&t"
            '
            'SimulationWindow
            '
            Me.ClientSize = New System.Drawing.Size(774, 262)
            Me.Controls.Add(Me.SimWindowToolStrip)
            Me.Controls.Add(Me.SimWindowMenuStrip)
            Me.MainMenuStrip = Me.SimWindowMenuStrip
            Me.Name = "SimulationWindow"
            Me.SimWindowToolStrip.ResumeLayout(False)
            Me.SimWindowToolStrip.PerformLayout()
            Me.SimWindowMenuStrip.ResumeLayout(False)
            Me.SimWindowMenuStrip.PerformLayout()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents SimWindowToolStrip As AnimatGuiCtrls.Controls.AnimatToolStrip
        Friend WithEvents lbStucture As System.Windows.Forms.ToolStripLabel
        Friend WithEvents cboStructure As System.Windows.Forms.ToolStripComboBox
        Friend WithEvents SimWindowMenuStrip As AnimatGuiCtrls.Controls.AnimatMenuStrip
        Friend WithEvents lblBodyPart As System.Windows.Forms.ToolStripLabel
        Friend WithEvents cboBodyPart As System.Windows.Forms.ToolStripComboBox
        Friend WithEvents PasteToolStripButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents CopyToolStripButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents CutToolStripButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents CutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents CopyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents PasteToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents EditToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

#End Region

#Region " Attributes "

        Protected m_doStructure As DataObjects.Physical.PhysicalStructure
        Protected m_strStructureID As String = ""

        Protected m_doBodyPart As Physical.BodyPart
        Protected m_strBodyPartID As String = ""

        Protected m_bDrawingStructureCombo As Boolean = False
        Protected m_bDrawingBodyPartCombo As Boolean = False

        Protected m_clBackColor As Color = Color.Black
        Protected m_bTrackCamera As Boolean = True

        Protected m_timerStartSimWindow As New System.Timers.Timer

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property IconName() As String
            Get
                If Not Me.PhysicalStructure Is Nothing Then
                    If Util.IsTypeOf(Me.PhysicalStructure.GetType, GetType(Physical.Organism), False) Then
                        Return "AnimatGUI.Organism.gif"
                    Else
                        Return "AnimatGUI.Structure.gif"
                    End If
                End If
                Return "AnimatGUI.Structure.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property TabImageName() As String
            Get
                If Not Me.PhysicalStructure Is Nothing Then
                    If Util.IsTypeOf(Me.PhysicalStructure.GetType, GetType(Physical.Organism), False) Then
                        Return "AnimatGUI.Organism.gif"
                    Else
                        Return "AnimatGUI.Structure.gif"
                    End If
                End If
                Return "AnimatGUI.Structure.gif"
            End Get
        End Property

        Public Overridable Property PhysicalStructure() As DataObjects.Physical.PhysicalStructure
            Get
                Return m_doStructure
            End Get
            Set(ByVal Value As DataObjects.Physical.PhysicalStructure)
                'If we are setting the structure to something different then reset the look at body ID.
                'We must do this before the code below or we will get an error.
                If Not m_doStructure Is Nothing And Not m_doStructure Is Value Then
                    Me.BodyPart = Nothing
                    RemoveHandler m_doStructure.AfterPropertyChanged, AddressOf Me.OnStructurePropertyChanged
                    RemoveHandler m_doStructure.BeforeRemoveItem, AddressOf Me.OnStructureRemoved
                End If

                If Not Value Is Nothing Then
                    SetSimData("LookatStructureID", Value.ID, True)
                Else
                    SetSimData("LookatStructureID", "", True)

                    If Not m_doStructure Is Nothing Then
                        m_doStructure.BodyEditor = Nothing
                    End If
                End If

                m_doStructure = Value

                m_bDrawingBodyPartCombo = True
                If Not m_doStructure Is Nothing Then
                    AddHandler m_doStructure.AfterPropertyChanged, AddressOf Me.OnStructurePropertyChanged
                    AddHandler m_doStructure.BeforeRemoveItem, AddressOf Me.OnStructureRemoved
                    If Not Me.TabPage Is Nothing Then
                        Me.TabPage.Title = Value.Name & " Body"
                    End If
                    Me.cboStructure.SelectedItem = m_doStructure
                Else
                    If Not Me.TabPage Is Nothing Then
                        Me.TabPage.Title = "Sim Window"
                    End If
                    Me.cboStructure.SelectedItem = "No Tracking"
                End If
                m_bDrawingBodyPartCombo = False

            End Set
        End Property

        Public Overridable Property BodyPart() As DataObjects.Physical.BodyPart
            Get
                Return m_doBodyPart
            End Get
            Set(ByVal Value As DataObjects.Physical.BodyPart)
                If Value Is Nothing Then
                    SetSimData("LookAtBodyID", "", True)
                Else
                    SetSimData("LookAtBodyID", Value.ID, True)
                End If

                If Not m_doBodyPart Is Nothing Then
                    RemoveHandler m_doBodyPart.BeforeRemoveItem, AddressOf Me.OnBodyPartRemoved
                End If

                m_doBodyPart = Value

                'Reset the value showing in the combo box.
                m_bDrawingBodyPartCombo = True
                GenerateBodyPartDropDown()
                Me.cboBodyPart.SelectedItem = m_doBodyPart
                m_bDrawingBodyPartCombo = False

                If Not m_doBodyPart Is Nothing Then
                    AddHandler m_doBodyPart.BeforeRemoveItem, AddressOf Me.OnBodyPartRemoved
                End If
            End Set
        End Property

        Public Overridable Property TrackCamera() As Boolean
            Get
                Return m_bTrackCamera
            End Get
            Set(ByVal Value As Boolean)
                SetSimData("TrackCamera", Value.ToString, True)
                m_bTrackCamera = Value
            End Set
        End Property

        Public Overrides ReadOnly Property FormMenuStrip() As AnimatGuiCtrls.Controls.AnimatMenuStrip
            Get
                Return Me.SimWindowMenuStrip
            End Get
        End Property

        Public Overrides ReadOnly Property FormToolStrip() As AnimatGuiCtrls.Controls.AnimatToolStrip
            Get
                Return Me.SimWindowToolStrip
            End Get
        End Property

        Public Overridable ReadOnly Property StructureID() As String
            Get
                Return m_strStructureID
            End Get
        End Property

        Public Overridable ReadOnly Property BodyPartID() As String
            Get
                Return m_strBodyPartID
            End Get
        End Property

        Public Overridable ReadOnly Property CameraPosition() As Vec3d
            Get
                Return New Vec3d(Nothing, 0, 0, 0)
            End Get
        End Property

        Public Overridable Property ViewBackColor() As Color
            Get
                Return m_clBackColor
            End Get
            Set(ByVal Value As Color)
                m_clBackColor = Value
            End Set
        End Property

#End Region

#Region " Methods "


        Public Overrides Sub Initialize(Optional ByVal frmParent As AnimatForm = Nothing)
            Try

                MyBase.Initialize(frmParent)

                AddHandler Util.Application.UnitsChanged, AddressOf Me.Application_UnitsChanged
                AddHandler Me.cboStructure.DropDown, AddressOf Me.OnStructureDropDown
                AddHandler Me.cboStructure.SelectedIndexChanged, AddressOf Me.OnStructureChanged
                AddHandler Me.cboBodyPart.DropDown, AddressOf Me.OnBodyPartDropDown
                AddHandler Me.cboBodyPart.SelectedIndexChanged, AddressOf Me.OnBodyPartChanged
                AddHandler Util.Application.BodyPartPasteStarting, AddressOf Me.OnBodyPartPasteStarting
                AddHandler Util.Application.BodyPartPasteEnding, AddressOf Me.OnBodyPartPasteEnding

                m_timerStartSimWindow.Enabled = False
                m_timerStartSimWindow.Interval = 100

                If Not m_doStructure Is Nothing Then
                    AddHandler m_doStructure.AfterPropertyChanged, AddressOf Me.OnStructurePropertyChanged
                    AddHandler m_doStructure.BeforeRemoveItem, AddressOf Me.OnStructureRemoved
                End If

                If Not m_doBodyPart Is Nothing Then
                    AddHandler m_doBodyPart.BeforeRemoveItem, AddressOf Me.OnBodyPartRemoved
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub


        Protected Overrides Sub ReconnectFormToWorkspace()

            If Not m_doStructure Is Nothing Then
                For Each deEntry As DictionaryEntry In Util.Environment.Structures
                    Dim doStruct As DataObjects.Physical.PhysicalStructure = DirectCast(deEntry.Value, DataObjects.Physical.PhysicalStructure)
                    If doStruct.ID = m_doStructure.ID Then
                        Me.PhysicalStructure = doStruct
                        Return
                    End If
                Next

                For Each deEntry As DictionaryEntry In Util.Environment.Organisms
                    Dim doStruct As DataObjects.Physical.PhysicalStructure = DirectCast(deEntry.Value, DataObjects.Physical.PhysicalStructure)
                    If doStruct.ID = m_doStructure.ID Then
                        Me.PhysicalStructure = doStruct
                        Return
                    End If
                Next
            End If

        End Sub

        Public Overridable Function GenerateSimWindowXml() As String
            Dim oXml As ManagedAnimatInterfaces.IStdXml = Util.Application.CreateStdXml()

            oXml.AddElement("WindowMgr")
            oXml.AddChildElement("Window")

            oXml.IntoElem()
            oXml.AddChildElement("Name", Me.Text)
            oXml.AddChildElement("ID", Me.ID)
            oXml.AddChildElement("Type", "Basic")
            oXml.AddChildElement("StandAlone", True)

            If Not m_doStructure Is Nothing Then
                oXml.AddChildElement("TrackCamera", m_bTrackCamera)
                oXml.AddChildElement("LookAtStructureID", m_doStructure.ID)
            Else
                oXml.AddChildElement("TrackCamera", False)
            End If

            If Not m_doBodyPart Is Nothing Then
                oXml.AddChildElement("LookAtBodyID", m_doBodyPart.ID)
            End If

            oXml.OutOfElem()

            Return oXml.Serialize()
        End Function

        Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.LoadData(oXml)

            oXml.IntoElem()
            m_bTrackCamera = oXml.GetChildBool("TrackCamera", m_bTrackCamera)
            m_strStructureID = oXml.GetChildString("StructureID", "")
            m_strBodyPartID = oXml.GetChildString("BodyPartID", "")
            oXml.OutOfElem()

            ReconnectFormToWorkspace()
        End Sub

        Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.SaveData(oXml)

            oXml.IntoElem()

            oXml.AddChildElement("TrackCamera", m_bTrackCamera)
            If Not m_doStructure Is Nothing Then
                oXml.AddChildElement("StructureID", m_doStructure.ID)
            End If
            If Not m_doBodyPart Is Nothing Then
                oXml.AddChildElement("BodyPartID", m_doBodyPart.ID)
            End If

            oXml.OutOfElem()

        End Sub


#End Region

#Region " Events "

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            MyBase.OnLoad(e)

            Try
                AddHandler m_timerStartSimWindow.Elapsed, AddressOf Me.OnStartSimWindow
                m_timerStartSimWindow.Enabled = True

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Delegate Sub OnStartSimWindowDelegate(ByVal sender As Object, ByVal eProps As System.Timers.ElapsedEventArgs)

        Protected Overridable Sub OnStartSimWindow(ByVal sender As Object, ByVal eProps As System.Timers.ElapsedEventArgs)

            'Do not attempt to start the window until the load process is completed.
            If Util.LoadInProgress Then
                Return
            End If

            m_timerStartSimWindow.Enabled = False

            If Me.InvokeRequired Then
                Me.Invoke(New OnStartSimWindowDelegate(AddressOf OnStartSimWindow), New Object() {sender, eProps})
                Return
            End If

            Try
                'Only do this code once. If for some reason the timer fires multiple times we should only call this code if
                'if the interface has not already been initialized.
                If m_doInterface Is Nothing Then
                    If m_strStructureID.Trim.Length > 0 AndAlso Me.PhysicalStructure Is Nothing Then
                        Me.PhysicalStructure = DirectCast(Util.Simulation.FindObjectByID(m_strStructureID), DataObjects.Physical.PhysicalStructure)
                        Me.PhysicalStructure.BodyEditor = Me

                        If m_strBodyPartID.Trim.Length > 0 AndAlso Me.BodyPart Is Nothing Then
                            Me.BodyPart = DirectCast(Util.Simulation.FindObjectByID(m_strBodyPartID), DataObjects.Physical.BodyPart)
                        End If
                    End If

                    Dim strWinXml As String = GenerateSimWindowXml()
                    Util.Application.SimulationInterface.AddWindow(Me.Handle, strWinXml)
                    InitializeSimulationReferences()

                    GenerateStructureDropDown()
                    GenerateBodyPartDropDown()
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overrides Sub OnFormClosing(ByVal e As System.Windows.Forms.FormClosingEventArgs)
            MyBase.OnFormClosing(e)

            Try
                RemoveHandler Util.Application.UnitsChanged, AddressOf Me.Application_UnitsChanged
                RemoveHandler Util.Application.BodyPartPasteStarting, AddressOf Me.OnBodyPartPasteStarting
                RemoveHandler Util.Application.BodyPartPasteEnding, AddressOf Me.OnBodyPartPasteEnding

                If Not m_doStructure Is Nothing Then
                    RemoveHandler m_doStructure.AfterPropertyChanged, AddressOf Me.OnStructurePropertyChanged
                    RemoveHandler m_doStructure.BeforeRemoveItem, AddressOf Me.OnStructureRemoved
                End If

                If Not m_doBodyPart Is Nothing Then
                    RemoveHandler m_doBodyPart.BeforeRemoveItem, AddressOf Me.OnBodyPartRemoved
                End If

                Util.Application.SimulationInterface.RemoveWindow(Me.Handle)
                m_doInterface = Nothing
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overrides Sub OnFormClosed(ByVal e As System.Windows.Forms.FormClosedEventArgs)
            MyBase.OnFormClosed(e)

            Me.PhysicalStructure = Nothing

        End Sub

        Public Overrides Sub OnGetFocus()
            MyBase.OnGetFocus()

            Try
                If Util.Application.SimulationInterface.FindItem(Me.ID, False) Then
                    Util.Application.SimulationInterface.OnWindowGetFocus(Me.ID)
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Public Overrides Sub OnLoseFocus()
            MyBase.OnLoseFocus()

            Try
                If Util.Application.SimulationInterface.FindItem(Me.ID, False) Then
                    Util.Application.SimulationInterface.OnWindowLoseFocus(Me.ID)
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub Application_UnitsChanged(ByVal ePrevMass As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits, _
                                  ByVal eNewMass As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits, _
                                  ByVal fltMassChange As Single, _
                                  ByVal ePrevDistance As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits, _
                                  ByVal eNewDistance As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits, _
                                  ByVal fltDistanceChange As Single)
            Try
                Util.ProjectProperties.RefreshProperties()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Public Overridable Sub GenerateStructureDropDown()
            m_bDrawingStructureCombo = True
            Me.cboStructure.Items.Clear()

            Dim strBlank As String = "No Tracking"
            Me.cboStructure.Items.Add(strBlank)
            If Me.PhysicalStructure Is Nothing Then
                Me.cboStructure.SelectedItem = strBlank
            End If

            For Each deEntry As DictionaryEntry In Util.Environment.Structures
                Dim doStruct As DataObjects.Physical.PhysicalStructure = DirectCast(deEntry.Value, DataObjects.Physical.PhysicalStructure)

                Me.cboStructure.Items.Add(doStruct)

                If Not Me.PhysicalStructure Is Nothing AndAlso Me.PhysicalStructure Is doStruct Then
                    Me.cboStructure.SelectedItem = doStruct
                End If
            Next

            For Each deEntry As DictionaryEntry In Util.Environment.Organisms
                Dim doStruct As DataObjects.Physical.PhysicalStructure = DirectCast(deEntry.Value, DataObjects.Physical.PhysicalStructure)

                Me.cboStructure.Items.Add(doStruct)

                If Not Me.PhysicalStructure Is Nothing AndAlso Me.PhysicalStructure Is doStruct Then
                    Me.cboStructure.SelectedItem = doStruct
                End If
            Next


            m_bDrawingStructureCombo = False
        End Sub

        Protected Overridable Sub GenerateBodyPartDropDown(ByVal aryNodes As Crownwood.DotNetMagic.Controls.NodeCollection)

            For Each tnNode As Crownwood.DotNetMagic.Controls.Node In aryNodes
                If Not tnNode.Tag Is Nothing AndAlso Util.IsTypeOf(tnNode.Tag.GetType, GetType(DataObjects.Physical.BodyPart)) Then
                    Dim bpPart As DataObjects.Physical.BodyPart = DirectCast(tnNode.Tag, DataObjects.Physical.BodyPart)
                    cboBodyPart.Items.Add(bpPart)

                    If Not m_doBodyPart Is Nothing AndAlso bpPart Is m_doBodyPart Then
                        Me.cboBodyPart.SelectedItem = bpPart
                    End If
                End If

                GenerateBodyPartDropDown(tnNode.Nodes)
            Next
        End Sub

        Public Overridable Sub GenerateBodyPartDropDown()
            Try
                m_bDrawingBodyPartCombo = True

                Me.cboBodyPart.Items.Clear()

                If Not Me.PhysicalStructure Is Nothing AndAlso Not Me.PhysicalStructure.WorkspaceNode Is Nothing Then
                    GenerateBodyPartDropDown(Me.PhysicalStructure.WorkspaceNode.Nodes)
                End If

            Catch ex As System.Exception
                Throw ex
            Finally
                m_bDrawingBodyPartCombo = False
            End Try
        End Sub

        Protected Overridable Sub OnStructureDropDown(ByVal sender As Object, ByVal e As System.EventArgs)
            Try
                GenerateStructureDropDown()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                m_bDrawingStructureCombo = False
            End Try

        End Sub

        Protected Overridable Sub OnStructureChanged(ByVal sender As Object, ByVal e As System.EventArgs)
            Try
                If Not m_bDrawingStructureCombo Then
                    If Not Me.cboStructure.SelectedItem Is Nothing AndAlso Util.IsTypeOf(Me.cboStructure.SelectedItem.GetType, GetType(DataObjects.Physical.PhysicalStructure), False) Then
                        Me.PhysicalStructure = DirectCast(Me.cboStructure.SelectedItem, DataObjects.Physical.PhysicalStructure)
                    Else
                        Me.PhysicalStructure = Nothing
                    End If
                End If

                GenerateBodyPartDropDown()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Overridable Sub OnBodyPartDropDown(ByVal sender As Object, ByVal e As System.EventArgs)
            Try
                GenerateBodyPartDropDown()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                m_bDrawingBodyPartCombo = False
            End Try

        End Sub

        Protected Overridable Sub OnBodyPartChanged(ByVal sender As Object, ByVal e As System.EventArgs)
            Try
                If Not m_bDrawingBodyPartCombo Then
                    If Not Me.cboBodyPart.SelectedItem Is Nothing AndAlso Util.IsTypeOf(Me.cboBodyPart.SelectedItem.GetType, GetType(DataObjects.Physical.BodyPart), False) Then
                        Me.BodyPart = DirectCast(Me.cboBodyPart.SelectedItem, DataObjects.Physical.BodyPart)
                    Else
                        Me.BodyPart = Nothing
                    End If
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Overridable Sub OnStructurePropertyChanged(ByRef doObject As AnimatGUI.Framework.DataObject, ByVal propInfo As System.Reflection.PropertyInfo)
            Try
                If propInfo.Name = "Name" AndAlso Not Me.TabPage Is Nothing Then
                    Me.TabPage.Title = m_doStructure.Name & "Body"
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub OnStructureRemoved(ByRef doObject As AnimatGUI.Framework.DataObject)
            Try
                'If we are deleting the structure this window is attached to then set it to be free moving first.
                Me.PhysicalStructure = Nothing
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub OnBodyPartRemoved(ByRef doObject As AnimatGUI.Framework.DataObject)
            Try
                'If we are deleting the body part this window is looking at then switch to the root body or null
                If Not m_doStructure Is Nothing Then
                    If Not m_doStructure Is Nothing AndAlso Not m_doStructure.RootBody Is Nothing AndAlso Not Me.BodyPart Is m_doStructure.RootBody Then
                        Me.BodyPart = m_doStructure.RootBody
                    Else
                        Me.BodyPart = Nothing
                    End If
                Else
                    Me.BodyPart = Nothing
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub CutToolStripButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CutToolStripButton.Click, CutToolStripMenuItem.Click
            BodyPart.CutSelected(Me)
        End Sub

        Private Sub CopyToolStripButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CopyToolStripButton.Click, CopyToolStripMenuItem.Click
            BodyPart.CopySelected()
        End Sub

        Private Sub PasteToolStripButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles PasteToolStripButton.Click, PasteToolStripMenuItem.Click
            BodyPart.PasteSelected(False)
        End Sub

        Protected Sub OnBodyPartPasteStarting()
            Me.PasteToolStripMenuItem.Checked = True
            Me.PasteToolStripButton.Checked = True
        End Sub

        Protected Sub OnBodyPartPasteEnding()
            Me.PasteToolStripMenuItem.Checked = False
            Me.PasteToolStripButton.Checked = False
        End Sub

#End Region

    End Class

End Namespace



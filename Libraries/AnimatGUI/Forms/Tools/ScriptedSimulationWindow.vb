Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports AnimatGuiCtrls.Controls
Imports AnimatGUI.Framework
Imports AnimatGUI.DataObjects
Imports System.Drawing.Printing
Imports System.Drawing.Imaging

Namespace Forms
    Namespace Tools

        Public Class ScriptedSimulationWindow
            Inherits ToolForm

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
                Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ScriptedSimulationWindow))
                Me.SimWindowToolStrip = New AnimatGuiCtrls.Controls.AnimatToolStrip()
                Me.PasteToolStripButton = New System.Windows.Forms.ToolStripButton()
                Me.CopyToolStripButton = New System.Windows.Forms.ToolStripButton()
                Me.CutToolStripButton = New System.Windows.Forms.ToolStripButton()
                Me.lbStucture = New System.Windows.Forms.ToolStripLabel()
                Me.cboStructure = New System.Windows.Forms.ToolStripComboBox()
                Me.lblBodyPart = New System.Windows.Forms.ToolStripLabel()
                Me.cboBodyPart = New System.Windows.Forms.ToolStripComboBox()
                Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
                Me.AddCameraPathToolStripButton = New System.Windows.Forms.ToolStripButton()
                Me.AddWaypointToolStripButton = New System.Windows.Forms.ToolStripButton()
                Me.SimWindowMenuStrip = New AnimatGuiCtrls.Controls.AnimatMenuStrip()
                Me.EditToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
                Me.PasteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
                Me.CopyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
                Me.CutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
                Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
                Me.AddCameraPathToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
                Me.AddWaypointToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
                Me.SimWindowToolStrip.SuspendLayout()
                Me.SimWindowMenuStrip.SuspendLayout()
                Me.SuspendLayout()
                '
                'SimWindowToolStrip
                '
                Me.SimWindowToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PasteToolStripButton, Me.CopyToolStripButton, Me.CutToolStripButton, Me.lbStucture, Me.cboStructure, Me.lblBodyPart, Me.cboBodyPart, Me.ToolStripSeparator1, Me.AddCameraPathToolStripButton, Me.AddWaypointToolStripButton})
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
                'ToolStripSeparator1
                '
                Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
                Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
                '
                'AddCameraPathToolStripButton
                '
                Me.AddCameraPathToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
                Me.AddCameraPathToolStripButton.Image = Global.AnimatGUI.ModuleInformation.AddCameraPath
                Me.AddCameraPathToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
                Me.AddCameraPathToolStripButton.Name = "AddCameraPathToolStripButton"
                Me.AddCameraPathToolStripButton.Size = New System.Drawing.Size(23, 22)
                Me.AddCameraPathToolStripButton.Text = "Add Camera Path"
                '
                'AddWaypointToolStripButton
                '
                Me.AddWaypointToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
                Me.AddWaypointToolStripButton.Image = Global.AnimatGUI.ModuleInformation.AddCameraWaypoint
                Me.AddWaypointToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
                Me.AddWaypointToolStripButton.Name = "AddWaypointToolStripButton"
                Me.AddWaypointToolStripButton.Size = New System.Drawing.Size(23, 22)
                Me.AddWaypointToolStripButton.Text = "Add Waypoint"
                '
                'SimWindowMenuStrip
                '
                Me.SimWindowMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EditToolStripMenuItem})
                Me.SimWindowMenuStrip.Location = New System.Drawing.Point(0, 0)
                Me.SimWindowMenuStrip.Name = "SimWindowMenuStrip"
                Me.SimWindowMenuStrip.SecurityMgr = Nothing
                Me.SimWindowMenuStrip.Size = New System.Drawing.Size(774, 24)
                Me.SimWindowMenuStrip.TabIndex = 1
                Me.SimWindowMenuStrip.Text = "MenuStrip1"
                Me.SimWindowMenuStrip.ToolName = ""
                Me.SimWindowMenuStrip.Visible = False
                '
                'EditToolStripMenuItem
                '
                Me.EditToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PasteToolStripMenuItem, Me.CopyToolStripMenuItem, Me.CutToolStripMenuItem, Me.ToolStripSeparator2, Me.AddCameraPathToolStripMenuItem, Me.AddWaypointToolStripMenuItem})
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
                Me.PasteToolStripMenuItem.Size = New System.Drawing.Size(167, 22)
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
                Me.CopyToolStripMenuItem.Size = New System.Drawing.Size(167, 22)
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
                Me.CutToolStripMenuItem.Size = New System.Drawing.Size(167, 22)
                Me.CutToolStripMenuItem.Text = "Cu&t"
                '
                'ToolStripSeparator2
                '
                Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
                Me.ToolStripSeparator2.Size = New System.Drawing.Size(164, 6)
                '
                'AddCameraPathToolStripMenuItem
                '
                Me.AddCameraPathToolStripMenuItem.Image = Global.AnimatGUI.ModuleInformation.AddCameraPath
                Me.AddCameraPathToolStripMenuItem.Name = "AddCameraPathToolStripMenuItem"
                Me.AddCameraPathToolStripMenuItem.Size = New System.Drawing.Size(167, 22)
                Me.AddCameraPathToolStripMenuItem.Text = "Add Camera Path"
                '
                'AddWaypointToolStripMenuItem
                '
                Me.AddWaypointToolStripMenuItem.Image = Global.AnimatGUI.ModuleInformation.AddCameraWaypoint
                Me.AddWaypointToolStripMenuItem.Name = "AddWaypointToolStripMenuItem"
                Me.AddWaypointToolStripMenuItem.Size = New System.Drawing.Size(167, 22)
                Me.AddWaypointToolStripMenuItem.Text = "Add Waypoint"
                '
                'ScriptedSimulationWindow
                '
                Me.ClientSize = New System.Drawing.Size(774, 262)
                Me.Controls.Add(Me.SimWindowToolStrip)
                Me.Controls.Add(Me.SimWindowMenuStrip)
                Me.MainMenuStrip = Me.SimWindowMenuStrip
                Me.Name = "Scripted Simulation Window"
                Me.SimWindowToolStrip.ResumeLayout(False)
                Me.SimWindowToolStrip.PerformLayout()
                Me.SimWindowMenuStrip.ResumeLayout(False)
                Me.SimWindowMenuStrip.PerformLayout()
                Me.ResumeLayout(False)
                Me.PerformLayout()

            End Sub
            Friend WithEvents SimWindowToolStrip As AnimatGuiCtrls.Controls.AnimatToolStrip
            Friend WithEvents SimWindowMenuStrip As AnimatGuiCtrls.Controls.AnimatMenuStrip
            Friend WithEvents PasteToolStripButton As System.Windows.Forms.ToolStripButton
            Friend WithEvents CopyToolStripButton As System.Windows.Forms.ToolStripButton
            Friend WithEvents CutToolStripButton As System.Windows.Forms.ToolStripButton
            Friend WithEvents CutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents CopyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents PasteToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents EditToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
            Friend WithEvents AddCameraPathToolStripButton As System.Windows.Forms.ToolStripButton
            Friend WithEvents AddWaypointToolStripButton As System.Windows.Forms.ToolStripButton
            Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
            Friend WithEvents AddCameraPathToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents AddWaypointToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents lbStucture As System.Windows.Forms.ToolStripLabel
            Friend WithEvents cboStructure As System.Windows.Forms.ToolStripComboBox
            Friend WithEvents lblBodyPart As System.Windows.Forms.ToolStripLabel
            Friend WithEvents cboBodyPart As System.Windows.Forms.ToolStripComboBox

#End Region

#Region " Attributes "

            Protected m_thDefaultStructure As AnimatGUI.TypeHelpers.LinkedStructureList
            Protected m_thDefaultPart As AnimatGUI.TypeHelpers.LinkedBodyPartList
            Protected m_svDefaultPosition As Framework.ScaledVector3

            'Only used during loading
            Protected m_strDefaultStructureID As String = ""
            Protected m_strDefaultBodyPartID As String = ""

            Protected m_doStructure As DataObjects.Physical.PhysicalStructure
            Protected m_doBodyPart As Physical.BodyPart

            Protected m_bDrawingStructureCombo As Boolean = False
            Protected m_bDrawingBodyPartCombo As Boolean = False

            Protected m_aryCameraPaths As Collections.SortedCameraPathsList

            Protected m_clBackColor As Color = Color.Black

            Protected m_timerStartSimWindow As New System.Timers.Timer

            Protected m_tnCameraPaths As Crownwood.DotNetMagic.Controls.Node

#End Region

#Region " Properties "

            <Browsable(False)> _
            Public Overrides ReadOnly Property ToolType() As String
                Get
                    Return "ScriptedSimWindow"
                End Get
            End Property

            Public Overrides ReadOnly Property IconName() As String
                Get
                    Return "AnimatGUI.ScriptedSimWindow.gif"
                End Get
            End Property

            Public Overrides ReadOnly Property TabImageName() As String
                Get
                    Return "AnimatGUI.ScriptedSimWindow.gif"
                End Get
            End Property

            Public Overrides ReadOnly Property WorkspaceImageName As String
                Get
                    Return "AnimatGUI.ScriptedSimWindow.gif"
                End Get
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

            Public Overridable ReadOnly Property CameraPaths() As Collections.SortedCameraPathsList
                Get
                    Return m_aryCameraPaths
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
                        Me.cboStructure.SelectedItem = m_doStructure
                    Else
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
                        SetSimData("DefaultPartID", "", True)
                    Else
                        SetSimData("LookAtBodyID", Value.ID, True)
                        SetSimData("DefaultPartID", Value.ID, True)
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

            Public Overridable ReadOnly Property DefaultStructureID() As String
                Get
                    Return m_strDefaultStructureID
                End Get
            End Property

            Public Overridable ReadOnly Property DefaultBodyPartID() As String
                Get
                    Return m_strDefaultBodyPartID
                End Get
            End Property

            <Browsable(False)> _
            Public Overridable Property DefaultLinkedStructure() As AnimatGUI.TypeHelpers.LinkedStructureList
                Get
                    Return m_thDefaultStructure
                End Get
                Set(ByVal Value As AnimatGUI.TypeHelpers.LinkedStructureList)
                    Dim thPrevLinked As AnimatGUI.TypeHelpers.LinkedStructureList = m_thDefaultStructure

                    DiconnectLinkedEvents()
                    m_thDefaultStructure = Value

                    If Not m_thDefaultStructure.PhysicalStructure Is thPrevLinked.PhysicalStructure Then
                        m_thDefaultPart = New TypeHelpers.LinkedBodyPartList(m_thDefaultStructure.PhysicalStructure, Nothing, GetType(AnimatGUI.DataObjects.Physical.BodyPart))
                    End If

                    Me.PhysicalStructure = m_thDefaultStructure.PhysicalStructure

                    ConnectLinkedEvents()
                End Set
            End Property

            <Browsable(False)> _
            Public Overridable Property DefaultLinkedPart() As AnimatGUI.TypeHelpers.LinkedBodyPartList
                Get
                    Return m_thDefaultPart
                End Get
                Set(ByVal Value As AnimatGUI.TypeHelpers.LinkedBodyPartList)
                    Dim thPrevLinked As AnimatGUI.TypeHelpers.LinkedBodyPartList = m_thDefaultPart

                    DiconnectLinkedEvents()
                    m_thDefaultPart = Value
                    ConnectLinkedEvents()

                    Me.PhysicalStructure = m_thDefaultStructure.PhysicalStructure
                    Me.BodyPart = m_thDefaultPart.BodyPart
                End Set
            End Property

            Public Overridable Property DefaultPosition() As Framework.ScaledVector3
                Get
                    Return m_svDefaultPosition
                End Get
                Set(ByVal value As Framework.ScaledVector3)
                    Me.SetSimData("DefaultPosition", value.GetSimulationXml("DefaultPosition"), True)
                    m_svDefaultPosition.CopyData(value)
                End Set
            End Property

#End Region

#Region " Methods "


            Public Overrides Sub Initialize(Optional ByVal frmParent As AnimatForm = Nothing)
                Try

                    MyBase.Initialize(frmParent)

                    Me.Name = "Scripted Simulation Window"

                    m_aryCameraPaths = New Collections.SortedCameraPathsList(Me.FormHelper)

                    AddHandler Util.Application.UnitsChanged, AddressOf Me.Application_UnitsChanged
                    AddHandler Util.Application.BodyPartPasteStarting, AddressOf Me.OnBodyPartPasteStarting
                    AddHandler Util.Application.BodyPartPasteEnding, AddressOf Me.OnBodyPartPasteEnding
                    AddHandler Me.cboStructure.DropDown, AddressOf Me.OnStructureDropDown
                    AddHandler Me.cboStructure.SelectedIndexChanged, AddressOf Me.OnStructureChanged
                    AddHandler Me.cboBodyPart.DropDown, AddressOf Me.OnBodyPartDropDown
                    AddHandler Me.cboBodyPart.SelectedIndexChanged, AddressOf Me.OnBodyPartChanged

                    m_timerStartSimWindow.Enabled = False
                    m_timerStartSimWindow.Interval = 100

                    If Not m_doStructure Is Nothing Then
                        AddHandler m_doStructure.AfterPropertyChanged, AddressOf Me.OnStructurePropertyChanged
                        AddHandler m_doStructure.BeforeRemoveItem, AddressOf Me.OnStructureRemoved
                    End If

                    If Not m_doBodyPart Is Nothing Then
                        AddHandler m_doBodyPart.BeforeRemoveItem, AddressOf Me.OnBodyPartRemoved
                    End If

                    m_thDefaultStructure = New AnimatGUI.TypeHelpers.LinkedStructureList(Nothing, TypeHelpers.LinkedStructureList.enumStructureType.All)
                    m_thDefaultPart = New AnimatGUI.TypeHelpers.LinkedBodyPartList(Nothing, Nothing, GetType(AnimatGUI.DataObjects.Physical.BodyPart))

                    m_svDefaultPosition = New ScaledVector3(Me.FormHelper, "DefaultPosition", "Initial location of the camera when simulation is started.", "Meters", "m")

                    AddHandler m_svDefaultPosition.ValueChanged, AddressOf Me.OnDefaultPositionValueChanged

                Catch ex As System.Exception
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End Try

            End Sub

            Public Overrides Sub ClearIsDirty()
                MyBase.ClearIsDirty()
                If Not m_aryCameraPaths Is Nothing Then m_aryCameraPaths.ClearIsDirty()
                If Not m_thDefaultStructure Is Nothing Then m_thDefaultStructure.ClearIsDirty()
                If Not m_thDefaultPart Is Nothing Then m_thDefaultPart.ClearIsDirty()
                If Not m_svDefaultPosition Is Nothing Then m_svDefaultPosition.ClearIsDirty()
            End Sub

            Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Name", Me.Name.GetType(), "Name", _
                                            "Path Properties", "Name", Me.Name))

                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("ID", Me.ID.GetType(), "ID", _
                                            "Path Properties", "ID", Me.ID, True))

                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Structure", m_thDefaultStructure.GetType, "DefaultLinkedStructure", _
                                            "Initial Properties", "When simulation starts the camera is set to look at this structure", m_thDefaultStructure, _
                                            GetType(AnimatGUI.TypeHelpers.DropDownListEditor), _
                                            GetType(AnimatGUI.TypeHelpers.LinkedStructureTypeConverter)))

                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Part", m_thDefaultPart.GetType, "DefaultLinkedPart", _
                                            "Initial Properties", "When simulation starts the camera is set to look at this part", m_thDefaultPart, _
                                            GetType(AnimatGUI.TypeHelpers.DropDownListEditor), _
                                            GetType(AnimatGUI.TypeHelpers.LinkedBodyPartTypeConverter)))

                Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = Me.DefaultPosition.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Position", pbNumberBag.GetType(), "DefaultPosition", _
                                            "Initial Properties", "Initial location of the camera when simulation is started.", pbNumberBag, _
                                            "", GetType(AnimatGUI.Framework.ScaledVector3.ScaledVector3PropBagConverter)))

            End Sub

            Public Overrides Function Clone() As ToolForm
                Try
                    Return New Forms.Tools.ScriptedSimulationWindow
                Catch ex As System.Exception
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End Try
            End Function

            Public Overrides Sub InitializeAfterLoad()
                MyBase.InitializeAfterLoad()

                Dim doPath As AnimatGUI.DataObjects.Visualization.CameraPath
                For Each deEntry As DictionaryEntry In m_aryCameraPaths
                    doPath = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Visualization.CameraPath)
                    doPath.InitializeAfterLoad()
                Next
            End Sub

            Public Overrides Sub InitializeSimulationReferences(Optional bShowError As Boolean = True)
                MyBase.InitializeSimulationReferences(bShowError)

                Dim doPath As AnimatGUI.DataObjects.Visualization.CameraPath
                For Each deEntry As DictionaryEntry In m_aryCameraPaths
                    doPath = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Visualization.CameraPath)
                    doPath.InitializeSimulationReferences(bShowError)
                Next
            End Sub

            Protected Overridable Sub ConnectLinkedEvents()
                DiconnectLinkedEvents()

                If Not m_thDefaultStructure Is Nothing AndAlso Not m_thDefaultStructure.PhysicalStructure Is Nothing Then
                    AddHandler m_thDefaultStructure.PhysicalStructure.AfterRemoveItem, AddressOf Me.OnAfterRemoveLinkedStructure
                End If
                If Not m_thDefaultPart Is Nothing AndAlso Not m_thDefaultPart.BodyPart Is Nothing Then
                    AddHandler m_thDefaultPart.BodyPart.AfterRemoveItem, AddressOf Me.OnAfterRemoveLinkedPart
                End If
            End Sub

            Protected Overridable Sub DiconnectLinkedEvents()
                If Not m_thDefaultStructure Is Nothing AndAlso Not m_thDefaultStructure.PhysicalStructure Is Nothing Then
                    RemoveHandler m_thDefaultStructure.PhysicalStructure.AfterRemoveItem, AddressOf Me.OnAfterRemoveLinkedStructure
                End If
                If Not m_thDefaultPart Is Nothing AndAlso Not m_thDefaultPart.BodyPart Is Nothing Then
                    RemoveHandler m_thDefaultPart.BodyPart.AfterRemoveItem, AddressOf Me.OnAfterRemoveLinkedPart
                End If
            End Sub

            Public Overridable Function HasPathWithStartTime(ByVal dblTime As Double) As Boolean

                Dim doPath As Visualization.CameraPath
                For Each deEntry As DictionaryEntry In m_aryCameraPaths
                    doPath = DirectCast(deEntry.Value, Visualization.CameraPath)
                    If doPath.StartTime.ActualValue = dblTime Then
                        Return True
                    End If
                Next

                Return False
            End Function

            Public Overloads Overrides Sub LoadExternalFile(ByVal strFilename As String)
                Try
                    Dim oXml As ManagedAnimatInterfaces.IStdXml = Util.Application.CreateStdXml()

                    'If no file exsists yet then one has not been saved. Just go with the default creation
                    Dim strFile As String = Util.GetFilePath(Util.Application.ProjectPath, strFilename)
                    If File.Exists(strFile) Then
                        oXml.Load(strFile)
                        oXml.FindElement("Form")
                        LoadExternalData(oXml)
                    End If

                    InitializeAfterLoad()
                Catch ex As System.Exception
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End Try

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

                If Not m_thDefaultStructure Is Nothing AndAlso Not m_thDefaultStructure.PhysicalStructure Is Nothing Then
                    oXml.AddChildElement("LookAtStructureID", m_thDefaultStructure.PhysicalStructure.ID)
                End If

                If Not m_thDefaultPart Is Nothing AndAlso Not m_thDefaultPart.BodyPart Is Nothing Then
                    oXml.AddChildElement("LookAtBodyID", m_thDefaultPart.BodyPart.ID)
                End If
                oXml.AddChildElement("TrackCamera", True)

                m_svDefaultPosition.SaveSimulationXml(oXml, Me.FormHelper, "Position")

                If m_aryCameraPaths.Count > 0 Then
                    oXml.AddChildElement("CameraPaths")
                    oXml.IntoElem()

                    Dim doPath As AnimatGUI.DataObjects.Visualization.CameraPath
                    For Each deEntry As DictionaryEntry In m_aryCameraPaths
                        doPath = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Visualization.CameraPath)
                        doPath.SaveSimulationXml(oXml)
                    Next

                    oXml.OutOfElem() 'Outof Waypoints Element
                End If

                oXml.OutOfElem()

                Return oXml.Serialize()
            End Function

            Protected Overrides Sub LoadExternalData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
                MyBase.LoadExternalData(oXml)

                oXml.IntoElem()

                m_strDefaultStructureID = Util.LoadID(oXml, "LinkedStructure", True, "")
                m_strDefaultBodyPartID = Util.LoadID(oXml, "LinkedBodyPart", True, "")

                m_svDefaultPosition.LoadData(oXml, "Position")

                m_aryCameraPaths.Clear()
                If oXml.FindChildElement("CameraPaths", False) Then
                    Dim doPath As DataObjects.Visualization.CameraPath

                    oXml.IntoElem() 'Into paths Element
                    Dim iCount As Integer = oXml.NumberOfChildren() - 1
                    For iIndex As Integer = 0 To iCount
                        oXml.FindChildByIndex(iIndex)

                        doPath = New DataObjects.Visualization.CameraPath(Me.FormHelper)
                        doPath.LoadData(oXml)

                        'Do not call the sim add method here. We will need to do that later when the window is created.
                        m_aryCameraPaths.Add(doPath.ID, doPath, False)
                    Next
                    oXml.OutOfElem()   'Outof paths Element
                End If

                oXml.OutOfElem()

            End Sub

            Protected Overrides Sub SaveExternalData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
                MyBase.SaveExternalData(oXml)

                oXml.IntoElem()

                If Not m_thDefaultStructure Is Nothing AndAlso Not m_thDefaultStructure.PhysicalStructure Is Nothing Then
                    oXml.AddChildElement("LinkedStructureID", m_thDefaultStructure.PhysicalStructure.ID)
                End If

                If Not m_thDefaultPart Is Nothing AndAlso Not m_thDefaultPart.BodyPart Is Nothing Then
                    oXml.AddChildElement("LinkedBodyPartID", m_thDefaultPart.BodyPart.ID)
                End If

                m_svDefaultPosition.SaveData(oXml, "Position")

                If m_aryCameraPaths.Count > 0 Then
                    oXml.AddChildElement("CameraPaths")
                    oXml.IntoElem()

                    Dim doPath As AnimatGUI.DataObjects.Visualization.CameraPath
                    For Each deEntry As DictionaryEntry In m_aryCameraPaths
                        doPath = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Visualization.CameraPath)
                        doPath.SaveData(oXml)
                    Next

                    oXml.OutOfElem() 'Outof Waypoints Element
                End If

                oXml.OutOfElem()

            End Sub

#Region " Treeview/Menu Methods "

            Public Overrides Sub CreateWorkspaceTreeView(ByVal doParent As Framework.DataObject, _
                                                           ByVal tnParentNode As Crownwood.DotNetMagic.Controls.Node, _
                                                           Optional ByVal bRootObject As Boolean = False)
                MyBase.CreateWorkspaceTreeView(doParent, tnParentNode, bRootObject)

                Dim doPath As AnimatGUI.DataObjects.Visualization.CameraPath
                For Each deEntry As DictionaryEntry In m_aryCameraPaths
                    doPath = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Visualization.CameraPath)
                    doPath.CreateWorkspaceTreeView(Me.FormHelper, m_tnWorkspaceNode)
                Next

            End Sub

            Public Overrides Function CreateObjectListTreeView(ByVal doParent As Framework.DataObject, _
                                                           ByVal tnParentNode As Crownwood.DotNetMagic.Controls.Node, _
                                                           ByVal mgrImageList As AnimatGUI.Framework.ImageManager) As Crownwood.DotNetMagic.Controls.Node
                Dim tnNode As Crownwood.DotNetMagic.Controls.Node = MyBase.CreateObjectListTreeView(doParent, tnParentNode, mgrImageList)

                If m_aryCameraPaths.Count > 0 Then
                    Dim doPath As AnimatGUI.DataObjects.Visualization.CameraPath
                    For Each deEntry As DictionaryEntry In m_aryCameraPaths
                        doPath = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Visualization.CameraPath)
                        doPath.CreateObjectListTreeView(Me.FormHelper, tnNode, mgrImageList)
                    Next
                End If

                Return tnNode
            End Function

            Public Overrides Function WorkspaceTreeviewPopupMenu(ByRef tnSelectedNode As Crownwood.DotNetMagic.Controls.Node, ByVal ptPoint As Point) As Boolean

                Dim doPath As DataObjects.Visualization.CameraPath
                For Each deEntry As DictionaryEntry In m_aryCameraPaths
                    doPath = DirectCast(deEntry.Value, Visualization.CameraPath)
                    If doPath.WorkspaceTreeviewPopupMenu(tnSelectedNode, ptPoint) Then Return True
                Next

                If tnSelectedNode Is m_tnWorkspaceNode Then
                    Dim mcExpandAll As New System.Windows.Forms.ToolStripMenuItem("Expand All", Util.Application.ToolStripImages.GetImage("AnimatGUI.Expand.gif"), New EventHandler(AddressOf Me.OnExpandAll))
                    Dim mcCollapseAll As New System.Windows.Forms.ToolStripMenuItem("Collapse All", Util.Application.ToolStripImages.GetImage("AnimatGUI.Collapse.gif"), New EventHandler(AddressOf Me.OnCollapseAll))

                    Dim mcAddCameraPath As New System.Windows.Forms.ToolStripMenuItem("New Camera Path", Util.Application.ToolStripImages.GetImage("AnimatGUI.AddCameraPath.gif"), New EventHandler(AddressOf Me.AddCameraPathToolStripButton_Click))

                    mcExpandAll.Tag = tnSelectedNode
                    mcCollapseAll.Tag = tnSelectedNode

                    ' Create the popup menu object
                    Dim popup As New AnimatContextMenuStrip("AnimatGUI.DataObjects.Physical.Environment.WorkspaceTreeviewPopupMenu", Util.SecurityMgr)
                    popup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {mcExpandAll, mcCollapseAll, mcAddCameraPath})

                    Util.ProjectWorkspace.ctrlTreeView.ContextMenuNode = popup
                    Return True
                End If

                Return False
            End Function


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

                        If bpPart.IsMovable Then
                            cboBodyPart.Items.Add(bpPart)

                            If Not m_doBodyPart Is Nothing AndAlso bpPart Is m_doBodyPart Then
                                Me.cboBodyPart.SelectedItem = bpPart
                            End If
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

            Protected Overridable Sub OnStructurePropertyChanged(ByVal doObject As AnimatGUI.Framework.DataObject, ByVal propInfo As System.Reflection.PropertyInfo)
                Try
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
                        If Not m_doStructure Is Nothing AndAlso Not m_doStructure.RootBody Is Nothing AndAlso Not doObject Is m_doStructure.RootBody Then
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

#End Region

#End Region

#Region " Events "

            Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)

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

                        Dim bpPart As AnimatGUI.DataObjects.Physical.BodyPart
                        If (Not Me.DefaultLinkedPart Is Nothing AndAlso Me.DefaultLinkedPart.BodyPart Is Nothing) AndAlso (m_strDefaultBodyPartID.Length > 0) Then
                            bpPart = DirectCast(Util.Simulation.FindObjectByID(m_strDefaultBodyPartID), DataObjects.Physical.BodyPart)

                            If Not bpPart Is Nothing Then
                                Me.DefaultLinkedStructure = New TypeHelpers.LinkedStructureList(bpPart.ParentStructure, TypeHelpers.LinkedStructureList.enumStructureType.All)
                                Me.DefaultLinkedPart = New TypeHelpers.LinkedBodyPartList(bpPart.ParentStructure, bpPart, GetType(AnimatGUI.DataObjects.Physical.BodyPart))
                            End If
                        End If

                        'Do this after settign the body part. Body part structure has precedence
                        Dim doStruct As AnimatGUI.DataObjects.Physical.PhysicalStructure
                        If (Not Me.DefaultLinkedStructure Is Nothing AndAlso Me.DefaultLinkedStructure.PhysicalStructure Is Nothing) AndAlso (m_strDefaultStructureID.Length > 0) Then
                            doStruct = DirectCast(Util.Simulation.FindObjectByID(m_strDefaultStructureID), DataObjects.Physical.PhysicalStructure)

                            If Not doStruct Is Nothing Then
                                Me.DefaultLinkedStructure = New TypeHelpers.LinkedStructureList(doStruct, TypeHelpers.LinkedStructureList.enumStructureType.All)
                            End If
                        End If

                        Dim strWinXml As String = GenerateSimWindowXml()
                        Util.Application.SimulationInterface.AddWindow(Me.Handle, "ScriptedSimWindow", strWinXml)
                        InitializeSimulationReferences()

                        GenerateStructureDropDown()
                        GenerateBodyPartDropDown()
                    End If

                    'Reset the visual mode to be collisions.
                    Util.Simulation.VisualSelectionMode = DataObjects.Simulation.enumVisualSelectionMode.SelectCollisions

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

            Private Sub OnAfterRemoveLinkedStructure(ByRef doObject As Framework.DataObject)
                Try
                    Me.DefaultLinkedStructure = New TypeHelpers.LinkedStructureList(Nothing, TypeHelpers.LinkedStructureList.enumStructureType.All)
                Catch ex As Exception
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End Try
            End Sub

            Private Sub OnAfterRemoveLinkedPart(ByRef doObject As Framework.DataObject)
                Try
                    Me.DefaultLinkedPart = New TypeHelpers.LinkedBodyPartList(Me.DefaultLinkedStructure.PhysicalStructure, Nothing, GetType(AnimatGUI.DataObjects.Physical.BodyPart))
                Catch ex As Exception
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


            Protected Sub OnBodyPartPasteStarting()
                Me.PasteToolStripMenuItem.Checked = True
                Me.PasteToolStripButton.Checked = True
            End Sub

            Protected Sub OnBodyPartPasteEnding()
                Me.PasteToolStripMenuItem.Checked = False
                Me.PasteToolStripButton.Checked = False
            End Sub

            Private Sub AddCameraPathToolStripButton_Click(sender As Object, e As System.EventArgs) Handles AddCameraPathToolStripButton.Click
                Try
                    Dim doCamera As New DataObjects.Visualization.CameraPath(Me.FormHelper)
                    doCamera.Name = "Path"
                    m_aryCameraPaths.Add(doCamera.ID, doCamera)
                    doCamera.CreateWorkspaceTreeView(Me.FormHelper, m_tnWorkspaceNode)
                    doCamera.SelectItem(False)
                Catch ex As System.Exception
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End Try
            End Sub

            Public Sub AddWaypointToolStripButton_Click(sender As Object, e As System.EventArgs) Handles AddWaypointToolStripButton.Click, AddWaypointToolStripMenuItem.Click
                Try
                    If Not Util.ProjectWorkspace.SelectedDataObject Is Nothing Then
                        Dim doCameraPath As DataObjects.Visualization.CameraPath

                        If Util.IsTypeOf(Util.ProjectWorkspace.SelectedDataObject.GetType(), GetType(DataObjects.Visualization.CameraPath), False) Then
                            doCameraPath = DirectCast(Util.ProjectWorkspace.SelectedDataObject, DataObjects.Visualization.CameraPath)
                        ElseIf Util.IsTypeOf(Util.ProjectWorkspace.SelectedDataObject.GetType(), GetType(DataObjects.Visualization.Waypoint), False) Then
                            Dim doWP As DataObjects.Visualization.Waypoint = DirectCast(Util.ProjectWorkspace.SelectedDataObject, DataObjects.Visualization.Waypoint)
                            doCameraPath = DirectCast(doWP.Parent, DataObjects.Visualization.CameraPath)
                        Else
                            Throw New System.Exception("You must have a camera path or waypoint selected in order to add a waypoint.")
                        End If

                        Dim doWaypoint As New DataObjects.Visualization.Waypoint(doCameraPath)
                        doWaypoint.Name = doCameraPath.NextWaypointName()

                        If Not m_doInterface Is Nothing Then
                            doWaypoint.Position.X.ActualValue = m_doInterface.GetDataValueImmediate("CameraPositionX")
                            doWaypoint.Position.Y.ActualValue = m_doInterface.GetDataValueImmediate("CameraPositionY")
                            doWaypoint.Position.Z.ActualValue = m_doInterface.GetDataValueImmediate("CameraPositionZ")
                        End If

                        doCameraPath.AddWaypointSetTimes(doWaypoint)
                        doWaypoint.CreateWorkspaceTreeView(doCameraPath, doCameraPath.WorkspaceNode)
                        doWaypoint.SelectItem(False)
                    End If

                Catch ex As System.Exception
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End Try
            End Sub

            'These three events handlers are called whenever a user manually changes the value of the position or rotation.
            'This is different from the OnPositionChanged event. Those events come up from the simulation.
            Protected Overridable Sub OnDefaultPositionValueChanged()
                Try
                    If Not Util.ProjectProperties Is Nothing Then
                        Me.SetSimData("Position", m_svDefaultPosition.GetSimulationXml("Position"), True)
                        Util.ProjectProperties.RefreshProperties()
                    End If
                Catch ex As System.Exception
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End Try
            End Sub

#End Region

        End Class

    End Namespace
End Namespace



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
Imports AnimatTools
Imports AnimatTools.Framework
Imports AnimatTools.DataObjects
Imports System.Drawing.Printing
Imports System.Drawing.Imaging

Namespace Forms.BodyPlan

    Public Class Editor
        Inherits MdiChild

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
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.SuspendLayout()
            '
            'Editor
            '
            Me.ClientSize = New System.Drawing.Size(984, 562)
            Me.Name = "Editor"
            Me.Text = "Body Plan Editor"
            Me.Title = "Body Plan Editor"
            Me.ResumeLayout(False)

        End Sub

#End Region

#Region " Attributes "

        Protected m_HierarchyBar As Forms.BodyPlan.Hierarchy
        Protected m_HierarchyContent As Content
        Protected m_mcHierarchyBar As MenuCommand

        Protected m_PropertiesBar As Forms.BodyPlan.Properties
        Protected m_PropertiesContent As Content
        Protected m_mcPropertiesBar As MenuCommand

        Protected m_ReceptiveFieldsBar As Forms.BodyPlan.ReceptiveFields
        Protected m_ReceptiveFieldsContent As Content
        Protected m_mcReceptiveFieldsBar As MenuCommand

        'Protected m_ErrorsBar As Forms.Behavior.Errors
        'Protected m_ErrorsContent As Content
        'Protected m_mcErrorsBar As MenuCommand

        Protected m_doStructure As DataObjects.Physical.PhysicalStructure

        Protected m_doSelectedObject As Framework.DataObject
        'Protected m_PrintHelper As New DataObjects.Behavior.PrintHelper
        Protected m_aryToolbarMenus As New SortedList

        Protected m_fltMouseSensitivity As Single = 0.005

        Protected m_mcSelectBodies As MenuCommand
        Protected m_mcSelectJoints As MenuCommand
        Protected m_mcAddBody As MenuCommand
        Protected m_mcSelectReceptiveFields As MenuCommand

#End Region

#Region " Properties "

        Public Overridable Property PhysicalStructure() As DataObjects.Physical.PhysicalStructure
            Get
                Return m_doStructure
            End Get
            Set(ByVal Value As DataObjects.Physical.PhysicalStructure)
                m_doStructure = Value
            End Set
        End Property

        Public Overridable ReadOnly Property PropertiesBar() As Forms.BodyPlan.Properties
            Get
                Return m_PropertiesBar
            End Get
        End Property

        Public Overridable ReadOnly Property HierarchyBar() As Forms.BodyPlan.Hierarchy
            Get
                Return m_HierarchyBar
            End Get
        End Property

        Public Overridable ReadOnly Property ReceptiveFieldsBar() As Forms.BodyPlan.ReceptiveFields
            Get
                Return m_ReceptiveFieldsBar
            End Get
        End Property

        'Public Overridable ReadOnly Property ErrorsBar() As Forms.Behavior.Errors
        '    Get
        '        Return m_ErrorsBar
        '    End Get
        'End Property

        Public Overridable ReadOnly Property BodyView() As Forms.BodyPlan.BodyView
            Get
                Return DirectCast(Me.Control, Forms.BodyPlan.BodyView)
            End Get
        End Property

        Public Overridable Property MouseSensitivity() As Single
            Get
                Return m_fltMouseSensitivity
            End Get
            Set(ByVal Value As Single)
                m_fltMouseSensitivity = Value
            End Set
        End Property

        Public Overridable ReadOnly Property SelectBodiesMenuItem() As MenuCommand
            Get
                Return m_mcSelectBodies
            End Get
        End Property

        Public Overridable ReadOnly Property SelectJointsMenuItem() As MenuCommand
            Get
                Return m_mcSelectJoints
            End Get
        End Property

        Public Overridable ReadOnly Property AddBodyMenuItem() As MenuCommand
            Get
                Return m_mcAddBody
            End Get
        End Property

        Public Overridable ReadOnly Property SelectReceptiveFieldsMenuItem() As MenuCommand
            Get
                Return m_mcSelectReceptiveFields
            End Get
        End Property

#End Region

#Region " Methods "

        Public Overrides Sub Initialize(ByVal frmApplication As AnimatApplication, _
                                        ByVal frmControl As AnimatForm)

            Try
                Util.DisableDirtyFlags = True

                Util.Application = Util.Application

                'Dim afDiagram As AnimatForm = Util.Application.CreateForm(Util.Application.BodyEditorDll, _
                '                                                        Util.Application.BodyEditorNamespace & ".Forms.BodyPlan.BodyViewOSG", _
                '                                                        "Body View", Me, False)
                Dim afDiagram As AnimatForm = New BodyPlan.BodyView
                CreateImageManager()
                MyBase.Initialize(Util.Application, afDiagram)

                m_PropertiesBar = New Forms.BodyPlan.Properties
                m_PropertiesBar.Initialize()

                m_PropertiesBar.Location = New Point(0, 0)
                m_PropertiesContent = m_dockManager.Contents.Add(m_PropertiesBar, "Properties", m_mgrToolStripImages.ImageList, m_mgrToolStripImages.GetImageIndex("AnimatTools.Properties.gif"))
                m_PropertiesContent.DisplaySize = New Size(200, 150)
                m_PropertiesContent.UserData = m_PropertiesBar
                'm_PropertiesBar.Content = m_PropertiesContent

                Dim wndContent As Crownwood.Magic.Docking.WindowContent = m_dockManager.AddContentWithState(m_PropertiesContent, State.DockLeft)

                m_HierarchyBar = New Forms.BodyPlan.Hierarchy
                m_HierarchyBar.Initialize()

                m_HierarchyBar.Location = New Point(0, 0)
                m_HierarchyContent = m_dockManager.Contents.Add(m_HierarchyBar, "Hierarchy", m_mgrToolStripImages.ImageList, m_mgrToolStripImages.GetImageIndex("AnimatTools.ProjectWorkspace.gif"))
                m_HierarchyContent.DisplaySize = New Size(200, 150)
                m_HierarchyContent.UserData = m_HierarchyBar
                'm_HierarchyBar.Content = m_HierarchyContent

                m_dockManager.AddContentToZone(m_HierarchyContent, wndContent.ParentZone, 0)

                'm_ReceptiveFieldsBar = DirectCast(Util.Application.CreateForm(Util.Application.BodyEditorDll, _
                '                                    Util.Application.BodyEditorNamespace & ".Forms.BodyPlan.ReceptiveFields", _
                '                                    "Receptive Fields", Me, False), AnimatTools.Forms.BodyPlan.ReceptiveFields)
                'm_ReceptiveFieldsBar.Initialize(Util.Application, Me)

                'm_ReceptiveFieldsBar.Location = New Point(0, 0)
                'm_ReceptiveFieldsContent = m_dockManager.Contents.Add(m_ReceptiveFieldsBar, "Receptive Fields", m_mgrToolStripImages.ImageList, m_mgrToolStripImages.GetImageIndex("AnimatTools.ReceptiveField.gif"))
                'm_ReceptiveFieldsContent.DisplaySize = New Size(200, 150)
                'm_ReceptiveFieldsContent.UserData = m_ReceptiveFieldsBar
                'm_ReceptiveFieldsBar.Content = m_ReceptiveFieldsContent

                'm_dockManager.AddContentToWindowContent(m_ReceptiveFieldsContent, wndContent)

                If Not Me.PhysicalStructure Is Nothing AndAlso Not TypeOf Me.PhysicalStructure Is DataObjects.Physical.Organism Then
                    'm_dockManager.HideContent(m_ReceptiveFieldsContent)
                End If


                'm_ErrorsBar = New Forms.Behavior.Errors
                'm_ErrorsBar.Initialize(Util.Application, Me)

                'm_ErrorsBar.Location = New Point(0, 0)
                'm_ErrorsContent = m_dockManager.Contents.Add(m_ErrorsBar, "Neural Network Errors")
                'm_ErrorsContent.DisplaySize = New Size(250, 150)
                'm_ErrorsContent.UserData = m_ErrorsBar

                'wndContent = m_dockManager.AddContentWithState(m_ErrorsContent, State.DockBottom)

                AddHandler m_dockManager.ContentClosing, AddressOf Me.OnDockContentClosing

                m_aryToolbarMenus.Clear()
                m_aryToolbarMenus.Add(m_HierarchyContent.Title, m_mcHierarchyBar)
                m_aryToolbarMenus.Add(m_PropertiesContent.Title, m_mcPropertiesBar)

                If Me.PhysicalStructure Is Nothing OrElse (Not Me.PhysicalStructure Is Nothing AndAlso TypeOf Me.PhysicalStructure Is DataObjects.Physical.Organism) Then
                    'm_aryToolbarMenus.Add(m_ReceptiveFieldsContent.Title, m_mcReceptiveFieldsBar)
                End If
                'm_aryToolbarMenus.Add(m_ErrorsContent.Title, m_mcErrorsBar)

                Dim myAssembly As System.Reflection.Assembly
                myAssembly = System.Reflection.Assembly.Load("AnimatTools")
                Me.Icon = Util.Application.ToolStripImages.LoadIcon(myAssembly, "AnimatTools.Joint.ico")

                If Not m_doStructure Is Nothing Then
                    m_doStructure.BodyEditor = Me
                    'm_doStructure.CreateBodyPlanTreeView(Util.Simulation, m_HierarchyBar)
                End If

                afDiagram.OnAfterMdiParentInitialized()
                m_HierarchyBar.OnAfterMdiParentInitialized()
                m_PropertiesBar.OnAfterMdiParentInitialized()
                'm_ReceptiveFieldsBar.OnAfterMdiParentInitialized()

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            Finally
                Util.DisableDirtyFlags = False
            End Try

        End Sub

#Region " Menu/Toolbar Methods "

        Protected Overrides Sub CreateToolstrips()
            'If Util.Application Is Nothing Then Throw New System.Exception("Application object is not defined.")
            'If m_frmControl Is Nothing Then Throw New System.Exception("Mdi Control object is not defined.")

            'm_menuMain = Util.Application.CreateDefaultMenu()

            ''Dim mcFile As MenuCommand = m_menuMain.MenuCommands("File")

            ''Dim mcSep1 As MenuCommand = New MenuCommand("-")
            ''Dim mcExport As New MenuCommand("Export", "Export", m_mgrToolStripImages.ImageList, _
            ''                                  m_mgrToolStripImages.GetImageIndex("AnimatTools.Export.gif"), _
            ''                                  New EventHandler(AddressOf Me.OnExport))

            ''Dim mcPageSetup As New MenuCommand("Page Setup", "PageSetup", m_mgrToolStripImages.ImageList, _
            ''                                  m_mgrToolStripImages.GetImageIndex("AnimatTools.PageSetup.gif"), _
            ''                                  New EventHandler(AddressOf Me.OnPageSetup))

            ''Dim mcPrintPreview As New MenuCommand("Print Preview", "PrintPreview", m_mgrToolStripImages.ImageList, _
            ''                                  m_mgrToolStripImages.GetImageIndex("AnimatTools.PrintPreview.gif"), _
            ''                                  New EventHandler(AddressOf Me.OnPrintPreview))

            ''Dim mcPrint As New MenuCommand("Print", "Print", m_mgrToolStripImages.ImageList, _
            ''                                  m_mgrToolStripImages.GetImageIndex("AnimatTools.Print.gif"), _
            ''                                  New EventHandler(AddressOf Me.OnPrint))

            ''mcFile.MenuCommands.AddRange(New MenuCommand() {mcSep1, mcExport, mcPageSetup, mcPrintPreview, mcPrint})



            'Dim mcView As MenuCommand = m_menuMain.MenuCommands("View")
            'Dim mcSep2 As MenuCommand = New MenuCommand("-")

            'Dim mcToolbars As MenuCommand = New MenuCommand("Toolbars")
            'm_mcHierarchyBar = New MenuCommand("Hierarchy", "Hierarchy", New EventHandler(AddressOf Me.OnViewHierarchyBar))
            'm_mcPropertiesBar = New MenuCommand("Properties", "Properties", New EventHandler(AddressOf Me.OnViewPropertiesBar))
            'm_mcReceptiveFieldsBar = New MenuCommand("Receptive Fields", "ReceptiveFields", New EventHandler(AddressOf Me.OnViewReceptiveFieldsBar))
            ''m_mcErrorsBar = New MenuCommand("Errors", "Errors", New EventHandler(AddressOf Me.OnViewErrorsBar))

            'm_mcHierarchyBar.Checked = True
            'm_mcPropertiesBar.Checked = True
            'm_mcReceptiveFieldsBar.Checked = True

            'mcView.MenuCommands.AddRange(New MenuCommand() {mcToolbars})


            'Dim mcEdit As MenuCommand = m_menuMain.MenuCommands("Edit")

            'Dim mcAddStim As New MenuCommand("Add Stimulus", "AddStimulus", m_mgrToolStripImages.ImageList, _
            '                                  m_mgrToolStripImages.GetImageIndex("AnimatTools.AddStimulus.gif"), _
            '                                  New EventHandler(AddressOf Me.OnAddStimulus))

            'm_mcSelectBodies = New MenuCommand("Select Bodies", "SelectBodies", System.Windows.Forms.Shortcut.CtrlB, New EventHandler(AddressOf Me.OnSelectBodies))
            'm_mcSelectBodies.Checked = True
            'm_mcSelectJoints = New MenuCommand("Select Joints", "SelectJoints", System.Windows.Forms.Shortcut.CtrlJ, New EventHandler(AddressOf Me.OnSelectJoints))
            'm_mcAddBody = New MenuCommand("Add Body", "AddBody", System.Windows.Forms.Shortcut.CtrlA, New EventHandler(AddressOf Me.OnAddBody))
            'm_mcSelectReceptiveFields = New MenuCommand("Select Receptive Fields", "SelectReceptiveFields", System.Windows.Forms.Shortcut.CtrlR, New EventHandler(AddressOf Me.OnSelectReceptiveFields))
            'Dim mcSepEditStart1 As MenuCommand = New MenuCommand("-")


            'Dim mcSepEditStart As MenuCommand = New MenuCommand("-")
            'Dim mcCut As New MenuCommand("Cut", "Cut", m_mgrToolStripImages.ImageList, _
            '                             m_mgrToolStripImages.GetImageIndex("AnimatTools.Cut.gif"), _
            '                             Shortcut.CtrlX, New EventHandler(AddressOf Me.OnCut))
            'Dim mcCopy As New MenuCommand("Copy", "Copy", m_mgrToolStripImages.ImageList, _
            '                                m_mgrToolStripImages.GetImageIndex("AnimatTools.Copy.gif"), _
            '                                Shortcut.CtrlC, New EventHandler(AddressOf Me.OnCopy))
            'Dim mcPaste As New MenuCommand("Paste", "Paste", m_mgrToolStripImages.ImageList, _
            '                                m_mgrToolStripImages.GetImageIndex("AnimatTools.Paste.gif"), _
            '                                Shortcut.CtrlV, New EventHandler(AddressOf Me.OnPaste))
            'Dim mcDelete As New MenuCommand("Delete", "Delete", m_mgrToolStripImages.ImageList, _
            '                                m_mgrToolStripImages.GetImageIndex("AnimatTools.Delete.gif"), _
            '                                Shortcut.Del, New EventHandler(AddressOf Me.OnDelete))

            'Dim mcSepFindStart As MenuCommand = New MenuCommand("-")
            'Dim mcSelectByType As New MenuCommand("Select By Type", "SelectByType", m_mgrToolStripImages.ImageList, _
            '                                  m_mgrToolStripImages.GetImageIndex("AnimatTools.SelectByType.gif"), _
            '                                  New EventHandler(AddressOf Me.OnSelectByType))
            'Dim mcRelabel As New MenuCommand("Relabel", "Relabel", m_mgrToolStripImages.ImageList, _
            '                                  m_mgrToolStripImages.GetImageIndex("AnimatTools.Relabel.gif"), _
            '                                  New EventHandler(AddressOf Me.OnRelabel))
            'Dim mcRelabelSelected As New MenuCommand("Relabel Selected", "RelabelSelected", m_mgrToolStripImages.ImageList, _
            '                                  m_mgrToolStripImages.GetImageIndex("AnimatTools.RelabelSelected.gif"), _
            '                                  New EventHandler(AddressOf Me.OnRelabelSelected))
            'Dim mcCompareItems As New MenuCommand("Compare Items", "CompareItems", m_mgrToolStripImages.ImageList, _
            '                      m_mgrToolStripImages.GetImageIndex("AnimatTools.Equals.gif"), _
            '                      New EventHandler(AddressOf Me.OnCompareItems))


            'If TypeOf Me.PhysicalStructure Is DataObjects.Physical.Organism Then
            '    mcEdit.MenuCommands.AddRange(New MenuCommand() {mcAddStim, m_mcSelectBodies, m_mcSelectJoints, m_mcAddBody, m_mcSelectReceptiveFields, _
            '                                                    mcSepEditStart, mcCut, mcCopy, mcPaste, mcDelete, mcSepFindStart, mcSelectByType, mcRelabel, mcRelabelSelected, mcCompareItems})
            'Else
            '    mcEdit.MenuCommands.AddRange(New MenuCommand() {mcAddStim, m_mcSelectBodies, m_mcSelectJoints, m_mcAddBody, _
            '                                                    mcSepEditStart, mcCut, mcCopy, mcPaste, mcDelete, mcSepFindStart, mcSelectByType, mcRelabel, mcRelabelSelected, mcCompareItems})
            'End If

            'AddHandler mcEdit.PopupStart, AddressOf Me.OnEditPopupStart
            'AddHandler mcEdit.PopupEnd, AddressOf Me.OnEditPopupEnd

        End Sub

        'Protected Overrides Sub CreateToolbar()
        '    'If Util.Application Is Nothing Then Throw New System.Exception("Application object is not defined.")
        '    'If m_frmControl Is Nothing Then Throw New System.Exception("Mdi Control object is not defined.")

        '    'm_barMain = Util.Application.CreateDefaultToolbar(m_menuMain)

        '    'Dim btnAddStimulus As New ToolBarButton
        '    'btnAddStimulus.ImageIndex = Util.Application.LargeImages.GetImageIndex("AnimatTools.AddStimulus.gif")
        '    'btnAddStimulus.ToolTipText = "Add Stimulus"

        '    'Dim btnDelete As New ToolBarButton
        '    'btnDelete.ImageIndex = Util.Application.LargeImages.GetImageIndex("AnimatTools.Delete.gif")
        '    'btnDelete.ToolTipText = "Delete"

        '    'Dim btnSelectByType As New ToolBarButton
        '    'btnSelectByType.ImageIndex = Util.Application.LargeImages.GetImageIndex("AnimatTools.SelectByType.gif")
        '    'btnSelectByType.ToolTipText = "Select Items by Object Type"

        '    'Dim btnRelabel As New ToolBarButton
        '    'btnRelabel.ImageIndex = Util.Application.LargeImages.GetImageIndex("AnimatTools.Relabel.gif")
        '    'btnRelabel.ToolTipText = "Relabel items using regular expressions."

        '    'Dim btnRelabelSelected As New ToolBarButton
        '    'btnRelabelSelected.ImageIndex = Util.Application.LargeImages.GetImageIndex("AnimatTools.RelabelSelected.gif")
        '    'btnRelabelSelected.ToolTipText = "Relabel and number selected items"

        '    'Dim btnCompareItem As New ToolBarButton
        '    'btnCompareItem.ImageIndex = Util.Application.LargeImages.GetImageIndex("AnimatTools.Equals.gif")
        '    'btnCompareItem.ToolTipText = "Compare Items"

        '    'm_barMain.Buttons.AddRange(New ToolBarButton() {btnAddStimulus, btnDelete, btnSelectByType, btnRelabel, btnRelabelSelected, btnCompareItem})

        '    'm_barMain.ButtonManager.SetButtonMenuItem(btnAddStimulus, m_menuMain.MenuCommands.FindMenuCommand("AddStimulus"))
        '    'm_barMain.ButtonManager.SetButtonMenuItem(btnDelete, m_menuMain.MenuCommands.FindMenuCommand("Delete"))
        '    'm_barMain.ButtonManager.SetButtonMenuItem(btnSelectByType, m_menuMain.MenuCommands.FindMenuCommand("SelectByType"))
        '    'm_barMain.ButtonManager.SetButtonMenuItem(btnRelabel, m_menuMain.MenuCommands.FindMenuCommand("Relabel"))
        '    'm_barMain.ButtonManager.SetButtonMenuItem(btnRelabelSelected, m_menuMain.MenuCommands.FindMenuCommand("RelabelSelected"))
        '    'm_barMain.ButtonManager.SetButtonMenuItem(btnCompareItem, m_menuMain.MenuCommands.FindMenuCommand("CompareItems"))

        'End Sub

        Protected Overridable Sub CreateImageManager()

            Try

                Dim myAssembly As System.Reflection.Assembly
                myAssembly = System.Reflection.Assembly.Load("AnimatTools")

                m_mgrToolStripImages = New AnimatTools.Framework.ImageManager
                m_mgrToolStripImages.ImageList.ImageSize = New Size(16, 16)
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatTools.ZoomIn.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatTools.ZoomOut.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatTools.PageSetup.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatTools.PrintPreview.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatTools.Print.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatTools.Export.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatTools.Swap.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatTools.Cut.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatTools.Copy.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatTools.Paste.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatTools.Delete.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatTools.SendToBack.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatTools.BringToFront.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatTools.Undo.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatTools.UndoGrey.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatTools.Redo.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatTools.RedoGrey.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatTools.ExternalStimulus.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatTools.ProjectWorkspace.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatTools.Wrench_Small.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatTools.Properties.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatTools.ReceptiveField.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatTools.AddStimulus.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatTools.SelectByType.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatTools.Relabel.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatTools.RelabelSelected.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatTools.Equals.gif")

                Util.Application.LargeImages.AddImage(myAssembly, "AnimatTools.ZoomInLarge.gif")
                Util.Application.LargeImages.AddImage(myAssembly, "AnimatTools.ZoomOutLarge.gif")
                Util.Application.LargeImages.AddImage(myAssembly, "AnimatTools.AddStimulus.gif")
                Util.Application.LargeImages.AddImage(myAssembly, "AnimatTools.Delete.gif")
                Util.Application.LargeImages.AddImage(myAssembly, "AnimatTools.SelectByType.gif")
                Util.Application.LargeImages.AddImage(myAssembly, "AnimatTools.Relabel.gif")
                Util.Application.LargeImages.AddImage(myAssembly, "AnimatTools.RelabelSelected.gif")
                Util.Application.LargeImages.AddImage(myAssembly, "AnimatTools.Equals.gif")

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

        Public Overrides Sub UndoRedoRefresh(ByVal oRefresh As Object)

            If Not oRefresh Is Nothing Then
                If TypeOf oRefresh Is AnimatTools.DataObjects.Physical.BodyPart Then
                    Me.PropertiesBar.SelectPart(DirectCast(oRefresh, AnimatTools.DataObjects.Physical.BodyPart), False)
                Else
                    Me.PropertiesBar.RefreshProperties()
                End If
            End If

        End Sub

        Public Overrides Sub RefreshProperties()
            Me.PropertiesBar.RefreshProperties()
        End Sub

        Public Overridable Sub ZoomBy(ByVal fltDelta As Single)

            Try

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overridable Sub ZoomTo(ByVal fltZoom As Single)

            Try

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Overridable Sub ViewToolbar(ByVal ctDock As Crownwood.Magic.Docking.Content)

            Try
                Dim mcCommand As MenuCommand

                If Not ctDock Is Nothing Then
                    If m_aryToolbarMenus.Contains(ctDock.Title) Then
                        mcCommand = DirectCast(m_aryToolbarMenus(ctDock.Title), MenuCommand)
                    End If

                    If ctDock.Visible Then
                        m_dockManager.HideContent(ctDock)
                        If Not mcCommand Is Nothing Then mcCommand.Checked = False
                    Else
                        m_dockManager.ShowContent(ctDock)
                        If Not mcCommand Is Nothing Then mcCommand.Checked = True
                    End If
                End If

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

        End Sub

#Region " Load/Save "

        Public Overrides Sub LoadData(ByRef oXml As AnimatTools.Interfaces.StdXml)

            oXml.IntoElem() 'Into Child Element

            Dim strEditorFile As String = oXml.GetChildString("EditorFile")
            LoadEditorFile(strEditorFile)

            oXml.OutOfElem() 'Outof Child Element

        End Sub

        Public Overridable Overloads Sub LoadEditorFile(ByVal strFilename As String)
            Try
                Util.DisableDirtyFlags = True

                Dim oXml As New AnimatTools.Interfaces.StdXml

                oXml.Load(Util.GetFilePath(Util.Application.ProjectPath, strFilename))
                oXml.FindElement("Editor")

                LoadEditorFile(oXml)
                Util.Simulation.CreatedInSim(True)  'Make sure to retag the loaded items so they know they are already created in the simulation.

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            Finally
                Util.DisableDirtyFlags = False
            End Try

        End Sub

        Protected Overridable Overloads Sub LoadEditorFile(ByRef oXml As AnimatTools.Interfaces.StdXml)

            Try
                If Util.Application.Simulation Is Nothing Then
                    Throw New System.Exception("No simulation has been loaded for this project. You can not open a body plan editor without a valid simulation object.")
                End If

                LoadWindowSize(oXml)

                Dim strStructure As String = oXml.GetChildString("Structure")
                Me.PhysicalStructure = DirectCast(Util.Application.Simulation.Environment.FindStructureFromAll(strStructure), DataObjects.Physical.PhysicalStructure)
                m_doStructure.BodyEditor = Me

                'If this is not an organism type of structure then lets hide all of the receptive field stuff. We could not do 
                'this before because we call iniatialize before we call Load, and it is in load that we determine if it is an organism or not.
                'If Not Me.PhysicalStructure Is Nothing AndAlso Not TypeOf Me.PhysicalStructure Is DataObjects.Physical.Organism Then
                '    If m_aryToolbarMenus.Contains(m_ReceptiveFieldsContent.Title) Then
                '        m_aryToolbarMenus.Remove(m_ReceptiveFieldsContent.Title)
                '    End If
                '    If m_menuMain.MenuCommands("View").MenuCommands("Toolbars").MenuCommands.Contains(m_mcReceptiveFieldsBar) Then
                '        m_menuMain.MenuCommands("View").MenuCommands("Toolbars").MenuCommands.Remove(m_mcReceptiveFieldsBar)
                '    End If
                '    m_dockManager.HideContent(m_ReceptiveFieldsContent)
                'End If

                If Not m_doStructure Is Nothing Then
                    'm_doStructure.CreateBodyPlanTreeView(Util.Simulation, m_HierarchyBar)
                End If

                Me.MouseSensitivity = oXml.GetChildFloat("MouseSensitivity", m_fltMouseSensitivity)
                Me.BodyView.LoadData(oXml)

                'Util.Application.LoadDockingConfig(m_dockManager, oXml)
                SynchronizeToolbarMenu(m_aryToolbarMenus)

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Public Overrides Sub SaveData(ByRef oXml As AnimatTools.Interfaces.StdXml)

            oXml.AddChildElement("Child")
            oXml.IntoElem() 'Into Child Elemement

            oXml.AddChildElement("AssemblyFile", Me.AssemblyFile)
            oXml.AddChildElement("ClassName", Me.ClassName)
            oXml.AddChildElement("EditorFile", Me.PhysicalStructure.BodyPlanEditorFile)

            oXml.AddChildElement("Form")
            oXml.IntoElem() 'Into Form Element

            oXml.AddChildElement("AssemblyFile", "LicensedAnimatTools.dll")
            oXml.AddChildElement("ClassName", "LicensedAnimatTools.Forms.Behavior.AddFlowDiagram")
            oXml.AddChildElement("Title", Me.Title)

            oXml.OutOfElem()  'Outof Form Element

            oXml.OutOfElem()  'Outof Child Element
        End Sub

        Protected Overridable Overloads Sub SaveEditorFile(ByVal strFilename As String)

            Try
                Dim oXml As New AnimatTools.Interfaces.StdXml

                SaveEditorFile(oXml)
                oXml.Save(Util.GetFilePath(Util.Application.ProjectPath, strFilename))

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Overloads Sub SaveEditorFile(ByRef oXml As AnimatTools.Interfaces.StdXml)

            Try
                oXml.AddElement("Editor")

                oXml.AddChildElement("AssemblyFile", Me.AssemblyFile)
                oXml.AddChildElement("ClassName", Me.ClassName)

                SaveWindowSize(oXml)

                If Not m_doStructure Is Nothing Then
                    oXml.AddChildElement("Structure", m_doStructure.ID)
                End If

                oXml.AddChildElement("MouseSensitivity", m_fltMouseSensitivity)
                Me.BodyView.SaveData(oXml)

                'Util.Application.SaveDockingConfig(m_dockManager, oXml)

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

#End Region

#Region " Events "

        Public Sub OnEditPopupStart(ByVal mc As MenuCommand)
            If Me.PropertiesBar.SelectedParts.Count > 0 Then
                mc.MenuCommands("Cut").Enabled = True
                mc.MenuCommands("Copy").Enabled = True
                mc.MenuCommands("Delete").Enabled = True
            Else
                mc.MenuCommands("Cut").Enabled = False
                mc.MenuCommands("Copy").Enabled = False
                mc.MenuCommands("Delete").Enabled = False
            End If

            mc.MenuCommands("Paste").Enabled = False
            Dim data As IDataObject = Clipboard.GetDataObject()
            If Not data Is Nothing AndAlso data.GetDataPresent("AnimatLab.Body.XMLFormat") Then
                Dim strXml As String = DirectCast(data.GetData("AnimatLab.Body.XMLFormat"), String)
                If strXml.Trim.Length > 0 Then
                    mc.MenuCommands("Paste").Enabled = True
                End If
            End If

            mc.MenuCommands("Undo").Enabled = Util.ModificationHistory.CanUndo
            mc.MenuCommands("Redo").Enabled = Util.ModificationHistory.CanRedo

        End Sub

        Public Sub OnEditPopupEnd(ByVal mc As MenuCommand)
            mc.MenuCommands("Undo").Enabled = True
            mc.MenuCommands("Redo").Enabled = True
            mc.MenuCommands("Cut").Enabled = True
            mc.MenuCommands("Copy").Enabled = True
            mc.MenuCommands("Delete").Enabled = True
            mc.MenuCommands("Paste").Enabled = True
        End Sub

        Protected Sub OnOpen(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnSave(ByVal sender As Object, ByVal e As System.EventArgs)

            Try


            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Sub OnCut(ByVal sender As Object, ByVal e As System.EventArgs)

            Try

                Util.CutInProgress = True
                If Me.PropertiesBar.SelectedParts.Count = 1 AndAlso Not Me.PropertiesBar.SelectedPart Is Nothing AndAlso _
                   TypeOf Me.PropertiesBar.SelectedPart Is AnimatTools.DataObjects.Physical.RigidBody Then
                    Me.PropertiesBar.SelectedPart.CopyBodyPart(True)
                End If
                Util.CutInProgress = False

                If Me.PropertiesBar.SelectedParts.Count = 1 AndAlso Not Me.PropertiesBar.SelectedPart Is Nothing AndAlso _
                    TypeOf Me.PropertiesBar.SelectedPart Is AnimatTools.DataObjects.Physical.RigidBody Then
                    Me.PropertiesBar.SelectedPart.Delete(False)
                End If

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            Finally
                Util.CutInProgress = False
            End Try

        End Sub

        Public Sub OnCopy(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                'You can only do a copy when you have ONE object selected. NO MORE!!
                Util.CopyInProgress = True
                If Me.PropertiesBar.SelectedParts.Count = 1 AndAlso Not Me.PropertiesBar.SelectedPart Is Nothing AndAlso _
                   TypeOf Me.PropertiesBar.SelectedPart Is AnimatTools.DataObjects.Physical.RigidBody Then
                    Me.PropertiesBar.SelectedPart.CopyBodyPart(False)
                End If

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            Finally
                Util.CopyInProgress = False
            End Try

        End Sub

        Public Sub OnPaste(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                'Me.CommandBar.CommandMode = Command.enumCommandMode.PasteBodies
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Sub OnDelete(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If Me.PropertiesBar.SelectedParts.Count > 0 Then
                    Dim strMessage As String = ""
                    If Me.PropertiesBar.SelectedParts.Count = 1 Then
                        strMessage = "Are you certain that you want to permanently delete this body part and all its children?"
                    ElseIf Me.PropertiesBar.SelectedParts.Count > 1 Then
                        strMessage = "Are you certain that you want to permanently delete these body parts and all their children?"
                    Else
                        Return
                    End If

                    If MessageBox.Show(strMessage, "Delete Body Parts", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                        Dim aryParts As AnimatTools.Collections.BodyParts = DirectCast(Me.PropertiesBar.SelectedParts.Copy(), AnimatTools.Collections.BodyParts)

                        Util.ModificationHistory.BeginHistoryGroup()

                        For Each doPart As AnimatTools.DataObjects.Physical.BodyPart In aryParts
                            doPart.Delete(False)
                        Next

                        Util.ModificationHistory.CommitHistoryGroup()
                    End If
                End If

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
                Util.ModificationHistory.AbortHistoryGroup()
            End Try

        End Sub

        Public Sub OnAddStimulus(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If Not Me.PropertiesBar.SelectedPart Is Nothing AndAlso Me.PropertiesBar.SelectedPart.AllowStimulus AndAlso Me.PropertiesBar.SelectedPart.CompatibleStimuli.Count > 0 Then
                    Me.PropertiesBar.SelectedPart.SelectStimulusType()
                End If
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Sub OnSelectBodies(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                'Me.CommandBar.CommandMode = Command.enumCommandMode.SelectBodies
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Sub OnSelectJoints(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                'Me.CommandBar.CommandMode = Command.enumCommandMode.SelectJoints
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Sub OnAddBody(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                'Me.CommandBar.CommandMode = Command.enumCommandMode.AddBodies
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Sub OnSelectReceptiveFields(ByVal sender As Object, ByVal e As System.EventArgs)
            Try
                'Me.CommandBar.CommandMode = Command.enumCommandMode.SelectReceptiveFields
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Sub OnSelectByType(ByVal sender As Object, ByVal e As System.EventArgs)
            Try
                Dim frmType As New AnimatTools.Forms.BodyPlan.SelectByType

                frmType.PhysicalStructure = Me.PhysicalStructure
                If frmType.ShowDialog = DialogResult.OK Then

                    Dim colObjects As New AnimatTools.Collections.DataObjects(Nothing)
                    m_doStructure.FindChildrenOfType(frmType.SelectedType, colObjects)

                    Me.PropertiesBar.SelectPart(Nothing, False)

                    Dim doPart As AnimatTools.DataObjects.Physical.BodyPart
                    For Each doData As Framework.DataObject In colObjects
                        If TypeOf doData Is AnimatTools.DataObjects.Physical.BodyPart Then
                            doPart = DirectCast(doData, AnimatTools.DataObjects.Physical.BodyPart)
                            Me.PropertiesBar.SelectPart(doPart, True)
                        End If
                    Next

                End If

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Sub OnRelabel(ByVal sender As Object, ByVal e As System.EventArgs)
            Try
                Dim frmRelabel As New AnimatTools.Forms.BodyPlan.Relabel

                frmRelabel.PhysicalStructure = Me.PhysicalStructure
                If frmRelabel.ShowDialog = DialogResult.OK Then
                    Util.Relable(frmRelabel.Items, frmRelabel.txtMatch.Text, frmRelabel.txtReplace.Text)
                End If

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Sub OnRelabelSelected(ByVal sender As Object, ByVal e As System.EventArgs)
            Try
                Dim frmRelabel As New Forms.RelabelSelected

                If Me.PropertiesBar.SelectedParts.Count > 0 Then
                    If frmRelabel.ShowDialog() = DialogResult.OK Then

                        Dim aryList As New ArrayList
                        For Each doPart As DataObjects.Physical.BodyPart In Me.PropertiesBar.SelectedParts
                            aryList.Add(doPart)
                        Next

                        Util.RelableSelected(aryList, frmRelabel.txtNewLabel.Text, frmRelabel.StartWith, frmRelabel.IncrementBy)
                    End If

                    Me.IsDirty = True
                End If

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub
        Public Sub OnCompareItems(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                Dim frmCompare As New AnimatTools.Forms.Tools.CompareItems
                frmCompare.PhysicalStructure = Me.PhysicalStructure

                frmCompare.SelectedItems().Clear()
                For Each doItem As AnimatTools.Framework.DataObject In m_PropertiesBar.SelectedParts
                    frmCompare.SelectedItems.Add(doItem)
                Next

                frmCompare.ShowDialog()

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#Region " Toolbar View Events "

        Protected Sub OnViewHierarchyBar(ByVal sender As Object, ByVal e As System.EventArgs)
            ViewToolbar(m_HierarchyContent)
        End Sub

        Protected Sub OnViewPropertiesBar(ByVal sender As Object, ByVal e As System.EventArgs)
            ViewToolbar(m_PropertiesContent)
        End Sub

        Protected Sub OnViewReceptiveFieldsBar(ByVal sender As Object, ByVal e As System.EventArgs)
            'ViewToolbar(m_ReceptiveFieldsContent)
        End Sub

#End Region

#Region " Zoom Events "

        Protected Sub OnZoomOutBy10(ByVal sender As Object, ByVal e As System.EventArgs)
            ZoomBy(-0.1)
        End Sub

        Protected Sub OnZoomInBy10(ByVal sender As Object, ByVal e As System.EventArgs)
            ZoomBy(0.1)
        End Sub

        Protected Sub OnZoomOutBy20(ByVal sender As Object, ByVal e As System.EventArgs)
            ZoomBy(-0.2)
        End Sub

        Protected Sub OnZoomInBy20(ByVal sender As Object, ByVal e As System.EventArgs)
            ZoomBy(0.2)
        End Sub

        Protected Sub OnZoom10(ByVal sender As Object, ByVal e As System.EventArgs)
            ZoomTo(0.1)
        End Sub

        Protected Sub OnZoom20(ByVal sender As Object, ByVal e As System.EventArgs)
            ZoomTo(0.2)
        End Sub

        Protected Sub OnZoom30(ByVal sender As Object, ByVal e As System.EventArgs)
            ZoomTo(0.3)
        End Sub

        Protected Sub OnZoom40(ByVal sender As Object, ByVal e As System.EventArgs)
            ZoomTo(0.4)
        End Sub

        Protected Sub OnZoom50(ByVal sender As Object, ByVal e As System.EventArgs)
            ZoomTo(0.5)
        End Sub

        Protected Sub OnZoom60(ByVal sender As Object, ByVal e As System.EventArgs)
            ZoomTo(0.6)
        End Sub

        Protected Sub OnZoom70(ByVal sender As Object, ByVal e As System.EventArgs)
            ZoomTo(0.7)
        End Sub

        Protected Sub OnZoom80(ByVal sender As Object, ByVal e As System.EventArgs)
            ZoomTo(0.8)
        End Sub

        Protected Sub OnZoom90(ByVal sender As Object, ByVal e As System.EventArgs)
            ZoomTo(0.9)
        End Sub

        Protected Sub OnZoom100(ByVal sender As Object, ByVal e As System.EventArgs)
            ZoomTo(1)
        End Sub

        Protected Sub OnZoom125(ByVal sender As Object, ByVal e As System.EventArgs)
            ZoomTo(1.25)
        End Sub

        Protected Sub OnZoom150(ByVal sender As Object, ByVal e As System.EventArgs)
            ZoomTo(1.5)
        End Sub

        Protected Sub OnZoom175(ByVal sender As Object, ByVal e As System.EventArgs)
            ZoomTo(1.75)
        End Sub

        Protected Sub OnZoom200(ByVal sender As Object, ByVal e As System.EventArgs)
            ZoomTo(2.0)
        End Sub

        Protected Sub OnZoom250(ByVal sender As Object, ByVal e As System.EventArgs)
            ZoomTo(2.5)
        End Sub

        Protected Sub OnZoom300(ByVal sender As Object, ByVal e As System.EventArgs)
            ZoomTo(3.0)
        End Sub

        Protected Sub OnZoom400(ByVal sender As Object, ByVal e As System.EventArgs)
            ZoomTo(4.0)
        End Sub

        Protected Sub OnZoom500(ByVal sender As Object, ByVal e As System.EventArgs)
            ZoomTo(5.0)
        End Sub

#End Region

        Protected Sub OnExport(ByVal sender As Object, ByVal e As System.EventArgs)

            Try

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#Region " Print Events "

        Protected Sub OnPageSetup(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                'm_PrintHelper.PageSetup()

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnPrintPreview(ByVal sender As Object, ByVal e As System.EventArgs)

            Try

                'If Not m_tabFiller.SelectedTab Is Nothing Then
                '    Dim bdDiagram As Behavior.Diagram
                '    Dim aryMetaDocs As New Collections.MetaDocuments(Nothing)

                '    For Each deEntry As DictionaryEntry In m_aryDiagrams
                '        bdDiagram = DirectCast(deEntry.Value, Behavior.Diagram)
                '        bdDiagram.GenerateMetafiles(aryMetaDocs)
                '    Next

                '    m_PrintHelper.Preview(aryMetaDocs)
                'End If

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnPrintDiagram(ByVal sender As Object, ByVal e As System.EventArgs)

            Try

                'If Not m_tabFiller.SelectedTab Is Nothing Then
                '    Dim bdDiagram As Behavior.Diagram
                '    Dim aryMetaDocs As New Collections.MetaDocuments(Nothing)

                '    For Each deEntry As DictionaryEntry In m_aryDiagrams
                '        bdDiagram = DirectCast(deEntry.Value, Behavior.Diagram)
                '        bdDiagram.GenerateMetafiles(aryMetaDocs)
                '    Next

                '    m_PrintHelper.Print(aryMetaDocs)
                'End If

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

        Protected Sub OnDockContentClosing(ByVal c As Content, ByVal e As System.ComponentModel.CancelEventArgs)
            ViewToolbar(c)
            e.Cancel = True
        End Sub

        Private Sub Editor_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
            Try
                Util.Application.SimulationInterface.RemoveWindow(Me.Control.Handle)

                MyBase.MdiChild_Closing(sender, e)

                If Not m_doStructure Is Nothing Then
                    m_doStructure.BodyEditor = Nothing
                    m_doStructure.ClearSelectedBodyParts()
                End If

                Util.ModificationHistory.RemoveMdiEvents(Me)

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace

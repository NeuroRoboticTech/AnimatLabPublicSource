Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports Crownwood.Magic.Common
Imports AnimatGuiCtrls.Controls
Imports Crownwood.Magic.Controls
Imports Crownwood.Magic.Docking
Imports Crownwood.Magic.Menus
Imports AnimatGUI
Imports AnimatGUI.Framework
Imports AnimatGUI.DataObjects
Imports System.Drawing.Printing
Imports System.Drawing.Imaging

Namespace Forms.Behavior

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
            components = New System.ComponentModel.Container
            Me.Title = "BehavioralEditor"
            Me.Size = New Size(1000, 600)
        End Sub

#End Region

#Region " Attributes "

        Protected m_aryDiagrams As New Collections.SortedDiagrams(Me.FormHelper)
        Protected m_aryConnectorTypes As New ArrayList

        Protected m_OutlookBar As OutlookBar
        Protected m_OutlookContent As Content
        Protected m_mcOutlookBar As MenuCommand

        Protected m_PropertiesBar As Forms.ProjectProperties
        Protected m_PropertiesContent As Content
        Protected m_mcPropertiesBar As MenuCommand

        Protected m_HierarchyBar As Forms.Behavior.Hierarchy
        Protected m_HierarchyContent As Content
        Protected m_mcHierarchyBar As MenuCommand

        Protected m_ErrorsBar As Forms.Behavior.Errors
        Protected m_ErrorsContent As Content
        Protected m_mcErrorsBar As MenuCommand

        Protected m_ModulesBar As Forms.Behavior.NeuralModules
        Protected m_ModulesContent As Content
        Protected m_mcModulesBar As MenuCommand

        Protected m_iMaxNodeCount As Integer

        Protected m_aryDiagramImages As New Collections.DiagramImages(Me.FormHelper, Me)

        Protected m_doOrganism As DataObjects.Physical.Organism

        Protected m_doSelectedObject As Framework.DataObject
        Protected m_arySelectedObjects As New ArrayList

        Protected m_PrintHelper As New DataObjects.Behavior.PrintHelper

        Protected m_aryToolbarMenus As New SortedList

        Protected m_bInGroupChange As Boolean = False
        Protected m_iGroupChangeCounter As Integer = 0
        Protected m_iNextUndoCode As Integer = 1000

#End Region

#Region " Properties "

        Public Overridable ReadOnly Property Diagrams() As Collections.SortedDiagrams
            Get
                Return m_aryDiagrams
            End Get
        End Property

        Public Overridable ReadOnly Property ConnectorTypes() As ArrayList
            Get
                Return m_aryConnectorTypes
            End Get
        End Property

        Public Overridable Property SelectedObject() As Framework.DataObject
            Get
                Return m_doSelectedObject
            End Get
            Set(ByVal Value As Framework.DataObject)
                m_doSelectedObject = Value
                m_arySelectedObjects.Clear()
                m_arySelectedObjects.Add(m_doSelectedObject)

                If Not m_doSelectedObject Is Nothing Then
                    m_PropertiesBar.PropertyData = m_doSelectedObject.Properties
                Else
                    m_PropertiesBar.PropertyData = Nothing
                End If
            End Set
        End Property

        Public Overridable ReadOnly Property SelectedObjects() As ArrayList
            Get
                Return m_arySelectedObjects
            End Get
        End Property

        Public Overridable Property MaxNodeCount() As Integer
            Get
                Return m_iMaxNodeCount
            End Get
            Set(ByVal Value As Integer)
                m_iMaxNodeCount = Value
            End Set
        End Property

        Public Overridable ReadOnly Property DiagramImages() As Collections.DiagramImages
            Get
                Return m_aryDiagramImages
            End Get
        End Property

        Public Overridable Property Organism() As DataObjects.Physical.Organism
            Get
                Return m_doOrganism
            End Get
            Set(ByVal Value As DataObjects.Physical.Organism)
                m_doOrganism = Value
            End Set
        End Property

        Public Overridable ReadOnly Property PropertiesBar() As Forms.ProjectProperties
            Get
                Return m_PropertiesBar
            End Get
        End Property

        Public Overridable ReadOnly Property HierarchyBar() As Forms.Behavior.Hierarchy
            Get
                Return m_HierarchyBar
            End Get
        End Property

        Public Overridable ReadOnly Property ErrorsBar() As Forms.Behavior.Errors
            Get
                Return m_ErrorsBar
            End Get
        End Property

        Public Overridable ReadOnly Property ModulesBar() As Forms.Behavior.NeuralModules
            Get
                Return m_ModulesBar
            End Get
        End Property

        Public Overridable Property InGroupChange() As Boolean
            Get
                Return m_bInGroupChange
            End Get
            Set(ByVal Value As Boolean)
                m_bInGroupChange = Value
            End Set
        End Property

        Public Overridable Property GroupChangeCounter() As Integer
            Get
                Return m_iGroupChangeCounter
            End Get
            Set(ByVal Value As Integer)
                m_iGroupChangeCounter = Value
            End Set
        End Property

#End Region

#Region " Methods "

        Public Overrides Sub Initialize(ByVal frmApplication As AnimatApplication, _
                                        ByVal frmControl As AnimatForm)

            Try
                'Util.DisableDirtyFlags = True

                'Util.Application = Util.Application

                'Dim afDiagram As AnimatForm = Util.Application.CreateForm("LicensedAnimatGUI.dll", _
                '                                                        "LicensedAnimatGUI.Forms.Behavior.AddFlowDiagram", _
                '                                                        "Page 1", Me)
                'Dim bdDiagram As Forms.Behavior.Diagram = DirectCast(afDiagram, Forms.Behavior.Diagram)
                'm_aryDiagrams.Add(bdDiagram.ID, bdDiagram)

                'CreateImageManager()
                'MyBase.Initialize(Util.Application, afDiagram)

                'm_OutlookBar = New OutlookBar
                'm_OutlookBar.Location = New Point(0, 0)
                'm_OutlookBar.Size = New Size(150, Me.ClientSize.Height)
                'm_OutlookBar.BorderStyle = BorderStyle.FixedSingle
                'm_OutlookContent = m_dockManager.Contents.Add(m_OutlookBar, "Toolbox", m_mgrToolStripImages.ImageList, m_mgrToolStripImages.GetImageIndex("AnimatGUI.Wrench_Small.gif"))
                'm_OutlookContent.UserData = m_OutlookBar
                'm_OutlookContent.DisplaySize = New Size(250, 150)
                'm_OutlookContent.AutoHideSize = New Size(250, 150)
                'm_OutlookBar.Initialize()
                'CreateIconBands()

                'Dim wndContent As Crownwood.Magic.Docking.WindowContent = m_dockManager.AddContentWithState(m_OutlookContent, State.DockLeft)

                'm_PropertiesBar = New Forms.Properties
                'm_PropertiesBar.Initialize(Util.Application, Me)

                'm_PropertiesBar.Location = New Point(0, 0)
                'm_PropertiesContent = m_dockManager.Contents.Add(m_PropertiesBar, "Properties", m_mgrToolStripImages.ImageList, m_mgrToolStripImages.GetImageIndex("AnimatGUI.Properties.gif"))
                'm_PropertiesContent.DisplaySize = New Size(250, 150)
                'm_PropertiesContent.AutoHideSize = New Size(250, 150)
                'm_PropertiesContent.UserData = m_PropertiesBar
                'm_PropertiesBar.Content = m_PropertiesContent

                'm_dockManager.AddContentToWindowContent(m_PropertiesContent, wndContent)

                'm_HierarchyBar = New Forms.Behavior.Hierarchy
                'm_HierarchyBar.Initialize(Util.Application, Me)

                'm_HierarchyBar.Location = New Point(0, 0)
                'm_HierarchyContent = m_dockManager.Contents.Add(m_HierarchyBar, "Hierarchy", m_mgrToolStripImages.ImageList, m_mgrToolStripImages.GetImageIndex("AnimatGUI.ProjectWorkspace.gif"))
                'm_HierarchyContent.DisplaySize = New Size(250, 150)
                'm_HierarchyContent.AutoHideSize = New Size(250, 150)
                'm_HierarchyContent.UserData = m_HierarchyBar
                'm_HierarchyBar.Content = m_HierarchyContent

                'm_dockManager.AddContentToWindowContent(m_HierarchyContent, wndContent)

                'm_ModulesBar = New Forms.Behavior.NeuralModules
                'm_ModulesBar.Initialize(Util.Application, Me)

                'm_ModulesBar.Location = New Point(0, 0)
                'm_ModulesContent = m_dockManager.Contents.Add(m_ModulesBar, "Modules", m_mgrToolStripImages.ImageList, m_mgrToolStripImages.GetImageIndex("AnimatGUI.Neuron.gif"))
                'm_ModulesContent.DisplaySize = New Size(250, 150)
                'm_ModulesContent.AutoHideSize = New Size(250, 150)
                'm_ModulesContent.UserData = m_ModulesBar
                'm_ModulesBar.Content = m_ModulesContent

                'm_dockManager.AddContentToWindowContent(m_ModulesContent, wndContent)

                'm_ErrorsBar = New Forms.Behavior.Errors
                'm_ErrorsBar.Initialize(Util.Application, Me)

                'm_ErrorsBar.Location = New Point(0, 0)
                'm_ErrorsContent = m_dockManager.Contents.Add(m_ErrorsBar, "Neural Network Errors", m_mgrToolStripImages.ImageList, m_mgrToolStripImages.GetImageIndex("AnimatGUI.Error_Small.gif"))
                'm_ErrorsContent.DisplaySize = New Size(250, 150)
                'm_ErrorsContent.AutoHideSize = New Size(250, 150)
                'm_ErrorsContent.UserData = m_ErrorsBar
                'm_ErrorsBar.Content = m_ErrorsContent

                'm_dockManager.AutoHideWindow(wndContent)

                'wndContent = m_dockManager.AddContentWithState(m_ErrorsContent, State.DockBottom)
                'm_dockManager.AutoHideWindow(wndContent)

                'AddHandler m_tabFiller.SelectionChanging, AddressOf Me.OnTabSelectionChanging
                'AddHandler m_tabFiller.SelectionChanged, AddressOf Me.OnTabSelectionChanged
                'AddHandler m_dockManager.ContentClosing, AddressOf Me.OnDockContentClosing

                'm_HierarchyBar.AddDiagram(Nothing, bdDiagram)

                ' ''I think there is a bug in the docking code. When the window first appears it does not appear
                ' ''to be taking into account the size of the tabs when calculating the height of the bars. This
                ' ''corrects for that.
                ''m_ModulesBar.Height = m_ModulesBar.Height - 70
                ''m_HierarchyBar.Height = m_HierarchyBar.Height - 70

                'm_PropertiesBar.PropertyData = bdDiagram.Properties

                'm_aryToolbarMenus.Clear()
                'm_aryToolbarMenus.Add(m_OutlookContent.Title, m_mcOutlookBar)
                'm_aryToolbarMenus.Add(m_PropertiesContent.Title, m_mcPropertiesBar)
                'm_aryToolbarMenus.Add(m_HierarchyContent.Title, m_mcHierarchyBar)
                'm_aryToolbarMenus.Add(m_ModulesContent.Title, m_mcModulesBar)
                'm_aryToolbarMenus.Add(m_ErrorsContent.Title, m_mcErrorsBar)

                'Dim myAssembly As System.Reflection.Assembly
                'myAssembly = System.Reflection.Assembly.Load("AnimatGUI")
                'Me.Icon = Util.Application.ToolStripImages.LoadIcon(myAssembly, "AnimatGUI.Neuron.ico")

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                Util.DisableDirtyFlags = False
            End Try

        End Sub

#Region " Menu/Toolbar Methods "

        Protected Overrides Sub CreateToolstrips()
            'If Util.Application Is Nothing Then Throw New System.Exception("Application object is not defined.")
            'If m_frmControl Is Nothing Then Throw New System.Exception("Mdi Control object is not defined.")

            'm_menuMain = Util.Application.CreateDefaultMenu()

            'Dim mcFile As MenuCommand = m_menuMain.MenuCommands("File")

            'Dim mcSep1 As MenuCommand = New MenuCommand("-")
            'Dim mcExport As New MenuCommand("Export", "Export", m_mgrToolStripImages.ImageList, _
            '                                  m_mgrToolStripImages.GetImageIndex("AnimatGUI.Export.gif"), _
            '                                  New EventHandler(AddressOf Me.OnExport))

            'Dim mcPageSetup As New MenuCommand("Page Setup", "PageSetup", m_mgrToolStripImages.ImageList, _
            '                                  m_mgrToolStripImages.GetImageIndex("AnimatGUI.PageSetup.gif"), _
            '                                  New EventHandler(AddressOf Me.OnPageSetup))

            'Dim mcPrintPreview As New MenuCommand("Print Preview", "PrintPreview", m_mgrToolStripImages.ImageList, _
            '                                  m_mgrToolStripImages.GetImageIndex("AnimatGUI.PrintPreview.gif"), _
            '                                  New EventHandler(AddressOf Me.OnPrintPreview))

            'Dim mcPrint As New MenuCommand("Print", "Print", m_mgrToolStripImages.ImageList, _
            '                                  m_mgrToolStripImages.GetImageIndex("AnimatGUI.Print.gif"), _
            '                                  New EventHandler(AddressOf Me.OnPrintDiagram))

            'mcFile.MenuCommands.AddRange(New MenuCommand() {mcSep1, mcExport, mcPageSetup, mcPrintPreview, mcPrint})


            'Dim mcFitToPage As New MenuCommand("Fit To Page", "FitToPage", New EventHandler(AddressOf Me.OnFitToPage))

            'Dim mcZoomIn As New MenuCommand("ZoomIn", "ZoomIn", m_mgrToolStripImages.ImageList, _
            '                                  m_mgrToolStripImages.GetImageIndex("AnimatGUI.ZoomIn.gif"), _
            '                                  New EventHandler(AddressOf Me.OnZoomInBy10))

            'Dim mcZoomInBy10 As New MenuCommand("In By 10%", "ZoomInBy10", Shortcut.CtrlK, New EventHandler(AddressOf Me.OnZoomInBy10))
            'Dim mcZoomInBy20 As New MenuCommand("In By 20%", "ZoomInBy20", New EventHandler(AddressOf Me.OnZoomInBy20))
            'Dim mcZoomIn100 As New MenuCommand("100%", "Zoom100", New EventHandler(AddressOf Me.OnZoom100))
            'Dim mcZoom125 As New MenuCommand("125%", "Zoom125", New EventHandler(AddressOf Me.OnZoom125))
            'Dim mcZoom150 As New MenuCommand("150%", "Zoom150", New EventHandler(AddressOf Me.OnZoom150))
            'Dim mcZoom175 As New MenuCommand("175%", "Zoom175", New EventHandler(AddressOf Me.OnZoom175))
            'Dim mcZoom200 As New MenuCommand("200%", "Zoom200", New EventHandler(AddressOf Me.OnZoom200))
            'Dim mcZoom250 As New MenuCommand("250%", "Zoom250", New EventHandler(AddressOf Me.OnZoom250))
            'Dim mcZoom300 As New MenuCommand("300%", "Zoom300", New EventHandler(AddressOf Me.OnZoom300))
            'Dim mcZoom400 As New MenuCommand("400%", "Zoom400", New EventHandler(AddressOf Me.OnZoom400))
            'Dim mcZoom500 As New MenuCommand("500%", "Zoom500", New EventHandler(AddressOf Me.OnZoom500))

            'mcZoomIn.MenuCommands.AddRange(New MenuCommand() {mcZoomInBy10, mcZoomInBy20, mcZoomIn100, mcZoom125, mcZoom150, mcZoom175, _
            '                                                  mcZoom200, mcZoom250, mcZoom300, mcZoom400, mcZoom500})

            'Dim mcZoomOut As New MenuCommand("ZoomOut", "ZoomOut", m_mgrToolStripImages.ImageList, _
            '                                  m_mgrToolStripImages.GetImageIndex("AnimatGUI.ZoomOut.gif"), _
            '                                  New EventHandler(AddressOf Me.OnZoomOutBy10))

            'Dim mcZoomOutBy10 As New MenuCommand("Out By 10%", "ZoomOutBy10", Shortcut.CtrlL, New EventHandler(AddressOf Me.OnZoomOutBy10))
            'Dim mcZoomOutBy20 As New MenuCommand("Out By 20%", "ZoomOutBy20", New EventHandler(AddressOf Me.OnZoomOutBy20))
            'Dim mcZoomOut100 As New MenuCommand("100%", "Zoom100", New EventHandler(AddressOf Me.OnZoom100))
            'Dim mcZoom90 As New MenuCommand("90%", "Zoom90", New EventHandler(AddressOf Me.OnZoom90))
            'Dim mcZoom80 As New MenuCommand("80%", "Zoom80", New EventHandler(AddressOf Me.OnZoom80))
            'Dim mcZoom70 As New MenuCommand("70%", "Zoom70", New EventHandler(AddressOf Me.OnZoom70))
            'Dim mcZoom60 As New MenuCommand("60%", "Zoom60", New EventHandler(AddressOf Me.OnZoom60))
            'Dim mcZoom50 As New MenuCommand("50%", "Zoom50", New EventHandler(AddressOf Me.OnZoom50))
            'Dim mcZoom40 As New MenuCommand("40%", "Zoom40", New EventHandler(AddressOf Me.OnZoom40))
            'Dim mcZoom30 As New MenuCommand("30%", "Zoom30", New EventHandler(AddressOf Me.OnZoom30))
            'Dim mcZoom20 As New MenuCommand("20%", "Zoom20", New EventHandler(AddressOf Me.OnZoom20))
            'Dim mcZoom10 As New MenuCommand("10%", "Zoom10", New EventHandler(AddressOf Me.OnZoom10))

            'mcZoomOut.MenuCommands.AddRange(New MenuCommand() {mcZoomOutBy10, mcZoomOutBy20, mcZoomOut100, mcZoom90, _
            '                                                   mcZoom80, mcZoom70, mcZoom60, mcZoom50, mcZoom40, mcZoom30, _
            '                                                   mcZoom20, mcZoom10})

            'Dim mcView As MenuCommand = m_menuMain.MenuCommands("View")
            'Dim mcSep2 As MenuCommand = New MenuCommand("-")

            'Dim mcToolbars As MenuCommand = New MenuCommand("Toolbars")
            'm_mcOutlookBar = New MenuCommand("Toolbox", "Toolbox", New EventHandler(AddressOf Me.OnViewToolboxBar))
            'm_mcPropertiesBar = New MenuCommand("Properties", "Properties", New EventHandler(AddressOf Me.OnViewPropertiesBar))
            'm_mcHierarchyBar = New MenuCommand("Hierarchy", "Hierarchy", New EventHandler(AddressOf Me.OnViewHierarchyBar))
            'm_mcModulesBar = New MenuCommand("Neural Modules", "NeuralModules", New EventHandler(AddressOf Me.OnViewModulesBar))
            'm_mcErrorsBar = New MenuCommand("Errors", "Errors", New EventHandler(AddressOf Me.OnViewErrorsBar))

            'm_mcOutlookBar.Checked = True
            'm_mcPropertiesBar.Checked = True
            'm_mcHierarchyBar.Checked = True
            'm_mcModulesBar.Checked = True
            'm_mcErrorsBar.Checked = True

            'mcToolbars.MenuCommands.AddRange(New MenuCommand() {m_mcOutlookBar, m_mcPropertiesBar, m_mcHierarchyBar, m_mcModulesBar, m_mcErrorsBar})

            'mcView.MenuCommands.AddRange(New MenuCommand() {mcToolbars, mcSep2, mcFitToPage, mcZoomOut, mcZoomIn})


            'Dim mcShape As New MenuCommand("Shape", "Shape")
            'mcShape.Description = "Shape Control Commands"

            'Dim mcAlign As New MenuCommand("Align", "Align", m_mgrToolStripImages.ImageList, m_mgrToolStripImages.GetImageIndex("AnimatGUI.Align.gif"))
            'Dim mcAlignTop As New MenuCommand("Top", "AlignTop", m_mgrToolStripImages.ImageList, _
            '                                  m_mgrToolStripImages.GetImageIndex("AnimatGUI.AlignTop.gif"), New EventHandler(AddressOf Me.OnAlignTop))
            'Dim mcAlignVerticalCenter As New MenuCommand("Veritcal Center", "AlignVeritcalCenter", _
            '                                             m_mgrToolStripImages.ImageList, _
            '                                             m_mgrToolStripImages.GetImageIndex("AnimatGUI.AlignVerticalCenter.gif"), _
            '                                             New EventHandler(AddressOf Me.OnAlignVerticalCenter))
            'Dim mcAlignBottom As New MenuCommand("Bottom", "AlignBottom", m_mgrToolStripImages.ImageList, _
            '                                     m_mgrToolStripImages.GetImageIndex("AnimatGUI.AlignBottom.gif"), _
            '                                     New EventHandler(AddressOf Me.OnAlignBottom))
            'Dim mcAlignLeft As New MenuCommand("Left", "AlignLeft", m_mgrToolStripImages.ImageList, _
            '                                 m_mgrToolStripImages.GetImageIndex("AnimatGUI.AlignLeft.gif"), _
            '                                 New EventHandler(AddressOf Me.OnAlignLeft))
            'Dim mcAlignHorizontalCenter As New MenuCommand("Horizontal Center", "AlignHorizontalCenter", _
            '                                             m_mgrToolStripImages.ImageList, _
            '                                             m_mgrToolStripImages.GetImageIndex("AnimatGUI.AlignHorizontalCenter.gif"), _
            '                                             New EventHandler(AddressOf Me.OnAlignHorizontalCenter))
            'Dim mcAlignRight As New MenuCommand("Right", "AlignRight", m_mgrToolStripImages.ImageList, _
            '                                    m_mgrToolStripImages.GetImageIndex("AnimatGUI.AlignRight.gif"), _
            '                                    New EventHandler(AddressOf Me.OnAlignRight))

            'mcAlign.MenuCommands.AddRange(New MenuCommand() {mcAlignTop, mcAlignVerticalCenter, mcAlignBottom, mcAlignLeft, mcAlignHorizontalCenter, mcAlignRight})

            'Dim mcDistribute As New MenuCommand("Distribute", "Distribute", m_mgrToolStripImages.ImageList, m_mgrToolStripImages.GetImageIndex("AnimatGUI.Distribute.gif"))
            'Dim mcDistributeVertical As New MenuCommand("Veritcal", "DistributeVertical", _
            '                                            m_mgrToolStripImages.ImageList, _
            '                                            m_mgrToolStripImages.GetImageIndex("AnimatGUI.DistributeVertical.gif"), _
            '                                            New EventHandler(AddressOf Me.OnDistributeVertical))
            'Dim mcDistributeHorizontal As New MenuCommand("Horizontal", "DistributeHorizontal", _
            '                                              m_mgrToolStripImages.ImageList, _
            '                                              m_mgrToolStripImages.GetImageIndex("AnimatGUI.DistributeHorizontal.gif"), _
            '                                              New EventHandler(AddressOf Me.OnDistributeHorizontal))

            'mcDistribute.MenuCommands.AddRange(New MenuCommand() {mcDistributeVertical, mcDistributeHorizontal})


            'Dim mcSize As New MenuCommand("Size", "Size", m_mgrToolStripImages.ImageList, m_mgrToolStripImages.GetImageIndex("AnimatGUI.Size.gif"))
            'Dim mcSizeBoth As New MenuCommand("Both", "SizeBoth", m_mgrToolStripImages.ImageList, _
            '                                  m_mgrToolStripImages.GetImageIndex("AnimatGUI.SizeBoth.gif"), _
            '                                  New EventHandler(AddressOf Me.OnSizeBoth))
            'Dim mcSizeWidth As New MenuCommand("Width", "SizeWidth", m_mgrToolStripImages.ImageList, _
            '                                   m_mgrToolStripImages.GetImageIndex("AnimatGUI.SizeWidth.gif"), _
            '                                   New EventHandler(AddressOf Me.OnSizeWidth))
            'Dim mcSizeHeight As New MenuCommand("Height", "SizeHeight", m_mgrToolStripImages.ImageList, _
            '                                    m_mgrToolStripImages.GetImageIndex("AnimatGUI.SizeHeight.gif"), _
            '                                    New EventHandler(AddressOf Me.OnSizeHeight))

            'mcSize.MenuCommands.AddRange(New MenuCommand() {mcSizeBoth, mcSizeWidth, mcSizeHeight})

            'mcShape.MenuCommands.AddRange(New MenuCommand() {mcAlign, mcDistribute, mcSize})

            'Dim mcEdit As MenuCommand = m_menuMain.MenuCommands("Edit")
            'Dim mcInsertPage As New MenuCommand("Insert Page", "InsertPage", m_mgrToolStripImages.ImageList, _
            '                                  m_mgrToolStripImages.GetImageIndex("AnimatGUI.SizeBoth.gif"), _
            '                                  New EventHandler(AddressOf Me.OnInsertPage))
            'Dim mcAddStim As New MenuCommand("Add Stimulus", "AddStimulus", m_mgrToolStripImages.ImageList, _
            '                                  m_mgrToolStripImages.GetImageIndex("AnimatGUI.AddStimulus.gif"), _
            '                                  New EventHandler(AddressOf Me.OnAddStimulus))
            'Dim mcSelectByType As New MenuCommand("Select By Type", "SelectByType", m_mgrToolStripImages.ImageList, _
            '                                  m_mgrToolStripImages.GetImageIndex("AnimatGUI.SelectByType.gif"), _
            '                                  New EventHandler(AddressOf Me.OnSelectByType))
            'Dim mcRelabel As New MenuCommand("Relabel", "Relabel", m_mgrToolStripImages.ImageList, _
            '                                  m_mgrToolStripImages.GetImageIndex("AnimatGUI.Relabel.gif"), _
            '                                  New EventHandler(AddressOf Me.OnRelabel))
            'Dim mcRelabelSelected As New MenuCommand("Relabel Selected", "RelabelSelected", m_mgrToolStripImages.ImageList, _
            '                                  m_mgrToolStripImages.GetImageIndex("AnimatGUI.RelabelSelected.gif"), _
            '                                  New EventHandler(AddressOf Me.OnRelabelSelected))
            'Dim mcShowConnections As New MenuCommand("Show Connections", "ShowConnections", m_mgrToolStripImages.ImageList, _
            '                                  m_mgrToolStripImages.GetImageIndex("AnimatGUI.Connections.gif"), _
            '                                  New EventHandler(AddressOf Me.OnShowConnections))
            'Dim mcCompareItems As New MenuCommand("Compare Items", "CompareItems", m_mgrToolStripImages.ImageList, _
            '                                  m_mgrToolStripImages.GetImageIndex("AnimatGUI.Equals.gif"), _
            '                                  New EventHandler(AddressOf Me.OnCompareItems))
            ''Dim mcAutoUpdate As New MenuCommand("Auto Update", "mcAutoUpdate", m_mgrToolStripImages.ImageList, _
            ' ''                                             m_mgrToolStripImages.GetImageIndex("AnimatGUI.Equals.gif"), _
            ''                                             New EventHandler(AddressOf Me.OnAutoUpdate))
            'Dim mcSepEditStart As MenuCommand = New MenuCommand("-")
            'Dim mcCut As New MenuCommand("Cut", "Cut", m_mgrToolStripImages.ImageList, _
            '                             m_mgrToolStripImages.GetImageIndex("AnimatGUI.Cut.gif"), _
            '                             Shortcut.CtrlX, New EventHandler(AddressOf Me.OnCut))
            'Dim mcCopy As New MenuCommand("Copy", "Copy", m_mgrToolStripImages.ImageList, _
            '                                m_mgrToolStripImages.GetImageIndex("AnimatGUI.Copy.gif"), _
            '                                Shortcut.CtrlC, New EventHandler(AddressOf Me.OnCopy))
            'Dim mcPaste As New MenuCommand("Paste", "Paste", m_mgrToolStripImages.ImageList, _
            '                                m_mgrToolStripImages.GetImageIndex("AnimatGUI.Paste.gif"), _
            '                                Shortcut.CtrlV, New EventHandler(AddressOf Me.OnPaste))
            'Dim mcPasteInPlace As New MenuCommand("Paste In Place", "PasteInPlace", m_mgrToolStripImages.ImageList, _
            '                                m_mgrToolStripImages.GetImageIndex("AnimatGUI.Paste.gif"), _
            '                                Shortcut.CtrlB, New EventHandler(AddressOf Me.OnPasteInPlace))
            'Dim mcDelete As New MenuCommand("Delete", "Delete", m_mgrToolStripImages.ImageList, _
            '                                m_mgrToolStripImages.GetImageIndex("AnimatGUI.Delete.gif"), _
            '                                Shortcut.Del, New EventHandler(AddressOf Me.OnDelete))

            'mcEdit.MenuCommands.AddRange(New MenuCommand() {mcInsertPage, mcAddStim, mcSelectByType, mcRelabel, mcRelabelSelected, mcShowConnections, mcCompareItems, mcSepEditStart, mcCut, mcCopy, mcPaste, mcPasteInPlace, mcDelete})

            'AddHandler mcEdit.PopupStart, AddressOf Me.OnEditPopupStart
            'AddHandler mcEdit.PopupEnd, AddressOf Me.OnEditPopupEnd

            'AddHandler mcShape.PopupStart, AddressOf Me.OnShapePopupStart
            'AddHandler mcShape.PopupEnd, AddressOf Me.OnShapePopupEnd

            'm_menuMain.MenuCommands.AddRange(New MenuCommand() {mcShape})

        End Sub

        'Protected Overrides Sub CreateToolbar()
        '    'If Util.Application Is Nothing Then Throw New System.Exception("Application object is not defined.")
        '    'If m_frmControl Is Nothing Then Throw New System.Exception("Mdi Control object is not defined.")

        '    'm_barMain = Util.Application.CreateDefaultToolbar(m_menuMain)

        '    'Dim btnZoomIn As New ToolBarButton
        '    'btnZoomIn.ImageIndex = Util.Application.LargeImages.GetImageIndex("AnimatGUI.ZoomIn.gif")
        '    'btnZoomIn.ToolTipText = "Zoom Into the Diagram"

        '    'Dim mcZoomInBy10 As New MenuItem("Zoom in by 10%  Ctrl+K", New EventHandler(AddressOf Me.OnZoomInBy10))
        '    'Dim mcZoomInBy20 As New MenuItem("Zoom in by 20%", New EventHandler(AddressOf Me.OnZoomInBy20))
        '    'Dim mcZoomIn100 As New MenuItem("100%", New EventHandler(AddressOf Me.OnZoom100))
        '    'Dim mcZoom125 As New MenuItem("125%", New EventHandler(AddressOf Me.OnZoom125))
        '    'Dim mcZoom150 As New MenuItem("150%", New EventHandler(AddressOf Me.OnZoom150))
        '    'Dim mcZoom175 As New MenuItem("175%", New EventHandler(AddressOf Me.OnZoom175))
        '    'Dim mcZoom200 As New MenuItem("200%", New EventHandler(AddressOf Me.OnZoom200))
        '    'Dim mcZoom250 As New MenuItem("250%", New EventHandler(AddressOf Me.OnZoom250))
        '    'Dim mcZoom300 As New MenuItem("300%", New EventHandler(AddressOf Me.OnZoom300))
        '    'Dim mcZoom400 As New MenuItem("400%", New EventHandler(AddressOf Me.OnZoom400))
        '    'Dim mcZoom500 As New MenuItem("500%", New EventHandler(AddressOf Me.OnZoom500))

        '    'Dim mcZoomIn As New ContextMenu(New MenuItem() {mcZoomInBy10, mcZoomInBy20, mcZoomIn100, mcZoom125, mcZoom150, mcZoom175, _
        '    '                                                mcZoom200, mcZoom250, mcZoom300, mcZoom400, mcZoom500})
        '    'btnZoomIn.Style = ToolBarButtonStyle.DropDownButton
        '    'btnZoomIn.DropDownMenu = mcZoomIn

        '    'Dim btnZoomOut As New ToolBarButton
        '    'btnZoomOut.ImageIndex = Util.Application.LargeImages.GetImageIndex("AnimatGUI.ZoomOut.gif")
        '    'btnZoomOut.ToolTipText = "Zoom out of the Diagram"

        '    'Dim mcZoomOutBy10 As New MenuItem("Zoom out by 10%  Ctrl+L", New EventHandler(AddressOf Me.OnZoomOutBy10))
        '    'Dim mcZoomOutBy20 As New MenuItem("Zoom out by 20%", New EventHandler(AddressOf Me.OnZoomOutBy20))
        '    'Dim mcZoomOut100 As New MenuItem("100%", New EventHandler(AddressOf Me.OnZoom100))
        '    'Dim mcZoom90 As New MenuItem("90%", New EventHandler(AddressOf Me.OnZoom90))
        '    'Dim mcZoom80 As New MenuItem("80%", New EventHandler(AddressOf Me.OnZoom80))
        '    'Dim mcZoom70 As New MenuItem("70%", New EventHandler(AddressOf Me.OnZoom70))
        '    'Dim mcZoom60 As New MenuItem("60%", New EventHandler(AddressOf Me.OnZoom60))
        '    'Dim mcZoom50 As New MenuItem("50%", New EventHandler(AddressOf Me.OnZoom50))
        '    'Dim mcZoom40 As New MenuItem("40%", New EventHandler(AddressOf Me.OnZoom40))
        '    'Dim mcZoom30 As New MenuItem("30%", New EventHandler(AddressOf Me.OnZoom30))
        '    'Dim mcZoom20 As New MenuItem("20%", New EventHandler(AddressOf Me.OnZoom20))
        '    'Dim mcZoom10 As New MenuItem("10%", New EventHandler(AddressOf Me.OnZoom10))

        '    'Dim mcZoomOut As New ContextMenu(New MenuItem() {mcZoomOutBy10, mcZoomOutBy20, mcZoomOut100, mcZoom90, mcZoom80, _
        '    '                                                  mcZoom70, mcZoom60, mcZoom50, mcZoom40, mcZoom30, mcZoom20, mcZoom10})
        '    'btnZoomOut.Style = ToolBarButtonStyle.DropDownButton
        '    'btnZoomOut.DropDownMenu = mcZoomOut

        '    'Dim btnAddStimulus As New ToolBarButton
        '    'btnAddStimulus.ImageIndex = Util.Application.LargeImages.GetImageIndex("AnimatGUI.AddStimulus.gif")
        '    'btnAddStimulus.ToolTipText = "Add Stimulus"

        '    'Dim btnDelete As New ToolBarButton
        '    'btnDelete.ImageIndex = Util.Application.LargeImages.GetImageIndex("AnimatGUI.Delete.gif")
        '    'btnDelete.ToolTipText = "Delete"

        '    'Dim btnSelectByType As New ToolBarButton
        '    'btnSelectByType.ImageIndex = Util.Application.LargeImages.GetImageIndex("AnimatGUI.SelectByType.gif")
        '    'btnSelectByType.ToolTipText = "Select Items by Object Type"

        '    'Dim btnRelabel As New ToolBarButton
        '    'btnRelabel.ImageIndex = Util.Application.LargeImages.GetImageIndex("AnimatGUI.Relabel.gif")
        '    'btnRelabel.ToolTipText = "Relabel items using regular expressions."

        '    'Dim btnRelabelSelected As New ToolBarButton
        '    'btnRelabelSelected.ImageIndex = Util.Application.LargeImages.GetImageIndex("AnimatGUI.RelabelSelected.gif")
        '    'btnRelabelSelected.ToolTipText = "Relabel and number selected items"

        '    'Dim btnShowConnections As New ToolBarButton
        '    'btnShowConnections.ImageIndex = Util.Application.LargeImages.GetImageIndex("AnimatGUI.Connections.gif")
        '    'btnShowConnections.ToolTipText = "Show the input/output connections for the selected neuron"

        '    'Dim btnAlign As New ToolBarButton
        '    'btnAlign.ImageIndex = Util.Application.LargeImages.GetImageIndex("AnimatGUI.Align.gif")
        '    'btnAlign.ToolTipText = "Align Nodes"

        '    'Dim btnCompareItem As New ToolBarButton
        '    'btnCompareItem.ImageIndex = Util.Application.LargeImages.GetImageIndex("AnimatGUI.Equals.gif")
        '    'btnCompareItem.ToolTipText = "Compare Items"

        '    'Dim mcAlignTop As New MenuItem("Top", New EventHandler(AddressOf Me.OnAlignTop))
        '    'Dim mcAlignVerticalCenter As New MenuItem("Vertical Center", New EventHandler(AddressOf Me.OnAlignVerticalCenter))
        '    'Dim mcAlignBottom As New MenuItem("Bottom", New EventHandler(AddressOf Me.OnAlignBottom))
        '    'Dim mcAlignLeft As New MenuItem("Left", New EventHandler(AddressOf Me.OnAlignLeft))
        '    'Dim mcAlignHorizontalCenter As New MenuItem("Vertical Center", New EventHandler(AddressOf Me.OnAlignHorizontalCenter))
        '    'Dim mcAlignRight As New MenuItem("Right", New EventHandler(AddressOf Me.OnAlignRight))
        '    'Dim muAlign As New ContextMenu(New MenuItem() {mcAlignTop, mcAlignVerticalCenter, mcAlignBottom, mcAlignLeft, mcAlignHorizontalCenter, mcAlignRight})

        '    'btnAlign.Style = ToolBarButtonStyle.DropDownButton
        '    'btnAlign.DropDownMenu = muAlign

        '    'Dim btnDistribute As New ToolBarButton
        '    'btnDistribute.ImageIndex = Util.Application.LargeImages.GetImageIndex("AnimatGUI.Distribute.gif")
        '    'btnDistribute.ToolTipText = "Distribute Nodes"

        '    'Dim mcDistributeVertical As New MenuItem("Vertical", New EventHandler(AddressOf Me.OnDistributeVertical))
        '    'Dim mcDistributeHorizontal As New MenuItem("Horizontal", New EventHandler(AddressOf Me.OnDistributeHorizontal))
        '    'Dim muDistribute As New ContextMenu(New MenuItem() {mcDistributeVertical, mcDistributeHorizontal})

        '    'btnDistribute.Style = ToolBarButtonStyle.DropDownButton
        '    'btnDistribute.DropDownMenu = muDistribute

        '    'Dim btnSize As New ToolBarButton
        '    'btnSize.ImageIndex = Util.Application.LargeImages.GetImageIndex("AnimatGUI.Size.gif")
        '    'btnSize.ToolTipText = "Size Nodes"

        '    'Dim mcSizeBoth As New MenuItem("Both", New EventHandler(AddressOf Me.OnSizeBoth))
        '    'Dim mcSizeWidth As New MenuItem("Width", New EventHandler(AddressOf Me.OnSizeWidth))
        '    'Dim mcSizeHeight As New MenuItem("Height", New EventHandler(AddressOf Me.OnSizeHeight))
        '    'Dim muSize As New ContextMenu(New MenuItem() {mcSizeBoth, mcSizeWidth, mcSizeHeight})

        '    'btnSize.Style = ToolBarButtonStyle.DropDownButton
        '    'btnSize.DropDownMenu = muSize

        '    'm_barMain.Buttons.AddRange(New ToolBarButton() {btnAddStimulus, btnDelete, btnSelectByType, btnRelabel, btnRelabelSelected, btnShowConnections, btnZoomIn, btnZoomOut, btnAlign, btnDistribute, btnSize, btnCompareItem})

        '    'm_barMain.ButtonManager.SetButtonMenuItem(btnAddStimulus, m_menuMain.MenuCommands.FindMenuCommand("AddStimulus"))
        '    'm_barMain.ButtonManager.SetButtonMenuItem(btnDelete, m_menuMain.MenuCommands.FindMenuCommand("Delete"))
        '    'm_barMain.ButtonManager.SetButtonMenuItem(btnSelectByType, m_menuMain.MenuCommands.FindMenuCommand("SelectByType"))
        '    'm_barMain.ButtonManager.SetButtonMenuItem(btnRelabel, m_menuMain.MenuCommands.FindMenuCommand("Relabel"))
        '    'm_barMain.ButtonManager.SetButtonMenuItem(btnRelabelSelected, m_menuMain.MenuCommands.FindMenuCommand("RelabelSelected"))
        '    'm_barMain.ButtonManager.SetButtonMenuItem(btnShowConnections, m_menuMain.MenuCommands.FindMenuCommand("ShowConnections"))
        '    'm_barMain.ButtonManager.SetButtonMenuItem(btnCompareItem, m_menuMain.MenuCommands.FindMenuCommand("CompareItems"))

        'End Sub

        Protected Overridable Sub CreateImageManager()

            Try

                Dim myAssembly As System.Reflection.Assembly
                myAssembly = System.Reflection.Assembly.Load("AnimatGUI")

                m_mgrToolStripImages = New AnimatGUI.Framework.ImageManager
                m_mgrToolStripImages.ImageList.ImageSize = New Size(16, 16)
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.Align.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.AlignGrey.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.AlignBottom.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.AlignBottomGrey.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.AlignHorizontalCenter.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.AlignHorizontalCenterGrey.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.AlignLeft.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.AlignLeftGrey.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.AlignRight.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.AlignRightGrey.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.AlignTop.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.AlignTopGrey.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.AlignVerticalCenter.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.AlignVerticalCenterGrey.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.Distribute.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.DistributeGrey.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.DistributeHorizontal.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.DistributeHorizontalGrey.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.DistributeVertical.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.DistributeVerticalGrey.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.Size.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.SizeGrey.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.SizeBoth.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.SizeBothGrey.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.SizeHeight.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.SizeHeightGrey.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.SizeWidth.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.SizeWidthGrey.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.ZoomIn.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.ZoomOut.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.PageSetup.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.PrintPreview.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.Print.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.Export.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.Cut.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.Copy.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.Paste.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.Delete.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.SendToBack.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.BringToFront.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.Undo.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.UndoGrey.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.Redo.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.RedoGrey.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.ProjectWorkspace.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.Wrench_Small.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.Properties.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.Neuron.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.Error_Small.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.ExternalStimulus.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.AddStimulus.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.SelectByType.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.Relabel.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.RelabelSelected.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.Connections.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.Equals.gif")

                'Util.Application.LargeImages.AddImage(myAssembly, "AnimatGUI.AlignLarge.gif")
                'Util.Application.LargeImages.AddImage(myAssembly, "AnimatGUI.AlignGreyLarge.gif")
                'Util.Application.LargeImages.AddImage(myAssembly, "AnimatGUI.DistributeLarge.gif")
                'Util.Application.LargeImages.AddImage(myAssembly, "AnimatGUI.DistributeGreyLarge.gif")
                'Util.Application.LargeImages.AddImage(myAssembly, "AnimatGUI.SizeLarge.gif")
                'Util.Application.LargeImages.AddImage(myAssembly, "AnimatGUI.SizeGreyLarge.gif")
                Util.Application.LargeImages.AddImage(myAssembly, "AnimatGUI.ZoomIn.gif")
                Util.Application.LargeImages.AddImage(myAssembly, "AnimatGUI.ZoomOut.gif")
                Util.Application.LargeImages.AddImage(myAssembly, "AnimatGUI.AddStimulus.gif")
                Util.Application.LargeImages.AddImage(myAssembly, "AnimatGUI.Delete.gif")
                Util.Application.LargeImages.AddImage(myAssembly, "AnimatGUI.Align.gif")
                Util.Application.LargeImages.AddImage(myAssembly, "AnimatGUI.Distribute.gif")
                Util.Application.LargeImages.AddImage(myAssembly, "AnimatGUI.Size.gif")
                Util.Application.LargeImages.AddImage(myAssembly, "AnimatGUI.SelectByType.gif")
                Util.Application.LargeImages.AddImage(myAssembly, "AnimatGUI.Relabel.gif")
                Util.Application.LargeImages.AddImage(myAssembly, "AnimatGUI.RelabelSelected.gif")
                Util.Application.LargeImages.AddImage(myAssembly, "AnimatGUI.Connections.gif")
                Util.Application.LargeImages.AddImage(myAssembly, "AnimatGUI.Equals.gif")

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

        'This goes through the application directory and tries to find any dll's that contain objects derived from 
        'Behavior.Node or Behavior.Link
        Protected Overridable Sub CreateIconBands()

            Try
                Dim ipPanel As IconPanel
                For Each pdPanel As DataObjects.Behavior.PanelData In Util.Application.AlphabeticalBehavioralPanels

                    ipPanel = New IconPanel
                    ipPanel.IconHeight = 55
                    m_OutlookBar.AddBand(pdPanel.m_strPanelName, ipPanel)

                    For Each bnNode As DataObjects.Behavior.Node In pdPanel.m_aryNodes
                        bnNode.ParentEditor = Me
                        ipPanel.AddIcon(bnNode.Name, bnNode.WorkspaceImage, bnNode.DragImage, bnNode)
                        bnNode.AfterAddedToIconBand()
                        bnNode.ParentEditor = Nothing
                    Next

                Next

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overridable Function SelectLinkType(ByRef bnOrigin As DataObjects.Behavior.Node, ByRef bnDestination As DataObjects.Behavior.Node, _
                                                   ByRef blLink As DataObjects.Behavior.Link, ByRef bRequiresAdapter As Boolean) As Boolean
            Dim bRetVal As Boolean = False

            If TypeOf (bnOrigin) Is DataObjects.Behavior.Nodes.Adapter OrElse TypeOf (bnDestination) Is DataObjects.Behavior.Nodes.Adapter Then
                'If either the origin or destintion is an adapter then we already know the type of link.
                blLink = New DataObjects.Behavior.Links.Adapter(Me.FormHelper)
                blLink.Origin = bnOrigin
                blLink.Destination = bnDestination
                bRequiresAdapter = False
                bRetVal = True
            ElseIf TypeOf (bnOrigin) Is DataObjects.Behavior.Nodes.OffPage OrElse TypeOf (bnDestination) Is DataObjects.Behavior.Nodes.OffPage Then
                bRetVal = SelectOffPageLinkType(bnOrigin, bnDestination, blLink, bRequiresAdapter)
            ElseIf bnOrigin.IsPhysicsEngineNode AndAlso bnDestination.IsPhysicsEngineNode Then
                'You can only draw graphical links between physics node objects.
                blLink = New DataObjects.Behavior.Links.Graphical(Me.FormHelper)
                blLink.Origin = bnOrigin
                blLink.Destination = bnDestination
                bRequiresAdapter = False
                bRetVal = True
            Else
                'If neither of the nodes are adapters then lets get a list of all compatible links between the two nodes.
                Dim aryCompatibleLinks As Collections.Links = FindCompatibleLinkTypes(bnOrigin, bnDestination)

                If aryCompatibleLinks.Count = 0 Then
                    'If there are no real link types that can be drawn between these two then just draw a graphical link.
                    blLink = New DataObjects.Behavior.Links.Graphical(Me.FormHelper)
                    blLink.Origin = bnOrigin
                    blLink.Destination = bnDestination
                    bRequiresAdapter = False
                    bRetVal = True
                ElseIf aryCompatibleLinks.Count = 1 Then
                    'If there is only one compatible link between these types then default to it.
                    blLink = DirectCast(aryCompatibleLinks(0).Clone(aryCompatibleLinks(0).Parent, False, Nothing), DataObjects.Behavior.Link)
                    blLink.Origin = bnOrigin
                    blLink.Destination = bnDestination
                    bRetVal = True

                    If TypeOf (blLink) Is AnimatGUI.DataObjects.Behavior.Links.Adapter Then
                        bRequiresAdapter = True
                    Else
                        bRequiresAdapter = False
                    End If
                Else
                    'Otherwise we are going to have to show a list to the user and have them choose the link type.
                    If bnOrigin.SelectLinkType(bnOrigin, bnDestination, blLink, aryCompatibleLinks) Then
                        bRetVal = True
                    End If
                End If
            End If

            Return bRetVal
        End Function

        Protected Overridable Function SelectOffPageLinkType(ByRef bnOrigin As DataObjects.Behavior.Node, ByRef bnDestination As DataObjects.Behavior.Node, _
                                                             ByRef blLink As DataObjects.Behavior.Link, ByRef bRequiresAdapter As Boolean) As Boolean
            'If this is an offpage connector then we need to find the original/destination node and then call select link type again for those nodes
            'For the moment do nothing.
            Dim bnNewOrigin As DataObjects.Behavior.Node
            Dim bnNewDestination As DataObjects.Behavior.Node

            If TypeOf (bnOrigin) Is DataObjects.Behavior.Nodes.OffPage Then
                Dim opOrigin As DataObjects.Behavior.Nodes.OffPage = DirectCast(bnOrigin, DataObjects.Behavior.Nodes.OffPage)

                If opOrigin.LinkedNode Is Nothing OrElse opOrigin.LinkedNode.Node Is Nothing Then
                    Throw New System.Exception("The off-page connector node '" + opOrigin.Text & "' must be associated " & _
                                               "with another node before you can connect it with a link.")
                End If

                bnNewOrigin = opOrigin.LinkedNode.Node
            Else
                bnNewOrigin = bnOrigin
            End If

            If TypeOf (bnDestination) Is DataObjects.Behavior.Nodes.OffPage Then
                Dim opDestination As DataObjects.Behavior.Nodes.OffPage = DirectCast(bnDestination, DataObjects.Behavior.Nodes.OffPage)

                If opDestination.LinkedNode Is Nothing OrElse opDestination.LinkedNode.Node Is Nothing Then
                    Throw New System.Exception("The off-page connector node '" + opDestination.Text & "' must be associated " & _
                                               "with another node before you can connect it with a link.")
                End If

                bnNewDestination = opDestination.LinkedNode.Node
            Else
                bnNewDestination = bnDestination
            End If

            If SelectLinkType(bnNewOrigin, bnNewDestination, blLink, bRequiresAdapter) Then
                blLink.Origin = bnOrigin
                blLink.Destination = bnDestination
                Return True
            Else
                Return False
            End If

        End Function

        Protected Overridable Function FindCompatibleLinkTypes(ByRef bnOrigin As DataObjects.Behavior.Node, _
                                                               ByRef bnDestination As DataObjects.Behavior.Node) _
                                                               As Collections.Links
            Dim aryCompatibleNodes As New ArrayList
            Dim bDone As Boolean = False
            Dim iIndex As Integer = 0
            Dim blDestination As DataObjects.Behavior.Link
            Dim aryCompatible As New Collections.Links(Nothing)

            For Each blOrigin As DataObjects.Behavior.Link In bnOrigin.CompatibleLinks
                bDone = False
                iIndex = 0
                While Not bDone And iIndex < bnDestination.CompatibleLinks.Count
                    blDestination = bnDestination.CompatibleLinks(iIndex)

                    If blDestination.GetType() Is blOrigin.GetType() Then
                        bDone = True
                        aryCompatible.Add(blDestination)
                    End If

                    iIndex = iIndex + 1
                End While
            Next

            Return aryCompatible
        End Function

        Public Overridable Function CreateDiagram(ByVal strAssemblyName As String, ByVal strClassName As String, _
                                                  ByVal bdParent As Behavior.Diagram, Optional ByVal strPageName As String = "") As Behavior.Diagram
            Dim bdDiagram As Forms.Behavior.Diagram = DirectCast(Util.Application.CreateForm(strAssemblyName, _
                                                                 strClassName, "Page", False),  _
                                                                 Forms.Behavior.Diagram)
            Dim afDiagram As AnimatForm = DirectCast(bdParent, AnimatForm)
            bdDiagram.Initialize(afDiagram)

            If strPageName.Trim.Length = 0 AndAlso Not m_tabFiller Is Nothing Then
                bdDiagram.TabPageName = "Page " & (m_tabFiller.TabPages.Count() + 1)
            Else
                bdDiagram.TabPageName = strPageName
            End If

            Dim oTabPage As Crownwood.Magic.Controls.TabPage = New Crownwood.Magic.Controls.TabPage(bdDiagram.TabPageName, bdDiagram)
            m_tabFiller.TabPages.Add(oTabPage)
            m_tabFiller.SelectedTab = oTabPage
            bdDiagram.DiagramIndex = m_tabFiller.TabPages.Count

            'For some reason the new tab that is added is not sizing correctly. However, I found that if you manually
            'change the size of the parent form then it does size correctly from then on. I have no idea why this happens.
            'But doing this statements below seemed like the easiest way to fix this stupid problem. It is ugly, but it works.
            Me.Width = Me.Width - 1
            Me.Height = Me.Height - 1

            Me.Width = Me.Width + 1
            Me.Height = Me.Height + 1

            bdDiagram.AddImages(m_aryDiagramImages)

            Return bdDiagram
        End Function

        Public Overridable Sub RestoreDiagram(ByVal bdDiagram As Forms.Behavior.Diagram)
            Dim oTabPage As Crownwood.Magic.Controls.TabPage = New Crownwood.Magic.Controls.TabPage(bdDiagram.TabPageName, bdDiagram)
            m_tabFiller.TabPages.Add(oTabPage)
            m_tabFiller.SelectedTab = oTabPage

            'For some reason the new tab that is added is not sizing correctly. However, I found that if you manually
            'change the size of the parent form then it does size correctly from then on. I have no idea why this happens.
            'But doing this statements below seemed like the easiest way to fix this stupid problem. It is ugly, but it works.
            Me.Width = Me.Width - 1
            Me.Height = Me.Height - 1

            Me.Width = Me.Width + 1
            Me.Height = Me.Height + 1
        End Sub

        Public Overridable Function AddDiagram(ByVal strAssemblyName As String, ByVal strClassName As String, _
                                               ByVal bdParent As Behavior.Diagram, Optional ByVal strPageName As String = "", _
                                               Optional ByVal strID As String = "") As Behavior.Diagram
            Dim bdDiagram As Forms.Behavior.Diagram = CreateDiagram(strAssemblyName, strClassName, bdParent, strPageName)

            If strID.Trim.Length > 0 Then
                bdDiagram.ID = strID
            End If

            m_aryDiagrams.Add(bdDiagram.ID, bdDiagram)
            m_HierarchyBar.AddDiagram(Nothing, bdDiagram)

            Return bdDiagram
        End Function

        Public Overridable Sub RemoveDiagram(ByVal bdDiagram As Behavior.Diagram)

            If m_aryDiagrams.Contains(bdDiagram.ID) Then
                bdDiagram.RemovingDiagram()
                m_aryDiagrams.Remove(bdDiagram.ID)
            End If

            Dim tabPage As Crownwood.Magic.Controls.TabPage = m_tabFiller.TabPages(bdDiagram.TabPageName)
            If Not tabPage Is Nothing Then
                m_tabFiller.TabPages.Remove(tabPage)
            End If

            m_HierarchyBar.RemoveDiagram(bdDiagram)
        End Sub

        Public Overridable Sub SelectedDiagram(ByVal bdDiagram As Behavior.Diagram)

            Try
                'If the tab we are selecting is the currently selected tab then lets get out of here.
                If Not m_tabFiller.SelectedTab Is Nothing AndAlso m_tabFiller.SelectedTab.Title = bdDiagram.TabPageName Then
                    Return
                End If

                m_tabFiller.SelectedTab = m_tabFiller.TabPages(bdDiagram.TabPageName)
                Me.PropertiesBar.PropertyData = bdDiagram.Properties()
                Me.HierarchyBar.DiagramSelected(bdDiagram)

                'For some reason the diagrams are not sizing correctly once they are zoomed. 
                'I am doing this just to make the editor as a whole call the resize code.
                'This fixes the problem, but it is ugly looking
                Me.Width = Me.Width - 1
                Me.Width = Me.Width + 1

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Public Overridable Sub ClearDiagrams()

            Dim aryIDs As New ArrayList
            For Each deEntry As DictionaryEntry In m_aryDiagrams
                aryIDs.Add(deEntry.Key)
            Next

            Dim bdDiagram As Diagram
            For Each strID As String In aryIDs
                bdDiagram = m_aryDiagrams(strID)
                RemoveDiagram(bdDiagram)
            Next

        End Sub

        Public Overridable Sub ChangeDiagramName(ByVal bdDiagram As Behavior.Diagram, ByVal strNewName As String)

            If strNewName.Trim.Length > 0 Then
                Dim tabPage As Crownwood.Magic.Controls.TabPage = m_tabFiller.TabPages(bdDiagram.TabPageName)
                tabPage.Title = strNewName
                bdDiagram.TabPageName = strNewName
                bdDiagram.DiagramTreeNode.Text = strNewName

                If Not bdDiagram.Subsystem Is Nothing Then
                    bdDiagram.Subsystem.Text = strNewName
                End If
            End If

        End Sub

        Public Overridable Function FindAvailableDiagramName(ByVal strTabPageName As String) As String

            If m_tabFiller.TabPages.Contains(strTabPageName) Then
                Dim strPageName As String

                For iVal As Integer = 1 To 200
                    strPageName = strTabPageName & "." & iVal

                    If Not m_tabFiller.TabPages.Contains(strPageName) Then
                        Return strPageName
                    End If
                Next
            Else
                Return strTabPageName
            End If

        End Function

        Public Overridable Sub SwapDiagramIndex(ByVal bdDiagram As Diagram, ByVal iNewIndex As Integer)

            If iNewIndex <= 0 OrElse iNewIndex > m_tabFiller.TabPages.Count Then
                Throw New System.Exception("The diagram index must be between 1 and " & m_tabFiller.TabPages.Count.ToString & ".")
            End If

            Dim bdSwap As Forms.Behavior.Diagram = DirectCast(m_tabFiller.TabPages(iNewIndex - 1).Control, Forms.Behavior.Diagram)

            bdSwap.DiagramIndex = bdDiagram.DiagramIndex
            bdDiagram.DiagramIndex = iNewIndex
            OrderDiagramTabs()

            m_tabFiller.SelectedIndex = iNewIndex - 1
        End Sub

        Protected Overridable Sub OrderDiagramTabs()

            'Now lets go through and reset the indices for each of the tab pages to maintain order.
            Dim aryPages As Crownwood.Magic.Controls.TabPage()
            ReDim aryPages(m_tabFiller.TabPages.Count - 1)

            Dim bdDiagram As Forms.Behavior.Diagram
            For Each tpPage As Crownwood.Magic.Controls.TabPage In m_tabFiller.TabPages
                bdDiagram = DirectCast(tpPage.Control, Diagram)

                If bdDiagram.DiagramIndex <= aryPages.Length Then
                    aryPages(bdDiagram.DiagramIndex - 1) = tpPage
                Else
                    Debug.WriteLine("Found a diagram index that is out of the range")
                End If
            Next

            'Lets check the pages array to make sure we do not have ANY blank entries
            'If we do then something went wrong and we need to renumber the pages
            Dim bFoundBlank As Boolean = False
            For Each tpPage As Crownwood.Magic.Controls.TabPage In aryPages
                If tpPage Is Nothing Then
                    bFoundBlank = True
                End If
            Next

            'If we found a blank then the diagram index values are screwed up somehow and
            'we need to fix them
            If bFoundBlank Then
                For Each tpPage As Crownwood.Magic.Controls.TabPage In m_tabFiller.TabPages
                    bdDiagram = DirectCast(tpPage.Control, Diagram)
                    bdDiagram.DiagramIndex = FindDiagramTabIndex(bdDiagram)
                    aryPages(bdDiagram.DiagramIndex - 1) = tpPage
                Next
            End If

            'remove all the tabs for now.
            Dim iSize As Integer = m_tabFiller.TabPages.Count - 1
            For iIndex As Integer = 0 To iSize
                m_tabFiller.TabPages.RemoveAt(0)
            Next

            m_tabFiller.TabPages.AddRange(aryPages)

        End Sub

        Public Overridable Function FindDiagramTabIndex(ByVal bdDiagram As Diagram, Optional ByVal bThrowError As Boolean = True) As Integer
            Dim iIndex As Integer = 1
            For Each tpPage As Crownwood.Magic.Controls.TabPage In m_tabFiller.TabPages
                If tpPage.Control Is bdDiagram Then
                    Return iIndex
                End If
                iIndex = iIndex + 1
            Next

            If bThrowError Then
                Throw New System.Exception("The diagram " & bdDiagram.Name & " was not found in the tab pages.")
            End If
            Return -1
        End Function

        Public Overridable Sub ResetDiagramTabIndexes()
            Dim bdDiagram As Forms.Behavior.Diagram
            For Each tpPage As Crownwood.Magic.Controls.TabPage In m_tabFiller.TabPages
                bdDiagram = DirectCast(tpPage.Control, Diagram)
                bdDiagram.DiagramIndex = FindDiagramTabIndex(bdDiagram)
            Next
        End Sub

        Public Overridable Sub CreateDiagramDropDownTree(ByVal tvTree As Crownwood.DotNetMagic.Controls.TreeControl)

            Try

                Dim tnNode As New Crownwood.DotNetMagic.Controls.Node("Behavioral Network")
                tvTree.Nodes.Add(tnNode)

                Dim bdDiagram As Forms.Behavior.Diagram
                For Each deEntry As DictionaryEntry In m_aryDiagrams
                    bdDiagram = DirectCast(deEntry.Value, Forms.Behavior.Diagram)
                    bdDiagram.CreateDiagramDropDownTree(tvTree, tnNode)
                Next

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overridable Function FindDiagram(ByVal strID As String, Optional ByVal bThrowError As Boolean = True) As Forms.Behavior.Diagram

            Dim bdDiagram As Forms.Behavior.Diagram
            Dim bdFound As Forms.Behavior.Diagram
            For Each deEntry As DictionaryEntry In m_aryDiagrams
                bdDiagram = DirectCast(deEntry.Value, Forms.Behavior.Diagram)

                bdFound = bdDiagram.FindDiagram(strID)
                If Not bdFound Is Nothing Then
                    Return bdFound
                End If
            Next

        End Function

        Public Overridable Function FindDiagramByName(ByVal strName As String, Optional ByVal bThrowError As Boolean = True) As Forms.Behavior.Diagram

            Dim bdDiagram As Forms.Behavior.Diagram
            Dim bdFound As Forms.Behavior.Diagram
            For Each deEntry As DictionaryEntry In m_aryDiagrams
                bdDiagram = DirectCast(deEntry.Value, Forms.Behavior.Diagram)

                bdFound = bdDiagram.FindDiagramByName(strName)
                If Not bdFound Is Nothing Then
                    Return bdFound
                End If
            Next

        End Function

        Public Overridable Sub PruneDiagramImages()

            Try

                Dim aryIDs As New ArrayList

                Dim diImage As DataObjects.Behavior.DiagramImage
                For Each deImage As DictionaryEntry In m_aryDiagramImages
                    diImage = DirectCast(deImage.Value, DataObjects.Behavior.DiagramImage)
                    If diImage.UserImage AndAlso ImageUseCount(diImage) = 0 Then
                        aryIDs.Add(diImage.ID)
                    End If
                Next

                For Each strID As String In aryIDs
                    m_aryDiagramImages.Remove(strID)
                Next

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        'Public Overridable Sub AddDiagramImage(ByVal diImage As DataObjects.Behavior.DiagramImage)

        '    Me.DiagramImages.Add(diImage.ID, diImage)

        '    Dim doDiagram As AnimatGUI.Forms.Behavior.Diagram
        '    For Each deEntry As DictionaryEntry In Me.Diagrams
        '        doDiagram = DirectCast(deEntry.Value, AnimatGUI.Forms.Behavior.Diagram)
        '        doDiagram.AddImage(diImage)
        '    Next

        'End Sub

        Public Overridable Function ImageUseCount(ByVal diImage As DataObjects.Behavior.DiagramImage) As Integer

            Dim iCount As Integer = 0
            Dim bdDiagram As Diagram
            For Each deEntry As DictionaryEntry In m_aryDiagrams
                bdDiagram = DirectCast(deEntry.Value, Diagram)
                iCount = iCount + bdDiagram.ImageUseCount(diImage)
            Next

            Return iCount
        End Function

        Public Overridable Sub ZoomBy(ByVal fltDelta As Single)

            Try
                If Not m_tabFiller.SelectedTab Is Nothing Then
                    Dim dgDiagram As Behavior.Diagram = DirectCast(m_tabFiller.SelectedTab.Control, Behavior.Diagram)
                    dgDiagram.ZoomBy(fltDelta)
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overridable Sub ZoomTo(ByVal fltZoom As Single)

            Try
                If Not m_tabFiller.SelectedTab Is Nothing Then
                    Dim dgDiagram As Behavior.Diagram = DirectCast(m_tabFiller.SelectedTab.Control, Behavior.Diagram)
                    dgDiagram.ZoomTo(fltZoom)
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overridable Sub GenerateTempSelectedIDs(ByVal bCopy As Boolean)

            Dim bdDiagram As AnimatGUI.Forms.Behavior.Diagram
            For Each deEntry As DictionaryEntry In m_aryDiagrams
                bdDiagram = DirectCast(deEntry.Value, AnimatGUI.Forms.Behavior.Diagram)
                bdDiagram.GenerateTempSelectedIDs(bCopy)
            Next

        End Sub

        Public Overridable Sub ClearTempSelectedIDs()

            Dim bdDiagram As AnimatGUI.Forms.Behavior.Diagram
            For Each deEntry As DictionaryEntry In m_aryDiagrams
                bdDiagram = DirectCast(deEntry.Value, AnimatGUI.Forms.Behavior.Diagram)
                bdDiagram.ClearTempSelectedIDs()
            Next

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
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            m_aryDiagrams.ClearIsDirty()
            m_aryDiagramImages.ClearIsDirty()
        End Sub

        Public Overridable Function GetNextUndoCode() As Integer
            m_iNextUndoCode = m_iNextUndoCode + 1
            Return m_iNextUndoCode
        End Function

        Public Overridable Sub SelectDataItem(ByVal doItem As AnimatGUI.DataObjects.Behavior.Data, Optional ByVal bOnlyItemSelected As Boolean = True)

            m_doSelectedObject = doItem
            If bOnlyItemSelected Then
                m_arySelectedObjects.Clear()

                If Not m_doSelectedObject Is Nothing Then
                    m_PropertiesBar.PropertyData = m_doSelectedObject.Properties
                    m_arySelectedObjects.Add(m_doSelectedObject)
                Else
                    m_PropertiesBar.PropertyData = Nothing
                End If
            Else
                If Not doItem Is Nothing Then
                    m_arySelectedObjects.Add(doItem)

                    Dim aryItems(m_arySelectedObjects.Count - 1) As AnimatGuiCtrls.Controls.PropertyBag
                    Dim iIndex As Integer = 0
                    For Each doSelPart As AnimatGUI.DataObjects.Behavior.Data In m_arySelectedObjects
                        If Not doSelPart Is Nothing Then
                            aryItems(iIndex) = doSelPart.Properties
                            iIndex = iIndex + 1
                        End If
                    Next

                    m_PropertiesBar.PropertyData = Nothing
                    m_PropertiesBar.PropertyArray = aryItems
                End If
            End If

        End Sub

        Public Overridable Sub DumpNodeLinkInfo()

            Dim bdDiagram As Forms.Behavior.Diagram
            For Each deEntry As DictionaryEntry In m_aryDiagrams
                bdDiagram = DirectCast(deEntry.Value, Forms.Behavior.Diagram)
                bdDiagram.DumpNodeLinkInfo()
            Next

        End Sub

#Region " Load/Save "

        Public Overrides Sub LoadData(ByRef oXml As AnimatGUI.Interfaces.StdXml)

            oXml.IntoElem() 'Into Child Element

            Dim strEditorFile As String = oXml.GetChildString("EditorFile")
            LoadEditorFile(strEditorFile)

            oXml.OutOfElem() 'Outof Child Element
        End Sub

        Public Overridable Overloads Sub LoadEditorFile(ByVal strFilename As String)
            Try
                Util.DisableDirtyFlags = True

                ClearDiagrams()

                Dim oXml As New AnimatGUI.Interfaces.StdXml

                oXml.Load(Util.GetFilePath(Util.Application.ProjectPath, strFilename))
                oXml.FindElement("Editor")

                LoadEditorFile(oXml)
                'Util.Simulation.InitializeSimulationReferences  'Make sure to retag the loaded items so they know they are already created in the simulation.

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                Util.DisableDirtyFlags = False
            End Try

        End Sub

        Protected Overridable Overloads Sub LoadEditorFile(ByRef oXml As AnimatGUI.Interfaces.StdXml)

            Try
                If Util.Application.Simulation Is Nothing Then
                    Throw New System.Exception("No simulation has been loaded for this project. You can not open a behavioral editor without a valid simulation object.")
                End If

                LoadWindowSize(oXml)

                Dim strOrganism As String = oXml.GetChildString("Organism")
                Me.Organism = DirectCast(Util.Application.Simulation.Environment.Organisms(strOrganism), DataObjects.Physical.Organism)
                'Me.Organism.BehavioralNodes.Clear()
                'Me.Organism.BehavioralLinks.Clear()
                Me.Organism.BehaviorEditor = Me
                Me.ModulesBar.PopulateNeuralModules()

                'Now lets go through and load each of the diagrams.
                LoadDiagrams(oXml)
                LoadDiagramImages(oXml)

                'We need to go through and initialize all the diagrams after loading.
                Dim bdDiagram As Forms.Behavior.Diagram
                For Each deEntry As DictionaryEntry In m_aryDiagrams
                    bdDiagram = DirectCast(deEntry.Value, AnimatGUI.Forms.Behavior.Diagram)
                    bdDiagram.InitializeAfterLoad()
                Next

                'Now go back though and verify that the data is valid. There are no missing links or floating graphical items, etc.
                For Each deEntry As DictionaryEntry In m_aryDiagrams
                    bdDiagram = DirectCast(deEntry.Value, AnimatGUI.Forms.Behavior.Diagram)
                    bdDiagram.VerifyData()
                Next

                OrderDiagramTabs()

                'Util.Application.LoadDockingConfig(m_dockManager, oXml)
                SynchronizeToolbarMenu(m_aryToolbarMenus)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub LoadDiagrams(ByRef oXml As AnimatGUI.Interfaces.StdXml)
            Dim strAssemblyFile As String
            Dim strClassName As String
            Dim strPageName As String
            Dim strID As String

            Try

                oXml.IntoChildElement("Diagrams")
                Dim bdDiagram As Forms.Behavior.Diagram
                Dim iCount As Integer = oXml.NumberOfChildren() - 1
                For iIndex As Integer = 0 To iCount
                    oXml.FindChildByIndex(iIndex)
                    oXml.IntoElem() 'Into Diagram element
                    strAssemblyFile = oXml.GetChildString("AssemblyFile")
                    strClassName = oXml.GetChildString("ClassName")
                    strID = Util.LoadID(oXml, "")
                    strPageName = oXml.GetChildString("PageName")
                    oXml.OutOfElem() 'Outof Diagram element

                    bdDiagram = AddDiagram(strAssemblyFile, strClassName, Nothing, strPageName, strID)
                    bdDiagram.LoadData(oXml)
                Next
                oXml.OutOfElem() ' OutOf the Diagrams Element

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub LoadDiagramImages(ByRef oXml As AnimatGUI.Interfaces.StdXml)

            Try

                m_aryDiagramImages.Clear()

                oXml.IntoChildElement("DiagramImages")
                Dim diImage As DataObjects.Behavior.DiagramImage
                Dim iCount As Integer = oXml.NumberOfChildren() - 1
                Dim aryImages(iCount) As DataObjects.Behavior.DiagramImage
                For iIndex As Integer = 0 To iCount
                    oXml.FindChildByIndex(iIndex)

                    diImage = New DataObjects.Behavior.DiagramImage(Nothing)
                    diImage.LoadData(oXml)

                    If Not m_aryDiagramImages.Contains(diImage.ID) Then
                        m_aryDiagramImages.Add(diImage.ID, diImage)
                    End If
                Next
                oXml.OutOfElem()

                ''Now lets go through and copy over any images that were NOT
                ''explicitly loaded by the user. We do not save these images in the file.
                ''we only use the image that already exists.
                'Dim diTempImage As DataObjects.Behavior.DiagramImage
                'iCount = UBound(aryImages)
                'For iIndex As Integer = 0 To iCount
                '    diImage = aryImages(iIndex)

                '    If Not diImage.UserImage Then
                '        'now we need to try and find this image in the in the current list
                '        'and replace the loaded one.
                '        diTempImage = m_aryDiagramImages(diImage.ID)
                '        diTempImage.DiagramIndex = diImage.DiagramIndex
                '        aryImages(diImage.DiagramIndex) = diTempImage
                '    End If
                'Next

                ''Now we clear out all of the old diagram images.
                'm_aryDiagramImages.Clear()

                ''And then add them back in the correct order.
                'For Each diImage In aryImages
                '    m_aryDiagramImages.Add(diImage.ID, diImage)
                'Next

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Public Overrides Sub SaveData(ByRef oXml As AnimatGUI.Interfaces.StdXml)

            oXml.AddChildElement("Child")
            oXml.IntoElem() 'Into Child Elemement

            oXml.AddChildElement("AssemblyFile", Me.AssemblyFile)
            oXml.AddChildElement("ClassName", Me.ClassName)
            oXml.AddChildElement("EditorFile", Me.Organism.BehavioralEditorFile)

            oXml.AddChildElement("Form")
            oXml.IntoElem() 'Into Form Element

            oXml.AddChildElement("AssemblyFile", "LicensedAnimatGUI.dll")
            oXml.AddChildElement("ClassName", "LicensedAnimatGUI.Forms.Behavior.AddFlowDiagram")
            oXml.AddChildElement("Title", Me.Title)

            oXml.OutOfElem()  'Outof Form Element

            SaveEditorFile(Me.Organism.BehavioralEditorFile)

            oXml.OutOfElem()  'Outof Child Element
        End Sub

        Protected Overridable Overloads Sub SaveEditorFile(ByVal strFilename As String)

            Try
                Dim oXml As New AnimatGUI.Interfaces.StdXml

                SaveEditorFile(oXml)
                oXml.Save(Util.GetFilePath(Util.Application.ProjectPath, strFilename))

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Overloads Sub SaveEditorFile(ByRef oXml As AnimatGUI.Interfaces.StdXml)

            Try
                oXml.AddElement("Editor")

                oXml.AddChildElement("AssemblyFile", Me.AssemblyFile)
                oXml.AddChildElement("ClassName", Me.ClassName)

                SaveWindowSize(oXml)

                If Not m_doOrganism Is Nothing Then
                    oXml.AddChildElement("Organism", m_doOrganism.ID)
                End If

                SaveNeuralModules(oXml)
                SaveDiagrams(oXml)
                PruneDiagramImages()
                SaveDiagramImages(oXml)

                'Util.Application.SaveDockingConfig(m_dockManager, oXml)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub SaveNeuralModules(ByRef oXml As AnimatGUI.Interfaces.StdXml)

            Try

                oXml.AddChildElement("NeuralModules")
                oXml.IntoElem() 'Into NeuralModules Element
                Dim nmModule As AnimatGUI.DataObjects.Behavior.NeuralModule
                For Each deEntry As DictionaryEntry In m_doOrganism.NeuralModules
                    nmModule = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Behavior.NeuralModule)
                    nmModule.SaveData(oXml)
                Next
                oXml.OutOfElem() 'Outof NeuralModules Element

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub SaveDiagrams(ByRef oXml As AnimatGUI.Interfaces.StdXml)

            Try

                'Now lets go through and save each of the diagrams.
                oXml.AddChildElement("Diagrams")
                oXml.IntoElem()
                Dim bdDiagram As Forms.Behavior.Diagram
                For Each deEntry As DictionaryEntry In m_aryDiagrams
                    bdDiagram = DirectCast(deEntry.Value, Forms.Behavior.Diagram)
                    bdDiagram.SaveData(oXml)
                Next
                oXml.OutOfElem()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub SaveDiagramImages(ByRef oXml As AnimatGUI.Interfaces.StdXml)

            Try

                'Now go through and save the diagram images
                oXml.AddChildElement("DiagramImages")
                oXml.IntoElem()
                Dim diImage As DataObjects.Behavior.DiagramImage
                For Each deEntry As DictionaryEntry In m_aryDiagramImages
                    diImage = DirectCast(deEntry.Value, DataObjects.Behavior.DiagramImage)
                    diImage.SaveData(oXml)
                Next
                oXml.OutOfElem()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

#End Region

#Region " Events "

        Protected Sub OnEditPopupStart(ByVal mc As MenuCommand)

            Try
                If Not m_tabFiller.SelectedTab Is Nothing Then
                    Dim dgDiagram As Behavior.Diagram = DirectCast(m_tabFiller.SelectedTab.Control, Behavior.Diagram)
                    dgDiagram.OnEditPopupStart(mc)
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnEditPopupEnd(ByVal mc As MenuCommand)

            Try
                If Not m_tabFiller.SelectedTab Is Nothing Then
                    Dim dgDiagram As Behavior.Diagram = DirectCast(m_tabFiller.SelectedTab.Control, Behavior.Diagram)
                    dgDiagram.OnEditPopupEnd(mc)
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnShapePopupStart(ByVal mc As MenuCommand)

            Try
                If Not m_tabFiller.SelectedTab Is Nothing Then
                    Dim dgDiagram As Behavior.Diagram = DirectCast(m_tabFiller.SelectedTab.Control, Behavior.Diagram)
                    dgDiagram.OnShapePopupStart(mc)
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnShapePopupEnd(ByVal mc As MenuCommand)

            Try
                If Not m_tabFiller.SelectedTab Is Nothing Then
                    Dim dgDiagram As Behavior.Diagram = DirectCast(m_tabFiller.SelectedTab.Control, Behavior.Diagram)
                    dgDiagram.OnShapePopupEnd(mc)
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnOpen(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnSave(ByVal sender As Object, ByVal e As System.EventArgs)

            Try


            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnInsertPage(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                AddDiagram("LicensedAnimatGUI.dll", "LicensedAnimatGUI.Forms.Behavior.AddFlowDiagram", Nothing)
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnAddStimulus(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If Not m_tabFiller.SelectedTab Is Nothing Then
                    Dim dgDiagram As Behavior.Diagram = DirectCast(m_tabFiller.SelectedTab.Control, Behavior.Diagram)
                    dgDiagram.AddStimulusToSelected()
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnSelectByType(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If Not m_tabFiller.SelectedTab Is Nothing Then
                    Dim dgDiagram As Behavior.Diagram = DirectCast(m_tabFiller.SelectedTab.Control, Behavior.Diagram)
                    dgDiagram.SelectByType()
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnRelabel(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If Not m_tabFiller.SelectedTab Is Nothing Then
                    Dim dgDiagram As Behavior.Diagram = DirectCast(m_tabFiller.SelectedTab.Control, Behavior.Diagram)
                    dgDiagram.Relabel()
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnRelabelSelected(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If Not m_tabFiller.SelectedTab Is Nothing Then
                    Dim dgDiagram As Behavior.Diagram = DirectCast(m_tabFiller.SelectedTab.Control, Behavior.Diagram)
                    dgDiagram.RelabelSelected()
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnShowConnections(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If Not m_tabFiller.SelectedTab Is Nothing Then
                    Dim dgDiagram As Behavior.Diagram = DirectCast(m_tabFiller.SelectedTab.Control, Behavior.Diagram)
                    dgDiagram.OnShowConnections(sender, e)
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnCompareItems(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If Not m_tabFiller.SelectedTab Is Nothing Then
                    Dim dgDiagram As Behavior.Diagram = DirectCast(m_tabFiller.SelectedTab.Control, Behavior.Diagram)
                    dgDiagram.OnCompareItems(sender, e)
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub


        Protected Sub OnCut(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If MessageBox.Show("Are you sure you want to cut the selected items?", "Cut Selected", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                    If Not m_tabFiller.SelectedTab Is Nothing Then
                        Dim dgDiagram As Behavior.Diagram = DirectCast(m_tabFiller.SelectedTab.Control, Behavior.Diagram)
                        dgDiagram.CutSelected()
                    End If
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnCopy(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If Not m_tabFiller.SelectedTab Is Nothing Then
                    Dim dgDiagram As Behavior.Diagram = DirectCast(m_tabFiller.SelectedTab.Control, Behavior.Diagram)
                    dgDiagram.CopySelected()
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnPaste(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If Not m_tabFiller.SelectedTab Is Nothing Then
                    Dim dgDiagram As Behavior.Diagram = DirectCast(m_tabFiller.SelectedTab.Control, Behavior.Diagram)
                    dgDiagram.PasteSelected(False)
                    ResetDiagramTabIndexes()
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnPasteInPlace(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If Not m_tabFiller.SelectedTab Is Nothing Then
                    Dim dgDiagram As Behavior.Diagram = DirectCast(m_tabFiller.SelectedTab.Control, Behavior.Diagram)
                    dgDiagram.PasteSelected(True)
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnDelete(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If MessageBox.Show("Are you sure you want to delete the selected items?", "Delete Selected", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                    If Not m_tabFiller.SelectedTab Is Nothing Then
                        Dim dgDiagram As Behavior.Diagram = DirectCast(m_tabFiller.SelectedTab.Control, Behavior.Diagram)
                        dgDiagram.DeleteSelected()
                        dgDiagram.RefreshDiagram()
                    End If
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnUndo(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If Not m_tabFiller.SelectedTab Is Nothing Then
                    Dim dgDiagram As Behavior.Diagram = DirectCast(m_tabFiller.SelectedTab.Control, Behavior.Diagram)
                    dgDiagram.OnUndo()
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnRedo(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If Not m_tabFiller.SelectedTab Is Nothing Then
                    Dim dgDiagram As Behavior.Diagram = DirectCast(m_tabFiller.SelectedTab.Control, Behavior.Diagram)
                    dgDiagram.OnRedo()
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Sub UndoRedoRefresh(ByVal oRefresh As Object)

            If Not oRefresh Is Nothing Then
                Dim doRefresh As AnimatGUI.Framework.DataObject

                If TypeOf oRefresh Is AnimatGUI.Framework.DataObject Then
                    doRefresh = DirectCast(oRefresh, AnimatGUI.Framework.DataObject)
                End If

                If TypeOf oRefresh Is AnimatGUI.DataObjects.Behavior.Data Then
                    Dim doData As AnimatGUI.DataObjects.Behavior.Data = DirectCast(oRefresh, AnimatGUI.DataObjects.Behavior.Data)

                    Me.SelectedDiagram(doData.ParentDiagram)

                    If Me.SelectedObject Is doData Then
                        Me.PropertiesBar.RefreshProperties()
                    Else
                        doData.ParentDiagram.SelectDataItem(doData)
                    End If

                ElseIf Not doRefresh Is Nothing AndAlso Not doRefresh.Parent Is Nothing AndAlso TypeOf doRefresh.Parent Is AnimatGUI.DataObjects.Behavior.Data Then
                    Dim doData As AnimatGUI.DataObjects.Behavior.Data = DirectCast(doRefresh.Parent, AnimatGUI.DataObjects.Behavior.Data)

                    Me.SelectedDiagram(doData.ParentDiagram)

                    If Me.SelectedObject Is doData Then
                        Me.PropertiesBar.RefreshProperties()
                    Else
                        doData.ParentDiagram.SelectDataItem(doData)
                    End If
                ElseIf Not doRefresh Is Nothing AndAlso TypeOf doRefresh Is AnimatGUI.DataObjects.Behavior.NeuralModule Then
                    If Not Me.ModulesBar.Content Is Nothing AndAlso Not Me.ModulesBar.Content.ParentWindowContent Is Nothing Then
                        Me.ModulesBar.Content.ParentWindowContent.BringContentToFront(Me.ModulesBar.Content)
                    End If

                    Me.ModulesBar.RefreshProperties()
                Else
                    Me.PropertiesBar.RefreshProperties()
                End If
            Else
                Me.PropertiesBar.RefreshProperties()
            End If

        End Sub

        Public Overrides Sub RefreshProperties()
            Me.PropertiesBar.RefreshProperties()
        End Sub

#Region " Toolbar View Events "

        Protected Sub OnViewToolboxBar(ByVal sender As Object, ByVal e As System.EventArgs)
            ViewToolbar(m_OutlookContent)
        End Sub

        Protected Sub OnViewPropertiesBar(ByVal sender As Object, ByVal e As System.EventArgs)
            ViewToolbar(m_PropertiesContent)
        End Sub

        Protected Sub OnViewHierarchyBar(ByVal sender As Object, ByVal e As System.EventArgs)
            ViewToolbar(m_HierarchyContent)
        End Sub
        Protected Sub OnViewModulesBar(ByVal sender As Object, ByVal e As System.EventArgs)
            ViewToolbar(m_ModulesContent)
        End Sub
        Protected Sub OnViewErrorsBar(ByVal sender As Object, ByVal e As System.EventArgs)
            ViewToolbar(m_ErrorsContent)
        End Sub

#End Region

#Region " Zoom Events "

        Protected Sub OnFitToPage(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If Not m_tabFiller.SelectedTab Is Nothing Then
                    Dim dgDiagram As Behavior.Diagram = DirectCast(m_tabFiller.SelectedTab.Control, Behavior.Diagram)
                    dgDiagram.FitToPage()
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

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

#Region " Format Events "

        Protected Sub OnAlignTop(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If Not m_tabFiller.SelectedTab Is Nothing Then
                    Dim dgDiagram As Behavior.Diagram = DirectCast(m_tabFiller.SelectedTab.Control, Behavior.Diagram)
                    dgDiagram.OnAlignTop(sender, e)
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnAlignVerticalCenter(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If Not m_tabFiller.SelectedTab Is Nothing Then
                    Dim dgDiagram As Behavior.Diagram = DirectCast(m_tabFiller.SelectedTab.Control, Behavior.Diagram)
                    dgDiagram.OnAlignVerticalCenter(sender, e)
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnAlignBottom(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If Not m_tabFiller.SelectedTab Is Nothing Then
                    Dim dgDiagram As Behavior.Diagram = DirectCast(m_tabFiller.SelectedTab.Control, Behavior.Diagram)
                    dgDiagram.OnAlignBottom(sender, e)
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnAlignLeft(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If Not m_tabFiller.SelectedTab Is Nothing Then
                    Dim dgDiagram As Behavior.Diagram = DirectCast(m_tabFiller.SelectedTab.Control, Behavior.Diagram)
                    dgDiagram.OnAlignLeft(sender, e)
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnAlignHorizontalCenter(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If Not m_tabFiller.SelectedTab Is Nothing Then
                    Dim dgDiagram As Behavior.Diagram = DirectCast(m_tabFiller.SelectedTab.Control, Behavior.Diagram)
                    dgDiagram.OnAlignHorizontalCenter(sender, e)
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnAlignRight(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If Not m_tabFiller.SelectedTab Is Nothing Then
                    Dim dgDiagram As Behavior.Diagram = DirectCast(m_tabFiller.SelectedTab.Control, Behavior.Diagram)
                    dgDiagram.OnAlignRight(sender, e)
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnDistributeVertical(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If Not m_tabFiller.SelectedTab Is Nothing Then
                    Dim dgDiagram As Behavior.Diagram = DirectCast(m_tabFiller.SelectedTab.Control, Behavior.Diagram)
                    dgDiagram.OnDistributeVertical(sender, e)
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnDistributeHorizontal(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If Not m_tabFiller.SelectedTab Is Nothing Then
                    Dim dgDiagram As Behavior.Diagram = DirectCast(m_tabFiller.SelectedTab.Control, Behavior.Diagram)
                    dgDiagram.OnDistributeHorizontal(sender, e)
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnSizeBoth(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If Not m_tabFiller.SelectedTab Is Nothing Then
                    Dim dgDiagram As Behavior.Diagram = DirectCast(m_tabFiller.SelectedTab.Control, Behavior.Diagram)
                    dgDiagram.OnSizeBoth(sender, e)
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnSizeWidth(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If Not m_tabFiller.SelectedTab Is Nothing Then
                    Dim dgDiagram As Behavior.Diagram = DirectCast(m_tabFiller.SelectedTab.Control, Behavior.Diagram)
                    dgDiagram.OnSizeWidth(sender, e)
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnSizeHeight(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If Not m_tabFiller.SelectedTab Is Nothing Then
                    Dim dgDiagram As Behavior.Diagram = DirectCast(m_tabFiller.SelectedTab.Control, Behavior.Diagram)
                    dgDiagram.OnSizeHeight(sender, e)
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

        Protected Sub OnExport(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                Dim frmExport As New Forms.Behavior.Export

                If frmExport.ShowDialog(Me) = DialogResult.OK Then
                    Dim strPath As String = frmExport.FileLocation
                    Dim bdDiagram As Forms.Behavior.Diagram
                    Dim eFormat As System.Drawing.Imaging.ImageFormat = frmExport.Format
                    Dim strExtension As String = frmExport.Extension

                    If frmExport.AllDiagrams Then
                        For Each deEntry As DictionaryEntry In m_aryDiagrams
                            bdDiagram = DirectCast(deEntry.Value, Forms.Behavior.Diagram)
                            bdDiagram.SaveDiagrams(strPath, eFormat, strExtension)
                        Next
                    Else
                        If Not m_tabFiller.SelectedTab Is Nothing Then
                            Dim dgDiagram As Behavior.Diagram = DirectCast(m_tabFiller.SelectedTab.Control, Behavior.Diagram)
                            dgDiagram.SaveDiagram(strPath, eFormat)
                        End If
                    End If

                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#Region " Print Events "

        Protected Sub OnPageSetup(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                m_PrintHelper.PageSetup()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnPrintPreview(ByVal sender As Object, ByVal e As System.EventArgs)

            Try

                If Not m_tabFiller.SelectedTab Is Nothing Then
                    Dim bdDiagram As Behavior.Diagram
                    Dim aryMetaDocs As New Collections.MetaDocuments(Nothing)

                    For Each deEntry As DictionaryEntry In m_aryDiagrams
                        bdDiagram = DirectCast(deEntry.Value, Behavior.Diagram)
                        bdDiagram.GenerateMetafiles(aryMetaDocs)
                    Next

                    m_PrintHelper.Preview(aryMetaDocs)
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnPrintDiagram(ByVal sender As Object, ByVal e As System.EventArgs)

            Try

                If Not m_tabFiller.SelectedTab Is Nothing Then
                    Dim bdDiagram As Behavior.Diagram
                    Dim aryMetaDocs As New Collections.MetaDocuments(Nothing)

                    For Each deEntry As DictionaryEntry In m_aryDiagrams
                        bdDiagram = DirectCast(deEntry.Value, Behavior.Diagram)
                        bdDiagram.GenerateMetafiles(aryMetaDocs)
                    Next

                    m_PrintHelper.Print(aryMetaDocs)
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

        Protected Sub OnTabSelectionChanging(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim tabControl As Crownwood.Magic.Controls.TabControl = DirectCast(sender, Crownwood.Magic.Controls.TabControl)
            Dim tabPage As Crownwood.Magic.Controls.TabPage = tabControl.SelectedTab

            If Not tabPage Is Nothing Then
                Dim afDiagram As Behavior.Diagram = DirectCast(tabPage.Control, Behavior.Diagram)
                afDiagram.TabDeselected()
            End If
        End Sub

        Protected Sub OnTabSelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim tabControl As Crownwood.Magic.Controls.TabControl = DirectCast(sender, Crownwood.Magic.Controls.TabControl)
            Dim tabPage As Crownwood.Magic.Controls.TabPage = tabControl.SelectedTab

            If Not tabPage Is Nothing Then
                Dim bdDiagram As Behavior.Diagram = DirectCast(tabPage.Control, Behavior.Diagram)
                bdDiagram.TabSelected()
                m_HierarchyBar.DiagramSelected(bdDiagram)

                'For some reason the diagrams are not sizing correctly once they are zoomed. 
                'I am doing this just to make the editor as a whole call the resize code.
                'This fixes the problem, but it is ugly looking
                Me.Width = Me.Width - 1
                Me.Width = Me.Width + 1
            End If
        End Sub

        Protected Sub OnDockContentClosing(ByVal c As Content, ByVal e As System.ComponentModel.CancelEventArgs)
            ViewToolbar(c)
            e.Cancel = True
        End Sub


        Protected Overrides Sub OnFormClosing(ByVal e As System.Windows.Forms.FormClosingEventArgs)
            MyBase.OnFormClosing(e)

            Try
                If Not m_doOrganism Is Nothing Then
                    m_doOrganism.BehaviorEditor = Nothing
                End If

                Util.ModificationHistory.RemoveMdiEvents(Me)

                'RemoveHandler Util.Application.UnitsChanged, AddressOf Me.Application_UnitsChanged
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overrides Sub OnFormClosed(ByVal e As System.Windows.Forms.FormClosedEventArgs)
            MyBase.OnFormClosed(e)

            'If Not m_doStructure Is Nothing Then
            '    m_doStructure.BodyEditor = Nothing
            'End If

        End Sub

#End Region

    End Class

End Namespace

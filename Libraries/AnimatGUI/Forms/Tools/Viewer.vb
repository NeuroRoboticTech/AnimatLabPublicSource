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

Namespace Forms.Tools

    Public Class Viewer
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
            Me.Title = "Body Plan Editor"
            Me.Size = New Size(600, 400)
        End Sub

#End Region

#Region " Attributes "

        Protected m_HierarchyBar As Forms.Tools.Hierarchy
        Protected m_HierarchyContent As Content
        Protected m_mcHierarchyBar As MenuCommand

        'Protected m_ErrorsBar As Forms.Behavior.Errors
        'Protected m_ErrorsContent As Content
        'Protected m_mcErrorsBar As MenuCommand

        Protected m_bCreateHierarchyBar As Boolean = True
        'Protected m_bCreatePropertyBar As Boolean = True

        Protected m_doSelectedObject As Framework.DataObject
        Protected m_aryToolbarMenus As New SortedList

        Protected m_doToolHolder As DataObjects.ToolHolder
        Protected m_iToolCount As Integer

#End Region

#Region " Properties "

        Public Overrides Property Title() As String
            Get
                Return m_strTitle
            End Get
            Set(ByVal Value As String)
                m_strTitle = Value
                Me.Text = m_strTitle

                'Change the name of the primary docked viewer to match the new title as well.
                Dim frmTool As AnimatTools.Forms.Tools.ToolForm
                If Not m_frmControl Is Nothing Then
                    frmTool = DirectCast(m_frmControl, AnimatTools.Forms.Tools.ToolForm)
                    frmTool.Title = m_strTitle
                End If

            End Set
        End Property

        Public Overridable ReadOnly Property ToolViewerFile() As String
            Get
                Return Me.Title & ".atvf"
            End Get
        End Property

        Public Overridable Property SelectedObject() As Framework.DataObject
            Get
                Return m_doSelectedObject
            End Get
            Set(ByVal Value As Framework.DataObject)
                m_doSelectedObject = Value
                If Not m_doSelectedObject Is Nothing Then
                    m_HierarchyBar.PropertyData = m_doSelectedObject.Properties
                Else
                    m_HierarchyBar.PropertyData = Nothing
                End If
            End Set
        End Property

        Public Overridable ReadOnly Property HierarchyBar() As Forms.Tools.Hierarchy
            Get
                Return m_HierarchyBar
            End Get
        End Property

        'Public Overridable ReadOnly Property ErrorsBar() As Forms.Behavior.Errors
        '    Get
        '        Return m_ErrorsBar
        '    End Get
        'End Property

        Public Overridable Property ToolCount() As Integer
            Get
                Return m_iToolCount
            End Get
            Set(ByVal Value As Integer)
                m_iToolCount = Value
            End Set
        End Property

        Public Overridable Property ToolHolder() As DataObjects.ToolHolder
            Get
                Return m_doToolHolder
            End Get
            Set(ByVal Value As DataObjects.ToolHolder)
                m_doToolHolder = Value
            End Set
        End Property

#End Region

#Region " Methods "

        Public Overrides Sub Initialize(ByVal frmApplication As AnimatApplication, _
                                        ByVal frmControl As AnimatForm)

            Try
                Util.DisableDirtyFlags = True

                Util.Application = Util.Application

                CreateImageManager()
                MyBase.Initialize(Util.Application, frmControl)

                Dim wndContent As Crownwood.Magic.Docking.WindowContent
                If m_bCreateHierarchyBar Then
                    m_HierarchyBar = New Forms.Tools.Hierarchy
                    m_HierarchyBar.Initialize()

                    m_HierarchyBar.Location = New Point(0, 0)
                    m_HierarchyContent = m_dockManager.Contents.Add(m_HierarchyBar, "Hierarchy", m_mgrToolStripImages.ImageList, m_mgrToolStripImages.GetImageIndex("AnimatTools.ProjectWorkspace.gif"))
                    m_HierarchyContent.DisplaySize = New Size(250, 150)
                    m_HierarchyContent.AutoHideSize = New Size(250, 150)
                    m_HierarchyContent.UserData = m_HierarchyBar

                    wndContent = m_dockManager.AddContentWithState(m_HierarchyContent, State.DockLeft)
                End If

                'm_ErrorsBar = New Forms.Behavior.Errors
                'm_ErrorsBar.Initialize(Util.Application, Me)

                'm_ErrorsBar.Location = New Point(0, 0)
                'm_ErrorsContent = m_dockManager.Contents.Add(m_ErrorsBar, "Neural Network Errors")
                'm_ErrorsContent.DisplaySize = New Size(250, 150)
                'm_ErrorsContent.UserData = m_ErrorsBar

                'wndContent = m_dockManager.AddContentWithState(m_ErrorsContent, State.DockBottom)

                AddHandler m_dockManager.ContentClosing, AddressOf Me.OnDockContentClosing

                If Not wndContent Is Nothing Then
                    m_dockManager.AutoHideWindow(wndContent)
                End If

                'I think there is a bug in the docking code. When the window first appears it does not appear
                'to be taking into account the size of the tabs when calculating the height of the bars. This
                'corrects for that.

                m_aryToolbarMenus.Clear()

                If Not m_HierarchyBar Is Nothing Then
                    m_HierarchyBar.Height = m_HierarchyBar.Height - 70
                    m_aryToolbarMenus.Add(m_HierarchyContent.Title, m_mcHierarchyBar)
                End If

                Dim myAssembly As System.Reflection.Assembly
                myAssembly = System.Reflection.Assembly.Load("AnimatTools")
                Me.Icon = Util.Application.ToolStripImages.LoadIcon(myAssembly, "AnimatTools.Wrench.ico")

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            Finally
                Util.DisableDirtyFlags = False
            End Try

        End Sub

#Region " Menu/Toolbar Methods "

        Protected Overrides Sub CreateToolstrips()
            If Util.Application Is Nothing Then Throw New System.Exception("Application object is not defined.")
            If m_frmControl Is Nothing Then Throw New System.Exception("Mdi Control object is not defined.")

            'm_frmControl.CreateToolStrips()
        End Sub

        'Protected Overrides Sub CreateToolbar()
        '    If Util.Application Is Nothing Then Throw New System.Exception("Application object is not defined.")
        '    If m_frmControl Is Nothing Then Throw New System.Exception("Mdi Control object is not defined.")

        '    m_barMain = m_frmControl.CreateToolbar(m_menuMain)
        'End Sub

        Protected Overridable Sub CreateImageManager()

            Try

                Dim myAssembly As System.Reflection.Assembly
                myAssembly = System.Reflection.Assembly.Load("AnimatTools")

                m_mgrToolStripImages = New AnimatTools.Framework.ImageManager
                m_mgrToolStripImages.ImageList.ImageSize = New Size(16, 16)
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatTools.PageSetup.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatTools.PrintPreview.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatTools.Print.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatTools.Export.gif")
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
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatTools.RedoGrey.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatTools.ProjectWorkspace.gif")
                m_mgrToolStripImages.AddImage(myAssembly, "AnimatTools.Properties.gif")

                Util.Application.LargeImages.AddImage(myAssembly, "AnimatTools.ZoomInLarge.gif")
                Util.Application.LargeImages.AddImage(myAssembly, "AnimatTools.ZoomOutLarge.gif")

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

        Protected Function FindViewerCount(ByVal strRootname As String) As Integer
            Dim strNumber As String
            Dim iNumber As Integer, iMax As Integer = -1

            strRootname = strRootname & "_"
            strRootname = strRootname.ToUpper
            Dim frmTool As Forms.Tools.ToolForm
            For Each oContent As Crownwood.Magic.Docking.Content In m_dockManager.Contents
                If Not oContent.UserData Is Nothing AndAlso TypeOf oContent.UserData Is Forms.Tools.ToolForm AndAlso Not oContent.UserData Is m_frmControl Then
                    frmTool = DirectCast(oContent.UserData, Forms.Tools.ToolForm)

                    strNumber = frmTool.Title.Substring(strRootname.Length - 1)

                    If IsNumeric(strNumber) AndAlso Not InStr(strNumber, ".") > 0 AndAlso Not InStr(strNumber, "-") > 0 Then
                        iNumber = CInt(strNumber)
                        If iNumber > iMax OrElse iMax < 0 Then
                            iMax = iNumber
                        End If
                    End If
                End If
            Next

            If iMax < 0 Then
                Return 0
            Else
                Return iMax
            End If
        End Function

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

            Dim frmTool As Forms.AnimatForm
            For Each oContent As Crownwood.Magic.Docking.Content In m_dockManager.Contents
                If Not oContent.UserData Is Nothing AndAlso TypeOf oContent.UserData Is Forms.AnimatForm Then
                    frmTool = DirectCast(oContent.UserData, Forms.AnimatForm)

                    frmTool.ClearIsDirty()
                End If
            Next

        End Sub

        Public Overrides Sub UndoRedoRefresh(ByVal oRefresh As Object)

            If Not oRefresh Is Nothing Then
                If Me.m_HierarchyBar.SelectedItem Is oRefresh Then
                    Me.m_HierarchyBar.RefreshProperties()
                Else
                    Me.m_HierarchyBar.SelectItem(oRefresh)
                End If
            End If

        End Sub

        Public Overrides Sub RefreshProperties()
            Me.m_HierarchyBar.RefreshProperties()
        End Sub

#Region " Load/Save "

        Public Overrides Sub LoadData(ByRef oXml As AnimatTools.Interfaces.StdXml)

            oXml.IntoElem() 'Into Child Element

            Me.HierarchyBar.ctrlTreeView.Height = oXml.GetChildInt("TreeViewHeight", CInt(Me.Height * 0.6))

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

                Dim strToolHolder As String = oXml.GetChildString("ToolHolder")
                m_doToolHolder = Util.Simulation.ToolHolders(strToolHolder)

                If m_doToolHolder Is Nothing Then
                    Throw New System.Exception("The toolholder for viewer '" & Me.Title & "' was not found.")
                End If

                oXml.FindChildElement("Form")
                m_frmControl.LoadData(oXml)
                Dim frmTool As AnimatTools.Forms.Tools.ToolForm = DirectCast(m_frmControl, AnimatTools.Forms.Tools.ToolForm)
                frmTool.ToolHolder = m_doToolHolder

                LoadWindowSize(oXml)

                'Now lets go through and load each of the diagrams.
                Dim strAssemblyFile As String
                Dim strClassName As String
                Dim strTitle As String

                If (oXml.FindChildElement("ToolForms", False)) Then
                    oXml.IntoChildElement("ToolForms")
                    Dim iCount As Integer = oXml.NumberOfChildren() - 1
                    For iIndex As Integer = 0 To iCount
                        oXml.FindChildByIndex(iIndex)

                        oXml.IntoElem() 'Into Form element
                        strAssemblyFile = oXml.GetChildString("AssemblyFile")
                        strClassName = oXml.GetChildString("ClassName")
                        strTitle = oXml.GetChildString("Title")
                        oXml.OutOfElem() 'Outof Diagram element

                        frmTool = DirectCast(Util.Application.CreateForm(strAssemblyFile, strClassName, strTitle, Me), AnimatTools.Forms.Tools.ToolForm)
                        'Util.Application.AddDockingForm(m_dockManager, frmTool, Me, strTitle)
                        frmTool.LoadData(oXml)
                        frmTool.ToolHolder = m_doToolHolder
                    Next
                    oXml.OutOfElem()
                End If

                'Util.Application.LoadDockingConfig(m_dockManager, oXml)
                SynchronizeToolbarMenu(m_aryToolbarMenus)

                m_iToolCount = FindViewerCount("Tool_")

                m_HierarchyBar.CreateTreeView()

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Public Overrides Sub SaveData(ByRef oXml As AnimatTools.Interfaces.StdXml)

            oXml.AddChildElement("Child")
            oXml.IntoElem() 'Into Child Elemement

            oXml.AddChildElement("AssemblyFile", Me.AssemblyFile)
            oXml.AddChildElement("ClassName", Me.ClassName)
            oXml.AddChildElement("EditorFile", Me.ToolViewerFile)

            If Not Me.HierarchyBar Is Nothing Then
                oXml.AddChildElement("TreeViewHeight", Me.HierarchyBar.ctrlTreeView.Height)
            End If

            oXml.AddChildElement("Form")
            oXml.IntoElem() 'Into Form Element

            oXml.AddChildElement("AssemblyFile", m_frmControl.AssemblyFile)
            oXml.AddChildElement("ClassName", m_frmControl.ClassName)
            oXml.AddChildElement("Title", Me.Title)

            oXml.OutOfElem()  'Outof Form Element

            SaveEditorFile(Me.ToolViewerFile)

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

            oXml.AddElement("Editor")

            oXml.AddChildElement("AssemblyFile", Me.AssemblyFile)
            oXml.AddChildElement("ClassName", Me.ClassName)

            SaveWindowSize(oXml)

            oXml.AddChildElement("ToolHolder", m_doToolHolder.ID)

            m_frmControl.SaveData(oXml)

            oXml.AddChildElement("ToolForms")

            oXml.IntoElem()
            Dim frmTool As Forms.Tools.ToolForm
            For Each oContent As Crownwood.Magic.Docking.Content In m_dockManager.Contents
                If Not oContent.UserData Is Nothing AndAlso TypeOf oContent.UserData Is Forms.Tools.ToolForm AndAlso Not oContent.UserData Is m_frmControl Then
                    frmTool = DirectCast(oContent.UserData, Forms.Tools.ToolForm)

                    frmTool.SaveData(oXml)
                End If
            Next
            oXml.OutOfElem()

            'Util.Application.SaveDockingConfig(m_dockManager, oXml)
        End Sub

        Public Overrides Sub SaveSimulationXml(ByRef oXml As AnimatTools.Interfaces.StdXml, Optional ByRef nmParentControl As AnimatTools.Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            Dim frmChart As AnimatTools.Forms.Tools.DataChart
            For Each conWindow As Content In m_dockManager.Contents
                If TypeOf (conWindow.Control) Is AnimatTools.Forms.Tools.DataChart Then
                    frmChart = DirectCast(conWindow.Control, AnimatTools.Forms.Tools.DataChart)
                    frmChart.SaveSimulationXml(oXml)
                End If
            Next

            If TypeOf m_frmControl Is AnimatTools.Forms.Tools.DataChart Then
                frmChart = DirectCast(m_frmControl, AnimatTools.Forms.Tools.DataChart)
                frmChart.SaveSimulationXml(oXml)
            End If

        End Sub

#End Region

#End Region

#Region " Events "


        Public Sub OnEditPopupStart(ByVal mc As MenuCommand)
            'If Me.PropertiesBar.SelectedParts.Count > 0 Then
            '    mc.MenuCommands("Cut").Enabled = True
            '    mc.MenuCommands("Copy").Enabled = True
            '    mc.MenuCommands("Delete").Enabled = True
            'Else
            '    mc.MenuCommands("Cut").Enabled = False
            '    mc.MenuCommands("Copy").Enabled = False
            '    mc.MenuCommands("Delete").Enabled = False
            'End If

            'mc.MenuCommands("Paste").Enabled = False
            'Dim data As IDataObject = Clipboard.GetDataObject()
            'If Not data Is Nothing AndAlso data.GetDataPresent("AnimatLab.Body.XMLFormat") Then
            '    Dim strXml As String = DirectCast(data.GetData("AnimatLab.Body.XMLFormat"), String)
            '    If strXml.Trim.Length > 0 Then
            '        mc.MenuCommands("Paste").Enabled = True
            '    End If
            'End If

            mc.MenuCommands("Undo").Enabled = Util.ModificationHistory.CanUndo
            mc.MenuCommands("Redo").Enabled = Util.ModificationHistory.CanRedo

        End Sub

        Public Sub OnEditPopupEnd(ByVal mc As MenuCommand)
            mc.MenuCommands("Undo").Enabled = True
            mc.MenuCommands("Redo").Enabled = True
            'mc.MenuCommands("Cut").Enabled = True
            'mc.MenuCommands("Copy").Enabled = True
            'mc.MenuCommands("Delete").Enabled = True
            'mc.MenuCommands("Paste").Enabled = True
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

        Protected Sub OnCut(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If Not m_tabFiller.SelectedTab Is Nothing Then
                    'Dim dgDiagram As Behavior.Diagram = DirectCast(m_tabFiller.SelectedTab.Control, Behavior.Diagram)
                    'dgDiagram.CutSelected()
                End If

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnCopy(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If Not m_tabFiller.SelectedTab Is Nothing Then
                    'Dim dgDiagram As Behavior.Diagram = DirectCast(m_tabFiller.SelectedTab.Control, Behavior.Diagram)
                    'dgDiagram.CopySelected()
                End If

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnPaste(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If Not m_tabFiller.SelectedTab Is Nothing Then
                    'Dim dgDiagram As Behavior.Diagram = DirectCast(m_tabFiller.SelectedTab.Control, Behavior.Diagram)
                    'dgDiagram.PasteSelected()
                End If

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnDelete(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If Not m_tabFiller.SelectedTab Is Nothing Then
                    'Dim dgDiagram As Behavior.Diagram = DirectCast(m_tabFiller.SelectedTab.Control, Behavior.Diagram)
                    'dgDiagram.DeleteSelected()
                End If

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnUndo(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If Not m_tabFiller.SelectedTab Is Nothing Then
                    'Dim dgDiagram As Behavior.Diagram = DirectCast(m_tabFiller.SelectedTab.Control, Behavior.Diagram)
                    'dgDiagram.OnUndo()
                End If

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnRedo(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If Not m_tabFiller.SelectedTab Is Nothing Then
                    'Dim dgDiagram As Behavior.Diagram = DirectCast(m_tabFiller.SelectedTab.Control, Behavior.Diagram)
                    'dgDiagram.OnRedo()
                End If

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#Region " Toolbar View Events "

        Protected Sub OnViewHierarchyBar(ByVal sender As Object, ByVal e As System.EventArgs)
            ViewToolbar(m_HierarchyContent)
        End Sub

#End Region

#Region " Drag and Drop Events "

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
            Try
                e.Cancel = False
                Me.IsDirty = True
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overrides Sub MdiChild_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
            Try
                MyBase.MdiChild_Closing(sender, e)

                Dim frmTool As AnimatTools.Forms.Tools.ToolForm
                If Not m_frmControl Is Nothing Then
                    frmTool = DirectCast(m_frmControl, AnimatTools.Forms.Tools.ToolForm)
                    frmTool.Viewer = Nothing
                End If

                For Each oContent As Crownwood.Magic.Docking.Content In m_dockManager.Contents
                    If Not oContent.UserData Is Nothing AndAlso TypeOf oContent.UserData Is Forms.Tools.ToolForm AndAlso Not oContent.UserData Is m_frmControl Then
                        frmTool = DirectCast(oContent.UserData, Forms.Tools.ToolForm)
                        frmTool.Viewer = Nothing
                    End If
                Next

                Util.ModificationHistory.RemoveMdiEvents(Me)

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region


    End Class

End Namespace

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

Namespace Forms

    Public Class MdiChild
        Inherits Crownwood.DotNetMagic.Forms.DotNetMagicForm

#Region " Windows Form Designer generated code "

        Public Sub New()
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()
        End Sub

        'Form overrides dispose to clean up the component list.
        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
                If Not (components Is Nothing) Then
                    components.Dispose()
                End If

                If Not m_dockManager Is Nothing Then m_dockManager.Dispose()
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
            Me.Title = "MdiChild"
        End Sub

#End Region

#Region " Attributes "

        Protected m_menuMain As AnimatGuiCtrls.Controls.AnimatMenuStrip
        Protected m_barMain As AnimatGuiCtrls.Controls.AnimatToolStrip
        Protected m_dockManager As DockingManager
        Protected m_mgrToolStripImages As AnimatGUI.Framework.ImageManager
        Protected m_tabFiller As Crownwood.Magic.Controls.TabControl
        Protected m_winContent As WindowContent
        Protected m_frmControl As AnimatForm

        Protected m_iGraph As Integer

        Protected m_strTitle As String = "Mdi Child Window"

        'This is the root dataobject for any data in this form. It helps the
        'form keep track of whether data items are dirty.
        Protected m_doFormHelper As New DataObjects.FormHelper(Me)

        Protected m_ptInitialLocation As New Point
        Protected m_ptNormalLocation As New Point
        Protected m_szNormalSize As New Size

#End Region

#Region " Properties "

        Public Overridable ReadOnly Property ToolStripImages() As AnimatGUI.Framework.ImageManager
            Get
                Return m_mgrToolStripImages
            End Get
        End Property

        Public ReadOnly Property Control() As AnimatForm
            Get
                Return m_frmControl
            End Get
        End Property

        Public ReadOnly Property DockManager() As DockingManager
            Get
                Return m_dockManager
            End Get
        End Property

        Public ReadOnly Property MainMenu() As AnimatGuiCtrls.Controls.AnimatMenuStrip
            Get
                Return m_menuMain
            End Get
        End Property

        Public ReadOnly Property MainToolBar() As AnimatGuiCtrls.Controls.AnimatToolStrip
            Get
                Return m_barMain
            End Get
        End Property

        Public Overridable ReadOnly Property AssemblyFile() As String
            Get
                Dim strPath As String, strFile As String
                Util.SplitPathAndFile(Me.GetType.Assembly.Location, strPath, strFile)
                Return strFile
            End Get
        End Property

        Public Overridable ReadOnly Property ClassName() As String
            Get
                Return Me.GetType.ToString
            End Get
        End Property

        Public ReadOnly Property FormHelper() As DataObjects.FormHelper
            Get
                Return m_doFormHelper
            End Get
        End Property

        Public Property IsDirty() As Boolean
            Get
                Return m_doFormHelper.IsDirty
            End Get
            Set(ByVal Value As Boolean)
                If Not Util.DisableDirtyFlags AndAlso m_doFormHelper.IsDirty <> Value Then
                    m_doFormHelper.IsDirty = Value
                End If
            End Set
        End Property

        Public Overridable Property Title() As String
            Get
                Return m_strTitle
            End Get
            Set(ByVal Value As String)
                m_strTitle = Value
                Me.Text = m_strTitle
            End Set
        End Property

        Public Overrides Property Text() As String
            Get
                Return MyBase.Text
            End Get
            Set(ByVal Value As String)
                If Not m_doFormHelper Is Nothing AndAlso m_doFormHelper.IsDirty Then
                    MyBase.Text = m_strTitle & " *"
                Else
                    MyBase.Text = m_strTitle
                End If
            End Set
        End Property

        Public Overridable ReadOnly Property TabFiller() As Crownwood.Magic.Controls.TabControl
            Get
                Return m_tabFiller
            End Get
        End Property

        Public Overridable ReadOnly Property NormalSize() As Size
            Get
                Return m_szNormalSize
            End Get
        End Property

        Public Overridable ReadOnly Property NormalLocation() As Point
            Get
                Return m_ptNormalLocation
            End Get
        End Property

        Public Overridable ReadOnly Property InitialLocation() As Point
            Get
                Return m_ptInitialLocation
            End Get
        End Property

        Public Overridable ReadOnly Property IsVisibleInClient() As Boolean
            Get

                Dim r As Rectangle = Util.Application.MdiClient.ClientRectangle()

                If Me.Top >= 0 AndAlso Me.Left >= 0 AndAlso Me.Top < r.Height AndAlso Me.Left < r.Width Then
                    Return True
                Else
                    Return False
                End If

            End Get
        End Property

#End Region

#Region " Methods "

        Public Overridable Sub Initialize(ByVal frmApplication As AnimatApplication, _
                                          ByVal frmControl As AnimatForm)

            If Util.Application Is Nothing Then
                Throw New System.Exception("The animat application is not defined.")
            End If

            Util.Application = Util.Application
            Me.MdiParent = Util.Application

            If Not Util.Application Is Nothing Then
                m_doFormHelper.Parent = Util.Application.FormHelper
            Else
                m_doFormHelper.Parent = Nothing
            End If

            If frmControl Is Nothing Then
                Throw New System.Exception("The control for the mdi child window is not defined.")
            End If

            m_frmControl = frmControl

            'If Not m_frmControl Is Nothing Then
            '    If m_frmControl.UseDocking Then
            '        m_tabFiller = New Crownwood.Magic.Controls.TabControl
            '        m_tabFiller.TabPages.Add(New Crownwood.Magic.Controls.TabPage(m_frmControl.TabPageName, m_frmControl))
            '        m_tabFiller.Appearance = m_frmControl.DockingTabAppearance
            '        m_tabFiller.HideTabsMode = m_frmControl.DockingHideTabsMode
            '        m_tabFiller.Dock = DockStyle.Fill
            '        m_tabFiller.Style = VisualStyle.IDE
            '        m_tabFiller.IDEPixelBorder = True
            '        Controls.Add(m_tabFiller)
            '        m_dockManager = New DockingManager(Me, VisualStyle.IDE)

            '        m_dockManager.InnerControl = m_tabFiller
            '    End If

            '    m_frmControl.Initialize(Util.Application, Me, Nothing)
            '    Me.Icon = m_frmControl.Icon
            'End If

            CreateToolStrips()

            AddHandler Me.Enter, AddressOf Me.Mdi_OnEnter
            AddHandler Me.Click, AddressOf Me.Mdi_OnClick

        End Sub

        Public Overridable Sub ShowAnimatForm()
            Me.AddToSimulation()
            If Not m_frmControl Is Nothing Then m_frmControl.OnMdiParentShowing()
            Me.Show()
        End Sub

        Protected Overridable Sub CreateToolStrips()
            If Util.Application Is Nothing Then Throw New System.Exception("Application object is not defined.")
            If m_frmControl Is Nothing Then Throw New System.Exception("Mdi Control object is not defined.")

            'm_frmControl.CreateToolStrips()
        End Sub

        Public Overridable Sub RefreshTitle()
            Me.Title = Me.Title
        End Sub

        Public Overridable Sub ClearIsDirty()
            Me.IsDirty = False
        End Sub

        Public Overridable Sub MakeVisible()

            Me.Visible = True

            If Not Me.IsVisibleInClient Then
                Me.Location = New Point(0, 0)
            End If

            If Me.WindowState = FormWindowState.Minimized Then
                Me.WindowState = FormWindowState.Normal
            End If

            Me.BringToFront()
        End Sub

        Public Overridable Sub UndoRedoRefresh(ByVal oRefresh As Object)
        End Sub

        Public Overridable Sub RefreshProperties()
        End Sub

        Protected Overridable Sub SynchronizeToolbarMenu(ByVal m_aryToolbarMenus As SortedList)
            Dim mcCommand As MenuCommand

            For Each ctDock As Content In m_dockManager.Contents
                If TypeOf (ctDock.Control) Is AnimatGUI.Forms.AnimatForm Then
                    If m_aryToolbarMenus.Contains(ctDock.Title) Then
                        mcCommand = DirectCast(m_aryToolbarMenus(ctDock.Title), MenuCommand)
                    End If

                    If ctDock.Visible Then
                        If Not mcCommand Is Nothing Then mcCommand.Checked = True
                    Else
                        If Not mcCommand Is Nothing Then mcCommand.Checked = False
                    End If
                End If
            Next

        End Sub

        Public Overridable Sub AddToSimulation()

            'Dim frmAnimat As AnimatForm = DirectCast(Me.Control, AnimatForm)
            'frmAnimat.AddToSimulation()

            'If Not m_frmControl Is Nothing AndAlso m_frmControl.UseDocking Then
            '    'First lets save all Docking Forms associated with this application.
            '    For Each conWindow As Content In m_dockManager.Contents
            '        If TypeOf (conWindow.Control) Is AnimatGUI.Forms.AnimatForm Then
            '            frmAnimat = DirectCast(conWindow.Control, AnimatForm)
            '            frmAnimat.AddToSimulation()
            '        End If
            '    Next
            'End If

        End Sub

        Public Overridable Sub RemoveFromSimulation()

            'Dim frmAnimat As AnimatForm = DirectCast(Me.Control, AnimatForm)
            'frmAnimat.RemoveFromSimulation()

            'If Not m_frmControl Is Nothing AndAlso m_frmControl.UseDocking Then
            '    'First lets save all Docking Forms associated with this application.
            '    For Each conWindow As Content In m_dockManager.Contents
            '        If TypeOf (conWindow.Control) Is AnimatGUI.Forms.AnimatForm Then
            '            frmAnimat = DirectCast(conWindow.Control, AnimatForm)
            '            frmAnimat.RemoveFromSimulation()
            '        End If
            '    Next
            'End If

        End Sub

        Protected Overridable Sub LoadWindowSize(ByRef oXml As AnimatGUI.Interfaces.StdXml)

            If oXml.FindChildElement("NormalLocation", False) Then
                m_ptNormalLocation = Util.LoadPoint(oXml, "NormalLocation")
                m_ptInitialLocation = m_ptNormalLocation
                Me.Location = m_ptNormalLocation
            Else
                m_ptNormalLocation.X = oXml.GetChildInt("Left")
                m_ptNormalLocation.Y = oXml.GetChildInt("Top")
            End If

            If oXml.FindChildElement("NormalSize", False) Then
                m_szNormalSize = Util.LoadSize(oXml, "NormalSize")
                Me.WindowState = DirectCast([Enum].Parse(GetType(FormWindowState), oXml.GetChildString("WindowState")), FormWindowState)
                Me.Size = m_szNormalSize
            Else
                Me.Width = oXml.GetChildInt("Width")
                Me.Height = oXml.GetChildInt("Height")
            End If
        End Sub

        Public Overridable Sub LoadData(ByRef oXml As AnimatGUI.Interfaces.StdXml)

            oXml.IntoElem()  'Into the Child element

            LoadWindowSize(oXml)

            m_frmControl.LoadData(oXml)

            'If m_frmControl.UseDocking Then
            '    oXml.IntoChildElement("DockingForms") 'Into DockingForms Element
            '    Dim iCount As Integer = oXml.NumberOfChildren() - 1
            '    For iIndex As Integer = 0 To iCount
            '        oXml.FindChildByIndex(iIndex)
            '        Util.Application.LoadDockingForm(m_dockManager, oXml)
            '    Next
            '    oXml.OutOfElem()   'Outof DockingForms Element

            '    Util.Application.LoadDockingConfig(m_dockManager, oXml)
            'End If

            oXml.OutOfElem()  'OutOf the Child element

        End Sub

        Protected Overridable Sub SaveWindowSize(ByRef oXml As AnimatGUI.Interfaces.StdXml)

            Util.SavePoint(oXml, "NormalLocation", m_ptNormalLocation)
            Util.SaveSize(oXml, "NormalSize", m_szNormalSize)
            oXml.AddChildElement("WindowState", Me.WindowState.ToString())

        End Sub

        Public Overridable Sub SaveData(ByRef oXml As AnimatGUI.Interfaces.StdXml)

            oXml.AddChildElement("Child")
            oXml.IntoElem() 'Into Child Elemement

            oXml.AddChildElement("AssemblyFile", Me.AssemblyFile)
            oXml.AddChildElement("ClassName", Me.ClassName)

            SaveWindowSize(oXml)

            Dim frmAnimat As AnimatForm = DirectCast(Me.Control, AnimatForm)
            frmAnimat.SaveData(oXml)

            'If m_frmControl.UseDocking Then
            '    oXml.AddChildElement("DockingForms")
            '    oXml.IntoElem()   'Into DockingForms Element

            '    'First lets save all Docking Forms associated with this application.
            '    For Each conWindow As Content In m_dockManager.Contents
            '        If TypeOf (conWindow.Control) Is AnimatGUI.Forms.AnimatForm Then
            '            frmAnimat = DirectCast(conWindow.Control, AnimatForm)
            '            frmAnimat.SaveData(oXml)
            '        End If
            '    Next
            '    oXml.OutOfElem()   'Outof DockingForms Element

            '    Util.Application.SaveDockingConfig(m_dockManager, oXml)
            'End If

            oXml.OutOfElem()  'Outof Child Element

        End Sub


        Public Overridable Sub SaveSimulationXml(ByRef oXml As Interfaces.StdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")

        End Sub

        Public Overridable Function GetSimulationXml(ByVal strName As String, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing) As String

            Dim oXml As New AnimatGUI.Interfaces.StdXml
            oXml.AddElement("Root")
            SaveSimulationXml(oXml, nmParentControl, strName)

            Return oXml.Serialize()
        End Function

#End Region

#Region " Events "

        Protected Sub Mdi_OnEnter(ByVal sender As Object, ByVal e As EventArgs)
            Try
                'Util.Application.ResetToolbars(m_menuMain, m_barMain)
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Sub Mdi_OnClick(ByVal sender As Object, ByVal e As EventArgs)
            Try
                'If Not Util.Application.CurrentMenu Is m_menuMain Then
                '    Util.Application.ResetToolbars(m_menuMain, m_barMain)
                'End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub MdiChild_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize
            Try

                If Me.WindowState = FormWindowState.Normal Then
                    m_szNormalSize = Me.Size
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub MdiChild_Move(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Move
            Try

                If Me.WindowState = FormWindowState.Normal Then
                    m_ptNormalLocation = Me.Location
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub MdiChild_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
            Try
                'First check to see if the application is dirty. If it is then ask to save the project
                If Me.IsDirty Then
                    Dim eResult As System.Windows.Forms.DialogResult = MessageBox.Show("There are unsaved changes for this window. " & _
                                                                                        "Do you want to save them before you close it?", _
                                                                                        "Save Changes", MessageBoxButtons.YesNoCancel)
                    If eResult = DialogResult.Cancel Then
                        e.Cancel = True
                        Return
                    ElseIf eResult = DialogResult.Yes Then
                        Util.Application.SaveProject(Util.Application.ProjectFile)
                    End If
                End If

                'm_frmControl.OnContentClosing(e)

                If Not e.Cancel Then
                    Me.RemoveFromSimulation()

                    If Not m_dockManager Is Nothing Then m_dockManager.Dispose()

                    If Not Util.Application Is Nothing Then

                        If Util.Application.ChildForms.Count <= 1 Then
                            ';Util.Application.ResetToolbars()
                        End If
                    End If
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace


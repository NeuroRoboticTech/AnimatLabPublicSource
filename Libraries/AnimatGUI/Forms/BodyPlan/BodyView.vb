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
Imports System.Drawing.Imaging

Namespace Forms.BodyPlan

    Public Class BodyView
        Inherits AnimatForm

#Region " Windows Form Designer generated code "

        Public Sub New()
            MyBase.New()

            'Add a new hud item to display the time and the axis by default
            m_aryHudItems.Clear()
            m_aryHudItems.Add(New DataObjects.Visualization.HudItem(Nothing))

            'This call is required by the Windows Form Designer.
            InitializeComponent()
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
            'BodyView
            '
            Me.ClientSize = New System.Drawing.Size(284, 262)
            Me.Name = "BodyView"
            Me.Text = "Body View"
            Me.Title = "Body View"
            Me.ResumeLayout(False)

        End Sub

#End Region

#Region " Attributes "

        Protected m_beEditor As AnimatTools.Forms.BodyPlan.Editor
        Protected m_clBackColor As Color = Color.Black
        Protected m_aryHudItems As New ArrayList

        Protected m_bTrackCamera As Boolean = True

#End Region

#Region " Properties "

        'Public Overrides ReadOnly Property DockingHideTabsMode() As Crownwood.Magic.Controls.TabControl.HideTabsModes
        '    Get
        '        Return Crownwood.Magic.Controls.TabControl.HideTabsModes.HideAlways
        '    End Get
        'End Property

        Public Overridable Property Editor() As Forms.BodyPlan.Editor
            Get
                Return m_beEditor
            End Get
            Set(ByVal Value As Forms.BodyPlan.Editor)
                m_beEditor = Value
            End Set
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

                'm_beEditor = DirectCast(frmMdiParent, AnimatTools.Forms.BodyPlan.Editor)

                AddHandler Util.Application.UnitsChanged, AddressOf Me.Application_UnitsChanged

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Sub OnAfterMdiParentInitialized()
        End Sub

        'Protected Overridable Sub OnCommandModeChanged(ByVal eNewMode As Forms.BodyPlan.Command.enumCommandMode)

        'End Sub

        Protected Overridable Sub CreatePopupMenu(ByVal ptScreen As Point)

            ' Create the popup menu object
            Dim popup As New PopupMenu

            ' Create the menu items
            Dim mcSelectBodies As New MenuCommand("Select Bodies", "SelectBodies", System.Windows.Forms.Shortcut.CtrlB, New EventHandler(AddressOf Me.OnSelectBodies))
            Dim mcSelectJoints As New MenuCommand("Select Joints", "SelectJoints", System.Windows.Forms.Shortcut.CtrlJ, New EventHandler(AddressOf Me.OnSelectJoints))
            Dim mcReceptiveFields As New MenuCommand("Select Receptive Fields", "SelectReceptiveFields", System.Windows.Forms.Shortcut.CtrlR, New EventHandler(AddressOf Me.OnSelectReceptiveFields))
            Dim mcAddBody As New MenuCommand("Add Body", "AddBody", System.Windows.Forms.Shortcut.CtrlA, New EventHandler(AddressOf Me.OnAddBody))
            Dim mcSepEditStart1 As MenuCommand = New MenuCommand("-")

            'If Me.Editor.CommandBar.CommandMode = Command.enumCommandMode.SelectBodies Then
            '    mcSelectBodies.Checked = True
            'ElseIf Me.Editor.CommandBar.CommandMode = Command.enumCommandMode.SelectJoints Then
            '    mcSelectJoints.Checked = True
            'ElseIf Me.Editor.CommandBar.CommandMode = Command.enumCommandMode.SelectReceptiveFields Then
            '    mcReceptiveFields.Checked = True
            'Else
            '    mcAddBody.Checked = True
            'End If

            If TypeOf Me.Editor.PhysicalStructure Is DataObjects.Physical.Organism Then
                popup.MenuCommands.AddRange(New MenuCommand() {mcSelectBodies, mcSelectJoints, mcAddBody, mcReceptiveFields, mcSepEditStart1})
            Else
                popup.MenuCommands.AddRange(New MenuCommand() {mcSelectBodies, mcSelectJoints, mcAddBody, mcSepEditStart1})
            End If

            Dim mcCut As New MenuCommand("Cut", "Cut", m_beEditor.ToolStripImages.ImageList, _
                                         m_beEditor.ToolStripImages.GetImageIndex("AnimatTools.Cut.gif"), _
                                         Shortcut.CtrlX, New EventHandler(AddressOf Me.OnCut))
            Dim mcCopy As New MenuCommand("Copy", "Copy", m_beEditor.ToolStripImages.ImageList, _
                                            m_beEditor.ToolStripImages.GetImageIndex("AnimatTools.Copy.gif"), _
                                            Shortcut.CtrlC, New EventHandler(AddressOf Me.OnCopy))
            Dim mcPaste As New MenuCommand("Paste", "Paste", m_beEditor.ToolStripImages.ImageList, _
                                            m_beEditor.ToolStripImages.GetImageIndex("AnimatTools.Paste.gif"), _
                                            Shortcut.CtrlV, New EventHandler(AddressOf Me.OnPaste))
            Dim mcDelete As New MenuCommand("Delete", "Delete", m_beEditor.ToolStripImages.ImageList, _
                                            m_beEditor.ToolStripImages.GetImageIndex("AnimatTools.Delete.gif"), _
                                            Shortcut.Del, New EventHandler(AddressOf Me.OnDelete))

            If Me.Editor.PropertiesBar.SelectedParts.Count = 0 Then
                mcCut.Enabled = False
                mcCopy.Enabled = False
                mcDelete.Enabled = False
            End If

            mcPaste.Enabled = False
            Dim data As IDataObject = Clipboard.GetDataObject()
            If Not data Is Nothing AndAlso data.GetDataPresent("AnimatLab.Body.XMLFormat") Then
                Dim strXml As String = DirectCast(data.GetData("AnimatLab.Body.XMLFormat"), String)
                If strXml.Trim.Length > 0 Then
                    mcPaste.Enabled = True
                End If
            End If

            popup.MenuCommands.AddRange(New MenuCommand() {mcCut, mcCopy, mcPaste, mcDelete})

            'If a node is selected then show the Add Stimulus entry
            If Me.Editor.PropertiesBar.SelectedParts.Count = 1 AndAlso Not Me.Editor.PropertiesBar.SelectedPart Is Nothing Then
                If Me.Editor.PropertiesBar.SelectedPart.AllowStimulus Then
                    Dim mcSep3 As MenuCommand = New MenuCommand("-")
                    Dim mcAddStimulus As New MenuCommand("Add Stimulus", "AddStimulus", m_beEditor.ToolStripImages.ImageList, _
                                                      m_beEditor.ToolStripImages.GetImageIndex("AnimatTools.ExternalStimulus.gif"), _
                                                      New EventHandler(AddressOf Me.OnAddStimulus))

                    Dim mcSwapPart As New MenuCommand("Swap Part", "SwapPart", Util.Application.ToolStripImages.ImageList, _
                                                 Util.Application.ToolStripImages.GetImageIndex("AnimatTools.Swap.gif"), _
                                                 New EventHandler(AddressOf Me.OnSwapBodyPart))

                    popup.MenuCommands.AddRange(New MenuCommand() {mcSep3, mcAddStimulus, mcSwapPart})
                End If
            End If

            Dim mcSepSelectStart As MenuCommand = New MenuCommand("-")
            Dim mcSelectByType As New MenuCommand("Select By Type", "SelectByType", m_beEditor.ToolStripImages.ImageList, _
                                            m_beEditor.ToolStripImages.GetImageIndex("AnimatTools.SelectByType.gif"), _
                                            New EventHandler(AddressOf Me.OnSelectByType))
            Dim mcRelabel As New MenuCommand("Relabel", "Relabel", m_beEditor.ToolStripImages.ImageList, _
                                              m_beEditor.ToolStripImages.GetImageIndex("AnimatTools.Relabel.gif"), _
                                              New EventHandler(AddressOf Me.OnRelabel))
            Dim mcRelabelSelected As New MenuCommand("Relabel Selected", "RelabelSelected", m_beEditor.ToolStripImages.ImageList, _
                                              m_beEditor.ToolStripImages.GetImageIndex("AnimatTools.RelabelSelected.gif"), _
                                              New EventHandler(AddressOf Me.OnRelabelSelected))

            If Me.Editor.PropertiesBar.SelectedParts.Count = 0 Then
                mcRelabelSelected.Enabled = False
            End If

            popup.MenuCommands.AddRange(New MenuCommand() {mcSepSelectStart, mcSelectByType, mcRelabel, mcRelabelSelected})

            ' Show it!
            Dim selected As MenuCommand = popup.TrackPopup(ptScreen)

            Me.Invalidate()
        End Sub

        Public Overridable Sub SwapBodyPart(ByVal doExistingPart As AnimatTools.DataObjects.Physical.BodyPart)

        End Sub

        Public Overridable Function GenerateSimWindowXml() As String
            Dim oXml As New AnimatTools.Interfaces.StdXml()

            oXml.AddElement("WindowMgr")
            oXml.AddChildElement("Window")

            oXml.IntoElem()
            oXml.AddChildElement("Name", Me.Text)
            oXml.AddChildElement("ID", Me.ID)
            oXml.AddChildElement("Type", "Basic")
            oXml.AddChildElement("StandAlone", True)

            If Not m_beEditor Is Nothing AndAlso Not m_beEditor.PhysicalStructure Is Nothing Then
                oXml.AddChildElement("TrackCamera", m_bTrackCamera)
                oXml.AddChildElement("LookAtStructureID", Me.m_beEditor.PhysicalStructure.ID)
                oXml.AddChildElement("LookAtBodyID", "")
            Else
                oXml.AddChildElement("TrackCamera", False)
                oXml.AddChildElement("LookAtStructureID", "")
                oXml.AddChildElement("LookAtBodyID", "")
            End If

            oXml.AddChildElement("HudItems")
            oXml.IntoElem()

            For Each hudItem As DataObjects.Visualization.HudItem In m_aryHudItems
                hudItem.SaveData(oXml)
            Next

            oXml.OutOfElem()

            oXml.OutOfElem()

            Return oXml.Serialize()
        End Function

#End Region

#Region " Events "

        Protected Sub OnCut(ByVal sender As Object, ByVal e As System.EventArgs)
            Me.Editor.OnCut(sender, e)
        End Sub

        Protected Sub OnCopy(ByVal sender As Object, ByVal e As System.EventArgs)
            Me.Editor.OnCopy(sender, e)
        End Sub

        Protected Sub OnPaste(ByVal sender As Object, ByVal e As System.EventArgs)
            Me.Editor.OnPaste(sender, e)
        End Sub

        Protected Sub OnDelete(ByVal sender As Object, ByVal e As System.EventArgs)
            Me.Editor.OnDelete(sender, e)
        End Sub

        Protected Sub OnSelectBodies(ByVal sender As Object, ByVal e As System.EventArgs)
            Me.Editor.OnSelectBodies(sender, e)
        End Sub

        Protected Sub OnSelectJoints(ByVal sender As Object, ByVal e As System.EventArgs)
            Me.Editor.OnSelectJoints(sender, e)
        End Sub

        Protected Sub OnAddBody(ByVal sender As Object, ByVal e As System.EventArgs)
            Me.Editor.OnAddBody(sender, e)
        End Sub

        Protected Sub OnSwapBodyPart(ByVal sender As Object, ByVal e As System.EventArgs)
            If Not Me.Editor.PropertiesBar.SelectedPart Is Nothing AndAlso Me.Editor.PropertiesBar.SelectedParts.Count = 1 Then
                Me.SwapBodyPart(Me.Editor.PropertiesBar.SelectedPart)
            End If
        End Sub

        Protected Sub OnSelectReceptiveFields(ByVal sender As Object, ByVal e As System.EventArgs)
            Me.Editor.OnSelectReceptiveFields(sender, e)
        End Sub

        Protected Sub OnAddStimulus(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If Me.Editor.PropertiesBar.SelectedParts.Count = 1 AndAlso Not Me.Editor.PropertiesBar.SelectedPart Is Nothing Then
                    Me.Editor.PropertiesBar.SelectedPart.SelectStimulusType()
                End If
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Sub OnSelectByType(ByVal sender As Object, ByVal e As System.EventArgs)
            Me.Editor.OnSelectByType(sender, e)
        End Sub

        Protected Sub OnRelabel(ByVal sender As Object, ByVal e As System.EventArgs)
            Me.Editor.OnRelabel(sender, e)
        End Sub

        Protected Sub OnRelabelSelected(ByVal sender As Object, ByVal e As System.EventArgs)
            Me.Editor.OnRelabelSelected(sender, e)
        End Sub

        Protected Sub Application_UnitsChanged(ByVal ePrevMass As AnimatTools.DataObjects.Physical.Environment.enumMassUnits, _
                                  ByVal eNewMass As AnimatTools.DataObjects.Physical.Environment.enumMassUnits, _
                                  ByVal fltMassChange As Single, _
                                  ByVal ePrevDistance As AnimatTools.DataObjects.Physical.Environment.enumDistanceUnits, _
                                  ByVal eNewDistance As AnimatTools.DataObjects.Physical.Environment.enumDistanceUnits, _
                                  ByVal fltDistanceChange As Single)
            Try
                If Not Me.Editor Is Nothing AndAlso Not Me.Editor.PropertiesBar Is Nothing Then
                    Me.Editor.PropertiesBar.RefreshProperties()
                    Me.Invalidate()
                End If

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Public Overrides Sub OnMdiParentShowing()
            Try
                Dim strWinXml As String = GenerateSimWindowXml()
                Util.Application.SimulationInterface.AddWindow(Me.Handle, strWinXml)
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub OnBodyView_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'Try
            '    Dim strWinXml As String = GenerateSimWindowXml()
            '    Util.Application.SimulationInterface.AddWindow(Me.pnlView.Handle, strWinXml)
            'Catch ex As System.Exception
            '    AnimatTools.Framework.Util.DisplayError(ex)
            'End Try

        End Sub

        Private Sub OnBodyView_Closing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
            Try
                RemoveHandler Util.Application.UnitsChanged, AddressOf Me.Application_UnitsChanged
                Util.Application.SimulationInterface.RemoveWindow(Me.Handle)
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace

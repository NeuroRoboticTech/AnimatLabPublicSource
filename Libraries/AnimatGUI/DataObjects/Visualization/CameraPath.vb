Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports AnimatGuiCtrls.Controls
Imports AnimatGUI.Framework

Namespace DataObjects.Visualization


    Public Class CameraPath
        Inherits AnimatGUI.Framework.DataObject

#Region " Attributes "

        Protected m_aryWaypoints As New Collections.SortedWaypointsList(Me)
        Protected m_aryWaypointsByName As New Collections.OrderedWaypointsList(Me)

        Protected m_thLinkedStructure As AnimatGUI.TypeHelpers.LinkedStructureList
        Protected m_thLinkedPart As AnimatGUI.TypeHelpers.LinkedBodyPartList

        Protected m_clLineColor As System.Drawing.Color = System.Drawing.Color.Red
        Protected m_bVisible As Boolean = True
        Protected m_bVisibleInSim As Boolean = False
        Protected m_bShowWaypoints As Boolean = False

        'Only used during loading
        Protected m_strLinkedStructureID As String = ""
        Protected m_strLinkedBodyPartID As String = ""

        Protected m_SimWindow As Forms.Tools.ScriptedSimulationWindow

        Protected m_snStartTime As Framework.ScaledNumber
        Protected m_snEndTime As Framework.ScaledNumber

#End Region

#Region " Properties "

        Public Overridable ReadOnly Property Type() As String
            Get
                Return "CameraPath"
            End Get
        End Property

        Public Overrides ReadOnly Property WorkspaceImageName As String
            Get
                Return "AnimatGUI.CameraPaths.gif"
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property LineColor() As System.Drawing.Color
            Get
                Return m_clLineColor
            End Get
            Set(ByVal Value As System.Drawing.Color)
                SetSimData("LineColor", Util.SaveColorXml("LineColor", Value), True)
                m_clLineColor = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property Waypoints() As Collections.SortedWaypointsList
            Get
                Return m_aryWaypoints
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property WaypointsByName() As Collections.OrderedWaypointsList
            Get
                Return m_aryWaypointsByName
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property LinkedStructure() As AnimatGUI.TypeHelpers.LinkedStructureList
            Get
                Return m_thLinkedStructure
            End Get
            Set(ByVal Value As AnimatGUI.TypeHelpers.LinkedStructureList)
                Dim thPrevLinked As AnimatGUI.TypeHelpers.LinkedStructureList = m_thLinkedStructure

                DiconnectLinkedEvents()
                m_thLinkedStructure = Value

                If Not m_thLinkedStructure.PhysicalStructure Is thPrevLinked.PhysicalStructure Then
                    m_thLinkedPart = New TypeHelpers.LinkedBodyPartList(m_thLinkedStructure.PhysicalStructure, Nothing, GetType(AnimatGUI.DataObjects.Physical.BodyPart))
                End If

                ConnectLinkedEvents()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property LinkedPart() As AnimatGUI.TypeHelpers.LinkedBodyPartList
            Get
                Return m_thLinkedPart
            End Get
            Set(ByVal Value As AnimatGUI.TypeHelpers.LinkedBodyPartList)
                Dim thPrevLinked As AnimatGUI.TypeHelpers.LinkedBodyPartList = m_thLinkedPart

                If Not Value Is Nothing AndAlso Not Value.BodyPart Is Nothing Then
                    SetSimData("TrackPartID", Value.BodyPart.ID, True)
                Else
                    SetSimData("TrackPartID", "", True)
                End If

                DiconnectLinkedEvents()
                m_thLinkedPart = Value
                ConnectLinkedEvents()
            End Set
        End Property

        Public Overridable Property StartTime() As Framework.ScaledNumber
            Get
                Return m_snStartTime
            End Get
            Set(ByVal value As Framework.ScaledNumber)
                If value.ActualValue < 0 Then
                    Throw New System.Exception("You cannot specify a start time less than zero.")
                End If

                'Check to see if there is already a path with the given start time
                If Not m_SimWindow Is Nothing AndAlso m_SimWindow.HasPathWithStartTime(value.ActualValue) Then
                    Throw New System.Exception("A path with a start time of '" & value.ActualValue & "' already exists.")
                End If

                Me.SetSimData("StartTime", value.ToString, True)
                m_snStartTime.CopyData(value)
                RecalculateTimes()
            End Set
        End Property

        Public Overridable Property EndTime() As Framework.ScaledNumber
            Get
                Return m_snEndTime
            End Get
            Set(ByVal value As Framework.ScaledNumber)
                Me.SetSimData("EndTime", value.ToString, True)
                m_snEndTime.CopyData(value)
                m_snEndTime.PropertiesReadOnly = True
            End Set
        End Property

        Public Overridable Property Visible() As Boolean
            Get
                Return m_bVisible
            End Get
            Set(ByVal value As Boolean)
                Me.SetSimData("Visible", value.ToString, True)
                m_bVisible = value
            End Set
        End Property

        Public Overridable Property VisibleInSim() As Boolean
            Get
                Return m_bVisibleInSim
            End Get
            Set(ByVal value As Boolean)
                Me.SetSimData("VisibleInSim", value.ToString, True)
                m_bVisibleInSim = value
            End Set
        End Property

        Public Overridable Property ShowWaypoints() As Boolean
            Get
                Return m_bShowWaypoints
            End Get
            Set(ByVal value As Boolean)
                Me.SetSimData("ShowWaypoints", value.ToString, True)
                m_bShowWaypoints = value
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            If TypeOf (doParent) Is DataObjects.FormHelper Then
                Dim doHelper As DataObjects.FormHelper = DirectCast(doParent, DataObjects.FormHelper)

                If Not doHelper.AnimatParent Is Nothing AndAlso Util.IsTypeOf(doHelper.AnimatParent.GetType(), GetType(Forms.Tools.ScriptedSimulationWindow), False) Then
                    m_SimWindow = DirectCast(doHelper.AnimatParent, Forms.Tools.ScriptedSimulationWindow)
                End If
            End If

            m_thLinkedStructure = New AnimatGUI.TypeHelpers.LinkedStructureList(Nothing, TypeHelpers.LinkedStructureList.enumStructureType.All)
            m_thLinkedPart = New AnimatGUI.TypeHelpers.LinkedBodyPartList(Nothing, Nothing, GetType(AnimatGUI.DataObjects.Physical.BodyPart))

            m_snStartTime = New ScaledNumber(Me, "StartTime", 0, ScaledNumber.enumNumericScale.None, "s", "s")
            m_snEndTime = New ScaledNumber(Me, "EndTime", 1, ScaledNumber.enumNumericScale.None, "s", "s")
            m_snEndTime.PropertiesReadOnly = True

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()
            If Not m_aryWaypoints Is Nothing Then m_aryWaypoints.ClearIsDirty()
            If Not m_aryWaypointsByName Is Nothing Then m_aryWaypointsByName.ClearIsDirty()
            If Not m_thLinkedStructure Is Nothing Then m_thLinkedStructure.ClearIsDirty()
            If Not m_thLinkedPart Is Nothing Then m_thLinkedPart.ClearIsDirty()
            If Not m_snStartTime Is Nothing Then m_snStartTime.ClearIsDirty()
            If Not m_snEndTime Is Nothing Then m_snEndTime.ClearIsDirty()
        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Name", Me.Name.GetType(), "Name", _
                                        "Path Properties", "Name", Me.Name))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("ID", Me.ID.GetType(), "ID", _
                                        "Path Properties", "ID", Me.ID, True))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Line Color", m_clLineColor.GetType(), "LineColor", _
                                        "Path Properties", "Sets the pen color used to draw the path.", m_clLineColor))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Visible", Me.Visible.GetType(), "Visible", _
                                        "Path Properties", "Sets whether the path is visible while the simulation is not running.", Me.Visible))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Visible In Sim", Me.Visible.GetType(), "VisibleInSim", _
                                        "Path Properties", "Sets whether the path is visible while the simulation is running.", Me.VisibleInSim))

            'propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Show Waypoints", Me.Visible.GetType(), "ShowWaypoints", _
            '                            "Path Properties", "Determines whether the waypoints and lines between them are shown or not.", Me.ShowWaypoints))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("LinkedStructure", m_thLinkedStructure.GetType, "LinkedStructure", _
                                        "Part Properties", "Associates this camera path to a structure to look at during its movement.", m_thLinkedStructure, _
                                        GetType(AnimatGUI.TypeHelpers.DropDownListEditor), _
                                        GetType(AnimatGUI.TypeHelpers.LinkedStructureTypeConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("LinkedPart", m_thLinkedPart.GetType, "LinkedPart", _
                                        "Part Properties", "Associates this camera path to a part to look at during its movement.", m_thLinkedPart, _
                                        GetType(AnimatGUI.TypeHelpers.DropDownListEditor), _
                                        GetType(AnimatGUI.TypeHelpers.LinkedBodyPartTypeConverter)))

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = m_snStartTime.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Start Time", pbNumberBag.GetType(), "StartTime", _
                                        "Time Properties", "Sets the time when this path will begin playing.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snEndTime.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("End Time", pbNumberBag.GetType(), "EndTime", _
                                        "Time Properties", "The time when this path will stop playing.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter), True))

        End Sub

        Protected Overrides Sub CloneInternal(ByVal doOriginal As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim bpOrig As CameraPath = DirectCast(doOriginal, CameraPath)

            m_aryWaypoints = DirectCast(bpOrig.m_aryWaypoints.Clone(Me, bCutData, doRoot), Collections.SortedWaypointsList)
            m_aryWaypointsByName = DirectCast(bpOrig.m_aryWaypointsByName.Clone(Me, bCutData, doRoot), Collections.OrderedWaypointsList)
            m_thLinkedStructure = DirectCast(bpOrig.m_thLinkedStructure.Clone(Me, bCutData, doRoot), TypeHelpers.LinkedStructureList)
            m_thLinkedPart = DirectCast(bpOrig.m_thLinkedPart.Clone(Me, bCutData, doRoot), TypeHelpers.LinkedBodyPartList)
            m_clLineColor = bpOrig.m_clLineColor
            m_snStartTime = DirectCast(bpOrig.m_snStartTime.Clone(Me, bCutData, doRoot), Framework.ScaledNumber)
            m_snEndTime = DirectCast(bpOrig.m_snEndTime.Clone(Me, bCutData, doRoot), Framework.ScaledNumber)
            m_bVisible = bpOrig.m_bVisible
            m_bVisibleInSim = bpOrig.m_bVisibleInSim
            m_bShowWaypoints = bpOrig.m_bShowWaypoints

        End Sub

        Public Overrides Function Clone(ByVal doParent As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject) As Framework.DataObject
            Dim oNewNode As New CameraPath(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Public Overrides Function FindObjectByID(ByVal strID As String) As Framework.DataObject

            Dim doObject As AnimatGUI.Framework.DataObject = MyBase.FindObjectByID(strID)
            If doObject Is Nothing AndAlso Not m_aryWaypoints Is Nothing Then doObject = m_aryWaypoints.FindObjectByID(strID)
            Return doObject

        End Function

#Region " Add-Remove to List Methods "

        Public Overrides Sub AddToSim(ByVal bThrowError As Boolean, Optional ByVal bDoNotInit As Boolean = False)
            If Not Util.Simulation Is Nothing AndAlso Not m_SimWindow Is Nothing Then
                Util.Application.SimulationInterface.AddItem(m_SimWindow.ID, "CameraPath", Me.ID, Me.GetSimulationXml("CameraPath"), bThrowError, bDoNotInit)
                InitializeSimulationReferences()
            End If
        End Sub

        Public Overrides Sub RemoveFromSim(ByVal bThrowError As Boolean)
            If Not Util.Simulation Is Nothing AndAlso Not m_doInterface Is Nothing AndAlso Not m_SimWindow Is Nothing Then
                Util.Application.SimulationInterface.RemoveItem(m_SimWindow.ID, "CameraPath", Me.ID, bThrowError)
            End If
            m_doInterface = Nothing
        End Sub

#End Region

        Public Overridable Sub AddWaypoint(ByVal doWaypoint As Waypoint, Optional ByVal bCallSimMethods As Boolean = True)
            m_aryWaypoints.Add(doWaypoint.ID, doWaypoint, bCallSimMethods)

            If Not IsNumeric(doWaypoint.Name) Then
                Throw New System.Exception("New waypoint names must be numeric")
            End If

            m_aryWaypointsByName.Add(CInt(doWaypoint.Name), doWaypoint)
        End Sub

        Protected Overridable Function FindLastWaypoint() As Waypoint

            Dim dblStart As Double = -1
            Dim doWaypoint As Waypoint
            Dim doLast As Waypoint
            For Each deEntry As DictionaryEntry In m_aryWaypointsByName
                doWaypoint = DirectCast(deEntry.Value, Waypoint)
                If doWaypoint.StartTime.ActualValue > dblStart Then
                    doLast = doWaypoint
                    dblStart = doWaypoint.StartTime.ActualValue
                End If
            Next

            Return doLast
        End Function

        Public Overridable Sub AddWaypointSetTimes(ByVal doWaypoint As Waypoint, Optional ByVal bCallSimMethods As Boolean = True)

            If Not IsNumeric(doWaypoint.Name) Then
                Throw New System.Exception("New waypoint names must be numeric")
            End If

            If m_aryWaypointsByName.Count > 0 Then
                Dim doEndWaypoint As Waypoint = FindLastWaypoint()
                doWaypoint.EndTime.ActualValue = doEndWaypoint.EndTime.ActualValue + doWaypoint.TimeSpan.ActualValue
                doWaypoint.StartTime.ActualValue = doEndWaypoint.EndTime.ActualValue
            End If

            doWaypoint.InitializeAfterLoad()
            m_aryWaypoints.Add(doWaypoint.ID, doWaypoint, bCallSimMethods)
            m_aryWaypointsByName.Add(CInt(doWaypoint.Name), doWaypoint)

            RecalculateTimes()
        End Sub

        Public Overridable Sub RemoveWaypoint(ByVal doWaypoint As Waypoint)
            m_aryWaypoints.Remove(doWaypoint.ID)
            m_aryWaypointsByName.Remove(CInt(doWaypoint.Name))
            RecalculateTimes()
        End Sub

        Public Overridable Function NextWaypointName() As String

            Dim iMaxName As Integer = -1
            Dim doWaypoint As Waypoint
            For Each deEntry As DictionaryEntry In m_aryWaypointsByName
                doWaypoint = DirectCast(deEntry.Value, Waypoint)
                Dim iName As Integer = CInt(doWaypoint.Name)

                If iName > iMaxName Then
                    iMaxName = iName
                End If
            Next

            iMaxName = iMaxName + 1
            Return iMaxName.ToString("D3")
        End Function

        Public Overridable Sub RecalculateTimes()

            Dim dblAccumualted As Double = 0
            Dim dblLastStartTime As Double = 0
            Dim doWaypoint As Waypoint
            Dim doPrevWaypoint As Waypoint
            For Each deEntry As DictionaryEntry In m_aryWaypointsByName
                doWaypoint = DirectCast(deEntry.Value, Waypoint)

                Dim dblStart As Double = Me.StartTime.ActualValue + dblAccumualted
                Dim dblEnd As Double = Me.StartTime.ActualValue + dblAccumualted + doWaypoint.TimeSpan.ActualValue

                doWaypoint.SetStartEndTime(dblStart, dblEnd)

                If Not doPrevWaypoint Is Nothing Then
                    Dim vDist As Vec3d = doPrevWaypoint.Position.Point - doWaypoint.Position.Point
                    Dim dblDist As Double = vDist.Magnitude
                    Dim dblVelocity As Double = dblDist / doPrevWaypoint.TimeSpan.ActualValue

                    doPrevWaypoint.SetDistanceVelocity(dblDist, dblVelocity)
                End If

                dblAccumualted = dblAccumualted + doWaypoint.TimeSpan.ActualValue
                dblLastStartTime = dblStart

                doWaypoint.LastWaypoint = False
                doPrevWaypoint = doWaypoint
            Next

            'Get the last waypoint
            If Not doPrevWaypoint Is Nothing Then
                doPrevWaypoint.LastWaypoint = True
                doPrevWaypoint.SetDistanceVelocity(0, 0)
            End If

            'The end time for the entire path is actually the start time of the last waypoint.
            If m_aryWaypoints.Count > 0 Then
                Me.EndTime = New ScaledNumber(Me, "EndTime", dblLastStartTime, ScaledNumber.enumNumericScale.None, "s", "s")
            Else
                Me.EndTime = New ScaledNumber(Me, "EndTime", Me.StartTime.ActualValue + 1, ScaledNumber.enumNumericScale.None, "s", "s")
            End If

            If Not Util.ProjectProperties Is Nothing Then
                Util.ProjectProperties.RefreshProperties()
            End If
        End Sub

        Public Overrides Sub AddToReplaceIDList(ByVal aryReplaceIDList As ArrayList, ByVal arySelectedItems As ArrayList)
            MyBase.AddToReplaceIDList(aryReplaceIDList, arySelectedItems)

            m_aryWaypoints.AddToReplaceIDList(aryReplaceIDList, arySelectedItems)
        End Sub

        Public Overrides Sub AddToRecursiveSelectedItemsList(ByVal arySelectedItems As ArrayList)
            MyBase.AddToRecursiveSelectedItemsList(arySelectedItems)

            m_aryWaypoints.AddToRecursiveSelectedItemsList(arySelectedItems)
        End Sub

        Public Overrides Sub InitializeAfterLoad()
            MyBase.InitializeAfterLoad()

            Dim doChild As AnimatGUI.Framework.DataObject
            For Each deEntry As DictionaryEntry In m_aryWaypoints
                doChild = DirectCast(deEntry.Value, AnimatGUI.Framework.DataObject)
                doChild.InitializeAfterLoad()
            Next

            Dim bpPart As AnimatGUI.DataObjects.Physical.BodyPart
            If (Not Me.LinkedPart Is Nothing AndAlso Me.LinkedPart.BodyPart Is Nothing) AndAlso (m_strLinkedBodyPartID.Length > 0) Then
                bpPart = DirectCast(Util.Simulation.FindObjectByID(m_strLinkedBodyPartID), DataObjects.Physical.BodyPart)

                If Not bpPart Is Nothing Then
                    Me.LinkedStructure = New TypeHelpers.LinkedStructureList(bpPart.ParentStructure, TypeHelpers.LinkedStructureList.enumStructureType.All)
                    Me.LinkedPart = New TypeHelpers.LinkedBodyPartList(bpPart.ParentStructure, bpPart, GetType(AnimatGUI.DataObjects.Physical.BodyPart))
                End If
            End If

            'Do this after settign the body part. Body part structure has precedence
            Dim doStruct As AnimatGUI.DataObjects.Physical.PhysicalStructure
            If (Not Me.LinkedStructure Is Nothing AndAlso Me.LinkedStructure.PhysicalStructure Is Nothing) AndAlso (m_strLinkedStructureID.Length > 0) Then
                doStruct = DirectCast(Util.Simulation.FindObjectByID(m_strLinkedStructureID), DataObjects.Physical.PhysicalStructure)

                If Not doStruct Is Nothing Then
                    Me.LinkedStructure = New TypeHelpers.LinkedStructureList(doStruct, TypeHelpers.LinkedStructureList.enumStructureType.All)
                End If
            End If

        End Sub

        Public Overrides Sub Automation_SetLinkedItem(ByVal strItemPath As String, ByVal strLinkedItemPath As String)

            Dim tnLinkedNode As Crownwood.DotNetMagic.Controls.Node = Util.FindTreeNodeByPath(strLinkedItemPath, Util.ProjectWorkspace.TreeView.Nodes)

            If tnLinkedNode Is Nothing OrElse tnLinkedNode.Tag Is Nothing OrElse Not Util.IsTypeOf(tnLinkedNode.Tag.GetType, GetType(DataObjects.Physical.BodyPart), False) Then
                Throw New System.Exception("The path to the specified linked node was not the correct node type.")
            End If

            Dim bpLinkedPart As DataObjects.Physical.BodyPart = DirectCast(tnLinkedNode.Tag, DataObjects.Physical.BodyPart)

            Me.LinkedStructure = New TypeHelpers.LinkedStructureList(bpLinkedPart.ParentStructure, TypeHelpers.LinkedStructureList.enumStructureType.All)
            Me.LinkedPart = New TypeHelpers.LinkedBodyPartList(bpLinkedPart.ParentStructure, bpLinkedPart, GetType(AnimatGUI.DataObjects.Physical.BodyPart))

            Util.ProjectWorkspace.RefreshProperties()
        End Sub

        Public Overrides Sub InitializeSimulationReferences(Optional ByVal bShowError As Boolean = True)
            'Only do this if not already initialized.
            If m_doInterface Is Nothing Then
                MyBase.InitializeSimulationReferences(bShowError)

                Dim doChild As AnimatGUI.Framework.DataObject
                For Each deEntry As DictionaryEntry In m_aryWaypoints
                    doChild = DirectCast(deEntry.Value, AnimatGUI.Framework.DataObject)
                    doChild.InitializeSimulationReferences(bShowError)
                Next
            End If
        End Sub

        Protected Overridable Sub ConnectLinkedEvents()
            DiconnectLinkedEvents()

            If Not m_thLinkedStructure Is Nothing AndAlso Not m_thLinkedStructure.PhysicalStructure Is Nothing Then
                AddHandler m_thLinkedStructure.PhysicalStructure.AfterRemoveItem, AddressOf Me.OnAfterRemoveLinkedStructure
            End If
            If Not m_thLinkedPart Is Nothing AndAlso Not m_thLinkedPart.BodyPart Is Nothing Then
                AddHandler m_thLinkedPart.BodyPart.AfterRemoveItem, AddressOf Me.OnAfterRemoveLinkedPart
            End If
        End Sub

        Protected Overridable Sub DiconnectLinkedEvents()
            If Not m_thLinkedStructure Is Nothing AndAlso Not m_thLinkedStructure.PhysicalStructure Is Nothing Then
                RemoveHandler m_thLinkedStructure.PhysicalStructure.AfterRemoveItem, AddressOf Me.OnAfterRemoveLinkedStructure
            End If
            If Not m_thLinkedPart Is Nothing AndAlso Not m_thLinkedPart.BodyPart Is Nothing Then
                RemoveHandler m_thLinkedPart.BodyPart.AfterRemoveItem, AddressOf Me.OnAfterRemoveLinkedPart
            End If
        End Sub

#Region " Treeview/Menu Methods "

        Public Overrides Sub CreateWorkspaceTreeView(ByVal doParent As Framework.DataObject, _
                                                       ByVal tnParentNode As Crownwood.DotNetMagic.Controls.Node, _
                                                       Optional ByVal bRootObject As Boolean = False)
            MyBase.CreateWorkspaceTreeView(doParent, tnParentNode, bRootObject)

            Dim doWaypoint As AnimatGUI.DataObjects.Visualization.Waypoint
            For Each deEntry As DictionaryEntry In m_aryWaypoints
                doWaypoint = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Visualization.Waypoint)
                doWaypoint.CreateWorkspaceTreeView(Me, m_tnWorkspaceNode)
            Next

        End Sub

        Public Overrides Function WorkspaceTreeviewPopupMenu(ByRef tnSelectedNode As Crownwood.DotNetMagic.Controls.Node, ByVal ptPoint As Point) As Boolean

            If tnSelectedNode Is m_tnWorkspaceNode Then
                Dim mcDelete As New System.Windows.Forms.ToolStripMenuItem("Delete Camera Path", Util.Application.ToolStripImages.GetImage("AnimatGUI.Delete.gif"), New EventHandler(AddressOf Util.Application.OnDeleteFromWorkspace))
                Dim mcAddWaypoint As New System.Windows.Forms.ToolStripMenuItem("New Waypoint", Util.Application.ToolStripImages.GetImage("AnimatGUI.AddCameraWaypoint.gif"), New EventHandler(AddressOf Me.AddCameraWaypoint))
                Dim mcSepExpand As New ToolStripSeparator()
                Dim mcExpandAll As New System.Windows.Forms.ToolStripMenuItem("Expand All", Util.Application.ToolStripImages.GetImage("AnimatGUI.Expand.gif"), New EventHandler(AddressOf Me.OnExpandAll))
                Dim mcCollapseAll As New System.Windows.Forms.ToolStripMenuItem("Collapse All", Util.Application.ToolStripImages.GetImage("AnimatGUI.Collapse.gif"), New EventHandler(AddressOf Me.OnCollapseAll))

                mcExpandAll.Tag = tnSelectedNode
                mcCollapseAll.Tag = tnSelectedNode


                ' Create the popup menu object
                Dim popup As New AnimatContextMenuStrip("AnimatGUI.DataObjects.Charting.Axis.WorkspaceTreeviewPopupMenu", Util.SecurityMgr)
                popup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {mcDelete, mcAddWaypoint, mcSepExpand, mcExpandAll, mcCollapseAll})

                Util.ProjectWorkspace.ctrlTreeView.ContextMenuNode = popup
                Return True
            End If

            Return False
        End Function

#End Region

        Public Overrides Function Delete(Optional ByVal bAskToDelete As Boolean = True, Optional ByVal e As Crownwood.DotNetMagic.Controls.TGCloseRequestEventArgs = Nothing) As Boolean

            Try
                If bAskToDelete AndAlso Util.ShowMessage("Are you certain that you want to delete this " & _
                                    "camera path and all its waypoints?", "Delete Node", MessageBoxButtons.YesNo) <> DialogResult.Yes Then
                    Return False
                End If

                Util.Application.AppIsBusy = True
                If Not m_SimWindow Is Nothing Then
                    m_SimWindow.CameraPaths.Remove(Me.ID)
                    Me.RemoveWorksapceTreeView()
                End If

                Return True
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                Util.Application.AppIsBusy = False
            End Try

        End Function

        Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            oXml.IntoElem()

            Me.Name = oXml.GetChildString("Name")
            Me.ID = oXml.GetChildString("ID")

            m_strLinkedStructureID = Util.LoadID(oXml, "LinkedStructure", True, "")
            m_strLinkedBodyPartID = Util.LoadID(oXml, "LinkedBodyPart", True, "")

            m_clLineColor = Util.LoadColor(oXml, "LineColor")

            m_snStartTime.LoadData(oXml, "StartTime")
            m_snEndTime.LoadData(oXml, "EndTime")

            m_bVisible = oXml.GetChildBool("Visible", m_bVisible)
            m_bVisibleInSim = oXml.GetChildBool("VisibleInSim", m_bVisibleInSim)
            m_bShowWaypoints = oXml.GetChildBool("ShowWaypoints", m_bShowWaypoints)

            If oXml.FindChildElement("Waypoints", False) Then
                Dim doWaypoint As Waypoint

                oXml.IntoElem() 'Into Waypoint Element
                Dim iCount As Integer = oXml.NumberOfChildren() - 1
                For iIndex As Integer = 0 To iCount
                    oXml.FindChildByIndex(iIndex)

                    doWaypoint = New Waypoint(Me)
                    doWaypoint.LoadData(oXml)

                    'Do not call the sim add method here. We will need to do that later when the window is created.
                    AddWaypoint(doWaypoint, False)
                Next
                oXml.OutOfElem()   'Outof Waypoint Element
            End If

            oXml.OutOfElem()

            RecalculateTimes()
        End Sub

        Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            oXml.AddChildElement("CameraPath")
            oXml.IntoElem()

            oXml.AddChildElement("AssemblyFile", Me.AssemblyFile)
            oXml.AddChildElement("ClassName", Me.ClassName)
            oXml.AddChildElement("ID", Me.ID)
            oXml.AddChildElement("Name", Me.Name)

            m_snStartTime.SaveData(oXml, "StartTime")
            m_snEndTime.SaveData(oXml, "EndTime")

            oXml.AddChildElement("Visible", Me.Visible)
            oXml.AddChildElement("VisibleInSim", Me.VisibleInSim)
            oXml.AddChildElement("ShowWaypoints", m_bShowWaypoints)

            If Not m_thLinkedStructure Is Nothing AndAlso Not m_thLinkedStructure.PhysicalStructure Is Nothing Then
                oXml.AddChildElement("LinkedStructureID", m_thLinkedStructure.PhysicalStructure.ID)
            End If

            If Not m_thLinkedPart Is Nothing AndAlso Not m_thLinkedPart.BodyPart Is Nothing Then
                oXml.AddChildElement("LinkedBodyPartID", m_thLinkedPart.BodyPart.ID)
            End If

            Util.SaveColor(oXml, "LineColor", m_clLineColor)

            If m_aryWaypoints.Count > 0 Then
                oXml.AddChildElement("Waypoints")
                oXml.IntoElem()

                Dim doWaypoint As Waypoint
                For Each deEntry As DictionaryEntry In m_aryWaypointsByName
                    doWaypoint = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Visualization.Waypoint)
                    doWaypoint.SaveData(oXml)
                Next

                oXml.OutOfElem() 'Outof Waypoints Element
            End If

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            oXml.AddChildElement("CameraPath")
            oXml.IntoElem()

            oXml.AddChildElement("ModuleName", Me.ModuleName)
            oXml.AddChildElement("Type", Me.Type)
            oXml.AddChildElement("Name", m_strName)
            oXml.AddChildElement("ID", m_strID)

            m_snStartTime.SaveSimulationXml(oXml, Me, "StartTime")
            m_snEndTime.SaveSimulationXml(oXml, Me, "EndTime")

            oXml.AddChildElement("Visible", Me.Visible)
            oXml.AddChildElement("VisibleInSim", Me.VisibleInSim)
            oXml.AddChildElement("ShowWaypoints", m_bShowWaypoints)

            If Not m_thLinkedStructure Is Nothing AndAlso Not m_thLinkedStructure.PhysicalStructure Is Nothing Then
                oXml.AddChildElement("LinkedStructureID", m_thLinkedStructure.PhysicalStructure.ID)
            End If

            If Not m_thLinkedPart Is Nothing AndAlso Not m_thLinkedPart.BodyPart Is Nothing Then
                oXml.AddChildElement("LinkedBodyPartID", m_thLinkedPart.BodyPart.ID)
            End If

            Util.SaveColor(oXml, "LineColor", m_clLineColor)

            If m_aryWaypoints.Count > 0 Then
                oXml.AddChildElement("Waypoints")
                oXml.IntoElem()

                Dim doWaypoint As Waypoint
                For Each deEntry As DictionaryEntry In m_aryWaypointsByName
                    doWaypoint = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Visualization.Waypoint)
                    doWaypoint.SaveSimulationXml(oXml, Me)
                Next

                oXml.OutOfElem() 'Outof Waypoints Element
            End If

            oXml.OutOfElem()

        End Sub

#End Region

#Region "Events"

        Private Sub OnAfterRemoveLinkedStructure(ByRef doObject As Framework.DataObject)
            Try
                Me.LinkedStructure = New TypeHelpers.LinkedStructureList(Nothing, TypeHelpers.LinkedStructureList.enumStructureType.All)
            Catch ex As Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub OnAfterRemoveLinkedPart(ByRef doObject As Framework.DataObject)
            Try
                Me.LinkedPart = New TypeHelpers.LinkedBodyPartList(Me.LinkedStructure.PhysicalStructure, Nothing, GetType(AnimatGUI.DataObjects.Physical.BodyPart))
            Catch ex As Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub AddCameraWaypoint(sender As Object, e As System.EventArgs)
            Try
                If Not m_SimWindow Is Nothing Then
                    m_SimWindow.AddWaypointToolStripButton_Click(sender, e)
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace

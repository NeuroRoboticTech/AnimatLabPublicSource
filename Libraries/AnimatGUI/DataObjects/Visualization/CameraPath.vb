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

        Protected m_thLinkedStructure As AnimatGUI.TypeHelpers.LinkedStructureList
        Protected m_thLinkedPart As AnimatGUI.TypeHelpers.LinkedBodyPartList

        Protected m_clLineColor As System.Drawing.Color = System.Drawing.Color.Red

        'Only used during loading
        Protected m_strLinkedStructureID As String = ""
        Protected m_strLinkedBodyPartID As String = ""

        Protected m_SimWindow As Forms.Tools.ScriptedSimulationWindow

#End Region

#Region " Properties "

        Public Overridable ReadOnly Property Type() As String
            Get
                Return "CameraPath"
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property LineColor() As System.Drawing.Color
            Get
                Return m_clLineColor
            End Get
            Set(ByVal Value As System.Drawing.Color)
                m_clLineColor = Value
            End Set
        End Property

        Public Overridable ReadOnly Property Waypoints() As Collections.SortedWaypointsList
            Get
                Return m_aryWaypoints
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

                DiconnectLinkedEvents()
                m_thLinkedPart = Value
                ConnectLinkedEvents()
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

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()
            If Not m_aryWaypoints Is Nothing Then m_aryWaypoints.ClearIsDirty()
            If Not m_thLinkedStructure Is Nothing Then m_thLinkedStructure.ClearIsDirty()
            If Not m_thLinkedPart Is Nothing Then m_thLinkedPart.ClearIsDirty()
        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Name", Me.Name.GetType(), "Name", _
                                        "Path Properties", "Name", Me.Name))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("ID", Me.ID.GetType(), "ID", _
                                        "Path Properties", "ID", Me.ID, True))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("LinkedStructure", m_thLinkedStructure.GetType, "LinkedStructure", _
                                        "Path Properties", "Associates this camera path to a structure to look at during its movement.", m_thLinkedStructure, _
                                        GetType(AnimatGUI.TypeHelpers.DropDownListEditor), _
                                        GetType(AnimatGUI.TypeHelpers.LinkedStructureTypeConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("LinkedPart", m_thLinkedPart.GetType, "LinkedPart", _
                                        "Path Properties", "Associates this camera path to a part to look at during its movement.", m_thLinkedPart, _
                                        GetType(AnimatGUI.TypeHelpers.DropDownListEditor), _
                                        GetType(AnimatGUI.TypeHelpers.LinkedBodyPartTypeConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Line Color", m_clLineColor.GetType(), "LineColor", _
                                        "Path Properties", "Sets the pen color used to draw the path.", m_clLineColor))

        End Sub

        Protected Overrides Sub CloneInternal(ByVal doOriginal As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim bpOrig As CameraPath = DirectCast(doOriginal, CameraPath)

            m_aryWaypoints = DirectCast(bpOrig.m_aryWaypoints.Clone(Me, bCutData, doRoot), Collections.SortedWaypointsList)
            m_thLinkedStructure = DirectCast(bpOrig.m_thLinkedStructure.Clone(Me, bCutData, doRoot), TypeHelpers.LinkedStructureList)
            m_thLinkedPart = DirectCast(bpOrig.m_thLinkedPart.Clone(Me, bCutData, doRoot), TypeHelpers.LinkedBodyPartList)
            m_clLineColor = bpOrig.m_clLineColor

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

        Public Overrides Sub AddToReplaceIDList(ByVal aryReplaceIDList As ArrayList)
            MyBase.AddToReplaceIDList(aryReplaceIDList)

            m_aryWaypoints.AddToReplaceIDList(aryReplaceIDList)
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


        Public Overrides Function WorkspaceTreeviewPopupMenu(ByRef tnSelectedNode As Crownwood.DotNetMagic.Controls.Node, ByVal ptPoint As Point) As Boolean

            If tnSelectedNode Is m_tnWorkspaceNode Then
                Dim mcAddWaypoint As New System.Windows.Forms.ToolStripMenuItem("New Waypoint", Util.Application.ToolStripImages.GetImage("AnimatGUI.AddCameraWaypoint.gif"), New EventHandler(AddressOf Me.AddCameraWaypoint))

                ' Create the popup menu object
                Dim popup As New AnimatContextMenuStrip("AnimatGUI.DataObjects.Physical.Environment.WorkspaceTreeviewPopupMenu", Util.SecurityMgr)
                popup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {mcAddWaypoint})

                Util.ProjectWorkspace.ctrlTreeView.ContextMenuNode = popup
                Return True
            End If

            Return False
        End Function

#End Region

        Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            oXml.IntoElem()

            Me.Name = oXml.GetChildString("Name")
            Me.ID = oXml.GetChildString("ID")

            m_strLinkedStructureID = Util.LoadID(oXml, "LinkedStructure", True, "")
            m_strLinkedBodyPartID = Util.LoadID(oXml, "LinkedBodyPart", True, "")

            m_clLineColor = Util.LoadColor(oXml, "LineColor")

            If oXml.FindChildElement("Waypoints", False) Then
                Dim doWaypoint As Waypoint

                oXml.IntoElem() 'Into Waypoint Element
                Dim iCount As Integer = oXml.NumberOfChildren() - 1
                For iIndex As Integer = 0 To iCount
                    oXml.FindChildByIndex(iIndex)

                    doWaypoint = New Waypoint(Me)
                    doWaypoint.LoadData(oXml)

                    'Do not call the sim add method here. We will need to do that later when the window is created.
                    m_aryWaypoints.Add(doWaypoint.ID, doWaypoint, False)
                Next
                oXml.OutOfElem()   'Outof Waypoint Element
            End If

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            oXml.AddChildElement("CameraPath")
            oXml.IntoElem()

            oXml.AddChildElement("AssemblyFile", Me.AssemblyFile)
            oXml.AddChildElement("ClassName", Me.ClassName)
            oXml.AddChildElement("ID", Me.ID)
            oXml.AddChildElement("Name", Me.Name)

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
                For Each deEntry As DictionaryEntry In m_aryWaypoints
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
            oXml.AddChildElement("StartTime", 0)
            oXml.AddChildElement("EndTime", 1)

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
                For Each deEntry As DictionaryEntry In m_aryWaypoints
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

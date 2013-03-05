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


    Public Class Waypoint
        Inherits AnimatGUI.Framework.DataObject

#Region " Attributes "

        Protected m_doParentPath As CameraPath
        Protected m_svPosition As Framework.ScaledVector3

        Protected m_snStartTime As Framework.ScaledNumber
        Protected m_snEndTime As Framework.ScaledNumber
        Protected m_snTimeSpan As Framework.ScaledNumber
        Protected m_snVelocity As Framework.ScaledNumber
        Protected m_snDistance As Framework.ScaledNumber

        Protected m_bLastWaypoint As Boolean = False

#End Region

#Region " Properties "

        Public Overridable ReadOnly Property Type() As String
            Get
                Return "Waypoint"
            End Get
        End Property

        Public Overrides ReadOnly Property WorkspaceImageName As String
            Get
                Return "AnimatGUI.CameraWaypoints.gif"
            End Get
        End Property

        Public Overrides Property Name As String
            Get
                Return MyBase.Name
            End Get
            Set(value As String)
                If Not IsNumeric(value) Then
                    Throw New System.Exception("New waypoint names must be numeric")
                End If

                If m_bIsInitialized Then
                    If Not m_doParentPath Is Nothing Then
                        If m_doParentPath.WaypointsByName.Contains(CInt(value)) Then
                            Throw New System.Exception("A waypoint with the name '" + value + "' already exists. Waypoint names must be unique.")
                        End If

                        m_doParentPath.WaypointsByName.Remove(CInt(Me.Name))
                    End If

                    MyBase.Name = value

                    If Not m_doParentPath Is Nothing Then
                        m_doParentPath.WaypointsByName.Add(CInt(Me.Name), Me)
                        m_doParentPath.RecalculateTimes()
                    End If
                Else
                    MyBase.Name = value
                End If
            End Set
        End Property

        Public Overridable Property Position() As Framework.ScaledVector3
            Get
                Return m_svPosition
            End Get
            Set(ByVal value As Framework.ScaledVector3)
                Me.SetSimData("Position", value.GetSimulationXml("Position"), True)
                m_svPosition.CopyData(value)
            End Set
        End Property

        Public Overridable Property Distance() As Framework.ScaledNumber
            Get
                Return m_snDistance
            End Get
            Set(ByVal value As Framework.ScaledNumber)
                If value.ActualValue < 0 Then
                    Throw New System.Exception("The distance must be greater than zero.")
                End If

                m_snDistance.CopyData(value)

                If Not m_doParentPath Is Nothing Then
                    m_doParentPath.RecalculateTimes()
                End If
            End Set
        End Property

        Public Overridable Property Velocity() As Framework.ScaledNumber
            Get
                Return m_snVelocity
            End Get
            Set(ByVal value As Framework.ScaledNumber)
                If value.ActualValue < 0 Then
                    Throw New System.Exception("The velocity must be greater than zero.")
                End If

                m_snVelocity.CopyData(value)
                Dim dblTime As Double = Me.Distance.ActualValue / m_snVelocity.ActualValue
                Me.TimeSpan = New ScaledNumber(Me, "TimeSpan", dblTime, ScaledNumber.enumNumericScale.None, "s", "s")
            End Set
        End Property

        Public Overridable Property TimeSpan() As Framework.ScaledNumber
            Get
                Return m_snTimeSpan
            End Get
            Set(ByVal value As Framework.ScaledNumber)
                If value.ActualValue <= 0 Then
                    Throw New System.Exception("The timespan must be greater than zero.")
                End If

                m_snTimeSpan.CopyData(value)

                If Not m_doParentPath Is Nothing Then
                    m_doParentPath.RecalculateTimes()
                End If
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
                If Not m_doParentPath Is Nothing AndAlso value.ActualValue < m_doParentPath.StartTime.ActualValue Then
                    Throw New System.Exception("You cannot specify a start time less than the start time of the path.")
                End If
                If value.ActualValue >= m_snEndTime.ActualValue Then
                    Throw New System.Exception("You cannot specify a start time greater than the end time.")
                End If

                Me.SetSimData("Time", value.ToString(), True)
                m_snStartTime.CopyData(value)
                m_snStartTime.PropertiesReadOnly = True
            End Set
        End Property

        Public Overridable Property EndTime() As Framework.ScaledNumber
            Get
                Return m_snEndTime
            End Get
            Set(ByVal value As Framework.ScaledNumber)
                If value.ActualValue < 0 Then
                    Throw New System.Exception("You cannot specify an end time less than zero.")
                End If
                If value.ActualValue <= m_snStartTime.ActualValue Then
                    Throw New System.Exception("You cannot specify an end time less than the start time.")
                End If

                m_snEndTime.CopyData(value)
                m_snEndTime.PropertiesReadOnly = True
            End Set
        End Property

        Public Overridable Property LastWaypoint() As Boolean
            Get
                Return m_bLastWaypoint
            End Get
            Set(ByVal value As Boolean)
                m_bLastWaypoint = value
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            If Not doParent Is Nothing AndAlso Util.IsTypeOf(doParent.GetType(), GetType(CameraPath), False) Then
                m_doParentPath = DirectCast(doParent, CameraPath)
            End If

            m_svPosition = New ScaledVector3(Me, "Position", "Location of the waypoint in world coordinates.", "Meters", "m")
            m_snStartTime = New ScaledNumber(Me, "StartTime", 0, ScaledNumber.enumNumericScale.None, "s", "s")
            m_snEndTime = New ScaledNumber(Me, "EndTime", 1, ScaledNumber.enumNumericScale.None, "s", "s")
            m_snTimeSpan = New ScaledNumber(Me, "TimeSpan", 1, ScaledNumber.enumNumericScale.None, "s", "s")
            m_snVelocity = New ScaledNumber(Me, "Velocity", 1, ScaledNumber.enumNumericScale.None, "m/s", "m/s")
            m_snDistance = New ScaledNumber(Me, "Distance", 1, ScaledNumber.enumNumericScale.None, "m", "m")

            m_snStartTime.PropertiesReadOnly = True
            m_snEndTime.PropertiesReadOnly = True

            AddHandler m_svPosition.ValueChanged, AddressOf Me.OnPositionValueChanged

        End Sub

        Public Overridable Sub SetStartEndTime(ByVal dblStartTime As Double, ByVal dblEndTime As Double)

            If dblStartTime < 0 Then
                Throw New System.Exception("You cannot specify a start time less than zero.")
            End If

            If dblEndTime < 0 Then
                Throw New System.Exception("You cannot specify an end time less than zero.")
            End If

            If Not m_doParentPath Is Nothing AndAlso dblStartTime < m_doParentPath.StartTime.ActualValue Then
                Throw New System.Exception("You cannot specify a start time less than the start time of the path.")
            End If

            If dblEndTime < dblStartTime Then
                Throw New System.Exception("You cannot specify an end time less than the start time.")
            End If

            m_snStartTime = New ScaledNumber(Me, "StartTime", dblStartTime, ScaledNumber.enumNumericScale.None, "s", "s")
            m_snEndTime = New ScaledNumber(Me, "EndTime", dblEndTime, ScaledNumber.enumNumericScale.None, "s", "s")

            m_snStartTime.PropertiesReadOnly = True
            m_snEndTime.PropertiesReadOnly = True

            Me.SetSimData("Time", m_snStartTime.ActualValue.ToString(), True)

        End Sub

        Public Overridable Sub SetDistanceVelocity(ByVal dblDistance As Double, ByVal dblVelocity As Double)

            m_snDistance = New ScaledNumber(Me, "Distance", dblDistance, ScaledNumber.enumNumericScale.None, "m", "m")
            m_snVelocity = New ScaledNumber(Me, "Velocity", dblVelocity, ScaledNumber.enumNumericScale.None, "m/s", "m/s")

            m_snDistance.PropertiesReadOnly = True

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()
            If Not m_svPosition Is Nothing Then m_svPosition.ClearIsDirty()
            If Not m_snStartTime Is Nothing Then m_snStartTime.ClearIsDirty()
            If Not m_snEndTime Is Nothing Then m_snEndTime.ClearIsDirty()
            If Not m_snTimeSpan Is Nothing Then m_snTimeSpan.ClearIsDirty()
            If Not m_snVelocity Is Nothing Then m_snVelocity.ClearIsDirty()
            If Not m_snDistance Is Nothing Then m_snDistance.ClearIsDirty()
        End Sub

        Protected Overrides Sub CloneInternal(ByVal doOriginal As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim bpOrig As Waypoint = DirectCast(doOriginal, Waypoint)

            m_svPosition = DirectCast(bpOrig.m_svPosition.Clone(Me, bCutData, doRoot), Framework.ScaledVector3)
            m_snStartTime = DirectCast(bpOrig.m_snStartTime.Clone(Me, bCutData, doRoot), Framework.ScaledNumber)
            m_snEndTime = DirectCast(bpOrig.m_snEndTime.Clone(Me, bCutData, doRoot), Framework.ScaledNumber)
            m_snTimeSpan = DirectCast(bpOrig.m_snTimeSpan.Clone(Me, bCutData, doRoot), Framework.ScaledNumber)
            m_snVelocity = DirectCast(bpOrig.m_snVelocity.Clone(Me, bCutData, doRoot), Framework.ScaledNumber)
            m_snDistance = DirectCast(bpOrig.m_snDistance.Clone(Me, bCutData, doRoot), Framework.ScaledNumber)
            m_bLastWaypoint = bpOrig.m_bLastWaypoint

        End Sub

        Public Overrides Function Clone(ByVal doParent As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject) As Framework.DataObject
            Dim oNewNode As New Waypoint(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Name", Me.Name.GetType(), "Name", _
                                        "Point Properties", "Name", Me.Name))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("ID", Me.ID.GetType(), "ID", _
                                        "Point Properties", "ID", Me.ID, True))


            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = Me.Position.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Position", pbNumberBag.GetType(), "Position", _
                                        "Point Properties", "Sets the location of this waypoint.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledVector3.ScaledVector3PropBagConverter)))

            If Not m_bLastWaypoint Then
                pbNumberBag = m_snTimeSpan.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Time Span", pbNumberBag.GetType(), "TimeSpan", _
                                            "Time Properties", "Sets the time span between this waypoint this waypoint and the next.", pbNumberBag, _
                                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

                pbNumberBag = m_snStartTime.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Start Time", pbNumberBag.GetType(), "StartTime", _
                                            "Time Properties", "The time when this point will begin execution.", pbNumberBag, _
                                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter), True))

                pbNumberBag = m_snTimeSpan.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("End Time", pbNumberBag.GetType(), "EndTime", _
                                            "Time Properties", "The time when this point will end execution.", pbNumberBag, _
                                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter), True))

                pbNumberBag = m_snTimeSpan.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Velocity", pbNumberBag.GetType(), "Velocity", _
                                            "Time Properties", "The velocity for moving along this segment.", pbNumberBag, _
                                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

                pbNumberBag = m_snDistance.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Distance", pbNumberBag.GetType(), "Distance", _
                                            "Time Properties", "The distance between this point and the next one.", pbNumberBag, _
                                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter), True))
            End If

        End Sub

#Region " Add-Remove to List Methods "

        Public Overrides Sub AddToSim(ByVal bThrowError As Boolean, Optional ByVal bDoNotInit As Boolean = False)
            If Not Util.Simulation Is Nothing Then
                Util.Application.SimulationInterface.AddItem(m_doParent.ID, "Waypoint", Me.ID, Me.GetSimulationXml("Waypoint"), bThrowError, bDoNotInit)
                InitializeSimulationReferences()
            End If
        End Sub

        Public Overrides Sub RemoveFromSim(ByVal bThrowError As Boolean)
            If Not Util.Simulation Is Nothing AndAlso Not m_doInterface Is Nothing Then
                Util.Application.SimulationInterface.RemoveItem(m_doParent.ID, "Waypoint", Me.ID, bThrowError)
            End If
            m_doInterface = Nothing
        End Sub

#End Region

        Public Overrides Function WorkspaceTreeviewPopupMenu(ByRef tnSelectedNode As Crownwood.DotNetMagic.Controls.Node, ByVal ptPoint As Point) As Boolean

            If tnSelectedNode Is m_tnWorkspaceNode Then
                Dim mcDelete As New System.Windows.Forms.ToolStripMenuItem("Delete Waypoint", Util.Application.ToolStripImages.GetImage("AnimatGUI.Delete.gif"), New EventHandler(AddressOf Util.Application.OnDeleteFromWorkspace))

                ' Create the popup menu object
                Dim popup As New AnimatContextMenuStrip("AnimatGUI.DataObjects.Charting.Axis.WorkspaceTreeviewPopupMenu", Util.SecurityMgr)
                popup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {mcDelete})

                Util.ProjectWorkspace.ctrlTreeView.ContextMenuNode = popup
                Return True
            End If

            Return False
        End Function

        Public Overrides Function Delete(Optional ByVal bAskToDelete As Boolean = True, Optional ByVal e As Crownwood.DotNetMagic.Controls.TGCloseRequestEventArgs = Nothing) As Boolean

            Try
                If bAskToDelete AndAlso Util.ShowMessage("Are you certain that you want to delete this " & _
                                    "waypoint?", "Delete Node", MessageBoxButtons.YesNo) <> DialogResult.Yes Then
                    Return False
                End If

                Util.Application.AppIsBusy = True
                If Not m_doParentPath Is Nothing Then
                    m_doParentPath.RemoveWaypoint(Me)
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
            MyBase.LoadData(oXml)

            oXml.IntoElem()

            Me.ID = oXml.GetChildString("ID")
            Me.Name = oXml.GetChildString("Name")

            m_svPosition.LoadData(oXml, "Position")
            m_snTimeSpan.LoadData(oXml, "TimeSpan")

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            oXml.AddChildElement("Waypoint")
            oXml.IntoElem()

            oXml.AddChildElement("AssemblyFile", Me.AssemblyFile)
            oXml.AddChildElement("ClassName", Me.ClassName)
            oXml.AddChildElement("ID", Me.ID)
            oXml.AddChildElement("Name", Me.Name)

            m_svPosition.SaveData(oXml, "Position")
            m_snTimeSpan.SaveData(oXml, "TimeSpan")

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            oXml.AddChildElement("Waypoint")
            oXml.IntoElem()

            oXml.AddChildElement("ModuleName", Me.ModuleName)
            oXml.AddChildElement("Type", Me.Type)
            oXml.AddChildElement("Name", m_strName)
            oXml.AddChildElement("ID", m_strID)
            m_svPosition.SaveSimulationXml(oXml, Me, "Position")
            m_snStartTime.SaveSimulationXml(oXml, Me, "Time")

            oXml.OutOfElem()

        End Sub

#End Region

#Region " Events "

        'These three events handlers are called whenever a user manually changes the value of the position or rotation.
        'This is different from the OnPositionChanged event. Those events come up from the simulation.
        Protected Overridable Sub OnPositionValueChanged()
            Try
                If Not Util.ProjectProperties Is Nothing Then
                    Me.SetSimData("Position", m_svPosition.GetSimulationXml("Position"), True)
                    Util.ProjectProperties.RefreshProperties()
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace


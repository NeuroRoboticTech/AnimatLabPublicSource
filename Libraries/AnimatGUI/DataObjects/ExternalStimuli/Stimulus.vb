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

Namespace DataObjects.ExternalStimuli

    Public MustInherit Class Stimulus
        Inherits AnimatGUI.DataObjects.DragObject


#Region " Enums "

        Public Enum enumValueType
            Constant
            Equation
        End Enum

#End Region

#Region " Attributes "

        Protected m_snEndTime As AnimatGUI.Framework.ScaledNumber
        Protected m_snStartTime As AnimatGUI.Framework.ScaledNumber
        Protected m_snStepInterval As AnimatGUI.Framework.ScaledNumber
        Protected m_bAlwaysActive As Boolean = False
        Protected m_eValueType As enumValueType = enumValueType.Constant
        Protected m_strEquation As String = ""

        Protected m_aryCompatibleDataObjects As New Collections.CompatibleDataObjects(Me)

        Protected m_doStimulatedItem As DragObject

#End Region

#Region " Properties "

        Public Overridable Property StimulatedItem() As DragObject
            Get
                Return m_doStimulatedItem
            End Get
            Set(value As DragObject)
                DisconnectItemEvents()
                m_doStimulatedItem = value
                ConnectItemEvents()
            End Set
        End Property

        Public Overrides Property Name() As String
            Get
                Return m_strName
            End Get
            Set(ByVal Value As String)
                If Value.Trim.Length = 0 Then
                    Throw New System.Exception("You can not set the name for the stimulus to blank.")
                End If

                'Now add it back in the list with the new name
                m_strName = Value.Trim

                Me.RemoveWorksapceTreeView()
                Me.CreateWorkspaceTreeView(Util.Simulation, Util.Simulation.StimuliTreeNode)
                If Not m_tnWorkspaceNode Is Nothing Then
                    Util.ProjectWorkspace.TreeView.SelectedNode = m_tnWorkspaceNode
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides Property Enabled() As Boolean
            Get
                Return MyBase.Enabled
            End Get
            Set(ByVal Value As Boolean)

                SetSimData("Enabled", Value.ToString, True)
                MyBase.Enabled = Value

                If Not Me.WorkspaceNode Is Nothing Then
                    If m_bEnabled Then
                        Me.WorkspaceNode.BackColor = Color.White
                    Else
                        Me.WorkspaceNode.BackColor = Color.Gray
                    End If
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property StartTime() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snStartTime
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The start time must be greater than 0.")
                End If

                If Value.ActualValue >= m_snEndTime.ActualValue Then
                    Throw New System.Exception("The start time must be less than the end time.")
                End If

                SetSimData("StartTime", Value.ActualValue.ToString, True)
                m_snStartTime.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property EndTime() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snEndTime
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The end time must be greater than 0.")
                End If

                If Value.ActualValue <= m_snStartTime.ActualValue Then
                    Throw New System.Exception("The end time must be greater than the end time.")
                End If

                SetSimData("EndTime", Value.ActualValue.ToString, True)
                m_snEndTime.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property StepInterval() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snStepInterval
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The step interval must be greater than 0.")
                End If

                If Value.ActualValue > (m_snEndTime.ActualValue - m_snStartTime.ActualValue) AndAlso Not m_bAlwaysActive Then
                    Throw New System.Exception("The step interval must not be greater than the duration of the stimulus.")
                End If

                SetSimData("StepInterval", Value.ActualValue.ToString, True)
                m_snStepInterval.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property AlwaysActive() As Boolean
            Get
                Return m_bAlwaysActive
            End Get
            Set(ByVal Value As Boolean)

                SetSimData("AlwaysActive", Value.ToString, True)
                m_bAlwaysActive = Value

                'If we change this property it could change the readonly settings of other properties so lets refresh the property grid.
                If Not Util.ProjectWorkspace Is Nothing AndAlso Not Util.ProjectWorkspace.TreeView.SelectedNode Is Nothing _
                   AndAlso Not Util.ProjectWorkspace.TreeView.SelectedNode.Tag Is Nothing AndAlso _
                   TypeOf Util.ProjectWorkspace.TreeView.SelectedNode.Tag Is AnimatGUI.Framework.DataObject Then

                    Dim doNode As AnimatGUI.Framework.DataObject = DirectCast(Util.ProjectWorkspace.TreeView.SelectedNode.Tag, AnimatGUI.Framework.DataObject)
                    If Not Util.ProjectWorkspace Is Nothing Then
                        Util.ProjectWorkspace.RefreshProperties()
                    End If
                End If
            End Set
        End Property

        Public Overridable Property ValueType() As enumValueType
            Get
                Return m_eValueType
            End Get
            Set(ByVal Value As enumValueType)

                m_eValueType = Value

                If m_eValueType = enumValueType.Equation Then
                    'If we are using a constant then make sure the equation is blanked in the sim so it will not be used.
                    SetSimData("ValueType", "Equation", True)
                End If

                If Not Util.ProjectWorkspace Is Nothing Then
                    Util.ProjectWorkspace.RefreshProperties()
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property Equation() As String
            Get
                Return m_strEquation
            End Get
            Set(ByVal Value As String)
                'Lets verify the equation before we use it.
                'We need to convert the infix equation to postfix
                Dim oMathEval As New MathStringEval
                oMathEval.AddVariable("t")
                oMathEval.Equation = Value
                oMathEval.Parse()

                SetSimData("Equation", oMathEval.PostFix, True)
                m_strEquation = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property StimulusNoLongerValid() As Boolean
            Get
                Return False
            End Get
        End Property

        Public MustOverride ReadOnly Property Description() As String

        Public Overridable ReadOnly Property StimulusModuleName() As String
            Get
                Return Me.ModuleName
            End Get
        End Property

        Public MustOverride ReadOnly Property StimulusClassType() As String

        Public Overridable ReadOnly Property ControlType() As String
            Get
                Return Me.StimulusClassType
            End Get
        End Property

        Public Overrides Property IsDirty() As Boolean
            Get
                Return MyBase.IsDirty
            End Get
            Set(ByVal Value As Boolean)
                Dim bOldVal As Boolean = MyBase.IsDirty

                MyBase.IsDirty = Value
            End Set
        End Property

        Public Overridable ReadOnly Property CompatibleDataObjects() As Collections.CompatibleDataObjects
            Get
                Return m_aryCompatibleDataObjects
            End Get
        End Property


#Region " DragObject Properties "

        <Browsable(False)> _
        Public Overrides Property ItemName() As String
            Get
                Return Me.Name
            End Get
            Set(ByVal Value As String)
                Me.Name = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.ExternalStimulus.gif"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property StructureID() As String
            Get
                Return ""
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property CanBeCharted() As Boolean
            Get
                Return False
            End Get
        End Property

#End Region

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            m_strID = System.Guid.NewGuid().ToString()
            m_snStartTime = New AnimatGUI.Framework.ScaledNumber(Me, "StartTime", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "seconds", "s")
            m_snEndTime = New AnimatGUI.Framework.ScaledNumber(Me, "EndTime", 5, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "seconds", "s")
            m_snStepInterval = New AnimatGUI.Framework.ScaledNumber(Me, "StepInterval", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "seconds", "s")

        End Sub

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim OrigStim As DataObjects.ExternalStimuli.Stimulus = DirectCast(doOriginal, Stimulus)

            m_strName = OrigStim.m_strName
            m_strID = OrigStim.m_strID
            m_bAlwaysActive = OrigStim.m_bAlwaysActive
            m_bEnabled = OrigStim.m_bEnabled
            m_snEndTime = DirectCast(OrigStim.m_snEndTime.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snStartTime = DirectCast(OrigStim.m_snStartTime.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snStepInterval = DirectCast(OrigStim.m_snStepInterval.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_eValueType = OrigStim.m_eValueType
            m_strEquation = OrigStim.m_strEquation
        End Sub

        Public Overrides Function Delete(Optional ByVal bAskToDelete As Boolean = True, Optional ByVal e As Crownwood.DotNetMagic.Controls.TGCloseRequestEventArgs = Nothing) As Boolean

            If Not bAskToDelete OrElse (bAskToDelete AndAlso Util.ShowMessage("Are you certain that you want to permanently delete this " & _
                                "stimulus?", "Delete Stimulus", MessageBoxButtons.YesNo) = DialogResult.Yes) Then
                Me.RemoveWorksapceTreeView()
                Util.Simulation.ProjectStimuli.Remove(Me.ID)
                Return False
            End If

            Return True
        End Function

        Public Overridable Sub AddCompatibleDataObject(ByVal doObject As Framework.DataObject)

            'If we do not already have this object as part of the list then add it.
            If Not m_aryCompatibleDataObjects.Contains(doObject.GetType.FullName) Then
                m_aryCompatibleDataObjects.Add(doObject.GetType.FullName, doObject)
            End If

        End Sub

        Protected Overridable Sub ConnectItemEvents()
            DisconnectItemEvents()

            If Not m_doStimulatedItem Is Nothing Then
                AddHandler m_doStimulatedItem.AfterRemoveItem, AddressOf Me.OnAfterRemoveItem
            End If
        End Sub

        Protected Overridable Sub DisconnectItemEvents()
            If Not m_doStimulatedItem Is Nothing Then
                RemoveHandler m_doStimulatedItem.AfterRemoveItem, AddressOf Me.OnAfterRemoveItem
            End If
        End Sub

#Region " DragObject Methods "

        Public Overrides Sub SaveDataColumnToXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

        End Sub

        Public Overrides Function FindDragObject(ByVal strStructureName As String, ByVal strDataItemID As String, Optional ByVal bThrowError As Boolean = True) As DataObjects.DragObject

            If Util.Simulation.ProjectStimuli.Contains(strDataItemID) Then
                Dim doStimulus As DataObjects.DragObject = DirectCast(Util.Simulation.ProjectStimuli(strDataItemID), DataObjects.DragObject)
                Return doStimulus
            Else
                Throw New System.Exception("No stimulus with the ID " & strDataItemID & " was found.")
            End If

            Return Nothing
        End Function

        Public Overrides Function CreateDataItemTreeView(ByVal frmDataItem As Forms.Tools.SelectDataItem, ByVal tnParent As Crownwood.DotNetMagic.Controls.Node, ByVal tpTemplatePartType As Type) As Crownwood.DotNetMagic.Controls.Node

            'If a template part type is supplied and this part is not one of those template types then do not add it to the tree view
            If tpTemplatePartType Is Nothing OrElse (Not tpTemplatePartType Is Nothing AndAlso Util.IsTypeOf(Me.GetType(), tpTemplatePartType, False)) Then
                Dim myAssembly As System.Reflection.Assembly
                myAssembly = System.Reflection.Assembly.Load(Me.AssemblyModuleName)
                frmDataItem.ImageManager.AddImage(myAssembly, Me.WorkspaceImageName)

                Dim tnNode As Crownwood.DotNetMagic.Controls.Node
                Dim tnStimuli As Crownwood.DotNetMagic.Controls.Node = Nothing
                For Each tnNode In frmDataItem.TreeView.Nodes
                    If tnNode.Text = "Stimuli" Then
                        tnStimuli = tnNode
                    End If
                Next

                If tnStimuli Is Nothing Then
                    tnStimuli = frmDataItem.TreeView.Nodes.Add(New Crownwood.DotNetMagic.Controls.Node("Stimuli"))
                End If

                tnNode = New Crownwood.DotNetMagic.Controls.Node(Me.ItemName)
                tnStimuli.Nodes.Add(tnNode)
                tnNode.ImageIndex = frmDataItem.ImageManager.GetImageIndex(Me.WorkspaceImageName)
                tnNode.SelectedImageIndex = frmDataItem.ImageManager.GetImageIndex(Me.WorkspaceImageName)
                tnNode.Tag = Me

                Return tnNode
            Else
                Return Nothing
            End If
        End Function

#End Region

#Region " Workspace Methods "

        Public Overrides Function WorkspaceTreeviewPopupMenu(ByRef tnSelectedNode As Crownwood.DotNetMagic.Controls.Node, ByVal ptPoint As Point) As Boolean

            If tnSelectedNode Is m_tnWorkspaceNode Then
                Dim mcDelete As New System.Windows.Forms.ToolStripMenuItem("Delete Stimulus", Util.Application.ToolStripImages.GetImage("AnimatGUI.Delete.gif"), New EventHandler(AddressOf Util.Application.OnDeleteFromWorkspace))

                ' Create the popup menu object
                Dim popup As New AnimatContextMenuStrip("AnimatGUI.DataObjects.ExternalStimuli.Stimulus.WorkspaceTreeviewPopupMenu", Util.SecurityMgr)
                popup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {mcDelete})

                Util.ProjectWorkspace.ctrlTreeView.ContextMenuNode = popup

                Return True
            End If

            Return False
        End Function

#End Region

#Region " Add-Remove to List Methods "

        Public Overrides Sub AddToSim(ByVal bThrowError As Boolean, Optional ByVal bDoNotInit As Boolean = False)
            Util.Application.SimulationInterface.AddItem("Simulator", "Stimulus", Me.ID, Me.GetSimulationXml("Stimulus"), bThrowError, bDoNotInit)
            InitializeSimulationReferences()
        End Sub

        Public Overrides Sub RemoveFromSim(ByVal bThrowError As Boolean)
            If Not m_doInterface Is Nothing Then
                Util.Application.SimulationInterface.RemoveItem("Simulator", "Stimulus", Me.ID, bThrowError)
            End If
            m_doInterface = Nothing
        End Sub

#End Region

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Name", m_strName.GetType(), "Name", _
                                        "Stimulus Properties", "The name of this stimulus.", m_strName))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("ID", Me.ID.GetType(), "ID", _
                                        "Stimulus Properties", "ID", Me.ID, True))

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = m_snStartTime.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Start Time", pbNumberBag.GetType(), "StartTime", _
                                        "Stimulus Properties", "This is the time at which the stimulus will be applied.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter), m_bAlwaysActive))

            pbNumberBag = m_snEndTime.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("End Time", pbNumberBag.GetType(), "EndTime", _
                                        "Stimulus Properties", "This is the time at which the stimulus will stop being applied.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter), m_bAlwaysActive))

            'pbNumberBag = m_snStepInterval.Properties
            'propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Step Interval", pbNumberBag.GetType(), "StepInterval", _
            '                            "Stimulus Properties", "If you only want the stimulus to be applied at certain, evenly spaced time steps " & _
            '                            "while the stimulus is active you can use this paramater. It must be some time value less than the total " & _
            '                            "durationd of the stimulus. If it is 0 then the stimulus will be applied at every step.", pbNumberBag, _
            '                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Always Active", m_bAlwaysActive.GetType(), "AlwaysActive", _
                                        "Stimulus Properties", "If this is true then this stimulus is active constantly during the simulation.", m_bAlwaysActive))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Enabled", m_bAlwaysActive.GetType(), "Enabled", _
                                        "Stimulus Properties", "If this is false then this stimulus is not applied.", m_bEnabled))

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            If Not m_snStartTime Is Nothing Then m_snStartTime.ClearIsDirty()
            If Not m_snEndTime Is Nothing Then m_snEndTime.ClearIsDirty()
            If Not m_snStepInterval Is Nothing Then m_snStepInterval.ClearIsDirty()
        End Sub

        Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            oXml.IntoElem()

            m_strName = oXml.GetChildString("Name")
            m_strID = oXml.GetChildString("ID")

            m_snStartTime.LoadData(oXml, "StartTime")
            m_snEndTime.LoadData(oXml, "EndTime")
            m_snStepInterval.LoadData(oXml, "StepInterval")

            m_bAlwaysActive = oXml.GetChildBool("AlwaysActive", m_bAlwaysActive)
            m_bEnabled = oXml.GetChildBool("Enabled", m_bEnabled)

            m_eValueType = DirectCast([Enum].Parse(GetType(enumValueType), oXml.GetChildString("ValueType", "Constant"), True), enumValueType)
            m_strEquation = oXml.GetChildString("Equation", m_strEquation)

            oXml.OutOfElem()

            Me.IsDirty = False
        End Sub

        Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            oXml.AddChildElement("Stimulus")
            oXml.IntoElem()

            oXml.AddChildElement("AssemblyFile", Me.AssemblyFile)
            oXml.AddChildElement("ClassName", Me.ClassName)

            oXml.AddChildElement("Name", m_strName)
            oXml.AddChildElement("ID", m_strID)

            m_snStartTime.SaveData(oXml, "StartTime")
            m_snEndTime.SaveData(oXml, "EndTime")
            m_snStepInterval.SaveData(oXml, "StepInterval")

            oXml.AddChildElement("AlwaysActive", m_bAlwaysActive)
            oXml.AddChildElement("Enabled", m_bEnabled)

            oXml.AddChildElement("ValueType", m_eValueType.ToString)
            oXml.AddChildElement("Equation", m_strEquation)

            oXml.OutOfElem() ' Outof Node Element

        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            oXml.AddChildElement("Stimulus")
            oXml.IntoElem()

            oXml.AddChildElement("AssemblyFile", Me.AssemblyFile)
            oXml.AddChildElement("ClassName", Me.ClassName)

            oXml.AddChildElement("Name", m_strName)
            oXml.AddChildElement("ID", m_strID)

            m_snStartTime.SaveSimulationXml(oXml, Me, "StartTime")
            m_snEndTime.SaveSimulationXml(oXml, Me, "EndTime")
            m_snStepInterval.SaveSimulationXml(oXml, Me, "StepInterval")

            oXml.AddChildElement("AlwaysActive", m_bAlwaysActive)
            oXml.AddChildElement("Enabled", m_bEnabled)

            oXml.AddChildElement("ValueType", m_eValueType.ToString)
            oXml.AddChildElement("Equation", m_strEquation)

            oXml.OutOfElem() ' Outof Node Element

        End Sub

#End Region

#End Region


#Region "Events"

        Private Sub OnAfterRemoveItem(ByRef doObject As Framework.DataObject)
            Try
                Me.Delete(False)
            Catch ex As Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace


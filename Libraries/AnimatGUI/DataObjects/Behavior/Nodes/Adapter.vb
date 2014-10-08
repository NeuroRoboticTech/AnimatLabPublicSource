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

Namespace DataObjects.Behavior.Nodes

    Public MustInherit Class Adapter
        Inherits Behavior.Node

#Region " Enums "

        Public Enum enumDelayBufferMode
            NoDelayBuffer
            DelayBufferInSimOnly
            DelayBufferAlwaysOn
        End Enum

#End Region

#Region " Attributes "

        Protected m_bnOrigin As Behavior.Node
        Protected m_bnDestination As Behavior.Node

        Protected m_gnGain As AnimatGUI.DataObjects.Gain

        'Only used during the loading process.
        Protected m_strOriginID As String = ""
        Protected m_strDestinationID As String = ""
        Protected m_strDataTypeID As String = ""
        Protected m_strTargetDataTypeID As String = ""
        Protected m_thSourceDataTypes As New TypeHelpers.DataTypeID(Me)
        Protected m_thTargetDataTypes As New TypeHelpers.DataTypeID(Me)

        Protected m_eDelayBufferMode As enumDelayBufferMode
        Protected m_snDelayBufferInterval As ScaledNumber

        Protected m_fltRobotIOScale As Single = 1

        Protected m_bSynchWithRobot As Boolean = False
        Protected m_snSynchUpdateInterval As ScaledNumber
        Protected m_snSynchUpdateStartInterval As ScaledNumber

        Protected m_snInitIODisableDuration As ScaledNumber

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Standard Adapter"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property IsDisplayedInIconPanel() As Boolean
            Get
                Return False
            End Get
        End Property

        <EditorAttribute(GetType(TypeHelpers.GainTypeEditor), GetType(System.Drawing.Design.UITypeEditor))> _
        Public Overridable Property Gain() As AnimatGUI.DataObjects.Gain
            Get
                Return m_gnGain
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Gain)
                If Not Value Is Nothing Then
                    If Not Value Is Nothing Then
                        SetSimData("Gain", Value.GetSimulationXml("Gain", Me), True)
                        Value.InitializeSimulationReferences()
                    End If

                    If Not m_gnGain Is Nothing Then m_gnGain.ParentData = Nothing
                    m_gnGain = Value
                    If Not m_gnGain Is Nothing Then
                        m_gnGain.ParentData = Me
                        m_gnGain.GainPropertyName = "Gain"
                    End If
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property Origin() As Behavior.Node
            Get
                Return m_bnOrigin
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property Destination() As Behavior.Node
            Get
                Return m_bnDestination
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property SourceDataTypes() As TypeHelpers.DataTypeID
            Get
                Return m_thSourceDataTypes
            End Get
            Set(ByVal Value As TypeHelpers.DataTypeID)
                If Not Value Is Nothing Then
                    m_thSourceDataTypes = Value
                    CheckForErrors()
                    SetGainLimits()

                    Me.SetSimData("OriginID", Me.GetSimulationXml("Adapter"), True)
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property TargetDataTypes() As TypeHelpers.DataTypeID
            Get
                Return m_thTargetDataTypes
            End Get
            Set(ByVal Value As TypeHelpers.DataTypeID)
                If Not Value Is Nothing Then
                    m_thTargetDataTypes = Value
                    Me.SetSimData("DestinationID", Me.GetSimulationXml("Adapter"), True)
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property NeuralModuleType() As System.Type
            Get
                Return GetType(AnimatGUI.DataObjects.Behavior.PhysicsModule)
            End Get
        End Property

        Public MustOverride ReadOnly Property AdapterType() As String

        <Browsable(False)> _
        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.StandardAdapter.gif"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property CanBeCharted() As Boolean
            Get
                Return True
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property AllowStimulus() As Boolean
            Get
                Return False
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property DelayBufferMode() As enumDelayBufferMode
            Get
                Return m_eDelayBufferMode
            End Get
            Set(ByVal Value As enumDelayBufferMode)
                SetSimData("DelayBufferMode", Convert.ToInt32(Value).ToString, True)
                m_eDelayBufferMode = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property DelayBufferInterval() As ScaledNumber
            Get
                Return m_snDelayBufferInterval
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 OrElse Value.ActualValue > 5 Then
                    Throw New System.Exception("The time step must be between the range 0 to 5 s.")
                End If

                SetSimData("DelayBufferInterval", Value.ActualValue.ToString, True)
                m_snDelayBufferInterval.CopyData(Value)
            End Set
        End Property

        Public Overridable Property RobotIOScale() As Single
            Get
                Return m_fltRobotIOScale
            End Get
            Set(value As Single)
                If value < 0 Then
                    Throw New System.Exception("The robot IO scale must be greater than 0.")
                End If

                SetSimData("RobotIOScale", value.ToString, True)
                m_fltRobotIOScale = value
            End Set
        End Property

        Public Overridable Property SynchWithRobot() As Boolean
            Get
                Return m_bSynchWithRobot
            End Get
            Set(value As Boolean)
                SetSimData("SynchWithRobot", value.ToString, True)
                m_bSynchWithRobot = value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property SynchUpdateInterval() As ScaledNumber
            Get
                Return m_snSynchUpdateInterval
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 OrElse Value.ActualValue > 5 Then
                    Throw New System.Exception("The synch update interval must be between the range 0 to 5 s.")
                End If

                SetSimData("SynchUpdateInterval", Value.ActualValue.ToString, True)
                m_snSynchUpdateInterval.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property SynchUpdateStartInterval() As ScaledNumber
            Get
                Return m_snSynchUpdateStartInterval
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 OrElse Value.ActualValue > 5 Then
                    Throw New System.Exception("The synch update start interval must be between the range 0 to 5 s.")
                End If

                SetSimData("SynchUpdateStartInterval", Value.ActualValue.ToString, True)
                m_snSynchUpdateStartInterval.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property InitIODisableDuration() As ScaledNumber
            Get
                Return m_snInitIODisableDuration
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 OrElse Value.ActualValue > 5 Then
                    Throw New System.Exception("The initial IO disable duration must be between the range 0 to 5 s.")
                End If

                SetSimData("InitIODisableDuration", Value.ActualValue.ToString, True)
                m_snInitIODisableDuration.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property AllowTemplateNode() As Boolean
            Get
                Return False
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            Try

                Shape = Behavior.Node.enumShape.Display
                Size = New SizeF(40, 40)
                Me.DrawColor = Color.Black
                Me.FillColor = Color.Salmon
                m_bEnabled = True

                Dim myAssembly As System.Reflection.Assembly
                myAssembly = System.Reflection.Assembly.Load("AnimatGUI")

                Me.WorkspaceImage = AnimatGUI.Framework.ImageManager.LoadImage(myAssembly, "AnimatGUI.StandardAdapter.gif")
                Me.Name = "Standard Adapter"

                Me.Font = New Font("Arial", 12, FontStyle.Bold)
                Me.Description = "Provides an interface adapter between nodes and the physics engine."

                AddCompatibleLink(New Behavior.Links.Adapter(Nothing))

                m_gnGain = New AnimatGUI.DataObjects.Gains.Polynomial(Me, "Gain", "Input Variable", "Output Variable", False, False)

                m_snDelayBufferInterval = New AnimatGUI.Framework.ScaledNumber(Me, "DelayBufferInterval", 100, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "seconds", "s")
                m_snInitIODisableDuration = New AnimatGUI.Framework.ScaledNumber(Me, "InitIODisableDuration", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "seconds", "s")
                m_snSynchUpdateInterval = New AnimatGUI.Framework.ScaledNumber(Me, "SynchUpdateInterval", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "seconds", "s")
                m_snSynchUpdateStartInterval = New AnimatGUI.Framework.ScaledNumber(Me, "SynchUpdateStartInterval", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "seconds", "s")

                m_thDataTypes.DataTypes.Clear()
                m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("Enable", "Enable", "", "", 0, 1))
                m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("CalculatedVal", "Calculated Value", "", "", 0, 1))
                m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("NextVal", "Next Value", "", "", 0, 1))
                m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("UpdatedValue", "Updated Value", "", "", 0, 1))
                m_thDataTypes.ID = "Enable"

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim bnOrig As Adapter = DirectCast(doOriginal, Adapter)

            m_gnGain = DirectCast(bnOrig.m_gnGain.Clone(Me, bCutData, doRoot), AnimatGUI.DataObjects.Gain)
            m_bEnabled = bnOrig.m_bEnabled
            m_snDelayBufferInterval = DirectCast(bnOrig.m_snDelayBufferInterval.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snInitIODisableDuration = DirectCast(bnOrig.m_snInitIODisableDuration.Clone(Me, bCutData, doRoot), ScaledNumber)

            m_bSynchWithRobot = bnOrig.m_bSynchWithRobot
            m_snSynchUpdateInterval = DirectCast(bnOrig.m_snSynchUpdateInterval.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snSynchUpdateStartInterval = DirectCast(bnOrig.m_snSynchUpdateStartInterval.Clone(Me, bCutData, doRoot), ScaledNumber)

            m_eDelayBufferMode = bnOrig.m_eDelayBufferMode
            m_fltRobotIOScale = bnOrig.m_fltRobotIOScale
        End Sub

        Public Overrides Sub AddToReplaceIDList(ByVal aryReplaceIDList As ArrayList, ByVal arySelectedItems As ArrayList)
            MyBase.AddToReplaceIDList(aryReplaceIDList, arySelectedItems)

            m_gnGain.AddToReplaceIDList(aryReplaceIDList, arySelectedItems)
        End Sub

        Public Overrides Sub CheckCanAttachAdapter()
            Throw New System.Exception("You cannot attach an adapter to another adapter.")
        End Sub

        Public Overrides Sub BeforeAddLink(ByVal blLink As Link)

            If blLink.ActualDestination Is Me AndAlso Me.InLinks.Count <> 1 Then
                Throw New System.Exception("You can only have one incoming link to an adapter node!")
            End If

            If blLink.ActualOrigin Is Me AndAlso Me.OutLinks.Count <> 1 Then
                Throw New System.Exception("You can only have one outgoing link from an adapter node!")
            End If

            If blLink.Origin Is Me AndAlso Not blLink.Destination Is Nothing Then
                blLink.Destination.CheckCanAttachAdapter()
            End If

            If blLink.Destination Is Me AndAlso Not blLink.Origin Is Nothing Then
                blLink.Origin.CheckCanAttachAdapter()
            End If

        End Sub

        Public Overrides Sub AfterAddLink(ByVal blLink As Link)

            'If this is the destination then get the origin from the other end
            If blLink.ActualDestination Is Me Then
                SetOrigin(blLink.Origin, False)
                Me.m_thSourceDataTypes = DirectCast(m_bnOrigin.DataTypes.Clone(Me, False, Nothing), TypeHelpers.DataTypeID)
                SetGainLimits()

                If Not blLink.ActualOrigin Is Nothing AndAlso blLink.ActualOrigin.IsSensorOrMotor Then
                    Me.m_bSynchWithRobot = True
                End If
            End If

            'If this is the Origin then get the destination from the other end
            If blLink.ActualOrigin Is Me Then
                SetDestination(blLink.Destination, False)

                If Not m_bnDestination.IncomingDataTypes Is Nothing AndAlso m_bnDestination.IncomingDataTypes.ID.Trim.Length > 0 Then
                    Me.m_thTargetDataTypes = DirectCast(m_bnDestination.IncomingDataTypes.Clone(Me, False, Nothing), TypeHelpers.DataTypeID)
                End If

                If Not blLink.ActualDestination Is Nothing AndAlso blLink.ActualDestination.IsSensorOrMotor Then
                    Me.m_bSynchWithRobot = True
                End If
            End If

            MyBase.AfterAddLink(blLink)
        End Sub

        Protected Sub SetGainLimits()
            If Not m_gnGain Is Nothing AndAlso Not m_thSourceDataTypes Is Nothing AndAlso Not m_thSourceDataTypes.Value Is Nothing Then
                m_gnGain.UpperLimit = New ScaledNumber(m_gnGain, "UpperLimit", m_thSourceDataTypes.Value.UpperLimit, _
                                                       m_thSourceDataTypes.Value.UpperLimitscale, _
                                                       m_thSourceDataTypes.Value.Units, _
                                                       m_thSourceDataTypes.Value.UnitsAbbreviation)
                m_gnGain.LowerLimit = New ScaledNumber(m_gnGain, "LowerLimit", m_thSourceDataTypes.Value.LowerLimit, _
                                                       m_thSourceDataTypes.Value.LowerLimitscale, _
                                                       m_thSourceDataTypes.Value.Units, _
                                                       m_thSourceDataTypes.Value.UnitsAbbreviation)
            End If
        End Sub

        Public Overrides Sub BeforeCopy(ByVal arySelectedItems As ArrayList)

            Dim blLink As AnimatGUI.DataObjects.Behavior.Link
            For Each deEntry As DictionaryEntry In m_aryLinks
                blLink = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Behavior.Link)
                If Not blLink.Selected Then
                    blLink.SelectItem(True)
                End If
            Next

        End Sub

        'We do not want adapters to show up in the node tree view drop down.
        Public Overrides Sub CreateNodeTreeView(ByRef tvTree As Crownwood.DotNetMagic.Controls.TreeControl, ByVal aryNodes As Crownwood.DotNetMagic.Controls.NodeCollection)
        End Sub

        Public Overrides Sub CheckForErrors()
            MyBase.CheckForErrors()

            If Util.Application.ProjectErrors Is Nothing Then Return

            If m_thSourceDataTypes Is Nothing OrElse m_thSourceDataTypes.ID Is Nothing OrElse m_thSourceDataTypes.ID.Trim.Length = 0 Then
                If Not Util.Application.ProjectErrors.Errors.Contains(DiagramErrors.DataError.GenerateID(Me, DiagramError.enumErrorTypes.DataTypeNotSet)) Then
                    Dim deError As New DiagramErrors.DataError(Me, DiagramError.enumErrorLevel.Error, DiagramError.enumErrorTypes.DataTypeNotSet, _
                                                               "The adapter '" & Me.Text & "' does not have a defined data type pointer value.")
                    Util.Application.ProjectErrors.Errors.Add(deError.ID, deError)
                End If
            Else
                If Util.Application.ProjectErrors.Errors.Contains(DiagramErrors.DataError.GenerateID(Me, DiagramError.enumErrorTypes.DataTypeNotSet)) Then
                    Util.Application.ProjectErrors.Errors.Remove(DiagramErrors.DataError.GenerateID(Me, DiagramError.enumErrorTypes.DataTypeNotSet))
                End If
            End If

        End Sub

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Gain", GetType(AnimatGUI.DataObjects.Gain), "Gain", _
                                        "Adapter Properties", "Sets the gain that controls the input/output relationship " & _
                                        "between the two selected items.", m_gnGain, _
                                        GetType(AnimatGUI.TypeHelpers.GainTypeEditor), _
                                        GetType(AnimatGUI.TypeHelpers.GainTypeConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Source Data Type ID", GetType(AnimatGUI.TypeHelpers.DataTypeID), "SourceDataTypes", _
                                        "Adapter Properties", "Sets the type of data to use as an input from the source node into the gain function.", m_thSourceDataTypes, _
                                        GetType(AnimatGUI.TypeHelpers.DropDownListEditor), _
                                        GetType(AnimatGUI.TypeHelpers.DataTypeIDTypeConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Target Data Type ID", GetType(AnimatGUI.TypeHelpers.DataTypeID), "TargetDataTypes", _
                                        "Adapter Properties", "Sets the type of data to set on the target node from the gain function.", m_thTargetDataTypes, _
                                        GetType(AnimatGUI.TypeHelpers.DropDownListEditor), _
                                        GetType(AnimatGUI.TypeHelpers.DataTypeIDTypeConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Enabled", GetType(Boolean), "Enabled", _
                                        "Adapter Properties", "Determines if this adapter is enabled or not.", m_bEnabled))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Delay Buffer Mode", m_eDelayBufferMode.GetType(), "DelayBufferMode", _
                                        "Adapter Properties", "Determines if this adapter uses a delay buffer for its IO.", m_eDelayBufferMode))

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = m_snDelayBufferInterval.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Delay Buffer Interval", pbNumberBag.GetType(), "DelayBufferInterval", _
                                        "Adapter Properties", "Sets the time interval to use for a delay buffer if one is enabled. " & _
                                        "Acceptable values are in the range 0 to 5 s.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snInitIODisableDuration.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Init IO Disable Duration", pbNumberBag.GetType(), "InitIODisableDuration", _
                                        "Adapter Properties", "Sets the duration for how long this adapter is disabled at simulation startup. " & _
                                        "Acceptable values are in the range 0 to 5 s.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            'Only show this stuff if there is a robot interface defined for it.
            If Not Me.Organism.RobotInterface Is Nothing Then
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("SynchWithRobot", GetType(Boolean), "SynchWithRobot", _
                                            "Robot Properties", "Determines whether this adapter is synched with a robot part during simulation." & _
                                            "Note that this will only be applied if the synch with robot setting on the robot interface is true also.", m_bSynchWithRobot))

                pbNumberBag = m_snSynchUpdateInterval.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Synch Update Interval", pbNumberBag.GetType(), "SynchUpdateInterval", _
                                            "Robot Properties", "Sets how often this adapter is updated when simulating a robot. " & _
                                            "Acceptable values are in the range 0 to 5 s.", pbNumberBag, _
                                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

                pbNumberBag = m_snSynchUpdateStartInterval.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Synch Update Start Interval", pbNumberBag.GetType(), "SynchUpdateStartInterval", _
                                            "Robot Properties", "Sets the interval after the disable duration when this adapter is first updated when simulating a robot. " & _
                                            "Acceptable values are in the range 0 to 5 s.", pbNumberBag, _
                                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Robot IO Scale", GetType(Single), "RobotIOScale", _
                                            "Robot Properties", "If you are simulating a robot then use this to scale the IO output of this adpater in simulation mode to more closely match the robot output. " & _
                                            "For example, motors are usually a little slower than in the simulation, so you would scale down the IO here to match your real motor. " & _
                                            "This is a percentage value of scaling with 1 as 100%", m_fltRobotIOScale))
            End If

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            If Not m_gnGain Is Nothing Then m_gnGain.ClearIsDirty()
            If Not m_snDelayBufferInterval Is Nothing Then m_snDelayBufferInterval.ClearIsDirty()
            If Not m_snInitIODisableDuration Is Nothing Then m_snInitIODisableDuration.ClearIsDirty()
            If Not m_snSynchUpdateInterval Is Nothing Then m_snSynchUpdateInterval.ClearIsDirty()
            If Not m_snSynchUpdateStartInterval Is Nothing Then m_snSynchUpdateStartInterval.ClearIsDirty()
        End Sub

        Public Overrides Function Delete(Optional bAskToDelete As Boolean = True, Optional e As Crownwood.DotNetMagic.Controls.TGCloseRequestEventArgs = Nothing) As Boolean
            Return MyBase.Delete(bAskToDelete, e)
        End Function

        Public Overrides Sub InitializeSimulationReferences(Optional ByVal bShowError As Boolean = True)
            Try
                If m_doInterface Is Nothing AndAlso Not Util.Application.SimulationInterface Is Nothing AndAlso Util.Application.SimulationInterface.SimOpen Then
                    If Not Util.Application.SimulationInterface.FindItem(Me.ID, False) Then
                        AddToSim(True)
                    End If

                    m_doInterface = Util.Application.CreateDataObjectInterface(Me.ID)
                End If

                m_gnGain.InitializeSimulationReferences(bShowError)
            Catch ex As System.Exception
                If bShowError Then
                    AnimatGUI.Framework.Util.DisplayError(ex)
                Else
                    Throw ex
                End If
            End Try
        End Sub

        Public Overrides Function CanCopy(ByVal aryItems As ArrayList) As Boolean

            If m_bnOrigin Is Nothing OrElse m_bnDestination Is Nothing Then
                Return False
            End If

            'Check to make sure my inlinks are in the list
            For Each deItem As DictionaryEntry In Me.InLinks
                Dim blIn As Link = DirectCast(deItem.Value, Link)
                If Not Util.FindIDInList(aryItems, blIn.ActualOrigin.ID) Then
                    Return False
                End If
            Next

            'Check to make sure my outlinks are in the list
            For Each deItem As DictionaryEntry In Me.OutLinks
                Dim blOut As Link = DirectCast(deItem.Value, Link)
                If Not Util.FindIDInList(aryItems, blOut.ActualDestination.ID) Then
                    Return False
                End If
            Next

            Return True
        End Function

        Public Overrides Function CreateObjectListTreeView(ByVal doParent As Framework.DataObject, _
                                                       ByVal tnParentNode As Crownwood.DotNetMagic.Controls.Node, _
                                                       ByVal mgrImageList As AnimatGUI.Framework.ImageManager) As Crownwood.DotNetMagic.Controls.Node
            Dim tnNode As Crownwood.DotNetMagic.Controls.Node = MyBase.CreateObjectListTreeView(doParent, tnParentNode, mgrImageList)
            m_gnGain.CreateObjectListTreeView(Me, tnNode, mgrImageList)
            Return tnNode
        End Function

        Public Overrides Function FindObjectByID(ByVal strID As String) As Framework.DataObject

            Dim doObject As AnimatGUI.Framework.DataObject = MyBase.FindObjectByID(strID)
            If doObject Is Nothing AndAlso Not m_gnGain Is Nothing Then doObject = m_gnGain.FindObjectByID(strID)

            Return doObject

        End Function

#Region " Add-Remove to List Methods "

        Public Overrides Sub AddToSim(ByVal bThrowError As Boolean, Optional ByVal bDoNotInit As Boolean = False)
            If m_bIsInitialized AndAlso Not NeuralModule Is Nothing Then
                NeuralModule.VerifyExistsInSim()
                If Not Util.Application.SimulationInterface.FindItem(Me.ID, False) Then
                    'If we just created this neuralmodule in the sim then this object might already exist now. We should only add it if it does not exist.
                    Util.Application.SimulationInterface.AddItem(Me.NeuralModule.ID(), "Adapter", Me.ID, Me.GetSimulationXml("Adapter"), bThrowError, bDoNotInit)
                End If
                InitializeSimulationReferences()
            End If
        End Sub

        Public Overrides Sub RemoveFromSim(ByVal bThrowError As Boolean)
            If Not NeuralModule Is Nothing AndAlso Not m_doInterface Is Nothing Then
                Util.Application.SimulationInterface.RemoveItem(Me.NeuralModule.ID(), "Adapter", Me.ID, bThrowError)
            End If
            m_doInterface = Nothing
            m_gnGain.RemoveFromSim(True)
        End Sub

        Public Overrides Sub BeforeAddToList(ByVal bCallSimMethods As Boolean, ByVal bThrowError As Boolean)
            Me.SignalBeforeAddItem(Me)
            'Adapters do NOT call AddToSim when first added to the list.
            'If bCallSimMethods Then AddToSim(bThrowError)
        End Sub

        Public Overrides Sub AfterAddToList(ByVal bCallSimMethods As Boolean, ByVal bThrowError As Boolean)
            MyBase.AfterAddToList(bCallSimMethods, bThrowError)

            If Not NeuralModule Is Nothing Then
                NeuralModule.Nodes.Add(Me.ID, Me)
            End If
        End Sub

        Public Overrides Sub AfterRemoveFromList(ByVal bCallSimMethods As Boolean, ByVal bThrowError As Boolean)
            MyBase.AfterRemoveFromList(bCallSimMethods, bThrowError)

            If Not NeuralModule Is Nothing AndAlso NeuralModule.Nodes.Contains(Me.ID) Then
                NeuralModule.Nodes.Remove(Me.ID)
            End If
        End Sub

        Public Overridable Sub CreateAdapterSimReferences(Optional ByVal bThrowError As Boolean = True)


        End Sub


#End Region

        Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.LoadData(oXml)

            oXml.IntoElem()

            m_strOriginID = Util.LoadID(oXml, "Origin", True, "")
            m_strDestinationID = Util.LoadID(oXml, "Destination", True, "")
            m_strDataTypeID = Util.LoadID(oXml, "DataType", True, "")
            m_strTargetDataTypeID = Util.LoadID(oXml, "TargetDataType", True, "")
            m_bEnabled = oXml.GetChildBool("Enabled", True)
            m_eDelayBufferMode = DirectCast([Enum].Parse(GetType(enumDelayBufferMode), oXml.GetChildString("DelayBufferMode", "NoDelayBuffer"), True), enumDelayBufferMode)
            m_snDelayBufferInterval.LoadData(oXml, "DelayBufferInterval", False)
            m_snInitIODisableDuration.LoadData(oXml, "InitIODisableDuration", False)
            m_fltRobotIOScale = oXml.GetChildFloat("RobotIOScale", m_fltRobotIOScale)

            If oXml.FindChildElement("Gain", False) Then
                oXml.IntoChildElement("Gain")
                Dim strAssemblyFile As String = oXml.GetChildString("AssemblyFile")
                Dim strClassName As String = oXml.GetChildString("ClassName")
                oXml.OutOfElem()

                m_gnGain = DirectCast(Util.LoadClass(strAssemblyFile, strClassName, Me), AnimatGUI.DataObjects.Gain)
                m_gnGain.LoadData(oXml, "Gain", "Gain")
            End If

            m_bSynchWithRobot = oXml.GetChildBool("SynchWithRobot", m_bSynchWithRobot)
            m_snSynchUpdateInterval.LoadData(oXml, "SynchUpdateInterval", False)
            m_snSynchUpdateStartInterval.LoadData(oXml, "SynchUpdateStartInterval", False)

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub InitializeAfterLoad()

            If Me.ID.ToLower = "507b20a7-8084-4ea5-acad-cce3369a1485" Then
                Dim i As Integer = 5
            End If

            Try
                If m_bnOrigin Is Nothing Then
                    If m_strOriginID.Trim.Length > 0 Then
                        SetOrigin(Me.Organism.FindBehavioralNode(m_strOriginID), False)

                        If Not m_bnOrigin.IsInitialized Then
                            SetOrigin(Nothing, False)
                            m_bIsInitialized = False
                            Return
                        End If

                        m_thSourceDataTypes = DirectCast(m_bnOrigin.DataTypes.Clone(Me, False, Nothing), TypeHelpers.DataTypeID)

                        If Not m_thSourceDataTypes Is Nothing AndAlso m_strDataTypeID.Trim.Length > 0 AndAlso m_strDataTypeID.Trim.Length > 0 Then
                            If Me.m_thSourceDataTypes.DataTypes.Contains(m_strDataTypeID) Then
                                Me.m_thSourceDataTypes.ID = m_strDataTypeID
                            End If
                        End If
                    End If
                End If

                If m_bnDestination Is Nothing Then
                    If m_strDestinationID.Trim.Length > 0 Then
                        SetDestination(Me.Organism.FindBehavioralNode(m_strDestinationID), False)
                    End If
                End If

                If Not m_bnDestination Is Nothing AndAlso Not m_bnDestination.IncomingDataTypes Is Nothing Then
                    m_thTargetDataTypes = DirectCast(m_bnDestination.IncomingDataTypes.Clone(Me, False, Nothing), TypeHelpers.DataTypeID)

                    If Not m_thTargetDataTypes Is Nothing AndAlso m_strTargetDataTypeID.Trim.Length > 0 AndAlso m_strTargetDataTypeID.Trim.Length > 0 Then
                        If Me.m_thTargetDataTypes.DataTypes.Contains(m_strTargetDataTypeID) Then
                            Me.m_thTargetDataTypes.ID = m_strTargetDataTypeID
                        End If
                    End If
                End If

                If Not (Not m_bnDestination Is Nothing AndAlso m_bnDestination.IncomingDataTypes Is Nothing AndAlso m_bnDestination.IsInitialized) Then
                    If Me.m_thTargetDataTypes Is Nothing OrElse Me.m_thTargetDataTypes.ID.Trim.Length = 0 Then
                        m_bIsInitialized = False
                        Return
                    End If
                End If

                MyBase.InitializeAfterLoad()

                If Not m_gnGain Is Nothing Then
                    m_gnGain.InitializeAfterLoad()
                End If

            Catch ex As System.Exception
                m_bIsInitialized = False
            End Try

        End Sub

        Protected Overridable Sub SetOrigin(ByVal bnValue As Behavior.Node, ByVal bCallSimMethods As Boolean)
            Dim bnPrev As Behavior.Node = m_bnOrigin

            DisconnectOriginEvents()
            If bCallSimMethods Then RemoveFromSim(True)
            m_bnOrigin = bnValue

            If bCallSimMethods Then
                Try
                    AddToSim(True)
                Catch ex As Exception
                    m_bnOrigin = bnPrev
                    AddToSim(True)
                    ConnectOriginEvents()
                    Throw ex
                End Try
            End If

            ConnectOriginEvents()
        End Sub

        Protected Sub ConnectOriginEvents()

            DisconnectOriginEvents()

            If Not m_bnOrigin Is Nothing Then
                AddHandler m_bnOrigin.AfterPropertyChanged, AddressOf Me.OnOriginPropertyChanged
                AddHandler m_bnOrigin.ReloadSourceDataTypes, AddressOf Me.OnReloadSourceDataTypes
            End If
        End Sub

        Protected Sub DisconnectOriginEvents()
            If Not m_bnOrigin Is Nothing Then
                RemoveHandler m_bnOrigin.AfterPropertyChanged, AddressOf Me.OnOriginPropertyChanged
                RemoveHandler m_bnOrigin.ReloadSourceDataTypes, AddressOf Me.OnReloadSourceDataTypes
            End If
        End Sub

        Protected Overridable Sub SetDestination(ByVal bnValue As Behavior.Node, ByVal bCallSimMethods As Boolean)
            Dim bnPrev As Behavior.Node = m_bnDestination

            DisconnectDestinationEvents()
            If bCallSimMethods Then RemoveFromSim(True)
            m_bnDestination = bnValue

            If bCallSimMethods Then
                Try
                    AddToSim(True)
                Catch ex As Exception
                    m_bnDestination = bnPrev
                    AddToSim(True)
                    ConnectDestinationEvents()
                    Throw ex
                End Try
            End If

            ConnectDestinationEvents()
        End Sub

        Public Overrides Sub BeforeRemoveLink(ByVal blLink As Behavior.Link)
            If Not blLink Is Nothing Then
                DisconnectOriginEvents()
                DisconnectDestinationEvents()
            End If
        End Sub

        Protected Sub ConnectDestinationEvents()

            DisconnectDestinationEvents()

            If Not m_bnDestination Is Nothing Then
                AddHandler m_bnDestination.AfterPropertyChanged, AddressOf Me.OnDestinationPropertyChanged
                AddHandler m_bnDestination.ReloadTargetDataTypes, AddressOf Me.OnReloadTargetDataTypes
            End If
        End Sub

        Protected Sub DisconnectDestinationEvents()
            If Not m_bnDestination Is Nothing Then
                RemoveHandler m_bnDestination.AfterPropertyChanged, AddressOf Me.OnDestinationPropertyChanged
                RemoveHandler m_bnDestination.ReloadTargetDataTypes, AddressOf Me.OnReloadTargetDataTypes
            End If
        End Sub

        Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.SaveData(oXml)

            oXml.IntoElem() 'Into Node Element

            If Not m_bnOrigin Is Nothing Then
                oXml.AddChildElement("OriginID", m_bnOrigin.ID)
            Else
                Throw New System.Exception("The orgin node for an adapter has been lost!!")
            End If

            If Not m_bnDestination Is Nothing Then
                oXml.AddChildElement("DestinationID", m_bnDestination.ID)
            Else
                Throw New System.Exception("The destination node for an adapter has been lost!!")
            End If

            If Not m_thSourceDataTypes Is Nothing Then
                oXml.AddChildElement("DataTypeID", m_thSourceDataTypes.ID)
            End If

            If Not m_thTargetDataTypes Is Nothing Then
                oXml.AddChildElement("TargetDataTypeID", m_thTargetDataTypes.ID)
            End If

            If Not m_gnGain Is Nothing Then
                m_gnGain.SaveData(oXml, "Gain")
            End If

            oXml.AddChildElement("DelayBufferMode", m_eDelayBufferMode.ToString)
            m_snDelayBufferInterval.SaveData(oXml, "DelayBufferInterval")
            m_snInitIODisableDuration.SaveData(oXml, "InitIODisableDuration")

            oXml.AddChildElement("Enabled", m_bEnabled)
            oXml.AddChildElement("RobotIOScale", m_fltRobotIOScale)

            oXml.AddChildElement("SynchWithRobot", m_bSynchWithRobot)
            m_snSynchUpdateInterval.SaveData(oXml, "SynchUpdateInterval")
            m_snSynchUpdateStartInterval.SaveData(oXml, "SynchUpdateStartInterval")

            oXml.OutOfElem() ' Outof Node Element

        End Sub

#End Region

#End Region

#Region "Event Handlers"

        Protected Overridable Sub OnOriginPropertyChanged(ByVal doObject As AnimatGUI.Framework.DataObject, ByVal propInfo As System.Reflection.PropertyInfo)
            Try
                If Util.IsTypeOf(doObject.GetType, GetType(Behavior.Node), False) Then
                    Dim bnNode As Behavior.Node = DirectCast(doObject, Behavior.Node)
                    If bnNode.NeedToUpdateAdapterID(propInfo) Then
                        Me.SetSimData("OriginID", Me.GetSimulationXml("Adapter"), True)
                    End If
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub OnReloadSourceDataTypes()
            Try

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub OnReloadTargetDataTypes()
            Try
                If Not m_bnDestination Is Nothing AndAlso Not m_bnDestination.IncomingDataTypes Is Nothing Then
                    Dim strSelID As String = m_thTargetDataTypes.ID

                    m_thTargetDataTypes = DirectCast(m_bnDestination.IncomingDataTypes.Clone(Me, False, Nothing), TypeHelpers.DataTypeID)

                    If Not m_thTargetDataTypes Is Nothing AndAlso strSelID.Trim.Length > 0 Then
                        If Me.m_thTargetDataTypes.DataTypes.Contains(strSelID) Then
                            Me.m_thTargetDataTypes.ID = strSelID
                        End If
                    End If
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub OnDestinationPropertyChanged(ByVal doObject As AnimatGUI.Framework.DataObject, ByVal propInfo As System.Reflection.PropertyInfo)
            Try
                If Util.IsTypeOf(doObject.GetType, GetType(Behavior.Node), False) Then
                    Dim bnNode As Behavior.Node = DirectCast(doObject, Behavior.Node)
                    If bnNode.NeedToUpdateAdapterID(propInfo) Then
                        Me.SetSimData("DestinationID", Me.GetSimulationXml("Adapter"), True)
                    End If
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overrides Sub OnOriginModified(ByVal blLink As Link)
            Try
                If m_bIsInitialized AndAlso Not Util.IsTypeOf(blLink.Origin.GetType, GetType(Behavior.Nodes.Adapter), False) Then
                    SetOrigin(blLink.Origin, True)
                End If
            Catch ex As Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overrides Sub OnDestinationModified(ByVal blLink As Link)
            Try
                If m_bIsInitialized AndAlso Not Util.IsTypeOf(blLink.Destination.GetType, GetType(Behavior.Nodes.Adapter), False) Then
                    SetDestination(blLink.Destination, True)
                End If
            Catch ex As Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overrides Sub OnBeforeParentRemoveFromList(ByRef doObject As AnimatGUI.Framework.DataObject)
            Try
                DisconnectOriginEvents()
                DisconnectDestinationEvents()
                MyBase.OnBeforeParentRemoveFromList(doObject)
            Catch ex As Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace
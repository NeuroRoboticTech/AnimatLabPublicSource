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

Namespace DataObjects
    Namespace Robotics

        Public MustInherit Class RobotPartInterface
            Inherits DataObjects.DragObject

#Region " Attributes "

            Protected m_bpParentIO As RobotIOControl

            Protected m_thLinkedPart As AnimatGUI.TypeHelpers.LinkedDataObject
            Protected m_thLinkedProperty As AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList

            'Only used during loading
            Protected m_strLinkedPartID As String = ""
            Protected m_strLinkedObjectProperty As String = ""

            Protected m_doOrganism As Physical.Organism

            Protected m_doPruner As New AnimatGUI.TypeHelpers.RobotTreePruner(Me)

            Protected m_iIOComponentID As Integer = 1

            Protected m_gnGain As AnimatGUI.DataObjects.Gain

            Protected m_aryCompatiblePartTypes As New ArrayList

#End Region

#Region " Properties "

#Region "DragObject Properties"

            Public Overrides Property ItemName As String
                Get
                    Return Me.Name()
                End Get
                Set(value As String)
                    Me.Name = value
                End Set
            End Property

            Public Overrides ReadOnly Property CanBeCharted As Boolean
                Get
                    Return True
                End Get
            End Property

#End Region

            Public MustOverride ReadOnly Property PartType() As String

            Public Overridable Property ParentIOControl As RobotIOControl
                Get
                    Return m_bpParentIO
                End Get
                Set(value As RobotIOControl)
                    m_bpParentIO = value
                End Set
            End Property

            <Browsable(False)> _
            Public Overridable Property LinkedPart() As AnimatGUI.TypeHelpers.LinkedDataObject
                Get
                    Return m_thLinkedPart
                End Get
                Set(ByVal Value As AnimatGUI.TypeHelpers.LinkedDataObject)
                    Dim thPrevLinked As AnimatGUI.TypeHelpers.LinkedDataObject = m_thLinkedPart

                    DiconnectLinkedPartEvents()
                    m_thLinkedPart = Value
                    ConnectLinkedPartEvents()

                    If Not m_thLinkedPart Is Nothing AndAlso Not m_thLinkedPart.Item Is Nothing Then
                        SetSimData("LinkedPartID", m_thLinkedPart.Item.ID, True)
                    Else
                        SetSimData("LinkedPartID", "", True)
                    End If

                End Set
            End Property

            <Browsable(False)> _
            Public Overridable Property LinkedProperty() As AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList
                Get
                    Return m_thLinkedProperty
                End Get
                Set(ByVal Value As AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList)
                    m_thLinkedProperty = Value

                    If Not m_thLinkedProperty Is Nothing AndAlso Not m_thLinkedProperty.Item Is Nothing Then
                        SetSimData("PropertyName", m_thLinkedProperty.PropertyName, True)
                    End If

                End Set
            End Property

            <Browsable(False)> _
            Public Overridable Property LinkedPropertyName() As String
                Get
                    If Not m_thLinkedProperty Is Nothing AndAlso Not m_thLinkedProperty.PropertyName Is Nothing Then
                        Return m_thLinkedProperty.PropertyName
                    Else
                        Return ""
                    End If
                End Get
                Set(value As String)
                    If m_thLinkedPart Is Nothing OrElse m_thLinkedPart.Item Is Nothing Then
                        Throw New System.Exception("You cannot set the linked object property name until the linked object is set.")
                    End If

                    Me.LinkedProperty = New TypeHelpers.LinkedDataObjectPropertiesList(m_thLinkedPart.Item, value, False, True)
                End Set
            End Property

            Public Overridable Property Organism() As Physical.Organism
                Get
                    Return m_doOrganism
                End Get
                Set(value As Physical.Organism)
                    m_doOrganism = value

                    If Not m_doOrganism Is Nothing Then
                        Me.LinkedPart = CreatePartList(m_doOrganism, Nothing)
                    End If

                End Set
            End Property

            Public Overridable ReadOnly Property CompatiblePartTypes() As ArrayList
                Get
                    Return m_aryCompatiblePartTypes
                End Get
            End Property

            Public Overridable Property IOComponentID As Integer
                Get
                    Return m_iIOComponentID
                End Get
                Set(value As Integer)
                    If value < 0 Then
                        Throw New System.Exception("Invalid component ID specified. ID: " & value)
                    End If
                    SetSimData("IOComponentID", value.ToString(), True)
                    m_iIOComponentID = value
                End Set
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
                            m_gnGain.ParentData = Nothing
                            m_gnGain.GainPropertyName = "Gain"
                        End If
                    End If
                End Set
            End Property

#End Region

#Region " Methods "

            Public Sub New(ByVal doParent As Framework.DataObject)
                MyBase.New(doParent)

                If Not doParent Is Nothing AndAlso Util.IsTypeOf(doParent.GetType(), GetType(RobotIOControl), False) Then
                    m_bpParentIO = DirectCast(doParent, RobotIOControl)
                    Me.Organism = m_bpParentIO.Organism

                    AddHandler m_bpParentIO.BeforeRemoveItem, AddressOf Me.OnBeforeParentRemoveFromList
                End If

                m_thLinkedPart = CreatePartList(m_doOrganism, Nothing)
                m_thLinkedProperty = CreateLinkedPropertyList(Nothing)

                m_aryCompatiblePartTypes.Clear()
                m_aryCompatiblePartTypes.Add(GetType(AnimatGUI.Framework.DataObject))

                m_gnGain = New AnimatGUI.DataObjects.Gains.Polynomial(Me, "Gain", "Input Variable", "Output Variable", False, False)

                m_thDataTypes.DataTypes.Clear()
                m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("IOValue", "IO Value", "", "", 0, 1))
                m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("StepIODuration", "IO Duration", "Seconds", "s", 0, 1))
                m_thDataTypes.ID = "IOValue"

            End Sub

            Protected MustOverride Function CreateLinkedPropertyList(ByVal doItem As AnimatGUI.Framework.DataObject) As AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList
            Protected MustOverride Function CreateLinkedPropertyList(ByVal doItem As AnimatGUI.Framework.DataObject, ByVal strPropertyName As String) As AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList

            Public Overrides Sub ClearIsDirty()
                MyBase.ClearIsDirty()
                If Not m_thLinkedPart Is Nothing Then m_thLinkedPart.ClearIsDirty()
                If Not m_thLinkedProperty Is Nothing Then m_thLinkedProperty.ClearIsDirty()
                If Not m_gnGain Is Nothing Then m_gnGain.ClearIsDirty()
            End Sub

            Public Overrides Sub AddToReplaceIDList(ByVal aryReplaceIDList As ArrayList, ByVal arySelectedItems As ArrayList)
                MyBase.AddToReplaceIDList(aryReplaceIDList, arySelectedItems)

                m_gnGain.AddToReplaceIDList(aryReplaceIDList, arySelectedItems)
            End Sub

            Public Overridable Function IsCompatibleWithPartType(ByVal doPart As Framework.DataObject) As Boolean
                For Each tpType As System.Type In m_aryCompatiblePartTypes
                    If Util.IsTypeOf(doPart.GetType(), tpType, False) Then
                        Return True
                    End If
                Next

                Return False
            End Function

            Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                                ByVal doRoot As AnimatGUI.Framework.DataObject)
                MyBase.CloneInternal(doOriginal, bCutData, doRoot)

                Dim OrigNode As RobotPartInterface = DirectCast(doOriginal, RobotPartInterface)

                DiconnectLinkedPartEvents()
                m_thLinkedPart = DirectCast(OrigNode.m_thLinkedPart.Clone(Me, bCutData, doRoot), TypeHelpers.LinkedDataObject)
                m_thLinkedProperty = DirectCast(OrigNode.m_thLinkedProperty.Clone(Me, bCutData, doRoot), TypeHelpers.LinkedDataObjectPropertiesList)
                m_iIOComponentID = OrigNode.m_iIOComponentID
                m_gnGain = DirectCast(OrigNode.m_gnGain.Clone(Me, bCutData, doRoot), AnimatGUI.DataObjects.Gain)

            End Sub

            Protected Sub SetGainLimits()
                'If Not m_gnGain Is Nothing AndAlso Not m_thDataTypes Is Nothing AndAlso Not m_thDataTypes.Value Is Nothing Then
                '    m_gnGain.UpperLimit = New ScaledNumber(m_gnGain, "UpperLimit", m_thDataTypes.Value.UpperLimit, _
                '                                           m_thDataTypes.Value.UpperLimitscale, _
                '                                           m_thDataTypes.Value.Units, _
                '                                           m_thDataTypes.Value.UnitsAbbreviation)
                '    m_gnGain.LowerLimit = New ScaledNumber(m_gnGain, "LowerLimit", m_thDataTypes.Value.LowerLimit, _
                '                                           m_thDataTypes.Value.LowerLimitscale, _
                '                                           m_thDataTypes.Value.Units, _
                '                                           m_thDataTypes.Value.UnitsAbbreviation)
                'End If
            End Sub

            Public Overridable Sub CheckForErrors()

                'If Util.Application.ProjectErrors Is Nothing Then Return

                'If m_thDataTypes Is Nothing OrElse m_thDataTypes.ID Is Nothing OrElse m_thDataTypes.ID.Trim.Length = 0 Then
                '    If Not Util.Application.ProjectErrors.Errors.Contains(AnimatGUI.DataObjects.Behavior.DiagramErrors.DataError.GenerateID(Me, AnimatGUI.DataObjects.Behavior.DiagramError.enumErrorTypes.DataTypeNotSet)) Then
                '        Dim deError As New AnimatGUI.DataObjects.Behavior.DiagramErrors.DataError(Me, AnimatGUI.DataObjects.Behavior.DiagramError.enumErrorLevel.Error, AnimatGUI.DataObjects.Behavior.DiagramError.enumErrorTypes.DataTypeNotSet, _
                '                                                   "The adapter '" & Me.Name & "' does not have a defined data type pointer value.")
                '        Util.Application.ProjectErrors.Errors.Add(deError.ID, deError)
                '    End If
                'Else
                '    If Util.Application.ProjectErrors.Errors.Contains(AnimatGUI.DataObjects.Behavior.DiagramErrors.DataError.GenerateID(Me, AnimatGUI.DataObjects.Behavior.DiagramError.enumErrorTypes.DataTypeNotSet)) Then
                '        Util.Application.ProjectErrors.Errors.Remove(AnimatGUI.DataObjects.Behavior.DiagramErrors.DataError.GenerateID(Me, AnimatGUI.DataObjects.Behavior.DiagramError.enumErrorTypes.DataTypeNotSet))
                '    End If
                'End If

            End Sub

            Public Overrides Function FindDragObject(strStructureName As String, strDataItemID As String, Optional bThrowError As Boolean = True) As DragObject
                Throw New System.Exception("FindDragObject not implemented")
            End Function

#Region " DataObject Methods "

            Public Overrides Function WorkspaceTreeviewPopupMenu(ByRef tnSelectedNode As Crownwood.DotNetMagic.Controls.Node, ByVal ptPoint As System.Drawing.Point) As Boolean

                If tnSelectedNode Is m_tnWorkspaceNode Then
                    Dim mcDelete As New System.Windows.Forms.ToolStripMenuItem("Delete robot part interface", Util.Application.ToolStripImages.GetImage("AnimatGUI.Delete.gif"), New EventHandler(AddressOf Util.Application.OnDeleteFromWorkspace))

                    ' Create the popup menu object
                    Dim popup As New AnimatContextMenuStrip("AnimatGUI.DataObjects.Robotics.RobotPartInterface.WorkspaceTreeviewPopupMenu", Util.SecurityMgr)
                    popup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {mcDelete})

                    If Me.CanBeCharted AndAlso Not Util.Application.LastSelectedChart Is Nothing AndAlso Not Util.Application.LastSelectedChart.LastSelectedAxis Is Nothing Then
                        ' Create the menu items
                        Dim mcAddToChart As New System.Windows.Forms.ToolStripMenuItem("Add to Chart", Util.Application.ToolStripImages.GetImage("AnimatGUI.AddChartItem.gif"), New EventHandler(AddressOf Util.Application.OnAddToChart))
                        popup.Items.Add(mcAddToChart)
                    End If

                    Util.ProjectWorkspace.ctrlTreeView.ContextMenuNode = popup

                    Return True
                End If

                Return False
            End Function

            Protected Overridable Sub ConnectLinkedPartEvents()
                DiconnectLinkedPartEvents()

                If Not m_thLinkedPart Is Nothing AndAlso Not m_thLinkedPart.Item Is Nothing Then
                    AddHandler m_thLinkedPart.Item.AfterRemoveItem, AddressOf Me.OnAfterRemoveLinkedPart

                    m_thLinkedProperty = CreateLinkedPropertyList(m_thLinkedPart.Item)
                End If
            End Sub

            Protected Overridable Sub DiconnectLinkedPartEvents()
                m_thLinkedProperty = CreateLinkedPropertyList(Nothing)

                If Not m_thLinkedPart Is Nothing AndAlso Not m_thLinkedPart.Item Is Nothing Then
                    RemoveHandler m_thLinkedPart.Item.AfterRemoveItem, AddressOf Me.OnAfterRemoveLinkedPart
                End If
            End Sub

            Protected Overridable Overloads Function CreatePartList(ByVal doStruct As Physical.PhysicalStructure, ByVal doItem As Framework.DataObject) As TypeHelpers.LinkedDataObject
                Return New AnimatGUI.TypeHelpers.LinkedDataObjectTree(doItem, m_doPruner)
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

            Public Overrides Sub InitializeSimulationReferences(Optional ByVal bShowError As Boolean = True)
                Try
                    MyBase.InitializeSimulationReferences(bShowError)
                    'm_gnGain.InitializeSimulationReferences(bShowError)
                Catch ex As System.Exception
                    If bShowError Then
                        AnimatGUI.Framework.Util.DisplayError(ex)
                    Else
                        Throw ex
                    End If
                End Try
            End Sub

#Region " Add-Remove to List Methods "

            Public Overrides Sub AddToSim(ByVal bThrowError As Boolean, Optional ByVal bDoNotInit As Boolean = False)
                If Not Me.Parent Is Nothing Then
                    Util.Application.SimulationInterface.AddItem(Me.Parent.ID, "RobotPartInterface", Me.ID, Me.GetSimulationXml("RobotPartInterface"), bThrowError, bDoNotInit)
                    InitializeSimulationReferences()
                End If
            End Sub

            Public Overrides Sub RemoveFromSim(ByVal bThrowError As Boolean)
                If Not Me.Parent Is Nothing AndAlso Not m_doInterface Is Nothing Then
                    Util.Application.SimulationInterface.RemoveItem(Me.Parent.ID, "RobotPartInterface", Me.ID, bThrowError)
                End If
                m_doInterface = Nothing
                m_gnGain.RemoveFromSim(True)
            End Sub

            Public Overrides Sub AfterRemoveFromList(ByVal bCallSimMethods As Boolean, ByVal bThrowError As Boolean)
                MyBase.AfterRemoveFromList(bCallSimMethods, bThrowError)
                DiconnectLinkedPartEvents()
            End Sub

#End Region

            Public Overrides Function Delete(Optional ByVal bAskToDelete As Boolean = True, Optional ByVal e As Crownwood.DotNetMagic.Controls.TGCloseRequestEventArgs = Nothing) As Boolean
                Try
                    If bAskToDelete AndAlso Util.ShowMessage("Are you sure you want to remove the robot part interface?", _
                                                             "Remove Robot Interface", MessageBoxButtons.YesNo) <> DialogResult.Yes Then
                        Return False
                    End If

                    Util.Application.AppIsBusy = True

                    Me.DiconnectLinkedPartEvents()
                    Me.ParentIOControl.Parts.Remove(Me.ID)
                    If Not Me.Parent Is Nothing Then Me.Parent.IsDirty = True

                    Me.RemoveWorksapceTreeView()
                    Return True
                Catch ex As Exception
                    Throw ex
                Finally
                    Util.Application.AppIsBusy = False
                End Try
            End Function

            Protected Overridable Function GetLinkedPartDropDownTreeType() As System.Type
                Return GetType(AnimatGUI.TypeHelpers.DropDownTreeEditor)
            End Function

            Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Name", Me.ID.GetType(), "Name", _
                                            "Properties", "Name", Me.Name))

                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("ID", Me.ID.GetType(), "ID", _
                                            "Properties", "ID", Me.ID, True))

                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Enabled", GetType(Boolean), "Enabled", _
                                            "Properties", "Determines if this controller is enabled or not.", m_bEnabled))

                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Linked Part", m_thLinkedPart.GetType, "LinkedPart", _
                                            "Properties", "Associates this robot interface to an ID of a part that exists within the body of the organism.", m_thLinkedPart, _
                                            GetLinkedPartDropDownTreeType(), _
                                            GetType(AnimatGUI.TypeHelpers.LinkedDataObjectTypeConverter)))

                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Linked Property", GetType(AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList), "LinkedProperty", _
                                            "Properties", "Determines the property that is set by this controller.", m_thLinkedProperty, _
                                            GetType(AnimatGUI.TypeHelpers.DropDownListEditor), _
                                            GetType(AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesTypeConverter)))

                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Gain", GetType(AnimatGUI.DataObjects.Gain), "Gain", _
                                            "Properties", "Sets the gain that controls the input/output relationship " & _
                                            "between the two selected items.", m_gnGain, _
                                            GetType(AnimatGUI.TypeHelpers.GainTypeEditor), _
                                            GetType(AnimatGUI.TypeHelpers.GainTypeConverter)))

            End Sub

            Public Overrides Sub InitializeAfterLoad()

                Try
                    MyBase.InitializeAfterLoad()

                    If m_bIsInitialized Then
                        Dim doPart As AnimatGUI.Framework.DataObject
                        If (Not Me.LinkedPart Is Nothing AndAlso Me.LinkedPart.Item Is Nothing) AndAlso (m_strLinkedPartID.Length > 0) Then
                            doPart = Util.Simulation.FindObjectByID(m_strLinkedPartID)

                            Me.LinkedPart = CreatePartList(m_doOrganism, doPart)

                            If m_strLinkedObjectProperty.Trim.Length > 0 AndAlso Not m_thLinkedPart Is Nothing AndAlso Not m_thLinkedPart.Item Is Nothing Then
                                m_thLinkedProperty = CreateLinkedPropertyList(m_thLinkedPart.Item, m_strLinkedObjectProperty)
                            End If

                        End If

                        If Not m_gnGain Is Nothing Then
                            m_gnGain.InitializeAfterLoad()
                        End If
                    End If

                Catch ex As System.Exception
                    m_bIsInitialized = False
                End Try
            End Sub

            Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

                oXml.IntoElem()  'Into RobotPartInterface Element

                m_strName = oXml.GetChildString("Name", Me.Name)
                m_strID = oXml.GetChildString("ID", Me.ID)
                m_bEnabled = oXml.GetChildBool("Enabled", m_bEnabled)
                m_strLinkedPartID = Util.LoadID(oXml, "LinkedPart", True, "") 'Note: The ID of the name is added in the LoadID method.
                m_strLinkedObjectProperty = oXml.GetChildString("PropertyName", "")
                m_iIOComponentID = oXml.GetChildInt("IOComponentID", m_iIOComponentID)

                If oXml.FindChildElement("Gain", False) Then
                    oXml.IntoChildElement("Gain")
                    Dim strAssemblyFile As String = oXml.GetChildString("AssemblyFile")
                    Dim strClassName As String = oXml.GetChildString("ClassName")
                    oXml.OutOfElem()

                    m_gnGain = DirectCast(Util.LoadClass(strAssemblyFile, strClassName, Me), AnimatGUI.DataObjects.Gain)
                    m_gnGain.LoadData(oXml, "Gain", "Gain")
                End If

                oXml.OutOfElem()

            End Sub

            Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

                oXml.AddChildElement("RobotPartInterface")
                oXml.IntoElem()

                oXml.AddChildElement("Name", Me.Name)
                oXml.AddChildElement("ID", Me.ID)
                oXml.AddChildElement("AssemblyFile", Me.AssemblyFile)
                oXml.AddChildElement("ClassName", Me.ClassName)
                oXml.AddChildElement("Enabled", m_bEnabled)
                oXml.AddChildElement("IOComponentID", m_iIOComponentID)

                If Not m_thLinkedPart Is Nothing AndAlso Not m_thLinkedPart.Item Is Nothing Then
                    oXml.AddChildElement("LinkedPartID", m_thLinkedPart.Item.ID)

                    If Not m_thLinkedProperty Is Nothing Then
                        oXml.AddChildElement("PropertyName", Me.LinkedPropertyName)
                    End If
                End If

                If Not m_gnGain Is Nothing Then
                    m_gnGain.SaveData(oXml, "Gain")
                End If

                oXml.OutOfElem()

            End Sub

            Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")

                oXml.AddChildElement("RobotPartInterface")
                oXml.IntoElem()

                oXml.AddChildElement("Name", Me.Name)
                oXml.AddChildElement("ID", Me.ID)
                oXml.AddChildElement("Type", Me.PartType)
                oXml.AddChildElement("ModuleName", Me.ModuleFilename)
                oXml.AddChildElement("IOComponentID", m_iIOComponentID)
                oXml.AddChildElement("Enabled", m_bEnabled)

                If Not m_thLinkedPart Is Nothing AndAlso Not m_thLinkedPart.Item Is Nothing Then
                    oXml.AddChildElement("LinkedPartID", m_thLinkedPart.Item.ID)

                    If Not m_thLinkedProperty Is Nothing Then
                        oXml.AddChildElement("PropertyName", Me.LinkedPropertyName)
                    End If
                End If

                If Not m_gnGain Is Nothing Then
                    m_gnGain.SaveSimulationXml(oXml, Me, "Gain")
                End If

                oXml.OutOfElem()

            End Sub

#End Region

#End Region

#Region "Events"

            Private Sub OnAfterRemoveLinkedPart(ByRef doObject As Framework.DataObject)
                Try
                    Me.LinkedPart = CreatePartList(m_doOrganism, Nothing)
                Catch ex As Exception
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End Try
            End Sub

            Protected Overrides Sub OnBeforeParentRemoveFromList(ByRef doObject As AnimatGUI.Framework.DataObject)
                Try
                    DiconnectLinkedPartEvents()
                    MyBase.OnBeforeParentRemoveFromList(doObject)
                Catch ex As Exception
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End Try
            End Sub

#End Region

        End Class


    End Namespace
End Namespace

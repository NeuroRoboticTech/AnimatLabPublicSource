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

Namespace DataObjects.Charting

    Public Class DataColumn
        Inherits Framework.DataObject

#Region " Attributes "

        'The dataitem module and class names are for the class that was dragged. 
        'For instance it is a FastNeuralNet.DataObjects.Neurons.Neuron class. But the
        'the ColumnModuleName and class type is the data needed to create the appropriate
        'class inside the c++ simulation.
        Protected m_strDataItemAssemblyFile As String = ""
        Protected m_strDataItemClassName As String = ""
        Protected m_strStructureID As String = ""
        Protected m_strDataItemID As String = ""
        Protected m_strDataItemName As String = ""
        Protected m_strDataItemImageName As String = ""
        Protected m_imgDataItemImage As Image
        Protected m_imgDataItemDragImage As Image
        Protected m_thDataType As AnimatGUI.TypeHelpers.DataTypeID
        Protected m_iDataSubSet As Integer = -1
        Protected m_iPrevDataSubset As Integer = -1
        Protected m_strColumnModuleName As String
        Protected m_strColumnClassType As String
        Protected m_bUseIncomingDataType As Boolean = False
        Protected m_strSelectedDataTypeID As String = ""
        Protected m_iColumnIndex As Integer = 0
        Protected m_doItem As DragObject

        Protected m_frmParentAxis As AnimatGUI.DataObjects.Charting.Axis

        Protected m_bAutoAddToAxis As Boolean = True

        Protected m_bSelectionInProgress As Boolean = False

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Overridable Property DataItem() As AnimatGUI.DataObjects.DragObject
            Get
                Return m_doItem
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.DragObject)
                If Not Value Is Nothing Then
                    Me.DataItemAssemblyFile = Value.AssemblyFile
                    Me.DataItemClassName = Value.ClassName
                    Me.StructureID = Value.StructureID
                    Me.DataItemID = Value.ID
                    Me.DataItemName = Value.ItemName
                    Me.DataItemImageName = Value.WorkspaceImageName
                    Me.DataItemImage = DirectCast(Value.WorkspaceImage.Clone(), System.Drawing.Image)
                    Me.DataItemDragImage = DirectCast(Value.DragImage.Clone(), System.Drawing.Image)
                    m_strColumnModuleName = Value.DataColumnModuleName
                    m_strColumnClassType = Value.DataColumnClassType

                    'By default we should use the normal Datatypes property from the selected object, but in 
                    'some instances we might want to limit this choice to the incoming data type for the selected object
                    If Not m_bUseIncomingDataType Then
                        m_thDataType = DirectCast(Value.DataTypes.Clone(Me, False, Nothing), AnimatGUI.TypeHelpers.DataTypeID)

                        If m_thDataType.DataTypes.Contains(m_strSelectedDataTypeID) Then
                            m_thDataType.ID = m_strSelectedDataTypeID
                        End If
                    Else
                        m_thDataType = New TypeHelpers.DataTypeID(Me)
                        If Not Value.IncomingDataType Is Nothing Then
                            m_thDataType.DataTypes.Add(DirectCast(Value.IncomingDataType.Clone(Value, False, Nothing), DataObjects.DataType))
                            m_thDataType.ID = Value.IncomingDataType.ID
                        End If
                    End If

                    DisconnectItemEvents()
                    m_doItem = Value
                    ConnectItemEvents()
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides Property Name() As String
            Get
                Return m_strName
            End Get
            Set(ByVal Value As String)
                If Value.Trim.Length <= 0 Then
                    Throw New System.Exception("The name of the data column can not be blank.")
                End If
                SetSimData("Name", Value, True)

                If m_bAutoAddToAxis Then
                    m_strName = Value

                    If Not m_doParent Is Nothing AndAlso Not m_doParent.WorkspaceNode Is Nothing Then
                        Me.RemoveWorksapceTreeView()
                        Me.CreateWorkspaceTreeView(m_doParent, m_doParent.WorkspaceNode)
                    End If
                Else
                    m_strName = Value
                End If

            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property StructureID() As String
            Get
                Return m_strStructureID
            End Get
            Set(ByVal Value As String)
                m_strStructureID = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property DataItemAssemblyFile() As String
            Get
                Return m_strDataItemAssemblyFile
            End Get
            Set(ByVal Value As String)
                m_strDataItemAssemblyFile = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property DataItemClassName() As String
            Get
                Return m_strDataItemClassName
            End Get
            Set(ByVal Value As String)
                m_strDataItemClassName = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property DataItemID() As String
            Get
                Return m_strDataItemID
            End Get
            Set(ByVal Value As String)
                m_strDataItemID = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property DataItemName() As String
            Get
                Return m_strDataItemName
            End Get
            Set(ByVal Value As String)
                m_strDataItemName = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property DataItemImageName() As String
            Get
                Return m_strDataItemImageName
            End Get
            Set(ByVal Value As String)
                m_strDataItemImageName = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property DataItemImage() As Image
            Get
                Return m_imgDataItemImage
            End Get
            Set(ByVal Value As Image)
                m_imgDataItemImage = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property DataItemDragImage() As Image
            Get
                Return m_imgDataItemDragImage
            End Get
            Set(ByVal Value As Image)
                m_imgDataItemDragImage = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property DataType() As AnimatGUI.TypeHelpers.DataTypeID
            Get
                Return m_thDataType
            End Get
            Set(ByVal Value As AnimatGUI.TypeHelpers.DataTypeID)
                If Not Value Is Nothing Then
                    'DataType is used in a number of sitations. For instance, it is used to create the list of possible selections
                    'for the user dialog box, not just for data columns in the simulation. Therefore, in this instance we need to check
                    'first whether this item already exists in the simulation. If it does then set the data type. If it does not then we
                    'assume this data column object is being used for some other purpose in the editor and ignore this in the simulation.
                    If Util.Application.SimulationInterface.FindItem(Me.ID, False) Then
                        SetSimData("DataType", Value.ID, True)
                    End If

                    m_thDataType.ID = Value.ID
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property DataTypeID() As String
            Get
                If Not m_thDataType Is Nothing Then
                    Return m_thDataType.ID
                End If

                Return ""
            End Get
            Set(ByVal Value As String)
                'DataType is used in a number of sitations. For instance, it is used to create the list of possible selections
                'for the user dialog box, not just for data columns in the simulation. Therefore, in this instance we need to check
                'first whether this item already exists in the simulation. If it does then set the data type. If it does not then we
                'assume this data column object is being used for some other purpose in the editor and ignore this in the simulation.
                If Util.Application.SimulationInterface.FindItem(Me.ID, False) Then
                    SetSimData("DataType", Value, True)
                End If

                m_thDataType.ID = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property ParentAxis() As AnimatGUI.DataObjects.Charting.Axis
            Get
                Return m_frmParentAxis
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Charting.Axis)
                m_frmParentAxis = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property DataSubSet() As Integer
            Get
                Return m_iDataSubSet
            End Get
            Set(ByVal Value As Integer)
                If Value < 0 Then
                    Throw New System.Exception("The data subset value must not be less than zero.")
                End If
                SetSimData("ColumnIndex", Value.ToString, True)

                m_iPrevDataSubset = m_iDataSubSet
                m_iDataSubSet = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property PrevDataSubSet() As Integer
            Get
                Return m_iPrevDataSubset
            End Get
            Set(ByVal Value As Integer)
                m_iPrevDataSubset = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property ColumnModuleName() As String
            Get
                Return m_strColumnModuleName
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property ColumnClassType() As String
            Get
                Return m_strColumnClassType
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property AutoAddToAxis() As Boolean
            Get
                Return m_bAutoAddToAxis
            End Get
            Set(ByVal Value As Boolean)
                m_bAutoAddToAxis = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property LineColor() As System.Drawing.Color
            Get
                Return System.Drawing.Color.Red
            End Get
            Set(ByVal Value As System.Drawing.Color)

            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property UseIncomingDataType() As Boolean
            Get
                Return m_bUseIncomingDataType
            End Get
            Set(ByVal Value As Boolean)
                m_bUseIncomingDataType = Value
            End Set
        End Property

        'This property is set when the select data item dialog is shown.
        'It keeps datacolumn properties from trying to update the chart
        'while users are simply trying to configure the item they are selecting.
        <Browsable(False)> _
        Public Overridable Property SelectionInProgress() As Boolean
            Get
                Return m_bSelectionInProgress
            End Get
            Set(ByVal Value As Boolean)
                m_bSelectionInProgress = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property RequiresAutoDataCollectInterval() As Boolean
            Get
                'If m_strDataItemAssemblyFile.Trim.Length = 0 OrElse m_strDataItemClassName.Trim.Length = 0 Then
                '    Return False
                'End If

                Dim oItem As Object = Util.Simulation.FindObjectByID(m_strDataItemID)
                Dim doItem As DataObjects.DragObject = Nothing
                If Not oItem Is Nothing AndAlso Util.IsTypeOf(oItem.GetType, GetType(DataObjects.DragObject)) Then
                    doItem = DirectCast(oItem, DataObjects.DragObject)
                    Return doItem.RequiresAutoDataCollectInterval
                Else
                    Throw New System.Exception("Could not find object with ID '" & m_strDataItemID & "'")
                End If
            '    Dim doItem As DataObjects.DragObject = DirectCast(
            '    'Dim doBase As DataObjects.DragObject = DirectCast(Util.LoadClass(m_strDataItemAssemblyFile, m_strDataItemClassName, Me), DataObjects.DragObject)
            '    'Dim oItem As DataObjects.DragObject = DirectCast(doBase.FindDragObject(m_strStructureID, m_strDataItemID, False), DataObjects.DragObject)

            '    If Not oItem Is Nothing Then
            '    Else
            '        Return False
            '    End If
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return Me.DataItemImageName
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            If TypeOf doParent Is Axis Then
                m_frmParentAxis = DirectCast(doParent, AnimatGUI.DataObjects.Charting.Axis)
            End If
        End Sub

        Public Sub AddtoAxis(ByVal doAxis As DataObjects.Charting.Axis)
            m_frmParentAxis = doAxis
            Dim strName As String = Me.Name
            Me.Name = strName

            'Setting the name automatically adds it into the list of data columns.
            Me.SelectItem()
        End Sub

        Public Sub RemoveFromAxis()
            m_frmParentAxis.DataColumns.Remove(Me.ID)

            Me.RemoveWorksapceTreeView()

            'If we are automatically setting the collect data interval then lets recalculate the value
            If Not m_frmParentAxis Is Nothing AndAlso Not m_frmParentAxis.ParentChart Is Nothing AndAlso m_frmParentAxis.ParentChart.AutoCollectDataInterval Then
                m_frmParentAxis.ParentChart.ResetCollectDataInterval()
            End If

        End Sub

        Public Overridable Sub UpdateChartConfiguration(ByRef iSubSet As Integer)
        End Sub

        Public Overridable Sub ReconfigureChartData(ByRef aryOldX(,) As Single, ByRef aryOldY(,) As Single, ByRef aryNewX(,) As Single, ByRef aryNewY(,) As Single)
        End Sub

        Public Overridable Sub PrepareForCharting()
            'Me.ParentAxis.AddColumnToSimulation(Me, True)
        End Sub

        Protected Overridable Function FindDataItem(ByVal strID As String) As DragObject
            Dim doObj As Framework.DataObject = Util.Simulation.FindObjectByID(strID)
            If Not doObj Is Nothing AndAlso Util.IsTypeOf(doObj.GetType, GetType(DragObject)) Then
                Return DirectCast(doObj, DragObject)
            Else
                Return Nothing
            End If
        End Function

        Protected Overridable Sub ConnectItemEvents()
            DisconnectItemEvents()

            If Not m_doItem Is Nothing Then
                AddHandler m_doItem.AfterRemoveItem, AddressOf Me.OnAfterRemoveItem
            End If
        End Sub

        Protected Overridable Sub DisconnectItemEvents()
            If Not m_doItem Is Nothing Then
                RemoveHandler m_doItem.AfterRemoveItem, AddressOf Me.OnAfterRemoveItem
            End If
        End Sub

        Public Overridable Function IsValidColumn() As Boolean

            Try
                Dim oItem As Object = Util.Simulation.FindObjectByID(m_strDataItemID)
                Dim doItem As DataObjects.DragObject = Nothing
                If Not oItem Is Nothing AndAlso Util.IsTypeOf(oItem.GetType, GetType(DataObjects.DragObject)) Then
                    doItem = DirectCast(oItem, DataObjects.DragObject)
                    Return True
                Else
                    Throw New System.Exception("Could not find object with ID '" & m_strDataItemID & "'")
                End If

                'If m_strDataItemAssemblyFile.Trim.Length = 0 OrElse m_strDataItemClassName.Trim.Length = 0 Then
                '    Return False
                'End If

                'Dim doBase As DataObjects.DragObject = DirectCast(Util.LoadClass(m_strDataItemAssemblyFile, m_strDataItemClassName, Me), DataObjects.DragObject)
                'Dim oItem As Object = doBase.FindDragObject(m_strStructureID, m_strDataItemID, False)

                'If Not oItem Is Nothing Then
                '    Return True
                'Else
                '    Return False
                'End If

            Catch ex As Exception
                'If we had an error in herer for any reason then it is not a valid column.
                Debug.WriteLine("execption caught in IsValidColumn: " & ex.Message)
                Return False
            End Try
        End Function

        Public Overridable Function SaveDataColumnToXml() As String
            Dim oXml As ManagedAnimatInterfaces.IStdXml = Util.Application.CreateStdXml()

            oXml.AddElement("Data")
            oXml.AddChildElement("DataColumn")
            SaveDataColumnXml(oXml)

            Return oXml.Serialize()
        End Function

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            oXml.AddChildElement("DataColumn")
            SaveDataColumnXml(oXml)

        End Sub

        Public Overridable Sub SaveDataColumnXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            Dim doItem As AnimatGUI.DataObjects.DragObject = Me.DataItem

            If doItem Is Nothing Then
                Throw New System.Exception("No data item was defined that could be used to create a data column in the simulation.")
            End If

            oXml.IntoElem()
            oXml.AddChildElement("ModuleName", m_strColumnModuleName)
            oXml.AddChildElement("Type", m_strColumnClassType)
            oXml.AddChildElement("ID", m_strID)
            oXml.AddChildElement("ColumnName", m_strName)
            oXml.AddChildElement("DataType", m_thDataType.ID.ToString())
            'oXml.AddChildElement("Column", m_iColumnIndex)
            oXml.OutOfElem()

            doItem.SaveDataColumnToXml(oXml)
        End Sub

        Public Overridable Function CreateDataColumn(ByVal doItem As AnimatGUI.DataObjects.DragObject, Optional ByVal bAutoAddToAxis As Boolean = True) As AnimatGUI.DataObjects.Charting.DataColumn
            Dim doColumn As New DataObjects.Charting.DataColumn(Me.ParentAxis)

            Dim strName As String = doItem.ItemName

            doColumn.UseIncomingDataType = Me.UseIncomingDataType
            doColumn.DataItem = doItem.DataColumnItem
            doColumn.AutoAddToAxis = bAutoAddToAxis
            doColumn.Name = strName
            Return doColumn
        End Function

#Region " TreeView Methods "

        Public Overrides Function WorkspaceTreeviewPopupMenu(ByRef tnSelectedNode As Crownwood.DotNetMagic.Controls.Node, ByVal ptPoint As System.Drawing.Point) As Boolean

            If tnSelectedNode Is m_tnWorkspaceNode Then
                Dim mcDelete As New System.Windows.Forms.ToolStripMenuItem("Delete Column", Util.Application.ToolStripImages.GetImage("AnimatGUI.Delete.gif"), New EventHandler(AddressOf Util.Application.OnDeleteFromWorkspace))

                ' Create the popup menu object
                Dim popup As New AnimatContextMenuStrip("AnimatGUI.DataObjects.Charting.DataColumn.WorkspaceTreeviewPopupMenu", Util.SecurityMgr)
                popup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {mcDelete})

                Util.ProjectWorkspace.ctrlTreeView.ContextMenuNode = popup

                Return True
            End If

            Return False
        End Function

#End Region

#Region " Add-Remove to List Methods "

        Public Overrides Sub AddToSim(ByVal bThrowError As Boolean, Optional ByVal bDoNotInit As Boolean = False)
            If Not Me.ParentAxis Is Nothing AndAlso Not Me.ParentAxis.ParentChart Is Nothing Then
                Util.Application.SimulationInterface.AddItem(Me.ParentAxis.ParentChart.ID, "DataColumn", Me.ID, Me.GetSimulationXml("DataColumn"), bThrowError, bDoNotInit)
                InitializeSimulationReferences()
            End If
        End Sub

        Public Overrides Sub RemoveFromSim(ByVal bThrowError As Boolean)
            If Not Me.ParentAxis Is Nothing AndAlso Not Me.ParentAxis.ParentChart Is Nothing AndAlso Not m_doInterface Is Nothing Then
                Util.Application.SimulationInterface.RemoveItem(Me.ParentAxis.ParentChart.ID, "DataColumn", Me.ID, bThrowError)
            End If
            m_doInterface = Nothing
        End Sub

#End Region

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("ID", Me.ID.GetType(), "ID", _
                                        "Column Properties", "ID", Me.ID, True))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Name", GetType(String), "Name", _
                                        "Column Properties", "The name of this column.", Name()))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Item Name", GetType(String), "DataItemName", _
                                        "Column Properties", "The data item of this column.", DataItemName(), True))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Data Type", GetType(AnimatGUI.TypeHelpers.DataTypeID), "DataType", _
                                            "Column Properties", "Sets the type of data to chart.", Me.DataType, _
                                            GetType(AnimatGUI.TypeHelpers.DropDownListEditor), _
                                            GetType(AnimatGUI.TypeHelpers.DataTypeIDTypeConverter)))

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim doItem As New DataColumn(doParent)
            doItem.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then doItem.AfterClone(Me, bCutData, doRoot, doItem)
            Return doItem
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doOrigColumn As DataColumn = DirectCast(doOriginal, DataColumn)

            m_strName = doOrigColumn.m_strName
            m_strDataItemAssemblyFile = doOrigColumn.m_strDataItemAssemblyFile
            m_strDataItemClassName = doOrigColumn.m_strDataItemClassName
            m_strStructureID = doOrigColumn.m_strStructureID
            m_strDataItemID = doOrigColumn.m_strDataItemID
            m_strDataItemName = doOrigColumn.m_strDataItemName
            m_strDataItemImageName = doOrigColumn.m_strDataItemImageName
            m_bUseIncomingDataType = doOrigColumn.m_bUseIncomingDataType

            If Not doOrigColumn.m_imgDataItemImage Is Nothing Then
                m_imgDataItemImage = DirectCast(doOrigColumn.m_imgDataItemImage.Clone, Image)
            End If

            If Not doOrigColumn.m_imgDataItemDragImage Is Nothing Then
                m_imgDataItemDragImage = DirectCast(doOrigColumn.m_imgDataItemDragImage.Clone, Image)
            End If

            If Not doOrigColumn.m_thDataType Is Nothing Then
                m_thDataType = DirectCast(doOrigColumn.m_thDataType.Clone(Me, bCutData, doRoot), TypeHelpers.DataTypeID)
            End If

            m_iDataSubSet = doOrigColumn.m_iDataSubSet
            m_iPrevDataSubset = doOrigColumn.m_iPrevDataSubset
            m_strColumnModuleName = doOrigColumn.m_strColumnModuleName
            m_strColumnClassType = doOrigColumn.m_strColumnClassType
            m_frmParentAxis = doOrigColumn.m_frmParentAxis
            m_bAutoAddToAxis = doOrigColumn.m_bAutoAddToAxis

        End Sub

        Public Overrides Function Delete(Optional ByVal bAskToDelete As Boolean = True, Optional ByVal e As Crownwood.DotNetMagic.Controls.TGCloseRequestEventArgs = Nothing) As Boolean

            Try
                Dim bDelete As Boolean = True
                If bAskToDelete AndAlso Util.ShowMessage("Are you certain that you want to delete this " & _
                                    "data column?", "Delete Data Column", MessageBoxButtons.YesNo) <> DialogResult.Yes Then
                    bDelete = False
                End If

                If bDelete AndAlso Not m_frmParentAxis Is Nothing AndAlso m_frmParentAxis.DataColumns.Contains(Me.ID) Then
                    Me.RemoveFromAxis()
                End If

                Return Not bDelete
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Function

        Public Overrides Sub InitializeAfterLoad()
            MyBase.InitializeAfterLoad()

            Me.DataItem = FindDataItem(m_strDataItemID)
        End Sub

        Public Overridable Overloads Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            oXml.IntoElem()

            m_strName = oXml.GetChildString("Name")
            m_strID = Util.LoadID(oXml, "")
            m_strDataItemAssemblyFile = oXml.GetChildString("DataItemAssemblyFile")
            m_strDataItemClassName = oXml.GetChildString("DataItemClassName")
            m_strDataItemID = Util.LoadID(oXml, "DataItem")
            m_iDataSubSet = oXml.GetChildInt("DataSubSet", m_iDataSubSet)
            m_iPrevDataSubset = oXml.GetChildInt("PrevDataSubset", m_iPrevDataSubset)
            m_bUseIncomingDataType = oXml.GetChildBool("UseIncomingDataType", m_bUseIncomingDataType)

            If oXml.FindChildElement("StructureID", False) Then
                m_strStructureID = oXml.GetChildString("StructureID")
            Else
                m_strStructureID = oXml.GetChildString("StructureName")
            End If

            m_strSelectedDataTypeID = oXml.GetChildString("SelectedDataType")

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            SaveDataWithName(oXml, "DataColumn")
        End Sub

        Public Overridable Sub SaveDataWithName(ByVal oXml As ManagedAnimatInterfaces.IStdXml, ByVal strDataName As String)

            oXml.AddChildElement(strDataName)
            oXml.IntoElem()

            oXml.AddChildElement("AssemblyFile", Me.AssemblyFile)
            oXml.AddChildElement("ClassName", Me.ClassName)

            oXml.AddChildElement("Name", m_strName)
            oXml.AddChildElement("ID", m_strID)
            oXml.AddChildElement("DataItemAssemblyFile", m_strDataItemAssemblyFile)
            oXml.AddChildElement("DataItemClassName", m_strDataItemClassName)
            oXml.AddChildElement("StructureID", m_strStructureID)
            oXml.AddChildElement("DataItemID", m_strDataItemID)
            oXml.AddChildElement("SelectedDataType", Me.DataType().ID)
            oXml.AddChildElement("DataSubSet", m_iDataSubSet)
            oXml.AddChildElement("PrevDataSubset", m_iPrevDataSubset)
            oXml.AddChildElement("UseIncomingDataType", m_bUseIncomingDataType)

            oXml.OutOfElem()

        End Sub

#End Region

#Region "Events"

        Private Sub OnAfterRemoveItem(ByRef doObject As Framework.DataObject)
            Try
                'When the linked part is deleted then delete this column.
                Me.Delete(False)
            Catch ex As Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace


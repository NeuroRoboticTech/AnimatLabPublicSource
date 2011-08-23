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
Imports AnimatGUI.Framework.UndoSystem

Namespace DataObjects.Behavior

    Public MustInherit Class Data
        Inherits AnimatGUI.DataObjects.DragObject

#Region " Enums "

        Public Enum enumBackmode
            Transparent
            Opaque
        End Enum

#End Region

#Region " Attributes "

        Protected m_strDescription As String = ""
        Protected m_ParentDiagram As Forms.Behavior.Diagram
        Protected m_doOrganism As DataObjects.Physical.Organism
        Protected m_bUpdateBatch As Boolean
        Protected m_bFound As Boolean
        Protected m_bOwnerDraw As Boolean = False
        Protected m_tpNeuralModuleType As System.Type
        'Protected m_NeuralModule As DataObjects.Behavior.NeuralModule = Nothing

        Protected m_bInitialized As Boolean = False

        'This is used when we are doing a copy/paste operation. We need to save the objects
        'with a different id than the original version. So we generate this temp id and use it
        'when saving.
        Protected m_strTempSelectedID As String = ""

        Protected m_tnTreeNode As TreeNode

#End Region

#Region " Properties "

        Public Overrides Property Name() As String
            Get
                Return Me.Text
            End Get
            Set(ByVal Value As String)
                Me.Text = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property SelectedID() As String
            Get
                If m_strTempSelectedID.Trim.Length = 0 Then
                    Return m_strID
                Else
                    Return m_strTempSelectedID
                End If
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property TempSelectedID() As String
            Get
                Return m_strTempSelectedID
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property Description() As String
            Get
                Return m_strDescription
            End Get
            Set(ByVal Value As String)
                m_strDescription = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property ParentDiagram() As Forms.Behavior.Diagram
            Get
                Return m_ParentDiagram
            End Get
            Set(ByVal Value As Forms.Behavior.Diagram)
                m_ParentDiagram = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property Organism() As DataObjects.Physical.Organism
            Get
                Return m_doOrganism
            End Get
            Set(ByVal Value As DataObjects.Physical.Organism)
                DisconnectOrganismEvents()
                m_doOrganism = Value
                ConnectOrganismEvents()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property NeuralModule() As DataObjects.Behavior.NeuralModule
            Get
                'If m_NeuralModule Is Nothing Then
                If Not m_doOrganism Is Nothing Then
                    Return m_doOrganism.FindNeuralModuleByType(Me.NeuralModuleType, False)
                Else
                    Return Nothing
                End If
                'End If
                'Return m_NeuralModule
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property RootForm() As System.Windows.Forms.Form
            Get
                'TODO
                'If Not m_ParentEditor Is Nothing Then
                '    Return m_ParentEditor
                'Else
                '    Return Util.Application
                'End If
            End Get
        End Property

        'This is used internally by the diagram when searching for items.
        <Browsable(False)> _
        Public Overridable Property Found() As Boolean
            Get
                Return m_bFound
            End Get
            Set(ByVal Value As Boolean)
                m_bFound = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property IsOwnerDrawn() As Boolean
            Get
                Return m_bOwnerDraw
            End Get
            Set(ByVal Value As Boolean)
                m_bOwnerDraw = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property TreeNode() As System.Windows.Forms.TreeNode
            Get
                Return m_tnTreeNode
            End Get
            Set(ByVal Value As System.Windows.Forms.TreeNode)
                m_tnTreeNode = Value
            End Set
        End Property

        'Tells if this data item can be added to a chart object. certain items like graphical node can not.
        <Browsable(False)> _
        Public Overrides ReadOnly Property CanBeCharted() As Boolean
            Get
                Return True
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property DragImageName() As String
            Get
                Return Me.WorkspaceImageName
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property StructureID() As String
            Get
                If Not Me.Organism Is Nothing Then
                    Return Me.Organism.ID
                Else
                    Return ""
                End If
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property Initialized() As Boolean
            Get
                Return m_bInitialized
            End Get
            Set(ByVal Value As Boolean)
                m_bInitialized = Value
            End Set
        End Property

        'Override the TimeStep property of the drag object.
        <Browsable(False)> _
        Public Overrides ReadOnly Property TimeStep() As Double
            Get
                'Try and get the neural module for this item and
                'and get the time step for that module to return.
                If Not Me.Organism Is Nothing AndAlso Not Me.NeuralModuleType Is Nothing Then
                    If Me.Organism.NeuralModules.Contains(Me.NeuralModuleType.FullName) Then
                        Dim nmMod As NeuralModule = DirectCast(m_doOrganism.NeuralModules(Me.NeuralModuleType.FullName), NeuralModule)
                        Return nmMod.TimeStep.ActualValue
                    Else
                        Return MyBase.TimeStep
                    End If
                Else
                    Return MyBase.TimeStep
                End If
            End Get
        End Property

        <Browsable(False)> _
        Public MustOverride ReadOnly Property NeuralModuleType() As System.Type

        <Browsable(False)> _
        Public MustOverride Property Text() As String

        '<Browsable(False)> _
        'Public MustOverride ReadOnly Property ImageName() As String

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            m_strDescription = ""
            m_bUpdateBatch = False

            If Not doParent Is Nothing AndAlso Util.IsTypeOf(doParent.GetType, GetType(Behavior.NeuralModule), False) Then
                Dim doNeuralModule As Behavior.NeuralModule = DirectCast(doParent, Behavior.NeuralModule)
                m_doOrganism = doNeuralModule.Organism
            ElseIf Not doParent Is Nothing AndAlso Util.IsTypeOf(doParent.GetType, GetType(DataObjects.Physical.Organism), False) Then
                m_doOrganism = DirectCast(doParent, DataObjects.Physical.Organism)
            End If

        End Sub

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim OrigNode As Behavior.Data = DirectCast(doOriginal, Behavior.Data)

            m_strName = OrigNode.m_strName
            m_bEnabled = OrigNode.m_bEnabled
            If Not m_WorkspaceImage Is Nothing Then m_WorkspaceImage = DirectCast(OrigNode.m_WorkspaceImage.Clone, System.Drawing.Image)
            If Not m_DragImage Is Nothing Then m_DragImage = DirectCast(OrigNode.m_DragImage.Clone, System.Drawing.Image)
            m_strDescription = OrigNode.m_strDescription
            m_ParentDiagram = OrigNode.m_ParentDiagram
            m_bOwnerDraw = OrigNode.m_bOwnerDraw
            m_doOrganism = OrigNode.m_doOrganism

        End Sub

        'You would use this to if you are setting a number of properties all at one time and do not want to 
        'have the addflow control updated for each change. After you have made your changes call the endbatch and the 
        'updates will take effect
        Public Overridable Sub BeginBatchUpdate()
            m_bUpdateBatch = True
        End Sub

        Public Overridable Sub EndBatchUpdate(Optional ByVal bUpdateChart As Boolean = True)
            m_bUpdateBatch = False
            If bUpdateChart Then UpdateChart()
        End Sub

        Protected Overridable Sub UpdateChart(Optional ByVal bForceUpdate As Boolean = False)
            If Not m_ParentDiagram Is Nothing AndAlso (Not m_bUpdateBatch OrElse bForceUpdate) Then
                m_ParentDiagram.UpdateChart(Me)
            End If

            If Not m_bUpdateBatch AndAlso Not Me.ParentDiagram Is Nothing Then
                Util.ModificationHistory.AddHistoryEvent(New DiagramChangedEvent(Me.ParentDiagram, Me))
            End If
        End Sub

        Protected Overridable Sub UpdateData(ByVal bSimple As Boolean)
            If Not m_ParentDiagram Is Nothing And Not m_bUpdateBatch Then
                m_ParentDiagram.UpdateData(Me, bSimple, False)
            End If
        End Sub

        Public Overridable Sub UpdateTreeNode()
            'If Not m_tnTreeNode Is Nothing Then
            '    If m_tnTreeNode.Text <> Me.Text Then
            '        m_tnTreeNode.Text = Me.Text
            '    End If
            'End If
            RemoveFromHierarchyBar()
            AddToHierarchyBar()
        End Sub

        Public Overridable Sub AddToHierarchyBar()

        End Sub

        Public Overridable Sub RemoveFromHierarchyBar()
            'Remove this icon from the heirarchy bar again
            If Not m_tnTreeNode Is Nothing Then
                m_tnTreeNode.Remove()
                m_tnTreeNode = Nothing
            End If
        End Sub

        Public Overridable Function BeforeEdit(ByVal strNewText As String) As Boolean
            Return False
        End Function

        Public Overridable Sub AfterEdit()
        End Sub

        Public Overridable Sub DoubleClicked()

            Dim frmProperties As New Forms.ProjectProperties

            frmProperties.PropertyData = Me.Properties
            frmProperties.StartPosition = FormStartPosition.CenterScreen
            frmProperties.Title = Me.Text & " Properties"
            frmProperties.MinimizeBox = False
            frmProperties.MaximizeBox = False
            frmProperties.Width = 400
            frmProperties.Height = 800
            frmProperties.ShowDialog()

        End Sub

        Public Overrides Sub AfterUndoRemove()
            ClearErrors()
        End Sub

        Public Overrides Sub AfterRedoRemove()
            RemoveFromHierarchyBar()
            ClearErrors()
        End Sub

        Public Overridable Sub OwnerDraw(ByVal sender As Object, ByVal szRect As RectangleF, ByVal eGraphics As System.Drawing.Graphics)
        End Sub

        Public Overrides Sub InitializeAfterLoad()
            m_bInitialized = True
        End Sub

        Public MustOverride Sub FailedToInitialize()

        Public Overridable Sub CheckForErrors()
        End Sub

        Public Overridable Sub ClearErrors()

            If Not Util.Application.ProjectErrors Is Nothing Then

                Dim deError As DiagramErrors.DataError
                Dim aryIDs As New ArrayList
                For Each deItem As DictionaryEntry In Util.Application.ProjectErrors.Errors
                    If Util.IsTypeOf(deItem.Value.GetType(), GetType(DiagramErrors.DataError), False) Then
                        deError = DirectCast(deItem.Value, DiagramErrors.DataError)
                        If deError.Item Is Me Then
                            aryIDs.Add(deError.ID)
                        End If
                    End If
                Next

                For Each strID As String In aryIDs
                    Util.Application.ProjectErrors.Errors.Remove(strID)
                Next

            End If

        End Sub

        Public Overridable Sub GenerateTempSelectedID(ByVal bCopy As Boolean)
            If bCopy Then
                m_strTempSelectedID = System.Guid.NewGuid().ToString()
            Else
                'Otherwise this must be a cut so lets keep the current ID instead of generating a new one.
                m_strTempSelectedID = m_strID
            End If

            'm_ParentDiagram.SetItemsTempID(Me, m_strTempSelectedID)
        End Sub

        Public Overridable Sub ClearTempSelectedID()
            m_strTempSelectedID = ""
            'm_ParentDiagram.SetItemsTempID(Me, m_strID)
        End Sub

        'Public Overrides Sub SaveDataColumnToXml(ByRef oXml As AnimatGUI.Interfaces.StdXml)
        'End Sub

        Public Overrides Function ToString() As String
            Return Me.Text
        End Function

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGUICtrls.Controls.PropertyTable)

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()
        End Sub

        Public Overrides Sub EnsureFormActive()

            If Not Me.Organism Is Nothing AndAlso Me.Organism.BehaviorEditor Is Nothing Then
                'Return Util.Application.EditBehavioralSystem(Me.Organism)
            End If

        End Sub

        Public Overrides Sub SaveSimulationXml(ByRef oXml As AnimatGUI.Interfaces.StdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            Try
                oXml.AddChildElement(strName)
                oXml.IntoElem()  'Into Node Element

                oXml.AddChildElement("AssemblyFile", Me.AssemblyFile)
                oXml.AddChildElement("ClassName", Me.ClassName)

                oXml.AddChildElement("ID", Me.SelectedID)
                oXml.AddChildElement("Name", Me.Name)

                oXml.OutOfElem()  'Outof Node Element

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

#End Region

#Region " Events "

        Protected Overridable Sub ConnectOrganismEvents()

            If Not m_doOrganism Is Nothing Then
                AddHandler m_doOrganism.AfterPropertyChanged, AddressOf Me.OnOrganismModified

                If Not Me.NeuralModule Is Nothing Then
                    AddHandler Me.NeuralModule.AfterPropertyChanged, AddressOf Me.OnNeuralModuleModified
                End If
            End If

        End Sub


        Protected Overridable Sub DisconnectOrganismEvents()

            If Not m_doOrganism Is Nothing Then
                RemoveHandler m_doOrganism.AfterPropertyChanged, AddressOf Me.OnOrganismModified

                If Not Me.NeuralModule Is Nothing Then
                    RemoveHandler Me.NeuralModule.AfterPropertyChanged, AddressOf Me.OnNeuralModuleModified
                End If
            End If

        End Sub

        Protected Overridable Sub OnOrganismModified(ByRef doObject As AnimatGUI.Framework.DataObject, ByVal propInfo As System.Reflection.PropertyInfo)

        End Sub

        Protected Overridable Sub OnNeuralModuleModified(ByRef doObject As AnimatGUI.Framework.DataObject, ByVal propInfo As System.Reflection.PropertyInfo)

        End Sub

#End Region

    End Class

End Namespace

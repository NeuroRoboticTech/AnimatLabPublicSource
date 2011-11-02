Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports AnimatGuiCtrls.Controls
Imports Crownwood.DotNetMagic.Common
Imports Crownwood.DotNetMagic.Docking
Imports Crownwood.DotNetMagic.Controls
Imports AnimatGUI.Framework

Namespace Forms

    Public Class AnimatForm
        Inherits Crownwood.DotNetMagic.Forms.DotNetMagicForm

#Region " Attributes "

        Protected m_frmParent As AnimatForm
        Protected m_strID As String = System.Guid.NewGuid().ToString()
        Protected m_ctContent As AnimatGUICtrls.Docking.AnimatContent
        Protected m_tabPage As Crownwood.DotNetMagic.Controls.TabPage
        Protected m_strTabPageName As String = ""
        Protected m_strTitle As String = ""

        Protected m_bFixedProperties As Boolean
        Protected m_Properties As PropertyTable

        'This is the root dataobject for any data in this form. It helps the
        'form keep track of whether data items are dirty.
        Protected m_doFormHelper As New DataObjects.FormHelper(Me)

        Protected m_bUndoRedoInProgress As Boolean = False
        Protected m_bSetValueInProgress As Boolean = False

        Protected m_doInterface As ManagedAnimatInterfaces.IDataObjectInterface = Nothing

        Protected m_WorkspaceImage As System.Drawing.Image
        Protected m_TabImage As System.Drawing.Image
        Protected m_tnWorkspaceNode As Crownwood.DotNetMagic.Controls.Node

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Overridable Property ID() As String
            Get
                Return m_strID
            End Get
            Set(ByVal Value As String)
                m_strID = Value
            End Set
        End Property

        Public Overridable ReadOnly Property ParentAnimatForm() As AnimatForm
            Get
                Return m_frmParent
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property RootForm() As System.Windows.Forms.Form
            Get
                Return Util.Application
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property Content() As AnimatGUICtrls.Docking.AnimatContent
            Get
                Return m_ctContent
            End Get
            Set(ByVal Value As AnimatGUICtrls.Docking.AnimatContent)
                m_ctContent = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property TabPage() As Crownwood.DotNetMagic.Controls.TabPage
            Get
                Return m_tabPage
            End Get
            Set(ByVal value As Crownwood.DotNetMagic.Controls.TabPage)
                m_tabPage = value
            End Set
        End Property

        Public Overridable ReadOnly Property TabImageName() As String
            Get
                Return "AnimatGUI.DefaultObject.gif"
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property TabImage() As System.Drawing.Image
            Get
                If m_TabImage Is Nothing Then
                    m_TabImage = ImageManager.LoadImage(Me.TabImageName)
                End If

                Return m_TabImage
            End Get
            Set(ByVal Value As System.Drawing.Image)
                m_TabImage = Value
            End Set
        End Property

        Public Overridable ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.DefaultObject.gif"
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property WorkspaceImage() As System.Drawing.Image
            Get
                If m_WorkspaceImage Is Nothing Then
                    m_WorkspaceImage = ImageManager.LoadImage(Me.WorkspaceImageName)
                End If

                Return m_WorkspaceImage
            End Get
            Set(ByVal Value As System.Drawing.Image)
                m_WorkspaceImage = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property WorkspaceNode() As Crownwood.DotNetMagic.Controls.Node
            Get
                Return m_tnWorkspaceNode
            End Get
            Set(ByVal value As Crownwood.DotNetMagic.Controls.Node)
                m_tnWorkspaceNode = value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property AssemblyFile() As String
            Get
                Dim strPath As String, strFile As String
                Util.SplitPathAndFile(Me.GetType.Assembly.Location, strPath, strFile)
                Return strFile
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property ClassName() As String
            Get
                Return Me.GetType.ToString
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property IconName() As String
            Get
                Return ""
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property TabPageName() As String
            Get
                Return m_strTabPageName
            End Get
            Set(ByVal Value As String)
                m_strTabPageName = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Property FixedProperties() As Boolean
            Get
                Return m_bFixedProperties
            End Get
            Set(ByVal Value As Boolean)
                m_bFixedProperties = Value
            End Set
        End Property

        <Browsable(False)> _
        Public ReadOnly Property Properties() As PropertyBag
            Get
                If m_Properties Is Nothing OrElse Not m_bFixedProperties Then
                    CreateProperties()
                End If

                Return m_Properties
            End Get
        End Property

        <Browsable(False)> _
        Public ReadOnly Property FormHelper() As DataObjects.FormHelper
            Get
                Return m_doFormHelper
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property Title() As String
            Get
                Return m_strTitle
            End Get
            Set(ByVal Value As String)
                m_strTitle = Value
                Me.Text = m_strTitle
            End Set
        End Property

        <Browsable(False)> _
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

        <Browsable(False)> _
        Public Overridable Property IsDirty() As Boolean
            Get
                Return m_doFormHelper.IsDirty
            End Get
            Set(ByVal Value As Boolean)
                If Not Util.DisableDirtyFlags AndAlso m_doFormHelper.IsDirty <> Value Then
                    m_doFormHelper.IsDirty = Value
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property SetValueInProgress() As Boolean
            Get
                Return m_bSetValueInProgress
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property UndoRedoInProgress() As Boolean
            Get
                Return m_bUndoRedoInProgress
            End Get
            Set(ByVal Value As Boolean)
                m_bUndoRedoInProgress = Value
            End Set
        End Property

        <Browsable(False)> _
        Public ReadOnly Property IsCtrlKeyPressed() As Boolean
            Get
                If Control.ModifierKeys = Keys.Control Then
                    Return True
                Else
                    Return False
                End If
            End Get
        End Property

        <Browsable(False)> _
        Public ReadOnly Property IsShiftKeyPressed() As Boolean
            Get
                If Control.ModifierKeys = Keys.Shift Then
                    Return True
                Else
                    Return False
                End If
            End Get
        End Property

        <Browsable(False)> _
        Public ReadOnly Property IsAltKeyPressed() As Boolean
            Get
                If Control.ModifierKeys = Keys.Alt Then
                    Return True
                Else
                    Return False
                End If
            End Get
        End Property

        Public Overridable ReadOnly Property Description() As String
            Get

            End Get
        End Property

        Public Overridable ReadOnly Property FormMenuStrip() As AnimatMenuStrip
            Get

            End Get
        End Property

        Public Overridable ReadOnly Property FormToolStrip() As AnimatToolStrip
            Get

            End Get
        End Property

        Public Overridable ReadOnly Property CheckSaveOnClose() As Boolean
            Get
                Return True
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub InitializeComponent()
            Me.SuspendLayout()
            '
            'AnimatForm
            '
            Me.ClientSize = New System.Drawing.Size(284, 262)
            Me.Name = "AnimatForm"
            Me.ResumeLayout(False)

        End Sub

        Public Overridable Sub Initialize(Optional ByVal frmParent As AnimatForm = Nothing)
            m_strTabPageName = "Page 1"
            m_frmParent = frmParent

            If Not m_frmParent Is Nothing Then
                m_doFormHelper.Parent = m_frmParent.FormHelper
            Else
                m_doFormHelper.Parent = Nothing
            End If


        End Sub

        Public Overridable Sub InitializeSimulationReferences()
            If m_doInterface Is Nothing AndAlso Not Util.Application.SimulationInterface Is Nothing AndAlso Util.Application.SimulationInterface.SimOpen Then
                m_doInterface = Util.Application.CreateDataObjectInterface(Me.ID)
            End If
        End Sub

        Public Overridable Function SetSimData(ByVal sDataType As String, ByVal sValue As String, ByVal bThrowError As Boolean) As Boolean
            If Not m_doInterface Is Nothing Then
                Return m_doInterface.SetData(sDataType, sValue, bThrowError)
            End If
        End Function

        Public Overridable Sub UpdateToolStrips()

        End Sub

        Public Overridable Overloads Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            oXml.IntoElem() 'Into Form Element
            m_strID = Util.LoadID(oXml, "")
            m_strTitle = oXml.GetChildString("Title")
            m_strTabPageName = oXml.GetChildString("TabPageName", "")
            oXml.OutOfElem()

        End Sub

        Public Overridable Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            oXml.AddChildElement("Form")
            oXml.IntoElem() 'Into Form Element

            oXml.AddChildElement("ID", m_strID)
            oXml.AddChildElement("Title", Me.Title)
            oXml.AddChildElement("TabPageName", m_strTabPageName)
            oXml.AddChildElement("AssemblyFile", Me.AssemblyFile)
            oXml.AddChildElement("ClassName", Me.ClassName)

            If Not Me.Content Is Nothing Then
                oXml.AddChildElement("BackgroundForm", Me.Content.BackgroundForm)
            End If

            oXml.OutOfElem()  'Outof Form Element
        End Sub

        Public Overridable Function SaveDataXml(Optional ByVal strRoot As String = "Root") As String
            Dim oXml As ManagedAnimatInterfaces.IStdXml = Util.Application.CreateStdXml()

            oXml.AddElement(strRoot)
            SaveData(oXml)

            Return oXml.Serialize()
        End Function

        Public Overridable Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")

        End Sub

        Public Overridable Function GetSimulationXml(ByVal strName As String, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing) As String

            Dim oXml As ManagedAnimatInterfaces.IStdXml = Util.Application.CreateStdXml()
            oXml.AddElement("Root")
            SaveSimulationXml(oXml, nmParentControl, strName)

            Return oXml.Serialize()
        End Function

        Public Overridable Sub InitializeAfterLoad()

        End Sub

        Public Overridable Sub OnBeforeFormAdded()
        End Sub

        Public Overridable Sub OnAfterFormAdded()
        End Sub

        Public Overridable Sub OnAfterMdiParentInitialized()
        End Sub

        Public Overridable Sub OnMdiParentShowing()
        End Sub

        Public Overridable Sub OnGetFocus()
        End Sub

        Public Overridable Sub OnLoseFocus()
        End Sub

        Protected Overridable Sub CreateProperties()

            m_Properties = New PropertyTable

            AddHandler m_Properties.GetValue, AddressOf Me.Properties_GetValue
            AddHandler m_Properties.SetValue, AddressOf Me.Properties_SetValue

            BuildProperties(m_Properties)
        End Sub

        Public Overridable Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

        End Sub

        Protected Overridable Sub Properties_GetValue(ByVal sender As Object, ByVal e As PropertySpecEventArgs)
            Try
                Dim propInfo As System.Reflection.PropertyInfo = Me.GetType().GetProperty(e.Property.PropertyName)
                If Not propInfo Is Nothing Then
                    If propInfo.CanRead Then
                        e.Value = propInfo.GetValue(Me, Nothing)
                    Else
                        Throw New System.Exception("The property '" & propInfo.Name & "' is write only.")
                    End If

                    If TypeOf (e.Value) Is Framework.DataObject Then
                        Dim doData As Framework.DataObject = DirectCast(e.Value, Framework.DataObject)

                        If doData.ViewSubProperties Then
                            e.Value = doData.Properties
                        Else
                            e.Value = doData
                        End If
                    End If
                Else
                    Throw New System.Exception("No property info returned for the property name '" & e.Property.PropertyName & "'.")
                End If

            Catch ex As System.Exception
                If Not ex.InnerException Is Nothing AndAlso ex.InnerException.Message.Trim.Length > 0 Then
                    AnimatGUI.Framework.Util.DisplayError(ex.InnerException)
                Else
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End If
            End Try
        End Sub

        Protected Overridable Function GetOriginalValueForHistory(ByVal propInfo As System.Reflection.PropertyInfo) As Object
            Dim origValue As Object

            If propInfo.CanRead AndAlso Util.ModificationHistory.AllowAddHistory Then
                Dim tempValue As Object = propInfo.GetValue(Me, Nothing)

                If Not tempValue Is Nothing AndAlso TypeOf tempValue Is AnimatGUI.Framework.DataObject Then
                    Dim doTemp As AnimatGUI.Framework.DataObject = DirectCast(tempValue, AnimatGUI.Framework.DataObject)
                    origValue = doTemp.Clone(doTemp.Parent, False, doTemp)
                ElseIf Not tempValue Is Nothing AndAlso TypeOf tempValue Is AnimatGUI.Collections.AnimatSortedList Then
                    Dim doTemp As AnimatGUI.Collections.AnimatSortedList = DirectCast(tempValue, AnimatGUI.Collections.AnimatSortedList)
                    origValue = doTemp.Clone()
                ElseIf Not tempValue Is Nothing AndAlso TypeOf tempValue Is AnimatGUI.Collections.AnimatCollectionBase Then
                    Dim doTemp As AnimatGUI.Collections.AnimatCollectionBase = DirectCast(tempValue, AnimatGUI.Collections.AnimatCollectionBase)
                    origValue = doTemp.Clone(doTemp.Parent, False, Nothing)
                ElseIf Not tempValue Is Nothing AndAlso TypeOf tempValue Is AnimatGUI.Collections.AnimatDictionaryBase Then
                    Dim doTemp As AnimatGUI.Collections.AnimatDictionaryBase = DirectCast(tempValue, AnimatGUI.Collections.AnimatDictionaryBase)
                    origValue = doTemp.Clone(doTemp.Parent, False, Nothing)
                Else
                    origValue = tempValue
                End If
            End If

            Return origValue
        End Function

        Protected Overridable Sub SaveChangeHistory(ByVal e As PropertySpecEventArgs, ByVal propInfo As System.Reflection.PropertyInfo, ByVal origValue As Object)
            Dim newValue As Object

            If Not e.Value Is Nothing AndAlso TypeOf e.Value Is AnimatGUI.Framework.DataObject Then
                Dim doTemp As AnimatGUI.Framework.DataObject = DirectCast(e.Value, AnimatGUI.Framework.DataObject)
                newValue = doTemp.Clone(doTemp.Parent, False, doTemp)
            ElseIf Not e.Value Is Nothing AndAlso TypeOf e.Value Is AnimatGUI.Collections.AnimatSortedList Then
                Dim doTemp As AnimatGUI.Collections.AnimatSortedList = DirectCast(e.Value, AnimatGUI.Collections.AnimatSortedList)
                newValue = doTemp.Clone()
            ElseIf Not e.Value Is Nothing AndAlso TypeOf e.Value Is AnimatGUI.Collections.AnimatCollectionBase Then
                Dim doTemp As AnimatGUI.Collections.AnimatCollectionBase = DirectCast(e.Value, AnimatGUI.Collections.AnimatCollectionBase)
                newValue = doTemp.Clone(doTemp.Parent, False, Nothing)
            ElseIf Not e.Value Is Nothing AndAlso TypeOf e.Value Is AnimatGUI.Collections.AnimatDictionaryBase Then
                Dim doTemp As AnimatGUI.Collections.AnimatDictionaryBase = DirectCast(e.Value, AnimatGUI.Collections.AnimatDictionaryBase)
                newValue = doTemp.Clone(doTemp.Parent, False, Nothing)
            Else
                newValue = e.Value
            End If

            Dim frmRoot As System.Windows.Forms.Form = Me.RootForm
            Util.ModificationHistory.AddHistoryEvent(New UndoSystem.PropertyChangedEvent(frmRoot, Me, propInfo, origValue, newValue))
        End Sub

        Protected Overridable Sub Properties_SetValue(ByVal sender As Object, ByVal e As PropertySpecEventArgs)
            Try
                Dim propInfo As System.Reflection.PropertyInfo = Me.GetType().GetProperty(e.Property.PropertyName)

                If Not propInfo Is Nothing Then
                    If propInfo.CanWrite Then
                        Dim lModificationCount As Long = Util.ModificationHistory.ModificationCount
                        Dim origValue As Object = GetOriginalValueForHistory(propInfo)

                        m_bSetValueInProgress = True
                        propInfo.SetValue(Me, e.Value, Nothing)
                        m_bSetValueInProgress = False
                        Me.IsDirty = True

                        'Just in case, lets directly set the isdirty flag of the animatapplication. If for some reason the
                        'isdirty signal does not propogate up properly then this flag will still be set and things will 
                        'be saved correctly.
                        Util.Application.IsDirty = True

                        'Only add the history for this propchange if the property did not already add it itself.
                        If lModificationCount = Util.ModificationHistory.ModificationCount AndAlso Util.ModificationHistory.AllowAddHistory Then
                            SaveChangeHistory(e, propInfo, origValue)
                        End If
                    Else
                        Throw New System.Exception("The property '" & propInfo.Name & "' is read only.")
                    End If
                Else
                    Throw New System.Exception("No property info returned for the property name '" & e.Property.PropertyName & "'.")
                End If

            Catch ex As System.Exception
                If Not ex.InnerException Is Nothing AndAlso ex.InnerException.Message.Trim.Length > 0 Then
                    If TypeOf ex.InnerException Is System.Reflection.TargetInvocationException AndAlso Not ex.InnerException.InnerException Is Nothing Then
                        AnimatGUI.Framework.Util.DisplayError(ex.InnerException.InnerException)
                    Else
                        AnimatGUI.Framework.Util.DisplayError(ex.InnerException)
                    End If
                Else
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End If

                'If we got an error while trying to set the value to a new value then we need to change back the
                'value that is currently displayed to the previous value.
                Try
                    e.Value = Me.GetType().GetProperty(e.Property.PropertyName).GetValue(Me, Nothing)
                Catch InnerEx As System.Exception
                    'If we could not fix it then do nothing
                End Try
            Finally
                m_bSetValueInProgress = False
            End Try
        End Sub

        Public Overridable Sub ManualAddPropertyHistory(ByVal strPropertyName As String, ByVal origValue As Object, ByVal newValue As Object, Optional ByVal bOverrideSetValue As Boolean = False)

            Try
                If (Not Me.SetValueInProgress OrElse (Me.SetValueInProgress AndAlso bOverrideSetValue)) AndAlso Not Me.UndoRedoInProgress AndAlso Util.ModificationHistory.AllowAddHistory Then
                    Dim propInfo As System.Reflection.PropertyInfo = Me.GetType().GetProperty(strPropertyName)
                    Dim frmRoot As System.Windows.Forms.Form = Me.RootForm
                    Util.ModificationHistory.AddHistoryEvent(New UndoSystem.PropertyChangedEvent(frmRoot, Me, propInfo, origValue, newValue))
                End If

            Catch ex As System.Exception

            End Try

        End Sub

        Public Overridable Sub ManualAddPropertyHistory(ByVal oObject As Object, ByVal strPropertyName As String, ByVal origValue As Object, ByVal newValue As Object, Optional ByVal bOverrideSetValue As Boolean = False)

            Try
                If (Not Me.SetValueInProgress OrElse (Me.SetValueInProgress AndAlso bOverrideSetValue)) AndAlso Not Me.UndoRedoInProgress AndAlso Util.ModificationHistory.AllowAddHistory Then
                    Dim propInfo As System.Reflection.PropertyInfo = oObject.GetType().GetProperty(strPropertyName)

                    Dim frmRoot As System.Windows.Forms.Form
                    If TypeOf oObject Is AnimatGUI.Framework.DataObject Then
                        Dim doObject As AnimatGUI.Framework.DataObject = DirectCast(oObject, AnimatGUI.Framework.DataObject)
                        frmRoot = doObject.RootForm
                    ElseIf TypeOf oObject Is AnimatGUI.Forms.AnimatForm Then
                        Dim frmObject As AnimatGUI.Forms.AnimatForm = DirectCast(oObject, AnimatGUI.Forms.AnimatForm)
                        frmRoot = frmObject.RootForm
                    End If

                    Util.ModificationHistory.AddHistoryEvent(New UndoSystem.PropertyChangedEvent(frmRoot, oObject, propInfo, origValue, newValue))
                End If

            Catch ex As System.Exception

            End Try

        End Sub

        Public Overridable Sub ManualAddHistory(ByVal doEvent As AnimatGUI.Framework.UndoSystem.HistoryEvent)

            Try
                If Not Me.UndoRedoInProgress AndAlso Util.ModificationHistory.AllowAddHistory Then
                    Util.ModificationHistory.AddHistoryEvent(doEvent)
                End If

            Catch ex As System.Exception

            End Try

        End Sub

        Public Overridable Sub ClearIsDirty()
            Me.IsDirty = False
        End Sub

        Public Overridable Sub RefreshTitle()
            Me.Title = Me.Title
        End Sub

        Public Overridable Sub UndoRedoRefresh(ByVal doRefresh As AnimatGUI.Framework.DataObject)
        End Sub

        Public Overridable Sub MakeVisible()
            If Not m_tabPage Is Nothing Then
                Dim leaf As TabGroupLeaf = Util.Application.AnimatTabbedGroups.FirstLeaf()

                Dim bFound As Boolean = False
                While Not leaf Is Nothing AndAlso Not bFound
                    If leaf.TabPages.Contains(m_tabPage) Then
                        leaf.TabControl.SelectedTab = m_tabPage
                        bFound = True
                    End If

                    leaf = Util.Application.AnimatTabbedGroups.NextLeaf(leaf)
                End While

                m_tabPage.Control.Select()
            End If
        End Sub

#Region " Add-Remove to List Methods "

        Public Overridable Sub BeforeAddToList(Optional ByVal bThrowError As Boolean = True)
        End Sub

        Public Overridable Sub AfterAddToList(Optional ByVal bThrowError As Boolean = True)
        End Sub

        Public Overridable Sub BeforeRemoveFromList(Optional ByVal bThrowError As Boolean = True)
        End Sub

        Public Overridable Sub AfterRemoveFromList(Optional ByVal bThrowError As Boolean = True)
        End Sub

#End Region

#Region " Workspace Methods "

        Public Overridable Sub CreateWorkspaceTreeView(ByVal doParent As Framework.DataObject, _
                                                       ByVal doParentNode As Crownwood.DotNetMagic.Controls.Node, _
                                                       Optional ByVal bRootObject As Boolean = False)
            'Me.RemoveWorksapceTreeView()

            If m_tnWorkspaceNode Is Nothing AndAlso (bRootObject OrElse (Not bRootObject AndAlso Not doParentNode Is Nothing)) Then
                m_tnWorkspaceNode = Util.ProjectWorkspace.AddTreeNode(doParentNode, Me.Name, Me.WorkspaceImageName)
                m_tnWorkspaceNode.Tag = Me

                If Me.Enabled Then
                    m_tnWorkspaceNode.BackColor = Color.White
                Else
                    m_tnWorkspaceNode.BackColor = Color.Gray
                End If
            End If
        End Sub

        Public Overridable Function WorkspaceTreeviewPopupMenu(ByRef tnSelectedNode As Crownwood.DotNetMagic.Controls.Node, ByVal ptPoint As Point) As Boolean

        End Function

        Public Overridable Sub WorkspaceTreeviewDoubleClick(ByVal tnSelectedNode As Crownwood.DotNetMagic.Controls.Node)

        End Sub

        Public Overridable Sub RemoveWorksapceTreeView()

            If Not m_tnWorkspaceNode Is Nothing Then
                m_tnWorkspaceNode.Remove()
                m_tnWorkspaceNode = Nothing
            End If

        End Sub

        Protected Overridable Sub ReconnectFormToWorkspace()
        End Sub

        Public Overridable Function Delete(Optional ByVal bAskToDelete As Boolean = True, Optional ByVal e As Crownwood.DotNetMagic.Controls.TGCloseRequestEventArgs = Nothing) As Boolean

        End Function

        Public Overridable Sub SelectItem(Optional ByVal bSelectMultiple As Boolean = False)
            If Not m_tnWorkspaceNode Is Nothing AndAlso Not Util.ProjectWorkspace.TreeView.SelectedNodes.Contains(m_tnWorkspaceNode) Then
                Util.ProjectWorkspace.TreeView.SelectNode(m_tnWorkspaceNode, False, bSelectMultiple)
            End If
        End Sub

        Public Overridable Sub DeselectItem()
            If Not m_tnWorkspaceNode Is Nothing AndAlso Util.ProjectWorkspace.TreeView.SelectedNodes.Contains(m_tnWorkspaceNode) Then
                Util.ProjectWorkspace.TreeView.DeselectNode(m_tnWorkspaceNode, True)
            End If
        End Sub

        Public Overridable Function BeforeSelected() As Boolean
        End Function

        Public Overridable Sub AfterSelected()
        End Sub

        Public Overridable Sub BeforeDeselected()
        End Sub

        Public Overridable Sub AfterDeselected()
        End Sub

        Public Overridable Sub ValidateFileToolStripItemState()

        End Sub

        Public Overridable Sub ValidateEditToolStripItemState()

        End Sub

        Public Overridable Sub ValidateViewToolStripItemState()

        End Sub

        Public Overridable Sub ValidateAddToolStripItemState()

        End Sub

        Public Overridable Sub ValidateHelpToolStripItemState()

        End Sub

#End Region

#End Region

#Region " Events "

        Protected Overridable Sub AnimatForm_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        End Sub

        Protected Overridable Sub AnimatForm_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        End Sub

        Public Overridable Sub PageCloseSaveRequest(ByVal e As Crownwood.DotNetMagic.Controls.TGCloseRequestEventArgs)

            If Me.IsDirty AndAlso Me.CheckSaveOnClose Then
                Dim eResult As System.Windows.Forms.DialogResult = DialogResult.OK
                eResult = Util.ShowMessage("There are unsaved changes in this form. " & _
                                                                                    "Do you want to save them before you exit?", _
                                                                                    "Save Changes", MessageBoxButtons.YesNoCancel)
                If eResult = DialogResult.Cancel Then
                    If Not e Is Nothing Then e.Cancel = True
                    Return
                ElseIf eResult = DialogResult.Yes Then
                    Util.Application.SaveProject(Util.Application.ProjectFile)
                End If
            End If

        End Sub

        Protected Overridable Sub OnExpandAll(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If Not sender Is Nothing AndAlso TypeOf sender Is ToolStripMenuItem Then
                    Dim mcCommand As ToolStripMenuItem = DirectCast(sender, ToolStripMenuItem)

                    If Not mcCommand.Tag Is Nothing AndAlso TypeOf mcCommand.Tag Is Crownwood.DotNetMagic.Controls.Node Then
                        Dim tnNode As Crownwood.DotNetMagic.Controls.Node = DirectCast(mcCommand.Tag, Crownwood.DotNetMagic.Controls.Node)
                        tnNode.ExpandAll()
                    End If
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Overridable Sub OnCollapseAll(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If Not sender Is Nothing AndAlso TypeOf sender Is ToolStripMenuItem Then
                    Dim mcCommand As ToolStripMenuItem = DirectCast(sender, ToolStripMenuItem)

                    If Not mcCommand.Tag Is Nothing AndAlso TypeOf mcCommand.Tag Is Crownwood.DotNetMagic.Controls.Node Then
                        Dim tnNode As Crownwood.DotNetMagic.Controls.Node = DirectCast(mcCommand.Tag, Crownwood.DotNetMagic.Controls.Node)
                        tnNode.Collapse()
                    End If
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

    End Class

End Namespace

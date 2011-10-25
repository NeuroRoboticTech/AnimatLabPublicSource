Imports System
Imports System.Threading
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports AnimatGuiCtrls.Controls

Namespace Framework

    Public MustInherit Class DataObject

#Region " Enums "

#End Region

#Region " Attributes "

        Protected m_strName As String = ""
        Protected m_strID As String = System.Guid.NewGuid().ToString()
        Protected m_strCloneID As String = ""

        Protected m_doParent As Framework.DataObject
        Protected m_bFixedProperties As Boolean
        Protected m_Properties As PropertyTable
        Protected m_bIsDirty As Boolean = False
        Protected m_bEnabled As Boolean = True

        Protected m_bUndoRedoInProgress As Boolean = False
        Protected m_bSetValueInProgress As Boolean = False

        Protected m_doInterface As Interfaces.DataObjectInterface = Nothing

        Protected m_WorkspaceImage As System.Drawing.Image
        Protected m_tnWorkspaceNode As Crownwood.DotNetMagic.Controls.Node

        Protected m_oTag As Object

        Protected m_bIsInitialized As Boolean = False

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Overridable Property Name() As String
            Get
                Return m_strName
            End Get
            Set(ByVal Value As String)
                If Value.Trim.Length = 0 Then
                    Throw New System.Exception("You can not set the name property to blank.")
                End If

                'Now add it back in the list with the new name
                m_strName = Value.Trim

                If Not m_tnWorkspaceNode Is Nothing Then
                    m_tnWorkspaceNode.Text = m_strName
                End If
            End Set
        End Property

        <[ReadOnly](True)> _
        Public Overridable Property ID() As String
            Get
                Return m_strID
            End Get
            Set(ByVal Value As String)
                m_strID = Value

                If m_strID.Trim.Length = 0 Then
                    m_strID = System.Guid.NewGuid().ToString()
                End If

            End Set
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property CloneID() As String
            Get
                Return m_strCloneID
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property Parent() As Framework.DataObject
            Get
                Return m_doParent
            End Get
            Set(ByVal Value As Framework.DataObject)
                m_doParent = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property Enabled() As Boolean
            Get
                Return m_bEnabled
            End Get
            Set(ByVal Value As Boolean)
                SetSimData("Enabled", Value.ToString, True)
                m_bEnabled = Value

                If Me.Enabled AndAlso Not m_tnWorkspaceNode Is Nothing Then
                    m_tnWorkspaceNode.BackColor = Color.White
                Else
                    m_tnWorkspaceNode.BackColor = Color.Gray
                End If

            End Set
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property Selected() As Boolean
            Get
                'Go through the treeview in the project workspace to see if this item is currently selected
                Return Util.ProjectWorkspace.TreeView.SelectedNodes.Contains(Me.WorkspaceNode)
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property RootForm() As System.Windows.Forms.Form
            Get
                If Not m_doParent Is Nothing Then
                    Return m_doParent.RootForm()
                Else
                    Return Util.Application
                End If
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property FixedProperties() As Boolean
            Get
                Return m_bFixedProperties
            End Get
            Set(ByVal Value As Boolean)
                m_bFixedProperties = Value
            End Set
        End Property

        'If this is true and it is a submember of another databoject then when you view the properties it will 
        'really be viewing the propertybag of this sub object.
        <Browsable(False)> _
        Public Overridable Property ViewSubProperties() As Boolean
            Get
                Return True
            End Get
            Set(ByVal Value As Boolean)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property Properties() As PropertyBag
            Get
                If m_Properties Is Nothing OrElse Not m_bFixedProperties Then
                    CreateProperties()
                End If

                Return m_Properties
            End Get
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
        Public Overridable ReadOnly Property AssemblyModuleName() As String
            Get
                Return Util.RootNamespace(Me.ClassName)
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property ModuleName() As String
            Get
                Return ""
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property ClassName() As String
            Get
                Return Me.GetType.ToString
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property SimClassName() As String
            Get
                Return ""
            End Get
        End Property

        '<Browsable(False)> _
        'Public MustOverride ReadOnly Property Type() As String

        <Browsable(False)> _
        Public Overridable ReadOnly Property TypeName() As String
            Get
                'By default just return the class name.
                Dim aryType() As String = Split(Me.GetType().ToString(), ".")
                Return aryType(aryType.Length - 1)
            End Get
        End Property

        <Browsable(False)> _
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
        Public Overridable Property IsDirty() As Boolean
            Get
                Return m_bIsDirty
            End Get
            Set(ByVal Value As Boolean)
                If Not Util.DisableDirtyFlags Then
                    m_bIsDirty = Value

                    If m_bIsDirty AndAlso Not m_doParent Is Nothing Then
                        m_doParent.IsDirty = True
                    End If
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
        Public Overridable ReadOnly Property SimInterface() As Interfaces.DataObjectInterface
            Get
                Return m_doInterface
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property Tag() As Object
            Get
                Return m_oTag
            End Get
            Set(ByVal Value As Object)
                m_oTag = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property IsInitialized() As Boolean
            Get
                Return m_bIsInitialized
            End Get
            Set(ByVal Value As Boolean)
                m_bIsInitialized = Value
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            m_doParent = doParent

            If Not Util.Application Is Nothing Then
                AddHandler Util.Application.ApplicationExiting, AddressOf Me.OnApplicationExiting
            End If
        End Sub

        Protected Overridable Sub CreateProperties()

            m_Properties = New PropertyTable
            m_Properties.Tag = Me

            AddHandler m_Properties.GetValue, AddressOf Me.Properties_GetValue
            AddHandler m_Properties.SetValue, AddressOf Me.Properties_SetValue

            BuildProperties(m_Properties)
        End Sub

        Public MustOverride Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

        Public Overridable Sub BuildPropertyDropDown(ByRef ctrlDropDown As System.Windows.Forms.Control)
        End Sub

        Protected Overridable Sub FormatDropDownList(ByRef lbList As ListBox)

            lbList.IntegralHeight = True ' resize to avoid partial items
            If lbList.ItemHeight > 0 Then
                If lbList.Height / lbList.ItemHeight < lbList.Items.Count Then
                    ' try to keep the listbox small but sufficient
                    Dim adjHei As Integer = lbList.Items.Count * lbList.ItemHeight
                    If adjHei > 200 Then adjHei = 200
                    lbList.Height = adjHei
                End If
            Else ' safeguard, although it shouldn't happen
                lbList.Height = 200
            End If

            Dim iMaxLength As Integer = -1
            For Each deEntry As TypeHelpers.DropDownEntry In lbList.Items
                If deEntry.Display.Length > iMaxLength OrElse iMaxLength = -1 Then
                    iMaxLength = deEntry.Display.Length
                End If
            Next

            If iMaxLength > -1 Then
                lbList.Width = CInt(iMaxLength * lbList.Font.SizeInPoints)
                If lbList.Width > 400 Then
                    lbList.Width = 400
                End If
            End If

            lbList.Sorted = True ' present in alphabetical order

        End Sub

        Protected Overridable Sub FormatDropDownTree(ByRef tvTree As Crownwood.DotNetMagic.Controls.TreeControl, ByVal iTotalNodes As Integer)

            If tvTree.MinimumNodeHeight > 0 Then
                If tvTree.Height / tvTree.MinimumNodeHeight < iTotalNodes Then
                    ' try to keep the listbox small but sufficient
                    Dim adjHei As Integer = (iTotalNodes + 2) * tvTree.MinimumNodeHeight
                    If adjHei > 500 Then adjHei = 500
                    tvTree.Height = adjHei
                End If
            Else ' safeguard, although it shouldn't happen
                tvTree.Height = 200
            End If

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
                    If e.Property.PropertyName.Trim.Length = 0 Then
                        e.Value = Nothing
                    Else
                        Throw New System.Exception("No property info returned for the property name '" & e.Property.PropertyName & "'.")
                    End If
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
            Dim origValue As Object
            Dim propInfo As System.Reflection.PropertyInfo

            Try
                propInfo = Me.GetType().GetProperty(e.Property.PropertyName)

                If Not propInfo Is Nothing Then
                    If propInfo.CanWrite Then
                        Dim lModificationCount As Long = Util.ModificationHistory.ModificationCount
                        origValue = GetOriginalValueForHistory(propInfo)

                        SignalBeforePropertyChanged(Me, propInfo)


                        m_bSetValueInProgress = True
                        propInfo.SetValue(Me, e.Value, Nothing)
                        m_bSetValueInProgress = False
                        Me.IsDirty = True

                        'Just in case, lets directly set the isdirty flag of the animatapplication. If for some reason the
                        'isdirty signal does not propogate up properly then this flag will still be set and things will 
                        'be saved correctly.
                        Util.Application.IsDirty = True

                        SignalAfterPropertyChanged(Me, propInfo)

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

        Public Overridable Sub FindChildrenOfType(ByVal tpTemplate As Type, ByVal colDataObjects As Collections.DataObjects)
            If tpTemplate Is Nothing OrElse Util.IsTypeOf(Me.GetType(), tpTemplate, False) Then
                colDataObjects.Add(Me)
            End If
        End Sub

        Public Overridable Function FindObjectByID(ByVal strID As String) As AnimatGUI.Framework.DataObject
            If Me.ID = strID Then
                Return Me
            Else
                Return Nothing
            End If
        End Function

        Public Overridable Sub ClearIsDirty()
            Me.IsDirty = False
        End Sub

        Protected Overridable Sub ScaleValueByUnit(ByVal strPropName As String, ByRef fltOrigValue As Single, ByVal fltScale As Single)
            Dim fltNewValue As Single = fltOrigValue * fltScale
            Me.ManualAddPropertyHistory(strPropName, fltOrigValue, fltNewValue)
            fltOrigValue = fltNewValue
        End Sub

        Protected Overridable Sub ScaleValueByUnit(ByVal strPropName As String, ByRef dblOrigValue As Double, ByVal fltScale As Single)
            Dim dblNewValue As Double = dblOrigValue * fltScale
            Me.ManualAddPropertyHistory(strPropName, dblOrigValue, dblNewValue)
            dblOrigValue = dblNewValue
        End Sub

        Protected Overridable Sub ScaleValueByUnit(ByVal strPropName As String, ByRef vOrigValue As AnimatGUI.Framework.Vec3d, ByVal fltScale As Single)
            Dim vNewValue As Vec3d = DirectCast(vOrigValue.Clone(vOrigValue.Parent, False, Nothing), Vec3d)
            vNewValue.X = vNewValue.X * fltScale
            vNewValue.Y = vNewValue.Y * fltScale
            vNewValue.Z = vNewValue.Z * fltScale

            Me.ManualAddPropertyHistory(strPropName, vOrigValue, vNewValue)
            vOrigValue = vNewValue
        End Sub

        Public Overridable Sub UnitsChanged(ByVal ePrevMass As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits, _
                                            ByVal eNewMass As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits, _
                                            ByVal fltMassChange As Single, _
                                            ByVal ePrevDistance As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits, _
                                            ByVal eNewDistance As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits, _
                                            ByVal fltDistanceChange As Single)

        End Sub

        'How Clone works
        'Clone is called on the object that is to be cloned. That object then creates a copy of itself and passes this 
        'object to the CloneInternal method. CloneInternal is a generic method that uses the underlying DataObject base class.
        'This allows each base class to have its own CloneInternal method that only needs to set the values that it knows about.
        'Once all of the params have been cloned we exit Clone Internal, and we have a clone version of the original part.
        'However, some objects like muscle have references to other parts that may have been cloned as well. we do not want the
        'reference to continue pointing to the old object, but instead it should point to the new cloned version. An example of this i
        'how a muscle keeps a list of attachment points. If we simply clone those points as is, then when the muscle is cloned it will
        'have exactly the same attachments specified, even though we may have cloned a number of those attachments, and they should be
        'hooked to those new version instead of the old. This is where the AfterClone method comes in. This method should ONLY be called
        'initially from the root object that is being cloned. Then it progresses down the tree of objects. This makes sure that the original
        'root and the new cloned root are passed into the method. This allows us to search the new cloned root to see if any of these other
        'parts, like the attachments, have also been cloned. If they have then we need to swap them out for the old parts. Also, whenever
        'a part is cloned the old ID it had before the clone is stored in the CloneID param. This allows us to know which parts came from where.

        Public MustOverride Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                           ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject

        Protected Overridable Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            m_strCloneID = doOriginal.m_strID
            If bCutData Then
                m_strID = doOriginal.m_strID
            End If
            m_strName = doOriginal.m_strName
            m_bEnabled = doOriginal.m_bEnabled
            'm_doParent = doOriginal.m_doParent
        End Sub

        Public Overridable Sub AfterClone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
        ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal doClone As AnimatGUI.Framework.DataObject)

        End Sub

        Public Overridable Sub EnsureFormActive()
        End Sub

        Public Overridable Sub LoadData(ByRef oXml As AnimatGUI.Interfaces.StdXml)
            Me.IsDirty = False
        End Sub

        Public Overridable Sub SaveData(ByRef oXml As AnimatGUI.Interfaces.StdXml)
        End Sub

        Public Overridable Sub InitializeAfterLoad()
            m_bIsInitialized = True
        End Sub

        Public Overridable Sub InitializeSimulationReferences()
            If m_doInterface Is Nothing AndAlso Not Util.Application.SimulationInterface Is Nothing AndAlso Util.Application.SimulationInterface.SimOpen Then
                m_doInterface = New Interfaces.DataObjectInterface(Util.Application.SimulationInterface, Me.ID)
            End If
        End Sub

        'Check to see if a simulation object exists that matches this object.
        Public Overridable Function SimObjectExists() As Boolean
            If Not Util.Application.SimulationInterface Is Nothing AndAlso Util.Application.SimulationInterface.SimOpen Then
                Return Util.Application.SimulationInterface.FindItem(Me.ID, False)
            End If

            Return False
        End Function

        Public Overridable Function SetSimData(ByVal sDataType As String, ByVal sValue As String, ByVal bThrowError As Boolean) As Boolean
            If Not m_doInterface Is Nothing Then
                Return m_doInterface.SetData(sDataType, sValue, bThrowError)
            End If
        End Function

        Public Overrides Function ToString() As String
            Return Me.Name
        End Function

        Public Overridable Sub InitAfterAppStart()

        End Sub

        Public Overridable Function Delete(Optional ByVal bAskToDelete As Boolean = True, Optional ByVal e As Crownwood.DotNetMagic.Controls.TGCloseRequestEventArgs = Nothing) As Boolean
            Me.RemoveWorksapceTreeView()
            Return False
        End Function

        Public Overridable Function DeleteSortCompare(ByVal doObj2 As DataObject) As Integer
            Return 0
        End Function

#Region " Copy Methods "

        Public Overridable Sub BeforeCopy()
        End Sub

        Public Overridable Sub AfterCopy()
        End Sub

        Public Overridable Function CanCopy(ByVal aryItems As ArrayList) As Boolean
            Return True
        End Function

        Public Overridable Sub AddToReplaceIDList(ByVal aryReplaceIDList As ArrayList)
            aryReplaceIDList.Add(Me.ID)
        End Sub

        Public Overridable Sub VerifyAfterPaste(ByVal aryItems As ArrayList)
        End Sub

#End Region

#Region " Undo-Redo Methods "

        Public Overridable Sub BeforeUndoRemove()
        End Sub

        Public Overridable Sub AfterUndoRemove()
        End Sub

        Public Overridable Sub BeforeRedoRemove()
        End Sub

        Public Overridable Sub AfterRedoRemove()
        End Sub

#End Region

#Region " Save Simulation Methods "

        Public Overridable Sub SaveSimulationXml(ByRef oXml As Interfaces.StdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")
        End Sub

        Public Overridable Function GetSimulationXml(ByVal strName As String, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing) As String

            Dim oXml As New AnimatGUI.Interfaces.StdXml
            oXml.AddElement("Root")
            SaveSimulationXml(oXml, nmParentControl, strName)

            Return oXml.Serialize()
        End Function

#End Region


#Region " Workspace Methods "

        Public Overridable Sub CreateWorkspaceTreeView(ByVal doParent As Framework.DataObject, ByVal doParentNode As Crownwood.DotNetMagic.Controls.Node)

            If m_tnWorkspaceNode Is Nothing Then
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

        Public Overridable Sub SelectItem(Optional ByVal bSelectMultiple As Boolean = False)

            Try
                If m_tnWorkspaceNode Is Nothing Then
                    Throw New System.Exception("Attempting to select an item before its workspace node is defined.")
                End If

                If Not Util.ProjectWorkspace.TreeView.SelectedNodes.Contains(m_tnWorkspaceNode) Then
                    Util.ProjectWorkspace.TreeView.SelectNode(m_tnWorkspaceNode, False, bSelectMultiple)
                ElseIf Not m_doInterface Is Nothing Then
                    m_doInterface.SelectItem(True, bSelectMultiple)
                End If

                SignalItemSelected(Me, bSelectMultiple)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Public Overridable Sub DeselectItem()
            Try
                If m_tnWorkspaceNode Is Nothing Then
                    Throw New System.Exception("Attempting to select an item before its workspace node is defined.")
                End If

                If Util.ProjectWorkspace.TreeView.SelectedNodes.Contains(m_tnWorkspaceNode) Then
                    Util.ProjectWorkspace.TreeView.DeselectNode(m_tnWorkspaceNode, True)
                End If

                If Not m_doInterface Is Nothing Then
                    m_doInterface.SelectItem(False, False)
                End If

                SignalItemDeselected(Me)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Public Overridable Function BeforeSelected() As Boolean
        End Function

        Public Overridable Sub AfterSelected()
        End Sub

        Public Overridable Sub BeforeDeselected()
        End Sub

        Public Overridable Sub AfterDeselected()
        End Sub

#End Region


#Region " Add-Remove to List Methods "

        Public Overridable Sub AddToSim(ByVal bThrowError As Boolean)
        End Sub

        Public Overridable Sub RemoveFromSim(ByVal bThrowError As Boolean)
            m_doInterface = Nothing
        End Sub

        Public Overridable Sub BeforeAddToList(ByVal bCallSimMethods As Boolean, ByVal bThrowError As Boolean)
            Me.SignalBeforeAddItem(Me)
            If bCallSimMethods Then AddToSim(bThrowError)
        End Sub

        Public Overridable Sub AfterAddToList(ByVal bCallSimMethods As Boolean, ByVal bThrowError As Boolean)
            Me.SignalAfterAddItem(Me)
        End Sub

        Public Overridable Sub BeforeRemoveFromList(ByVal bCallSimMethods As Boolean, ByVal bThrowError As Boolean)
            Me.SignalBeforeRemoveItem(Me)
            If bCallSimMethods Then RemoveFromSim(bThrowError)
        End Sub

        Public Overridable Sub AfterRemoveFromList(ByVal bCallSimMethods As Boolean, ByVal bThrowError As Boolean)
            Me.SignalAfterRemoveItem(Me)
        End Sub

#End Region

#End Region

#Region " Events "

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

        Public Event BeforePropertyChanged(ByRef doObject As AnimatGUI.Framework.DataObject, ByVal propInfo As System.Reflection.PropertyInfo)
        Public Event AfterPropertyChanged(ByRef doObject As AnimatGUI.Framework.DataObject, ByVal propInfo As System.Reflection.PropertyInfo)

        Public Event BeforeAddItem(ByRef doObject As AnimatGUI.Framework.DataObject)
        Public Event AfterAddItem(ByRef doObject As AnimatGUI.Framework.DataObject)

        Public Event BeforeRemoveItem(ByRef doObject As AnimatGUI.Framework.DataObject)
        Public Event AfterRemoveItem(ByRef doObject As AnimatGUI.Framework.DataObject)

        Public Event ItemSelected(ByRef doObject As AnimatGUI.Framework.DataObject, ByVal bSelectMultiple As Boolean)
        Public Event ItemDeselected(ByRef doObject As AnimatGUI.Framework.DataObject)

        Protected Sub SignalBeforePropertyChanged(ByRef doObject As AnimatGUI.Framework.DataObject, ByVal propInfo As System.Reflection.PropertyInfo)
            RaiseEvent BeforePropertyChanged(doObject, propInfo)
        End Sub

        Protected Sub SignalAfterPropertyChanged(ByRef doObject As AnimatGUI.Framework.DataObject, ByVal propInfo As System.Reflection.PropertyInfo)
            RaiseEvent AfterPropertyChanged(doObject, propInfo)
        End Sub

        Protected Sub SignalBeforeAddItem(ByRef doObject As AnimatGUI.Framework.DataObject)
            RaiseEvent BeforeAddItem(doObject)
        End Sub

        Protected Sub SignalAfterAddItem(ByRef doObject As AnimatGUI.Framework.DataObject)
            RaiseEvent AfterAddItem(doObject)
        End Sub

        Protected Sub SignalBeforeRemoveItem(ByRef doObject As AnimatGUI.Framework.DataObject)
            RaiseEvent BeforeRemoveItem(doObject)
        End Sub

        Protected Sub SignalAfterRemoveItem(ByRef doObject As AnimatGUI.Framework.DataObject)
            RaiseEvent AfterRemoveItem(doObject)
        End Sub

        Protected Sub SignalItemSelected(ByRef doObject As AnimatGUI.Framework.DataObject, ByVal bSelectMultiple As Boolean)
            RaiseEvent ItemSelected(doObject, bSelectMultiple)
        End Sub

        Protected Sub SignalItemDeselected(ByRef doObject As AnimatGUI.Framework.DataObject)
            RaiseEvent ItemDeselected(doObject)
        End Sub

        Protected Sub OnApplicationExiting()
            Try
                'Lets clear out the interface pointer becase the simulation will be shutdown and any pointers will no longer be valid.
                m_doInterface = Nothing
            Catch ex As Exception

            End Try
        End Sub

#End Region

#Region "Comparers"

        Protected Class NameComparer
            Implements IComparer

            ' Calls CaseInsensitiveComparer.Compare with the parameters reversed.
            Function Compare(ByVal x As [Object], ByVal y As [Object]) As Integer Implements IComparer.Compare
                If Not (TypeOf x Is DataObject AndAlso TypeOf y Is DataObject) Then Return 0

                Dim bnX As DataObject = DirectCast(x, DataObject)
                Dim bnY As DataObject = DirectCast(y, DataObject)

                Return New CaseInsensitiveComparer().Compare(bnX.Name, bnY.Name)

            End Function 'IComparer.Compare

        End Class

#End Region

    End Class

End Namespace



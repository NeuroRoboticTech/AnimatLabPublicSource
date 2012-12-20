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

Namespace DataObjects.Physical

    Public MustInherit Class BodyPart
        Inherits MovableItem

#Region " Delegates "

#End Region

#Region " Attributes "

        Protected m_ButtonImage As System.Drawing.Image

#End Region

#Region " Properties "


        'Type tells what type of bodypart (hinge, box, etc..
        Public MustOverride ReadOnly Property Type() As String

        'BodyPartType tells if it is a rigidbody or a joint.
        Public Overridable ReadOnly Property BodyPartType() As String
            Get
                Return Me.PartType.ToString
            End Get
        End Property

        'BodyPartName is a descriptive name used in the UI that tells the type of part.
        Public Overridable ReadOnly Property BodyPartName() As String
            Get
                Return Me.Type
            End Get
        End Property

        'AssemblyClass tells if it is a rigidbody or a joint.
        Public MustOverride ReadOnly Property PartType() As System.Type

        <Browsable(False)> _
        Public Overridable ReadOnly Property ParentStructure() As AnimatGUI.DataObjects.Physical.PhysicalStructure
            Get
                Dim doParent As AnimatGUI.DataObjects.Physical.PhysicalStructure
                Dim doTemp As AnimatGUI.DataObjects.Physical.BodyPart

                If Not Me.Parent Is Nothing AndAlso (TypeOf Me.Parent Is Physical.BodyPart OrElse TypeOf Me.Parent Is Physical.PhysicalStructure) Then
                    If TypeOf Me.Parent Is Physical.BodyPart Then
                        doTemp = DirectCast(Me.Parent, Physical.BodyPart)
                        Return doTemp.ParentStructure
                    ElseIf TypeOf Me.Parent Is Physical.PhysicalStructure Then
                        doParent = DirectCast(Me.Parent, Physical.PhysicalStructure)
                        Return doParent
                    End If
                End If

                Return Nothing
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property DragImageName() As String
            Get
                Return WorkspaceImageName()
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property ButtonImage() As System.Drawing.Image
            Get
                If m_ButtonImage Is Nothing AndAlso Me.ButtonImageName.Trim.Length > 0 Then
                    Dim myAssembly As System.Reflection.Assembly
                    myAssembly = System.Reflection.Assembly.Load(Me.AssemblyModuleName)
                    m_ButtonImage = ImageManager.LoadImage(myAssembly, Me.ButtonImageName)
                End If

                Return m_ButtonImage
            End Get
            Set(ByVal Value As System.Drawing.Image)
                If Not Value Is Nothing Then
                    m_ButtonImage = Value
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property ButtonImageName() As String
            Get
                Return ""
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property StructureID() As String
            Get
                If Not Me.ParentStructure Is Nothing Then
                    Return Me.ParentStructure.ID
                End If

                Return ""
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property AllowUserAdd() As Boolean
            Get
                Return True
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property TotalSubChildren() As Integer
            Get
                Return 0
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()
            If Not m_thDataTypes Is Nothing Then m_thDataTypes.ClearIsDirty()
        End Sub

        Protected Overrides Sub CloneInternal(ByVal doOriginal As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim bpOrig As BodyPart = DirectCast(doOriginal, BodyPart)

        End Sub

        'This method is called for each part type during the catalog of the modules. It sets up the 
        'list of part types that cannot be added to one another in the simulation object.
        Public Overridable Sub SetupPartTypesExclusions()
            Util.Application.AddPartTypeExclusion(GetType(Bodies.FluidPlane), Me.GetType)
            Util.Application.AddPartTypeExclusion(GetType(Bodies.Attachment), Me.GetType)
            Util.Application.AddPartTypeExclusion(GetType(Bodies.LinearHillMuscle), Me.GetType)
            Util.Application.AddPartTypeExclusion(GetType(Bodies.LinearHillStretchReceptor), Me.GetType)
            Util.Application.AddPartTypeExclusion(GetType(Bodies.Mouth), Me.GetType)
            Util.Application.AddPartTypeExclusion(GetType(Bodies.OdorSensor), Me.GetType)
            Util.Application.AddPartTypeExclusion(GetType(Bodies.Stomach), Me.GetType)
            Util.Application.AddPartTypeExclusion(GetType(Bodies.Spring), Me.GetType)
        End Sub

        Public Overridable Function FindBodyPart(ByVal strID As String) As BodyPart
            Return Nothing
        End Function

        Public Overridable Function FindBodyPartByName(ByVal strName As String) As BodyPart
            Return Nothing
        End Function

        Public Overridable Function FindBodyPartByCloneID(ByVal strId As String) As BodyPart
            Return Nothing
        End Function

        Public Overrides Function FindDragObject(ByVal strStructureName As String, ByVal strDataItemID As String, Optional ByVal bThrowError As Boolean = True) As DataObjects.DragObject

            Dim oStructure As Object = Util.Environment.FindStructureFromAll(strStructureName, bThrowError)
            If oStructure Is Nothing Then Return Nothing

            Dim doStructure As AnimatGUI.DataObjects.Physical.PhysicalStructure = DirectCast(oStructure, AnimatGUI.DataObjects.Physical.PhysicalStructure)
            Dim doPart As AnimatGUI.DataObjects.Physical.BodyPart = Nothing

            If Not doStructure Is Nothing Then
                doPart = doStructure.FindBodyPart(strDataItemID, False)
                If doPart Is Nothing AndAlso bThrowError Then
                    Throw New System.Exception("The drag object with id '" & strDataItemID & "' was not found.")
                End If
            End If

            Return doPart

        End Function

        Public Overridable Function CreateJointTreeView(ByRef tvTree As Crownwood.DotNetMagic.Controls.TreeControl, ByVal tnParent As Crownwood.DotNetMagic.Controls.Node, _
                                                        ByVal thSelectedPart As TypeHelpers.LinkedBodyPart) As Crownwood.DotNetMagic.Controls.Node
            Return Nothing
        End Function

        Public Overridable Function CreateRigidBodyTreeView(ByRef tvTree As Crownwood.DotNetMagic.Controls.TreeControl, ByVal tnParent As Crownwood.DotNetMagic.Controls.Node, _
                                                           ByVal thSelectedPart As TypeHelpers.LinkedBodyPart) As Crownwood.DotNetMagic.Controls.Node
            Return Nothing
        End Function

        Public Overrides Function Delete(Optional ByVal bAskToDelete As Boolean = True, Optional ByVal e As Crownwood.DotNetMagic.Controls.TGCloseRequestEventArgs = Nothing) As Boolean

            Try
                If bAskToDelete Then
                    If Util.ShowMessage("Are you certain that you want to permanently delete this " & _
                                        "body part and all its children?", "Delete Body Part", MessageBoxButtons.YesNo) <> DialogResult.Yes Then
                        Return True
                    End If
                End If

                Util.Application.AppIsBusy = True
                If Not Me.ParentStructure Is Nothing Then
                    If Not m_doInterface Is Nothing Then
                        RemoveHandler m_doInterface.OnPositionChanged, AddressOf Me.OnPositionChanged
                        RemoveHandler m_doInterface.OnRotationChanged, AddressOf Me.OnRotationChanged
                        RemoveHandler m_doInterface.OnSelectionChanged, AddressOf Me.OnSelectionChanged
                    End If

                    Me.ParentStructure.DeleteBodyPart(Me)

                    If Not Me.ParentStructure Is Nothing Then
                        Me.ParentStructure.SelectItem()
                    End If
                End If

                Return False
            Catch ex As Exception
                Throw ex
            Finally
                Util.Application.AppIsBusy = False
            End Try
        End Function

        Public Overridable Sub CopyBodyPart()

            Dim doClone As AnimatGUI.DataObjects.Physical.RigidBody = DirectCast(Me.Clone(Me.Parent, False, Me), AnimatGUI.DataObjects.Physical.RigidBody)

            Dim oXml As ManagedAnimatInterfaces.IStdXml = Util.Application.CreateStdXml()

            SaveSelected(oXml)

            'oXml.Save("C:\Projects\bin\Experiments\Copy.txt")
            Dim strXml As String = oXml.Serialize()

            Dim data As New System.Windows.Forms.DataObject
            data.SetData("AnimatLab.BodyPlan.XMLFormat", strXml)
            Clipboard.SetDataObject(data, True)

        End Sub

        Public Overridable Function SaveSelected(ByVal oXml As ManagedAnimatInterfaces.IStdXml) As Boolean

            oXml.AddElement("BodyPlan")

            'First lets sort the selected items into nodes and links and generate temp selected ids
            Dim aryReplaceIDs As New ArrayList

            'Call BeforeCopy first
            BeforeCopy()

            Me.AddToReplaceIDList(aryReplaceIDs)

            'Save the replaceme ID list
            oXml.AddChildElement("ReplaceIDList")
            oXml.IntoElem() 'Into ReplaceIDList Element
            For Each strID As String In aryReplaceIDs
                oXml.AddChildElement("ID", strID)
            Next
            oXml.OutOfElem() 'Outof ReplaceIDList Element

            SaveData(ParentStructure, oXml)

            AfterCopy()

            Return True
        End Function

        Public Overridable Sub VerifyCanBePasted()
        End Sub

        'This is called when creating a new body part. It sets the size of the part to its defaults.
        Public Overridable Sub SetDefaultSizes()

        End Sub

        Public Overrides Function WorkspaceTreeviewPopupMenu(ByRef tnSelectedNode As Crownwood.DotNetMagic.Controls.Node, ByVal ptPoint As System.Drawing.Point) As Boolean

            If tnSelectedNode Is m_tnWorkspaceNode Then
                Dim popup As New AnimatContextMenuStrip("AnimatGUI.DataObjects.Physical.BodyPart.WorkspaceTreeviewPopupMenu", Util.SecurityMgr)

                If Me.AllowStimulus AndAlso Me.CompatibleStimuli.Count > 0 Then
                    ' Create the menu items
                    Dim mcAddStimulus As New System.Windows.Forms.ToolStripMenuItem("Add Stimulus", Util.Application.ToolStripImages.GetImage("AnimatGUI.AddStimulus.gif"), New EventHandler(AddressOf Me.OnAddStimulus))
                    popup.Items.Add(mcAddStimulus)
                End If

                Dim mcSwapPart As New System.Windows.Forms.ToolStripMenuItem("Swap Part", Util.Application.ToolStripImages.GetImage("AnimatGUI.Swap.gif"), New EventHandler(AddressOf Me.OnSwapBodyPart))
                popup.Items.Add(mcSwapPart)

                Dim mcCut As New System.Windows.Forms.ToolStripMenuItem("Cut", Util.Application.ToolStripImages.GetImage("AnimatGUI.Cut.gif"), New EventHandler(AddressOf Me.OnCutBodyPart))
                Dim mcCopy As New System.Windows.Forms.ToolStripMenuItem("Copy", Util.Application.ToolStripImages.GetImage("AnimatGUI.Copy.gif"), New EventHandler(AddressOf Me.OnCopyBodyPart))
                Dim mcDelete As New System.Windows.Forms.ToolStripMenuItem("Delete Part", Util.Application.ToolStripImages.GetImage("AnimatGUI.Delete.gif"), New EventHandler(AddressOf Util.Application.OnDeleteFromWorkspace))
                popup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {mcCut, mcCopy, mcDelete})

                If Not Me.ParentStructure Is Nothing Then
                    Dim mcRelabel As New System.Windows.Forms.ToolStripMenuItem("Relabel Children", Util.Application.ToolStripImages.GetImage("AnimatGUI.Relabel.gif"), New EventHandler(AddressOf Me.OnRelabelChildren))
                    popup.Items.Add(mcRelabel)
                End If

                Dim mcSepExpand As New ToolStripSeparator()
                Dim mcExpandAll As New System.Windows.Forms.ToolStripMenuItem("Expand All", Util.Application.ToolStripImages.GetImage("AnimatGUI.Expand.gif"), New EventHandler(AddressOf Me.OnExpandAll))
                Dim mcCollapseAll As New System.Windows.Forms.ToolStripMenuItem("Collapse All", Util.Application.ToolStripImages.GetImage("AnimatGUI.Collapse.gif"), New EventHandler(AddressOf Me.OnCollapseAll))

                mcExpandAll.Tag = tnSelectedNode
                mcCollapseAll.Tag = tnSelectedNode

                ' Create the popup menu object
                popup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {mcSepExpand, mcExpandAll, mcCollapseAll})

                Util.ProjectWorkspace.ctrlTreeView.ContextMenuNode = popup

                Return True
            End If

            Return False
        End Function

        Public Overrides Sub SelectStimulusType()
            Dim frmStimulusType As New Forms.ExternalStimuli.SelectStimulusType
            frmStimulusType.CompatibleStimuli = Me.CompatibleStimuli

            If frmStimulusType.ShowDialog(Util.Application) = DialogResult.OK Then
                Dim doStimulus As DataObjects.ExternalStimuli.BodyPartStimulus = DirectCast(frmStimulusType.SelectedStimulus.Clone(Util.Application.FormHelper, False, Nothing), DataObjects.ExternalStimuli.BodyPartStimulus)
                doStimulus.PhysicalStructure = Me.ParentStructure
                doStimulus.BodyPart = Me

                Util.Simulation.NewStimuliIndex = Util.Simulation.NewStimuliIndex + 1
                doStimulus.Name = "Stimulus_" & Util.Simulation.NewStimuliIndex

                Util.Simulation.ProjectStimuli.Add(doStimulus.ID, doStimulus)
            End If
        End Sub

        Public MustOverride Sub RenameBodyParts(ByVal doStructure As AnimatGUI.DataObjects.Physical.PhysicalStructure)

        Public Overridable Sub ClearSelectedBodyParts()
            Me.DeselectItem()
        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

        End Sub

        Public Overridable Overloads Sub LoadData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.LoadData(oXml)

            oXml.IntoElem() 'Into BodyPart Element

            oXml.OutOfElem() 'Outof BodyPart Element

        End Sub

        Public Overridable Overloads Sub SaveData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.SaveData(oXml, Me.BodyPartType)

            oXml.IntoElem() 'Into Child Elemement

            oXml.AddChildElement("Type", Me.Type)
            oXml.AddChildElement("PartType", Me.PartType.ToString)

            oXml.OutOfElem() 'Outof BodyPart Element

        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, Me.BodyPartType)

            oXml.IntoElem() 'Into Child Elemement

            oXml.AddChildElement("Type", Me.Type)
            oXml.AddChildElement("PartType", Me.PartType.ToString)

            oXml.OutOfElem() 'Outof BodyPart Element
        End Sub

        Public Overrides Sub EnsureFormActive()
            If Not Me.ParentStructure Is Nothing AndAlso Me.ParentStructure.BodyEditor Is Nothing Then
                'Return Util.Application.EditBodyPlan(Me.ParentStructure)
            End If
        End Sub

        Public Overridable Sub BeforeAddBody()
            If Not Util.Application Is Nothing Then
                Util.Application.SignalBeforeAddBody(Me)
            End If
        End Sub

        Public Overridable Sub AfterAddBody()
            If Not Util.Application Is Nothing Then
                Util.Application.SignalAfterAddBody(Me)
            End If
        End Sub

        Public MustOverride Function SwapBodyPartList() As AnimatGUI.Collections.BodyParts
        Public MustOverride Sub SwapBodyPartCopy(ByVal doOriginal As AnimatGUI.DataObjects.Physical.BodyPart)

#End Region

#Region " Events "

        Protected Overridable Sub OnAddStimulus(ByVal sender As Object, ByVal e As System.EventArgs)

            Try

                Me.SelectStimulusType()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        'Protected Overridable Sub OnAddBodyPart(ByVal sender As Object, ByVal e As System.EventArgs)
        'End Sub

        Protected Overridable Sub OnRelabelChildren(ByVal sender As Object, ByVal e As System.EventArgs)
            Try
                Dim frmRelabel As New AnimatGUI.Forms.BodyPlan.Relabel

                frmRelabel.SelectedItem = Me
                frmRelabel.RootNode = Me
                If frmRelabel.ShowDialog = DialogResult.OK Then
                    Util.Relable(frmRelabel.Items, frmRelabel.txtMatch.Text, frmRelabel.txtReplace.Text)
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Overridable Sub OnSwapBodyPart(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                'Me.ParentStructure.BodyEditor.BodyView.SwapBodyPart(Me)
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Overridable Sub OnCopyBodyPart(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                Me.ParentStructure.BodyEditor.CopySelected()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Overridable Sub OnCutBodyPart(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                Me.ParentStructure.BodyEditor.CutSelected()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

    End Class

End Namespace

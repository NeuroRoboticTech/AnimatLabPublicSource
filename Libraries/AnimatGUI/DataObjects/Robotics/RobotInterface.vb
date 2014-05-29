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

        Public MustInherit Class RobotInterface
            Inherits DataObjects.DragObject

#Region " Attributes "

            Protected m_doOrganism As Physical.Organism
            Protected m_snPhysicsTimeStep As AnimatGUI.Framework.ScaledNumber
            Protected m_doPhysics As Physical.PhysicsEngine
            Protected m_bSynchSim As Boolean = True

            Protected m_aryIOControls As New Collections.SortedRobotIOControls(Me)

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

            Public Overrides ReadOnly Property WorkspaceImageName As String
                Get
                    Return "AnimatGUI.RobotInterface.gif"
                End Get
            End Property

            <Browsable(False)> _
            Public Overridable Property PhysicsTimeStep() As AnimatGUI.Framework.ScaledNumber
                Get
                    Return m_snPhysicsTimeStep
                End Get
                Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                    If Not Value Is Nothing Then
                        SetSimData("PhysicsTimeStep", Value.ActualValue.ToString(), True)
                        If Not Value Is Nothing Then m_snPhysicsTimeStep.CopyData(Value)
                    End If
                End Set
            End Property

            Public Overridable ReadOnly Property Organism() As Physical.Organism
                Get
                    Return m_doOrganism
                End Get
            End Property

            Public Overridable ReadOnly Property Physics As Physical.PhysicsEngine
                Get
                    Return m_doPhysics
                End Get
            End Property

            Public MustOverride ReadOnly Property PartType() As String

            Public Overridable ReadOnly Property IOControls As Collections.SortedRobotIOControls
                Get
                    Return m_aryIOControls
                End Get
            End Property

            Public Overridable Property SynchSim As Boolean
                Get
                    Return m_bSynchSim
                End Get
                Set(value As Boolean)
                    SetSimData("SynchSim", value.ToString, True)
                    m_bSynchSim = value
                End Set
            End Property

#End Region

#Region " Methods "

            Public Sub New(ByVal doParent As Framework.DataObject)
                MyBase.New(doParent)

                If Not doParent Is Nothing AndAlso Util.IsTypeOf(doParent.GetType(), GetType(Physical.Organism), False) Then
                    m_doOrganism = DirectCast(doParent, Physical.Organism)
                End If

                m_strName = "RobotInterface"
                m_snPhysicsTimeStep = New AnimatGUI.Framework.ScaledNumber(Me, "PhysicsTimeStep", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "seconds", "s")
            End Sub

            Public Overrides Sub ClearIsDirty()
                MyBase.ClearIsDirty()
                m_snPhysicsTimeStep.ClearIsDirty()
                m_aryIOControls.ClearIsDirty()
            End Sub

            Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                                ByVal doRoot As AnimatGUI.Framework.DataObject)
                MyBase.CloneInternal(doOriginal, bCutData, doRoot)

                Dim OrigNode As RobotInterface = DirectCast(doOriginal, RobotInterface)

                m_snPhysicsTimeStep = DirectCast(OrigNode.m_snPhysicsTimeStep.Clone(Me, bCutData, doRoot), ScaledNumber)
                m_aryIOControls = DirectCast(OrigNode.m_aryIOControls.Clone(Me, bCutData, doRoot), AnimatGUI.Collections.SortedRobotIOControls)
                m_bSynchSim = OrigNode.m_bSynchSim
            End Sub

            Public Overridable Sub GenerateStandaloneSimFile()

                Dim frmExport As New Forms.ExportStandaloneSim()
                frmExport.Physics = Me.Physics
                If frmExport.ShowDialog() <> Windows.Forms.DialogResult.OK Then
                    Return
                End If

                Dim oXml As ManagedAnimatInterfaces.IStdXml = Util.Application.SaveStandAlone(True, True, True, False, frmExport.Physics, Me)
                oXml.Save(Util.Application.ProjectPath & "\" & frmExport.txtProjectName.Text)

                Util.ShowMessage("Robot simulation file for " & m_doOrganism.Name & " created successfully.", "Exported simulation file", MessageBoxButtons.OK)
            End Sub


#Region " DataObject Methods "


#Region " Workspace TreeView "

            Public Overrides Sub CreateWorkspaceTreeView(ByVal doParent As Framework.DataObject, _
                                                           ByVal tnParentNode As Crownwood.DotNetMagic.Controls.Node, _
                                                           Optional ByVal bRootObject As Boolean = False)
                MyBase.CreateWorkspaceTreeView(doParent, tnParentNode, bRootObject)

                For Each deEntry As DictionaryEntry In m_aryIOControls
                    Dim nmControl As RobotIOControl = DirectCast(deEntry.Value, RobotIOControl)
                    nmControl.CreateWorkspaceTreeView(Me, m_tnWorkspaceNode)
                Next
            End Sub

            Public Overrides Function CreateObjectListTreeView(ByVal doParent As Framework.DataObject, _
                                                           ByVal tnParentNode As Crownwood.DotNetMagic.Controls.Node, _
                                                           ByVal mgrImageList As AnimatGUI.Framework.ImageManager) As Crownwood.DotNetMagic.Controls.Node
                Dim tnNode As Crownwood.DotNetMagic.Controls.Node = MyBase.CreateObjectListTreeView(doParent, tnParentNode, mgrImageList)

                For Each deEntry As DictionaryEntry In m_aryIOControls
                    Dim nmControl As RobotIOControl = DirectCast(deEntry.Value, RobotIOControl)
                    nmControl.CreateObjectListTreeView(Me, tnNode, mgrImageList)
                Next

                Return tnNode
            End Function

            Public Overrides Function WorkspaceTreeviewPopupMenu(ByRef tnSelectedNode As Crownwood.DotNetMagic.Controls.Node, ByVal ptPoint As System.Drawing.Point) As Boolean

                If tnSelectedNode Is m_tnWorkspaceNode Then
                    ' Create the popup menu object
                    Dim popup As New AnimatContextMenuStrip("AnimatGUI.DataObjects.Robotics.RobotInterface.WorkspaceTreeviewPopupMenu", Util.SecurityMgr)

                    Dim mcAddIOControl As New System.Windows.Forms.ToolStripMenuItem("Add IO Control", Util.Application.ToolStripImages.GetImage("AnimatGUI.AddRobotIOControl.gif"), New EventHandler(AddressOf Me.OnAddRobotIOControl))
                    Dim mcDelete As New System.Windows.Forms.ToolStripMenuItem("Delete Robot Interface", Util.Application.ToolStripImages.GetImage("AnimatGUI.Delete.gif"), New EventHandler(AddressOf Util.Application.OnDeleteFromWorkspace))

                    popup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {mcAddIOControl, mcDelete})

                    Util.ProjectWorkspace.ctrlTreeView.ContextMenuNode = popup

                    Return True
                End If

                Return False
            End Function

            Public Overrides Function CreateDataItemTreeView(ByVal frmDataItem As Forms.Tools.SelectDataItem, ByVal tnParent As Crownwood.DotNetMagic.Controls.Node, ByVal tpTemplatePartType As Type) As Crownwood.DotNetMagic.Controls.Node
                Dim tnNode As Crownwood.DotNetMagic.Controls.Node = MyBase.CreateDataItemTreeView(frmDataItem, tnParent, tpTemplatePartType)

                For Each deEntry As DictionaryEntry In m_aryIOControls
                    Dim nmControl As RobotIOControl = DirectCast(deEntry.Value, RobotIOControl)
                    nmControl.CreateDataItemTreeView(frmDataItem, tnNode, tpTemplatePartType)
                Next

            End Function

#End Region

#Region " Find Methods "

            Public Overrides Sub FindChildrenOfType(ByVal tpTemplate As System.Type, ByVal colDataObjects As Collections.DataObjects)
                MyBase.FindChildrenOfType(tpTemplate, colDataObjects)
                m_aryIOControls.FindChildrenOfType(tpTemplate, colDataObjects)
            End Sub

            Public Overrides Function FindObjectByID(ByVal strID As String) As Framework.DataObject

                Dim doObject As AnimatGUI.Framework.DataObject = MyBase.FindObjectByID(strID)
                If doObject Is Nothing AndAlso Not m_aryIOControls Is Nothing Then doObject = m_aryIOControls.FindObjectByID(strID)
                Return doObject

            End Function

            Public Overrides Function FindDragObject(strStructureName As String, strDataItemID As String, Optional bThrowError As Boolean = True) As DragObject
                Throw New System.Exception("FindDragObject not implemented")
            End Function

#End Region

            Public Overrides Sub InitializeSimulationReferences(Optional ByVal bShowError As Boolean = True)
                MyBase.InitializeSimulationReferences(bShowError)
                m_aryIOControls.InitializeSimulationReferences(bShowError)
            End Sub

#Region " Add-Remove to List Methods "

            Public Overrides Sub AddToSim(ByVal bThrowError As Boolean, Optional ByVal bDoNotInit As Boolean = False)
                If Not Me.Parent Is Nothing Then
                    Util.Application.SimulationInterface.AddItem(Me.Parent.ID, "RobotInterface", Me.ID, Me.GetSimulationXml("RobotInterface"), bThrowError, bDoNotInit)
                    InitializeSimulationReferences()
                End If
            End Sub

            Public Overrides Sub RemoveFromSim(ByVal bThrowError As Boolean)
                If Not Me.Parent Is Nothing AndAlso Not m_doInterface Is Nothing Then
                    Util.Application.SimulationInterface.RemoveItem(Me.Parent.ID, "RobotInterface", Me.ID, bThrowError)
                End If
                m_doInterface = Nothing
            End Sub

#End Region

            Public Overrides Function Delete(Optional ByVal bAskToDelete As Boolean = True, Optional ByVal e As Crownwood.DotNetMagic.Controls.TGCloseRequestEventArgs = Nothing) As Boolean
                Try
                    If bAskToDelete AndAlso Util.ShowMessage("Are you sure you want to remove the robot interface?", _
                                                             "Remove Robot Interface", MessageBoxButtons.YesNo) <> DialogResult.Yes Then
                        Return False
                    End If

                    Util.Application.AppIsBusy = True
                    Me.RemoveFromSim(True)
                    m_doOrganism.RobotInterface = Nothing
                    Me.RemoveWorksapceTreeView()
                    If Not Me.Parent Is Nothing Then Me.Parent.IsDirty = True
                    Return True
                Catch ex As Exception
                    Throw ex
                Finally
                    Util.Application.AppIsBusy = False
                End Try
            End Function

            Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Name", Me.ID.GetType(), "Name", _
                                            "Properties", "Name", Me.Name))

                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("ID", Me.ID.GetType(), "ID", _
                                            "Properties", "ID", Me.ID, True))

                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Enabled", GetType(Boolean), "Enabled", _
                                            "Properties", "Determines if this controller is enabled or not.", m_bEnabled))

                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Synch Sim", GetType(Boolean), "SynchSim", _
                                            "Properties", "Determines whether or not the IO update of the simulation is synched with the time step of the robot.", m_bSynchSim))

                Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = m_snPhysicsTimeStep.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Physics Time Step", pbNumberBag.GetType(), "PhysicsTimeStep", _
                                            "Properties", "Overrides the physics step set in the simulation for the robot. " & _
                                            "This allows you to set a different physics time step for the robotic simulation.", pbNumberBag, _
                                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))
            End Sub

            Public Overrides Sub InitializeAfterLoad()
                MyBase.InitializeAfterLoad()

                Dim ioControl As RobotIOControl
                For Each deEntry As DictionaryEntry In m_aryIOControls
                    ioControl = DirectCast(deEntry.Value, RobotIOControl)
                    ioControl.InitializeAfterLoad()
                Next

            End Sub

            Public Overridable Sub LoadIOControls(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

                Try
                    Dim oMod As Object
                    Dim ioControl As RobotIOControl

                    Util.Application.AppStatusText = "Loading " & Me.TypeName & " " & Me.Name & " IO controls"

                    m_aryIOControls.Clear()

                    If oXml.FindChildElement("IOControls", False) Then
                        oXml.IntoChildElement("IOControls")
                        Dim iCount As Integer = oXml.NumberOfChildren() - 1
                        For iIndex As Integer = 0 To iCount
                            'If the module cannot be found then do not die because of this, just keep trying to go on.
                            oMod = Util.LoadClass(oXml, iIndex, Me, False)
                            If Not oMod Is Nothing Then
                                ioControl = DirectCast(oMod, RobotIOControl)
                                ioControl.LoadData(oXml)
                                m_aryIOControls.Add(ioControl.ID, ioControl)
                            End If
                        Next
                        oXml.OutOfElem() 'Outof IOControls Element
                    End If

                Catch ex As System.Exception
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End Try

            End Sub

            Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

                oXml.IntoElem()  'Into RobotInterface Element

                m_strName = oXml.GetChildString("Name", Me.Name)
                m_strID = oXml.GetChildString("ID", Me.ID)
                m_bEnabled = oXml.GetChildBool("Enabled", m_bEnabled)
                m_bSynchSim = oXml.GetChildBool("SynchSim", m_bSynchSim)

                m_snPhysicsTimeStep.LoadData(oXml, "PhysicsTimeStep")

                LoadIOControls(oXml)

                oXml.OutOfElem()

            End Sub


            Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

                oXml.AddChildElement("RobotInterface")
                oXml.IntoElem()

                oXml.AddChildElement("Name", Me.Name)
                oXml.AddChildElement("ID", Me.ID)
                oXml.AddChildElement("AssemblyFile", Me.AssemblyFile)
                oXml.AddChildElement("ClassName", Me.ClassName)
                oXml.AddChildElement("SynchSim", Me.SynchSim)

                m_snPhysicsTimeStep.SaveData(oXml, "PhysicsTimeStep")

                Util.Application.AppStatusText = "Saving " & Me.TypeName & " " & Me.Name & " IO controls"
                oXml.AddChildElement("IOControls")
                oXml.IntoElem()
                Dim ioControl As RobotIOControl
                For Each deEntry As DictionaryEntry In m_aryIOControls
                    ioControl = DirectCast(deEntry.Value, RobotIOControl)
                    ioControl.SaveData(oXml)
                Next
                oXml.OutOfElem() 'Outof IOControls

                oXml.OutOfElem()

            End Sub

            Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")

                oXml.AddChildElement("RobotInterface")
                oXml.IntoElem()

                oXml.AddChildElement("Name", Me.Name)
                oXml.AddChildElement("ID", Me.ID)
                oXml.AddChildElement("Type", Me.PartType)
                oXml.AddChildElement("ModuleName", Me.ModuleFilename)

                m_snPhysicsTimeStep.SaveSimulationXml(oXml, Me, "PhysicsTimeStep")
                oXml.AddChildElement("SynchSim", Me.SynchSim)

                Util.Application.AppStatusText = "Saving " & Me.TypeName & " " & Me.Name & " IO controls"
                oXml.AddChildElement("IOControls")
                oXml.IntoElem()
                Dim ioControl As RobotIOControl
                For Each deEntry As DictionaryEntry In m_aryIOControls
                    ioControl = DirectCast(deEntry.Value, RobotIOControl)
                    ioControl.SaveSimulationXml(oXml, Me)
                Next
                oXml.OutOfElem() 'Outof IOControls

                oXml.OutOfElem()

            End Sub

#End Region

#End Region

#Region " Events "

            Protected Overridable Sub OnAddRobotIOControl(ByVal sender As Object, ByVal e As System.EventArgs)
                Try
                    Dim frmSelInterface As New Forms.SelectObject()
                    frmSelInterface.Objects = Util.Application.RobotIOControls
                    frmSelInterface.PartTypeName = "Robot IO Controls"

                    If frmSelInterface.ShowDialog() = DialogResult.OK Then
                        'Then create the new one.
                        Dim doIOControl As Robotics.RobotIOControl = DirectCast(frmSelInterface.Selected.Clone(Me, False, Nothing), Robotics.RobotIOControl)
                        doIOControl.CreateWorkspaceTreeView(Me, m_tnWorkspaceNode)
                        m_aryIOControls.Add(doIOControl.ID, doIOControl)
                    End If

                Catch ex As System.Exception
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End Try
            End Sub

#End Region

        End Class

    End Namespace
End Namespace

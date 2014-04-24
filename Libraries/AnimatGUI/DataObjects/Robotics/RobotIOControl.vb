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

        Public MustInherit Class RobotIOControl
            Inherits Framework.DataObject

#Region " Attributes "

            Protected m_doOrganism As Physical.Organism
            Protected m_doPhysics As Physical.PhysicsEngine
            Protected m_doParentInterface As RobotInterface

            Protected m_aryAvailablePartTypes As New Collections.RobotPartInterfaces(Me)
            Protected m_aryParts As New Collections.SortedRobotPartInterfaces(Me)

#End Region

#Region " Properties "

            Public Overrides ReadOnly Property WorkspaceImageName As String
                Get
                    Return "AnimatGUI.RobotIO.gif"
                End Get
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

            Public Overridable Property ParentInterface As RobotInterface
                Get
                    Return m_doParentInterface
                End Get
                Set(value As RobotInterface)
                    m_doParentInterface = value
                End Set
            End Property

            Public MustOverride ReadOnly Property PartType() As String

            Public Overridable ReadOnly Property AvailablePartTypes As Collections.RobotPartInterfaces
                Get
                    Return m_aryAvailablePartTypes
                End Get
            End Property

            Public Overridable ReadOnly Property Parts As Collections.SortedRobotPartInterfaces
                Get
                    Return m_aryParts
                End Get
            End Property

#End Region

#Region " Methods "

            Public Sub New(ByVal doParent As Framework.DataObject)
                MyBase.New(doParent)

                If Not doParent Is Nothing AndAlso Util.IsTypeOf(doParent.GetType(), GetType(RobotInterface), False) Then
                    m_doParentInterface = DirectCast(doParent, RobotInterface)
                    m_doOrganism = m_doParentInterface.Organism
                End If
                m_strName = "RobotIOControl"
            End Sub

            Public Overrides Sub ClearIsDirty()
                MyBase.ClearIsDirty()

                m_aryParts.ClearIsDirty()
            End Sub

            Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                                ByVal doRoot As AnimatGUI.Framework.DataObject)
                MyBase.CloneInternal(doOriginal, bCutData, doRoot)

                Dim OrigNode As RobotIOControl = DirectCast(doOriginal, RobotIOControl)

                m_aryParts = DirectCast(OrigNode.m_aryParts.Clone(Me, bCutData, doRoot), Collections.SortedRobotPartInterfaces)
            End Sub

#Region " DataObject Methods "


#Region " Workspace TreeView "

            Public Overrides Sub CreateWorkspaceTreeView(ByVal doParent As Framework.DataObject, _
                                                           ByVal tnParentNode As Crownwood.DotNetMagic.Controls.Node, _
                                                           Optional ByVal bRootObject As Boolean = False)
                MyBase.CreateWorkspaceTreeView(doParent, tnParentNode, bRootObject)

                For Each deEntry As DictionaryEntry In m_aryParts
                    Dim doPart As RobotPartInterface = DirectCast(deEntry.Value, RobotPartInterface)
                    doPart.CreateWorkspaceTreeView(Me, m_tnWorkspaceNode)
                Next
            End Sub

            Public Overrides Function CreateObjectListTreeView(ByVal doParent As Framework.DataObject, _
                                                           ByVal tnParentNode As Crownwood.DotNetMagic.Controls.Node, _
                                                           ByVal mgrImageList As AnimatGUI.Framework.ImageManager) As Crownwood.DotNetMagic.Controls.Node
                Dim tnNode As Crownwood.DotNetMagic.Controls.Node = MyBase.CreateObjectListTreeView(doParent, tnParentNode, mgrImageList)

                For Each deEntry As DictionaryEntry In m_aryParts
                    Dim doPart As RobotPartInterface = DirectCast(deEntry.Value, RobotPartInterface)
                    doPart.CreateObjectListTreeView(Me, tnNode, mgrImageList)
                Next

                Return tnNode
            End Function

            Public Overrides Function WorkspaceTreeviewPopupMenu(ByRef tnSelectedNode As Crownwood.DotNetMagic.Controls.Node, ByVal ptPoint As System.Drawing.Point) As Boolean

                If tnSelectedNode Is m_tnWorkspaceNode Then
                    ' Create the popup menu object
                    Dim popup As New AnimatContextMenuStrip("AnimatGUI.DataObjects.Robotics.RobotInterface.WorkspaceTreeviewPopupMenu", Util.SecurityMgr)

                    Dim mcAddPart As New System.Windows.Forms.ToolStripMenuItem("Add Part", Util.Application.ToolStripImages.GetImage("AnimatGUI.AddRobotInterface.gif"), New EventHandler(AddressOf Me.OnAddRobotPart))
                    Dim mcDelete As New System.Windows.Forms.ToolStripMenuItem("Delete IO Control", Util.Application.ToolStripImages.GetImage("AnimatGUI.Delete.gif"), New EventHandler(AddressOf Util.Application.OnDeleteFromWorkspace))

                    popup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {mcAddPart, mcDelete})

                    Util.ProjectWorkspace.ctrlTreeView.ContextMenuNode = popup

                    Return True
                End If

                Return False
            End Function

#End Region


#Region " Find Methods "

            Public Overrides Sub FindChildrenOfType(ByVal tpTemplate As System.Type, ByVal colDataObjects As Collections.DataObjects)
                MyBase.FindChildrenOfType(tpTemplate, colDataObjects)
                m_aryParts.FindChildrenOfType(tpTemplate, colDataObjects)
            End Sub

            Public Overrides Function FindObjectByID(ByVal strID As String) As Framework.DataObject

                Dim doObject As AnimatGUI.Framework.DataObject = MyBase.FindObjectByID(strID)
                If doObject Is Nothing AndAlso Not m_aryParts Is Nothing Then doObject = m_aryParts.FindObjectByID(strID)
                Return doObject

            End Function

#End Region

            Public Overrides Function Delete(Optional ByVal bAskToDelete As Boolean = True, Optional ByVal e As Crownwood.DotNetMagic.Controls.TGCloseRequestEventArgs = Nothing) As Boolean
                Try
                    If bAskToDelete AndAlso Util.ShowMessage("Are you sure you want to remove the robot IO control and all children?", _
                                                             "Remove Robot Interface", MessageBoxButtons.YesNo) <> DialogResult.Yes Then
                        Return False
                    End If

                    Util.Application.AppIsBusy = True
                    m_doParentInterface.IOControls.Remove(Me.ID)
                    Me.RemoveWorksapceTreeView()
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
            End Sub

            Public Overrides Sub InitializeAfterLoad()
                MyBase.InitializeAfterLoad()

                Dim doPart As RobotPartInterface
                For Each deEntry As DictionaryEntry In m_aryParts
                    doPart = DirectCast(deEntry.Value, RobotPartInterface)
                    doPart.InitializeAfterLoad()
                Next

            End Sub

            Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

                oXml.IntoElem()  'Into RobotInterface Element

                m_strName = oXml.GetChildString("Name", Me.Name)
                m_strID = oXml.GetChildString("ID", Me.ID)
                m_bEnabled = oXml.GetChildBool("Enabled", m_bEnabled)

                m_aryParts.Clear()

                If oXml.FindChildElement("Parts", False) Then
                    Dim oMod As Object
                    Dim doPart As RobotPartInterface

                    oXml.IntoChildElement("Parts")
                    Dim iCount As Integer = oXml.NumberOfChildren() - 1
                    For iIndex As Integer = 0 To iCount
                        'If the module cannot be found then do not die because of this, just keep trying to go on.
                        oMod = Util.LoadClass(oXml, iIndex, Me, False)
                        If Not oMod Is Nothing Then
                            doPart = DirectCast(oMod, RobotPartInterface)
                            doPart.LoadData(oXml)
                            m_aryParts.Add(doPart.ID, doPart)
                        End If
                    Next
                    oXml.OutOfElem() 'Outof Parts Element
                End If

                oXml.OutOfElem()

            End Sub


            Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

                oXml.AddChildElement("RobotIO")
                oXml.IntoElem()

                oXml.AddChildElement("Name", Me.Name)
                oXml.AddChildElement("ID", Me.ID)
                oXml.AddChildElement("AssemblyFile", Me.AssemblyFile)
                oXml.AddChildElement("ClassName", Me.ClassName)

                oXml.AddChildElement("Parts")
                oXml.IntoElem()
                Dim doPart As RobotPartInterface
                For Each deEntry As DictionaryEntry In m_aryParts
                    doPart = DirectCast(deEntry.Value, RobotPartInterface)
                    doPart.SaveData(oXml)
                Next
                oXml.OutOfElem() 'Outof Parts

                oXml.OutOfElem()

            End Sub

            Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")

                oXml.AddChildElement("RobotIO")
                oXml.IntoElem()

                oXml.AddChildElement("Name", Me.Name)
                oXml.AddChildElement("ID", Me.ID)
                oXml.AddChildElement("Type", Me.PartType)
                oXml.AddChildElement("ModuleName", Me.ModuleName)

                oXml.AddChildElement("Parts")
                oXml.IntoElem()
                Dim doPart As RobotPartInterface
                For Each deEntry As DictionaryEntry In m_aryParts
                    doPart = DirectCast(deEntry.Value, RobotPartInterface)
                    doPart.SaveSimulationXml(oXml, Me)
                Next
                oXml.OutOfElem() 'Outof Parts

                oXml.OutOfElem()

            End Sub

#End Region

#End Region

#Region " Events "

            Protected Overridable Sub OnAddRobotPart(ByVal sender As Object, ByVal e As System.EventArgs)
                Try
                    Dim frmSelInterface As New Forms.SelectObject()
                    frmSelInterface.Objects = Me.AvailablePartTypes
                    frmSelInterface.PartTypeName = "Robot Part"

                    If frmSelInterface.ShowDialog() = DialogResult.OK Then
                        'Then create the new one.
                        Dim doPart As Robotics.RobotPartInterface = DirectCast(frmSelInterface.Selected.Clone(Me, False, Nothing), Robotics.RobotPartInterface)
                        doPart.CreateWorkspaceTreeView(Me, m_tnWorkspaceNode)
                        m_aryParts.Add(doPart.ID, doPart)
                    End If

                Catch ex As System.Exception
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End Try
            End Sub

#End Region

        End Class

    End Namespace
End Namespace

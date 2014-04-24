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
            Inherits Framework.DataObject

#Region " Attributes "

            Protected m_bpParentIO As RobotIOControl

            Protected m_thLinkedPart As AnimatGUI.TypeHelpers.LinkedBodyPart

            'Only used during loading
            Protected m_strLinkedBodyPartID As String = ""

            Protected m_doOrganism As Physical.Organism

            Protected m_doPruner As New AnimatGUI.TypeHelpers.RobotTreePruner(Me)

#End Region

#Region " Properties "

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
            Public Overridable Property LinkedPart() As AnimatGUI.TypeHelpers.LinkedBodyPart
                Get
                    Return m_thLinkedPart
                End Get
                Set(ByVal Value As AnimatGUI.TypeHelpers.LinkedBodyPart)
                    Dim thPrevLinked As AnimatGUI.TypeHelpers.LinkedBodyPart = m_thLinkedPart

                    If Not Value Is Nothing AndAlso Not Value.BodyPart Is Nothing AndAlso _
                       Not Value.BodyPart.RobotPartInterface Is Nothing AndAlso _
                       Not Value.BodyPart.RobotPartInterface Is Me Then
                        Throw New System.Exception("The part '" & Value.BodyPart.Name & "' is already associated with robot interface '" & Value.BodyPart.RobotPartInterface.Name & "'.")
                    End If

                    DiconnectLinkedPartEvents()
                    m_thLinkedPart = Value
                    ConnectLinkedPartEvents()

                End Set
            End Property

            Public Overridable Property Organism() As Physical.Organism
                Get
                    Return m_doOrganism
                End Get
                Set(value As Physical.Organism)
                    m_doOrganism = value

                    If Not m_doOrganism Is Nothing Then
                        Me.LinkedPart = CreateBodyPartList(m_doOrganism, Nothing, CompatiblePartType())
                    End If

                End Set
            End Property

            Public MustOverride ReadOnly Property CompatiblePartType() As Type

#End Region

#Region " Methods "

            Public Sub New(ByVal doParent As Framework.DataObject)
                MyBase.New(doParent)

                If Not doParent Is Nothing AndAlso Util.IsTypeOf(doParent.GetType(), GetType(RobotIOControl), False) Then
                    m_bpParentIO = DirectCast(doParent, RobotIOControl)
                    Me.Organism = m_bpParentIO.Organism
                End If

                m_thLinkedPart = CreateBodyPartList(m_doOrganism, Nothing, CompatiblePartType())

            End Sub

            Public Overrides Sub ClearIsDirty()
                MyBase.ClearIsDirty()
                m_thLinkedPart.ClearIsDirty()
            End Sub

            Public Overridable Function IsCompatibleWithPartType(ByVal bpPart As Physical.BodyPart) As Boolean
                Return False
            End Function

            Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                                ByVal doRoot As AnimatGUI.Framework.DataObject)
                MyBase.CloneInternal(doOriginal, bCutData, doRoot)

                Dim OrigNode As RobotPartInterface = DirectCast(doOriginal, RobotPartInterface)

                DiconnectLinkedPartEvents()
                m_thLinkedPart = DirectCast(OrigNode.m_thLinkedPart.Clone(Me, bCutData, doRoot), TypeHelpers.LinkedBodyPart)

            End Sub

#Region " DataObject Methods "

            Public Overrides Function WorkspaceTreeviewPopupMenu(ByRef tnSelectedNode As Crownwood.DotNetMagic.Controls.Node, ByVal ptPoint As System.Drawing.Point) As Boolean

                If tnSelectedNode Is m_tnWorkspaceNode Then
                    Dim mcDelete As New System.Windows.Forms.ToolStripMenuItem("Delete robot part interface", Util.Application.ToolStripImages.GetImage("AnimatGUI.Delete.gif"), New EventHandler(AddressOf Util.Application.OnDeleteFromWorkspace))

                    ' Create the popup menu object
                    Dim popup As New AnimatContextMenuStrip("AnimatGUI.DataObjects.Robotics.RobotPartInterface.WorkspaceTreeviewPopupMenu", Util.SecurityMgr)
                    popup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {mcDelete})

                    Util.ProjectWorkspace.ctrlTreeView.ContextMenuNode = popup

                    Return True
                End If

                Return False
            End Function

            Protected Overridable Sub ConnectLinkedPartEvents()
                DiconnectLinkedPartEvents()

                If Not m_thLinkedPart Is Nothing AndAlso Not m_thLinkedPart.BodyPart Is Nothing Then
                    m_thLinkedPart.BodyPart.RobotPartInterface = Me
                    AddHandler m_thLinkedPart.BodyPart.AfterRemoveItem, AddressOf Me.OnAfterRemoveLinkedPart
                End If
            End Sub

            Protected Overridable Sub DiconnectLinkedPartEvents()
                If Not m_thLinkedPart Is Nothing AndAlso Not m_thLinkedPart.BodyPart Is Nothing Then
                    m_thLinkedPart.BodyPart.RobotPartInterface = Nothing
                    RemoveHandler m_thLinkedPart.BodyPart.AfterRemoveItem, AddressOf Me.OnAfterRemoveLinkedPart
                End If
            End Sub

            Protected Overridable Overloads Function CreateBodyPartList(ByVal doStruct As Physical.PhysicalStructure, ByVal doBodyPart As Physical.BodyPart, ByVal tpBodyPartType As System.Type) As TypeHelpers.LinkedBodyPart
                Return New AnimatGUI.TypeHelpers.LinkedBodyPartTree(doStruct, doBodyPart, tpBodyPartType, m_doPruner)
            End Function

            Public Overrides Function Delete(Optional ByVal bAskToDelete As Boolean = True, Optional ByVal e As Crownwood.DotNetMagic.Controls.TGCloseRequestEventArgs = Nothing) As Boolean
                Try
                    If bAskToDelete AndAlso Util.ShowMessage("Are you sure you want to remove the robot part interface?", _
                                                             "Remove Robot Interface", MessageBoxButtons.YesNo) <> DialogResult.Yes Then
                        Return False
                    End If

                    Util.Application.AppIsBusy = True
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

                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Linked Part", m_thLinkedPart.GetType, "LinkedPart", _
                                            "Properties", "Associates this robot interface to an ID of a part that exists within the body of the organism.", m_thLinkedPart, _
                                            GetType(AnimatGUI.TypeHelpers.DropDownTreeEditor), _
                                            GetType(AnimatGUI.TypeHelpers.LinkedBodyPartTypeConverter)))

            End Sub

            Public Overrides Sub InitializeAfterLoad()

                Try
                    MyBase.InitializeAfterLoad()

                    If m_bIsInitialized Then
                        Dim bpPart As AnimatGUI.DataObjects.Physical.BodyPart
                        If (Not Me.LinkedPart Is Nothing AndAlso Me.LinkedPart.BodyPart Is Nothing) AndAlso (m_strLinkedBodyPartID.Length > 0) Then
                            bpPart = m_doOrganism.FindBodyPart(m_strLinkedBodyPartID, False)

                            If Not bpPart Is Nothing Then
                                Me.LinkedPart = CreateBodyPartList(m_doOrganism, bpPart, CompatiblePartType())
                            Else
                                Util.Application.DeleteItemAfterLoading(Me)
                                Util.DisplayError(New System.Exception("The body part connector ID: " & Me.ID & " was unable to find its linked item ID: " & m_strLinkedBodyPartID & " in the diagram. This node and all links will be removed."))
                            End If
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
                m_strLinkedBodyPartID = Util.LoadID(oXml, "LinkedBodyPart", True, "") 'Note: The ID of the name is added in the LoadID method.

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

                If Not m_thLinkedPart Is Nothing AndAlso Not m_thLinkedPart.BodyPart Is Nothing Then
                    oXml.AddChildElement("LinkedBodyPartID", m_thLinkedPart.BodyPart.ID)
                End If

                oXml.OutOfElem()

            End Sub

            Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")

                oXml.AddChildElement("RobotPartInterface")
                oXml.IntoElem()

                oXml.AddChildElement("Name", Me.Name)
                oXml.AddChildElement("ID", Me.ID)
                oXml.AddChildElement("Type", Me.PartType)
                oXml.AddChildElement("ModuleName", Me.ModuleName)

                If Not m_thLinkedPart Is Nothing AndAlso Not m_thLinkedPart.BodyPart Is Nothing Then
                    oXml.AddChildElement("LinkedBodyPartID", m_thLinkedPart.BodyPart.ID)
                End If

                oXml.OutOfElem()

            End Sub

#End Region

#End Region

#Region "Events"

            Private Sub OnAfterRemoveLinkedPart(ByRef doObject As Framework.DataObject)
                Try
                    Me.LinkedPart = CreateBodyPartList(m_doOrganism, Nothing, CompatiblePartType())
                Catch ex As Exception
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End Try
            End Sub

#End Region

        End Class


    End Namespace
End Namespace

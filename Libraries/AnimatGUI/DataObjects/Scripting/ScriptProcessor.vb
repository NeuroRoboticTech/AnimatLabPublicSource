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
    Namespace Scripting

        Public MustInherit Class ScriptProcessor
            Inherits DataObjects.DragObject

#Region " Attributes "

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
                    Return False
                End Get
            End Property

#End Region

            Public Overrides ReadOnly Property WorkspaceImageName As String
                Get
                    Return "AnimatGUI.RobotInterface.gif"
                End Get
            End Property

            Public MustOverride ReadOnly Property PartType() As String

#End Region

#Region " Methods "

            Public Sub New(ByVal doParent As Framework.DataObject)
                MyBase.New(doParent)

                m_strName = "ScriptProcessor"
            End Sub

#Region " DataObject Methods "

#Region " Workspace TreeView "


            Public Overrides Function WorkspaceTreeviewPopupMenu(ByRef tnSelectedNode As Crownwood.DotNetMagic.Controls.Node, ByVal ptPoint As System.Drawing.Point) As Boolean

                If tnSelectedNode Is m_tnWorkspaceNode Then
                    ' Create the popup menu object
                    Dim popup As New AnimatContextMenuStrip("AnimatGUI.DataObjects.Robotics.RobotInterface.WorkspaceTreeviewPopupMenu", Util.SecurityMgr)

                    Dim mcDelete As New System.Windows.Forms.ToolStripMenuItem("Delete Script", Util.Application.ToolStripImages.GetImage("AnimatGUI.Delete.gif"), New EventHandler(AddressOf Util.Application.OnDeleteFromWorkspace))

                    popup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {mcDelete})

                    Util.ProjectWorkspace.ctrlTreeView.ContextMenuNode = popup

                    Return True
                End If

                Return False
            End Function

#End Region


            Public Overrides Function FindDragObject(strStructureName As String, strDataItemID As String, Optional bThrowError As Boolean = True) As DragObject
                Throw New System.Exception("FindDragObject not implemented")
            End Function

#Region " Add-Remove to List Methods "

            Public Overrides Sub AddToSim(ByVal bThrowError As Boolean, Optional ByVal bDoNotInit As Boolean = False)
                If Not Me.Parent Is Nothing Then
                    If Util.Application.SimulationInterface.FindItem(Me.Parent.ID, False) Then
                        Util.Application.SimulationInterface.AddItem(Me.Parent.ID, "Script", Me.ID, Me.GetSimulationXml("Script"), bThrowError, bDoNotInit)
                    ElseIf Me.Parent Is Util.Environment Then
                        'If we are adding it to the environment then add it to the sim object.
                        Util.Application.SimulationInterface.AddItem(Util.Simulation.ID, "Script", Me.ID, Me.GetSimulationXml("Script"), bThrowError, bDoNotInit)
                    End If
                    InitializeSimulationReferences()
                End If
            End Sub

            Public Overrides Sub RemoveFromSim(ByVal bThrowError As Boolean)
                If Not Me.Parent Is Nothing AndAlso Not m_doInterface Is Nothing Then
                    If Util.Application.SimulationInterface.FindItem(Me.Parent.ID, False) Then
                        Util.Application.SimulationInterface.RemoveItem(Me.Parent.ID, "Script", Me.ID, bThrowError)
                    ElseIf Me.Parent Is Util.Environment Then
                        Util.Application.SimulationInterface.RemoveItem(Util.Simulation.ID, "Script", Me.ID, bThrowError)
                    End If
                End If
                m_doInterface = Nothing
            End Sub

#End Region

            Public Overrides Function Delete(Optional ByVal bAskToDelete As Boolean = True, Optional ByVal e As Crownwood.DotNetMagic.Controls.TGCloseRequestEventArgs = Nothing) As Boolean
                Try
                    If bAskToDelete AndAlso Util.ShowMessage("Are you sure you want to remove this script?", _
                                                             "Remove Script", MessageBoxButtons.YesNo) <> DialogResult.Yes Then
                        Return False
                    End If

                    Util.Application.AppIsBusy = True
                    Me.RemoveFromSim(True)
                    Me.RemoveWorksapceTreeView()
                    If Not Me.Parent Is Nothing Then Me.Parent.IsDirty = True

                    If Me.Parent Is Util.Environment Then
                        Util.Environment.Script = Nothing
                    ElseIf Util.IsTypeOf(Me.Parent.GetType(), GetType(Physical.PhysicalStructure), False) Then
                        Dim doStruct As Physical.PhysicalStructure = DirectCast(Me.Parent, Physical.PhysicalStructure)
                        doStruct.Script = Nothing
                    End If

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

            Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

                oXml.IntoElem()  'Into Script Element

                m_strName = oXml.GetChildString("Name", Me.Name)
                m_strID = oXml.GetChildString("ID", Me.ID)
                m_bEnabled = oXml.GetChildBool("Enabled", m_bEnabled)

                oXml.OutOfElem()

            End Sub


            Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

                oXml.AddChildElement("Script")
                oXml.IntoElem()

                oXml.AddChildElement("Name", Me.Name)
                oXml.AddChildElement("ID", Me.ID)
                oXml.AddChildElement("AssemblyFile", Me.AssemblyFile)
                oXml.AddChildElement("ClassName", Me.ClassName)

                oXml.OutOfElem()

            End Sub

            Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")

                oXml.AddChildElement("Script")
                oXml.IntoElem()

                oXml.AddChildElement("Name", Me.Name)
                oXml.AddChildElement("ID", Me.ID)
                oXml.AddChildElement("Type", Me.PartType)
                oXml.AddChildElement("ModuleName", Me.ModuleFilename)

                oXml.OutOfElem()

            End Sub

#End Region

#End Region

#Region " Events "

#End Region

        End Class

    End Namespace
End Namespace

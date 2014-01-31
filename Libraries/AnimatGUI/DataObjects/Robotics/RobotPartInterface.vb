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

            Protected m_bpParentPart As Physical.BodyPart

#End Region

#Region " Properties "

            Public MustOverride ReadOnly Property PartType() As String

#End Region

#Region " Methods "

            Public Sub New(ByVal doParent As Framework.DataObject)
                MyBase.New(doParent)

                m_bpParentPart = DirectCast(doParent, Physical.BodyPart)
            End Sub

            Public Overridable Function IsCompatibleWithPartType(ByVal bpPart As Physical.BodyPart) As Boolean
                Return False
            End Function


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

            Public Overrides Function Delete(Optional ByVal bAskToDelete As Boolean = True, Optional ByVal e As Crownwood.DotNetMagic.Controls.TGCloseRequestEventArgs = Nothing) As Boolean
                Try
                    If bAskToDelete AndAlso Util.ShowMessage("Are you sure you want to remove the robot part interface?", _
                                                             "Remove Robot Interface", MessageBoxButtons.YesNo) <> DialogResult.Yes Then
                        Return False
                    End If

                    Util.Application.AppIsBusy = True
                    m_bpParentPart.RobotPartInterface = Nothing
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

            Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

                oXml.IntoElem()  'Into RobotPartInterface Element

                m_strName = oXml.GetChildString("Name", Me.Name)
                m_strID = oXml.GetChildString("ID", Me.ID)
                m_bEnabled = oXml.GetChildBool("Enabled", m_bEnabled)

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

                oXml.OutOfElem()

            End Sub

            Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")

                oXml.AddChildElement("RobotPartInterface")
                oXml.IntoElem()

                oXml.AddChildElement("Name", Me.Name)
                oXml.AddChildElement("ID", Me.ID)
                oXml.AddChildElement("Type", Me.PartType)
                oXml.AddChildElement("ModuleName", Me.ModuleName)

                oXml.OutOfElem()

            End Sub

#End Region

#End Region

        End Class


    End Namespace
End Namespace

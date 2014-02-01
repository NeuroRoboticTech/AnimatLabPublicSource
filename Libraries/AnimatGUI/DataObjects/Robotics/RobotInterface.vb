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
            Inherits Framework.DataObject

#Region " Attributes "

            Protected m_doOrganism As Physical.Organism
            Protected m_snPhysicsTimeStep As AnimatGUI.Framework.ScaledNumber

#End Region

#Region " Properties "

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
                        If Not Value Is Nothing Then m_snPhysicsTimeStep.CopyData(Value)
                    End If
                End Set
            End Property

            Public Overridable ReadOnly Property Organism() As Physical.Organism
                Get
                    Return m_doOrganism
                End Get
            End Property

            Public MustOverride ReadOnly Property Physics As Physical.PhysicsEngine

            Public MustOverride ReadOnly Property PartType() As String

#End Region

#Region " Methods "

            Public Sub New(ByVal doParent As Framework.DataObject)
                MyBase.New(doParent)

                m_doOrganism = DirectCast(doParent, Physical.Organism)
                m_strName = "RobotInterface"
                m_snPhysicsTimeStep = New AnimatGUI.Framework.ScaledNumber(Me, "PhysicsTimeStep", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "", "")
            End Sub

            Public Overrides Sub ClearIsDirty()
                MyBase.ClearIsDirty()
                m_snPhysicsTimeStep.ClearIsDirty()
            End Sub

            Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                                ByVal doRoot As AnimatGUI.Framework.DataObject)
                MyBase.CloneInternal(doOriginal, bCutData, doRoot)

                Dim OrigNode As RobotInterface = DirectCast(doOriginal, RobotInterface)

                m_snPhysicsTimeStep = DirectCast(OrigNode.m_snPhysicsTimeStep.Clone(Me, bCutData, doRoot), ScaledNumber)
            End Sub

            Public Overridable Sub GenerateStandaloneSimFile()
                Dim oXml As ManagedAnimatInterfaces.IStdXml = Util.Application.SaveStandAlone(True, True, True, False, Me)
                oXml.Save(Util.Application.ProjectPath & "\" & Util.Application.ProjectName & "_" & m_doOrganism.Name.Replace(" ", "_") & ".asim")

                Util.ShowMessage("Robot simulation file for " & m_doOrganism.Name & " created successfully.", "Exported simulation file", MessageBoxButtons.OK)
            End Sub

#Region " DataObject Methods "

            Public Overrides Function WorkspaceTreeviewPopupMenu(ByRef tnSelectedNode As Crownwood.DotNetMagic.Controls.Node, ByVal ptPoint As System.Drawing.Point) As Boolean

                If tnSelectedNode Is m_tnWorkspaceNode Then
                    Dim mcDelete As New System.Windows.Forms.ToolStripMenuItem("Delete Robot Interface", Util.Application.ToolStripImages.GetImage("AnimatGUI.Delete.gif"), New EventHandler(AddressOf Util.Application.OnDeleteFromWorkspace))

                    ' Create the popup menu object
                    Dim popup As New AnimatContextMenuStrip("AnimatGUI.DataObjects.Robotics.RobotInterface.WorkspaceTreeviewPopupMenu", Util.SecurityMgr)
                    popup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {mcDelete})

                    Util.ProjectWorkspace.ctrlTreeView.ContextMenuNode = popup

                    Return True
                End If

                Return False
            End Function

            Public Overrides Function Delete(Optional ByVal bAskToDelete As Boolean = True, Optional ByVal e As Crownwood.DotNetMagic.Controls.TGCloseRequestEventArgs = Nothing) As Boolean
                Try
                    If bAskToDelete AndAlso Util.ShowMessage("Are you sure you want to remove the robot interface?", _
                                                             "Remove Robot Interface", MessageBoxButtons.YesNo) <> DialogResult.Yes Then
                        Return False
                    End If

                    Util.Application.AppIsBusy = True
                    m_doOrganism.RobotInterface = Nothing
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

                Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = m_snPhysicsTimeStep.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Physics Time Step", pbNumberBag.GetType(), "PhysicsTimeStep", _
                                            "Properties", "Overrides the physics step set in the simulation for the robot. " & _
                                            "This allows you to set a different physics time step for the robotic simulation.", pbNumberBag, _
                                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))
            End Sub

            Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

                oXml.IntoElem()  'Into RobotInterface Element

                m_strName = oXml.GetChildString("Name", Me.Name)
                m_strID = oXml.GetChildString("ID", Me.ID)
                m_bEnabled = oXml.GetChildBool("Enabled", m_bEnabled)

                m_snPhysicsTimeStep.LoadData(oXml, "PhysicsTimeStep")

                oXml.OutOfElem()

            End Sub


            Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

                oXml.AddChildElement("RobotInterface")
                oXml.IntoElem()

                oXml.AddChildElement("Name", Me.Name)
                oXml.AddChildElement("ID", Me.ID)
                oXml.AddChildElement("AssemblyFile", Me.AssemblyFile)
                oXml.AddChildElement("ClassName", Me.ClassName)

                m_snPhysicsTimeStep.SaveData(oXml, "PhysicsTimeStep")

                oXml.OutOfElem()

            End Sub

            Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")

                oXml.AddChildElement("RobotInterface")
                oXml.IntoElem()

                oXml.AddChildElement("Name", Me.Name)
                oXml.AddChildElement("ID", Me.ID)
                oXml.AddChildElement("Type", Me.PartType)
                oXml.AddChildElement("ModuleName", Me.ModuleName)

                m_snPhysicsTimeStep.SaveSimulationXml(oXml, Me, "PhysicsTimeStep")

                oXml.OutOfElem()

            End Sub

#End Region

#End Region

        End Class

    End Namespace
End Namespace

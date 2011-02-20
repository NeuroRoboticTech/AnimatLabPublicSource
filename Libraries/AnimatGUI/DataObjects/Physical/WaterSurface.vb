Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports Crownwood.Magic.Common
Imports Crownwood.Magic.Controls
Imports Crownwood.Magic.Docking
Imports Crownwood.Magic.Menus
Imports AnimatTools.Framework

Namespace DataObjects.Physical

    Public Class WaterSurface
        Inherits DataObjects.Physical.PhysicalStructure

#Region " Attributes "

#End Region

#Region " Properties "

        Protected Overrides ReadOnly Property Structures(ByVal dsSim As AnimatTools.DataObjects.Simulation) As Collections.SortedStructures
            Get
                Return Nothing
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            m_strID = "WATER"

            'For now we need to default the ground to just use a plane for its rigid body.
            Dim doPlane As AnimatTools.DataObjects.Physical.RigidBody
            ' doPlane = DirectCast(Util.Simulation.CreateObject("VortexAnimatTools.DataObjects.Physical.RigidBodies.Plane", Me), AnimatTools.DataObjects.Physical.RigidBody)
            doPlane.Color = System.Drawing.Color.Blue
            doPlane.Name = "WaterPlane"

            Util.ModificationHistory.AllowAddHistory = False
            Me.AddRootBody(doPlane)
            Util.ModificationHistory.AllowAddHistory = True

        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As Crownwood.Magic.Controls.PropertyTable)

            propTable.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Name", m_strName.GetType(), "Name", _
                                        "Surface Properties", "The name for this organism. ", m_strName, True))

            propTable.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("ID", Me.ID.GetType(), "ID", _
                                        "Surface Properties", "ID", Me.ID, True))

            propTable.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Body Plan", Me.BodyPlanFile.GetType(), "BodyPlanFile", _
                                        "Surface Properties", "Specifies the body plan file.", Me.BodyPlanFile, True))

            Dim pbNumberBag As Crownwood.Magic.Controls.PropertyBag = Me.XLocationScaled.Properties
            propTable.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("X", pbNumberBag.GetType(), "XLocationScaled", _
                                        "Location", "Sets the x location of this body part.", pbNumberBag, _
                                        "", GetType(AnimatTools.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = Me.YLocationScaled.Properties
            propTable.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Y", pbNumberBag.GetType(), "YLocationScaled", _
                                        "Location", "Sets the y location of this body part.", pbNumberBag, _
                                        "", GetType(AnimatTools.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = Me.ZLocationScaled.Properties
            propTable.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Z", pbNumberBag.GetType(), "ZLocationScaled", _
                                        "Location", "Sets the z location of this body part.", pbNumberBag, _
                                        "", GetType(AnimatTools.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

        End Sub

        Public Overrides Sub CreateWorkspaceTreeView(ByVal dsSim As AnimatTools.DataObjects.Simulation, _
                                                     ByVal frmWorkspace As Forms.ProjectWorkspace)

            m_wsStructureNode = Me.ParentTreeNode(dsSim).Nodes.Add(Me.Name)
            m_wsStructureNode.ImageIndex = frmWorkspace.ImageManager.GetImageIndex("AnimatTools.Water.gif")
            m_wsStructureNode.SelectedImageIndex = frmWorkspace.ImageManager.GetImageIndex("AnimatTools.Water.gif")
            m_wsStructureNode.Tag = Me

        End Sub

        Public Overrides Function WorkspaceTreeviewPopupMenu(ByRef tnSelectedNode As TreeNode, ByVal ptPoint As Point) As Boolean

            If tnSelectedNode Is m_wsStructureNode Then
                ' Create the menu items
                Dim mcDelete As New MenuCommand("Delete Water Surface ", "DeleteWaterSurface", Util.Application.SmallImages.ImageList, _
                                                  Util.Application.SmallImages.GetImageIndex("AnimatTools.Delete.gif"), _
                                                  New EventHandler(AddressOf Me.OnDeleteStructure))

                ' Create the popup menu object
                Dim popup As New PopupMenu

                popup.MenuCommands.Add(mcDelete)

                ' Show it!
                Dim selected As MenuCommand = popup.TrackPopup(ptPoint)

                Return True
            End If

            Return False
        End Function

        Public Overridable Overloads Sub LoadData(ByRef oXml As Interfaces.StdXml)

            oXml.IntoElem()

            m_strName = oXml.GetChildString("Name")
            m_strStructureType = oXml.GetChildString("Type", m_strStructureType)
            m_fwPosition = Util.LoadVec3d(oXml, "Position", Me)

            oXml.OutOfElem()

            LoadBodyPlan()

        End Sub

        Public Overloads Overrides Sub SaveData(ByRef oXml As Interfaces.StdXml)

            'For now lets set the height of the plane to be the same as the position of the ground/water.
            'If Not m_dbRoot Is Nothing Then
            '    m_dbRoot.XWorldLocation = 0
            '    m_dbRoot.YWorldLocation = CSng(m_fwPosition.Y)
            '    m_dbRoot.ZWorldLocation = 0
            'End If

            oXml.AddChildElement("WaterSurface")
            oXml.IntoElem()

            oXml.AddChildElement("ID", m_strName)
            oXml.AddChildElement("Name", m_strName)
            oXml.AddChildElement("Type", m_strStructureType)

            SaveBodyPlan(oXml)

            Util.SaveVector(oXml, "Position", m_fwPosition)

            oXml.OutOfElem()

        End Sub

#End Region

#Region " Events "

        Protected Overrides Sub OnDeleteStructure(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                MyBase.OnDeleteStructure(sender, e)

                'If we really removed the ground structure then remove it from the environment as well.
                If m_wsStructureNode Is Nothing Then
                    Me.RemoveFiles()
                    'Util.Environment.WaterSurface = Nothing
                    Util.Environment.IsDirty = True

                    Util.Application.EnableDefaultMenuItem("Edit", "Add Water", True)
                    Util.Application.EnableDefaultToolbarItem("Add Water", True)
                End If

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

    End Class

End Namespace


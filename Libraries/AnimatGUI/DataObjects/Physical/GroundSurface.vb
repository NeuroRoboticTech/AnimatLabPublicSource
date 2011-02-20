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

    Public Class GroundSurface
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

            'For now we need to default the ground to just use a plane for its rigid body.
            Dim doPlane As AnimatTools.DataObjects.Physical.RigidBody
            'doPlane = New AnimatTools.DataObjects.Physical.DirectCast(Util.Simulation.CreateObject("VortexAnimatTools.DataObjects.Physical.RigidBodies.Plane", Me), AnimatTools.DataObjects.Physical.RigidBody)
            'doPlane = DirectCast(Util.Simulation.CreateObject("RigidBody", "Plane", Me), AnimatTools.DataObjects.Physical.RigidBody)
            doPlane.Color = System.Drawing.Color.Brown
            doPlane.Name = "GroundPlane"

            Util.ModificationHistory.AllowAddHistory = False
            Me.AddRootBody(doPlane)
            Util.ModificationHistory.AllowAddHistory = True

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatTools.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatTools.Framework.DataObject) As AnimatTools.Framework.DataObject
            Dim doItem As New GroundSurface(doParent)
            doItem.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then doItem.AfterClone(Me, bCutData, doRoot, doItem)
            Return doItem
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

            oXml.AddChildElement("GroundSurface")
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
                    Util.Environment.IsDirty = True
                    Util.Application.EnableDefaultMenuItem("Edit", "Add Ground", True)
                    Util.Application.EnableDefaultToolbarItem("Add Ground", True)
                End If

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

    End Class

End Namespace


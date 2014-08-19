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

Namespace TypeHelpers

    Public Class ContactSensorPruner
        Inherits TreeListPruner

        Protected m_bShowOnlyContactSensors As Boolean = True

        Public Sub New(ByVal bShowOnlyContactSensors As Boolean)
            m_bShowOnlyContactSensors = bShowOnlyContactSensors
        End Sub

        Protected Sub MakeSelectable(ByVal tnNode As Crownwood.DotNetMagic.Controls.Node)
            tnNode.Selectable = True
            tnNode.ForeColor = Color.Black
        End Sub

        Protected Sub MakeUnSelectable(ByVal tnNode As Crownwood.DotNetMagic.Controls.Node)
            tnNode.Selectable = False
            tnNode.ForeColor = System.Drawing.SystemColors.ControlDark
        End Sub

        Public Overrides Function PruneTree(ByVal tnNodes As Crownwood.DotNetMagic.Controls.NodeCollection) As Integer
            Dim iSelectable As Integer = 0
            Dim aryRemoveNodes As New ArrayList

            For Each tnNode As Crownwood.DotNetMagic.Controls.Node In tnNodes

                Dim bSelected As Boolean = False
                If Not tnNode.Tag Is Nothing Then
                    If Util.IsTypeOf(tnNode.Tag.GetType(), GetType(AnimatGUI.TypeHelpers.LinkedDataObject)) Then
                        Dim liPart As AnimatGUI.TypeHelpers.LinkedDataObject = DirectCast(tnNode.Tag, AnimatGUI.TypeHelpers.LinkedDataObject)

                        If Not liPart.Item Is Nothing AndAlso Util.IsTypeOf(liPart.Item.GetType(), GetType(DataObjects.Physical.RigidBody)) Then
                            Dim doPart As DataObjects.Physical.RigidBody = DirectCast(liPart.Item, DataObjects.Physical.RigidBody)

                            If (doPart.IsContactSensor AndAlso m_bShowOnlyContactSensors) OrElse (Not doPart.IsContactSensor AndAlso Not m_bShowOnlyContactSensors) Then
                                MakeSelectable(tnNode)
                                bSelected = True
                                iSelectable = iSelectable + 1
                            End If

                            liPart.Pruner = Me
                        End If

                    End If
                End If

                If Not bSelected Then
                    MakeUnSelectable(tnNode)
                End If

                Dim iBranchSelectables As Integer = PruneTree(tnNode.Nodes)

                If iBranchSelectables = 0 And Not bSelected Then
                    aryRemoveNodes.Add(tnNode)
                End If
                iSelectable = iSelectable + iBranchSelectables
            Next

            'Prune out any nodes that have nothing selectable in them.
            For Each tnNode As Crownwood.DotNetMagic.Controls.Node In aryRemoveNodes
                tnNodes.Remove(tnNode)
            Next

            Return iSelectable
        End Function

        Public Overrides Sub PruneList(ByVal lbList As ListBox)
        End Sub
    End Class

End Namespace


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

    Public Class RobotTreePruner
        Inherits TreeListPruner

        Protected m_doPartInterface As AnimatGUI.DataObjects.Robotics.RobotPartInterface

        Public Sub New(ByVal doParent As AnimatGUI.DataObjects.Robotics.RobotPartInterface)
            m_doPartInterface = doParent
        End Sub

        Protected Sub MakeSelectable(ByVal tnNode As Crownwood.DotNetMagic.Controls.Node)
            tnNode.Selectable = True
            tnNode.ForeColor = Color.Red
        End Sub

        Protected Sub MakeUnSelectable(ByVal tnNode As Crownwood.DotNetMagic.Controls.Node)
            tnNode.Selectable = False
            tnNode.ForeColor = Color.Black
        End Sub

        Public Overrides Sub PruneTree(ByVal tnNodes As Crownwood.DotNetMagic.Controls.NodeCollection)

            For Each tnNode As Crownwood.DotNetMagic.Controls.Node In tnNodes

                Dim bSelected As Boolean = False
                If Not tnNode.Tag Is Nothing Then
                    If Util.IsTypeOf(tnNode.Tag.GetType(), GetType(AnimatGUI.TypeHelpers.LinkedBodyPart)) Then
                        Dim liPart As AnimatGUI.TypeHelpers.LinkedBodyPart = DirectCast(tnNode.Tag, AnimatGUI.TypeHelpers.LinkedBodyPart)

                        If Not liPart.BodyPart Is Nothing AndAlso Util.IsTypeOf(liPart.BodyPart.GetType(), GetType(AnimatGUI.DataObjects.Physical.BodyPart)) Then
                            Dim bpPart As AnimatGUI.DataObjects.Physical.BodyPart = DirectCast(liPart.BodyPart, AnimatGUI.DataObjects.Physical.BodyPart)

                            If m_doPartInterface.IsCompatibleWithPartType(bpPart) AndAlso (bpPart.RobotPartInterface Is Nothing OrElse bpPart.RobotPartInterface Is m_doPartInterface) Then
                                MakeSelectable(tnNode)
                                bSelected = True
                            End If
                        End If

                    End If
                End If

                If Not bSelected Then
                    MakeUnSelectable(tnNode)
                End If

                PruneTree(tnNode.Nodes)
            Next

        End Sub

        Public Overrides Sub PruneList(ByVal lbList As ListBox)
        End Sub
    End Class

End Namespace


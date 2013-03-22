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

Namespace DataObjects.Behavior.Nodes

    Public Class RigidBody
        Inherits BodyPart

#Region " Attributes "

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Rigid Body"
            End Get
        End Property

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.RigidBodyNode.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property DragImageName As String
            Get
                Return "AnimatGUI.RigidBodyNode.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property BaseErrorType As DiagramError.enumErrorTypes
            Get
                Return DiagramError.enumErrorTypes.RigidBodyNotSet
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            Try
                m_thLinkedPart = New AnimatGUI.TypeHelpers.LinkedBodyPartTree(Me)

                m_tpBodyPartType = GetType(AnimatGUI.DataObjects.Physical.RigidBody)

                Shape = AnimatGUI.DataObjects.Behavior.Node.enumShape.Merge
                Size = New System.Drawing.SizeF(50, 50)
                Me.DrawColor = Color.Black
                Me.FillColor = Color.Chartreuse
                Me.Font = New Font("Arial", 14, FontStyle.Bold)
                Me.Alignment = enumAlignment.CenterMiddle

                Me.Name = "Rigid Body"
                Me.Description = "This node allows the user to collect data directly from a rigid body."

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNewNode As New Behavior.Nodes.RigidBody(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function


#Region " DataObject Methods "


#End Region

#End Region

    End Class

End Namespace


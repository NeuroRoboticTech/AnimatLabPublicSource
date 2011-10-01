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

    Public Class Joint
        Inherits BodyPart

#Region " Attributes "

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Joint"
            End Get
        End Property

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.Joint.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property DragImageName As String
            Get
                Return "AnimatGUI.DragHinge.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property BaseErrorType As DiagramError.enumErrorTypes
            Get
                Return DiagramError.enumErrorTypes.JointNotSet
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            Try
                m_thLinkedPart = New AnimatGUI.TypeHelpers.LinkedBodyPartTree(Me)

                m_tpBodyPartType = GetType(AnimatGUI.DataObjects.Physical.Joint)

                Me.DiagramImageName = "AnimatGUI.HingeNodeImage.gif"
                Me.Name = "Joint"
                Me.Description = "This node allows the user to collect data directly from a joint or to control a motorized joint."

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNewNode As New Behavior.Nodes.Joint(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

#Region " DataObject Methods "

#End Region

#End Region

    End Class

End Namespace


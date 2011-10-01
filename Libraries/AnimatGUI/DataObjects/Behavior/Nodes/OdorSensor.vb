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

    Public Class OdorSensor
        Inherits BodyPart

#Region " Attributes "

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Odor Sensor"
            End Get
        End Property

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.OdorSensor.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property DragImageName As String
            Get
                Return "AnimatGUI.OdorSensor.gif"
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

                m_tpBodyPartType = GetType(AnimatGUI.DataObjects.Physical.Bodies.OdorSensor)

                Me.Size = New System.Drawing.SizeF(55, 55)
                Me.DiagramImageName = "AnimatGUI.OdorSensor.gif"
                Me.Name = "Odor Sensor"
                Me.Description = "This node allows the user to collect data directly from an odor sensor."

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNewNode As New Behavior.Nodes.OdorSensor(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Protected Overloads Overrides Function CreateBodyPartList(ByVal doParent As AnimatGUI.Framework.DataObject) As TypeHelpers.LinkedBodyPart
            Return New AnimatGUI.TypeHelpers.LinkedBodyPartList(doParent)
        End Function

        Protected Overloads Overrides Function CreateBodyPartList(ByVal doStruct As Physical.PhysicalStructure, ByVal doBodyPart As Physical.BodyPart, ByVal tpBodyPartType As System.Type) As TypeHelpers.LinkedBodyPart
            Return New AnimatGUI.TypeHelpers.LinkedBodyPartList(doStruct, doBodyPart, tpBodyPartType)
        End Function

        Protected Overrides Function GetBodyPartListDropDownType() As System.Type
            Return GetType(AnimatGUI.TypeHelpers.DropDownListEditor)
        End Function

#Region " DataObject Methods "


#End Region

#End Region

    End Class

End Namespace


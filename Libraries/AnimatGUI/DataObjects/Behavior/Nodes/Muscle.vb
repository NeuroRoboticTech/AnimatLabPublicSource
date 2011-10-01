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

    Public Class Muscle
        Inherits BodyPart

#Region " Attributes "

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Muscle"
            End Get
        End Property

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.ArmMuscle.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property DragImageName As String
            Get
                Return "AnimatGUI.DragMuscle.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property BaseErrorType As DiagramError.enumErrorTypes
            Get
                Return DiagramError.enumErrorTypes.MuscleNotSet
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            Try
                m_tpBodyPartType = GetType(AnimatGUI.DataObjects.Physical.Bodies.MuscleBase)

                Me.Size = New System.Drawing.SizeF(30, 150)
                Me.DiagramImageName = "AnimatGUI.Muscle.gif"
                Me.Name = "Muscle"
                Me.Description = "This node allows the user to collect data directly from a joint or to control a motorized joint."

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Overloads Overrides Function CreateBodyPartList(ByVal doParent As AnimatGUI.Framework.DataObject) As TypeHelpers.LinkedBodyPart
            Return New AnimatGUI.TypeHelpers.LinkedBodyPartList(doParent)
        End Function

        Protected Overloads Overrides Function CreateBodyPartList(ByVal doStruct As Physical.PhysicalStructure, ByVal doBodyPart As Physical.BodyPart, ByVal tpBodyPartType As System.Type) As TypeHelpers.LinkedBodyPart
            Return New AnimatGUI.TypeHelpers.LinkedBodyPartList(doStruct, doBodyPart, tpBodyPartType)
        End Function

        Protected Overrides Function GetBodyPartListDropDownType() As System.Type
            Return GetType(AnimatGUI.TypeHelpers.DropDownListEditor)
        End Function

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNewNode As New Behavior.Nodes.Muscle(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Public Overrides Sub CheckForErrors()
            MyBase.CheckForErrors()

            If Util.Application.ProjectErrors Is Nothing Then Return

            If Not (m_thLinkedPart Is Nothing OrElse m_thLinkedPart.BodyPart Is Nothing) Then

                'We need to check and see if there are any other muscle types with this same linked muscle body part.
                'There should only be one muscle behavioral node with this muscle ID
                If Not Me.Organism Is Nothing Then
                    Dim aryMuscles As New AnimatGUI.Collections.DataObjects(Nothing)
                    Me.Organism.FindChildrenOfType(Me.GetType, aryMuscles)

                    Dim bMatchFound As Boolean = False
                    For Each doMuscle As Behavior.Nodes.Muscle In aryMuscles
                        If Not doMuscle.LinkedPart Is Nothing AndAlso Not doMuscle.LinkedPart.BodyPart Is Nothing AndAlso doMuscle.LinkedPart.BodyPart.ID = m_thLinkedPart.BodyPart.ID AndAlso Not doMuscle Is Me Then
                            bMatchFound = True
                        End If
                    Next

                    If (bMatchFound) Then
                        If Not Util.Application.ProjectErrors.Errors.Contains(DiagramErrors.DataError.GenerateID(Me, DiagramError.enumErrorTypes.MuscleIDDuplicate)) Then
                            Dim deError As New DiagramErrors.DataError(Me, DiagramError.enumErrorLevel.Error, DiagramError.enumErrorTypes.MuscleIDDuplicate, _
                                                     "The body part id for the " & Me.TypeName & " '" & Me.Text & "' is used by more than one muscle reference. This can cause interference in setting the membrane voltage for the muscle. Instead use one motor neuron and have it do any integration of different inputs.")
                            Util.Application.ProjectErrors.Errors.Add(deError.ID, deError)
                        End If
                    Else
                        If Util.Application.ProjectErrors.Errors.Contains(DiagramErrors.DataError.GenerateID(Me, DiagramError.enumErrorTypes.MuscleIDDuplicate)) Then
                            Util.Application.ProjectErrors.Errors.Remove(DiagramErrors.DataError.GenerateID(Me, DiagramError.enumErrorTypes.MuscleIDDuplicate))
                        End If
                    End If

                End If
            End If

        End Sub

#Region " DataObject Methods "


#End Region

#End Region

    End Class

End Namespace


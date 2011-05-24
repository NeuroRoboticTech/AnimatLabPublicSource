Imports System.Windows.Forms
Imports System.Text.RegularExpressions
Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Windows.Forms.Design
Imports AnimatGUI.Framework

Namespace TypeHelpers

    Public Class AttachmentsTypeEditor
        Inherits UITypeEditor
        Private service As IWindowsFormsEditorService

        Protected Sub ProcessMuscle(ByVal doMuscle As DataObjects.Physical.Bodies.MuscleBase)
            Dim frmAttachments As New Forms.BodyPlan.EditAttachments
            Dim aryAttachments As Collections.Attachments = DirectCast(doMuscle.AttachmentPoints.Copy(), Collections.Attachments)

            frmAttachments.Attachments = aryAttachments
            frmAttachments.ParentStructure = doMuscle.ParentStructure

            If frmAttachments.ShowDialog() = DialogResult.OK Then
                Dim aryOldPoints As Collections.Attachments = DirectCast(doMuscle.AttachmentPoints.Clone(doMuscle.AttachmentPoints.Parent, False, Nothing), Collections.Attachments)
                Dim aryNewPoints As Collections.Attachments = DirectCast(aryAttachments.Clone(aryAttachments.Parent, False, Nothing), Collections.Attachments)

                doMuscle.AttachmentPoints = aryAttachments

                'If this is the first time we are setting the attachment points for the muscles then lets setup the length properties of the muscle.
                If aryOldPoints.Count <= 1 AndAlso aryNewPoints.Count >= 2 Then
                    Dim snOldLength As ScaledNumber = DirectCast(doMuscle.LengthTension.RestingLength.Clone(doMuscle.LengthTension.RestingLength.Parent, False, Nothing), ScaledNumber)
                    Dim snOldLwidth As ScaledNumber = DirectCast(doMuscle.LengthTension.Lwidth.Clone(doMuscle.LengthTension.Lwidth.Parent, False, Nothing), ScaledNumber)
                    Dim snOldLowerLimit As ScaledNumber = DirectCast(doMuscle.LengthTension.LowerLimit.Clone(doMuscle.LengthTension.LowerLimit.Parent, False, Nothing), ScaledNumber)
                    Dim snOldUpperLimit As ScaledNumber = DirectCast(doMuscle.LengthTension.UpperLimit.Clone(doMuscle.LengthTension.UpperLimit.Parent, False, Nothing), ScaledNumber)

                    Dim snNewLength As ScaledNumber = DirectCast(doMuscle.LengthTension.RestingLength.Clone(doMuscle.LengthTension.RestingLength.Parent, False, Nothing), ScaledNumber)
                    Dim snNewLwidth As ScaledNumber = DirectCast(doMuscle.LengthTension.Lwidth.Clone(doMuscle.LengthTension.Lwidth.Parent, False, Nothing), ScaledNumber)
                    Dim snNewLowerLimit As ScaledNumber = DirectCast(doMuscle.LengthTension.LowerLimit.Clone(doMuscle.LengthTension.LowerLimit.Parent, False, Nothing), ScaledNumber)
                    Dim snNewUpperLimit As ScaledNumber = DirectCast(doMuscle.LengthTension.UpperLimit.Clone(doMuscle.LengthTension.UpperLimit.Parent, False, Nothing), ScaledNumber)

                    snNewLength.SetFromValue(doMuscle.Length.ActualValue, CInt(Util.Environment.DisplayDistanceUnits))
                    snNewLwidth.SetFromValue((snNewLength.ActualValue * 0.3F), CInt(Util.Environment.DisplayDistanceUnits))
                    snNewLowerLimit.SetFromValue(snNewLength.ActualValue * 0.9)
                    snNewUpperLimit.SetFromValue(snNewLength.ActualValue * 1.1)

                    doMuscle.LengthTension.RestingLength = snNewLength
                    doMuscle.LengthTension.Lwidth = snNewLwidth
                    doMuscle.LengthTension.UpperLimit = snNewUpperLimit
                    doMuscle.LengthTension.LowerLimit = snNewLowerLimit

                    Util.ModificationHistory.BeginHistoryGroup()
                    doMuscle.ManualAddPropertyHistory("AttachmentPoints", aryOldPoints, aryNewPoints, True)
                    doMuscle.LengthTension.ManualAddPropertyHistory("RestingLength", snOldLength, snNewLength, True)
                    doMuscle.LengthTension.ManualAddPropertyHistory("Lwidth", snOldLwidth, snNewLwidth, True)
                    doMuscle.LengthTension.ManualAddPropertyHistory("LowerLimit", snOldLowerLimit, snNewLowerLimit, True)
                    doMuscle.LengthTension.ManualAddPropertyHistory("UpperLimit", snOldUpperLimit, snNewUpperLimit, True)
                    Util.ModificationHistory.CommitHistoryGroup()

                    If doMuscle.Enabled = False Then
                        doMuscle.Enabled = True
                    End If
                Else
                    doMuscle.ManualAddPropertyHistory("AttachmentPoints", aryOldPoints, aryNewPoints, True)
                End If

            End If
        End Sub

        Protected Sub ProcessSpring(ByVal doSpring As DataObjects.Physical.Bodies.Spring)
            Dim frmAttachments As New Forms.BodyPlan.EditAttachments
            Dim aryAttachments As Collections.Attachments = DirectCast(doSpring.AttachmentPoints.Copy(), Collections.Attachments)

            frmAttachments.Attachments = aryAttachments
            frmAttachments.ParentStructure = doSpring.ParentStructure

            If frmAttachments.ShowDialog() = DialogResult.OK Then
                Dim aryOldPoints As Collections.Attachments = DirectCast(doSpring.AttachmentPoints.Clone(doSpring.AttachmentPoints.Parent, False, Nothing), Collections.Attachments)
                Dim aryNewPoints As Collections.Attachments = DirectCast(aryAttachments.Clone(aryAttachments.Parent, False, Nothing), Collections.Attachments)

                doSpring.AttachmentPoints = aryAttachments

                'If this is the first time we are setting the attachment points for the muscles then lets setup the length properties of the muscle.
                If aryOldPoints.Count <= 1 AndAlso aryNewPoints.Count >= 2 Then
                    Dim snOldLength As ScaledNumber = DirectCast(doSpring.NaturalLength.Clone(doSpring.NaturalLength.Parent, False, Nothing), ScaledNumber)
                    Dim snNewLength As ScaledNumber = DirectCast(doSpring.NaturalLength.Clone(doSpring.NaturalLength.Parent, False, Nothing), ScaledNumber)

                    snNewLength.SetFromValue(doSpring.Length.ActualValue, CInt(Util.Environment.DisplayDistanceUnits))

                    doSpring.NaturalLength = snNewLength

                    Util.ModificationHistory.BeginHistoryGroup()
                    doSpring.ManualAddPropertyHistory("AttachmentPoints", aryOldPoints, aryNewPoints, True)
                    doSpring.NaturalLength.ManualAddPropertyHistory("NaturalLength", snOldLength, snNewLength, True)
                    Util.ModificationHistory.CommitHistoryGroup()

                    If doSpring.Enabled = False Then
                        doSpring.Enabled = True
                    End If
                Else
                    doSpring.ManualAddPropertyHistory("AttachmentPoints", aryOldPoints, aryNewPoints, True)
                End If

            End If
        End Sub

        Public Overloads Overrides Function EditValue(ByVal context As ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object) As Object
            Dim frmPairEditor As New AnimatGUI.Forms.BodyPlan.EditCollisionPairs

            Try

                If (Not context Is Nothing And Not context.Instance Is Nothing And Not provider Is Nothing AndAlso Util.IsTypeOf(context.Instance.GetType, GetType(AnimatGuiCtrls.Controls.PropertyBag), False)) Then
                    Dim pbBag As AnimatGuiCtrls.Controls.PropertyBag = DirectCast(context.Instance, AnimatGuiCtrls.Controls.PropertyBag)

                    If Not pbBag.Tag Is Nothing AndAlso Util.IsTypeOf(pbBag.Tag.GetType, GetType(DataObjects.Physical.Bodies.MuscleBase), False) Then
                        Dim doMuscle As DataObjects.Physical.Bodies.MuscleBase = DirectCast(pbBag.Tag, DataObjects.Physical.Bodies.MuscleBase)
                        ProcessMuscle(doMuscle)
                    ElseIf Not pbBag.Tag Is Nothing AndAlso Util.IsTypeOf(pbBag.Tag.GetType, GetType(DataObjects.Physical.Bodies.Spring), False) Then
                        Dim doSpring As DataObjects.Physical.Bodies.Spring = DirectCast(pbBag.Tag, DataObjects.Physical.Bodies.Spring)
                        ProcessSpring(doSpring)
                    End If
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

            Return Nothing
        End Function

        Public Overloads Overrides Function GetEditStyle(ByVal context As ITypeDescriptorContext) As UITypeEditorEditStyle
            If (Not context Is Nothing And Not context.Instance Is Nothing) Then
                ' we'll show a modal form
                Return UITypeEditorEditStyle.Modal
            End If
            Return MyBase.GetEditStyle(context)
        End Function
    End Class

End Namespace


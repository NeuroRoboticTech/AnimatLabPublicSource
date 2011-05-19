Imports System.Windows.Forms
Imports System.Text.RegularExpressions
Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Windows.Forms.Design
Imports AnimatGUI.Framework

Namespace TypeHelpers

    Public Class CalcMuscleStimEditor
        Inherits UITypeEditor
        Private service As IWindowsFormsEditorService

        Public Overloads Overrides Function EditValue(ByVal context As ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object) As Object
            Dim frmPairEditor As New AnimatGUI.Forms.BodyPlan.EditCollisionPairs

            Try

                If (Not context Is Nothing And Not context.Instance Is Nothing And Not provider Is Nothing AndAlso Util.IsTypeOf(context.Instance.GetType, GetType(AnimatGuiCtrls.Controls.PropertyBag), False)) Then
                    Dim pbBag As AnimatGuiCtrls.Controls.PropertyBag = DirectCast(context.Instance, AnimatGuiCtrls.Controls.PropertyBag)

                    If Not pbBag.Tag Is Nothing AndAlso Util.IsTypeOf(pbBag.Tag.GetType, GetType(DataObjects.Physical.Bodies.LinearHillMuscle), False) Then
                        Dim doMuscle As DataObjects.Physical.Bodies.LinearHillMuscle = DirectCast(pbBag.Tag, DataObjects.Physical.Bodies.LinearHillMuscle)

                        Dim frmCalc As New Forms.BodyPlan.CalculateMuscleStimulus

                        frmCalc.Muscle = doMuscle
                        frmCalc.ShowDialog()
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


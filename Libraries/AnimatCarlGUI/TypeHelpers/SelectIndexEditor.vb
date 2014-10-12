Imports System.Windows.Forms
Imports System.Text.RegularExpressions
Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Windows.Forms.Design
Imports AnimatGUI.Framework

Namespace TypeHelpers

    Public Class SelectedIndexEditor
        Inherits UITypeEditor
        Private service As IWindowsFormsEditorService

        Public Overloads Overrides Function EditValue(ByVal context As ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object) As Object

            Try

                If (Not context Is Nothing AndAlso Not context.Instance Is Nothing AndAlso Util.IsTypeOf(context.Instance.GetType, GetType(AnimatGuiCtrls.Controls.PropertyBag), False)) Then
                    Dim pbBag As AnimatGuiCtrls.Controls.PropertyBag = DirectCast(context.Instance, AnimatGuiCtrls.Controls.PropertyBag)

                    If Not pbBag.Tag Is Nothing AndAlso Util.IsTypeOf(pbBag.Tag.GetType, GetType(DataObjects.ExternalStimuli.FiringRate), False) Then
                        Dim doRate As DataObjects.ExternalStimuli.FiringRate = DirectCast(pbBag.Tag, DataObjects.ExternalStimuli.FiringRate)

                        If Not doRate.Node Is Nothing AndAlso Util.IsTypeOf(doRate.Node.GetType(), GetType(DataObjects.Behavior.NeuronGroup), False) Then
                            Dim doNeuron As DataObjects.Behavior.NeuronGroup = DirectCast(doRate.Node, DataObjects.Behavior.NeuronGroup)

                            If Not doNeuron Is Nothing Then
                                Dim frmEditor As New Forms.SelectIndex()
                                frmEditor.Min = 0
                                frmEditor.Max = doNeuron.NeuronCount - 1
                                frmEditor.Indices = doRate.CellsToStim

                                If frmEditor.ShowDialog() = DialogResult.OK Then
                                    doRate.CellsToStim.Clear()
                                    For Each iIdx As Integer In frmEditor.lbIndices.Items
                                        doRate.CellsToStim.Add(iIdx)
                                    Next
                                End If
                            End If
                        End If
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


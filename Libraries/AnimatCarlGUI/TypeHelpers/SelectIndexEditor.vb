Imports System.Windows.Forms
Imports System.Text.RegularExpressions
Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Windows.Forms.Design
Imports AnimatGUI.Framework
Imports AnimatCarlGUI.DataObjects.Behavior.NodeTypes
Imports AnimatCarlGUI.DataObjects.Behavior.SynapseTypes

Namespace TypeHelpers

    Public Class SelectedIndexEditor
        Inherits UITypeEditor
        Private service As IWindowsFormsEditorService

        Protected Overridable Sub ProcessFiringRate(ByVal context As ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object, _
                                                         ByVal doRate As DataObjects.ExternalStimuli.FiringRate)

            Dim doNeuron As NeuronGroup = DirectCast(doRate.Node, NeuronGroup)

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
        End Sub

        Protected Overridable Sub ProcessAdapter(ByVal context As ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object, _
                                                         ByVal doAdapter As DataObjects.Behavior.NodeTypes.PhysicalToNodeAdapter)

            Dim doNeuron As SpikeGeneratorGroup = DirectCast(doAdapter.Destination, SpikeGeneratorGroup)

            If Not doNeuron Is Nothing Then
                Dim frmEditor As New Forms.SelectIndex()
                frmEditor.Min = 0
                frmEditor.Max = doNeuron.NeuronCount - 1
                frmEditor.Indices = doAdapter.CellsToStim

                If frmEditor.ShowDialog() = DialogResult.OK Then
                    doAdapter.CellsToStim.Clear()
                    For Each iIdx As Integer In frmEditor.lbIndices.Items
                        doAdapter.CellsToStim.Add(iIdx)
                    Next
                End If
            End If
        End Sub

        Protected Overridable Sub ProcessSpikingCurrentSynapse(ByVal context As ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object, _
                                                               ByVal doSynapse As DataObjects.Behavior.SynapseTypes.SpikingCurrentSynapse)

            Dim doNeuron As NeuronGroup = DirectCast(doSynapse.Origin, NeuronGroup)

            If Not doNeuron Is Nothing Then
                Dim frmEditor As New Forms.SelectIndex()
                frmEditor.Min = 0
                frmEditor.Max = doNeuron.NeuronCount - 1
                frmEditor.Indices = doSynapse.CellsToMonitor

                If frmEditor.ShowDialog() = DialogResult.OK Then
                    doSynapse.CellsToMonitor.Clear()
                    For Each iIdx As Integer In frmEditor.lbIndices.Items
                        doSynapse.CellsToMonitor.Add(iIdx)
                    Next
                End If
            End If
        End Sub

        Public Overloads Overrides Function EditValue(ByVal context As ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object) As Object

            Try

                If (Not context Is Nothing AndAlso Not context.Instance Is Nothing AndAlso Util.IsTypeOf(context.Instance.GetType, GetType(AnimatGuiCtrls.Controls.PropertyBag), False)) Then
                    Dim pbBag As AnimatGuiCtrls.Controls.PropertyBag = DirectCast(context.Instance, AnimatGuiCtrls.Controls.PropertyBag)

                    If Not pbBag.Tag Is Nothing AndAlso Util.IsTypeOf(pbBag.Tag.GetType, GetType(DataObjects.ExternalStimuli.FiringRate), False) Then
                        Dim doRate As DataObjects.ExternalStimuli.FiringRate = DirectCast(pbBag.Tag, DataObjects.ExternalStimuli.FiringRate)

                        If Not doRate.Node Is Nothing AndAlso Util.IsTypeOf(doRate.Node.GetType(), GetType(NeuronGroup), False) Then
                            ProcessFiringRate(context, provider, value, doRate)
                        End If
                    ElseIf Not pbBag.Tag Is Nothing AndAlso Util.IsTypeOf(pbBag.Tag.GetType, GetType(DataObjects.Behavior.NodeTypes.PhysicalToNodeAdapter), False) Then
                        Dim doAdapter As DataObjects.Behavior.NodeTypes.PhysicalToNodeAdapter = DirectCast(pbBag.Tag, DataObjects.Behavior.NodeTypes.PhysicalToNodeAdapter)

                        If Not doAdapter.Destination Is Nothing AndAlso Util.IsTypeOf(doAdapter.Destination.GetType(), GetType(SpikeGeneratorGroup), False) Then
                            ProcessAdapter(context, provider, value, doAdapter)
                        End If
                    ElseIf Not pbBag.Tag Is Nothing AndAlso Util.IsTypeOf(pbBag.Tag.GetType, GetType(DataObjects.Behavior.SynapseTypes.SpikingCurrentSynapse), False) Then
                        Dim doSynapse As DataObjects.Behavior.SynapseTypes.SpikingCurrentSynapse = DirectCast(pbBag.Tag, DataObjects.Behavior.SynapseTypes.SpikingCurrentSynapse)

                        ProcessSpikingCurrentSynapse(context, provider, value, doSynapse)
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


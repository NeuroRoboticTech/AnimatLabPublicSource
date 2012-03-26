Imports System.Windows.Forms
Imports System.Text.RegularExpressions
Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Windows.Forms.Design
Imports AnimatGUI.Framework

Namespace TypeHelpers

    Public Class GainTypeEditor
        Inherits UITypeEditor
        Private service As IWindowsFormsEditorService

        Public Overloads Overrides Function EditValue(ByVal context As ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object) As Object
            Dim frmGainEditor As New AnimatGUI.Forms.Gain.EditGain

            Try

                If ((TypeOf (value) Is AnimatGUI.DataObjects.Gain) OrElse value Is Nothing) Then
                    Dim oGain As AnimatGUI.DataObjects.Gain
                    If Not value Is Nothing Then
                        oGain = DirectCast(value, AnimatGUI.DataObjects.Gain)
                    Else
                        oGain = New AnimatGUI.DataObjects.Gains.Polynomial(Nothing, "Gain", "Input Variable", "Output Variable", False, False)
                    End If

                    frmGainEditor.Gain = DirectCast(oGain.Clone(oGain.Parent, False, Nothing), AnimatGUI.DataObjects.Gain)
                    frmGainEditor.BarAssemblyFile = oGain.BarAssemblyFile
                    frmGainEditor.BarClassName = oGain.BarClassName
                    frmGainEditor.MdiParent = Nothing

                    If Not context Is Nothing Then
                        frmGainEditor.PropertyName = context.PropertyDescriptor.Name
                        frmGainEditor.Text = "Edit " & context.PropertyDescriptor.Name
                    Else
                        frmGainEditor.PropertyName = "Gain"
                        frmGainEditor.Text = "Edit Gain"
                    End If

                    Util.ModificationHistory.AllowAddHistory = False
                    If frmGainEditor.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        oGain = frmGainEditor.Gain
                    End If

                    Return oGain
                End If

                Return value

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)

                If Not frmGainEditor Is Nothing Then
                    Try
                        frmGainEditor.Close()
                        frmGainEditor = Nothing
                    Catch ex1 As System.Exception

                    End Try
                End If
            Finally
                Util.ModificationHistory.AllowAddHistory = True
            End Try

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

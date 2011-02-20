Imports System.Windows.Forms
Imports System.Text.RegularExpressions
Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Windows.Forms.Design
Imports AnimatTools.Framework

Namespace TypeHelpers

    Public Class MicrocontrollerTypeEditor
        Inherits UITypeEditor
        Private service As IWindowsFormsEditorService

        Public Overloads Overrides Function EditValue(ByVal context As ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object) As Object
            Dim frmEditor As New AnimatTools.Forms.BodyPlan.Microcontrollers

            Try

                If (Not context Is Nothing And Not context.Instance Is Nothing And Not provider Is Nothing) Then
                    service = CType(provider.GetService(GetType(IWindowsFormsEditorService)), IWindowsFormsEditorService)
                    If (Not service Is Nothing) AndAlso ((TypeOf (value) Is AnimatTools.Collections.SortedMicrocontrollers) OrElse value Is Nothing) Then
                        Dim aryMicrocontrollers As AnimatTools.Collections.SortedMicrocontrollers
                        If Not value Is Nothing Then
                            aryMicrocontrollers = DirectCast(value, AnimatTools.Collections.SortedMicrocontrollers)
                        Else
                            'It should never pass us in a null reference. If it does then something is screwed up.
                            Return Nothing
                        End If

                        frmEditor.Microcontrollers = DirectCast(aryMicrocontrollers.Clone(aryMicrocontrollers.Parent, False, Nothing), AnimatTools.Collections.SortedMicrocontrollers)

                        Util.ModificationHistory.AllowAddHistory = False
                        If frmEditor.ShowDialog() = DialogResult.OK Then
                            Return frmEditor.Microcontrollers
                        End If

                        Return aryMicrocontrollers
                    End If
                End If

                Return value

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)

                If Not frmEditor Is Nothing Then
                    Try
                        frmEditor.Close()
                        frmEditor = Nothing
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


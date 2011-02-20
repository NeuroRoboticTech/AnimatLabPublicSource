Imports System.Windows.Forms
Imports System.Text.RegularExpressions
Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Windows.Forms.Design
Imports AnimatGUI.Framework

Namespace TypeHelpers

    Public Class IonChannelTypeEditor
        Inherits UITypeEditor
        Private service As IWindowsFormsEditorService

        Public Overloads Overrides Function EditValue(ByVal context As ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object) As Object
            Dim frmEditor As New Forms.EditIonChannels

            Try

                If (Not context Is Nothing And Not context.Instance Is Nothing And Not provider Is Nothing) Then
                    service = CType(provider.GetService(GetType(IWindowsFormsEditorService)), IWindowsFormsEditorService)
                    If (Not service Is Nothing) AndAlso ((TypeOf (value) Is Collections.IonChannels) OrElse value Is Nothing) Then
                        Dim aryChannels As Collections.IonChannels
                        If Not value Is Nothing Then
                            aryChannels = DirectCast(value, Collections.IonChannels)
                        Else
                            'It should never pass us in a null reference. If it does then something is screwed up.
                            Return Nothing
                        End If

                        'frmEditor.IonChannels = DirectCast(aryChannels.Clone(aryChannels.Parent, False, Nothing), Collections.IonChannels)
                        frmEditor.IonChannels = aryChannels

                        Util.ModificationHistory.AllowAddHistory = False
                        If frmEditor.ShowDialog() = DialogResult.OK Then
                            Return frmEditor.IonChannels
                        End If

                        Return aryChannels
                    End If
                End If

                Return value

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)

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


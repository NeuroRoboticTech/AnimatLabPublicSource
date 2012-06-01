Imports System.Windows.Forms
Imports System.Text.RegularExpressions
Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Windows.Forms.Design
Imports AnimatGUI.Framework

Namespace TypeHelpers

    Public Class OdorTypeEditor
        Inherits UITypeEditor
        Private service As IWindowsFormsEditorService

        Public Overloads Overrides Function EditValue(ByVal context As ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object) As Object
            Dim frmOdorEditor As New AnimatGUI.Forms.BodyPlan.Odor

            Try
                If ((TypeOf (value) Is AnimatGUI.Collections.SortedOdors) OrElse value Is Nothing) Then
                    Dim aryOdors As AnimatGUI.Collections.SortedOdors
                    If Not value Is Nothing Then
                        aryOdors = DirectCast(value, AnimatGUI.Collections.SortedOdors)
                    Else
                        'It should never pass us in a null reference. If it does then something is screwed up.
                        Return Nothing
                    End If

                    frmOdorEditor.OdorSources = aryOdors

                    Util.ModificationHistory.AllowAddHistory = False
                    frmOdorEditor.ShowDialog()

                    Return aryOdors
                End If

                Return value

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)

                If Not frmOdorEditor Is Nothing Then
                    Try
                        frmOdorEditor.Close()
                        frmOdorEditor = Nothing
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


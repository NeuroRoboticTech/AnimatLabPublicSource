Imports System.Windows.Forms
Imports System.Text.RegularExpressions
Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Windows.Forms.Design
Imports AnimatGUI.Framework

Namespace TypeHelpers

    Public Class CollisionPairsTypeEditor
        Inherits UITypeEditor
        Private service As IWindowsFormsEditorService

        Public Overloads Overrides Function EditValue(ByVal context As ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object) As Object
            Dim frmPairEditor As New AnimatGUI.Forms.BodyPlan.EditCollisionPairs

            Try

                If (Not context Is Nothing And Not context.Instance Is Nothing And Not provider Is Nothing) Then
                    service = CType(provider.GetService(GetType(IWindowsFormsEditorService)), IWindowsFormsEditorService)
                    If (Not service Is Nothing) AndAlso ((TypeOf (value) Is AnimatGUI.Collections.CollisionPairs) OrElse value Is Nothing) Then
                        Dim aryPairs As AnimatGUI.Collections.CollisionPairs
                        If Not value Is Nothing Then
                            aryPairs = DirectCast(value, AnimatGUI.Collections.CollisionPairs)
                        Else
                            'It should never pass us in a null reference. If it does then something is screwed up.
                            Return Nothing
                        End If

                        frmPairEditor.CollisionPairs = DirectCast(aryPairs.Clone(aryPairs.Parent, False, Nothing), AnimatGUI.Collections.CollisionPairs)

                        Util.ModificationHistory.AllowAddHistory = False
                        If frmPairEditor.ShowDialog() = Windows.Forms.DialogResult.OK Then
                            aryPairs = frmPairEditor.CollisionPairs
                        End If

                        Return aryPairs
                    End If
                End If

                Return value

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)

                If Not frmPairEditor Is Nothing Then
                    Try
                        frmPairEditor.Close()
                        frmPairEditor = Nothing
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


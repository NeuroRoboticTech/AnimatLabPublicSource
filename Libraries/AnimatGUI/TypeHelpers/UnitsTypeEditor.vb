Imports System.Windows.Forms
Imports System.Text.RegularExpressions
Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Windows.Forms.Design
Imports AnimatGUI.Framework

Namespace TypeHelpers

    Public Class UnitsTypeEditor
        Inherits UITypeEditor
        Private service As IWindowsFormsEditorService

        Public Overloads Overrides Function EditValue(ByVal context As ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object) As Object
            Dim frmEditor As New Forms.UnitScaleSettings

            Try

                If (Not context Is Nothing And Not context.Instance Is Nothing And Not provider Is Nothing) Then
                    service = CType(provider.GetService(GetType(IWindowsFormsEditorService)), IWindowsFormsEditorService)
                    If (Not service Is Nothing) AndAlso ((TypeOf (value) Is AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits OrElse _
                        TypeOf (value) Is AnimatGUI.DataObjects.Physical.Environment.enumMassUnits) OrElse value Is Nothing) Then

                        frmEditor.MassUnits = Util.Environment.MassUnits
                        frmEditor.DistanceUnits = Util.Environment.DistanceUnits

                        If frmEditor.ShowDialog() = Windows.Forms.DialogResult.OK Then
                            Util.Application.ChangeUnits(frmEditor.MassUnits, frmEditor.DistanceUnits)
                        End If

                        Return value
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


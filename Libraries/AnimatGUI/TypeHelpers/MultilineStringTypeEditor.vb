Imports System.Windows.Forms
Imports System.Text.RegularExpressions
Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Windows.Forms.Design

Namespace TypeHelpers

    Public Class MultiLineStringTypeEditor
        Inherits UITypeEditor
        Private service As IWindowsFormsEditorService

        Public Overloads Overrides Function EditValue(ByVal context As ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object) As Object
            If (Not context Is Nothing And Not context.Instance Is Nothing And Not provider Is Nothing) Then
                service = CType(provider.GetService(GetType(IWindowsFormsEditorService)), IWindowsFormsEditorService)
                If (Not service Is Nothing) AndAlso ((TypeOf (value) Is String) OrElse value Is Nothing) Then
                    Dim strValue As String = ""
                    If Not value Is Nothing Then strValue = DirectCast(value, String)
                    AnimatGUI.Forms.MultiLineStringEditor.Edit(strValue, "Edit " & context.PropertyDescriptor.Name)
                    Return strValue
                End If
            End If
            Return value
        End Function

        Public Overloads Overrides Function GetEditStyle(ByVal context As ITypeDescriptorContext) As UITypeEditorEditStyle
            If (Not context Is Nothing And Not context.Instance Is Nothing) Then
                ' we'll show a modal form
                Return UITypeEditorEditStyle.Modal
                'Return UITypeEditorEditStyle.None
            End If
            Return MyBase.GetEditStyle(context)
        End Function
    End Class

End Namespace

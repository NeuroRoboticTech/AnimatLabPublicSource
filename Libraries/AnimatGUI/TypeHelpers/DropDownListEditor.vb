Imports System.Windows.Forms
Imports System.Text.RegularExpressions
Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Windows.Forms.Design

Namespace TypeHelpers

    Public Class DropDownListEditor
        Inherits UITypeEditor
        Private edSvc As IWindowsFormsEditorService

        Public Overloads Overrides Function GetEditStyle(ByVal context As _
                        ITypeDescriptorContext) As UITypeEditorEditStyle
            If Not context Is Nothing AndAlso Not context.Instance Is Nothing Then
                Return UITypeEditorEditStyle.DropDown
            End If
            Return UITypeEditorEditStyle.None
        End Function

        <RefreshProperties(RefreshProperties.All)> _
        Public Overloads Overrides Function EditValue(ByVal context As ITypeDescriptorContext, _
                                                      ByVal provider As System.IServiceProvider, _
                                                      ByVal value As [Object]) As [Object]

            If context Is Nothing OrElse provider Is Nothing OrElse context.Instance Is Nothing Then
                Return Nothing
            End If

            If value Is Nothing OrElse Not TypeOf (value) Is AnimatGUI.Framework.DataObject Then
                Return MyBase.EditValue(provider, value)
            End If
            Dim doValue As AnimatGUI.Framework.DataObject = DirectCast(value, AnimatGUI.Framework.DataObject)

            Dim oEdSvc As Object = provider.GetService(GetType(IWindowsFormsEditorService))
            If oEdSvc Is Nothing Then
                ' nothing we can do here either
                Return MyBase.EditValue(provider, value)
            End If
            Me.edSvc = DirectCast(oEdSvc, IWindowsFormsEditorService)

            ' prepare the listbox
            Dim lst As New ListBox
            Dim ctrlBox As System.Windows.Forms.Control = DirectCast(lst, System.Windows.Forms.Control)
            doValue.BuildPropertyDropDown(ctrlBox)
            AddHandler lst.SelectedIndexChanged, AddressOf Me.handleSelection
            Me.edSvc.DropDownControl(lst)

            ' we're back
            If Not lst.SelectedItem Is Nothing Then
                Dim leEntry As AnimatGUI.TypeHelpers.DropDownEntry = DirectCast(lst.SelectedItem, AnimatGUI.TypeHelpers.DropDownEntry)
                value = leEntry.Value
            End If

            Return value
        End Function

        Private Sub handleSelection(ByVal sender As Object, ByVal e As EventArgs)
            If Me.edSvc Is Nothing Then Return
            Me.edSvc.CloseDropDown()
        End Sub

    End Class

End Namespace


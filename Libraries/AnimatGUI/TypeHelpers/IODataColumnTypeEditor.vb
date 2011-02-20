Imports System.Windows.Forms
Imports System.Text.RegularExpressions
Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Windows.Forms.Design
Imports AnimatTools.Framework

Namespace TypeHelpers

    Public Class IODataColumnTypeEditor
        Inherits UITypeEditor
        Private service As IWindowsFormsEditorService

        Public Overloads Overrides Function EditValue(ByVal context As ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object) As Object
            Dim frmSelectItem As AnimatTools.Forms.Tools.SelectDataItem

            Try

                If (Not context Is Nothing And Not context.Instance Is Nothing And Not provider Is Nothing) Then
                    service = CType(provider.GetService(GetType(IWindowsFormsEditorService)), IWindowsFormsEditorService)
                    If (Not service Is Nothing) AndAlso ((TypeOf (value) Is Crownwood.Magic.Controls.PropertyBag) OrElse value Is Nothing) Then

                        Dim pbBag As Crownwood.Magic.Controls.PropertyBag = DirectCast(value, Crownwood.Magic.Controls.PropertyBag)

                        If pbBag.Tag Is Nothing OrElse Not TypeOf pbBag.Tag Is DataObjects.Charting.DataColumn Then
                            Throw New System.Exception("Unable to find the data column!")
                        End If

                        Dim doData As AnimatTools.DataObjects.Charting.DataColumn = DirectCast(pbBag.Tag, AnimatTools.DataObjects.Charting.DataColumn)

                        Dim doStruct As DataObjects.Physical.PhysicalStructure
                        Dim doIOData As DataObjects.Physical.IODataEntry
                        Dim doController As DataObjects.Physical.Microcontroller
                        If Not doData.Parent Is Nothing AndAlso TypeOf doData.Parent Is DataObjects.Physical.IODataEntry Then
                            doIOData = DirectCast(doData.Parent, DataObjects.Physical.IODataEntry)

                            If Not doData.Parent.Parent Is Nothing AndAlso TypeOf doData.Parent.Parent Is DataObjects.Physical.Microcontroller Then
                                doController = DirectCast(doData.Parent.Parent, DataObjects.Physical.Microcontroller)

                                If Not doData.Parent.Parent.Parent Is Nothing AndAlso TypeOf doData.Parent.Parent.Parent Is DataObjects.Physical.PhysicalStructure Then
                                    doStruct = DirectCast(doData.Parent.Parent.Parent, DataObjects.Physical.PhysicalStructure)
                                Else
                                    Throw New System.Exception("The parent of the parent of the parent of the selected data column is not a structure!")
                                End If
                            Else
                                Throw New System.Exception("The parent of the parent of the selected data column is not a microcontroller!")
                            End If
                        Else
                            Throw New System.Exception("The parent of the selected data column is not an IODataEntry!")
                        End If

                        frmSelectItem = New AnimatTools.Forms.Tools.SelectDataItem

                        Util.ModificationHistory.AllowAddHistory = False
                        frmSelectItem.ColumnType = New DataObjects.Charting.DataColumn(doData.Parent)
                        frmSelectItem.SelectedStructure = DirectCast(doStruct, DataObjects.Physical.PhysicalStructure)
                        frmSelectItem.TemplatePartType = GetType(DataObjects.Physical.BodyPart)
                        frmSelectItem.BuildTreeView()
                        If frmSelectItem.ShowDialog(Nothing) = DialogResult.OK Then
                            Return frmSelectItem.DataColumn
                        End If

                        Return doData
                    End If
                End If

                Return value

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)

                If Not frmSelectItem Is Nothing Then
                    Try
                        frmSelectItem.Close()
                        frmSelectItem = Nothing
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


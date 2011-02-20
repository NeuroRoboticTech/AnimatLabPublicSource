Imports System.Windows.Forms
Imports System.Text.RegularExpressions
Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Windows.Forms.Design
Imports AnimatTools.Framework

Namespace TypeHelpers

    Public Class IODataColumnTypeConverter
        Inherits ExpandableObjectConverter

        Public Overloads Overrides Function CanConvertTo(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal destinationType As System.Type) As Boolean

            If Util.IsTypeOf(destinationType, GetType(AnimatTools.DataObjects.Charting.DataColumn)) Then
                Return True
            End If
            Return MyBase.CanConvertTo(context, destinationType)

        End Function

        Public Overloads Overrides Function ConvertTo(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal culture As System.Globalization.CultureInfo, ByVal value As Object, ByVal destinationType As System.Type) As Object

            If Not value Is Nothing AndAlso TypeOf (value) Is Crownwood.Magic.Controls.PropertyBag Then
                Dim pbBag As Crownwood.Magic.Controls.PropertyBag = DirectCast(value, Crownwood.Magic.Controls.PropertyBag)

                If pbBag.Tag Is Nothing OrElse Not TypeOf pbBag.Tag Is DataObjects.Charting.DataColumn Then
                    Return ""
                End If

                Dim doData As AnimatTools.DataObjects.Charting.DataColumn = DirectCast(pbBag.Tag, AnimatTools.DataObjects.Charting.DataColumn)

                If Not doData.DataItem Is Nothing AndAlso Not doData.DataType Is Nothing Then
                    Dim doType As DataObjects.DataType = doData.DataType.DataTypes(doData.DataType.ID)
                    Return doData.DataItem.Name & " -> " & doType.Name
                Else
                    Return ""
                End If
            Else
                Return ""
            End If

        End Function

    End Class

End Namespace

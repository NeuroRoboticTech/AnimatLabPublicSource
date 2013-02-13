Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports AnimatGuiCtrls.Controls
Imports System.Text.RegularExpressions
Imports System.Drawing.Design
Imports System.Windows.Forms.Design
Imports AnimatGUI.Framework

Namespace TypeHelpers

    Public Class LinkedDataObjectPropertiesTypeConverter
        Inherits ExpandableObjectConverter

        Public Overloads Overrides Function CanConvertTo(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal destinationType As System.Type) As Boolean

            If Util.IsTypeOf(destinationType, GetType(AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList)) Then
                Return True
            End If
            Return MyBase.CanConvertTo(context, destinationType)

        End Function

        Public Overloads Overrides Function ConvertTo(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal culture As System.Globalization.CultureInfo, ByVal value As Object, ByVal destinationType As System.Type) As Object

            If Not value Is Nothing AndAlso Util.IsTypeOf(value.GetType, GetType(AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList), False) Then
                Dim thItem As AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList = DirectCast(value, AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList)

                If Not thItem.Item Is Nothing Then
                    Return thItem.PropertyName
                Else
                    Return ""
                End If
            ElseIf Not value Is Nothing AndAlso TypeOf (value) Is AnimatGuiCtrls.Controls.PropertyBag Then
                'Sometimes it passes this to us in a property bag for some reason. If so then extract it.
                Dim propBag As AnimatGuiCtrls.Controls.PropertyBag = DirectCast(value, AnimatGuiCtrls.Controls.PropertyBag)
                If Not propBag.Tag Is Nothing AndAlso TypeOf (propBag.Tag) Is AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList Then
                    Dim doObj As AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList = DirectCast(propBag.Tag, AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList)
                    If Not doObj Is Nothing AndAlso Not doObj.Item Is Nothing Then
                        Return doObj.PropertyName
                    End If
                End If
            End If

        End Function

    End Class

End Namespace

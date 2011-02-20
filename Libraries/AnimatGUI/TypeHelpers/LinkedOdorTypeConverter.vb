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

    Public Class LinkedOdorTypeConverter
        Inherits ExpandableObjectConverter

        Public Overloads Overrides Function CanConvertTo(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal destinationType As System.Type) As Boolean

            If Util.IsTypeOf(destinationType, GetType(AnimatGUI.TypeHelpers.LinkedOdorType)) Then
                Return True
            End If
            Return MyBase.CanConvertTo(context, destinationType)

        End Function

        Public Overloads Overrides Function ConvertTo(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal culture As System.Globalization.CultureInfo, ByVal value As Object, ByVal destinationType As System.Type) As Object

            If Not value Is Nothing AndAlso Util.IsTypeOf(value.GetType, GetType(AnimatGUI.TypeHelpers.LinkedOdorType), False) Then
                Dim thOdor As AnimatGUI.TypeHelpers.LinkedOdorType = DirectCast(value, AnimatGUI.TypeHelpers.LinkedOdorType)

                If Not thOdor.OdorType Is Nothing Then
                    Return thOdor.OdorType.Name
                Else
                    Return ""
                End If
            End If

        End Function

    End Class

End Namespace

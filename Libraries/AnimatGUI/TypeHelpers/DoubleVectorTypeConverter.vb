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

    Public Class Vec3dTypeConverter
        Inherits ExpandableObjectConverter

        Public Overloads Overrides Function CanConvertFrom(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal sourceType As System.Type) As Boolean

            If sourceType Is GetType(String) Then
                Return True
            End If

            Return MyBase.CanConvertFrom(context, sourceType)
        End Function

        Public Overloads Overrides Function ConvertFrom(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal culture As System.Globalization.CultureInfo, ByVal value As Object) As Object

            If TypeOf (value) Is String Then
                Dim strValue As String = DirectCast(value, String)
                Dim vValue As New Vec3d(Nothing, 0, 0, 0)

                If strValue.Trim.Length > 0 Then
                    'Get rid of all spaces, (, and )
                    strValue = strValue.Replace(" ", "").Replace("(", "").Replace(")", "")

                    Dim aryParts() As String = Split(strValue, ",")
                    Dim iParts As Integer = UBound(aryParts)

                    If iParts >= 0 AndAlso IsNumeric(aryParts(0)) Then
                        'Get the x value
                        vValue.X = CDbl(aryParts(0))
                    End If

                    If iParts >= 1 AndAlso IsNumeric(aryParts(1)) Then
                        'Get the x value
                        vValue.Y = CDbl(aryParts(1))
                    End If

                    If iParts >= 2 AndAlso IsNumeric(aryParts(2)) Then
                        'Get the x value
                        vValue.Z = CDbl(aryParts(2))
                    End If
                End If

                Return vValue
            End If

            Return MyBase.ConvertFrom(context, culture, value)
        End Function

        Public Overloads Overrides Function CanConvertTo(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal destinationType As System.Type) As Boolean

            If Util.IsTypeOf(destinationType, GetType(AnimatGUI.Framework.Vec3d)) Then
                Return True
            End If
            Return MyBase.CanConvertTo(context, destinationType)

        End Function

        Public Overloads Overrides Function ConvertTo(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal culture As System.Globalization.CultureInfo, ByVal value As Object, ByVal destinationType As System.Type) As Object

            If Not value Is Nothing AndAlso Util.IsTypeOf(value.GetType, GetType(AnimatGUI.Framework.Vec3d), False) Then
                Dim fwVector As AnimatGUI.Framework.Vec3d = DirectCast(value, AnimatGUI.Framework.Vec3d)
                Return fwVector.ToString()
            End If

        End Function

    End Class

End Namespace

Imports System.Windows.Forms
Imports System.Text.RegularExpressions
Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Windows.Forms.Design
Imports AnimatGUI.Framework

Namespace TypeHelpers

    Public Class ConstrainLimitTypeConverter
        Inherits ExpandableObjectConverter

        Public Overloads Overrides Function CanConvertFrom(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal sourceType As System.Type) As Boolean

            Return MyBase.CanConvertFrom(context, sourceType)
        End Function

        Public Overloads Overrides Function CanConvertTo(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal destinationType As System.Type) As Boolean

            If destinationType Is GetType(AnimatGuiCtrls.Controls.PropertyBag) Then
                Return True
            End If
            Return MyBase.CanConvertTo(context, destinationType)

        End Function

        Public Overloads Overrides Function ConvertTo(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal culture As System.Globalization.CultureInfo, ByVal value As Object, ByVal destinationType As System.Type) As Object
            If destinationType Is GetType(String) AndAlso TypeOf (value) Is AnimatGuiCtrls.Controls.PropertyTable Then
                Dim pbValue As AnimatGuiCtrls.Controls.PropertyTable = DirectCast(value, AnimatGuiCtrls.Controls.PropertyTable)

                Dim psAngle As AnimatGuiCtrls.Controls.PropertySpec = DirectCast(pbValue.Properties(0), AnimatGuiCtrls.Controls.PropertySpec)

                If Not psAngle.DefaultValue Is Nothing AndAlso TypeOf (psAngle.DefaultValue) Is AnimatGuiCtrls.Controls.PropertyTable Then
                    Dim pbAngle As AnimatGuiCtrls.Controls.PropertyTable = DirectCast(psAngle.DefaultValue, AnimatGuiCtrls.Controls.PropertyTable)

                    If Not pbAngle.Tag Is Nothing AndAlso TypeOf (pbAngle.Tag) Is ScaledNumber Then
                        Dim snAngle As ScaledNumber = DirectCast(pbAngle.Tag, ScaledNumber)
                        Return snAngle.Text()
                    End If
                End If

                Return ""
            ElseIf destinationType Is GetType(String) AndAlso TypeOf (value) Is AnimatGUI.DataObjects.Physical.ConstraintLimit Then
                Dim svValue As AnimatGUI.DataObjects.Physical.ConstraintLimit = DirectCast(value, AnimatGUI.DataObjects.Physical.ConstraintLimit)

                Dim strValue As String = svValue.ToString
                Return strValue
            End If

            Return MyBase.ConvertTo(context, culture, value, destinationType)
        End Function

    End Class

End Namespace

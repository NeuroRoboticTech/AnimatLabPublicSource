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

    Public Class DataTypeIDTypeConverter
        Inherits ExpandableObjectConverter

        Public Overrides Function CanConvertFrom(context As System.ComponentModel.ITypeDescriptorContext, sourceType As System.Type) As Boolean

            If sourceType Is GetType(String) Then
                Return True
            End If
            Return MyBase.CanConvertFrom(context, sourceType)
        End Function

        Public Overloads Overrides Function CanConvertTo(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal destinationType As System.Type) As Boolean

            If Util.IsTypeOf(destinationType, GetType(AnimatGUI.TypeHelpers.DataTypeID)) Then
                Return True
            End If
            Return MyBase.CanConvertTo(context, destinationType)

        End Function

        Public Overloads Overrides Function ConvertTo(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal culture As System.Globalization.CultureInfo, ByVal value As Object, ByVal destinationType As System.Type) As Object

            If Not value Is Nothing AndAlso Util.IsTypeOf(value.GetType, GetType(AnimatGUI.TypeHelpers.DataTypeID), False) Then
                Dim thDataType As AnimatGUI.TypeHelpers.DataTypeID = DirectCast(value, AnimatGUI.TypeHelpers.DataTypeID)
                If Not thDataType.Value Is Nothing Then
                    Return thDataType.Value.Name
                Else
                    Return ""
                End If
            ElseIf Not value Is Nothing AndAlso Util.IsTypeOf(value.GetType, GetType(System.String), False) Then
                Return ""
            End If

        End Function

        Public Overloads Overrides Function ConvertFrom(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal culture As System.Globalization.CultureInfo, ByVal value As Object) As Object
            If TypeOf (value) Is String Then
                Dim strValue As String = DirectCast(value, String)

                Dim aryName() As String = Split(strValue, ".")

                If aryName.Length < 3 Then
                    Throw New System.Exception("You must specify the class name, data type name, and datatype on the conversion string")
                End If

                Dim strDataTypeName As String = aryName(aryName.Length - 2)
                Dim strDataType As String = aryName(aryName.Length - 1)
                ReDim Preserve aryName(aryName.Length - 3)
                Dim strClass As String = Join(aryName, ".")

                Dim doObj As AnimatGUI.Framework.DataObject = Util.Simulation.CreateObject(strClass, Nothing)

                Dim dtType As AnimatGUI.TypeHelpers.DataTypeID = DirectCast(Util.GetObjectProperty(doObj, strDataTypeName), AnimatGUI.TypeHelpers.DataTypeID)

                dtType.ID = strDataType

                Return dtType
            End If
        End Function


    End Class

End Namespace

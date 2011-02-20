Imports System.Windows.Forms
Imports System.Text.RegularExpressions
Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Windows.Forms.Design
Imports AnimatGUI.Framework
Imports System.IO
Imports System.Drawing

Namespace TypeHelpers

    Public Class ImageTypeConverter
        Inherits ExpandableObjectConverter

        Public Overloads Overrides Function CanConvertTo(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal destinationType As System.Type) As Boolean

            If Util.IsTypeOf(destinationType, GetType(AnimatGUI.DataObjects.Behavior.DiagramImage), False) Then
                Return True
            End If
            Return MyBase.CanConvertTo(context, destinationType)

        End Function

        Public Overloads Overrides Function CanConvertFrom(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal sourceType As System.Type) As Boolean

            If sourceType Is GetType(String) Then
                Return True
            End If

            Return MyBase.CanConvertFrom(context, sourceType)
        End Function

        Public Overloads Overrides Function ConvertTo(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal culture As System.Globalization.CultureInfo, ByVal value As Object, ByVal destinationType As System.Type) As Object

            If Not value Is Nothing AndAlso Util.IsTypeOf(value.GetType, GetType(AnimatGUI.DataObjects.Behavior.DiagramImage), False) Then
                Dim diImage As AnimatGUI.DataObjects.Behavior.DiagramImage = DirectCast(value, AnimatGUI.DataObjects.Behavior.DiagramImage)
                Return diImage.FilePath
            End If

        End Function

        Public Overloads Overrides Function ConvertFrom(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal culture As System.Globalization.CultureInfo, ByVal value As Object) As Object

            Try

                If Not value Is Nothing AndAlso TypeOf (value) Is String Then

                    Dim strValue As String = DirectCast(value, String)

                    If strValue.Trim.Length = 0 Then
                        Return Nothing
                    Else
                        Dim imgNew As New Bitmap(strValue)

                        Dim diImage As New DataObjects.Behavior.DiagramImage(Nothing)
                        diImage.WorkspaceImage = imgNew
                        diImage.FilePath = strValue
                        diImage.ID = strValue
                        diImage.UserImage = True

                        Return diImage
                    End If

                End If

                Return MyBase.ConvertFrom(context, culture, value)

            Catch ex As System.Exception
                Return Nothing
            End Try

        End Function

    End Class

End Namespace


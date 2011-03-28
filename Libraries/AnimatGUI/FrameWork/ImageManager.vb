Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml

Namespace Framework

    Public Class ImageManager

#Region " Attributes "

        Protected m_imgList As New System.Windows.Forms.ImageList
        Protected m_aryImages As New System.Collections.ArrayList

#End Region

#Region " Properties "

        Public ReadOnly Property ImageList() As System.Windows.Forms.ImageList
            Get
                Return m_imgList
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub Clear()
            m_imgList.Images.Clear()
            m_aryImages.Clear()
        End Sub

        Public Function FindImageName(ByVal strImageName As String) As Integer

            Dim strImgName As String = strImageName.ToUpper.Trim
            Dim iIndex As Integer = 0
            For Each strName As String In m_aryImages
                If strName.ToUpper().Trim = strImgName Then
                    Return iIndex
                End If
                iIndex = iIndex + 1
            Next

            Return -1
        End Function

        Public Function AddImage(ByVal strImageName As String, _
                                 Optional ByVal bThrowError As Boolean = True) As Boolean

            Try
                If strImageName.Length > 0 Then
                    Dim myAssembly As System.Reflection.Assembly
                    myAssembly = Util.GetAssembly(strImageName)

                    Return AddImage(myAssembly, strImageName, bThrowError)
                End If
            Catch ex As System.Exception
                If bThrowError Then Throw ex
                Return False
            End Try

        End Function

        Public Function AddImage(ByVal strAssembly As String, _
                                 ByVal strImageName As String, _
                                 Optional ByVal bThrowError As Boolean = True) As Boolean

            Try
                If strAssembly.Length > 0 AndAlso strImageName.Length > 0 Then
                    Dim myAssembly As System.Reflection.Assembly
                    myAssembly = System.Reflection.Assembly.Load(strAssembly)

                    Return AddImage(myAssembly, strImageName, bThrowError)
                End If
            Catch ex As System.Exception
                If bThrowError Then Throw ex
                Return False
            End Try

        End Function

        Public Function AddImage(ByRef myAssembly As System.Reflection.Assembly, _
                                 ByVal strImageName As String, _
                                 Optional ByVal bThrowError As Boolean = True) As Boolean

            Try
                If strImageName.Length = 0 Then Return False

                'If we can not find this image name then add it. Otherwise just skip over it.
                If FindImageName(strImageName) = -1 Then
                    Dim stream As System.IO.Stream = myAssembly.GetManifestResourceStream(strImageName)

                    If stream Is Nothing Then
                        Throw New System.Exception("Unable to load the resource image file '" & _
                                                   strImageName & "' from the assembly '" & myAssembly.Location & "'.")
                    End If

                    Dim imgLoaded As New Bitmap(stream)

                    m_imgList.Images.Add(imgLoaded)
                    m_aryImages.Add(strImageName.ToUpper.Trim)
                End If

                Return True
            Catch ex As System.Exception
                If bThrowError Then Throw ex
                Return False
            End Try

        End Function

        Public Function AddImage(ByVal strImageName As String, _
                                 ByVal imgBitmap As Image, _
                                 Optional ByVal bThrowError As Boolean = True) As Boolean

            Try
                If strImageName.Length = 0 Then Return False

                If imgBitmap Is Nothing Then
                    Throw New System.Exception("The image bitmap named '" & strImageName & "' is not defined.")
                End If

                'If we can not find this image name then add it. Otherwise just skip over it.
                If FindImageName(strImageName) = -1 Then
                    m_imgList.Images.Add(imgBitmap)
                    m_aryImages.Add(strImageName.ToUpper.Trim)
                End If

                Return True
            Catch ex As System.Exception
                If bThrowError Then Throw ex
                Return False
            End Try

        End Function

        Public Function AddImage(ByVal strImageName As String, _
                                 ByVal imgBitmap As Image, _
                                 ByVal imgColor As System.Drawing.Color, _
                                 Optional ByVal bThrowError As Boolean = True) As Boolean

            Try
                If strImageName.Length = 0 Then Return False

                If imgBitmap Is Nothing Then
                    Throw New System.Exception("The image bitmap named '" & strImageName & "' is not defined.")
                End If

                'If we can not find this image name then add it. Otherwise just skip over it.
                If FindImageName(strImageName) = -1 Then
                    m_imgList.Images.Add(imgBitmap, imgColor)
                    m_aryImages.Add(strImageName.ToUpper.Trim)
                End If

                Return True
            Catch ex As System.Exception
                If bThrowError Then Throw ex
                Return False
            End Try

        End Function

        Public Overloads Shared Function LoadImage(ByVal strImageName As String, _
                                 Optional ByVal bThrowError As Boolean = True) As System.Drawing.Image

            Try
                If strImageName.Length = 0 Then Return Nothing

                Dim myAssembly As System.Reflection.Assembly
                myAssembly = Util.GetAssembly(strImageName)

                Return LoadImage(myAssembly, strImageName, bThrowError)
            Catch ex As System.Exception
                If bThrowError Then Throw ex
            End Try

        End Function

        Public Overloads Shared Function LoadImage(ByVal strAssemblyName As String, _
                                                   ByVal strImageName As String, _
                                                   Optional ByVal bThrowError As Boolean = True) As System.Drawing.Image

            Try
                If strAssemblyName.Length > 0 AndAlso strImageName.Length > 0 Then
                    Dim myAssembly As System.Reflection.Assembly = System.Reflection.Assembly.Load(strAssemblyName)
                    Return LoadImage(myAssembly, strImageName, bThrowError)
                End If
            Catch ex As System.Exception
                If bThrowError Then Throw ex
            End Try

        End Function

        Public Overloads Shared Function LoadImage(ByRef myAssembly As System.Reflection.Assembly, _
                                                    ByVal strImageName As String, _
                                                    Optional ByVal bThrowError As Boolean = True) As System.Drawing.Image

            Try
                If strImageName.Length = 0 Then Return Nothing

                Dim stream As System.IO.Stream = myAssembly.GetManifestResourceStream(strImageName)

                If stream Is Nothing Then
                    Throw New System.Exception("Unable to load the resource image file '" & _
                                                strImageName & "' from the assembly '" & myAssembly.Location & "'.")
                End If

                Dim imgLoaded As New Bitmap(stream)
                Return imgLoaded

            Catch ex As System.Exception
                If bThrowError Then Throw ex
            End Try

        End Function

        Public Shared Function LoadIcon(ByRef myAssembly As System.Reflection.Assembly, _
                                        ByVal strIconName As String, _
                                        Optional ByVal bThrowError As Boolean = True) As System.Drawing.Icon

            Try
                If strIconName.Length = 0 Then Return Nothing

                Dim stream As System.IO.Stream = myAssembly.GetManifestResourceStream(strIconName)
                Dim imgLoaded As New System.Drawing.Icon(stream)

                Return imgLoaded
            Catch ex As System.Exception
                If bThrowError Then Throw ex
            End Try

        End Function

        Public Shared Function LoadText(ByRef myAssembly As System.Reflection.Assembly, _
                                        ByVal strTextName As String, _
                                        Optional ByVal bThrowError As Boolean = True) As String

            Try
                Dim stream As System.IO.Stream = myAssembly.GetManifestResourceStream(strTextName)

                If stream Is Nothing Then
                    Throw New System.Exception("Unable to load the resource text file '" & _
                                                strTextName & "' from the assembly '" & myAssembly.Location & "'.")
                End If

                Dim reader As StreamReader = New StreamReader(stream)
                Return reader.ReadToEnd()

            Catch ex As System.Exception
                If bThrowError Then Throw ex
            End Try

        End Function

        Public Function GetImageIndex(ByVal strImageName As String) As Integer

            Dim iIndex As Integer = FindImageName(strImageName)
            If iIndex = -1 AndAlso strImageName.Length > 0 Then
                Throw New System.Exception("No image named " & strImageName & " was found in the image manager.")
            End If

            Return iIndex
        End Function

        Public Function GetImage(ByVal strImageName As String) As Image
            Return m_imgList.Images(GetImageIndex(strImageName))
        End Function

        Public Overloads Shared Sub ImageToXml(ByRef oXml As Interfaces.StdXml, ByVal strName As String, ByVal image As Image)

            Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder
            Dim stringWriter As System.IO.StringWriter = New System.IO.StringWriter(sb)
            Dim xmlWriter As System.Xml.XmlTextWriter = New System.Xml.XmlTextWriter(stringWriter)
            ImageToXml(xmlWriter, strName, image)
            Dim strImageXml As String = sb.ToString & vbCrLf

            oXml.AddChildDoc(strImageXml)
            Dim iLength As Integer = strImageXml.Length()

        End Sub

        Public Overloads Shared Sub ImageToXML(ByVal tw As XmlTextWriter, ByVal strName As String, ByVal image As Image)

            tw.WriteStartElement(strName)

            Dim stream As System.IO.MemoryStream = New System.IO.MemoryStream
            image.Save(stream, image.RawFormat)
            stream.Position = 0

            Dim buffer() As Byte
            Dim readByte As Integer = 1000
            Dim br As New System.IO.BinaryReader(stream)
            ReDim buffer(999)

            Do While (readByte >= 1000)
                readByte = br.Read(buffer, 0, 1000)
                tw.WriteBase64(buffer, 0, readByte)
            Loop

            stream.Flush()
            stream.Close()

            tw.WriteEndElement()

        End Sub

        Public Overloads Shared Function XmlToImage(ByVal oXml As Interfaces.StdXml, ByVal strName As String) As Image

            oXml.FindChildElement(strName)
            Dim strImageXml As String = oXml.GetChildDoc()
            Dim iLength As Integer = strImageXml.Length()
            'strImageXml = oXml.GetChildString(strName)
            Dim stringReader As System.IO.StringReader = New System.IO.StringReader(strImageXml)
            Dim xmlReader As System.Xml.XmlTextReader = New System.Xml.XmlTextReader(stringReader)
            xmlReader.Read()
            Return XmlToImage(xmlReader)

        End Function

        Public Overloads Shared Function XmlToImage(ByVal tr As XmlTextReader) As Image

            Dim stream As System.IO.MemoryStream = New System.IO.MemoryStream
            Dim base64 As Byte()
            ReDim base64(999)
            Dim base64len As Integer = tr.ReadBase64(base64, 0, 1000)

            While (0 <> base64len)
                stream.Write(base64, 0, base64len)
                base64len = tr.ReadBase64(base64, 0, 1000)
            End While

            Return Image.FromStream(stream)
        End Function

#End Region

    End Class

End Namespace


Imports System.Windows.Forms
Imports Microsoft.VisualStudio.TestTools.UITesting.Keyboard
Imports System.Runtime.Remoting
Imports System.Runtime.Remoting.Channels
Imports System.Runtime.Remoting.Channels.Tcp
Imports System
Imports System.CodeDom.Compiler
Imports System.Configuration
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Text.RegularExpressions
Imports System.IO
Imports System.Xml.Serialization

Namespace Framework

    <Serializable()> _
    Public Class DataAnalyzer

        Public InflectionPoints As New ArrayList
        Public MaximumPoints As New ArrayList
        Public MinimumPoints As New ArrayList
        Public TestPoints As New ArrayList

        Public StartPoint As CriticalPoint
        Public EndPoint As CriticalPoint

        Public IgnoreAdditionalPoints As Boolean = False

        Public Sub FindCriticalPoints(ByVal aryTime As List(Of Double), ByVal aryData As List(Of Double), Optional ByVal iStartIdx As Integer = -1, Optional ByVal iEndIdx As Integer = -1)

            If iStartIdx < 0 Then
                iStartIdx = 0
            End If
            If iEndIdx < 0 Then
                iEndIdx = aryTime.Count - 1
            End If

            If iStartIdx >= iEndIdx Then
                Throw New System.Exception("Invalid start/end indices: Start: " & iStartIdx & ", End: " & iEndIdx)
            End If
            If iEndIdx > aryTime.Count - 1 Then
                Throw New System.Exception("end index greater than array: End: " & iEndIdx)
            End If

            Dim aryFirstDeriv As List(Of Double) = GetArrayValueDiffs(aryData)
            Dim arySecondDeriv As List(Of Double) = GetArrayValueDiffs(aryFirstDeriv)

            InflectionPoints.Clear()
            MaximumPoints.Clear()
            MinimumPoints.Clear()

            FindCriticalPoints(aryFirstDeriv, aryTime, aryData, CriticalPoint.enumCriticalType.Maximum, iStartIdx, iEndIdx, 0)
            FindCriticalPoints(aryFirstDeriv, aryTime, aryData, CriticalPoint.enumCriticalType.Minimum, iStartIdx, iEndIdx, 0)
            'FindCriticalPoints(arySecondDeriv, aryTime, aryData, CriticalPoint.enumCriticalType.Inflection, iStartIdx, iEndIdx, 0)
            FindEndPoints(aryTime, aryData, iStartIdx, iEndIdx)
        End Sub

        Protected Function GetArrayValueDiffs(ByVal aryData As List(Of Double)) As List(Of Double)
            Dim aryDiffs As New List(Of Double)

            'Get the diffs array
            For iIdx = 1 To aryData.Count - 1
                aryDiffs.Add((aryData(iIdx) - aryData(iIdx - 1)))
            Next

            Return aryDiffs
        End Function

        Protected Sub FindCriticalPoints(ByVal aryDeriv As List(Of Double), ByVal aryTime As List(Of Double), ByVal aryData As List(Of Double), ByVal eType As CriticalPoint.enumCriticalType, ByVal iStartIdx As Integer, ByVal iEndIdx As Integer, ByVal iDerivIdxOffset As Integer)

            For iIdx = 1 To aryDeriv.Count - 1
                'If sign of second deriv is different then we have an inflection point.
                If Math.Sign(aryDeriv(iIdx)) <> Math.Sign(aryDeriv(iIdx - 1)) AndAlso aryDeriv(iIdx) <> 0 Then
                    If aryDeriv(iIdx - 1) <> 0 OrElse (aryDeriv(iIdx - 1) = 0 AndAlso Math.Sign(FindFirstPrevNonZero(aryDeriv, iIdx - 1)) <> Math.Sign(aryDeriv(iIdx))) Then

                        Dim oPoint As New CriticalPoint()
                        'We want the time just before we found the change.
                        oPoint.Idx = iIdx - iDerivIdxOffset
                        'Because we lost 1 or 2 index values when computing the diff values we must offset back by the appropriate amount to find the
                        'correct time and value entries in the original data. 1 is for first deriv, 2 is for second deriv.
                        oPoint.Time = aryTime(iIdx - iDerivIdxOffset)
                        oPoint.Value = aryData(iIdx - iDerivIdxOffset)

                        Select Case (eType)
                            Case CriticalPoint.enumCriticalType.Inflection
                                InflectionPoints.Add(oPoint)
                            Case CriticalPoint.enumCriticalType.Maximum
                                If Math.Sign(aryDeriv(iIdx)) < Math.Sign(aryDeriv(iIdx - 1)) Then
                                    oPoint.Type = CriticalPoint.enumCriticalType.Maximum
                                    MaximumPoints.Add(oPoint)
                                End If
                            Case CriticalPoint.enumCriticalType.Minimum
                                If Math.Sign(aryDeriv(iIdx)) > Math.Sign(aryDeriv(iIdx - 1)) Then
                                    oPoint.Type = CriticalPoint.enumCriticalType.Minimum
                                    MinimumPoints.Add(oPoint)
                                End If
                        End Select

                    End If
                End If
            Next
        End Sub

        Protected Function FindFirstPrevNonZero(ByVal aryDeriv As List(Of Double), ByVal iStartIdx As Integer) As Double

            For iIdx As Integer = iStartIdx To 0 Step -1
                If Math.Abs(aryDeriv(iIdx)) <> 0 Then
                    Return aryDeriv(iIdx)
                End If
            Next

            Return 0
        End Function

        Protected Sub FindEndPoints(ByVal aryTime As List(Of Double), ByVal aryData As List(Of Double), ByVal iStartIdx As Integer, ByVal iEndIdx As Integer)

            StartPoint = New CriticalPoint
            StartPoint.Idx = iStartIdx
            StartPoint.Type = CriticalPoint.enumCriticalType.Endpoint
            StartPoint.Time = aryTime(iStartIdx)
            StartPoint.Value = aryData(iStartIdx)

            EndPoint = New CriticalPoint
            EndPoint.Idx = iEndIdx
            EndPoint.Type = CriticalPoint.enumCriticalType.Endpoint
            EndPoint.Time = aryTime(iEndIdx)
            EndPoint.Value = aryData(iEndIdx)

        End Sub

        Public Function SaveXml() As String
            Dim xml_serializer As New XmlSerializer(GetType(DataAnalyzer))
            Dim string_writer As New StringWriter
            xml_serializer.Serialize(string_writer, Me)

            Dim strXml As String = string_writer.ToString()

            string_writer.Close()

            Return strXml
        End Function

        Public Sub SaveData(ByVal strFile As String)
            Using outfile As New StreamWriter(strFile)
                Dim strXml As String = SaveXml()
                outfile.Write(strXml)
            End Using
        End Sub

        Public Shared Function LoadXml(ByVal strXml As String) As DataAnalyzer
            Dim xml_serializer As New XmlSerializer(GetType(DataAnalyzer))
            Dim string_reader As New StringReader(strXml)

            Dim oAnal As DataAnalyzer = _
                DirectCast(xml_serializer.Deserialize(string_reader), DataAnalyzer)
            string_reader.Close()

            Return oAnal
        End Function

        Public Shared Function LoadData(ByVal strFile As String) As DataAnalyzer
            Dim strXml As String
            Using infile As New StreamReader(strFile)
                strXml = infile.ReadToEnd()
            End Using

            Return LoadXml(strXml)
        End Function

        Public Sub CompareData(ByVal oTest As DataAnalyzer, ByVal strTestname As String)

            If Not IgnoreAdditionalPoints Then
                If oTest.InflectionPoints.Count <> Me.InflectionPoints.Count Then
                    Throw New System.Exception("Inflection points do not match for test '" & strTestname & "'")
                End If

                If oTest.MaximumPoints.Count <> Me.MaximumPoints.Count Then
                    Throw New System.Exception("Maximum points do not match for test '" & strTestname & "'")
                End If

                If oTest.MinimumPoints.Count <> Me.MinimumPoints.Count Then
                    Throw New System.Exception("Minimum points do not match for test '" & strTestname & "'")
                End If
            End If

            'test each of the points in the template and compare it to the points in the test analysis
            For Each oPoint As CriticalPoint In Me.InflectionPoints
                If oPoint.ShouldCompare Then
                    oTest.TestPoint(oPoint, strTestname)
                End If
            Next

            For Each oPoint As CriticalPoint In Me.MaximumPoints
                If oPoint.ShouldCompare Then
                    oTest.TestPoint(oPoint, strTestname)
                End If
            Next

            For Each oPoint As CriticalPoint In Me.MinimumPoints
                If oPoint.ShouldCompare Then
                    oTest.TestPoint(oPoint, strTestname)
                End If
            Next

            For Each oPoint As CriticalPoint In Me.TestPoints
                If oPoint.ShouldCompare Then
                    oTest.TestPoint(oPoint, strTestname)
                End If
            Next

            If StartPoint.ShouldCompare Then
                oTest.TestPoint(StartPoint, strTestname)
            End If

            If EndPoint.ShouldCompare Then
                oTest.TestPoint(EndPoint, strTestname)
            End If

        End Sub

        Protected Sub TestPoint(ByVal oTemp As CriticalPoint, ByVal strTestname As String)

            Select Case oTemp.Type
                Case CriticalPoint.enumCriticalType.Endpoint
                    TestEndPoint(oTemp, strTestname)
                Case CriticalPoint.enumCriticalType.Inflection
                    TestPoint(InflectionPoints, oTemp, strTestname)
                Case CriticalPoint.enumCriticalType.Maximum
                    TestPoint(MaximumPoints, oTemp, strTestname)
                Case CriticalPoint.enumCriticalType.Minimum
                    TestPoint(MinimumPoints, oTemp, strTestname)
            End Select
        End Sub

        Protected Sub TestPoint(ByVal aryPoints As ArrayList, ByVal oTemp As CriticalPoint, ByVal strTestname As String)

            For Each oTest As CriticalPoint In aryPoints
                If oTest.ComparePoint(oTemp, strTestname, Me) Then
                    Return
                End If
            Next

            Throw New System.Exception("No match for test '" & strTestname & "'. Template point: " & oTemp.SaveXml())
        End Sub

        Protected Sub TestEndPoint(ByVal oTemp As CriticalPoint, ByVal strTestName As String)

            If StartPoint.ComparePoint(oTemp, strTestName, Me) Then
                Return
            End If

            If EndPoint.ComparePoint(oTemp, strTestName, Me) Then
                Return
            End If

            Throw New System.Exception("No match for test '" & strTestName & "'. Template point: " & oTemp.SaveXml())
        End Sub

    End Class

End Namespace

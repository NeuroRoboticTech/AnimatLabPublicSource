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
    Public Class CriticalPoint

        Public Enum enumCriticalType
            Inflection
            Maximum
            Minimum
            Endpoint
        End Enum

        Public Enum enumComparisonType
            Percentage
            Fixed
            None
        End Enum

        Public Type As enumCriticalType
        Public Time As Double
        Public Value As Double
        Public Idx As Integer

        Public CompareTime As enumComparisonType = enumComparisonType.Fixed
        Public TimeError As Double = 0.005

        Public CompareValue As enumComparisonType = enumComparisonType.Fixed
        Public ValueError As Double = 0.005

        Public Overridable ReadOnly Property ShouldCompare() As Boolean
            Get
                If CompareTime <> CriticalPoint.enumComparisonType.None OrElse CompareValue <> CriticalPoint.enumComparisonType.None Then
                    Return True
                End If

                Return False
            End Get
        End Property

        Public Function SaveXml() As String
            Dim xml_serializer As New XmlSerializer(GetType(CriticalPoint))
            Dim string_writer As New StringWriter
            xml_serializer.Serialize(string_writer, Me)

            Dim strXml As String = string_writer.ToString()

            string_writer.Close()

            Return strXml
        End Function

        Public Function ComparePoint(ByVal oTemp As CriticalPoint, ByVal strTestName As String, ByVal oAnalysis As DataAnalyzer) As Boolean
            Dim bRet As Boolean = True

            If oTemp.CompareTime = enumComparisonType.Fixed Then
                If Math.Abs(oTemp.Time - Me.Time) > oTemp.TimeError Then
                    bRet = False
                End If
            ElseIf oTemp.CompareTime = enumComparisonType.Percentage Then
                Throw New System.Exception("Cannot do percentage comparisons for time values.")
            End If

            If oTemp.CompareValue = enumComparisonType.Fixed Then
                If Math.Abs(oTemp.Value - Me.Value) > oTemp.ValueError Then
                    bRet = False
                End If
            ElseIf oTemp.CompareValue = enumComparisonType.Percentage Then
                If oTemp.Type <> enumCriticalType.Maximum AndAlso oTemp.Type <> enumCriticalType.Minimum Then
                    Throw New System.Exception("Can only do percentage difference on min and max values.")
                End If

                Dim oPercComp As CriticalPoint = FindPercCompPoint(oAnalysis)

                Dim dblPercDiff As Double = 0
                If (oPercComp.Value - oTemp.Value) <> 0 Then
                    dblPercDiff = Math.Abs(oTemp.Value - Me.Value) / Math.Abs(oPercComp.Value - oTemp.Value)
                End If

                If dblPercDiff > oTemp.ValueError Then
                    bRet = False
                End If
            End If

            Return bRet
        End Function

        Protected Function FindPercCompPoint(ByVal oAnalysis As DataAnalyzer) As CriticalPoint

            Select Case Me.Type
                Case enumCriticalType.Endpoint, _
                     enumCriticalType.Inflection
                    Throw New System.Exception("percenatge comparisons are only valid for minimum and maximum values.")
                Case enumCriticalType.Maximum
                    Return FindClosestPrevPoint(oAnalysis.MinimumPoints, oAnalysis.StartPoint)
                Case enumCriticalType.Minimum
                    Return FindClosestPrevPoint(oAnalysis.MaximumPoints, oAnalysis.StartPoint)
            End Select

            Throw New System.Exception("invalid type.")
        End Function

        Protected Function FindClosestPrevPoint(ByVal aryPoints As ArrayList, ByVal oStartPoint As CriticalPoint) As CriticalPoint
            Dim minPoint As CriticalPoint = Nothing
            Dim dblDiff As Double = 0
            Dim dblMinDiff As Double = 100000000

            For Each oPoint As CriticalPoint In aryPoints
                dblDiff = oPoint.Time - Me.Time
                If dblDiff < 0 AndAlso dblDiff < dblMinDiff Then
                    minPoint = oPoint
                    dblMinDiff = dblDiff
                End If
            Next

            If minPoint Is Nothing Then
                minPoint = oStartPoint
            End If

            Return minPoint
        End Function

    End Class

End Namespace

Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports AnimatTesting.Framework

Namespace UnitTests
 
    '''<summary>
    '''This is a test class for Convert_V1ToV2Test and is intended
    '''to contain all Convert_V1ToV2Test Unit Tests
    '''</summary>
    <TestClass()> _
    Public Class DataAnalysisTest
        Inherits AnimatUnitTest

        Private testContextInstance As TestContext

        '''<summary>
        '''Gets or sets the test context which provides
        '''information about and functionality for the current test run.
        '''</summary>
        Public Property TestContext() As TestContext
            Get
                Return testContextInstance
            End Get
            Set(value As TestContext)
                testContextInstance = value
            End Set
        End Property

        <ClassInitialize()> _
        Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)
            InitializeConfiguration()
        End Sub

        'Use ClassCleanup to run code after all tests in a class have run
        <ClassCleanup()> _
        Public Shared Sub MyClassCleanup()
        End Sub


#Region "Additional test attributes"

        'Use TestInitialize to run code before running each test
        <TestInitialize()> _
        Public Overrides Sub MyTestInitialize()
        End Sub

        'Use TestCleanup to run code after each test has run
        <TestCleanup()> _
        Public Overrides Sub MyTestCleanup()
        End Sub

#End Region

        <TestMethod()> _
        Public Sub Test_CriticalPoints()
            Dim strDataFile As String = "C:\Projects\AnimatLabSDK\AnimatLabPublicSource\Libraries\AnimatTesting\TestData\UnitTests\DataAnalyzer\data.txt"
            Dim iColumn As Integer = 0
            Dim iStartIdx As Integer = -1
            Dim iEndIdx As Integer = -1

            Dim aryChartColumns() As String = {""}
            Dim aryChartData As New List(Of List(Of Double))
            Util.ReadCSVFileToList(strDataFile, aryChartColumns, aryChartData, True)

            Dim aryTime As List(Of Double) = aryChartData(0)
            Dim aryXcubed As List(Of Double) = aryChartData(1)
            Dim aryXsin As List(Of Double) = aryChartData(2)
            Dim oPoint As CriticalPoint
            Dim oAnalysis As New Framework.DataAnalyzer
            oAnalysis.FindCriticalPoints(aryTime, aryXcubed, iStartIdx, iEndIdx)

            'Assert.AreEqual(oAnalysis.InflectionPoints.Count, 1)
            Assert.AreEqual(oAnalysis.MaximumPoints.Count, 0)
            Assert.AreEqual(oAnalysis.MinimumPoints.Count, 0)

            'oPoint = DirectCast(oAnalysis.InflectionPoints(0), CriticalPoint)
            'Assert.AreEqual(oPoint.Time, 1.0)
            'Assert.AreEqual(oPoint.Value, 0.0)
            'Assert.AreEqual(oPoint.Idx, 100)

            oAnalysis.SaveData("C:\Projects\AnimatLabSDK\AnimatLabPublicSource\Libraries\AnimatTesting\TestData\UnitTests\DataAnalyzer\X3_Analysis.txt")

            Dim strXml As String = oAnalysis.SaveXml()
            Dim oAnal2 As DataAnalyzer = DataAnalyzer.LoadXml(strXml)

            'Assert.AreEqual(oAnalysis.InflectionPoints.Count, 1)
            Assert.AreEqual(oAnalysis.MaximumPoints.Count, 0)
            Assert.AreEqual(oAnalysis.MinimumPoints.Count, 0)

            'oPoint = DirectCast(oAnalysis.InflectionPoints(0), CriticalPoint)
            'Assert.AreEqual(oPoint.Time, 1.0)
            'Assert.AreEqual(oPoint.Value, 0.0)
            'Assert.AreEqual(oPoint.Idx, 100)


            oAnalysis.FindCriticalPoints(aryTime, aryXsin, iStartIdx, iEndIdx)

            'Assert.AreEqual(oAnalysis.InflectionPoints.Count, 3)
            Assert.AreEqual(oAnalysis.MaximumPoints.Count, 2)
            Assert.AreEqual(oAnalysis.MinimumPoints.Count, 1)
            'oPoint = DirectCast(oAnalysis.InflectionPoints(0), CriticalPoint)
            'Assert.AreEqual(oPoint.Time, 0.62)
            'Assert.AreEqual(oPoint.Value, 0.041581)
            'Assert.AreEqual(oPoint.Idx, 62)

            oPoint = DirectCast(oAnalysis.MaximumPoints(0), CriticalPoint)
            Assert.AreEqual(oPoint.Time, 0.31)
            Assert.AreEqual(oPoint.Value, 0.99978)
            Assert.AreEqual(oPoint.Idx, 31)

            oPoint = DirectCast(oAnalysis.MinimumPoints(0), CriticalPoint)
            Assert.AreEqual(oPoint.Time, 0.94)
            Assert.AreEqual(oPoint.Value, -0.99992)
            Assert.AreEqual(oPoint.Idx, 94)

            oAnalysis.SaveData("C:\Projects\AnimatLabSDK\AnimatLabPublicSource\Libraries\AnimatTesting\TestData\UnitTests\DataAnalyzer\Sin_Analysis.txt")

        End Sub

        <TestMethod()> _
        Public Sub Test_Compare()
            Dim strDataFile As String = "C:\Projects\AnimatLabSDK\AnimatLabPublicSource\Libraries\AnimatTesting\TestData\UnitTests\DataAnalyzer\data1.txt"
            Dim strX3Template As String = "C:\Projects\AnimatLabSDK\AnimatLabPublicSource\Libraries\AnimatTesting\TestData\UnitTests\DataAnalyzer\X3_Analysis.txt"
            Dim strSinTemplate As String = "C:\Projects\AnimatLabSDK\AnimatLabPublicSource\Libraries\AnimatTesting\TestData\UnitTests\DataAnalyzer\Sin_Analysis.txt"
            Dim iColumn As Integer = 0
            Dim iStartIdx As Integer = -1
            Dim iEndIdx As Integer = -1

            Dim oX3Compare As DataAnalyzer = DataAnalyzer.LoadData(strX3Template)
            Dim oSinCompare As DataAnalyzer = DataAnalyzer.LoadData(strSinTemplate)

            Dim aryChartColumns() As String = {""}
            Dim aryChartData As New List(Of List(Of Double))
            Util.ReadCSVFileToList(strDataFile, aryChartColumns, aryChartData, True)

            Dim aryTime As List(Of Double) = aryChartData(0)
            Dim aryXcubed As List(Of Double) = aryChartData(1)
            Dim aryXsin As List(Of Double) = aryChartData(2)
            Dim aryXsin2 As List(Of Double) = aryChartData(3)
            Dim oPoint As CriticalPoint

            Dim oAnalysis As New Framework.DataAnalyzer
            oAnalysis.FindCriticalPoints(aryTime, aryXcubed, iStartIdx, iEndIdx)

            Try
                oX3Compare.CompareData(oAnalysis, "X3")
            Catch ex As Exception
                Assert.AreEqual("No match for test 'X3'.", ex.Message.Substring(0, 23))
            End Try

            'oPoint = DirectCast(oX3Compare.InflectionPoints(0), CriticalPoint)
            'oPoint.TimeError = 0.11
            'Try
            '    oX3Compare.CompareData(oAnalysis, "X3")
            'Catch ex As Exception
            '    Assert.AreEqual("No match for test 'X3'.", ex.Message.Substring(0, 23))
            'End Try

            'oPoint.ValueError = 0.11
            'Try
            '    oX3Compare.CompareData(oAnalysis, "X3")
            'Catch ex As Exception
            '    Assert.AreEqual("No match for test 'X3'.", ex.Message.Substring(0, 23))
            'End Try

            oX3Compare.StartPoint.CompareValue = CriticalPoint.enumComparisonType.None
            oX3Compare.EndPoint.CompareValue = CriticalPoint.enumComparisonType.None
            oX3Compare.CompareData(oAnalysis, "X3")

            'Sin(analysis)
            oAnalysis.FindCriticalPoints(aryTime, aryXsin, iStartIdx, iEndIdx)

            For Each oPoint In oSinCompare.MinimumPoints
                oPoint.CompareValue = CriticalPoint.enumComparisonType.Percentage
                oPoint.ValueError = 0.005
                oPoint.TimeError = 0.03
            Next

            For Each oPoint In oSinCompare.MaximumPoints
                oPoint.CompareValue = CriticalPoint.enumComparisonType.Percentage
                oPoint.ValueError = 0.005
                oPoint.TimeError = 0.03
            Next

            For Each oPoint In oSinCompare.InflectionPoints
                oPoint.TimeError = 0.03
            Next

            Try
                oSinCompare.CompareData(oAnalysis, "Sin")
            Catch ex As Exception
                Assert.AreEqual("No match for test 'Sin'.", ex.Message.Substring(0, 24))
            End Try

            For Each oPoint In oSinCompare.MinimumPoints
                oPoint.TimeError = 0.04
                oPoint.CompareValue = CriticalPoint.enumComparisonType.Percentage
                oPoint.ValueError = 0.2
            Next

            For Each oPoint In oSinCompare.MaximumPoints
                oPoint.TimeError = 0.04
                oPoint.CompareValue = CriticalPoint.enumComparisonType.Percentage
                oPoint.ValueError = 0.2
            Next

            For Each oPoint In oSinCompare.InflectionPoints
                oPoint.TimeError = 0.04
                oPoint.ValueError = 0.2
            Next

            oSinCompare.StartPoint.TimeError = 0.04
            oSinCompare.StartPoint.CompareValue = CriticalPoint.enumComparisonType.Fixed
            oSinCompare.StartPoint.ValueError = 0.3

            oSinCompare.EndPoint.TimeError = 0.04
            oSinCompare.EndPoint.CompareValue = CriticalPoint.enumComparisonType.Fixed
            oSinCompare.EndPoint.ValueError = 0.3

            oSinCompare.CompareData(oAnalysis, "Sin")


        End Sub



    End Class


End Namespace


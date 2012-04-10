Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports AnimatTesting.Framework

Namespace UnitTests
    Namespace FileConversions

        '''<summary>
        '''This is a test class for Convert_V1ToV2Test and is intended
        '''to contain all Convert_V1ToV2Test Unit Tests
        '''</summary>
        <TestClass()> _
        Public Class Convert_V1ToV2Test
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
                    testContextInstance = Value
                End Set
            End Property

            <ClassInitialize()> _
            Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)
                InitializeConfiguration()
            End Sub

            'Use ClassCleanup to run code after all tests in a class have run
            <ClassCleanup()> _
            Public Shared Sub MyClassCleanup()
                Dim iVal As Integer = 5
            End Sub

            '''<summary>
            '''A test for ConvertFiles
            '''</summary>
            <TestMethod()> _
            Public Sub ConvertFilesTest()
                'Dim target As Convert_V1ToV2 = New Convert_V1ToV2()
                'Dim strProjectPath As String = "C:\Projects\AnimatLabSDK\Experiments\ConversionTests\OldVersion\HingeTest"
                'Dim strProjectName As String = "HingeTest"
                'Dim strProjectPath As String = "C:\Projects\AnimatLabSDK\Experiments\ConversionTests\OldVersion\ConeNN"
                'Dim strProjectName As String = "ConeNN"
                'Dim strProjectPath As String = "C:\Projects\AnimatLabSDK\Experiments\ConversionTests\OldVersion\ConeTest"
                'Dim strProjectName As String = "ConeTest"
                'Dim strProjectPath As String = "C:\Projects\AnimatLabSDK\Experiments\ConversionTests\OldVersion\NeuralTest"
                'Dim strProjectName As String = "NeuralTest"
                'Dim strProjectPath As String = "C:\Projects\AnimatLabSDK\Experiments\ConversionTests\OldVersion\SphereTest"
                'Dim strProjectName As String = "SphereTest"
                'Dim strProjectPath As String = "C:\Projects\AnimatLabSDK\Experiments\ConversionTests\OldVersion\MuscleTest"
                'Dim strProjectName As String = "MuscleTest"
                'Dim strProjectPath As String = "C:\Projects\AnimatLabSDK\Experiments\ConversionTests\OldVersion\CylinderTest"
                'Dim strProjectName As String = "CylinderTest"
                'Dim strProjectPath As String = "C:\Projects\AnimatLabSDK\Experiments\ConversionTests\OldVersion\MeshTest"
                'Dim strProjectName As String = "MeshTest"
                Dim strProject As String = "C:\Projects\AnimatLabSDK\Experiments\ConversionTests\OldVersion\PrismaticTest\PrismaticTest.aproj"

                UnitTest("AnimatGUI.dll", "AnimatGUI.DataObjects.FileConverters.Convert_V1ToV2", "ConvertFiles", New Object() {strProject})
                'This is a test.
                'Assert.Inconclusive("A method that does not return a value cannot be verified.")
            End Sub
        End Class

    End Namespace
End Namespace


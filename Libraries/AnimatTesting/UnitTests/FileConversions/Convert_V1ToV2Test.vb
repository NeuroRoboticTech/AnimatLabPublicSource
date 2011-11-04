Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports AnimatGUI.DataObjects.FileConverters

Namespace UnitTests
    Namespace FileConversions

        '''<summary>
        '''This is a test class for Convert_V1ToV2Test and is intended
        '''to contain all Convert_V1ToV2Test Unit Tests
        '''</summary>
        <TestClass()> _
        Public Class Convert_V1ToV2Test


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

#Region "Additional test attributes"
            '
            'You can use the following additional attributes as you write your tests:
            '
            'Use ClassInitialize to run code before running the first test in the class
            '<ClassInitialize()>  _
            'Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)
            'End Sub
            '
            'Use ClassCleanup to run code after all tests in a class have run
            '<ClassCleanup()>  _
            'Public Shared Sub MyClassCleanup()
            'End Sub
            '
            'Use TestInitialize to run code before running each test
            '<TestInitialize()>  _
            'Public Sub MyTestInitialize()
            'End Sub
            '
            'Use TestCleanup to run code after each test has run
            '<TestCleanup()>  _
            'Public Sub MyTestCleanup()
            'End Sub
            '
#End Region


            '''<summary>
            '''A test for ConvertFiles
            '''</summary>
            <TestMethod()> _
            Public Sub ConvertFilesTest()
                Dim target As Convert_V1ToV2 = New Convert_V1ToV2() ' TODO: Initialize to an appropriate value
                'Dim strProjectPath As String = "C:\Projects\AnimatLabSDK\Experiments\ConversionTests\OldVersion\SimpleTest"
                'Dim strProjectName As String = "SimpleTest"
                Dim strProjectPath As String = "C:\Projects\AnimatLabSDK\Experiments\ConversionTests\OldVersion\ConeNN"
                'Dim strProjectName As String = "ConeNN"
                'Dim strProjectPath As String = "C:\Projects\AnimatLabSDK\Experiments\ConversionTests\OldVersion\HingeTest"
                Dim strProjectName As String = "HingeTest"

                AnimatGUI.Framework.Util.Application = New AnimatGUI.Forms.AnimatApplication

                target.ConvertFiles(strProjectPath, strProjectName)
                'Assert.Inconclusive("A method that does not return a value cannot be verified.")
            End Sub
        End Class

    End Namespace
End Namespace


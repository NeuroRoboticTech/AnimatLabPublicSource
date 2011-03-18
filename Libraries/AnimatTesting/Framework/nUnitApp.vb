Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.Reflection

''' \namespace AnimatTesting::Framework 
'''
''' \brief  Contains classes related to starting up the nUnit testing framework. 
Namespace Framework

    ''' \brief  Starts the nUnit testing framework. 
    '''
    ''' \details When the AnimatTester application starts up it looks in the AnimatTester.exe.config file for the 
    ''' 		 Assemblyname and classname for the app to start. We are specifying this assembly and class. It then
    ''' 		 calls teh StartApplication method to start the nUnit form. The nUnit form <b>MUST</b> be run from 
    ''' 		 within the AnimatTester.exe because if you do not then the simulation license will not be valid.
    ''' 		 It then gets the nUnit file to start from the TestFile setting of the app.config file and loads that
    ''' 		 up in nUnit. Within nUnit you need to add any new test assemblies to this test file. Within the test 
    ''' 		 assemblies you need to use the appropriate Test and TestFixture atrributes so the tests showup in nUnit.
    ''' 		 
    ''' \author dcofer
    ''' \date   3/16/2011
    Public Class nUnitApp

        ''' \brief  Starts the nUnit testing dialog.
        ''' 		
        ''' \details This application starts the nUnit testing application and loads in the TestFile that was specified 
        ''' 		 in the app.config file. The user can then run tests. 
        '''
        ''' \author dcofer
        ''' \date   3/16/2011
        '''
        ''' \param  bModal  true to modal. 
        Public Overridable Sub StartApplication(ByVal bModal As Boolean)

            Try
                'Get the nUnit file
                Dim args() As String = {System.Configuration.ConfigurationManager.AppSettings("TestFile")}

                'Run nUnit
                NUnit.Gui.AppEntry.Main(args)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

    End Class

End Namespace


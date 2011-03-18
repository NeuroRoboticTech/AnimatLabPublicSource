Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.Reflection
Imports NUnit.Framework
Imports AnimatGUI
Imports AnimatGUI.Framework
Imports NUnit.Extensions.Forms

''' \namespace AnimatTesting::Tests 
'''
''' \brief  Contains unit test classes. 
Namespace Tests

    ''' \brief  Simulation Unit tests. 
    '''
    ''' \author dcofer
    ''' \date   3/16/2011
    <TestFixture()> _
    Public Class Simulation_Test
        Inherits NUnitFormTest

        ''' Reference to the animatlab application form
        Protected m_doApp As AnimatGUI.Forms.AnimatApplication

        ''' A blocking event that is used to wait until a simulation has stopped.
        Protected m_doSimEvent As New Threading.ManualResetEvent(False)

        ''' \brief  Sets up the tests
        ''' 
        ''' \details This opens the AnimatLab application form and hooks in the necessary events
        '''
        ''' \author dcofer
        ''' \date   3/16/2011
        Public Overrides Sub Setup()
            MyBase.Setup()

            m_doApp = New AnimatGUI.Forms.AnimatApplication()
            'Dim doApp As New AnimatGUI.Forms.AnimatApplication()
            'm_doApp = doApp
            m_doApp.StartApplication(False)
            AddHandler m_doApp.SimulationStopped, AddressOf Me.OnSimulationStopped

        End Sub

        ''' \brief  Tears down the test
        ''' 		
        ''' \details This closes the AnimatLab application form.
        '''
        ''' \author dcofer
        ''' \date   3/16/2011
        Public Overrides Sub TearDown()
            m_doApp.Close()
        End Sub

         <Test()> _
        Public Sub MySimTest()
            Dim iVal As Integer = 5

            m_doSimEvent.Reset()
            m_doApp.LoadProject("C:\Projects\AnimatLabSDK\Experiments\NewProject2\NewProject2.aproj")
            m_doApp.ToggleSimulation()

            'Wait here till we get the simulation stopped event.
            m_doSimEvent.WaitOne()

            m_doApp.CloseProject(False)

        End Sub

        ''' \brief  Called when the simulation stopped event is called.
        ''' 		
        ''' \details This is an event handler for the AnimatLab application. When the SimulationStopped event is fired
        ''' 		 this is called and it sets the wait event to signal to the waiting test that the simulation has stopped running
        ''' 		 and that it can then proceed with the rest of its processing. 
        '''
        ''' \author dcofer
        ''' \date   3/16/2011
        Private Sub OnSimulationStopped()
            m_doSimEvent.Set()
        End Sub

    End Class

End Namespace

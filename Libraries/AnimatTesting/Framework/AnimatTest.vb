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

Namespace Framework

    ''' \brief  Animat Test base class. 
    '''
    ''' \details All animat tests need to be derived from this class. It contains base functionality
    ''' 		 needed for the tests to function properly.
    ''' 
    ''' \author dcofer
    ''' \date   3/26/2011
    Public Class AnimatTest
        Inherits NUnitFormTest

#Region " Attributes "

        ''' Reference to the animatlab application form
        Protected m_doApp As AnimatGUI.Forms.AnimatApplication

        ''' Tells whether the simulation has finished processing
        Protected m_bSimDone As Boolean = False

        ''' Maximum simulation duration that is allowed.
        Protected m_fltMaxSimDuration As Single = 100.0

#End Region

#Region " Properties "

        Public Overridable Property MaxSimDuration() As Single
            Get
                Return m_fltMaxSimDuration
            End Get
            Set(ByVal value As Single)
                If value < 1 Then
                    Throw New System.Exception("MaxSimDuration must be >= 1")
                End If

                m_fltMaxSimDuration = value
            End Set
        End Property

#End Region

#Region " Methods "

        ''' \brief  Sets up the tests
        ''' 
        ''' \details This opens the AnimatLab application form and hooks in the necessary events
        '''
        ''' \author dcofer
        ''' \date   3/16/2011
        Public Overrides Sub Setup()
            MyBase.Setup()

            m_doApp = New AnimatGUI.Forms.AnimatApplication()
            m_doApp.StartApplication(False)

            'Hook in event handlers
            AddHandler m_doApp.SimulationStarting, AddressOf Me.OnSimulationStarting
            AddHandler m_doApp.SimulationStarted, AddressOf Me.OnSimulationStarted
            AddHandler m_doApp.SimulationResuming, AddressOf Me.OnSimulationResuming
            AddHandler m_doApp.SimulationStopped, AddressOf Me.OnSimulationStopped
            AddHandler m_doApp.SimulationPaused, AddressOf Me.OnSimulationPaused

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

        Public Overridable Sub RunSimulation(Optional ByVal strProject As String = "", Optional ByVal bCloseProject As Boolean = True)

            If strProject.Trim.Length > 0 Then
                m_doApp.LoadProject("C:\Projects\AnimatLabSDK\Experiments\NewProject\NewProject.aproj")
            End If

            m_doApp.ToggleSimulation()

            While Not m_bSimDone
                Application.DoEvents()
                Threading.Thread.Sleep(10)

                'If we have been running for more than 100 seconds then something is probably wrong.
                If m_doApp.SimulationController.CurrentSimulationTime > m_fltMaxSimDuration Then
                    Throw New System.Exception("Exeeded maximum simulation timeout period.")
                End If
            End While

            If bCloseProject Then
                m_doApp.CloseProject(False)
            End If

        End Sub

#End Region

#Region " Events "

        ''' \brief  Called when the simulation starting event is called.
        ''' 		
        ''' \details This is an event handler for the AnimatLab application. When the SimulationStarting event is fired
        ''' 		 this is called. Override this method if you need to know this information in your test.
        '''
        ''' \author dcofer
        ''' \date   3/16/2011
        Protected Overridable Sub OnSimulationStarting()

        End Sub

        ''' \brief  Called when the simulation started event is called.
        ''' 		
        ''' \details This is an event handler for the AnimatLab application. When the SimulationStarted event is fired
        ''' 		 this is called. Override this method if you need to know this information in your test.
        '''
        ''' \author dcofer
        ''' \date   3/16/2011
        Protected Overridable Sub OnSimulationStarted()

        End Sub

        ''' \brief  Called when the simulation resuming event is called.
        ''' 		
        ''' \details This is an event handler for the AnimatLab application. When the SimulationResuming event is fired
        ''' 		 this is called. Override this method if you need to know this information in your test.
        '''
        ''' \author dcofer
        ''' \date   3/16/2011
        Protected Overridable Sub OnSimulationResuming()

        End Sub

        ''' \brief  Called when the simulation pausing event is called.
        ''' 		
        ''' \details This is an event handler for the AnimatLab application. When the SimulationPausing event is fired
        ''' 		 this is called. Override this method if you need to know this information in your test.
        '''
        ''' \author dcofer
        ''' \date   3/16/2011
        Protected Overridable Sub OnSimulationPaused()

        End Sub

        ''' \brief  Called when the simulation stopped event is called.
        ''' 		
        ''' \details This is an event handler for the AnimatLab application. When the SimulationStopped event is fired
        ''' 		 this is called Override this method if you need to know this information in your test, but be sure
        ''' 		 to call the base class method.
        '''
        ''' \author dcofer
        ''' \date   3/16/2011
        Protected Overridable Sub OnSimulationStopped()
            'm_doSimEvent.Set()
            m_bSimDone = True
        End Sub


#End Region

    End Class

End Namespace

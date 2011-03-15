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

Namespace Testing

    <TestFixture()> _
    Public Class Simulation_Test
        Inherits NUnitFormTest

        Protected m_doApp As AnimatGUI.Forms.AnimatApplication
        Protected m_doSimEvent As New Threading.ManualResetEvent(False)

        Public Overrides Sub Setup()
            MyBase.Setup()

            m_doApp = New AnimatGUI.Forms.AnimatApplication()
            'Dim doApp As New AnimatGUI.Forms.AnimatApplication()
            'm_doApp = doApp
            m_doApp.StartApplication(False)
            AddHandler m_doApp.SimulationStopped, AddressOf Me.OnSimulationStopped

        End Sub

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


        Private Sub OnSimulationStopped()
            m_doSimEvent.Set()
        End Sub

    End Class

End Namespace

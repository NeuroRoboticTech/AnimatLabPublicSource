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

''' <summary> Contains unit test classes. </summary>
'''
''' <remarks> dcofer, 6/28/2011.</remarks>
Namespace Tests

    ''' <summary> Simulation test. </summary>
    '''
    ''' <remarks> dcofer, 6/28/2011.</remarks>
    <TestFixture()> _
    Public Class Simulation_Test
        Inherits Framework.AnimatTest

        ''' <summary> Tests my simulation.</summary>
        '''
        ''' <remarks> dcofer, 6/28/2011.</remarks>
        <Test()> _
       Public Sub MySimTest()

            RunSimulation("C:\Projects\AnimatLabSDK\Experiments\NewProject\NewProject.aproj")

        End Sub

        ''' <summary> Tests compare.</summary>
        '''
        ''' <remarks> dcofer, 6/28/2011.</remarks>
        <Test()> _
       Public Sub CompareTest()

            CompareSimResults("Test_", 0.1, "C:\Projects\AnimatLabSDK\Experiments\nUnitTest\nUnitTest.aproj")

        End Sub

        ''' <summary> Tests form.</summary>
        '''
        ''' <remarks> dcofer, 6/28/2011.</remarks>
        <Test()> _
       Public Sub FormTest()

            'm_doApp.LoadProject("C:\Projects\AnimatLabSDK\Experiments\nUnitTest\nUnitTest.aproj")
            Dim button As ButtonTester = New ButtonTester("OpenToolStripButton")
            button.FireEvent("Click")

        End Sub

    End Class

End Namespace

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
        Inherits Framework.AnimatTest


         <Test()> _
        Public Sub MySimTest()

            RunSimulation("C:\Projects\AnimatLabSDK\Experiments\NewProject\NewProject.aproj")

        End Sub


    End Class

End Namespace

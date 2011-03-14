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

        Public Overrides Sub Setup()
            MyBase.Setup()

            Dim strDir As String = System.Environment.CurrentDirectory
            Dim iVal As Integer = 5

            m_doApp = New AnimatGUI.Forms.AnimatApplication()
            'Dim doApp As New AnimatGUI.Forms.AnimatApplication()
            'm_doApp = doApp
            m_doApp.StartApplication(False)
        End Sub

        <Test()> _
        Public Sub MySimTest()
            Dim iVal As Integer = 5
            'If Not m_doApp Is Nothing Then
            '    Dim doApp As AnimatGUI.Forms.AnimatApplication = DirectCast(m_doApp, AnimatGUI.Forms.AnimatApplication)
            '    doApp.LoadProject("C:\Projects\AnimatLabSDK\Experiments\NewProject2\NewProject2.aproj")
            'End If

        End Sub

    End Class

End Namespace

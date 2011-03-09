Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.Reflection

Namespace Testing

    Public Class nUnitApp


        Public Overridable Sub StartApplication()

            Try
                Dim argsOrig() As String = System.Environment.GetCommandLineArgs()

                If argsOrig.Length < 2 Then
                    Throw New System.Exception("you must specify the nUnit test file to run on the command line.")
                End If

                'Get the nUnit file
                Dim args() As String = {argsOrig(1)}

                'Run nUnit
                NUnit.Gui.AppEntry.Main(args)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

    End Class

End Namespace


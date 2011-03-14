Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.Reflection

Namespace Testing

    Public Class nUnitApp


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


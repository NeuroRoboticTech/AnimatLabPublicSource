Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports AnimatGuiCtrls.Controls
Imports AnimatGUI
Imports AnimatGUI.Framework
Imports AnimatGUI.DataObjects

Namespace Forms

    Public Class ConvertPhysics

#Region " Events "

        Private Sub ConvertPhysics_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
            Try
                m_btnOk = Me.btnOk
                m_btnCancel = Me.btnCancel

                lblCurrentPhysics.Text = "Current Physics Engine: " & Util.Application.SimPhysicsSystem

                cboPhysicsEngine.Items.Clear()
                If Util.Application.SimPhysicsSystem = "Bullet" Then
                    cboPhysicsEngine.Items.Add("Vortex")
                Else
                    cboPhysicsEngine.Items.Add("Bullet")
                End If

                cboPhysicsEngine.SelectedIndex = 0

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        'Private Sub btnOk_Click(sender As System.Object, e As System.EventArgs)
        '    Try
        '        Me.Close()
        '    Catch ex As System.Exception
        '        AnimatGUI.Framework.Util.DisplayError(ex)
        '    End Try
        'End Sub

        'Private Sub btnCancel_Click(sender As System.Object, e As System.EventArgs)
        '    Try
        '        Me.Close()
        '    Catch ex As System.Exception
        '        AnimatGUI.Framework.Util.DisplayError(ex)
        '    End Try
        'End Sub

#End Region

    End Class

End Namespace

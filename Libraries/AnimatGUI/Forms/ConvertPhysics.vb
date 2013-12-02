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

#Region "Attributes"

        Protected m_bShowAllPhysicsOptions As Boolean = False

#End Region

#Region "Properties"

        Public Property ShowAllPhysicsOptions As Boolean
            Get
                Return m_bShowAllPhysicsOptions
            End Get
            Set(ByVal Value As Boolean)
                m_bShowAllPhysicsOptions = Value
            End Set
        End Property

#End Region

#Region "Methods"

#Region "Automation Methods"

        Public Sub SetPhysics(ByVal strPhysics As String)

            Dim iIdx As Integer = 0
            For Each strItem As String In cboPhysicsEngine.Items
                If strItem = strPhysics Then
                    cboPhysicsEngine.SelectedIndex = iIdx
                    Return
                End If

                iIdx = iIdx + 1
            Next

        End Sub

#End Region

#End Region

#Region " Events "

        Private Sub ConvertPhysics_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
            Try
                m_btnOk = Me.btnOk
                m_btnCancel = Me.btnCancel

                lblCurrentPhysics.Text = "Current Physics Engine: " & Util.Application.SimPhysicsSystem

                cboPhysicsEngine.Items.Clear()
                If Not m_bShowAllPhysicsOptions Then
                    If Util.Application.SimPhysicsSystem = "Bullet" Then
                        cboPhysicsEngine.Items.Add("Vortex")
                    Else
                        cboPhysicsEngine.Items.Add("Bullet")
                    End If
                Else
                    cboPhysicsEngine.Items.Add("Bullet")
                    cboPhysicsEngine.Items.Add("Vortex")
                End If

                cboPhysicsEngine.SelectedIndex = 0

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub


#End Region

    End Class

End Namespace

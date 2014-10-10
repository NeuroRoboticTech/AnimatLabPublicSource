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
            For Each doEngine As DataObjects.Physical.PhysicsEngine In cboPhysicsEngine.Items
                If doEngine.Name = strPhysics Then
                    cboPhysicsEngine.SelectedIndex = iIdx
                    Return
                End If

                iIdx = iIdx + 1
            Next

            Throw New System.Exception("Specified physics system '" & strPhysics & "' not found.")
        End Sub

#End Region

#End Region

#Region " Events "

        Private Sub ConvertPhysics_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
            Try
                m_btnOk = Me.btnOk
                m_btnCancel = Me.btnCancel

                lblCurrentPhysics.Text = "Current Physics Engine: " & Util.Application.Physics.Name

                cboPhysicsEngine.Items.Clear()
                For Each doEngine As DataObjects.Physical.PhysicsEngine In Util.Application.PhysicsEngines
                    If Util.Application.Physics.Name <> doEngine.Name OrElse (Util.Application.Physics.Name = doEngine.Name AndAlso m_bShowAllPhysicsOptions) Then
                        If doEngine.AllowUserToChoose Then
                            cboPhysicsEngine.Items.Add(doEngine)
                        End If
                    End If
                Next
                cboPhysicsEngine.SelectedIndex = 0

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub


#End Region

    End Class

End Namespace

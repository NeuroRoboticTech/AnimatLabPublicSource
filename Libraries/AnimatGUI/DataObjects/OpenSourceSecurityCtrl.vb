Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports System.Text.RegularExpressions
Imports System.Reflection

Public Class OpenSourceSecurityManager
    Inherits AnimatGuiCtrls.Security.SecurityManager

    Private m_oApp As ManagedAnimatInterfaces.ISimApplication
    Private m_bIsValidSerialNumber As Boolean = True

    Private m_tpMeshType As System.Type

    Private m_Logger As System.IO.StreamWriter

    Public Overrides ReadOnly Property IsValidSerialNumber() As Boolean
        Get
            'I am removing the for-pay upgrade. So it will always be the "Pro" version.
            Return True 'm_bIsValidSerialNumber
        End Get
    End Property

    Private ReadOnly Property LicenseFileDirectory() As String
        Get
            Dim strAppData As String = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\AnimatLab"

            If Not Directory.Exists(strAppData) Then
                Directory.CreateDirectory(strAppData)

                If Not Directory.Exists(strAppData) Then
                    Throw New System.Exception("Unable to create the app data directory for AnimatLab. Please check your permissions for App data access.")
                End If
            End If

            Return strAppData
        End Get
    End Property

    Private ReadOnly Property LicenseFileLocation() As String
        Get
            Return LicenseFileDirectory & "\AnimatLab2.exe.license"
        End Get
    End Property

    Public Sub New(ByVal oParent As ManagedAnimatInterfaces.ISimApplication)
        MyBase.New(oParent)

        Try
            m_oApp = oParent

        Catch ex As Exception
            Dim strError As String = ex.Message
            If Not ex.InnerException Is Nothing Then
                strError = strError & vbCrLf & ex.InnerException.Message
            End If
        End Try

    End Sub


    Public Overrides Function ValidateSerialNumber(ByVal strSerialNum As String) As Boolean
        m_bIsValidSerialNumber = True
        Return True
    End Function

    Public Overrides Function MachineCode() As String
        Return ""
    End Function

    Public Overrides Function ValidationFailureError() As String
        Return ""
    End Function

    Public Overrides Function IsEvaluationLicense() As Boolean
        Return False
    End Function

    Public Overrides Function IsEvaluationExpired() As Boolean
        Return False
    End Function

    Public Overrides Function EvaluationDaysLeft() As Integer
        Return -1
    End Function

End Class

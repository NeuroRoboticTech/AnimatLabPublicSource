Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.Data
Imports System.Reflection
Imports System.Runtime.Remoting
Imports System.Runtime.Remoting.Channels
Imports System.Runtime.Remoting.Channels.Tcp

Public Class Server
    Inherits MarshalByRefObject

    Public Shared m_doApp As Object
    Public Shared m_iPort As Integer = -1
    Protected m_tcpChannel As TcpChannel

    Sub New()

    End Sub

    Public Sub Initialize(ByVal oApp As Object, ByVal iPort As Integer)
        m_doApp = oApp
        m_iPort = iPort

        m_tcpChannel = New TcpChannel(m_iPort)
        System.Runtime.Remoting.Channels.ChannelServices.RegisterChannel(m_tcpChannel, True)
        RemotingConfiguration.RegisterWellKnownServiceType(GetType(AnimatServer.Server), "AnimatLab", WellKnownObjectMode.Singleton)

    End Sub

    Public Function GetProperty(ByVal strPropertyName As String) As Object
        Dim oProp As PropertyInfo = m_doApp.GetType().GetProperty(strPropertyName)
        Return oProp.GetValue(m_doApp, Nothing)
    End Function

    Public Sub SetProperty(ByVal strPropertyName As String, ByVal oData As Object)
        Dim oProp As PropertyInfo = m_doApp.GetType().GetProperty(strPropertyName)
        oProp.SetValue(m_doApp, oData, Nothing)
    End Sub

    Public Function ExecuteMethod(ByVal strMethodName As String, ByVal aryParams() As Object) As Object
        Dim oMethod As MethodInfo = m_doApp.GetType().GetMethod("ExecuteMethod")
        Return oMethod.Invoke(m_doApp, New Object() {strMethodName, aryParams})
    End Function

End Class
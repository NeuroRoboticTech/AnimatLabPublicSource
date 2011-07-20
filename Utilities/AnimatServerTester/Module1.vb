Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.Data
Imports System.Reflection
Imports System.Runtime.Remoting
Imports System.Runtime.Remoting.Channels
Imports System.Runtime.Remoting.Channels.Tcp

Module Module1


    Sub Main()

        Dim tcpChannel As New TcpChannel
        System.Runtime.Remoting.Channels.ChannelServices.RegisterChannel(tcpChannel, True)

        Dim oServer As AnimatServer.Server
        oServer = DirectCast(Activator.GetObject(GetType(AnimatServer.Server), "tcp://localhost:8080/AnimatLab"), AnimatServer.Server)

        'Dim objRet = oServer.ExecuteMethod("SimulateForSpecifiedTime", New Object() {3.44})
        Dim objRet = oServer.ExecuteMethod("ToggleSimulation", Nothing)

        objRet = oServer.GetProperty("SimIsRunning")
        Debug.WriteLine("SimIsRunning: " & objRet.ToString())

        objRet = oServer.ExecuteMethod("StopSimulation", Nothing)

        objRet = oServer.GetProperty("SimIsRunning")
        Debug.WriteLine("SimIsRunning: " & objRet.ToString())

    End Sub

End Module

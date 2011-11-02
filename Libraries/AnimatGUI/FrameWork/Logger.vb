Imports System
Imports System.Threading
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports AnimatGuiCtrls.Controls

Namespace Framework

    Public Class Logger
        Implements ManagedAnimatInterfaces.ILogger

        Protected m_strLogPrefix As String = ""
        Protected m_strLogFile As String = ""
        Protected m_eTraceLevel As ManagedAnimatInterfaces.ILogger.enumLogLevel = ManagedAnimatInterfaces.ILogger.enumLogLevel.Detail
        Protected m_swLogFile As StreamWriter

        Public Property LogPrefix As String Implements ManagedAnimatInterfaces.ILogger.LogPrefix
            Get
                Return m_strLogPrefix
            End Get
            Set(value As String)
                m_strLogPrefix = value

                m_strLogFile = m_strLogPrefix & "_" & System.Guid.NewGuid().ToString() & ".txt"

                If File.Exists(m_strLogFile) Then
                    File.Delete(m_strLogFile)
                End If

                m_swLogFile = New StreamWriter(m_strLogFile)
                m_swLogFile.AutoFlush = True
                m_swLogFile.WriteLine("Create log file.")

            End Set
        End Property

        Public Property TraceLevel As ManagedAnimatInterfaces.ILogger.enumLogLevel Implements ManagedAnimatInterfaces.ILogger.TraceLevel
            Get
                Return m_eTraceLevel
            End Get
            Set(value As ManagedAnimatInterfaces.ILogger.enumLogLevel)
                m_eTraceLevel = value
            End Set
        End Property

        Public Sub LogMsg(eLevel As ManagedAnimatInterfaces.ILogger.enumLogLevel, sMessage As String) Implements ManagedAnimatInterfaces.ILogger.LogMsg
            If eLevel <= m_eTraceLevel Then
                m_swLogFile.WriteLine(sMessage)
                m_swLogFile.Flush()
            End If
        End Sub

    End Class

End Namespace

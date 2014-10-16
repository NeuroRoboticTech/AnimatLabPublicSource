Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports AnimatGuiCtrls.Controls
Imports AnimatGUI.Framework
Imports AnimatGUI.Framework.UndoSystem

Namespace DataObjects.Behavior

    'This is used to allow neural modules the chance to dynamicelly alter the adapter that will
    'be chosen for a given origin/destination type. Typically the destination sets the adapter, but
    'if you want to use a different adapter type across items in different modules that does not work
    'easily. This gives one of the modules a chance to override the default behavior and choose their
    'adapter instead.
    Public Class LinkPair
        Public m_strOriginType As String
        Public m_strDestinationType As String
        Public m_strLinkType As String

        Public Sub New()
        End Sub

        Public Sub New(ByVal strOriginType As String, ByVal strDestinationType As String, ByVal strLinkType As String)
            m_strOriginType = strOriginType
            m_strDestinationType = strDestinationType
            m_strLinkType = strLinkType
        End Sub

        Public Function CompareNodes(ByVal strOriginType As String, ByVal strDestinationType As String) As Boolean
            'First do a direct compairson
            If m_strOriginType.Trim.ToUpper = strOriginType.Trim.ToUpper _
                AndAlso m_strDestinationType.Trim.ToUpper = strDestinationType.Trim.ToUpper Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function CompareNodes(ByVal a As LinkPair, ByVal b As LinkPair) As Boolean
            'First do a direct compairson
            If a.m_strOriginType.Trim.ToUpper = b.m_strOriginType.Trim.ToUpper _
                AndAlso a.m_strDestinationType.Trim.ToUpper = b.m_strDestinationType.Trim.ToUpper Then
                Return True
            Else
                Return False
            End If
        End Function

        Protected Shared Function Compare(ByVal a As LinkPair, ByVal b As LinkPair) As Boolean
            'First do a direct compairson
            If a.m_strOriginType.Trim.ToUpper = b.m_strOriginType.Trim.ToUpper _
                AndAlso a.m_strDestinationType.Trim.ToUpper = b.m_strDestinationType.Trim.ToUpper _
                AndAlso a.m_strLinkType.Trim.ToUpper = b.m_strLinkType.Trim.ToUpper Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Operator =(ByVal a As LinkPair, ByVal b As LinkPair) As Boolean
            Return Compare(a, b)
        End Operator

        Public Shared Operator <>(ByVal a As LinkPair, ByVal b As LinkPair) As Boolean
            Return Not Compare(a, b)
        End Operator

        Public Overrides Function ToString() As String
            Return "Origin: " + m_strOriginType + ", Destination: " + m_strDestinationType + ", Link: " + m_strLinkType
        End Function
    End Class

End Namespace

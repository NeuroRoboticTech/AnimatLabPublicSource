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

Namespace DataObjects.Behavior.SynapseTypes

    'This is to keep track of pairs of neuron indices within two different neural groups.
    Public Class NeuronIndexPair
        Public m_iFromIdx As Integer
        Public m_iToIdx As Integer

        Public Sub New()
        End Sub

        Public Sub New(ByVal iFromIdx As Integer, ByVal iToIdx As Integer)
            m_iFromIdx = iFromIdx
            m_iToIdx = m_iToIdx
        End Sub

        Public Function CompareNodes(ByVal iFromIdx As Integer, ByVal iToIdx As Integer) As Boolean
            'First do a direct compairson
            If m_iFromIdx = iFromIdx AndAlso m_iToIdx = iToIdx Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function Compare(ByVal a As NeuronIndexPair, ByVal b As NeuronIndexPair) As Boolean
            'First do a direct compairson
            If a.m_iFromIdx = b.m_iFromIdx _
                AndAlso a.m_iToIdx = b.m_iToIdx Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Operator =(ByVal a As NeuronIndexPair, ByVal b As NeuronIndexPair) As Boolean
            Return Compare(a, b)
        End Operator

        Public Shared Operator <>(ByVal a As NeuronIndexPair, ByVal b As NeuronIndexPair) As Boolean
            Return Not Compare(a, b)
        End Operator

        Public Overrides Function ToString() As String
            Return "Origin: " + m_iFromIdx.ToString() + ", Destination: " + m_iToIdx.ToString()
        End Function
    End Class

End Namespace

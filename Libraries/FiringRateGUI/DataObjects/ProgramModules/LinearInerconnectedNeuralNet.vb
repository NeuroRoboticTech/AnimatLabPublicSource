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

Namespace DataObjects.ProgramModules

    Public Class LinearInerconnectedNeuralNet
        Inherits AnimatGUI.DataObjects.ProgramModule

#Region " Attributes "

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property Description() As String
            Get
                Return "Creates a set of N neurons that are all interconnected using a sine function for thier synaptic strengths."
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            m_strName = "Linear Interconnected Neural Network"
        End Sub

        Public Overrides Sub ShowDialog()
            Try
                Dim frmNet As New Forms.ProgramModules.LinearInterconnectedNeuralNet
                frmNet.ShowDialog()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
        End Function

#End Region

#End Region

    End Class

End Namespace

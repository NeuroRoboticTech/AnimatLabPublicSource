Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports Crownwood.Magic.Common
Imports AnimatGuiCtrls.Controls
Imports Crownwood.Magic.Docking
Imports Crownwood.Magic.Menus
Imports AnimatGUI.Framework

Namespace DataObjects.ExternalStimuli

    Public Class BurstCurrent
        Inherits AnimatGUI.DataObjects.ExternalStimuli.Current

#Region " Attributes "

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Burst Current"
            End Get
        End Property

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.BurstCurrent.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property Description() As String
            Get
                Return "This stimulus applies an on and off current in repetitive cycles within bursts. So you could have an " & _
                       "On current of 20 na for 50ms and then have an off current of -10na for 20 ms. Then you could have this cycle " & _
                       "continue for 10 cycles with a space of another 10 cycles where the Off burst current is applied."
            End Get
        End Property

        Public Overrides ReadOnly Property DragImageName() As String
            Get
                Return "AnimatGUI.BurstCurrentLarge.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property CurrentType() As String
            Get
                Return "Burst"
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim doStim As BurstCurrent = New BurstCurrent(doParent)
            CloneInternal(doStim, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then doStim.AfterClone(Me, bCutData, doRoot, doStim)
            Return doStim
        End Function

#Region " DataObject Methods "

#End Region

#End Region

    End Class

End Namespace


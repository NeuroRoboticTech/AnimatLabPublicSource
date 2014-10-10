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

Namespace DataObjects.ExternalStimuli

    Public Class MotorPosition
        Inherits MotorVelocity

#Region " Attributes "

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Motor Position"
            End Get
        End Property

        Public Overrides Property Description() As String
            Get
                Return "This stimulus sets the position of a motorized joint."
            End Get
            Set(value As String)
            End Set
        End Property

        Public Overrides ReadOnly Property StimulusClassType() As String
            Get
                Return "MotorPosition"
            End Get
        End Property

        Public Overrides ReadOnly Property StimulusType() As String
            Get
                Return "Position"
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)
            m_snVelocity = New ScaledNumber(Me, "Velocity", 0, ScaledNumber.enumNumericScale.None, "rad", "rad")
        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim doStim As DataObjects.ExternalStimuli.MotorPosition = New DataObjects.ExternalStimuli.MotorPosition(doParent)
            CloneInternal(doStim, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then doStim.AfterClone(Me, bCutData, doRoot, doStim)
            Return doStim
        End Function

#Region " DataObject Methods "


#End Region

#End Region

    End Class

End Namespace

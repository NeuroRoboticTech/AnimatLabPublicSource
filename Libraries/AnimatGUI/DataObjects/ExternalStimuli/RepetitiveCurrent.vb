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

    Public Class RepetitiveCurrent
        Inherits AnimatGUI.DataObjects.ExternalStimuli.Current

#Region " Attributes "

#End Region

#Region " Properties "

        Public Overrides Property StartTime() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snStartTime
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The start time must be greater than 0.")
                End If

                If Value.ActualValue >= m_snEndTime.ActualValue Then
                    Throw New System.Exception("The start time must be less than the end time.")
                End If

                SetSimData("StartTime", Value.ActualValue.ToString, True)
                m_snStartTime.CopyData(Value)

                Me.BurstOnDuration = New ScaledNumber(Me, m_snEndTime.ActualValue - m_snStartTime.ActualValue)
                Me.BurstOffDuration = New ScaledNumber(Me, 0)

            End Set
        End Property

        Public Overrides Property EndTime() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snEndTime
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The end time must be greater than 0.")
                End If

                If Value.ActualValue <= m_snStartTime.ActualValue Then
                    Throw New System.Exception("The end time must be greater than the end time.")
                End If

                SetSimData("EndTime", Value.ActualValue.ToString, True)
                m_snEndTime.CopyData(Value)

                Me.BurstOnDuration = New ScaledNumber(Me, m_snEndTime.ActualValue - m_snStartTime.ActualValue)
                Me.BurstOffDuration = New ScaledNumber(Me, 0)
            End Set
        End Property

        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Repetitive Current"
            End Get
        End Property

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.RepetitiveCurrent.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property Description() As String
            Get
                Return "This stimulus applies an on and off current in repetitive cycles. So you could have an " & _
                       "On current of 20 na for 50ms and then have an off current of -10na for 20 ms. You can apply " & _
                       "the current for a spefic duration, or continuously."
            End Get
        End Property

        Public Overrides ReadOnly Property DragImageName() As String
            Get
                Return "AnimatGUI.RepetitiveCurrentLarge.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property CurrentType() As String
            Get
                Return "Repetitive"
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            m_snBurstOnDuration = New AnimatGUI.Framework.ScaledNumber(Me, "BurstOnDuration", 5, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "seconds", "s")
            m_snBurstOffDuration = New AnimatGUI.Framework.ScaledNumber(Me, "BurstOffDuration", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "seconds", "s")
        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim doStim As RepetitiveCurrent = New RepetitiveCurrent(doParent)
            CloneInternal(doStim, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then doStim.AfterClone(Me, bCutData, doRoot, doStim)
            Return doStim
        End Function

        Public Overrides Function GetSimulationXml(ByVal strName As String, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing) As String

            If Me.AlwaysActive Then
                Me.BurstOnDuration = New ScaledNumber(Me, 20000)
                Me.BurstOffDuration = New ScaledNumber(Me, 0)

            Else
                Me.BurstOnDuration = New ScaledNumber(Me, m_snEndTime.ActualValue - m_snStartTime.ActualValue)
                Me.BurstOffDuration = New ScaledNumber(Me, 0)

            End If

            Return MyBase.GetSimulationXml(strName, nmParentControl)
        End Function

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            If propTable.Properties.Contains("Burst Off Current") Then propTable.Properties.Remove("Burst Off Current")
            If propTable.Properties.Contains("Burst On Duration") Then propTable.Properties.Remove("Burst On Duration")
            If propTable.Properties.Contains("Burst Off Duration") Then propTable.Properties.Remove("Burst Off Duration")

        End Sub

#End Region

#End Region

    End Class

End Namespace


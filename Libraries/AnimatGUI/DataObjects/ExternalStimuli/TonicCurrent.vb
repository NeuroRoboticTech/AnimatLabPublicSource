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

    Public Class TonicCurrent
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

                Me.CycleOnDuration = New ScaledNumber(Me, m_snEndTime.ActualValue - m_snStartTime.ActualValue)
                Me.CycleOffDuration = New ScaledNumber(Me, 0)
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

                Me.CycleOnDuration = New ScaledNumber(Me, m_snEndTime.ActualValue - m_snStartTime.ActualValue)
                Me.CycleOffDuration = New ScaledNumber(Me, 0)
                Me.BurstOnDuration = New ScaledNumber(Me, m_snEndTime.ActualValue - m_snStartTime.ActualValue)
                Me.BurstOffDuration = New ScaledNumber(Me, 0)
            End Set
        End Property

        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Tonic Current"
            End Get
        End Property

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.TonicCurrent.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property Description() As String
            Get
                Return "This stimulus injects a constant current into the selected cell for a specific amount of time, or continuously."
            End Get
        End Property

        Public Overrides ReadOnly Property DragImageName() As String
            Get
                Return "AnimatGUI.TonicCurrentLarge.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property CurrentType() As String
            Get
                Return "Tonic"
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            m_snCycleOnDuration = New AnimatGUI.Framework.ScaledNumber(Me, "CycleOnDuration", 5, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "seconds", "s")
            m_snCycleOffDuration = New AnimatGUI.Framework.ScaledNumber(Me, "CycleOffDuration", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "seconds", "s")
            m_snBurstOnDuration = New AnimatGUI.Framework.ScaledNumber(Me, "BurstOnDuration", 5, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "seconds", "s")
            m_snBurstOffDuration = New AnimatGUI.Framework.ScaledNumber(Me, "BurstOffDuration", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "seconds", "s")

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim doStim As TonicCurrent = New TonicCurrent(doParent)
            CloneInternal(doStim, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then doStim.AfterClone(Me, bCutData, doRoot, doStim)
            Return doStim
        End Function

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            If propTable.Properties.Contains("Cycle On Current") Then propTable.Properties.Remove("Cycle On Current")
            If propTable.Properties.Contains("Cycle Off Current") Then propTable.Properties.Remove("Cycle Off Current")
            If propTable.Properties.Contains("Burst Off Current") Then propTable.Properties.Remove("Burst Off Current")

            If propTable.Properties.Contains("Cycle On Duration") Then propTable.Properties.Remove("Cycle On Duration")
            If propTable.Properties.Contains("Cycle Off Duration") Then propTable.Properties.Remove("Cycle Off Duration")
            If propTable.Properties.Contains("Burst On Duration") Then propTable.Properties.Remove("Burst On Duration")
            If propTable.Properties.Contains("Burst Off Duration") Then propTable.Properties.Remove("Burst Off Duration")

            If m_eValueType = enumValueType.Equation Then
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Current", m_strEquation.GetType(), "Equation", _
                                            "Stimulus Properties", "If setup to use equations, then this is the one used.", m_strEquation))
            Else
                Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = m_snCurrentOn.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Current", pbNumberBag.GetType(), "CurrentOn", _
                                            "Stimulus Properties", "The current applied during the stimulus.", pbNumberBag, _
                                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))
            End If



        End Sub

#End Region

#End Region

    End Class

End Namespace

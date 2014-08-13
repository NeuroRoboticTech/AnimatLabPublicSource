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

Namespace DataObjects
    Namespace Robotics

        Public Class PulsedLinkage
            Inherits RemoteControlLinkage

#Region " Attributes "

            Protected m_iMatchValue As Integer = 0
            Protected m_snPulseDuration As AnimatGUI.Framework.ScaledNumber
            Protected m_snPulseCurrent As AnimatGUI.Framework.ScaledNumber

#End Region

#Region " Properties "

            Public Overrides ReadOnly Property WorkspaceImageName As String
                Get
                    Return "AnimatGUI.RobotIO.gif"
                End Get
            End Property

            Public Overrides ReadOnly Property LinkageType As String
                Get
                    Return "PulsedLinkage"
                End Get
            End Property

            <Browsable(False)> _
            Public Overridable Property MatchValue As Integer
                Get
                    Return m_iMatchValue
                End Get
                Set(value As Integer)
                    If value <= 0 Then
                        Throw New System.Exception("The match value must be greater than or equal to zero.")
                    End If

                    SetSimData("MatchValue", value.ToString, True)
                    m_iMatchValue = value
                End Set
            End Property

            <Browsable(False)> _
            Public Overridable Property PulseDuration() As AnimatGUI.Framework.ScaledNumber
                Get
                    Return m_snPulseDuration
                End Get
                Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                    If Not Value Is Nothing Then
                        If Value.ActualValue <= 0 Then
                            Throw New System.Exception("The pulse duration must be greater than zero.")
                        End If

                        SetSimData("PulseDuration", Value.ActualValue.ToString(), True)
                        If Not Value Is Nothing Then m_snPulseDuration.CopyData(Value)
                    End If
                End Set
            End Property

            <Browsable(False)> _
            Public Overridable Property PulseCurrent() As AnimatGUI.Framework.ScaledNumber
                Get
                    Return m_snPulseCurrent
                End Get
                Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                    If Not Value Is Nothing Then
                        SetSimData("PulseCurrent", Value.ActualValue.ToString(), True)
                        If Not Value Is Nothing Then m_snPulseCurrent.CopyData(Value)
                    End If
                End Set
            End Property

#End Region

#Region " Methods "

            Public Sub New(ByVal doParent As Framework.DataObject)
                MyBase.New(doParent)

                m_strName = "Pulsed Link"

                m_snPulseDuration = New AnimatGUI.Framework.ScaledNumber(Me, "PulseDuration", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "seconds", "s")
                m_snPulseCurrent = New AnimatGUI.Framework.ScaledNumber(Me, "PulseCurrent", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.nano, "Amp", "A")
            End Sub

            Public Sub New(ByVal doParent As Framework.DataObject, ByVal strName As String, ByVal strSourceDataTypeID As String, ByVal doGain As Gain)
                MyBase.New(doParent)

                m_snPulseDuration = New AnimatGUI.Framework.ScaledNumber(Me, "PulseDuration", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "seconds", "s")
                m_snPulseCurrent = New AnimatGUI.Framework.ScaledNumber(Me, "PulseCurrent", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.nano, "Amp", "A")
            End Sub

            Public Overrides Sub ClearIsDirty()
                MyBase.ClearIsDirty()
                m_snPulseDuration.ClearIsDirty()
                m_snPulseCurrent.ClearIsDirty()
            End Sub

            Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                                ByVal doRoot As AnimatGUI.Framework.DataObject)
                MyBase.CloneInternal(doOriginal, bCutData, doRoot)

                Dim OrigNode As PulsedLinkage = DirectCast(doOriginal, PulsedLinkage)

                m_snPulseDuration = DirectCast(OrigNode.m_snPulseDuration.Clone(Me, bCutData, doRoot), ScaledNumber)
                m_snPulseCurrent = DirectCast(OrigNode.m_snPulseCurrent.Clone(Me, bCutData, doRoot), ScaledNumber)
            End Sub

            Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
                Dim oNew As New PulsedLinkage(doParent)
                oNew.CloneInternal(Me, bCutData, doRoot)
                If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNew.AfterClone(Me, bCutData, doRoot, oNew)
                Return oNew
            End Function

            Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
                MyBase.BuildProperties(propTable)

                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Match Value", m_iMatchValue.GetType(), "MatchValue", _
                                            "Properties", "The value from the remote control that we will be checking for.", m_iMatchValue))

                Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = m_snPulseDuration.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Pulse Duration", pbNumberBag.GetType(), "PulseDuration", _
                                            "Properties", "Sets the duration of the current pulse that will be applied each time a matching value is found at a time step. ", pbNumberBag, _
                                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

                pbNumberBag = m_snPulseCurrent.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Pulse Current", pbNumberBag.GetType(), "PulseCurrent", _
                                            "Properties", "Sets the current that will be applied for a fixed duration when a matching value is found. ", pbNumberBag, _
                                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))
            End Sub

            Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
                MyBase.LoadData(oXml)

                oXml.IntoElem()  'Into RobotInterface Element

                m_snPulseDuration.LoadData(oXml, "PulseDuration")
                m_snPulseCurrent.LoadData(oXml, "PulseCurrent")

                oXml.OutOfElem()

            End Sub

            Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
                MyBase.SaveData(oXml)

                oXml.IntoElem()

                m_snPulseDuration.SaveData(oXml, "PulseDuration")
                m_snPulseCurrent.SaveData(oXml, "PulseCurrent")

                oXml.OutOfElem()

            End Sub

            Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")
                MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

                oXml.IntoElem()

                m_snPulseDuration.SaveSimulationXml(oXml, Me, "PulseDuration")
                m_snPulseCurrent.SaveSimulationXml(oXml, Me, "PulseCurrent")

                oXml.OutOfElem()

            End Sub

#End Region

        End Class

    End Namespace
End Namespace

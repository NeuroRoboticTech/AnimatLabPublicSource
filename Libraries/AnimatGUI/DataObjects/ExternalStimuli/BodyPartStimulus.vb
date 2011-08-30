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

    Public MustInherit Class BodyPartStimulus
        Inherits AnimatGUI.DataObjects.ExternalStimuli.Stimulus

#Region " Attributes "

        Protected m_doStructure As DataObjects.Physical.PhysicalStructure
        Protected m_doBodyPart As DataObjects.Physical.BodyPart

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Overridable Property PhysicalStructure() As DataObjects.Physical.PhysicalStructure
            Get
                Return m_doStructure
            End Get
            Set(ByVal Value As DataObjects.Physical.PhysicalStructure)
                m_doStructure = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property StructureName() As String
            Get
                If Not m_doStructure Is Nothing Then
                    Return m_doStructure.Name
                End If

                Return ""
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property BodyPart() As DataObjects.Physical.BodyPart
            Get
                Return m_doBodyPart
            End Get
            Set(ByVal Value As DataObjects.Physical.BodyPart)
                m_doBodyPart = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property PartName() As String
            Get
                If Not m_doBodyPart Is Nothing Then
                    Return m_doBodyPart.ItemName
                End If

                Return ""
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property StimulusModuleName() As String
            Get
                If Not m_doBodyPart Is Nothing Then
                    Return m_doBodyPart.DataColumnModuleName
                End If

                Return ""
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property StimulusNoLongerValid() As Boolean
            Get
                Dim doStruct As DataObjects.Physical.PhysicalStructure = Nothing
                Dim doPart As DataObjects.Physical.BodyPart = Nothing

                Try
                    doStruct = Util.Environment.FindStructureFromAll(m_doStructure.ID, False)
                    doPart = doStruct.FindBodyPart(m_doBodyPart.ID, False)
                Catch ex As System.Exception
                End Try

                If Not doStruct Is Nothing AndAlso Not doPart Is Nothing Then
                    Return False
                Else
                    Return True
                End If
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doPart As DataObjects.ExternalStimuli.BodyPartStimulus = DirectCast(doOriginal, DataObjects.ExternalStimuli.BodyPartStimulus)

            m_doStructure = doPart.m_doStructure
            m_doBodyPart = doPart.m_doBodyPart
        End Sub

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            If Not m_doStructure Is Nothing Then
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Structure", GetType(String), "StructureName", _
                                            "Stimulus Properties", "The name of the structure to which this stimulus is applied.", Me.StructureName, True))
            End If

            If Not m_doBodyPart Is Nothing Then
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Body Part", GetType(String), "PartName", _
                                            "Stimulus Properties", "The name of the body part to which this stimulus is applied.", Me.PartName, True))
            End If

        End Sub

        Public Overrides Sub LoadData(ByRef oXml As AnimatGUI.Interfaces.StdXml)
            MyBase.LoadData(oXml)

            oXml.IntoElem()

            Dim strStructureID As String = oXml.GetChildString("StructureID", "")
            If strStructureID.Trim.Length > 0 Then
                Dim objStruct As Object = Util.Environment.FindStructureFromAll(strStructureID)
                If Not objStruct Is Nothing Then
                    m_doStructure = DirectCast(objStruct, DataObjects.Physical.PhysicalStructure)
                End If
            End If

            Dim strPartID As String = oXml.GetChildString("PartID", "")
            If Not m_doStructure Is Nothing AndAlso strPartID.Trim.Length > 0 Then
                m_doBodyPart = m_doStructure.FindBodyPart(strPartID, False)
            End If

            oXml.OutOfElem()

            Me.IsDirty = False
        End Sub

        Public Overrides Sub SaveData(ByRef oXml As AnimatGUI.Interfaces.StdXml)
            MyBase.SaveData(oXml)

            oXml.IntoElem()

            'Make sure the structure and body are set and that the references are still valid.
            If Not m_doStructure Is Nothing AndAlso Not m_doBodyPart Is Nothing Then
                If Not Util.Environment.FindStructureFromAll(m_doStructure.ID, False) Is Nothing AndAlso Not m_doStructure.FindBodyPart(m_doBodyPart.ID, False) Is Nothing Then
                    oXml.AddChildElement("StructureID", m_doStructure.ID)
                    oXml.AddChildElement("PartID", m_doBodyPart.ID)
                End If
            End If

            oXml.OutOfElem() ' Outof Node Element

        End Sub

        Public Overrides Sub SaveSimulationXml(ByRef oXml As AnimatGUI.Interfaces.StdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

            oXml.IntoElem()

            'Make sure the structure and body are set and that the references are still valid.
            If Not m_doStructure Is Nothing AndAlso Not m_doBodyPart Is Nothing Then
                If Not Util.Environment.FindStructureFromAll(m_doStructure.ID, False) Is Nothing AndAlso Not m_doStructure.FindBodyPart(m_doBodyPart.ID, False) Is Nothing Then
                    oXml.AddChildElement("StructureID", m_doStructure.ID)
                    oXml.AddChildElement("PartID", m_doBodyPart.ID)
                End If
            End If

            oXml.OutOfElem() ' Outof Node Element
        End Sub

#End Region

#End Region

    End Class

End Namespace

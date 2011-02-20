Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports Crownwood.Magic.Common
Imports Crownwood.Magic.Controls
Imports Crownwood.Magic.Docking
Imports Crownwood.Magic.Menus
Imports AnimatTools.Framework

Namespace DataObjects.ExternalStimuli

    Public Class BodyPartInput
        Inherits AnimatTools.DataObjects.ExternalStimuli.BodyPartStimulus

#Region " Attributes "

        Protected m_fltInput As Single = 0

#End Region

#Region " Properties "

        Public Overridable Property Input() As Single
            Get
                Return m_fltInput
            End Get
            Set(ByVal Value As Single)
                m_fltInput = Value
            End Set
        End Property

        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Body Part Input"
            End Get
        End Property

        Public Overrides ReadOnly Property ImageName() As String
            Get
                Return "AnimatTools.BodyPartInputStimulus.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property Description() As String
            Get
                Return "This stimulus allows you to add an input value to any body part."
            End Get
        End Property

        Public Overrides ReadOnly Property StimulusClassType() As String
            Get
                If Not m_doBodyPart Is Nothing Then
                    If TypeOf m_doBodyPart Is AnimatTools.DataObjects.Physical.Joint Then
                        Return "JointInput"
                    Else
                        Return "RigidBodyInput"
                    End If
                End If
                Return ""
            End Get
        End Property

        Public Overrides ReadOnly Property ControlType() As String
            Get
                Return "BodyPartInput"
            End Get
        End Property

        Public Overrides ReadOnly Property ImageName() As String
            Get
                Return "AnimatTools.BodyPartStimulus_Small.gif"
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatTools.Framework.DataObject)
            MyBase.New(doParent)

        End Sub

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatTools.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatTools.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doPart As DataObjects.ExternalStimuli.BodyPartInput = DirectCast(doOriginal, DataObjects.ExternalStimuli.BodyPartInput)

            m_fltInput = doPart.m_fltInput

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatTools.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatTools.Framework.DataObject) As AnimatTools.Framework.DataObject
            Dim doStim As DataObjects.ExternalStimuli.BodyPartInput = New DataObjects.ExternalStimuli.BodyPartInput(doParent)
            CloneInternal(doStim, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then doStim.AfterClone(Me, bCutData, doRoot, doStim)
            Return doStim
        End Function

        Public Overrides Function GetSimulationXml(ByVal strName As String, Optional ByRef nmParentControl As AnimatTools.Framework.DataObject = Nothing) As String

            If m_doStructure Is Nothing Then
                Throw New System.Exception("No structure was defined for the stimulus '" & m_strName & "'.")
            End If

            If m_doBodyPart Is Nothing Then
                Throw New System.Exception("No bodypart was defined for the stimulus '" & m_strName & "'.")
            End If

            Dim oXml As New AnimatTools.Interfaces.StdXml
            oXml.AddElement("Root")
            SaveSimulationXml(oXml, nmParentControl, strName)

            Return oXml.Serialize()
        End Function

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As Crownwood.Magic.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            propTable.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Input", m_fltInput.GetType(), "Input", _
                                        "Stimulus Properties", "The value of the input for this stimulus. ", m_fltInput))

        End Sub

        Public Overrides Sub LoadData(ByRef oXml As AnimatTools.Interfaces.StdXml)
            MyBase.LoadData(oXml)

            oXml.IntoElem()
            m_fltInput = oXml.GetChildFloat("Input", m_fltInput)
            oXml.OutOfElem()

            Me.IsDirty = False
        End Sub

        Public Overrides Sub SaveData(ByRef oXml As AnimatTools.Interfaces.StdXml)
            MyBase.SaveData(oXml)

            oXml.IntoElem()
            oXml.AddChildElement("Input", m_fltInput)
            oXml.OutOfElem() ' Outof Node Element

        End Sub

        Public Overrides Sub SaveSimulationXml(ByRef oXml As AnimatTools.Interfaces.StdXml, Optional ByRef nmParentControl As AnimatTools.Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            If m_doStructure Is Nothing Then
                Throw New System.Exception("No structure was defined for the stimulus '" & m_strName & "'.")
            End If

            If m_doBodyPart Is Nothing Then
                Throw New System.Exception("No bodypart was defined for the stimulus '" & m_strName & "'.")
            End If

            oXml.AddChildElement("Stimulus")

            oXml.IntoElem()
            oXml.AddChildElement("ID", m_strID)
            oXml.AddChildElement("Name", m_strName)
            oXml.AddChildElement("AlwaysActive", m_bAlwaysActive)

            oXml.AddChildElement("ModuleName", Me.StimulusModuleName)
            oXml.AddChildElement("Type", Me.StimulusClassType)

            oXml.AddChildElement("StructureID", m_doStructure.ID)
            oXml.AddChildElement("BodyID", m_doBodyPart.ID)

            m_snStartTime.SaveSimulationXml(oXml, Me, "StartTime")
            m_snEndTime.SaveSimulationXml(oXml, Me, "EndTime")

            oXml.AddChildElement("Input", m_fltInput)

            oXml.OutOfElem()

        End Sub

#End Region

#End Region

    End Class

End Namespace


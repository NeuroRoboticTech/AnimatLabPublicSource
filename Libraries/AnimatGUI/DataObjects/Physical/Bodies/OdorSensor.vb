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

Namespace DataObjects.Physical.Bodies

    Public Class OdorSensor
        Inherits Physical.Bodies.Sensor

#Region " Attributes "

        Protected m_thOdorType As AnimatGUI.TypeHelpers.LinkedOdorType
        Protected m_strOdorTypeID As String = ""

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.OdorSensor_TreeView.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonImageName() As String
            Get
                Return "AnimatGUI.OdorSensor_Button.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property Type() As String
            Get
                Return "OdorSensor"
            End Get
        End Property

        Public Overrides ReadOnly Property PartType() As System.Type
            Get
                Return GetType(AnimatGUI.DataObjects.Physical.Bodies.OdorSensor)
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property OdorType() As AnimatGUI.TypeHelpers.LinkedOdorType
            Get
                Return m_thOdorType
            End Get
            Set(ByVal Value As AnimatGUI.TypeHelpers.LinkedOdorType)
                Dim strID As String = ""
                If Not Value Is Nothing AndAlso Not Value.OdorType Is Nothing Then
                    strID = Value.OdorType.ID
                End If
                SetSimData("OdorTypeID", strID, True)
                m_thOdorType = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property ModuleName() As String
            Get
#If Not Debug Then
                Return "VortexAnimatPrivateSim_VC9.dll"
#Else
                Return "VortexAnimatPrivateSim_VC9D.dll"
#End If
            End Get
        End Property

#End Region

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
            m_clDiffuse = Color.LightBlue

            m_thOdorType = New AnimatGUI.TypeHelpers.LinkedOdorTypeList(Nothing)

            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("OdorValue", "Odor Value", "", "", -1000, 1000))
            m_thDataTypes.ID = "OdorValue"

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            If Not m_thOdorType Is Nothing Then m_thOdorType.ClearIsDirty()
        End Sub

        Public Overrides Function Clone(ByVal doParent As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject) As Framework.DataObject
            Dim oNewNode As New Bodies.OdorSensor(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doOrig As Bodies.OdorSensor = DirectCast(doOriginal, Bodies.OdorSensor)

            m_thOdorType = DirectCast(doOrig.m_thOdorType.Clone(Me, bCutData, doRoot), TypeHelpers.LinkedOdorType)
        End Sub

        Public Overrides Sub InitializeAfterLoad()
            MyBase.InitializeAfterLoad()

            If m_strOdorTypeID.Trim.Length > 0 Then
                If Util.Environment.OdorTypes.Contains(m_strOdorTypeID) Then
                    m_thOdorType = New AnimatGUI.TypeHelpers.LinkedOdorTypeList(Util.Environment.OdorTypes(m_strOdorTypeID))
                End If
            End If
        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Odor Type", GetType(AnimatGUI.TypeHelpers.LinkedOdorType), "OdorType", _
                          "Part Properties", "The odor type to detect.", _
                          m_thOdorType, GetType(AnimatGUI.TypeHelpers.DropDownListEditor), _
                          GetType(AnimatGUI.TypeHelpers.LinkedOdorTypeConverter)))

        End Sub

        Public Overloads Overrides Sub LoadData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByRef oXml As Interfaces.StdXml)
            MyBase.LoadData(doStructure, oXml)

            oXml.IntoElem() 'Into RigidBody Element

            m_strOdorTypeID = oXml.GetChildString("OdorTypeID", "")

            oXml.OutOfElem() 'Outof RigidBody Element

        End Sub

        Public Overloads Overrides Sub SaveData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByRef oXml As Interfaces.StdXml)
            MyBase.SaveData(doStructure, oXml)

            oXml.IntoElem() 'Into Child Elemement

            If Not m_thOdorType Is Nothing AndAlso Not m_thOdorType.OdorType Is Nothing Then
                oXml.AddChildElement("OdorTypeID", m_thOdorType.OdorType.ID)
            End If

            oXml.OutOfElem() 'Outof BodyPart Element

        End Sub

        Public Overrides Sub SaveSimulationXml(ByRef oXml As Interfaces.StdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

            oXml.IntoElem()

            If Not m_thOdorType Is Nothing AndAlso Not m_thOdorType.OdorType Is Nothing Then
                oXml.AddChildElement("OdorTypeID", m_thOdorType.OdorType.ID)
            End If

            oXml.OutOfElem()

        End Sub

    End Class


End Namespace

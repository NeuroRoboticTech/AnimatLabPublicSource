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

Namespace DataObjects.Physical.Joints

    Public Class Linear
        Inherits Physical.Joint

#Region " Enums "

        Public Enum enumLinearType
            Linear1D
            Linear2D
            Linear3D
        End Enum
#End Region

#Region " Attributes "

        Protected m_eLinearType As enumLinearType

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.Joint.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonImageName() As String
            Get
                Return "AnimatGUI.Joint.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property Type() As String
            Get
                Return "Linear"
            End Get
        End Property

        Public Overrides ReadOnly Property PartType() As System.Type
            Get
                Return GetType(AnimatGUI.DataObjects.Physical.Joints.Linear)
            End Get
        End Property

        Public Overridable Property LinearType() As enumLinearType
            Get
                Return m_eLinearType
            End Get
            Set(ByVal value As enumLinearType)
                SetSimData("LinearType", value.ToString(), True)
                m_eLinearType = value
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property ModuleName() As String
            Get
#If Not Debug Then
                Return "VortexAnimatPrivateSim_VC10.dll"
#Else
                Return "VortexAnimatPrivateSim_VC10D.dll"
#End If
            End Get
        End Property

        Public Overrides ReadOnly Property AllowUserAdd As Boolean
            Get
                'TODO: This part does not work yet so I am disabling the ability for users to add it.
                Return False
            End Get
        End Property

#End Region

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
            m_strDescription = ""
            m_eLinearType = enumLinearType.Linear1D

            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("WorldPositionX", "Position X Axis", "Meters", "m", -10, 10))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("WorldPositionY", "Position Y Axis", "Meters", "m", -10, 10))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("WorldPositionZ", "Position Z Axis", "Meters", "m", -10, 10))
            m_thDataTypes.ID = "WorldPositionX"

        End Sub

        Public Overrides Sub SetDefaultSizes()
            MyBase.SetDefaultSizes()
            m_snSize.ActualValue = 0.05 * Util.Environment.DistanceUnitValue
        End Sub

        Public Overrides Function Clone(ByVal doParent As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject) As Framework.DataObject
            Dim oNewNode As New Joints.Linear(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doOrig As Joints.Linear = DirectCast(doOriginal, Joints.Linear)
            m_eLinearType = doOrig.m_eLinearType
        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Linear Type", m_eLinearType.GetType(), "LinearType", _
                                        "Part Properties", "Sets the linear type for this joint.", m_eLinearType))

        End Sub

        Public Overloads Overrides Sub LoadData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByRef oXml As Interfaces.StdXml)
            MyBase.LoadData(doStructure, oXml)

            oXml.IntoElem() 'Into Joint Element

            m_eLinearType = DirectCast([Enum].Parse(GetType(enumLinearType), oXml.GetChildString("LinearType"), True), enumLinearType)

            oXml.OutOfElem() 'Outof Joint Element

        End Sub

        Public Overloads Overrides Sub SaveData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByRef oXml As Interfaces.StdXml)
            MyBase.SaveData(doStructure, oXml)

            oXml.IntoElem() 'Into Joint Elemement

            oXml.AddChildElement("LinearType", m_eLinearType.ToString)

            oXml.OutOfElem() 'Outof Joint Element

        End Sub

        Public Overrides Sub SaveSimulationXml(ByRef oXml As Interfaces.StdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

            oXml.IntoElem()

            oXml.AddChildElement("LinearType", m_eLinearType.ToString)

            oXml.OutOfElem()

        End Sub

    End Class


End Namespace

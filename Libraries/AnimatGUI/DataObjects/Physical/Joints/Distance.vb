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

Namespace DataObjects.Physical.Joints

    Public Class Distance
        Inherits Physical.Joint

#Region " Attributes "

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.BallSocket_Treeview.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonImageName() As String
            Get
                Return "AnimatGUI.Distance_SelectType.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property Type() As String
            Get
                Return "Distance"
            End Get
        End Property

        Public Overrides ReadOnly Property PartType() As System.Type
            Get
                Return GetType(AnimatGUI.DataObjects.Physical.Joints.Distance)
            End Get
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

#End Region

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
            m_strDescription = ""

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
            Dim oNewNode As New Joints.Distance(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Name", m_strID.GetType(), "Name", _
                                        "Part Properties", "The name of this item.", m_strName))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("ID", Me.ID.GetType(), "ID", _
                                        "Part Properties", "ID", Me.ID, True))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Description", m_strDescription.GetType(), "Description", _
                                        "Part Properties", "Sets the description for this body part.", m_strDescription, _
                                        GetType(AnimatGUI.TypeHelpers.MultiLineStringTypeEditor)))

        End Sub

    End Class


End Namespace

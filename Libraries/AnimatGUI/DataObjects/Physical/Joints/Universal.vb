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

    Public Class Universal
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
                Return "AnimatGUI.Universal_SelectType.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property Type() As String
            Get
                Return "Universal"
            End Get
        End Property

        Public Overrides ReadOnly Property PartType() As System.Type
            Get
                Return GetType(AnimatGUI.DataObjects.Physical.Joints.Universal)
            End Get
        End Property

        Public Overrides ReadOnly Property UsesRadians() As Boolean
            Get
                Return True
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

            If Util.Application.Physics.AllowConstraintRelaxation Then
                m_doRelaxation1 = New ConstraintRelaxation(Me, "X Axis Displacement", "Sets the relaxation for the X displacement axis.", ConstraintRelaxation.enumCoordinateID.Relaxation1)
                m_doRelaxation2 = New ConstraintRelaxation(Me, "Y Axis Displacement", "Sets the relaxation for the Y displacement axis.", ConstraintRelaxation.enumCoordinateID.Relaxation2)
                m_doRelaxation3 = New ConstraintRelaxation(Me, "Z Axis Displacement", "Sets the relaxation for the Z displacement axis.", ConstraintRelaxation.enumCoordinateID.Relaxation3)
                m_doRelaxation4 = New ConstraintRelaxation(Me, "X Axis Rotation", "Sets the relaxation for the X displacement axis.", ConstraintRelaxation.enumCoordinateID.Relaxation4)
            End If

        End Sub

        Public Overrides Sub SetDefaultSizes()
            MyBase.SetDefaultSizes()
            m_snSize.ActualValue = 0.05 * Util.Environment.DistanceUnitValue
        End Sub

        Public Overrides Function Clone(ByVal doParent As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject) As Framework.DataObject
            Dim oNewNode As New Joints.Universal(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function


    End Class


End Namespace

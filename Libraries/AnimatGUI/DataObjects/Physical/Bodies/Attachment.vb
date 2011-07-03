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

    Public Class Attachment
        Inherits Physical.Bodies.Sensor

#Region " Attributes "


#End Region

#Region " Properties "

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.MuscleAttachment_Treeview.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonImageName() As String
            Get
                Return "AnimatGUI.MuscleAttachment_Button.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property Type() As String
            Get
                Return "Attachment"
            End Get
        End Property

        Public Overrides ReadOnly Property PartType() As System.Type
            Get
                Return GetType(AnimatGUI.DataObjects.Physical.Bodies.Attachment)
            End Get
        End Property
#End Region

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
            m_clDiffuse = Color.Orange
        End Sub

        Public Overrides Function Clone(ByVal doParent As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject) As Framework.DataObject
            Dim oNewNode As New Bodies.Attachment(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Public Overrides Sub SetupPartTypesExclusions()
            Util.Application.AddPartTypeExclusion(GetType(Terrain), Me.GetType)
            Util.Application.AddPartTypeExclusion(GetType(Plane), Me.GetType)
            Util.Application.AddPartTypeExclusion(GetType(FluidPlane), Me.GetType)
            Util.Application.AddPartTypeExclusion(GetType(LinearHillMuscle), Me.GetType)
            Util.Application.AddPartTypeExclusion(GetType(LinearHillStretchReceptor), Me.GetType)
            Util.Application.AddPartTypeExclusion(GetType(Mouth), Me.GetType)
            Util.Application.AddPartTypeExclusion(GetType(OdorSensor), Me.GetType)
            Util.Application.AddPartTypeExclusion(GetType(Stomach), Me.GetType)
        End Sub

    End Class


End Namespace

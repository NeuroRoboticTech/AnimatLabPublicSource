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

Namespace DataObjects.Behavior.Nodes

    Public Class RigidBody
        Inherits BodyPart

#Region " Attributes "

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Rigid Body"
            End Get
        End Property

        Public Overrides ReadOnly Property NeuralModuleType() As System.Type
            Get
                Return GetType(AnimatGUI.DataObjects.Behavior.PhysicsModule)
            End Get
        End Property

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.Bone.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property DragImageName As String
            Get
                Return "AnimatGUI.DragBone.gif"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property CanBeCharted() As Boolean
            Get
                Return False
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property AllowStimulus() As Boolean
            Get
                Return False
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            Try
                m_thLinkedPart = New AnimatGUI.TypeHelpers.LinkedBodyPartTree(Me)

                m_tpBodyPartType = GetType(AnimatGUI.DataObjects.Physical.RigidBody)

                Me.Size = New System.Drawing.SizeF(30, 150)
                Me.DiagramImageName = "AnimatGUI.BoneNodeImage.gif"
                Me.Name = "Joint"
                Me.Description = "This node allows the user to collect data directly from a rigid body."

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNewNode As New Behavior.Nodes.RigidBody(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Public Overrides Sub CheckForErrors()
            MyBase.CheckForErrors()

            If Util.Application.ProjectErrors Is Nothing Then Return

            If m_thLinkedPart Is Nothing OrElse m_thLinkedPart.BodyPart Is Nothing Then
                If Util.Application.ProjectErrors.Errors.Contains(DiagramErrors.DataError.GenerateID(Me, DiagramError.enumErrorTypes.RigidBodyNotSet)) Then
                    Dim deError As DiagramErrors.DataError = New DiagramErrors.DataError(Me, DiagramError.enumErrorLevel.Error, DiagramError.enumErrorTypes.RigidBodyNotSet, _
                                                  "The reference for the rigid body '" + Me.Text + "' is not set.")
                    Util.Application.ProjectErrors.Errors.Add(deError.ID, deError)
                End If
            Else
                If Util.Application.ProjectErrors.Errors.Contains(DiagramErrors.DataError.GenerateID(Me, DiagramError.enumErrorTypes.RigidBodyNotSet)) Then
                    Util.Application.ProjectErrors.Errors.Remove(DiagramErrors.DataError.GenerateID(Me, DiagramError.enumErrorTypes.RigidBodyNotSet))
                End If
            End If

        End Sub

        Public Overrides Function CreateDataItemTreeView(ByVal frmDataItem As Forms.Tools.SelectDataItem, ByVal tnParent As Crownwood.DotNetMagic.Controls.Node, ByVal tpTemplatePartType As Type) As Crownwood.DotNetMagic.Controls.Node
            Return Nothing
        End Function

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            'Now lets add the property for the linked muscle.
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Body ID", GetType(AnimatGUI.TypeHelpers.LinkedBodyPartTree), "LinkedPart", _
                                        "Body Properties", "Associates this body to an ID of a body that exists within the  organism.", m_thLinkedPart, _
                                        GetType(AnimatGUI.TypeHelpers.DropDownTreeEditor), _
                                        GetType(AnimatGUI.TypeHelpers.LinkedBodyPartTypeConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Node Type", GetType(String), "TypeName", _
                                        "Body Properties", "Returns the type of this node.", TypeName(), True))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Description", m_strDescription.GetType(), "ToolTip", _
                                        "Body Properties", "Sets the description for this body connection.", m_strToolTip, _
                                        GetType(AnimatGUI.TypeHelpers.MultiLineStringTypeEditor)))

        End Sub

        Public Overrides Sub InitializeAfterLoad()

            Try
                MyBase.InitializeAfterLoad()

                If m_bIsInitialized Then
                    Dim bpPart As AnimatGUI.DataObjects.Physical.BodyPart
                    If (m_strLinkedBodyPartID.Length > 0) Then
                        bpPart = m_doOrganism.FindBodyPart(m_strLinkedBodyPartID, False)

                        m_thLinkedPart = New AnimatGUI.TypeHelpers.LinkedBodyPartTree(m_doOrganism, bpPart, m_tpBodyPartType)
                        SetDataType()
                    End If
                End If

            Catch ex As System.Exception
                m_bIsInitialized = False
            End Try
        End Sub

        ''' \brief  Initializes the simulation references.
        '''
        ''' \details This type of object has no related simulation object, so do not call base class here.
        ''' 		 
        ''' \author dcofer
        ''' \date   9/25/2011
        Public Overrides Sub InitializeSimulationReferences()
        End Sub

#End Region

#End Region

    End Class

End Namespace


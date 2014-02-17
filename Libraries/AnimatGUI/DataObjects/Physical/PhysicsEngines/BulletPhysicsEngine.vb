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

Namespace DataObjects.Physical.PhysicsEngines

    Public Class BulletPhysicsEngine
        Inherits PhysicsEngine

#Region " Attributes "

#End Region

#Region " Properties "

        Public Overrides Property Name As String
            Get
                Return "Bullet"
            End Get
            Set(value As String)
                'Cannot change the name.
            End Set
        End Property

        Public Overrides ReadOnly Property AllowUserToChoose() As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overrides ReadOnly Property UseMassForRigidBodyDefinitions() As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overrides ReadOnly Property AllowDynamicTriangleMesh() As Boolean
            Get
                Return False
            End Get
        End Property

        Public Overrides ReadOnly Property AllowPhysicsSubsteps() As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overrides ReadOnly Property ShowSeparateConstraintLimits() As Boolean
            Get
                Return False
            End Get
        End Property

        Public Overrides ReadOnly Property UseHydrodynamicsMagnus() As Boolean
            Get
                Return False
            End Get
        End Property

        Public Overrides ReadOnly Property ProvidesJointForceFeedback() As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overrides ReadOnly Property GenerateMotorAssist() As Boolean
            Get
                Return True
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            m_aryConstraintRelaxations.Add("Hinge_LinearX", True)
            m_aryConstraintRelaxations.Add("Hinge_LinearY", True)
            m_aryConstraintRelaxations.Add("Hinge_LinearZ", True)
            m_aryConstraintRelaxations.Add("Hinge_AngularX", True)
            m_aryConstraintRelaxations.Add("Hinge_AngularY", True)
            m_aryConstraintRelaxations.Add("Hinge_AngularZ", True)

            m_aryConstraintRelaxations.Add("Prismatic_LinearX", True)
            m_aryConstraintRelaxations.Add("Prismatic_LinearY", True)
            m_aryConstraintRelaxations.Add("Prismatic_LinearZ", True)
            m_aryConstraintRelaxations.Add("Prismatic_AngularX", True)
            m_aryConstraintRelaxations.Add("Prismatic_AngularY", True)
            m_aryConstraintRelaxations.Add("Prismatic_AngularZ", True)

            m_aryConstraintRelaxations.Add("RPRO_LinearX", True)
            m_aryConstraintRelaxations.Add("RPRO_LinearY", True)
            m_aryConstraintRelaxations.Add("RPRO_LinearZ", True)
            m_aryConstraintRelaxations.Add("RPRO_AngularX", True)
            m_aryConstraintRelaxations.Add("RPRO_AngularY", True)
            m_aryConstraintRelaxations.Add("RPRO_AngularZ", True)

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim doItem As New BulletPhysicsEngine(doParent)
            Return doItem
        End Function

        Public Overrides Function CreateJointRelaxation(ByVal strType As String, ByVal eCoordinate As ConstraintRelaxation.enumCoordinateID) As ConstraintRelaxation
            Select Case (strType.Trim.ToUpper)
                Case "BALLSOCKET"
                    Return CreateEmptyJointRelaxation(eCoordinate)
                Case "DISTANCE"
                    Return CreateEmptyJointRelaxation(eCoordinate)
                Case "FREEJOINT"
                    Return CreateEmptyJointRelaxation(eCoordinate)
                Case "HINGE"
                    Return CreateHingeJointRelaxation(eCoordinate)
                Case "PRISMATIC"
                    Return CreatePrismaticJointRelaxation(eCoordinate)
                Case "RPRO"
                    Return CreateDefaultJointRelaxation(eCoordinate)
                Case "STATIC"
                    Return CreateEmptyJointRelaxation(eCoordinate)
                Case "UNIVERSAL"
                    Return CreateEmptyJointRelaxation(eCoordinate)
            End Select
        End Function

        Protected Overridable Function CreateDefaultJointRelaxation(ByVal eCoordinate As ConstraintRelaxation.enumCoordinateID) As ConstraintRelaxation
            Select Case (eCoordinate)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation1
                    Return New ConstraintRelaxationBullet(Me, "Z Axis Displacement", "Sets the relaxation for the Z displacement axis.", ConstraintRelaxation.enumCoordinateID.Relaxation1, ConstraintRelaxation.enumCoordinateAxis.LinearZ)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation2
                    Return New ConstraintRelaxationBullet(Me, "X Axis Displacement", "Sets the relaxation for the X displacement axis.", ConstraintRelaxation.enumCoordinateID.Relaxation2, ConstraintRelaxation.enumCoordinateAxis.LinearX)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation3
                    Return New ConstraintRelaxationBullet(Me, "Y Axis Displacement", "Sets the relaxation for the Y displacement axis.", ConstraintRelaxation.enumCoordinateID.Relaxation3, ConstraintRelaxation.enumCoordinateAxis.LinearY)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation4
                    Return New ConstraintRelaxationBullet(Me, "Z Axis Rotation", "Sets the relaxation for the Z rotation axis.", ConstraintRelaxation.enumCoordinateID.Relaxation4, ConstraintRelaxation.enumCoordinateAxis.AngularZ)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation5
                    Return New ConstraintRelaxationBullet(Me, "X Axis Rotation", "Sets the relaxation for the X rotation axis.", ConstraintRelaxation.enumCoordinateID.Relaxation5, ConstraintRelaxation.enumCoordinateAxis.AngularX)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation6
                    Return New ConstraintRelaxationBullet(Me, "Y Axis Rotation", "Sets the relaxation for the Y rotation axis.", ConstraintRelaxation.enumCoordinateID.Relaxation6, ConstraintRelaxation.enumCoordinateAxis.AngularY)
            End Select

            Return Nothing
        End Function

        Protected Overridable Function CreateHingeJointRelaxation(ByVal eCoordinate As ConstraintRelaxation.enumCoordinateID) As ConstraintRelaxation
            Select Case (eCoordinate)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation1
                    Return New ConstraintRelaxationBullet(Me, "Z Axis Displacement", "Sets the relaxation for the Z displacement axis.", ConstraintRelaxation.enumCoordinateID.Relaxation1, ConstraintRelaxation.enumCoordinateAxis.LinearZ)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation2
                    Return New ConstraintRelaxationBullet(Me, "X Axis Displacement", "Sets the relaxation for the X displacement axis.", ConstraintRelaxation.enumCoordinateID.Relaxation2, ConstraintRelaxation.enumCoordinateAxis.LinearX)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation3
                    Return New ConstraintRelaxationBullet(Me, "Y Axis Displacement", "Sets the relaxation for the Y displacement axis.", ConstraintRelaxation.enumCoordinateID.Relaxation3, ConstraintRelaxation.enumCoordinateAxis.LinearY)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation4
                    Return New ConstraintRelaxationBullet(Me, "Z Axis Rotation", "Sets the relaxation for the Z rotation axis.", ConstraintRelaxation.enumCoordinateID.Relaxation4, ConstraintRelaxation.enumCoordinateAxis.AngularZ)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation5
                    Return New ConstraintRelaxationBullet(Me, "X Axis Rotation", "Sets the relaxation for the X rotation axis.", ConstraintRelaxation.enumCoordinateID.Relaxation5, ConstraintRelaxation.enumCoordinateAxis.AngularX)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation6
                    Return New ConstraintRelaxationBullet(Me, "Y Axis Rotation", "Sets the relaxation for the Y rotation axis.", ConstraintRelaxation.enumCoordinateID.Relaxation6, ConstraintRelaxation.enumCoordinateAxis.AngularY)
            End Select

            Return Nothing
        End Function

        Protected Overridable Function CreatePrismaticJointRelaxation(ByVal eCoordinate As ConstraintRelaxation.enumCoordinateID) As ConstraintRelaxation
            Select Case (eCoordinate)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation1
                    Return New ConstraintRelaxationBullet(Me, "X Axis Displacement", "Sets the relaxation for the X displacement axis.", ConstraintRelaxation.enumCoordinateID.Relaxation1, ConstraintRelaxation.enumCoordinateAxis.LinearX)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation2
                    Return New ConstraintRelaxationBullet(Me, "Y Axis Displacement", "Sets the relaxation for the Y displacement axis.", ConstraintRelaxation.enumCoordinateID.Relaxation2, ConstraintRelaxation.enumCoordinateAxis.LinearY)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation3
                    Return New ConstraintRelaxationBullet(Me, "Z Axis Displacement", "Sets the relaxation for the Z displacement axis.", ConstraintRelaxation.enumCoordinateID.Relaxation3, ConstraintRelaxation.enumCoordinateAxis.LinearZ)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation4
                    Return New ConstraintRelaxationBullet(Me, "Z Axis Rotation", "Sets the relaxation for the Z rotation axis.", ConstraintRelaxation.enumCoordinateID.Relaxation4, ConstraintRelaxation.enumCoordinateAxis.AngularZ)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation5
                    Return New ConstraintRelaxationBullet(Me, "X Axis Rotation", "Sets the relaxation for the X rotation axis.", ConstraintRelaxation.enumCoordinateID.Relaxation5, ConstraintRelaxation.enumCoordinateAxis.AngularX)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation6
                    Return New ConstraintRelaxationBullet(Me, "Y Axis Rotation", "Sets the relaxation for the Y rotation axis.", ConstraintRelaxation.enumCoordinateID.Relaxation6, ConstraintRelaxation.enumCoordinateAxis.AngularY)
            End Select

            Return Nothing
        End Function

#End Region

    End Class

End Namespace

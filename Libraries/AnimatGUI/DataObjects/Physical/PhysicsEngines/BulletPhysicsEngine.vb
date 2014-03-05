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

        Public Overrides ReadOnly Property LibraryVersionPrefix() As String
            Get
                If m_strLibraryVersion.ToUpper() = "DOUBLE" Then
                    Return "_double"
                Else
                    Return "_single"
                End If
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

            m_aryLibraryVersions.Add("Single")
            m_aryLibraryVersions.Add("Double")

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim doItem As New BulletPhysicsEngine(doParent)
            Return doItem
        End Function

        Public Overrides Function CreateJointRelaxation(ByVal strType As String, ByVal eCoordinate As ConstraintRelaxation.enumCoordinateID, ByVal doParent As Framework.DataObject) As ConstraintRelaxation
            Select Case (strType.Trim.ToUpper)
                Case "BALLSOCKET"
                    Return CreateEmptyJointRelaxation(eCoordinate, doParent)
                Case "DISTANCE"
                    Return CreateEmptyJointRelaxation(eCoordinate, doParent)
                Case "FREEJOINT"
                    Return CreateEmptyJointRelaxation(eCoordinate, doParent)
                Case "HINGE"
                    Return CreateHingeJointRelaxation(eCoordinate, doParent)
                Case "PRISMATIC"
                    Return CreatePrismaticJointRelaxation(eCoordinate, doParent)
                Case "RPRO"
                    Return CreateRPROJointRelaxation(eCoordinate, doParent)
                Case "STATIC"
                    Return CreateEmptyJointRelaxation(eCoordinate, doParent)
                Case "UNIVERSAL"
                    Return CreateEmptyJointRelaxation(eCoordinate, doParent)
            End Select
        End Function

        Protected Overridable Function CreateHingeJointRelaxation(ByVal eCoordinate As ConstraintRelaxation.enumCoordinateID, ByVal doParent As Framework.DataObject) As ConstraintRelaxation
            Select Case (eCoordinate)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation1
                    Return New ConstraintRelaxationBullet(doParent, "Z Axis Displacement", "Sets the relaxation for the Z displacement axis.", ConstraintRelaxation.enumCoordinateID.Relaxation1, ConstraintRelaxation.enumCoordinateAxis.LinearZ, False)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation2
                    Return New ConstraintRelaxationBullet(doParent, "X Axis Displacement", "Sets the relaxation for the X displacement axis.", ConstraintRelaxation.enumCoordinateID.Relaxation2, ConstraintRelaxation.enumCoordinateAxis.LinearX, False)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation3
                    Return New ConstraintRelaxationBullet(doParent, "Y Axis Displacement", "Sets the relaxation for the Y displacement axis.", ConstraintRelaxation.enumCoordinateID.Relaxation3, ConstraintRelaxation.enumCoordinateAxis.LinearY, False)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation4
                    Return New ConstraintRelaxationBullet(doParent, "Z Axis Rotation", "Sets the relaxation for the Z rotation axis.", ConstraintRelaxation.enumCoordinateID.Relaxation4, ConstraintRelaxation.enumCoordinateAxis.AngularZ, True)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation5
                    Return New ConstraintRelaxationBullet(doParent, "X Axis Rotation", "Sets the relaxation for the X rotation axis.", ConstraintRelaxation.enumCoordinateID.Relaxation5, ConstraintRelaxation.enumCoordinateAxis.AngularX, False)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation6
                    Return New ConstraintRelaxationBullet(doParent, "Y Axis Rotation", "Sets the relaxation for the Y rotation axis.", ConstraintRelaxation.enumCoordinateID.Relaxation6, ConstraintRelaxation.enumCoordinateAxis.AngularY, False)
            End Select

            Return Nothing
        End Function

        Protected Overridable Function CreatePrismaticJointRelaxation(ByVal eCoordinate As ConstraintRelaxation.enumCoordinateID, ByVal doParent As Framework.DataObject) As ConstraintRelaxation
            Select Case (eCoordinate)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation1
                    Return New ConstraintRelaxationBullet(doParent, "X Axis Displacement", "Sets the relaxation for the X displacement axis.", ConstraintRelaxation.enumCoordinateID.Relaxation1, ConstraintRelaxation.enumCoordinateAxis.LinearX, True)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation2
                    Return New ConstraintRelaxationBullet(doParent, "Y Axis Displacement", "Sets the relaxation for the Y displacement axis.", ConstraintRelaxation.enumCoordinateID.Relaxation2, ConstraintRelaxation.enumCoordinateAxis.LinearY, False)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation3
                    Return New ConstraintRelaxationBullet(doParent, "Z Axis Displacement", "Sets the relaxation for the Z displacement axis.", ConstraintRelaxation.enumCoordinateID.Relaxation3, ConstraintRelaxation.enumCoordinateAxis.LinearZ, False)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation4
                    Return New ConstraintRelaxationBullet(doParent, "Z Axis Rotation", "Sets the relaxation for the Z rotation axis.", ConstraintRelaxation.enumCoordinateID.Relaxation4, ConstraintRelaxation.enumCoordinateAxis.AngularZ, False)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation5
                    Return New ConstraintRelaxationBullet(doParent, "X Axis Rotation", "Sets the relaxation for the X rotation axis.", ConstraintRelaxation.enumCoordinateID.Relaxation5, ConstraintRelaxation.enumCoordinateAxis.AngularX, False)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation6
                    Return New ConstraintRelaxationBullet(doParent, "Y Axis Rotation", "Sets the relaxation for the Y rotation axis.", ConstraintRelaxation.enumCoordinateID.Relaxation6, ConstraintRelaxation.enumCoordinateAxis.AngularY, False)
            End Select

            Return Nothing
        End Function

        Protected Overridable Function CreateRPROJointRelaxation(ByVal eCoordinate As ConstraintRelaxation.enumCoordinateID, ByVal doParent As Framework.DataObject) As ConstraintRelaxation
            Select Case (eCoordinate)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation1
                    Return New ConstraintRelaxationBullet(doParent, "X Axis Displacement", "Sets the relaxation for the X displacement axis.", ConstraintRelaxation.enumCoordinateID.Relaxation1, ConstraintRelaxation.enumCoordinateAxis.LinearX, False)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation2
                    Return New ConstraintRelaxationBullet(doParent, "Y Axis Displacement", "Sets the relaxation for the Y displacement axis.", ConstraintRelaxation.enumCoordinateID.Relaxation2, ConstraintRelaxation.enumCoordinateAxis.LinearY, False)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation3
                    Return New ConstraintRelaxationBullet(doParent, "Z Axis Displacement", "Sets the relaxation for the Z displacement axis.", ConstraintRelaxation.enumCoordinateID.Relaxation3, ConstraintRelaxation.enumCoordinateAxis.LinearZ, False)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation4
                    Return New ConstraintRelaxationBullet(doParent, "X Axis Rotation", "Sets the relaxation for the X rotation axis.", ConstraintRelaxation.enumCoordinateID.Relaxation4, ConstraintRelaxation.enumCoordinateAxis.AngularX, False)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation5
                    Return New ConstraintRelaxationBullet(doParent, "Y Axis Rotation", "Sets the relaxation for the Y rotation axis.", ConstraintRelaxation.enumCoordinateID.Relaxation5, ConstraintRelaxation.enumCoordinateAxis.AngularY, False)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation6
                    Return New ConstraintRelaxationBullet(doParent, "Z Axis Rotation", "Sets the relaxation for the Z rotation axis.", ConstraintRelaxation.enumCoordinateID.Relaxation6, ConstraintRelaxation.enumCoordinateAxis.AngularZ, False)
            End Select

            Return Nothing
        End Function

        Public Overrides Function CreateConstraintLimit(ByVal strType As String, ByVal doParent As Framework.DataObject) As ConstraintLimit
            Dim strType1 As String = strType.Trim.ToUpper
            If strType1 = "HINGE" Then
                Return New Joints.HingeLimitBullet(doParent)
            ElseIf strType1 = "PRISMATIC" Then
                Return New Joints.PrismaticLimitBullet(doParent)
            Else
                Return Nothing
            End If
        End Function

#End Region

    End Class

End Namespace

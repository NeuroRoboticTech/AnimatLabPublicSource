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

    Public Class VortexPhysicsEngine
        Inherits PhysicsEngine

#Region " Attributes "

#End Region

#Region " Properties "

        Public Overrides Property Name As String
            Get
                Return "Vortex"
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
                Return False
            End Get
        End Property

        Public Overrides ReadOnly Property AllowDynamicTriangleMesh() As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overrides ReadOnly Property AllowPhysicsSubsteps() As Boolean
            Get
                Return False
            End Get
        End Property

        Public Overrides ReadOnly Property ShowSeparateConstraintLimits() As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overrides ReadOnly Property UseHydrodynamicsMagnus() As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overrides ReadOnly Property ProvidesJointForceFeedback() As Boolean
            Get
                Return False
            End Get
        End Property

        Public Overrides ReadOnly Property GenerateMotorAssist() As Boolean
            Get
                Return False
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            'Setup the consstraint relaxation map.
            m_aryConstraintRelaxations.Add("BallSocket_LinearX", True)
            m_aryConstraintRelaxations.Add("BallSocket_LinearY", True)
            m_aryConstraintRelaxations.Add("BallSocket_LinearZ", True)
            m_aryConstraintRelaxations.Add("BallSocket_AngularX", False)
            m_aryConstraintRelaxations.Add("BallSocket_AngularY", False)
            m_aryConstraintRelaxations.Add("BallSocket_AngularZ", False)

            m_aryConstraintRelaxations.Add("Distance_LinearX", True)
            m_aryConstraintRelaxations.Add("Distance_LinearY", False)
            m_aryConstraintRelaxations.Add("Distance_LinearZ", False)
            m_aryConstraintRelaxations.Add("Distance_AngularX", False)
            m_aryConstraintRelaxations.Add("Distance_AngularY", False)
            m_aryConstraintRelaxations.Add("Distance_AngularZ", False)

            m_aryConstraintRelaxations.Add("FreeJoint_LinearX", False)
            m_aryConstraintRelaxations.Add("FreeJoint_LinearY", False)
            m_aryConstraintRelaxations.Add("FreeJoint_LinearZ", False)
            m_aryConstraintRelaxations.Add("FreeJoint_AngularX", False)
            m_aryConstraintRelaxations.Add("FreeJoint_AngularY", False)
            m_aryConstraintRelaxations.Add("FreeJoint_AngularZ", False)

            m_aryConstraintRelaxations.Add("Hinge_LinearX", True)
            m_aryConstraintRelaxations.Add("Hinge_LinearY", True)
            m_aryConstraintRelaxations.Add("Hinge_LinearZ", True)
            m_aryConstraintRelaxations.Add("Hinge_AngularX", True)
            m_aryConstraintRelaxations.Add("Hinge_AngularY", True)
            m_aryConstraintRelaxations.Add("Hinge_AngularZ", False)

            m_aryConstraintRelaxations.Add("Prismatic_LinearX", False)
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

            m_aryConstraintRelaxations.Add("Static_LinearX", False)
            m_aryConstraintRelaxations.Add("Static_LinearY", False)
            m_aryConstraintRelaxations.Add("Static_LinearZ", False)
            m_aryConstraintRelaxations.Add("Static_AngularX", False)
            m_aryConstraintRelaxations.Add("Static_AngularY", False)
            m_aryConstraintRelaxations.Add("Static_AngularZ", False)

            m_aryConstraintRelaxations.Add("Universal_LinearX", True)
            m_aryConstraintRelaxations.Add("Universal_LinearY", True)
            m_aryConstraintRelaxations.Add("Universal_LinearZ", True)
            m_aryConstraintRelaxations.Add("Universal_AngularX", True)
            m_aryConstraintRelaxations.Add("Universal_AngularY", False)
            m_aryConstraintRelaxations.Add("Universal_AngularZ", False)

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim doItem As New VortexPhysicsEngine(doParent)
            Return doItem
        End Function

        Public Overrides Function CreateJointRelaxation(ByVal strType As String, ByVal eCoordinate As ConstraintRelaxation.enumCoordinateID, ByVal doParent As Framework.DataObject) As ConstraintRelaxation
            Select Case (strType.Trim.ToUpper)
                Case "BALLSOCKET"
                    Return CreateBallSocketJointRelaxation(eCoordinate, doParent)
                Case "DISTANCE"
                    Return CreateDistanceJointRelaxation(eCoordinate, doParent)
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
                    Return CreateUniversalJointRelaxation(eCoordinate, doParent)
            End Select
        End Function

        Protected Overridable Function CreateBallSocketJointRelaxation(ByVal eCoordinate As ConstraintRelaxation.enumCoordinateID, ByVal doParent As Framework.DataObject) As ConstraintRelaxation
            Select Case (eCoordinate)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation1
                    Return New ConstraintRelaxationVortex(doParent, "X Axis Displacement", "Sets the relaxation for the X displacement axis.", ConstraintRelaxation.enumCoordinateID.Relaxation1, ConstraintRelaxation.enumCoordinateAxis.LinearX)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation2
                    Return New ConstraintRelaxationVortex(doParent, "Y Axis Displacement", "Sets the relaxation for the Y displacement axis.", ConstraintRelaxation.enumCoordinateID.Relaxation2, ConstraintRelaxation.enumCoordinateAxis.LinearY)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation3
                    Return New ConstraintRelaxationVortex(doParent, "Z Axis Displacement", "Sets the relaxation for the Z displacement axis.", ConstraintRelaxation.enumCoordinateID.Relaxation3, ConstraintRelaxation.enumCoordinateAxis.LinearZ)
            End Select

            Return Nothing
        End Function

        Protected Overridable Function CreateDistanceJointRelaxation(ByVal eCoordinate As ConstraintRelaxation.enumCoordinateID, ByVal doParent As Framework.DataObject) As ConstraintRelaxation
            Select Case (eCoordinate)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation1
                    Return New ConstraintRelaxationVortex(doParent, "Distance Displacement", "Sets the relaxation for the distance constraint.", ConstraintRelaxation.enumCoordinateID.Relaxation1, ConstraintRelaxation.enumCoordinateAxis.LinearX)
            End Select

            Return Nothing
        End Function

        Protected Overridable Function CreateHingeJointRelaxation(ByVal eCoordinate As ConstraintRelaxation.enumCoordinateID, ByVal doParent As Framework.DataObject) As ConstraintRelaxation
            Select Case (eCoordinate)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation1
                    Return New ConstraintRelaxationVortex(doParent, "X Axis Displacement", "Sets the relaxation for the X displacement axis.", ConstraintRelaxation.enumCoordinateID.Relaxation1, ConstraintRelaxation.enumCoordinateAxis.LinearX)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation2
                    Return New ConstraintRelaxationVortex(doParent, "Y Axis Displacement", "Sets the relaxation for the Y displacement axis.", ConstraintRelaxation.enumCoordinateID.Relaxation2, ConstraintRelaxation.enumCoordinateAxis.LinearY)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation3
                    Return New ConstraintRelaxationVortex(doParent, "Z Axis Displacement", "Sets the relaxation for the Z displacement axis.", ConstraintRelaxation.enumCoordinateID.Relaxation3, ConstraintRelaxation.enumCoordinateAxis.LinearZ)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation4
                    Return New ConstraintRelaxationVortex(doParent, "X Axis Rotation", "Sets the relaxation for the X rotation axis.", ConstraintRelaxation.enumCoordinateID.Relaxation4, ConstraintRelaxation.enumCoordinateAxis.AngularX)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation5
                    Return New ConstraintRelaxationVortex(doParent, "Y Axis Rotation", "Sets the relaxation for the Y rotation axis.", ConstraintRelaxation.enumCoordinateID.Relaxation5, ConstraintRelaxation.enumCoordinateAxis.AngularY)
            End Select

            Return Nothing
        End Function

        Protected Overridable Function CreatePrismaticJointRelaxation(ByVal eCoordinate As ConstraintRelaxation.enumCoordinateID, ByVal doParent As Framework.DataObject) As ConstraintRelaxation
            Select Case (eCoordinate)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation1
                    Return New ConstraintRelaxationVortex(doParent, "Z Axis Displacement", "Sets the relaxation for the Z displacement axis.", ConstraintRelaxation.enumCoordinateID.Relaxation1, ConstraintRelaxation.enumCoordinateAxis.LinearZ)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation2
                    Return New ConstraintRelaxationVortex(doParent, "Y Axis Displacement", "Sets the relaxation for the Y displacement axis.", ConstraintRelaxation.enumCoordinateID.Relaxation2, ConstraintRelaxation.enumCoordinateAxis.LinearY)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation3
                    Return New ConstraintRelaxationVortex(doParent, "X Axis Rotation", "Sets the relaxation for the X rotation axis.", ConstraintRelaxation.enumCoordinateID.Relaxation3, ConstraintRelaxation.enumCoordinateAxis.AngularX)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation4
                    Return New ConstraintRelaxationVortex(doParent, "Z Axis Rotation", "Sets the relaxation for the Z rotation axis.", ConstraintRelaxation.enumCoordinateID.Relaxation4, ConstraintRelaxation.enumCoordinateAxis.AngularZ)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation5
                    Return New ConstraintRelaxationVortex(doParent, "Y Axis Rotation", "Sets the relaxation for the Y rotation axis.", ConstraintRelaxation.enumCoordinateID.Relaxation5, ConstraintRelaxation.enumCoordinateAxis.AngularY)
            End Select

            Return Nothing
        End Function

        Protected Overridable Function CreateRPROJointRelaxation(ByVal eCoordinate As ConstraintRelaxation.enumCoordinateID, ByVal doParent As Framework.DataObject) As ConstraintRelaxation
            Select Case (eCoordinate)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation1
                    Return New ConstraintRelaxationVortex(doParent, "X Axis Displacement", "Sets the relaxation for the X displacement axis.", ConstraintRelaxation.enumCoordinateID.Relaxation1, ConstraintRelaxation.enumCoordinateAxis.LinearX)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation2
                    Return New ConstraintRelaxationVortex(doParent, "Y Axis Displacement", "Sets the relaxation for the Y displacement axis.", ConstraintRelaxation.enumCoordinateID.Relaxation2, ConstraintRelaxation.enumCoordinateAxis.LinearY)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation3
                    Return New ConstraintRelaxationVortex(doParent, "Z Axis Displacement", "Sets the relaxation for the Z displacement axis.", ConstraintRelaxation.enumCoordinateID.Relaxation3, ConstraintRelaxation.enumCoordinateAxis.LinearZ)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation4
                    Return New ConstraintRelaxationVortex(doParent, "X Axis Rotation", "Sets the relaxation for the X rotation axis.", ConstraintRelaxation.enumCoordinateID.Relaxation4, ConstraintRelaxation.enumCoordinateAxis.AngularX)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation5
                    Return New ConstraintRelaxationVortex(doParent, "Y Axis Rotation", "Sets the relaxation for the Y rotation axis.", ConstraintRelaxation.enumCoordinateID.Relaxation5, ConstraintRelaxation.enumCoordinateAxis.AngularY)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation6
                    Return New ConstraintRelaxationVortex(doParent, "Z Axis Rotation", "Sets the relaxation for the Z rotation axis.", ConstraintRelaxation.enumCoordinateID.Relaxation6, ConstraintRelaxation.enumCoordinateAxis.AngularZ)
            End Select

            Return Nothing
        End Function

        Protected Overridable Function CreateUniversalJointRelaxation(ByVal eCoordinate As ConstraintRelaxation.enumCoordinateID, ByVal doParent As Framework.DataObject) As ConstraintRelaxation
            Select Case (eCoordinate)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation1
                    Return New ConstraintRelaxationVortex(doParent, "X Axis Displacement", "Sets the relaxation for the X displacement axis.", ConstraintRelaxation.enumCoordinateID.Relaxation1, ConstraintRelaxation.enumCoordinateAxis.LinearX)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation2
                    Return New ConstraintRelaxationVortex(doParent, "Y Axis Displacement", "Sets the relaxation for the Y displacement axis.", ConstraintRelaxation.enumCoordinateID.Relaxation2, ConstraintRelaxation.enumCoordinateAxis.LinearY)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation3
                    Return New ConstraintRelaxationVortex(doParent, "Z Axis Displacement", "Sets the relaxation for the Z displacement axis.", ConstraintRelaxation.enumCoordinateID.Relaxation3, ConstraintRelaxation.enumCoordinateAxis.LinearZ)
                Case ConstraintRelaxation.enumCoordinateID.Relaxation4
                    Return New ConstraintRelaxationVortex(doParent, "X Axis Rotation", "Sets the relaxation for the X rotation axis.", ConstraintRelaxation.enumCoordinateID.Relaxation4, ConstraintRelaxation.enumCoordinateAxis.AngularX)
            End Select

            Return Nothing
        End Function

        Public Overrides Function CreateConstraintLimit(ByVal strType As String, ByVal doParent As Framework.DataObject) As ConstraintLimit
            Return New ConstraintLimit(doParent)
        End Function

#End Region

    End Class

End Namespace

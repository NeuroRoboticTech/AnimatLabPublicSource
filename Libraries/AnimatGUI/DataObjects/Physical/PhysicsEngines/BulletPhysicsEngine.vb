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

        Public Overrides ReadOnly Property AllowConstraintRelaxation() As Boolean
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

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim doItem As New BulletPhysicsEngine(doParent)
            Return doItem
        End Function

#End Region

    End Class

End Namespace

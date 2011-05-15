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

    Public MustInherit Class MuscleBase
        Inherits Physical.RigidBody

#Region " Attributes "
        '      		protected VortexAnimatTools.Collections.MuscleAttachments m_aryAttachmentPoints;
        'protected ArrayList m_aryAttachmentPointIDs;

        'protected AnimatTools.Framework.ScaledNumber m_snMaxTension;
        'protected AnimatTools.Framework.ScaledNumber m_snMuscleLength;

        'protected AnimatTools.DataObjects.Gains.MuscleGains.StimulusTension m_StimTension;
        'protected AnimatTools.DataObjects.Gains.MuscleGains.LengthTension m_LengthTension;
        'protected AnimatTools.DataObjects.Gains.MuscleGains.VelocityTension m_VelocityTension;



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

        Public Overrides Property LocalPosition() As Framework.ScaledVector3
            Get
                Return MyBase.LocalPosition
            End Get
            Set(ByVal value As Framework.ScaledVector3)
                'LocalPosition is never changed on a muscle. It is always 0,0,0
            End Set
        End Property

        Public Overrides Property WorldPosition() As Framework.ScaledVector3
            Get
                Return MyBase.WorldPosition
            End Get
            Set(ByVal value As Framework.ScaledVector3)
                'WorldPosition is never changed on a muscle. It is always 0,0,0
            End Set
        End Property

        Public Overrides Property Rotation() As Framework.ScaledVector3
            Get
                Return MyBase.Rotation
            End Get
            Set(ByVal value As Framework.ScaledVector3)
                'Rotation is never changed on an attachment. It is always 0,0,0
            End Set
        End Property

        Public Overrides ReadOnly Property CanBeRootBody() As Boolean
            Get
                Return False
            End Get
        End Property

        Public Overrides ReadOnly Property UsesAJoint() As Boolean
            Get
                Return False
            End Get
        End Property

        Public Overrides ReadOnly Property HasDynamics() As Boolean
            Get
                Return False
            End Get
        End Property

        Public Overrides ReadOnly Property DefaultAddGraphics() As Boolean
            Get
                Return False
            End Get
        End Property

#End Region

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
            m_bIsCollisionObject = False
            m_clDiffuse = Color.Blue
        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)


        End Sub

        Public Overloads Overrides Sub LoadData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByRef oXml As Interfaces.StdXml)
            MyBase.LoadData(doStructure, oXml)

            oXml.IntoElem() 'Into RigidBody Element

            oXml.OutOfElem() 'Outof RigidBody Element

        End Sub

        Public Overloads Overrides Sub SaveData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByRef oXml As Interfaces.StdXml)
            MyBase.SaveData(doStructure, oXml)

            oXml.IntoElem() 'Into Child Elemement

            oXml.OutOfElem() 'Outof BodyPart Element

        End Sub

        Public Overrides Sub SaveSimulationXml(ByRef oXml As Interfaces.StdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

            oXml.IntoElem()


            oXml.OutOfElem()

        End Sub

    End Class


End Namespace

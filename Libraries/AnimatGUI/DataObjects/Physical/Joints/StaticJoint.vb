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

    Public Class StaticJoint
        Inherits Physical.Joint

#Region " Attributes "

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.Static_Treeview.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonImageName() As String
            Get
                Return "AnimatGUI.Sphere_SelectType.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property Type() As String
            Get
                Return "Static"
            End Get
        End Property

        Public Overrides ReadOnly Property PartType() As System.Type
            Get
                Return GetType(AnimatGUI.DataObjects.Physical.Joints.StaticJoint)
            End Get
        End Property

        Public Overrides ReadOnly Property UsesRadians() As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overrides ReadOnly Property CanBeCharted() As Boolean
            Get
                Return False
            End Get
        End Property

        Public Overrides ReadOnly Property AllowAddChildBody() As Boolean
            Get
                Return False
            End Get
        End Property

#End Region

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
            m_strDescription = ""

        End Sub

        Public Overrides Sub SetDefaultSizes()
            MyBase.SetDefaultSizes()
            m_snSize.ActualValue = 0.05 * Util.Environment.DistanceUnitValue
        End Sub

        Public Overrides Function Clone(ByVal doParent As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject) As Framework.DataObject
            Dim oNewNode As New Joints.StaticJoint(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Public Overrides Sub VerifyCanBePasted()

            If Not m_doParent Is Nothing AndAlso Util.IsTypeOf(m_doParent.GetType, GetType(RigidBody), False) Then
                Dim doParent As RigidBody = DirectCast(m_doParent, RigidBody)
                If doParent.ChildBodies.Count > 0 Then
                    Throw New System.Exception("You cannot paste a rigid body using a static joint when the body has children.")
                End If
            End If
        End Sub

        ''' \brief  Initializes the simulation references.
        ''' 		
        ''' \details I am overriding this method and doing nothing because there is no actual static joint class in the simulation code.
        ''' 		 A static joint is just one where the JointToParent is NULL. So we should never attempt to set the simulation reference
        ''' 		 for a static joint type because it does not exist.
        '''
        ''' \author dcofer
        ''' \date   4/16/2011
        Public Overrides Sub InitializeSimulationReferences()
        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Name", m_strID.GetType(), "Name", _
                                        "Part Properties", "The name of this item.", m_strName))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("ID", Me.ID.GetType(), "ID", _
                                        "Part Properties", "ID", Me.ID, True))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Description", m_strDescription.GetType(), "Description", _
                                        "Part Properties", "Sets the description for this body part.", m_strDescription, _
                                        GetType(AnimatGUI.TypeHelpers.MultiLineStringTypeEditor)))

        End Sub

        ''' \brief  Saves the simulation xml.
        '''
        ''' \details I am overriding this method and doing nothing because there is no actual static joint class in the simulation code.
        ''' 		 A static joint is just one where the JointToParent is NULL. So by saving nothing here we are in fact adding the static
        ''' 		 joint to the simulation.
        ''' 		 
        ''' \author dcofer
        ''' \date   4/16/2011
        '''
        ''' \param [in,out] oXml    The xml to save to.
        ''' \param  nmParentControl (optional) [in,out] The parent control.
        ''' \param  strName         (optional) name of the element.
        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")
        End Sub

    End Class


End Namespace

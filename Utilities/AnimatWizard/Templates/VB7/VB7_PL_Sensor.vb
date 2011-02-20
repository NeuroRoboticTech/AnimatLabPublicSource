Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports Crownwood.Magic.Controls
Imports AnimatTools.Framework
Imports Microsoft.DirectX
Imports Microsoft.DirectX.Direct3D
Imports VortexAnimatTools.DataObjects
Imports VortexAnimatTools.DataObjects.Physical.PropertyHelpers

Namespace DataObjects.Physical.RigidBodies

    Public Class [*BODY_PART_NAME*]
        Inherits VortexAnimatTools.DataObjects.Physical.RigidBodies.Sensor

#Region " Attributes "

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Overrides ReadOnly Property ImageName() As String
            Get
                Return "[*PROJECT_NAME*]Tools.Default_TreeView.gif"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property ButtonImageName() As String
            Get
                Return "[*PROJECT_NAME*]Tools.Default_Button.gif"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property Type() As String
            Get
                Return "[*BODY_PART_NAME*]"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property BodyPartName() As String
            Get
                Return "[*BODY_PART_DISPLAY_NAME*]"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property PartType() As System.Type
            Get
                Return GetType([*PROJECT_NAME*]Tools.DataObjects.Physical.RigidBodies.[*BODY_PART_NAME*])
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property ModuleName() As String
            Get
                If Util.Simulation.UseReleaseLibraries Then
                    Return "[*PROJECT_NAME*]_vc7.dll"
                Else
                    Return "[*PROJECT_NAME*]_vc7D.dll"
                End If
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatTools.Framework.DataObject)
            MyBase.New(doParent)

            Try
                'Add your init code here

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Function CreateNewBodyPart(ByVal doParent As AnimatTools.Framework.DataObject) As AnimatTools.DataObjects.Physical.BodyPart
            Return New Physical.RigidBodies.[*BODY_PART_NAME*](doParent)
        End Function

        Public Overrides Function Clone(ByVal doParent As AnimatTools.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatTools.Framework.DataObject) As AnimatTools.Framework.DataObject
            Dim oNewNode As New Physical.RigidBodies.[*BODY_PART_NAME*](doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatTools.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatTools.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim bnOrig As Physical.RigidBodies.[*BODY_PART_NAME*] = DirectCast(doOriginal, Physical.RigidBodies.[*BODY_PART_NAME*])

            'Add any extra attributes to be copied here.

        End Sub

#Region " DataObject Methods "

        Protected Overrides Sub BuildProperties()
            MyBase.BuildProperties()

        End Sub

        Public Overloads Overrides Sub LoadData(ByRef dsSim As AnimatTools.DataObjects.Simulation, ByRef doStructure As AnimatTools.DataObjects.Physical.PhysicalStructure, ByRef oXml As AnimatTools.Interfaces.StdXml)
            MyBase.LoadData(dsSim, doStructure, oXml)

            oXml.IntoElem() 'Into RigidBody Element

            oXml.OutOfElem() 'Outof RigidBody Element	
        End Sub

        Public Overloads Overrides Sub SaveData(ByRef dsSim As AnimatTools.DataObjects.Simulation, ByRef doStructure As AnimatTools.DataObjects.Physical.PhysicalStructure, ByRef oXml As AnimatTools.Interfaces.StdXml)
            MyBase.SaveData(dsSim, doStructure, oXml)

            oXml.IntoElem()

            oXml.OutOfElem()
        End Sub

#End Region

#End Region

    End Class

End Namespace
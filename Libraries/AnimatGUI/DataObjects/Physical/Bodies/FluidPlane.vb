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

Namespace DataObjects.Physical.Bodies

    Public Class FluidPlane
        Inherits Plane

#Region " Attributes "

        Protected m_svVelocity As ScaledVector3

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.FluidPlane_Treeview.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonImageName() As String
            Get
                Return "AnimatGUI.FluidPlane_SelectType.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property Type() As String
            Get
                Return "FluidPlane"
            End Get
        End Property

        Public Overrides ReadOnly Property PartType() As System.Type
            Get
                Return GetType(AnimatGUI.DataObjects.Physical.Bodies.FluidPlane)
            End Get
        End Property

        Public Overridable Property Velocity() As Framework.ScaledVector3
            Get
                Return m_svVelocity
            End Get
            Set(ByVal value As Framework.ScaledVector3)
                Me.SetSimData("Velocity", value.GetSimulationXml("Velocity"), True)
                m_svVelocity.CopyData(value)
            End Set
        End Property

        Public Overrides Property Density() As ScaledNumber
            Get
                Return m_snDensity
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue <= 0 Then
                    Throw New System.Exception("The density can not be less than or equal to zero.")
                End If

                'The bullet physics engine uses mass as its key value to define a rigid body, but vortex uses density. So we need to alter
                'what we are using as a key param based on application settings.
                Me.SetSimData("Density", Value.ToString, True)
                m_snDensity.CopyData(Value)
                UpdateMassVolumeDensity()

            End Set
        End Property

#End Region

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
            m_strDescription = ""

            'Start out white, and ambiently lit.
            m_clAmbient = Color.FromArgb(255, 0, 0, 255)
            m_clDiffuse = Color.FromArgb(255, 0, 0, 255)
            m_clSpecular = Color.FromArgb(255, 64, 64, 64)
            m_fltShininess = 64

            m_svVelocity = New ScaledVector3(Me, "Velocity", "Fluid velocity.", "Meters/Second", "m/s")

            AddHandler m_svVelocity.ValueChanged, AddressOf Me.OnVelocityValueChanged

            If Not Util.Environment Is Nothing Then
                AddHandler Util.Environment.AfterPropertyChanged, AddressOf Me.OnEnvironmentPropChanged
            End If
        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()
            If Not m_svVelocity Is Nothing Then m_svVelocity.ClearIsDirty()
        End Sub

        Public Overrides Function Clone(ByVal doParent As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject) As Framework.DataObject
            Dim oNewNode As New Bodies.FluidPlane(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doOrig As Bodies.FluidPlane = DirectCast(doOriginal, Bodies.FluidPlane)
            m_svVelocity = DirectCast(doOrig.m_svVelocity.Clone(Me, bCutData, doRoot), Framework.ScaledVector3)
        End Sub

        Public Overrides Sub SetDefaultSizes()
            MyBase.SetDefaultSizes()
            Dim fltVal As Single = 100 * Util.Environment.DistanceUnitValue
        End Sub

        Public Overrides Sub BeforeAddBody()
            MyBase.BeforeAddBody()

            If Me.IsRoot Then
                Me.Rotation.X.ActualValue = -90
            End If
        End Sub

        Public Overrides Sub AfterAddBody()
            MyBase.AfterAddBody()

            Util.Environment.SimulateHydrodynamics = True
        End Sub

        Public Overrides Sub SetupPartTypesExclusions()
            'A fluid plane can only be added to a terrain or plane type.
            For Each bpBody As BodyPart In Util.Application.BodyPartTypes
                If Not (TypeOf bpBody Is Terrain OrElse TypeOf bpBody Is Plane) Then
                    Util.Application.AddPartTypeExclusion(bpBody.GetType, Me.GetType)
                End If

                'Cant add anything to a fluid plane.
                Util.Application.AddPartTypeExclusion(Me.GetType, bpBody.GetType)
            Next
        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            If propTable.Properties.Contains("Mass") Then propTable.Properties.Remove("Mass")
            If propTable.Properties.Contains("Volume") Then propTable.Properties.Remove("Volume")

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = m_svVelocity.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Velocity", pbNumberBag.GetType(), "Velocity", _
                                        "Coordinates", "Sets the velocity of the fluid medium.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledVector3.ScaledVector3PropBagConverter), Not AllowGuiCoordinateChange()))

            'Add density back in.
            pbNumberBag = m_snDensity.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Density", pbNumberBag.GetType(), "Density", _
                                        "Part Properties", "Sets the density of this body part.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

        End Sub

        Protected Overrides Sub UpdateMassVolumeDensity()

            If Not Me.SimInterface Is Nothing Then
                If Util.Application.Physics.UseMassForRigidBodyDefinitions Then
                    'Do not reset density for fluid planes here.
                    'm_snDensity.ActualValue = Me.SimInterface.GetDataValueImmediate("Density")
                    m_snVolume.ActualValue = Me.SimInterface.GetDataValueImmediate("Volume")
                Else
                    m_snMass.ActualValue = Me.SimInterface.GetDataValueImmediate("Mass")
                    m_snVolume.ActualValue = Me.SimInterface.GetDataValueImmediate("Volume")
                End If
            End If

            If Not Util.ProjectProperties Is Nothing Then
                Util.ProjectProperties.RefreshProperties()
            End If
        End Sub

        Public Overloads Overrides Sub LoadData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.LoadData(doStructure, oXml)

            oXml.IntoElem() 'Into RigidBody Element

            m_svVelocity.LoadData(oXml, "Velocity")

            oXml.OutOfElem() 'Outof RigidBody Element

        End Sub

        Public Overloads Overrides Sub SaveData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.SaveData(doStructure, oXml)

            oXml.IntoElem() 'Into Child Elemement

            m_svVelocity.SaveData(oXml, "Velocity")

            oXml.OutOfElem() 'Outof BodyPart Element

        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

            oXml.IntoElem()

            m_svVelocity.SaveSimulationXml(oXml, Me, "Velocity")

            oXml.OutOfElem()

        End Sub

#Region " Events "

        Protected Overridable Sub OnVelocityValueChanged(ByVal iIdx As Integer, ByVal snParam As ScaledNumber)
            Try
                Me.SetSimData("Velocity", m_svVelocity.GetSimulationXml("Velocity"), True)
                Util.ProjectProperties.RefreshProperties()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Public Overridable Sub OnEnvironmentPropChanged(ByVal doObject As AnimatGUI.Framework.DataObject, ByVal propInfo As System.Reflection.PropertyInfo)
            Try
                If propInfo.Name = "Gravity" Then
                    Me.SetSimData("Gravity", Util.Environment.Gravity.ToString, True)
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

    End Class


End Namespace

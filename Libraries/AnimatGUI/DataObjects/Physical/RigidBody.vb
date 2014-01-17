Imports System
Imports System.Drawing
Imports System.Collections
Imports System.Threading
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports AnimatGuiCtrls.Controls
Imports AnimatGUI.Framework

Namespace DataObjects.Physical

    Public MustInherit Class RigidBody
        Inherits Physical.BodyPart

#Region " Delegates "

        Delegate Function AddChildBodyDelegate(ByVal vPos As Framework.Vec3d, ByVal vNorm As Framework.Vec3d, ByVal bDoNotOrient As Boolean) As Boolean

#End Region

#Region " Attributes "

        Protected m_JointToParent As DataObjects.Physical.Joint
        Protected m_aryChildBodies As New Collections.SortedRigidBodies(Me)

        Protected m_bFreeze As Boolean = False
        Protected m_bContactSensor As Boolean = False
        Protected m_bIsCollisionObject As Boolean = False
        Protected m_snDensity As ScaledNumber
        Protected m_snMass As ScaledNumber
        Protected m_snVolume As ScaledNumber

        Protected m_svCOM As ScaledVector3

        Protected m_svBuoyancyCenter As ScaledVector3
        Protected m_fltBuoyancyScale As Single = 1
        Protected m_svLinearDrag As ScaledVector3
        Protected m_svAngularDrag As ScaledVector3
        Protected m_snMaxHydroForce As ScaledNumber
        Protected m_snMaxHydroTorque As ScaledNumber
        Protected m_fltMagnus As Single = 0
        Protected m_bEnableFluids As Boolean = False

        Protected m_tnOdorSources As Crownwood.DotNetMagic.Controls.Node

        Protected m_aryOdorSources As New Collections.SortedOdors(Me)

        Protected m_bFoodSource As Boolean = False
        Protected m_snFoodQuantity As ScaledNumber
        Protected m_snFoodReplenishRate As ScaledNumber
        Protected m_snFoodEnergyContent As ScaledNumber
        Protected m_snMaxFoodQuantity As ScaledNumber

        Protected m_doReceptiveFieldSensor As ContactSensor
        Protected m_vSelectedVertex As Vec3d    'The selected vertex for this part.

        Protected m_bIsRoot As Boolean = False

        Protected m_thMaterialType As TypeHelpers.LinkedMaterialType
        Protected m_strMaterialTypeID As String = ""

#End Region

#Region " Properties "

        Public Overridable Property JointToParent() As DataObjects.Physical.Joint
            Get
                Return m_JointToParent
            End Get
            Set(ByVal Value As DataObjects.Physical.Joint)
                m_JointToParent = Value
            End Set
        End Property

        Public Overridable ReadOnly Property ChildBodies() As Collections.SortedRigidBodies
            Get
                Return m_aryChildBodies
            End Get
        End Property

        Public Overrides ReadOnly Property BodyPartType() As String
            Get
                Return "RigidBody"
            End Get
        End Property

        Public Overridable Property Freeze() As Boolean
            Get
                Return m_bFreeze
            End Get
            Set(ByVal Value As Boolean)
                Me.SetSimData("Freeze", Value.ToString, True)
                m_bFreeze = Value
            End Set
        End Property

        Public Overridable Property IsContactSensor() As Boolean
            Get
                Return m_bContactSensor
            End Get
            Set(ByVal Value As Boolean)
                m_bContactSensor = Value
            End Set
        End Property

        Public Overridable Property IsCollisionObject() As Boolean
            Get
                Return m_bIsCollisionObject
            End Get
            Set(ByVal value As Boolean)
                m_bIsCollisionObject = value
                SetupInitialTransparencies()
            End Set
        End Property

        ''' \brief  Gets the default add graphics setting.
        ''' 		
        ''' \details Sometimes you only want to add a collision object without also adding a graphics object. 
        ''' 		 An example of this is the plane. This property lets us know about that.
        '''
        ''' \value  .
        Public Overridable ReadOnly Property DefaultAddGraphics() As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overrides ReadOnly Property DefaultVisualSelectionMode() As Simulation.enumVisualSelectionMode
            Get
                If Me.IsCollisionObject Then
                    Return Simulation.enumVisualSelectionMode.SelectCollisions
                Else
                    Return Simulation.enumVisualSelectionMode.SelectGraphics
                End If
            End Get
        End Property

        Public Overridable Property COM() As Framework.ScaledVector3
            Get
                Return m_svCOM
            End Get
            Set(ByVal value As Framework.ScaledVector3)
                Me.SetSimData("COM", value.GetSimulationXml("COM"), True)
                m_svCOM.CopyData(value)
            End Set
        End Property

        Public Overridable Property Density() As ScaledNumber
            Get
                Return m_snDensity
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue <= 0 Then
                    Throw New System.Exception("The density can not be less than or equal to zero.")
                End If

                'The bullet physics engine uses mass as its key value to define a rigid body, but vortex uses density. So we need to alter
                'what we are using as a key param based on application settings.
                If Not Util.Application.Physics.UseMassForRigidBodyDefinitions Then
                    Me.SetSimData("Density", Value.ToString, True)
                    m_snDensity.CopyData(Value)
                    UpdateMassVolumeDensity()
                Else
                    m_snVolume.ActualValue = Me.SimInterface.GetDataValueImmediate("Volume")

                    If m_snVolume.ActualValue > 0 Then
                        Dim fltMass As Double = CSng(Value.ActualValue * m_snVolume.ActualValue)

                        'Value above uses the display units value, while volume is always in m^3. We need to do a conversion to get them to match correctly.
                        fltMass = fltMass / (Util.Environment.DisplayDistanceUnitValue ^ 3)
                        Dim snNewVal As New ScaledNumber(Me, "Mass", 1, ScaledNumber.enumNumericScale.Kilo, "g", "g")
                        snNewVal.ActualValue = fltMass

                        Me.Mass = snNewVal
                    Else
                        Throw New System.Exception("The volume has not been defined yet.")
                    End If
                End If

            End Set
        End Property

        Public Overridable Property Mass() As ScaledNumber
            Get
                Return m_snMass
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue <= 0 Then
                    Throw New System.Exception("The mass can not be less than or equal to zero.")
                End If

                'The bullet physics engine uses mass as its key value to define a rigid body, but vortex uses density. So we need to alter
                'what we are using as a key param based on application settings.
                If Util.Application.Physics.UseMassForRigidBodyDefinitions Then
                    Me.SetSimData("Mass", Value.ToString, True)
                    m_snMass.CopyData(Value)
                    UpdateMassVolumeDensity()
                Else
                    m_snVolume.ActualValue = Me.SimInterface.GetDataValueImmediate("Volume")

                    If m_snVolume.ActualValue > 0 Then
                        Dim fltDensityGramsPerMeterCube As Single = CSng(Value.ActualValue / m_snVolume.ActualValue)
                        Dim fltDensityGramPerDistUnitCube As Single = CSng(fltDensityGramsPerMeterCube * Math.Pow(CDbl(Util.Environment.DisplayDistanceUnitValue), 3.0))

                        Dim snNewVal As New ScaledNumber(Me, "Density", 1, ScaledNumber.enumNumericScale.Kilo, "g/m^3", "g/m^3")
                        snNewVal.ActualValue = fltDensityGramPerDistUnitCube
                        Me.Density = snNewVal
                    Else
                        Throw New System.Exception("The volume has not been defined yet.")
                    End If
                End If

            End Set
        End Property

        Public Overridable ReadOnly Property Volume() As ScaledNumber
            Get
                Return m_snVolume
            End Get
        End Property

        Public Overridable Property BuoyancyCenter() As Framework.ScaledVector3
            Get
                Return m_svBuoyancyCenter
            End Get
            Set(ByVal value As Framework.ScaledVector3)
                Me.SetSimData("BuoyancyCenter", value.GetSimulationXml("BuoyancyCenter"), True)
                m_svBuoyancyCenter.CopyData(value)
            End Set
        End Property

        Public Overridable Property BuoyancyScale() As Single
            Get
                Return m_fltBuoyancyScale
            End Get
            Set(ByVal Value As Single)
                If Value < 0 Then
                    Throw New System.Exception("The buoyancy scale can not be less than to zero.")
                End If
                SetSimData("BuoyancyScale", Value.ToString, True)

                m_fltBuoyancyScale = Value
            End Set
        End Property

        Public Overridable Property LinearDrag() As Framework.ScaledVector3
            Get
                Return m_svLinearDrag
            End Get
            Set(ByVal value As Framework.ScaledVector3)
                Me.SetSimData("LinearDrag", value.GetSimulationXml("LinearDrag"), True)
                m_svLinearDrag.CopyData(value)
            End Set
        End Property

        Public Overridable Property AngularDrag() As Framework.ScaledVector3
            Get
                Return m_svAngularDrag
            End Get
            Set(ByVal value As Framework.ScaledVector3)
                Me.SetSimData("AngularDrag", value.GetSimulationXml("AngularDrag"), True)
                m_svAngularDrag.CopyData(value)
            End Set
        End Property

        Public Overridable Property MaxHydroForce() As ScaledNumber
            Get
                Return m_snMaxHydroForce
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The maximum hydrodynamic force can not be less than zero.")
                End If
                Me.SetSimData("MaxHydroForce", Value.ToString, True)

                m_snMaxHydroForce.CopyData(Value)
            End Set
        End Property

        Public Overridable Property MaxHydroTorque() As ScaledNumber
            Get
                Return m_snMaxHydroTorque
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The maximum hydrodynamic torque can not be less than zero.")
                End If
                Me.SetSimData("MaxHydroTorque", Value.ToString, True)

                m_snMaxHydroTorque.CopyData(Value)
            End Set
        End Property

        Public Overridable Property Magnus() As Single
            Get
                Return m_fltMagnus
            End Get
            Set(ByVal Value As Single)
                If Value < 0 Then
                    Throw New System.Exception("The magnus coefficient can not be less than to zero.")
                End If
                SetSimData("Magnus", Value.ToString, True)

                m_fltMagnus = Value
            End Set
        End Property

        Public Overridable Property EnableFluids() As Boolean
            Get
                Return m_bEnableFluids
            End Get
            Set(ByVal Value As Boolean)
                SetSimData("EnableFluids", Value.ToString, True)
                m_bEnableFluids = Value
            End Set
        End Property

        Public Overridable ReadOnly Property CanBeRootBody() As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overridable ReadOnly Property UsesAJoint() As Boolean
            Get
                If Me.IsCollisionObject AndAlso Not Me.IsContactSensor Then
                    Return True
                Else
                    Return False
                End If
            End Get
        End Property

        Public Overridable ReadOnly Property HasStaticJoint() As Boolean
            Get
                If Not Me.JointToParent Is Nothing AndAlso Util.IsTypeOf(Me.JointToParent.GetType(), GetType(Joints.StaticJoint), False) Then
                    Return True
                Else
                    Return False
                End If
            End Get
        End Property

        Public Overridable ReadOnly Property HasStaticChild() As Boolean
            Get
                Dim dbChild As AnimatGUI.DataObjects.Physical.RigidBody
                For Each deEntry As DictionaryEntry In m_aryChildBodies
                    dbChild = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Physical.RigidBody)
                    If dbChild.HasStaticJoint Then
                        Return True
                    End If
                Next

                Return False
            End Get
        End Property

        Public Overridable Property IsRoot() As Boolean
            Get
                Return m_bIsRoot
            End Get
            Set(ByVal Value As Boolean)
                m_bIsRoot = Value
            End Set
        End Property

        <EditorAttribute(GetType(TypeHelpers.OdorTypeEditor), GetType(System.Drawing.Design.UITypeEditor))> _
        Public Overridable Property OdorSources() As Collections.SortedOdors
            Get
                Return m_aryOdorSources
            End Get
            Set(ByVal Value As Collections.SortedOdors)
                If Not Value Is Nothing Then
                    m_aryOdorSources = Value
                End If
            End Set
        End Property

        Public Overridable Property FoodSource() As Boolean
            Get
                Return m_bFoodSource
            End Get
            Set(ByVal Value As Boolean)
                Me.SetSimData("FoodSource", Value.ToString, True)
                m_bFoodSource = Value

                Util.Application.ProjectWorkspace.RefreshProperties()
            End Set
        End Property

        Public Overridable Property FoodQuantity() As ScaledNumber
            Get
                Return m_snFoodQuantity
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The food quantity can not be less than zero.")
                End If
                Me.SetSimData("FoodQuantity", Value.ToString, True)

                m_snFoodQuantity.CopyData(Value)
            End Set
        End Property

        Public Overridable Property MaxFoodQuantity() As ScaledNumber
            Get
                Return m_snMaxFoodQuantity
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The max food quantity can not be less than zero.")
                End If
                Me.SetSimData("MaxFoodQuantity", Value.ToString, True)

                m_snMaxFoodQuantity.CopyData(Value)
            End Set
        End Property

        Public Overridable Property FoodReplenishRate() As ScaledNumber
            Get
                Return m_snFoodReplenishRate
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The food replenish rate can not be less than zero.")
                End If
                Me.SetSimData("FoodReplenishRate", Value.ToString, True)

                m_snFoodReplenishRate.CopyData(Value)
            End Set
        End Property

        Public Overridable Property FoodEnergyContent() As ScaledNumber
            Get
                Return m_snFoodEnergyContent
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The food Eenergy content can not be less than zero.")
                End If
                Me.SetSimData("FoodEnergyContent", Value.ToString, True)

                m_snFoodEnergyContent.CopyData(Value)
            End Set
        End Property

        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Rigid Body"
            End Get
        End Property

        Public Overridable ReadOnly Property ReceptiveFieldSensor() As ContactSensor
            Get
                Return m_doReceptiveFieldSensor
            End Get
        End Property

        Public Overridable ReadOnly Property SelectedVertex() As Vec3d
            Get
                Return m_vSelectedVertex
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property MaterialType() As AnimatGUI.TypeHelpers.LinkedMaterialType
            Get
                Return m_thMaterialType
            End Get
            Set(ByVal Value As AnimatGUI.TypeHelpers.LinkedMaterialType)
                If Not Value Is Nothing AndAlso Not Value.MaterialType Is Nothing Then
                    SetSimData("MaterialTypeID", Value.MaterialType.ID, True)
                    DisconnectLinkedMaterialEvents()
                    m_thMaterialType = Value
                    ConnectLinkedMaterialEvents()
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property MaterialTypeName() As String
            Get
                If Not m_thMaterialType Is Nothing AndAlso Not m_thMaterialType.MaterialType Is Nothing Then
                    Return m_thMaterialType.MaterialType.Name
                Else
                    Return ""
                End If
            End Get
            Set(ByVal Value As String)
                For Each deEntry As DictionaryEntry In Util.Simulation.Environment.MaterialTypes
                    Dim doMatType As DataObjects.Physical.MaterialType = DirectCast(deEntry.Value, DataObjects.Physical.MaterialType)
                    If doMatType.Name = Value Then
                        Dim thType As New AnimatGUI.TypeHelpers.LinkedMaterialType(Me, doMatType)
                        Me.MaterialType = thType
                        Return
                    End If
                Next
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property TotalSubChildren() As Integer
            Get
                Dim iBodies As Integer = Me.ChildBodies.Count

                If Not m_JointToParent Is Nothing Then
                    iBodies = iBodies + 1
                End If

                Dim doChild As RigidBody
                For Each deEntry As DictionaryEntry In Me.ChildBodies
                    doChild = DirectCast(deEntry.Value, RigidBody)
                    iBodies = iBodies + doChild.TotalSubChildren
                Next

                Return iBodies
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            m_thDataTypes.DataTypes.Clear()

            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("BodyTorqueX", "Torque X Axis", "Newton-Meters", "Nm", -1000, 1000))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("BodyTorqueY", "Torque Y Axis", "Newton-Meters", "Nm", -1000, 1000))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("BodyTorqueZ", "Torque Z Axis", "Newton-Meters", "Nm", -1000, 1000))

            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("BodyForceX", "Force X Axis", "Newtons", "N", -1000, 1000))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("BodyForceY", "Force Y Axis", "Newtons", "N", -1000, 1000))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("BodyForceZ", "Force Z Axis", "Newtons", "N", -1000, 1000))

            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("WorldPositionX", "Position X Axis", "Meters", "m", -10, 10))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("WorldPositionY", "Position Y Axis", "Meters", "m", -10, 10))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("WorldPositionZ", "Position Z Axis", "Meters", "m", -10, 10))

            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("RotationX", "Rotation X Axis", "Radians", "rad", -4, 4))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("RotationY", "Rotation Y Axis", "Radians", "rad", -4, 4))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("RotationZ", "Rotation Z Axis", "Radians", "rad", -4, 4))

            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("BodyLinearVelocityX", "Linear Velocity X Axis", "m/s", "m/s", -5, 5))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("BodyLinearVelocityY", "Linear Velocity Y Axis", "m/s", "m/s", -5, 5))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("BodyLinearVelocityZ", "Linear Velocity Z Axis", "m/s", "m/s", -5, 5))

            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("BodyAngularVelocityX", "Angular Velocity X Axis", "rad/s", "rad/s", -5, 5))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("BodyAngularVelocityY", "Angular Velocity Y Axis", "rad/s", "rad/s", -5, 5))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("BodyAngularVelocityZ", "Angular Velocity Z Axis", "rad/s", "rad/s", -5, 5))

            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("BodyLinearAccelerationX", "Linear Acceleration X Axis", "m/s^2", "m/s^2", -5, 5))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("BodyLinearAccelerationY", "Linear Acceleration Y Axis", "m/s^2", "m/s^2", -5, 5))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("BodyLinearAccelerationZ", "Linear Acceleration Z Axis", "m/s^2", "m/s^2", -5, 5))

            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("BodyAngularAccelerationX", "Angular Acceleration X Axis", "rad/s^2", "rad/s^2", -5, 5))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("BodyAngularAccelerationY", "Angular Acceleration Y Axis", "rad/s^2", "rad/s^2", -5, 5))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("BodyAngularAccelerationZ", "Angular Acceleration Z Axis", "rad/s^2", "rad/s^2", -5, 5))

            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("BodyBuoyancy", "Buoyancy", "Newtons", "N", -100, 100))

            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("BodyDragForceX", "Drag Force X Axis", "Newtons", "N", -100, 100))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("BodyDragForceY", "Drag Force Y Axis", "Newtons", "N", -100, 100))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("BodyDragForceZ", "Drag Force Z Axis", "Newtons", "N", -100, 100))

            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("BodyDragTorqueX", "Drag Torque X Axis", "Newton-Meters", "Nm", -100, 100))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("BodyDragTorqueY", "Drag Torque Y Axis", "Newton-Meters", "Nm", -100, 100))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("BodyDragTorqueZ", "Drag Torque Z Axis", "Newton-Meters", "Nm", -100, 100))

            'm_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("BodyBuoyancy", "Buoyancy", "Newtons", "N", 0, 100))

            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("FoodQuantity", "Food Quantity", "", "", 0, 1000))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("FoodEaten", "Food Eaten", "", "", 0, 1000))

            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("Mass", "Mass", "Kilograms", "Kg", -5000, 5000))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("Volume", "Volume", "Cubic Meters", "(m^3)", -100, 100))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("Visible", "Visible", "", "", 0, 1))

            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("ContactCount", "Contact Count", "", "", 0, 1))

            m_thDataTypes.ID = "BodyForceX"

            m_thIncomingDataType = New AnimatGUI.DataObjects.DataType("BodyForceX", "Body Force X", "Newtons", "N", -100, 100, ScaledNumber.enumNumericScale.None, ScaledNumber.enumNumericScale.None)

            m_svCOM = New ScaledVector3(Me, "COM", "Location of the COM relative to the (0,0,0) point of this part.", "Meters", "m")
            m_svBuoyancyCenter = New ScaledVector3(Me, "BuoyancyCenter", "Location of the center of buoyancy relative to the (0,0,0) point of this part.", "Meters", "m")
            m_svLinearDrag = New ScaledVector3(Me, "LinearDrag", "Linear drag coefficients of this part.", "", "")
            m_svLinearDrag.CopyData(1, 1, 1, True)
            m_svAngularDrag = New ScaledVector3(Me, "AngularDrag", "Angular drag coefficients of this part.", "", "")
            m_svAngularDrag.CopyData(0.05, 0.05, 0.05, True)

            m_snMaxHydroForce = New ScaledNumber(Me, "MaxHydroForce", 50, ScaledNumber.enumNumericScale.None, "Netwons", "N")
            m_snMaxHydroTorque = New ScaledNumber(Me, "MaxHydroTorque", 20, ScaledNumber.enumNumericScale.None, "Netwon-Meters", "Nm")

            If Not Util.Simulation Is Nothing AndAlso Not Util.Simulation.Environment Is Nothing AndAlso Not Util.Simulation.Environment.MaterialTypes Is Nothing AndAlso _
                Util.Simulation.Environment.MaterialTypes.ContainsKey("DEFAULTMATERIAL") Then
                m_thMaterialType = New AnimatGUI.TypeHelpers.LinkedMaterialType(Me, Util.Simulation.Environment.MaterialTypes.Item("DEFAULTMATERIAL"))
            Else
                m_thMaterialType = New AnimatGUI.TypeHelpers.LinkedMaterialType(Me)
            End If

            AddHandler m_svCOM.ValueChanged, AddressOf Me.OnCOMValueChanged
            AddHandler m_svBuoyancyCenter.ValueChanged, AddressOf Me.OnBuoyancyCenterValueChanged
            AddHandler m_svLinearDrag.ValueChanged, AddressOf Me.OnLinearDragValueChanged
            AddHandler m_svAngularDrag.ValueChanged, AddressOf Me.OnAngularDragValueChanged

            If Not Util.Environment Is Nothing Then
                m_snDensity = Util.Environment.DefaultDensity
            Else
                m_snDensity = New ScaledNumber(Me, "Density", 1, ScaledNumber.enumNumericScale.Kilo, "g/m^3", "g/m^3")
            End If

            m_snMass = New ScaledNumber(Me, "Mass", 1, ScaledNumber.enumNumericScale.Kilo, "g", "g")
            m_snVolume = New ScaledNumber(Me, "Volume", 1, ScaledNumber.enumNumericScale.None, "(m^3)", "(m^3)")

            m_snFoodQuantity = New ScaledNumber(Me, "FoodQuantity", 100, ScaledNumber.enumNumericScale.None, "", "")
            m_snMaxFoodQuantity = New ScaledNumber(Me, "MaxFoodQuantity", 10, ScaledNumber.enumNumericScale.Kilo, "", "")
            m_snFoodReplenishRate = New ScaledNumber(Me, "FoodReplenishRate", 1, ScaledNumber.enumNumericScale.None, "Quantity/s", "Q/s")
            m_snFoodEnergyContent = New ScaledNumber(Me, "FoodEnergyContent", 1, ScaledNumber.enumNumericScale.Kilo, "Calories/Quantity", "C/Q")

            'Receptive field sensor is NOT set initially. It will only be set when it is needed, and cleared if it is no longer needed.
            m_doReceptiveFieldSensor = Nothing
            m_vSelectedVertex = Nothing

        End Sub

        Protected Overridable Sub UpdateMassVolumeDensity()

            If Not Me.SimInterface Is Nothing Then
                If Util.Application.Physics.UseMassForRigidBodyDefinitions Then
                    'If the mass was loaded in as less than zero then we need to get it again from the simulation.
                    If m_snMass.ActualValue < 0 Then
                        m_snMass.ActualValue = Me.SimInterface.GetDataValueImmediate("Mass")
                    End If

                    m_snDensity.ActualValue = Me.SimInterface.GetDataValueImmediate("Density")
                    m_snVolume.ActualValue = Me.SimInterface.GetDataValueImmediate("Volume")
                Else
                    m_snMass.ActualValue = Me.SimInterface.GetDataValueImmediate("Mass")
                    m_snVolume.ActualValue = Me.SimInterface.GetDataValueImmediate("Volume")
                End If
            End If

            'If this is a static joint then update the parent body mass volume density as well.
            If Not Util.Application.Physics.UseMassForRigidBodyDefinitions AndAlso Me.HasStaticJoint Then
                If Not Me.Parent Is Nothing AndAlso Util.IsTypeOf(Me.Parent.GetType(), GetType(RigidBody), False) Then
                    Dim doParentBody As RigidBody = DirectCast(Me.Parent, RigidBody)
                    If Not doParentBody Is Nothing Then
                        doParentBody.UpdateMassVolumeDensity()
                    End If
                End If
            End If

            If Not Util.ProjectProperties Is Nothing Then
                Util.ProjectProperties.RefreshProperties()
            End If
        End Sub

        Public Overrides Sub InitAfterAppStart()
            MyBase.InitAfterAppStart()
            AddCompatibleStimulusType("ForceInput")
        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()
            If Not m_JointToParent Is Nothing Then m_JointToParent.ClearIsDirty()
            m_aryChildBodies.ClearIsDirty()
            m_aryOdorSources.ClearIsDirty()
            m_svBuoyancyCenter.ClearIsDirty()
            m_svLinearDrag.ClearIsDirty()
            m_svAngularDrag.ClearIsDirty()
            m_snMaxHydroForce.ClearIsDirty()
            m_snMaxHydroTorque.ClearIsDirty()
            m_svCOM.ClearIsDirty()
            m_snDensity.ClearIsDirty()
            m_snMass.ClearIsDirty()
            m_snVolume.ClearIsDirty()
            m_snFoodQuantity.ClearIsDirty()
            m_snMaxFoodQuantity.ClearIsDirty()
            m_snFoodReplenishRate.ClearIsDirty()
            m_snFoodEnergyContent.ClearIsDirty()
            If Not m_doReceptiveFieldSensor Is Nothing Then m_doReceptiveFieldSensor.ClearIsDirty()
        End Sub

        Public Overrides Sub SetupInitialTransparencies()
            If Not m_Transparencies Is Nothing Then

                If IsCollisionObject Then
                    m_Transparencies.GraphicsTransparency = 50
                    m_Transparencies.CollisionsTransparency = 0
                    m_Transparencies.JointsTransparency = 50
                    m_Transparencies.ReceptiveFieldsTransparency = 50
                    m_Transparencies.SimulationTransparency = 100
                Else
                    m_Transparencies.GraphicsTransparency = 0
                    m_Transparencies.CollisionsTransparency = 50
                    m_Transparencies.JointsTransparency = 50
                    m_Transparencies.ReceptiveFieldsTransparency = 50
                    m_Transparencies.SimulationTransparency = 0
                End If
            End If
        End Sub

        Protected Sub ConnectLinkedMaterialEvents()
            If Not m_thMaterialType Is Nothing AndAlso Not m_thMaterialType.MaterialType Is Nothing Then
                AddHandler m_thMaterialType.MaterialType.ReplaceMaterial, AddressOf Me.OnReplaceMaterial
            End If
        End Sub


        Protected Sub DisconnectLinkedMaterialEvents()
            If Not m_thMaterialType Is Nothing AndAlso Not m_thMaterialType.MaterialType Is Nothing Then
                RemoveHandler m_thMaterialType.MaterialType.ReplaceMaterial, AddressOf Me.OnReplaceMaterial
            End If
        End Sub

        Public Overrides Function FindBodyPart(ByVal strID As String) As BodyPart

            Dim bpPart As BodyPart
            If Not m_JointToParent Is Nothing Then bpPart = m_JointToParent.FindBodyPart(strID)
            If Not bpPart Is Nothing Then Return bpPart

            If Me.ID = strID Then Return Me

            Dim dbChild As AnimatGUI.DataObjects.Physical.RigidBody
            For Each deEntry As DictionaryEntry In m_aryChildBodies
                dbChild = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Physical.RigidBody)
                bpPart = dbChild.FindBodyPart(strID)
                If Not bpPart Is Nothing Then Return bpPart
            Next

            Return Nothing
        End Function

        Public Overrides Function FindBodyPartByName(ByVal strName As String) As BodyPart

            Dim bpPart As BodyPart
            If Not m_JointToParent Is Nothing Then bpPart = m_JointToParent.FindBodyPartByName(strName)
            If Not bpPart Is Nothing Then Return bpPart

            If Me.Name = strName Then Return Me

            Dim dbChild As AnimatGUI.DataObjects.Physical.RigidBody
            For Each deEntry As DictionaryEntry In m_aryChildBodies
                dbChild = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Physical.RigidBody)
                bpPart = dbChild.FindBodyPartByName(strName)
                If Not bpPart Is Nothing Then Return bpPart
            Next

            Return Nothing
        End Function

        Public Overrides Function FindBodyPartByCloneID(ByVal strId As String) As BodyPart

            Dim bpPart As BodyPart
            If Not m_JointToParent Is Nothing Then bpPart = m_JointToParent.FindBodyPartByCloneID(strId)
            If Not bpPart Is Nothing Then Return bpPart

            If Me.CloneID = strId Then Return Me

            Dim dbChild As AnimatGUI.DataObjects.Physical.RigidBody
            For Each deEntry As DictionaryEntry In m_aryChildBodies
                dbChild = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Physical.RigidBody)
                bpPart = dbChild.FindBodyPartByCloneID(strId)
                If Not bpPart Is Nothing Then Return bpPart
            Next

            Return Nothing
        End Function

        Public Overloads Overrides Sub FindChildrenOfType(ByVal tpTemplate As System.Type, ByVal colDataObjects As Collections.DataObjects)
            MyBase.FindChildrenOfType(tpTemplate, colDataObjects)

            If Not m_JointToParent Is Nothing Then
                m_JointToParent.FindChildrenOfType(tpTemplate, colDataObjects)
            End If

            Dim dbChild As AnimatGUI.DataObjects.Physical.RigidBody
            For Each deEntry As DictionaryEntry In m_aryChildBodies
                dbChild = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Physical.RigidBody)
                dbChild.FindChildrenOfType(tpTemplate, colDataObjects)
            Next

        End Sub

        Public Overrides Function FindObjectByID(ByVal strID As String) As Framework.DataObject

            Dim doObject As AnimatGUI.Framework.DataObject = MyBase.FindObjectByID(strID)
            If doObject Is Nothing AndAlso Not m_JointToParent Is Nothing Then doObject = m_JointToParent.FindObjectByID(strID)
            If doObject Is Nothing AndAlso Not m_aryChildBodies Is Nothing Then doObject = m_aryChildBodies.FindObjectByID(strID)
            If doObject Is Nothing AndAlso Not m_doReceptiveFieldSensor Is Nothing Then m_doReceptiveFieldSensor.FindObjectByID(strID)
            If doObject Is Nothing AndAlso Not m_aryOdorSources Is Nothing Then doObject = m_aryOdorSources.FindObjectByID(strID)
            Return doObject

        End Function

        Public Overrides Function CreateJointTreeView(ByRef tvTree As Crownwood.DotNetMagic.Controls.TreeControl, ByVal tnParent As Crownwood.DotNetMagic.Controls.Node, _
                                                      ByVal thSelectedPart As TypeHelpers.LinkedBodyPart) As Crownwood.DotNetMagic.Controls.Node

            If Not m_JointToParent Is Nothing Then
                tnParent = m_JointToParent.CreateJointTreeView(tvTree, tnParent, thSelectedPart)
            End If

            Dim dbChild As AnimatGUI.DataObjects.Physical.RigidBody
            For Each deEntry As DictionaryEntry In m_aryChildBodies
                dbChild = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Physical.RigidBody)
                dbChild.CreateJointTreeView(tvTree, tnParent, thSelectedPart)
            Next

        End Function

        Public Overrides Function CreateRigidBodyTreeView(ByRef tvTree As Crownwood.DotNetMagic.Controls.TreeControl, ByVal tnParent As Crownwood.DotNetMagic.Controls.Node, _
                                                          ByVal thSelectedPart As TypeHelpers.LinkedBodyPart) As Crownwood.DotNetMagic.Controls.Node

            Dim tnBody As New Crownwood.DotNetMagic.Controls.Node(Me.Name)
            tnParent.Nodes.Add(tnBody)
            tnBody.ForeColor = Color.Red
            Dim thPart As TypeHelpers.LinkedBodyPart = DirectCast(thSelectedPart.Clone(thSelectedPart.Parent, False, Nothing), TypeHelpers.LinkedBodyPart)
            thPart.BodyPart = Me
            tnBody.Tag = thPart

            Dim dbChild As AnimatGUI.DataObjects.Physical.RigidBody
            For Each deEntry As DictionaryEntry In m_aryChildBodies
                dbChild = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Physical.RigidBody)
                dbChild.CreateRigidBodyTreeView(tvTree, tnBody, thSelectedPart)
            Next

        End Function

        Public Overrides Sub CreateWorkspaceTreeView(ByVal doParent As Framework.DataObject, _
                                                       ByVal tnParentNode As Crownwood.DotNetMagic.Controls.Node, _
                                                       Optional ByVal bRootObject As Boolean = False)

            Dim tnParent As Crownwood.DotNetMagic.Controls.Node = tnParentNode
            If Not m_JointToParent Is Nothing Then
                m_JointToParent.CreateWorkspaceTreeView(Me, tnParentNode)
                tnParent = m_JointToParent.WorkspaceNode
            End If

            MyBase.CreateWorkspaceTreeView(doParent, tnParent, bRootObject)

            Dim dbChild As AnimatGUI.DataObjects.Physical.RigidBody
            For Each deEntry As DictionaryEntry In m_aryChildBodies
                dbChild = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Physical.RigidBody)
                dbChild.CreateWorkspaceTreeView(Me, m_tnWorkspaceNode)
            Next

        End Sub

        Public Overrides Function CreateObjectListTreeView(ByVal doParent As Framework.DataObject, _
                                                       ByVal tnParentNode As Crownwood.DotNetMagic.Controls.Node, _
                                                       ByVal mgrImageList As AnimatGUI.Framework.ImageManager) As Crownwood.DotNetMagic.Controls.Node

            Dim tnParent As Crownwood.DotNetMagic.Controls.Node = tnParentNode
            If Not m_JointToParent Is Nothing Then
                tnParent = m_JointToParent.CreateObjectListTreeView(Me, tnParentNode, mgrImageList)
            End If

            Dim tnNode As Crownwood.DotNetMagic.Controls.Node = MyBase.CreateObjectListTreeView(doParent, tnParent, mgrImageList)

            Dim dbChild As AnimatGUI.DataObjects.Physical.RigidBody
            For Each deEntry As DictionaryEntry In m_aryChildBodies
                dbChild = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Physical.RigidBody)
                dbChild.CreateObjectListTreeView(Me, tnNode, mgrImageList)
            Next

            If Not m_doReceptiveFieldSensor Is Nothing Then m_doReceptiveFieldSensor.CreateObjectListTreeView(Me, tnNode, mgrImageList)
            If Not m_aryOdorSources Is Nothing AndAlso m_aryOdorSources.Count > 0 Then
                Dim tnOdorSources As Crownwood.DotNetMagic.Controls.Node = Util.AddTreeNode(tnNode, "Odor Sources", "AnimatGUI.DefaultObject.gif", "", mgrImageList)
                Dim doObj As Framework.DataObject
                For Each deEntry As DictionaryEntry In m_aryOdorSources
                    doObj = DirectCast(deEntry.Value, Framework.DataObject)
                    doObj.CreateObjectListTreeView(Me, tnOdorSources, mgrImageList)
                Next
            End If

            Return tnNode
        End Function

        Public Overrides Sub RemoveWorksapceTreeView()

            If Not m_JointToParent Is Nothing Then
                If Not m_JointToParent.WorkspaceNode Is Nothing Then
                    m_JointToParent.RemoveWorksapceTreeView()
                End If
            End If

            MyBase.RemoveWorksapceTreeView()
        End Sub

        Public Overrides Function WorkspaceTreeviewPopupMenu(ByRef tnSelectedNode As Crownwood.DotNetMagic.Controls.Node, ByVal ptPoint As System.Drawing.Point) As Boolean

            If tnSelectedNode Is m_tnWorkspaceNode Then
                If tnSelectedNode Is m_tnWorkspaceNode Then
                    Dim popup As AnimatContextMenuStrip = CreateWorkspaceTreeViewPopupMenu(tnSelectedNode, ptPoint)

                    If Me.IsCollisionObject AndAlso Me.AllowUserAdd Then
                        Dim mcSepExpand As New ToolStripSeparator()
                        Dim mcAddChild As New System.Windows.Forms.ToolStripMenuItem("Add child body", Util.Application.ToolStripImages.GetImage("AnimatGUI.AddPart.gif"), New EventHandler(AddressOf Me.OnAddChildBody))
                        popup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {mcSepExpand, mcAddChild})
                    End If

                    Util.ProjectWorkspace.ctrlTreeView.ContextMenuNode = popup
                    Return True
                End If
            Else
                If Not m_JointToParent Is Nothing Then
                    If m_JointToParent.WorkspaceTreeviewPopupMenu(tnSelectedNode, ptPoint) Then
                        Return True
                    End If
                End If

                Dim doChild As AnimatGUI.DataObjects.Physical.RigidBody
                For Each deEntry As DictionaryEntry In m_aryChildBodies
                    doChild = DirectCast(deEntry.Value, DataObjects.Physical.RigidBody)
                    If doChild.WorkspaceTreeviewPopupMenu(tnSelectedNode, ptPoint) Then
                        Return True
                    End If
                Next
            End If

            Return False
        End Function

        Public Overrides Function CreateDataItemTreeView(ByVal frmDataItem As Forms.Tools.SelectDataItem, ByVal tnParent As Crownwood.DotNetMagic.Controls.Node, ByVal tpTemplatePartType As Type) As Crownwood.DotNetMagic.Controls.Node

            If tpTemplatePartType Is Nothing OrElse (Not tpTemplatePartType Is Nothing AndAlso Util.IsTypeOf(Me.GetType(), tpTemplatePartType, False)) Then
                Dim tnJointToParent As Crownwood.DotNetMagic.Controls.Node
                If Not m_JointToParent Is Nothing Then
                    tnJointToParent = m_JointToParent.CreateDataItemTreeView(frmDataItem, tnParent, tpTemplatePartType)
                End If

                Dim tnNewParent As Crownwood.DotNetMagic.Controls.Node
                If m_JointToParent Is Nothing Then
                    tnNewParent = MyBase.CreateDataItemTreeView(frmDataItem, tnParent, tpTemplatePartType)
                Else
                    tnNewParent = MyBase.CreateDataItemTreeView(frmDataItem, tnJointToParent, tpTemplatePartType)
                End If

                Dim dbChild As AnimatGUI.DataObjects.Physical.RigidBody
                For Each deEntry As DictionaryEntry In m_aryChildBodies
                    dbChild = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Physical.RigidBody)
                    dbChild.CreateDataItemTreeView(frmDataItem, tnNewParent, tpTemplatePartType)
                Next
            End If

        End Function

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            If Me.IsRoot Then
                If propTable.Properties.Contains("Local Position") Then propTable.Properties.Remove("Local Position")
            End If

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag
            If Me.IsCollisionObject Then
                If Util.Simulation.Environment.SimulateHydrodynamics Then
                    pbNumberBag = m_svBuoyancyCenter.Properties
                    propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Buoyancy Center", pbNumberBag.GetType(), "BuoyancyCenter", _
                                                "Hydrodynamics", "This is the relative position to the center of the buoyancy in the body.", pbNumberBag, _
                                                "", GetType(AnimatGUI.Framework.ScaledVector3.ScaledVector3PropBagConverter)))

                    propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Buoyancy Scale", m_fltBuoyancyScale.GetType(), "BuoyancyScale", _
                                       "Hydrodynamics", "This is a scale used to calculate the buoyancy value. It is a scale factor " & _
                                       "applied to the buoyancy force which accounts for the fact that a given volume might actually have holes " & _
                                       "or concavity in it which would affect the buoyancy force on the object.", m_fltBuoyancyScale))

                    If Util.Application.Physics.UseHydrodynamicsMagnus Then
                        pbNumberBag = m_svLinearDrag.Properties
                        propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Drag", pbNumberBag.GetType(), "LinearDrag", _
                                                    "Hydrodynamics", "This is the drag coefficients for the three axis for the body.", pbNumberBag, _
                                                    "", GetType(AnimatGUI.Framework.ScaledVector3.ScaledVector3PropBagConverter)))

                        propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Magnus", m_fltMagnus.GetType(), "Magnus", _
                                         "Hydrodynamics", "The Magnus coefficient for the body.", m_fltMagnus))
                    Else
                        pbNumberBag = m_svLinearDrag.Properties
                        propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Linear Drag", pbNumberBag.GetType(), "LinearDrag", _
                                                    "Hydrodynamics", "This is the linear drag coefficients for the three axis for the body.", pbNumberBag, _
                                                    "", GetType(AnimatGUI.Framework.ScaledVector3.ScaledVector3PropBagConverter)))

                        pbNumberBag = m_svAngularDrag.Properties
                        propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Angular Drag", pbNumberBag.GetType(), "AngularDrag", _
                                                    "Hydrodynamics", "This is the angular drag coefficients for the three axis for the body.", pbNumberBag, _
                                                    "", GetType(AnimatGUI.Framework.ScaledVector3.ScaledVector3PropBagConverter)))

                        pbNumberBag = m_snMaxHydroForce.Properties
                        propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Max Force", pbNumberBag.GetType(), "MaxHydroForce", _
                                                    "Hydrodynamics", "Sets the maximum hydrodynamic force that can be applied to this part.", pbNumberBag, _
                                                    "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

                        pbNumberBag = m_snMaxHydroTorque.Properties
                        propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Max Torque", pbNumberBag.GetType(), "MaxHydroTorque", _
                                                    "Hydrodynamics", "Sets the maximum hydrodynamic torque that can be applied to this part.", pbNumberBag, _
                                                    "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))
                    End If


                    propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Enable Fluids", m_bEnableFluids.GetType(), "EnableFluids", _
                                     "Hydrodynamics", "Enables fluid interactions for this specific body.", m_bEnableFluids))
                End If

                If Not Me.IsContactSensor Then
                    pbNumberBag = m_snDensity.Properties
                    propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Density", pbNumberBag.GetType(), "Density", _
                                                "Mass Properties", "Sets the density of this body part.", pbNumberBag, _
                                                "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

                    If Util.Application.Physics.UseMassForRigidBodyDefinitions OrElse Not Me.HasStaticJoint Then
                        pbNumberBag = m_snMass.Properties
                        propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Mass", pbNumberBag.GetType(), "Mass", _
                                                    "Mass Properties", "Sets the mass of this body part.", pbNumberBag, _
                                                    "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

                        pbNumberBag = m_snVolume.Properties
                        propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Volume", pbNumberBag.GetType(), "Volume", _
                                                    "Mass Properties", "Tells the volume of this body part. Please note that this number is always in cubic meters.", pbNumberBag, _
                                                    "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter), True))

                    End If
                End If

                'Center Of Mass
                pbNumberBag = Me.COM.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Center of Mass", pbNumberBag.GetType(), "COM", _
                                            "Coordinates", "Sets the COM of this body part relative to the center of the part.", pbNumberBag, _
                                            "", GetType(AnimatGUI.Framework.ScaledVector3.ScaledVector3PropBagConverter)))

                'm_JointToParent Is Nothing
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Freeze", m_bFreeze.GetType(), "Freeze", _
                                                "Part Properties", "If the root body is frozen then it is locked in place in the environment.", m_bFreeze))

                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Material Type", GetType(AnimatGUI.TypeHelpers.LinkedMaterialType), "MaterialType", _
                               "Part Properties", "The material to be used for this part.", _
                               m_thMaterialType, GetType(AnimatGUI.TypeHelpers.DropDownListEditor), _
                               GetType(AnimatGUI.TypeHelpers.LinkedMaterialTypeConverter)))
            Else
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Contact Sensor", m_bContactSensor.GetType(), "IsContactSensor", _
                                            "Part Properties", "Sets whether or not this part can detect contacts.", m_bContactSensor, True))
            End If

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Is Collision Object", m_bIsCollisionObject.GetType(), "IsCollisionObject", _
                                        "Part Properties", "If this is true then it is a collision object.", m_bIsCollisionObject, True))


            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Odor Sources", m_aryOdorSources.GetType(), "OdorSources", _
                                        "Odor Properties", "Edit the odor sources that this part can emit.", m_aryOdorSources, _
                                        GetType(TypeHelpers.OdorTypeEditor), GetType(TypeHelpers.OdorTypeConverter)))

            If m_bFoodSource Then
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Food Source", m_bFoodSource.GetType(), "FoodSource", _
                                            "Food Properties", "Determines whether this rigid body is a food source."))

                pbNumberBag = Me.m_snFoodQuantity.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Food Quantity", pbNumberBag.GetType(), "FoodQuantity", _
                                            "Food Properties", "Sets the initial quantity of food available at this food source.", pbNumberBag, _
                                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

                pbNumberBag = Me.m_snMaxFoodQuantity.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Max Food Quantity", pbNumberBag.GetType(), "MaxFoodQuantity", _
                                            "Food Properties", "Sets the maximum quantity of food available at this food source.", pbNumberBag, _
                                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

                pbNumberBag = Me.m_snFoodReplenishRate.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Food Replenish Rate", pbNumberBag.GetType(), "FoodReplenishRate", _
                                            "Food Properties", "Sets the rate at which food is replensihed in this food source.", pbNumberBag, _
                                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

                pbNumberBag = Me.m_snFoodEnergyContent.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Food Energy Content", pbNumberBag.GetType(), "FoodEnergyContent", _
                                            "Food Properties", "Sets the calorie content for each piece of food.", pbNumberBag, _
                                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))
            Else
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Food Source", m_bFoodSource.GetType(), "FoodSource", _
                                            "Food Properties", "Determines whether this rigid body is a food source."))
            End If

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Texture", GetType(String), "Texture", _
                                        "Visibility", "Sets the bmp texture file to wrap onto this body part.", Me.Texture, GetType(TypeHelpers.ImageFileEditor)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Body Type", Me.Type.GetType(), "Type", _
                                        "Part Properties", "Type of rigid body.", Me.Type, True))

        End Sub

        Public Overrides Sub AfterClone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal doClone As AnimatGUI.Framework.DataObject)
            MyBase.AfterClone(doParent, bCutData, doOriginal, doClone)

            If Not m_JointToParent Is Nothing Then
                m_JointToParent.AfterClone(Me, bCutData, doOriginal, doClone)
            End If

            Dim doChild As RigidBody
            For Each deItem As DictionaryEntry In Me.m_aryChildBodies
                doChild = DirectCast(deItem.Value, RigidBody)
                doChild.AfterClone(Me, bCutData, doOriginal, doClone)
            Next

        End Sub

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doOrigPart As RigidBody = DirectCast(doOriginal, RigidBody)

            If Not doOrigPart.m_JointToParent Is Nothing Then
                m_JointToParent = DirectCast(doOrigPart.m_JointToParent.Clone(doOrigPart.m_JointToParent.Parent, bCutData, doRoot), Joint)
            Else
                m_JointToParent = Nothing
            End If

            'Remove the old change handlers.
            If Not m_svCOM Is Nothing Then RemoveHandler m_svCOM.ValueChanged, AddressOf Me.OnCOMValueChanged
            If Not m_svBuoyancyCenter Is Nothing Then RemoveHandler m_svBuoyancyCenter.ValueChanged, AddressOf Me.OnBuoyancyCenterValueChanged
            If Not m_svLinearDrag Is Nothing Then RemoveHandler m_svLinearDrag.ValueChanged, AddressOf Me.OnLinearDragValueChanged
            If Not m_svAngularDrag Is Nothing Then RemoveHandler m_svAngularDrag.ValueChanged, AddressOf Me.OnAngularDragValueChanged

            m_bFreeze = doOrigPart.m_bFreeze
            m_bContactSensor = doOrigPart.m_bContactSensor
            m_bIsCollisionObject = doOrigPart.m_bIsCollisionObject
            m_snDensity = DirectCast(doOrigPart.m_snDensity.Clone(doOrigPart.m_snDensity.Parent, bCutData, doRoot), ScaledNumber)

            'sometimes the units of the body parts loaded in from the plug-in initialization are not correct for the scale settings
            'of this project. Lets just always reset them to be correct when doing a clone.
            m_snDensity.Units = "g/" & Util.Environment.DistanceUnitAbbreviation(Util.Environment.DisplayDistanceUnits) & "^3"
            m_snDensity.UnitsAbbreviation = m_snDensity.Units

            m_snMass = DirectCast(doOrigPart.m_snMass.Clone(doOrigPart.m_snMass.Parent, bCutData, doRoot), ScaledNumber)
            m_snVolume = DirectCast(doOrigPart.m_snVolume.Clone(doOrigPart.m_snVolume.Parent, bCutData, doRoot), ScaledNumber)

            m_svBuoyancyCenter = DirectCast(doOrigPart.m_svBuoyancyCenter.Clone(Me, bCutData, doRoot), ScaledVector3)
            m_fltBuoyancyScale = doOrigPart.m_fltBuoyancyScale
            m_svLinearDrag = DirectCast(doOrigPart.m_svLinearDrag.Clone(Me, bCutData, doRoot), ScaledVector3)
            m_svAngularDrag = DirectCast(doOrigPart.m_svAngularDrag.Clone(Me, bCutData, doRoot), ScaledVector3)
            m_snMaxHydroForce = DirectCast(doOrigPart.m_snMaxHydroForce.Clone(doOrigPart.m_snMaxHydroForce.Parent, bCutData, doRoot), ScaledNumber)
            m_snMaxHydroTorque = DirectCast(doOrigPart.m_snMaxHydroTorque.Clone(doOrigPart.m_snMaxHydroTorque.Parent, bCutData, doRoot), ScaledNumber)
            m_fltMagnus = doOrigPart.m_fltMagnus
            m_bEnableFluids = doOrigPart.m_bEnableFluids

            m_bIsRoot = doOrigPart.m_bIsRoot
            m_bFoodSource = doOrigPart.m_bFoodSource
            m_snFoodQuantity = DirectCast(doOrigPart.m_snFoodQuantity.Clone(doOrigPart.m_snFoodQuantity.Parent, bCutData, doRoot), ScaledNumber)
            m_snMaxFoodQuantity = DirectCast(doOrigPart.m_snMaxFoodQuantity.Clone(doOrigPart.m_snMaxFoodQuantity.Parent, bCutData, doRoot), ScaledNumber)
            m_snFoodReplenishRate = DirectCast(doOrigPart.m_snFoodReplenishRate.Clone(doOrigPart.m_snFoodReplenishRate.Parent, bCutData, doRoot), ScaledNumber)
            m_snFoodEnergyContent = DirectCast(doOrigPart.m_snFoodEnergyContent.Clone(doOrigPart.m_snFoodEnergyContent.Parent, bCutData, doRoot), ScaledNumber)
            m_svCOM = DirectCast(doOrigPart.m_svCOM.Clone(Me, bCutData, doRoot), ScaledVector3)

            'Add new change handlers.
            AddHandler m_svCOM.ValueChanged, AddressOf Me.OnCOMValueChanged
            AddHandler m_svBuoyancyCenter.ValueChanged, AddressOf Me.OnBuoyancyCenterValueChanged
            AddHandler m_svLinearDrag.ValueChanged, AddressOf Me.OnLinearDragValueChanged
            AddHandler m_svAngularDrag.ValueChanged, AddressOf Me.OnAngularDragValueChanged

            Me.MaterialType = DirectCast(doOrigPart.m_thMaterialType.Clone(Me, bCutData, doRoot), TypeHelpers.LinkedMaterialType)

            m_aryChildBodies = DirectCast(doOrigPart.m_aryChildBodies.Clone(Me, bCutData, doRoot), Collections.SortedRigidBodies)

            'Dim doOrigChild As RigidBody
            'Dim doNewChild As RigidBody
            'For Each deItem As DictionaryEntry In doOrigPart.m_aryChildBodies
            '    doOrigChild = DirectCast(deItem.Value, RigidBody)
            '    doNewChild = DirectCast(doOrigChild.Clone(Me, bCutData, doRoot), RigidBody)
            '    m_aryChildBodies.Add(doNewChild.ID, doNewChild, False)
            'Next

            m_aryOdorSources = DirectCast(doOrigPart.m_aryOdorSources.Clone(Me, bCutData, doRoot), Collections.SortedOdors)

            If Not m_doReceptiveFieldSensor Is Nothing Then
                m_doReceptiveFieldSensor = DirectCast(doOrigPart.m_doReceptiveFieldSensor.Clone(Me, bCutData, doRoot), ContactSensor)
            End If

        End Sub

        Public Overrides Sub AddToReplaceIDList(ByVal aryReplaceIDList As ArrayList, ByVal arySelectedItems As ArrayList)
            MyBase.AddToReplaceIDList(aryReplaceIDList, arySelectedItems)

            If Not m_JointToParent Is Nothing Then
                m_JointToParent.AddToReplaceIDList(aryReplaceIDList, arySelectedItems)
            End If

            m_aryChildBodies.AddToReplaceIDList(aryReplaceIDList, arySelectedItems)
            m_aryOdorSources.AddToReplaceIDList(aryReplaceIDList, arySelectedItems)
            If Not m_doReceptiveFieldSensor Is Nothing Then
                m_doReceptiveFieldSensor.AddToReplaceIDList(aryReplaceIDList, arySelectedItems)
            End If
        End Sub

        Public Overrides Sub AddToRecursiveSelectedItemsList(ByVal arySelectedItems As ArrayList)
            MyBase.AddToRecursiveSelectedItemsList(arySelectedItems)

            If Not m_JointToParent Is Nothing Then
                m_JointToParent.AddToRecursiveSelectedItemsList(arySelectedItems)
            End If

            m_aryChildBodies.AddToRecursiveSelectedItemsList(arySelectedItems)
            m_aryOdorSources.AddToRecursiveSelectedItemsList(arySelectedItems)
            If Not m_doReceptiveFieldSensor Is Nothing Then
                m_doReceptiveFieldSensor.AddToRecursiveSelectedItemsList(arySelectedItems)
            End If
        End Sub

        Public Overrides Sub RenameBodyParts(ByVal doStructure As AnimatGUI.DataObjects.Physical.PhysicalStructure)

            Me.m_strID = System.Guid.NewGuid.ToString()

            Try
                doStructure.NewBodyIndex = doStructure.NewBodyIndex + 1
                Me.Name = "Body_" & doStructure.NewBodyIndex
            Catch ex As System.Exception
                Me.Name = "Body_" & System.Guid.NewGuid.ToString()
            End Try

            If Not m_JointToParent Is Nothing Then
                m_JointToParent.RenameBodyParts(doStructure)
            End If

            Dim doChild As AnimatGUI.DataObjects.Physical.RigidBody
            For Each deItem As DictionaryEntry In Me.ChildBodies
                doChild = DirectCast(deItem.Value, AnimatGUI.DataObjects.Physical.RigidBody)
                doChild.RenameBodyParts(doStructure)
            Next
        End Sub

        Public Overrides Sub ClearSelectedBodyParts()
            MyBase.ClearSelectedBodyParts()

            If Not m_JointToParent Is Nothing Then
                m_JointToParent.ClearSelectedBodyParts()
            End If

            Dim doChild As AnimatGUI.DataObjects.Physical.RigidBody
            For Each deItem As DictionaryEntry In Me.ChildBodies
                doChild = DirectCast(deItem.Value, AnimatGUI.DataObjects.Physical.RigidBody)
                doChild.ClearSelectedBodyParts()
            Next

        End Sub

        Public Overrides Function SwapBodyPartList() As AnimatGUI.Collections.BodyParts

            'Go through the list and only use body parts that allow dynamics
            Dim aryPartList As New AnimatGUI.Collections.BodyParts(Nothing)
            For Each doPart As DataObjects.Physical.BodyPart In Util.Application.RigidBodyTypes
                If doPart.HasDynamics Then
                    aryPartList.Add(doPart)
                End If
            Next

            Return aryPartList
        End Function

        Public Overrides Sub SwapBodyPartCopy(ByVal doOriginal As AnimatGUI.DataObjects.Physical.BodyPart)

            Dim doExisting As AnimatGUI.DataObjects.Physical.RigidBody = DirectCast(doOriginal, AnimatGUI.DataObjects.Physical.RigidBody)

            Me.Name = doExisting.Name
            Me.ID = doExisting.ID
            Me.Density = doExisting.Density
            Me.Mass = doExisting.Mass
            Me.m_snVolume.ActualValue = doExisting.m_snVolume.ActualValue
            Me.Ambient = doExisting.Ambient
            Me.Diffuse = doExisting.Diffuse
            Me.Specular = doExisting.Specular
            Me.Shininess = doExisting.Shininess
            Me.m_svBuoyancyCenter = doExisting.m_svBuoyancyCenter
            Me.m_fltBuoyancyScale = doExisting.m_fltBuoyancyScale
            Me.m_svLinearDrag = doExisting.m_svLinearDrag
            Me.m_svAngularDrag = doExisting.m_svAngularDrag
            Me.m_snMaxHydroForce = doExisting.m_snMaxHydroForce
            Me.m_snMaxHydroTorque = doExisting.m_snMaxHydroTorque
            Me.m_fltMagnus = doExisting.m_fltMagnus
            Me.m_bEnableFluids = doExisting.m_bEnableFluids
            Me.Description = doExisting.Description
            Me.FoodEnergyContent = doExisting.FoodEnergyContent
            Me.FoodQuantity = doExisting.FoodQuantity
            Me.FoodReplenishRate = doExisting.FoodReplenishRate
            Me.FoodSource = doExisting.FoodSource
            Me.Freeze = doExisting.Freeze
            Me.IsRoot = doExisting.IsRoot
            Me.MaxFoodQuantity = doExisting.MaxFoodQuantity
            Me.OdorSources = doExisting.OdorSources
            Me.Texture = doExisting.Texture
            Me.Visible = doExisting.Visible
            Me.Transparencies = doExisting.Transparencies

        End Sub

        Public Overrides Sub InitializeAfterLoad()
            MyBase.InitializeAfterLoad()

            If Not m_JointToParent Is Nothing Then
                m_JointToParent.InitializeAfterLoad()
            End If

            Dim doChild As AnimatGUI.DataObjects.Physical.RigidBody
            For Each deEntry As DictionaryEntry In m_aryChildBodies
                doChild = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Physical.RigidBody)
                doChild.InitializeAfterLoad()
            Next

            If Not m_doReceptiveFieldSensor Is Nothing Then
                m_doReceptiveFieldSensor.InitializeAfterLoad()
            End If

            If m_strMaterialTypeID.Trim.Length > 0 Then
                If Util.Environment.MaterialTypes.Contains(m_strMaterialTypeID) Then
                    Me.MaterialType = New AnimatGUI.TypeHelpers.LinkedMaterialType(Me, Util.Environment.MaterialTypes(m_strMaterialTypeID))
                End If
            End If

        End Sub

        Public Overrides Sub InitializeSimulationReferences(Optional ByVal bShowError As Boolean = True)
            'Only do this if not already initialized.
            If m_doInterface Is Nothing Then
                MyBase.InitializeSimulationReferences(bShowError)

                If Not m_doInterface Is Nothing Then
                    AddHandler m_doInterface.OnAddBodyClicked, AddressOf Me.OnAddBodyClicked
                    AddHandler m_doInterface.OnSelectedVertexChanged, AddressOf Me.OnSelectedVertexChanged

                    UpdateMassVolumeDensity()
                End If

                If Not m_JointToParent Is Nothing Then
                    m_JointToParent.InitializeSimulationReferences(bShowError)
                End If

                Dim doChild As AnimatGUI.DataObjects.Physical.RigidBody
                For Each deEntry As DictionaryEntry In m_aryChildBodies
                    doChild = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Physical.RigidBody)
                    doChild.InitializeSimulationReferences(bShowError)
                Next

                Dim doOdor As Odor
                For Each deEntry As DictionaryEntry In m_aryOdorSources
                    doOdor = DirectCast(deEntry.Value, Odor)
                    doOdor.InitializeSimulationReferences(bShowError)
                Next

                If Not m_doReceptiveFieldSensor Is Nothing Then
                    m_doReceptiveFieldSensor.InitializeSimulationReferences(bShowError)
                End If
            End If
        End Sub

        Public Overridable Sub EnableCollisions(ByVal doOtherBody As RigidBody)

            'Only re-enable collisions if they are not parent child parts.
            If Not (m_doParent Is doOtherBody OrElse doOtherBody.Parent Is Me) Then
                m_doInterface.EnableCollisions(doOtherBody.ID)
            End If

        End Sub

        Public Overridable Sub DisableCollisions(ByVal doOtherBody As RigidBody)
            m_doInterface.DisableCollisions(doOtherBody.ID)
        End Sub

        Public Overloads Overrides Sub LoadData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.LoadData(doStructure, oXml)

            m_aryChildBodies.Clear()

            oXml.IntoElem() 'Into RigidBody Element

            m_bContactSensor = oXml.GetChildBool("IsContactSensor", m_bContactSensor)
            m_bIsCollisionObject = oXml.GetChildBool("IsCollisionObject", m_bIsCollisionObject)

            m_bFreeze = oXml.GetChildBool("Freeze", m_bFreeze)

            m_snDensity.LoadData(oXml, "Density")
            If oXml.FindChildElement("Mass", False) Then
                m_snMass.LoadData(oXml, "Mass")
            ElseIf Util.Application.Physics.UseMassForRigidBodyDefinitions Then
                'If we could not load the mass in and we need that for the definition then lets save
                'that here temporarrily as -1 to let the sim know this is invalid and needs to be calculated
                'directly. Then we will reload it from the simulation later.
                m_snMass.ActualValue = -1
            End If

            m_svCOM.LoadData(oXml, "COM", False)

            m_svBuoyancyCenter.LoadData(oXml, "BuoyancyCenter", False)
            m_fltBuoyancyScale = oXml.GetChildFloat("BuoyancyScale", m_fltBuoyancyScale)

            If oXml.FindChildElement("Drag", False) Then
                m_svLinearDrag.LoadData(oXml, "Drag", False)
            Else
                m_svLinearDrag.LoadData(oXml, "LinearDrag", False)
                m_svAngularDrag.LoadData(oXml, "AngularDrag", False)
                m_snMaxHydroForce.LoadData(oXml, "MaxHydroForce", False)
                m_snMaxHydroTorque.LoadData(oXml, "MaxHydroTorque", False)
            End If

            m_fltMagnus = oXml.GetChildFloat("Magnus", m_fltMagnus)
            m_bEnableFluids = oXml.GetChildBool("EnableFluids", m_bEnableFluids)

            m_strMaterialTypeID = oXml.GetChildString("MaterialTypeID", "DEFAULTMATERIAL")

            'If this is the root body element then do not attempt to load the joint. Otherwise it must have a joint
            If Not Me.IsRoot Then
                If Me.UsesAJoint Then
                    If oXml.FindChildElement("Joint", False) Then
                        m_JointToParent = DirectCast(Util.Simulation.CreateObject(oXml, "Joint", Me), DataObjects.Physical.Joint)
                        m_JointToParent.LoadData(doStructure, oXml)
                    End If
                End If
            End If

            If oXml.FindChildElement("ChildBodies", False) Then
                Dim dbBody As DataObjects.Physical.RigidBody

                oXml.IntoElem() 'Into ChildBodies Element
                Dim iCount As Integer = oXml.NumberOfChildren() - 1
                For iIndex As Integer = 0 To iCount
                    oXml.FindChildByIndex(iIndex)

                    dbBody = DirectCast(Util.Simulation.CreateObject(oXml, "RigidBody", Me), DataObjects.Physical.RigidBody)
                    dbBody.LoadData(doStructure, oXml)
                    m_aryChildBodies.Add(dbBody.ID, dbBody, False)
                Next
                oXml.OutOfElem()   'Outof ChildBodies Element
            End If

            If oXml.FindChildElement("OdorSources", False) Then
                Dim doOdor As DataObjects.Physical.Odor

                oXml.IntoElem() 'Into ChildBodies Element
                Dim iCount As Integer = oXml.NumberOfChildren() - 1
                For iIndex As Integer = 0 To iCount
                    oXml.FindChildByIndex(iIndex)

                    doOdor = New Odor(Me)
                    doOdor.LoadData(oXml)

                    If Not doOdor.OdorType Is Nothing Then
                        m_aryOdorSources.Add(doOdor.ID, doOdor, False)
                    End If
                Next
                oXml.OutOfElem()   'Outof ChildBodies Element
            End If

            m_bFoodSource = oXml.GetChildBool("FoodSource", m_bFoodSource)

            If m_bFoodSource Then
                m_snFoodQuantity.LoadData(oXml, "FoodQuantity")
                m_snMaxFoodQuantity.LoadData(oXml, "MaxFoodQuantity")
                m_snFoodReplenishRate.LoadData(oXml, "FoodReplenishRate")
                m_snFoodEnergyContent.LoadData(oXml, "FoodEnergyContent")
            End If

            If oXml.FindChildElement("ReceptiveFieldSensor", False) Then
                Try
                    m_doReceptiveFieldSensor = New ContactSensor(Me)
                    m_doReceptiveFieldSensor.LoadData(oXml)
                Catch ex As ContactSensor.ContactSensorOrganismException
                    'If we get this type of exception then it mens that we are trying to add this rigid body to regular structure.
                    'In that case we want to just eat this error and not have a receptive field sensor.
                    m_doReceptiveFieldSensor = Nothing
                End Try
            End If

            oXml.OutOfElem() 'Outof RigidBody Element

        End Sub

        Public Overloads Overrides Sub SaveData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.SaveData(doStructure, oXml)

            oXml.IntoElem() 'Into Child Elemement

            oXml.AddChildElement("IsContactSensor", m_bContactSensor)
            oXml.AddChildElement("IsCollisionObject", m_bIsCollisionObject)

            m_svBuoyancyCenter.SaveData(oXml, "BuoyancyCenter")
            oXml.AddChildElement("BuoyancyScale", m_fltBuoyancyScale)
            m_svLinearDrag.SaveData(oXml, "LinearDrag")
            m_svAngularDrag.SaveData(oXml, "AngularDrag")
            m_snMaxHydroForce.SaveData(oXml, "MaxHydroForce")
            m_snMaxHydroTorque.SaveData(oXml, "MaxHydroTorque")
            oXml.AddChildElement("Magnus", m_fltMagnus)
            oXml.AddChildElement("EnableFluids", m_bEnableFluids)

            m_snDensity.SaveData(oXml, "Density")

            'If we are using density (IE: Vortex) and this body either has static children or is a static child then we 
            'need to get the estimated mass and save that out instead of the mass shown. The reason is that the mass for
            'static children for vortex is shown as 0 and added to the mass of the parent. If we try and convert this project
            'then it will load up in bullet incorrectly with a mass of 0, so we use the estimated masses.
            If Not Util.Application.Physics.UseMassForRigidBodyDefinitions AndAlso (Me.HasStaticJoint OrElse Me.HasStaticChild OrElse Me.Mass.ActualValue = 0) Then
                Dim snMass As ScaledNumber = DirectCast(m_snMass.Clone(m_snMass.Parent, False, Me), ScaledNumber)
                snMass.ActualValue = Me.SimInterface.GetDataValueImmediate("EstimatedMass")
                snMass.SaveData(oXml, "Mass")
            Else
                m_snMass.SaveData(oXml, "Mass")
            End If

            m_svCOM.SaveData(oXml, "COM")

            If Not m_thMaterialType Is Nothing AndAlso Not m_thMaterialType.MaterialType Is Nothing Then
                oXml.AddChildElement("MaterialTypeID", m_thMaterialType.MaterialType.ID)
            End If

            oXml.AddChildElement("Freeze", m_bFreeze)

            If Not m_JointToParent Is Nothing Then
                m_JointToParent.SaveData(doStructure, oXml)
            End If

            If m_aryChildBodies.Count > 0 Then
                oXml.AddChildElement("ChildBodies")
                oXml.IntoElem()

                Dim doChild As AnimatGUI.DataObjects.Physical.RigidBody
                For Each deEntry As DictionaryEntry In m_aryChildBodies
                    doChild = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Physical.RigidBody)
                    doChild.SaveData(doStructure, oXml)
                Next

                oXml.OutOfElem() 'Outof ChildBodies Element
            End If

            If m_aryOdorSources.Count > 0 Then
                oXml.AddChildElement("OdorSources")
                oXml.IntoElem()

                Dim doOdor As AnimatGUI.DataObjects.Physical.Odor
                For Each deEntry As DictionaryEntry In m_aryOdorSources
                    doOdor = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Physical.Odor)
                    doOdor.SaveData(oXml)
                Next

                oXml.OutOfElem() 'Outof ChildBodies Element
            End If

            oXml.AddChildElement("FoodSource", m_bFoodSource)
            If m_bFoodSource Then
                m_snFoodQuantity.SaveData(oXml, "FoodQuantity")
                m_snMaxFoodQuantity.SaveData(oXml, "MaxFoodQuantity")
                m_snFoodReplenishRate.SaveData(oXml, "FoodReplenishRate")
                m_snFoodEnergyContent.SaveData(oXml, "FoodEnergyContent")
            End If

            If Not m_doReceptiveFieldSensor Is Nothing Then
                m_doReceptiveFieldSensor.SaveData(oXml)
            End If

            oXml.OutOfElem() 'Outof BodyPart Element

        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

            oXml.IntoElem() 'Into Child Elemement

            oXml.AddChildElement("IsContactSensor", m_bContactSensor)
            oXml.AddChildElement("IsCollisionObject", m_bIsCollisionObject)

            m_svBuoyancyCenter.SaveSimulationXml(oXml, Me, "BuoyancyCenter")
            oXml.AddChildElement("BuoyancyScale", m_fltBuoyancyScale)
            m_svLinearDrag.SaveSimulationXml(oXml, Me, "LinearDrag")
            m_svAngularDrag.SaveSimulationXml(oXml, Me, "AngularDrag")
            m_snMaxHydroForce.SaveSimulationXml(oXml, Me, "MaxHydroForce")
            m_snMaxHydroTorque.SaveSimulationXml(oXml, Me, "MaxHydroTorque")
            oXml.AddChildElement("Magnus", m_fltMagnus)
            oXml.AddChildElement("EnableFluids", m_bEnableFluids)

            m_snDensity.SaveSimulationXml(oXml, Me, "Density")
            m_snMass.SaveSimulationXml(oXml, Me, "Mass")
            m_svCOM.SaveSimulationXml(oXml, Me, "COM")

            oXml.AddChildElement("Freeze", m_bFreeze)

            If Not m_thMaterialType Is Nothing AndAlso Not m_thMaterialType.MaterialType Is Nothing Then
                oXml.AddChildElement("MaterialTypeID", m_thMaterialType.MaterialType.ID)
            End If

            If Not m_JointToParent Is Nothing Then
                m_JointToParent.SaveSimulationXml(oXml)
            End If

            If m_aryChildBodies.Count > 0 Then
                oXml.AddChildElement("ChildBodies")
                oXml.IntoElem()

                Dim doChild As AnimatGUI.DataObjects.Physical.RigidBody
                For Each deEntry As DictionaryEntry In m_aryChildBodies
                    doChild = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Physical.RigidBody)
                    doChild.SaveSimulationXml(oXml)
                Next

                oXml.OutOfElem() 'Outof ChildBodies Element
            End If

            If m_aryOdorSources.Count > 0 Then
                oXml.AddChildElement("OdorSources")
                oXml.IntoElem()

                Dim doOdor As AnimatGUI.DataObjects.Physical.Odor
                For Each deEntry As DictionaryEntry In m_aryOdorSources
                    doOdor = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Physical.Odor)
                    doOdor.SaveSimulationXml(oXml)
                Next

                oXml.OutOfElem() 'Outof ChildBodies Element
            End If

            oXml.AddChildElement("FoodSource", m_bFoodSource)
            m_snFoodQuantity.SaveSimulationXml(oXml, Me, "FoodQuantity")
            m_snMaxFoodQuantity.SaveSimulationXml(oXml, Me, "MaxFoodQuantity")
            m_snFoodReplenishRate.SaveSimulationXml(oXml, Me, "FoodReplenishRate")
            m_snFoodEnergyContent.SaveSimulationXml(oXml, Me, "FoodEnergyContent")

            If Not m_doReceptiveFieldSensor Is Nothing Then
                m_doReceptiveFieldSensor.SaveSimulationXml(oXml, Me, "ReceptiveFieldSensor")
            End If

            oXml.OutOfElem() 'Outof BodyPart Element

        End Sub

        Public Overrides Sub SetDefaultSizes()
            MyBase.SetDefaultSizes()
            m_snDensity.ActualValue = 1 * Util.Environment.MassUnitValue
            Me.EnableFluids = Util.Environment.SimulateHydrodynamics
        End Sub

        Protected Overridable Sub OrientNewPart(ByVal vPos As Framework.Vec3d, ByVal vNorm As Framework.Vec3d)
            m_doInterface.OrientNewPart(vPos.X, vPos.Y, vPos.Z, vNorm.X, vNorm.Y, vNorm.Z)
        End Sub

        Protected Overridable Function CreateNewBody(ByVal vPos As Framework.Vec3d, ByVal vNorm As Framework.Vec3d, ByRef bAddDefaultGraphics As Boolean) As RigidBody
            Dim rbNew As RigidBody

            'First Select the new rigid body part type
            Dim frmSelectParts As New Forms.BodyPlan.SelectPartType()
            frmSelectParts.PartType = GetType(Physical.RigidBody)
            frmSelectParts.IsRoot = False
            frmSelectParts.ParentBody = Me

            If frmSelectParts.ShowDialog() <> DialogResult.OK Then Return Nothing

            rbNew = DirectCast(frmSelectParts.SelectedPart.Clone(Me, False, Nothing), RigidBody)
            rbNew.SetDefaultSizes()

            If rbNew.HasDynamics Then
                rbNew.IsCollisionObject = frmSelectParts.rdCollision.Checked
                If rbNew.IsCollisionObject Then
                    bAddDefaultGraphics = frmSelectParts.chkAddGraphics.Checked
                    rbNew.IsContactSensor = frmSelectParts.chkIsSensor.Checked
                End If
            Else
                rbNew.IsCollisionObject = False
                rbNew.IsContactSensor = False
            End If

            Me.ParentStructure.NewBodyIndex = Me.ParentStructure.NewBodyIndex + 1
            rbNew.Name = "Body_" & Me.ParentStructure.NewBodyIndex
            rbNew.IsRoot = False

            Return rbNew
        End Function

        Public Overrides Sub VerifyCanBePasted()
            If Not Me.JointToParent Is Nothing Then
                Me.JointToParent.VerifyCanBePasted()
            End If
        End Sub

        Public Overridable Sub VerifyCanAddChildren()
            If Me.IsContactSensor Then
                Throw New System.Exception("You cannot add children to a contact sensor class.")
            End If
        End Sub

        Public Overridable Overloads Function AddChildBody(ByVal vPos As Framework.Vec3d, ByVal vNorm As Framework.Vec3d, ByVal bDoNotOrient As Boolean) As Boolean
            Dim rbNew As RigidBody
            Dim bAddDefaultGraphics As Boolean = False
            Dim bPastedPart As Boolean = False

            Try
                Me.VerifyCanAddChildren()

                'If this is a part/joint combo that does not allow you to directly add child parts then we need to add the part to the parent.
                If Not Me.JointToParent Is Nothing AndAlso Not Me.JointToParent.AllowAddChildBody Then
                    If Not Me.Parent Is Nothing AndAlso Util.IsTypeOf(Me.Parent.GetType(), GetType(RigidBody)) Then
                        Dim rbParent As RigidBody = DirectCast(Me.Parent, RigidBody)
                        Return rbParent.AddChildBody(vPos, vNorm, bDoNotOrient)
                    Else
                        Throw New System.Exception("Unable to add the part type.")
                    End If
                End If

                rbNew = Util.GetPastedBodyPart(Me.ParentStructure, Me, False)

                If rbNew Is Nothing Then
                    rbNew = CreateNewBody(vPos, vNorm, bAddDefaultGraphics)
                    If rbNew Is Nothing Then Return False
                Else
                    bPastedPart = True
                End If

                'Now, if it needs a joint then select the joint type to use
                If rbNew.UsesAJoint Then
                    If Not rbNew.SelectJointType(Me, vPos, vNorm) Then
                        Return False
                    End If
                End If

                rbNew.VerifyCanBePasted()

                'Now add the new part to the parent
                AddChildBody(rbNew, bAddDefaultGraphics)

                rbNew.InitializeAfterLoad()
                rbNew.AddToSim(True)

                If (Not bDoNotOrient) Then
                    rbNew.OrientNewPart(vPos, vNorm)
                End If

                rbNew.SelectItem(False)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                If bPastedPart Then
                    Util.Application.ToggleBodyPartPasteInProgress()
                End If
            End Try
        End Function

        Public Overridable Overloads Sub AddChildBody(ByVal rbChildBody As AnimatGUI.DataObjects.Physical.RigidBody, ByVal bAddDefaultGraphics As Boolean)

            rbChildBody.IsRoot = False

            If Not Me.ChildBodies.Contains(rbChildBody.ID) Then
                rbChildBody.BeforeAddBody()

                Me.ChildBodies.Add(rbChildBody.ID, rbChildBody, False)

                'If this is  a collision objectthen we need to add a default graphics object for this item.
                If bAddDefaultGraphics AndAlso rbChildBody.IsCollisionObject Then
                    rbChildBody.CreateDefaultGraphicsObject()
                ElseIf Not bAddDefaultGraphics AndAlso rbChildBody.IsCollisionObject Then
                    'If it is a collision object and no default graphic is added then we want to set its
                    'simulation transparency to 0 so it is visible in the sim.
                    rbChildBody.Transparencies.SimulationTransparency = 0
                End If

            End If

            Dim doStruct As AnimatGUI.DataObjects.Physical.PhysicalStructure = Me.ParentStructure
            If Not rbChildBody Is Nothing AndAlso Not m_tnWorkspaceNode Is Nothing Then
                rbChildBody.CreateWorkspaceTreeView(Me, m_tnWorkspaceNode)
            End If

            Util.Application.AddPartToolStripButton.Checked = False

            rbChildBody.AfterAddBody()

            'Me.ManualAddHistory(New AnimatGUI.Framework.UndoSystem.AddBodyPartEvent(doStruct.BodyEditor, doStruct, Me, rbChildBody))
        End Sub

        Protected Overridable Function SelectJointType(ByVal doParent As RigidBody, ByVal vPos As Framework.Vec3d, ByVal vNorm As Framework.Vec3d) As Boolean

            'First Select the new rigid body part type
            Dim frmSelectParts As New Forms.BodyPlan.SelectPartType()
            frmSelectParts.PartType = GetType(Physical.Joint)
            frmSelectParts.IsRoot = False
            frmSelectParts.ParentBody = Me
            frmSelectParts.ChildBody = doParent

            If frmSelectParts.ShowDialog() <> DialogResult.OK Then Return False

            m_JointToParent = DirectCast(frmSelectParts.SelectedPart.Clone(Me, False, Nothing), Joint)
            m_JointToParent.SetDefaultSizes()

            Me.ParentStructure.NewJointIndex = Me.ParentStructure.NewJointIndex + 1
            m_JointToParent.Name = "Joint_" & Me.ParentStructure.NewJointIndex

            'm_JointToParent.LocalPosition.CopyData(CSng(vPos.X), CSng(vPos.Y), CSng(vPos.Z))
            m_JointToParent.LocalPosition.CopyData(0, 0.1, 0)

            Return True
        End Function

        Public Overridable Sub GetChildPartsList(ByVal aryParts As AnimatGUI.Collections.SortedBodyParts)

            aryParts.Add(Me.ID, Me)

            If Not m_JointToParent Is Nothing Then
                aryParts.Add(m_JointToParent.ID, m_JointToParent)
            End If

            Dim doChild As AnimatGUI.DataObjects.Physical.RigidBody
            For Each deEntry As DictionaryEntry In m_aryChildBodies
                doChild = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Physical.RigidBody)
                doChild.GetChildPartsList(aryParts)
            Next

        End Sub

        Public Overridable Sub GetChildBodiesList(ByVal aryBodies As AnimatGUI.Collections.SortedRigidBodies)

            aryBodies.Add(Me.ID, Me)

            Dim doChild As AnimatGUI.DataObjects.Physical.RigidBody
            For Each deEntry As DictionaryEntry In m_aryChildBodies
                doChild = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Physical.RigidBody)
                doChild.GetChildBodiesList(aryBodies)
            Next

        End Sub

        Public Overridable Sub GetChildJointsList(ByVal aryJoints As AnimatGUI.Collections.SortedJoints)

            If Not m_JointToParent Is Nothing Then
                aryJoints.Add(m_JointToParent.ID, m_JointToParent)
            End If

            Dim doChild As AnimatGUI.DataObjects.Physical.RigidBody
            For Each deEntry As DictionaryEntry In m_aryChildBodies
                doChild = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Physical.RigidBody)
                doChild.GetChildJointsList(aryJoints)
            Next

        End Sub

        Public Overrides Sub UnitsChanged(ByVal ePrevMass As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits, _
                                          ByVal eNewMass As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits, _
                                          ByVal fltMassChange As Single, _
                                          ByVal ePrevDistance As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits, _
                                          ByVal eNewDistance As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits, _
                                          ByVal fltDistanceChange As Single)

            Dim iDistDiff As Integer = CInt(Util.Environment.DisplayDistanceUnits) - CInt(Util.Environment.DisplayDistanceUnits(ePrevDistance))

            If Not Util.Application.Physics.UseMassForRigidBodyDefinitions Then
                Dim fltDensityDistChange As Single = CSng(10 ^ iDistDiff)

                Dim fltValue As Double = (m_snDensity.ActualValue / Math.Pow(10, CInt(ePrevMass))) * (Math.Pow(fltDensityDistChange, 3) / fltMassChange)
                Dim eSCale As ScaledNumber.enumNumericScale = CType(Util.Environment.MassUnits, ScaledNumber.enumNumericScale)
                Dim strUnits As String = "g/" & Util.Environment.DistanceUnitAbbreviation(Util.Environment.DisplayDistanceUnits) & "^3"
                Me.Density = New ScaledNumber(Me, "Density", fltValue, eSCale, strUnits, strUnits)
            End If

            If Not m_doReceptiveFieldSensor Is Nothing Then
                m_doReceptiveFieldSensor.UnitsChanged(ePrevMass, eNewMass, fltMassChange, ePrevDistance, eNewDistance, fltDistanceChange)
            End If

            If Not m_JointToParent Is Nothing Then
                m_JointToParent.UnitsChanged(ePrevMass, eNewMass, fltMassChange, ePrevDistance, eNewDistance, fltDistanceChange)
            End If

            Dim doChild As RigidBody
            For Each deEntry As DictionaryEntry In m_aryChildBodies
                doChild = DirectCast(deEntry.Value, RigidBody)
                doChild.UnitsChanged(ePrevMass, eNewMass, fltMassChange, ePrevDistance, eNewDistance, fltDistanceChange)
            Next
        End Sub

        Public Overridable Sub CreateDefaultGraphicsObject()

            Dim doGraphics As RigidBody = DirectCast(Me.Clone(Me, False, Me), Physical.RigidBody)
            doGraphics.SetDefaultSizes()
            doGraphics.m_JointToParent = Nothing
            doGraphics.IsCollisionObject = False
            doGraphics.IsContactSensor = False
            doGraphics.IsRoot = False
            doGraphics.Name = doGraphics.Name & "_Graphics"

            'The graphics object is always created direclty atop the collision object
            doGraphics.LocalPosition.CopyData(0, 0, 0)
            doGraphics.Rotation.CopyData(0, 0, 0)

            doGraphics.Diffuse = Drawing.Color.Blue

            Me.AddChildBody(doGraphics, False)

        End Sub

        Public Overridable Sub ResetEnableFluidsForRigidBodies()
            Me.EnableFluids = Util.Environment.SimulateHydrodynamics

            Dim doChild As RigidBody
            For Each deEntry As DictionaryEntry In m_aryChildBodies
                doChild = DirectCast(deEntry.Value, RigidBody)
                doChild.ResetEnableFluidsForRigidBodies()
            Next
        End Sub

        Public Overridable Sub AddReceptiveFieldSensor()

            If Not m_doReceptiveFieldSensor Is Nothing Then
                Throw New System.Exception("A receptive field sensor is already defined.")
            End If

            m_doReceptiveFieldSensor = New ContactSensor(Me)

            'Make sure it is added to the simulation
            m_doReceptiveFieldSensor.BeforeAddToList(True, True)
            m_doReceptiveFieldSensor.AfterAddToList(True, True)

            RaiseEvent ContactSensorAdded()

        End Sub

        Public Overridable Sub RemoveReceptiveFieldSensorIfNeeded()

            If Not m_doReceptiveFieldSensor Is Nothing AndAlso m_doReceptiveFieldSensor.Fields.Count = 0 AndAlso _
                m_doReceptiveFieldSensor.FieldPairs.Count = 0 AndAlso m_doReceptiveFieldSensor.Adapters.Count = 0 Then
                RemoveReceptiveFieldSensor()
            End If

        End Sub

        Protected Overridable Sub RemoveReceptiveFieldSensor()

            m_doReceptiveFieldSensor.BeforeRemoveFromList(True, True)
            m_doReceptiveFieldSensor.AfterRemoveFromList(True, True)
            m_doReceptiveFieldSensor = Nothing

            RaiseEvent ContactSensorRemoved()

        End Sub

        Public Overrides Sub RaiseMovedEvent()
            MyBase.RaiseMovedEvent()

            If Not Me.JointToParent Is Nothing Then
                m_JointToParent.RaiseMovedEvent()
            End If

            For Each deEntry As DictionaryEntry In Me.ChildBodies
                Dim doBody As RigidBody = DirectCast(deEntry.Value, RigidBody)
                doBody.RaiseMovedEvent()
            Next

        End Sub

        Public Overridable Function ResetReceptiveFieldsAfterPropChange(ByVal propInfo As Reflection.PropertyInfo) As Boolean
            Return False
        End Function

#Region " Add-Remove to List Methods "

        Public Overrides Sub AddToSim(ByVal bThrowError As Boolean, Optional ByVal bDoNotInit As Boolean = False)
            If Not Me.Parent Is Nothing Then
                Util.Application.SimulationInterface.AddItem(Me.Parent.ID, "RigidBody", Me.ID, Me.GetSimulationXml("RigidBody"), bThrowError, bDoNotInit)
                InitializeSimulationReferences()
            End If
        End Sub

        Public Overrides Sub BeforeAddToList(ByVal bCallSimMethods As Boolean, ByVal bThrowError As Boolean)

            'Verify that this part can be added to the parent 
            If Not Me.IsRoot Then
                If Not Me.Parent Is Nothing AndAlso Util.IsTypeOf(Me.Parent.GetType(), GetType(RigidBody)) Then
                    Dim doParent As RigidBody = DirectCast(Me.Parent, RigidBody)
                    If Not Util.Application.CanAddPartAsChild(doParent.GetType, Me.GetType) Then
                        Throw New System.Exception("You attempted to add a part to a type that does not support it as a child object.")
                    End If
                Else
                    Throw New System.Exception("Could not convert parent part to rigid body")
                End If
            End If

            Me.SignalBeforeAddItem(Me)
            If bCallSimMethods Then AddToSim(bThrowError)

            If Not m_JointToParent Is Nothing Then
                m_JointToParent.BeforeAddToList(bCallSimMethods, bThrowError)
            End If

            If Not m_doReceptiveFieldSensor Is Nothing Then
                m_doReceptiveFieldSensor.BeforeAddToList(bCallSimMethods, bThrowError)
            End If

            Dim doChild As RigidBody
            For Each deEntry As DictionaryEntry In m_aryChildBodies
                doChild = DirectCast(deEntry.Value, RigidBody)
                doChild.BeforeAddToList(bCallSimMethods, bThrowError)
            Next

            Dim doOdor As Odor
            For Each deEntry As DictionaryEntry In m_aryOdorSources
                doOdor = DirectCast(deEntry.Value, Odor)
                doOdor.BeforeAddToList(bCallSimMethods, bThrowError)
            Next

        End Sub

        Public Overrides Sub AfterAddToList(ByVal bCallSimMethods As Boolean, ByVal bThrowError As Boolean)
            MyBase.AfterAddToList(bCallSimMethods, bThrowError)

            If Not m_JointToParent Is Nothing Then
                m_JointToParent.AfterAddToList(bCallSimMethods, bThrowError)
            End If

            If Not m_doReceptiveFieldSensor Is Nothing Then
                m_doReceptiveFieldSensor.AfterAddToList(bCallSimMethods, bThrowError)
            End If

            Dim doChild As RigidBody
            For Each deEntry As DictionaryEntry In m_aryChildBodies
                doChild = DirectCast(deEntry.Value, RigidBody)
                doChild.AfterAddToList(bCallSimMethods, bThrowError)
            Next

            Dim doOdor As Odor
            For Each deEntry As DictionaryEntry In m_aryOdorSources
                doOdor = DirectCast(deEntry.Value, Odor)
                doOdor.AfterAddToList(bCallSimMethods, bThrowError)
            Next

        End Sub

        Public Overrides Sub RemoveFromSim(ByVal bThrowError As Boolean)
            If Not Me.Parent Is Nothing AndAlso Not m_doInterface Is Nothing Then
                Util.Application.SimulationInterface.RemoveItem(Me.Parent.ID(), "RigidBody", Me.ID, bThrowError)
            End If
            m_doInterface = Nothing
        End Sub

        Public Overrides Sub BeforeRemoveFromList(ByVal bCallSimMethods As Boolean, ByVal bThrowError As Boolean)
            Me.SignalBeforeRemoveItem(Me)

            If Not m_JointToParent Is Nothing Then
                m_JointToParent.BeforeRemoveFromList(bCallSimMethods, bThrowError)
            End If

            If Not m_doReceptiveFieldSensor Is Nothing Then
                m_doReceptiveFieldSensor.BeforeRemoveFromList(bCallSimMethods, bThrowError)
            End If

            Dim doChild As RigidBody
            For Each deEntry As DictionaryEntry In m_aryChildBodies
                doChild = DirectCast(deEntry.Value, RigidBody)
                doChild.BeforeRemoveFromList(bCallSimMethods, bThrowError)
            Next

            Dim doOdor As Odor
            For Each deEntry As DictionaryEntry In m_aryOdorSources
                doOdor = DirectCast(deEntry.Value, Odor)
                doOdor.BeforeRemoveFromList(bCallSimMethods, bThrowError)
            Next

            If bCallSimMethods Then RemoveFromSim(bThrowError)
        End Sub

        Public Overrides Sub AfterRemoveFromList(ByVal bCallSimMethods As Boolean, ByVal bThrowError As Boolean)
            MyBase.AfterRemoveFromList(bCallSimMethods, bThrowError)

            If Not m_JointToParent Is Nothing Then
                m_JointToParent.AfterRemoveFromList(bCallSimMethods, bThrowError)
            End If

            If Not m_doReceptiveFieldSensor Is Nothing Then
                m_doReceptiveFieldSensor.AfterRemoveFromList(bCallSimMethods, bThrowError)
            End If

            Dim doChild As RigidBody
            For Each deEntry As DictionaryEntry In m_aryChildBodies
                doChild = DirectCast(deEntry.Value, RigidBody)
                doChild.AfterRemoveFromList(bCallSimMethods, bThrowError)
            Next

            Dim doOdor As Odor
            For Each deEntry As DictionaryEntry In m_aryOdorSources
                doOdor = DirectCast(deEntry.Value, Odor)
                doOdor.AfterRemoveFromList(bCallSimMethods, bThrowError)
            Next
        End Sub

#End Region

#End Region

#Region " Events "

        Public Event ContactSensorAdded()
        Public Event ContactSensorRemoved()

        'These three events handlers are called whenever a user manually changes the value of the COM, Buoyancycenter or drag.
        Protected Overridable Sub OnCOMValueChanged()
            Try
                Me.SetSimData("COM", m_svCOM.GetSimulationXml("COM"), True)
                Util.ProjectProperties.RefreshProperties()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub OnBuoyancyCenterValueChanged()
            Try
                Me.SetSimData("BuoyancyCenter", m_svBuoyancyCenter.GetSimulationXml("BuoyancyCenter"), True)
                Util.ProjectProperties.RefreshProperties()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub OnLinearDragValueChanged()
            Try
                Me.SetSimData("LinearDrag", m_svLinearDrag.GetSimulationXml("LinearDrag"), True)
                Util.ProjectProperties.RefreshProperties()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub OnAngularDragValueChanged()
            Try
                Me.SetSimData("AngularDrag", m_svAngularDrag.GetSimulationXml("AngularDrag"), True)
                Util.ProjectProperties.RefreshProperties()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#Region " DataObjectInterface Events "


        Protected Overridable Sub OnAddChildBody(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                Dim vPos As New Framework.Vec3d(Me, Me.WorldPosition.X.ActualValue, Me.WorldPosition.Y.ActualValue, Me.WorldPosition.Z.ActualValue)
                Dim vNorm As New Framework.Vec3d(Me, 1, 0, 0)

                Dim aryObjs(2) As Object
                aryObjs(0) = vPos
                aryObjs(1) = vNorm
                aryObjs(2) = True
                Util.Application.BeginInvoke(New AddChildBodyDelegate(AddressOf Me.AddChildBody), aryObjs)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        'All events coming up from the DataObjectInterface are actually coming from a different thread.
        'The one for the simulation. This means that we have to use BeginInvoke to recall a different 
        'method on the GUI thread or it will cause big problems. So all of these methods do that.

        Protected Overridable Sub OnAddBodyClicked(ByVal fltPosX As Single, ByVal fltPosY As Single, ByVal fltPosZ As Single, _
                                                   ByVal fltNormX As Single, ByVal fltNormY As Single, ByVal fltNormZ As Single)

            Try
                Dim vPos As New Framework.Vec3d(Me, fltPosX, fltPosY, fltPosZ)
                Dim vNorm As New Framework.Vec3d(Me, fltNormX, fltNormY, fltNormZ)

                Dim aryObjs(2) As Object
                aryObjs(0) = vPos
                aryObjs(1) = vNorm
                aryObjs(2) = False
                Util.Application.BeginInvoke(New AddChildBodyDelegate(AddressOf Me.AddChildBody), aryObjs)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub OnSelectedVertexChanged(ByVal fltX As Single, ByVal fltY As Single, ByVal fltZ As Single)
            Try
                m_vSelectedVertex = New Vec3d(Me, fltX, fltY, fltZ)
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub OnReplaceMaterial(doReplacement As Physical.MaterialType)
            Try
                Me.MaterialType = New TypeHelpers.LinkedMaterialType(Me, doReplacement)
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Public Overridable Sub Automation_AddBodyClicked(ByVal fltPosX As Single, ByVal fltPosY As Single, ByVal fltPosZ As Single, _
                                                   ByVal fltNormX As Single, ByVal fltNormY As Single, ByVal fltNormZ As Single)
            OnAddBodyClicked(fltPosX, fltPosY, fltPosZ, fltNormX, fltNormY, fltNormZ)
        End Sub

        Public Overridable Sub Automation_SelectedVertexChanged(ByVal fltX As Single, ByVal fltY As Single, ByVal fltZ As Single)
            OnSelectedVertexChanged(fltX, fltY, fltZ)
        End Sub

#End Region

#End Region


    End Class

End Namespace

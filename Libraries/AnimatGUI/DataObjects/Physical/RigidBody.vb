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
Imports Crownwood.Magic.Common
Imports Crownwood.Magic.Docking
Imports Crownwood.Magic.Menus
Imports AnimatGUI.Framework

Namespace DataObjects.Physical

    Public MustInherit Class RigidBody
        Inherits Physical.BodyPart

#Region " Delegates "

        Delegate Function AddChildBodyDelegate(ByVal vPos As Framework.Vec3d, ByVal vNorm As Framework.Vec3d) As Boolean

#End Region

#Region " Attributes "

        Protected m_JointToParent As DataObjects.Physical.Joint
        Protected m_aryChildBodies As New Collections.SortedRigidBodies(Me)

        Protected m_bFreeze As Boolean = False
        Protected m_bContactSensor As Boolean = False
        Protected m_bIsCollisionObject As Boolean = False
        Protected m_snDensity As ScaledNumber

        Protected m_svCOM As ScaledVector3

        Protected m_svBuoyancyCenter As ScaledVector3
        Protected m_fltBuoyancyScale As Single = 1
        Protected m_svDrag As ScaledVector3
        Protected m_fltMagnus As Single = 0
        Protected m_bEnableFluids As Boolean = False

        Protected m_snReceptiveFieldDistance As ScaledNumber
        Protected m_gnReceptiveFieldGain As Gain
        Protected m_gnReceptiveCurrentGain As Gain
        Protected m_aryReceptiveFields As New ArrayList
        Protected m_aryReceptiveFieldPairs As New Collections.AnimatSortedList(Me)
        Protected m_vSelectedReceptiveField As Vec3d    'The vertex of the selected receptive field.

        Protected m_aryOdorSources As New Collections.SortedOdors(Me)

        Protected m_bFoodSource As Boolean = False
        Protected m_snFoodQuantity As ScaledNumber
        Protected m_snFoodReplenishRate As ScaledNumber
        Protected m_snFoodEnergyContent As ScaledNumber
        Protected m_snMaxFoodQuantity As ScaledNumber

        Protected m_bIsRoot As Boolean = False

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
                If m_JointToParent Is Nothing Then
                    Me.SetSimData("Freeze", Value.ToString, True)
                    m_bFreeze = Value
                Else
                    Throw New System.Exception("You can only freeze the root body of a structure.")
                End If
            End Set
        End Property

        Public Overridable Property ContactSensor() As Boolean
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

                Me.SetSimData("Density", Value.ToString, True)
                m_snDensity.CopyData(Value)
            End Set
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

        Public Overridable Property Drag() As Framework.ScaledVector3
            Get
                Return m_svDrag
            End Get
            Set(ByVal value As Framework.ScaledVector3)
                Me.SetSimData("Drag", value.GetSimulationXml("Drag"), True)
                m_svDrag.CopyData(value)
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

        Public Overridable Property ReceptiveFieldDistance() As ScaledNumber
            Get
                Return m_snReceptiveFieldDistance
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue <= 0 Then
                    Throw New System.Exception("The receptive field distance can not be less than or equal to zero.")
                End If

                m_snReceptiveFieldDistance.CopyData(Value)
            End Set
        End Property

        Public Overridable Property ReceptiveFieldGain() As Gain
            Get
                Return m_gnReceptiveFieldGain
            End Get
            Set(ByVal Value As Gain)
                If Not Value Is Nothing Then
                    m_gnReceptiveFieldGain = Value
                Else
                    Throw New System.Exception("You can not set the receptive field gain to null.")
                End If
            End Set
        End Property

        Public Overridable Property ReceptiveCurrentGain() As Gain
            Get
                Return m_gnReceptiveCurrentGain
            End Get
            Set(ByVal Value As Gain)
                If Not Value Is Nothing Then
                    m_gnReceptiveCurrentGain = Value
                Else
                    Throw New System.Exception("You can not set the receptive current gain to null.")
                End If
            End Set
        End Property

        Public Overridable ReadOnly Property ReceptiveFields() As ArrayList
            Get
                Return m_aryReceptiveFields
            End Get
        End Property

        Public Overridable ReadOnly Property ReceptiveFieldPairs() As Collections.AnimatSortedList
            Get
                Return m_aryReceptiveFieldPairs
            End Get
        End Property

        Public Overridable Property SelectedReceptiveField() As Vec3d
            Get
                Return m_vSelectedReceptiveField
            End Get
            Set(ByVal Value As Vec3d)
                m_vSelectedReceptiveField = Value
            End Set
        End Property

        Public Overridable ReadOnly Property CanBeRootBody() As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overridable ReadOnly Property UsesAJoint() As Boolean
            Get
                If Me.IsCollisionObject AndAlso Not Me.ContactSensor Then
                    Return True
                Else
                    Return False
                End If
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

                Util.ProjectProperties.RefreshProperties()
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

        ''If this a rigid body then we do not want to allow the user to be able to change the position or orientation
        ''of the body. They need to do this using the structure/organism.
        '<Browsable(False)> _
        'Public Overrides ReadOnly Property AllowGuiCoordinateChange() As Boolean
        '    Get
        '        If Me.IsRoot Then
        '            Return False
        '        Else
        '            Return True
        '        End If
        '    End Get
        'End Property

        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Rigid Body"
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

            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("BodyDragForceX", "Drag Force X Axis", "Newtons", "N", -100, 100))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("BodyDragForceY", "Drag Force Y Axis", "Newtons", "N", -100, 100))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("BodyDragForceZ", "Drag Force Z Axis", "Newtons", "N", -100, 100))

            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("BodyDragTorqueX", "Drag Torque X Axis", "Newton-Meters", "Nm", -100, 100))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("BodyDragTorqueY", "Drag Torque Y Axis", "Newton-Meters", "Nm", -100, 100))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("BodyDragTorqueZ", "Drag Torque Z Axis", "Newton-Meters", "Nm", -100, 100))

            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("BodyAddedMassForceX", "Added Mass Force X Axis", "Newtons", "N", -100, 100))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("BodyAddedMassForceY", "Added Mass Force Y Axis", "Newtons", "N", -100, 100))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("BodyAddedMassForceZ", "Added Mass Force Z Axis", "Newtons", "N", -100, 100))

            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("BodyAddedMassTorqueX", "Added Mass Torque X Axis", "Newton-Meters", "Nm", -100, 100))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("BodyAddedMassTorqueY", "Added Mass Torque Y Axis", "Newton-Meters", "Nm", -100, 100))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("BodyAddedMassTorqueZ", "Added Mass Torque Z Axis", "Newton-Meters", "Nm", -100, 100))

            'm_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("BodyBuoyancy", "Buoyancy", "Newtons", "N", 0, 100))

            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("FoodQuantity", "Food Quantity", "", "", 0, 1000))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("FoodEaten", "Food Eaten", "", "", 0, 1000))

            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("Mass", "Mass", "Kilograms", "Kg", -5000, 5000))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("Volume", "Volume", "Cubic Meters", "m^3", -100, 100))

            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("ContactCount", "Contact Count", "", "", 0, 1))

            m_thDataTypes.ID = "BodyForceX"

            m_thIncomingDataType = New AnimatGUI.DataObjects.DataType("BodyForceX", "Body Force X", "Newtons", "N", -100, 100, ScaledNumber.enumNumericScale.None, ScaledNumber.enumNumericScale.None)

            m_snReceptiveFieldDistance = New ScaledNumber(Me, "RecptiveFieldDistance", 25, ScaledNumber.enumNumericScale.centi, "Meters", "m")

            m_svCOM = New ScaledVector3(Me, "COM", "Location of the COM relative to the (0,0,0) point of this part.", "Meters", "m")
            m_svBuoyancyCenter = New ScaledVector3(Me, "BuoyancyCenter", "Location of the center of buoyancy relative to the (0,0,0) point of this part.", "Meters", "m")
            m_svDrag = New ScaledVector3(Me, "Drag", "Drag coefficients of this part.", "", "")
            m_svDrag.CopyData(1, 1, 1, True)

            AddHandler m_svCOM.ValueChanged, AddressOf Me.OnCOMValueChanged
            AddHandler m_svBuoyancyCenter.ValueChanged, AddressOf Me.OnBuoyancyCenterValueChanged
            AddHandler m_svDrag.ValueChanged, AddressOf Me.OnDragValueChanged

            If Not Util.Environment Is Nothing Then
                m_snDensity = Util.Environment.DefaultDensity
            Else
                m_snDensity = New ScaledNumber(Me, "Density", 1, ScaledNumber.enumNumericScale.Kilo, "g/m^3", "g/m^3")
            End If

            m_snFoodQuantity = New ScaledNumber(Me, "FoodQuantity", 100, ScaledNumber.enumNumericScale.None, "", "")
            m_snMaxFoodQuantity = New ScaledNumber(Me, "MaxFoodQuantity", 10, ScaledNumber.enumNumericScale.Kilo, "", "")
            m_snFoodReplenishRate = New ScaledNumber(Me, "FoodReplenishRate", 1, ScaledNumber.enumNumericScale.None, "Quantity/s", "Q/s")
            m_snFoodEnergyContent = New ScaledNumber(Me, "FoodEnergyContent", 1, ScaledNumber.enumNumericScale.Kilo, "Calories/Quantity", "C/Q")

            Dim gnRFGain As New Gains.Bell(Me, "ReceptiveFieldGain", "Meters", "Gain")
            gnRFGain.XOffset.ActualValue = 0
            gnRFGain.Amplitude.ActualValue = 1
            gnRFGain.Width.ActualValue = 10
            gnRFGain.YOffset.ActualValue = 0
            gnRFGain.LowerLimit.ActualValue = -1
            gnRFGain.UpperLimit.ActualValue = 1
            m_gnReceptiveFieldGain = gnRFGain

            Dim gnRCGain As New Gains.Polynomial(Me, "ReceptiveCurrnetGain", "Force", "Current")
            gnRCGain.A.ActualValue = 0
            gnRCGain.B.ActualValue = 0
            gnRCGain.C.ActualValue = 0.000000001
            gnRCGain.D.ActualValue = 0
            gnRCGain.UseLimits = True
            gnRCGain.LowerLimit.ActualValue = 0
            gnRCGain.UpperLimit.ActualValue = 10
            gnRCGain.LowerOutput.ActualValue = 0
            gnRCGain.UpperOutput.SetFromValue(0.00000007, ScaledNumber.enumNumericScale.nano)
            m_gnReceptiveCurrentGain = gnRCGain

            m_vSelectedReceptiveField = New AnimatGUI.Framework.Vec3d(Nothing, 0, 0, 0)

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
            m_svDrag.ClearIsDirty()
            m_svCOM.ClearIsDirty()
            m_snReceptiveFieldDistance.ClearIsDirty()
            m_vSelectedReceptiveField.ClearIsDirty()
            m_snDensity.ClearIsDirty()
            m_snFoodQuantity.ClearIsDirty()
            m_snMaxFoodQuantity.ClearIsDirty()
            m_snFoodReplenishRate.ClearIsDirty()
            m_snFoodEnergyContent.ClearIsDirty()
            m_gnReceptiveFieldGain.ClearIsDirty()
            m_gnReceptiveCurrentGain.ClearIsDirty()
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
            If doObject Is Nothing AndAlso Not m_gnReceptiveFieldGain Is Nothing Then doObject = m_gnReceptiveFieldGain.FindObjectByID(strID)
            If doObject Is Nothing AndAlso Not m_gnReceptiveCurrentGain Is Nothing Then doObject = m_gnReceptiveCurrentGain.FindObjectByID(strID)
            If doObject Is Nothing AndAlso Not m_aryReceptiveFieldPairs Is Nothing Then doObject = m_aryReceptiveFieldPairs.FindObjectByID(strID)
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

        Public Overrides Sub CreateWorkspaceTreeView(ByVal doParent As Framework.DataObject, ByVal doParentNode As Crownwood.DotNetMagic.Controls.Node)

            Dim tnParent As Crownwood.DotNetMagic.Controls.Node = doParentNode
            If Not m_JointToParent Is Nothing Then
                m_JointToParent.CreateWorkspaceTreeView(Me, doParentNode)
                tnParent = m_JointToParent.WorkspaceNode
            End If

            MyBase.CreateWorkspaceTreeView(doParent, tnParent)

            Dim dbChild As AnimatGUI.DataObjects.Physical.RigidBody
            For Each deEntry As DictionaryEntry In m_aryChildBodies
                dbChild = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Physical.RigidBody)
                dbChild.CreateWorkspaceTreeView(Me, m_tnWorkspaceNode)
            Next

        End Sub

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
                Return MyBase.WorkspaceTreeviewPopupMenu(tnSelectedNode, ptPoint)
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

                    pbNumberBag = m_svDrag.Properties
                    propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Drag", pbNumberBag.GetType(), "Drag", _
                                                "Hydrodynamics", "This is the drag coefficients for the three axis for the body.", pbNumberBag, _
                                                "", GetType(AnimatGUI.Framework.ScaledVector3.ScaledVector3PropBagConverter)))

                    propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Magnus", m_fltMagnus.GetType(), "Magnus", _
                                     "Hydrodynamics", "The Magnus coefficient for the body.", m_fltMagnus))

                    propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Enable Fluids", m_bEnableFluids.GetType(), "EnableFluids", _
                                     "Hydrodynamics", "Enables fluid interactions for this specific body.", m_bEnableFluids))
                End If

                pbNumberBag = m_snDensity.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Density", pbNumberBag.GetType(), "Density", _
                                            "Part Properties", "Sets the density of this body part.", pbNumberBag, _
                                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

                'Center Of Mass
                pbNumberBag = Me.COM.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Center of Mass", pbNumberBag.GetType(), "COM", _
                                            "Coordinates", "Sets the COM of this body part relative to the center of the part.", pbNumberBag, _
                                            "", GetType(AnimatGUI.Framework.ScaledVector3.ScaledVector3PropBagConverter)))

                'm_JointToParent Is Nothing
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Freeze", m_bFreeze.GetType(), "Freeze", _
                                                "Part Properties", "If the root body is frozen then it is locked in place in the environment.", m_bFreeze))

            Else
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Contact Sensor", m_bContactSensor.GetType(), "ContactSensor", _
                                            "Part Properties", "Sets whether or not this part can detect contacts.", m_bContactSensor))
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
                                        "Visibility", "Sets the bmp texture file to wrap onto this body part.", Me.Texture, GetType(TypeHelpers.ImageFileEditor))) 'GetType(System.Windows.Forms.Design.FileNameEditor)))


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

            m_bFreeze = False   '' only the root object can be frozen.
            m_bContactSensor = doOrigPart.m_bContactSensor
            m_bIsCollisionObject = doOrigPart.m_bIsCollisionObject
            m_snDensity = DirectCast(doOrigPart.m_snDensity, ScaledNumber)
            m_snReceptiveFieldDistance = DirectCast(doOrigPart.m_snReceptiveFieldDistance, ScaledNumber)

            m_svBuoyancyCenter = DirectCast(doOrigPart.m_svBuoyancyCenter.Clone(Me, bCutData, doRoot), ScaledVector3)
            m_fltBuoyancyScale = doOrigPart.m_fltBuoyancyScale
            m_svDrag = DirectCast(doOrigPart.m_svDrag.Clone(Me, bCutData, doRoot), ScaledVector3)
            m_fltMagnus = doOrigPart.m_fltMagnus
            m_bEnableFluids = doOrigPart.m_bEnableFluids

            m_bIsRoot = doOrigPart.m_bIsRoot
            m_bFoodSource = doOrigPart.m_bFoodSource
            m_snFoodQuantity = DirectCast(doOrigPart.m_snFoodQuantity, ScaledNumber)
            m_snMaxFoodQuantity = DirectCast(doOrigPart.m_snMaxFoodQuantity, ScaledNumber)
            m_snFoodReplenishRate = DirectCast(doOrigPart.m_snFoodReplenishRate, ScaledNumber)
            m_snFoodEnergyContent = DirectCast(doOrigPart.m_snFoodEnergyContent, ScaledNumber)
            m_svCOM = DirectCast(doOrigPart.m_svCOM.Clone(Me, bCutData, doRoot), ScaledVector3)

            m_aryChildBodies.Clear()

            Dim doOrigChild As RigidBody
            Dim doNewChild As RigidBody
            For Each deItem As DictionaryEntry In doOrigPart.m_aryChildBodies
                doOrigChild = DirectCast(deItem.Value, RigidBody)
                doNewChild = DirectCast(doOrigChild.Clone(Me, bCutData, doRoot), RigidBody)
                m_aryChildBodies.Add(doNewChild.ID, doNewChild)
            Next

            m_aryOdorSources = DirectCast(doOrigPart.m_aryOdorSources.Clone(Me, bCutData, doRoot), Collections.SortedOdors)

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
            Me.Ambient = doExisting.Ambient
            Me.Diffuse = doExisting.Diffuse
            Me.Specular = doExisting.Specular
            Me.Shininess = doExisting.Shininess
            Me.m_svBuoyancyCenter = doExisting.m_svBuoyancyCenter
            Me.m_fltBuoyancyScale = doExisting.m_fltBuoyancyScale
            Me.m_svDrag = doExisting.m_svDrag
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
            Me.ReceptiveCurrentGain = doExisting.ReceptiveCurrentGain
            Me.ReceptiveFieldDistance = doExisting.ReceptiveFieldDistance
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

            Dim doPair As ReceptiveFieldPair
            For Each deEntry As DictionaryEntry In m_aryReceptiveFieldPairs
                doPair = DirectCast(deEntry.Value, ReceptiveFieldPair)
                doPair.InitializeAfterLoad()
            Next

        End Sub

        Public Overrides Sub InitializeSimulationReferences()
            MyBase.InitializeSimulationReferences()

            If Not m_doInterface Is Nothing Then
                AddHandler m_doInterface.OnAddBodyClicked, AddressOf Me.OnAddBodyClicked
            End If

            If Not m_JointToParent Is Nothing Then
                m_JointToParent.InitializeSimulationReferences()
            End If

            Dim doChild As AnimatGUI.DataObjects.Physical.RigidBody
            For Each deEntry As DictionaryEntry In m_aryChildBodies
                doChild = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Physical.RigidBody)
                doChild.InitializeSimulationReferences()
            Next

            Dim doPair As ReceptiveFieldPair
            For Each deEntry As DictionaryEntry In m_aryReceptiveFieldPairs
                doPair = DirectCast(deEntry.Value, ReceptiveFieldPair)
                doPair.InitializeSimulationReferences()
            Next

        End Sub

        Public Overloads Overrides Sub LoadData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByRef oXml As Interfaces.StdXml)
            MyBase.LoadData(doStructure, oXml)

            m_aryChildBodies.Clear()

            oXml.IntoElem() 'Into RigidBody Element

            m_bContactSensor = oXml.GetChildBool("IsContactSensor", m_bContactSensor)
            m_bIsCollisionObject = oXml.GetChildBool("IsCollisionObject", m_bIsCollisionObject)
            m_bFreeze = oXml.GetChildBool("Freeze", m_bFreeze)

            m_snDensity.LoadData(oXml, "Density")
            m_svCOM.LoadData(oXml, "COM", False)

            m_svBuoyancyCenter.LoadData(oXml, "BuoyancyCenter", False)
            m_fltBuoyancyScale = oXml.GetChildFloat("BuoyancyScale", m_fltBuoyancyScale)
            m_svDrag.LoadData(oXml, "Drag", False)
            m_fltMagnus = oXml.GetChildFloat("Magnus", m_fltMagnus)
            m_bEnableFluids = oXml.GetChildBool("EnableFluids", m_bEnableFluids)

            If oXml.FindChildElement("ReceptiveFields", False) Then
                oXml.IntoElem()

                m_snReceptiveFieldDistance.LoadData(oXml, "ReceptiveFieldDistance")
                m_gnReceptiveFieldGain.LoadData(oXml, "FieldGain", "ReceptiveFieldGain")
                m_gnReceptiveCurrentGain.LoadData(oXml, "CurrentGain", "ReceptiveCurrentGain")

                m_aryReceptiveFieldPairs.Clear()

                oXml.IntoChildElement("FieldPairs")
                Dim doPair As ReceptiveFieldPair
                Dim iCount As Integer = oXml.NumberOfChildren() - 1
                For iIndex As Integer = 0 To iCount
                    oXml.FindChildByIndex(iIndex)

                    doPair = New ReceptiveFieldPair(Me)
                    doPair.LoadData(doStructure, oXml)
                    m_aryReceptiveFieldPairs.Add(doPair.ID, doPair)
                Next
                oXml.OutOfElem()   'Outof ChildBodies Element

                oXml.OutOfElem()
            End If

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
                    m_aryChildBodies.Add(dbBody.ID, dbBody)
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
                        m_aryOdorSources.Add(doOdor.ID, doOdor)
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

            oXml.OutOfElem() 'Outof RigidBody Element

        End Sub

        Public Overloads Overrides Sub SaveData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByRef oXml As Interfaces.StdXml)
            MyBase.SaveData(doStructure, oXml)

            oXml.IntoElem() 'Into Child Elemement

            oXml.AddChildElement("IsContactSensor", m_bContactSensor)
            oXml.AddChildElement("IsCollisionObject", m_bIsCollisionObject)

            m_svBuoyancyCenter.SaveData(oXml, "BuoyancyCenter")
            oXml.AddChildElement("BuoyancyScale", m_fltBuoyancyScale)
            m_svDrag.SaveData(oXml, "Drag")
            oXml.AddChildElement("Magnus", m_fltMagnus)
            oXml.AddChildElement("EnableFluids", m_bEnableFluids)

            m_snDensity.SaveData(oXml, "Density")
            m_svCOM.SaveData(oXml, "COM")

            If m_aryReceptiveFieldPairs.Count > 0 AndAlso TypeOf doStructure Is AnimatGUI.DataObjects.Physical.Organism Then
                oXml.AddChildElement("ReceptiveFields")
                oXml.IntoElem()

                m_snReceptiveFieldDistance.SaveData(oXml, "ReceptiveFieldDistance")
                m_gnReceptiveFieldGain.SaveData(oXml, "FieldGain")
                m_gnReceptiveCurrentGain.SaveData(oXml, "CurrentGain")

                oXml.AddChildElement("FieldPairs")
                oXml.IntoElem()
                Dim doPair As ReceptiveFieldPair
                Dim aryRemovePairs As New ArrayList
                For Each deEntry As DictionaryEntry In m_aryReceptiveFieldPairs
                    doPair = DirectCast(deEntry.Value, ReceptiveFieldPair)

                    If doPair.IsValidPair Then
                        doPair.SaveData(doStructure, oXml)
                    Else
                        aryRemovePairs.Add(doPair)
                    End If
                Next
                oXml.OutOfElem()

                oXml.OutOfElem()

                For Each doPair In aryRemovePairs
                    m_aryReceptiveFieldPairs.Remove(doPair.ID)
                Next
            End If

            If Me Is doStructure.RootBody Then
                oXml.AddChildElement("Freeze", m_bFreeze)
            End If

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

            oXml.OutOfElem() 'Outof BodyPart Element

        End Sub

        Public Overrides Sub SaveSimulationXml(ByRef oXml As Interfaces.StdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

            oXml.IntoElem() 'Into Child Elemement

            oXml.AddChildElement("IsContactSensor", m_bContactSensor)
            oXml.AddChildElement("IsCollisionObject", m_bIsCollisionObject)

            m_svBuoyancyCenter.SaveSimulationXml(oXml, Me, "BuoyancyCenter")
            oXml.AddChildElement("BuoyancyScale", m_fltBuoyancyScale)
            m_svDrag.SaveSimulationXml(oXml, Me, "Drag")
            oXml.AddChildElement("Magnus", m_fltMagnus)
            oXml.AddChildElement("EnableFluids", m_bEnableFluids)

            m_snDensity.SaveSimulationXml(oXml, Me, "Density")
            m_svCOM.SaveSimulationXml(oXml, Me, "COM")

            If m_aryReceptiveFieldPairs.Count > 0 AndAlso Not Me.ParentStructure Is Nothing AndAlso TypeOf Me.ParentStructure Is AnimatGUI.DataObjects.Physical.Organism Then
                oXml.AddChildElement("ReceptiveFields")
                oXml.IntoElem()

                m_snReceptiveFieldDistance.SaveSimulationXml(oXml, Me, "ReceptiveFieldDistance")
                m_gnReceptiveFieldGain.SaveSimulationXml(oXml, Me, "FieldGain")
                m_gnReceptiveCurrentGain.SaveSimulationXml(oXml, Me, "CurrentGain")

                oXml.AddChildElement("FieldPairs")
                oXml.IntoElem()
                Dim doPair As ReceptiveFieldPair
                Dim aryRemovePairs As New ArrayList
                For Each deEntry As DictionaryEntry In m_aryReceptiveFieldPairs
                    doPair = DirectCast(deEntry.Value, ReceptiveFieldPair)

                    If doPair.IsValidPair Then
                        doPair.SaveSimulationXml(oXml)
                    Else
                        aryRemovePairs.Add(doPair)
                    End If
                Next
                oXml.OutOfElem()

                oXml.OutOfElem()

                For Each doPair In aryRemovePairs
                    m_aryReceptiveFieldPairs.Remove(doPair.ID)
                Next
            End If

            If Me.IsRoot Then
                oXml.AddChildElement("Freeze", m_bFreeze)
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
            If m_bFoodSource Then
                m_snFoodQuantity.SaveSimulationXml(oXml, Me, "FoodQuantity")
                m_snMaxFoodQuantity.SaveSimulationXml(oXml, Me, "MaxFoodQuantity")
                m_snFoodReplenishRate.SaveSimulationXml(oXml, Me, "FoodReplenishRate")
                m_snFoodEnergyContent.SaveSimulationXml(oXml, Me, "FoodEnergyContent")
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

        Public Overridable Overloads Function AddChildBody(ByVal vPos As Framework.Vec3d, ByVal vNorm As Framework.Vec3d) As Boolean
            Dim rbNew As RigidBody

            Try

                'First Select the new rigid body part type
                Dim frmSelectParts As New Forms.BodyPlan.SelectPartType()
                frmSelectParts.PartType = GetType(Physical.RigidBody)
                frmSelectParts.IsRoot = False
                frmSelectParts.ParentBody = Me

                If frmSelectParts.ShowDialog() <> DialogResult.OK Then Return False

                rbNew = DirectCast(frmSelectParts.SelectedPart.Clone(Me, False, Nothing), RigidBody)
                rbNew.SetDefaultSizes()

                Dim bAddDefaultGraphics As Boolean = False
                If rbNew.HasDynamics Then
                    rbNew.IsCollisionObject = frmSelectParts.rdCollision.Checked
                    If rbNew.IsCollisionObject Then
                        bAddDefaultGraphics = frmSelectParts.chkAddGraphics.Checked
                    Else
                        rbNew.ContactSensor = frmSelectParts.chkIsSensor.Checked
                    End If
                Else
                    rbNew.IsCollisionObject = False
                    rbNew.ContactSensor = False
                End If

                Me.ParentStructure.NewBodyIndex = Me.ParentStructure.NewBodyIndex + 1
                rbNew.Name = "Body_" & Me.ParentStructure.NewBodyIndex
                rbNew.IsRoot = False

                'Now, if it needs a joint then select the joint type to use
                If rbNew.UsesAJoint Then
                    If Not rbNew.SelectJointType(vPos, vNorm) Then
                        Return False
                    End If
                End If

                'rbNew.LocalPosition.CopyData(0, 0, 0.1, True)

                'Now add the new part to the parent
                AddChildBody(rbNew, bAddDefaultGraphics)

                rbNew.OrientNewPart(vPos, vNorm)
                rbNew.SelectItem(False)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Function

        Public Overridable Overloads Sub AddChildBody(ByVal rbChildBody As AnimatGUI.DataObjects.Physical.RigidBody, ByVal bAddDefaultGraphics As Boolean)

            rbChildBody.IsRoot = False

            If Not Me.ChildBodies.Contains(rbChildBody.ID) Then
                rbChildBody.BeforeAddBody()

                Me.ChildBodies.Add(rbChildBody.ID, rbChildBody)

                'If this is  a collision objectthen we need to add a default graphics object for this item.
                If bAddDefaultGraphics AndAlso rbChildBody.IsCollisionObject Then
                    rbChildBody.CreateDefaultGraphicsObject()
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

        Protected Overridable Function SelectJointType(ByVal vPos As Framework.Vec3d, ByVal vNorm As Framework.Vec3d) As Boolean

            'First Select the new rigid body part type
            Dim frmSelectParts As New Forms.BodyPlan.SelectPartType()
            frmSelectParts.PartType = GetType(Physical.Joint)
            frmSelectParts.IsRoot = False
            frmSelectParts.ParentBody = Me

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
            Dim fltDensityDistChange As Single = CSng(10 ^ iDistDiff)

            Dim fltValue As Double = (m_snDensity.ActualValue / Math.Pow(10, CInt(ePrevMass))) * (Math.Pow(fltDensityDistChange, 3) / fltMassChange)
            Dim eSCale As ScaledNumber.enumNumericScale = CType(Util.Environment.MassUnits, ScaledNumber.enumNumericScale)
            Dim strUnits As String = "g/" & Util.Environment.DistanceUnitAbbreviation(Util.Environment.DisplayDistanceUnits) & "^3"
            m_snDensity = New ScaledNumber(Me, "Density", fltValue, eSCale, strUnits, strUnits)

            m_snReceptiveFieldDistance.ActualValue = m_snReceptiveFieldDistance.ActualValue * fltDistanceChange

            If Not m_JointToParent Is Nothing Then
                m_JointToParent.UnitsChanged(ePrevMass, eNewMass, fltMassChange, ePrevDistance, eNewDistance, fltDistanceChange)
            End If

            Dim doChild As RigidBody
            For Each deEntry As DictionaryEntry In m_aryChildBodies
                doChild = DirectCast(deEntry.Value, RigidBody)
                doChild.UnitsChanged(ePrevMass, eNewMass, fltMassChange, ePrevDistance, eNewDistance, fltDistanceChange)
            Next
        End Sub

        Protected Overridable Function IncludeReceptiveFieldPairsInModule(ByVal nmNeuralModule As DataObjects.Behavior.NeuralModule) As Boolean

            Dim doOrganism As DataObjects.Physical.Organism

            If Not Me.ParentStructure Is Nothing AndAlso TypeOf Me.ParentStructure Is DataObjects.Physical.Organism Then
                doOrganism = DirectCast(Me.ParentStructure, DataObjects.Physical.Organism)

                Dim doPair As ReceptiveFieldPair
                For Each deEntry As DictionaryEntry In m_aryReceptiveFieldPairs
                    doPair = DirectCast(deEntry.Value, ReceptiveFieldPair)

                    If nmNeuralModule.GetType Is doPair.Neuron.NeuralModuleType Then
                        If Not doOrganism.FindObjectByID(doPair.Neuron.ID) Is Nothing Then
                            Return True
                        End If
                    End If
                Next
            End If

            Return False
        End Function

        Public Overridable Sub AddContactAdapters(ByVal nmPhysicsModule As DataObjects.Behavior.NeuralModule, ByVal m_aryNodes As Collections.SortedNodes)
            Dim doOrganism As DataObjects.Physical.Organism

            If Not Me.ParentStructure Is Nothing AndAlso TypeOf Me.ParentStructure Is DataObjects.Physical.Organism AndAlso m_aryReceptiveFieldPairs.Count > 0 Then
                doOrganism = DirectCast(Me.ParentStructure, DataObjects.Physical.Organism)

                'For each rigid body we could have one contact adapter for each neural module. Basically we need to divide out the associated neurons
                'so that they have a target neural module ID the same as their native module. This will ensure that those neurons are updated at the correct
                'times during the simulation. So first we make an array of adapters for each neural module, and then as we loop through the association pairs
                'we add them to the adapter associated with a given neural module and then only save the adapters that end up with neuron pairs in them.
                Dim aryAdapters As New ArrayList
                Dim doAdapter As DataObjects.Behavior.Nodes.ContactAdapter
                For Each deEntry As DictionaryEntry In doOrganism.NeuralModules
                    doAdapter = New DataObjects.Behavior.Nodes.ContactAdapter(nmPhysicsModule)
                    doAdapter.RigidBody = Me
                    doAdapter.TargetNeuralModule = DirectCast(deEntry.Value, DataObjects.Behavior.NeuralModule)
                    aryAdapters.Add(doAdapter)
                Next

                Dim doPair As ReceptiveFieldPair
                For Each deRFEntry As DictionaryEntry In m_aryReceptiveFieldPairs
                    doPair = DirectCast(deRFEntry.Value, ReceptiveFieldPair)

                    If Not doOrganism.FindObjectByID(doPair.Neuron.ID) Is Nothing Then
                        For Each doAdapter In aryAdapters
                            If doAdapter.TargetNeuralModule.GetType() Is doPair.Neuron.NeuralModuleType Then
                                doAdapter.ReceptiveFieldPairs.Add(doPair)
                            End If
                        Next
                    End If
                Next

                For Each doAdapter In aryAdapters
                    If doAdapter.ReceptiveFieldPairs.Count > 0 Then
                        m_aryNodes.Add(doAdapter.ID, doAdapter)
                    End If
                Next
            End If

            'dwc change
            'Now add the contact adapters for any children objects.
            Dim doChild As RigidBody
            For Each deEntry As DictionaryEntry In m_aryChildBodies
                doChild = DirectCast(deEntry.Value, RigidBody)
                doChild.AddContactAdapters(nmPhysicsModule, m_aryNodes)
            Next

        End Sub

        Public Overridable Function FindReceptiveField(ByVal fltX As Single, ByVal fltY As Single, ByVal fltZ As Single, ByRef iIndex As Integer) As Boolean
            iIndex = m_aryReceptiveFields.BinarySearch(New AnimatGUI.Framework.Vec3d(Nothing, fltX, fltY, fltZ))

            If (iIndex < 0) Then
                Return False
            Else
                Return True
            End If
        End Function


        Public Overridable Sub DumpReceptiveFields()
            Dim i As Integer = 0
            For Each vField As AnimatGUI.Framework.Vec3d In m_aryReceptiveFields
                Debug.WriteLine("Index: " + i.ToString() + "  (" + vField.X.ToString() + ", " + vField.Y.ToString() + ", " + vField.Z.ToString() + ")")
                i = i + 1
            Next
        End Sub

        Public Overridable Sub SortReceptiveFields()
            'I need to sort the list of receptive fields, and I need to remove any duplicates
            m_aryReceptiveFields.Sort()

            Dim aryFields As New ArrayList

            For Each vField As Vec3d In m_aryReceptiveFields
                'Only add in vectors that have not already been added
                If aryFields.BinarySearch(vField) <> 0 Then
                    aryFields.Add(vField)
                End If
            Next

            'Now reset the receptive fields array to be this new array
            m_aryReceptiveFields = aryFields
        End Sub

        'If the receptive field vertices are changed then we need to go back through the list of field pairs and find the vertex that is closest to
        'each of the vertices in the pairs.
        Protected Overridable Sub VerifyReceptiveFielPairs()

            Dim doPair As ReceptiveFieldPair
            For Each deEntry As DictionaryEntry In Me.ReceptiveFieldPairs
                doPair = DirectCast(deEntry.Value, ReceptiveFieldPair)
                doPair.Vertex = FindClosestVertex(doPair.Vertex)
            Next
        End Sub

        Protected Overridable Function FindClosestVertex(ByVal vOrig As Vec3d) As Vec3d
            Dim fltMin As Double = -1
            Dim fltDist As Double
            Dim vMin As Vec3d

            For Each vVertex As Vec3d In Me.ReceptiveFields
                fltDist = Util.Distance(vOrig, vVertex)
                If fltMin = -1 OrElse fltDist < fltMin Then
                    fltMin = fltDist
                    vMin = vVertex
                End If
            Next

            Return vMin
        End Function

        Public Overridable Sub CreateDefaultGraphicsObject()

            Dim doGraphics As RigidBody = DirectCast(Me.Clone(Me, False, Me), Physical.RigidBody)
            doGraphics.SetDefaultSizes()
            doGraphics.m_JointToParent = Nothing
            doGraphics.IsCollisionObject = False
            doGraphics.ContactSensor = False
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

#Region " Add-Remove to List Methods "

        Public Overrides Sub BeforeAddToList(Optional ByVal bThrowError As Boolean = True)
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

            MyBase.BeforeAddToList(bThrowError)
            If Not m_JointToParent Is Nothing Then
                m_JointToParent.BeforeAddToList(bThrowError)
            End If

            Util.Application.SimulationInterface.AddItem(Me.Parent.ID, "RigidBody", Me.GetSimulationXml("RigidBody"), bThrowError)
            InitializeSimulationReferences()
        End Sub

        Public Overrides Sub AfterAddToList(Optional ByVal bThrowError As Boolean = True)
            MyBase.AfterAddToList(bThrowError)
            If Not m_JointToParent Is Nothing Then
                m_JointToParent.AfterAddToList(bThrowError)
            End If
        End Sub

        Public Overrides Sub BeforeRemoveFromList(Optional ByVal bThrowError As Boolean = True)
            MyBase.BeforeRemoveFromList(bThrowError)
            If Not m_JointToParent Is Nothing Then
                m_JointToParent.BeforeRemoveFromList(bThrowError)
            End If

            Util.Application.SimulationInterface.RemoveItem(Me.Parent.ID(), "RigidBody", Me.ID, bThrowError)
            m_doInterface = Nothing
        End Sub

        Public Overrides Sub AfterRemoveFromList(Optional ByVal bThrowError As Boolean = True)
            MyBase.AfterRemoveFromList(bThrowError)
            If Not m_JointToParent Is Nothing Then
                m_JointToParent.AfterRemoveFromList(bThrowError)
            End If
        End Sub

#End Region

#End Region

#Region " Events "

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

        Protected Overridable Sub OnDragValueChanged()
            Try
                Me.SetSimData("Drag", m_svDrag.GetSimulationXml("Drag"), True)
                Util.ProjectProperties.RefreshProperties()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#Region " DataObjectInterface Events "

        'All events coming up from the DataObjectInterface are actually coming from a different thread.
        'The one for the simulation. This means that we have to use BeginInvoke to recall a different 
        'method on the GUI thread or it will cause big problems. So all of these methods do that.

        Protected Overridable Sub OnAddBodyClicked(ByVal fltPosX As Single, ByVal fltPosY As Single, ByVal fltPosZ As Single, _
                                                   ByVal fltNormX As Single, ByVal fltNormY As Single, ByVal fltNormZ As Single)

            Try
                Dim vPos As New Framework.Vec3d(Me, fltPosX, fltPosY, fltPosZ)
                Dim vNorm As New Framework.Vec3d(Me, fltNormX, fltNormY, fltNormZ)

                Dim aryObjs(1) As Object
                aryObjs(0) = vPos
                aryObjs(1) = vNorm
                Util.Application.BeginInvoke(New AddChildBodyDelegate(AddressOf Me.AddChildBody), aryObjs)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

#End Region

    End Class

End Namespace

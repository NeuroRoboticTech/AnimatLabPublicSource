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

    Public Class Prismatic
        Inherits Physical.Joint

#Region " Attributes "

        Protected m_doLowerLimit As ConstraintLimit
        Protected m_doUpperLimit As ConstraintLimit
        Protected m_bEnableMotor As Boolean = False
        Protected m_snMaxForce As AnimatGUI.Framework.ScaledNumber
        Protected m_snMaxVelocity As AnimatGUI.Framework.ScaledNumber
        Protected m_eMotorType As Joint.enumJointMotorTypes = enumJointMotorTypes.VelocityControl
        Protected m_fltServoGain As Single = 100

        Protected m_doAssistPID As PidControl

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.Prismatic_Treeview.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonImageName() As String
            Get
                Return "AnimatGUI.Prismatic_SelectType.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property Type() As String
            Get
                Return "Prismatic"
            End Get
        End Property

        Public Overrides ReadOnly Property PartType() As System.Type
            Get
                Return GetType(AnimatGUI.DataObjects.Physical.Joints.Prismatic)
            End Get
        End Property

        Public Overridable Property LowerLimit() As ConstraintLimit
            Get
                Return m_doLowerLimit
            End Get
            Set(ByVal value As ConstraintLimit)
                m_doLowerLimit.CopyData(value)
            End Set
        End Property

        Public Overridable Property UpperLimit() As ConstraintLimit
            Get
                Return m_doUpperLimit
            End Get
            Set(ByVal value As ConstraintLimit)
                m_doUpperLimit.CopyData(value)
            End Set
        End Property

        Public Overridable Property EnableMotor() As Boolean
            Get
                Return m_bEnableMotor
            End Get
            Set(ByVal value As Boolean)
                SetSimData("EnableMotor", value.ToString, True)
                m_bEnableMotor = value
            End Set
        End Property


        Public Overridable Property MotorType() As Joint.enumJointMotorTypes
            Get
                Return m_eMotorType
            End Get
            Set(ByVal value As Joint.enumJointMotorTypes)
                SetSimData("MotorType", Convert.ToInt32(m_eMotorType).ToString(), True)
                m_eMotorType = value

                If m_eMotorType = enumJointMotorTypes.PositionVelocityControl Then
                    m_thIncomingDataTypes.DataTypes.Clear()
                    m_thIncomingDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("DesiredPosition", "DesiredPosition", "Meters", "m", -3.142, 3.142, ScaledNumber.enumNumericScale.None, ScaledNumber.enumNumericScale.None))
                    m_thIncomingDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("DesiredVelocity", "Desired Velocity", "m/s", "m/s", -5, 5, ScaledNumber.enumNumericScale.None, ScaledNumber.enumNumericScale.None))
                    m_thIncomingDataTypes.ID = "DesiredPosition"
                    Me.SignalReloadTargetDataTypes()
                ElseIf m_eMotorType = enumJointMotorTypes.PositionControl Then
                    m_thIncomingDataTypes.DataTypes.Clear()
                    m_thIncomingDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("DesiredPosition", "DesiredPosition", "Meters", "m", -3.142, 3.142, ScaledNumber.enumNumericScale.None, ScaledNumber.enumNumericScale.None))
                    m_thIncomingDataTypes.ID = "DesiredPosition"
                    Me.SignalReloadTargetDataTypes()
                Else
                    m_thIncomingDataTypes.DataTypes.Clear()
                    m_thIncomingDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("DesiredVelocity", "Desired Velocity", "m/s", "m/s", -5, 5, ScaledNumber.enumNumericScale.None, ScaledNumber.enumNumericScale.None))
                    m_thIncomingDataTypes.ID = "DesiredVelocity"
                    Me.SignalReloadTargetDataTypes()
                End If

                If Not Util.ProjectWorkspace Is Nothing Then
                    Util.ProjectWorkspace.RefreshProperties()
                End If
            End Set
        End Property

        Public Overridable Property MaxForce() As ScaledNumber
            Get
                Return m_snMaxForce
            End Get
            Set(ByVal value As ScaledNumber)
                If value.ActualValue <= 0 Then
                    Throw New System.Exception("The maximum torque must be greater than zero.")
                End If

                SetSimData("MaxForce", value.ActualValue.ToString(), True)
                m_snMaxForce.CopyData(value)
            End Set
        End Property

        Public Overridable Property MaxVelocity() As ScaledNumber
            Get
                Return m_snMaxVelocity
            End Get
            Set(ByVal value As ScaledNumber)
                If value.ActualValue <= 0 Then
                    Throw New System.Exception("The maximum velocity must be greater than zero.")
                End If

                SetSimData("MaxVelocity", m_snMaxVelocity.ActualValue.ToString(), True)
                m_snMaxVelocity.CopyData(value)
            End Set
        End Property

        Public Overridable Property ServoGain() As Single
            Get
                Return m_fltServoGain
            End Get
            Set(ByVal value As Single)
                If value < 0 Then
                    Throw New System.Exception("The gain value must be greater than or equal to zero.")
                End If
                SetSimData("ServoGain", value.ToString, True)
                m_fltServoGain = value
            End Set
        End Property

        Public Overrides ReadOnly Property UsesRadians() As Boolean
            Get
                Return False
            End Get
        End Property

        Public Overridable Property AssistPID() As PidControl
            Get
                Return m_doAssistPID
            End Get
            Set(ByVal value As PidControl)
                m_doAssistPID = value
            End Set
        End Property

#End Region

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
            m_strDescription = ""

            m_doLowerLimit = Util.Application.Physics.CreateConstraintLimit(Me.Type, Me)
            m_doUpperLimit = Util.Application.Physics.CreateConstraintLimit(Me.Type, Me)

            m_doLowerLimit.PairedLimit = m_doUpperLimit
            m_doUpperLimit.PairedLimit = m_doLowerLimit
            m_doLowerLimit.IsLowerLimit = True
            m_doUpperLimit.IsLowerLimit = False
            m_doLowerLimit.AngleLimit = False
            m_doUpperLimit.AngleLimit = False

            m_snMaxForce = New AnimatGUI.Framework.ScaledNumber(Me, "MaxForce", 100, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Newtons", "N")
            m_snMaxVelocity = New AnimatGUI.Framework.ScaledNumber(Me, "MaxVelocity", 100, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "rad/s", "rad/s")

            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("JointPosition", "Position", "Meters", "m", -10, 10))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("JointDesiredPosition", "Desired Position", "Meters", "m", -10, 10))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("JointActualVelocity", "Velocity", "m/s", "m/s", -5, 5))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("JointDesiredVelocity", "Desired Velocity", "m/s", "m/s", -5, 5))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("Enable", "Enable", "", "", 0, 1))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("WorldPositionX", "Position X Axis", "Meters", "m", -10, 10))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("WorldPositionY", "Position Y Axis", "Meters", "m", -10, 10))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("WorldPositionZ", "Position Z Axis", "Meters", "m", -10, 10))

            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("Temperature", "Temperature", "Celcius", "C", -100, 100))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("Voltage", "Voltage", "Volts", "V", -20, 20))

            If Util.Application.Physics.ProvidesJointForceFeedback Then
                m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("MotorForceToAX", "Motor Force Applied to Body A, X Axis", "Newtons", "N", -10, 10))
                m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("MotorForceToAY", "Motor Force Applied to Body A, Y Axis", "Newtons", "N", -10, 10))
                m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("MotorForceToAZ", "Motor Force Applied to Body A, Z Axis", "Newtons", "N", -10, 10))

                m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("MotorForceToBX", "Motor Force Applied to Body B, X Axis", "Newtons", "N", -10, 10))
                m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("MotorForceToBY", "Motor Force Applied to Body B, Y Axis", "Newtons", "N", -10, 10))
                m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("MotorForceToBZ", "Motor Force Applied to Body B, Z Axis", "Newtons", "N", -10, 10))

                m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("MotorTorqueToAX", "Motor Torque Applied to Body A, X Axis", "Newtons", "N", -10, 10))
                m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("MotorTorqueToAY", "Motor Torque Applied to Body A, Y Axis", "Newtons", "N", -10, 10))
                m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("MotorTorqueToAZ", "Motor Torque Applied to Body A, Z Axis", "Newtons", "N", -10, 10))

                m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("MotorTorqueToBX", "Motor Torque Applied to Body B, X Axis", "Newtons", "N", -10, 10))
                m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("MotorTorqueToBY", "Motor Torque Applied to Body B, Y Axis", "Newtons", "N", -10, 10))
                m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("MotorTorqueToBZ", "Motor Torque Applied to Body B, Z Axis", "Newtons", "N", -10, 10))
            End If

            m_doAssistPID = New PidControl(Me)
            m_doAssistPID.Enabled = True

            If Util.Application.Physics.GenerateMotorAssist Then
                m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("MotorAssistForceToAX", "Motor Assist Force Applied to Body A, X Axis", "Newtons", "N", -10, 10))
                m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("MotorAssistForceToAY", "Motor Assist Force Applied to Body A, Y Axis", "Newtons", "N", -10, 10))
                m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("MotorAssistForceToAZ", "Motor Assist Force Applied to Body A, Z Axis", "Newtons", "N", -10, 10))

                m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("MotorAssistForceToBX", "Motor Assist Force Applied to Body B, X Axis", "Newtons", "N", -10, 10))
                m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("MotorAssistForceToBY", "Motor Assist Force Applied to Body B, Y Axis", "Newtons", "N", -10, 10))
                m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("MotorAssistForceToBZ", "Motor Assist Force Applied to Body B, Z Axis", "Newtons", "N", -10, 10))

                m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("MotorAssistTorqueToAX", "Motor Assist Torque Applied to Body A, X Axis", "Newtons", "N", -10, 10))
                m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("MotorAssistTorqueToAY", "Motor Assist Torque Applied to Body A, Y Axis", "Newtons", "N", -10, 10))
                m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("MotorAssistTorqueToAZ", "Motor Assist Torque Applied to Body A, Z Axis", "Newtons", "N", -10, 10))

                m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("MotorAssistTorqueToBX", "Motor Assist Torque Applied to Body B, X Axis", "Newtons", "N", -10, 10))
                m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("MotorAssistTorqueToBY", "Motor Assist Torque Applied to Body B, Y Axis", "Newtons", "N", -10, 10))
                m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("MotorAssistTorqueToBZ", "Motor Assist Torque Applied to Body B, Z Axis", "Newtons", "N", -10, 10))

                m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("MotorAssistForceMagnitude", "Motor Assist Force Magnitude", "Newtons", "N", -10, 10))
            End If

            m_thDataTypes.ID = "JointPosition"

            m_doFriction = New ConstraintFriction(Me)

        End Sub

        Public Overrides Sub InitAfterAppStart()
            MyBase.InitAfterAppStart()
            AddCompatibleStimulusType("MotorVelocity")
            AddCompatibleStimulusType("MotorPosition")
            AddCompatibleStimulusType("PositionClamp")
        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()
            If Not m_doLowerLimit Is Nothing Then m_doLowerLimit.ClearIsDirty()
            If Not m_doUpperLimit Is Nothing Then m_doUpperLimit.ClearIsDirty()

            If Not m_snMaxForce Is Nothing Then m_snMaxForce.ClearIsDirty()
            If Not m_snMaxVelocity Is Nothing Then m_snMaxVelocity.ClearIsDirty()
            If Not m_doAssistPID Is Nothing Then m_doAssistPID.ClearIsDirty()
        End Sub

        Public Overrides Sub SetDefaultSizes()
            MyBase.SetDefaultSizes()
            m_snSize.ActualValue = 0.02 * Util.Environment.DistanceUnitValue
            m_doLowerLimit.LimitPos.ActualValue = -1 * Util.Environment.DistanceUnitValue
            m_doUpperLimit.LimitPos.ActualValue = 1 * Util.Environment.DistanceUnitValue
        End Sub

        Public Overrides Function Clone(ByVal doParent As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject) As Framework.DataObject
            Dim oNewNode As New Joints.Prismatic(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doOrig As Joints.Prismatic = DirectCast(doOriginal, Joints.Prismatic)
            m_doLowerLimit = DirectCast(doOrig.m_doLowerLimit.Clone(Me, bCutData, doRoot), ConstraintLimit)
            m_doUpperLimit = DirectCast(doOrig.m_doUpperLimit.Clone(Me, bCutData, doRoot), ConstraintLimit)

            m_doLowerLimit.PairedLimit = m_doUpperLimit
            m_doUpperLimit.PairedLimit = m_doLowerLimit
            m_doLowerLimit.IsLowerLimit = True
            m_doUpperLimit.IsLowerLimit = False
            m_doLowerLimit.AngleLimit = False
            m_doUpperLimit.AngleLimit = False

            m_bEnableMotor = doOrig.m_bEnableMotor
            m_eMotorType = doOrig.m_eMotorType
            m_fltServoGain = doOrig.ServoGain
            m_snMaxForce = DirectCast(doOrig.m_snMaxForce.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snMaxVelocity = DirectCast(doOrig.m_snMaxVelocity.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_doAssistPID = DirectCast(doOrig.m_doAssistPID.Clone(Me, bCutData, doRoot), PidControl)
        End Sub

        Public Overrides Sub InitializeSimulationReferences(Optional ByVal bShowError As Boolean = True)
            MyBase.InitializeSimulationReferences(bShowError)

            m_doLowerLimit.InitializeSimulationReferences(bShowError)
            m_doUpperLimit.InitializeSimulationReferences(bShowError)

            If Util.Application.Physics.GenerateMotorAssist AndAlso Not m_doAssistPID Is Nothing Then m_doAssistPID.InitializeSimulationReferences(bShowError)
        End Sub

        Public Overrides Sub AddToReplaceIDList(aryReplaceIDList As System.Collections.ArrayList, ByVal arySelectedItems As ArrayList)
            MyBase.AddToReplaceIDList(aryReplaceIDList, arySelectedItems)

            If Not m_doLowerLimit Is Nothing Then m_doLowerLimit.AddToReplaceIDList(aryReplaceIDList, arySelectedItems)
            If Not m_doUpperLimit Is Nothing Then m_doUpperLimit.AddToReplaceIDList(aryReplaceIDList, arySelectedItems)
            If Not m_doAssistPID Is Nothing Then m_doAssistPID.AddToReplaceIDList(aryReplaceIDList, arySelectedItems)
        End Sub

        Public Overrides Function FindObjectByID(ByVal strID As String) As Framework.DataObject

            Dim doObject As AnimatGUI.Framework.DataObject = MyBase.FindObjectByID(strID)
            If doObject Is Nothing AndAlso Not m_doAssistPID Is Nothing Then doObject = m_doAssistPID.FindObjectByID(strID)

            Return doObject

        End Function

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag

            If Util.Application.Physics.ShowSeparateConstraintLimits Then
                pbNumberBag = m_doLowerLimit.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Lower Limit", pbNumberBag.GetType(), "LowerLimit", _
                                            "Constraints", "Sets the values for the minimum angle constraint.", pbNumberBag, _
                                            "", GetType(AnimatGUI.TypeHelpers.ConstrainLimitTypeConverter)))

                pbNumberBag = m_doUpperLimit.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Upper Limit", pbNumberBag.GetType(), "UpperLimit", _
                                            "Constraints", "Sets the values for the maximum angle constraint.", pbNumberBag, _
                                            "", GetType(AnimatGUI.TypeHelpers.ConstrainLimitTypeConverter)))
            Else
                m_doLowerLimit.BuildPropertiesInline(propTable, False, "Lower", "LowerLimit.")
                m_doUpperLimit.BuildPropertiesInline(propTable, True, "Upper", "UpperLimit.")
            End If

            pbNumberBag = m_snMaxForce.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Max Motor Force", pbNumberBag.GetType(), "MaxForce", _
                                            "Motor Properties", "Sets the maximum force that this motor can apply to obtain a desired velocity of movement.", pbNumberBag, _
                                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snMaxVelocity.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Max Velocity", pbNumberBag.GetType(), "MaxVelocity", _
                                        "Motor Properties", "Sets the maximum positive or negative velocity that the motor can move this part.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Enable Motor", m_bEnableMotor.GetType(), "EnableMotor", _
                          "Motor Properties", "Sets whether the motor is enabled for this joint.", m_bEnableMotor))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Motor Type", m_eMotorType.GetType(), "MotorType", _
                          "Motor Properties", "Sets the type for this motor.", m_eMotorType))

            If m_eMotorType = enumJointMotorTypes.PositionControl OrElse m_eMotorType = enumJointMotorTypes.PositionVelocityControl Then
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Servo Gain", m_fltServoGain.GetType(), "ServoGain", _
                              "Motor Properties", "Sets the magnitude of the feedback gain for the servo motor.", m_fltServoGain))
            End If

            If Util.Application.Physics.GenerateMotorAssist Then
                pbNumberBag = m_doAssistPID.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Assist PID", pbNumberBag.GetType(), "AssistPID", _
                                                "Motor Properties", "Sets the PID controller for motor assist.", pbNumberBag, _
                                                "", GetType(PidControlPropBagConverter)))
            End If

        End Sub


        Public Overloads Overrides Sub LoadData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.LoadData(doStructure, oXml)

            oXml.IntoElem() 'Into Joint Element

            m_doLowerLimit.LoadData(oXml, "LowerLimit")
            m_doUpperLimit.LoadData(oXml, "UpperLimit")
            m_snMaxForce.LoadData(oXml, "MaxForce")
            m_snMaxVelocity.LoadData(oXml, "MaxVelocity")

            EnableMotor = oXml.GetChildBool("EnableMotor", m_bEnableMotor)

            If oXml.FindChildElement("MotorType", False) Then
                MotorType = DirectCast([Enum].Parse(GetType(Joint.enumJointMotorTypes), oXml.GetChildString("MotorType"), True), Joint.enumJointMotorTypes)
            Else
                Dim bServoMotor As Boolean = oXml.GetChildBool("ServoMotor", False)
                If bServoMotor Then
                    MotorType = enumJointMotorTypes.PositionControl
                Else
                    MotorType = enumJointMotorTypes.VelocityControl
                End If
            End If

            ServoGain = oXml.GetChildFloat("ServoGain", m_fltServoGain)

            If Not m_doAssistPID Is Nothing AndAlso oXml.FindChildElement("PID", False) Then
                m_doAssistPID.LoadData(oXml)
            End If

            oXml.OutOfElem() 'Outof Joint Element

        End Sub

        Public Overloads Overrides Sub SaveData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.SaveData(doStructure, oXml)

            oXml.IntoElem() 'Into Joint Elemement

            m_doLowerLimit.SaveData(oXml, "LowerLimit")
            m_doUpperLimit.SaveData(oXml, "UpperLimit")
            m_snMaxForce.SaveData(oXml, "MaxForce")
            m_snMaxVelocity.SaveData(oXml, "MaxVelocity")

            oXml.AddChildElement("EnableMotor", m_bEnableMotor)
            oXml.AddChildElement("MotorType", m_eMotorType.ToString())
            oXml.AddChildElement("ServoGain", m_fltServoGain)

            If Not m_doAssistPID Is Nothing Then
                m_doAssistPID.SaveData(oXml)
            End If

            oXml.OutOfElem() 'Outof Joint Element

        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

            oXml.IntoElem()

            m_doLowerLimit.SaveSimulationXml(oXml, Me, "LowerLimit")
            m_doUpperLimit.SaveSimulationXml(oXml, Me, "UpperLimit")

            m_snMaxForce.SaveSimulationXml(oXml, Me, "MaxForce")
            m_snMaxVelocity.SaveSimulationXml(oXml, Me, "MaxVelocity")

            oXml.AddChildElement("EnableMotor", m_bEnableMotor)
            oXml.AddChildElement("MotorType", Convert.ToInt32(m_eMotorType))
            oXml.AddChildElement("ServoGain", m_fltServoGain)

            If Not m_doAssistPID Is Nothing AndAlso Util.Application.Physics.GenerateMotorAssist Then
                m_doAssistPID.SaveSimulationXml(oXml, Me)
            End If

            oXml.OutOfElem()

        End Sub

    End Class


End Namespace

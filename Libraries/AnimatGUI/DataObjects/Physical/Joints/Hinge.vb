﻿Imports System
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

Namespace DataObjects.Physical.Joints

    Public Class Hinge
        Inherits Physical.Joint

#Region " Attributes "

        Protected m_doMinConstraint As ConstraintLimit
        Protected m_doMaxConstraint As ConstraintLimit

        Protected m_snMinAngle As AnimatGUI.Framework.ScaledNumber
        Protected m_snMaxAngle As AnimatGUI.Framework.ScaledNumber
        Protected m_bEnableMotor As Boolean = False
        Protected m_snMaxTorque As AnimatGUI.Framework.ScaledNumber
        Protected m_snMaxVelocity As AnimatGUI.Framework.ScaledNumber
        Protected m_bServoMotor As Boolean = False
        Protected m_fltServoGain As Single = 100
        Protected m_snSize As AnimatGUI.Framework.ScaledNumber

#End Region

#Region " Properties "


        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.Joint.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonImageName() As String
            Get
                Return "AnimatGUI.Joint.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property Type() As String
            Get
                Return "Hinge"
            End Get
        End Property

        Public Overrides ReadOnly Property PartType() As System.Type
            Get
                Return GetType(AnimatGUI.DataObjects.Physical.Joints.Hinge)
            End Get
        End Property

        Public Overridable Property MinConstraint() As ConstraintLimit
            Get
                Return m_doMinConstraint
            End Get
            Set(ByVal value As ConstraintLimit)
                m_doMinConstraint.CopyData(value)
            End Set
        End Property

        Public Overridable Property MaxConstraint() As ConstraintLimit
            Get
                Return m_doMaxConstraint
            End Get
            Set(ByVal value As ConstraintLimit)
                m_doMaxConstraint.CopyData(value)
            End Set
        End Property

        Public Overridable Property MinAngle() As ScaledNumber
            Get
                Return m_snMinAngle
            End Get
            Set(ByVal value As ScaledNumber)
                If value.ActualValue > m_snMaxAngle.ActualValue Then
                    Throw New System.Exception("The minimum angle cannot be larger than the maximum angle.")
                End If
                If value.ActualValue < -180 Then
                    Throw New System.Exception("The minium angle cannot be less than -180 degrees.")
                End If
                If value.ActualValue > 180 Then
                    Throw New System.Exception("The minium angle cannot be greater than 180 degrees.")
                End If

                SetSimData("MinAngle", Util.DegreesToRadians(CSng(value.ActualValue)).ToString(), True)
                m_snMinAngle.CopyData(value)
            End Set
        End Property

        Public Overridable ReadOnly Property MinAngleRadians() As Single
            Get
                Return Util.DegreesToRadians(CSng(m_snMinAngle.ActualValue))
            End Get
        End Property

        Public Overridable Property MaxAngle() As ScaledNumber
            Get
                Return m_snMaxAngle
            End Get
            Set(ByVal value As ScaledNumber)
                If value.ActualValue < m_snMinAngle.ActualValue Then
                    Throw New System.Exception("The maximum angle cannot be less than the minimum angle.")
                End If
                If value.ActualValue < -180 Then
                    Throw New System.Exception("The maximum angle cannot be less than -180 degrees.")
                End If
                If value.ActualValue > 180 Then
                    Throw New System.Exception("The maximum angle cannot be greater than 180 degrees.")
                End If

                SetSimData("MaxAngle", Util.DegreesToRadians(CSng(value.ActualValue)).ToString(), True)
                m_snMaxAngle.CopyData(value)
            End Set
        End Property

        Public Overridable ReadOnly Property MaxAngleRadians() As Single
            Get
                Return Util.DegreesToRadians(CSng(m_snMaxAngle.ActualValue))
            End Get
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

        Public Overridable Property ServoMotor() As Boolean
            Get
                Return m_bServoMotor
            End Get
            Set(ByVal value As Boolean)
                SetSimData("ServoMotor", value.ToString, True)
                m_bServoMotor = value
            End Set
        End Property

        Public Overridable Property MaxTorque() As ScaledNumber
            Get
                Return m_snMaxTorque
            End Get
            Set(ByVal value As ScaledNumber)
                'If value.ActualValue < m_snMinAngle.ActualValue Then
                '    Throw New System.Exception("The maximum angle cannot be less than the minimum angle.")
                'End If
                'If value.ActualValue < -180 Then
                '    Throw New System.Exception("The maximum angle cannot be less than -180 degrees.")
                'End If
                'If value.ActualValue > 180 Then
                '    Throw New System.Exception("The maximum angle cannot be greater than 180 degrees.")
                'End If

                SetSimData("MaxTorque", value.ActualValue.ToString(), True)
                m_snMaxTorque.CopyData(value)
            End Set
        End Property

        Public Overridable Property MaxVelocity() As ScaledNumber
            Get
                Return m_snMaxVelocity
            End Get
            Set(ByVal value As ScaledNumber)
                'If value.ActualValue < m_snMinAngle.ActualValue Then
                '    Throw New System.Exception("The maximum angle cannot be less than the minimum angle.")
                'End If
                'If value.ActualValue < -180 Then
                '    Throw New System.Exception("The maximum angle cannot be less than -180 degrees.")
                'End If
                'If value.ActualValue > 180 Then
                '    Throw New System.Exception("The maximum angle cannot be greater than 180 degrees.")
                'End If

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

        Public Overridable Property Size() As ScaledNumber
            Get
                Return m_snSize
            End Get
            Set(ByVal value As ScaledNumber)
                If value.ActualValue <= 0 Then
                    Throw New System.Exception("The size cannot be less than or equal to zero.")
                End If

                SetSimData("Size", value.ActualValue.ToString, True)
                m_snSize.CopyData(value)
            End Set
        End Property

#End Region

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
            m_strDescription = ""

            m_doMinConstraint = New ConstraintLimit(Me)
            m_doMaxConstraint = New ConstraintLimit(Me)

            m_doMinConstraint.PairedLimit = m_doMaxConstraint
            m_doMaxConstraint.PairedLimit = m_doMinConstraint
            m_doMinConstraint.LimitDescription = "Minimum"
            m_doMaxConstraint.LimitDescription = "Maximum"

            m_snMinAngle = New AnimatGUI.Framework.ScaledNumber(Me, "MinRotationScaled", -45, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Degrees", "Deg")
            m_snMaxAngle = New AnimatGUI.Framework.ScaledNumber(Me, "MaxRotationScaled", 45, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Degrees", "Deg")
            m_snMaxTorque = New AnimatGUI.Framework.ScaledNumber(Me, "MaxTorque", 100, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Newton-Meters", "Nm")
            m_snMaxVelocity = New AnimatGUI.Framework.ScaledNumber(Me, "MaxVelocity", 100, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "rad/s", "rad/s")
            m_snSize = New AnimatGUI.Framework.ScaledNumber(Me, "Size", 2, AnimatGUI.Framework.ScaledNumber.enumNumericScale.centi, "Meters", "m")

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()
            If Not m_doMinConstraint Is Nothing Then m_doMinConstraint.ClearIsDirty()
            If Not m_doMaxConstraint Is Nothing Then m_doMaxConstraint.ClearIsDirty()

            If Not m_snMinAngle Is Nothing Then m_snMinAngle.ClearIsDirty()
            If Not m_snMaxAngle Is Nothing Then m_snMaxAngle.ClearIsDirty()
            If Not m_snMaxTorque Is Nothing Then m_snMaxTorque.ClearIsDirty()
            If Not m_snMaxVelocity Is Nothing Then m_snMaxVelocity.ClearIsDirty()
            If Not m_snSize Is Nothing Then m_snSize.ClearIsDirty()
        End Sub

        Public Overrides Function Clone(ByVal doParent As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject) As Framework.DataObject
            Dim oNewNode As New Joints.Hinge(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doOrig As Joints.Hinge = DirectCast(doOriginal, Joints.Hinge)
            m_doMinConstraint = DirectCast(doOrig.m_doMinConstraint.Clone(Me, bCutData, doRoot), ConstraintLimit)
            m_doMaxConstraint = DirectCast(doOrig.m_doMaxConstraint.Clone(Me, bCutData, doRoot), ConstraintLimit)

            m_snMinAngle = DirectCast(doOrig.m_snMinAngle.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snMaxAngle = DirectCast(doOrig.m_snMaxAngle.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_bEnableMotor = doOrig.m_bEnableMotor
            m_bServoMotor = doOrig.m_bServoMotor
            m_fltServoGain = doOrig.ServoGain
            m_snMaxTorque = DirectCast(doOrig.m_snMaxTorque.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snMaxVelocity = DirectCast(doOrig.m_snMaxVelocity.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snSize = DirectCast(doOrig.m_snSize.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag
            pbNumberBag = m_snMinAngle.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Minimum", pbNumberBag.GetType(), "MinAngle", _
                                        "Constraints", "Sets the minimum angle rotation that is allowed for this joint in degrees.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            'pbNumberBag = m_doMinConstraint.Properties
            'propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Min Limit", pbNumberBag.GetType(), "MinConstraint", _
            '                            "Part Properties", "Sets the values for the minimum angle constraint.", pbNumberBag, _
            '                            "", GetType(AnimatGUI.TypeHelpers.ConstrainLimitTypeConverter)))

            'pbNumberBag = m_doMaxConstraint.Properties
            'propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Max Limit", pbNumberBag.GetType(), "MaxConstraint", _
            '                            "Part Properties", "Sets the values for the maximum angle constraint.", pbNumberBag, _
            '                            "", GetType(AnimatGUI.TypeHelpers.ConstrainLimitTypeConverter)))

            pbNumberBag = m_snMinAngle.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Maximum", pbNumberBag.GetType(), "MaxAngle", _
                                        "Constraints", "Sets the maximum angle rotation that is allowed for this joint in degrees.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snMaxTorque.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Max Motor Torque", pbNumberBag.GetType(), "MaxTorque", _
                                        "Motor Properties", "Sets the maximum torque that this motor can apply to obtain a desired velocity of movement.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snMaxTorque.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Max Velocity", pbNumberBag.GetType(), "MaxVelocity", _
                                        "Motor Properties", "Sets the maximum positive or negative velocity that the motor can move this part.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Enable Motor", m_bEnableMotor.GetType(), "EnableMotor", _
                          "Motor Properties", "Sets whether the motor is enabled for this joint.", m_bEnableMotor))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Servo Motor", m_bServoMotor.GetType(), "ServoMotor", _
                          "Motor Properties", "Sets whether this is a servo or DC motor. If it is a servo then the Input specifies position, otherwise it specifies velocity.", m_bServoMotor))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Servo Gain", m_fltServoGain.GetType(), "ServoGain", _
                          "Motor Properties", "Sets the magnitude of the feedback gain for the servo motor.", m_fltServoGain))

            pbNumberBag = m_snSize.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Size", pbNumberBag.GetType(), "Size", _
                                        "Part Properties", "Sets the overall size of the part.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

        End Sub


        Public Overloads Overrides Sub LoadData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByRef oXml As Interfaces.StdXml)
            MyBase.LoadData(doStructure, oXml)

            oXml.IntoElem() 'Into Joint Element

            m_snMinAngle.LoadData(oXml, "MinAngle")
            m_snMaxAngle.LoadData(oXml, "MaxAngle")
            m_snMaxTorque.LoadData(oXml, "MaxTorque")
            m_snMaxTorque.LoadData(oXml, "MaxVelocity")
            m_snSize.LoadData(oXml, "Size")

            m_bEnableMotor = oXml.GetChildBool("EnableMotor", m_bEnableMotor)
            m_bServoMotor = oXml.GetChildBool("ServoMotor", m_bServoMotor)
            m_fltServoGain = oXml.GetChildFloat("ServoGain", m_fltServoGain)

            'based on whether this is a servo motor or not the incoming data type will change.
            If (m_bServoMotor) Then
                m_thIncomingDataType = New AnimatGUI.DataObjects.DataType("Position", "Position", "rad", "rad", -3.142, 3.142, ScaledNumber.enumNumericScale.None, ScaledNumber.enumNumericScale.None)
            Else
                m_thIncomingDataType = New AnimatGUI.DataObjects.DataType("DesiredVelocity", "Desired Velocity", "rad/s", "rad/s", -5, 5, ScaledNumber.enumNumericScale.None, ScaledNumber.enumNumericScale.None)
            End If

            oXml.OutOfElem() 'Outof Joint Element

        End Sub

        Public Overloads Overrides Sub SaveData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByRef oXml As Interfaces.StdXml)
            MyBase.SaveData(doStructure, oXml)

            oXml.IntoElem() 'Into Joint Elemement

            m_snMinAngle.SaveData(oXml, "MinAngle")
            m_snMaxAngle.SaveData(oXml, "MaxAngle")
            m_snMaxTorque.SaveData(oXml, "MaxTorque")
            m_snMaxTorque.SaveData(oXml, "MaxVelocity")
            m_snSize.SaveData(oXml, "Size")

            oXml.AddChildElement("EnableMotor", m_bEnableMotor)
            oXml.AddChildElement("ServoMotor", m_bServoMotor)
            oXml.AddChildElement("ServoGain", m_fltServoGain)

            oXml.OutOfElem() 'Outof Joint Element

        End Sub

        Public Overrides Sub SaveSimulationXml(ByRef oXml As Interfaces.StdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

            oXml.IntoElem()

            'm_snMinAngle.SaveSimulationXml(oXml, Me, "MinAngle")
            'm_snMaxAngle.SaveSimulationXml(oXml, Me, "MaxAngle")
            'm_snMaxTorque.SaveSimulationXml(oXml, Me, "MaxTorque")
            'm_snMaxTorque.SaveSimulationXml(oXml, Me, "MaxVelocity")
            'm_snSize.SaveSimulationXml(oXml, Me, "Size")

            'oXml.AddChildElement("EnableMotor", m_bEnableMotor)
            'oXml.AddChildElement("ServoMotor", m_bServoMotor)
            'oXml.AddChildElement("ServoGain", m_fltServoGain)

            oXml.OutOfElem()

        End Sub

    End Class


End Namespace

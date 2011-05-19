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

Namespace DataObjects.Physical.Bodies

    Public Class LinearHillMuscle
        Inherits Physical.Bodies.MuscleBase

#Region " Attributes "

        Protected m_snKse As ScaledNumber
        Protected m_snKpe As ScaledNumber
        Protected m_snB As ScaledNumber

        Protected m_snPeLengthPercentage As ScaledNumber
        Protected m_snMinPeLengthPercentage As ScaledNumber

        Protected m_snIbDischargeConstant As ScaledNumber

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.Muscle_Treeview.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonImageName() As String
            Get
                Return "AnimatGUI.Muscle_Button.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property Type() As String
            Get
                Return "LinearHillMuscle"
            End Get
        End Property

        Public Overrides ReadOnly Property PartType() As System.Type
            Get
                Return GetType(AnimatGUI.DataObjects.Physical.Bodies.LinearHillMuscle)
            End Get
        End Property

        Public Overridable Property Kse() As ScaledNumber
            Get
                Return m_snKse
            End Get
            Set(ByVal value As ScaledNumber)
                If Not value Is Nothing Then
                    If value.ActualValue <= 0 Then
                        Throw New System.Exception("The stiffness of the SE spring must be greater than zero.")
                    End If

                    SetSimData("Kse", value.ActualValue.ToString, True)
                    m_snKse.CopyData(value)
                End If
            End Set
        End Property

        Public Overridable Property Kpe() As ScaledNumber
            Get
                Return m_snKpe
            End Get
            Set(ByVal value As ScaledNumber)
                If Not value Is Nothing Then
                    If value.ActualValue <= 0 Then
                        Throw New System.Exception("The stiffness of the PE spring must be greater than zero.")
                    End If

                    SetSimData("Kpe", value.ActualValue.ToString, True)
                    m_snKpe.CopyData(value)
                End If
            End Set
        End Property

        Public Overridable Property B() As ScaledNumber
            Get
                Return m_snB
            End Get
            Set(ByVal value As ScaledNumber)
                If Not value Is Nothing Then
                    If value.ActualValue <= 0 Then
                        Throw New System.Exception("The damping must be greater than zero.")
                    End If

                    SetSimData("B", value.ActualValue.ToString, True)
                    m_snB.CopyData(value)
                End If
            End Set
        End Property

        Public Overridable Property PeLengthPercentage() As ScaledNumber
            Get
                Return m_snPeLengthPercentage
            End Get
            Set(ByVal value As ScaledNumber)
                If Not value Is Nothing Then
                    If value.ActualValue <= 0 OrElse value.ActualValue >= 100 Then
                        Throw New System.Exception("The Pe Length percentage must be between 0 and 100.")
                    End If

                    If m_snMinPeLengthPercentage.ActualValue > value.ActualValue Then
                        Throw New System.Exception("The Pe Length percentage can not be made lower than the minimum pe length percentage.")
                    End If

                    SetSimData("PeLengthPercentage", value.ActualValue.ToString, True)
                    m_snPeLengthPercentage.CopyData(value)
                End If
            End Set
        End Property

        Public Overridable Property MinPeLengthPercentage() As ScaledNumber
            Get
                Return m_snMinPeLengthPercentage
            End Get
            Set(ByVal value As ScaledNumber)
                If Not value Is Nothing Then
                    If value.ActualValue <= 0 OrElse value.ActualValue >= 100 Then
                        Throw New System.Exception("The Pe Length percentage must be between 0 and 100.")
                    End If

                    If m_snPeLengthPercentage.ActualValue < value.ActualValue Then
                        Throw New System.Exception("The Min Pe Length percentage can not be made greater than the pe length percentage.")
                    End If

                    SetSimData("MinPeLengthPercentage", value.ActualValue.ToString, True)
                    m_snMinPeLengthPercentage.CopyData(value)
                End If
            End Set
        End Property

        Public Overridable Property IbDischargeConstant() As ScaledNumber
            Get
                Return m_snIbDischargeConstant
            End Get
            Set(ByVal value As ScaledNumber)
                If Not value Is Nothing Then
                    If value.ActualValue < 0 Then
                        Throw New System.Exception("The Ib discharge rate must be greater than or equal to zero.")
                    End If

                    SetSimData("IbDischargeConstant", value.ActualValue.ToString, True)
                    m_snIbDischargeConstant.CopyData(value)
                End If

            End Set
        End Property

#End Region

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            m_thDataTypes.DataTypes.Clear()

            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("Tension", "Tension", "Newtons", "N", 0, 1000))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("Tdot", "Change in Tension", "Newtons per second", "N/s", 0, 1000))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("MuscleLength", "Muscle Length", "Meters", "m", 0, 1))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("SeLength", "Se Length", "Meters", "m", 0, 1))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("SeDisplacement", "Se Displacement", "Meters", "m", 0, 1))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("PeLength", "Pe Length", "Meters", "m", 0, 1))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("A", "A", "Newtons", "N", 0, 1000))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("Activation", "Activation", "", "", 0, 1000))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("Vmuscle", "Muscle Velocity", "m/s", "m/s", -3, 3))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("Vse", "Se Velocity", "m/s", "m/s", -3, 3))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("Vpe", "Pe Velocity", "m/s", "m/s", -3, 3))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("AvgVmuscle", "Averaged Muscle Velocity", "m/s", "m/s", -3, 3))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("MembraneVoltage", "Membrane Voltage", "Volts", "V", -100, 100, ScaledNumber.enumNumericScale.milli, ScaledNumber.enumNumericScale.milli))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("Displacement", "Muscle Displacement", "Meters", "m", 0, 1))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("DisplacementRatio", "Muscle Displacement Ratio", "", "", -1, 1))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("Enable", "Enable", "", "", 0, 1))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("Ib", "Ib Discharge Rate", "Spikes/s", "Spikes/s", 0, 1000))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("Tl", "Tension-Length Percentage", "%", "%", 0, 1000))
            m_thDataTypes.ID = "Tension"

            m_thIncomingDataType = New AnimatGUI.DataObjects.DataType("MembraneVoltage", "Membrane Voltage", "Volts", "V", -100, 100, ScaledNumber.enumNumericScale.milli, ScaledNumber.enumNumericScale.milli)

            m_StimTension = New AnimatGUI.DataObjects.Gains.MuscleGains.StimulusTension(Me)
            m_LengthTension = New AnimatGUI.DataObjects.Gains.MuscleGains.LengthTension(Me)
            m_LengthTension.ShowLcePercentage = False

            m_snMaxTension = New AnimatGUI.Framework.ScaledNumber(Me, "MaxTension", 100, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Newtons", "N")
            m_snMuscleLength = New AnimatGUI.Framework.ScaledNumber(Me, "MuscleLength", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Meters", "m")
            m_snKse = New AnimatGUI.Framework.ScaledNumber(Me, "Kse", 10, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Newtons per meter", "N/m")
            m_snKpe = New AnimatGUI.Framework.ScaledNumber(Me, "Kpe", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Newtons per meter", "N/m")
            m_snB = New AnimatGUI.Framework.ScaledNumber(Me, "B", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Newton-seconds per meter", "Ns/m")

            m_snPeLengthPercentage = New AnimatGUI.Framework.ScaledNumber(Me, "PeLengthPercentage", 90, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "%", "%")
            m_snMinPeLengthPercentage = New AnimatGUI.Framework.ScaledNumber(Me, "MinPeLengthPercentage", 5, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "%", "%")

            m_snIbDischargeConstant = New AnimatGUI.Framework.ScaledNumber(Me, "Ib Discharge Constant", 100, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Spikes/sN", "Spikes/sN")

        End Sub

        Public Overrides Function Clone(ByVal doParent As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject) As Framework.DataObject
            Dim oNewNode As New Bodies.LinearHillMuscle(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doOrigBody As LinearHillMuscle = DirectCast(doOriginal, LinearHillMuscle)

            m_snKse = DirectCast(doOrigBody.m_snKse.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snKpe = DirectCast(doOrigBody.m_snKpe.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snB = DirectCast(doOrigBody.m_snB.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snPeLengthPercentage = DirectCast(doOrigBody.m_snPeLengthPercentage.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snMinPeLengthPercentage = DirectCast(doOrigBody.m_snMinPeLengthPercentage.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snIbDischargeConstant = DirectCast(doOrigBody.m_snIbDischargeConstant.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)

        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)


            Dim pbSubBag As AnimatGuiCtrls.Controls.PropertyBag = m_StimTension.Properties
            propTable.Properties.Add(New PropertySpec("Stimulus-Tension", pbSubBag.GetType(), _
                          "StimTension", "Muscle Properties", "Sets the stmilus-tension properties of the muscle.", pbSubBag, _
                        GetType(AnimatGUI.TypeHelpers.GainTypeEditor), GetType(AnimatGuiCtrls.Controls.ExpandablePropBagConverter)))

            pbSubBag = m_LengthTension.Properties
            propTable.Properties.Add(New PropertySpec("Length-Tension", pbSubBag.GetType(), _
                        "LengthTension", "Muscle Properties", "Sets the length-tension properties of the muscle.", pbSubBag, _
                         GetType(AnimatGUI.TypeHelpers.GainTypeEditor), GetType(AnimatGuiCtrls.Controls.ExpandablePropBagConverter)))

            pbSubBag = m_snKse.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Kse", pbSubBag.GetType(), "Kse", _
                  "Muscle Properties", "Determines the stiffness of the SE spring element. This is the primarily the stiffness of the tendon.", _
                  pbSubBag, "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbSubBag = m_snKpe.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Kpe", pbSubBag.GetType(), "Kpe", _
                          "Muscle Properties", "Determines the stiffness of the PE spring element. This controls force developed from passive stretch of the muscle.", pbSubBag, _
                          "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbSubBag = m_snB.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("B", pbSubBag.GetType(), "B", _
                   "Muscle Properties", "Determines the linear, viscous damping of this muscle. This model does NOT use a non-linear hill force-velocity curve.", pbSubBag, _
                   "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbSubBag = m_snPeLengthPercentage.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Pe Length Percentage", pbSubBag.GetType(), "PeLengthPercentage", _
                          "Muscle Properties", "The percentage of the resting length of the muscle that Pe section takes up.", pbSubBag, _
                          "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbSubBag = m_snMinPeLengthPercentage.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Min Pe Length Percentage", pbSubBag.GetType(), "MinPeLengthPercentage", _
                          "Muscle Properties", "The minimum length, as a percentage of resting length, that the Pe section can go attain.", pbSubBag, _
                          "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            'TODO
            'pbSubBag = m_aryAttachmentPoints.Properties
            'propTable.Properties.Add(New PropertySpec("Calculate Stimulus", pbSubBag.GetType(), "", _
            '       "Muscle Properties", "Used to calculate the stimulus needed to develop a specific tension.", pbSubBag, _
            '       GetType(AnimatGUI.TypeHelpers.CalcMuscleEditor), GetType(AnimatGuiCtrls.Controls.ExpandablePropBagConverter)))

            pbSubBag = m_snIbDischargeConstant.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Ib Discharge Constant", pbSubBag.GetType(), "IbDischargeConstant", _
                   "Muscle Properties", "Relates the muscle tension to the discharge rate of the type Ib fibers.", pbSubBag, _
                   "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))
        End Sub

        Public Overloads Overrides Sub LoadData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByRef oXml As Interfaces.StdXml)
            MyBase.LoadData(doStructure, oXml)

            oXml.IntoElem() 'Into RigidBody Element

            m_StimTension.LoadData(oXml, "StimulusTension", "StimTension")
            m_LengthTension.LoadData(oXml, "LengthTension", "LengthTension")
            m_snKse.LoadData(oXml, "Kse")
            m_snKpe.LoadData(oXml, "Kpe")
            m_snB.LoadData(oXml, "B")
            m_snPeLengthPercentage.LoadData(oXml, "PeLength")
            m_snMinPeLengthPercentage.LoadData(oXml, "MinPeLength")
            m_snIbDischargeConstant.LoadData(oXml, "IbDischarge")

            oXml.OutOfElem() 'Outof RigidBody Element

        End Sub

        Public Overloads Overrides Sub SaveData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByRef oXml As Interfaces.StdXml)
            MyBase.SaveData(doStructure, oXml)

            oXml.IntoElem() 'Into Child Elemement

            m_StimTension.SaveData(oXml, "StimulusTension")
            m_LengthTension.SaveData(oXml, "LengthTension")
            m_snKse.SaveData(oXml, "Kse")
            m_snKpe.SaveData(oXml, "Kpe")
            m_snB.SaveData(oXml, "B")
            m_snPeLengthPercentage.SaveData(oXml, "PeLength")
            m_snMinPeLengthPercentage.SaveData(oXml, "MinPeLength")
            m_snIbDischargeConstant.SaveData(oXml, "IbDischarge")

            oXml.OutOfElem() 'Outof BodyPart Element

        End Sub

        Public Overrides Sub SaveSimulationXml(ByRef oXml As Interfaces.StdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

            oXml.IntoElem()

            m_StimTension.SaveSimulationXml(oXml, Me, "StimulusTension")
            m_LengthTension.SaveSimulationXml(oXml, Me, "LengthTension")
            m_snKse.SaveSimulationXml(oXml, Me, "Kse")
            m_snKpe.SaveSimulationXml(oXml, Me, "Kpe")
            m_snB.SaveSimulationXml(oXml, Me, "B")
            m_snPeLengthPercentage.SaveSimulationXml(oXml, Me, "PeLength")
            m_snMinPeLengthPercentage.SaveSimulationXml(oXml, Me, "MinPeLength")
            m_snIbDischargeConstant.SaveSimulationXml(oXml, Me, "IbDischarge")

            oXml.OutOfElem()

        End Sub

    End Class


End Namespace
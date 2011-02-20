Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports Crownwood.Magic.Common
Imports Crownwood.Magic.Controls
Imports Crownwood.Magic.Docking
Imports Crownwood.Magic.Menus
Imports AnimatTools.Framework

Namespace DataObjects.ExternalStimuli

    Public Class PostureControl
        Inherits AnimatTools.DataObjects.ExternalStimuli.Stimulus

#Region " Attributes "

        Protected m_doOrganism As AnimatTools.DataObjects.Physical.Organism

        Protected m_snBeta As AnimatTools.Framework.ScaledNumber
        Protected m_snDelta As AnimatTools.Framework.ScaledNumber
        Protected m_snPitch As AnimatTools.Framework.ScaledNumber

        Protected m_snAbDelay As AnimatTools.Framework.ScaledNumber
        Protected m_snAbPropGain As AnimatTools.Framework.ScaledNumber
        Protected m_snAbPeriod As AnimatTools.Framework.ScaledNumber
        Protected m_snLegPeriod As AnimatTools.Framework.ScaledNumber
        Protected m_bEnableAbControl As Boolean = True
        Protected m_bLockAbJump As Boolean = True
        Protected m_bTumblingSetup As Boolean = False

        Protected m_fltFrontRearLegJointHeight As Double = 0
        Protected m_fltEffectiveCoxaLength As Double = 0

        Protected m_doDorsalAbNeuron As AnimatTools.DataObjects.Behavior.Node
        'Protected m_doVentralAbNeuron As AnimatTools.DataObjects.Behavior.Node

        'Attachments that are used to calculate the beta angles
        Protected m_doThorax As AnimatTools.DataObjects.Physical.RigidBody
        Protected m_doAb1 As AnimatTools.DataObjects.Physical.RigidBody
        Protected m_doLeftTendonLock As AnimatTools.DataObjects.Physical.RigidBody
        Protected m_doRightTendonLock As AnimatTools.DataObjects.Physical.RigidBody

        Protected m_doLeftThoraxCoxaBeta As AnimatTools.DataObjects.Physical.RigidBody
        Protected m_doLeftSemilunarBeta As AnimatTools.DataObjects.Physical.RigidBody
        Protected m_doLeftTibiaBeta As AnimatTools.DataObjects.Physical.RigidBody
        Protected m_doRightThoraxCoxaBeta As AnimatTools.DataObjects.Physical.RigidBody
        Protected m_doRightSemilunarBeta As AnimatTools.DataObjects.Physical.RigidBody
        Protected m_doRightTibiaBeta As AnimatTools.DataObjects.Physical.RigidBody

        Protected m_doLeftRearCoxa As AnimatTools.DataObjects.Physical.RigidBody
        Protected m_doLeftRearFemur As AnimatTools.DataObjects.Physical.RigidBody
        Protected m_doLeftRearTibia As AnimatTools.DataObjects.Physical.RigidBody
        Protected m_doLeftMiddleFemur As AnimatTools.DataObjects.Physical.RigidBody
        Protected m_doLeftMiddleTibia As AnimatTools.DataObjects.Physical.RigidBody
        Protected m_doLeftFrontFemur As AnimatTools.DataObjects.Physical.RigidBody
        Protected m_doLeftFrontTibia As AnimatTools.DataObjects.Physical.RigidBody

        Protected m_doHeadAxisRef As AnimatTools.DataObjects.Physical.RigidBody
        Protected m_doTailAxisRef As AnimatTools.DataObjects.Physical.RigidBody
        Protected m_doCOMRef As AnimatTools.DataObjects.Physical.RigidBody
        Protected m_doRollAxisRef As AnimatTools.DataObjects.Physical.RigidBody
        Protected m_doPronotumFrontRef As AnimatTools.DataObjects.Physical.RigidBody
        Protected m_doPronotumRearRef As AnimatTools.DataObjects.Physical.RigidBody

        Protected m_doLeftFrontCoxaFemur As AnimatTools.DataObjects.Physical.Joint
        Protected m_doLeftFrontFemurTibia As AnimatTools.DataObjects.Physical.Joint
        Protected m_doLeftFrontTibiaTarsus As AnimatTools.DataObjects.Physical.Joint

        Protected m_doLeftMiddleCoxaFemur As AnimatTools.DataObjects.Physical.Joint
        Protected m_doLeftMiddleFemurTibia As AnimatTools.DataObjects.Physical.Joint
        Protected m_doLeftMiddleTibiaTarsus As AnimatTools.DataObjects.Physical.Joint

        Protected m_doLeftRearThoracicCoxa As AnimatTools.DataObjects.Physical.Joint
        Protected m_doLeftRearCoxaFemur As AnimatTools.DataObjects.Physical.Joint
        Protected m_doLeftSemilunarJoint As AnimatTools.DataObjects.Physical.Joint
        Protected m_doLeftRearFemurTibia As AnimatTools.DataObjects.Physical.Joint
        Protected m_doLeftRearTibiaTarsus As AnimatTools.DataObjects.Physical.Joint

        Protected m_doRightFrontCoxaFemur As AnimatTools.DataObjects.Physical.Joint
        Protected m_doRightFrontFemurTibia As AnimatTools.DataObjects.Physical.Joint
        Protected m_doRightFrontTibiaTarsus As AnimatTools.DataObjects.Physical.Joint

        Protected m_doRightMiddleCoxaFemur As AnimatTools.DataObjects.Physical.Joint
        Protected m_doRightMiddleFemurTibia As AnimatTools.DataObjects.Physical.Joint
        Protected m_doRightMiddleTibiaTarsus As AnimatTools.DataObjects.Physical.Joint

        Protected m_doRightRearThoracicCoxa As AnimatTools.DataObjects.Physical.Joint
        Protected m_doRightRearCoxaFemur As AnimatTools.DataObjects.Physical.Joint
        Protected m_doRightSemilunarJoint As AnimatTools.DataObjects.Physical.Joint
        Protected m_doRightRearFemurTibia As AnimatTools.DataObjects.Physical.Joint
        Protected m_doRightRearTibiaTarsus As AnimatTools.DataObjects.Physical.Joint

        Protected m_doLeftMiddleFootDown As AnimatTools.DataObjects.Physical.RigidBody
        Protected m_doLeftFrontFootDown As AnimatTools.DataObjects.Physical.RigidBody
        Protected m_doRightMiddleFootDown As AnimatTools.DataObjects.Physical.RigidBody
        Protected m_doRightFrontFootDown As AnimatTools.DataObjects.Physical.RigidBody

        Protected m_doLeftRearTarsusDown As AnimatTools.DataObjects.Physical.RigidBody
        Protected m_doRightRearTarsusDown As AnimatTools.DataObjects.Physical.RigidBody

        Protected m_doAb1Joint As AnimatTools.DataObjects.Physical.Joint
        Protected m_doAb2Joint As AnimatTools.DataObjects.Physical.Joint
        Protected m_doAb3Joint As AnimatTools.DataObjects.Physical.Joint
        Protected m_doAb4Joint As AnimatTools.DataObjects.Physical.Joint

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Overridable Property Organism() As AnimatTools.DataObjects.Physical.Organism
            Get
                Return m_doOrganism
            End Get
            Set(ByVal Value As AnimatTools.DataObjects.Physical.Organism)
                m_doOrganism = Value

                SetBodyParts()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property OrganismName() As String
            Get
                If Not m_doOrganism Is Nothing Then
                    Return m_doOrganism.Name
                End If

                Return ""
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property StimulusModuleName() As String
            Get
                If Util.Application.Simulation.UseReleaseLibraries Then
                    Return "GrasshopperPosture_vc7.dll"
                Else
                    Return "GrasshopperPosture_vc7D.dll"
                End If
            End Get
        End Property

        Public Overrides ReadOnly Property StimulusClassType() As String
            Get
                Return "PostureControl"
            End Get
        End Property

        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "PostureControl"
            End Get
        End Property

        Public Overrides ReadOnly Property ImageName() As String
            Get
                Return "GrasshopperGrid.Grasshopper.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property Description() As String
            Get
                Return "This stimulus allows you to control the posutre of a grasshopper model."
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property WorkspaceNodeAssemblyName() As String
            Get
                Return "GrasshopperGrid"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property WorkspaceNodeImageName() As String
            Get
                Return "GrasshopperGrid.Grasshopper.gif"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property StimulusNoLongerValid() As Boolean
            Get
                Dim doStruct As AnimatTools.DataObjects.Physical.PhysicalStructure

                Try
                    doStruct = Util.Environment.FindStructureFromAll(m_doOrganism.ID, False)
                Catch ex As System.Exception
                End Try

                If Not doStruct Is Nothing Then
                    Return False
                Else
                    Return True
                End If
            End Get
        End Property

        Public Overrides ReadOnly Property CanBeCharted() As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overrides ReadOnly Property DragImageName() As String
            Get
                Return Me.ImageName
            End Get
        End Property

        Public Overrides ReadOnly Property DataColumnClassType() As String
            Get
                Return "StimulusData"
            End Get
        End Property

        Public Overrides ReadOnly Property StructureID() As String
            Get
                If Not Me.Organism Is Nothing Then
                    Return Me.Organism.ID
                Else
                    Return ""
                End If
            End Get
        End Property

        Public Overridable Property Beta() As AnimatTools.Framework.ScaledNumber
            Get
                Return m_snBeta
            End Get
            Set(ByVal Value As AnimatTools.Framework.ScaledNumber)
                If Value.ActualValue < 0 OrElse Value.ActualValue > 100 Then
                    Throw New System.Exception("The beta angle must be between the range 0 and 100 degrees.")
                End If

                m_snBeta.CopyData(Value)
            End Set
        End Property

        Public Overridable Property Delta() As AnimatTools.Framework.ScaledNumber
            Get
                Return m_snDelta
            End Get
            Set(ByVal Value As AnimatTools.Framework.ScaledNumber)
                If Value.ActualValue < -100 OrElse Value.ActualValue > 100 Then
                    Throw New System.Exception("The delta angle must be between the range -30 and 30 degrees.")
                End If

                m_snDelta.CopyData(Value)
            End Set
        End Property

        Public Overridable Property Pitch() As AnimatTools.Framework.ScaledNumber
            Get
                Return m_snPitch
            End Get
            Set(ByVal Value As AnimatTools.Framework.ScaledNumber)
                If Value.ActualValue < -20 OrElse Value.ActualValue > 40 Then
                    Throw New System.Exception("The pitch angle must be between the range -20 and 40 degrees.")
                End If

                m_snPitch.CopyData(Value)
            End Set
        End Property

        Public Overridable Property EnableAbControl() As Boolean
            Get
                Return m_bEnableAbControl
            End Get
            Set(ByVal Value As Boolean)
                m_bEnableAbControl = Value
            End Set
        End Property

        Public Overridable Property LockAbJump() As Boolean
            Get
                Return m_bLockAbJump
            End Get
            Set(ByVal Value As Boolean)
                m_bLockAbJump = Value
            End Set
        End Property

        Public Overridable Property AbDelay() As AnimatTools.Framework.ScaledNumber
            Get
                Return m_snAbDelay
            End Get
            Set(ByVal Value As AnimatTools.Framework.ScaledNumber)
                If Value.ActualValue < 0 OrElse Value.ActualValue > 1 Then
                    Throw New System.Exception("The beta angle must be between the range 0 and 1 second.")
                End If

                m_snAbDelay.CopyData(Value)
            End Set
        End Property

        Public Overridable Property AbPropGain() As AnimatTools.Framework.ScaledNumber
            Get
                Return m_snAbPropGain
            End Get
            Set(ByVal Value As AnimatTools.Framework.ScaledNumber)
                m_snAbPropGain.CopyData(Value)
            End Set
        End Property

        Public Overridable Property AbPeriod() As AnimatTools.Framework.ScaledNumber
            Get
                Return m_snAbPeriod
            End Get
            Set(ByVal Value As AnimatTools.Framework.ScaledNumber)
                m_snAbPeriod.CopyData(Value)
            End Set
        End Property

        Public Overridable Property LegPeriod() As AnimatTools.Framework.ScaledNumber
            Get
                Return m_snLegPeriod
            End Get
            Set(ByVal Value As AnimatTools.Framework.ScaledNumber)
                m_snLegPeriod.CopyData(Value)
            End Set
        End Property

        Public Overridable Property TumblingSetup() As Boolean
            Get
                Return m_bTumblingSetup
            End Get
            Set(ByVal Value As Boolean)
                m_bTumblingSetup = Value
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatTools.Framework.DataObject)
            MyBase.New(doParent)

            m_snBeta = New AnimatTools.Framework.ScaledNumber(Me, "Beta", 45, AnimatTools.Framework.ScaledNumber.enumNumericScale.None, "Degrees", "Degrees")
            m_snDelta = New AnimatTools.Framework.ScaledNumber(Me, "Delta", 0, AnimatTools.Framework.ScaledNumber.enumNumericScale.None, "Degrees", "Degrees")
            m_snPitch = New AnimatTools.Framework.ScaledNumber(Me, "Pitch", 0, AnimatTools.Framework.ScaledNumber.enumNumericScale.None, "Degrees", "Degrees")

            m_snAbDelay = New AnimatTools.Framework.ScaledNumber(Me, "AbDelay", 0, AnimatTools.Framework.ScaledNumber.enumNumericScale.milli, "Seconds", "s")
            m_snAbPropGain = New AnimatTools.Framework.ScaledNumber(Me, "AbPropGain", 0, AnimatTools.Framework.ScaledNumber.enumNumericScale.None, "", "")
            m_snAbPeriod = New AnimatTools.Framework.ScaledNumber(Me, "AbPeriod", 0, AnimatTools.Framework.ScaledNumber.enumNumericScale.milli, "Seconds", "s")
            m_snLegPeriod = New AnimatTools.Framework.ScaledNumber(Me, "LegPeriod", 0, AnimatTools.Framework.ScaledNumber.enumNumericScale.milli, "Seconds", "s")

            m_thDataTypes.DataTypes.Add(New AnimatTools.DataObjects.DataType("LeftBetaD", "Left Beta Degrees", "Degrees", "D", -180, 180))
            m_thDataTypes.DataTypes.Add(New AnimatTools.DataObjects.DataType("RightBetaD", "Right Beta Degrees", "Degrees", "D", -180, 180))
            m_thDataTypes.DataTypes.Add(New AnimatTools.DataObjects.DataType("LeftBetaR", "Left Beta Radians", "Radians", "rad", -3.141, 3.141))
            m_thDataTypes.DataTypes.Add(New AnimatTools.DataObjects.DataType("RightBetaR", "Right Beta Radians", "Radians", "rad", -3.141, 3.141))
            m_thDataTypes.DataTypes.Add(New AnimatTools.DataObjects.DataType("LeftOftD", "Left Oft Degrees", "Degrees", "D", -180, 180))
            m_thDataTypes.DataTypes.Add(New AnimatTools.DataObjects.DataType("RightOftD", "Right Oft Degrees", "Degrees", "D", -180, 180))
            m_thDataTypes.DataTypes.Add(New AnimatTools.DataObjects.DataType("LeftOftR", "Left Oft Radians", "Radians", "rad", -3.141, 3.141))
            m_thDataTypes.DataTypes.Add(New AnimatTools.DataObjects.DataType("RightOftR", "Right Oft Radians", "Radians", "rad", -3.141, 3.141))
            m_thDataTypes.DataTypes.Add(New AnimatTools.DataObjects.DataType("PitchD", "Pitch Degrees", "Degrees", "D", -180, 180))
            m_thDataTypes.DataTypes.Add(New AnimatTools.DataObjects.DataType("PitchR", "Pitch Radians", "Radians", "rad", -3.141, 3.141))
            m_thDataTypes.DataTypes.Add(New AnimatTools.DataObjects.DataType("HeadPitchD", "Head Pitch Degrees", "Degrees", "D", -180, 180))
            m_thDataTypes.DataTypes.Add(New AnimatTools.DataObjects.DataType("YawD", "Yaw Degrees", "Degrees", "D", -180, 180))
            m_thDataTypes.DataTypes.Add(New AnimatTools.DataObjects.DataType("YawR", "Yaw Radians", "Radians", "rad", -3.141, 3.141))
            m_thDataTypes.DataTypes.Add(New AnimatTools.DataObjects.DataType("RollD", "Roll Degrees", "Degrees", "D", -180, 180))
            m_thDataTypes.DataTypes.Add(New AnimatTools.DataObjects.DataType("RollR", "Roll Radians", "Radians", "rad", -3.141, 3.141))
            m_thDataTypes.DataTypes.Add(New AnimatTools.DataObjects.DataType("LeftBetaChange", "Left Beta Change", "Radians", "rad", -3.141, 3.141))
            m_thDataTypes.DataTypes.Add(New AnimatTools.DataObjects.DataType("RightBetaChange", "Right Beta Change", "Radians", "rad", -3.141, 3.141))
            m_thDataTypes.DataTypes.Add(New AnimatTools.DataObjects.DataType("PitchChange", "Pitch Change", "Radians", "rad", -3.141, 3.141))
            m_thDataTypes.DataTypes.Add(New AnimatTools.DataObjects.DataType("LeftGammaD", "Left Gamma Degrees", "Degrees", "D", -180, 180))
            m_thDataTypes.DataTypes.Add(New AnimatTools.DataObjects.DataType("RightGammaD", "Right Gamma Degrees", "Degrees", "D", -180, 180))
            m_thDataTypes.DataTypes.Add(New AnimatTools.DataObjects.DataType("LeftGammaR", "Left Gamma Radians", "Radians", "rad", -3.141, 3.141))
            m_thDataTypes.DataTypes.Add(New AnimatTools.DataObjects.DataType("RightGammaR", "Right Gamma Radians", "Radians", "rad", -3.141, 3.141))
            m_thDataTypes.DataTypes.Add(New AnimatTools.DataObjects.DataType("AlphaR", "Alpha Radians", "Radians", "rad", -3.141, 3.141))
            m_thDataTypes.DataTypes.Add(New AnimatTools.DataObjects.DataType("AlphaD", "Alpha Degrees", "Degrees", "D", -180, 180))
            m_thDataTypes.DataTypes.Add(New AnimatTools.DataObjects.DataType("CoxaTibiaGap", "Coxa-Tibia Gap Length", "Meters", "m", -1, 1))
            m_thDataTypes.DataTypes.Add(New AnimatTools.DataObjects.DataType("PitchVelD", "Pitch Velocity Degrees", "Degrees/s", "D/s", -1000, 1000))
            m_thDataTypes.DataTypes.Add(New AnimatTools.DataObjects.DataType("PitchVelR", "Pitch Velocity Radians", "Radians/s", "rad/s", -1000, 1000))
            m_thDataTypes.ID = "RightBetaD"

        End Sub

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatTools.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatTools.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doPart As DataObjects.ExternalStimuli.PostureControl = DirectCast(doOriginal, DataObjects.ExternalStimuli.PostureControl)

            m_doOrganism = doPart.m_doOrganism
            m_snBeta = DirectCast(doPart.m_snBeta.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snDelta = DirectCast(doPart.m_snDelta.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snPitch = DirectCast(doPart.m_snPitch.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snAbDelay = DirectCast(doPart.m_snAbDelay.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snAbPropGain = DirectCast(doPart.m_snAbPropGain.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snAbPeriod = DirectCast(doPart.m_snAbPeriod.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snLegPeriod = DirectCast(doPart.m_snLegPeriod.Clone(Me, bCutData, doRoot), ScaledNumber)

            m_bTumblingSetup = doPart.m_bTumblingSetup
            m_bEnableAbControl = doPart.m_bEnableAbControl
            m_bLockAbJump = doPart.m_bLockAbJump

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatTools.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatTools.Framework.DataObject) As AnimatTools.Framework.DataObject
            Dim doStim As DataObjects.ExternalStimuli.PostureControl = New DataObjects.ExternalStimuli.PostureControl(doParent)
            CloneInternal(doStim, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then doStim.AfterClone(Me, bCutData, doRoot, doStim)
            Return doStim
        End Function

        Public Overrides Function SaveStimulusToXml() As String

            Dim oXml As New AnimatTools.Interfaces.StdXml

            If m_doOrganism Is Nothing Then
                Throw New System.Exception("No organism was defined for the stimulus '" & m_strName & "'.")
            End If

            oXml.AddElement("Stimuli")
            SaveXml(oXml)

            Return oXml.Serialize()
        End Function


        Public Overrides Sub SaveXml(ByRef oXml As AnimatTools.Interfaces.StdXml)

            If m_doOrganism Is Nothing Then
                Throw New System.Exception("No organism was defined for the stimulus '" & m_strName & "'.")
            End If

            oXml.AddChildElement("Stimulus")

            oXml.IntoElem()
            oXml.AddChildElement("ID", m_strID)
            oXml.AddChildElement("Name", m_strName)
            oXml.AddChildElement("AlwaysActive", m_bAlwaysActive)

            oXml.AddChildElement("ModuleName", Me.StimulusModuleName)
            oXml.AddChildElement("Type", Me.StimulusClassType)

            oXml.AddChildElement("StructureID", m_doOrganism.ID)

            'Convert these angles to radians.
            Dim fltBeta As Single, fltDelta As Single, fltPitch As Single

            fltBeta = CSng(((m_snBeta.ActualValue) * Math.PI) / 180)
            fltDelta = CSng((m_snDelta.ActualValue * Math.PI) / 180)
            fltPitch = CSng(((m_snPitch.ActualValue + 5) * Math.PI) / 180)

            oXml.AddChildElement("Beta", fltBeta)
            oXml.AddChildElement("Delta", fltDelta)
            oXml.AddChildElement("Pitch", fltPitch)
            oXml.AddChildElement("EnableAbControl", m_bEnableAbControl)
            oXml.AddChildElement("LockAbJump", m_bLockAbJump)
            oXml.AddChildElement("TumblingSetup", m_bTumblingSetup)

            oXml.AddChildElement("AbDelay", m_snAbDelay.ActualValue)
            oXml.AddChildElement("AbPropGain", m_snAbPropGain.ActualValue)
            oXml.AddChildElement("AbPeriod", m_snAbPeriod.ActualValue)
            oXml.AddChildElement("LegPeriod", m_snLegPeriod.ActualValue)

            oXml.AddChildElement("Thorax", m_doThorax.ID)
            oXml.AddChildElement("Ab1", m_doAb1.ID)
            oXml.AddChildElement("LeftTendonLock", m_doLeftTendonLock.ID)
            oXml.AddChildElement("RightTendonLock", m_doRightTendonLock.ID)

            oXml.AddChildElement("LeftThoraxCoxaBeta", m_doLeftThoraxCoxaBeta.ID)
            oXml.AddChildElement("LeftSemilunarBeta", m_doLeftSemilunarBeta.ID)
            oXml.AddChildElement("LeftTibiaBeta", m_doLeftTibiaBeta.ID)
            oXml.AddChildElement("RightThoraxCoxaBeta", m_doRightThoraxCoxaBeta.ID)
            oXml.AddChildElement("RightSemilunarBeta", m_doRightSemilunarBeta.ID)
            oXml.AddChildElement("RightTibiaBeta", m_doRightTibiaBeta.ID)

            oXml.AddChildElement("LeftFrontCoxaFemur", m_doLeftFrontCoxaFemur.ID)
            oXml.AddChildElement("LeftFrontFemurTibia", m_doLeftFrontFemurTibia.ID)
            oXml.AddChildElement("LeftFrontTibiaTarsus", m_doLeftFrontTibiaTarsus.ID)

            oXml.AddChildElement("LeftMiddleCoxaFemur", m_doLeftMiddleCoxaFemur.ID)
            oXml.AddChildElement("LeftMiddleFemurTibia", m_doLeftMiddleFemurTibia.ID)
            oXml.AddChildElement("LeftMiddleTibiaTarsus", m_doLeftMiddleTibiaTarsus.ID)

            oXml.AddChildElement("LeftRearThoracicCoxa", m_doLeftRearThoracicCoxa.ID)
            oXml.AddChildElement("LeftRearCoxaFemur", m_doLeftRearCoxaFemur.ID)
            oXml.AddChildElement("LeftSemilunarJoint", m_doLeftSemilunarJoint.ID)
            oXml.AddChildElement("LeftRearFemurTibia", m_doLeftRearFemurTibia.ID)
            oXml.AddChildElement("LeftRearTibiaTarsus", m_doLeftRearTibiaTarsus.ID)

            oXml.AddChildElement("RightFrontCoxaFemur", m_doRightFrontCoxaFemur.ID)
            oXml.AddChildElement("RightFrontFemurTibia", m_doRightFrontFemurTibia.ID)
            oXml.AddChildElement("RightFrontTibiaTarsus", m_doRightFrontTibiaTarsus.ID)

            oXml.AddChildElement("RightMiddleCoxaFemur", m_doRightMiddleCoxaFemur.ID)
            oXml.AddChildElement("RightMiddleFemurTibia", m_doRightMiddleFemurTibia.ID)
            oXml.AddChildElement("RightMiddleTibiaTarsus", m_doRightMiddleTibiaTarsus.ID)

            oXml.AddChildElement("RightRearThoracicCoxa", m_doRightRearThoracicCoxa.ID)
            oXml.AddChildElement("RightRearCoxaFemur", m_doRightRearCoxaFemur.ID)
            oXml.AddChildElement("RightSemilunarJoint", m_doRightSemilunarJoint.ID)
            oXml.AddChildElement("RightRearFemurTibia", m_doRightRearFemurTibia.ID)
            oXml.AddChildElement("RightRearTibiaTarsus", m_doRightRearTibiaTarsus.ID)

            oXml.AddChildElement("LeftFrontFemur", m_doLeftFrontFemur.ID)
            oXml.AddChildElement("LeftFrontTibia", m_doLeftFrontTibia.ID)
            oXml.AddChildElement("LeftMiddleFemur", m_doLeftMiddleFemur.ID)
            oXml.AddChildElement("LeftMiddleTibia", m_doLeftMiddleTibia.ID)
            oXml.AddChildElement("LeftRearCoxa", m_doLeftRearCoxa.ID)
            oXml.AddChildElement("LeftRearFemur", m_doLeftRearFemur.ID)
            oXml.AddChildElement("LeftRearTibia", m_doLeftRearTibia.ID)

            oXml.AddChildElement("LeftMiddleFootDown", m_doLeftMiddleFootDown.ID)
            oXml.AddChildElement("LeftFrontFootDown", m_doLeftFrontFootDown.ID)
            oXml.AddChildElement("RightMiddleFootDown", m_doRightMiddleFootDown.ID)
            oXml.AddChildElement("RightFrontFootDown", m_doRightFrontFootDown.ID)

            If Not m_doLeftRearTarsusDown Is Nothing Then
                oXml.AddChildElement("LeftRearTarsusDown", m_doLeftRearTarsusDown.ID)
                oXml.AddChildElement("RightRearTarsusDown", m_doRightRearTarsusDown.ID)
            End If

            oXml.AddChildElement("HeadAxisRef", m_doHeadAxisRef.ID)
            oXml.AddChildElement("TailAxisRef", m_doTailAxisRef.ID)
            oXml.AddChildElement("COMRef", m_doCOMRef.ID)
            oXml.AddChildElement("RollAxisRef", m_doRollAxisRef.ID)

            If Not m_doPronotumFrontRef Is Nothing Then
                oXml.AddChildElement("PronotumFrontRef", m_doPronotumFrontRef.ID)
                oXml.AddChildElement("PronotumRearRef", m_doPronotumRearRef.ID)
            End If

            oXml.AddChildElement("Ab1Joint", m_doAb1Joint.ID)
            oXml.AddChildElement("Ab2Joint", m_doAb2Joint.ID)
            oXml.AddChildElement("Ab3Joint", m_doAb3Joint.ID)
            oXml.AddChildElement("Ab4Joint", m_doAb4Joint.ID)

            oXml.AddChildElement("StartTime", m_snStartTime.ActualValue)
            oXml.AddChildElement("EndTime", m_snEndTime.ActualValue)

            Dim dxLeftRearThoracicCoxa As VortexAnimatTools.DataObjects.Physical.Joints.Joint_DX = DirectCast(m_doLeftRearThoracicCoxa, VortexAnimatTools.DataObjects.Physical.Joints.Joint_DX)
            Dim dxLeftFrontCoxaFemur As VortexAnimatTools.DataObjects.Physical.Joints.Joint_DX = DirectCast(m_doLeftFrontCoxaFemur, VortexAnimatTools.DataObjects.Physical.Joints.Joint_DX)
            Dim dxLeftRearCoxaFemur As VortexAnimatTools.DataObjects.Physical.Joints.Joint_DX = DirectCast(m_doLeftRearCoxaFemur, VortexAnimatTools.DataObjects.Physical.Joints.Joint_DX)

            m_fltFrontRearLegJointHeight = dxLeftFrontCoxaFemur.AbsoluteLocation.Y - dxLeftRearThoracicCoxa.AbsoluteLocation.Y
            m_fltEffectiveCoxaLength = dxLeftRearThoracicCoxa.AbsoluteLocation.Z - dxLeftRearCoxaFemur.AbsoluteLocation.Z

            If Not m_doDorsalAbNeuron Is Nothing Then
                oXml.AddChildElement("DorsalAbNeuron", m_doDorsalAbNeuron.NodeIndex)
            Else
                oXml.AddChildElement("DorsalAbNeuron", -1)
            End If

            'If Not m_doVentralAbNeuron Is Nothing Then
            '    oXml.AddChildElement("VentralAbNeuron", m_doVentralAbNeuron.NodeIndex)
            'Else
            '    oXml.AddChildElement("VentralAbNeuron", -1)
            'End If

            oXml.AddChildElement("FrontRearLegJointHeight", m_fltFrontRearLegJointHeight)
            oXml.AddChildElement("EffectiveCoxaLength", m_fltEffectiveCoxaLength)

            oXml.OutOfElem()

        End Sub

        Protected Sub SetBodyParts()

            'Lets get the associated body parts
            If Not m_doOrganism Is Nothing Then
                m_doThorax = DirectCast(m_doOrganism.FindBodyPartByName("Thorax"), AnimatTools.DataObjects.Physical.RigidBody)
                m_doAb1 = DirectCast(m_doOrganism.FindBodyPartByName("Ab1"), AnimatTools.DataObjects.Physical.RigidBody)
                m_doLeftTendonLock = DirectCast(m_doOrganism.FindBodyPartByName("Left Tendon Lock"), AnimatTools.DataObjects.Physical.RigidBody)
                m_doRightTendonLock = DirectCast(m_doOrganism.FindBodyPartByName("Right Tendon Lock"), AnimatTools.DataObjects.Physical.RigidBody)

                m_doLeftThoraxCoxaBeta = DirectCast(m_doOrganism.FindBodyPartByName("Left Coxa-Femur Beta Angle"), AnimatTools.DataObjects.Physical.RigidBody)
                m_doLeftSemilunarBeta = DirectCast(m_doOrganism.FindBodyPartByName("Left Semilunar Attach 2"), AnimatTools.DataObjects.Physical.RigidBody)
                m_doLeftTibiaBeta = DirectCast(m_doOrganism.FindBodyPartByName("Left Tibia Beta Angle"), AnimatTools.DataObjects.Physical.RigidBody)

                m_doRightThoraxCoxaBeta = DirectCast(m_doOrganism.FindBodyPartByName("Right Coxa-Femur Beta Angle"), AnimatTools.DataObjects.Physical.RigidBody)
                m_doRightSemilunarBeta = DirectCast(m_doOrganism.FindBodyPartByName("Right Semilunar Attach 2"), AnimatTools.DataObjects.Physical.RigidBody)
                m_doRightTibiaBeta = DirectCast(m_doOrganism.FindBodyPartByName("Right Tibia Beta Angle"), AnimatTools.DataObjects.Physical.RigidBody)

                m_doLeftFrontCoxaFemur = DirectCast(m_doOrganism.FindBodyPartByName("Left Front Coxa Femur"), AnimatTools.DataObjects.Physical.Joint)
                m_doLeftFrontFemurTibia = DirectCast(m_doOrganism.FindBodyPartByName("Left Front Femur Tibia"), AnimatTools.DataObjects.Physical.Joint)
                m_doLeftFrontTibiaTarsus = DirectCast(m_doOrganism.FindBodyPartByName("Left Front Tibia Tarsus"), AnimatTools.DataObjects.Physical.Joint)

                m_doLeftMiddleCoxaFemur = DirectCast(m_doOrganism.FindBodyPartByName("Left Middle Coxa Femur"), AnimatTools.DataObjects.Physical.Joint)
                m_doLeftMiddleFemurTibia = DirectCast(m_doOrganism.FindBodyPartByName("Left Middle Femur Tibia"), AnimatTools.DataObjects.Physical.Joint)
                m_doLeftMiddleTibiaTarsus = DirectCast(m_doOrganism.FindBodyPartByName("Left Middle Tibia Tarsus"), AnimatTools.DataObjects.Physical.Joint)

                m_doLeftRearThoracicCoxa = DirectCast(m_doOrganism.FindBodyPartByName("Left Rear Thoracic Coxa Joint"), AnimatTools.DataObjects.Physical.Joint)
                m_doLeftRearCoxaFemur = DirectCast(m_doOrganism.FindBodyPartByName("Left Rear Coxa Femur"), AnimatTools.DataObjects.Physical.Joint)
                m_doLeftSemilunarJoint = DirectCast(m_doOrganism.FindBodyPartByName("Left Semilunar Joint"), AnimatTools.DataObjects.Physical.Joint)
                m_doLeftRearFemurTibia = DirectCast(m_doOrganism.FindBodyPartByName("Left Rear Femur Tibia"), AnimatTools.DataObjects.Physical.Joint)
                m_doLeftRearTibiaTarsus = DirectCast(m_doOrganism.FindBodyPartByName("Left Rear Tibia Tarsus"), AnimatTools.DataObjects.Physical.Joint)

                m_doRightFrontCoxaFemur = DirectCast(m_doOrganism.FindBodyPartByName("Right Front Coxa Femur"), AnimatTools.DataObjects.Physical.Joint)
                m_doRightFrontFemurTibia = DirectCast(m_doOrganism.FindBodyPartByName("Right Front Femur Tibia"), AnimatTools.DataObjects.Physical.Joint)
                m_doRightFrontTibiaTarsus = DirectCast(m_doOrganism.FindBodyPartByName("Right Front Tibia Tarsus"), AnimatTools.DataObjects.Physical.Joint)

                m_doRightMiddleCoxaFemur = DirectCast(m_doOrganism.FindBodyPartByName("Right Middle Coxa Femur"), AnimatTools.DataObjects.Physical.Joint)
                m_doRightMiddleFemurTibia = DirectCast(m_doOrganism.FindBodyPartByName("Right Middle Femur Tibia"), AnimatTools.DataObjects.Physical.Joint)
                m_doRightMiddleTibiaTarsus = DirectCast(m_doOrganism.FindBodyPartByName("Right Middle Tibia Tarsus"), AnimatTools.DataObjects.Physical.Joint)

                m_doRightRearThoracicCoxa = DirectCast(m_doOrganism.FindBodyPartByName("Right Rear Thoracic Coxa Joint"), AnimatTools.DataObjects.Physical.Joint)
                m_doRightRearCoxaFemur = DirectCast(m_doOrganism.FindBodyPartByName("Right Rear Coxa Femur"), AnimatTools.DataObjects.Physical.Joint)
                m_doRightSemilunarJoint = DirectCast(m_doOrganism.FindBodyPartByName("Right Semilunar Joint"), AnimatTools.DataObjects.Physical.Joint)
                m_doRightRearFemurTibia = DirectCast(m_doOrganism.FindBodyPartByName("Right Rear Femur Tibia"), AnimatTools.DataObjects.Physical.Joint)
                m_doRightRearTibiaTarsus = DirectCast(m_doOrganism.FindBodyPartByName("Right Rear Tibia Tarsus"), AnimatTools.DataObjects.Physical.Joint)

                m_doLeftFrontFemur = DirectCast(m_doOrganism.FindBodyPartByName("Left Front Femur"), AnimatTools.DataObjects.Physical.RigidBody)
                m_doLeftFrontTibia = DirectCast(m_doOrganism.FindBodyPartByName("Left Front Tibia"), AnimatTools.DataObjects.Physical.RigidBody)
                m_doLeftMiddleFemur = DirectCast(m_doOrganism.FindBodyPartByName("Left Middle Femur"), AnimatTools.DataObjects.Physical.RigidBody)
                m_doLeftMiddleTibia = DirectCast(m_doOrganism.FindBodyPartByName("Left Middle Tibia"), AnimatTools.DataObjects.Physical.RigidBody)
                m_doLeftRearCoxa = DirectCast(m_doOrganism.FindBodyPartByName("Left Rear Coxa"), AnimatTools.DataObjects.Physical.RigidBody)
                m_doLeftRearFemur = DirectCast(m_doOrganism.FindBodyPartByName("Left Rear Femur"), AnimatTools.DataObjects.Physical.RigidBody)
                m_doLeftRearTibia = DirectCast(m_doOrganism.FindBodyPartByName("Left Rear Tibia"), AnimatTools.DataObjects.Physical.RigidBody)

                m_doLeftMiddleFootDown = DirectCast(m_doOrganism.FindBodyPartByName("Left Middle Foot Down"), AnimatTools.DataObjects.Physical.RigidBody)
                m_doLeftFrontFootDown = DirectCast(m_doOrganism.FindBodyPartByName("Left Front Foot Down"), AnimatTools.DataObjects.Physical.RigidBody)
                m_doRightMiddleFootDown = DirectCast(m_doOrganism.FindBodyPartByName("Right Middle Foot Down"), AnimatTools.DataObjects.Physical.RigidBody)
                m_doRightFrontFootDown = DirectCast(m_doOrganism.FindBodyPartByName("Right Front Foot Down"), AnimatTools.DataObjects.Physical.RigidBody)

                If Not m_doOrganism.FindBodyPartByName("Left Rear Tarsus Down", False) Is Nothing Then
                    m_doLeftRearTarsusDown = DirectCast(m_doOrganism.FindBodyPartByName("Left Rear Tarsus Down"), AnimatTools.DataObjects.Physical.RigidBody)
                    m_doRightRearTarsusDown = DirectCast(m_doOrganism.FindBodyPartByName("Right Rear Tarsus Down"), AnimatTools.DataObjects.Physical.RigidBody)
                End If

                m_doAb1Joint = DirectCast(m_doOrganism.FindBodyPartByName("Thorax_Ab1"), AnimatTools.DataObjects.Physical.Joint)
                m_doAb2Joint = DirectCast(m_doOrganism.FindBodyPartByName("Ab1_Ab2"), AnimatTools.DataObjects.Physical.Joint)
                m_doAb3Joint = DirectCast(m_doOrganism.FindBodyPartByName("Ab2_Ab3"), AnimatTools.DataObjects.Physical.Joint)
                m_doAb4Joint = DirectCast(m_doOrganism.FindBodyPartByName("Ab3_Ab4"), AnimatTools.DataObjects.Physical.Joint)

                m_doHeadAxisRef = DirectCast(m_doOrganism.FindBodyPartByName("Head Axis Reference"), AnimatTools.DataObjects.Physical.RigidBody)
                m_doTailAxisRef = DirectCast(m_doOrganism.FindBodyPartByName("Tail Axis Reference"), AnimatTools.DataObjects.Physical.RigidBody)
                m_doCOMRef = DirectCast(m_doOrganism.FindBodyPartByName("COM Reference"), AnimatTools.DataObjects.Physical.RigidBody)
                m_doRollAxisRef = DirectCast(m_doOrganism.FindBodyPartByName("Roll Axis Reference"), AnimatTools.DataObjects.Physical.RigidBody)

                If Not m_doOrganism.FindBodyPartByName("Pronotum_Front Ref", False) Is Nothing Then
                    m_doPronotumFrontRef = DirectCast(m_doOrganism.FindBodyPartByName("Pronotum_Front Ref"), AnimatTools.DataObjects.Physical.RigidBody)
                End If
                If Not m_doOrganism.FindBodyPartByName("Pronotum_Rear Ref", False) Is Nothing Then
                    m_doPronotumRearRef = DirectCast(m_doOrganism.FindBodyPartByName("Pronotum_Rear Ref"), AnimatTools.DataObjects.Physical.RigidBody)
                End If

                m_doDorsalAbNeuron = m_doOrganism.FindBehavioralNodeByName("Dorsal Control", False)
                'm_doVentralAbNeuron = m_doOrganism.FindBehavioralNodeByName("Ventral Control", False)

            End If

        End Sub

#Region " DataObject Methods "

        Protected Overrides Sub BuildProperties()
            MyBase.BuildProperties()

            If Not m_doOrganism Is Nothing Then
                m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Organism", GetType(String), "OrganismName", _
                                            "Stimulus Properties", "The name of the organism to which this stimulus is applied.", Me.Organism, True))
            End If


            ''Now lets add the properties for this neuron
            Dim pbNumberBag As Crownwood.Magic.Controls.PropertyBag = m_snBeta.Properties
            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Beta", pbNumberBag.GetType(), "Beta", _
                                        "Stimulus Properties", "Sets the desired beta angle for the jump. " & _
                                        "This is the angle made between the femur-tiba and the ground", pbNumberBag, _
                                        "", GetType(AnimatTools.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snDelta.Properties
            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Delta", pbNumberBag.GetType(), "Delta", _
                                        "Stimulus Properties", "Sets the desired delta angle for the jump. " & _
                                        "This is the angle made between the beta angle and the thorax-coxa joint. " & _
                                        "It determines if the coxa is aligned with the beta angle.", pbNumberBag, _
                                        "", GetType(AnimatTools.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snPitch.Properties
            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Pitch", pbNumberBag.GetType(), "Pitch", _
                                        "Stimulus Properties", "Sets the desired body pitch angle for the jump. ", _
                                        pbNumberBag, "", GetType(AnimatTools.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snAbDelay.Properties
            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Ab Delay", pbNumberBag.GetType(), "AbDelay", _
                                        "Stimulus Properties", "Sets the delay for the abdomen control system. ", _
                                        pbNumberBag, "", GetType(AnimatTools.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snAbPropGain.Properties
            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Ab Prop Gain", pbNumberBag.GetType(), "AbPropGain", _
                                        "Stimulus Properties", "Sets the proportional gain for the abdomen control system. ", _
                                        pbNumberBag, "", GetType(AnimatTools.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snAbPeriod.Properties
            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Ab Period", pbNumberBag.GetType(), "AbPeriod", _
                                        "Stimulus Properties", "Sets the time period of movement for the abdomen control system. ", _
                                        pbNumberBag, "", GetType(AnimatTools.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snLegPeriod.Properties
            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Leg Period", pbNumberBag.GetType(), "LegPeriod", _
                                        "Stimulus Properties", "Sets the time period of movement for the CF Leg joint after the jump.", _
                                        pbNumberBag, "", GetType(AnimatTools.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Enable Ab Control", m_bEnableAbControl.GetType(), "EnableAbControl", _
                                        "Stimulus Properties", "If this is true then the ab control system is enabled.", m_bEnableAbControl))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Lock Ab Jump", m_bLockAbJump.GetType(), "LockAbJump", _
                                        "Stimulus Properties", "If true then the ab joints are locked when jump starts.", m_bLockAbJump))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Tumbling Setup", m_bTumblingSetup.GetType(), "TumblingSetup", _
                                        "Stimulus Properties", "Set to true to reproduce experiments from tumbling paper.", m_bTumblingSetup))

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            If Not m_snBeta Is Nothing Then m_snBeta.ClearIsDirty()
            If Not m_snDelta Is Nothing Then m_snDelta.ClearIsDirty()
            If Not m_snPitch Is Nothing Then m_snPitch.ClearIsDirty()

            If Not m_snAbDelay Is Nothing Then m_snAbDelay.ClearIsDirty()
            If Not m_snAbPropGain Is Nothing Then m_snAbPropGain.ClearIsDirty()
            If Not m_snAbPeriod Is Nothing Then m_snAbPeriod.ClearIsDirty()
            If Not m_snLegPeriod Is Nothing Then m_snLegPeriod.ClearIsDirty()

        End Sub

        Public Overrides Sub LoadData(ByRef oXml As AnimatTools.Interfaces.StdXml)
            MyBase.LoadData(oXml)

            oXml.IntoElem()

            Dim strStructureID As String = oXml.GetChildString("StructureID", "")
            If strStructureID.Trim.Length > 0 Then
                Dim objStruct As Object = Util.Environment.FindStructureFromAll(strStructureID)
                If Not objStruct Is Nothing Then
                    m_doOrganism = DirectCast(objStruct, AnimatTools.DataObjects.Physical.Organism)
                    SetBodyParts()
                End If
            End If

            m_snBeta.LoadData(oXml, "Beta")
            m_snDelta.LoadData(oXml, "Delta")
            m_snPitch.LoadData(oXml, "Pitch")
            m_bEnableAbControl = oXml.GetChildBool("EnableAbControl", True)
            m_bLockAbJump = oXml.GetChildBool("LockAbJump", True)
            m_bTumblingSetup = oXml.GetChildBool("TumblingSetup", False)

            If oXml.FindChildElement("AbDelay", False) Then
                m_snAbDelay.LoadData(oXml, "AbDelay")
                m_snAbPropGain.LoadData(oXml, "AbPropGain")
            End If
            If oXml.FindChildElement("AbPeriod", False) Then
                m_snAbPeriod.LoadData(oXml, "AbPeriod")
                m_snLegPeriod.LoadData(oXml, "LegPeriod")
            End If

            oXml.OutOfElem()

            Me.IsDirty = False
        End Sub

        Public Overrides Sub SaveData(ByRef oXml As AnimatTools.Interfaces.StdXml)
            MyBase.SaveData(oXml)

            oXml.IntoElem()

            'Make sure the structure and body are set and that the references are still valid.
            If Not m_doOrganism Is Nothing Then
                If Not Util.Environment.FindStructureFromAll(m_doOrganism.ID, False) Is Nothing Then
                    oXml.AddChildElement("StructureID", m_doOrganism.ID)
                End If
            End If

            m_snBeta.SaveData(oXml, "Beta")
            m_snDelta.SaveData(oXml, "Delta")
            m_snPitch.SaveData(oXml, "Pitch")

            m_snAbDelay.SaveData(oXml, "AbDelay")
            m_snAbPropGain.SaveData(oXml, "AbPropGain")
            m_snAbPeriod.SaveData(oXml, "AbPeriod")
            m_snLegPeriod.SaveData(oXml, "LegPeriod")

            oXml.AddChildElement("EnableAbControl", m_bEnableAbControl)
            oXml.AddChildElement("LockAbJump", m_bLockAbJump)
            oXml.AddChildElement("TumblingSetup", m_bTumblingSetup)

            oXml.OutOfElem() ' Outof Node Element

        End Sub

        Public Overrides Sub SaveDataColumnToXml(ByRef oXml As AnimatTools.Interfaces.StdXml)
            oXml.IntoElem()
            oXml.AddChildElement("StimulusID", Me.ID)
            oXml.OutOfElem()
        End Sub

#End Region

#End Region

    End Class

End Namespace

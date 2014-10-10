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

Namespace DataObjects.ExternalStimuli

    Public Class MotorVelocity
        Inherits AnimatGUI.DataObjects.ExternalStimuli.BodyPartStimulus

#Region " Attributes "

        Protected m_snVelocity As ScaledNumber
        Protected m_bDisableWhenDone As Boolean = False

        Protected m_doJoint As DataObjects.Physical.Joint

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Overrides Property BodyPart() As DataObjects.Physical.BodyPart
            Get
                Return m_doBodyPart
            End Get
            Set(ByVal Value As DataObjects.Physical.BodyPart)
                m_doBodyPart = Value

                SetVelocityUnits()
            End Set
        End Property

        Public Overridable Property Velocity() As ScaledNumber
            Get
                Return m_snVelocity
            End Get
            Set(ByVal Value As ScaledNumber)
                If Not Value Is Nothing Then
                    SetVelocity(Value.ActualValue.ToString)
                    m_snVelocity.CopyData(Value)
                End If
            End Set
        End Property

        Public Overrides Property ValueType() As enumValueType
            Get
                Return m_eValueType
            End Get
            Set(ByVal Value As enumValueType)
                m_eValueType = Value

                SetSimData("ValueType", Value.ToString, True)
                SetVelocity()

                If Not Util.ProjectWorkspace Is Nothing Then
                    Util.ProjectWorkspace.RefreshProperties()
                End If
            End Set
        End Property

        Public Overrides Property Equation() As String
            Get
                Return m_strEquation
            End Get
            Set(ByVal Value As String)
                If Value.Trim.Length = 0 Then
                    Throw New System.Exception("Equation cannot be blank.")
                End If

                SetVelocity(Value)
                m_strEquation = Value
            End Set
        End Property

        Public Overridable Property DisableWhenDone() As Boolean
            Get
                Return m_bDisableWhenDone
            End Get
            Set(ByVal Value As Boolean)
                SetSimData("DisableWhenDone", Value.ToString, True)
                m_bDisableWhenDone = Value
            End Set
        End Property

        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Motor Velocity"
            End Get
        End Property

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.MotorVelocity.gif"
            End Get
        End Property

        Public Overrides Property Description() As String
            Get
                Return "This stimulus sets the velocity of a motorized joint."
            End Get
            Set(value As String)
            End Set
        End Property

        Public Overrides ReadOnly Property StimulusClassType() As String
            Get
                Return "MotorVelocity"
            End Get
        End Property

        Public Overrides ReadOnly Property DragImageName() As String
            Get
                Return "AnimatGUI.MotorVelocity_Large.gif"
            End Get
        End Property

        Public Overridable ReadOnly Property StimulusType() As String
            Get
                'We have a separate MotorPosition stimulus now, but this code is here mainly to handle cases
                'where we are loading in an old project file. It still needs to be able to handle this edge case.
                If Not m_doJoint Is Nothing AndAlso m_doJoint.IsMotorized Then
                    If m_doJoint.MotorType = Physical.Joint.enumJointMotorTypes.PositionControl Then
                        Return "Position"
                    Else
                        Return "Velocity"
                    End If
                Else
                    Return "Velocity"
                End If
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            m_snVelocity = New ScaledNumber(Me, "Velocity", 0, ScaledNumber.enumNumericScale.None, "rad/s", "rad/s")

            If Not Util.Environment Is Nothing Then
                m_snVelocity.SetFromValue(0, Util.Environment.DistanceUnits)
            End If

        End Sub

        Protected Overrides Sub SetSimEquation(ByVal strEquation As String)
            If strEquation.Trim.Length > 0 Then
                'Lets verify the equation before we use it.
                'We need to convert the infix equation to postfix
                Dim oMathEval As New MathStringEval
                oMathEval.AddVariable("t")
                oMathEval.AddVariable("p")
                oMathEval.AddVariable("v")
                oMathEval.Equation = strEquation
                oMathEval.Parse()

                SetSimData("Velocity", oMathEval.PostFix, True)
            Else
                SetSimData("Equation", "0", True)
            End If
        End Sub

        Protected Sub SetVelocity(Optional ByVal strEquation As String = "")

            If m_eValueType = enumValueType.Constant Then
                If strEquation.Trim.Length = 0 Then strEquation = Me.Velocity.ActualValue.ToString
                SetSimData("Velocity", strEquation, True)
            Else
                If strEquation.Trim.Length = 0 Then strEquation = Me.Equation
                SetSimEquation(strEquation)
            End If

        End Sub

        Protected Overridable Sub SetVelocityUnits()
            If Not m_doBodyPart Is Nothing AndAlso TypeOf m_doBodyPart Is AnimatGUI.DataObjects.Physical.Joint Then
                Dim doJoint As AnimatGUI.DataObjects.Physical.Joint = DirectCast(m_doBodyPart, AnimatGUI.DataObjects.Physical.Joint)

                Dim strUnits As String = doJoint.ScaleUnits

                If StimulusType() = "Velocity" Then strUnits = strUnits & "/s"

                m_snVelocity.SetScaleUnits(strUnits, strUnits)
            End If

        End Sub

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doPart As DataObjects.ExternalStimuli.MotorVelocity = DirectCast(doOriginal, DataObjects.ExternalStimuli.MotorVelocity)

            m_snVelocity = DirectCast(doPart.m_snVelocity.Clone(Me, bCutData, doRoot), ScaledNumber)

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim doStim As DataObjects.ExternalStimuli.MotorVelocity = New DataObjects.ExternalStimuli.MotorVelocity(doParent)
            CloneInternal(doStim, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then doStim.AfterClone(Me, bCutData, doRoot, doStim)
            Return doStim
        End Function

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            m_snVelocity.ClearIsDirty()
        End Sub

        Public Overrides Sub InitializeAfterLoad()
            MyBase.InitializeAfterLoad()

            If Not Me.StimulatedItem Is Nothing AndAlso Util.IsTypeOf(Me.StimulatedItem.GetType(), GetType(DataObjects.Physical.Joint), False) Then
                m_doJoint = DirectCast(Me.StimulatedItem, DataObjects.Physical.Joint)
            End If
        End Sub

        Public Overrides Function GetSimulationXml(ByVal strName As String, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing) As String

            If m_doStructure Is Nothing Then
                Throw New System.Exception("No structure was defined for the stimulus '" & m_strName & "'.")
            End If

            If m_doBodyPart Is Nothing Then
                Throw New System.Exception("No bodypart was defined for the stimulus '" & m_strName & "'.")
            End If

            Dim oXml As ManagedAnimatInterfaces.IStdXml = Util.Application.CreateStdXml()
            oXml.AddElement("Root")
            SaveSimulationXml(oXml, nmParentControl, strName)

            Return oXml.Serialize()
        End Function

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            If m_doStructure Is Nothing Then
                Throw New System.Exception("No structure was defined for the stimulus '" & m_strName & "'.")
            End If

            If m_doBodyPart Is Nothing Then
                Throw New System.Exception("No bodypart was defined for the stimulus '" & m_strName & "'.")
            End If

            oXml.AddChildElement("Stimulus")

            oXml.IntoElem()
            oXml.AddChildElement("ID", m_strID)
            oXml.AddChildElement("Name", m_strName)
            oXml.AddChildElement("AlwaysActive", m_bAlwaysActive)
            oXml.AddChildElement("Enabled", m_bEnabled)

            oXml.AddChildElement("ModuleName", Me.StimulusModuleName)
            oXml.AddChildElement("Type", Me.StimulusClassType)

            oXml.AddChildElement("StructureID", m_doStructure.ID)
            oXml.AddChildElement("JointID", m_doBodyPart.ID)

            oXml.AddChildElement("StartTime", m_snStartTime.ActualValue)
            oXml.AddChildElement("EndTime", m_snEndTime.ActualValue)


            oXml.AddChildElement("TargetID", StimulusType())

            If m_eValueType = enumValueType.Constant Then
                oXml.AddChildElement("Equation", m_snVelocity.ActualValue)
            Else
                'We need to convert the infix equation to postfix
                Dim oMathEval As New MathStringEval
                oMathEval.AddVariable("t")
                oMathEval.AddVariable("p")
                oMathEval.AddVariable("v")
                oMathEval.Equation = m_strEquation
                oMathEval.Parse()
                oXml.AddChildElement("Equation", oMathEval.PostFix)
            End If

            oXml.AddChildElement("DisableMotorWhenDone", m_bDisableWhenDone)

            oXml.OutOfElem()
        End Sub

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Value Type", m_eValueType.GetType(), "ValueType", _
                                        "Stimulus Properties", "Determines if a constant or an equation is used to determine the velocity/position.", m_eValueType))

            If m_eValueType = enumValueType.Equation Then
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Equation", m_strEquation.GetType(), "Equation", _
                                            "Stimulus Properties", "If setup to use equations, then this is the one used.", m_strEquation))
            Else
                Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = m_snVelocity.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec(StimulusType(), pbNumberBag.GetType(), "Velocity", _
                                            "Stimulus Properties", "The velocity/position to move the motorized joint.", pbNumberBag, _
                                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))
            End If

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Disable When Done", m_bDisableWhenDone.GetType(), "DisableWhenDone", _
                                        "Stimulus Properties", "If this is true then the motor is disabled at the end of the simulus period.", m_bDisableWhenDone))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Stim Type", StimulusType().GetType(), "StimulusType", _
                                        "Stimulus Properties", "Tells if it is a velocity or position motor stimulus.", StimulusType(), True))

        End Sub

        Public Overrides Sub UnitsChanged(ByVal ePrevMass As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits, _
                                          ByVal eNewMass As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits, _
                                          ByVal fltMassChange As Single, _
                                          ByVal ePrevDistance As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits, _
                                          ByVal eNewDistance As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits, _
                                          ByVal fltDistanceChange As Single)
            m_snVelocity.ActualValue = m_snVelocity.ActualValue * fltDistanceChange
        End Sub

        Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.LoadData(oXml)

            SetVelocityUnits()

            oXml.IntoElem()

            If ScaledNumber.IsValidXml(oXml, "Velocity") Then
                m_snVelocity.LoadData(oXml, "Velocity")
            Else
                Dim fltVelocity As Single = oXml.GetChildFloat("Velocity", CSng(m_snVelocity.ActualValue))
                m_snVelocity.ActualValue = fltVelocity
            End If

            m_bDisableWhenDone = oXml.GetChildBool("DisableMotorWhenDone", m_bDisableWhenDone)

            oXml.OutOfElem()

            Me.IsDirty = False
        End Sub

        Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.SaveData(oXml)

            oXml.IntoElem()

            m_snVelocity.SaveData(oXml, "Velocity")
            oXml.AddChildElement("DisableMotorWhenDone", m_bDisableWhenDone)

            oXml.OutOfElem() ' Outof Node Element

        End Sub

#End Region

#End Region

    End Class

End Namespace

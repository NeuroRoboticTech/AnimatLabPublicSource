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

    Public MustInherit Class MuscleBase
        Inherits Physical.Bodies.Line
        Implements IMuscle

#Region " Attributes "

        Protected m_snMaxTension As Framework.ScaledNumber

        Protected m_StimTension As DataObjects.Gains.MuscleGains.StimulusTension
        Protected m_LengthTension As DataObjects.Gains.MuscleGains.LengthTension

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.MuscleAttachment_Treeview.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonImageName() As String
            Get
                Return "AnimatGUI.MuscleAttachment_Button.gif"
            End Get
        End Property

        Public Overridable Property MaxTension() As ScaledNumber
            Get
                Return m_snMaxTension
            End Get
            Set(ByVal value As ScaledNumber)
                If Not value Is Nothing Then
                    If value.ActualValue <= 0 Then
                        Throw New System.Exception("The Max tension must be greater than zero.")
                    End If

                    SetSimData("MaxTension", value.ActualValue.ToString, True)
                    m_snMaxTension.CopyData(value)
                End If
            End Set
        End Property

        <Category("Stimulus-Tension"), Description("Sets the stmilus-tension properties of the muscle.")> _
        Public Overridable Property StimulusTension() As Gains.MuscleGains.StimulusTension Implements IMuscle.StimulusTension
            Get
                If m_StimTension Is Nothing Then
                    m_StimTension = New Gains.MuscleGains.StimulusTension(Me)
                End If

                Return m_StimTension
            End Get
            Set(ByVal value As Gains.MuscleGains.StimulusTension)
                If Not value Is Nothing Then
                    SetSimData("StimulusTension", value.GetSimulationXml("Gain", Me), True)
                    value.InitializeSimulationReferences()
                End If

                If Not m_StimTension Is Nothing Then m_StimTension.ParentData = Nothing
                m_StimTension = value
                If Not m_StimTension Is Nothing Then
                    m_StimTension.GainPropertyName = "StimulusTension"
                End If
            End Set
        End Property

        Public Property LengthTension() As Gains.MuscleGains.LengthTension Implements IMuscle.LengthTension
            Get
                If m_LengthTension Is Nothing Then
                    m_LengthTension = New Gains.MuscleGains.LengthTension(Me)
                End If

                Return m_LengthTension
            End Get
            Set(ByVal value As Gains.MuscleGains.LengthTension)
                If Not value Is Nothing Then
                    SetSimData("LengthTension", value.GetSimulationXml("Gain", Me), True)
                    value.InitializeSimulationReferences()
                End If

                If Not m_LengthTension Is Nothing Then m_LengthTension.ParentData = Nothing
                m_LengthTension = value
                If Not m_LengthTension Is Nothing Then
                    m_LengthTension.GainPropertyName = "LengthTension"
                End If
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
            m_bIsCollisionObject = False
            m_clDiffuse = Color.Blue

            m_snMaxTension = New ScaledNumber(Me, "MaxTension", 100, ScaledNumber.enumNumericScale.None, "Newtons", "N")

            m_StimTension = New AnimatGUI.DataObjects.Gains.MuscleGains.StimulusTension(Me)
            m_LengthTension = New AnimatGUI.DataObjects.Gains.MuscleGains.LengthTension(Me)

        End Sub

        Public Overrides Sub InitializeSimulationReferences()
            MyBase.InitializeSimulationReferences()

            m_StimTension.InitializeSimulationReferences()
            m_LengthTension.InitializeSimulationReferences()
        End Sub

        Protected Overrides Sub CloneInternal(ByVal doOriginal As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doOrigBody As MuscleBase = DirectCast(doOriginal, MuscleBase)

            m_StimTension = DirectCast(doOrigBody.m_StimTension.Clone(Me, bCutData, doRoot), AnimatGUI.DataObjects.Gains.MuscleGains.StimulusTension)
            m_LengthTension = DirectCast(doOrigBody.m_LengthTension.Clone(Me, bCutData, doRoot), AnimatGUI.DataObjects.Gains.MuscleGains.LengthTension)
            m_snMaxTension = DirectCast(doOrigBody.m_snMaxTension.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)

        End Sub

        Public Overrides Sub SwapBodyPartCopy(ByVal doOriginal As BodyPart)
            MyBase.SwapBodyPartCopy(doOriginal)

            If Util.IsTypeOf(doOriginal.GetType, GetType(MuscleBase), False) Then
                Dim msOrig As MuscleBase = DirectCast(doOriginal, MuscleBase)

                m_snMaxTension = msOrig.m_snMaxTension
                m_StimTension = msOrig.m_StimTension
                m_LengthTension = msOrig.m_LengthTension
            End If
        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            Dim strType As String = Replace(Me.Type, "LinearHill", "")

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag
            pbNumberBag = m_snMaxTension.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Maximum Tension", pbNumberBag.GetType(), "MaxTension", _
                                        strType & " Properties", "A param that determines the maximum tension this muscle can possible generate.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_StimTension.Properties
            propTable.Properties.Add(New PropertySpec("Stimulus-Tension", pbNumberBag.GetType(), _
                          "StimulusTension", strType & " Properties", "Sets the stmilus-tension properties of the muscle.", pbNumberBag, _
                        GetType(AnimatGUI.TypeHelpers.GainTypeEditor), GetType(AnimatGuiCtrls.Controls.ExpandablePropBagConverter)))

            pbNumberBag = m_LengthTension.Properties
            propTable.Properties.Add(New PropertySpec("Length-Tension", pbNumberBag.GetType(), _
                        "LengthTension", strType & " Properties", "Sets the length-tension properties of the muscle.", pbNumberBag, _
                         GetType(AnimatGUI.TypeHelpers.GainTypeEditor), GetType(AnimatGuiCtrls.Controls.ExpandablePropBagConverter)))

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            If Not m_snMaxTension Is Nothing Then m_snMaxTension.ClearIsDirty()
            If Not m_StimTension Is Nothing Then m_StimTension.ClearIsDirty()
            If Not m_LengthTension Is Nothing Then m_LengthTension.ClearIsDirty()
        End Sub

        Public Overloads Overrides Sub LoadData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByRef oXml As Interfaces.StdXml)
            MyBase.LoadData(doStructure, oXml)

            oXml.IntoElem() 'Into RigidBody Element

            m_StimTension.LoadData(oXml, "StimulusTension", "StimulusTension")
            m_LengthTension.LoadData(oXml, "LengthTension", "LengthTension")

            m_snMaxTension.LoadData(oXml, "MaximumTension")

            oXml.OutOfElem() 'Outof RigidBody Element

        End Sub

        Public Overloads Overrides Sub SaveData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByRef oXml As Interfaces.StdXml)
            MyBase.SaveData(doStructure, oXml)

            oXml.IntoElem() 'Into Child Elemement

            m_StimTension.SaveData(oXml, "StimulusTension")
            m_LengthTension.SaveData(oXml, "LengthTension")

            m_snMaxTension.SaveData(oXml, "MaximumTension")

            oXml.OutOfElem() 'Outof BodyPart Element

        End Sub

        Public Overrides Sub SaveSimulationXml(ByRef oXml As Interfaces.StdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

            oXml.IntoElem() 'Into Child Elemement

            m_StimTension.SaveSimulationXml(oXml, Me, "StimulusTension")
            m_LengthTension.SaveSimulationXml(oXml, Me, "LengthTension")

            m_snMaxTension.SaveSimulationXml(oXml, Me, "MaximumTension")

            oXml.OutOfElem() 'Outof BodyPart Element

        End Sub

#End Region

    End Class


End Namespace

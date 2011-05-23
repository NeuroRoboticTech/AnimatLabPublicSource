Imports System
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

    Public Class LinearHillStretchReceptor
        Inherits Physical.Bodies.LinearHillMuscle

#Region " Attributes "

        Protected m_bApplyTension As Boolean = False
        Protected m_snIaDischargeConstant As ScaledNumber
        Protected m_snIIDischargeConstant As ScaledNumber

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.MuscleSpindle_Treeview.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonImageName() As String
            Get
                Return "AnimatGUI.MuscleSpindle_Button.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property Type() As String
            Get
                Return "LinearHillStretchReceptor"
            End Get
        End Property

        Public Overrides ReadOnly Property PartType() As System.Type
            Get
                Return GetType(AnimatGUI.DataObjects.Physical.Bodies.LinearHillStretchReceptor)
            End Get
        End Property

        Public Overridable Property ApplyTension() As Boolean
            Get
                Return m_bApplyTension
            End Get
            Set(ByVal value As Boolean)
                SetSimData("ApplyTension", value.ToString, True)
                m_bApplyTension = value
            End Set
        End Property

        Public Overridable Property IaDischargeConstant() As ScaledNumber
            Get
                Return m_snIaDischargeConstant
            End Get
            Set(ByVal value As ScaledNumber)
                If Not value Is Nothing Then
                    If value.ActualValue <= 0 Then
                        Throw New System.Exception("The Ia discharge rate must be greater than or equal to zero.")
                    End If

                    SetSimData("IaDischarge", value.ActualValue.ToString, True)
                    m_snIaDischargeConstant.CopyData(value)
                End If
            End Set
        End Property

        Public Overridable Property IIDischargeConstant() As ScaledNumber
            Get
                Return m_snIIDischargeConstant
            End Get
            Set(ByVal value As ScaledNumber)
                If Not value Is Nothing Then
                    If value.ActualValue <= 0 Then
                        Throw New System.Exception("The II discharge rate must be greater than or equal to zero.")
                    End If

                    SetSimData("IIDischarge", value.ActualValue.ToString, True)
                    m_snIIDischargeConstant.CopyData(value)
                End If
            End Set
        End Property

#End Region

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("Ia", "Ia Discharge Rate", "Spikes/s", "Spikes/s", 0, 1000))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("II", "II Discharge Rate", "Spikes/s", "Spikes/s", 0, 1000))
            m_thDataTypes.ID = "Ia"

            m_snIaDischargeConstant = New AnimatGUI.Framework.ScaledNumber(Me, "Ia Discharge Constant", 100, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Spikes/sm", "Spikes/sm")
            m_snIIDischargeConstant = New AnimatGUI.Framework.ScaledNumber(Me, "II Discharge Constant", 100, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Spikes/sm", "Spikes/sm")

        End Sub

        Public Overrides Function Clone(ByVal doParent As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject) As Framework.DataObject
            Dim oNewNode As New Bodies.LinearHillStretchReceptor(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doOrigBody As LinearHillStretchReceptor = DirectCast(doOriginal, LinearHillStretchReceptor)

            m_bApplyTension = doOrigBody.m_bApplyTension
            m_snIaDischargeConstant = DirectCast(doOrigBody.m_snIaDischargeConstant.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snIIDischargeConstant = DirectCast(doOrigBody.m_snIIDischargeConstant.Clone(Me, bCutData, doRoot), ScaledNumber)

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            If Not m_snIaDischargeConstant Is Nothing Then m_snIaDischargeConstant.ClearIsDirty()
            If Not m_snIIDischargeConstant Is Nothing Then m_snIIDischargeConstant.ClearIsDirty()
        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Apply Tension", m_bApplyTension.GetType(), "ApplyTension", _
                                     "Muscle Properties", "Determines whether the stretch receptor actually applies tension.", m_bApplyTension))

            Dim pbSubBag As AnimatGuiCtrls.Controls.PropertyBag = m_snIaDischargeConstant.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Ia Discharge Constant", pbSubBag.GetType(), "IaDischargeConstant", _
            "Muscle Properties", "Relates the length of segments of the stretch receptor to the discharge rate of the type Ia fibers.", pbSubBag, _
            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbSubBag = m_snIIDischargeConstant.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("II Discharge Constant", pbSubBag.GetType(), "IIDischargeConstant", _
             "Muscle Properties", "Relates the length of segments of the stretch receptor to the discharge rate of the type II fibers.", pbSubBag, _
             "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

        End Sub

        Public Overloads Overrides Sub LoadData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByRef oXml As Interfaces.StdXml)
            MyBase.LoadData(doStructure, oXml)

            oXml.IntoElem() 'Into RigidBody Element

            m_bApplyTension = oXml.GetChildBool("ApplyTension")
            m_snIaDischargeConstant.LoadData(oXml, "IaDischarge")
            m_snIIDischargeConstant.LoadData(oXml, "IIDischarge")

            oXml.OutOfElem() 'Outof RigidBody Element

        End Sub

        Public Overloads Overrides Sub SaveData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByRef oXml As Interfaces.StdXml)
            MyBase.SaveData(doStructure, oXml)

            oXml.IntoElem() 'Into Child Elemement

            oXml.AddChildElement("ApplyTension", m_bApplyTension)
            m_snIaDischargeConstant.SaveData(oXml, "IaDischarge")
            m_snIIDischargeConstant.SaveData(oXml, "IIDischarge")

            oXml.OutOfElem() 'Outof BodyPart Element

        End Sub

        Public Overrides Sub SaveSimulationXml(ByRef oXml As Interfaces.StdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

            oXml.IntoElem()

            oXml.AddChildElement("ApplyTension", m_bApplyTension)
            m_snIaDischargeConstant.SaveSimulationXml(oXml, Me, "IaDischarge")
            m_snIIDischargeConstant.SaveSimulationXml(oXml, Me, "IIDischarge")

            oXml.OutOfElem()

        End Sub

    End Class


End Namespace

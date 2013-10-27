Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports AnimatGuiCtrls.Controls
Imports AnimatGUI.DataObjects
Imports AnimatGUI.Collections
Imports AnimatGUI.Framework

Namespace DataObjects.Physical

    Public Class MaterialTypeVortex
        Inherits MaterialType

#Region " Attributes "

        'The primary coefficient of friction parameter.
        Protected m_snFrictionLinearPrimary As ScaledNumber

        'The secondary coefficient of friction parameter.
        Protected m_snFrictionLinearSecondary As ScaledNumber

        'The angular normal coefficient of friction parameter.
        Protected m_snFrictionAngularNormal As ScaledNumber

        'The angular primary coefficient of friction parameter.
        Protected m_snFrictionAngularPrimary As ScaledNumber

        'The angular secondary coefficient of friction parameter.
        Protected m_snFrictionAngularSecondary As ScaledNumber

        'The maximum primary friction that can be created.
        Protected m_snFrictionLinearPrimaryMax As ScaledNumber

        'The maximum secondary friction that can created.
        Protected m_snFrictionLinearSecondaryMax As ScaledNumber

        'The maximum angular normal friction that can be created.
        Protected m_snFrictionAngularNormalMax As ScaledNumber

        'The maximum angular primary friction that can be created.
        Protected m_snFrictionAngularPrimaryMax As ScaledNumber

        'The maximum angular secondary friction that can be created.
        Protected m_snFrictionAngularSecondaryMax As ScaledNumber

        'The compliance of the collision between those two materials.
        Protected m_snCompliance As ScaledNumber

        'The damping of the collision between those two materials.
        Protected m_snDamping As ScaledNumber

        'The restitution of the collision between those two materials.
        Protected m_snRestitution As ScaledNumber

        'The primary linear slip of the collision between those two materials.
        Protected m_snSlipLinearPrimary As ScaledNumber

        'The secondary linear slip of the collision between those two materials.
        Protected m_snSlipLinearSecondary As ScaledNumber

        'The angular normal slip of the collision between those two materials.
        Protected m_snSlipAngularNormal As ScaledNumber

        'The angular primary slip of the collision between those two materials.
        Protected m_snSlipAngularPrimary As ScaledNumber

        'The angular secondary slip of the collision between those two materials.
        Protected m_snSlipAngularSecondary As ScaledNumber

        'The linear primary slide of the collision between those two materials.
        Protected m_snSlideLinearPrimary As ScaledNumber

        'The secondary linear slide of the collision between those two materials.
        Protected m_snSlideLinearSecondary As ScaledNumber

        'The angular normal slide of the collision between those two materials.
        Protected m_snSlideAngularNormal As ScaledNumber

        'The angular primary slide of the collision between those two materials.
        Protected m_snSlideAngularPrimary As ScaledNumber

        'The angular secondary slide of the collision between those two materials.
        Protected m_snSlideAngularSecondary As ScaledNumber

        'The maximum adhesion of the collision between those two materials.
        Protected m_snMaxAdhesion As ScaledNumber

#End Region

#Region " Properties "

        Public Overridable Property FrictionLinearPrimary() As ScaledNumber
            Get
                Return m_snFrictionLinearPrimary
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The linear primary coefficient of friction can not be less than zero.")
                End If
                SetSimData("FrictionLinearPrimary", Value.ActualValue.ToString, True)
                m_snFrictionLinearPrimary.CopyData(Value)
            End Set
        End Property

        Public Overridable Property FrictionLinearSecondary() As ScaledNumber
            Get
                Return m_snFrictionLinearSecondary
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The linear secondary coefficient of friction can not be less than zero.")
                End If
                SetSimData("FrictionLinearSecondary", Value.ActualValue.ToString, True)
                m_snFrictionLinearSecondary.CopyData(Value)
            End Set
        End Property

        Public Overridable Property FrictionAngularNormal() As ScaledNumber
            Get
                Return m_snFrictionAngularNormal
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The angular normal coefficient of friction can not be less than zero.")
                End If
                SetSimData("FrictionAngularNormal", Value.ActualValue.ToString, True)
                m_snFrictionAngularNormal.CopyData(Value)
            End Set
        End Property

        Public Overridable Property FrictionAngularPrimary() As ScaledNumber
            Get
                Return m_snFrictionAngularPrimary
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The angular primary coefficient of friction can not be less than zero.")
                End If
                SetSimData("FrictionAngularPrimary", Value.ActualValue.ToString, True)
                m_snFrictionAngularPrimary.CopyData(Value)
            End Set
        End Property

        Public Overridable Property FrictionAngularSecondary() As ScaledNumber
            Get
                Return m_snFrictionAngularSecondary
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The angular secondary coefficient of friction can not be less than zero.")
                End If
                SetSimData("FrictionAngularSecondary", Value.ActualValue.ToString, True)
                m_snFrictionAngularSecondary.CopyData(Value)
            End Set
        End Property

        Public Overridable Property FrictionLinearPrimaryMax() As ScaledNumber
            Get
                Return m_snFrictionLinearPrimaryMax
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The maximum linear primary friction can not be less than zero.")
                End If
                SetSimData("FrictionLinearPrimaryMax", Value.ActualValue.ToString, True)
                m_snFrictionLinearPrimaryMax.CopyData(Value)
            End Set
        End Property

        Public Overridable Property FrictionLinearSecondaryMax() As ScaledNumber
            Get
                Return m_snFrictionLinearSecondaryMax
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The maximum linear secondary friction can not be less than zero.")
                End If
                SetSimData("FrictionLinearSecondaryMax", Value.ActualValue.ToString, True)
                m_snFrictionLinearSecondaryMax.CopyData(Value)
            End Set
        End Property

        Public Overridable Property FrictionAngularNormalMax() As ScaledNumber
            Get
                Return m_snFrictionAngularNormalMax
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The maximum angular normal friction can not be less than zero.")
                End If
                SetSimData("FrictionAngularNormalMax", Value.ActualValue.ToString, True)
                m_snFrictionAngularNormalMax.CopyData(Value)
            End Set
        End Property

        Public Overridable Property FrictionAngularPrimaryMax() As ScaledNumber
            Get
                Return m_snFrictionAngularPrimaryMax
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The maximum primary normal friction can not be less than zero.")
                End If
                SetSimData("FrictionAngularPrimaryMax", Value.ActualValue.ToString, True)
                m_snFrictionAngularPrimaryMax.CopyData(Value)
            End Set
        End Property

        Public Overridable Property FrictionAngularSecondaryMax() As ScaledNumber
            Get
                Return m_snFrictionAngularSecondaryMax
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The maximum secondary normal friction can not be less than zero.")
                End If
                SetSimData("FrictionAngularSecondaryMax", Value.ActualValue.ToString, True)
                m_snFrictionAngularSecondaryMax.CopyData(Value)
            End Set
        End Property

        Public Overridable Property Compliance() As ScaledNumber
            Get
                Return m_snCompliance
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The compliance of the collision between materials can not be less than zero.")
                End If
                SetSimData("Compliance", Value.ActualValue.ToString, True)
                m_snCompliance.CopyData(Value)
            End Set
        End Property


        Public Overridable Property Damping() As ScaledNumber
            Get
                Return m_snDamping
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The damping of the collision between materials can not be less than zero.")
                End If
                SetSimData("Damping", Value.ActualValue.ToString, True)
                m_snDamping.CopyData(Value)
            End Set
        End Property

        Public Overridable Property Restitution() As ScaledNumber
            Get
                Return m_snRestitution
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The restitution of the collision between materials can not be less than zero.")
                End If
                SetSimData("Restitution", Value.ActualValue.ToString, True)
                m_snRestitution.CopyData(Value)
            End Set
        End Property

        Public Overridable Property SlipLinearPrimary() As ScaledNumber
            Get
                Return m_snSlipLinearPrimary
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The primary linear slip of the collision between materials can not be less than zero.")
                End If
                SetSimData("SlipLinearPrimary", Value.ActualValue.ToString, True)
                m_snSlipLinearPrimary.CopyData(Value)
            End Set
        End Property

        Public Overridable Property SlipLinearSecondary() As ScaledNumber
            Get
                Return m_snSlipLinearSecondary
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The secondary linear slip of the collision between materials can not be less than zero.")
                End If
                SetSimData("SlipLinearSecondary", Value.ActualValue.ToString, True)
                m_snSlipLinearSecondary.CopyData(Value)
            End Set
        End Property

        Public Overridable Property SlipAngularNormal() As ScaledNumber
            Get
                Return m_snSlipAngularNormal
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The angular normal slip of the collision between materials can not be less than zero.")
                End If
                SetSimData("SlipAngularNormal", Value.ActualValue.ToString, True)
                m_snSlipAngularNormal.CopyData(Value)
            End Set
        End Property

        Public Overridable Property SlipAngularPrimary() As ScaledNumber
            Get
                Return m_snSlipAngularPrimary
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The angular primary slip of the collision between materials can not be less than zero.")
                End If
                SetSimData("SlipAngularPrimary", Value.ActualValue.ToString, True)
                m_snSlipAngularPrimary.CopyData(Value)
            End Set
        End Property

        Public Overridable Property SlipAngularSecondary() As ScaledNumber
            Get
                Return m_snSlipAngularSecondary
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The angular secondary slip of the collision between materials can not be less than zero.")
                End If
                SetSimData("SlipAngularSecondary", Value.ActualValue.ToString, True)
                m_snSlipAngularSecondary.CopyData(Value)
            End Set
        End Property

        Public Overridable Property SlideLinearPrimary() As ScaledNumber
            Get
                Return m_snSlideLinearPrimary
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The primary linear slide of the collision between materials can not be less than zero.")
                End If
                SetSimData("SlideLinearPrimary", Value.ActualValue.ToString, True)
                m_snSlideLinearPrimary.CopyData(Value)
            End Set
        End Property

        Public Overridable Property SlideLinearSecondary() As ScaledNumber
            Get
                Return m_snSlideLinearSecondary
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The secondary linear slide of the collision between materials can not be less than zero.")
                End If
                SetSimData("SlideLinearSecondary", Value.ActualValue.ToString, True)
                m_snSlideLinearSecondary.CopyData(Value)
            End Set
        End Property

        Public Overridable Property SlideAngularNormal() As ScaledNumber
            Get
                Return m_snSlideAngularNormal
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The angular normal slide of the collision between materials can not be less than zero.")
                End If
                SetSimData("SlideAngularNormal", Value.ActualValue.ToString, True)
                m_snSlideAngularNormal.CopyData(Value)
            End Set
        End Property

        Public Overridable Property SlideAngularPrimary() As ScaledNumber
            Get
                Return m_snSlideAngularPrimary
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The angular primary slide of the collision between materials can not be less than zero.")
                End If
                SetSimData("SlideAngularPrimary", Value.ActualValue.ToString, True)
                m_snSlideAngularPrimary.CopyData(Value)
            End Set
        End Property

        Public Overridable Property SlideAngularSecondary() As ScaledNumber
            Get
                Return m_snSlideAngularSecondary
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The angular secondary slide of the collision between materials can not be less than zero.")
                End If
                SetSimData("SlideAngularSecondary", Value.ActualValue.ToString, True)
                m_snSlideAngularSecondary.CopyData(Value)
            End Set
        End Property

        Public Overridable Property MaxAdhesion() As ScaledNumber
            Get
                Return m_snMaxAdhesion
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The maximum adhesion of the collision between materials can not be less than zero.")
                End If
                SetSimData("MaxAdhesion", Value.ActualValue.ToString, True)
                m_snMaxAdhesion.CopyData(Value)
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            m_snFrictionLinearPrimary = New AnimatGUI.Framework.ScaledNumber(Me, "FrictionLinearPrimary", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None)
            m_snFrictionLinearSecondary = New AnimatGUI.Framework.ScaledNumber(Me, "FrictionLinearSecondary", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None)
            m_snFrictionAngularNormal = New AnimatGUI.Framework.ScaledNumber(Me, "FrictionAngularNormal", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None)
            m_snFrictionAngularPrimary = New AnimatGUI.Framework.ScaledNumber(Me, "FrictionAngularPrimary", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None)
            m_snFrictionAngularSecondary = New AnimatGUI.Framework.ScaledNumber(Me, "FrictionAngularSecondary", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None)
            m_snFrictionLinearPrimaryMax = New AnimatGUI.Framework.ScaledNumber(Me, "FrictionLinearPrimaryMax", 10, ScaledNumber.enumNumericScale.None, "Newtons", "N")
            m_snFrictionLinearSecondaryMax = New AnimatGUI.Framework.ScaledNumber(Me, "FrictionLinearSecondaryMax", 10, ScaledNumber.enumNumericScale.None, "Newtons", "N")
            m_snFrictionAngularNormalMax = New AnimatGUI.Framework.ScaledNumber(Me, "FrictionAngularNormalMax", 10, ScaledNumber.enumNumericScale.None, "Newtons", "N")
            m_snFrictionAngularPrimaryMax = New AnimatGUI.Framework.ScaledNumber(Me, "FrictionAngularPrimaryMax", 10, ScaledNumber.enumNumericScale.None, "Newtons", "N")
            m_snFrictionAngularSecondaryMax = New AnimatGUI.Framework.ScaledNumber(Me, "FrictionAngularSecondaryMax", 10, ScaledNumber.enumNumericScale.None, "Newtons", "N")
            m_snCompliance = New AnimatGUI.Framework.ScaledNumber(Me, "Compliance", 0.1, ScaledNumber.enumNumericScale.micro, "m/N", "m/N")
            m_snDamping = New AnimatGUI.Framework.ScaledNumber(Me, "Damping", 5000, ScaledNumber.enumNumericScale.Kilo, "g/s", "g/s")
            m_snRestitution = New AnimatGUI.Framework.ScaledNumber(Me, "Restitution", 0, ScaledNumber.enumNumericScale.None)
            m_snSlipLinearPrimary = New AnimatGUI.Framework.ScaledNumber(Me, "SlipLinearPrimary", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "s/Kg", "s/Kg")
            m_snSlipLinearSecondary = New AnimatGUI.Framework.ScaledNumber(Me, "SlipLinearSecondary", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "s/Kg", "s/Kg")
            m_snSlipAngularNormal = New AnimatGUI.Framework.ScaledNumber(Me, "SlipAngularNormal", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "s/Kg", "s/Kg")
            m_snSlipAngularPrimary = New AnimatGUI.Framework.ScaledNumber(Me, "SlipAngularPrimary", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "s/Kg", "s/Kg")
            m_snSlipAngularSecondary = New AnimatGUI.Framework.ScaledNumber(Me, "SlipAngularSecondary", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "s/Kg", "s/Kg")
            m_snSlideLinearPrimary = New AnimatGUI.Framework.ScaledNumber(Me, "SlideLinearPrimary", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "m/s", "m/s")
            m_snSlideLinearSecondary = New AnimatGUI.Framework.ScaledNumber(Me, "SlideLinearSecondary", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "m/s", "m/s")
            m_snSlideAngularNormal = New AnimatGUI.Framework.ScaledNumber(Me, "SlideAngularNormal", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "m/s", "m/s")
            m_snSlideAngularPrimary = New AnimatGUI.Framework.ScaledNumber(Me, "SlideAngularPrimary", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "m/s", "m/s")
            m_snSlideAngularSecondary = New AnimatGUI.Framework.ScaledNumber(Me, "SlideAngularSecondary", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "m/s", "m/s")
            m_snMaxAdhesion = New AnimatGUI.Framework.ScaledNumber(Me, "MaxAdhesion", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Newtons", "N")

            If Not Util.Environment Is Nothing Then
                Compliance.ActualValue = 0.000001 * Util.ConvertMassUnits(Util.Environment.MassUnits.ToString)
                Damping.ActualValue = 50000000 / Util.ConvertMassUnits(Util.Environment.MassUnits.ToString)
            End If

        End Sub

        Public Overrides Sub RegisterMaterialType()
            MaterialType.RegisterMaterialType("Vortex", Me)
        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            m_snFrictionLinearPrimary.ClearIsDirty()
            m_snFrictionLinearSecondary.ClearIsDirty()
            m_snFrictionAngularNormal.ClearIsDirty()
            m_snFrictionAngularPrimary.ClearIsDirty()
            m_snFrictionAngularSecondary.ClearIsDirty()
            m_snFrictionLinearPrimaryMax.ClearIsDirty()
            m_snFrictionLinearSecondaryMax.ClearIsDirty()
            m_snFrictionAngularNormalMax.ClearIsDirty()
            m_snFrictionAngularPrimaryMax.ClearIsDirty()
            m_snFrictionAngularSecondaryMax.ClearIsDirty()
            m_snCompliance.ClearIsDirty()
            m_snDamping.ClearIsDirty()
            m_snRestitution.ClearIsDirty()
            m_snSlipLinearPrimary.ClearIsDirty()
            m_snSlipLinearSecondary.ClearIsDirty()
            m_snSlipAngularNormal.ClearIsDirty()
            m_snSlipAngularPrimary.ClearIsDirty()
            m_snSlipAngularSecondary.ClearIsDirty()
            m_snSlideLinearPrimary.ClearIsDirty()
            m_snSlideLinearSecondary.ClearIsDirty()
            m_snSlideAngularNormal.ClearIsDirty()
            m_snSlideAngularPrimary.ClearIsDirty()
            m_snSlideAngularSecondary.ClearIsDirty()
            m_snMaxAdhesion.ClearIsDirty()

        End Sub

        Public Overrides Function Clone(ByVal doParent As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject) As Framework.DataObject
            Dim oNewNode As New MaterialTypeVortex(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doOrig As MaterialTypeVortex = DirectCast(doOriginal, MaterialTypeVortex)

            m_snFrictionLinearPrimary = DirectCast(doOrig.m_snFrictionLinearPrimary.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snFrictionLinearSecondary = DirectCast(doOrig.m_snFrictionLinearSecondary.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snFrictionAngularNormal = DirectCast(doOrig.m_snFrictionAngularNormal.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snFrictionAngularPrimary = DirectCast(doOrig.m_snFrictionAngularPrimary.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snFrictionAngularSecondary = DirectCast(doOrig.m_snFrictionAngularSecondary.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snFrictionLinearPrimaryMax = DirectCast(doOrig.m_snFrictionLinearPrimaryMax.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snFrictionLinearSecondaryMax = DirectCast(doOrig.m_snFrictionLinearSecondaryMax.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snFrictionAngularNormalMax = DirectCast(doOrig.m_snFrictionAngularNormalMax.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snFrictionAngularPrimaryMax = DirectCast(doOrig.m_snFrictionAngularPrimaryMax.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snFrictionAngularSecondaryMax = DirectCast(doOrig.m_snFrictionAngularSecondaryMax.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snCompliance = DirectCast(doOrig.m_snCompliance.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snDamping = DirectCast(doOrig.m_snDamping.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snRestitution = DirectCast(doOrig.m_snRestitution.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snSlipLinearPrimary = DirectCast(doOrig.m_snSlipLinearPrimary.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snSlipLinearSecondary = DirectCast(doOrig.m_snSlipLinearSecondary.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snSlipAngularNormal = DirectCast(doOrig.m_snSlipAngularNormal.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snSlipAngularPrimary = DirectCast(doOrig.m_snSlipAngularPrimary.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snSlipAngularSecondary = DirectCast(doOrig.m_snSlipAngularSecondary.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snSlideLinearPrimary = DirectCast(doOrig.m_snSlideLinearPrimary.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snSlideLinearSecondary = DirectCast(doOrig.m_snSlideLinearSecondary.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snSlideAngularNormal = DirectCast(doOrig.m_snSlideAngularNormal.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snSlideAngularPrimary = DirectCast(doOrig.m_snSlideAngularPrimary.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snSlideAngularSecondary = DirectCast(doOrig.m_snSlideAngularSecondary.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snMaxAdhesion = DirectCast(doOrig.m_snMaxAdhesion.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)

        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Name", m_strID.GetType(), "Name", _
                                        "Material Properties", "The name of this material.", m_strName, False))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("ID", Me.ID.GetType(), "ID", _
                                        "Material Properties", "ID", Me.ID, True))

            pbNumberBag = m_snFrictionLinearPrimary.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Linear Primary", pbNumberBag.GetType(), "FrictionLinearPrimary", _
                            "Friction Coefficients", "The primary coefficient of friction", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snFrictionLinearSecondary.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Linear Secondary", pbNumberBag.GetType(), "FrictionLinearSecondary", _
                            "Friction Coefficients", "The secondary coefficient of friction", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snFrictionAngularNormal.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Angular Normal", pbNumberBag.GetType(), "FrictionAngularNormal", _
                            "Friction Coefficients", "The friction for angular motion around the normal (this simulates spinning friction)", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snFrictionAngularPrimary.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Angular Primary", pbNumberBag.GetType(), "FrictionAngularPrimary", _
                            "Friction Coefficients", "The friction for angular motion around the primary axis (this simulates rolling resistance)", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snFrictionAngularSecondary.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Angular Secondary", pbNumberBag.GetType(), "FrictionAngularSecondary", _
                            "Friction Coefficients", "The friction for angular motion around the secondary axis (this simulates rolling resistance)", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snFrictionLinearPrimaryMax.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Linear Primary", pbNumberBag.GetType(), "FrictionLinearPrimaryMax", _
                            "Max Frictions", "The maximum linear primary friction allowed", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snFrictionLinearSecondaryMax.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Linear Secondary", pbNumberBag.GetType(), "FrictionLinearSecondaryMax", _
                            "Max Frictions", "The maximum linear secondary friction allowed", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snFrictionAngularNormalMax.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Angular Normal", pbNumberBag.GetType(), "FrictionAngularNormalMax", _
                            "Max Frictions", "The maximum angular normal friction allowed", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snFrictionAngularPrimaryMax.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Angular Primary", pbNumberBag.GetType(), "FrictionAngularPrimaryMax", _
                            "Max Frictions", "The maximum angular primary friction allowed", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snFrictionAngularSecondaryMax.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Angular Secondary", pbNumberBag.GetType(), "FrictionAngularSecondaryMax", _
                            "Max Frictions", "The maximum angular secondary friction allowed", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snCompliance.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Compliance", pbNumberBag.GetType(), "Compliance", _
                            "Material Properties", "The compliance for collisions between RigidBodies with these two materials.", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snDamping.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Damping", pbNumberBag.GetType(), "Damping", _
                            "Material Properties", "The damping for collisions between RigidBodies with these two materials.", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snRestitution.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Restitution", pbNumberBag.GetType(), "Restitution", _
                            "Material Properties", "When a collision occurs between two rigid bodies, the impulse corresponding to the force is equal" & _
                            " to the total change in momentum that each body undergoes. This change of momentum is affected by the degree" & _
                            " of resilience of each body, that is, the extent to which energy is diffused.<br>The coefficient of restitution" & _
                            " is a parameter representing the degree of resilience of a particular material pair. To make simulations more " & _
                            " efficient, it is best to set a restitution threshold as well. Impacts that measure less than the threshold will " & _
                            "be ignored, to avoid jitter in the simulation. Small impulses do not add to the realism of most simulations.", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snSlipLinearPrimary.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Linear Primary", pbNumberBag.GetType(), "SlipLinearPrimary", _
                            "Slip", "Contact slip allows a tangential loss at the contact position to be defined. For example, this" & _
                            " is a useful parameter to set for the interaction between a cylindrical wheel and a terrain where, without a " & _
                            "minimum amount of slip, the vehicle would have a hard time turning.", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snSlipLinearSecondary.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Linear Secondary", pbNumberBag.GetType(), "SlipLinearSecondary", _
                            "Slip", "Contact slip allows a tangential loss at the contact position to be defined. For example, this" & _
                            " is a useful parameter to set for the interaction between a cylindrical wheel and a terrain where, without a " & _
                            "minimum amount of slip, the vehicle would have a hard time turning.", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snSlipAngularNormal.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Angular Normal", pbNumberBag.GetType(), "SlipAngularNormal", _
                            "Slip", "Contact slip allows a tangential loss at the contact position to be defined. For example, this" & _
                            " is a useful parameter to set for the interaction between a cylindrical wheel and a terrain where, without a " & _
                            "minimum amount of slip, the vehicle would have a hard time turning.", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snSlipAngularPrimary.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Angular Primary", pbNumberBag.GetType(), "SlipAngularPrimary", _
                            "Slip", "Contact slip allows a tangential loss at the contact position to be defined. For example, this" & _
                            " is a useful parameter to set for the interaction between a cylindrical wheel and a terrain where, without a " & _
                            "minimum amount of slip, the vehicle would have a hard time turning.", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snSlipAngularSecondary.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Angular Secondary", pbNumberBag.GetType(), "SlipAngularSecondary", _
                            "Slip", "Contact slip allows a tangential loss at the contact position to be defined. For example, this" & _
                            " is a useful parameter to set for the interaction between a cylindrical wheel and a terrain where, without a " & _
                            "minimum amount of slip, the vehicle would have a hard time turning.", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snSlideLinearPrimary.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Linear Primary", pbNumberBag.GetType(), "SlideLinearPrimary", _
                            "Slide", "The contact sliding parameter allows a desired relative linear velocity to be specified between" & _
                            " the colliding parts at the contact position. A conveyor belt would be an example of an application. The belt " & _
                            "part itself would not be moving.", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snSlideLinearSecondary.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Linear Secondary", pbNumberBag.GetType(), "SlideLinearSecondary", _
                            "Slide", "The contact sliding parameter allows a desired relative linear velocity to be specified between" & _
                            " the colliding parts at the contact position. A conveyor belt would be an example of an application. The belt " & _
                            "part itself would not be moving.", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snSlideAngularNormal.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Angular Normal", pbNumberBag.GetType(), "SlideAngularNormal", _
                            "Slide", "The contact sliding parameter allows a desired relative linear velocity to be specified between" & _
                            " the colliding parts at the contact position. A conveyor belt would be an example of an application. The belt " & _
                            "part itself would not be moving.", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snSlideAngularPrimary.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Angular Primary", pbNumberBag.GetType(), "SlideAngularPrimary", _
                            "Slide", "The contact sliding parameter allows a desired relative linear velocity to be specified between" & _
                            " the colliding parts at the contact position. A conveyor belt would be an example of an application. The belt " & _
                            "part itself would not be moving.", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snSlideAngularSecondary.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Angular Secondary", pbNumberBag.GetType(), "SlideAngularSecondary", _
                            "Slide", "The contact sliding parameter allows a desired relative linear velocity to be specified between" & _
                            " the colliding parts at the contact position. A conveyor belt would be an example of an application. The belt " & _
                            "part itself would not be moving.", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snMaxAdhesion.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Maximum Adhesion", pbNumberBag.GetType(), "MaxAdhesion", _
                            "Material Properties", "Adhesive force allows objects to stick together, as if they were glued. This property provides " & _
                            "the minimal force needed to separate the two objects.", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

        End Sub

        Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            oXml.IntoElem()

            m_strID = oXml.GetChildString("ID")
            m_strName = oXml.GetChildString("Name")

            If oXml.FindChildElement("FrictionLinearPrimary", False) Then m_snFrictionLinearPrimary.LoadData(oXml, "FrictionLinearPrimary")
            If oXml.FindChildElement("FrictionLinearSecondary", False) Then m_snFrictionLinearSecondary.LoadData(oXml, "FrictionLinearSecondary")
            If oXml.FindChildElement("FrictionAngularNormal", False) Then m_snFrictionAngularNormal.LoadData(oXml, "FrictionAngularNormal")
            If oXml.FindChildElement("FrictionAngularPrimary", False) Then m_snFrictionAngularPrimary.LoadData(oXml, "FrictionAngularPrimary")
            If oXml.FindChildElement("FrictionAngularSecondary", False) Then m_snFrictionAngularSecondary.LoadData(oXml, "FrictionAngularSecondary")
            If oXml.FindChildElement("FrictionLinearPrimaryMax", False) Then m_snFrictionLinearPrimaryMax.LoadData(oXml, "FrictionLinearPrimaryMax")
            If oXml.FindChildElement("FrictionLinearSecondaryMax", False) Then m_snFrictionLinearSecondaryMax.LoadData(oXml, "FrictionLinearSecondaryMax")
            If oXml.FindChildElement("FrictionAngularNormalMax", False) Then m_snFrictionAngularNormalMax.LoadData(oXml, "FrictionAngularNormalMax")
            If oXml.FindChildElement("FrictionAngularPrimaryMax", False) Then m_snFrictionAngularPrimaryMax.LoadData(oXml, "FrictionAngularPrimaryMax")
            If oXml.FindChildElement("FrictionAngularSecondaryMax", False) Then m_snFrictionAngularSecondaryMax.LoadData(oXml, "FrictionAngularSecondaryMax")
            If oXml.FindChildElement("Compliance", False) Then m_snCompliance.LoadData(oXml, "Compliance")
            If oXml.FindChildElement("Damping", False) Then m_snDamping.LoadData(oXml, "Damping")
            If oXml.FindChildElement("Restitution", False) Then m_snRestitution.LoadData(oXml, "Restitution")

            If oXml.FindChildElement("SlipLinearPrimary", False) Then m_snSlipLinearPrimary.LoadData(oXml, "SlipLinearPrimary")
            If oXml.FindChildElement("SlipLinearSecondary", False) Then m_snSlipLinearSecondary.LoadData(oXml, "SlipLinearSecondary")
            If oXml.FindChildElement("SlipAngularNormal", False) Then m_snSlipAngularNormal.LoadData(oXml, "SlipAngularNormal")
            If oXml.FindChildElement("SlipAngularPrimary", False) Then m_snSlipAngularPrimary.LoadData(oXml, "SlipAngularPrimary")
            If oXml.FindChildElement("SlipAngularSecondary", False) Then m_snSlipAngularSecondary.LoadData(oXml, "SlipAngularSecondary")

            If oXml.FindChildElement("SlideLinearPrimary", False) Then m_snSlideLinearPrimary.LoadData(oXml, "SlideLinearPrimary")
            If oXml.FindChildElement("SlideLinearSecondary", False) Then m_snSlideLinearSecondary.LoadData(oXml, "SlideLinearSecondary")
            If oXml.FindChildElement("SlideAngularNormal", False) Then m_snSlideAngularNormal.LoadData(oXml, "SlideAngularNormal")
            If oXml.FindChildElement("SlideAngularPrimary", False) Then m_snSlideAngularPrimary.LoadData(oXml, "SlideAngularPrimary")
            If oXml.FindChildElement("SlideAngularSecondary", False) Then m_snSlideAngularSecondary.LoadData(oXml, "SlideAngularSecondary")

            If oXml.FindChildElement("MaximumAdhesion", False) Then m_snMaxAdhesion.LoadData(oXml, "MaximumAdhesion")

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            oXml.AddChildElement("MaterialType")
            oXml.IntoElem()

            oXml.AddChildElement("AssemblyFile", Me.AssemblyFile)
            oXml.AddChildElement("ClassName", Me.ClassName)

            oXml.AddChildElement("ID", Me.ID)
            oXml.AddChildElement("Name", Me.Name)

            m_snFrictionLinearPrimary.SaveData(oXml, "FrictionLinearPrimary")
            m_snFrictionLinearSecondary.SaveData(oXml, "FrictionLinearSecondary")
            m_snFrictionAngularNormal.SaveData(oXml, "FrictionAngularNormal")
            m_snFrictionAngularPrimary.SaveData(oXml, "FrictionAngularPrimary")
            m_snFrictionAngularSecondary.SaveData(oXml, "FrictionAngularSecondary")
            m_snFrictionLinearPrimaryMax.SaveData(oXml, "FrictionLinearPrimaryMax")
            m_snFrictionLinearSecondaryMax.SaveData(oXml, "FrictionLinearSecondaryMax")
            m_snFrictionAngularNormalMax.SaveData(oXml, "FrictionAngularNormalMax")
            m_snFrictionAngularPrimaryMax.SaveData(oXml, "FrictionAngularPrimaryMax")
            m_snFrictionAngularSecondaryMax.SaveData(oXml, "FrictionAngularSecondaryMax")
            m_snCompliance.SaveData(oXml, "Compliance")
            m_snDamping.SaveData(oXml, "Damping")
            m_snRestitution.SaveData(oXml, "Restitution")

            m_snSlipLinearPrimary.SaveData(oXml, "SlipLinearPrimary")
            m_snSlipLinearSecondary.SaveData(oXml, "SlipLinearSecondary")
            m_snSlipAngularNormal.SaveData(oXml, "SlipAngularNormal")
            m_snSlipAngularPrimary.SaveData(oXml, "SlipAngularPrimary")
            m_snSlipAngularSecondary.SaveData(oXml, "SlipAngularSecondary")

            m_snSlideLinearPrimary.SaveData(oXml, "SlideLinearPrimary")
            m_snSlideLinearSecondary.SaveData(oXml, "SlideLinearSecondary")
            m_snSlideAngularNormal.SaveData(oXml, "SlideAngularNormal")
            m_snSlideAngularPrimary.SaveData(oXml, "SlideAngularPrimary")
            m_snSlideAngularSecondary.SaveData(oXml, "SlideAngularSecondary")

            m_snMaxAdhesion.SaveData(oXml, "MaximumAdhesion")

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            oXml.AddChildElement("MaterialType")
            oXml.IntoElem()

            oXml.AddChildElement("ID", Me.ID)
            oXml.AddChildElement("Name", Me.Name)
            oXml.AddChildElement("Type", "Vortex")

            m_snFrictionLinearPrimary.SaveSimulationXml(oXml, Me, "FrictionLinearPrimary")
            m_snFrictionLinearSecondary.SaveSimulationXml(oXml, Me, "FrictionLinearSecondary")
            m_snFrictionAngularNormal.SaveSimulationXml(oXml, Me, "FrictionAngularNormal")
            m_snFrictionAngularPrimary.SaveSimulationXml(oXml, Me, "FrictionAngularPrimary")
            m_snFrictionAngularSecondary.SaveSimulationXml(oXml, Me, "FrictionAngularSecondary")
            m_snFrictionLinearPrimaryMax.SaveSimulationXml(oXml, Me, "FrictionLinearPrimaryMax")
            m_snFrictionLinearSecondaryMax.SaveSimulationXml(oXml, Me, "FrictionLinearSecondaryMax")
            m_snFrictionAngularNormalMax.SaveSimulationXml(oXml, Me, "FrictionAngularNormalMax")
            m_snFrictionAngularPrimaryMax.SaveSimulationXml(oXml, Me, "FrictionAngularPrimaryMax")
            m_snFrictionAngularSecondaryMax.SaveSimulationXml(oXml, Me, "FrictionAngularSecondaryMax")
            m_snCompliance.SaveSimulationXml(oXml, Me, "Compliance")
            m_snDamping.SaveSimulationXml(oXml, Me, "Damping")
            m_snRestitution.SaveSimulationXml(oXml, Me, "Restitution")

            m_snSlipLinearPrimary.SaveSimulationXml(oXml, Me, "SlipLinearPrimary")
            m_snSlipLinearSecondary.SaveSimulationXml(oXml, Me, "SlipLinearSecondary")
            m_snSlipAngularNormal.SaveSimulationXml(oXml, Me, "SlipAngularNormal")
            m_snSlipAngularPrimary.SaveSimulationXml(oXml, Me, "SlipAngularPrimary")
            m_snSlipAngularSecondary.SaveSimulationXml(oXml, Me, "SlipAngularSecondary")

            m_snSlideLinearPrimary.SaveSimulationXml(oXml, Me, "SlideLinearPrimary")
            m_snSlideLinearSecondary.SaveSimulationXml(oXml, Me, "SlideLinearSecondary")
            m_snSlideAngularNormal.SaveSimulationXml(oXml, Me, "SlideAngularNormal")
            m_snSlideAngularPrimary.SaveSimulationXml(oXml, Me, "SlideAngularPrimary")
            m_snSlideAngularSecondary.SaveSimulationXml(oXml, Me, "SlideAngularSecondary")

            m_snMaxAdhesion.SaveSimulationXml(oXml, Me, "MaximumAdhesion")

            oXml.OutOfElem()
        End Sub

#End Region

    End Class

End Namespace

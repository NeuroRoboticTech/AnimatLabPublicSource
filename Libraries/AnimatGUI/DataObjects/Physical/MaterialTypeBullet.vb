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

    Public Class MaterialTypeBullet
        Inherits MaterialType

#Region " Attributes "

        'The primary coefficient of friction parameter.
        Protected m_snFrictionLinearPrimary As ScaledNumber

        'The angular primary coefficient of friction parameter.
        Protected m_snFrictionAngularPrimary As ScaledNumber

        'The restitution of the collision between those two materials.
        Protected m_snRestitution As ScaledNumber

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

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            m_snFrictionLinearPrimary = New AnimatGUI.Framework.ScaledNumber(Me, "FrictionLinearPrimary", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None)
            m_snFrictionAngularPrimary = New AnimatGUI.Framework.ScaledNumber(Me, "FrictionAngularPrimary", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None)
            m_snRestitution = New AnimatGUI.Framework.ScaledNumber(Me, "Restitution", 0, ScaledNumber.enumNumericScale.None)

        End Sub

        Public Overrides Sub RegisterMaterialType()
            MaterialType.RegisterMaterialType("Bullet", Me)
        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            m_snFrictionLinearPrimary.ClearIsDirty()
            m_snFrictionAngularPrimary.ClearIsDirty()
            m_snRestitution.ClearIsDirty()

        End Sub

        Public Overrides Function Clone(ByVal doParent As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject) As Framework.DataObject
            Dim oNewNode As New MaterialTypeBullet(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doOrig As MaterialTypeBullet = DirectCast(doOriginal, MaterialTypeBullet)

            m_snFrictionLinearPrimary = DirectCast(doOrig.m_snFrictionLinearPrimary.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snFrictionAngularPrimary = DirectCast(doOrig.m_snFrictionAngularPrimary.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snRestitution = DirectCast(doOrig.m_snRestitution.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)

        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Name", m_strID.GetType(), "Name", _
                                        "Material Properties", "The name of this material.", m_strName, False))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("ID", Me.ID.GetType(), "ID", _
                                        "Material Properties", "ID", Me.ID, True))

            pbNumberBag = m_snFrictionLinearPrimary.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Friction", pbNumberBag.GetType(), "FrictionLinearPrimary", _
                            "Material Properties", "The primary coefficient of friction", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snFrictionAngularPrimary.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Rolling Friction", pbNumberBag.GetType(), "FrictionAngularPrimary", _
                            "Material Properties", "The friction for angular motion around the primary axis (this simulates rolling resistance)", pbNumberBag, _
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

        End Sub

        Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            oXml.IntoElem()

            m_strID = oXml.GetChildString("ID")
            m_strName = oXml.GetChildString("Name")

            If oXml.FindChildElement("FrictionLinearPrimary", False) Then m_snFrictionLinearPrimary.LoadData(oXml, "FrictionLinearPrimary")
            If oXml.FindChildElement("FrictionAngularPrimary", False) Then m_snFrictionAngularPrimary.LoadData(oXml, "FrictionAngularPrimary")
            If oXml.FindChildElement("Restitution", False) Then m_snRestitution.LoadData(oXml, "Restitution")

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
            m_snFrictionAngularPrimary.SaveData(oXml, "FrictionAngularPrimary")
            m_snRestitution.SaveData(oXml, "Restitution")

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            oXml.AddChildElement("MaterialType")
            oXml.IntoElem()

            oXml.AddChildElement("ID", Me.ID)
            oXml.AddChildElement("Name", Me.Name)
            oXml.AddChildElement("Type", "Default")

            m_snFrictionLinearPrimary.SaveSimulationXml(oXml, Me, "FrictionLinearPrimary")
            m_snFrictionAngularPrimary.SaveSimulationXml(oXml, Me, "FrictionAngularPrimary")
            m_snRestitution.SaveSimulationXml(oXml, Me, "Restitution")

            oXml.OutOfElem()
        End Sub

#End Region

    End Class

End Namespace

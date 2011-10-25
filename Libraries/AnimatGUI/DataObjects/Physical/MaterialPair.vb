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
Imports AnimatGUI.DataObjects.Physical
Imports AnimatGUI.Framework

Namespace DataObjects.Physical
    'This associates two different matieralTypes and has the physical interaction properties for when those two types collide. 
    'Things like coefficient of static friction.
    Public Class MaterialPair
        Inherits AnimatGUI.Framework.DataObject

#Region " Attributes "

        Protected m_mtMaterial1 As MaterialType
        Protected m_mtMaterial2 As MaterialType

        Protected m_strMaterial1ID As String = ""
        Protected m_strMaterial2ID As String = ""

        'The primary coefficient of friction parameter.
        Protected m_snFrictionPrimary As ScaledNumber

        'The secondary coefficient of friction parameter.
        Protected m_snFrictionSecondary As ScaledNumber

        'The maximum primary friction that can be created.
        Protected m_snMaxFrictionPrimary As ScaledNumber

        'The maximum secondary friction that can created.
        Protected m_snMaxFrictionSecondary As ScaledNumber

        'The compliance of the collision between those two materials.
        Protected m_snCompliance As ScaledNumber

        'The damping of the collision between those two materials.
        Protected m_snDamping As ScaledNumber

        'The restitution of the collision between those two materials.
        Protected m_snRestitution As ScaledNumber

        'The primary slip of the collision between those two materials.
        Protected m_snSlipPrimary As ScaledNumber

        'The secondary slip of the collision between those two materials.
        Protected m_snSlipSecondary As ScaledNumber

        'The primary slide of the collision between those two materials.
        Protected m_snSlidePrimary As ScaledNumber

        'The primary slide of the collision between those two materials.
        Protected m_snSlideSecondary As ScaledNumber

        'The maximum adhesion of the collision between those two materials.
        Protected m_snMaxAdhesion As ScaledNumber

#End Region

#Region " Properties "

        Public Overridable Property FrictionPrimary() As ScaledNumber
            Get
                Return m_snFrictionPrimary
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The primary coefficient of friction can not be less than zero.")
                End If
                m_snFrictionPrimary.CopyData(Value)
            End Set
        End Property

        Public Overridable Property FrictionSecondary() As ScaledNumber
            Get
                Return m_snFrictionSecondary
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The secondary coefficient of friction can not be less than zero.")
                End If
                m_snFrictionSecondary.CopyData(Value)
            End Set
        End Property

        Public Overridable Property MaxFrictionPrimary() As ScaledNumber
            Get
                Return m_snMaxFrictionPrimary
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The maximum primary friction can not be less than zero.")
                End If
                m_snMaxFrictionPrimary.CopyData(Value)
            End Set
        End Property

        Public Overridable Property MaxSecondaryFriction() As ScaledNumber
            Get
                Return m_snMaxFrictionSecondary
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The maximum secondary friction can not be less than zero.")
                End If
                m_snMaxFrictionSecondary.CopyData(Value)
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
                m_snRestitution.CopyData(Value)
            End Set
        End Property

        Public Overridable Property PrimarySlip() As ScaledNumber
            Get
                Return m_snSlipPrimary
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The primary slip of the collision between materials can not be less than zero.")
                End If
                m_snSlipPrimary.CopyData(Value)
            End Set
        End Property

        Public Overridable Property SecondarySlip() As ScaledNumber
            Get
                Return m_snSlipSecondary
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The secondary slip of the collision between materials can not be less than zero.")
                End If
                m_snSlipSecondary.CopyData(Value)
            End Set
        End Property

        Public Overridable Property PrimarySlide() As ScaledNumber
            Get
                Return m_snSlidePrimary
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The primary slide of the collision between materials can not be less than zero.")
                End If
                m_snSlidePrimary.CopyData(Value)
            End Set
        End Property

        Public Overridable Property SecondarySlide() As ScaledNumber
            Get
                Return m_snSlideSecondary
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The secondary slide of the collision between materials can not be less than zero.")
                End If
                m_snSlideSecondary.CopyData(Value)
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
                m_snMaxAdhesion.CopyData(Value)
            End Set
        End Property

        Public Overridable Property Material1() As MaterialType
            Get
                Return m_mtMaterial1
            End Get
            Set(ByVal Value As MaterialType)
                m_mtMaterial1 = Value
            End Set
        End Property

        Public Overridable Property Material2() As MaterialType
            Get
                Return m_mtMaterial2
            End Get
            Set(ByVal Value As MaterialType)
                m_mtMaterial2 = Value
            End Set
        End Property

        Public Overridable ReadOnly Property Material1Name() As String
            Get
                If Not m_mtMaterial1 Is Nothing Then
                    Return m_mtMaterial1.Name
                Else
                    Return ""
                End If
            End Get
        End Property

        Public Overridable ReadOnly Property Material2Name() As String
            Get
                If Not m_mtMaterial2 Is Nothing Then
                    Return m_mtMaterial2.Name
                Else
                    Return ""
                End If
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
            m_snFrictionPrimary = New AnimatGUI.Framework.ScaledNumber(Me, "FrictionPrimary", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None)
            m_snFrictionSecondary = New AnimatGUI.Framework.ScaledNumber(Me, "FrictionSecondary", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None)
            m_snMaxFrictionPrimary = New AnimatGUI.Framework.ScaledNumber(Me, "MaxFrictionPrimary", 500, ScaledNumber.enumNumericScale.None, "Newtons", "N")
            m_snMaxFrictionSecondary = New AnimatGUI.Framework.ScaledNumber(Me, "MaxSecondaryFriction", 500, ScaledNumber.enumNumericScale.None, "Newtons", "N")
            'Linear compliance
            m_snCompliance = New AnimatGUI.Framework.ScaledNumber(Me, "Compliance", 0.1, ScaledNumber.enumNumericScale.micro, "m/N", "m/N")
            'Linear damping
            m_snDamping = New AnimatGUI.Framework.ScaledNumber(Me, "Damping", 200, ScaledNumber.enumNumericScale.Kilo, "g/s", "g/s")
            m_snRestitution = New AnimatGUI.Framework.ScaledNumber(Me, "Restitution", 0, ScaledNumber.enumNumericScale.None)
            m_snSlipPrimary = New AnimatGUI.Framework.ScaledNumber(Me, "PrimarySlip", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "s/Kg", "s/Kg")
            m_snSlipSecondary = New AnimatGUI.Framework.ScaledNumber(Me, "SecondarySlip", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "s/Kg", "s/Kg")
            m_snSlidePrimary = New AnimatGUI.Framework.ScaledNumber(Me, "PrimarySlide", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "m/s", "m/s")
            m_snSlideSecondary = New AnimatGUI.Framework.ScaledNumber(Me, "SecondarySlide", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "m/s", "m/s")
            m_snMaxAdhesion = New AnimatGUI.Framework.ScaledNumber(Me, "MaxAdhesion", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Newtons", "N")
            m_mtMaterial1 = Nothing
            m_mtMaterial2 = Nothing
        End Sub

        Public Sub New(ByVal doParent As Framework.DataObject, ByVal mtMaterial1 As MaterialType, ByVal mtMaterial2 As MaterialType)
            Me.New(doParent)
            m_mtMaterial1 = mtMaterial1
            m_mtMaterial2 = mtMaterial2
        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            m_snFrictionPrimary.ClearIsDirty()
            m_snFrictionSecondary.ClearIsDirty()
            m_snMaxFrictionPrimary.ClearIsDirty()
            m_snMaxFrictionSecondary.ClearIsDirty()
            m_snCompliance.ClearIsDirty()
            m_snDamping.ClearIsDirty()
            m_snRestitution.ClearIsDirty()
            m_snSlipPrimary.ClearIsDirty()
            m_snSlipSecondary.ClearIsDirty()
            m_snSlidePrimary.ClearIsDirty()
            m_snSlideSecondary.ClearIsDirty()
            m_snMaxAdhesion.ClearIsDirty()

        End Sub

        Public Overrides Function Clone(ByVal doParent As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject) As Framework.DataObject
            Dim oNewNode As New MaterialPair(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doOrig As MaterialPair = DirectCast(doOriginal, MaterialPair)
            m_mtMaterial1 = DirectCast(doOrig.m_mtMaterial1, MaterialType)
            m_mtMaterial2 = DirectCast(doOrig.m_mtMaterial2, MaterialType)

            m_snFrictionPrimary = DirectCast(doOrig.m_snFrictionPrimary, ScaledNumber)
            m_snFrictionSecondary = DirectCast(doOrig.m_snFrictionSecondary, ScaledNumber)
            m_snMaxFrictionPrimary = DirectCast(doOrig.m_snMaxFrictionPrimary, ScaledNumber)
            m_snMaxFrictionSecondary = DirectCast(doOrig.m_snMaxFrictionSecondary, ScaledNumber)
            m_snCompliance = DirectCast(doOrig.m_snCompliance, ScaledNumber)
            m_snDamping = DirectCast(doOrig.m_snDamping, ScaledNumber)
            m_snRestitution = DirectCast(doOrig.m_snRestitution, ScaledNumber)
            m_snSlipPrimary = DirectCast(doOrig.m_snSlipPrimary, ScaledNumber)
            m_snSlipSecondary = DirectCast(doOrig.m_snSlipSecondary, ScaledNumber)
            m_snSlidePrimary = DirectCast(doOrig.m_snSlidePrimary, ScaledNumber)
            m_snSlideSecondary = DirectCast(doOrig.m_snSlideSecondary, ScaledNumber)
            m_snMaxAdhesion = DirectCast(doOrig.m_snMaxAdhesion, ScaledNumber)

        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Material1", GetType(String), "Material1Name", _
                            "Pair Properties", "The unique name of first material in the pair", _
                            Me.Material1Name, True))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Material2", GetType(String), "Material2Name", _
                            "Pair Properties", "The unique name of second material in the pair", _
                            Me.Material2Name, True))

            pbNumberBag = m_snFrictionPrimary.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Primary Friction Coefficient", pbNumberBag.GetType(), "FrictionPrimary", _
                            "Pair Properties", "The primary coefficient of friction", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snFrictionSecondary.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Secondary Friction Coefficient", pbNumberBag.GetType(), "FrictionSecondary", _
                            "Pair Properties", "The secondary coefficient of friction", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snMaxFrictionPrimary.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Maximum Primary Friction", pbNumberBag.GetType(), "MaxFrictionPrimary", _
                            "Pair Properties", "The maximum primary friction allowed", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snMaxFrictionSecondary.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Maximum Secondary Friction", pbNumberBag.GetType(), "MaxSecondaryFriction", _
                            "Pair Properties", "The maximum secondary friction allowed", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snCompliance.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Compliance", pbNumberBag.GetType(), "Compliance", _
                            "Pair Properties", "The compliance for collisions between RigidBodies with these two materials.", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snDamping.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Damping", pbNumberBag.GetType(), "Damping", _
                            "Pair Properties", "The damping for collisions between RigidBodies with these two materials.", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snRestitution.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Restitution", pbNumberBag.GetType(), "Restitution", _
                            "Pair Properties", "When a collision occurs between two rigid bodies, the impulse corresponding to the force is equal" & _
                            " to the total change in momentum that each body undergoes. This change of momentum is affected by the degree" & _
                            " of resilience of each body, that is, the extent to which energy is diffused.<br>The coefficient of restitution" & _
                            " is a parameter representing the degree of resilience of a particular material pair. To make simulations more " & _
                            " efficient, it is best to set a restitution threshold as well. Impacts that measure less than the threshold will " & _
                            "be ignored, to avoid jitter in the simulation. Small impulses do not add to the realism of most simulations.", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snSlipPrimary.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Primary Slip", pbNumberBag.GetType(), "PrimarySlip", _
                            "Pair Properties", "Contact slip allows a tangential loss at the contact position to be defined. For example, this" & _
                            " is a useful parameter to set for the interaction between a cylindrical wheel and a terrain where, without a " & _
                            "minimum amount of slip, the vehicle would have a hard time turning.", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snSlipSecondary.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Secondary Slip", pbNumberBag.GetType(), "SecondarySlip", _
                            "Pair Properties", "Contact slip allows a tangential loss at the contact position to be defined. For example, this" & _
                            " is a useful parameter to set for the interaction between a cylindrical wheel and a terrain where, without a " & _
                            "minimum amount of slip, the vehicle would have a hard time turning.", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snSlidePrimary.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Primary Slide", pbNumberBag.GetType(), "PrimarySlide", _
                            "Pair Properties", "The contact sliding parameter allows a desired relative linear velocity to be specified between" & _
                            " the colliding parts at the contact position. A conveyor belt would be an example of an application. The belt " & _
                            "part itself would not be moving.", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snSlideSecondary.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Secondary Slide", pbNumberBag.GetType(), "SecondarySlide", _
                            "Pair Properties", "The contact sliding parameter allows a desired relative linear velocity to be specified between" & _
                            " the colliding parts at the contact position. A conveyor belt would be an example of an application. The belt " & _
                            "part itself would not be moving.", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snMaxAdhesion.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Maximum Adhesion", pbNumberBag.GetType(), "MaxAdhesion", _
                            "Pair Properties", "Adhesive force allows objects to stick together, as if they were glued. This property provides " & _
                            "the minimal force needed to separate the two objects.", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))
        End Sub

        Public Overrides Sub InitializeAfterLoad()
            MyBase.InitializeAfterLoad()

            If m_mtMaterial1 Is Nothing Then
                m_mtMaterial1 = DirectCast(Util.Environment.MaterialTypes(m_strMaterial1ID), DataObjects.Physical.MaterialType)
            End If

            If m_mtMaterial2 Is Nothing Then
                m_mtMaterial2 = DirectCast(Util.Environment.MaterialTypes(m_strMaterial2ID), DataObjects.Physical.MaterialType)
            End If

        End Sub

        Public Overrides Sub LoadData(ByRef oXml As AnimatGUI.Interfaces.StdXml)

            oXml.IntoElem()

            m_strID = oXml.GetChildString("ID")
            m_strName = oXml.GetChildString("Name")

            m_strMaterial1ID = oXml.GetChildString("Material1ID")
            m_strMaterial2ID = oXml.GetChildString("Material2ID")

            m_snFrictionPrimary.LoadData(oXml, "FrictionPrimaryCoefficient")
            m_snFrictionSecondary.LoadData(oXml, "SecondaryFrictionCoefficient")
            m_snMaxFrictionPrimary.LoadData(oXml, "PrimaryMaximumFriction")
            m_snMaxFrictionSecondary.LoadData(oXml, "SecondaryMaximumFriction")
            m_snCompliance.LoadData(oXml, "Compliance")
            m_snDamping.LoadData(oXml, "Damping")
            m_snRestitution.LoadData(oXml, "Restitution")
            m_snSlipPrimary.LoadData(oXml, "PrimarySlip")
            m_snSlipSecondary.LoadData(oXml, "SecondarySlip")
            m_snSlidePrimary.LoadData(oXml, "PrimarySlide")
            m_snSlideSecondary.LoadData(oXml, "SecondarySlide")
            m_snMaxAdhesion.LoadData(oXml, "MaximumAdhesion")

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveData(ByRef oXml As Interfaces.StdXml)

            oXml.AddChildElement("MaterialPair")
            oXml.IntoElem()

            oXml.AddChildElement("AssemblyFile", Me.AssemblyFile)
            oXml.AddChildElement("ClassName", Me.ClassName)

            oXml.AddChildElement("ID", Me.ID)
            oXml.AddChildElement("Name", Me.Name)

            oXml.AddChildElement("Material1ID", m_mtMaterial1.ID)
            oXml.AddChildElement("Material2ID", m_mtMaterial2.ID)

            m_snFrictionPrimary.SaveData(oXml, "FrictionPrimaryCoefficient")
            m_snFrictionSecondary.SaveData(oXml, "SecondaryFrictionCoefficient")
            m_snMaxFrictionPrimary.SaveData(oXml, "PrimaryMaximumFriction")
            m_snMaxFrictionSecondary.SaveData(oXml, "SecondaryMaximumFriction")
            m_snCompliance.SaveData(oXml, "Compliance")
            m_snDamping.SaveData(oXml, "Damping")
            m_snRestitution.SaveData(oXml, "Restitution")
            m_snSlipPrimary.SaveData(oXml, "PrimarySlip")
            m_snSlipSecondary.SaveData(oXml, "SecondarySlip")
            m_snSlidePrimary.SaveData(oXml, "PrimarySlide")
            m_snSlideSecondary.SaveData(oXml, "SecondarySlide")
            m_snMaxAdhesion.SaveData(oXml, "MaximumAdhesion")

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveSimulationXml(ByRef oXml As Interfaces.StdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            oXml.AddChildElement("MaterialPair")
            oXml.IntoElem()

            oXml.AddChildElement("ID", Me.ID)
            oXml.AddChildElement("Name", Me.Name)

            oXml.AddChildElement("Material1ID", m_mtMaterial1.ID)
            oXml.AddChildElement("Material2ID", m_mtMaterial2.ID)

            m_snFrictionPrimary.SaveSimulationXml(oXml, Me, "FrictionPrimaryCoefficient")
            m_snFrictionSecondary.SaveSimulationXml(oXml, Me, "SecondaryFrictionCoefficient")
            m_snMaxFrictionPrimary.SaveSimulationXml(oXml, Me, "PrimaryMaximumFriction")
            m_snMaxFrictionSecondary.SaveSimulationXml(oXml, Me, "SecondaryMaximumFriction")
            m_snCompliance.SaveSimulationXml(oXml, Me, "Compliance")
            m_snDamping.SaveSimulationXml(oXml, Me, "Damping")
            m_snRestitution.SaveSimulationXml(oXml, Me, "Restitution")
            m_snSlipPrimary.SaveSimulationXml(oXml, Me, "PrimarySlip")
            m_snSlipSecondary.SaveSimulationXml(oXml, Me, "SecondarySlip")
            m_snSlidePrimary.SaveSimulationXml(oXml, Me, "PrimarySlide")
            m_snSlideSecondary.SaveSimulationXml(oXml, Me, "SecondarySlide")
            m_snMaxAdhesion.SaveSimulationXml(oXml, Me, "MaximumAdhesion")

            oXml.OutOfElem()
        End Sub

        Public Overrides Function ToString() As String
            If m_mtMaterial1.Name <= m_mtMaterial2.Name Then
                Return m_mtMaterial1.Name & " - " & m_mtMaterial2.Name
            Else
                Return m_mtMaterial2.Name & " - " & m_mtMaterial1.Name
            End If
        End Function

#End Region

    End Class

End Namespace

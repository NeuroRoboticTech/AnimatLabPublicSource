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

    Public Class ConstraintFriction
        Inherits Framework.DataObject

#Region " Attributes "

        'The primary coefficient of friction parameter.
        Protected m_snCoefficient As ScaledNumber

        'The maximum primary friction that can be created.
        Protected m_snMaxForce As ScaledNumber

        'The maximum secondary friction that can created.
        Protected m_snLoss As ScaledNumber

        'The static friction scale
        Protected m_snStaticFrictionScale As ScaledNumber

        'Proportional friction
        Protected m_bProportional As Boolean = True

#End Region

#Region " Properties "

        Public Overridable Property Coefficient() As ScaledNumber
            Get
                Return m_snCoefficient
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The linear primary coefficient of friction can not be less than zero.")
                End If
                SetSimData("Coefficient", Value.ActualValue.ToString, True)
                m_snCoefficient.CopyData(Value)
            End Set
        End Property

        Public Overridable Property MaxForce() As ScaledNumber
            Get
                Return m_snMaxForce
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The maximum linear primary friction can not be less than zero.")
                End If
                SetSimData("MaxForce", Value.ActualValue.ToString, True)
                m_snMaxForce.CopyData(Value)
            End Set
        End Property

        Public Overridable Property Loss() As ScaledNumber
            Get
                Return m_snLoss
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The maximum linear secondary friction can not be less than zero.")
                End If
                SetSimData("Loss", Value.ActualValue.ToString, True)
                m_snLoss.CopyData(Value)
            End Set
        End Property

        Public Overridable Property Proportional() As Boolean
            Get
                Return m_bProportional
            End Get
            Set(ByVal Value As Boolean)
                SetSimData("Proportional", Value.ToString, True)
                m_bProportional = Value
            End Set
        End Property

        Public Overridable Property StaticFrictionScale() As ScaledNumber
            Get
                Return m_snStaticFrictionScale
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The static friction scale can not be less than zero.")
                End If
                SetSimData("StaticFrictionScale", Value.ActualValue.ToString, True)
                m_snStaticFrictionScale.CopyData(Value)
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            m_snCoefficient = New AnimatGUI.Framework.ScaledNumber(Me, "Coefficient", 0.02, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None)
            m_snMaxForce = New AnimatGUI.Framework.ScaledNumber(Me, "MaxForce", 10, ScaledNumber.enumNumericScale.None, "Newtons", "N")
            m_snLoss = New AnimatGUI.Framework.ScaledNumber(Me, "Loss", 0, ScaledNumber.enumNumericScale.None, "s/Kg", "s/Kg")
            m_snStaticFrictionScale = New AnimatGUI.Framework.ScaledNumber(Me, "StaticFrictionScale", 1, ScaledNumber.enumNumericScale.None, "", "")
            m_bEnabled = True

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            m_snCoefficient.ClearIsDirty()
            m_snMaxForce.ClearIsDirty()
            m_snLoss.ClearIsDirty()
            m_snStaticFrictionScale.ClearIsDirty()

        End Sub

        Public Overrides Function Clone(ByVal doParent As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject) As Framework.DataObject
            Dim oNewNode As New ConstraintFriction(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doOrig As ConstraintFriction = DirectCast(doOriginal, ConstraintFriction)

            m_snCoefficient = DirectCast(doOrig.m_snCoefficient.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snMaxForce = DirectCast(doOrig.m_snMaxForce.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snLoss = DirectCast(doOrig.m_snLoss.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snStaticFrictionScale = DirectCast(doOrig.m_snStaticFrictionScale.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_bProportional = doOrig.m_bProportional

        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Name", m_strID.GetType(), "Name", _
                                        "Friction Properties", "The name of this material.", m_strName, True))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("ID", Me.ID.GetType(), "ID", _
                                        "Friction Properties", "ID", Me.ID, True))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Enabled", Me.Enabled.GetType(), "Enabled", _
                                        "Friction Properties", "Enabled", Me.Enabled))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Proportional", Me.Proportional.GetType(), "Proportional", _
                                        "Friction Properties", "If true then the applied friction is scaled by the force applied.", Me.Proportional))

            pbNumberBag = m_snCoefficient.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Coefficient", pbNumberBag.GetType(), "Coefficient", _
                            "Friction Properties", "The primary coefficient of friction", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snMaxForce.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Maximum Force", pbNumberBag.GetType(), "MaxForce", _
                            "Friction Properties", "The maximum friction force allowed", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snLoss.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Loss", pbNumberBag.GetType(), "Loss", _
                            "Friction Properties", "The velocity loss for friction", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snStaticFrictionScale.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Static Friction Scale", pbNumberBag.GetType(), "StaticFrictionScale", _
                            "Friction Properties", "The scale value of static vs dynamic friction", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))
        End Sub

#Region " Add-Remove to List Methods "

        Public Overrides Sub AddToSim(ByVal bThrowError As Boolean, Optional ByVal bDoNotInit As Boolean = False)
            If m_doInterface Is Nothing AndAlso Not Util.Simulation Is Nothing Then
                Util.Application.SimulationInterface.AddItem(Util.Simulation.ID, "Friction", Me.ID, Me.GetSimulationXml("Friction"), bThrowError, bDoNotInit)
                InitializeSimulationReferences()
            End If
        End Sub

        Public Overrides Sub RemoveFromSim(ByVal bThrowError As Boolean)
            If Not m_doInterface Is Nothing AndAlso Not Util.Simulation Is Nothing Then
                Util.Application.SimulationInterface.RemoveItem(Util.Simulation.ID, "Friction", Me.ID, bThrowError)
            End If
            m_doInterface = Nothing
        End Sub

#End Region

        Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            oXml.IntoElem()

            m_strID = oXml.GetChildString("ID")
            m_strName = oXml.GetChildString("Name")
            m_bEnabled = oXml.GetChildBool("Enabled")
            m_bProportional = oXml.GetChildBool("Proportional")

            m_snCoefficient.LoadData(oXml, "Coefficient")
            m_snMaxForce.LoadData(oXml, "MaxForce")
            m_snLoss.LoadData(oXml, "Loss")
            m_snStaticFrictionScale.LoadData(oXml, "StaticFrictionScale")

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            oXml.AddChildElement("Friction")
            oXml.IntoElem()

            oXml.AddChildElement("AssemblyFile", Me.AssemblyFile)
            oXml.AddChildElement("ClassName", Me.ClassName)

            oXml.AddChildElement("ID", Me.ID)
            oXml.AddChildElement("Name", Me.Name)
            oXml.AddChildElement("Enabled", Me.Enabled)
            oXml.AddChildElement("Proportional", Me.Proportional)

            m_snCoefficient.SaveData(oXml, "Coefficient")
            m_snMaxForce.SaveData(oXml, "MaxForce")
            m_snLoss.SaveData(oXml, "Loss")
            m_snStaticFrictionScale.SaveData(oXml, "StaticFrictionScale")

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            oXml.AddChildElement("Friction")
            oXml.IntoElem()

            oXml.AddChildElement("ID", Me.ID)
            oXml.AddChildElement("Name", Me.Name)
            oXml.AddChildElement("Type", "Default")
            oXml.AddChildElement("Enabled", Me.Enabled)

            m_snCoefficient.SaveSimulationXml(oXml, Me, "Coefficient")
            m_snMaxForce.SaveSimulationXml(oXml, Me, "MaxForce")
            m_snLoss.SaveSimulationXml(oXml, Me, "Loss")
            m_snStaticFrictionScale.SaveSimulationXml(oXml, Me, "StaticFrictionScale")

            oXml.OutOfElem()
        End Sub

#End Region

    End Class

#Region " ConstraintFrictionPropBagConverter "

    Public Class ConstraintFrictionPropBagConverter
        Inherits ExpandableObjectConverter

        Public Overloads Overrides Function CanConvertFrom(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal sourceType As System.Type) As Boolean

            Return MyBase.CanConvertFrom(context, sourceType)
        End Function

        Public Overloads Overrides Function CanConvertTo(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal destinationType As System.Type) As Boolean

            If destinationType Is GetType(AnimatGuiCtrls.Controls.PropertyBag) Then
                Return True
            End If
            Return MyBase.CanConvertTo(context, destinationType)

        End Function

        Public Overloads Overrides Function ConvertTo(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal culture As System.Globalization.CultureInfo, ByVal value As Object, ByVal destinationType As System.Type) As Object
            If destinationType Is GetType(String) AndAlso TypeOf (value) Is AnimatGuiCtrls.Controls.PropertyTable Then
                Dim pbValue As AnimatGuiCtrls.Controls.PropertyTable = DirectCast(value, AnimatGuiCtrls.Controls.PropertyTable)

                If Not pbValue Is Nothing AndAlso Not pbValue.Tag Is Nothing AndAlso TypeOf (pbValue.Tag) Is ConstraintFriction Then
                    Dim svValue As ConstraintFriction = DirectCast(pbValue.Tag, ConstraintFriction)
                    Return svValue.ToString
                End If

                Return ""
            ElseIf destinationType Is GetType(String) AndAlso TypeOf (value) Is ConstraintFriction Then
                Dim svValue As ConstraintFriction = DirectCast(value, ConstraintFriction)

                If svValue.Enabled Then
                    Return svValue.Coefficient.ToString
                Else
                    Return ""
                End If
            End If

            Return MyBase.ConvertTo(context, culture, value, destinationType)
        End Function

    End Class

#End Region

End Namespace

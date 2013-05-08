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

    Public Class ConstraintRelaxation
        Inherits Framework.DataObject

#Region " Enums "

        Public Enum enumCoordinateID
            PrimaryAxisDisplacemnt = 0
            SecondaryAxisDisplacement = 1
            ThirdAxisDisplacement = 2
            SecondaryAxisRotation = 3
            ThirdAxisRotation = 4
        End Enum

#End Region

#Region " Attributes "

        'The coordinate ID that this is associated with
        Protected m_eCoordinateID As enumCoordinateID = enumCoordinateID.PrimaryAxisDisplacemnt

        'The stiffness of the constraint coordinate.
        Protected m_snStiffness As ScaledNumber

        'The damping of the constraint coordinate
        Protected m_snDamping As ScaledNumber

        'The loss for the constraint coordinate.
        Protected m_snLoss As ScaledNumber

#End Region

#Region " Properties "

        Public Overridable Property CoordinateID() As enumCoordinateID
            Get
                Return m_eCoordinateID
            End Get
            Set(ByVal Value As enumCoordinateID)
                SetSimData("CoordinateID", Convert.ToInt32(m_eCoordinateID).ToString, True)
                m_eCoordinateID = Value
            End Set
        End Property

        Public Overridable Property Stiffness() As ScaledNumber
            Get
                Return m_snStiffness
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The stiffness of the collision between materials can not be less than zero.")
                End If
                SetSimData("Stiffness", Value.ActualValue.ToString, True)
                m_snStiffness.CopyData(Value)
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

        Public Overridable Property Loss() As ScaledNumber
            Get
                Return m_snLoss
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The Loss of the collision between materials can not be less than zero.")
                End If
                SetSimData("Loss", Value.ActualValue.ToString, True)
                m_snLoss.CopyData(Value)
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            m_snStiffness = New AnimatGUI.Framework.ScaledNumber(Me, "Stiffness", 100, ScaledNumber.enumNumericScale.Kilo, "m/N", "m/N")
            m_snDamping = New AnimatGUI.Framework.ScaledNumber(Me, "Damping", 5, ScaledNumber.enumNumericScale.Kilo, "g/s", "g/s")
            m_snLoss = New AnimatGUI.Framework.ScaledNumber(Me, "Loss", 0, ScaledNumber.enumNumericScale.None)
            m_bEnabled = False

        End Sub

        Public Sub New(ByVal doParent As Framework.DataObject, ByVal strName As String, ByVal eCoordID As enumCoordinateID)
            MyBase.New(doParent)

            m_strName = strName
            m_eCoordinateID = eCoordID

            m_snStiffness = New AnimatGUI.Framework.ScaledNumber(Me, "Stiffness", 100, ScaledNumber.enumNumericScale.Kilo, "m/N", "m/N")
            m_snDamping = New AnimatGUI.Framework.ScaledNumber(Me, "Damping", 5, ScaledNumber.enumNumericScale.Kilo, "g/s", "g/s")
            m_snLoss = New AnimatGUI.Framework.ScaledNumber(Me, "Loss", 0, ScaledNumber.enumNumericScale.None)
            m_bEnabled = False

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            m_snStiffness.ClearIsDirty()
            m_snDamping.ClearIsDirty()
            m_snLoss.ClearIsDirty()

        End Sub

        Public Overrides Function Clone(ByVal doParent As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject) As Framework.DataObject
            Dim oNewNode As New ConstraintRelaxation(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doOrig As ConstraintRelaxation = DirectCast(doOriginal, ConstraintRelaxation)

            m_snStiffness = DirectCast(doOrig.m_snStiffness.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snDamping = DirectCast(doOrig.m_snDamping.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snLoss = DirectCast(doOrig.m_snLoss.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)

        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Name", m_strID.GetType(), "Name", _
                                        "Relaxation Properties", "The name of this material.", m_strName, True))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("ID", Me.ID.GetType(), "ID", _
                                        "Relaxation Properties", "ID", Me.ID, True))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Enabled", Me.Enabled.GetType(), "Enabled", _
                                        "Relaxation Properties", "Determines whether the relaxation parameter for this coordinate axis is turn on or not", Me.Enabled))

            pbNumberBag = m_snStiffness.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Stiffness", pbNumberBag.GetType(), "Stiffness", _
                            "Relaxation Properties", "The stiffness for this constraint coordinate axis.", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snDamping.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Damping", pbNumberBag.GetType(), "Damping", _
                            "Relaxation Properties", "The damping for this constraint coordinate axis.", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snLoss.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Loss", pbNumberBag.GetType(), "Loss", _
                            "Relaxation Properties", "The velocicty loss for this constraint coordinate axis.", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

        End Sub

#Region " Add-Remove to List Methods "

        Public Overrides Sub AddToSim(ByVal bThrowError As Boolean, Optional ByVal bDoNotInit As Boolean = False)
            If m_doInterface Is Nothing AndAlso Not Util.Simulation Is Nothing Then
                Util.Application.SimulationInterface.AddItem(Util.Simulation.ID, "ConstraintRelaxation", Me.ID, Me.GetSimulationXml("ConstraintRelaxation"), bThrowError, bDoNotInit)
                InitializeSimulationReferences()
            End If
        End Sub

        Public Overrides Sub RemoveFromSim(ByVal bThrowError As Boolean)
            If Not m_doInterface Is Nothing AndAlso Not Util.Simulation Is Nothing Then
                Util.Application.SimulationInterface.RemoveItem(Util.Simulation.ID, "ConstraintRelaxation", Me.ID, bThrowError)
            End If
            m_doInterface = Nothing
        End Sub

#End Region

        Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            oXml.IntoElem()

            m_strID = oXml.GetChildString("ID")
            m_strName = oXml.GetChildString("Name")
            m_bEnabled = oXml.GetChildBool("Enabled")

            m_snStiffness.LoadData(oXml, "Stiffness")
            m_snDamping.LoadData(oXml, "Damping")
            m_snLoss.LoadData(oXml, "Loss")

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            oXml.AddChildElement(Me.Name)
            oXml.IntoElem()

            oXml.AddChildElement("AssemblyFile", Me.AssemblyFile)
            oXml.AddChildElement("ClassName", Me.ClassName)

            oXml.AddChildElement("ID", Me.ID)
            oXml.AddChildElement("Name", Me.Name)
            oXml.AddChildElement("Enabled", Me.Enabled)

            oXml.AddChildElement("CoordinateID", Convert.ToInt32(m_eCoordinateID))
            m_snStiffness.SaveData(oXml, "Stiffness")
            m_snDamping.SaveData(oXml, "Damping")
            m_snLoss.SaveData(oXml, "Loss")

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            oXml.AddChildElement(Me.Name)
            oXml.IntoElem()

            oXml.AddChildElement("ID", Me.ID)
            oXml.AddChildElement("Name", Me.Name)
            oXml.AddChildElement("Type", "Default")
            oXml.AddChildElement("Enabled", Me.Enabled)

            oXml.AddChildElement("CoordinateID", Convert.ToInt32(m_eCoordinateID))
            m_snStiffness.SaveSimulationXml(oXml, Me, "Stiffness")
            m_snDamping.SaveSimulationXml(oXml, Me, "Damping")
            m_snLoss.SaveSimulationXml(oXml, Me, "Loss")

            oXml.OutOfElem()
        End Sub

#End Region

    End Class

#Region " ConstraintRelaxationPropBagConverter "

    Public Class ConstraintRelaxationPropBagConverter
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

                If Not pbValue Is Nothing AndAlso Not pbValue.Tag Is Nothing AndAlso TypeOf (pbValue.Tag) Is ConstraintRelaxation Then
                    Dim svValue As ConstraintRelaxation = DirectCast(pbValue.Tag, ConstraintRelaxation)

                    If svValue.Enabled Then
                        Return svValue.Stiffness.ToString
                    Else
                        Return ""
                    End If
                End If

                Return ""
            ElseIf destinationType Is GetType(String) AndAlso TypeOf (value) Is ConstraintRelaxation Then
                Dim svValue As ConstraintRelaxation = DirectCast(value, ConstraintRelaxation)

                If svValue.Enabled Then
                    Return svValue.Stiffness.ToString
                Else
                    Return ""
                End If
            End If

            Return MyBase.ConvertTo(context, culture, value, destinationType)
        End Function

    End Class

#End Region

End Namespace

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

    Public Class ConstraintRelaxationVortex
        Inherits ConstraintRelaxation

#Region " Attributes "

        'The loss for the constraint coordinate.
        Protected m_snLoss As ScaledNumber

#End Region

#Region " Properties "
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

            m_snStiffness = New AnimatGUI.Framework.ScaledNumber(Me, "Stiffness", 100, ScaledNumber.enumNumericScale.Kilo, "N/m", "N/m")
            m_snDamping = New AnimatGUI.Framework.ScaledNumber(Me, "Damping", 5, ScaledNumber.enumNumericScale.Kilo, "g/s", "g/s")
            m_snLoss = New AnimatGUI.Framework.ScaledNumber(Me, "Loss", 0, ScaledNumber.enumNumericScale.None)
            m_bEnabled = False

        End Sub

        Public Sub New(ByVal doParent As Framework.DataObject, ByVal strName As String, ByVal strDescription As String, ByVal eCoordID As enumCoordinateID, ByVal eCoordAxis As enumCoordinateAxis)
            MyBase.New(doParent)

            m_strName = strName
            m_strDescription = strDescription
            m_eCoordinateID = eCoordID
            m_eCoordinateAxis = eCoordAxis

            m_snStiffness = New AnimatGUI.Framework.ScaledNumber(Me, "Stiffness", 100, ScaledNumber.enumNumericScale.Kilo, "N/m", "N/m")
            m_snDamping = New AnimatGUI.Framework.ScaledNumber(Me, "Damping", 5, ScaledNumber.enumNumericScale.Kilo, "g/s", "g/s")
            m_snLoss = New AnimatGUI.Framework.ScaledNumber(Me, "Loss", 0, ScaledNumber.enumNumericScale.None)
            m_bEnabled = False

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            m_snLoss.ClearIsDirty()

        End Sub

        Public Overrides Function Clone(ByVal doParent As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject) As Framework.DataObject
            Dim oNewNode As New ConstraintRelaxationVortex(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doOrig As ConstraintRelaxationVortex = DirectCast(doOriginal, ConstraintRelaxationVortex)

            m_snLoss = DirectCast(doOrig.m_snLoss.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)

        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag

            pbNumberBag = m_snLoss.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Loss", pbNumberBag.GetType(), "Loss", _
                            "Relaxation Properties", "The velocicty loss for this constraint coordinate axis.", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

        End Sub

        Public Overloads Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml, ByVal strTagName As String)
            MyBase.LoadData(oXml, strTagName)

            If oXml.FindChildElement(strTagName, False) Then
                oXml.IntoElem()

                m_snLoss.LoadData(oXml, "Loss")

                oXml.OutOfElem()
            End If

        End Sub

        Public Overloads Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml, ByVal strTagName As String)
            MyBase.SaveData(oXml, strTagName)

            oXml.IntoElem()

            m_snLoss.SaveData(oXml, "Loss")

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

            oXml.IntoElem()

            m_snLoss.SaveSimulationXml(oXml, Me, "Loss")

            oXml.OutOfElem()
        End Sub

#End Region

    End Class

End Namespace

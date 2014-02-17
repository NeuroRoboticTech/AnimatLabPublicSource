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

    Public Class ConstraintRelaxationBullet
        Inherits ConstraintRelaxation

#Region " Attributes "

        'The maximum position value to which this part can move.
        Protected m_snMaxLimit As ScaledNumber

        'The minimum position value to which this part can move.
        Protected m_snMinLimit As ScaledNumber

        'The Equilibrum position for this relaxation. Should normally be 0
        Protected m_snEquilibriumPosition As ScaledNumber

#End Region

#Region " Properties "

        Public Overridable Property MaxLimit() As ScaledNumber
            Get
                Return m_snMaxLimit
            End Get
            Set(ByVal Value As ScaledNumber)
                SetSimData("MaxLimit", Value.ActualValue.ToString, True)
                m_snMaxLimit.CopyData(Value)
            End Set
        End Property

        Public Overridable Property MinLimit() As ScaledNumber
            Get
                Return m_snMinLimit
            End Get
            Set(ByVal Value As ScaledNumber)
                SetSimData("MinLimit", Value.ActualValue.ToString, True)
                m_snMinLimit.CopyData(Value)
            End Set
        End Property

        Public Overridable Property EquilibriumPosition() As ScaledNumber
            Get
                Return m_snEquilibriumPosition
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The equilibrium position must be 0 or greater.")
                End If

                SetSimData("EquilibriumPosition", Value.ActualValue.ToString, True)
                m_snEquilibriumPosition.CopyData(Value)
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            m_snDamping = New AnimatGUI.Framework.ScaledNumber(Me, "Damping", 0.1, ScaledNumber.enumNumericScale.None, "", "")
            m_snEquilibriumPosition = New AnimatGUI.Framework.ScaledNumber(Me, "EquilibriumPosition", 0.0, ScaledNumber.enumNumericScale.None, "Meters", "M")

            NewLimits(doParent)
        End Sub

        Public Sub New(ByVal doParent As Framework.DataObject, ByVal strName As String, ByVal strDescription As String, ByVal eCoordID As enumCoordinateID, ByVal eCoordAxis As enumCoordinateAxis)
            MyBase.New(doParent)

            m_strName = strName
            m_strDescription = strDescription
            m_eCoordinateID = eCoordID
            m_eCoordinateAxis = eCoordAxis

            m_snStiffness = New AnimatGUI.Framework.ScaledNumber(Me, "Stiffness", 100, ScaledNumber.enumNumericScale.Kilo, "N/m", "N/m")
            m_snDamping = New AnimatGUI.Framework.ScaledNumber(Me, "Damping", 0.1, ScaledNumber.enumNumericScale.None, "", "")
            m_snEquilibriumPosition = New AnimatGUI.Framework.ScaledNumber(Me, "EquilibriumPosition", 0.0, ScaledNumber.enumNumericScale.None, "Meters", "m")
            m_bEnabled = False
            NewLimits(doParent)

        End Sub

        Protected Sub NewLimits(ByVal doParent As Framework.DataObject)

            If m_eCoordinateID = CoordinateID.Relaxation1 OrElse m_eCoordinateID = CoordinateID.Relaxation2 OrElse m_eCoordinateID = CoordinateID.Relaxation3 Then
                m_snMinLimit = New AnimatGUI.Framework.ScaledNumber(Me, "MinLimit", -0.1, ScaledNumber.enumNumericScale.None, "Meters", "m")
                m_snMaxLimit = New AnimatGUI.Framework.ScaledNumber(Me, "MaxLimit", 0.1, ScaledNumber.enumNumericScale.None, "Meters", "m")
            Else
                m_snMinLimit = New AnimatGUI.Framework.ScaledNumber(Me, "MinLimit", -0.1, ScaledNumber.enumNumericScale.None, "Radians", "rad")
                m_snMaxLimit = New AnimatGUI.Framework.ScaledNumber(Me, "MaxLimit", 0.1, ScaledNumber.enumNumericScale.None, "Radians", "rad")
            End If
        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            m_snMinLimit.ClearIsDirty()
            m_snMaxLimit.ClearIsDirty()
            m_snEquilibriumPosition.ClearIsDirty()

        End Sub

        Public Overrides Function Clone(ByVal doParent As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject) As Framework.DataObject
            Dim oNewNode As New ConstraintRelaxationBullet(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doOrig As ConstraintRelaxationBullet = DirectCast(doOriginal, ConstraintRelaxationBullet)

            m_snMinLimit = DirectCast(doOrig.m_snMinLimit.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snMaxLimit = DirectCast(doOrig.m_snMaxLimit.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snEquilibriumPosition = DirectCast(doOrig.m_snEquilibriumPosition.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)

        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            If propTable.Properties.Contains("Damping") Then propTable.Properties.Remove("Damping")

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag

            pbNumberBag = m_snDamping.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Damping Constant", pbNumberBag.GetType(), "Damping", _
                            "Relaxation Properties", "The damping constant for this constraint coordinate axis. This is not a true damping value, " & _
                            "but just a scaling factor that helps damp down the motion.", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snMinLimit.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Min Limit", pbNumberBag.GetType(), "MinLimit", _
                            "Relaxation Properties", "The minimum distance that this relaxation can move.", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snMaxLimit.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Max Limit", pbNumberBag.GetType(), "MaxLimit", _
                            "Relaxation Properties", "The maximum distance that this relaxation can move.", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snEquilibriumPosition.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Equilibrium Position", pbNumberBag.GetType(), "EquilibriumPosition", _
                            "Relaxation Properties", "The equilibrium position for the spring that controls this relaxation. This should typically be 0.", pbNumberBag, _
                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

        End Sub

        Public Overloads Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml, ByVal strTagName As String)
            MyBase.LoadData(oXml, strTagName)

            If oXml.FindChildElement(strTagName, False) Then
                oXml.IntoElem()

                If oXml.FindElement("MinLimit", False) Then m_snMinLimit.LoadData(oXml, "MinLimit")
                If oXml.FindElement("MaxLimit", False) Then m_snMaxLimit.LoadData(oXml, "MaxLimit")
                If oXml.FindElement("EqPos", False) Then m_snEquilibriumPosition.LoadData(oXml, "EqPos")

                oXml.OutOfElem()
            End If

        End Sub

        Public Overloads Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml, ByVal strTagName As String)
            MyBase.SaveData(oXml, strTagName)

            oXml.IntoElem()

            m_snMinLimit.SaveData(oXml, "MinLimit")
            m_snMaxLimit.SaveData(oXml, "MaxLimit")
            m_snEquilibriumPosition.SaveData(oXml, "EqPos")

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

            oXml.IntoElem()

            m_snMinLimit.SaveSimulationXml(oXml, Me, "MinLimit")
            m_snMaxLimit.SaveSimulationXml(oXml, Me, "MaxLimit")
            m_snEquilibriumPosition.SaveSimulationXml(oXml, Me, "EqPos")

            oXml.OutOfElem()
        End Sub

#End Region

    End Class

End Namespace

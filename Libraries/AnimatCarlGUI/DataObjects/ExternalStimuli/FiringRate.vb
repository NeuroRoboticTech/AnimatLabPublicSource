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

    Public Class FiringRate
        Inherits AnimatGUI.DataObjects.ExternalStimuli.NodeStimulus

#Region " Enums "

        Public Enum enumCoverage
            WholePopulation
            Individuals
        End Enum

#End Region

#Region " Attributes "

        Protected m_snConstantRate As AnimatGUI.Framework.ScaledNumber

        Protected m_eCoverage As enumCoverage = enumCoverage.WholePopulation

        Protected m_aryCellsToStim As New Collections.NeuralIndices(Me)

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property ModuleName As String
            Get
                Return "AnimatCarlSimCUDA"
            End Get
        End Property

        Public Overrides ReadOnly Property StimulusClassType() As String
            Get
                Return "FiringRate"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property StimulusNoLongerValid() As Boolean
            Get

                Try
                    m_doNode = m_doOrganism.FindBehavioralNode(m_doNode.ID, False)
                Catch ex As System.Exception
                End Try

                If m_doNode Is Nothing Then
                    Return True
                Else
                    Return False
                End If
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property ConstantRate() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snConstantRate
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue < 0 OrElse Value.ActualValue > 500 Then
                    Throw New System.Exception("The constant firing rate must be between 0 and 500 Hz.")
                End If

                SetSimData("ConstantRate", Value.ActualValue.ToString, True)
                m_snConstantRate.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property Coverage() As enumCoverage
            Get
                Return m_eCoverage
            End Get
            Set(value As enumCoverage)
                SetSimData("Coverage", value.ToString(), True)
                m_eCoverage = value

                If Not Util.ProjectWorkspace Is Nothing Then
                    Util.ProjectWorkspace.RefreshProperties()
                End If
            End Set
        End Property

        Public Overridable Property CellsToStim As Collections.NeuralIndices
            Get
                Return m_aryCellsToStim
            End Get
            Set(value As Collections.NeuralIndices)
                'Set nothing here. It is set in the property editor

                'We do need to reset the list of indices in the simulator though
                Dim oXml As ManagedAnimatInterfaces.IStdXml = Util.Application.CreateStdXml()
                oXml.AddElement("Root")
                oXml.AddChildElement("Cells")
                oXml.IntoElem()
                For Each iIdx As Integer In m_aryCellsToStim
                    oXml.AddChildElement("Cell", iIdx)
                Next
                oXml.IntoElem()

                SetSimData("CellsToStim", oXml.Serialize(), True)
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            m_snConstantRate = New AnimatGUI.Framework.ScaledNumber(Me, "ConstantRate", 50, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Hz", "Hz")

            'Lets add the data types that this node understands.
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("ActiveRate", "Firing Rate", "Hertz", "Hz", 0, 100, ScaledNumber.enumNumericScale.None, ScaledNumber.enumNumericScale.None))
            m_thDataTypes.ID = "ActiveRate"

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNewNode As New FiringRate(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doRate As DataObjects.ExternalStimuli.FiringRate = DirectCast(doOriginal, DataObjects.ExternalStimuli.FiringRate)

            m_snConstantRate = DirectCast(doRate.m_snConstantRate.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_eCoverage = doRate.m_eCoverage
            m_aryCellsToStim = DirectCast(doRate.m_aryCellsToStim.Clone(Me, bCutData, doRoot), Collections.NeuralIndices)
        End Sub

        Public Overrides Function GetSimulationXml(ByVal strName As String, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing) As String

            If m_doOrganism Is Nothing Then
                Throw New System.Exception("No organism was defined for the stimulus '" & m_strName & "'.")
            End If

            If Not m_doNode Is Nothing Then
                m_doNode = m_doOrganism.FindBehavioralNode(m_doNode.ID, False)
            End If

            Dim oXml As ManagedAnimatInterfaces.IStdXml = Util.Application.CreateStdXml()

            'We need to get a new reference to the neuron here because it may be different than the one we originally got. The reason is that when the
            'project is first loaded we load in a list of the neurons. But if the user opens the behavioral editor then we need to reload that list because
            'we have to seperate out the neurons further by subsystem. So the second time they are loaded they would be a different object. Items like the 
            'ID should not be different, but changes to the node index would be different.
            If Not m_doNode Is Nothing Then
                oXml.AddElement("Root")
                SaveSimulationXml(oXml, nmParentControl, strName)
            End If

            Return oXml.Serialize()
        End Function

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            If m_doOrganism Is Nothing Then
                Throw New System.Exception("No organism was defined for the stimulus '" & m_strName & "'.")
            End If

            If Not m_doNode Is Nothing Then
                m_doNode = m_doOrganism.FindBehavioralNode(m_doNode.ID, False)
            End If

            'We need to get a new reference to the neuron here because it may be different than the one we originally got. The reason is that when the
            'project is first loaded we load in a list of the neurons. But if the user opens the behavioral editor then we need to reload that list because
            'we have to seperate out the neurons further by subsystem. So the second time they are loaded they would be a different object. Items like the 
            'ID should not be different, but changes to the node index would be different.
            If Not m_doNode Is Nothing Then

                oXml.AddChildElement("Stimulus")

                oXml.IntoElem()
                oXml.AddChildElement("ID", m_strID)
                oXml.AddChildElement("Name", m_strName)
                oXml.AddChildElement("AlwaysActive", m_bAlwaysActive)
                oXml.AddChildElement("Enabled", m_bEnabled)

                oXml.AddChildElement("ModuleName", Me.ModuleName)
                oXml.AddChildElement("Type", Me.StimulusClassType)

                oXml.AddChildElement("OrganismID", m_doOrganism.ID)
                oXml.AddChildElement("TargetNodeID", m_doNode.ID)

                oXml.AddChildElement("StartTime", m_snStartTime.ActualValue)
                oXml.AddChildElement("EndTime", m_snEndTime.ActualValue)

                If m_eValueType = enumValueType.Equation Then
                    'We need to convert the infix equation to postfix
                    Dim oMathEval As New MathStringEval
                    oMathEval.AddVariable("t")
                    oMathEval.Equation = m_strEquation
                    oMathEval.Parse()
                    oXml.AddChildElement("Equation", oMathEval.PostFix)
                End If

                oXml.AddChildElement("ConstantRate", m_snConstantRate.ActualValue)
                oXml.AddChildElement("Coverage", m_eCoverage.ToString())

                If m_eCoverage = enumCoverage.Individuals AndAlso m_aryCellsToStim.Count > 0 Then
                    oXml.AddChildElement("Cells")
                    oXml.IntoElem()  'Into Cells

                    For Each iCell As Integer In m_aryCellsToStim
                        oXml.AddChildElement("Cell", iCell)
                    Next

                    oXml.OutOfElem() 'Outof Cells
                End If

                oXml.OutOfElem()
            End If

        End Sub

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Value Type", m_eValueType.GetType(), "ValueType", _
                                        "Stimulus Properties", "Determines if a constant or an equation is used to determine the tonic current.", m_eValueType))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Coverage", m_eCoverage.GetType(), "Coverage", _
                                        "Stimulus Properties", "Determines whether we apply this stimulus to the entire population or individual cells.", m_eCoverage))

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag
            If m_eValueType = enumValueType.Equation Then
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Rate Equation", m_strEquation.GetType(), "Equation", _
                                            "Stimulus Properties", "An equation to determine the firing rate.", m_strEquation))
            Else
                pbNumberBag = m_snConstantRate.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Constant Rate", pbNumberBag.GetType(), "ConstantRate", _
                                            "Stimulus Properties", "A constant firing rate to apply for this stimulus.", pbNumberBag, _
                                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))
            End If

            If m_eCoverage = enumCoverage.Individuals Then
                pbNumberBag = m_aryCellsToStim.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Cells To Stimulate", pbNumberBag.GetType(), "CellsToStim", _
                                            "Stimulus Properties", "The list of cell indices to stimulate.", pbNumberBag, _
                                            GetType(TypeHelpers.SelectedIndexEditor), GetType(AnimatGuiCtrls.Controls.ExpandablePropBagConverter)))
            End If

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            m_snConstantRate.ClearIsDirty()
            m_aryCellsToStim.ClearIsDirty()
        End Sub


        Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.LoadData(oXml)

            oXml.IntoElem()

            m_snConstantRate.LoadData(oXml, "ConstantRate", False)
            m_eCoverage = DirectCast([Enum].Parse(GetType(enumCoverage), oXml.GetChildString("Coverage"), True), enumCoverage)

            m_aryCellsToStim.Clear()
            If oXml.FindChildElement("Cells", False) Then
                oXml.IntoElem()

                Dim iCount As Integer = oXml.NumberOfChildren() - 1
                For iIdx As Integer = 0 To iCount
                    oXml.FindChildByIndex(iIdx)
                    Dim iNeuronIdx As Integer = oXml.GetChildInt()
                    m_aryCellsToStim.Add(iNeuronIdx)
                Next

                oXml.OutOfElem()
            End If

            oXml.OutOfElem()

            Me.IsDirty = False
        End Sub

        Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            If Not m_doNode Is Nothing Then
                m_doNode = m_doOrganism.FindBehavioralNode(m_doNode.ID, False)
            End If

            If Not m_doNode Is Nothing Then
                MyBase.SaveData(oXml)

                oXml.IntoElem()

                m_snConstantRate.SaveData(oXml, "ConstantRate")
                oXml.AddChildElement("Coverage", m_eCoverage.ToString)

                If m_eCoverage = enumCoverage.Individuals AndAlso m_aryCellsToStim.Count > 0 Then
                    oXml.AddChildElement("Cells")
                    oXml.IntoElem()  'Into Cells

                    For Each iCell As Integer In m_aryCellsToStim
                        oXml.AddChildElement("Cell", iCell)
                    Next

                    oXml.OutOfElem() 'Outof Cells
                End If

                oXml.OutOfElem() ' Outof Node Element
            End If

        End Sub

#End Region

#End Region

    End Class

End Namespace

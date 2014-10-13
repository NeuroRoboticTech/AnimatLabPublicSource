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

Namespace DataObjects.Behavior.NodeTypes

    Public Class NodeToNodeAdapter
        Inherits AnimatGUI.DataObjects.Behavior.Nodes.NodeToNodeAdapter

#Region " Attributes "

        Protected m_eCoverage As enumCoverage = enumCoverage.WholePopulation

        Protected m_aryCellsToStim As New Collections.NeuralIndices(Me)

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property ModuleName As String
            Get
                Return "AnimatCarlSimCUDA"
            End Get
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

            Me.Name = "Node To Node Adapter"
            Me.Description = "Provides an interface adapter between two nodes."
        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNewNode As New NodeToNodeAdapter(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Protected Overrides Sub CloneInternal(doOriginal As AnimatGUI.Framework.DataObject, bCutData As Boolean, doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doRate As NodeToNodeAdapter = DirectCast(doOriginal, NodeToNodeAdapter)

            m_eCoverage = doRate.m_eCoverage
            m_aryCellsToStim = DirectCast(doRate.m_aryCellsToStim.Clone(Me, bCutData, doRoot), Collections.NeuralIndices)
        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            m_aryCellsToStim.ClearIsDirty()
        End Sub

        Protected Overrides Sub SaveSimulationXmlInternal(oXml As ManagedAnimatInterfaces.IStdXml, nmSource As AnimatGUI.DataObjects.Behavior.NeuralModule, _
                                                          nmTarget As AnimatGUI.DataObjects.Behavior.NeuralModule, bnOrigin As AnimatGUI.DataObjects.Behavior.Node, _
                                                          bnDestination As AnimatGUI.DataObjects.Behavior.Node, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, _
                                                          Optional strName As String = "")
            MyBase.SaveSimulationXmlInternal(oXml, nmSource, nmTarget, bnOrigin, bnDestination, nmParentControl, strName)

            oXml.IntoElem()

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
        End Sub

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Coverage", m_eCoverage.GetType(), "Coverage", _
                                        "Adapter Properties", "Determines whether we apply this stimulus to the entire population or individual cells.", m_eCoverage))

            If m_eCoverage = enumCoverage.Individuals Then
                Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = m_aryCellsToStim.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Cells To Stimulate", pbNumberBag.GetType(), "CellsToStim", _
                                            "Adapter Properties", "The list of cell indices to stimulate.", pbNumberBag, _
                                            GetType(TypeHelpers.SelectedIndexEditor), GetType(AnimatGuiCtrls.Controls.ExpandablePropBagConverter)))
            End If

        End Sub

        Public Overrides Sub SaveData(oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.SaveData(oXml)

            oXml.IntoElem()

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

        End Sub

        Public Overrides Sub LoadData(oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.LoadData(oXml)

            oXml.IntoElem()

            m_eCoverage = DirectCast([Enum].Parse(GetType(enumCoverage), oXml.GetChildString("Coverage", "WholePopulation"), True), enumCoverage)

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

#End Region

#End Region

    End Class

End Namespace

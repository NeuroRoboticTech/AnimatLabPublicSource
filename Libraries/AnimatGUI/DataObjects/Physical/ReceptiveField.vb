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

Namespace DataObjects.Physical

    Public Class ReceptiveField
        Inherits Framework.DataObject

#Region " Attributes "

        Protected m_vVertex As Vec3d
        Protected m_aryPairs As New Collections.SortedReceptiveFieldPairs(Me)

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Overridable Property Vertex() As Vec3d
            Get
                Return m_vVertex
            End Get
            Set(ByVal Value As Vec3d)
                m_vVertex = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property FieldPairs() As Collections.SortedReceptiveFieldPairs
            Get
                Return m_aryPairs
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Public Sub New(ByVal doParent As Framework.DataObject, ByVal vVertex As Vec3d)
            MyBase.New(doParent)
            m_vVertex = vVertex
        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim doNewRF As New ReceptiveField(doParent, m_vVertex)
            Return doNewRF
        End Function

#Region " Add-Remove to List Methods "

        Public Overrides Sub AddToSim(ByVal bThrowError As Boolean, Optional ByVal bDoNotInit As Boolean = False)
            If Not m_doParent Is Nothing Then
                Util.Application.SimulationInterface.AddItem(m_doParent.ID, "ReceptiveField", Me.ID, Me.GetSimulationXml("ReceptiveField"), bThrowError, bDoNotInit)
                InitializeSimulationReferences()
            End If
        End Sub

        Public Overrides Sub RemoveFromSim(ByVal bThrowError As Boolean)
            If Not m_doInterface Is Nothing AndAlso Not m_doParent Is Nothing Then
                Util.Application.SimulationInterface.RemoveItem(m_doParent.ID, "ReceptiveField", Me.ID, bThrowError)
            End If
            m_doInterface = Nothing
        End Sub

#End Region

        Public Overridable Overloads Sub LoadData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            m_aryPairs.Clear()

            oXml.IntoElem()
            m_vVertex = Util.LoadVec3d(oXml, "Vertex", Nothing)
            oXml.OutOfElem()
        End Sub

        Public Overridable Overloads Sub SaveData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            oXml.AddChildElement("ReceptiveField")
            oXml.IntoElem()

            oXml.AddChildElement("ID", m_strID)
            Util.SaveVector(oXml, "Vertex", m_vVertex)

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            oXml.AddChildElement("ReceptiveField")
            oXml.IntoElem()

            oXml.AddChildElement("ID", m_strID)
            Util.SaveVector(oXml, "Vertex", m_vVertex)

            oXml.OutOfElem()

        End Sub

#End Region

    End Class

End Namespace




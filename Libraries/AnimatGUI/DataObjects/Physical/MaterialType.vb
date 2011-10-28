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

    Public Class MaterialType
        Inherits Framework.DataObject

#Region " Attributes "

#End Region

#Region " Properties "

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Name", m_strID.GetType(), "Name", _
                                        "Material Properties", "The name of this material.", m_strName, False))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("ID", Me.ID.GetType(), "ID", _
                                        "Material Properties", "ID", Me.ID, True))

        End Sub

        Public Overrides Function Clone(ByVal doParent As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject) As Framework.DataObject
            Dim doItem As New MaterialType(doParent)
            doItem.CloneInternal(Me, bCutData, doRoot)
            If Not doItem Is Nothing AndAlso doItem Is Me Then doItem.AfterClone(Me, bCutData, doRoot, doItem)
            Return doItem
        End Function

#Region " Add-Remove to List Methods "

        Public Overrides Sub AddToSim(ByVal bThrowError As Boolean)
            If m_doInterface Is Nothing AndAlso Not Util.Simulation Is Nothing Then
                Util.Application.SimulationInterface.AddItem(Util.Simulation.ID, "MaterialType", Me.ID, Me.GetSimulationXml("MaterialType"), bThrowError)
                InitializeSimulationReferences()
            End If
        End Sub

        Public Overrides Sub RemoveFromSim(ByVal bThrowError As Boolean)
            If Not m_doInterface Is Nothing AndAlso Not Util.Simulation Is Nothing Then
                Util.Application.SimulationInterface.RemoveItem(Util.Simulation.ID, "MaterialType", Me.ID, bThrowError)
            End If
            m_doInterface = Nothing
        End Sub

#End Region

        Public Overrides Sub LoadData(ByRef oXml As AnimatGUI.Interfaces.StdXml)

            oXml.IntoElem()

            m_strID = oXml.GetChildString("ID")
            m_strName = oXml.GetChildString("Name")

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveData(ByRef oXml As Interfaces.StdXml)

            oXml.AddChildElement("MaterialType")
            oXml.IntoElem()

            oXml.AddChildElement("AssemblyFile", Me.AssemblyFile)
            oXml.AddChildElement("ClassName", Me.ClassName)

            oXml.AddChildElement("ID", Me.ID)
            oXml.AddChildElement("Name", Me.Name)

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveSimulationXml(ByRef oXml As Interfaces.StdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            oXml.AddChildElement("MaterialType")
            oXml.IntoElem()

            oXml.AddChildElement("ID", Me.ID)
            oXml.AddChildElement("Name", Me.Name)

            oXml.OutOfElem()
        End Sub

#End Region

#Region " Events "

        'This is the new event that rigid bodies will subscribe to when they set the material type for themselves.
        'When this is fired they will replace this material type with the new one that is passed in. Within the 
        ' material editor window they will be able to delete a material type. When they do we will first open a new
        ' dialog to allow them to pick the new material. If they hit ok then we will then call ReplaceMaterial to signla
        ' this event to all subscribing objects.
        Public Event ReplaceMaterial(ByVal doReplacement As MaterialType)

        'This method is called after the users have picked the new material to switch to using.
        Public Sub RemovingType(ByVal doReplacement As MaterialType)
            RaiseEvent ReplaceMaterial(doReplacement)
        End Sub

#End Region

    End Class

End Namespace

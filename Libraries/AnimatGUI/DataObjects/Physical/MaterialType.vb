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

    Public MustInherit Class MaterialType
        Inherits Framework.DataObject

#Region " Attributes "

        Protected Shared m_aryMaterialTypesPerPhysics As New SortedList()

#End Region

#Region " Properties "

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

        End Sub

        Public Shared Sub RegisterMaterialType(ByVal strPhysicsEngine As String, ByVal oMaterialType As MaterialType)
            m_aryMaterialTypesPerPhysics.Add(strPhysicsEngine, oMaterialType)
        End Sub

        Public Shared Sub UnRegisterMaterialType(ByVal strPhysicsEngine As String)
            If m_aryMaterialTypesPerPhysics.ContainsKey(strPhysicsEngine) Then
                m_aryMaterialTypesPerPhysics.Remove(strPhysicsEngine)
            End If
        End Sub

        Public Shared Sub ClearRegisteredMaterialTypes()
            m_aryMaterialTypesPerPhysics.Clear()
        End Sub

        Public Shared Function CreateMaterialType(ByRef doParent As AnimatGUI.Framework.DataObject, ByVal bCut As Boolean, ByRef doRoot As AnimatGUI.Framework.DataObject) As MaterialType
            If m_aryMaterialTypesPerPhysics.ContainsKey(Util.Application.Physics.Name) Then
                Dim oMatType As MaterialType = DirectCast(m_aryMaterialTypesPerPhysics.Item(Util.Application.Physics.Name), MaterialType)
                Return DirectCast(oMatType.Clone(doParent, bCut, doRoot), MaterialType)
            Else
                Throw New System.Exception("No material type for the physics engine '" & Util.Application.Physics.Name & "' has been registered.")
            End If
        End Function

        Public MustOverride Sub RegisterMaterialType()

#Region " Add-Remove to List Methods "

        Public Overrides Sub AddToSim(ByVal bThrowError As Boolean, Optional ByVal bDoNotInit As Boolean = False)
            If m_doInterface Is Nothing AndAlso Not Util.Simulation Is Nothing Then
                Util.Application.SimulationInterface.AddItem(Util.Simulation.ID, "MaterialType", Me.ID, Me.GetSimulationXml("MaterialType"), bThrowError, bDoNotInit)
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

        Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            oXml.IntoElem()

            m_strID = oXml.GetChildString("ID")
            m_strName = oXml.GetChildString("Name")

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            oXml.AddChildElement("MaterialType")
            oXml.IntoElem()

            oXml.AddChildElement("AssemblyFile", Me.AssemblyFile)
            oXml.AddChildElement("ClassName", Me.ClassName)

            oXml.AddChildElement("ID", Me.ID)
            oXml.AddChildElement("Name", Me.Name)

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            oXml.AddChildElement("MaterialType")
            oXml.IntoElem()

            oXml.AddChildElement("ID", Me.ID)
            oXml.AddChildElement("Name", Me.Name)
            oXml.AddChildElement("Type", "Default")

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

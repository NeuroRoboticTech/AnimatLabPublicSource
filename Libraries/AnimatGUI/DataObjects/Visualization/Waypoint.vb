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

Namespace DataObjects.Visualization


    Public Class Waypoint
        Inherits AnimatGUI.Framework.DataObject

#Region " Attributes "

        Protected m_svPosition As Framework.ScaledVector3
        Protected m_snTime As Framework.ScaledNumber

#End Region

#Region " Properties "

        Public Overridable ReadOnly Property Type() As String
            Get
                Return "Waypoint"
            End Get
        End Property

        Public Overridable Property Position() As Framework.ScaledVector3
            Get
                Return m_svPosition
            End Get
            Set(ByVal value As Framework.ScaledVector3)
                Me.SetSimData("Position", value.GetSimulationXml("Position"), True)
                m_svPosition.CopyData(value)
            End Set
        End Property

        Public Overridable Property Time() As Framework.ScaledNumber
            Get
                Return m_snTime
            End Get
            Set(ByVal value As Framework.ScaledNumber)
                Me.SetSimData("Time", value.GetSimulationXml("Time"), True)
                m_snTime.CopyData(value)
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            m_svPosition = New ScaledVector3(Me, "Position", "Location of the waypoint in world coordinates.", "Meters", "m")
            m_snTime = New ScaledNumber(Me, "Time", 0, ScaledNumber.enumNumericScale.None, "s", "s")

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()
            If Not m_svPosition Is Nothing Then m_svPosition.ClearIsDirty()
            If Not m_snTime Is Nothing Then m_snTime.ClearIsDirty()
        End Sub

        Protected Overrides Sub CloneInternal(ByVal doOriginal As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim bpOrig As Waypoint = DirectCast(doOriginal, Waypoint)

            m_svPosition = DirectCast(bpOrig.m_svPosition.Clone(Me, bCutData, doRoot), Framework.ScaledVector3)
            m_snTime = DirectCast(bpOrig.m_snTime.Clone(Me, bCutData, doRoot), Framework.ScaledNumber)

        End Sub

        Public Overrides Function Clone(ByVal doParent As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject) As Framework.DataObject
            Dim oNewNode As New Waypoint(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Name", Me.Name.GetType(), "Name", _
                                        "Point Properties", "Name", Me.Name))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("ID", Me.ID.GetType(), "ID", _
                                        "Point Properties", "ID", Me.ID, True))

 
            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = Me.Position.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Position", pbNumberBag.GetType(), "Position", _
                                        "Point Properties", "Sets the location of this waypoint.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledVector3.ScaledVector3PropBagConverter)))

            pbNumberBag = m_snTime.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Time", pbNumberBag.GetType(), "Time", _
                                        "Point Properties", "Sets the time for this waypoint.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

        End Sub

#Region " Add-Remove to List Methods "

        Public Overrides Sub AddToSim(ByVal bThrowError As Boolean, Optional ByVal bDoNotInit As Boolean = False)
            'If Not Util.Simulation Is Nothing Then
            '    Util.Application.SimulationInterface.AddItem(m_doParent.ID, "CameraPath", Me.ID, Me.GetSimulationXml("CameraPath"), bThrowError, bDoNotInit)
            '    InitializeSimulationReferences()
            'End If
        End Sub

        Public Overrides Sub RemoveFromSim(ByVal bThrowError As Boolean)
            'If Not Util.Simulation Is Nothing AndAlso Not m_doInterface Is Nothing Then
            '    Util.Application.SimulationInterface.RemoveItem(m_doParent.ID, "CameraPath", Me.ID, bThrowError)
            'End If
            'm_doInterface = Nothing
        End Sub

#End Region

        Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.LoadData(oXml)

            oXml.IntoElem()

            Me.Name = oXml.GetChildString("Name")
            Me.ID = oXml.GetChildString("ID")

            m_svPosition.LoadData(oXml, "Position")
            m_snTime.LoadData(oXml, "Time")

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            oXml.AddChildElement("CameraPath")
            oXml.IntoElem()

            oXml.AddChildElement("AssemblyFile", Me.AssemblyFile)
            oXml.AddChildElement("ClassName", Me.ClassName)
            oXml.AddChildElement("ID", Me.ID)

            m_svPosition.SaveData(oXml, "Position")
            m_snTime.SaveData(oXml, "Time")

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            oXml.AddChildElement("CameraPath")
            oXml.IntoElem()

            oXml.AddChildElement("ModuleName", Me.ModuleName)
            oXml.AddChildElement("Type", Me.Type)
            oXml.AddChildElement("Name", m_strName)
            oXml.AddChildElement("ID", m_strID)
            m_svPosition.SaveSimulationXml(oXml, Me, "Position")
            m_snTime.SaveSimulationXml(oXml, Me, "Time")

            oXml.OutOfElem()

        End Sub

#End Region

    End Class

End Namespace


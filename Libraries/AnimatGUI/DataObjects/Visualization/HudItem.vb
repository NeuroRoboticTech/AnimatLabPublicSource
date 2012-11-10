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

    Public Class HudItem
        Inherits AnimatGUI.Framework.DataObject

#Region " Attributes "

        Protected m_strHudType As String = "HudText"
        Protected m_clColor As System.Drawing.Color = Color.White
        Protected m_ptPosition As System.Drawing.Point = New System.Drawing.Point(10, 10)
        Protected m_iCharSize As Integer = 30
        Protected m_strText As String = "Time: %3.3f"
        Protected m_strDisplayTargetID As String = "Simulator"
        Protected m_strDisplayDataType As String = "Time"
        Protected m_strUpdateTargetID As String = "Simulator"
        Protected m_strUpdateDataType As String = "RealTime"
        Protected m_fltUpdateInterval As Single = 0.25

#End Region

#Region " Properties "

        Public Overridable ReadOnly Property Type() As String
            Get
                Return "HudText"
            End Get
        End Property


#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject, ByVal strHudType As String, ByVal clColor As System.Drawing.Color, _
                       ByVal ptPosition As System.Drawing.Point, ByVal iCharSize As Integer, ByVal strText As String, _
                       ByVal strDisplayTargetID As String, ByVal strDisplayDataType As String, _
                       ByVal strUpdateTargetID As String, ByVal strUpdateDataType As String, ByVal fltUpdateInterval As Single)
            MyBase.New(doParent)

            m_strName = m_strID

            m_strHudType = strHudType
            m_clColor = clColor
            m_ptPosition = ptPosition
            m_iCharSize = iCharSize
            m_strText = strText
            m_strDisplayTargetID = strDisplayTargetID
            m_strDisplayDataType = strDisplayDataType
            m_strUpdateTargetID = strUpdateTargetID
            m_strUpdateDataType = strUpdateDataType
            m_fltUpdateInterval = fltUpdateInterval
        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

        End Sub

        Public Overrides Function Clone(ByVal doParent As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject) As Framework.DataObject

        End Function


#Region " Add-Remove to List Methods "

        Public Overrides Sub AddToSim(ByVal bThrowError As Boolean, Optional ByVal bDoNotInit As Boolean = False)
            If Not Util.Simulation Is Nothing Then
                Util.Application.SimulationInterface.AddItem(Util.Simulation.ID, "HudItem", Me.ID, Me.GetSimulationXml("HudItem"), bThrowError, bDoNotInit)
                InitializeSimulationReferences()
            End If
        End Sub

        Public Overrides Sub RemoveFromSim(ByVal bThrowError As Boolean)
            If Not Util.Simulation Is Nothing AndAlso Not m_doInterface Is Nothing Then
                Util.Application.SimulationInterface.RemoveItem(Util.Simulation.ID, "HudItem", Me.ID, bThrowError)
            End If
            m_doInterface = Nothing
        End Sub

#End Region

        Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.LoadData(oXml)

            oXml.IntoElem()

            m_strHudType = oXml.GetChildString("HudType", m_strHudType)
            m_iCharSize = oXml.GetChildInt("CharSize", m_iCharSize)
            m_strText = oXml.GetChildString("Text", m_strText)
            m_strDisplayTargetID = oXml.GetChildString("DisplayTargetID", m_strDisplayTargetID)
            m_strDisplayDataType = oXml.GetChildString("DisplayDataType", m_strDisplayDataType)
            m_strUpdateTargetID = oXml.GetChildString("UpdateTargetID", m_strUpdateTargetID)
            m_strUpdateDataType = oXml.GetChildString("UpdateDataType", m_strUpdateDataType)
            m_fltUpdateInterval = oXml.GetChildFloat("UpdateInterval", m_fltUpdateInterval)
            m_ptPosition = Util.LoadPoint(oXml, "Position")
            m_clColor = Util.LoadColor(oXml, "Color", m_clColor)

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            oXml.AddChildElement("HudItem")
            oXml.IntoElem()

            oXml.AddChildElement("Name", m_strName)
            oXml.AddChildElement("ID", m_strID)
            oXml.AddChildElement("Type", m_strHudType)
            oXml.AddChildElement("CharSize", m_iCharSize)
            oXml.AddChildElement("Text", m_strText)
            oXml.AddChildElement("DisplayTargetID", m_strDisplayTargetID)
            oXml.AddChildElement("DisplayDataType", m_strDisplayDataType)
            oXml.AddChildElement("UpdateTargetID", m_strUpdateTargetID)
            oXml.AddChildElement("UpdateDataType", m_strUpdateDataType)
            oXml.AddChildElement("UpdateInterval", m_fltUpdateInterval)
            Util.SavePoint(oXml, "Position", m_ptPosition)
            Util.SaveColor(oXml, "Color", m_clColor)

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            oXml.AddChildElement("HudItem")
            oXml.IntoElem()

            oXml.AddChildElement("ModuleName", Me.ModuleName)
            oXml.AddChildElement("Type", Me.Type)
            oXml.AddChildElement("Name", m_strName)
            oXml.AddChildElement("ID", m_strID)
            oXml.AddChildElement("Type", m_strHudType)
            oXml.AddChildElement("CharSize", m_iCharSize)
            oXml.AddChildElement("Text", m_strText)
            oXml.AddChildElement("DisplayTargetID", m_strDisplayTargetID)
            oXml.AddChildElement("DisplayDataType", m_strDisplayDataType)
            oXml.AddChildElement("UpdateTargetID", m_strUpdateTargetID)
            oXml.AddChildElement("UpdateDataType", m_strUpdateDataType)
            oXml.AddChildElement("UpdateInterval", m_fltUpdateInterval)
            Util.SavePoint(oXml, "Position", m_ptPosition)
            Util.SaveColor(oXml, "Color", m_clColor)

            oXml.OutOfElem()

        End Sub

#End Region

    End Class

End Namespace


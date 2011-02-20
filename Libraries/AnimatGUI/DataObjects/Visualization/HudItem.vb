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
        Protected m_strTargetID As String = "Simulator"
        Protected m_strDataType As String = "Time"

#End Region

#Region " Properties "

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
            m_strName = m_strID
        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

        End Sub

        Public Overrides Function Clone(ByVal doParent As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject) As Framework.DataObject

        End Function

        Public Overrides Sub LoadData(ByRef oXml As AnimatGUI.Interfaces.StdXml)
            MyBase.LoadData(oXml)

            oXml.IntoElem()

            m_strHudType = oXml.GetChildString("HudType", m_strHudType)
            m_iCharSize = oXml.GetChildInt("CharSize", m_iCharSize)
            m_strText = oXml.GetChildString("Text", m_strText)
            m_strTargetID = oXml.GetChildString("TargetID", m_strTargetID)
            m_strDataType = oXml.GetChildString("DataType", m_strDataType)
            m_ptPosition = Util.LoadPoint(oXml, "Position")
            m_clColor = Util.LoadColor(oXml, "Color", m_clColor)

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveData(ByRef oXml As AnimatGUI.Interfaces.StdXml)

            oXml.AddChildElement("HudItem")
            oXml.IntoElem()

            oXml.AddChildElement("Name", m_strName)
            oXml.AddChildElement("ID", m_strID)
            oXml.AddChildElement("Type", m_strHudType)
            oXml.AddChildElement("CharSize", m_iCharSize)
            oXml.AddChildElement("Text", m_strText)
            oXml.AddChildElement("TargetID", m_strTargetID)
            oXml.AddChildElement("DataType", m_strDataType)
            Util.SavePoint(oXml, "Position", m_ptPosition)
            Util.SaveColor(oXml, "Color", m_clColor)

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveSimulationXml(ByRef oXml As AnimatGUI.Interfaces.StdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            oXml.AddChildElement("HudItem")
            oXml.IntoElem()

            oXml.AddChildElement("Name", m_strName)
            oXml.AddChildElement("ID", m_strID)
            oXml.AddChildElement("Type", m_strHudType)
            oXml.AddChildElement("CharSize", m_iCharSize)
            oXml.AddChildElement("Text", m_strText)
            oXml.AddChildElement("TargetID", m_strTargetID)
            oXml.AddChildElement("DataType", m_strDataType)
            Util.SavePoint(oXml, "Position", m_ptPosition)
            Util.SaveColor(oXml, "Color", m_clColor)

            oXml.OutOfElem()

        End Sub

#End Region

    End Class

End Namespace


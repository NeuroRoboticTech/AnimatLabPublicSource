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

    Public Class OdorType
        Inherits Framework.DataObject

#Region " Attributes "

        Protected m_snDiffusionConstant As ScaledNumber
        Protected m_aryOdorSources As New Collections.SortedOdors(Me)

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Overridable Property DiffusionConstant() As ScaledNumber
            Get
                Return m_snDiffusionConstant
            End Get
            Set(ByVal Value As ScaledNumber)
                If Not Value Is Nothing Then
                    If Value.ActualValue < 0 Then
                        Throw New System.Exception("The diffusion constant must be greater than or equal to zero.")
                    End If

                    SetSimData("DiffusionConstant", Value.ActualValue.ToString, True)
                    m_snDiffusionConstant.CopyData(Value)
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property OdorSources() As Collections.SortedOdors
            Get
                Return m_aryOdorSources
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
            m_strID = System.Guid.NewGuid().ToString()

            m_snDiffusionConstant = New ScaledNumber(Me, "DiffusionConstant", 1, ScaledNumber.enumNumericScale.None, "m^2/s", "m^2/s")
        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Name", m_strID.GetType(), "Name", _
                                        "Odor Type Properties", "The name of this odor Type.", m_strName))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("ID", Me.ID.GetType(), "ID", _
                                        "Odor Type Properties", "ID", Me.ID, True))

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = m_snDiffusionConstant.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Diffusion Constant", pbNumberBag.GetType(), "DiffusionConstant", _
                                        "Odor Type Properties", "Sets the rate of diffusion of this odor.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()
            If Not m_snDiffusionConstant Is Nothing Then m_snDiffusionConstant.ClearIsDirty()
        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim doItem As New OdorType(doParent)
            doItem.m_snDiffusionConstant = DirectCast(m_snDiffusionConstant.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_aryOdorSources = DirectCast(m_aryOdorSources.Copy(), Collections.SortedOdors)
            Return doItem
        End Function

#Region " Add-Remove to List Methods "

        Public Overrides Sub AddToSim(ByVal bThrowError As Boolean)
            If Not Util.Simulation Is Nothing Then
                Util.Application.SimulationInterface.AddItem(Util.Simulation.ID, "OdorType", Me.ID, Me.GetSimulationXml("OdorType"), bThrowError)
                InitializeSimulationReferences()
            End If
        End Sub

        Public Overrides Sub RemoveFromSim(ByVal bThrowError As Boolean)
            If Not m_doInterface Is Nothing AndAlso Not Util.Simulation Is Nothing Then
                Util.Application.SimulationInterface.RemoveItem(Util.Simulation.ID, "OdorType", Me.ID, bThrowError)
            End If
            m_doInterface = Nothing
        End Sub

#End Region

        Public Overridable Overloads Sub LoadData(ByRef oXml As Interfaces.StdXml)

            oXml.IntoElem() 'Into OdorType Element

            m_strName = oXml.GetChildString("Name")
            m_strID = oXml.GetChildString("ID")
            m_snDiffusionConstant.LoadData(oXml, "DiffusionConstant")

            oXml.OutOfElem() 'Outof OdorType Element

        End Sub

        Public Overridable Overloads Sub SaveData(ByRef oXml As Interfaces.StdXml)

            oXml.AddChildElement("OdorType")
            oXml.IntoElem()

            oXml.AddChildElement("Name", m_strName)
            oXml.AddChildElement("ID", m_strID)
            m_snDiffusionConstant.SaveData(oXml, "DiffusionConstant")

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveSimulationXml(ByRef oXml As Interfaces.StdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            oXml.AddChildElement("OdorType")
            oXml.IntoElem()

            oXml.AddChildElement("Name", m_strName)
            oXml.AddChildElement("ID", m_strID)
            m_snDiffusionConstant.SaveSimulationXml(oXml, Me, "DiffusionConstant")

            oXml.OutOfElem()
        End Sub

#End Region

    End Class

End Namespace

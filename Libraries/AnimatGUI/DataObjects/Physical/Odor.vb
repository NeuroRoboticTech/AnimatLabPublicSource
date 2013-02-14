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

    Public Class Odor
        Inherits Framework.DataObject

#Region " Attributes "

        Protected m_doOdorType As OdorType
        Protected m_fltQuantity As Single = 100
        Protected m_bUseFoodQuantity As Boolean = False

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Overrides Property Name() As String
            Get
                If Not m_doOdorType Is Nothing Then
                    Return m_doOdorType.Name
                Else
                    Return ""
                End If
            End Get
            Set(ByVal Value As String)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property OdorType() As OdorType
            Get
                Return m_doOdorType
            End Get
            Set(ByVal Value As OdorType)
                If Not Value Is Nothing Then
                    m_doOdorType = Value
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property DiffusionConstant() As ScaledNumber
            Get
                If Not m_doOdorType Is Nothing Then
                    Return m_doOdorType.DiffusionConstant
                End If
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property Quantity() As Single
            Get
                If m_bUseFoodQuantity Then
                    If Not Me.Parent Is Nothing AndAlso TypeOf Me.Parent Is AnimatGUI.DataObjects.Physical.RigidBody Then
                        Dim doPart As AnimatGUI.DataObjects.Physical.RigidBody = DirectCast(Me.Parent, AnimatGUI.DataObjects.Physical.RigidBody)
                        Return CType(doPart.FoodQuantity.ActualValue, Single)
                    Else
                        Return m_fltQuantity
                    End If
                Else
                    Return m_fltQuantity
                End If
            End Get
            Set(ByVal Value As Single)
                If Value < 0 Then
                    Throw New System.Exception("Quantity must be greater than or eqaul to zero.")
                End If

                SetSimData("Quantity", Value.ToString, True)
                m_fltQuantity = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property UseFoodQuantity() As Boolean
            Get
                Return m_bUseFoodQuantity
            End Get
            Set(ByVal Value As Boolean)
                SetSimData("UseFoodQuantity", Value.ToString, True)
                m_bUseFoodQuantity = Value
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Name", m_strID.GetType(), "Name", _
                                        "Odor Properties", "The name of this odor.", m_strName, True))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("ID", Me.ID.GetType(), "ID", _
                                        "Odor Properties", "ID", Me.ID, True))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Enabled", Me.Enabled.GetType(), "Enabled", _
                                         "Odor Properties", "Enabled", Me.Enabled))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Use Food Quantity", m_bUseFoodQuantity.GetType(), "UseFoodQuantity", _
                                        "Odor Properties", "If this is true then the odor quantity is determined by the food quantity value of this part.", m_bUseFoodQuantity))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Quantity", m_fltQuantity.GetType(), "Quantity", _
                                        "Odor Properties", "The quantity of substance producing this odor.", m_fltQuantity, m_bUseFoodQuantity))

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = Me.DiffusionConstant.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Diffusion Constant", pbNumberBag.GetType(), "DiffusionConstant", _
                                        "Odor Type Properties", "Sets the rate of diffusion of this odor.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter), True))

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim doItem As New Odor(doParent)

            doItem.m_doOdorType = m_doOdorType
            doItem.m_fltQuantity = m_fltQuantity
            doItem.m_bUseFoodQuantity = m_bUseFoodQuantity

            Return doItem
        End Function

#Region " Add-Remove to List Methods "

        Public Overrides Sub AddToSim(ByVal bThrowError As Boolean, Optional ByVal bDoNotInit As Boolean = False)
            If Not m_doParent Is Nothing Then
                Util.Application.SimulationInterface.AddItem(m_doParent.ID, "Odor", Me.ID, Me.GetSimulationXml("Odor"), bThrowError, bDoNotInit)
                InitializeSimulationReferences()
            End If
        End Sub

        Public Overrides Sub RemoveFromSim(ByVal bThrowError As Boolean)
            If Not m_doInterface Is Nothing AndAlso Not m_doParent Is Nothing Then
                Util.Application.SimulationInterface.RemoveItem(m_doParent.ID, "Odor", Me.ID, bThrowError)
            End If
            m_doInterface = Nothing
        End Sub

#End Region

        Public Overridable Overloads Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            Try
                oXml.FindChildElement("Odor")
                oXml.IntoElem() 'Into OdorType Element

                m_strName = oXml.GetChildString("Name")
                m_strID = oXml.GetChildString("ID")
                m_bEnabled = oXml.GetChildBool("Enabled", m_bEnabled)
                Dim strOdorTypeID As String = oXml.GetChildString("OdorTypeID")
                m_fltQuantity = oXml.GetChildFloat("Quantity")
                m_bUseFoodQuantity = oXml.GetChildBool("UseFoodQuantity", False)

                oXml.OutOfElem() 'Outof OdorType Element

                If Util.Environment.OdorTypes.Contains(strOdorTypeID) Then
                    m_doOdorType = Util.Environment.OdorTypes(strOdorTypeID)

                    If Not m_doOdorType.OdorSources.Contains(Me.ID) Then
                        m_doOdorType.OdorSources.Add(Me.ID, Me, False)
                    End If
                Else
                    Throw New System.Exception("No odor type with ID '" & strOdorTypeID & "' was found.")
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Public Overridable Overloads Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            If Not m_doOdorType Is Nothing Then
                oXml.AddChildElement("Odor")
                oXml.IntoElem()

                oXml.AddChildElement("Name", m_doOdorType.Name)
                oXml.AddChildElement("ID", m_strID)
                oXml.AddChildElement("OdorTypeID", m_doOdorType.ID)
                oXml.AddChildElement("Quantity", m_fltQuantity)
                oXml.AddChildElement("UseFoodQuantity", m_bUseFoodQuantity)
                oXml.AddChildElement("Enabled", m_bEnabled)

                oXml.OutOfElem()
            End If

        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            If Not m_doOdorType Is Nothing Then
                oXml.AddChildElement("Odor")
                oXml.IntoElem()

                oXml.AddChildElement("Name", m_doOdorType.Name)
                oXml.AddChildElement("ID", m_strID)
                oXml.AddChildElement("OdorTypeID", m_doOdorType.ID)
                oXml.AddChildElement("Quantity", m_fltQuantity)
                oXml.AddChildElement("UseFoodQuantity", m_bUseFoodQuantity)
                oXml.AddChildElement("Enabled", m_bEnabled)

                oXml.OutOfElem()
            End If
        End Sub

#End Region

    End Class

End Namespace

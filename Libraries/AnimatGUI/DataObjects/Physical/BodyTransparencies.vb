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

    Public Class BodyTransparencies
        Inherits Framework.DataObject

#Region " Attributes "

        Protected m_fltGraphicsTrans As Single
        Protected m_fltCollisionsTrans As Single
        Protected m_fltJointsTrans As Single
        Protected m_fltRecFieldTrans As Single
        Protected m_fltSimTrans As Single

#End Region

#Region " Properties "

        Public Overridable Property GraphicsTransparency() As Single
            Get
                Return m_fltGraphicsTrans
            End Get
            Set(ByVal value As Single)
                If value < 0 Then
                    Throw New System.Exception("Transparency values cannont be less than 0%.")
                End If
                If value > 100 Then
                    Throw New System.Exception("Transparency values cannot be greater than 100%.")
                End If
                m_doParent.SetSimData("GraphicsAlpha", CalcAlpha(value).ToString(), True)
                m_fltGraphicsTrans = value
            End Set
        End Property

        Public Overridable Property CollisionsTransparency() As Single
            Get
                Return m_fltCollisionsTrans
            End Get
            Set(ByVal value As Single)
                If value < 0 Then
                    Throw New System.Exception("Transparency values cannont be less than 0%.")
                End If
                If value > 100 Then
                    Throw New System.Exception("Transparency values cannot be greater than 100%.")
                End If
                m_doParent.SetSimData("CollisionAlpha", CalcAlpha(value).ToString(), True)
                m_fltCollisionsTrans = value
            End Set
        End Property

        Public Overridable Property JointsTransparency() As Single
            Get
                Return m_fltJointsTrans
            End Get
            Set(ByVal value As Single)
                If value < 0 Then
                    Throw New System.Exception("Transparency values cannont be less than 0%.")
                End If
                If value > 100 Then
                    Throw New System.Exception("Transparency values cannot be greater than 100%.")
                End If
                m_doParent.SetSimData("JointsAlpha", CalcAlpha(value).ToString(), True)
                m_fltJointsTrans = value
            End Set
        End Property

        Public Overridable Property ReceptiveFieldsTransparency() As Single
            Get
                Return m_fltRecFieldTrans
            End Get
            Set(ByVal value As Single)
                If value < 0 Then
                    Throw New System.Exception("Transparency values cannont be less than 0%.")
                End If
                If value > 100 Then
                    Throw New System.Exception("Transparency values cannot be greater than 100%.")
                End If
                m_doParent.SetSimData("ReceptiveFieldsAlpha", CalcAlpha(value).ToString(), True)
                m_fltRecFieldTrans = value
            End Set
        End Property

        Public Overridable Property SimulationTransparency() As Single
            Get
                Return m_fltSimTrans
            End Get
            Set(ByVal value As Single)
                If value < 0 Then
                    Throw New System.Exception("Transparency values cannont be less than 0%.")
                End If
                If value > 100 Then
                    Throw New System.Exception("Transparency values cannot be greater than 100%.")
                End If
                m_doParent.SetSimData("SimulationAlpha", CalcAlpha(value).ToString(), True)
                m_fltSimTrans = value
            End Set
        End Property

        'Alpha values in the gui are 0-100%, we need to convert this to 1-0
        Protected Overridable Function CalcAlpha(ByVal fltVal As Single) As Single
            Return CSng((100.0 - fltVal) / 100.0)
        End Function

        Public Overridable ReadOnly Property GraphicsAlpha() As Single
            Get
                Return CSng((100.0 - m_fltGraphicsTrans) / 100.0)
            End Get
        End Property

        Public Overridable ReadOnly Property CollisionsAlpha() As Single
            Get
                Return CSng((100.0 - m_fltCollisionsTrans) / 100.0)
            End Get
        End Property

        Public Overridable ReadOnly Property JointsAlpha() As Single
            Get
                Return CSng((100.0 - m_fltJointsTrans) / 100.0)
            End Get
        End Property

        Public Overridable ReadOnly Property ReceptiveFieldsAlpha() As Single
            Get
                Return CSng((100.0 - m_fltRecFieldTrans) / 100.0)
            End Get
        End Property

        Public Overridable ReadOnly Property SimulationAlpha() As Single
            Get
                Return CSng((100.0 - m_fltSimTrans) / 100.0)
            End Get
        End Property

#End Region

#Region " Methods "


        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim doItem As New BodyTransparencies(doParent)

            doItem.m_fltGraphicsTrans = m_fltGraphicsTrans
            doItem.m_fltCollisionsTrans = m_fltCollisionsTrans
            doItem.m_fltJointsTrans = m_fltJointsTrans
            doItem.m_fltRecFieldTrans = m_fltRecFieldTrans
            doItem.m_fltSimTrans = m_fltSimTrans

            Return doItem
        End Function

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Graphics", m_fltGraphicsTrans.GetType(), "GraphicsTransparency", _
                                        "Properties", "The transparency for this item when in graphics selection mode.", m_fltGraphicsTrans))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Collisions", m_fltCollisionsTrans.GetType(), "CollisionsTransparency", _
                                        "Properties", "The transparency for this item when in collisions selection mode.", m_fltCollisionsTrans))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Joints", m_fltJointsTrans.GetType(), "JointsTransparency", _
                                        "Properties", "The transparency for this item when in joints selection mode.", m_fltJointsTrans))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Receptive Fields", m_fltRecFieldTrans.GetType(), "ReceptiveFieldsTransparency", _
                            "Properties", "The transparency for this item when in receptive fields selection mode.", m_fltRecFieldTrans))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Simulation", m_fltSimTrans.GetType(), "SimulationTransparency", _
                            "Properties", "The transparency for this item when in simulation mode.", m_fltSimTrans))

        End Sub

        Public Overridable Overloads Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            If oXml.FindChildElement("Transparencies", False) Then
                oXml.IntoElem() 'Into Transparencies Element

                m_fltGraphicsTrans = oXml.GetChildFloat("Graphics", m_fltGraphicsTrans)
                m_fltCollisionsTrans = oXml.GetChildFloat("Collisions", m_fltCollisionsTrans)
                m_fltJointsTrans = oXml.GetChildFloat("Joints", m_fltJointsTrans)
                m_fltRecFieldTrans = oXml.GetChildFloat("RecFields", m_fltRecFieldTrans)
                m_fltSimTrans = oXml.GetChildFloat("Simulation", m_fltSimTrans)

                oXml.OutOfElem() 'Outof BodyPart Element
            End If

        End Sub

        Public Overridable Overloads Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            oXml.AddChildElement("Transparencies")

            oXml.IntoElem() 'Into Child Elemement

            oXml.AddChildElement("Graphics", m_fltGraphicsTrans)
            oXml.AddChildElement("Collisions", m_fltCollisionsTrans)
            oXml.AddChildElement("Joints", m_fltJointsTrans)
            oXml.AddChildElement("RecFields", m_fltRecFieldTrans)
            oXml.AddChildElement("Simulation", m_fltSimTrans)

            oXml.OutOfElem() 'Outof BodyPart Element

        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            'Convert it back from transparencies to alpha and from percents to 0-1
            oXml.AddChildElement("GraphicsAlpha", Me.GraphicsAlpha)
            oXml.AddChildElement("CollisionsAlpha", Me.CollisionsAlpha)
            oXml.AddChildElement("JointsAlpha", Me.JointsAlpha)
            oXml.AddChildElement("ReceptiveFieldsAlpha", Me.ReceptiveFieldsAlpha)
            oXml.AddChildElement("SimulationAlpha", Me.SimulationAlpha)

        End Sub

#End Region

#Region " BodyTransparencyPropBagConverter "

        Public Class BodyTransparencyPropBagConverter
            Inherits ExpandableObjectConverter

            Public Overloads Overrides Function CanConvertFrom(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal sourceType As System.Type) As Boolean

                Return MyBase.CanConvertFrom(context, sourceType)
            End Function

            Public Overloads Overrides Function CanConvertTo(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal destinationType As System.Type) As Boolean

                If destinationType Is GetType(AnimatGuiCtrls.Controls.PropertyBag) Then
                    Return True
                End If
                Return MyBase.CanConvertTo(context, destinationType)

            End Function

            Public Overloads Overrides Function ConvertTo(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal culture As System.Globalization.CultureInfo, ByVal value As Object, ByVal destinationType As System.Type) As Object
                If destinationType Is GetType(String) AndAlso TypeOf (value) Is AnimatGuiCtrls.Controls.PropertyTable Then
                    'Dim pbValue As AnimatGuiCtrls.Controls.PropertyTable = DirectCast(value, AnimatGuiCtrls.Controls.PropertyTable)

                    'Dim psValue As AnimatGuiCtrls.Controls.PropertySpec = DirectCast(pbValue.Properties(0), AnimatGuiCtrls.Controls.PropertySpec)
                    'Dim psScale As AnimatGuiCtrls.Controls.PropertySpec = DirectCast(pbValue.Properties(1), AnimatGuiCtrls.Controls.PropertySpec)
                    'Dim psUnits As AnimatGuiCtrls.Controls.PropertySpec = DirectCast(pbValue.Properties(2), AnimatGuiCtrls.Controls.PropertySpec)
                    'Dim psUnitsAbbrev As AnimatGuiCtrls.Controls.PropertySpec = DirectCast(pbValue.Properties(3), AnimatGuiCtrls.Controls.PropertySpec)

                    'Dim eScale As ScaledNumber.enumNumericScale = DirectCast(psScale.DefaultValue, ScaledNumber.enumNumericScale)
                    'Dim strUnits As String = DirectCast(psUnitsAbbrev.DefaultValue, String)

                    'Dim strValue As String = Convert.ToDouble(psValue.DefaultValue).ToString("0.###") & " " & _
                    '                         ScaledNumber.ScaleAbbreviation(eScale) & strUnits

                    'Return strValue
                    Return ""
                ElseIf destinationType Is GetType(String) AndAlso TypeOf (value) Is BodyTransparencies Then
                    Dim svValue As BodyTransparencies = DirectCast(value, BodyTransparencies)

                    Dim strValue As String = svValue.ToString
                    Return strValue
                End If

                Return MyBase.ConvertTo(context, culture, value, destinationType)
            End Function

        End Class

#End Region

    End Class

End Namespace

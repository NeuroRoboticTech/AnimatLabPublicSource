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

Namespace DataObjects
    Namespace ProjectMigrations

        Public Class Migrate_V2
            Inherits ProjectMigration

            Public Overrides ReadOnly Property ConvertFrom As String
                Get
                    Return "2"
                End Get
            End Property

            Public Overrides ReadOnly Property ConvertTo As String
                Get
                    Return "3"
                End Get
            End Property

            Sub New()
                MyBase.New()
            End Sub

            Protected Overrides Sub ConvertProjectNode(ByVal xnProject As XmlNode, ByVal strPhysics As String)

                m_xnProjectXml.UpdateSingleNodeValue(xnProject, "Version", ConvertTo(), False)

                m_xnProjectXml.RemoveNode(xnProject, "Physics", False)
                m_xnProjectXml.AddNodeValue(xnProject, "Physics", strPhysics)

                Dim xnSimNode As XmlNode = m_xnProjectXml.GetNode(xnProject, "Simulation")
                Dim xmlEnv As XmlNode = m_xnProjectXml.GetNode(xnSimNode, "Environment")
                Dim xmlMaterialTypes As XmlNode = m_xnProjectXml.GetNode(xmlEnv, "MaterialTypes")
                Dim xmlMaterialPairs As XmlNode = m_xnProjectXml.GetNode(xmlEnv, "MaterialPairs")

                'Go through the material types and find the material pair for this one and the default material
                'We will use that one for the conversion.
                For Each xnType As XmlNode In xmlMaterialTypes.ChildNodes
                    ConvertMaterialType(xnType, xmlMaterialPairs)
                Next

                m_xnProjectXml.RemoveNode(xmlEnv, "MaterialPairs")

            End Sub

            Protected Sub ConvertMaterialType(ByVal xnType As XmlNode, ByVal xmlMaterialPairs As XmlNode)
                Dim strID As String = m_xnProjectXml.GetSingleNodeValue(xnType, "ID")

                For Each xnPair As XmlNode In xmlMaterialPairs.ChildNodes
                    Dim strID1 As String = m_xnProjectXml.GetSingleNodeValue(xnPair, "Material1ID")
                    Dim strID2 As String = m_xnProjectXml.GetSingleNodeValue(xnPair, "Material2ID")

                    If (strID1 = strID AndAlso strID2 = "DEFAULTMATERIAL") OrElse (strID2 = strID AndAlso strID1 = "DEFAULTMATERIAL") Then
                        Dim dblVal As Double = 0
                        Dim strScale As String = ""
                        Dim dblActualVal As Double = 0

                        m_xnProjectXml.LoadScaleNumber(xnPair, "FrictionPrimaryCoefficient", dblVal, strScale, dblActualVal)
                        m_xnProjectXml.AddScaledNumber(xnType, "FrictionLinearPrimary", dblVal, strScale, dblActualVal)

                        m_xnProjectXml.LoadScaleNumber(xnPair, "SecondaryFrictionCoefficient", dblVal, strScale, dblActualVal)
                        m_xnProjectXml.AddScaledNumber(xnType, "FrictionLinearSecondary", dblVal, strScale, dblActualVal)

                        m_xnProjectXml.AddScaledNumber(xnType, "FrictionAngularNormal", 0, "None", 0)
                        m_xnProjectXml.AddScaledNumber(xnType, "FrictionAngularPrimary", 0, "None", 0)
                        m_xnProjectXml.AddScaledNumber(xnType, "FrictionAngularSecondary", 0, "None", 0)

                        m_xnProjectXml.LoadScaleNumber(xnPair, "PrimaryMaximumFriction", dblVal, strScale, dblActualVal)
                        m_xnProjectXml.AddScaledNumber(xnType, "FrictionLinearPrimaryMax", dblVal, strScale, dblActualVal)

                        m_xnProjectXml.LoadScaleNumber(xnPair, "SecondaryMaximumFriction", dblVal, strScale, dblActualVal)
                        m_xnProjectXml.AddScaledNumber(xnType, "FrictionLinearSecondaryMax", dblVal, strScale, dblActualVal)

                        m_xnProjectXml.AddScaledNumber(xnType, "FrictionAngularNormalMax", 5, "None", 5)
                        m_xnProjectXml.AddScaledNumber(xnType, "FrictionAngularPrimaryMax", 5, "None", 5)
                        m_xnProjectXml.AddScaledNumber(xnType, "FrictionAngularSecondaryMax", 5, "None", 5)

                        m_xnProjectXml.LoadScaleNumber(xnPair, "Compliance", dblVal, strScale, dblActualVal)
                        m_xnProjectXml.AddScaledNumber(xnType, "Compliance", dblVal, strScale, dblActualVal)

                        m_xnProjectXml.LoadScaleNumber(xnPair, "Damping", dblVal, strScale, dblActualVal)
                        m_xnProjectXml.AddScaledNumber(xnType, "Damping", dblVal, strScale, dblActualVal)

                        m_xnProjectXml.LoadScaleNumber(xnPair, "Restitution", dblVal, strScale, dblActualVal)
                        m_xnProjectXml.AddScaledNumber(xnType, "Restitution", dblVal, strScale, dblActualVal)

                        m_xnProjectXml.LoadScaleNumber(xnPair, "PrimarySlip", dblVal, strScale, dblActualVal)
                        m_xnProjectXml.AddScaledNumber(xnType, "SlipLinearPrimary", dblVal, strScale, dblActualVal)

                        m_xnProjectXml.LoadScaleNumber(xnPair, "SecondarySlip", dblVal, strScale, dblActualVal)
                        m_xnProjectXml.AddScaledNumber(xnType, "SlipLinearSecondary", dblVal, strScale, dblActualVal)

                        m_xnProjectXml.AddScaledNumber(xnType, "SlipAngularNormal", 0, "None", 0)
                        m_xnProjectXml.AddScaledNumber(xnType, "SlipAngularPrimary", 0, "None", 0)
                        m_xnProjectXml.AddScaledNumber(xnType, "SlipAngularSecondary", 0, "None", 0)

                        m_xnProjectXml.LoadScaleNumber(xnPair, "PrimarySlide", dblVal, strScale, dblActualVal)
                        m_xnProjectXml.AddScaledNumber(xnType, "SlideLinearPrimary", dblVal, strScale, dblActualVal)

                        m_xnProjectXml.LoadScaleNumber(xnPair, "SecondarySlide", dblVal, strScale, dblActualVal)
                        m_xnProjectXml.AddScaledNumber(xnType, "SlideLinearSecondary", dblVal, strScale, dblActualVal)

                        m_xnProjectXml.AddScaledNumber(xnType, "SlideAngularNormal", 0, "None", 0)
                        m_xnProjectXml.AddScaledNumber(xnType, "SlideAngularPrimary", 0, "None", 0)
                        m_xnProjectXml.AddScaledNumber(xnType, "SlideAngularSecondary", 0, "None", 0)

                        m_xnProjectXml.LoadScaleNumber(xnPair, "MaximumAdhesion", dblVal, strScale, dblActualVal)
                        m_xnProjectXml.AddScaledNumber(xnType, "MaximumAdhesion", dblVal, strScale, dblActualVal)

                    End If
                Next

            End Sub

        End Class

    End Namespace
End Namespace

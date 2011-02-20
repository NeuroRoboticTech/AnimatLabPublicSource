Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports Crownwood.Magic.Common
Imports AnimatGuiCtrls.Controls
Imports Crownwood.Magic.Docking
Imports Crownwood.Magic.Menus
Imports AnimatGUI.Framework

Namespace DataObjects.Physical


    Public Class ConstraintLimit
        Inherits AnimatGUI.Framework.DataObject

#Region " Attributes "

        Protected m_strLimitDescr As String
        Protected m_doPairedLimit As ConstraintLimit
        Protected m_snAngle As AnimatGUI.Framework.ScaledNumber
        Protected m_snDamping As ScaledNumber
        Protected m_snRestitution As ScaledNumber
        Protected m_snStiffness As ScaledNumber

#End Region

#Region " Properties "

        Public Overridable Property LimitDescription() As String
            Get
                Return m_strLimitDescr
            End Get
            Set(ByVal value As String)
                m_strLimitDescr = value
            End Set
        End Property
        Public Overridable Property PairedLimit() As ConstraintLimit
            Get
                Return m_doPairedLimit
            End Get
            Set(ByVal value As ConstraintLimit)
                m_doPairedLimit = value
            End Set
        End Property

        Public Overridable Property Angle() As ScaledNumber
            Get
                Return m_snAngle
            End Get
            Set(ByVal value As ScaledNumber)
                If Not m_doPairedLimit Is Nothing AndAlso value.ActualValue > m_doPairedLimit.Angle.ActualValue Then
                    Throw New System.Exception("The " & m_strLimitDescr.ToLower() & " angle cannot be larger than its opposite angle.")
                End If
                If value.ActualValue < -180 Then
                    Throw New System.Exception("The " & m_strLimitDescr.ToLower() & " angle cannot be less than -180 degrees.")
                End If
                If value.ActualValue > 180 Then
                    Throw New System.Exception("The " & m_strLimitDescr.ToLower() & " angle cannot be greater than 180 degrees.")
                End If

                SetSimData("Angle", Util.DegreesToRadians(CSng(value.ActualValue)).ToString(), True)
                m_snAngle.CopyData(value)
            End Set
        End Property

        Public Overridable Property Damping() As ScaledNumber
            Get
                Return m_snDamping
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The damping can not be less than zero.")
                End If

                SetSimData("Damping", m_snDamping.ActualValue.ToString, True)
                m_snDamping.CopyData(Value)
            End Set
        End Property

        Public Overridable Property Restitution() As ScaledNumber
            Get
                Return m_snRestitution
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 OrElse Value.ActualValue > 1 Then
                    Throw New System.Exception("The restitution must be between 0 and 1.")
                End If

                SetSimData("Restitution", m_snRestitution.ActualValue.ToString, True)
                m_snRestitution.CopyData(Value)
            End Set
        End Property

        Public Overridable Property Stiffness() As ScaledNumber
            Get
                Return m_snStiffness
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The stiffness can not be less than zero.")
                End If

                SetSimData("Stiffness", m_snStiffness.ActualValue.ToString, True)
                m_snStiffness.CopyData(Value)
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            m_snAngle = New AnimatGUI.Framework.ScaledNumber(Me, "Angle", -45, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Degrees", "Deg")
            m_snStiffness = New AnimatGUI.Framework.ScaledNumber(Me, "Stiffness", 5, ScaledNumber.enumNumericScale.Mega, "N/m", "N/m")
            m_snDamping = New AnimatGUI.Framework.ScaledNumber(Me, "Damping", 0, ScaledNumber.enumNumericScale.Kilo, "g/s", "g/s")
            m_snRestitution = New AnimatGUI.Framework.ScaledNumber(Me, "Restitution", 0, ScaledNumber.enumNumericScale.None, "v/v", "v/v")

        End Sub

        Public Overridable Sub CopyData(ByVal doConstraint As ConstraintLimit)

            m_snAngle.CopyData(doConstraint.m_snAngle)
            m_snDamping.CopyData(doConstraint.m_snDamping)
            m_snRestitution.CopyData(doConstraint.m_snRestitution)
            m_snStiffness.CopyData(doConstraint.m_snStiffness)

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            m_snAngle.ClearIsDirty()
            m_snStiffness.ClearIsDirty()
            m_snDamping.ClearIsDirty()
            m_snRestitution.ClearIsDirty()
        End Sub

        Public Overrides Function Clone(ByVal doParent As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject) As Framework.DataObject
            Dim oNewNode As New ConstraintLimit(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doOrig As ConstraintLimit = DirectCast(doOriginal, ConstraintLimit)

            m_doPairedLimit = doOrig.m_doPairedLimit
            m_snAngle = DirectCast(doOrig.m_snAngle, ScaledNumber)
            m_snStiffness = DirectCast(doOrig.m_snStiffness, ScaledNumber)
            m_snDamping = DirectCast(doOrig.m_snDamping, ScaledNumber)
            m_snRestitution = DirectCast(doOrig.m_snRestitution, ScaledNumber)

        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag
            pbNumberBag = m_snAngle.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Angle", pbNumberBag.GetType(), "Angle", _
                                        "Constraints", "Sets the " & m_strLimitDescr.ToLower() & " angle rotation that is allowed for this joint in degrees.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Enable Limit", m_bEnabled.GetType(), "Enabled", _
                                        "Constraints", "Enables or disables this joint limit constraints.", m_bEnabled))

            pbNumberBag = m_snDamping.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Damping", pbNumberBag.GetType(), "Damping", _
                                        "Constraints", "The damping term for this limit. If the stiffness and damping " & _
                                        "of an individual limit are both zero, it is effectively deactivated. This is the damping " & _
                                        "of the virtual spring used when the joint reaches its limit. It is not frictional damping " & _
                                        "for the motion around the joint.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snRestitution.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Restitution", pbNumberBag.GetType(), "Restitution", _
                                        "Constraints", "The coefficient of restitution is the ratio of rebound velocity to " & _
                                        "impact velocity when the joint reaches the low or high stop. This is used if the limit stiffness " & _
                                        "is greater than zero. Restitution must be in the range zero to one inclusive.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snStiffness.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Stiffness", pbNumberBag.GetType(), "Stiffness", _
                                        "Constraints", "The spring constant is used for restitution force when a limited " & _
                                        "joint reaches one of its stops. This limit property must be zero or positive. " & _
                                        "If the stiffness and damping of an individual limit are both zero, it is effectively deactivated.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))



        End Sub

        Public Overloads Sub LoadData(ByRef oXml As AnimatGUI.Interfaces.StdXml, ByVal strPropName As String)

            If oXml.FindChildElement(strPropName, False) Then
                oXml.IntoElem() 'Into Constraint Element

                m_strID = oXml.GetChildString("ID")
                m_strName = oXml.GetChildString("Name", m_strID)

                If m_strID.Trim.Length = 0 Then
                    m_strID = System.Guid.NewGuid().ToString()
                End If

                If m_strName.Trim.Length = 0 Then
                    m_strName = m_strID
                End If

                m_bEnabled = oXml.GetChildBool("Enabled", m_bEnabled)
                m_snAngle.LoadData(oXml, "Angle")
                m_snDamping.LoadData(oXml, "Damping")
                m_snRestitution.LoadData(oXml, "Restitution")
                m_snStiffness.LoadData(oXml, "Stiffness")

                oXml.OutOfElem() 'Outof Constraint Element
            End If

        End Sub

        Public Overloads Sub SaveData(ByRef oXml As Interfaces.StdXml, ByVal strPropName As String)

            oXml.AddChildElement(strPropName)
            oXml.IntoElem() 'Into Child Elemement

            oXml.AddChildElement("Name", m_strName)
            oXml.AddChildElement("ID", m_strID)
            oXml.AddChildElement("Enabled", m_bEnabled)
            m_snAngle.SaveData(oXml, "Angle")
            m_snDamping.SaveData(oXml, "Damping")
            m_snRestitution.SaveData(oXml, "Restitution")
            m_snStiffness.SaveData(oXml, "Stiffness")

            oXml.OutOfElem() 'Outof BodyPart Element

        End Sub

        Public Overrides Sub SaveSimulationXml(ByRef oXml As Interfaces.StdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            oXml.AddChildElement(strName)
            oXml.IntoElem() 'Into Child Elemement

            oXml.AddChildElement("Name", m_strName)
            oXml.AddChildElement("ID", m_strID)
            oXml.AddChildElement("Enabled", m_bEnabled)
            m_snAngle.SaveSimulationXml(oXml, Me, "Angle")
            m_snDamping.SaveSimulationXml(oXml, Me, "Damping")
            m_snRestitution.SaveSimulationXml(oXml, Me, "Restitution")
            m_snStiffness.SaveSimulationXml(oXml, Me, "Stiffness")

            oXml.OutOfElem() 'Outof BodyPart Element

        End Sub

        Public Overrides Function ToString() As String
            Return m_snAngle.ToString()
        End Function

#End Region

    End Class

End Namespace

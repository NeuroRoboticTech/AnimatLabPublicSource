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


    Public Class ConstraintLimit
        Inherits AnimatGUI.Framework.DataObject

#Region " Attributes "

        Protected m_doPairedLimit As ConstraintLimit
        Protected m_snLimitPos As AnimatGUI.Framework.ScaledNumber
        Protected m_snDamping As ScaledNumber
        Protected m_snRestitution As ScaledNumber
        Protected m_snStiffness As ScaledNumber
        Protected m_bAngleLimit As Boolean = True
        Protected m_bIsLowerLimit As Boolean = True

#End Region

#Region " Properties "

        Public Overridable ReadOnly Property LimitDescription() As String
            Get
                If m_bIsLowerLimit Then
                    Return "Lower"
                Else
                    Return "Upper"
                End If
            End Get
        End Property
        Public Overridable Property PairedLimit() As ConstraintLimit
            Get
                Return m_doPairedLimit
            End Get
            Set(ByVal value As ConstraintLimit)
                m_doPairedLimit = value
            End Set
        End Property

        Public Overridable Property AngleLimit() As Boolean
            Get
                Return m_bAngleLimit
            End Get
            Set(ByVal value As Boolean)
                m_bAngleLimit = value

                If m_bAngleLimit Then
                    m_snLimitPos = New AnimatGUI.Framework.ScaledNumber(Me, "LimitPos", -45, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Degrees", "Deg")
                Else
                    m_snLimitPos = New AnimatGUI.Framework.ScaledNumber(Me, "LimitPos", 10, AnimatGUI.Framework.ScaledNumber.enumNumericScale.centi, "meters", "m")
                End If
            End Set
        End Property

        Public Overridable Property IsLowerLimit() As Boolean
            Get
                Return m_bIsLowerLimit
            End Get
            Set(ByVal value As Boolean)
                m_bIsLowerLimit = value
            End Set
        End Property

        Public Overridable Property LimitPos() As ScaledNumber
            Get
                Return m_snLimitPos
            End Get
            Set(ByVal value As ScaledNumber)
                If Not m_doPairedLimit Is Nothing Then
                    If m_bIsLowerLimit AndAlso value.ActualValue > m_doPairedLimit.LimitPos.ActualValue Then
                        Throw New System.Exception("The " & LimitDescription().ToLower() & " position cannot be larger than its opposite angle.")
                    ElseIf Not m_bIsLowerLimit AndAlso value.ActualValue < m_doPairedLimit.LimitPos.ActualValue Then
                        Throw New System.Exception("The " & LimitDescription().ToLower() & " position cannot be less than its opposite angle.")
                    End If
                End If

                If m_bAngleLimit Then
                    If value.ActualValue < -180 Then
                        Throw New System.Exception("The " & LimitDescription.ToLower() & " angle cannot be less than -180 degrees.")
                    End If
                    If value.ActualValue > 180 Then
                        Throw New System.Exception("The " & LimitDescription.ToLower() & " angle cannot be greater than 180 degrees.")
                    End If

                    SetSimData("LimitPos", Util.DegreesToRadians(CSng(value.ActualValue)).ToString(), True)
                    m_snLimitPos.CopyData(value)
                Else
                    SetSimData("LimitPos", value.ActualValue.ToString(), True)
                    m_snLimitPos.CopyData(value)
                End If
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

                SetSimData("Damping", Value.ActualValue.ToString, True)
                m_snDamping.CopyData(Value)
            End Set
        End Property

        Public Overridable Property Restitution() As ScaledNumber
            Get
                Return m_snRestitution
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 OrElse Value.ActualValue > 10000 Then
                    Throw New System.Exception("The restitution must be between 0 and 1.")
                End If

                SetSimData("Restitution", Value.ActualValue.ToString, True)
                m_snRestitution.CopyData(Value)
            End Set
        End Property

        Public Overridable Property Stiffness() As ScaledNumber
            Get
                Return m_snStiffness
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue <= 0 Then
                    Throw New System.Exception("The stiffness can not be less than or equal to zero.")
                End If

                SetSimData("Stiffness", Value.ActualValue.ToString, True)
                m_snStiffness.CopyData(Value)
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            m_snLimitPos = New AnimatGUI.Framework.ScaledNumber(Me, "LimitPos", -45, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Degrees", "Deg")

            If Util.Application.Physics.Name = "Vortex" Then
                m_snStiffness = New AnimatGUI.Framework.ScaledNumber(Me, "Stiffness", 1, ScaledNumber.enumNumericScale.Kilo, "N/m", "N/m")
                m_snDamping = New AnimatGUI.Framework.ScaledNumber(Me, "Damping", 250, ScaledNumber.enumNumericScale.None, "g/s", "g/s")
            Else
                m_snStiffness = New AnimatGUI.Framework.ScaledNumber(Me, "Stiffness", 100, ScaledNumber.enumNumericScale.None, "N/m", "N/m")
                m_snDamping = New AnimatGUI.Framework.ScaledNumber(Me, "Damping", 500, ScaledNumber.enumNumericScale.None, "g/s", "g/s")
            End If

            m_snRestitution = New AnimatGUI.Framework.ScaledNumber(Me, "Restitution", 0, ScaledNumber.enumNumericScale.None, "v/v", "v/v")

        End Sub

        Public Overridable Sub CopyData(ByVal doConstraint As ConstraintLimit)

            m_snLimitPos.CopyData(doConstraint.m_snLimitPos)
            m_snDamping.CopyData(doConstraint.m_snDamping)
            m_snRestitution.CopyData(doConstraint.m_snRestitution)
            m_snStiffness.CopyData(doConstraint.m_snStiffness)

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            m_snLimitPos.ClearIsDirty()
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
            m_snLimitPos = DirectCast(doOrig.m_snLimitPos.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snStiffness = DirectCast(doOrig.m_snStiffness.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snDamping = DirectCast(doOrig.m_snDamping.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snRestitution = DirectCast(doOrig.m_snRestitution.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_bIsLowerLimit = doOrig.m_bIsLowerLimit
            m_bAngleLimit = doOrig.m_bAngleLimit

        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag
            pbNumberBag = m_snLimitPos.Properties
            If m_bAngleLimit Then
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Angle", pbNumberBag.GetType(), "LimitPos", _
                                            "Constraints", "Sets the " & LimitDescription.ToLower() & " angle rotation that is allowed for this joint in degrees.", pbNumberBag, _
                                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))
            Else
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Position", pbNumberBag.GetType(), "LimitPos", _
                                            "Constraints", "Sets the " & LimitDescription.ToLower() & " position that is allowed for this joint.", pbNumberBag, _
                                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))
            End If

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

        Public Overridable Sub BuildPropertiesInline(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable, ByVal bIncludeOtherProps As Boolean, ByVal strSetName As String, ByVal strExtraPropName As String)

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag
            pbNumberBag = m_snLimitPos.Properties
            If m_bAngleLimit Then
                Dim strName As String = "Angle"
                If strSetName.Trim.Length > 0 Then strName = strSetName
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Angle", pbNumberBag.GetType(), strExtraPropName & "LimitPos", _
                                            "Constraint " & strSetName, "Sets the " & LimitDescription.ToLower() & " angle rotation that is allowed for this joint in degrees.", pbNumberBag, _
                                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))
            Else
                Dim strName As String = "Position"
                If strSetName.Trim.Length > 0 Then strName = strSetName
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Position", pbNumberBag.GetType(), strExtraPropName & "LimitPos", _
                                            "Constraint " & strSetName, "Sets the " & LimitDescription.ToLower() & " position that is allowed for this joint.", pbNumberBag, _
                                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))
            End If

            If bIncludeOtherProps Then
                Dim strName As String = "Damping"
                If strSetName.Trim.Length > 0 Then strName = strSetName
                pbNumberBag = m_snDamping.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Damping", pbNumberBag.GetType(), strExtraPropName & "Damping", _
                                            "Constraint " & strSetName, "The damping term for this limit. If the stiffness and damping " & _
                                            "of an individual limit are both zero, it is effectively deactivated. This is the damping " & _
                                            "of the virtual spring used when the joint reaches its limit. It is not frictional damping " & _
                                            "for the motion around the joint.", pbNumberBag, _
                                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

                pbNumberBag = m_snRestitution.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Restitution", pbNumberBag.GetType(), strExtraPropName & "Restitution", _
                                            "Constraint " & strSetName, "The coefficient of restitution is the ratio of rebound velocity to " & _
                                            "impact velocity when the joint reaches the low or high stop. This is used if the limit stiffness " & _
                                            "is greater than zero. Restitution must be in the range zero to one inclusive.", pbNumberBag, _
                                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

                strName = "Stiffness"
                If strSetName.Trim.Length > 0 Then strName = strSetName
                pbNumberBag = m_snStiffness.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Stiffness", pbNumberBag.GetType(), strExtraPropName & "Stiffness", _
                                            "Constraint " & strSetName, "The spring constant is used for restitution force when a limited " & _
                                            "joint reaches one of its stops. This limit property must be zero or positive. " & _
                                            "If the stiffness and damping of an individual limit are both zero, it is effectively deactivated.", pbNumberBag, _
                                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))
            End If

        End Sub

        Public Overloads Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml, ByVal strPropName As String)

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

                m_snLimitPos.LoadData(oXml, "LimitPos")
                m_snDamping.LoadData(oXml, "Damping")
                m_snRestitution.LoadData(oXml, "Restitution")
                m_snStiffness.LoadData(oXml, "Stiffness")

                oXml.OutOfElem() 'Outof Constraint Element
            End If

        End Sub

        Public Overloads Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml, ByVal strPropName As String)

            oXml.AddChildElement(strPropName)
            oXml.IntoElem() 'Into Child Elemement

            oXml.AddChildElement("Name", m_strName)
            oXml.AddChildElement("ID", m_strID)
            m_snLimitPos.SaveData(oXml, "LimitPos")
            m_snDamping.SaveData(oXml, "Damping")
            m_snRestitution.SaveData(oXml, "Restitution")
            m_snStiffness.SaveData(oXml, "Stiffness")

            oXml.OutOfElem() 'Outof BodyPart Element

        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            oXml.AddChildElement(strName)
            oXml.IntoElem() 'Into Child Elemement

            oXml.AddChildElement("Name", m_strName)
            oXml.AddChildElement("ID", m_strID)

            If m_bAngleLimit Then
                oXml.AddChildElement("LimitPos", Util.DegreesToRadians(CSng(m_snLimitPos.ActualValue)))
            Else
                oXml.AddChildElement("LimitPos", m_snLimitPos.ActualValue)
            End If
            m_snDamping.SaveSimulationXml(oXml, Me, "Damping")
            m_snRestitution.SaveSimulationXml(oXml, Me, "Restitution")
            m_snStiffness.SaveSimulationXml(oXml, Me, "Stiffness")

            oXml.OutOfElem() 'Outof BodyPart Element

        End Sub

        Public Overrides Function ToString() As String
            Return m_snLimitPos.ToString()
        End Function

#End Region

    End Class

End Namespace

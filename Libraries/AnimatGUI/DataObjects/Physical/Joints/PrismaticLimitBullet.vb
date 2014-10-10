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

Namespace DataObjects.Physical.Joints


    Public Class PrismaticLimitBullet
        Inherits ConstraintLimit

#Region " Attributes "

#End Region

#Region " Properties "

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

        End Sub

        Public Overrides Function Clone(ByVal doParent As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject) As Framework.DataObject
            Dim oNewNode As New PrismaticLimitBullet(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
        End Sub

        Public Overrides Sub BuildPropertiesInline(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable, ByVal bIncludeOtherProps As Boolean, ByVal strSetName As String, ByVal strExtraPropName As String)

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag
            pbNumberBag = m_snLimitPos.Properties
            Dim strName As String = strSetName + " " + "Position"
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec(strName, pbNumberBag.GetType(), strExtraPropName & "LimitPos", _
                                        "Constraint Limit", "Sets the " & LimitDescription.ToLower() & " position that is allowed for this joint.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            If bIncludeOtherProps Then
                pbNumberBag = m_snDamping.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Damping", pbNumberBag.GetType(), strExtraPropName & "Damping", _
                                            "Constraint Limit", "The damping term for this limit. If the stiffness and damping " & _
                                            "of an individual limit are both zero, it is effectively deactivated. This is the damping " & _
                                            "of the virtual spring used when the joint reaches its limit. It is not frictional damping " & _
                                            "for the motion around the joint.", pbNumberBag, _
                                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

                pbNumberBag = m_snStiffness.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Stiffness", pbNumberBag.GetType(), strExtraPropName & "Stiffness", _
                                            "Constraint Limit", "The spring constant is used for restitution force when a limited " & _
                                            "joint reaches one of its stops. This limit property must be zero or positive. " & _
                                            "If the stiffness and damping of an individual limit are both zero, it is effectively deactivated.", pbNumberBag, _
                                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))
            End If

        End Sub

#End Region

    End Class

End Namespace

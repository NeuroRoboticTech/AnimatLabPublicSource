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


    Public Class HingeLimitBullet
        Inherits ConstraintLimit

#Region " Attributes "

#End Region

#Region " Properties "

        Public Overrides Property Restitution() As ScaledNumber
            Get
                Return m_snRestitution
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 OrElse Value.ActualValue > 100 Then
                    Throw New System.Exception("The bounce must be between 0 and 100.")
                End If

                SetSimData("Restitution", Value.ActualValue.ToString, True)
                m_snRestitution.CopyData(Value)
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

        End Sub

        Public Overrides Function Clone(ByVal doParent As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject) As Framework.DataObject
            Dim oNewNode As New HingeLimitBullet(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
        End Sub

        Public Overrides Sub BuildPropertiesInline(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable, ByVal bIncludeOtherProps As Boolean, ByVal strSetName As String, ByVal strExtraPropName As String)

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag
            pbNumberBag = m_snLimitPos.Properties
            Dim strName As String = strSetName + " " + "Angle"
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec(strName, pbNumberBag.GetType(), strExtraPropName & "LimitPos", _
                                        "Constraint Limit", "Sets the " & LimitDescription.ToLower() & " angle rotation that is allowed for this joint in degrees.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            If bIncludeOtherProps Then
                pbNumberBag = m_snRestitution.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Bounce", pbNumberBag.GetType(), strExtraPropName & "Restitution", _
                                            "Constraint Limit", "Determines the bounce for this constraint limit.", pbNumberBag, _
                                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))
            End If

        End Sub

#End Region

    End Class

End Namespace

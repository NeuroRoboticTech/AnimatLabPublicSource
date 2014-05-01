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
    Namespace Robotics

        Public MustInherit Class InputSystem
            Inherits RobotPartInterface

#Region " Attributes "

#End Region

#Region " Properties "

#End Region

#Region " Methods "

            Public Sub New(ByVal doParent As Framework.DataObject)
                MyBase.New(doParent)

                m_strName = "InputSystem"
            End Sub

            Protected Overrides Function CreateLinkedPropertyList(ByVal doItem As AnimatGUI.Framework.DataObject) As AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList
                Return New AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList(doItem, True, False)
            End Function

            Protected Overrides Function CreateLinkedPropertyList(ByVal doItem As AnimatGUI.Framework.DataObject, ByVal strPropertyName As String) As AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList
                Return New AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList(doItem, strPropertyName, True, False)
            End Function

#End Region

        End Class

    End Namespace
End Namespace

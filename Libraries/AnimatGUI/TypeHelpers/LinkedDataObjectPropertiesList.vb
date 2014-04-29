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

Namespace TypeHelpers

    Public Class LinkedDataObjectPropertiesList
        Inherits TypeHelpers.LinkedDataObject

#Region " Attributes "

        Protected m_strPropertyName As String

#End Region

#Region " Properties "

        Public Overridable ReadOnly Property PropertyName() As String
            Get
                Return m_strPropertyName
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doItem As AnimatGUI.Framework.DataObject)
            MyBase.New(doItem)
        End Sub

        Public Sub New(ByVal doItem As AnimatGUI.Framework.DataObject, ByVal strPropertyName As String)
            MyBase.New(doItem)
            m_strPropertyName = strPropertyName

            If Not doItem Is Nothing AndAlso Not doItem.SimInterface Is Nothing Then
                Dim aryNames As New System.Collections.ArrayList
                Dim aryTypes As New System.Collections.ArrayList
                Dim aryDirections As New System.Collections.ArrayList
                m_doItem.SimInterface.QueryProperties(aryNames, aryTypes, aryDirections)

                If Not aryNames.Contains(strPropertyName) Then
                    Throw New System.Exception("No Property was found for object '" & doItem.ID & "' with the property name '" & strPropertyName & "'")
                End If
            End If

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNew As New LinkedDataObjectPropertiesList(doParent)
            oNew.CloneInternal(Me, bCutData, doRoot)
            Return oNew
        End Function

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
        End Sub

        Public Overrides Sub BuildPropertyDropDown(ByRef ctrlDropDown As System.Windows.Forms.Control)
            If m_doItem Is Nothing OrElse m_doItem.SimInterface Is Nothing Then Return

            If Not TypeOf (ctrlDropDown) Is ListBox Then
                Throw New System.Exception("The control passed into LinkedBodyPart.BuildPropertyDropDown is not a listbox type")
            End If

            Dim lbList As ListBox = DirectCast(ctrlDropDown, ListBox)

            'First lets find all body parts in this organism of the specified type.
            Dim colParts As New AnimatGUI.Collections.DataObjects(Nothing)
            Dim aryNames As New System.Collections.ArrayList
            Dim aryTypes As New System.Collections.ArrayList
            Dim aryDirections As New System.Collections.ArrayList
            m_doItem.SimInterface.QueryProperties(aryNames, aryTypes, aryDirections)

            lbList.BeginUpdate()
            lbList.Items.Clear()
            Dim lbSelectedItem As AnimatGUI.TypeHelpers.DropDownEntry = Nothing
            For iIdx As Integer = 0 To (aryNames.Count - 1)
                Dim strPropName As String = aryNames(iIdx).ToString
                Dim strPropType As String = aryTypes(iIdx).ToString

                If strPropType = "Boolean" OrElse strPropType = "Integer" OrElse strPropType = "Float" Then
                    Dim thLinkedProp As LinkedDataObjectPropertiesList = New LinkedDataObjectPropertiesList(m_doItem, strPropName)
                    Dim leItem As New AnimatGUI.TypeHelpers.DropDownEntry(strPropName, thLinkedProp)

                    lbList.Items.Add(leItem)

                    If m_strPropertyName = strPropName Then
                        lbSelectedItem = leItem
                    End If
                End If
            Next

            If Not lbSelectedItem Is Nothing Then lbList.SelectedItem = lbSelectedItem
            lbList.DisplayMember = "Display"
            lbList.ValueMember = "Value"

            MyBase.FormatDropDownList(lbList)

            lbList.EndUpdate()
            lbList.Invalidate()

        End Sub

#End Region

    End Class

End Namespace

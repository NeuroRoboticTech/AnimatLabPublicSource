Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports AnimatGuiCtrls.Controls

Namespace TypeHelpers

    <TypeConverter(GetType(AnimatGUI.TypeHelpers.DataTypeIDTypeConverter))> _
    Public Class DataTypeID
        Inherits Framework.DataObject

#Region " Attributes "

        Protected m_aryDataTypes As New AnimatGUI.Collections.DataTypes(Nothing)
        Protected m_strSelectedID As String = ""

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Overridable ReadOnly Property DataTypes() As AnimatGUI.Collections.DataTypes
            Get
                Return m_aryDataTypes
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides Property ID() As String
            Get
                Return m_strSelectedID
            End Get
            Set(ByVal Value As String)
                If m_aryDataTypes.ContainsKey(Value) Then
                    m_strSelectedID = Value
                Else
                    Throw New System.Exception("No id like '" & Value & "' was found in the data types collection.")
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property Value() As AnimatGUI.DataObjects.DataType
            Get
                Return m_aryDataTypes(m_strSelectedID)
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides Property ViewSubProperties() As Boolean
            Get
                Return False
            End Get
            Set(ByVal Value As Boolean)
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim dtNew As New DataTypeID(doParent)
            dtNew.m_aryDataTypes = DirectCast(Me.m_aryDataTypes.CloneList(), AnimatGUI.Collections.DataTypes)
            dtNew.m_strSelectedID = m_strSelectedID
            Return dtNew
        End Function

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
        End Sub

        Public Overrides Sub BuildPropertyDropDown(ByRef ctrlDropDown As System.Windows.Forms.Control)

            If Not TypeOf (ctrlDropDown) Is ListBox Then
                Throw New System.Exception("The control passed into LinkedSynapse.BuildPropertyDropDown is not a listbox type")
            End If

            Dim lbList As ListBox = DirectCast(ctrlDropDown, ListBox)

            lbList.BeginUpdate()
            lbList.Items.Clear()
            Dim lbSelectedItem As AnimatGUI.TypeHelpers.DropDownEntry = Nothing
            Dim doType As AnimatGUI.DataObjects.DataType
            For Each deItem As DictionaryEntry In m_aryDataTypes
                doType = DirectCast(deItem.Value, AnimatGUI.DataObjects.DataType)

                Dim thDataType As DataTypeID = DirectCast(Me.Clone(Me.Parent, False, Nothing), DataTypeID)
                thDataType.ID = doType.ID

                Dim leItem As New AnimatGUI.TypeHelpers.DropDownEntry(doType.Name, thDataType)

                lbList.Items.Add(leItem)

                If m_strSelectedID = doType.ID Then
                    lbSelectedItem = leItem
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

#End Region

    End Class

End Namespace


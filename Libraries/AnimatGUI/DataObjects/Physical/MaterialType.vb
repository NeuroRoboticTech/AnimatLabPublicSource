Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports AnimatGuiCtrls.Controls
Imports AnimatGUI.DataObjects

Namespace DataObjects.Physical

    ' This is what each rigid body will have as a property. It can have a name like "concrete" for example. '
    Public Class MaterialType
        Inherits Framework.DataObject

#Region " Attributes "

#End Region

#Region " Properties "

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Name", m_strID.GetType(), "Name", _
                                        "Material Properties", "The name of this material.", m_strName, False))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("ID", Me.ID.GetType(), "ID", _
                                        "Material Properties", "ID", Me.ID, True))

        End Sub

        Public Overrides Function Clone(ByVal doParent As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject) As Framework.DataObject
            Dim doItem As New MaterialType(doParent)
            doItem.CloneInternal(Me, bCutData, doRoot)
            If Not doItem Is Nothing AndAlso doItem Is Me Then doItem.AfterClone(Me, bCutData, doRoot, doItem)
            Return doItem
        End Function

        Public Overrides Sub AfterAddToList(Optional ByVal bThrowError As Boolean = True)
            MyBase.AfterAddToList(bThrowError)
            'stub for now
            'When a new materialType is added to the collection we need to create new materialpair objects for all the various combinations
            ' (you will need to make sure that you only do one combination pair
            'add the needed materialpair combinations. 
        End Sub

        'This is the new event that rigid bodies will subscribe to when they set the material type for themselves.
        'When this is fired they will replace this material type with the new one that is passed in. Within the 
        ' material editor window they will be able to delete a material type. When they do we will first open a new
        ' dialog to allow them to pick the new material. If they hit ok then we will then call ReplaceMaterial to signla
        ' this event to all subscribing objects.
        Public Event MaterialReplaced(ByVal doNewMaterial As MaterialType)

        'This method is called after the users have picked the new material to switch to using.
        Public Sub ReplaceMatierial(ByVal doNewMaterial As MaterialType)
            RaiseEvent MaterialReplaced(doNewMaterial)
        End Sub

#End Region

    End Class

End Namespace



'Each Rigidbody will have a material type associated with it. 
'It will hook into the add/remove events and when a material type is removed it will switch its material type to the replace material.

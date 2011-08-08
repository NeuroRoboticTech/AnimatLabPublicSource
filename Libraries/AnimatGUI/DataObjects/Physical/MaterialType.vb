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

        Public Overridable Sub AfterAddToList(Optional ByVal bThrowError As Boolean = True)
            'stub for now
            'When a new materialType is added to the collection we need to create new materialpair objects for all the various combinations
            ' (you will need to make sure that you only do one combination pair
            'add the needed materialpair combinations. 
        End Sub

        Public Overridable Sub AfterRemoveFromList(ByVal replaceMaterial As MaterialType, Optional ByVal bThrowError As Boolean = True)

        End Sub

        'Public Overridable Sub CopyData(ByVal mtMaterial As MaterialType)
        '    'copy attributes from argument into Me
        'End Sub

#End Region

    End Class

End Namespace



'Each Rigidbody will have a material type associated with it. 
'It will hook into the add/remove events and when a material type is removed it will switch its material type to the replace material.

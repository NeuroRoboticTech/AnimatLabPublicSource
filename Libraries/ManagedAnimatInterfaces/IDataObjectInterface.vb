
Public Interface IDataObjectInterface

#Region "Deletgates"

    Delegate Sub PositionChangedHandler()
    Delegate Sub RotationChangedHandler()
    Delegate Sub SelectionChangedHandler(ByVal bSelected As Boolean, ByVal bSelectMultiple As Boolean)
    Delegate Sub AddBodyClickedHandler(ByVal fltPosX As Single, ByVal fltPosY As Single, ByVal fltPosZ As Single, _
                                       ByVal fltNormX As Single, ByVal fltNormY As Single, ByVal fltNormZ As Single)
    Delegate Sub SelectedVertexChangedHandler(ByVal fltPosX As Single, ByVal fltPosY As Single, ByVal fltPosZ As Single)

#End Region


#Region "Properties"

    Property Position(ByVal iIndex As Integer) As Single
    Property WorldPosition(ByVal iIndex As Integer) As Single
    Property Rotation(ByVal iIndex As Integer) As Single

#End Region

#Region "Methods"

    Function SetData(ByVal sDataType As String, ByVal sValue As String, ByVal bThrowError As Boolean) As Boolean
    Sub SelectItem(ByVal bVal As Boolean, ByVal bSelectMultiple As Boolean)

    Sub GetDataPointer(ByVal sData As String)
    Function GetDataValue(ByVal sData As String) As Single
    Function GetDataValueImmediate(ByVal sData As String) As Single

    Function GetBoundingBoxValue(ByVal iIndex As Integer) As Single
    Sub OrientNewPart(ByVal dblXPos As Double, ByVal dblYPos As Double, ByVal dblZPos As Double, _
                      ByVal dblXNorm As Double, ByVal dblYNorm As Double, ByVal dblZNorm As Double)
    Function CalculateLocalPosForWorldPos(ByVal dblXWorldX As Double, ByVal dblWorldY As Double, ByVal dblWorldZ As Double, _
                                          ByVal aryLocalPos As System.Collections.ArrayList) As Boolean

#End Region


#Region "Events"

    Event OnPositionChanged As PositionChangedHandler
    Event OnRotationChanged As RotationChangedHandler
    Event OnSelectionChanged As SelectionChangedHandler
    Event OnAddBodyClicked As AddBodyClickedHandler
    Event OnSelectedVertexChanged As SelectedVertexChangedHandler

    Sub FirePositionChangedEvent()
    Sub FireRotationChangedEvent()
    Sub FireSelectionChangedEvent(ByVal bSelected As Boolean, ByVal bSelectMultiple As Boolean)
    Sub FireAddBodyClickedEvent(ByVal fltPosX As Single, ByVal fltPosY As Single, ByVal fltPosZ As Single, _
                                ByVal fltNormX As Single, ByVal fltNormY As Single, ByVal fltNormZ As Single)
    Sub FireSelectedVertexChangedEvent(ByVal fltPosX As Single, ByVal fltPosY As Single, ByVal fltPosZ As Single)

#End Region

End Interface

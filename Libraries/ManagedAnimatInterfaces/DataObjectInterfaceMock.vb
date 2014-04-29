
Public Class DataObjectInterfaceMock
    Implements IDataObjectInterface

    Public Sub New(ByVal SimInt As ISimulatorInterface, ByVal strID As String)

    End Sub

    Public Function CalculateLocalPosForWorldPos(dblXWorldX As Double, dblWorldY As Double, dblWorldZ As Double, aryLocalPos As System.Collections.ArrayList) As Boolean Implements IDataObjectInterface.CalculateLocalPosForWorldPos
        Return False
    End Function

    Public Sub FireAddBodyClickedEvent(fltPosX As Single, fltPosY As Single, fltPosZ As Single, fltNormX As Single, fltNormY As Single, fltNormZ As Single) Implements IDataObjectInterface.FireAddBodyClickedEvent

    End Sub

    Public Sub FirePositionChangedEvent() Implements IDataObjectInterface.FirePositionChangedEvent

    End Sub

    Public Sub FireRotationChangedEvent() Implements IDataObjectInterface.FireRotationChangedEvent

    End Sub

    Public Sub FireSelectedVertexChangedEvent(fltPosX As Single, fltPosY As Single, fltPosZ As Single) Implements IDataObjectInterface.FireSelectedVertexChangedEvent

    End Sub

    Public Sub FireSelectionChangedEvent(bSelected As Boolean, bSelectMultiple As Boolean) Implements IDataObjectInterface.FireSelectionChangedEvent

    End Sub

    Public Function GetBoundingBoxValue(iIndex As Integer) As Single Implements IDataObjectInterface.GetBoundingBoxValue
        Return 0
    End Function

    Public Sub GetDataPointer(sData As String) Implements IDataObjectInterface.GetDataPointer

    End Sub

    Public Function GetDataValue(sData As String) As Single Implements IDataObjectInterface.GetDataValue
        Return 0
    End Function

    Public Function GetDataValueImmediate(sData As String) As Single Implements IDataObjectInterface.GetDataValueImmediate

        If sData.ToUpper = "TIMESTEP" Then
            Return 0.02F
        ElseIf sData.ToUpper() = "PHYSICSTIMESTEP" Then
            Return 0.02F
        Else
            Return 0
        End If
    End Function

    Public Event OnAddBodyClicked(fltPosX As Single, fltPosY As Single, fltPosZ As Single, fltNormX As Single, fltNormY As Single, fltNormZ As Single) Implements IDataObjectInterface.OnAddBodyClicked

    Public Event OnPositionChanged() Implements IDataObjectInterface.OnPositionChanged

    Public Event OnRotationChanged() Implements IDataObjectInterface.OnRotationChanged

    Public Event OnSelectedVertexChanged(fltPosX As Single, fltPosY As Single, fltPosZ As Single) Implements IDataObjectInterface.OnSelectedVertexChanged

    Public Event OnSelectionChanged(bSelected As Boolean, bSelectMultiple As Boolean) Implements IDataObjectInterface.OnSelectionChanged

    Public Sub OrientNewPart(dblXPos As Double, dblYPos As Double, dblZPos As Double, dblXNorm As Double, dblYNorm As Double, dblZNorm As Double) Implements IDataObjectInterface.OrientNewPart

    End Sub

    Public Property Position(iIndex As Integer) As Single Implements IDataObjectInterface.Position
        Get
            Return 0
        End Get
        Set(value As Single)

        End Set
    End Property

    Public Property Rotation(iIndex As Integer) As Single Implements IDataObjectInterface.Rotation
        Get
            Return 0
        End Get
        Set(value As Single)

        End Set
    End Property

    Public Sub SelectItem(bVal As Boolean, bSelectMultiple As Boolean) Implements IDataObjectInterface.SelectItem

    End Sub

    Public Function SetData(sDataType As String, sValue As String, bThrowError As Boolean) As Boolean Implements IDataObjectInterface.SetData
        Return False
    End Function

    Public Sub QueryProperties(ByVal aryPropertyNames As System.Collections.ArrayList, ByVal aryPropertyTypes As System.Collections.ArrayList, ByVal aryDirections As System.Collections.ArrayList) Implements IDataObjectInterface.QueryProperties

    End Sub

    Public Property WorldPosition(iIndex As Integer) As Single Implements IDataObjectInterface.WorldPosition
        Get
            Return 0
        End Get
        Set(value As Single)

        End Set
    End Property

    Public Sub EnableCollisions(ByVal strOtherBodyID As String) Implements IDataObjectInterface.EnableCollisions

    End Sub

    Public Sub DisableCollisions(ByVal strOtherBodyID As String) Implements IDataObjectInterface.DisableCollisions

    End Sub

    Public Function GetLocalTransformMatrixString() As String Implements IDataObjectInterface.GetLocalTransformMatrixString
        Return ""
    End Function

End Class

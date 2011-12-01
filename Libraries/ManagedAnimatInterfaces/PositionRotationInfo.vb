
Public Class PositionRotationInfo

    Public m_fltXPos As Single
    Public m_fltYPos As Single
    Public m_fltZPos As Single

    Public m_fltXRot As Single
    Public m_fltYRot As Single
    Public m_fltZRot As Single

    Public Sub New()

    End Sub

    Public Sub New(ByVal fltXPos As Single, ByVal fltYPos As Single, ByVal fltZPos As Single, _
                   ByVal fltXRot As Single, ByVal fltYRot As Single, ByVal fltZRot As Single)
        m_fltXPos = fltXPos
        m_fltYPos = fltYPos
        m_fltZPos = fltZPos

        m_fltXRot = fltXRot
        m_fltYRot = fltYRot
        m_fltZRot = fltZRot
    End Sub

End Class

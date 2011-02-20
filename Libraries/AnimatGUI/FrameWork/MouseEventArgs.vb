Imports System

Namespace Framework
    Public Class MouseEventArgs
        Inherits Windows.Forms.MouseEventArgs

#Region " Attributes "

        Protected m_iOldX As Integer
        Protected m_iOldY As Integer
        Protected m_iDeltaX As Integer
        Protected m_iDeltaY As Integer
        Protected m_bShift As Boolean
        Protected m_bControl As Boolean
        Protected m_bXDown As Boolean
        Protected m_bYDown As Boolean
        Protected m_bZDown As Boolean
        Protected m_bSDown As Boolean
        Protected m_fltScale As Single

#End Region

#Region " Properties "
        Public ReadOnly Property OldX() As Integer
            Get
                Return m_iOldX
            End Get
        End Property
        Public ReadOnly Property OldY() As Integer
            Get
                Return m_iOldY
            End Get
        End Property
        Public ReadOnly Property DeltaX() As Integer
            Get
                Return m_iDeltaX
            End Get
        End Property
        Public ReadOnly Property DeltaY() As Integer
            Get
                Return m_iDeltaY
            End Get
        End Property
        Public ReadOnly Property Shift() As Boolean
            Get
                Return m_bShift
            End Get
        End Property
        Public ReadOnly Property Control() As Boolean
            Get
                Return m_bControl
            End Get
        End Property
        Public ReadOnly Property XKey() As Boolean
            Get
                Return m_bXDown
            End Get
        End Property
        Public ReadOnly Property YKey() As Boolean
            Get
                Return m_bYDown
            End Get
        End Property
        Public ReadOnly Property ZKey() As Boolean
            Get
                Return m_bZDown
            End Get
        End Property
        Public ReadOnly Property SKey() As Boolean
            Get
                Return m_bSDown
            End Get
        End Property
        Public ReadOnly Property Scale() As Single
            Get
                Return m_fltScale
            End Get
        End Property

#End Region

        Sub New(ByVal e As System.Windows.Forms.MouseEventArgs, ByVal oldX As Integer, ByVal oldY As Integer, ByVal shiftDown As Boolean, ByVal controlDown As Boolean, ByVal bXDown As Boolean, ByVal bYDown As Boolean, ByVal bZDown As Boolean, ByVal bSDown As Boolean, ByVal fltScale As Single)
            MyBase.New(e.Button, e.Clicks, e.X, e.Y, e.Delta)
            m_iOldX = oldX
            m_iOldY = oldY
            m_iDeltaX = e.X - oldX
            m_iDeltaY = e.Y - oldY
            m_bShift = shiftDown
            m_bControl = controlDown
            m_bXDown = bXDown
            m_bYDown = bYDown
            m_bZDown = bZDown
            m_bSDown = bSDown
            m_fltScale = fltScale
        End Sub

    End Class
End Namespace


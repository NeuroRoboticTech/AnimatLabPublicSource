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

Namespace Framework.UndoSystem

    Public Class ScaleUnitChangedEvent
        Inherits AnimatGUI.Framework.UndoSystem.HistoryEvent

#Region " Attributes "

        Protected m_ePrevMassUnits As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits
        Protected m_eNewMassUnits As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits
        Protected m_ePrevDistanceUnits As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits
        Protected m_eNewDistanceUnits As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits

#End Region

#Region " Properties "

        Public Overridable Property PrevMassUnits() As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits
            Get
                Return m_eNewMassUnits
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits)
                m_eNewMassUnits = Value
            End Set
        End Property

        Public Overridable Property NewMassUnits() As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits
            Get
                Return m_ePrevMassUnits
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits)
                m_ePrevMassUnits = Value
            End Set
        End Property

        Public Overridable Property PrevDistanceUnits() As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits
            Get
                Return m_ePrevDistanceUnits
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits)
                m_ePrevDistanceUnits = Value
            End Set
        End Property

        Public Overridable Property NewDistanceUnits() As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits
            Get
                Return m_eNewDistanceUnits
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits)
                m_eNewDistanceUnits = Value
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal frmParent As Windows.Forms.Form, _
                        ByVal ePrevMass As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits, _
                        ByVal eNewMass As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits, _
                        ByVal ePrevDistance As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits, _
                        ByVal eNewDistance As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits)
            MyBase.New(frmParent)

            m_eNewMassUnits = ePrevMass
            m_eNewMassUnits = eNewMass
            m_ePrevDistanceUnits = ePrevDistance
            m_eNewDistanceUnits = eNewDistance
        End Sub

        Public Overrides Sub Undo()
            Try
                Util.ModificationHistory.AllowAddHistory = False
                Util.Application.UndoRedoInProgress = True
                Util.Application.ChangeUnits(m_ePrevMassUnits, m_ePrevDistanceUnits)
            Catch ex As System.Exception
                Throw ex
            Finally
                Util.Application.UndoRedoInProgress = False
                Util.ModificationHistory.AllowAddHistory = True
            End Try
        End Sub

        Public Overrides Sub Redo()
            Try
                Util.ModificationHistory.AllowAddHistory = False
                Util.Application.UndoRedoInProgress = True
                Util.Application.ChangeUnits(m_eNewMassUnits, m_eNewDistanceUnits)
            Catch ex As System.Exception
                Throw ex
            Finally
                Util.Application.UndoRedoInProgress = False
                Util.ModificationHistory.AllowAddHistory = True
            End Try
        End Sub

#End Region

    End Class

End Namespace


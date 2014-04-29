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

        Public MustInherit Class MotorControlSystem
            Inherits RobotPartInterface

#Region " Attributes "

            Protected m_iServoID As Integer = 1

#End Region

#Region " Properties "

            Public Overridable Property ServoID As Integer
                Get
                    Return m_iServoID
                End Get
                Set(value As Integer)
                    If value <= 0 Then
                        Throw New System.Exception("Invalid servo ID specified. ServoID: " & value)
                    End If
                    SetSimData("ServoID", value.ToString(), True)
                    m_iServoID = value
                End Set
            End Property

#End Region

#Region " Methods "

            Public Sub New(ByVal doParent As Framework.DataObject)
                MyBase.New(doParent)

                m_strName = "MotorControlSystem"
            End Sub

            Public Overrides Sub ClearIsDirty()
                MyBase.ClearIsDirty()
            End Sub

            Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                                ByVal doRoot As AnimatGUI.Framework.DataObject)
                MyBase.CloneInternal(doOriginal, bCutData, doRoot)

                Dim OrigNode As MotorControlSystem = DirectCast(doOriginal, MotorControlSystem)

                m_iServoID = OrigNode.m_iServoID
            End Sub

            Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

                MyBase.BuildProperties(propTable)

                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Servo ID", Me.ServoID.GetType(), "ServoID", _
                                            "Properties", "ServoID", Me.ServoID))

            End Sub

            Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
                MyBase.LoadData(oXml)

                oXml.IntoElem()  'Into RobotInterface Element
                m_iServoID = oXml.GetChildInt("ServoID", m_iServoID)
                oXml.OutOfElem()

            End Sub


            Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
                MyBase.SaveData(oXml)

                oXml.IntoElem()
                oXml.AddChildElement("ServoID", m_iServoID)
                oXml.OutOfElem()

            End Sub

            Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")
                MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

                oXml.IntoElem()
                oXml.AddChildElement("ServoID", m_iServoID)
                oXml.OutOfElem()

            End Sub

#End Region

        End Class


    End Namespace
End Namespace

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
            Inherits OutputSystem

#Region " Attributes "

#End Region

#Region " Properties "

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
            End Sub

            Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

                MyBase.BuildProperties(propTable)

                'This is not used for motor systems. We already know what we are connecting with.
                If propTable.Properties.Contains("Linked Property") Then propTable.Properties.Remove("Linked Property")
                If propTable.Properties.Contains("Gain") Then propTable.Properties.Remove("Gain")

                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Servo ID", Me.IOComponentID.GetType(), "IOComponentID", _
                                            "Properties", "ID of the servo to move for this part.", Me.IOComponentID))

            End Sub

            Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
                MyBase.LoadData(oXml)

                'oXml.IntoElem()  'Into RobotInterface Element
                'oXml.OutOfElem()

            End Sub


            Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
                MyBase.SaveData(oXml)

                'oXml.IntoElem()
                'oXml.OutOfElem()

            End Sub

            Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")
                MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

                'oXml.IntoElem()
                'oXml.OutOfElem()

            End Sub

#End Region

        End Class


    End Namespace
End Namespace

Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports Crownwood.Magic.Common
Imports AnimatGuiCtrls.Controls
Imports Crownwood.Magic.Docking
Imports Crownwood.Magic.Menus
Imports AnimatGUI.Framework

Namespace DataObjects.ExternalStimuli

    Public Class Force
        Inherits AnimatGUI.DataObjects.ExternalStimuli.BodyPartStimulus

#Region " Attributes "

        Dim m_snForceX As ScaledNumber
        Dim m_snForceY As ScaledNumber
        Dim m_snForceZ As ScaledNumber

        Dim m_snTorqueX As ScaledNumber
        Dim m_snTorqueY As ScaledNumber
        Dim m_snTorqueZ As ScaledNumber

        Dim m_snPositionX As ScaledNumber
        Dim m_snPositionY As ScaledNumber
        Dim m_snPositionZ As ScaledNumber

#End Region

#Region " Properties "

        Public Overridable Property ForceX() As ScaledNumber
            Get
                Return m_snForceX
            End Get
            Set(ByVal Value As ScaledNumber)
                If Not Value Is Nothing Then
                    SetSimData("ForceX", Value.ActualValue.ToString, True)
                    m_snForceX.CopyData(Value)
                End If
            End Set
        End Property

        Public Overridable Property ForceY() As ScaledNumber
            Get
                Return m_snForceY
            End Get
            Set(ByVal Value As ScaledNumber)
                If Not Value Is Nothing Then
                    SetSimData("ForceY", Value.ActualValue.ToString, True)
                    m_snForceY.CopyData(Value)
                End If
            End Set
        End Property

        Public Overridable Property ForceZ() As ScaledNumber
            Get
                Return m_snForceZ
            End Get
            Set(ByVal Value As ScaledNumber)
                If Not Value Is Nothing Then
                    SetSimData("ForceZ", Value.ActualValue.ToString, True)
                    m_snForceZ.CopyData(Value)
                End If
            End Set
        End Property

        Public Overridable Property TorqueX() As ScaledNumber
            Get
                Return m_snTorqueX
            End Get
            Set(ByVal Value As ScaledNumber)
                If Not Value Is Nothing Then
                    SetSimData("TorqueX", Value.ActualValue.ToString, True)
                    m_snTorqueX.CopyData(Value)
                End If
            End Set
        End Property

        Public Overridable Property TorqueY() As ScaledNumber
            Get
                Return m_snTorqueY
            End Get
            Set(ByVal Value As ScaledNumber)
                If Not Value Is Nothing Then
                    SetSimData("TorqueY", Value.ActualValue.ToString, True)
                    m_snTorqueY.CopyData(Value)
                End If
            End Set
        End Property

        Public Overridable Property TorqueZ() As ScaledNumber
            Get
                Return m_snTorqueZ
            End Get
            Set(ByVal Value As ScaledNumber)
                If Not Value Is Nothing Then
                    SetSimData("TorqueZ", Value.ActualValue.ToString, True)
                    m_snTorqueZ.CopyData(Value)
                End If
            End Set
        End Property

        Public Overridable Property PositionX() As ScaledNumber
            Get
                Return m_snPositionX
            End Get
            Set(ByVal Value As ScaledNumber)
                If Not Value Is Nothing Then
                    SetSimData("PositionX", Value.ActualValue.ToString, True)
                    m_snPositionX.SetFromValue(Value.ActualValue, Util.Environment.DistanceUnits)
                End If
            End Set
        End Property

        Public Overridable Property PositionY() As ScaledNumber
            Get
                Return m_snPositionY
            End Get
            Set(ByVal Value As ScaledNumber)
                If Not Value Is Nothing Then
                    SetSimData("PositionY", Value.ActualValue.ToString, True)
                    m_snPositionY.SetFromValue(Value.ActualValue, Util.Environment.DistanceUnits)
                End If
            End Set
        End Property

        Public Overridable Property PositionZ() As ScaledNumber
            Get
                Return m_snPositionZ
            End Get
            Set(ByVal Value As ScaledNumber)
                If Not Value Is Nothing Then
                    SetSimData("PositionZ", Value.ActualValue.ToString, True)
                    m_snPositionZ.SetFromValue(Value.ActualValue, Util.Environment.DistanceUnits)
                End If
            End Set
        End Property

        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Force"
            End Get
        End Property

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.ForceStimulus.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property Description() As String
            Get
                Return "This stimulus applies a force or torque to a body part in the structure."
            End Get
        End Property

        Public Overrides ReadOnly Property StimulusClassType() As String
            Get
                Return "ForceInput"
            End Get
        End Property

        Public Overrides ReadOnly Property DragImageName() As String
            Get
                Return "AnimatGUI.ForceStimulus_Large.gif"
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            m_snForceX = New ScaledNumber(Me, "ForceX", 0, ScaledNumber.enumNumericScale.None, "Netwons", "N")
            m_snForceY = New ScaledNumber(Me, "ForceY", 0, ScaledNumber.enumNumericScale.None, "Netwons", "N")
            m_snForceZ = New ScaledNumber(Me, "ForceZ", 0, ScaledNumber.enumNumericScale.None, "Netwons", "N")

            m_snTorqueX = New ScaledNumber(Me, "TorqueX", 0, ScaledNumber.enumNumericScale.None, "Netwon-Meters", "Nm")
            m_snTorqueY = New ScaledNumber(Me, "TorqueY", 0, ScaledNumber.enumNumericScale.None, "Netwon-Meters", "Nm")
            m_snTorqueZ = New ScaledNumber(Me, "TorqueZ", 0, ScaledNumber.enumNumericScale.None, "Netwon-Meters", "Nm")

            m_snPositionX = New ScaledNumber(Me, "PositionX", 0, ScaledNumber.enumNumericScale.None, "meters", "m")
            m_snPositionY = New ScaledNumber(Me, "PositionY", 0, ScaledNumber.enumNumericScale.None, "meters", "m")
            m_snPositionZ = New ScaledNumber(Me, "PositionZ", 0, ScaledNumber.enumNumericScale.None, "meters", "m")

            If Not Util.Environment Is Nothing Then
                m_snPositionX.SetFromValue(0, Util.Environment.DistanceUnits)
                m_snPositionY.SetFromValue(0, Util.Environment.DistanceUnits)
                m_snPositionZ.SetFromValue(0, Util.Environment.DistanceUnits)
            End If

        End Sub

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doPart As DataObjects.ExternalStimuli.Force = DirectCast(doOriginal, DataObjects.ExternalStimuli.Force)

            m_snForceX = DirectCast(doPart.m_snForceX.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snForceY = DirectCast(doPart.m_snForceY.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snForceZ = DirectCast(doPart.m_snForceZ.Clone(Me, bCutData, doRoot), ScaledNumber)

            m_snTorqueX = DirectCast(doPart.m_snTorqueX.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snTorqueY = DirectCast(doPart.m_snTorqueY.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snTorqueZ = DirectCast(doPart.m_snTorqueZ.Clone(Me, bCutData, doRoot), ScaledNumber)

            m_snPositionX = DirectCast(doPart.m_snPositionX.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snPositionY = DirectCast(doPart.m_snPositionY.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snPositionZ = DirectCast(doPart.m_snPositionZ.Clone(Me, bCutData, doRoot), ScaledNumber)

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim doStim As DataObjects.ExternalStimuli.Force = New DataObjects.ExternalStimuli.Force(doParent)
            CloneInternal(doStim, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then doStim.AfterClone(Me, bCutData, doRoot, doStim)
            Return doStim
        End Function

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            m_snForceX.ClearIsDirty()
            m_snForceY.ClearIsDirty()
            m_snForceZ.ClearIsDirty()

            m_snTorqueX.ClearIsDirty()
            m_snTorqueY.ClearIsDirty()
            m_snTorqueZ.ClearIsDirty()

            m_snPositionX.ClearIsDirty()
            m_snPositionY.ClearIsDirty()
            m_snPositionZ.ClearIsDirty()

        End Sub

        Public Overrides Function GetSimulationXml(ByVal strName As String, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing) As String

            If m_doStructure Is Nothing Then
                Throw New System.Exception("No structure was defined for the stimulus '" & m_strName & "'.")
            End If

            If m_doBodyPart Is Nothing Then
                Throw New System.Exception("No bodypart was defined for the stimulus '" & m_strName & "'.")
            End If

            Dim oXml As New AnimatGUI.Interfaces.StdXml
            oXml.AddElement("Root")
            SaveSimulationXml(oXml, nmParentControl, strName)

            Return oXml.Serialize()
        End Function

        Public Overrides Sub SaveSimulationXml(ByRef oXml As AnimatGUI.Interfaces.StdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            If m_doStructure Is Nothing Then
                Throw New System.Exception("No structure was defined for the stimulus '" & m_strName & "'.")
            End If

            If m_doBodyPart Is Nothing Then
                Throw New System.Exception("No bodypart was defined for the stimulus '" & m_strName & "'.")
            End If

            oXml.AddChildElement("Stimulus")

            oXml.IntoElem()
            oXml.AddChildElement("ID", m_strID)
            oXml.AddChildElement("Name", m_strName)
            oXml.AddChildElement("AlwaysActive", m_bAlwaysActive)

            oXml.AddChildElement("ModuleName", Me.StimulusModuleName)
            oXml.AddChildElement("Type", Me.StimulusClassType)

            oXml.AddChildElement("StructureID", m_doStructure.ID)
            oXml.AddChildElement("BodyID", m_doBodyPart.ID)

            oXml.AddChildElement("StartTime", m_snStartTime.ActualValue)
            oXml.AddChildElement("EndTime", m_snEndTime.ActualValue)

            Util.SaveVector(oXml, "RelativePosition", New Vec3d(Nothing, m_snPositionX.ActualValue, m_snPositionY.ActualValue, m_snPositionZ.ActualValue))

            oXml.AddChildElement("ForceX", m_snForceX.ActualValue)
            oXml.AddChildElement("ForceY", m_snForceY.ActualValue)
            oXml.AddChildElement("ForceZ", m_snForceZ.ActualValue)

            oXml.AddChildElement("TorqueX", m_snTorqueX.ActualValue)
            oXml.AddChildElement("TorqueY", m_snTorqueY.ActualValue)
            oXml.AddChildElement("TorqueZ", m_snTorqueZ.ActualValue)

            oXml.OutOfElem()

        End Sub

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = m_snPositionX.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("X", pbNumberBag.GetType(), "PositionX", _
                                        "Location", "Sets the x location where the force will be applied on the body relative to the center of the body.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = Me.m_snPositionY.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Y", pbNumberBag.GetType(), "PositionY", _
                                        "Location", "Sets the y location where the force will be applied on the body relative to the center of the body.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = Me.m_snPositionZ.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Z", pbNumberBag.GetType(), "PositionZ", _
                                        "Location", "Sets the z location where the force will be applied on the body relative to the center of the body.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))


            pbNumberBag = Me.m_snForceX.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("X", pbNumberBag.GetType(), "ForceX", _
                                        "Force", "Sets the x value of the force vector that will be applied to the body.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = Me.m_snForceY.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Y", pbNumberBag.GetType(), "ForceY", _
                                        "Force", "Sets the y value of the force vector that will be applied to the body.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = Me.m_snForceZ.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Z", pbNumberBag.GetType(), "ForceZ", _
                                        "Force", "Sets the z value of the force vector that will be applied to the body.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))


            pbNumberBag = Me.m_snTorqueX.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("X", pbNumberBag.GetType(), "TorqueX", _
                                        "Torque", "Sets the x value of the torque vector that will be applied to the body.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = Me.m_snTorqueY.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Y", pbNumberBag.GetType(), "TorqueY", _
                                        "Torque", "Sets the y value of the torque vector that will be applied to the body.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = Me.m_snTorqueZ.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Z", pbNumberBag.GetType(), "TorqueZ", _
                                        "Torque", "Sets the z value of the torque vector that will be applied to the body.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))


        End Sub

        Public Overrides Sub UnitsChanged(ByVal ePrevMass As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits, _
                                          ByVal eNewMass As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits, _
                                          ByVal fltMassChange As Single, _
                                          ByVal ePrevDistance As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits, _
                                          ByVal eNewDistance As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits, _
                                          ByVal fltDistanceChange As Single)

            ''Me.PositionX = New ScaledNumber(Me, (m_snPositionX.ActualValue * fltDistanceChange), eNewDistance)
            ''Me.PositionY = New ScaledNumber(Me, (m_snPositionX.ActualValue * fltDistanceChange), eNewDistance)
            ''Me.PositionZ = New ScaledNumber(Me, (m_snPositionX.ActualValue * fltDistanceChange), eNewDistance)

            ''m_snPositionX.SetFromValue(, )
            'm_snPositionY.SetFromValue(m_snPositionY.ActualValue * fltDistanceChange, eNewDistance)
            'm_snPositionZ.SetFromValue(m_snPositionZ.ActualValue * fltDistanceChange, eNewDistance)

            'm_snForceX.ActualValue = m_snForceX.ActualValue * fltMassChange * fltDistanceChange
            'm_snForceY.ActualValue = m_snForceY.ActualValue * fltMassChange * fltDistanceChange
            'm_snForceZ.ActualValue = m_snForceZ.ActualValue * fltMassChange * fltDistanceChange

            'm_snTorqueX.ActualValue = m_snTorqueX.ActualValue * fltMassChange * fltDistanceChange
            'm_snTorqueY.ActualValue = m_snTorqueY.ActualValue * fltMassChange * fltDistanceChange
            'm_snTorqueZ.ActualValue = m_snTorqueZ.ActualValue * fltMassChange * fltDistanceChange

        End Sub

        Public Overrides Sub LoadData(ByRef oXml As AnimatGUI.Interfaces.StdXml)
            MyBase.LoadData(oXml)

            oXml.IntoElem()

            If oXml.FindChildElement("ForceX", False) Then
                m_snPositionX.LoadData(oXml, "PositionX")
                m_snPositionY.LoadData(oXml, "PositionY")
                m_snPositionZ.LoadData(oXml, "PositionZ")

                m_snForceX.LoadData(oXml, "ForceX")
                m_snForceY.LoadData(oXml, "ForceY")
                m_snForceZ.LoadData(oXml, "ForceZ")

                m_snTorqueX.LoadData(oXml, "TorqueX")
                m_snTorqueY.LoadData(oXml, "TorqueY")
                m_snTorqueZ.LoadData(oXml, "TorqueZ")
            Else
                Dim vForce As Vec3d = Util.LoadVec3d(oXml, "Force", Me)
                Dim vTorque As Vec3d = Util.LoadVec3d(oXml, "Torque", Me)

                m_snForceX.ActualValue = vForce.X
                m_snForceY.ActualValue = vForce.Y
                m_snForceZ.ActualValue = vForce.Z

                m_snTorqueX.ActualValue = vTorque.X
                m_snTorqueY.ActualValue = vTorque.Y
                m_snTorqueZ.ActualValue = vTorque.Z
            End If

            oXml.OutOfElem()

            Me.IsDirty = False
        End Sub

        Public Overrides Sub SaveData(ByRef oXml As AnimatGUI.Interfaces.StdXml)
            MyBase.SaveData(oXml)

            oXml.IntoElem()

            m_snPositionX.SaveData(oXml, "PositionX")
            m_snPositionY.SaveData(oXml, "PositionY")
            m_snPositionZ.SaveData(oXml, "PositionZ")

            m_snForceX.SaveData(oXml, "ForceX")
            m_snForceY.SaveData(oXml, "ForceY")
            m_snForceZ.SaveData(oXml, "ForceZ")

            m_snTorqueX.SaveData(oXml, "TorqueX")
            m_snTorqueY.SaveData(oXml, "TorqueY")
            m_snTorqueZ.SaveData(oXml, "TorqueZ")

            oXml.OutOfElem() ' Outof Node Element

        End Sub

#End Region

#End Region

    End Class

End Namespace

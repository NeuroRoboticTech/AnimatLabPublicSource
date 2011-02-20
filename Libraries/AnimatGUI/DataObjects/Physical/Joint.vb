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

Namespace DataObjects.Physical

    Public MustInherit Class Joint
        Inherits Physical.BodyPart

#Region " Attributes "

        Protected m_bEnableLimits As Boolean = True
        Protected m_snDamping As ScaledNumber
        Protected m_snRestitution As ScaledNumber
        Protected m_snStiffness As ScaledNumber

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property BodyPartType() As String
            Get
                Return "Joint"
            End Get
        End Property

        Public Overrides ReadOnly Property DefaultVisualSelectionMode() As Simulation.enumVisualSelectionMode
            Get
                Return Simulation.enumVisualSelectionMode.SelectJoints
            End Get
        End Property

        Public Overridable ReadOnly Property UsesRadians() As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overridable ReadOnly Property ScaleUnits() As String
            Get
                If Me.UsesRadians Then
                    Return "rad/s"
                Else
                    Return "m/s"
                End If
            End Get
        End Property

        Public Overridable ReadOnly Property InputStimulus() As String
            Get
                Return "Velocity"
            End Get
        End Property

        Public Overridable Property EnableLimits() As Boolean
            Get
                Return m_bEnableLimits
            End Get
            Set(ByVal Value As Boolean)
                SetSimData("EnableLimits", m_bEnableLimits.ToString, True)
                m_bEnableLimits = Value
            End Set
        End Property

        Public Overridable Property Damping() As ScaledNumber
            Get
                Return m_snDamping
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The damping can not be less than zero.")
                End If

                SetSimData("Damping", m_snDamping.ActualValue.ToString, True)
                m_snDamping.CopyData(Value)
            End Set
        End Property

        Public Overridable Property Restitution() As ScaledNumber
            Get
                Return m_snRestitution
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 OrElse Value.ActualValue > 1 Then
                    Throw New System.Exception("The restitution must be between 0 and 1.")
                End If

                SetSimData("Restitution", m_snRestitution.ActualValue.ToString, True)
                m_snRestitution.CopyData(Value)
            End Set
        End Property

        Public Overridable Property Stiffness() As ScaledNumber
            Get
                Return m_snStiffness
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The stiffness can not be less than zero.")
                End If

                SetSimData("Stiffness", m_snStiffness.ActualValue.ToString, True)
                m_snStiffness.CopyData(Value)
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            m_thIncomingDataType = New AnimatGUI.DataObjects.DataType("DesiredVelocity", "Desired Velocity", "m/s", "m/s", -5, 5, ScaledNumber.enumNumericScale.None, ScaledNumber.enumNumericScale.None)

            m_snStiffness = New AnimatGUI.Framework.ScaledNumber(Me, "Stiffness", 5, ScaledNumber.enumNumericScale.Mega, "N/m", "N/m")
            m_snDamping = New AnimatGUI.Framework.ScaledNumber(Me, "Damping", 0, ScaledNumber.enumNumericScale.Kilo, "g/s", "g/s")
            m_snRestitution = New AnimatGUI.Framework.ScaledNumber(Me, "Restitution", 0, ScaledNumber.enumNumericScale.None, "v/v", "v/v")

            'Now we need to setup handlers for the parent and child move and rotate events so we can tell the joint object 
            'in the simulation that these changes have occured. It will need to re-adjust the joint to take this into account.
            If Not doParent Is Nothing AndAlso Util.IsTypeOf(doParent.GetType, GetType(DataObjects.Physical.BodyPart)) Then
                Dim bpChild As DataObjects.Physical.BodyPart = DirectCast(doParent, DataObjects.Physical.BodyPart)
                AddHandler bpChild.PartMoved, AddressOf Me.OnAttachedPartMoved
                AddHandler bpChild.PartRotated, AddressOf Me.OnAttachedPartRotated

                If Not bpChild.Parent Is Nothing AndAlso Util.IsTypeOf(bpChild.Parent.GetType, GetType(DataObjects.Physical.BodyPart)) Then
                    Dim bpParent As DataObjects.Physical.BodyPart = DirectCast(bpChild.Parent, DataObjects.Physical.BodyPart)
                    AddHandler bpParent.PartMoved, AddressOf Me.OnAttachedPartMoved
                    AddHandler bpParent.PartRotated, AddressOf Me.OnAttachedPartRotated
                End If
            End If

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            m_snStiffness.ClearIsDirty()
            m_snDamping.ClearIsDirty()
            m_snRestitution.ClearIsDirty()
        End Sub

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doOrigJoint As Joint = DirectCast(doOriginal, Joint)

            m_bEnableLimits = doOrigJoint.m_bEnableLimits
            m_snStiffness = DirectCast(doOrigJoint.m_snStiffness, ScaledNumber)
            m_snDamping = DirectCast(doOrigJoint.m_snDamping, ScaledNumber)
            m_snRestitution = DirectCast(doOrigJoint.m_snRestitution, ScaledNumber)

        End Sub

        Public Overrides Function CreateJointTreeView(ByRef tvTree As TreeView, ByVal tnParent As TreeNode, _
                                                      ByVal thSelectedPart As TypeHelpers.LinkedBodyPart) As TreeNode

            Dim tnJoint As TreeNode
            tnJoint = tnParent.Nodes.Add(Me.Name)
            tnJoint.ForeColor = Color.Red
            Dim thPart As TypeHelpers.LinkedBodyPart = DirectCast(thSelectedPart.Clone(thSelectedPart.Parent, False, Nothing), TypeHelpers.LinkedBodyPart)
            thPart.BodyPart = Me
            tnJoint.Tag = thPart

            Return tnJoint
        End Function

        Public Overrides Function FindBodyPart(ByVal strID As String) As BodyPart

            If Me.ID = strID Then
                Return Me
            Else
                Return Nothing
            End If

        End Function

        Public Overrides Function FindBodyPartByName(ByVal strName As String) As BodyPart

            If Me.Name = strName Then
                Return Me
            Else
                Return Nothing
            End If

        End Function

        Public Overrides Function FindBodyPartByCloneID(ByVal strId As String) As BodyPart

            If Me.CloneID = strId Then
                Return Me
            Else
                Return Nothing
            End If

        End Function

        Public Overrides Sub RenameBodyParts(ByVal doStructure As AnimatGUI.DataObjects.Physical.PhysicalStructure)

            Me.m_strID = System.Guid.NewGuid.ToString()

            Try
                doStructure.NewJointIndex = doStructure.NewJointIndex + 1
                Me.Name = "Joint_" & doStructure.NewJointIndex
            Catch ex As System.Exception
                Me.Name = "Joint_" & System.Guid.NewGuid.ToString()
            End Try
        End Sub

        Public Overrides Sub ClearSelectedBodyParts()
            m_bSelected = False
        End Sub

        Public Overrides Function SwapBodyPartList() As AnimatGUI.Collections.BodyParts

            'Go through the list and only use body parts that allow dynamics
            Dim aryPartList As New AnimatGUI.Collections.BodyParts(Nothing)
            For Each doPart As DataObjects.Physical.BodyPart In Util.Application.JointTypes
                If doPart.HasDynamics Then
                    aryPartList.Add(doPart)
                End If
            Next

            Return aryPartList
        End Function

        Public Overrides Sub SwapBodyPartCopy(ByVal doOriginal As AnimatGUI.DataObjects.Physical.BodyPart)

            Dim doExisting As AnimatGUI.DataObjects.Physical.Joint = DirectCast(doOriginal, AnimatGUI.DataObjects.Physical.Joint)

            Me.Name = doExisting.Name
            Me.ID = doExisting.ID
            Me.Description = doExisting.Description
            m_bpBodyNode = doOriginal.BodyTreeNode

            Util.Application.WorkspaceImages.AddImage(Me.WorkspaceImageName)

            m_bpBodyNode.ImageIndex = Util.Application.WorkspaceImages.GetImageIndex(Me.WorkspaceImageName)
            m_bpBodyNode.SelectedImageIndex = Util.Application.WorkspaceImages.GetImageIndex(Me.WorkspaceImageName)
            m_bpBodyNode.Tag = Me

        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Enable Limits", m_bEnableLimits.GetType(), "EnableLimits", _
                                        "Constraints", "Enables or disables the joint limit constraints.", m_bEnableLimits))

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = m_snDamping.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Damping", pbNumberBag.GetType(), "Damping", _
                                        "Constraints", "The damping term for this limit. If the stiffness and damping " & _
                                        "of an individual limit are both zero, it is effectively deactivated. This is the damping " & _
                                        "of the virtual spring used when the joint reaches its limit. It is not frictional damping " & _
                                        "for the motion around the joint.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snRestitution.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Restitution", pbNumberBag.GetType(), "Restitution", _
                                        "Constraints", "The coefficient of restitution is the ratio of rebound velocity to " & _
                                        "impact velocity when the joint reaches the low or high stop. This is used if the limit stiffness " & _
                                        "is greater than zero. Restitution must be in the range zero to one inclusive.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snStiffness.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Stiffness", pbNumberBag.GetType(), "Stiffness", _
                                        "Constraints", "The spring constant is used for restitution force when a limited " & _
                                        "joint reaches one of its stops. This limit property must be zero or positive. " & _
                                        "If the stiffness and damping of an individual limit are both zero, it is effectively deactivated.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

        End Sub

        Public Overloads Overrides Sub LoadData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByRef oXml As Interfaces.StdXml)

            MyBase.LoadData(doStructure, oXml)

            oXml.IntoElem() 'Into BodyPart Element

            If oXml.FindChildElement("Constraint", False) Then
                oXml.IntoElem()

                If oXml.FindChildElement("EnableLimits", False) Then
                    m_bEnableLimits = oXml.GetChildBool("EnableLimits")
                    m_snDamping.LoadData(oXml, "Damping")
                    m_snRestitution.LoadData(oXml, "Restitution")
                    m_snStiffness.LoadData(oXml, "Stiffness")
                End If

                oXml.OutOfElem()
            End If

            oXml.OutOfElem() 'Outof BodyPart Element

        End Sub

        Public Overloads Overrides Sub SaveData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByRef oXml As Interfaces.StdXml)

            MyBase.SaveData(doStructure, oXml)

            oXml.IntoElem() 'Into BodyPart Element

            oXml.AddChildElement("Constraint")
            oXml.IntoElem()
            oXml.AddChildElement("EnableLimits", m_bEnableLimits)
            m_snDamping.SaveData(oXml, "Damping")
            m_snRestitution.SaveData(oXml, "Restitution")
            m_snStiffness.SaveData(oXml, "Stiffness")
            oXml.OutOfElem()

            oXml.OutOfElem() 'Outof BodyPart Element

        End Sub

        Public Overrides Sub SaveSimulationXml(ByRef oXml As Interfaces.StdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

            oXml.IntoElem() 'Into BodyPart Element

            oXml.AddChildElement("Constraint")
            oXml.IntoElem()
            oXml.AddChildElement("EnableLimits", m_bEnableLimits)
            m_snDamping.SaveSimulationXml(oXml, Me, "Damping")
            m_snRestitution.SaveSimulationXml(oXml, Me, "Restitution")
            m_snStiffness.SaveSimulationXml(oXml, Me, "Stiffness")
            oXml.OutOfElem()

            oXml.OutOfElem() 'Outof BodyPart Element

        End Sub
#End Region

#Region " Events "

        Protected Sub OnAttachedPartMoved(ByRef bpPart As AnimatGUI.DataObjects.Physical.BodyPart)
            Try
                Me.SetSimData("AttachedPartMovedOrRotated", bpPart.ID, True)
            Catch ex As Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Sub OnAttachedPartRotated(ByRef bpPart As AnimatGUI.DataObjects.Physical.BodyPart)
            Try
                Me.SetSimData("AttachedPartMovedOrRotated", bpPart.ID, True)
            Catch ex As Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace


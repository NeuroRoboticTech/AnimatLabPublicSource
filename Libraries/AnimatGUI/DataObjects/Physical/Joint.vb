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

        Protected m_snSize As AnimatGUI.Framework.ScaledNumber
        Protected m_bEnableLimts As Boolean = True

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

        Public Overridable Property Size() As ScaledNumber
            Get
                Return m_snSize
            End Get
            Set(ByVal value As ScaledNumber)
                If value.ActualValue <= 0 Then
                    Throw New System.Exception("The size cannot be less than or equal to zero.")
                End If

                SetSimData("Size", value.ActualValue.ToString, True)
                m_snSize.CopyData(value)
            End Set
        End Property

        Public Overridable Property EnableLimits() As Boolean
            Get
                Return m_bEnableLimts
            End Get
            Set(ByVal value As Boolean)
                SetSimData("EnableLimits", value.ToString, True)
                m_bEnableLimts = value
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            m_thIncomingDataType = New AnimatGUI.DataObjects.DataType("DesiredVelocity", "Desired Velocity", "m/s", "m/s", -5, 5, ScaledNumber.enumNumericScale.None, ScaledNumber.enumNumericScale.None)

            m_snSize = New AnimatGUI.Framework.ScaledNumber(Me, "Size", 2, AnimatGUI.Framework.ScaledNumber.enumNumericScale.centi, "Meters", "m")

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

            If Not m_snSize Is Nothing Then m_snSize.ClearIsDirty()
        End Sub

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doOrigJoint As Joint = DirectCast(doOriginal, Joint)
            m_snSize = DirectCast(doOrigJoint.m_snSize.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_bEnableLimts = doOrigJoint.m_bEnableLimts
        End Sub

        Public Overrides Function CreateJointTreeView(ByRef tvTree As Crownwood.DotNetMagic.Controls.TreeControl, ByVal tnParent As Crownwood.DotNetMagic.Controls.Node, _
                                                      ByVal thSelectedPart As TypeHelpers.LinkedBodyPart) As Crownwood.DotNetMagic.Controls.Node

            Dim tnJoint As New Crownwood.DotNetMagic.Controls.Node(Me.Name)
            tnParent.Nodes.Add(tnJoint)
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
            Me.EnableLimits = doExisting.m_bEnableLimts
            m_bpBodyNode = doOriginal.BodyTreeNode

            Util.Application.WorkspaceImages.AddImage(Me.WorkspaceImageName)

            m_bpBodyNode.ImageIndex = Util.Application.WorkspaceImages.GetImageIndex(Me.WorkspaceImageName)
            m_bpBodyNode.SelectedImageIndex = Util.Application.WorkspaceImages.GetImageIndex(Me.WorkspaceImageName)
            m_bpBodyNode.Tag = Me

        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag
            pbNumberBag = m_snSize.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Size", pbNumberBag.GetType(), "Size", _
                                        "Part Properties", "Sets the overall size of the part.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Enable Limits", m_bEnabled.GetType(), "EnableLimits", _
                                        "Constraints", "Enables or disables this joint limit constraints.", m_bEnabled))

        End Sub

        Public Overloads Overrides Sub LoadData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByRef oXml As Interfaces.StdXml)

            MyBase.LoadData(doStructure, oXml)

            oXml.IntoElem() 'Into Joint Element

            m_snSize.LoadData(oXml, "Size")
            m_bEnableLimts = oXml.GetChildBool("EnableLimits", m_bEnableLimts)

            oXml.OutOfElem() 'Outof Joint Element

        End Sub

        Public Overloads Overrides Sub SaveData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByRef oXml As Interfaces.StdXml)

            MyBase.SaveData(doStructure, oXml)

            oXml.IntoElem() 'Into Joint Element

            m_snSize.SaveData(oXml, "Size")
            oXml.AddChildElement("EnableLimits", m_bEnableLimts)

            oXml.OutOfElem() 'Outof Joint Element

        End Sub

        Public Overrides Sub SaveSimulationXml(ByRef oXml As Interfaces.StdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

            oXml.IntoElem() 'Into Joint Element

            m_snSize.SaveSimulationXml(oXml, Me, "Size")
            oXml.AddChildElement("EnableLimits", m_bEnableLimts)

            oXml.OutOfElem() 'Outof Joint Element

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


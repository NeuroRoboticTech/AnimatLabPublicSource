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

Namespace DataObjects.Physical

    Public Class Environment
        Inherits DataObjects.DragObject

#Region " Enums "

        Public Enum enumDistanceUnits
            Kilometers = 3
            Centameters = 2
            Decameters = 1
            Meters = 0
            Decimeters = -1
            Centimeters = -2
            Millimeters = -3
        End Enum

        Public Enum enumMassUnits
            Kilograms = 3
            Centagrams = 2
            Decagrams = 1
            Grams = 0
            Decigrams = -1
            Centigrams = -2
            Milligrams = -3
        End Enum

#End Region

#Region " Attributes "

        Protected m_snPhysicsTimeStep As ScaledNumber
        Protected m_snGravity As ScaledNumber

        Protected m_snMouseSpringStiffness As ScaledNumber
        Protected m_snMouseSpringDamping As ScaledNumber

        Protected m_snLinearCompliance As ScaledNumber
        Protected m_snLinearDamping As ScaledNumber
        Protected m_snAngularCompliance As ScaledNumber
        Protected m_snAngularDamping As ScaledNumber
        Protected m_snLinearKineticLoss As ScaledNumber
        Protected m_snAngularKineticLoss As ScaledNumber

        Protected m_eDistanceUnits As enumDistanceUnits = enumDistanceUnits.Decimeters
        Protected m_eMassUnits As enumMassUnits = enumMassUnits.Grams
        Protected m_bSimulateHydrodynamics As Boolean = False

        Protected m_snRecFieldSelRadius As ScaledNumber

        Protected m_iNewOrganismCount As Integer
        Protected m_iNewStructureCount As Integer

        Protected m_aryOrganisms As New Collections.SortedStructures(Me)
        Protected m_aryStructures As New Collections.SortedStructures(Me)
        Protected m_aryOdorTypes As New Collections.SortedOdorTypes(Me)

        Protected m_tnOrganisms As Crownwood.DotNetMagic.Controls.Node
        Protected m_tnStructures As Crownwood.DotNetMagic.Controls.Node

        Protected m_bAutoGenerateRandomSeed As Boolean = True
        Protected m_iManualRandomSeed As Integer = 12345

#End Region

#Region " Properties "

#Region " DragObject Properties "

        <Browsable(False)> _
        Public Overrides Property ItemName() As String
            Get
                Return Me.Name
            End Get
            Set(ByVal Value As String)
                Me.Name = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.Environment.gif"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property DragImageName() As String
            Get
                Return WorkspaceImageName()
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property CanBeCharted() As Boolean
            Get
                Return False
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property StructureID() As String
            Get
                Return ""
            End Get
        End Property

#End Region

        Public Property PhysicsTimeStep() As ScaledNumber
            Get
                Return m_snPhysicsTimeStep
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue <= 0 Then
                    Throw New System.Exception("You can not set the physics time step value to be less than or equal to zero!")
                End If

                Me.SetSimData("PhysicsTimeStep", Value.ActualValue.ToString, True)
                m_snPhysicsTimeStep.CopyData(Value)
            End Set
        End Property

        Public Property Gravity() As ScaledNumber
            Get
                Return m_snGravity
            End Get
            Set(ByVal Value As ScaledNumber)
                Me.SetSimData("Gravity", Value.ActualValue.ToString, True)
                m_snGravity.CopyData(Value)
            End Set
        End Property


        Public Property SimulateHydrodynamics() As Boolean
            Get
                Return m_bSimulateHydrodynamics
            End Get
            Set(ByVal Value As Boolean)
                Me.SetSimData("SimulateHydrodynamics", Value.ToString, True)
                m_bSimulateHydrodynamics = Value
                ResetEnableFluidsForRigidBodies()
            End Set
        End Property

        'Public Property FluidDensity() As ScaledNumber
        '    Get
        '        Return m_snFluidDensity
        '    End Get
        '    Set(ByVal Value As ScaledNumber)
        '        If Value.ActualValue < 0 Then
        '            Throw New System.Exception("You can not set the fluid density value to be less than zero!")
        '        End If

        '        Me.SetSimData("FluidDensity", Value.ActualValue.ToString, True)
        '        m_snFluidDensity.CopyData(Value)
        '    End Set
        'End Property

        Public Property MouseSpringStiffness() As ScaledNumber
            Get
                Return m_snMouseSpringStiffness
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("You can not set the mouse spring stiffness to be less than zero!")
                End If

                Me.SetSimData("MouseSpringStiffness", Value.ActualValue.ToString, True)
                m_snMouseSpringStiffness.CopyData(Value)
            End Set
        End Property

        Public Property MouseSpringDamping() As ScaledNumber
            Get
                Return m_snMouseSpringDamping
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("You can not set the mouse spring stiffness to be less than zero!")
                End If

                Me.SetSimData("MouseSpringDamping", Value.ActualValue.ToString, True)
                m_snMouseSpringDamping.CopyData(Value)
            End Set
        End Property

        Public Property LinearCompliance() As ScaledNumber
            Get
                Return m_snLinearCompliance
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("You can not set the linear compliance to be less than zero!")
                End If

                Me.SetSimData("LinearCompliance", Value.ActualValue.ToString, True)
                m_snLinearCompliance.CopyData(Value)
            End Set
        End Property

        Public Property LinearDamping() As ScaledNumber
            Get
                Return m_snLinearDamping
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("You can not set the linear damping to be less than zero!")
                End If

                Me.SetSimData("LinearDamping", Value.ActualValue.ToString, True)
                m_snLinearDamping.CopyData(Value)
            End Set
        End Property

        Public Property AngularCompliance() As ScaledNumber
            Get
                Return m_snAngularCompliance
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("You can not set the angular compliance to be less than zero!")
                End If

                Me.SetSimData("AngularCompliance", Value.ActualValue.ToString, True)
                m_snAngularCompliance.CopyData(Value)
            End Set
        End Property

        Public Property AngularDamping() As ScaledNumber
            Get
                Return m_snAngularDamping
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("You can not set the angular damping to be less than zero!")
                End If

                Me.SetSimData("AngularDamping", Value.ActualValue.ToString, True)
                m_snAngularDamping.CopyData(Value)
            End Set
        End Property

        Public Property LinearKineticLoss() As ScaledNumber
            Get
                Return m_snLinearKineticLoss
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("You can not set the linear kinetic loss to be less than zero!")
                End If

                Me.SetSimData("LinearKineticLoss", Value.ActualValue.ToString, True)
                m_snLinearKineticLoss.CopyData(Value)
            End Set
        End Property

        Public Property AngularKineticLoss() As ScaledNumber
            Get
                Return m_snAngularKineticLoss
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("You can not set the angular kinetic loss to be less than zero!")
                End If

                Me.SetSimData("AngularKineticLoss", Value.ActualValue.ToString, True)
                m_snAngularKineticLoss.CopyData(Value)
            End Set
        End Property

        Public Property RecFieldSelRadius() As ScaledNumber
            Get
                Return m_snRecFieldSelRadius
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("You can not set the receptive field selected radius to be less than zero!")
                End If

                Me.SetSimData("RecFieldSelRadius", Value.ActualValue.ToString, True)
                m_snRecFieldSelRadius.CopyData(Value)
            End Set
        End Property

        Public Property DistanceUnits() As enumDistanceUnits
            Get
                Return m_eDistanceUnits
            End Get
            Set(ByVal Value As enumDistanceUnits)
                Me.SetSimData("DistanceUnits", Value.ToString, True)
                m_eDistanceUnits = Value
            End Set
        End Property

        Public ReadOnly Property DistanceUnitValue() As Single
            Get
                Return CSng(Math.Pow(10, CInt(m_eDistanceUnits)))
            End Get
        End Property

        Public ReadOnly Property DistanceUnitAbbreviation(ByVal eUnits As Environment.enumDistanceUnits) As String
            Get
                Select Case eUnits
                    Case enumDistanceUnits.Kilometers
                        Return "Km"
                    Case enumDistanceUnits.Centameters
                        Return "Cm"
                    Case enumDistanceUnits.Decameters
                        Return "Dm"
                    Case enumDistanceUnits.Meters
                        Return "m"
                    Case enumDistanceUnits.Decimeters
                        Return "dm"
                    Case enumDistanceUnits.Centimeters
                        Return "cm"
                    Case enumDistanceUnits.Millimeters
                        Return "mm"
                End Select

                Return "m"
            End Get
        End Property

        Public ReadOnly Property DisplayDistanceUnits() As enumDistanceUnits
            Get
                Return DisplayDistanceUnits(Me.DistanceUnits)
            End Get
        End Property

        Public ReadOnly Property DisplayDistanceUnits(ByVal eUnits As enumDistanceUnits) As enumDistanceUnits
            Get
                Select Case eUnits
                    Case enumDistanceUnits.Kilometers
                        Return enumDistanceUnits.Kilometers
                    Case enumDistanceUnits.Centameters
                        Return enumDistanceUnits.Centameters
                    Case enumDistanceUnits.Decameters
                        Return enumDistanceUnits.Meters
                    Case enumDistanceUnits.Meters
                        Return enumDistanceUnits.Meters
                    Case enumDistanceUnits.Decimeters
                        Return enumDistanceUnits.Centimeters
                    Case enumDistanceUnits.Centimeters
                        Return enumDistanceUnits.Centimeters
                    Case enumDistanceUnits.Millimeters
                        Return enumDistanceUnits.Millimeters
                End Select

                Return enumDistanceUnits.Meters
            End Get
        End Property

        Public ReadOnly Property DisplayDistanceUnitValue() As Single
            Get
                Return Me.DisplayDistanceUnitValue(Me.DistanceUnits)
            End Get
        End Property

        Public ReadOnly Property DisplayDistanceUnitValue(ByVal eUnits As enumDistanceUnits) As Single
            Get
                Return CSng(Math.Pow(10, CInt(Me.DisplayDistanceUnits(eUnits))))
            End Get
        End Property

        Public Property MassUnits() As enumMassUnits
            Get
                Return m_eMassUnits
            End Get
            Set(ByVal Value As enumMassUnits)
                Me.SetSimData("MassUnits", Value.ToString, True)
                m_eMassUnits = Value
            End Set
        End Property

        Public ReadOnly Property MassUnitValue() As Single
            Get
                Return CSng(Math.Pow(10, CInt(m_eMassUnits)))
            End Get
        End Property

        Public ReadOnly Property MassUnitAbbreviation() As String
            Get
                Select Case m_eMassUnits
                    Case enumMassUnits.Kilograms
                        Return "Kg"
                    Case enumMassUnits.Centagrams
                        Return "Cg"
                    Case enumMassUnits.Grams
                        Return "g"
                    Case enumMassUnits.Centigrams
                        Return "cg"
                    Case enumMassUnits.Milligrams
                        Return "mg"
                End Select

                Return "g"
            End Get
        End Property

        'This calculates the default density. We will be using the density of water as the 
        'default setting, but we must set it up appropriately.
        Public Overridable ReadOnly Property DefaultDensity() As ScaledNumber
            Get

                Dim fltValue As Double = Me.MassUnitValue
                Dim eSCale As ScaledNumber.enumNumericScale = CType(Util.Environment.MassUnits, ScaledNumber.enumNumericScale)
                Dim strUnits As String = "g/" & Util.Environment.DistanceUnitAbbreviation(Me.DisplayDistanceUnits) & "^3"

                Return New ScaledNumber(Me, "Density", fltValue, eSCale, strUnits, strUnits)
            End Get
        End Property

        Public Overridable ReadOnly Property Organisms() As Collections.SortedStructures
            Get
                Return m_aryOrganisms
            End Get
        End Property

        Public Overridable ReadOnly Property Structures() As Collections.SortedStructures
            Get
                Return m_aryStructures
            End Get
        End Property

        Public Overridable ReadOnly Property OdorTypes() As Collections.SortedOdorTypes
            Get
                Return m_aryOdorTypes
            End Get
        End Property

        Public Overridable ReadOnly Property OrganismsTreeNode() As Crownwood.DotNetMagic.Controls.Node
            Get
                Return m_tnOrganisms
            End Get
        End Property

        Public Overridable ReadOnly Property StructuresTreeNode() As Crownwood.DotNetMagic.Controls.Node
            Get
                Return m_tnStructures
            End Get
        End Property

        Public Overridable Property NewOrganismCount() As Integer
            Get
                Return m_iNewOrganismCount
            End Get
            Set(ByVal Value As Integer)
                m_iNewOrganismCount = Value
            End Set
        End Property

        Public Overridable Property NewStructureCount() As Integer
            Get
                Return m_iNewStructureCount
            End Get
            Set(ByVal Value As Integer)
                m_iNewStructureCount = Value
            End Set
        End Property

        Public Overridable Property AutoGenerateRandomSeed() As Boolean
            Get
                Return m_bAutoGenerateRandomSeed
            End Get
            Set(ByVal Value As Boolean)
                Me.SetSimData("AutoGenerateRandomSeed", Value.ToString, True)
                m_bAutoGenerateRandomSeed = Value

                'Reselect this node to see if we need to display the manual seed or not.
                If Not Util.ProjectWorkspace Is Nothing Then
                    Util.ProjectWorkspace.RefreshProperties()
                End If
            End Set
        End Property

        Public Overridable Property ManualRandomSeed() As Integer
            Get
                Return m_iManualRandomSeed
            End Get
            Set(ByVal Value As Integer)
                Me.SetSimData("ManualRandomSeed", Value.ToString, True)
                m_iManualRandomSeed = Value
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            m_strName = "Environment"
            m_snPhysicsTimeStep = New AnimatGUI.Framework.ScaledNumber(Me, "PhysicsTimeStep", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "seconds", "s")
            m_snGravity = New AnimatGUI.Framework.ScaledNumber(Me, "Gravity", -9.81, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "m/s^2", "m/s^2")

            'If Not Util.Environment Is Nothing Then
            '    m_snFluidDensity = Util.Environment.DefaultDensity
            'Else
            '    m_snFluidDensity = New ScaledNumber(Me, "FluidDensity", 1000, ScaledNumber.enumNumericScale.Kilo, "g/m^2", "g/m^2")
            'End If

            m_snMouseSpringStiffness = New AnimatGUI.Framework.ScaledNumber(Me, "MouseSpringStiffness", 1, ScaledNumber.enumNumericScale.None, "N/m", "N/m")
            m_snMouseSpringDamping = New AnimatGUI.Framework.ScaledNumber(Me, "MouseSpringDamping", 100, ScaledNumber.enumNumericScale.None, "g/s", "g/s")

            m_snLinearCompliance = New ScaledNumber(Me, "LinearCompliance", 0.1, ScaledNumber.enumNumericScale.micro, "m/N", "m/N")
            m_snLinearDamping = New ScaledNumber(Me, "LinearDamping", 200, ScaledNumber.enumNumericScale.Kilo, "g/s", "g/s")

            m_snAngularCompliance = New ScaledNumber(Me, "AngularCompliance", 0.1, ScaledNumber.enumNumericScale.micro, "m/N", "m/N")
            m_snAngularDamping = New ScaledNumber(Me, "AngularDamping", 50, ScaledNumber.enumNumericScale.Kilo, "g/s", "g/s")

            m_snLinearKineticLoss = New ScaledNumber(Me, "LinearKineticLoss", 1, ScaledNumber.enumNumericScale.micro, "g/s", "g/s")
            m_snAngularKineticLoss = New ScaledNumber(Me, "AngularKineticLoss", 1, ScaledNumber.enumNumericScale.micro, "g/s", "g/s")

            m_snRecFieldSelRadius = New ScaledNumber(Me, "RecFieldSelRadius", 5, ScaledNumber.enumNumericScale.milli, "Meters", "m")

        End Sub

        Public Overridable Sub AddOrganism()
            Me.OnNewOrganism(Me, New System.EventArgs)
        End Sub

        Public Overridable Sub AddStructure()
            Me.OnNewStructure(Me, New System.EventArgs)
        End Sub

#Region " Treeview/Menu Methods "

        Public Overrides Sub CreateWorkspaceTreeView(ByVal doParent As Framework.DataObject, ByVal doParentNode As Crownwood.DotNetMagic.Controls.Node)

            MyBase.CreateWorkspaceTreeView(doParent, doParentNode)
            m_tnWorkspaceNode.Select()

            m_tnOrganisms = Util.ProjectWorkspace.AddTreeNode(m_tnWorkspaceNode, "Organisms", "AnimatGUI.Organisms.gif")
            m_tnStructures = Util.ProjectWorkspace.AddTreeNode(m_tnWorkspaceNode, "Structures", "AnimatGUI.Structures.gif")

            Dim doOrganism As DataObjects.Physical.Organism
            For Each deEntry As DictionaryEntry In m_aryOrganisms
                doOrganism = DirectCast(deEntry.Value, DataObjects.Physical.Organism)
                doOrganism.CreateWorkspaceTreeView(Me, m_tnOrganisms)
            Next

            Dim doStructure As DataObjects.Physical.PhysicalStructure
            For Each deEntry As DictionaryEntry In m_aryStructures
                doStructure = DirectCast(deEntry.Value, DataObjects.Physical.PhysicalStructure)
                doStructure.CreateWorkspaceTreeView(Me, m_tnStructures)
            Next

            m_iNewOrganismCount = Util.ExtractIDCount("Organism", m_aryOrganisms)
            m_iNewStructureCount = Util.ExtractIDCount("Structure", m_aryStructures)

        End Sub

        Public Overrides Function WorkspaceTreeviewPopupMenu(ByRef tnSelectedNode As Crownwood.DotNetMagic.Controls.Node, ByVal ptPoint As Point) As Boolean

            If tnSelectedNode Is m_tnOrganisms Then
                PopupStructureMenu(tnSelectedNode, ptPoint, False)
                Return True
            End If

            If tnSelectedNode Is m_tnStructures Then
                PopupStructureMenu(tnSelectedNode, ptPoint, True)
                Return True
            End If

            Dim doOrganism As DataObjects.Physical.Organism
            For Each deEntry As DictionaryEntry In m_aryOrganisms
                doOrganism = DirectCast(deEntry.Value, DataObjects.Physical.Organism)
                If doOrganism.WorkspaceTreeviewPopupMenu(tnSelectedNode, ptPoint) Then Return True
            Next

            Dim doStructure As DataObjects.Physical.PhysicalStructure
            For Each deEntry As DictionaryEntry In m_aryStructures
                doStructure = DirectCast(deEntry.Value, DataObjects.Physical.PhysicalStructure)
                If doStructure.WorkspaceTreeviewPopupMenu(tnSelectedNode, ptPoint) Then Return True
            Next

            If tnSelectedNode Is m_tnWorkspaceNode Then
                Dim mcExpandAll As New System.Windows.Forms.ToolStripMenuItem("Expand All", Util.Application.ToolStripImages.GetImage("AnimatGUI.Expand.gif"), New EventHandler(AddressOf Me.OnExpandAll))
                Dim mcCollapseAll As New System.Windows.Forms.ToolStripMenuItem("Collapse All", Util.Application.ToolStripImages.GetImage("AnimatGUI.Collapse.gif"), New EventHandler(AddressOf Me.OnCollapseAll))

                mcExpandAll.Tag = tnSelectedNode
                mcCollapseAll.Tag = tnSelectedNode

                ' Create the popup menu object
                Dim popup As New AnimatContextMenuStrip("AnimatGUI.DataObjects.Physical.Environment.WorkspaceTreeviewPopupMenu", Util.SecurityMgr)
                popup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {mcExpandAll, mcCollapseAll})
            End If

            Return False
        End Function

        Protected Overridable Sub PopupStructureMenu(ByRef tnSelectedNode As Crownwood.DotNetMagic.Controls.Node, ByVal ptPoint As Point, ByVal bStruct As Boolean)

            Dim mcInsert As System.Windows.Forms.ToolStripMenuItem

            If bStruct Then
                mcInsert = New System.Windows.Forms.ToolStripMenuItem("New Structure", Util.Application.ToolStripImages.GetImage("AnimatGUI.AddStructure.gif"), New EventHandler(AddressOf Me.OnNewStructure))
            Else
                mcInsert = New System.Windows.Forms.ToolStripMenuItem("New Organism", Util.Application.ToolStripImages.GetImage("AnimatGUI.AddOrganism.gif"), New EventHandler(AddressOf Me.OnNewOrganism))
            End If

            Dim mcSepExpand As New ToolStripSeparator()
            Dim mcExpandAll As New System.Windows.Forms.ToolStripMenuItem("Expand All", Util.Application.ToolStripImages.GetImage("AnimatGUI.Expand.gif"), New EventHandler(AddressOf Me.OnExpandAll))
            Dim mcCollapseAll As New System.Windows.Forms.ToolStripMenuItem("Collapse All", Util.Application.ToolStripImages.GetImage("AnimatGUI.Collapse.gif"), New EventHandler(AddressOf Me.OnCollapseAll))

            mcExpandAll.Tag = tnSelectedNode
            mcCollapseAll.Tag = tnSelectedNode

            ' Create the popup menu object
            Dim popup As New AnimatContextMenuStrip("AnimatGUI.DataObjects.Physical.Environment.PopupStructureMenu", Util.SecurityMgr)
            popup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {mcInsert, mcSepExpand, mcExpandAll, mcCollapseAll})

            Util.ProjectWorkspace.ctrlTreeView.ContextMenuNode = popup
        End Sub

        Public Overrides Function WorkspaceTreeviewDoubleClick(ByRef tnSelectedNode As Crownwood.DotNetMagic.Controls.Node) As Boolean

            If tnSelectedNode Is m_tnWorkspaceNode Then
                Util.Application.EditEnvironment()
                Return True
            End If

            Dim doOrganism As DataObjects.Physical.Organism
            For Each deEntry As DictionaryEntry In m_aryOrganisms
                doOrganism = DirectCast(deEntry.Value, DataObjects.Physical.Organism)
                If doOrganism.WorkspaceTreeviewDoubleClick(tnSelectedNode) Then Return True
            Next

            Dim doStructure As DataObjects.Physical.PhysicalStructure
            For Each deEntry As DictionaryEntry In m_aryStructures
                doStructure = DirectCast(deEntry.Value, DataObjects.Physical.PhysicalStructure)
                If doStructure.WorkspaceTreeviewDoubleClick(tnSelectedNode) Then Return True
            Next

            Return False

        End Function

#End Region

#Region " Find Methods "

        Public Overridable Function FindOrganism(ByVal strID As String, Optional ByVal bThrowError As Boolean = True) As DataObjects.Physical.PhysicalStructure
            Dim doStructure As DataObjects.Physical.PhysicalStructure = Nothing

            If Me.Organisms.Contains(strID) Then
                doStructure = Me.Organisms(strID)
            ElseIf bThrowError Then
                Throw New System.Exception("No organism with the id '" & strID & "' was found.")
            End If

            Return doStructure
        End Function

        Public Overridable Function FindStructure(ByVal strID As String, Optional ByVal bThrowError As Boolean = True) As DataObjects.Physical.PhysicalStructure
            Dim doStructure As DataObjects.Physical.PhysicalStructure = Nothing

            If Me.Structures.Contains(strID) Then
                doStructure = Me.Structures(strID)
            End If

            If bThrowError Then
                Throw New System.Exception("No structure with the id '" & strID & "' was found.")
            End If

            Return doStructure
        End Function

        Public Overridable Function FindStructureFromAll(ByVal strID As String, Optional ByVal bThrowError As Boolean = True) As DataObjects.Physical.PhysicalStructure
            Dim doStructure As DataObjects.Physical.PhysicalStructure

            doStructure = FindStructure(strID, False)
            If Not doStructure Is Nothing Then Return doStructure

            doStructure = FindOrganism(strID, False)
            If Not doStructure Is Nothing Then Return doStructure

            If bThrowError Then
                Throw New System.Exception("No structure with the id '" & strID & "' was found.")
            End If

            Return doStructure
        End Function

        Public Overridable Function FindOrganismByName(ByVal strName As String, Optional ByVal bThrowError As Boolean = True) As DataObjects.Physical.PhysicalStructure
            Dim doStructure As DataObjects.Physical.PhysicalStructure

            For Each deEntry As DictionaryEntry In m_aryOrganisms
                doStructure = DirectCast(deEntry.Value, PhysicalStructure)
                If doStructure.Name = strName Then
                    Return doStructure
                End If
            Next

            If bThrowError Then
                Throw New System.Exception("No organism with the name '" & strName & "' was found.")
            End If

            Return doStructure
        End Function

        Public Overridable Function FindStructureByName(ByVal strName As String, Optional ByVal bThrowError As Boolean = True) As DataObjects.Physical.PhysicalStructure
            Dim doStructure As DataObjects.Physical.PhysicalStructure

            For Each deEntry As DictionaryEntry In m_aryStructures
                doStructure = DirectCast(deEntry.Value, PhysicalStructure)
                If doStructure.Name = strName Then
                    Return doStructure
                End If
            Next

            If bThrowError Then
                Throw New System.Exception("No structure with the name '" & strName & "' was found.")
            End If

            Return doStructure
        End Function

        Public Overridable Function FindStructureFromAllByName(ByVal strName As String, Optional ByVal bThrowError As Boolean = True) As DataObjects.Physical.PhysicalStructure
            Dim doStructure As DataObjects.Physical.PhysicalStructure

            doStructure = FindOrganismByName(strName, False)
            If Not doStructure Is Nothing Then Return doStructure

            doStructure = FindStructureByName(strName, False)
            If Not doStructure Is Nothing Then Return doStructure

            If bThrowError Then
                Throw New System.Exception("No structure with the name '" & strName & "' was found.")
            End If

            Return doStructure
        End Function

#End Region

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGUICtrls.Controls.PropertyTable)

            Dim pbNumberBag As AnimatGUICtrls.Controls.PropertyBag = m_snPhysicsTimeStep.Properties
            propTable.Properties.Add(New AnimatGUICtrls.Controls.PropertySpec("Physics Time Step", pbNumberBag.GetType(), "PhysicsTimeStep", _
                                        "Settings", "This is the increment that is taken between each time step of the physics simulator. ", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snGravity.Properties
            propTable.Properties.Add(New AnimatGUICtrls.Controls.PropertySpec("Gravity", pbNumberBag.GetType(), "Gravity", _
                                        "Settings", "Sets the gravity for the simulation. This is applied along the y axis. Gravity is always specified in " & _
                                        "meters per second squared. regardless of the distance units specified.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            propTable.Properties.Add(New AnimatGUICtrls.Controls.PropertySpec("ID", Me.ID.GetType(), "ID", _
                                        "Settings", "ID", Me.ID, True))

            propTable.Properties.Add(New AnimatGUICtrls.Controls.PropertySpec("AutoGenerate Random Seed", m_bAutoGenerateRandomSeed.GetType(), "AutoGenerateRandomSeed", _
                                        "Settings", "If this is true then the random number generator is automatically seeded at the beginning of a simulation to ensure " & _
                                        "different numbers are generated each run. If it is false then the seed specified in the Manual Random Seed property is used.", m_bAutoGenerateRandomSeed))

            If Not m_bAutoGenerateRandomSeed Then
                propTable.Properties.Add(New AnimatGUICtrls.Controls.PropertySpec("Manual Random Seed", m_iManualRandomSeed.GetType(), "ManualRandomSeed", _
                                            "Settings", "Allows the user to manual set the random number seed to use for the random number generator.", m_iManualRandomSeed))
            End If

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("SimulateHydrodynamics", m_bSimulateHydrodynamics.GetType(), "SimulateHydrodynamics", _
                                        "Settings", "Determines whether hydrodynamic effects such as buoyancy and drag act upon the bodies in the simulation. " & _
                                        "If this is turned off then the simulation will run slightly faster.", m_bSimulateHydrodynamics))

            'pbNumberBag = m_snFluidDensity.Properties
            'propTable.Properties.Add(New AnimatGUICtrls.Controls.PropertySpec("Fluid Density", pbNumberBag.GetType(), "FluidDensity", _
            '                            "Hydrodynamics", "The density of the fluid medium. This is only used if hydrodynamics are being simulated.", pbNumberBag, _
            '                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            propTable.Properties.Add(New AnimatGUICtrls.Controls.PropertySpec("DistanceUnits", GetType(String), "DistanceUnits", _
                                        "Units", "Determines the distance unit measurements used within the configuration files.", _
                                        m_eDistanceUnits, GetType(AnimatGUI.TypeHelpers.UnitsTypeEditor), GetType(AnimatGUI.TypeHelpers.UnitsTypeConverter)))

            propTable.Properties.Add(New AnimatGUICtrls.Controls.PropertySpec("MassUnits", GetType(String), "MassUnits", _
                                        "Units", "Determines the mass unit measurements used within the configuration files.", _
                                        m_eMassUnits, GetType(AnimatGUI.TypeHelpers.UnitsTypeEditor), GetType(AnimatGUI.TypeHelpers.UnitsTypeConverter)))

            pbNumberBag = m_snRecFieldSelRadius.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Selected Vertex Radius", pbNumberBag.GetType(), "RecFieldSelRadius", _
                                        "Units", "The radius of the sphere used to show the selecte vertex in receptive field mode.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snMouseSpringStiffness.Properties
            propTable.Properties.Add(New AnimatGUICtrls.Controls.PropertySpec("Mouse Spring Stiffness", pbNumberBag.GetType(), "MouseSpringStiffness", _
                                        "Mouse Spring Settings", "Sets the stiffness of the spring used when applying forces using the mouse during a simulation.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snMouseSpringDamping.Properties
            propTable.Properties.Add(New AnimatGUICtrls.Controls.PropertySpec("Mouse Spring Damping", pbNumberBag.GetType(), "MouseSpringDamping", _
                                        "Mouse Spring Settings", "Sets the damping of the spring used when applying forces using the mouse during a simulation.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snLinearCompliance.Properties
            propTable.Properties.Add(New AnimatGUICtrls.Controls.PropertySpec("Linear Compliance", pbNumberBag.GetType(), "LinearCompliance", _
                                        "World Stability", "The compliance value of the spring used in linear collisions within the simulator.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snLinearDamping.Properties
            propTable.Properties.Add(New AnimatGUICtrls.Controls.PropertySpec("Linear Damping", pbNumberBag.GetType(), "LinearDamping", _
                                        "World Stability", "The damping value of the spring used in linear collisions within the simulator.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snAngularCompliance.Properties
            propTable.Properties.Add(New AnimatGUICtrls.Controls.PropertySpec("Angular Compliance", pbNumberBag.GetType(), "AngularCompliance", _
                                        "World Stability", "The compliance value of the spring used in angular collisions within the simulator.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snAngularDamping.Properties
            propTable.Properties.Add(New AnimatGUICtrls.Controls.PropertySpec("Angular Damping", pbNumberBag.GetType(), "AngularDamping", _
                                        "World Stability", "The damping value of the spring used in angular collisions within the simulator.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snLinearKineticLoss.Properties
            propTable.Properties.Add(New AnimatGUICtrls.Controls.PropertySpec("Linear Kinetic Loss", pbNumberBag.GetType(), "LinearKineticLoss", _
                                        "World Stability", "The amount of kinetic loss for linear collisions within the simulator.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snAngularKineticLoss.Properties
            propTable.Properties.Add(New AnimatGUICtrls.Controls.PropertySpec("Angular Kinetic Loss", pbNumberBag.GetType(), "AngularKineticLoss", _
                                        "World Stability", "The amount of kinetic loss for angular collisions within the simulator.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()
            m_aryOrganisms.ClearIsDirty()
            m_aryStructures.ClearIsDirty()
            m_aryOdorTypes.ClearIsDirty()

            If Not m_snPhysicsTimeStep Is Nothing Then m_snPhysicsTimeStep.ClearIsDirty()
            If Not m_snGravity Is Nothing Then m_snGravity.ClearIsDirty()
            If Not m_snMouseSpringStiffness Is Nothing Then m_snMouseSpringStiffness.ClearIsDirty()
            If Not m_snMouseSpringDamping Is Nothing Then m_snMouseSpringDamping.ClearIsDirty()

            If Not m_snLinearCompliance Is Nothing Then m_snLinearCompliance.ClearIsDirty()
            If Not m_snLinearDamping Is Nothing Then m_snLinearDamping.ClearIsDirty()
            If Not m_snAngularCompliance Is Nothing Then m_snAngularCompliance.ClearIsDirty()
            If Not m_snAngularDamping Is Nothing Then m_snAngularDamping.ClearIsDirty()
            If Not m_snLinearKineticLoss Is Nothing Then m_snLinearKineticLoss.ClearIsDirty()
            If Not m_snAngularKineticLoss Is Nothing Then m_snAngularKineticLoss.ClearIsDirty()
            If Not m_snRecFieldSelRadius Is Nothing Then m_snRecFieldSelRadius.ClearIsDirty()

        End Sub

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doOrig As AnimatGUI.DataObjects.Physical.Environment = DirectCast(doOriginal, Environment)

            m_snPhysicsTimeStep = DirectCast(doOrig.m_snPhysicsTimeStep.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snGravity = DirectCast(doOrig.m_snGravity.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snMouseSpringStiffness = DirectCast(doOrig.m_snMouseSpringStiffness.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snMouseSpringDamping = DirectCast(doOrig.m_snMouseSpringDamping.Clone(Me, bCutData, doRoot), ScaledNumber)

            m_snLinearCompliance = DirectCast(doOrig.m_snLinearCompliance.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snLinearDamping = DirectCast(doOrig.m_snLinearDamping.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snAngularCompliance = DirectCast(doOrig.m_snAngularCompliance.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snAngularDamping = DirectCast(doOrig.m_snAngularDamping.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snLinearKineticLoss = DirectCast(doOrig.m_snLinearKineticLoss.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snAngularKineticLoss = DirectCast(doOrig.m_snAngularKineticLoss.Clone(Me, bCutData, doRoot), ScaledNumber)

            m_snRecFieldSelRadius = DirectCast(doOrig.m_snRecFieldSelRadius.Clone(Me, bCutData, doRoot), ScaledNumber)

            m_eDistanceUnits = doOrig.m_eDistanceUnits
            m_eMassUnits = doOrig.m_eMassUnits
            m_bSimulateHydrodynamics = doOrig.m_bSimulateHydrodynamics

            m_iNewOrganismCount = doOrig.m_iNewOrganismCount
            m_iNewStructureCount = doOrig.m_iNewStructureCount

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim doItem As New Environment(doParent)
            doItem.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then doItem.AfterClone(Me, bCutData, doRoot, doItem)
            Return doItem
        End Function

        Public Overrides Sub UnitsChanged(ByVal ePrevMass As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits, _
                                          ByVal eNewMass As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits, _
                                          ByVal fltMassChange As Single, _
                                          ByVal ePrevDistance As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits, _
                                          ByVal eNewDistance As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits, _
                                          ByVal fltDistanceChange As Single)

            Dim iDistDiff As Integer = CInt(Me.DisplayDistanceUnits) - CInt(Util.Environment.DisplayDistanceUnits(ePrevDistance))
            Dim fltDensityDistChange As Single = CSng(10 ^ iDistDiff)

            m_snMouseSpringStiffness.ActualValue = m_snMouseSpringStiffness.ActualValue / fltDistanceChange
            m_snMouseSpringDamping.ActualValue = m_snMouseSpringStiffness.ActualValue / fltMassChange

            m_snLinearCompliance.ActualValue = m_snLinearCompliance.ActualValue * fltDistanceChange
            m_snLinearDamping.ActualValue = m_snLinearDamping.ActualValue / fltDistanceChange

            m_snAngularCompliance.ActualValue = m_snAngularCompliance.ActualValue * fltDistanceChange
            m_snAngularDamping.ActualValue = m_snAngularDamping.ActualValue / fltDistanceChange

            Dim doStruct As PhysicalStructure
            For Each deEntry As DictionaryEntry In m_aryOrganisms
                doStruct = DirectCast(deEntry.Value, PhysicalStructure)
                doStruct.UnitsChanged(ePrevMass, eNewMass, fltMassChange, ePrevDistance, eNewDistance, fltDistanceChange)
            Next

            For Each deEntry As DictionaryEntry In m_aryStructures
                doStruct = DirectCast(deEntry.Value, PhysicalStructure)
                doStruct.UnitsChanged(ePrevMass, eNewMass, fltMassChange, ePrevDistance, eNewDistance, fltDistanceChange)
            Next

        End Sub

        'Called whenever SimulateHydrodynamics is toggled. This resets the EnableFluid property of all rigid bodies.
        Protected Overridable Sub ResetEnableFluidsForRigidBodies()

            'Go through the organisms and structures and have them reset their enable fluids value.
            Dim doOrganism As DataObjects.Physical.Organism
            For Each deEntry As DictionaryEntry In m_aryOrganisms
                doOrganism = DirectCast(deEntry.Value, DataObjects.Physical.Organism)
                doOrganism.ResetEnableFluidsForRigidBodies()
            Next

            Dim doStructure As DataObjects.Physical.PhysicalStructure
            For Each deEntry As DictionaryEntry In m_aryStructures
                doStructure = DirectCast(deEntry.Value, DataObjects.Physical.PhysicalStructure)
                doStructure.ResetEnableFluidsForRigidBodies()
            Next
        End Sub

        Public Overridable Overloads Sub LoadData(ByRef oXml As Interfaces.StdXml)

            oXml.IntoChildElement("Environment") 'Into Environment Element

            m_snPhysicsTimeStep.LoadData(oXml, "PhysicsTimeStep")
            m_snGravity.LoadData(oXml, "Gravity")
            m_snMouseSpringStiffness.LoadData(oXml, "MouseSpringStiffness")
            m_snMouseSpringDamping.LoadData(oXml, "MouseSpringDamping")
            m_snLinearCompliance.LoadData(oXml, "LinearCompliance")
            m_snLinearDamping.LoadData(oXml, "LinearDamping")
            m_snAngularCompliance.LoadData(oXml, "AngularCompliance")
            m_snAngularDamping.LoadData(oXml, "AngularDamping")
            m_snLinearKineticLoss.LoadData(oXml, "LinearKineticLoss")
            m_snAngularKineticLoss.LoadData(oXml, "AngularKineticLoss")

            Me.SimulateHydrodynamics = oXml.GetChildBool("SimulateHydrodynamics", m_bSimulateHydrodynamics)

            If oXml.FindChildElement("AutoGenerateRandomSeed", False) Then
                m_bAutoGenerateRandomSeed = oXml.GetChildBool("AutoGenerateRandomSeed")
                m_iManualRandomSeed = oXml.GetChildInt("ManualRandomSeed")
            End If

            m_eMassUnits = DirectCast([Enum].Parse(GetType(enumMassUnits), oXml.GetChildString("MassUnits"), True), enumMassUnits)
            m_eDistanceUnits = DirectCast([Enum].Parse(GetType(enumDistanceUnits), oXml.GetChildString("DistanceUnits"), True), enumDistanceUnits)

            'm_snRecFieldSelRadius.LoadData(oXml, "RecFieldSelRadius")

            'Odor types must be loaded before structures
            Dim iCount As Integer
            If oXml.FindChildElement("OdorTypes", False) Then
                oXml.IntoChildElement("OdorTypes") 'Into Structures Element
                iCount = oXml.NumberOfChildren() - 1

                Dim newOdorType As OdorType
                For iIndex As Integer = 0 To iCount
                    oXml.FindChildByIndex(iIndex)

                    newOdorType = New DataObjects.Physical.OdorType(Me)
                    newOdorType.LoadData(oXml)

                    m_aryOdorTypes.Add(newOdorType.ID, newOdorType)
                Next
                oXml.OutOfElem() 'Outof Structures Element
            End If

            Dim newOrganism As DataObjects.Physical.Organism
            oXml.IntoChildElement("Organisms") 'Into Organisms Element
            iCount = oXml.NumberOfChildren() - 1

            For iIndex As Integer = 0 To iCount
                oXml.FindChildByIndex(iIndex)

                newOrganism = New DataObjects.Physical.Organism(Me)
                newOrganism.LoadData(oXml)

                m_aryOrganisms.Add(newOrganism.ID, newOrganism)
            Next
            oXml.OutOfElem() 'Outof Organisms Element

            Dim newStructure As DataObjects.Physical.PhysicalStructure
            oXml.IntoChildElement("Structures") 'Into Structures Element
            iCount = oXml.NumberOfChildren() - 1

            For iIndex As Integer = 0 To iCount
                oXml.FindChildByIndex(iIndex)

                newStructure = New DataObjects.Physical.PhysicalStructure(Me)
                newStructure.LoadData(oXml)

                m_aryStructures.Add(newStructure.ID, newStructure)
            Next
            oXml.OutOfElem() 'Outof Structures Element

            oXml.OutOfElem() 'Outof Environment Element

        End Sub

        Public Overridable Overloads Sub SaveData(ByRef oXml As Interfaces.StdXml)

            oXml.AddChildElement("Environment")
            oXml.IntoElem()

            m_snPhysicsTimeStep.SaveData(oXml, "PhysicsTimeStep")
            m_snGravity.SaveData(oXml, "Gravity")
            m_snMouseSpringStiffness.SaveData(oXml, "MouseSpringStiffness")
            m_snMouseSpringDamping.SaveData(oXml, "MouseSpringDamping")

            m_snLinearCompliance.SaveData(oXml, "LinearCompliance")
            m_snLinearDamping.SaveData(oXml, "LinearDamping")
            m_snAngularCompliance.SaveData(oXml, "AngularCompliance")
            m_snAngularDamping.SaveData(oXml, "AngularDamping")
            m_snLinearKineticLoss.SaveData(oXml, "LinearKineticLoss")
            m_snAngularKineticLoss.SaveData(oXml, "AngularKineticLoss")

            m_snRecFieldSelRadius.SaveData(oXml, "RecFieldSelRadius")

            oXml.AddChildElement("SimulateHydrodynamics", m_bSimulateHydrodynamics)

            oXml.AddChildElement("MassUnits", m_eMassUnits.ToString())
            oXml.AddChildElement("DistanceUnits", m_eDistanceUnits.ToString())

            oXml.AddChildElement("AutoGenerateRandomSeed", m_bAutoGenerateRandomSeed)
            oXml.AddChildElement("ManualRandomSeed", m_iManualRandomSeed)

            'If we are saving the config file for downloading to a  robot then we should only
            'save the selected organism and none of the other organisms or structures.
            If m_aryOdorTypes.Count > 0 Then
                oXml.AddChildElement("OdorTypes")
                oXml.IntoElem()
                Dim doOdor As DataObjects.Physical.OdorType
                For Each deEntry As DictionaryEntry In m_aryOdorTypes
                    doOdor = DirectCast(deEntry.Value, DataObjects.Physical.OdorType)
                    doOdor.SaveData(oXml)
                Next
                oXml.OutOfElem() 'Outof Organisms Element
            End If

            oXml.AddChildElement("Organisms")
            oXml.IntoElem()
            Dim doOrganism As DataObjects.Physical.Organism
            For Each deEntry As DictionaryEntry In m_aryOrganisms
                doOrganism = DirectCast(deEntry.Value, DataObjects.Physical.Organism)
                doOrganism.SaveData(oXml)
            Next
            oXml.OutOfElem() 'Outof Organisms Element

            oXml.AddChildElement("Structures")
            oXml.IntoElem()
            Dim doStructure As DataObjects.Physical.PhysicalStructure
            For Each deEntry As DictionaryEntry In m_aryStructures
                doStructure = DirectCast(deEntry.Value, DataObjects.Physical.PhysicalStructure)
                doStructure.SaveData(oXml)
            Next
            oXml.OutOfElem() 'Outof Structures Element

            oXml.OutOfElem() 'Outof Environment Element

        End Sub

        Public Overrides Sub SaveSimulationXml(ByRef oXml As Interfaces.StdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            oXml.AddChildElement("Environment")
            oXml.IntoElem()

            oXml.AddChildElement("PhysicsTimeStep", m_snPhysicsTimeStep.ActualValue)
            oXml.AddChildElement("Gravity", m_snGravity.ActualValue)
            oXml.AddChildElement("MouseSpringStiffness", m_snMouseSpringStiffness.ActualValue)
            oXml.AddChildElement("MouseSpringDamping", m_snMouseSpringDamping.ActualValue)

            oXml.AddChildElement("LinearCompliance", m_snLinearCompliance.ActualValue)
            oXml.AddChildElement("LinearDamping", m_snLinearDamping.ActualValue)
            oXml.AddChildElement("AngularCompliance", m_snAngularCompliance.ActualValue)
            oXml.AddChildElement("AngularDamping", m_snAngularDamping.ActualValue)
            oXml.AddChildElement("LinearKineticLoss", m_snLinearKineticLoss.ActualValue)
            oXml.AddChildElement("AngularKineticLoss", m_snAngularKineticLoss.ActualValue)

            m_snRecFieldSelRadius.SaveSimulationXml(oXml, Nothing, "RecFieldSelRadius")

            oXml.AddChildElement("SimulateHydrodynamics", m_bSimulateHydrodynamics)

            oXml.AddChildElement("MassUnits", m_eMassUnits.ToString())
            oXml.AddChildElement("DistanceUnits", m_eDistanceUnits.ToString())

            oXml.AddChildElement("AutoGenerateRandomSeed", m_bAutoGenerateRandomSeed)
            oXml.AddChildElement("ManualRandomSeed", m_iManualRandomSeed)

            'If we are saving the config file for downloading to a  robot then we should only
            'save the selected organism and none of the other organisms or structures.
            If m_aryOdorTypes.Count > 0 Then
                oXml.AddChildElement("OdorTypes")
                oXml.IntoElem()
                Dim doOdor As DataObjects.Physical.OdorType
                For Each deEntry As DictionaryEntry In m_aryOdorTypes
                    doOdor = DirectCast(deEntry.Value, DataObjects.Physical.OdorType)
                    doOdor.SaveSimulationXml(oXml, Me)
                Next
                oXml.OutOfElem() 'Outof Organisms Element
            End If

            oXml.AddChildElement("Organisms")
            oXml.IntoElem()
            Dim doOrganism As DataObjects.Physical.Organism
            For Each deEntry As DictionaryEntry In m_aryOrganisms
                doOrganism = DirectCast(deEntry.Value, DataObjects.Physical.Organism)
                doOrganism.SaveSimulationXml(oXml, Me)
            Next
            oXml.OutOfElem() 'Outof Organisms Element

            oXml.AddChildElement("Structures")
            oXml.IntoElem()
            Dim doStructure As DataObjects.Physical.PhysicalStructure
            For Each deEntry As DictionaryEntry In m_aryStructures
                doStructure = DirectCast(deEntry.Value, DataObjects.Physical.PhysicalStructure)
                doStructure.SaveSimulationXml(oXml, Me)
            Next
            oXml.OutOfElem() 'Outof Structures Element

            oXml.OutOfElem() 'Outof Environment Element

        End Sub

        Public Overrides Sub InitializeAfterLoad()
            MyBase.InitializeAfterLoad()

            Dim newStructure As DataObjects.Physical.PhysicalStructure
            For Each deEntry As DictionaryEntry In m_aryOrganisms
                newStructure = DirectCast(deEntry.Value, PhysicalStructure)
                newStructure.InitializeAfterLoad()
            Next

            For Each deEntry As DictionaryEntry In m_aryStructures
                newStructure = DirectCast(deEntry.Value, PhysicalStructure)
                newStructure.InitializeAfterLoad()
            Next

        End Sub

        Public Overrides Function FindObjectByID(ByVal strID As String) As Framework.DataObject

            Dim doObject As AnimatGUI.Framework.DataObject = MyBase.FindObjectByID(strID)
            If doObject Is Nothing AndAlso Not m_aryOrganisms Is Nothing Then doObject = m_aryOrganisms.FindObjectByID(strID)
            If doObject Is Nothing AndAlso Not m_aryStructures Is Nothing Then doObject = m_aryStructures.FindObjectByID(strID)
            If doObject Is Nothing AndAlso Not m_aryOdorTypes Is Nothing Then doObject = m_aryOdorTypes.FindObjectByID(strID)
            Return doObject

        End Function

        Public Overrides Sub InitializeSimulationReferences()
            'The environment is really just a pointer to the simulation object in the sim.
            If m_doInterface Is Nothing AndAlso Not Util.Application.SimulationInterface Is Nothing AndAlso Util.Application.SimulationInterface.SimOpen Then
                m_doInterface = New Interfaces.DataObjectInterface(Util.Application.SimulationInterface, Util.Simulation.ID)
            End If

            Dim doObject As AnimatGUI.Framework.DataObject
            For Each deEntry As DictionaryEntry In m_aryOdorTypes
                doObject = DirectCast(deEntry.Value, AnimatGUI.Framework.DataObject)
                doObject.InitializeSimulationReferences()
            Next

            For Each deEntry As DictionaryEntry In m_aryOrganisms
                doObject = DirectCast(deEntry.Value, AnimatGUI.Framework.DataObject)
                doObject.InitializeSimulationReferences()
            Next

            For Each deEntry As DictionaryEntry In m_aryStructures
                doObject = DirectCast(deEntry.Value, AnimatGUI.Framework.DataObject)
                doObject.InitializeSimulationReferences()
            Next

        End Sub

#End Region

        Public Overrides Function FindDragObject(ByVal strStructureName As String, ByVal strDataItemID As String, Optional ByVal bThrowError As Boolean = True) As DragObject
            Return Nothing
        End Function

#End Region

#Region " Events "

        Protected Sub OnNewOrganism(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                Dim doOrganism As New DataObjects.Physical.Organism(Me)

                m_iNewOrganismCount = m_iNewOrganismCount + 1
                doOrganism.Name = "Organism_" & m_iNewOrganismCount
                Me.Organisms.Add(doOrganism.ID, doOrganism)

                doOrganism.CreateWorkspaceTreeView(Me, m_tnOrganisms)
                doOrganism.WorkspaceNode.ExpandAll()
                Util.ProjectWorkspace.TreeView.SelectedNode = doOrganism.WorkspaceNode

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnNewStructure(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                Dim doStructure As New DataObjects.Physical.PhysicalStructure(Me)

                m_iNewStructureCount = m_iNewStructureCount + 1
                doStructure.Name = "Structure_" & m_iNewStructureCount
                Me.Structures.Add(doStructure.ID, doStructure)

                doStructure.CreateWorkspaceTreeView(Me, m_tnStructures)
                doStructure.SelectItem()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnAddGround(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                'If Not Util.Environment.GroundSurface Is Nothing Then
                '    Throw New System.Exception("You can only add one ground surface")
                'End If

                'm_doGround = New DataObjects.Physical.GroundSurface(Me)
                'm_doGround.ID = "Ground"
                'm_doGround.Name = "Ground"

                'm_doGround.CreateWorkspaceTreeView(Util.Simulation, Util.ProjectWorkspace)
                'Util.ProjectWorkspace.TreeView.SelectedNode = m_doGround.WorkspaceStructureNode

                'Util.Application.EnableDefaultMenuItem("Edit", "Add Ground", False)
                'Util.Application.EnableDefaultToolbarItem("Add Ground", False)

                'Util.Application.SaveProject(Util.Application.ProjectFile)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnAddWater(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                'If Not Util.Environment.WaterSurface Is Nothing Then
                '    Throw New System.Exception("You can only add one water surface")
                'End If

                'm_doWater = New DataObjects.Physical.WaterSurface(Me)
                'm_doWater.ID = "Water"
                'm_doWater.Name = "Water"

                'm_doWater.CreateWorkspaceTreeView(Util.Simulation, Util.ProjectWorkspace)
                'Util.ProjectWorkspace.TreeView.SelectedNode = m_doWater.WorkspaceStructureNode
                'm_bSimulateHydrodynamics = True

                'Util.Application.EnableDefaultMenuItem("Edit", "Add Water", False)
                'Util.Application.EnableDefaultToolbarItem("Add Water", False)

                'Util.Application.SaveProject(Util.Application.ProjectFile)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

    End Class

End Namespace


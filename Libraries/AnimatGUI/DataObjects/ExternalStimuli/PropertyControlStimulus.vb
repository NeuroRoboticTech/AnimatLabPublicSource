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

Namespace DataObjects.ExternalStimuli

    Public Class PropertyControlStimulus
        Inherits AnimatGUI.DataObjects.ExternalStimuli.Stimulus

#Region " Attributes "

        Protected m_thLinkedObject As AnimatGUI.TypeHelpers.LinkedDataObjectTree
        Protected m_thLinkedProperty As AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList
        Protected m_fltSetThreshold As Single = 0.5
        Protected m_fltInitialValue As Single = 0
        Protected m_fltFinalValue As Single = 1
        Protected m_fltStimulusValue As Single = 1

        'Only used during loading
        Protected m_strLinkedObjectID As String = ""
        Protected m_strLinkedObjectProperty As String = ""

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Overrides Property StimulatedItem() As Framework.DataObject
            Get
                Return m_doStimulatedItem
            End Get
            Set(value As Framework.DataObject)
                DisconnectItemEvents()
                m_doStimulatedItem = value
                Me.LinkedObject = New TypeHelpers.LinkedDataObjectTree(value)
                Me.LinkedProperty = New TypeHelpers.LinkedDataObjectPropertiesList(value)
                ConnectItemEvents()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property LinkedObject() As AnimatGUI.TypeHelpers.LinkedDataObjectTree
            Get
                Return m_thLinkedObject
            End Get
            Set(ByVal Value As AnimatGUI.TypeHelpers.LinkedDataObjectTree)
                If Not Value Is Nothing AndAlso Not Value.Item Is Nothing Then
                    SetSimData("TargetID", Value.Item.ID, True)
                Else
                    SetSimData("TargetID", "", True)
                End If

                m_thLinkedObject = Value

                If Not m_thLinkedObject Is Nothing AndAlso Not m_thLinkedObject.Item Is m_doStimulatedItem Then
                    m_thLinkedProperty = New TypeHelpers.LinkedDataObjectPropertiesList(Nothing)
                    Me.StimulatedItem = m_thLinkedObject.Item
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property LinkedProperty() As AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList
            Get
                Return m_thLinkedProperty
            End Get
            Set(ByVal Value As AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList)
                If Not Value Is Nothing AndAlso Not Value.Item Is Nothing AndAlso Not Value.PropertyName Is Nothing Then
                    SetSimData("PropertyName", Value.PropertyName, True)
                Else
                    SetSimData("PropertyName", "", True)
                End If

                m_thLinkedProperty = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property LinkedPropertyName() As String
            Get
                If Not m_thLinkedProperty Is Nothing AndAlso Not m_thLinkedProperty.PropertyName Is Nothing Then
                    Return m_thLinkedProperty.PropertyName
                Else
                    Return ""
                End If
            End Get
            Set(value As String)
                If m_thLinkedObject Is Nothing OrElse m_thLinkedObject.Item Is Nothing Then
                    Throw New System.Exception("You cannot set the linked object property name until the linked object is set.")
                End If

                Me.LinkedProperty = New TypeHelpers.LinkedDataObjectPropertiesList(m_thLinkedObject.Item, value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property SetThreshold() As Single
            Get
                Return m_fltSetThreshold
            End Get
            Set(ByVal Value As Single)
                If Value < 0 Then
                    Throw New System.Exception("Set threshold value must be greater than 0.")
                End If

                SetSimData("SetThreshold", Value.ToString, True)
                m_fltSetThreshold = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property InitialValue() As Single
            Get
                Return m_fltInitialValue
            End Get
            Set(ByVal Value As Single)
                SetSimData("InitialValue", Value.ToString, True)
                m_fltInitialValue = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property FinalValue() As Single
            Get
                Return m_fltFinalValue
            End Get
            Set(ByVal Value As Single)
                SetSimData("FinalValue", Value.ToString, True)
                m_fltFinalValue = Value
            End Set
        End Property

        Public Overridable Property StimulusValue() As Single
            Get
                Return m_fltStimulusValue
            End Get
            Set(ByVal Value As Single)
                SetStimulusValue(Value.ToString)
                m_fltStimulusValue = Value
            End Set
        End Property

        Public Overrides Property ValueType() As enumValueType
            Get
                Return m_eValueType
            End Get
            Set(ByVal Value As enumValueType)
                m_eValueType = Value

                SetSimData("ValueType", Value.ToString, True)
                SetStimulusValue()

                If Not Util.ProjectWorkspace Is Nothing Then
                    Util.ProjectWorkspace.RefreshProperties()
                End If
            End Set
        End Property

        Public Overrides Property Equation() As String
            Get
                Return m_strEquation
            End Get
            Set(ByVal Value As String)
                If Value.Trim.Length = 0 Then
                    Throw New System.Exception("Equation cannot be blank.")
                End If

                SetStimulusValue(Value)
                m_strEquation = Value
            End Set
        End Property

        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Property Control"
            End Get
        End Property

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.EnablerStimulus_Small.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property Description() As String
            Get
                Return "This stimulus can set any property of any object within the system."
            End Get
        End Property

        Public Overrides ReadOnly Property StimulusClassType() As String
            Get
                Return "PropertyControlStimulus"
            End Get
        End Property

        Public Overrides ReadOnly Property DragImageName() As String
            Get
                Return "AnimatGUI.EnablerStimulus_Small.gif"
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            m_thLinkedObject = New AnimatGUI.TypeHelpers.LinkedDataObjectTree(Nothing)
            m_thLinkedProperty = New AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList(Nothing)
        End Sub

        Protected Overrides Sub SetSimEquation(ByVal strEquation As String)
            If strEquation.Trim.Length > 0 Then
                'Lets verify the equation before we use it.
                'We need to convert the infix equation to postfix
                Dim oMathEval As New MathStringEval
                oMathEval.AddVariable("t")
                oMathEval.Equation = strEquation
                oMathEval.Parse()

                SetSimData("Equation", oMathEval.PostFix, True)
            Else
                SetSimData("Equation", "0", True)
            End If
        End Sub

        Protected Sub SetStimulusValue(Optional ByVal strEquation As String = "")

            If m_eValueType = enumValueType.Constant Then
                If strEquation.Trim.Length = 0 Then strEquation = Me.StimulusValue.ToString
                SetSimData("Equation", strEquation, True)
            Else
                If strEquation.Trim.Length = 0 Then strEquation = Me.Equation
                SetSimEquation(strEquation)
            End If

        End Sub

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim bpPart As DataObjects.ExternalStimuli.PropertyControlStimulus = DirectCast(doOriginal, DataObjects.ExternalStimuli.PropertyControlStimulus)
            bpPart.m_thLinkedObject = DirectCast(m_thLinkedObject.Clone(Me, bCutData, doRoot), TypeHelpers.LinkedDataObjectTree)
            bpPart.m_thLinkedProperty = DirectCast(m_thLinkedProperty.Clone(Me, bCutData, doRoot), TypeHelpers.LinkedDataObjectPropertiesList)
            bpPart.m_fltSetThreshold = m_fltSetThreshold
            bpPart.m_fltInitialValue = m_fltInitialValue
            bpPart.m_fltFinalValue = m_fltFinalValue
            bpPart.m_fltStimulusValue = m_fltStimulusValue

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim doStim As DataObjects.ExternalStimuli.PropertyControlStimulus = New DataObjects.ExternalStimuli.PropertyControlStimulus(doParent)
            CloneInternal(doStim, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then doStim.AfterClone(Me, bCutData, doRoot, doStim)
            Return doStim
        End Function

        Public Overrides Function GetSimulationXml(ByVal strName As String, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing) As String

            If m_thLinkedObject Is Nothing OrElse m_thLinkedObject.Item Is Nothing Then
                Throw New System.Exception("No target object was defined for the stimulus '" & m_strName & "'.")
            End If

            Dim oXml As ManagedAnimatInterfaces.IStdXml = Util.Application.CreateStdXml()
            oXml.AddElement("Root")
            SaveSimulationXml(oXml, nmParentControl, strName)

            Return oXml.Serialize()
        End Function

        Public Overrides Sub InitializeAfterLoad()
            MyBase.InitializeAfterLoad()

            If m_strLinkedObjectID.Trim.Length > 0 Then
                Me.StimulatedItem = Util.Simulation.FindObjectByID(m_strLinkedObjectID)
                Me.LinkedProperty = New TypeHelpers.LinkedDataObjectPropertiesList(Me.StimulatedItem, m_strLinkedObjectProperty)
            End If
        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")

           If m_thLinkedObject Is Nothing OrElse m_thLinkedObject.Item Is Nothing Then
                Throw New System.Exception("No target object was defined for the stimulus '" & m_strName & "'.")
            End If

            oXml.AddChildElement("Stimulus")

            oXml.IntoElem()
            oXml.AddChildElement("ID", m_strID)
            oXml.AddChildElement("Name", m_strName)
            oXml.AddChildElement("AlwaysActive", m_bAlwaysActive)
            oXml.AddChildElement("Enabled", m_bEnabled)

            oXml.AddChildElement("ModuleName", Me.StimulusModuleName)
            oXml.AddChildElement("Type", Me.StimulusClassType)

            oXml.AddChildElement("TargetID", Me.LinkedObject.Item.ID)
            oXml.AddChildElement("PropertyName", Me.LinkedPropertyName)
            oXml.AddChildElement("SetThreshold", Me.SetThreshold)
            oXml.AddChildElement("InitialValue", Me.InitialValue)
            oXml.AddChildElement("FinalValue", Me.FinalValue)

            oXml.AddChildElement("StartTime", m_snStartTime.ActualValue)
            oXml.AddChildElement("EndTime", m_snEndTime.ActualValue)

            If m_eValueType = enumValueType.Constant Then
                oXml.AddChildElement("Equation", m_fltStimulusValue)
            Else
                'We need to convert the infix equation to postfix
                Dim oMathEval As New MathStringEval
                oMathEval.AddVariable("t")
                oMathEval.Equation = m_strEquation
                oMathEval.Parse()
                oXml.AddChildElement("Equation", oMathEval.PostFix)
            End If

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub Automation_SetLinkedItem(ByVal strItemPath As String, ByVal strLinkedItemPath As String)

            Dim tnLinkedNode As Crownwood.DotNetMagic.Controls.Node = Util.FindTreeNodeByPath(strLinkedItemPath, Util.ProjectWorkspace.TreeView.Nodes)

            If tnLinkedNode Is Nothing OrElse tnLinkedNode.Tag Is Nothing OrElse Not Util.IsTypeOf(tnLinkedNode.Tag.GetType, GetType(Framework.DataObject), False) Then
                Throw New System.Exception("The path to the specified linked node was not the correct node type.")
            End If

            Dim doLinkedObject As Framework.DataObject = DirectCast(tnLinkedNode.Tag, Framework.DataObject)

            Me.LinkedObject = New TypeHelpers.LinkedDataObjectTree(doLinkedObject)

            Util.ProjectWorkspace.RefreshProperties()
        End Sub

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Linked Object", GetType(AnimatGUI.TypeHelpers.LinkedDataObjectTree), "LinkedObject", _
                                        "Stimulus Properties", "Sets the object that is associated with this connector.", m_thLinkedObject, _
                                        GetType(AnimatGUI.TypeHelpers.DropDownTreeEditor), _
                                        GetType(AnimatGUI.TypeHelpers.LinkedDataObjectTypeConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Linked Property", GetType(AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList), "LinkedProperty", _
                                        "Stimulus Properties", "Determines the property that is set by this controller.", m_thLinkedProperty, _
                                        GetType(AnimatGUI.TypeHelpers.DropDownListEditor), _
                                        GetType(AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesTypeConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Set Threshold", m_fltSetThreshold.GetType(), "SetThreshold", _
                                        "Stimulus Properties", "Threshold at which the property value will be set. This is the difference between the current value and value when it was last set", Me.SetThreshold))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Start Value", m_fltInitialValue.GetType(), "InitialValue", _
                                        "Stimulus Properties", "Initial value the property control will set for the property when simulation starts", Me.InitialValue))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("End Value", m_fltFinalValue.GetType(), "FinalValue", _
                                        "Stimulus Properties", "Final value the property control will set for the property when simulation ends", Me.FinalValue))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Value Type", m_eValueType.GetType(), "ValueType", _
                                        "Stimulus Properties", "Determines if a constant or an equation is used to set the object property value.", m_eValueType))

            If m_eValueType = enumValueType.Equation Then
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Equation", m_strEquation.GetType(), "Equation", _
                                            "Stimulus Properties", "If setup to use equations, then this is the one used.", m_strEquation))
            Else
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Stimulus Value", m_fltStimulusValue.GetType(), "StimulusValue", _
                                            "Stimulus Properties", "If setup to use constant, then this is the one used.", m_fltStimulusValue))
            End If

        End Sub

        Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.LoadData(oXml)

            oXml.IntoElem()
            m_strLinkedObjectID = oXml.GetChildString("LinkedDataObjectID", "")
            m_strLinkedObjectProperty = oXml.GetChildString("LinkedDataObjectProperty", "")
            m_fltSetThreshold = oXml.GetChildFloat("SetThreshold", m_fltSetThreshold)
            m_fltInitialValue = oXml.GetChildFloat("InitialValue", m_fltInitialValue)
            m_fltFinalValue = oXml.GetChildFloat("FinalValue", m_fltFinalValue)
            m_fltStimulusValue = oXml.GetChildFloat("StimulusValue", m_fltStimulusValue)
            oXml.OutOfElem()

            Me.IsDirty = False
        End Sub

        Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.SaveData(oXml)

            oXml.IntoElem()

            If Not m_thLinkedObject Is Nothing AndAlso Not m_thLinkedObject.Item Is Nothing Then
                oXml.AddChildElement("LinkedDataObjectID", m_thLinkedObject.Item.ID)
                oXml.AddChildElement("LinkedDataObjectProperty", Me.LinkedPropertyName)
            End If

            oXml.AddChildElement("SetThreshold", m_fltSetThreshold)
            oXml.AddChildElement("InitialValue", m_fltInitialValue)
            oXml.AddChildElement("FinalValue", m_fltFinalValue)
            oXml.AddChildElement("StimulusValue", m_fltStimulusValue)

            oXml.OutOfElem() ' Outof Node Element

        End Sub

#End Region

#End Region

    End Class

End Namespace


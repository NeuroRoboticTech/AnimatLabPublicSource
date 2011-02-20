Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports Crownwood.Magic.Controls
Imports AnimatTools.Framework

Namespace DataObjects.Behavior.Nodes

    Public Class Enabler
        Inherits Behavior.Nodes.BodyPart

#Region " Attributes "

        Protected m_fltThreshold As Single = 0
        Protected m_bEnableAboveThreshold As Boolean = True

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Overrides Property ParentEditor() As AnimatTools.Forms.Behavior.Editor
            Get
                Return m_ParentEditor
            End Get
            Set(ByVal Value As AnimatTools.Forms.Behavior.Editor)
                m_ParentEditor = Value

                If Not m_ParentEditor Is Nothing AndAlso Not m_ParentEditor.Organism Is Nothing Then
                    Me.Organism = m_ParentEditor.Organism
                    m_thLinkedPart = New AnimatTools.TypeHelpers.LinkedBodyPartTree(m_ParentEditor.Organism, Nothing, m_tpBodyPartType)
                End If
            End Set
        End Property

        Public Overrides Property LinkedPart() As AnimatTools.TypeHelpers.LinkedBodyPart
            Get
                Return m_thLinkedPart
            End Get
            Set(ByVal Value As AnimatTools.TypeHelpers.LinkedBodyPart)

                If Not Value Is Nothing Then
                    If Not Value.BodyPart Is Nothing Then
                        If Value.BodyPart.DataTypes.DataTypes.Contains("Enable") Then
                            m_thLinkedPart = Value
                        Else
                            Throw New System.Exception("The body part " & Value.BodyPart.Name & " does not have an enable property. Please choose a different part.")
                        End If
                    End If
                End If
            End Set
        End Property

        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Enabler"
            End Get
        End Property

        Public Overrides ReadOnly Property ImageName() As String
            Get
                Return "AnimatTools.EnablerNode.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property IncomingDataType() As AnimatTools.DataObjects.DataType
            Get
                If Not m_thLinkedPart Is Nothing AndAlso Not m_thLinkedPart.BodyPart Is Nothing Then
                    Return m_thIncomingDataType
                End If
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property Threshold() As Single
            Get
                Return m_fltThreshold
            End Get
            Set(ByVal Value As Single)
                m_fltThreshold = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property EnableAboveThreshold() As Boolean
            Get
                Return m_bEnableAboveThreshold
            End Get
            Set(ByVal Value As Boolean)
                m_bEnableAboveThreshold = Value
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            Try

                Shape = Behavior.Node.enumShape.SummingJunction
                Size = New SizeF(40, 40)
                Me.DrawColor = Color.Red
                Me.DrawWidth = 2

                Dim myAssembly As System.Reflection.Assembly
                myAssembly = System.Reflection.Assembly.Load("AnimatTools")

                Me.Image = AnimatTools.Framework.ImageManager.LoadImage(myAssembly, "AnimatTools.EnablerNode.gif")
                Me.Name = "Enabler Item"

                Me.Font = New Font("Arial", 12, FontStyle.Bold)
                Me.Description = "Allows the other items in the behavioral system to enable/disable any physical part that has an enable property. This includes things like muscles and joints."

                m_tpBodyPartType = GetType(AnimatTools.DataObjects.Physical.BodyPart)

                m_thDataTypes.DataTypes.Clear()
                m_thDataTypes.DataTypes.Add(New AnimatTools.DataObjects.DataType("Enable", "Enable", "", "", 0, 1))
                m_thDataTypes.ID = "Enable"

                m_thIncomingDataType = New AnimatTools.DataObjects.DataType("Enable", "Enable", "", "", 0, 1)

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatTools.Framework.DataObject) As AnimatTools.Framework.DataObject
            Dim oNewNode As New Behavior.Nodes.Enabler(doParent)
            oNewNode.CloneInternal(Me)
            Return oNewNode
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatTools.Framework.DataObject)
            MyBase.CloneInternal(doOriginal)

            Dim bpPart As Nodes.Enabler = DirectCast(doOriginal, Nodes.Enabler)
            bpPart.m_fltThreshold = Me.m_fltThreshold
            bpPart.m_bEnableAboveThreshold = Me.m_bEnableAboveThreshold

        End Sub

        Public Overrides Sub CheckForErrors()
            'There can be no "Errors" for graphical nodes
        End Sub

        Public Overrides Function CreateDataItemTreeView(ByVal frmDataItem As Forms.Tools.SelectDataItem, ByVal tnParent As TreeNode, ByVal tpTemplatePartType As Type) As TreeNode
        End Function


        Public Overrides Sub InitializeAfterLoad(ByVal iAttempt As Integer)

            Try
                MyBase.InitializeAfterLoad(iAttempt)

                If (m_bInitialized) Then
                    Dim bpPart As AnimatTools.DataObjects.Physical.BodyPart
                    If (m_strLinkedBodyPartID.Length > 0) Then
                        bpPart = m_doOrganism.FindBodyPart(m_strLinkedBodyPartID, False)

                        m_thLinkedPart = New AnimatTools.TypeHelpers.LinkedBodyPartTree(m_doOrganism, bpPart, m_tpBodyPartType)
                    End If
                End If

            Catch ex As System.Exception
                m_bInitialized = False
                If (iAttempt = 1) Then
                    AnimatTools.Framework.Util.DisplayError(ex)
                End If
            End Try
        End Sub

#Region " DataObject Methods "

        Protected Overrides Sub BuildProperties()
            MyBase.BuildProperties()

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Part ID", GetType(AnimatTools.TypeHelpers.LinkedBodyPart), "LinkedPart", _
                                        "Node Properties", "Associates this node to an ID of a body part that exists within the body of the organism.", _
                                        m_thLinkedPart, GetType(AnimatTools.TypeHelpers.DropDownTreeEditor), GetType(AnimatTools.TypeHelpers.LinkedBodyPartTypeConverter)))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Enable Above Threshold", GetType(Boolean), "EnableAboveThreshold", _
                                        "Node Properties", "If this is true and the the input value is above the threshold value then the body part is enabled. Other wise it is disabled", m_bEnableAboveThreshold))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Threshold", GetType(Single), "Threshold", _
                                        "Node Properties", "If enable above threhsold true and the the input value is above the threshold value then the body part is enabled. Other wise it is disabled", m_fltThreshold))

        End Sub

        Public Overrides Sub LoadData(ByRef oXml As Interfaces.StdXml)
            MyBase.LoadData(oXml)

            oXml.IntoElem()
            m_bEnableAboveThreshold = oXml.GetChildBool("EnableAboveThreshold", True)
            m_fltThreshold = oXml.GetChildFloat("Threshold", 0)
            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveData(ByRef oXml As Interfaces.StdXml)
            MyBase.SaveData(oXml)

            oXml.IntoElem() 'Into Node Element

            oXml.AddChildElement("EnableAboveThreshold", m_bEnableAboveThreshold)
            oXml.AddChildElement("Threshold", m_fltThreshold)

            oXml.OutOfElem() ' Outof Node Element
        End Sub

#End Region

#End Region

    End Class

End Namespace

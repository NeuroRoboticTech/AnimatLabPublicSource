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

Namespace DataObjects.Behavior.Nodes

    Public MustInherit Class BodyPart
        Inherits AnimatGUI.DataObjects.Behavior.Node

#Region " Attributes "

        Protected m_thLinkedPart As AnimatGUI.TypeHelpers.LinkedBodyPart
        Protected m_tpBodyPartType As System.Type

        'Only used during loading
        Protected m_strLinkedBodyPartID As String

#End Region

#Region " Properties "

        Public Overrides Property Organism As Physical.Organism
            Get
                Return MyBase.Organism
            End Get
            Set(ByVal value As Physical.Organism)
                MyBase.Organism = value

                If Not Me.Organism Is Nothing Then
                    m_thLinkedPart = CreateBodyPartList(Me.Organism, Nothing, m_tpBodyPartType)
                    m_thDataTypes = New AnimatGUI.TypeHelpers.DataTypeID(Me)
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property LinkedPart() As AnimatGUI.TypeHelpers.LinkedBodyPart
            Get
                Return m_thLinkedPart
            End Get
            Set(ByVal Value As AnimatGUI.TypeHelpers.LinkedBodyPart)
                m_thLinkedPart = Value
                SetDataType()
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Body Part"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property NeuralModuleType() As System.Type
            Get
                Return Nothing
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property IsPhysicsEngineNode() As Boolean
            Get
                Return True
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property AllowStimulus() As Boolean
            Get
                Return False
            End Get
        End Property

        'For some objects, like body parts, what we really want to chart is not the behavior.bodypart object, but the underlying linked object.
        'This gives the object a way to return a reference to someing else to be charted instead of itself if this is needed.
        <Browsable(False)> _
        Public Overrides ReadOnly Property DataColumnItem() As DragObject
            Get
                If m_thLinkedPart Is Nothing OrElse m_thLinkedPart.BodyPart Is Nothing Then
                    Throw New System.Exception("You can not add a body part for graphing until you have set the part ID that it is associated to.")
                End If

                Return m_thLinkedPart.BodyPart
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property CanBeCharted() As Boolean
            Get
                If m_thLinkedPart Is Nothing OrElse m_thLinkedPart.BodyPart Is Nothing Then
                    Return False
                Else
                    Return True
                End If
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property IncomingDataType() As AnimatGUI.DataObjects.DataType
            Get
                If Not m_thLinkedPart Is Nothing AndAlso Not m_thLinkedPart.BodyPart Is Nothing Then
                    Return m_thLinkedPart.BodyPart.IncomingDataType
                End If
                Return Nothing
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property LinkedID() As String
            Get
                If Not m_thLinkedPart Is Nothing AndAlso Not m_thLinkedPart.BodyPart Is Nothing Then
                    Return m_thLinkedPart.BodyPart.ID
                Else
                    Return Me.ID
                End If
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            Try

                m_thLinkedPart = CreateBodyPartList(Me)

                Shape = AnimatGUI.DataObjects.Behavior.Node.enumShape.Rectangle
                Size = New System.Drawing.SizeF(50, 50)
                Me.DrawColor = Color.Transparent
                Me.FillColor = Color.White
                Me.AutoSize = AnimatGUI.DataObjects.Behavior.Node.enumAutoSize.ImageToNode
                Me.Font = New Font("Arial", 14, FontStyle.Bold)
                Me.Alignment = enumAlignment.CenterBottom

                Dim myAssembly As System.Reflection.Assembly
                myAssembly = System.Reflection.Assembly.Load("AnimatGUI")
                Me.WorkspaceImage = AnimatGUI.Framework.ImageManager.LoadImage(myAssembly, Me.WorkspaceImageName)
                Me.DragImage = AnimatGUI.Framework.ImageManager.LoadImage(myAssembly, Me.DragImageName, False)

                AddCompatibleLink(New AnimatGUI.DataObjects.Behavior.Links.Adapter(Nothing))

                m_tpBodyPartType = GetType(AnimatGUI.DataObjects.Physical.BodyPart)
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Overridable Overloads Function CreateBodyPartList(ByVal doParent As AnimatGUI.Framework.DataObject) As TypeHelpers.LinkedBodyPart
            Return New AnimatGUI.TypeHelpers.LinkedBodyPartTree(doParent)
        End Function

        Protected Overridable Overloads Function CreateBodyPartList(ByVal doStruct As Physical.PhysicalStructure, ByVal doBodyPart As Physical.BodyPart, ByVal tpBodyPartType As System.Type) As TypeHelpers.LinkedBodyPart
            Return New AnimatGUI.TypeHelpers.LinkedBodyPartTree(doStruct, doBodyPart, tpBodyPartType)
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim OrigNode As Data = DirectCast(doOriginal, Data)

            Dim bpPart As Nodes.BodyPart = DirectCast(OrigNode, Nodes.BodyPart)
            bpPart.m_thLinkedPart = DirectCast(m_thLinkedPart.Clone(Me, bCutData, doRoot), TypeHelpers.LinkedBodyPart)

        End Sub

        Protected Sub SetDataType()
            If Not m_thLinkedPart.BodyPart Is Nothing AndAlso Not m_thLinkedPart.BodyPart.DataTypes Is Nothing Then
                m_thDataTypes = DirectCast(m_thLinkedPart.BodyPart.DataTypes.Clone(m_thLinkedPart.BodyPart.DataTypes.Parent, False, Nothing), TypeHelpers.DataTypeID)

                'Go through and change all of the adapters connected to this body part.
                For Each deItem As System.Collections.DictionaryEntry In Me.m_aryOutLinks
                    If Util.IsTypeOf(deItem.Value.GetType(), GetType(AnimatGUI.DataObjects.Behavior.Links.Adapter), False) Then
                        Dim blAdapter As AnimatGUI.DataObjects.Behavior.Links.Adapter = DirectCast(deItem.Value, AnimatGUI.DataObjects.Behavior.Links.Adapter)

                        If Not blAdapter.Destination Is Nothing Then
                            blAdapter.Destination.DataTypes = DirectCast(m_thDataTypes.Clone(blAdapter.Destination, False, Nothing), TypeHelpers.DataTypeID)
                        End If
                    End If
                Next
            Else
                m_thDataTypes = New AnimatGUI.TypeHelpers.DataTypeID(Me)
            End If

            CheckForErrors()
        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            If Not m_thLinkedPart Is Nothing Then m_thLinkedPart.ClearIsDirty()
        End Sub

        Public Overrides Sub CheckCanAttachAdapter()
            'Only allow an adapter if we have a linked body part.
            If Not (Not m_thLinkedPart Is Nothing AndAlso Not m_thLinkedPart.BodyPart Is Nothing) Then
                Throw New System.Exception("You must specify a linked body part before you can add an adapter to this node.")
            End If
        End Sub

        Public Overrides Function NeedToUpdateAdapterID(ByVal propInfo As System.Reflection.PropertyInfo) As Boolean
            If propInfo.Name = "LinkedPart" Then
                Return True
            End If
        End Function

        Public Overrides Sub LoadData(ByRef oXml As Interfaces.StdXml)
            MyBase.LoadData(oXml)

            oXml.IntoElem()
            m_strLinkedBodyPartID = Util.LoadID(oXml, "LinkedBodyPart", True, "")
            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveData(ByRef oXml As Interfaces.StdXml)
            MyBase.SaveData(oXml)

            oXml.IntoElem() 'Into Node Element

            If Not m_thLinkedPart Is Nothing AndAlso Not m_thLinkedPart.BodyPart Is Nothing Then
                oXml.AddChildElement("LinkedBodyPartID", m_thLinkedPart.BodyPart.ID)
            End If

            oXml.OutOfElem() ' Outof Node Element
        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            propTable.Properties.Remove("ID")
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("ID", Me.LinkedID.GetType(), "LinkedID", _
                                        "Node Properties", "ID", Me.LinkedID, True))
        End Sub

#End Region

    End Class

End Namespace


Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.ComponentModel.Design.Serialization
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports Crownwood.Magic.Controls
Imports AnimatTools.Framework

Namespace DataObjects.Behavior

    Public MustInherit Class NeuronType
        Inherits AnimatTools.DataObjects.Behavior.Data

#Region " Attributes "

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Overrides Property Name() As String
            Get
                Return m_strName
            End Get
            Set(ByVal Value As String)
                m_strName = Value

                If Not Me.TreeNode Is Nothing Then
                    Me.TreeNode.Text = m_strName
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides Property Text() As String
            Get
                Return m_strName
            End Get
            Set(ByVal Value As String)
                m_strName = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides Property ItemName() As String
            Get
                Return Me.Text
            End Get
            Set(ByVal Value As String)
                Me.Text = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property NeuralModuleType() As System.Type
            Get
                Return GetType(eBrainGUI.DataObjects.Behavior.NeuralModule)
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides Property ViewSubProperties() As Boolean
            Get
                Return False
            End Get
            Set(ByVal Value As Boolean)
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatTools.Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatTools.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatTools.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim OrigLink As AnimatTools.DataObjects.Behavior.Data = DirectCast(doOriginal, AnimatTools.DataObjects.Behavior.Data)

            Dim blOrig As NeuronType = DirectCast(OrigLink, NeuronType)

        End Sub

        Public Overrides Sub FailedToInitialize()
        End Sub

        Public Overrides Function FindDragObject(ByVal strStructureName As String, ByVal strDataItemID As String, Optional ByVal bThrowError As Boolean = True) As AnimatTools.DataObjects.DragObject
        End Function

        Public Overrides Function ToString() As String
            Return Me.Name
        End Function

#Region " DataObject Methods "

        Protected Overrides Sub BuildProperties()

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Neuron Type", GetType(String), "TypeName", _
                                        "Neuron Type Properties", "Returns the type of this neuron.", TypeName(), True))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Name", m_strName.GetType(), "Name", _
                                        "Neuron Type Properties", "Sets the name of this neuron type.", m_strName, _
                                        GetType(AnimatTools.TypeHelpers.MultiLineStringTypeEditor)))

        End Sub

        Public Overrides Sub SaveSimulationXml(ByRef oXml As AnimatTools.Interfaces.StdXml, Optional ByRef nmParentControl As AnimatTools.Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            oXml.AddChildElement("Type")
            oXml.IntoElem()

            oXml.AddChildElement("ID", Me.ID)
            oXml.AddChildElement("Name", Me.Text)
            oXml.AddChildElement("Enabled", Me.Enabled)

            oXml.OutOfElem() 'Outof Type

        End Sub

        Public Overrides Sub LoadData(ByRef oXml As AnimatTools.Interfaces.StdXml)
            Try
                Me.IsDirty = False

                oXml.IntoElem()  'Into Link Element

                m_strID = Util.LoadID(oXml, "")
                Me.Name = oXml.GetChildString("Name")

                oXml.OutOfElem()  'Outof Link Element

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Sub SaveData(ByRef oXml As AnimatTools.Interfaces.StdXml)

            Try

                oXml.AddChildElement("Link")
                oXml.IntoElem()  'Into Link Element

                oXml.AddChildElement("AssemblyFile", Me.AssemblyFile)
                oXml.AddChildElement("ClassName", Me.ClassName)

                oXml.AddChildElement("ID", Me.SelectedID)
                oXml.AddChildElement("Name", Me.Name)

                oXml.OutOfElem()  'Outof Link Element
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Sub InitializeAfterLoad(ByVal iAttempt As Integer)
            m_bInitialized = True
        End Sub

#End Region

#End Region

    End Class

End Namespace


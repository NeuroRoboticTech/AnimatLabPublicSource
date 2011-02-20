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

Namespace DataObjects.Behavior.Synapses

    Public Class [*SYNAPSE_NAME*]
        Inherits AnimatTools.DataObjects.Behavior.Links.Synapse

#Region " Attributes "

        'Add any extra properties that you need here. Shown thoughout is an example for a scaled number attribute.
        'Example: Protected m_snWeight As AnimatTools.Framework.ScaledNumber

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "[*SYNAPSE_DISPLAY_NAME*]"
            End Get
        End Property

        Public Overrides ReadOnly Property NeuralModuleType() As System.Type
            Get
                Return GetType([*PROJECT_NAME*]Tools.DataObjects.Behavior.NeuralModule)
            End Get
        End Property

        Public Overridable ReadOnly Property SynapseType() As String
            Get
                Return "[*SYNAPSE_TYPE*]"
            End Get
        End Property

        Public Overrides ReadOnly Property ImageName() As String
            Get
                Return "[*PROJECT_NAME*]Tools.Synapse.gif"
            End Get
        End Property

        'Add extra properties here. Shown is an example for a scaled number property
        'Public Overridable Property Weight() As AnimatTools.Framework.ScaledNumber
        '    Get
        '        Return m_snWeight
        '    End Get
        '    Set(ByVal Value As AnimatTools.Framework.ScaledNumber)
        '        m_snWeight.CopyData(Value)
        '    End Set
        'End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatTools.Framework.DataObject)
            MyBase.New(doParent)

            m_bEnabled = True

            'Add code here to create any scalednumber properties.
            'Example: m_snWeight = New AnimatTools.Framework.ScaledNumber(Me, "Weight", 1, AnimatTools.Framework.ScaledNumber.enumNumericScale.nano, "Amps", "A")

            'You can change the properties and color of your synapse item here.
            Me.DrawColor = Color.Black
            Me.ArrowDestination = New Arrow(Me, AnimatTools.DataObjects.Behavior.Link.enumArrowStyle.Fork, AnimatTools.DataObjects.Behavior.Link.enumArrowSize.Medium, AnimatTools.DataObjects.Behavior.Link.enumArrowAngle.deg30, False)

            Dim myAssembly As System.Reflection.Assembly
            myAssembly = System.Reflection.Assembly.Load("[*PROJECT_NAME*]Tools")

            Me.Image = AnimatTools.Framework.ImageManager.LoadImage(myAssembly, Me.ImageName, False)
            Me.Name = Me.TypeName

            Me.Font = New Font("Arial", 12)
            Me.Description = "[*SYNAPSE_DESCRIPTION*]"

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatTools.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatTools.Framework.DataObject) As AnimatTools.Framework.DataObject
            Dim oNewLink As New Synapses.[*SYNAPSE_NAME*](doParent)
            oNewLink.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewLink.AfterClone(Me, bCutData, doRoot, oNewLink)
            Return oNewLink
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatTools.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatTools.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim bnLink As Synapses.[*SYNAPSE_NAME*] = DirectCast(doOriginal, Synapses.[*SYNAPSE_NAME*])

            m_bEnabled = bnLink.m_bEnabled
            'Add any extra attributes to be copied here.
            'Example: m_snWeight = DirectCast(bnLink.m_snWeight.Clone(Me, bCutData, doRoot), ScaledNumber)
        End Sub

        Public Overrides Sub SaveNetwork(ByRef oXml As AnimatTools.Interfaces.StdXml, ByRef nmModule As AnimatTools.DataObjects.Behavior.NeuralModule)

            'Only save this as a synapse if the origin node is another FastNeuralNet neuron
            If Not Util.IsTypeOf(Me.Origin.GetType(), GetType(DataObjects.Behavior.Neurons.[*NEURON_NAME*]), False) Then
                Return
            End If

            Dim fnNeuron As DataObjects.Behavior.Neurons.[*NEURON_NAME*] = DirectCast(Me.Origin, DataObjects.Behavior.Neurons.[*NEURON_NAME*])

            oXml.AddChildElement("Synapse")
            oXml.IntoElem()

            oXml.AddChildElement("Enabled", m_bEnabled)
            oXml.AddChildElement("Type", Me.SynapseType)
            Util.SaveVector(oXml, "From", New AnimatTools.Framework.Vec3i(Me, fnNeuron.NodeIndex, 0, 0))
            'Example: oXml.AddChildElement("Weight", m_snWeight.ActualValue)

            oXml.OutOfElem()

        End Sub

#Region " DataObject Methods "

        Protected Overrides Sub BuildProperties()
            MyBase.BuildProperties()

            m_Properties.Properties.Remove("Link Type")

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Synapse Type", GetType(String), "TypeName", _
                                        "Synapse Properties", "Returns the type of this link.", TypeName(), True))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Enabled", GetType(Boolean), "Enabled", _
                                        "Synapse Properties", "Determines if this synapse is enabled or not.", m_bEnabled))

            'Add extra attributes to show up in the properties grid here. An example for a scaled number item is shown.
            'Dim pbNumberBag As Crownwood.Magic.Controls.PropertyBag = m_snWeight.Properties
            'm_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Weight", pbNumberBag.GetType(), "Weight", _
            '                            "Synapse Properties", "Sets the weight of this synaptic connection.", pbNumberBag, _
            '                            "", GetType(AnimatTools.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            'Call the ClearIsDirty flag for any DataObject classes like ScaledNumbers
            'Example: If Not m_snWeight Is Nothing Then m_snWeight.ClearIsDirty()
        End Sub

        Public Overrides Sub LoadData(ByRef oXml As AnimatTools.Interfaces.StdXml)

            Try
                MyBase.LoadData(oXml)

                oXml.IntoElem()

                m_bEnabled = oXml.GetChildBool("Enabled", m_bEnabled)

                'Add Code to load in any other attributes here
                'Example: m_snWeight.LoadData(oXml, "Weight")

                oXml.OutOfElem()
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Sub SaveData(ByRef oXml As AnimatTools.Interfaces.StdXml)

            Try
                MyBase.SaveData(oXml)

                oXml.IntoElem() 'Into Node Element

                oXml.AddChildElement("Enabled", m_bEnabled)

                'Add Code to save any other attributes here
                'Example: m_snWeight.SaveData(oXml, "Weight")

                oXml.OutOfElem() ' Outof Node Element

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

#End Region

    End Class

End Namespace

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

Namespace DataObjects.Behavior.SynapseTypes

    Public Class IndividualSynapse
        Inherits SynapseGroup

#Region " Attributes "

        Protected m_aryNeuronPairs As New Collections.NeuronPairIndices(Me)

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Individual Synapse"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property SimClassName() As String
            Get
                Return "IndividualSynapse"
            End Get
        End Property

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatCarlGUI.ExcitatorySynapse.gif"
            End Get
        End Property

        Public Overridable Property NeuronPairs As Collections.NeuronPairIndices
            Get
                Return m_aryNeuronPairs
            End Get
            Set(value As Collections.NeuronPairIndices)
                'Set nothing here. It is set in the property editor

                'We do need to reset the list of indices in the simulator though
                Dim oXml As ManagedAnimatInterfaces.IStdXml = Util.Application.CreateStdXml()
                oXml.AddElement("Root")
                SaveNeuronPairs(oXml)

                SetSimData("NeuronPairs", oXml.Serialize(), True)
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            m_bEnabled = True

            Me.DrawColor = Color.Black

            Dim myAssembly As System.Reflection.Assembly
            myAssembly = System.Reflection.Assembly.Load("AnimatCarlGUI")

            Me.WorkspaceImage = AnimatGUI.Framework.ImageManager.LoadImage(myAssembly, "AnimatCarlGUI.ExcitatorySynapse.gif", False)
            Me.Name = "Individual"

            Me.ArrowDestination = New Arrow(Me, AnimatGUI.DataObjects.Behavior.Link.enumArrowStyle.Fork, AnimatGUI.DataObjects.Behavior.Link.enumArrowSize.Medium, AnimatGUI.DataObjects.Behavior.Link.enumArrowAngle.deg30, False)

            Me.Font = New Font("Arial", 12)
            Me.Description = "A Synapse that connects individual neruons between two groups."

            ''Lets add the data types that this node understands.
            'm_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("Weight", "Weight", "Amps", "A", -100, 100, ScaledNumber.enumNumericScale.nano, ScaledNumber.enumNumericScale.nano))
            'm_thDataTypes.ID = "Weight"

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNewLink As New IndividualSynapse(doParent)
            oNewLink.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewLink.AfterClone(Me, bCutData, doRoot, oNewLink)
            Return oNewLink
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doRate As IndividualSynapse = DirectCast(doOriginal, IndividualSynapse)

            m_aryNeuronPairs = DirectCast(doRate.m_aryNeuronPairs.Clone(Me, bCutData, doRoot), Collections.NeuronPairIndices)
        End Sub

        Protected Overridable Sub SaveNeuronPairs(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            oXml.AddChildElement("NeuronPairs")
            oXml.IntoElem()
            For Each doPair As NeuronIndexPair In m_aryNeuronPairs
                oXml.AddChildElement("Pair")
                oXml.SetChildAttrib("From", doPair.m_iFromIdx)
                oXml.SetChildAttrib("To", doPair.m_iToIdx)
            Next
            oXml.IntoElem()
        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

            oXml.IntoElem()

            SaveNeuronPairs(oXml)

            oXml.OutOfElem()

        End Sub

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            'propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("No Direct Connect", GetType(Boolean), "NoDirectConnect", _
            '                            "Neural Properties", "If true then it does not connect pre-synaptic neuron J to the corresponding post-synaptic neuron J", m_bNoDirectConnect))

        End Sub

        Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            Try
                MyBase.LoadData(oXml)

                oXml.IntoElem()

                If oXml.FindChildElement("NeuronPairs", False) Then
                    oXml.IntoElem()

                    Dim iCount As Integer = oXml.NumberOfChildren() - 1
                    For iIdx As Integer = 0 To iCount
                        oXml.FindChildByIndex(iIdx)
                        Dim iFromIdx As Integer = oXml.GetChildAttribInt("From")
                        Dim iToIdx As Integer = oXml.GetChildAttribInt("From")

                        Dim oPair As New NeuronIndexPair(iFromIdx, iToIdx)
                        m_aryNeuronPairs.Add(oPair)
                    Next

                    oXml.OutOfElem()
                End If

                oXml.OutOfElem()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            Try
                MyBase.SaveData(oXml)

                oXml.IntoElem() 'Into Node Element

                SaveNeuronPairs(oXml)

                oXml.OutOfElem() ' Outof Node Element

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

#End Region

    End Class

End Namespace

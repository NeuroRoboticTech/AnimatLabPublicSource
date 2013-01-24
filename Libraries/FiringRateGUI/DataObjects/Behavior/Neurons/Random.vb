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

Namespace DataObjects.Behavior.Neurons

    Public Class Random
        Inherits Neurons.Normal

#Region " Attributes "

        Protected m_snIl As AnimatGUI.Framework.ScaledNumber
        Protected m_gnCurrentDistribution As AnimatGUI.DataObjects.Gain
        Protected m_gnBurstLengthDistribution As AnimatGUI.DataObjects.Gain
        Protected m_gnInterburstLengthDistribution As AnimatGUI.DataObjects.Gain

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Random Firing Rate Neuron"
            End Get
        End Property

        Public Overridable Property Il() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snIl
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)

                SetSimData("Il", Value.ActualValue.ToString, True)
                m_snIl.CopyData(Value)
            End Set
        End Property

        <EditorAttribute(GetType(AnimatGUI.TypeHelpers.GainTypeEditor), GetType(System.Drawing.Design.UITypeEditor))> _
        Public Overridable Property CurrentDistribution() As AnimatGUI.DataObjects.Gain
            Get
                Return m_gnCurrentDistribution
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Gain)
                If Not Value Is Nothing Then
                    SetSimData("CurrentDistribution", Value.GetSimulationXml("CurrentGraph", Me), True)
                    Value.InitializeSimulationReferences()
                End If

                If Not m_gnCurrentDistribution Is Nothing Then m_gnCurrentDistribution.ParentData = Nothing
                m_gnCurrentDistribution = Value
                If Not m_gnCurrentDistribution Is Nothing Then
                    m_gnCurrentDistribution.ParentData = Me
                    m_gnCurrentDistribution.GainPropertyName = "CurrentDistribution"
                End If
            End Set
        End Property

        <EditorAttribute(GetType(AnimatGUI.TypeHelpers.GainTypeEditor), GetType(System.Drawing.Design.UITypeEditor))> _
        Public Overridable Property BurstLengthDistribution() As AnimatGUI.DataObjects.Gain
            Get
                Return m_gnBurstLengthDistribution
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Gain)
                If Not Value Is Nothing Then
                    SetSimData("BurstLengthDistribution", Value.GetSimulationXml("BurstGraph", Me), True)
                    Value.InitializeSimulationReferences()
                End If

                If Not m_gnBurstLengthDistribution Is Nothing Then m_gnBurstLengthDistribution.ParentData = Nothing
                m_gnBurstLengthDistribution = Value
                If Not m_gnBurstLengthDistribution Is Nothing Then
                    m_gnBurstLengthDistribution.ParentData = Me
                    m_gnBurstLengthDistribution.GainPropertyName = "BurstLengthDistribution"
                End If
            End Set
        End Property

        <EditorAttribute(GetType(AnimatGUI.TypeHelpers.GainTypeEditor), GetType(System.Drawing.Design.UITypeEditor))> _
        Public Overridable Property InterburstLengthDistribution() As AnimatGUI.DataObjects.Gain
            Get
                Return m_gnInterburstLengthDistribution
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Gain)
                If Not Value Is Nothing Then
                    SetSimData("InterburstLengthDistribution", Value.GetSimulationXml("InterBurstGraph", Me), True)
                    Value.InitializeSimulationReferences()
                End If

                If Not m_gnInterburstLengthDistribution Is Nothing Then m_gnInterburstLengthDistribution.ParentData = Nothing
                m_gnInterburstLengthDistribution = Value
                If Not m_gnInterburstLengthDistribution Is Nothing Then
                    m_gnInterburstLengthDistribution.ParentData = Me
                    m_gnInterburstLengthDistribution.GainPropertyName = "InterburstLengthDistribution"
                End If
            End Set
        End Property

        Public Overrides ReadOnly Property NeuronType() As String
            Get
                Return "Random"
            End Get
        End Property

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "FiringRateGUI.RandomNeuron.gif"
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            Try

                Shape = AnimatGUI.DataObjects.Behavior.Node.enumShape.Ellipse
                Size = New SizeF(40, 40)
                Me.DrawColor = Color.Black
                Me.FillColor = Color.Red

                Dim myAssembly As System.Reflection.Assembly
                myAssembly = System.Reflection.Assembly.Load("FiringRateGUI")

                Me.WorkspaceImage = AnimatGUI.Framework.ImageManager.LoadImage(myAssembly, "FiringRateGUI.RandomNeuron.gif", False)
                Me.Name = "Random Firing Rate Neuron"
                Me.Description = "A firing rate neuron type in the fast neural network library that provides random firing frequencies."

                m_snIl = New AnimatGUI.Framework.ScaledNumber(Me, "Il", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.nano, "Amps", "A")
                m_gnCurrentDistribution = New AnimatGUI.DataObjects.Gains.Polynomial(Me, "CurrentDistribution", "Random Variable", "Amps", False, False, False)
                m_gnBurstLengthDistribution = New AnimatGUI.DataObjects.Gains.Polynomial(Me, "BurstLengthDistribution", "Random Variable", "Seconds", False, False, False)
                m_gnInterburstLengthDistribution = New AnimatGUI.DataObjects.Gains.Polynomial(Me, "InterburstLengthDistribution", "Random Variable", "Seconds", False, False, False)


                m_gnCurrentDistribution.LowerLimit.Value = 0
                m_gnCurrentDistribution.UpperLimit.Value = 100
                m_gnBurstLengthDistribution.LowerLimit.Value = 0
                m_gnBurstLengthDistribution.UpperLimit.Value = 100
                m_gnInterburstLengthDistribution.LowerLimit.Value = 0
                m_gnInterburstLengthDistribution.UpperLimit.Value = 100

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNewNode As New Neurons.Random(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim bnOrig As Neurons.Random = DirectCast(doOriginal, Neurons.Random)

            m_snIl = DirectCast(bnOrig.m_snIl.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_gnCurrentDistribution = DirectCast(bnOrig.m_gnCurrentDistribution.Clone(Me, bCutData, doRoot), AnimatGUI.DataObjects.Gain)
            m_gnBurstLengthDistribution = DirectCast(bnOrig.m_gnBurstLengthDistribution.Clone(Me, bCutData, doRoot), AnimatGUI.DataObjects.Gain)
            m_gnInterburstLengthDistribution = DirectCast(bnOrig.m_gnInterburstLengthDistribution.Clone(Me, bCutData, doRoot), AnimatGUI.DataObjects.Gain)
        End Sub

        Public Overrides Sub AddToReplaceIDList(ByVal aryReplaceIDList As ArrayList)
            MyBase.AddToReplaceIDList(aryReplaceIDList)

            m_gnCurrentDistribution.AddToReplaceIDList(aryReplaceIDList)
            m_gnBurstLengthDistribution.AddToReplaceIDList(aryReplaceIDList)
            m_gnInterburstLengthDistribution.AddToReplaceIDList(aryReplaceIDList)
        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl)

            oXml.IntoElem() 'Into Neuron element

            oXml.AddChildElement("Il", m_snIl.ActualValue)
            m_gnCurrentDistribution.SaveSimulationXml(oXml, Me, "CurrentGraph")
            m_gnBurstLengthDistribution.SaveSimulationXml(oXml, Me, "BurstGraph")
            m_gnInterburstLengthDistribution.SaveSimulationXml(oXml, Me, "InterBurstGraph")

            oXml.OutOfElem() 'Outof Neuron

        End Sub

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = m_snIl.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Il", pbNumberBag.GetType(), "Il", _
                                        "Neural Properties", "Sets the hyperpolarizing current that brings the " & _
                                        "membrane potential back down after it has been firing.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Current Distribution", GetType(AnimatGUI.DataObjects.Gain), "CurrentDistribution", _
                                        "Neural Properties", "Sets the gain that controls the probability distribution " & _
                                        "for the size of the random current.", m_gnCurrentDistribution, _
                                        GetType(AnimatGUI.TypeHelpers.GainTypeEditor), _
                                        GetType(AnimatGUI.TypeHelpers.GainTypeConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Burst Length", GetType(AnimatGUI.DataObjects.Gain), "BurstLengthDistribution", _
                                        "Neural Properties", "Sets the gain that controls the probability distribution " & _
                                        "for the length of the random burst.", m_gnBurstLengthDistribution, _
                                        GetType(AnimatGUI.TypeHelpers.GainTypeEditor), _
                                        GetType(AnimatGUI.TypeHelpers.GainTypeConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("InterBurst Length", GetType(AnimatGUI.DataObjects.Gain), "InterburstLengthDistribution", _
                                        "Neural Properties", "Sets the gain that controls the probability distribution " & _
                                        "for the length between bursts.", m_gnInterburstLengthDistribution, _
                                        GetType(AnimatGUI.TypeHelpers.GainTypeEditor), _
                                        GetType(AnimatGUI.TypeHelpers.GainTypeConverter)))

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            If Not m_snIl Is Nothing Then m_snIl.ClearIsDirty()
            If Not m_gnCurrentDistribution Is Nothing Then m_gnCurrentDistribution.ClearIsDirty()
            If Not m_gnBurstLengthDistribution Is Nothing Then m_gnBurstLengthDistribution.ClearIsDirty()
            If Not m_gnInterburstLengthDistribution Is Nothing Then m_gnInterburstLengthDistribution.ClearIsDirty()
        End Sub

        Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.LoadData(oXml)

            oXml.IntoElem()

            m_snIl.LoadData(oXml, "Il")

            If oXml.FindChildElement("CurrentDistribution", False) Then
                oXml.IntoChildElement("CurrentDistribution")
                Dim strAssemblyFile As String = oXml.GetChildString("AssemblyFile")
                Dim strClassName As String = oXml.GetChildString("ClassName")
                oXml.OutOfElem()

                m_gnCurrentDistribution = DirectCast(Util.LoadClass(strAssemblyFile, strClassName, Me), AnimatGUI.DataObjects.Gain)
                m_gnCurrentDistribution.LoadData(oXml, "CurrentDistribution", "CurrentDistribution")
            End If

            If oXml.FindChildElement("BurstLengthDistribution", False) Then
                oXml.IntoChildElement("BurstLengthDistribution")
                Dim strAssemblyFile As String = oXml.GetChildString("AssemblyFile")
                Dim strClassName As String = oXml.GetChildString("ClassName")
                oXml.OutOfElem()

                m_gnBurstLengthDistribution = DirectCast(Util.LoadClass(strAssemblyFile, strClassName, Me), AnimatGUI.DataObjects.Gain)
                m_gnBurstLengthDistribution.LoadData(oXml, "BurstLengthDistribution", "BurstLengthDistribution")
            End If

            If oXml.FindChildElement("InterburstLengthDistribution", False) Then
                oXml.IntoChildElement("InterburstLengthDistribution")
                Dim strAssemblyFile As String = oXml.GetChildString("AssemblyFile")
                Dim strClassName As String = oXml.GetChildString("ClassName")
                oXml.OutOfElem()

                m_gnInterburstLengthDistribution = DirectCast(Util.LoadClass(strAssemblyFile, strClassName, Me), AnimatGUI.DataObjects.Gain)
                m_gnInterburstLengthDistribution.LoadData(oXml, "InterburstLengthDistribution", "InterburstLengthDistribution")
            End If

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.SaveData(oXml)

            oXml.IntoElem() 'Into Node Element

            m_snIl.SaveData(oXml, "Il")
            m_gnCurrentDistribution.SaveData(oXml, "CurrentDistribution")
            m_gnBurstLengthDistribution.SaveData(oXml, "BurstLengthDistribution")
            m_gnInterburstLengthDistribution.SaveData(oXml, "InterburstLengthDistribution")

            oXml.OutOfElem() ' Outof Node Element

        End Sub

#End Region

#End Region

    End Class

End Namespace


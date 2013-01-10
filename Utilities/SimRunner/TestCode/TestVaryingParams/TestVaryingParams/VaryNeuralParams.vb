
Public Class VaryNeuralParams
    Inherits AnimatGUI.DataObjects.Macro

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "Varies params of a synapse as a test."
        End Get
    End Property

    Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
        MyBase.New(doParent)

        m_strName = "Vary Synapse Params"
    End Sub

    Public Overrides Sub Execute()

        Dim oApp As AnimatGUI.Forms.AnimatApplication = AnimatGUI.Framework.Util.Application

        Dim oSynapse As IntegrateFireGUI.DataObjects.Behavior.Synapse = DirectCast(oApp.Simulation.Environment.FindObjectByID("1281bb1b-8642-4f2e-98b8-a45a8fd07ecc"), IntegrateFireGUI.DataObjects.Behavior.Synapse)

        For dblSynapse As Double = 0.1 To 1 Step 0.1
            oSynapse.SynapticConductance.ActualValue = (dblSynapse * 0.000001)
            oApp.ExportStandAloneSim("C:\Projects\AnimatLabSDK\AnimatLabPublicSource\Utilities\SimRunner\SimFiles\SynapseCond_" & CInt(dblSynapse * 100) & ".asim")
        Next

        MsgBox("Created simulation files.")

    End Sub

End Class

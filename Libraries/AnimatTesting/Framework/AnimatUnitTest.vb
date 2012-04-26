
Namespace Framework

    <TestClass()> _
    Public MustInherit Class AnimatUnitTest
        Inherits AnimatTest


#Region "Additional test attributes"
     
        'Use TestInitialize to run code before running each test
        <TestInitialize()> _
        Public Overridable Sub MyTestInitialize()
            StartApplication("", m_bAttachServerOnly)
        End Sub

        'Use TestCleanup to run code after each test has run
        <TestCleanup()> _
        Public Overridable Sub MyTestCleanup()
            'Detach from the server.
            DetachServer()
        End Sub

#End Region


    End Class

End Namespace

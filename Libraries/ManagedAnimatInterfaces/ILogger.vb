

Public Interface ILogger

    Enum enumLogLevel
        None = 0
        ErrorType = 10 ' only trace error
        Info = 20 ' some extra info
        Debug = 30 ' debugging info
        Detail = 40 ' detailed debugging info
    End Enum

    Property LogPrefix As String
    Property TraceLevel As enumLogLevel

    Sub LogMsg(ByVal eLevel As enumLogLevel, ByVal sMessage As String)

End Interface


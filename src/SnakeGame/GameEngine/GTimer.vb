Public MustInherit Class GTimer
    Inherits GObject

    Public Property Enabled As Boolean

    Public Property LastEvent As Date
    Public Property TimeUntilEvent As TimeSpan

    Public Sub Start()
        LastEvent = Now
        Enabled = True
    End Sub
    Public Sub [Stop]()
        Enabled = False
    End Sub

    Public Sub Init(ts As TimeSpan)
        TimeUntilEvent = ts
    End Sub

    Public Overrides Sub Update(delta As Single)
        If (TimeUntilEvent = Nothing) Or (Not Enabled) Then Exit Sub

        If (Now - LastEvent) > TimeUntilEvent Then
            LastEvent = Now
            DoActions(delta)
        End If
    End Sub

    Public MustOverride Sub DoActions(delta As Single)


End Class

Public Class HighScore
    Inherits GObject

    Public Property SnakeName As String

    Public Property Time As Date

    Public Property Score As UShort

    Public Overrides Sub Update(delta As Single)
        Throw New NotImplementedException()
    End Sub

End Class

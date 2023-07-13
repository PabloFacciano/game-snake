Public Class Food
    Inherits GVisibleObject

    Private Const ScoreMax As Integer = 100

    Public Property Rect As RectangleF
    Public Property Score As Integer
    Public Property CreatedDate As Date

    Private CurrentGame As SnakeGame
    Public Sub New(ByRef g As SnakeGame)
        CurrentGame = g
    End Sub

    Public Sub initRandom()

        Dim size As Integer = GetRandom(50, 120)
        Me.Rect = New Rectangle(
            GetRandom(size, Me.CurrentGame.ScreenSize.Width - size * 2),
            GetRandom(size, Me.CurrentGame.ScreenSize.Height - size * 2),
            size,
            size
        )

        Me.Score = size

    End Sub

    Public Overrides Sub Update(delta As Single)

        If Rect.Width > 50 Then
            Me.Rect = New RectangleF(
              Me.Rect.X + delta,
              Me.Rect.Y + delta,
              Me.Rect.Width - delta * 2,
              Me.Rect.Height - delta * 2
            )
            Me.Score = CInt(Rect.Width)
        End If

    End Sub

    Public Overrides Sub Draw(ByRef g As Graphics)

        Dim c As Color = Color.DarkRed
        If Score > 0 Then
            c = Color.DarkSlateBlue
        End If

        Using b As New SolidBrush(c)
            g.FillEllipse(b, Me.Rect)
        End Using
    End Sub

End Class

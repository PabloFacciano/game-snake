Public Class SnakeGame
    Inherits Game



    Public Sub New(ByRef f As FrmGame)
        MyBase.New(f)
        Const DEFAULT_SNAKE_SIZE As Single = 100


        Dim player = New Snake(Me) With {
            .Name = "Player",
            .Score = 1,
            .TurnLeftKey = Keys.Left,
            .TurnRightKey = Keys.Right,
            .AccelerationKey = Keys.Up,
            .BrakeKey = Keys.Space
        }
        player.Init(
            DEFAULT_SNAKE_SIZE
        )
        GameObjects.Add(player)


        Dim timerFood As New FoodTimer(Me)
        timerFood.Init(New TimeSpan(0, 0, 3))
        timerFood.Start()
        GameObjects.Add(timerFood)

    End Sub

End Class

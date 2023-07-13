Public Class FoodTimer
    Inherits GTimer


    Private CurrentGame As SnakeGame
    Public Sub New(ByRef g As SnakeGame)
        CurrentGame = g
    End Sub

    Public Overrides Sub DoActions(delta As Single)

        ' Agregar comida
        Dim f As New Food(CurrentGame)
        f.initRandom()
        CurrentGame.GameObjectsToAdd.Add(f)

    End Sub

End Class

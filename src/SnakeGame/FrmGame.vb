Imports System.Windows.Forms

Public Class FrmGame

    Private myGame As SnakeGame

    Private Sub FrmGame_Load(sender As Object, e As EventArgs) Handles Me.Load
        myGame = New SnakeGame(Me)
    End Sub

End Class

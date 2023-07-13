Imports System.Net

Public Class Snake
    Inherits GVisibleObject

    Private CurrentGame As SnakeGame
    Public Sub New(ByRef g As SnakeGame)
        CurrentGame = g
    End Sub

    Public Property Name As String = "Snake"
    Public Property Points As List(Of PointF)
    Public Property Score As Integer = 0

    Public Property Acceleration As Single = 30
    Public Property Brakes As Single = 50
    Public Property Velocity As Single = 100
    Public Property AngleVelocity As Single = 180
    Public Property DirectionAngle As Single = 0

    Public Property TurnLeftKey As Keys
    Public Property TurnRightKey As Keys
    Public Property AccelerationKey As Keys
    Public Property BrakeKey As Keys

    Private DrawPen As Pen
    Private Property Width As Single = 15
    Private Property Color As Color = SystemColors.Highlight

    Public Sub UpdateWidthColor(c As Color, w As Single)
        Me.Color = c
        Me.Width = w
        DrawPen = New Pen(Me.Color, Me.Width)
        DrawPen.StartCap = Drawing2D.LineCap.Round
        DrawPen.EndCap = Drawing2D.LineCap.Triangle
    End Sub

    Public Sub Init(size As Single)
        Me.Points = New List(Of PointF) From {
            New PointF(0, 0),
            New PointF(size, 0)
        }
        UpdateWidthColor(Me.Color, Me.Width)
    End Sub

    Public Overrides Sub Update(delta As Single)

        HandleTurn(delta)
        HandleMovement(delta)
        HandleFood(delta)

    End Sub

    Private Sub HandleTurn(delta As Single)

        ' Check Keys
        Dim Turn As Single = 0
        If Me.CurrentGame.IsKeyDown(Me.TurnLeftKey) Then
            Turn -= 1
        ElseIf Me.CurrentGame.IsKeyDown(Me.TurnRightKey) Then
            Turn += 1
        End If

        ' New Angle
        If Turn <> 0 Then
            Me.DirectionAngle += (Turn * Me.AngleVelocity * delta)
            If Me.DirectionAngle >= 360 Then
                Me.DirectionAngle -= 360
            ElseIf Me.DirectionAngle < 0 Then
                Me.DirectionAngle += 360
            End If
            Me.Points.Add(Me.Points.Last)
        End If

    End Sub
    Private Sub HandleMovement(delta As Single)

        ' Acceleration
        Dim CurrentAcceleration As Single = 0
        If CurrentGame.IsKeyDown(AccelerationKey) Then
            CurrentAcceleration += Acceleration
        ElseIf CurrentGame.IsKeyDown(BrakeKey) Then
            CurrentAcceleration -= Brakes
        End If
        If CurrentAcceleration <> 0 Then
            CurrentAcceleration *= delta
            Velocity += CurrentAcceleration
        End If

        ' Move
        Dim CurrentVelocity As Single = Math.Max(Me.Velocity, 30) * delta

        MoveLastPoint(CurrentVelocity)
        MoveFirstPoint(CurrentVelocity)

    End Sub
    Private Sub MoveFirstPoint(vel As Single)

        Dim distance = DistanceBetweenTwoPoints(Me.Points(0), Me.Points(1))

        If distance > vel Then
            Dim NewPoint As PointF = MovePointAInDirectionToPointBWithVelocity(Me.Points(0), Me.Points(1), vel)

            Me.Points(0) = NewPoint
            Exit Sub
        End If

        Me.Points.RemoveAt(0)
        MoveFirstPoint(vel - distance)

    End Sub
    Private Sub MoveLastPoint(vel As Single)
        Dim NewPoint As PointF = MovePointWithAngleAndVelocity(Me.Points(Me.Points.Count - 1), Me.DirectionAngle, vel)
        Me.Points(Me.Points.Count - 1) = NewPoint
    End Sub
    Private Sub HandleFood(delta As Single)

        For Each F In CurrentGame.GameObjects
            If TypeOf F Is Food Then
                Dim currentFood As Food = CType(F, Food)

                ' Comiendo?
                If IsPointInsideEllipse(Me.Points.Last, currentFood.Rect) Then

                    F.MarkedToDelete = True
                    MoveFirstPoint(-currentFood.Score / 10)
                    Score += currentFood.Score

                End If


            End If
        Next

    End Sub

    Public Overrides Sub Draw(ByRef g As Graphics)

        g.DrawLines(DrawPen, Me.Points.ToArray)

    End Sub

End Class

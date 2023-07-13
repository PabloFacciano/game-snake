Module GUtils

    Public Function MovePointWithAngleAndVelocity(ByVal p As PointF, ByVal angle As Double, ByVal distance As Double) As PointF
        ' Convertir el ángulo de grados a radianes
        Dim anguloRadianes As Double = angle * Math.PI / 180.0
        ' Retornar las coordenadas del nuevo punto
        Return New PointF(
            p.X + distance * Math.Cos(anguloRadianes),
            p.Y + distance * Math.Sin(anguloRadianes)
        )
    End Function

    Public Function MovePointAInDirectionToPointBWithVelocity(ByVal A As PointF, ByVal B As PointF, ByVal Velocity As Single) As PointF
        ' Calcular la diferencia entre las coordenadas de los puntos A y B
        Dim diffX As Single = B.X - A.X
        Dim diffY As Single = B.Y - A.Y

        ' Calcular la distancia entre los puntos A y B
        Dim distance As Single = Math.Sqrt(diffX * diffX + diffY * diffY)

        ' Calcular las componentes de velocidad en X e Y
        Dim velocityX As Single = Velocity * (diffX / distance)
        Dim velocityY As Single = Velocity * (diffY / distance)

        ' Calcular las nuevas coordenadas del punto A
        Dim newX As Single = A.X + velocityX
        Dim newY As Single = A.Y + velocityY

        ' Retornar las nuevas coordenadas como un nuevo punto
        Return New PointF(newX, newY)
    End Function

    Public Function MovePointAInDirectionToPointBWithVelocityAndMaxB(ByVal A As PointF, ByVal B As PointF, ByVal Velocity As Single) As PointF
        ' Calcular la diferencia entre las coordenadas de los puntos A y B
        Dim diffX As Single = B.X - A.X
        Dim diffY As Single = B.Y - A.Y

        ' Calcular la distancia entre los puntos A y B
        Dim distance As Single = Math.Sqrt(diffX * diffX + diffY * diffY)

        ' Verificar si el punto A está más allá del punto B
        If distance <= Velocity Then
            ' El punto A ya está lo suficientemente cerca del punto B, se devuelve el punto B
            Console.WriteLine("Punto A ha llegado a punto B")
            Return B
        End If

        ' Calcular las componentes de velocidad en X e Y
        Dim velocityX As Single = Velocity * (diffX / distance)
        Dim velocityY As Single = Velocity * (diffY / distance)

        ' Calcular las nuevas coordenadas del punto A
        Dim newX As Single = A.X + velocityX
        Dim newY As Single = A.Y + velocityY

        ' Retornar las nuevas coordenadas del punto A
        Return New PointF(newX, newY)
    End Function


    Public Function DistanceBetweenTwoPoints(ByVal A As PointF, ByVal B As PointF) As Single
        ' Calcular la diferencia entre las coordenadas de los puntos A y B
        Dim diffX As Single = B.X - A.X
        Dim diffY As Single = B.Y - A.Y

        ' Calcular la distancia utilizando el teorema de Pitágoras
        Dim distance As Single = Math.Sqrt(diffX * diffX + diffY * diffY)

        ' Retornar la distancia calculada
        Return distance
    End Function

    Public Function IsPointInsideRect(ByVal p As PointF, ByVal rect As RectangleF) As Boolean
        If p.X >= rect.Left AndAlso p.X <= rect.Right AndAlso
       p.Y >= rect.Top AndAlso p.Y <= rect.Bottom Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function IsPointInsideEllipse(ByVal p As PointF, ByVal rect As RectangleF) As Boolean
        Dim halfWidth As Single = rect.Width / 2
        Dim halfHeight As Single = rect.Height / 2
        Dim centerX As Single = rect.Left + halfWidth
        Dim centerY As Single = rect.Top + halfHeight

        Dim normalizedX As Single = (p.X - centerX) / halfWidth
        Dim normalizedY As Single = (p.Y - centerY) / halfHeight

        Dim distanceSquared As Single = (normalizedX * normalizedX) + (normalizedY * normalizedY)
        Return distanceSquared <= 1.0
    End Function

    Public Function GetRandom(ByVal Min As Integer, ByVal Max As Integer) As Integer
        ' by making Generator static, we preserve the same instance '
        ' (i.e., do not create new instances with the same seed over and over) '
        ' between calls '
        Static Generator As System.Random = New System.Random()
        Return Generator.Next(Min, Max)
    End Function

    Public Sub DrawTable(ByRef g As Graphics, color As Color, rect As RectangleF, cell As SizeF)
        Using p As New Pen(color)
            Dim x1, x2, y1, y2 As Single
            Dim x = rect.X
            Dim y = rect.Y
            Dim width = cell.Width
            Dim height = cell.Height
            Dim columns As Integer = rect.Width / cell.Width
            Dim rows As Integer = rect.Height / cell.Height

            ' Verticales
            For xx = 0 To columns
                x1 = x + (xx * width)
                x2 = x1
                y1 = y
                y2 = y + (rows * height)
                g.DrawLine(p, x1, y1, x2, y2)
            Next

            ' Horizontales
            For yy = 0 To rows
                x1 = x
                x2 = x + (columns * width)
                y1 = y + (yy * height)
                y2 = y1
                g.DrawLine(p, x1, y1, x2, y2)
            Next
        End Using
    End Sub

End Module

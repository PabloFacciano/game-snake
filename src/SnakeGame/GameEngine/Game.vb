Public MustInherit Class Game

    Private frm As Form
    Public Sub New(f As Form)
        frm = f
        lastFps = Date.Now
        AddHandler frm.Shown, AddressOf FormHandler_Shown
        AddHandler frm.Paint, AddressOf FormHandler_Paint
        AddHandler frm.KeyDown, AddressOf FormHandler_KeyDown
        AddHandler frm.KeyUp, AddressOf FormHandler_KeyUp
    End Sub

    Public Property ExpectedFramesPerSecond As Integer = 30
    Public Property ScreenSize As New Size(1024, 600)
    Public Property ScreenResize As PictureBoxSizeMode = PictureBoxSizeMode.Zoom

    Public Property CurrentFPS As Integer
    Public Property CurrentTicks As Integer

    Public Property ShowDebugInfo As Boolean = True

    Public Property GameObjects As New List(Of GObject)

    Public Property GameObjectsToAdd As New List(Of GObject)

    Private stopwatch As Stopwatch = New Stopwatch()
    Private ticks As Integer

    Private Sub FormHandler_Shown(sender As Object, e As EventArgs)
        ScreenSize = frm.ClientSize

        ' Iniciar el stopwatch para medir el tiempo delta
        stopwatch.Start()

        Do While frm.Visible

            ' Calcular el tiempo delta entre frames
            Dim deltaTime As Double = stopwatch.Elapsed.TotalSeconds

            ' Reiniciar el stopwatch
            stopwatch.Restart()

            ticks += 1
            If deltaTime >= 1.0 Then
                CurrentTicks = ticks
                ticks = 0
            End If

            ' Actualizar el estado del juego utilizando el tiempo delta
            ' Multiplicar todas las velocidades y desplazamientos por deltaTime
            ' para ajustarlos correctamente según la duración del frame
            For Each obj In GameObjects
                obj.Update(deltaTime)
            Next

            ' Eliminar los objetos marcados
            For index = GameObjects.Count - 1 To 0 Step -1
                If GameObjects(index).MarkedToDelete Then
                    GameObjects.RemoveAt(index)
                End If
            Next

            ' Agregar los nuevos objetos
            GameObjects.AddRange(GameObjectsToAdd)
            GameObjectsToAdd.Clear()

            ' Renderizar el juego
            frm.Invalidate()

            ' Procesar eventos de entrada
            Application.DoEvents()

            ' Descansar un poco
            Dim targetFrameTime As Double = 1.0 / ExpectedFramesPerSecond ' 60 FPS
            Dim sleepTimeMilliseconds As Integer = CInt((targetFrameTime - deltaTime) * 1000)
            If sleepTimeMilliseconds > 0 Then
                Threading.Thread.Sleep(sleepTimeMilliseconds)
            End If

        Loop
    End Sub


    Dim lastFps As Date
    Dim fps As Integer = 0
    Dim OneSecond As New TimeSpan(0, 0, 1)
    Private Sub FormHandler_Paint(sender As Object, e As PaintEventArgs)
        Using bmp As New Bitmap(ScreenSize.Width, ScreenSize.Height)
            Using g As Graphics = Graphics.FromImage(bmp)

                Dim player As PointF = CType(GameObjects(0), Snake).Points.Last
                With g
                    .CompositingMode = Drawing2D.CompositingMode.SourceOver
                    .CompositingQuality = Drawing2D.CompositingQuality.AssumeLinear
                    .InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                    .SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
                    .TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
                    .Clear(SystemColors.Control)
                    '' Center Screen:
                    '.TranslateTransform(ScreenSize.Width / 2, ScreenSize.Height / 2)
                    ' Follow player:
                    .TranslateTransform(ScreenSize.Width / 2 - player.X, ScreenSize.Height / 2 - player.Y)
                End With

                DrawTable(g, Color.DarkGray, New RectangleF(0, 0, ScreenSize.Width, ScreenSize.Height), New SizeF(50, 50))

                For Each obj In GameObjects
                    If TypeOf obj Is GVisibleObject Then
                        Dim obj2 = CType(obj, GVisibleObject)
                        obj2.Draw(g)
                    End If
                Next

            End Using

            Dim imgRect As New RectangleF(PointF.Empty, ScreenSize)

            If ScreenResize = PictureBoxSizeMode.CenterImage Then
                imgRect.X = (frm.ClientSize.Width / 2) - (ScreenSize.Width / 2)
                imgRect.Y = (frm.ClientSize.Height / 2) - (ScreenSize.Height / 2)
                imgRect.Size = ScreenSize
            ElseIf ScreenResize = PictureBoxSizeMode.Zoom Then
                Dim containerRatio = 1.0F * frm.ClientSize.Width / frm.ClientSize.Height
                Dim imageRatio = 1.0F * ScreenSize.Width / ScreenSize.Height

                If (containerRatio > imageRatio) Then
                    imgRect.Height = frm.ClientSize.Height
                    imgRect.Width = CInt(imgRect.Height * imageRatio)
                    imgRect.X = (frm.ClientSize.Width - imgRect.Width) \ 2
                    imgRect.Y = 0
                Else
                    imgRect.Width = frm.ClientSize.Width
                    imgRect.Height = CInt(imgRect.Width / imageRatio)
                    imgRect.X = 0
                    imgRect.Y = (frm.ClientSize.Height - imgRect.Height) \ 2
                End If
            End If


            e.Graphics.DrawImage(bmp, imgRect)
        End Using

        If ShowDebugInfo Then
            e.Graphics.DrawString("FPS: " & CurrentFPS & vbNewLine & "Ticks: " & CurrentTicks, SystemFonts.DefaultFont, SystemBrushes.ActiveCaption, Point.Empty)
        End If

        fps += 1
        If ((Now - lastFps) > OneSecond) Then
            lastFps = Now
            CurrentFPS = fps
            fps = 0
        End If
    End Sub


    Public Event KeyDown(key As KeyEventArgs)
    Public Event KeyUp(key As KeyEventArgs)
    Private Keys As New List(Of Keys)
    Public Function IsKeyDown(key As Keys) As Boolean
        Return Keys.Contains(key)
    End Function
    Private Sub FormHandler_KeyUp(sender As Object, e As KeyEventArgs)
        If Keys.Contains(e.KeyData) Then
            Keys.Remove(e.KeyData)
            RaiseEvent KeyUp(e)
        End If
    End Sub
    Private Sub FormHandler_KeyDown(sender As Object, e As KeyEventArgs)
        If Not Keys.Contains(e.KeyData) Then
            Keys.Add(e.KeyData)
            RaiseEvent KeyDown(e)
        End If
    End Sub

    'Private Sub frm_MouseLeave(sender As Object, e As EventArgs) Handles frm.MouseLeave

    'End Sub

    'Private Sub frm_MouseMove(sender As Object, e As MouseEventArgs) Handles frm.MouseMove

    'End Sub

    'Private Sub frm_MouseUp(sender As Object, e As MouseEventArgs) Handles frm.MouseUp

    'End Sub

    'Private Sub frm_MouseWheel(sender As Object, e As MouseEventArgs) Handles frm.MouseWheel

    'End Sub

    'Private Sub frm_MouseDown(sender As Object, e As MouseEventArgs) Handles frm.MouseDown

    'End Sub

    'Private Sub frm_MouseClick(sender As Object, e As MouseEventArgs) Handles frm.MouseClick

    'End Sub

    'Private Sub frm_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles frm.MouseDoubleClick

    'End Sub

End Class

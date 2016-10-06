Public Class FrmJuego

    ''' <summary>
    ''' Constante que nos indica el tamaño de las cartas en pixeles
    ''' </summary>
    Private tamCartas As New Size(150, 200)

    ''' <summary>
    ''' Botón de la carta destapada en juego.
    ''' </summary>
    Private botonApuntado As Button

    ''' <summary>
    ''' Lista de botones que se agregarán al formulario que contendrán las imágenes
    ''' de los pares a encontrar.
    ''' </summary>
    Private botonesCartas As New List(Of Button)()

    ''' <summary>
    ''' Creamos un generador de números aleatorios cuya semilla esté dada por la hora exacta del sistema.
    ''' Así aseguramos que los números sean muy aleatorios xD.
    ''' </summary>
    Private aleatorio As New Random(TimeOfDay.GetHashCode())

    ''' <summary>
    ''' Número de filas de cartas en el formulario.
    ''' </summary>
    Private cartasAlto As Integer

    ''' <summary>
    ''' Número de columnas de cartas en el formulario.
    ''' </summary>
    Private cartasAncho As Integer

    ''' <summary>
    ''' Número de pares encontrados en el juego actual.
    ''' </summary>
    Private paresEncontrados As Integer

    ''' <summary>
    ''' Tiempo transcurrido a lo largo de la partida sin contar el tiempo entre diálogos
    ''' de salir y reiniciar.
    ''' </summary>
    Private tiempoJuego As TimeSpan

    ''' <summary>
    ''' Número de errores cometido en la partida actual.
    ''' </summary>
    Private errores As Integer

    Private Sub FrmJuego_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        MsgBox("¡Bienvenido al juego del memorama!" & vbCrLf & vbCrLf &
               "Por favor coloque todas las imágenes que deseé usar" & vbCrLf &
               "en el directorio del programa con el prefijo ""img""" & vbCrLf &
               "y presione aceptar para comenzar.", MsgBoxStyle.Information, "Memorama")

        ' No queremos bordes para no estropear el acomodo de los controles por
        ' el falso tamaño del formulario que estos causan.
        FormBorderStyle = FormBorderStyle.None

        ' Por cada archivo en el directorio de trabajo del programa cuyo nombre
        ' comience con "img" ...
        For Each nombreArchivo As String In System.IO.Directory.GetFiles(".", "img*")
            ' Intentamos abrirlo como si fuera una imagen, si no simplemente omitiro
            Try

                Dim imagenNueva As Bitmap = Bitmap.FromFile(nombreArchivo)
                ' Creamos un botón que contendrá una imagen que irá en el memorama que contendrá:
                '  - Una lista de imágenes que se pueden mostrar, simulando una carta de poker
                '    tapada o destapada para poder visualizar su estado en el juego.
                '    - La lista incluye una etiqueta que será el nombre del archivo del que fué
                '      tomada la imagen, utilizaremos este dato a la hora de comprobar pares.
                '    - La primera imagen de la lista será la imagen que se tiene que encontrar su par
                '    - La segunda será su imagen cuando la carta está tapada (mostrada por default).
                Dim botonNuevo As New Button() With {
                    .ImageList = New ImageList() With {
                        .ImageSize = tamCartas,
                        .ColorDepth = ColorDepth.Depth24Bit,
                        .Tag = nombreArchivo
                    },
                    .Size = tamCartas
                }
                ' Agregamos las imágenes a la lista antes de crear el botón del par.
                ' Lo que pasa es que a pesar de que cuando asignamos la misma lista de imágenes
                ' al otro botón y que esto se hace por referencia, lo cual implica que ambos botones tendrán
                ' las mismas imágenes aunque se le asigne a un sólo botón, pero al hacerlo
                ' sólo se actualiza en la pantalla el botón que era poseedor original de la lista
                ' de imágenes. Entonces para hacer que los 2 se actualicen al mismo tiempo, primero
                ' agregamos todas las imágenes de un botón antes de pasar la referencia de la lista
                ' al otro botón :D
                botonNuevo.ImageList.Images.Add(imagenNueva)
                botonNuevo.ImageList.Images.Add(My.Resources.Carta)

                ' Creamos un botón para la carta par con las mismas propiedades que el botón de su par.
                Dim botonCopia As New Button() With {
                    .ImageList = botonNuevo.ImageList,
                    .Size = botonNuevo.Size
                }

                ' Asociamos la subritina al evento clic de los botones y los agregamos a la lista de botones.
                AddHandler botonNuevo.Click, AddressOf ClickBotonCarta
                AddHandler botonCopia.Click, AddressOf ClickBotonCarta
                botonesCartas.Add(botonNuevo)
                botonesCartas.Add(botonCopia)
                Controls.Add(botonNuevo)
                Controls.Add(botonCopia)

            Catch ex As OutOfMemoryException

            End Try
        Next

        If botonesCartas.Count = 0 Then
            MsgBox("Lo sentimos, no se ha encontrado con ninguna imagen en el directorio.", MsgBoxStyle.Critical, "Error")
            Close()
            Exit Sub
        End If

        ' Número de cartas que se acomodarán a lo largo del formulario y será igual a la parte entera
        ' de 3/4 de la raíz cuadrada del número total de botones.
        cartasAlto = Math.Sqrt(botonesCartas.Count) * 3 / 4
        cartasAncho = Math.Ceiling(botonesCartas.Count / cartasAlto)
        ' Redimensionamos el formulario para que quepan exatmanete todas las cartas y lo localizamos a la mitad de la pantalla.
        Size = New Size(cartasAncho * (tamCartas.Width + 20) + 20, cartasAlto * (tamCartas.Height + 20) + 50 + btnSalir.Height + lblTiempo.Height)
        Location = New Point((Screen.PrimaryScreen.Bounds.Width - Size.Width) / 2, (Screen.PrimaryScreen.Bounds.Height - Size.Height) / 2)
        ' Acomodamos todas las cartas en el formulario reiniciando la partida.
        ReiniciarPartida()

    End Sub

    Private Sub ReiniciarPartida()

        ' Reiniciar las variables
        botonApuntado = Nothing
        paresEncontrados = 0
        errores = 0
        tiempoJuego = New TimeSpan()
        Timer1.Enabled = True
        lblErrores.Text = "0"

        ' Para cada fila de cartas...
        For Y = 0 To cartasAlto - 1
            ' Para cada columna de cartas...
            ' el ciclo se detendrá si se acabaron las cartas aunque no se haya acabado la fila
            For X = 0 To IIf(Y = cartasAlto - 1, (botonesCartas.Count Mod cartasAncho) - 1, cartasAncho - 1)
                ' Índice del botón que se procesa actualmente de la lista creada anteriormente.
                Dim elemento As Integer = Y * cartasAncho + X
                ' Ahora asignamos una posición aleatoria para cada carta y para ello
                ' utilizamos la propiedad Tag del botón que contendrá un entero representanto el
                ' índice de posiciónamiento en el formulario y puede ir de 0 al número total de
                ' botones. Este valor se lo asignaremos de forma aleatoria y comprobamos si no
                ' se ha repetido anteriormente en otro botón.
                Dim repetir As Boolean
                Do
                    botonesCartas(elemento).Tag = aleatorio.Next(botonesCartas.Count)
                    repetir = False
                    For C = 0 To elemento - 1
                        If botonesCartas(C).Tag = botonesCartas(elemento).Tag Then
                            repetir = True
                            Exit For
                        End If
                    Next
                Loop While repetir
                ' Antes de posicionarla, hacemos que el botón muestre la imagen de la carta tapada y quite su borde.
                botonesCartas(elemento).ImageIndex = 1
                botonesCartas(elemento).FlatStyle = FlatStyle.Flat
                botonesCartas(elemento).Location = New Point((botonesCartas(elemento).Tag Mod cartasAncho) * (tamCartas.Width + 20) + 20, (botonesCartas(elemento).Tag \ cartasAncho) * (tamCartas.Height + 20) + 30 + lblTiempo.Height)
                ' Hacemos que el índice de navegación por el tabulador sea el mismo que si índice de
                ' posicionamiento en el formulario para evitar trampas con el tabulador.
                botonesCartas(elemento).TabIndex = botonesCartas(elemento).Tag
            Next
        Next
    End Sub

    ''' <summary>
    ''' Función que se manda a llamar cada que se aprieta un botón.
    ''' </summary>
    ''' <param name="sender">Botón que mandó a llamar el evento.</param>
    ''' <param name="e">Parámetros del evento.</param>
    Private Sub ClickBotonCarta(sender As Object, e As EventArgs)

        ' Casteamos el objeto que mandó a llamar el evento, el cual obviamente fué un botón.
        Dim boton As Button = DirectCast(sender, Button)
        ' Si hemos pulsado sobre un botón que ya tenía una carta destapada, no hacer nada.
        If boton.ImageIndex = 0 Then Exit Sub

        ' Tomamos una acción dependiendo de si ya teníamos o no una carta destapada ne juego.
        If Not botonApuntado Is Nothing Then
            ' Si hemos destapado una carta y ya teníamos una carta destapada en juego, mostrar
            ' la recién destapada.
            boton.ImageIndex = 0
            ' Si hemos encontrado su par ...
            If boton.ImageList.Tag.Equals(botonApuntado.ImageList.Tag) Then
                ' Incrementar el número de pares encontrados
                paresEncontrados += 1
                ' Si ya encontró todos los pares solicitar si desea repetir el juego.
                If paresEncontrados = botonesCartas.Count / 2 Then
                    Timer1.Enabled = False
                    If MsgBox("¡Felicidades!, ha completado el juego." & vbCrLf & "¿Desea volver a jugar?", MsgBoxStyle.YesNo, "Juego completado") = MsgBoxResult.Yes Then
                        ReiniciarPartida()
                        Exit Sub
                    Else
                        Close()
                    End If
                End If
            Else
                ' Ocultar ambas cartas y mostrar un mensaje de par incorrecto.
                errores += 1
                lblErrores.Text = errores
                MsgBox("Vuelva a intentarlo", MsgBoxStyle.Exclamation, "Carta incorrecta")
                boton.ImageIndex = 1
                botonApuntado.ImageIndex = 1
            End If
            ' Devolvemos el borde del botón de la carta destapada en juego a la normalidad
            botonApuntado.FlatStyle = FlatStyle.Flat
            ' Ya se han destapado 2 cartas, no importa si se encontró su par o no,
            ' indicar que ya no hay cartas en juego a comprobar par.
            botonApuntado = Nothing
        Else
            ' Si no había cartas en juego destapadas, destapar la seleccionada.
            botonApuntado = boton
            boton.FlatStyle = FlatStyle.Standard
            boton.ImageIndex = 0
        End If

    End Sub

    Private Sub btnSalir_Click(sender As Object, e As EventArgs) Handles btnSalir.Click
        Timer1.Enabled = False
        If MsgBox("¿Seguro que desea salir?", MsgBoxStyle.YesNo, "Salir") = MsgBoxResult.Yes Then
            Close()
        End If
        Timer1.Enabled = True
    End Sub

    Private Sub btnReiniciar_Click(sender As Object, e As EventArgs) Handles btnReiniciar.Click
        Timer1.Enabled = False
        If MsgBox("¿Desea abandonar la partida en curso para reacomodar las cartas?", MsgBoxStyle.YesNo, "Reiniciar") = MsgBoxResult.Yes Then
            ReiniciarPartida()
        End If
        Timer1.Enabled = True
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        tiempoJuego += New TimeSpan(0, 0, 1)
        lblTiempo.Text = tiempoJuego.ToString()
    End Sub
End Class

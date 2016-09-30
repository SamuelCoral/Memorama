<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmJuego
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.btnSalir = New System.Windows.Forms.Button()
        Me.btnReiniciar = New System.Windows.Forms.Button()
        Me.lblTiempo = New System.Windows.Forms.Label()
        Me.lblErrores = New System.Windows.Forms.Label()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'btnSalir
        '
        Me.btnSalir.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSalir.Location = New System.Drawing.Point(197, 226)
        Me.btnSalir.Name = "btnSalir"
        Me.btnSalir.Size = New System.Drawing.Size(75, 23)
        Me.btnSalir.TabIndex = 0
        Me.btnSalir.Text = "Salir"
        Me.btnSalir.UseVisualStyleBackColor = True
        '
        'btnReiniciar
        '
        Me.btnReiniciar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnReiniciar.Location = New System.Drawing.Point(13, 225)
        Me.btnReiniciar.Name = "btnReiniciar"
        Me.btnReiniciar.Size = New System.Drawing.Size(75, 23)
        Me.btnReiniciar.TabIndex = 1
        Me.btnReiniciar.Text = "Reiniciar"
        Me.btnReiniciar.UseVisualStyleBackColor = True
        '
        'lblTiempo
        '
        Me.lblTiempo.AutoSize = True
        Me.lblTiempo.BackColor = System.Drawing.Color.Transparent
        Me.lblTiempo.Font = New System.Drawing.Font("Arial", 26.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTiempo.ForeColor = System.Drawing.Color.Lime
        Me.lblTiempo.Location = New System.Drawing.Point(13, 13)
        Me.lblTiempo.Name = "lblTiempo"
        Me.lblTiempo.Size = New System.Drawing.Size(154, 41)
        Me.lblTiempo.TabIndex = 2
        Me.lblTiempo.Text = "00:00:00"
        '
        'lblErrores
        '
        Me.lblErrores.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblErrores.AutoSize = True
        Me.lblErrores.BackColor = System.Drawing.Color.Transparent
        Me.lblErrores.Font = New System.Drawing.Font("Arial", 24.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblErrores.ForeColor = System.Drawing.Color.Red
        Me.lblErrores.Location = New System.Drawing.Point(191, 16)
        Me.lblErrores.Name = "lblErrores"
        Me.lblErrores.Size = New System.Drawing.Size(33, 36)
        Me.lblErrores.TabIndex = 3
        Me.lblErrores.Text = "0"
        '
        'Timer1
        '
        Me.Timer1.Interval = 1000
        '
        'FrmJuego
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = Global.Memorama.My.Resources.Resources.cerebro
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(284, 261)
        Me.Controls.Add(Me.lblErrores)
        Me.Controls.Add(Me.lblTiempo)
        Me.Controls.Add(Me.btnReiniciar)
        Me.Controls.Add(Me.btnSalir)
        Me.Name = "FrmJuego"
        Me.Text = "Memorama"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnSalir As Button
    Friend WithEvents btnReiniciar As Button
    Friend WithEvents lblTiempo As Label
    Friend WithEvents lblErrores As Label
    Friend WithEvents Timer1 As Timer
End Class

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.txtBoxDMC = New System.Windows.Forms.TextBox()
        Me.lblDMC = New System.Windows.Forms.Label()
        Me.lblStatusDB = New System.Windows.Forms.Label()
        Me.lblData = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblMessage = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'txtBoxDMC
        '
        Me.txtBoxDMC.Location = New System.Drawing.Point(12, 57)
        Me.txtBoxDMC.Name = "txtBoxDMC"
        Me.txtBoxDMC.Size = New System.Drawing.Size(485, 20)
        Me.txtBoxDMC.TabIndex = 0
        '
        'lblDMC
        '
        Me.lblDMC.AutoSize = True
        Me.lblDMC.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDMC.Location = New System.Drawing.Point(12, 37)
        Me.lblDMC.Name = "lblDMC"
        Me.lblDMC.Size = New System.Drawing.Size(38, 17)
        Me.lblDMC.TabIndex = 1
        Me.lblDMC.Text = "DMC"
        '
        'lblStatusDB
        '
        Me.lblStatusDB.AutoSize = True
        Me.lblStatusDB.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatusDB.Location = New System.Drawing.Point(75, 9)
        Me.lblStatusDB.Name = "lblStatusDB"
        Me.lblStatusDB.Size = New System.Drawing.Size(48, 17)
        Me.lblStatusDB.TabIndex = 2
        Me.lblStatusDB.Text = "Status"
        '
        'lblData
        '
        Me.lblData.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblData.Location = New System.Drawing.Point(110, 91)
        Me.lblData.Name = "lblData"
        Me.lblData.Size = New System.Drawing.Size(387, 101)
        Me.lblData.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(63, 17)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "SQL DB:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(12, 91)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(99, 17)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Response DB:"
        '
        'lblMessage
        '
        Me.lblMessage.AutoSize = True
        Me.lblMessage.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMessage.Location = New System.Drawing.Point(49, 37)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.Size = New System.Drawing.Size(0, 17)
        Me.lblMessage.TabIndex = 6
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(512, 201)
        Me.Controls.Add(Me.lblMessage)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblData)
        Me.Controls.Add(Me.lblStatusDB)
        Me.Controls.Add(Me.lblDMC)
        Me.Controls.Add(Me.txtBoxDMC)
        Me.KeyPreview = True
        Me.Name = "MainForm"
        Me.Text = "DMC Verifier"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtBoxDMC As TextBox
    Friend WithEvents lblDMC As Label
    Friend WithEvents lblStatusDB As Label
    Friend WithEvents lblData As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents lblMessage As Label
End Class

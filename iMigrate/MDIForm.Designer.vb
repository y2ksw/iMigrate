<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MDIForm
    Inherits System.Windows.Forms.Form

    'Form esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
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

    'Richiesto da Progettazione Windows Form
    Private components As System.ComponentModel.IContainer

    'NOTA: la procedura che segue è richiesta da Progettazione Windows Form
    'Può essere modificata in Progettazione Windows Form.  
    'Non modificarla mediante l'editor del codice.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EsciToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UtilitàToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MozillaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FirefoxToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BackupToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RestoreToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ThunderbirdToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BackupToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.RestoreToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.UtilitàToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1118, 24)
        Me.MenuStrip1.TabIndex = 1
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EsciToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "&File"
        '
        'EsciToolStripMenuItem
        '
        Me.EsciToolStripMenuItem.Name = "EsciToolStripMenuItem"
        Me.EsciToolStripMenuItem.Size = New System.Drawing.Size(94, 22)
        Me.EsciToolStripMenuItem.Text = "&Esci"
        '
        'UtilitàToolStripMenuItem
        '
        Me.UtilitàToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MozillaToolStripMenuItem})
        Me.UtilitàToolStripMenuItem.Name = "UtilitàToolStripMenuItem"
        Me.UtilitàToolStripMenuItem.Size = New System.Drawing.Size(50, 20)
        Me.UtilitàToolStripMenuItem.Text = "&Utilità"
        '
        'MozillaToolStripMenuItem
        '
        Me.MozillaToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FirefoxToolStripMenuItem, Me.ThunderbirdToolStripMenuItem})
        Me.MozillaToolStripMenuItem.Name = "MozillaToolStripMenuItem"
        Me.MozillaToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.MozillaToolStripMenuItem.Text = "&Mozilla"
        '
        'FirefoxToolStripMenuItem
        '
        Me.FirefoxToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BackupToolStripMenuItem, Me.RestoreToolStripMenuItem})
        Me.FirefoxToolStripMenuItem.Name = "FirefoxToolStripMenuItem"
        Me.FirefoxToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.FirefoxToolStripMenuItem.Text = "&Firefox"
        '
        'BackupToolStripMenuItem
        '
        Me.BackupToolStripMenuItem.Name = "BackupToolStripMenuItem"
        Me.BackupToolStripMenuItem.Size = New System.Drawing.Size(125, 22)
        Me.BackupToolStripMenuItem.Text = "&Backup ..."
        '
        'RestoreToolStripMenuItem
        '
        Me.RestoreToolStripMenuItem.Name = "RestoreToolStripMenuItem"
        Me.RestoreToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.RestoreToolStripMenuItem.Text = "&Restore ..."
        '
        'ThunderbirdToolStripMenuItem
        '
        Me.ThunderbirdToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BackupToolStripMenuItem1, Me.RestoreToolStripMenuItem1})
        Me.ThunderbirdToolStripMenuItem.Name = "ThunderbirdToolStripMenuItem"
        Me.ThunderbirdToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.ThunderbirdToolStripMenuItem.Text = "&Thunderbird"
        Me.ThunderbirdToolStripMenuItem.Visible = False
        '
        'BackupToolStripMenuItem1
        '
        Me.BackupToolStripMenuItem1.Name = "BackupToolStripMenuItem1"
        Me.BackupToolStripMenuItem1.Size = New System.Drawing.Size(152, 22)
        Me.BackupToolStripMenuItem1.Text = "&Backup ..."
        '
        'RestoreToolStripMenuItem1
        '
        Me.RestoreToolStripMenuItem1.Name = "RestoreToolStripMenuItem1"
        Me.RestoreToolStripMenuItem1.Size = New System.Drawing.Size(152, 22)
        Me.RestoreToolStripMenuItem1.Text = "&Restore ..."
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'MDIForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1118, 547)
        Me.Controls.Add(Me.MenuStrip1)
        Me.IsMdiContainer = True
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "MDIForm"
        Me.Text = "Form1"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents FileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents EsciToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents UtilitàToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents MozillaToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents FirefoxToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents BackupToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RestoreToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ThunderbirdToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents BackupToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents RestoreToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents SaveFileDialog1 As SaveFileDialog
    Friend WithEvents OpenFileDialog1 As OpenFileDialog
End Class

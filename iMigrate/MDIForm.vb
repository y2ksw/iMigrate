Public Class MDIForm
    Private Sub MDIForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Text = $"{Application.ProductName} {Application.ProductVersion}"

    End Sub

    Private Sub BackupToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BackupToolStripMenuItem.Click

        Dim a As String
        Dim zipfilename As String

        If MsgBox("Si desidera salvare il profilo corrente di Mozilla Firefox?", MsgBoxStyle.Question Or MsgBoxStyle.YesNoCancel, "Backup") <> MsgBoxResult.Yes Then
            Exit Sub
        End If

        With SaveFileDialog1
            .AddExtension = True
            .OverwritePrompt = True
            .DefaultExt = "zip"
            If .FileName.Length = 0 Then
                a = $"firefox-{My.User.Name}-{Now.ToString("yyyy-MM-dd-HH-mm-ss")}.zip"
                a = Replace(a, "\", "_")
                .FileName = a
            End If
            .ShowDialog()
            If .FileName.Length = 0 Then
                Exit Sub
            End If
            zipfilename = .FileName
        End With

    End Sub
End Class

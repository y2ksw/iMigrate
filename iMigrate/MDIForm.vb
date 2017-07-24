Imports System.IO.Compression

Public Class MDIForm

    Private Sub MDIForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Text = $"{Application.ProductName} {Application.ProductVersion}"

    End Sub

    Private Sub MDIForm_Shown(sender As Object, e As EventArgs) Handles Me.Shown

        Timer1.Enabled = True

    End Sub

    ''' <summary>
    ''' Mozilla Firefox Backup.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub BackupToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BackupToolStripMenuItem.Click

        Dim a As String
        Dim zipfilename As String
        Dim p As String = Path.GetFullPath($"{Application.LocalUserAppDataPath}\..\..\..\..")
        Dim username = Split(p, "\")(2)
        Dim inifilename As String = $"{p}\Roaming\Mozilla\Firefox\profiles.ini"
        Dim localpath As String
        Dim roamingpath As String
        Dim zipfilename2 As String
        Dim zipfilename3 As String
        Dim ini As INIFile
        Dim tmp1 As String
        Dim tmp2 As String

        If Not File.Exists(inifilename) Then
            MsgBox("Non è stato trovato un profilo Mozilla Firefox valido!", MsgBoxStyle.Critical, "ERRORE")
            Exit Sub
        End If

        If UsefulFunctions.ProcessExists("Firefox") Then
            MsgBox("E' attualmente attivo Mozilla Firefox! Chiudere il browser e riprovare.", MsgBoxStyle.Exclamation, "AVVISO")
            Exit Sub
        End If

        With SaveFileDialog1
            .AddExtension = True
            .OverwritePrompt = True
            .DefaultExt = "zip"
            a = $"firefox-{username}-{Now.ToString("yyyy-MM-dd-HH-mm-ss")}.zip"
            a = Replace(a, "\", "_")
            .FileName = a
            If .ShowDialog() <> DialogResult.OK Then
                Exit Sub
            End If
            zipfilename = .FileName
        End With

        ini = New INIFile(inifilename)
        ini.Load()
        localpath = $"{p}\Local\Mozilla\Firefox\{Replace(ini.data("Profile0")("Path"), "/", "\")}"
        roamingpath = $"{p}\Roaming\Mozilla\Firefox\{Replace(ini.data("Profile0")("Path"), "/", "\")}"

        Cursor = Cursors.WaitCursor
        zipfilename2 = Path.GetDirectoryName(zipfilename) & "\local-" & Path.GetFileName(zipfilename)
        tmp1 = Path.GetTempFileName
        UsefulFunctions.FileDelete(tmp1)
        Try
            ZipFile.CreateFromDirectory(localpath, tmp1)
        Catch ex As Exception
            UsefulFunctions.FileDelete(tmp1)
            MsgBox(ex.Message, MsgBoxStyle.Critical, "ERRORE")
            Exit Sub
        Finally
            Cursor = Cursors.Default
        End Try

        Cursor = Cursors.WaitCursor
        zipfilename3 = Path.GetDirectoryName(zipfilename) & "\roaming-" & Path.GetFileName(zipfilename)
        tmp2 = Path.GetTempFileName
        UsefulFunctions.FileDelete(tmp2)
        Try
            ZipFile.CreateFromDirectory(roamingpath, tmp2)
        Catch ex As Exception
            UsefulFunctions.FileDelete(tmp1)
            UsefulFunctions.FileDelete(tmp2)
            MsgBox(ex.Message, MsgBoxStyle.Critical, "ERRORE")
            Exit Sub
        Finally
            Cursor = Cursors.Default
        End Try

        Cursor = Cursors.WaitCursor
        Try
            File.Move(tmp1, zipfilename2)
            File.Move(tmp2, zipfilename3)
        Catch ex As Exception
            UsefulFunctions.FileDelete(tmp1)
            UsefulFunctions.FileDelete(tmp2)
            MsgBox(ex.Message, MsgBoxStyle.Critical, "ERRORE")
            Exit Sub
        Finally
            Cursor = Cursors.Default
        End Try

        MsgBox($"Il backup è stato creato nella cartella '{Path.GetDirectoryName(zipfilename)}'!", MsgBoxStyle.Information, "MOZILLA FIREFOX BACKUP")

        Shell("explorer " & """" & Path.GetDirectoryName(zipfilename) & """", AppWinStyle.NormalFocus)

    End Sub

    ''' <summary>
    ''' Mozilla Firefox Restore.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub RestoreToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RestoreToolStripMenuItem.Click

        Dim zipfilenames As String()
        Dim p As String = Path.GetFullPath($"{Application.LocalUserAppDataPath}\..\..\..\..")
        Dim username = Split(p, "\")(2)
        Dim inifilename As String = $"{p}\Roaming\Mozilla\Firefox\profiles.ini"
        Dim localpath As String
        Dim roamingpath As String
        Dim zipfilename2 As String
        Dim zipfilename3 As String
        Dim ini As INIFile

        If Not File.Exists(inifilename) Then
            MsgBox("Non è stato trovato un profilo Mozilla Firefox valido!", MsgBoxStyle.Critical, "ERRORE")
            Exit Sub
        End If

        If UsefulFunctions.ProcessExists("Firefox") Then
            MsgBox("E' attualmente attivo Mozilla Firefox! Chiudere il browser e riprovare.", MsgBoxStyle.Exclamation, "AVVISO")
            Exit Sub
        End If

        With OpenFileDialog1
            .AddExtension = True
            .CheckFileExists = True
            .DefaultExt = "zip"
            .FileName = $"*-firefox-{username}-*.zip"
            .Multiselect = True
            If .ShowDialog() <> DialogResult.OK Then
                Exit Sub
            End If
            Select Case .FileNames.Count
                Case 0
                    Exit Sub
                Case 2
                Case Else
                    MsgBox("E' necessario selezionare entrambi i file di backup 'local-firefox-*' e 'roaming-firefox-*' per ripristinare la configurazione di Mozilla Firefox!", MsgBoxStyle.Exclamation, "AVVISO")
                    Exit Sub
            End Select
            zipfilenames = .FileNames
        End With

        ini = New INIFile(inifilename)
        ini.Load()
        localpath = $"{p}\Local\Mozilla\Firefox\{Replace(ini.data("Profile0")("Path"), "/", "\")}"
        roamingpath = $"{p}\Roaming\Mozilla\Firefox\{Replace(ini.data("Profile0")("Path"), "/", "\")}"

        zipfilename2 = zipfilenames(0)
        zipfilename3 = zipfilenames(1)
        If Mid(Path.GetFileName(zipfilename3), 1, 5) = "local" Then
            zipfilename2 = zipfilenames(1)
            zipfilename3 = zipfilenames(0)
        ElseIf Mid(Path.GetFileName(zipfilename2), 1, 7) = "roaming" Then
            zipfilename2 = zipfilenames(1)
            zipfilename3 = zipfilenames(0)
        End If

        Cursor = Cursors.WaitCursor
        UsefulFunctions.DirectoryBAK(localpath)
        Try
            ZipFile.ExtractToDirectory(zipfilename2, localpath)
        Catch ex As Exception
            UsefulFunctions.DirectoryRestoreBAK(localpath)
            MsgBox(ex.Message, MsgBoxStyle.Critical, "ERRORE")
            Exit Sub
        Finally
            Cursor = Cursors.Default
        End Try

        Cursor = Cursors.WaitCursor
        UsefulFunctions.DirectoryBAK(roamingpath)
        Try
            ZipFile.ExtractToDirectory(zipfilename3, roamingpath)
        Catch ex As Exception
            UsefulFunctions.DirectoryRestoreBAK(localpath)
            UsefulFunctions.DirectoryRestoreBAK(roamingpath)
            MsgBox(ex.Message, MsgBoxStyle.Critical, "ERRORE")
            Exit Sub
        Finally
            Cursor = Cursors.Default
        End Try

        UsefulFunctions.DirectoryDelete(localpath & ".bak")
        UsefulFunctions.DirectoryDelete(roamingpath & ".bak")

        MsgBox("Il backup è stato ripristinato!", MsgBoxStyle.Information, "MOZILLA FIREFOX RESTORE")

    End Sub

    ''' <summary>
    ''' Mozilla Thunderbird Backup.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub BackupToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles BackupToolStripMenuItem1.Click

        Dim a As String
        Dim zipfilename As String
        Dim p As String = Path.GetFullPath($"{Application.LocalUserAppDataPath}\..\..\..\..")
        Dim username = Split(p, "\")(2)
        Dim inifilename As String = $"{p}\Roaming\Thunderbird\profiles.ini"
        Dim localpath As String
        Dim roamingpath As String
        Dim zipfilename2 As String
        Dim zipfilename3 As String
        Dim ini As INIFile
        Dim tmp1 As String
        Dim tmp2 As String

        If Not File.Exists(inifilename) Then
            MsgBox("Non è stato trovato un profilo Mozilla Thunderbird valido!", MsgBoxStyle.Critical, "ERRORE")
            Exit Sub
        End If

        If UsefulFunctions.ProcessExists("Thunderbird") Then
            MsgBox("E' attualmente attivo Mozilla Thunderbird! Chiudere il mail client e riprovare.", MsgBoxStyle.Exclamation, "AVVISO")
            Exit Sub
        End If

        With SaveFileDialog1
            .AddExtension = True
            .OverwritePrompt = True
            .DefaultExt = "zip"
            a = $"thunderbird-{username}-{Now.ToString("yyyy-MM-dd-HH-mm-ss")}.zip"
            a = Replace(a, "\", "_")
            .FileName = a
            If .ShowDialog() <> DialogResult.OK Then
                Exit Sub
            End If
            If .FileName.Length = 0 Then
                Exit Sub
            End If
            zipfilename = .FileName
        End With

        ini = New INIFile(inifilename)
        ini.Load()
        localpath = $"{p}\Local\Thunderbird\{Replace(ini.data("Profile0")("Path"), "/", "\")}"
        roamingpath = $"{p}\Roaming\Thunderbird\{Replace(ini.data("Profile0")("Path"), "/", "\")}"

        Cursor = Cursors.WaitCursor
        zipfilename2 = Path.GetDirectoryName(zipfilename) & "\local-" & Path.GetFileName(zipfilename)
        tmp1 = Path.GetTempFileName
        UsefulFunctions.FileDelete(tmp1)
        Try
            ZipFile.CreateFromDirectory(localpath, tmp1)
        Catch ex As Exception
            UsefulFunctions.FileDelete(tmp1)
            MsgBox(ex.Message, MsgBoxStyle.Critical, "ERRORE")
            Exit Sub
        Finally
            Cursor = Cursors.Default
        End Try

        Cursor = Cursors.WaitCursor
        zipfilename3 = Path.GetDirectoryName(zipfilename) & "\roaming-" & Path.GetFileName(zipfilename)
        tmp2 = Path.GetTempFileName
        UsefulFunctions.FileDelete(tmp2)
        Try
            ZipFile.CreateFromDirectory(roamingpath, tmp2)
        Catch ex As Exception
            UsefulFunctions.FileDelete(tmp1)
            UsefulFunctions.FileDelete(tmp2)
            MsgBox(ex.Message, MsgBoxStyle.Critical, "ERRORE")
            Exit Sub
        Finally
            Cursor = Cursors.Default
        End Try

        Cursor = Cursors.WaitCursor
        Try
            File.Move(tmp1, zipfilename2)
            File.Move(tmp2, zipfilename3)
        Catch ex As Exception
            UsefulFunctions.FileDelete(tmp1)
            UsefulFunctions.FileDelete(tmp2)
            MsgBox(ex.Message, MsgBoxStyle.Critical, "ERRORE")
            Exit Sub
        Finally
            Cursor = Cursors.Default
        End Try

        MsgBox($"Il backup è stato creato nella cartella '{Path.GetDirectoryName(zipfilename)}'!", MsgBoxStyle.Information, "MOZILLA THUNDERBIRD BACKUP")

        Shell("explorer " & """" & Path.GetDirectoryName(zipfilename) & """", AppWinStyle.NormalFocus)

    End Sub

    ''' <summary>
    ''' Mozilla Thunderbird Restore.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub RestoreToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles RestoreToolStripMenuItem1.Click

        Dim zipfilenames As String()
        Dim p As String = Path.GetFullPath($"{Application.LocalUserAppDataPath}\..\..\..\..")
        Dim username = Split(p, "\")(2)
        Dim inifilename As String = $"{p}\Roaming\Thunderbird\profiles.ini"
        Dim localpath As String
        Dim roamingpath As String
        Dim zipfilename2 As String
        Dim zipfilename3 As String
        Dim ini As INIFile

        If Not File.Exists(inifilename) Then
            MsgBox("Non è stato trovato un profilo Mozilla Thunderbird valido!", MsgBoxStyle.Critical, "ERRORE")
            Exit Sub
        End If

        If UsefulFunctions.ProcessExists("Thunderbird") Then
            MsgBox("E' attualmente attivo Mozilla Thunderbird! Chiudere il mail client e riprovare.", MsgBoxStyle.Exclamation, "AVVISO")
            Exit Sub
        End If

        With OpenFileDialog1
            .AddExtension = True
            .CheckFileExists = True
            .DefaultExt = "zip"
            .FileName = $"*-thunderbird-{username}-*.zip"
            .Multiselect = True
            If .ShowDialog() <> DialogResult.OK Then
                Exit Sub
            End If
            Select Case .FileNames.Count
                Case 0
                    Exit Sub
                Case 2
                Case Else
                    MsgBox("E' necessario selezionare entrambi i file di backup 'local-thunderbird-*' e 'roaming-thunderbird-*' per ripristinare la configurazione di Mozilla Thunderbird!", MsgBoxStyle.Exclamation, "AVVISO")
                    Exit Sub
            End Select
            zipfilenames = .FileNames
        End With

        ini = New INIFile(inifilename)
        ini.Load()
        localpath = $"{p}\Local\Thunderbird\{Replace(ini.data("Profile0")("Path"), "/", "\")}"
        roamingpath = $"{p}\Roaming\Thunderbird\{Replace(ini.data("Profile0")("Path"), "/", "\")}"

        zipfilename2 = zipfilenames(0)
        zipfilename3 = zipfilenames(1)
        If Mid(Path.GetFileName(zipfilename3), 1, 5) = "local" Then
            zipfilename2 = zipfilenames(1)
            zipfilename3 = zipfilenames(0)
        ElseIf Mid(Path.GetFileName(zipfilename2), 1, 7) = "roaming" Then
            zipfilename2 = zipfilenames(1)
            zipfilename3 = zipfilenames(0)
        End If

        Cursor = Cursors.WaitCursor
        UsefulFunctions.DirectoryBAK(localpath)
        Try
            ZipFile.ExtractToDirectory(zipfilename2, localpath)
        Catch ex As Exception
            UsefulFunctions.DirectoryRestoreBAK(localpath)
            MsgBox(ex.Message, MsgBoxStyle.Critical, "ERRORE")
            Exit Sub
        Finally
            Cursor = Cursors.Default
        End Try

        Cursor = Cursors.WaitCursor
        UsefulFunctions.DirectoryBAK(roamingpath)
        Try
            ZipFile.ExtractToDirectory(zipfilename3, roamingpath)
        Catch ex As Exception
            UsefulFunctions.DirectoryRestoreBAK(localpath)
            UsefulFunctions.DirectoryRestoreBAK(roamingpath)
            MsgBox(ex.Message, MsgBoxStyle.Critical, "ERRORE")
            Exit Sub
        Finally
            Cursor = Cursors.Default
        End Try

        UsefulFunctions.DirectoryDelete(localpath & ".bak")
        UsefulFunctions.DirectoryDelete(roamingpath & ".bak")

        MsgBox("Il backup è stato ripristinato!", MsgBoxStyle.Information, "MOZILLA THUNDERBIRD RESTORE")

    End Sub

    Private Sub ContattiToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ContattiToolStripMenuItem.Click

        Contacts.ShowDialog()

    End Sub

    Private Sub EsciToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EsciToolStripMenuItem.Click

        Environment.Exit(0)

    End Sub

    Private Sub AggiornaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AggiornaToolStripMenuItem.Click

        Dim wu As WebUpdate = New WebUpdate()

        If Not wu.HaveInternetConnection() Then
            MsgBox("Non è disponibile Internet!", MsgBoxStyle.Exclamation, "AVVISO")
            Exit Sub
        End If

        If Not wu.IsUpdateAvailable Then
            MsgBox("Non è disponibile alcun aggiornamento.", MsgBoxStyle.Information, "AGGIORNAMENTO")
            Exit Sub
        End If

        wu.UpdateProgramAndExitIfNewer()

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        Dim wu As WebUpdate = New WebUpdate()

        Timer1.Enabled = False

        If wu.HaveInternetConnection() Then
            wu.UpdateProgramAndExitIfNewer()
        End If

    End Sub

    ''' <summary>
    ''' FileZilla FTP Client Backup.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub BackupToolStripMenuItem3_Click(sender As Object, e As EventArgs) Handles BackupToolStripMenuItem3.Click

        Dim a As String
        Dim zipfilename As String
        Dim p As String = Path.GetFullPath($"{Application.LocalUserAppDataPath}\..\..\..\..")
        Dim username = Split(p, "\")(2)
        Dim inifilename As String = $"{p}\Roaming\FileZilla\filezilla.xml"
        Dim roamingpath As String = $"{p}\Roaming\FileZilla"
        Dim tmpfilename As String

        If Not File.Exists(inifilename) Then
            MsgBox("Non è stato trovato un profilo FileZilla FTP Client valido!", MsgBoxStyle.Critical, "ERRORE")
            Exit Sub
        End If

        If UsefulFunctions.ProcessExists("filezilla") Then
            MsgBox("E' attualmente attivo FileZilla FTP Client! Chiudere l'applicazione e riprovare.", MsgBoxStyle.Exclamation, "AVVISO")
            Exit Sub
        End If

        With SaveFileDialog1
            .AddExtension = True
            .OverwritePrompt = True
            .DefaultExt = "zip"
            a = $"filezilla-client-{username}-{Now.ToString("yyyy-MM-dd-HH-mm-ss")}.zip"
            a = Replace(a, "\", "_")
            .FileName = a
            If .ShowDialog() <> DialogResult.OK Then
                Exit Sub
            End If
            zipfilename = .FileName
        End With

        Cursor = Cursors.WaitCursor
        tmpfilename = Path.GetTempFileName
        UsefulFunctions.FileDelete(tmpfilename)
        Try
            ZipFile.CreateFromDirectory(roamingpath, tmpfilename)
        Catch ex As Exception
            UsefulFunctions.FileDelete(tmpfilename)
            MsgBox(ex.Message, MsgBoxStyle.Critical, "ERRORE")
            Exit Sub
        Finally
            Cursor = Cursors.Default
        End Try

        Cursor = Cursors.WaitCursor
        Try
            File.Move(tmpfilename, zipfilename)
        Catch ex As Exception
            UsefulFunctions.FileDelete(tmpfilename)
            MsgBox(ex.Message, MsgBoxStyle.Critical, "ERRORE")
            Exit Sub
        Finally
            Cursor = Cursors.Default
        End Try

        MsgBox($"Il backup è stato creato nella cartella '{Path.GetDirectoryName(zipfilename)}'!", MsgBoxStyle.Information, "FILEZILLA FTP CLIENT BACKUP")

        Shell("explorer " & """" & Path.GetDirectoryName(zipfilename) & """", AppWinStyle.NormalFocus)

    End Sub

    ''' <summary>
    ''' FileZilla FTP Client Restore.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub RestoreToolStripMenuItem3_Click(sender As Object, e As EventArgs) Handles RestoreToolStripMenuItem3.Click

        Dim zipfilename As String
        Dim p As String = Path.GetFullPath($"{Application.LocalUserAppDataPath}\..\..\..\..")
        Dim username = Split(p, "\")(2)
        Dim inifilename As String = $"{p}\Roaming\FileZilla\filezilla.xml"
        Dim roamingpath As String = $"{p}\Roaming\FileZilla"

        If Not File.Exists(inifilename) Then
            MsgBox("Non è stato trovato un profilo FileZilla FTP Client valido!", MsgBoxStyle.Critical, "ERRORE")
            Exit Sub
        End If

        If UsefulFunctions.ProcessExists("filezilla") Then
            MsgBox("E' attualmente attivo FileZilla FTP Client! Chiudere l'applicazione e riprovare.", MsgBoxStyle.Exclamation, "AVVISO")
            Exit Sub
        End If

        With OpenFileDialog1
            .AddExtension = True
            .CheckFileExists = True
            .DefaultExt = "zip"
            .FileName = $"filezilla-client-{username}-*.zip"
            .Multiselect = True
            If .ShowDialog() <> DialogResult.OK Then
                Exit Sub
            End If
            Select Case .FileNames.Count
                Case 0
                    Exit Sub
                Case 1
                Case Else
                    MsgBox("E' necessario selezionare il file di backup 'filezilla-client-*' per ripristinare la configurazione di FileZilla FTP Client!", MsgBoxStyle.Exclamation, "AVVISO")
                    Exit Sub
            End Select
            zipfilename = .FileName
        End With

        Cursor = Cursors.WaitCursor
        UsefulFunctions.DirectoryBAK(roamingpath)
        Try
            ZipFile.ExtractToDirectory(zipfilename, roamingpath)
        Catch ex As Exception
            UsefulFunctions.DirectoryRestoreBAK(roamingpath)
            MsgBox(ex.Message, MsgBoxStyle.Critical, "ERRORE")
            Exit Sub
        Finally
            Cursor = Cursors.Default
        End Try

        UsefulFunctions.DirectoryDelete(roamingpath & ".bak")

        MsgBox("Il backup è stato ripristinato!", MsgBoxStyle.Information, "FILEZILLA FTP CLIENT RESTORE")

    End Sub

    ''' <summary>
    ''' FileZilla FTP Server Backup.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub BackupToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles BackupToolStripMenuItem2.Click

        Dim a As String
        Dim zipfilename As String
        Dim p As String = Path.GetFullPath($"{Application.LocalUserAppDataPath}\..\..\..\..")
        Dim username = Split(p, "\")(2)
        Dim apppath As String = UsefulFunctions.GetCurrentUserKeyValue("Software\FileZilla Server", "Install_Dir")
        Dim inifilename As String = $"{apppath}\FileZilla Server.xml"
        Dim tmpfilename As String

        If Not File.Exists(inifilename) Then
            MsgBox("Non è stato trovato un profilo FileZilla Server valido!", MsgBoxStyle.Critical, "ERRORE")
            Exit Sub
        End If

        If UsefulFunctions.ProcessExists("FileZilla Server Interface") Then
            MsgBox("E' attualmente attivo FileZilla Server Interface! Chiudere l'applicazione e riprovare.", MsgBoxStyle.Exclamation, "AVVISO")
            Exit Sub
        End If

        With SaveFileDialog1
            .AddExtension = True
            .OverwritePrompt = True
            .DefaultExt = "gz"
            a = $"filezilla-server-{username}-{Now.ToString("yyyy-MM-dd-HH-mm-ss")}.gz"
            a = Replace(a, "\", "_")
            .FileName = a
            If .ShowDialog() <> DialogResult.OK Then
                Exit Sub
            End If
            zipfilename = .FileName
        End With

        Cursor = Cursors.WaitCursor
        tmpfilename = Path.GetTempFileName
        UsefulFunctions.FileDelete(tmpfilename)
        Try
            Using fs As FileStream = New FileStream(tmpfilename, FileMode.Create)
                Using gz As GZipStream = New GZipStream(fs, CompressionLevel.Optimal)
                    Using src As FileStream = New FileStream(inifilename, FileMode.Open, FileAccess.Read, FileShare.Read)
                        src.CopyTo(gz)
                    End Using
                End Using
            End Using
        Catch ex As Exception
            UsefulFunctions.FileDelete(tmpfilename)
            MsgBox(ex.Message, MsgBoxStyle.Critical, "ERRORE")
            Exit Sub
        Finally
            Cursor = Cursors.Default
        End Try

        Cursor = Cursors.WaitCursor
        Try
            File.Move(tmpfilename, zipfilename)
        Catch ex As Exception
            UsefulFunctions.FileDelete(tmpfilename)
            MsgBox(ex.Message, MsgBoxStyle.Critical, "ERRORE")
            Exit Sub
        Finally
            Cursor = Cursors.Default
        End Try

        MsgBox($"Il backup è stato creato nella cartella '{Path.GetDirectoryName(zipfilename)}'!", MsgBoxStyle.Information, "FILEZILLA SERVER BACKUP")

        Shell("explorer " & """" & Path.GetDirectoryName(zipfilename) & """", AppWinStyle.NormalFocus)

    End Sub

    ''' <summary>
    ''' FileZilla FTP Server Restore.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub RestoreToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles RestoreToolStripMenuItem2.Click

        Dim a As String
        Dim zipfilename As String
        Dim p As String = Path.GetFullPath($"{Application.LocalUserAppDataPath}\..\..\..\..")
        Dim username = Split(p, "\")(2)
        Dim apppath As String = UsefulFunctions.GetCurrentUserKeyValue("Software\FileZilla Server", "Install_Dir")
        Dim inifilename As String = $"{apppath}\FileZilla Server.xml"

        If Not File.Exists(inifilename) Then
            MsgBox("Non è stato trovato un profilo FileZilla FTP Client valido!", MsgBoxStyle.Critical, "ERRORE")
            Exit Sub
        End If

        If UsefulFunctions.ProcessExists("FileZilla Server Interface") Then
            MsgBox("E' attualmente attivo FileZilla Server Interface! Chiudere l'applicazione e riprovare.", MsgBoxStyle.Exclamation, "AVVISO")
            Exit Sub
        End If

        With OpenFileDialog1
            .AddExtension = True
            .CheckFileExists = True
            .DefaultExt = "gz"
            .FileName = $"filezilla-server-{username}-*.gz"
            .Multiselect = True
            If .ShowDialog() <> DialogResult.OK Then
                Exit Sub
            End If
            Select Case .FileNames.Count
                Case 0
                    Exit Sub
                Case 1
                Case Else
                    MsgBox("E' necessario selezionare il file di backup 'filezilla-server-*' per ripristinare la configurazione di FileZilla Server!", MsgBoxStyle.Exclamation, "AVVISO")
                    Exit Sub
            End Select
            zipfilename = .FileName
        End With

        MsgBox($"Perché sia possibile sostituire il file di configurazione, aggiungere l'utente 'Everyone' con 'Controllo completo' al file '{inifilename}'. Dopo la modifica, rimuovere questi diritti!", MsgBoxStyle.Exclamation, "FILEZILLA SERVER RESTORE")

        Cursor = Cursors.WaitCursor

        Try
            Using src As FileStream = New FileStream(zipfilename, FileMode.Open, FileAccess.Read, FileShare.Read)
                Using gz As GZipStream = New GZipStream(src, CompressionMode.Decompress)
                    Using fs As FileStream = File.Create(inifilename)
                        gz.CopyTo(fs)
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "ERRORE")
            Exit Sub
        Finally
            Cursor = Cursors.Default
        End Try

        MsgBox("Il backup è stato ripristinato! Perché siano applicate le eventuali modifiche, è necessario riavviare il servizio.", MsgBoxStyle.Information, "FILEZILLA SERVER RESTORE")

    End Sub
End Class

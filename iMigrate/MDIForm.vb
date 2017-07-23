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
                    MsgBox("E' necessario selezionare i file di backup 'local*' e 'roaming*' per ripristinare la configurazione di Mozilla Firefox!", MsgBoxStyle.Exclamation, "AVVISO")
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
                    MsgBox("E' necessario selezionare i file di backup 'local*' e 'roaming*' per ripristinare la configurazione di Mozilla Thunderbird!", MsgBoxStyle.Exclamation, "AVVISO")
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

End Class

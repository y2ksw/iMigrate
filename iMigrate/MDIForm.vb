﻿Public Class MDIForm
    Private Sub MDIForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Text = $"{Application.ProductName} {Application.ProductVersion}"

    End Sub

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

        With SaveFileDialog1
            .AddExtension = True
            .OverwritePrompt = True
            .DefaultExt = "zip"
            If .FileName.Length = 0 Then
                a = $"firefox-{username}-{Now.ToString("yyyy-MM-dd-HH-mm-ss")}.zip"
                a = Replace(a, "\", "_")
                .FileName = a
            End If
            .ShowDialog()
            If .FileName.Length = 0 Then
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
        If File.Exists(tmp1) Then
            File.Delete(tmp1)
        End If
        Try
            ZipFile.CreateFromDirectory(localpath, tmp1)
        Catch ex As Exception
            If File.Exists(tmp1) Then
                File.Delete(tmp1)
            End If
            MsgBox(ex.Message, MsgBoxStyle.Critical, "ERRORE")
            Exit Sub
        Finally
            Cursor = Cursors.Default
        End Try

        Cursor = Cursors.WaitCursor
        zipfilename3 = Path.GetDirectoryName(zipfilename) & "\roaming-" & Path.GetFileName(zipfilename)
        tmp2 = Path.GetTempFileName
        If File.Exists(tmp2) Then
            File.Delete(tmp2)
        End If
        Try
            ZipFile.CreateFromDirectory(roamingpath, tmp2)
        Catch ex As Exception
            If File.Exists(tmp1) Then
                File.Delete(tmp1)
            End If
            If File.Exists(tmp2) Then
                File.Delete(tmp2)
            End If
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
            If File.Exists(tmp1) Then
                File.Delete(tmp1)
            End If
            If File.Exists(tmp2) Then
                File.Delete(tmp2)
            End If
            MsgBox(ex.Message, MsgBoxStyle.Critical, "ERRORE")
            Exit Sub
        Finally
            Cursor = Cursors.Default
        End Try

        MsgBox($"Il backup è stato creato nella cartella '{Path.GetDirectoryName(zipfilename)}'!", MsgBoxStyle.Information, "MOZILLA FIREFOX BACKUP")

        Shell("explorer " & """" & Path.GetDirectoryName(zipfilename) & """", AppWinStyle.NormalFocus)

    End Sub

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

        With OpenFileDialog1
            .AddExtension = True
            .CheckFileExists = True
            .DefaultExt = "zip"
            .FileName = $"*-firefox-{username}-*.zip"
            .Multiselect = True
            .ShowDialog()
            If .FileNames.Count <> 2 Then
                MsgBox("E' necessario selezionare i file di backup 'local*' e 'roaming*' per ripristinare la configurazione di Mozilla Firefox!", MsgBoxStyle.Exclamation, "AVVISO")
                Exit Sub
            End If
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
        Try
            If Directory.Exists(localpath & ".bak") Then
                Directory.Delete(localpath & ".bak", True)
            End If
            Directory.Move(localpath, localpath & ".bak")
            ZipFile.ExtractToDirectory(zipfilename2, localpath)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "ERRORE")
            Exit Sub
        Finally
            Cursor = Cursors.Default
        End Try

        Cursor = Cursors.WaitCursor
        Try
            If Directory.Exists(roamingpath & ".bak") Then
                Directory.Delete(roamingpath & ".bak", True)
            End If
            Directory.Move(roamingpath, roamingpath & ".bak")
            ZipFile.ExtractToDirectory(zipfilename3, roamingpath)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "ERRORE")
            Exit Sub
        Finally
            Cursor = Cursors.Default
        End Try

        If Directory.Exists(localpath & ".bak") Then
            Directory.Delete(localpath & ".bak", True)
        End If
        If Directory.Exists(roamingpath & ".bak") Then
            Directory.Delete(roamingpath & ".bak", True)
        End If

        MsgBox("Il backup è stato ripristinato!", MsgBoxStyle.Information, "MOZILLA FIREFOX RESTORE")

    End Sub
End Class

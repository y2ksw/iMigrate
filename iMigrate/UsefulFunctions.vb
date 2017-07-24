Imports Microsoft.Win32
Imports System.Diagnostics
Imports System.IO

''' <summary>
''' Contains a number of very useful and ready-to-use functions.
''' </summary>
Module UsefulFunctions

    ''' <summary>
    ''' Verifies if a process exists.
    ''' </summary>
    ''' <param name="lpszProcessName"></param>
    ''' <returns></returns>
    Public Function ProcessExists(ByVal lpszProcessName As String) As Boolean

        Dim ps As Process() = Process.GetProcessesByName(lpszProcessName)

        Return (ps.Count > 0)

    End Function

    ''' <summary>
    ''' Deletes a file and ignores errors.
    ''' </summary>
    ''' <param name="lpszFilename"></param>
    Public Sub FileDelete(ByVal lpszFilename As String)

        Try
            If File.Exists(lpszFilename) Then
                File.Delete(lpszFilename)
            End If
        Catch : End Try

    End Sub

    ''' <summary>
    ''' Moves a directory to *.bak.
    ''' </summary>
    ''' <param name="lpszPath"></param>
    Public Sub DirectoryBAK(ByVal lpszPath As String)

        DirectoryDelete(lpszPath & ".bak")

        Try
            Directory.Move(lpszPath, lpszPath & ".bak")
        Catch : End Try

    End Sub

    ''' <summary>
    ''' Deletes a directory and subfolders and ignores errors.
    ''' </summary>
    ''' <param name="lpszPath"></param>
    Public Sub DirectoryDelete(ByVal lpszPath As String)

        Try
            If Directory.Exists(lpszPath) Then
                Directory.Delete(lpszPath, True)
            End If
        Catch : End Try

    End Sub

    ''' <summary>
    ''' Restores a *.bak directory if present.
    ''' </summary>
    ''' <param name="lpszPath"></param>
    Public Sub DirectoryRestoreBAK(ByVal lpszPath As String)

        If Not Directory.Exists(lpszPath & ".bak") Then
            Return
        End If

        DirectoryDelete(lpszPath)

        Try
            Directory.Move(lpszPath & ".bak", lpszPath)
        Catch : End Try

    End Sub

    ''' <summary>
    ''' Returns a HKEY_CURRENT_USER Registry Value.
    ''' </summary>
    ''' <param name="lpszSubKey"></param>
    ''' <param name="lpszField"></param>
    ''' <returns></returns>
    Public Function GetCurrentUserKeyValue(ByVal lpszSubKey As String, ByVal lpszField As String) As String

        Dim Value As String = ""

        Try
            Using k As RegistryKey = Registry.CurrentUser.OpenSubKey(lpszSubKey)
                Value = k.GetValue(lpszField).ToString
            End Using
        Catch : End Try

        Return Value

    End Function

    ''' <summary>
    ''' Moves a file to *.bak.
    ''' </summary>
    ''' <param name="lpszPath"></param>
    Public Sub FileBAK(ByVal lpszPath As String)

        FileDelete(lpszPath & ".bak")

        Try
            File.Move(lpszPath, lpszPath & ".bak")
        Catch : End Try

    End Sub

    ''' <summary>
    ''' Restores a *.bak file if present.
    ''' </summary>
    ''' <param name="lpszPath"></param>
    Public Sub FileRestoreBAK(ByVal lpszPath As String)

        If Not File.Exists(lpszPath & ".bak") Then
            Return
        End If

        FileDelete(lpszPath)

        Try
            File.Move(lpszPath & ".bak", lpszPath)
        Catch : End Try

    End Sub

End Module

Imports System.IO
Imports IniParser
Imports IniParser.Model

Public Class INIFile

    Public Filename As String
    Public data As IniData

    Private Parser As IniParser.FileIniDataParser

    Public Sub New(ByVal lpszFilename As String)

        Parser = New FileIniDataParser
        data = New IniData

        Filename = lpszFilename

    End Sub

    Public Sub Load()

        data = Parser.ReadFile(Filename)

    End Sub

    Public Sub Save()

        Parser.WriteFile(Filename, data)

    End Sub

End Class

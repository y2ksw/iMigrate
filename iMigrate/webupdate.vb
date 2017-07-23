Imports System.IO
Imports System.Net
Imports System.Text

Public Class WebUpdate

    Private appversion As String = ""
    Private tempfilename As String = ""
    Private host As String = "www.trimelli.com"
    Private site As String = "http://www.trimelli.com/pub/imigrate"

    ''' <summary>
    ''' Constructor
    ''' </summary>
    Public Sub New()

        If Not HaveInternetConnection() Then
            appversion = Application.ProductVersion.ToString
            Return
        End If

        Using wc = New WebClient
            Try
                appversion = wc.DownloadString(site & "/version.txt")
            Catch ex As Exception
                appversion = Application.ProductVersion.ToString
            End Try
        End Using

    End Sub

    ''' <summary>
    ''' Is there an update available?
    ''' </summary>
    ''' <returns></returns>
    Public Function IsUpdateAvailable() As Boolean

        Return StrComp(appversion, Application.ProductVersion) <> 0

    End Function

    ''' <summary>
    ''' Downloads the update file
    ''' </summary>
    ''' <returns></returns>
    Public Function DownloadUpdate() As Boolean

        Dim f As String

        If tempfilename.Length > 0 Then
            If File.Exists(tempfilename) Then
                Return True
            End If
        End If

        tempfilename = Path.GetTempFileName

        Using wc = New WebClient
            Try
                wc.DownloadFile(site & "/setup.msi", tempfilename)
            Catch ex As Exception
                Return False
            End Try
        End Using

        f = $"{tempfilename}.msi"

        If File.Exists(f) Then
            File.Delete(f)
        End If
        File.Move(tempfilename, f)

        tempfilename = f

        Return True

    End Function

    Public Sub UpdateProgramAndExitIfNewer()

        Dim psi As ProcessStartInfo

        If Not IsUpdateAvailable() Then
            Exit Sub
        End If

        If MessageBox.Show("E' disponibile un aggiornamento. Si desidera aggiornare il programma ora?", "AGGIORNAMENTO", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = DialogResult.Cancel Then
            Exit Sub
        End If

        If Not DownloadUpdate() Then
            MessageBox.Show("Non è stato possibile scaricare l'aggiornamento. Il programma riproverà più tardi.", "AGGIORNAMENTO", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If tempfilename.Length > 0 Then
            If Not File.Exists(tempfilename) Then
                Exit Sub
            End If
        End If

        psi = New ProcessStartInfo With
            {
                .FileName = tempfilename,
                .WindowStyle = ProcessWindowStyle.Normal,
                .WorkingDirectory = Path.GetTempPath
            }

        Process.Start(psi)

        Environment.Exit(0)

    End Sub

    Public Function ApplicationProductVersion() As String

        Return Application.ProductVersion

    End Function

    Public Function RemoteApplicationProductVersion() As String

        Return appversion

    End Function

    Public Function HaveInternetConnection() As Boolean

        Dim iph As IPHostEntry

        Try
            iph = Dns.GetHostEntry(host)
            If iph.AddressList.First.ToString = "192.168.1.1" Then
                Return False
            End If
            Return My.Computer.Network.Ping(iph.AddressList.First.ToString)
        Catch : End Try

        Return False

    End Function

End Class

Public Class Contacts
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Close()

    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked

        LinkLabel1.LinkVisited = True

        Try
            Process.Start(LinkLabel1.Text)
        Catch : End Try

    End Sub
End Class
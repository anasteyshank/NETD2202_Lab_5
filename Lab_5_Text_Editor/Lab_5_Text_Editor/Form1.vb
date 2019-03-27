Option Strict On

Imports System.IO

Public Class frmTextEditor

#Region "Declarations"
    Const NEW_FILE As String = "File1"
    Dim filePath As String = NEW_FILE
    Dim fileContent As String = String.Empty
    Dim clickedCancel As Boolean = False
#End Region

#Region "Event Handlers"
    Private Sub mnuOpen_Click(sender As Object, e As EventArgs) Handles mnuOpen.Click

        ContentChanged()

        If Not clickedCancel Then

            Me.Text = "Open File"
            openFileDialog.Filter = "txt files (*.txt)|*.txt"

            If openFileDialog.ShowDialog() = DialogResult.OK Then
                Try
                    filePath = openFileDialog.FileName
                    Dim fileStream As New FileStream(filePath, FileMode.Open, FileAccess.Read)
                    Dim readStream As New StreamReader(fileStream)

                    txtTextEditor.Text = readStream.ReadToEnd()

                    fileContent = txtTextEditor.Text
                    RenewTitle()
                    openFileDialog.FileName = String.Empty
                    readStream.Close()
                Catch ex As Exception
                    MessageBox.Show(ex.ToString())
                End Try

            End If
        End If

        clickedCancel = False

    End Sub

    Private Sub mnuSave_Click(sender As Object, e As EventArgs) Handles mnuSave.Click
        Me.Text = "Save File"
        If filePath = NEW_FILE Then

            saveFileDialog.Filter = "txt files (*.txt)|*.txt"

            If saveFileDialog.ShowDialog = DialogResult.OK Then
                Try
                    filePath = saveFileDialog.FileName
                Catch ex As Exception
                    MessageBox.Show(ex.ToString())
                End Try
            Else
                Exit Sub
            End If
        End If
        SaveFile(filePath)
        RenewTitle()
    End Sub

    Private Sub mnuSaveAs_Click(sender As Object, e As EventArgs) Handles mnuSaveAs.Click
        Me.Text = "Save As"
        saveFileDialog.Filter = "txt file (*.txt)|*.txt"
        If saveFileDialog.ShowDialog() = DialogResult.OK Then
            Try
                filePath = saveFileDialog.FileName
                SaveFile(filePath)
                RenewTitle()
            Catch ex As Exception
                MessageBox.Show(ex.ToString())
            End Try

        End If
    End Sub

    Private Sub mnuNew_Click(sender As Object, e As EventArgs) Handles mnuNew.Click

        ContentChanged()

        If Not clickedCancel Then
            txtTextEditor.Text = String.Empty
            fileContent = txtTextEditor.Text
            filePath = NEW_FILE
            RenewTitle()
        End If

        clickedCancel = False

    End Sub

    Private Sub mnuCut_Click(sender As Object, e As EventArgs) Handles mnuCut.Click
        My.Computer.Clipboard.SetText(txtTextEditor.SelectedText)
        txtTextEditor.SelectedText() = String.Empty
    End Sub

    Private Sub mnuCopy_Click(sender As Object, e As EventArgs) Handles mnuCopy.Click
        My.Computer.Clipboard.SetText(txtTextEditor.SelectedText)
    End Sub

    Private Sub mnuPaste_Click(sender As Object, e As EventArgs) Handles mnuPaste.Click
        txtTextEditor.SelectedText() = My.Computer.Clipboard.GetText()
    End Sub

    Private Sub mnuExit_click(sender As Object, e As EventArgs) Handles mnuExit.Click
        Application.Exit()
    End Sub

    Private Sub frmTextEditor_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        e.Cancel = True
        ContentChanged()
        If Not clickedCancel Then
            e.Cancel = False
        End If
        clickedCancel = False
    End Sub

    Private Sub mnuAbout_Click(sender As Object, e As EventArgs) Handles mnuAbout.Click
        MsgBox("NETD2202 - Net Development I" & vbCrLf & vbCrLf & "Lab 5: Text Editor" & vbCrLf & vbCrLf & "Anastasiia Kononirenko" & vbCrLf & vbCrLf & "Created on March 23d 2019", MsgBoxStyle.OkOnly, "About")
    End Sub

    Private Sub frmTextEditor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RenewTitle()
    End Sub

#End Region

#Region "Subs"
    Private Sub RenewTitle()
        Me.Text = Path.GetFileName(filePath) & " - Text Editor"
    End Sub

    Private Sub SaveFile(path As String)
        Dim fileStream As New FileStream(path, FileMode.Create, FileAccess.Write)
        Dim writeStream As New StreamWriter(fileStream)

        writeStream.Write(txtTextEditor.Text)
        writeStream.Close()

        saveFileDialog.FileName = String.Empty
        fileContent = txtTextEditor.Text
    End Sub

    Private Sub ContentChanged()
        If txtTextEditor.Text <> fileContent Then
            Dim msgBoxResult = MsgBox("Do you want to save changes to " & filePath.ToString() & "?", MsgBoxStyle.YesNoCancel, "Text Editor")
            If msgBoxResult = DialogResult.Yes Then
                mnuSave.PerformClick()
            ElseIf msgBoxResult = DialogResult.Cancel Then
                clickedCancel = True
            End If
        End If
    End Sub

#End Region

End Class

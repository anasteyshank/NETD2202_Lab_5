' Author:       Anastasiia Kononirenko
' Student ID:   100717670
' Date:         27/03/2019
' Purpose:      Lab_5_Text_Editor
' Description:  A windows forms application to keep a list of cars and information about them.

Option Strict On

Imports System.IO

Public Class frmTextEditor

#Region "Declarations"
    Const NEW_FILE As String = "File1"  ' holds the default file name
    Dim filePath As String = NEW_FILE   ' holds the file path
    Dim fileContent As String = String.Empty  ' holds the file content
    Dim clickedCancel As Boolean = False      ' a variable to identify whether the user clicked the cancel button
#End Region

#Region "Event Handlers"
    ''' <summary>
    ''' mnuOpen_Click - checks if the content of the file has been changed and opens a selected file
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub mnuOpen_Click(sender As Object, e As EventArgs) Handles mnuOpen.Click
        ContentChanged()    ' check if the content of the file has been changed

        If Not clickedCancel Then   ' check if the user clcked the cancel button
            Me.Text = "Open File"   ' change the title of the form
            openFileDialog.Filter = "txt files (*.txt)|*.txt"   ' allow to only open text files

            If openFileDialog.ShowDialog() = DialogResult.OK Then   ' check if the user clicked OK in the open file dialog
                Try ' try to read the file
                    filePath = openFileDialog.FileName  ' store the file path
                    Dim fileStream As New FileStream(filePath, FileMode.Open, FileAccess.Read)  ' declare a FileStream object   
                    Dim readStream As New StreamReader(fileStream)  ' declare a StreamReader object

                    txtTextEditor.Text = readStream.ReadToEnd() ' load the data from a text file to the form
                    fileContent = txtTextEditor.Text    ' renew the file content
                    RenewTitle()    ' change the title bar
                    openFileDialog.FileName = String.Empty  ' clear the file name of the open file dialog
                    readStream.Close()  ' close the ReadStream
                Catch ex As Exception   ' catch an exception
                    MessageBox.Show(ex.ToString())
                End Try
            Else
                RenewTitle()    ' change the title bar
            End If
        End If
    End Sub

    ''' <summary>
    ''' mnuSave_Click - saves the text file
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub mnuSave_Click(sender As Object, e As EventArgs) Handles mnuSave.Click
        Me.Text = "Save File" ' change the title of the form
        If filePath = NEW_FILE Then ' check if the file is new
            saveFileDialog.Filter = "txt files (*.txt)|*.txt"   ' allow to only open text files
            If saveFileDialog.ShowDialog = DialogResult.OK Then ' check if the user clicked OK in the save file dialog
                Try
                    filePath = saveFileDialog.FileName  ' store the file path
                Catch ex As Exception   ' catch an exception
                    MessageBox.Show(ex.ToString())
                End Try
            Else
                RenewTitle()    ' change the title bar
                Exit Sub        ' exit the sub
            End If
        End If
        SaveFile(filePath)  ' call the SaveFile function
        RenewTitle()    ' change the title bar
    End Sub

    ''' <summary>
    ''' mnuSaveAs_Click - saves the text file
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub mnuSaveAs_Click(sender As Object, e As EventArgs) Handles mnuSaveAs.Click
        Me.Text = "Save As"   ' change the title of the form
        saveFileDialog.Filter = "txt file (*.txt)|*.txt"    ' allow to only open text files
        If saveFileDialog.ShowDialog() = DialogResult.OK Then   ' check if the user clicked OK in the save file dialog
            Try
                filePath = saveFileDialog.FileName  ' store the file path
                SaveFile(filePath)  ' call the SaveFile function
            Catch ex As Exception   ' catch an exception
                MessageBox.Show(ex.ToString())
            End Try
        End If
        RenewTitle()    ' change the title bar
    End Sub

    ''' <summary>
    ''' mnuNew_Click - opens a new file
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub mnuNew_Click(sender As Object, e As EventArgs) Handles mnuNew.Click
        ContentChanged()    ' check if the content of the file has been changed
        If Not clickedCancel Then   ' check if the user clcked the cancel button
            txtTextEditor.Text = String.Empty   ' clear the txtTextEditor text box
            fileContent = txtTextEditor.Text    ' renew the file content
            filePath = NEW_FILE     ' renew the file path
            RenewTitle()    ' change the title bar
        End If
    End Sub

    ''' <summary>
    ''' mnuCut_Click - removes the selected text
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub mnuCut_Click(sender As Object, e As EventArgs) Handles mnuCut.Click
        My.Computer.Clipboard.SetText(txtTextEditor.SelectedText)   ' store the selected text
        txtTextEditor.SelectedText() = String.Empty     ' remove the selected text
    End Sub

    ''' <summary>
    ''' mnuCopy_Click - copies the selected text
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub mnuCopy_Click(sender As Object, e As EventArgs) Handles mnuCopy.Click
        My.Computer.Clipboard.SetText(txtTextEditor.SelectedText)   ' store the selected text
    End Sub

    ''' <summary>
    ''' mnuPaste_Click - inserts the text into the spot selected by the user
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub mnuPaste_Click(sender As Object, e As EventArgs) Handles mnuPaste.Click
        txtTextEditor.SelectedText() = My.Computer.Clipboard.GetText()  ' insert the text into the spot selected by the user
    End Sub

    ''' <summary>
    ''' mnuExit_click - exits the application
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub mnuExit_click(sender As Object, e As EventArgs) Handles mnuExit.Click
        Application.Exit()
    End Sub

    ''' <summary>
    ''' frmTextEditor_FormClosing - checks if the text file was saved before closing the form
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub frmTextEditor_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        e.Cancel = True     ' cancel the closing event
        ContentChanged()    ' check if the content of the file has been changed
        If Not clickedCancel Then   ' check if the user clcked the cancel button
            e.Cancel = False    ' enable the closing event
        End If
    End Sub

    ''' <summary>
    ''' mnuAbout_Click - displays the information about the application
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub mnuAbout_Click(sender As Object, e As EventArgs) Handles mnuAbout.Click
        MsgBox("NETD2202 - Net Development I" & vbCrLf & vbCrLf & "Lab 5: Text Editor" & vbCrLf & vbCrLf & "Anastasiia Kononirenko" & vbCrLf & vbCrLf & "Created on March 23d 2019", MsgBoxStyle.OkOnly, "About")
    End Sub

    ''' <summary>
    ''' frmTextEditor_Load - set the title bar of the form
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub frmTextEditor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RenewTitle()    ' change the title bar
    End Sub

#End Region

#Region "Subs"
    ''' <summary>
    ''' RenewTitle - change the title bar of the form
    ''' </summary>
    Private Sub RenewTitle()
        Me.Text = Path.GetFileName(filePath) & " - Text Editor"
    End Sub

    ''' <summary>
    ''' SaveFile - saves the file content
    ''' </summary>
    ''' <param name="path"></param>
    Private Sub SaveFile(path As String)
        Dim fileStream As New FileStream(path, FileMode.Create, FileAccess.Write)   ' declare a File Strean object
        Dim writeStream As New StreamWriter(fileStream) ' declare a StreamWriter object

        writeStream.Write(txtTextEditor.Text)   ' rewrite the content of the file
        writeStream.Close() ' close the WriteStream

        saveFileDialog.FileName = String.Empty  ' clear the file name of the save file dialog
        fileContent = txtTextEditor.Text    ' renew the file content
    End Sub

    ''' <summary>
    ''' ContentChanged - checks if the content of the file was changed
    ''' </summary>
    Private Sub ContentChanged()
        clickedCancel = False   ' set the value of the clickedCancel variable to False
        If txtTextEditor.Text <> fileContent Then   ' check if the content of the file was changed
            Dim msgBoxResult = MsgBox("Do you want to save changes to " & filePath.ToString() & "?", MsgBoxStyle.YesNoCancel, "Text Editor")    ' ask the user if they want to save changes
            If msgBoxResult = DialogResult.Yes Then ' check if the user clicked the Yes button
                mnuSave.PerformClick()  ' save the changes
            ElseIf msgBoxResult = DialogResult.Cancel Then  ' check if the user clicked the Cancel button
                clickedCancel = True    ' set the value of the clickedCancel variable to True
            End If
        End If
    End Sub
#End Region

End Class

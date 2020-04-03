' Name:             Scott Alton
' File:             TextEditor.vb
' Last Modified:    April 3, 2020
' Course:           .NET Development I
' Description:      This application acts as a basic text editor, comparable to Notepad and allows users to open, create, edit, and save
'                   text files as they desire. It includes copy, paste, and cut clipboard features that users are accustomed to in most other
'                   text editors for ease-of-use, as well as several keyboard shortcuts that make it useful for making notes and reading text files.                

Option Strict On

Imports System.IO

Public Class frmTextEditor

    ' VARIABLE DECLARATIONS

    Dim isFileOpen As Boolean = False
    Dim openFilePath As String = String.Empty
    Dim selectedText As String

    'EVENT HANDLERS

    ''' <summary>
    ''' Event Handler for New Button - this handles events when the user clicks on the New button, within the file menu
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub mnuFileNew_Click(sender As Object, e As EventArgs) Handles mnuFileNew.Click

        txtTextEditorInput.Clear()
        isFileOpen = False
        openFilePath = String.Empty
        UpdateFormTitle()

    End Sub

    ''' <summary>
    ''' Event Handler for Exit Button - this handles events when the user clicks on the Exit button, within the file menu
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub mnuFileExit_Click(sender As Object, e As EventArgs) Handles mnuFileExit.Click

        Me.Close()

    End Sub

    ''' <summary>
    ''' Event Handler for About Button - provides basic information about the program
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub mnuHelpAbout_Click(sender As Object, e As EventArgs) Handles mnuHelpAbout.Click

        ' Output basic program information in a messagebox
        MessageBox.Show("Text Editor" & vbCrLf & vbCrLf & "By Scott Alton" & vbCrLf & vbCrLf & "April 1 2020")

    End Sub

    ''' <summary>
    ''' Event Handler for Open Button - opens an existing file by searching through system directories, and once selected,
    ''' providing output within main textbox for viewing and editing.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub mnuFileOpen_Click(sender As Object, e As EventArgs) Handles mnuFileOpen.Click

        ' Declare objects required to open and read files using local directories 
        Dim openFile As FileStream
        Dim fileReader As StreamReader

        ' If the file is readable, open it 
        If opdOpen.ShowDialog() = DialogResult.OK Then

            openFilePath = opdOpen.FileName
            isFileOpen = True

            ' Constructor for new file objects based on a user's selection from the filestream, which once selected will be read by the stream-reader
            openFile = New FileStream(openFilePath, FileMode.Open, FileAccess.Read)
            fileReader = New StreamReader(openFile)

            ' Write  complete text from the file selected to open to the text editor textbox
            txtTextEditorInput.Text = fileReader.ReadToEnd()

            ' Close the file-reader once it is done writing all text from the file
            fileReader.Close()

            ' Update form title by appending the file path name beside the application name
            UpdateFormTitle()

        End If

    End Sub

    ''' <summary>
    ''' Event Handler for Save Button - this handles events when the user clicks on the Save button, within the file menu
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub mnuFileSave_Click(sender As Object, e As EventArgs) Handles mnuFileSave.Click

        ' Check if the file does not yet have a saved file path 
        If openFilePath = String.Empty Then

            ' If no filename has been set, forward this action to trigger the saveAs button functionality where a filename will be set
            mnuFileSaveAs_Click(sender, e)

        Else

            ' Call the SaveTextFile method which writes all modifications in the event that the open file already has a saved name and path
            SaveTextFile(openFilePath)

        End If

    End Sub

    ''' <summary>
    ''' Event Handler for SaveAs Button - this handles events when the user clicks on the SaveAs button, within the file menu
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub mnuFileSaveAs_Click(sender As Object, e As EventArgs) Handles mnuFileSaveAs.Click

        ' Declare the SaveFileDialog object
        Dim sfdSaveAs As New SaveFileDialog

        ' Sets filters for the save dialog
        sfdSaveAs.Filter = "Text files (*.txt)|All files (*.*)|*.*|"

        ' If the user selects a file name to save that is acceptable, save the file with the assigned name 
        If sfdSaveAs.ShowDialog() = DialogResult.OK Then

            ' Set the new filePath and update the form title by calling the UpdateFormTitle method
            openFilePath = sfdSaveAs.FileName
            UpdateFormTitle()

            ' Save the file to the system using the SaveTextFile method
            SaveTextFile(openFilePath)

        End If

    End Sub

    ''' <summary>
    ''' Event Handler for Close Button - this handlers events when the user clicks on the Close button and prompts the user to save their work if text has been added to an unsaved document
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub mnuFileClose_Click(sender As Object, e As EventArgs) Handles mnuFileClose.Click

        ' Close the current file contents from the textbox and start new blank text file
        mnuFileNew_Click(sender, e)

    End Sub

    ''' <summary>
    ''' Event Handler for Copy Button - this handles events when the user clicks on the Copy button, of triggers the button using a shortcut, which then will write the selected
    ''' text to the clipboard, as highlighted by the user
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub mnuEditCopy_Click(sender As Object, e As EventArgs) Handles mnuEditCopy.Click

        ' Gather text that is selected by the user from the editor textbook 
        selectedText = txtTextEditorInput.SelectedText.ToString

        ' Call a method that will set this selected text to the user's clipboard for future use
        CopyText(selectedText)

    End Sub

    ''' <summary>
    ''' Event Handler for Cut Button - this handles events when the user clicks on the Cut button, and removes the selected text from the textbox after writing it to the clipboard
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub mnuEditCut_Click(sender As Object, e As EventArgs) Handles mnuEditCut.Click

        ' Gather text that is selected by the user from the editor textbook 
        selectedText = txtTextEditorInput.SelectedText.ToString

        ' Call the CutText method to remove text and set it to the clipboard
        CutText(selectedText)

    End Sub

    ''' <summary>
    ''' Event Handler for Paste Button - this handles events when the user clicks on the Paste button, or triggers the button using a shortcut, which then will write the text copied to the clipboard 
    ''' to wherever the user has selected with the editor textbox
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub mnuEditPaste_Click(sender As Object, e As EventArgs) Handles mnuEditPaste.Click

        PasteText()

    End Sub

    ' METHODS

    ''' <summary>
    ''' SaveTextFile Method - writes all changes made to the editor textbox to the system using the streamwriter 
    ''' </summary>
    ''' <param name="path"></param>
    Friend Sub SaveTextFile(path As String)

        ' Variable declarations - declare filestream and streamwriter objects
        Dim fileToAccess As New FileStream(openFilePath, FileMode.Create, FileAccess.Write)
        Dim writer As New StreamWriter(fileToAccess)

        ' Writer text entered into editor textbox to the file using the filewriter 
        writer.Write(txtTextEditorInput.Text)

        ' Close the filewriter
        writer.Close()

    End Sub


    ''' <summary>
    ''' UpdateFormTitle Method - Alters the title of the form that displays at the top of the application based on whether the file has been saved to the system. 
    ''' If so, the name will change dynamically based on the file title, and if the file has not been saved, then the application title only will appear.
    ''' </summary>
    Friend Sub UpdateFormTitle()

        ' Default title text, the title of the application
        Me.Text = "Text Editor"

        ' If there is anything set as the name for the open file path, append it to the title string after the application's name 
        If Not openFilePath = String.Empty Then

            Me.Text &= " | " & openFilePath

        End If

    End Sub

    ''' <summary>
    ''' CopyText Method - takes in text selected by the user and sets it to the clipboard for future use
    ''' </summary>
    Friend Sub CopyText(selectedText As String)

        ' Verify that at least one character of text has been selected to be copied to the clipboard
        If Not selectedText.Length = 0 Then

            ' If something has been selected, copy it to the clipboard
            My.Computer.Clipboard.SetText(selectedText)

        End If

    End Sub

    ''' <summary>
    ''' CutText Method - removes the text selected by the user from the editor textbox and sets it to the clipboard for future use
    ''' </summary>
    Friend Sub CutText(selectedText As String)

        ' Verify that at least one character of text has been selected to be cut 
        If Not selectedText.Length = 0 Then

            ' If something has been selected, copy it to the clipboard
            My.Computer.Clipboard.SetText(Convert.ToString(selectedText))

            ' Remove the selected text from the textbox
            txtTextEditorInput.SelectedText = ""

        End If

    End Sub

    ''' <summary>
    ''' PasteText Method - if text has been set to the clipboard, paste it into the textbox wherever the user has selected
    ''' </summary>
    Friend Sub PasteText()

        ' If there is text on the clipboard, write it into textbox
        If Clipboard.ContainsText Then

            txtTextEditorInput.Text = txtTextEditorInput.Text.Insert(txtTextEditorInput.SelectionStart, Clipboard.GetText)

        End If

    End Sub

End Class

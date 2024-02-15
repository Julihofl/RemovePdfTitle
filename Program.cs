using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using Microsoft.WindowsAPICodePack.Dialogs;

class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        CommonOpenFileDialog commonOpenFileDialog = new CommonOpenFileDialog();
        commonOpenFileDialog.Title = "Wählen Sie einen Ordner mit Ihren PDF-Dateien aus";
        commonOpenFileDialog.IsFolderPicker = true;
        if (commonOpenFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
        {
            foreach (string filePath in commonOpenFileDialog.FileNames)
            {
                if (Directory.Exists(filePath))
                {
                    ProcessFolder(filePath);
                }
                else if (File.Exists(filePath) && filePath.ToLower().EndsWith(".pdf"))
                {
                    ProcessFile(filePath);
                }
            }

            MessageBox.Show($"Titel aller PDF-Dateien im Ordner \"{commonOpenFileDialog.FileName}\" erfolgreich entfernt.", "Erfolgreich", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    static void ProcessFolder(string folderPath)
    {
        string[] pdfFiles = Directory.GetFiles(folderPath, "*.pdf", SearchOption.AllDirectories);

        foreach (string pdfFile in pdfFiles)
        {
            ProcessFile(pdfFile);
        }
    }

    static void ProcessFile(string filePath)
    {
        try
        {
            PdfDocument document = PdfReader.Open(filePath);
            document.Info.Title = "";
            document.Save(filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fehler beim Bearbeiten des PDF-Dokuments ({Path.GetFileName(filePath)}): {ex.Message}");
        }
    }
}

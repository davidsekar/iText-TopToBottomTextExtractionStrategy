using ITextReadPdf.Model;

namespace ITextReadPdf.Service.DirectoryService;

public interface IDirectoryService
{
    string WorkingDirectory { get; }

    string Filename { get; }

    XpdfParameter GetParameter(string pdfFilePath);

    string GetArguments(XpdfParameter parameter);
}
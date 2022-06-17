using ITextReadPdf.Model;

namespace ITextReadPdf.Service.DirectoryService;

public class DirectoryServiceWindows : DirectoryServiceBase
{
    private const string PDFToText = "pdftotext.exe";

    public override string Filename
    {
        get
        {
            string filename = Path.Combine(this.WorkingDirectory, PDFToText);
            return filename;
        }
    }

    public override string GetArguments(XpdfParameter parameter)
    {
        var arguments = this.JoinXpdfParameters(parameter);

        return arguments;
    }
}
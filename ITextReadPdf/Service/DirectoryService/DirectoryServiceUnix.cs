using ITextReadPdf.Model;

namespace ITextReadPdf.Service.DirectoryService;

public class DirectoryServiceUnix : DirectoryServiceBase
{
    private const string PDFToText = "pdftotext";
    private const string Bash = "/bin/bash";

    public override string Filename => Bash;

    public override string GetArguments(XpdfParameter parameter)
    {
        string arguments = this.JoinXpdfParameters(parameter);

        string newArguments = $"-c \"chmod +x ./{PDFToText}; ./{PDFToText} {arguments}\"";

        return newArguments;
    }
}
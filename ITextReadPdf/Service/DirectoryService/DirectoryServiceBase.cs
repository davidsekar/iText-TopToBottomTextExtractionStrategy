using ITextReadPdf.Model;

namespace ITextReadPdf.Service.DirectoryService;

public abstract class DirectoryServiceBase : IDirectoryService
{
    public abstract string Filename { get; }

    public string WorkingDirectory
    {
        get
        {
            string workingDirectory;

#if NET45
                workingDirectory = AppDomain.CurrentDomain.BaseDirectory;
#else
            workingDirectory = AppContext.BaseDirectory;
#endif

            return workingDirectory;
        }
    }

    public XpdfParameter GetParameter(string pdfFilePath)
    {
        return new XpdfParameter
        {
            Encoding = "-enc UTF-8",
            PdfFilename = pdfFilePath,
            OutputFilename = Path.Combine(this.WorkingDirectory, Guid.NewGuid() + ".txt")
        };
    }

    public abstract string GetArguments(XpdfParameter parameter);

    protected static string WrapWith(string text, string ends)
    {
        return ends + text + ends;
    }

    protected static string WrapQuotes(string text)
    {
        return WrapWith(text, "\"");
    }

    protected string JoinXpdfParameters(XpdfParameter parameter)
    {
        string[] argumentsArray =
        {
            parameter.Encoding,
            WrapQuotes(parameter.PdfFilename),
            WrapQuotes(parameter.OutputFilename),
        };

        string arguments = string.Join(" ", argumentsArray);
        return arguments;
    }
}
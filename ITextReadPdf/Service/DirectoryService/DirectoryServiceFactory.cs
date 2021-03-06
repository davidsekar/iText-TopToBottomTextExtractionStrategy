using ITextReadPdf.Service.RuntimeInformation;

namespace ITextReadPdf.Service.DirectoryService;

public static class DirectoryServiceFactory
{
    public static IDirectoryService GetDirectoryService(IRuntimeInformation runtimeInformation)
    {
        IDirectoryService result = null;

        switch (runtimeInformation.GetOSPlatform())
        {
            case OS.Windows:
                result = new DirectoryServiceWindows();
                break;

            case OS.Linux:
                result = new DirectoryServiceLinux();
                break;

            case OS.OSX:
                result = new DirectoryServiceOSX();
                break;

            default:
                throw new ArgumentException("XpdfNet currently only supports Linux, Windows and OSX.");
        }

        return result;
    }
}
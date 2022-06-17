using System.Runtime.InteropServices;

namespace ITextReadPdf.Service.RuntimeInformation;

public class MyRuntimeInformation : IRuntimeInformation
{
    public OS GetOSPlatform()
    {
        OS os;

#if NET45
            os = OS.Windows;
#else
        var isLinux = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        var isWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        os = this.GetOSPlatform(isLinux, isWindows);
#endif

        return os;
    }

    public OS GetOSPlatform(bool isLinux, bool isWindows)
    {
        OS os = OS.OSX;

        if (isLinux)
        {
            os = OS.Linux;
        }
        else if (isWindows)
        {
            os = OS.Windows;
        }

        return os;
    }
}
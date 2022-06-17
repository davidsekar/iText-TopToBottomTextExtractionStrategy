using System.ComponentModel;
using System.Diagnostics;

namespace ITextReadPdf.Service
{
    public class ProcessService
    {
        private readonly Process process;
        private readonly string filename;
        private readonly string arguments;
        private readonly string workingDirectory;

        public ProcessService(string filename, string arguments, string workingDirectory)
        {
            this.filename = filename;
            this.arguments = arguments;
            this.workingDirectory = workingDirectory;

            process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = filename,
                    Arguments = arguments,
                    UseShellExecute = false,
                    WorkingDirectory = workingDirectory,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
        }

        public void StartAndWaitForExit()
        {
            try
            {
                process.Start();
                process.WaitForExit();
            }
            catch (Win32Exception ex)
            {
                throw new Win32Exception(
                    $"Message: {ex.Message}. InnerException: {ex.InnerException}. Filename: {filename}; Arguments: {arguments}; WorkingDirectory: {workingDirectory};");
            }
        }
    }
}

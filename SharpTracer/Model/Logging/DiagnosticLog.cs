using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;

namespace SharpTracer.Base
{
	public enum Level
	{
		ERROR,
		WARN,
		INFO,
		DEFAULT
	}

	public enum LogDestination
	{
		Console = 1,
		Log = 2,
		Exception = 4,
	}


	public class DiagnosticLog
    {
        private bool _initialised;
        private bool m_initialised = false;
        private static FileStream reportStream;
        private static StreamWriter logWriter;

        public Uri FilePath { get; set; }

        public DiagnosticLog()
        {
            _initialised = false;
        }

        public virtual void Initialise(Uri filePath)
        {
            FilePath = filePath;
            DateTime timestamp = DateTime.Now;
            string timeword = timestamp.ToShortDateString() + "\t" + timestamp.ToLongTimeString();
            try
            {
                if (File.Exists(filePath.LocalPath))
                {
                    try
                    {
                        reportStream = new FileStream(filePath.LocalPath, FileMode.Append);
                    }
                    catch
                    {
                        Uri newpath;
                        Uri.TryCreate( Path.Combine(Path.GetDirectoryName(FilePath.LocalPath),
                            (Path.GetFileNameWithoutExtension(FilePath.LocalPath) + timestamp.ToString("yyddmmhhmmssff") + Path.GetExtension(FilePath.LocalPath))),
                            UriKind.RelativeOrAbsolute,
                            out newpath);
                        FilePath = newpath;

                        Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(FilePath.LocalPath)));
                        reportStream = new FileStream(FilePath.LocalPath, FileMode.Create);
                    }
                }
                else
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(FilePath.LocalPath));
                    reportStream = new FileStream(FilePath.LocalPath, FileMode.Create);
                }
                m_initialised = true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Opening a Log file failed: \n\n" + e.Message+"\n\n"+filePath.LocalPath, "Starting Log File");
            }

            if (!m_initialised) return;
            logWriter = new StreamWriter(reportStream);
            logWriter.Write("\n==============={0}===============\n", timeword);
            _initialised = true;
        }

        public void CloseLog()
        {
            if (!_initialised) throw new Exception("Diagnostic log not initialised");
            try
            {
                logWriter?.Write("====================================================\n");
                logWriter?.Flush();
                if (!m_initialised) return;
                logWriter?.Close();
                reportStream?.Close();
            }
            catch (System.ObjectDisposedException) { }
        }

        protected void AddEntry(Level level, string message,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string caller = "",
            [CallerFilePath] string callingFilePath = "")
        {
            if (!_initialised) throw new Exception("Diagnostic log not initialised");
            if (!m_initialised) return;
            if (logWriter.BaseStream == null) return;
            switch (level)
            {
                case Level.INFO:
                case Level.WARN:
                    logWriter.Write("{0}::\t\t{1}\n", Enum.GetName(typeof(Level), level), message);
					Console.WriteLine( "{0}::\t\t{1}\n", Enum.GetName( typeof( Level ), level ), message );
					break;
                case Level.ERROR:
                    logWriter.Write("ERROR::\t in file {0} in function {1} at line {2}\n\t Message: {3}\n", callingFilePath, caller, lineNumber, message);
					Console.WriteLine( "ERROR::\t in file {0} in function {1} at line {2}\n\t Message: {3}\n", callingFilePath, caller, lineNumber, message );
					break;
                default: break;
            }

            logWriter.Flush();
        }
    }
}

using System;
using System.IO;
using System.Text;

namespace ClassLib.DataStructures
{
    /// <summary>
    /// Klasa pozwalająca na logowanie informacji systemowych do plików tekstowych
    /// </summary>
    public class Logger
    {
        #region Constant Fields
        private const string LOG_BASE_FILE_NAME = "Log";
        private const string LOG_BASE_LOCATION = @"C:\LogDir\";
        #endregion

        #region Private Fields
        private readonly string fileName;
        private object fileWritingLockObject = new object();
        #endregion

        #region Constructor
        public Logger()
        {
            fileName = CreateFileName();

            FileInfo fi = new FileInfo(fileName);
            if (fi.Directory != null && fi.DirectoryName != null && !fi.Directory.Exists)
                Directory.CreateDirectory(fi.DirectoryName);

            if (!File.Exists(fileName))
            {
                FileStream fs = new FileStream(fileName, FileMode.CreateNew);
            }
        }
        #endregion

        #region Public Methods
        public void Write(string message, LoggingCategory loggingCategory)
        {
            if (String.IsNullOrEmpty(message))
                return;

            message = String.Format("{0} {1}", CreateBaseMessage(loggingCategory), message);

            lock(fileWritingLockObject)
                File.AppendAllLines(fileName, new[] {message});
        }
        #endregion

        #region Private Methods
        private string CreateFileName()
        {
            StringBuilder fileNameStringBuilder = new StringBuilder();

            fileNameStringBuilder.Append(LOG_BASE_LOCATION);
            fileNameStringBuilder.Append("\\");
            fileNameStringBuilder.Append(LOG_BASE_FILE_NAME);
            fileNameStringBuilder.Append("[");
            fileNameStringBuilder.Append(String.Format("{0}-{1}-{2}-{3}-{4}", DateTime.Now.Year, DateTime.Now.Month,
                DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute));
            fileNameStringBuilder.Append("].txt");

            return fileNameStringBuilder.ToString();
        }

        private string CreateBaseMessage(LoggingCategory loggingCategory)
        {
            StringBuilder baseMesssageStringBuilder = new StringBuilder();

            baseMesssageStringBuilder.Append("[");
            baseMesssageStringBuilder.Append(DateTime.Now);
            baseMesssageStringBuilder.Append("]");
            baseMesssageStringBuilder.Append(" CATEGORY: ");

            switch (loggingCategory)
            {
                case LoggingCategory.Error:
                    baseMesssageStringBuilder.Append("ERROR. ");
                    break;
                case LoggingCategory.Information:
                    baseMesssageStringBuilder.Append("INFORMATION. ");
                    break;
                case LoggingCategory.Warning:
                    baseMesssageStringBuilder.Append("WARNING. ");
                    break;
            }

            baseMesssageStringBuilder.Append(" MESSAGE: ");

            return baseMesssageStringBuilder.ToString();
        }
        #endregion
    }

    /// <summary>
    /// Stała wyliczeniowa określająca rodzaj logowanej informacji
    /// </summary>
    public enum LoggingCategory
    {
        Information,
        Warning,
        Error
    }
}

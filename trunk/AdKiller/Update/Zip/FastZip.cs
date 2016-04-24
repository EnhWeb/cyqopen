using System;
using System.IO;
using ICSharpCode.SharpZipLib.Core;

namespace ICSharpCode.SharpZipLib.Zip
{
    public class FastZip
    {

        #region Constructors

        public FastZip()
        {
        }

        #endregion



        #region ExtractZip
        public void ExtractZip(string zipFileName, string targetDirectory)
        {
            Stream inputStream = File.Open(zipFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            ExtractZip(inputStream, targetDirectory);
        }
        private void ExtractZip(Stream inputStream, string targetDirectory)
        {
            continueRunning_ = true;
            extractNameTransform_ = new WindowsNameTransform(targetDirectory);
           // restoreDateTimeOnExtract_ = true;

            using (zipFile_ = new ZipFile(inputStream))
            {
                zipFile_.IsStreamOwner = true;
                System.Collections.IEnumerator enumerator = zipFile_.GetEnumerator();
                while (continueRunning_ && enumerator.MoveNext())
                {
                    ZipEntry entry = (ZipEntry)enumerator.Current;
                    ExtractEntry(entry);
                }
            }
        }
        #endregion

        #region Internal Processing


        void ExtractFileEntry(ZipEntry entry, string targetName)
        {
            if (continueRunning_)
            {
                try
                {
                    using (FileStream outputStream = File.Create(targetName))
                    {
                        if (buffer_ == null)
                        {
                            buffer_ = new byte[4096];
                        }
                        StreamUtils.Copy(zipFile_.GetInputStream(entry), outputStream, buffer_);
                    }
                }
                catch
                {
                    continueRunning_ = false;
                }
            }

        }

        void ExtractEntry(ZipEntry entry)
        {
            bool doExtraction = entry.IsCompressionMethodSupported();
            string targetName = entry.Name;

            if (doExtraction)
            {
                if (entry.IsFile)
                {
                    targetName = extractNameTransform_.TransformFile(targetName);
                }
                else if (entry.IsDirectory)
                {
                    targetName = extractNameTransform_.TransformDirectory(targetName);
                }

                doExtraction = !((targetName == null) || (targetName.Length == 0));
            }

            // TODO: Fire delegate/throw exception were compression method not supported, or name is invalid?

            string dirName = null;

            if (doExtraction)
            {
                if (entry.IsDirectory)
                {
                    dirName = targetName;
                }
                else
                {
                    dirName = Path.GetDirectoryName(Path.GetFullPath(targetName));
                }
            }

            if (doExtraction && !Directory.Exists(dirName))
            {
                if (!entry.IsDirectory)
                {
                    try
                    {
                        Directory.CreateDirectory(dirName);
                    }
                    catch
                    {
                        doExtraction = false;
                    }
                }
            }

            if (doExtraction && entry.IsFile)
            {
                ExtractFileEntry(entry, targetName);
            }
        }

        #endregion

        #region Instance Fields
        bool continueRunning_;
        byte[] buffer_;
        ZipFile zipFile_;
       // string sourceDirectory_;
       // bool restoreDateTimeOnExtract_;
       // bool createEmptyDirectories_;
       // IEntryFactory entryFactory_ = new ZipEntryFactory();
        INameTransform extractNameTransform_;
        #endregion
    }
}

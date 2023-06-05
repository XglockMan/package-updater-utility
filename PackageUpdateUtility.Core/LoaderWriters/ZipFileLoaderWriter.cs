using System;
using System.IO;
using System.IO.Compression;

using PackageUpdateUtility.Core;

namespace PackageUpdateUtility.Core.FileLoaders
{
    public class ZipFileLoaderWriter : ILoaderWriter
    {
        public static void ParseZipPath(string path, out string zipPath, out string zipEntryPath)
        {

            int zipIndex = path.LastIndexOf(".zip", StringComparison.Ordinal) + 4;

            zipPath = path.Substring(0, zipIndex);
            zipEntryPath = path.Substring(zipIndex + 1);
        }

        public void Load(FileEnvironment fileEnvironment, Action<Stream> action)
        {
            ParseZipPath(fileEnvironment.Path, out string zipPath, out string zipEntryPath);
            using (var _zipToOpen = new FileStream(zipPath, FileMode.Open))
            using (var _archive = new ZipArchive(_zipToOpen, ZipArchiveMode.Read))
            using (var _stream = _archive.GetEntry(zipEntryPath).Open())
            {
                action(_stream);
            }
        }

        public void Write(FileEnvironment fileEnvironment, Action<Stream> action)
        {
            ParseZipPath(fileEnvironment.Path, out string zipPath, out string _entryPath);
            using (var _zipFile = new FileStream(zipPath, FileMode.Open))
            using (var _archiveStream = new ZipArchive(_zipFile, ZipArchiveMode.Update))
            using (var _stream = _archiveStream.GetEntry(_entryPath).Open())
            {
                _stream.SetLength(0);
                action(_stream);
            }
        }
    }
}
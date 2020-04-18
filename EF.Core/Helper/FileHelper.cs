using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace EF.Core.Helper
{
    public static class FileHelper
    {
        public static bool Exist(string path)
        {
            return File.Exists(path);
        }

        public static void Delete(string path)
        {
            File.Delete(path);
        }


        public static void ExistDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public static string[] GetFiles(string path, string searchPattern)
        {
            return Directory.GetFiles(path, searchPattern);
        }


        public static string GetExtensionName(string path)
        {
            return Path.GetExtension(path);
        }

        public static string GetDirectoryName(string path)
        {
            return Path.GetDirectoryName(path);
        }


        public static char GetDirectorySeparatorChar()
        {
            return Path.DirectorySeparatorChar;
        }

        public static string GetFileNameWithoutExtension(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        public static string GetMD5HashOfFile(string file)
        {
            var md5 = new MD5CryptoServiceProvider();

            using (var stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read, 8192))
            {
                md5.ComputeHash(stream);
            }

            byte[] hash = md5.Hash;

            return Convert.ToBase64String(hash);
        }

    }
}

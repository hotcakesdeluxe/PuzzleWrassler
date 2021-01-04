using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace PHL.Common.Utility
{

    public static class FileUtility
    {

        public static void ReplaceFileContents(string sourcePath, string destPath)
        {
            string source = $"{Application.dataPath}{sourcePath}";
            string dest = $"{Directory.GetParent(Application.dataPath).FullName}{destPath}";
            string text = File.ReadAllText(source);
            File.WriteAllText(dest, text);
        }

    }
}

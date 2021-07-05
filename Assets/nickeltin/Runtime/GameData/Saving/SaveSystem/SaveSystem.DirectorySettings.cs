using System;


namespace nickeltin.Runtime.GameData.Saving
{
    public sealed partial class SaveSystem
    {
        [Serializable]
        public class DirectorySettings
        {
            public Path path;
            public string extension = ".save";

            public string GetSubFolderPath(string subfolderName) => path + FormatFolderName(subfolderName);

            public string GetFilePath(string subFolderName, string fileName) => GetSubFolderPath(subFolderName) + fileName + extension;

            private string FormatFolderName(string folderName)
            {
                if (folderName.EndsWith("/")) return folderName;
                return folderName + '/';
            }
        }
    }
}
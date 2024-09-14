using System;
using System.Globalization;
using System.IO;
using UnityEngine;

namespace CORE.Scripts
{
    public class DataTimeHelpers : MonoBehaviour
    {
        // Method to parse a datetime string from a filename based on a specified format
        public static bool TryParseDateTimeFromFilename(string filename, string dateFormat, out DateTime result)
        {
            string[] parts = filename.Split('_');
            string datePart = parts.Length > 2 ? parts[2].Split('.')[0] : string.Empty;
            return DateTime.TryParseExact(datePart, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
        }

        // Method to find the file with the closest datetime to a given reference datetime
        public static string FindClosestFileByTimestamp(string directoryPath, string filenamePattern, DateTime referenceTime, string dateFormat)
        {
            var directoryInfo = new DirectoryInfo(directoryPath);
            var files = directoryInfo.GetFiles(filenamePattern);

            DateTime closestTime = DateTime.MaxValue;
            string closestFile = null;

            foreach (var file in files)
            {
                if (TryParseDateTimeFromFilename(file.Name, dateFormat, out DateTime fileTime))
                {
                    if (closestFile == null || Math.Abs((fileTime - referenceTime).Ticks) < Math.Abs((closestTime - referenceTime).Ticks))
                    {
                        closestTime = fileTime;
                        closestFile = file.FullName;
                    }
                }
            }

            return closestFile;
        }
    }
}

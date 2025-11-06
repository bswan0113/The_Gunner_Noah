using System;
using System.IO;
using UnityEngine;

namespace Utils
{
    public static class JsonSerializer
    {
        public static bool SaveToJson<T>(T data, string fileName)
        {
            string path = GetFilePath(fileName);
            string tempPath = path + ".tmp";
            string backupPath = path + ".backup";

            try
            {
                string directory = Path.GetDirectoryName(path);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                string json = JsonUtility.ToJson(data, true);
                File.WriteAllText(tempPath, json);

                if (File.Exists(path))
                {
                    File.Replace(tempPath, path, backupPath);
                }
                else
                {
                    File.Move(tempPath, path);
                }
                Debug.Log($"[JsonSerializer] Save Success!! Path: {path}");
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"[JsonSerializer] Failed to save data to {path}. Reason: {e}");

                if (File.Exists(tempPath))
                {
                    File.Delete(tempPath);
                }
                return false;
            }
        }

        public static T LoadFromJson<T>(string fileName) where T : new()
        {
            string path = GetFilePath(fileName);
            string backupPath = path + ".backup";

            if (File.Exists(path))
            {
                try
                {
                    string json = File.ReadAllText(path);
                    return JsonUtility.FromJson<T>(json);
                }
                catch (Exception e)
                {
                    Debug.LogError($"[JsonSerializer] : {e.Message}");
                }
            }
            if (File.Exists(backupPath))
            {
                try
                {
                    string json = File.ReadAllText(backupPath);
                    T data = JsonUtility.FromJson<T>(json);
                    SaveToJson(data, fileName);
                    return data;
                }
                catch (Exception e)
                {
                    Debug.LogError($"[JsonSerializer] : {e.Message}");
                }
            }
            return new T();
        }

        public static bool FileExists(string fileName)
        {
            return File.Exists(GetFilePath(fileName));
        }

        public static bool DeleteFile(string fileName)
        {
            try
            {
                string path = GetFilePath(fileName);
                if (File.Exists(path))
                {
                    File.Delete(path);
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return false;
            }
        }

        private static string GetFilePath(string fileName)
        {

            return Path.Combine(Application.persistentDataPath, fileName);
        }
    }
}
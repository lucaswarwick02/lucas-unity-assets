using Newtonsoft.Json;
using UnityEngine;
using System.IO;
using File = System.IO.File;

namespace Arcadian.System
{
    public abstract class AbstractSaveData<T> where T : AbstractSaveData<T>, new()
    {
        public static T Current { private set; get; }
        
        private static readonly JsonSerializerSettings Settings = new()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            TypeNameHandling = TypeNameHandling.Auto
        };
        
        private static T StringToSaveData (string saveDataString)
        {
            return JsonConvert.DeserializeObject<T>(saveDataString, Settings);
        }

        private static string SaveDataToString (T saveData)
        {
            return JsonConvert.SerializeObject(saveData, Formatting.Indented, Settings);
        }
        
        protected static string SaveDataPath()
        {
            return Path.Combine(Application.persistentDataPath, new T().FileName());
        }

        public static void NewSave()
        {
            Current = new T().DefaultSaveData();
            var saveDataString = SaveDataToString(Current);
            
            File.WriteAllText(SaveDataPath(), saveDataString);
        }

        public static void Save()
        {
            var saveDataString = SaveDataToString(Current);
            
            File.WriteAllText(SaveDataPath(), saveDataString);
        }

        public static void Load()
        {
            var saveDataString = File.ReadAllText(SaveDataPath());
            Current = StringToSaveData(saveDataString);

            Save();
        }

        public static bool Exists()
        {
            return File.Exists(SaveDataPath());
        }

        public static bool IsLoaded()
        {
            return Current != null;
        }

        protected abstract T DefaultSaveData();

        protected abstract string FileName();
    }
}
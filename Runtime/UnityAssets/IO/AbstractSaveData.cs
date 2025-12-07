using Newtonsoft.Json;
using UnityEngine;
using System.IO;
using File = System.IO.File;

namespace LucasWarwick02.UnityAssets
{
    /// <summary>
    /// An generic abstract base class that provides a JSON-based save system for persistent game data. It handles file reading, writing, and serialization through <c>Newtonsoft.Json</c>, while maintaining a static current instance for active save data. Derived classes define default values and filenames, making it easy to implement consistent and type-safe systems across multiple data types.
    /// 
    /// I'm not that happy with how this class is laid out, as it's evolved over time. Suggestions welcome!
    /// </summary>
    /// <typeparam name="T">Same type as the class to allow for all variables to be saved.</typeparam>
    public abstract class AbstractSaveData<T> where T : AbstractSaveData<T>, new()
    {
        /// <summary>
        /// The save data that is currently loaded.
        /// </summary>
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

        /// <summary>
        /// Create a new object, and save it.
        /// </summary>
        public static void NewSave()
        {
            Current = new T().DefaultSaveData();
            var saveDataString = SaveDataToString(Current);

            File.WriteAllText(SaveDataPath(), saveDataString);
        }

        /// <summary>
        /// Save the current save data.
        /// </summary>
        public static void Save()
        {
            var saveDataString = SaveDataToString(Current);

            File.WriteAllText(SaveDataPath(), saveDataString);
        }

        /// <summary>
        /// Load the save data.
        /// </summary>
        public static void Load()
        {
            var saveDataString = File.ReadAllText(SaveDataPath());
            Current = StringToSaveData(saveDataString);

            Save();
        }

        /// <summary>
        /// Does the save data already exist?
        /// </summary>
        /// <returns></returns>
        public static bool Exists()
        {
            return File.Exists(SaveDataPath());
        }

        /// <summary>
        /// Is the save data currently loaded?
        /// </summary>
        /// <returns></returns>
        public static bool IsLoaded()
        {
            return Current != null;
        }

        /// <summary>
        /// Default values for the save data
        /// </summary>
        /// <returns>Save data filled with default values.</returns>
        protected abstract T DefaultSaveData();

        /// <summary>
        /// Default name for the file.
        /// </summary>
        /// <returns>Name + extension of the file</returns>
        protected abstract string FileName();
    }
}
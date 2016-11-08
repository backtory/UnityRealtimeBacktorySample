using System;
using UnityEngine;

namespace Assets.Backtory.core{

    public interface IStorage
    {
        void Put(string key, string data);

        string Get(string key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns>true if something actually removed</returns>
        void Remove(string key);

        void Clear();
    }

    public class PlayerPrefsStorage : IStorage
    {
        public void Clear()
        {
            PlayerPrefs.DeleteAll();
        }

        public string Get(string key)
        {
            return PlayerPrefs.GetString(key);
        }

        public void Put(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        public void Remove(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }
    }
}
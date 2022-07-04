using System;
using UnityEngine;

namespace Framework
{
    public class ConfigManager : MonoBehaviour
    {
        [SerializeField]
        private TextAsset configFile;

        [Serializable]
        public struct GridConfig
        {
            public int width;
            public int height;
        }

        [Serializable]
        public struct ConfigContent
        {
            public GridConfig grid;
        }

        private ConfigContent _config;

        private void Awake()
        {
            LoadConfigFromFile();
        }

        private void LoadConfigFromFile()
        {
            _config = JsonUtility.FromJson<ConfigContent>(configFile.text);
        }

        public ConfigContent GetConfig()
        {
            return _config;
        }
    }
}
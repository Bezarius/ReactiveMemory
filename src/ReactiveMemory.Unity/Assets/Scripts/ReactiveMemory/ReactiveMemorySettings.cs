using System;
using UnityEditor;
using UnityEngine;

namespace ReactiveMemory
{
    [CreateAssetMenu(fileName = "ReactiveMemorySettings", menuName = "Settings/Reactive Memory Settings")]
    public class ReactiveMemorySettings : ScriptableObject
    {
        public string namespaceName = "ReactiveMemory";
        public DefaultAsset inputDirectory; // = "Assets/Generated";
        public DefaultAsset outputDirectory; // = "Assets/Generated";

        public ReactiveMemoryAssetGenerationSettings[] reactiveMemoryDirs;

        private static ReactiveMemorySettings _instance;

        public static ReactiveMemorySettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<ReactiveMemorySettings>("ReactiveMemorySettings");
                    if (_instance == null)
                    {
                        Debug.LogError("ReactiveMemorySettings asset not found. Make sure to create a ReactiveMemorySettings asset.");
                    }
                }
                return _instance;
            }
        }
    }

    [Serializable]
    public class ReactiveMemoryAssetGenerationSettings
    {
        public DefaultAsset Folder;
        public string Namespace;
    }
}
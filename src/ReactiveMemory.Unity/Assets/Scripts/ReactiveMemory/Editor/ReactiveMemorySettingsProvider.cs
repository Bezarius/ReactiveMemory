using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ReactiveMemory.Editor
{
    public class ReactiveMemorySettingsProvider : SettingsProvider
    {
        private SerializedObject _serializedObject;

        private SerializedProperty _namespaceName;
        private SerializedProperty _reactiveMemoryDirs;

        private const string PreferencesPath = "Project/Reactive Memory Settings";

        [SettingsProvider]
        public static SettingsProvider CreatePreferencesProvider()
        {
            var provider = new ReactiveMemorySettingsProvider(PreferencesPath, SettingsScope.Project);
            return provider;
        }

        private ReactiveMemorySettingsProvider(string path, SettingsScope scope)
            : base(path, scope)
        {
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            var settings = ReactiveMemorySettings.Instance;
            if (settings == null)
            {
                Debug.LogError("ReactiveMemorySettings not found, create it.");
            }
            _serializedObject = new SerializedObject(settings);
            _namespaceName = _serializedObject.FindProperty(nameof(ReactiveMemorySettings.namespaceName));
            _reactiveMemoryDirs = _serializedObject.FindProperty(nameof(ReactiveMemorySettings.reactiveMemoryDirs));
        }

        public override void OnGUI(string searchContext)
        {
            EditorGUILayout.LabelField("Reactive Memory Settings", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(_namespaceName);
            EditorGUILayout.PropertyField(_reactiveMemoryDirs, true);

            if (EditorGUI.EndChangeCheck())
            {
                _serializedObject.ApplyModifiedProperties();
            }

            if (GUILayout.Button("Generate"))
            {
                var settings = ReactiveMemorySettings.Instance;
                for (var i = 0; i < settings.reactiveMemoryDirs.Length; i++)
                {
                    var dir = settings.reactiveMemoryDirs[i].Folder;
                    var @namespace = settings.reactiveMemoryDirs[i].Namespace;
                    var inputRelativePath = AssetDatabase.GetAssetPath(dir).Replace("Assets/", "./");
                    var outputRelativePath = inputRelativePath + "/Generated";
                    var result = CmdCommandExecutor.Execute("dotnet", $"rmgen -i {inputRelativePath} -o {outputRelativePath} -n {@namespace}");
                    Debug.Log("Generating MemoryDatabase.cs result: " + result);
                    Debug.Log("Generating MessagePack generated file...");
                    var formatterPath = Path.Combine(outputRelativePath, "Formatters");
                    var msgPackResult = CmdCommandExecutor.Execute("mpc", $"-i {inputRelativePath} -o {formatterPath} -n {@namespace}");
                    Debug.Log("Generating MessagePack generated file: " + msgPackResult);
                }
                AssetDatabase.Refresh();
            }
        }
    }
}
using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ReactiveMemory.Editor
{
    public class ReactiveMemorySettingsProvider : SettingsProvider
    {
        public const string PkgName = "ReactiveMemory.Generator";

        private SerializedObject _serializedObject;
        private SerializedProperty _reactiveMemoryDirs;
        private bool _generatorIsInstalled = false;


        private const string PreferencesPath = "Project/Reactive Memory";

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
            _reactiveMemoryDirs = _serializedObject.FindProperty(nameof(ReactiveMemorySettings.reactiveMemoryDirs));
            _generatorIsInstalled = NPMHelper.CheckPackageExists(PkgName);
        }


        public override void OnGUI(string searchContext)
        {
            EditorGUILayout.LabelField("Reactive Memory Settings", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(_reactiveMemoryDirs, true);
            if (EditorGUI.EndChangeCheck())
            {
                _serializedObject.ApplyModifiedProperties();
            }


            if (!_generatorIsInstalled)
            {
                if (GUILayout.Button("Install"))
                {
                    NPMHelper.InstallPackage(PkgName);
                    _generatorIsInstalled = NPMHelper.CheckPackageExists(PkgName);
                }
                    
            }
            else
            {
                if (GUILayout.Button("Generate"))
                {
                    ReactiveMemoryGenerator.Run();
                }
                if (GUILayout.Button("Remove"))
                {
                    NPMHelper.DeletePackage(PkgName);
                    _generatorIsInstalled = NPMHelper.CheckPackageExists(PkgName);
                }
            }
        }
    }

    public static class ReactiveMemoryGenerator
    {
        public static void Run()
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

    public static class NPMHelper
    {
        public static bool CheckPackageExists(string packageName)
        {
            string output = CmdCommandExecutor.Execute("dotnet", $"tool list --global");
            return output.Contains(packageName.ToLower());
        }

        public static string InstallPackage(string packageName, string version = null)
        {
            string args = $"tool install --global {packageName}";
            if (!string.IsNullOrEmpty(version))
            {
                args += $" --version {version}";
            }

            return CmdCommandExecutor.Execute("dotnet", args);
        }

        public static string DeletePackage(string packageName)
        {
            string args = $"tool uninstall --global {packageName}";
            return CmdCommandExecutor.Execute("dotnet", args);
        }
    }
}
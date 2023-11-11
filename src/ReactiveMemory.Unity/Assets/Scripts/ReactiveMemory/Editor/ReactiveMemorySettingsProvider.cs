using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;

namespace ReactiveMemory.Editor
{
    public class ReactiveMemorySettingsProvider : SettingsProvider
    {
        private const string PkgRmGenName = "ReactiveMemory.Generator";
        private const string PkgMpcName = "Messagepack.Generator";

        private SerializedObject _serializedObject;
        private SerializedProperty _reactiveMemoryDirs;
        private bool _rmGeneratorIsInstalled;
        private bool _mpcGeneratorIsInstalled;


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
            _rmGeneratorIsInstalled = NPMHelper.CheckPackageExists(PkgRmGenName);
            _mpcGeneratorIsInstalled = NPMHelper.CheckPackageExists(PkgMpcName);
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


            if (!_rmGeneratorIsInstalled)
            {
                if (GUILayout.Button("Install Reactive Memory"))
                {
                    NPMHelper.InstallPackage(PkgRmGenName);
                    _rmGeneratorIsInstalled = NPMHelper.CheckPackageExists(PkgRmGenName);
                }
            }
            else
            {
                if (GUILayout.Button("Remove Reactive Memory"))
                {
                    NPMHelper.DeletePackage(PkgRmGenName);
                    _rmGeneratorIsInstalled = NPMHelper.CheckPackageExists(PkgRmGenName);
                }
            }

            if (!_mpcGeneratorIsInstalled)
            {
                if (GUILayout.Button("Install MessagePack"))
                {
                    NPMHelper.InstallPackage(PkgMpcName);
                    _mpcGeneratorIsInstalled = NPMHelper.CheckPackageExists(PkgMpcName);
                }
            }
            else
            {
                if (GUILayout.Button("Remove MessagePack"))
                {
                    NPMHelper.DeletePackage(PkgMpcName);
                    _mpcGeneratorIsInstalled = NPMHelper.CheckPackageExists(PkgMpcName);
                }
            }

            if (_rmGeneratorIsInstalled && _mpcGeneratorIsInstalled && GUILayout.Button("Generate"))
            {
                Generate();
            }

            if (_rmGeneratorIsInstalled && _mpcGeneratorIsInstalled && GUILayout.Button("Regenerate"))
            {
                Generate(true);
            }

            return;

            void Generate(bool force = false)
            {
                EditorUtility.DisplayProgressBar("Generating", $"Try update {PkgRmGenName}", 0);
                NPMHelper.TryUpdate(PkgRmGenName);
                EditorUtility.DisplayProgressBar("Generating", $"Try update {PkgMpcName}", 15);
                NPMHelper.TryUpdate(PkgMpcName);
                ReactiveMemoryGenerator.Execute(force);
            }
        }
    }

    public static class ReactiveMemoryGenerator
    {
        public static void Execute(bool force = false)
        {
            var settings = ReactiveMemorySettings.Instance;

            for (var i = 0; i < settings.reactiveMemoryDirs.Length; i++)
            {
                var dir = settings.reactiveMemoryDirs[i].Folder;
                var @namespace = settings.reactiveMemoryDirs[i].Namespace;
                var workingDirectory = Application.dataPath;
                var input = (workingDirectory + AssetDatabase.GetAssetPath(dir).Replace("Assets/", "/"));
                var output = input + "/Generated";
                var formatterPath = output + "/Formatters";
                
                try
                {
                    ExecuteProcess("dotnet-rmgen", input, output, @namespace, force);
                }
                catch (Exception e)
                {
                    Debug.LogError("dotnet-rmgen error: " + e.Message);
                }

                
                try
                {
                    ExecuteProcess("mpc", input, formatterPath, @namespace, force);
                }
                catch (Exception e)
                {
                    Debug.LogError("mpc error: " + e.Message);
                }
            }
            
            return;

            void ExecuteProcess(string app, string input, string output, string ns, bool force = false)
            {
                if (force && Directory.Exists(output))
                {
                    Directory.Delete(output, true);
                }
                var cmd = $"-i  \"{input}\" -o \"{output}\" -n \"{ns}\"";
                
                ExecuteProcessAsync(app, cmd);
            }
        }
        
        public static Task<string> ExecuteProcessAsync(string app, string arguments)
        {
            Debug.Log("Execute: " + app + " " + arguments);
            var psi = new ProcessStartInfo
            {
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                FileName = app,
                Arguments = arguments,
                WorkingDirectory = Application.dataPath
            };

            Process p;
            try
            {
                p = Process.Start(psi);
            }
            catch (Exception ex)
            {
                return Task.FromException<string>(ex);
            }

            var tcs = new TaskCompletionSource<string>();
            p.EnableRaisingEvents = true;
            p.Exited += (object sender, System.EventArgs e) =>
            {
                var data = p.StandardOutput.ReadToEnd();
                p.Dispose();
                p = null;
                tcs.TrySetResult(data);
            };
            return tcs.Task;
        }
    }

    public static class NPMHelper
    {
        public static bool CheckPackageExists(string packageName)
        {
            var output = CmdCommandExecutor.Execute("dotnet", $"tool list --global");
            return output.Contains(packageName.ToLower());
        }

        public static string InstallPackage(string packageName, string version = null)
        {
            var args = $"tool install --global {packageName}";
            if (!string.IsNullOrEmpty(version))
            {
                args += $" --version {version}";
            }

            return CmdCommandExecutor.Execute("dotnet", args);
        }

        public static void TryUpdate(string packageName)
        {
            Process.Start("dotnet", $"tool update --global {packageName}");
        }

        public static string DeletePackage(string packageName)
        {
            var args = $"tool uninstall --global {packageName}";
            return CmdCommandExecutor.Execute("dotnet", args);
        }
    }
}
using System;
using System.Diagnostics;
using System.Text;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace ReactiveMemory.Editor
{
    public static class CmdCommandExecutor
    {
        public static string Execute(string fileName, string arguments)
        {
            var psi = new ProcessStartInfo
            {
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                FileName = fileName,
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
                return ex.ToString();
            }

            p.WaitForExit();

            var data = p.StandardOutput.ReadToEnd();
            var error = p.StandardError.ReadToEnd();

            if (p.ExitCode != 0)
                Debug.Log("CmdCommandExecutor error: " + error);

            return data;
        }
    }
}
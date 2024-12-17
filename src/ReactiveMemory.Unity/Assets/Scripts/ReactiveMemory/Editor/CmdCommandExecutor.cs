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
                Arguments = arguments
            };

            Process p;
            try
            {
                p = Process.Start(psi);
            }
            catch (Exception ex)
            {
                Debug.LogError("CmdCommandExecutor error: " + ex.ToString());
                return ex.ToString();
            }

            p.WaitForExit();

            if (p.ExitCode == 0) return p.StandardOutput.ReadToEnd();
            
            Debug.LogError("CmdCommandExecutor error: " + p.StandardError.ReadToEnd());
            return string.Empty;
        }
    }
}
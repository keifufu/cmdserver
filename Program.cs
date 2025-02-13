using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Diagnostics;
using System.IO;

public class CmdServer
{
  private const int Port = 6914;
  public static void Main()
  {
      TcpListener server = new TcpListener(IPAddress.Any, Port);
      server.Start();
      Console.WriteLine("Server started, listening for commands...");

      while (true)
      {
        TcpClient client = server.AcceptTcpClient();
        Console.WriteLine("Client connected.");

        using (NetworkStream stream = client.GetStream())
        using (StreamReader reader = new StreamReader(stream))
        using (StreamWriter writer = new StreamWriter(stream) { AutoFlush = true })
        {
          try
          {
            string command = reader.ReadLine();
            Console.WriteLine($"Received command: {command}");
            string result = RunCommand(command);
            writer.WriteLine(result);
          }
          catch (Exception ex)
          {
            writer.WriteLine($"Error: {ex.Message}");
          }
        }

        client.Close();
        Console.WriteLine("Client disconnected.");
    }
  }

  private static string RunCommand(string command)
  {
    try
    {
      ProcessStartInfo startInfo = new ProcessStartInfo
      {
        FileName = "/usr/bin/env",
        Arguments = $"bash -c \"{command}\"",
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
        CreateNoWindow = true
      };

      using (Process process = Process.Start(startInfo))
      {
        if (process == null) return "Failed to start process.";
        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();
        process.WaitForExit();
        return string.IsNullOrEmpty(error) ? output : $"{output}\nError: {error}";
      }
    }
    catch (Exception ex)
    {
      return $"Execution failed: {ex.Message}";
    }
  }
}

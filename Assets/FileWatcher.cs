using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class FileWatcher : MonoBehaviour
{
    public BasicRenderer Renderer;
    public string Directory;
    public string Filter;

    private Dictionary<string, StreamReader> _streamReaders;

    // Use this for initialization
    void Start()
    {
        _streamReaders = new Dictionary<string, StreamReader>();

        var watch = new FileSystemWatcher();
        watch.Path = Directory;
        watch.Filter = Filter;
        watch.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.Size;
        watch.Changed += new FileSystemEventHandler(OnFilesystemChange);
        watch.EnableRaisingEvents = true;
    }

    private void OnFilesystemChange(object sender, FileSystemEventArgs e)
    {
        if (!_streamReaders.ContainsKey(e.FullPath))
        {
            try
            {
                FileStream fs = new FileStream(e.FullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader sr = new StreamReader(fs);
                _streamReaders[e.FullPath] = sr;
                if (!sr.EndOfStream)
                {
                    sr.ReadToEnd();
                }
            }
            catch (Exception)
            {
                Debug.Log("Couldn't open filestream for " + e.FullPath);
                throw;
            }
        }
    }

    void OnDestroy()
    {
        foreach (StreamReader sr in _streamReaders.Values)
        {
            try
            {
                ((IDisposable) sr).Dispose();
            }
            catch
            {
                Debug.Log("Couldn't close filestream.");
            }
        }
    }

    private void handleLine(string line)
    {
        if (Renderer != null)
        {
            Renderer.handleEvent(line.ToLower().Contains("error") ? "error" : "info");
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (StreamReader sr in _streamReaders.Values)
        {
            while (!sr.EndOfStream)
            {
                handleLine(sr.ReadLine());
            }
        }
    }
}
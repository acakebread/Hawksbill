// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:03:59 by seancooper
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System;

namespace Hawksbill
{
    [CreateAssetMenu (menuName = "Hawksbill/File/Folder Sync")]
    public class FolderSync : ScriptableObject
    {
        public string sourcePath;
        public string destinationPath;
        public bool refresh;

        void OnValidate()
        {
            copy = new string[0];
            delete = new string[0];
            newer = new string[0];
            status = "";
        }

        public string status;
        public string[] copy;
        public string[] delete;
        public string[] newer;

        public void sync()
        {
            build ();
            delete.ToList ().ForEach (p => File.Delete (destinationPath + p));
            copy.ToList ().ForEach (p => copyAndCreate (p));
            removeEmptyFolders ();
            build ();
        }

        // public void combine()
        // {
        //     build ();
        //     delete.ToList ().ForEach (p => File.Delete (destinationPath + p));
        //     copy.ToList ().ForEach (p => copyAndCreate (p));
        //     removeEmptyFolders ();
        //     build ();
        // }

        public void build()
        {
            if (Directory.Exists (sourcePath) && Directory.Exists (destinationPath))
            {
                status = "Ready to sync!";
                string[] s = getDirectories (sourcePath).ToArray (), d = getDirectories (destinationPath).ToArray ();
                delete = d.Where (p => !s.Contains (p)).ToArray ();
                copy = s.Where (p => !d.Contains (p) || !isSame (p)).ToArray ();
                newer = d.Where (p => s.Contains (p) && isDestinationNewer (p)).ToArray ();
            }
            else
            {
                status = "Source / Destinations both need to exist!";
            }
        }

        bool isSourceNewer(string path) => File.GetLastWriteTime (sourcePath + path) > File.GetLastWriteTime (destinationPath + path);
        bool isDestinationNewer(string path) => File.GetLastWriteTime (destinationPath + path) > File.GetLastWriteTime (sourcePath + path);
        bool isDestinationOlder(string path) => File.GetLastWriteTime (destinationPath + path) < File.GetLastWriteTime (sourcePath + path);
        bool isSame(string path) => File.GetLastWriteTime (destinationPath + path) == File.GetLastWriteTime (sourcePath + path);

        IEnumerable<string> getDirectories(string path) =>
            Directory.EnumerateFiles (path, "*.*", SearchOption.AllDirectories).
                Select (p => p.Substring (path.Length)).
                Where (f => (new FileInfo (f).Attributes & FileAttributes.Hidden & FileAttributes.System) == 0);

        void copyAndCreate(string path)
        {
            (new FileInfo (destinationPath + path)).Directory.Create ();
            File.Copy (sourcePath + path, destinationPath + path, true);
        }

        void removeEmptyFolders()
        {
            var empty = Directory.GetDirectories (destinationPath, "*.*", SearchOption.AllDirectories).Where (p => Directory.GetDirectories (p).Length == 0 && Directory.GetFiles (p).Length == 0).ToList ();
            if (empty.Count == 0) return;
            empty.ForEach (d => Directory.Delete (d));
            removeEmptyFolders ();
        }
    }
}

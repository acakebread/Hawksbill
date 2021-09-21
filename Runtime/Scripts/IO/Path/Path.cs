// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created {date} by {username}
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Hawksbill.IO
{
    public class Path
    {
        string path;
        public Path(string path)
        {
            this.path = path;
            validate ();
        }

        void validate()
        {
            path = path.Replace ("\\", "/");
            if (path.StartsWith ("assets", true, null)) path = Application.dataPath + path.Substring ("assets".Length);
        }

        // representation
        public Path parentPath => String.Join ("/", path.Split ('/').TakeLessOne ());
        public Path folderPath => new Path ((isDirectory ? new DirectoryInfo (path) : new FileInfo (path).Directory).FullName);
        public string assetPath => "Assets" + path.Substring (Application.dataPath.Length);
        public string urlPath => throw new Exception ("Unsupported and unimplement - please implement!");

        // interface
        public bool isDirectory => File.GetAttributes (path).HasFlag (FileAttributes.Directory);
        public bool exists => File.Exists (path) || Directory.Exists (path);
        public string extension => exists ? System.IO.Path.GetExtension (path) : "";
        public TimeSpan createdAge => exists ? DateTime.Now - File.GetCreationTime (path) : new TimeSpan ();

        // Operator
        static string add(string lhs, string rhs) =>
            (lhs.Last () == '/' ? lhs.Substring (0, lhs.Length - 1) : lhs + "/") + (rhs.First () == '/' ? rhs.Substring (1) : rhs);
        public static Path operator +(Path lhs, Path rhs) { return new Path (add (lhs, rhs)); }
        public static Path operator +(Path lhs, string rhs) { return new Path (add (lhs, rhs)); }
        public static Path operator +(string lhs, Path rhs) { return new Path (add (lhs, rhs)); }

        // Conversion
        public static implicit operator string(Path p) => p.path;
        public static implicit operator Path(string s) => new Path (s);

        // Get folders
        public IEnumerable<Path> getFolders(bool recursive = true) =>
                Directory.GetDirectories (path, "*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).Select (p => new Path (p));
        public IEnumerable<Path> getEmptyFolders(bool recursive = true) =>
                getFolders (recursive).Where (p => Directory.GetFiles (p).Length == 0);

        public override string ToString() => path;

        // static 
        public void writeText(string text)
        {
            Directory.CreateDirectory (parentPath);
            File.WriteAllText (path, text);
#if UNITY_EDITOR
            AssetDatabase.Refresh ();
#endif
        }
        public bool readText(out string text)
        {
            bool exists = File.Exists (path);
            text = exists ? File.ReadAllText (path) : "";
            return exists;
        }

        // HELPERS
        /// <summary>Divides a URI into Path and Filename</summary>
        public static string[] GetPathAndFilename(string uri)
        {
            int index = uri.LastIndexOf ('/');
            return index == -1 ? new string[] { "", uri } : new string[] { uri.Substring (0, index), uri.Substring (index + 1) };
        }
    }
}

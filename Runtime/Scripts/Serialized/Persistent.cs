// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:05:45 by seancooper
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Hawksbill.Serialization
{
    [Serializable]
    public class Persistent
    {
        //public Format format;
        public string path = "test";
        protected Serializer serializer => new Serializer ();

        public void write()
        {
            var path = Application.persistentDataPath + "/" + this.path;
            var bytes = serializer.serialize (this);
            File.WriteAllBytes (path, bytes);
        }
        public void read()
        {
            var path = Application.persistentDataPath + "/" + this.path;
            var bytes = File.ReadAllBytes (path);
            serializer.deserializeOver (bytes, this);
        }
    }
}

// protected void toFBCloudStorage() { } // binary/json
// protected void toFBFirestore() { }  // database/json
// protected void toDrive() { } // binary/json 
// protected void toPersistentData() { } // same as toDrive
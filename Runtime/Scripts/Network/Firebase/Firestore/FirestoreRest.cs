// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:03:22 by seancooper
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Hawksbill.Network;
using UnityEngine.Networking;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Hawksbill.Firebase.Firestore
{
    public class FirestoreRest : IDisposable
    {
        public const string DefaultURL = "https://firestore.googleapis.com/v1/projects/{projectid}/databases/(default)/documents/";

        public readonly Location location;

        public FirestoreRest(Location location)
        {
            this.location = location;
        }

        public void Dispose() { }

        // url paths
        string getBaseURL() => DefaultURL.Replace ("{projectid}", location.project);
        string getPathURL(params string[] paths) => getBaseURL () + String.Join ("/", paths);
        string getQueryURL() => getBaseURL () + ":runQuery";

        async Task<string> request(UnityWebRequest request)
        {
            Location.Token token = await location.loginAsync ();
            request.SetRequestHeader ("Authorization", "Bearer " + token.idToken);
            await request.SendWebRequest ();
            return request.isSuccessful () ? request.downloadHandler.text : null;
        }

        /// <summary>Create a new document to the Firestore Collection</summary>
        public async Task<string> addDocumentToCollection(string collection, string data) =>
            await request (UnityWebRequest.Post (getPathURL (collection), data));

        /// <summary>Get a specific document from the Firestore</summary>
        public async Task<Dictionary<string, object>> getDocument(string documentPath)
        {
            var json = await request (UnityWebRequest.Get (getPathURL (documentPath)));
            JsonParser.FromJson (json, out Dictionary<string, object> result);
            return result;
        }

        // /// <summary>Get a specific document from the Firestore and override object</summary>
        // public async Task<T> getDocument<T>(string documentPath, T target)
        // {
        //     var json = await request (UnityWebRequest.Get (getPathURL (documentPath)));
        //     JsonParser.FromJson (json, target);
        //     return target;
        // }

        /// <summary>Get documents from the Firestore collection</summary>
        public async Task<Dictionary<string, object>[]> getDocuments(string collection)
        {
            var json = await request (UnityWebRequest.Get (getPathURL (collection)));
            return JsonParser.FromJson (json, out Dictionary<string, object>[] result) ? result : null;
        }

        /// <summary>Get documents from the Firestore collection via query</summary>
        public async Task<Dictionary<string, object>[]> getDocuments(string collection, string where = null, string select = null)
        {
            var json = await request (new UnityWebRequest (getQueryURL (), UnityWebRequest.kHttpVerbPOST)
            {
                uploadHandler = new UploadHandlerRaw (Encoding.UTF8.GetBytes (QStructureQuery (collection, where, select))) { contentType = "application/json" },
                downloadHandler = new DownloadHandlerBuffer (),
            });
            return JsonParser.FromJson (json, out Dictionary<string, object>[] result) ? result : null;
        }

        static string QStructureQuery(string collection, string where, string select)
        {
            const string format = "{\"structuredQuery\":{{query}}}";
            return format.Replace ("{query}", String.Join (",", new string[] { QCollection (collection), where, select }.Where (s => s != null)));
        }
        static string QCollection(string collection)
        {
            const string format = "\"from\":[{\"collectionId\":\"{collectionId}\"}]";
            return format.Replace ("{collectionId}", collection);
        }

        /// <summary>Create a Where query in Json format</summary>
        public static string QWhere(string field, object value, OP op = OP.EQUAL)
        {
            const string format = "\"where\":{\"fieldFilter\":{\"field\":{\"fieldPath\":\"{fieldPath}\"},\"op\":\"{op}\",\"value\":{\"{valuetype}\":\"{value}\"}}}";
            return format.Replace ("{fieldPath}", field).
                Replace ("{op}", Enum.GetName (typeof (OP), op)).
                Replace ("{valuetype}", TypeConversion.TypeToName (value.GetType ())).
                Replace ("{value}", value.ToString ());
        }

        /// <summary>Create a Select query in Json format</summary>
        public static string QSelect(string[] fields)
        {
            const string format = "\"select\":{\"fields\":[{fields}]}", subformat = "{\"fieldPath\":\"{value}\"}";
            return format.Replace ("{fields}", String.Join (",", fields.Select (p => subformat.Replace ("{value}", p))));
        }
    }

    static class UnityWebRequestEx
    {
        public static bool isSuccessful(this UnityWebRequest request, DownloadHandler download = null)
        {
            if (request.result == UnityWebRequest.Result.Success) return true;
            Debug.LogWarning ("WebRequest failed: '" + request.error + "'" + (download == null ? "" : " " + download.text));
            return false;
        }

        public static TaskAwaiter GetAwaiter(this AsyncOperation asyncOp)
        {
            var tcs = new TaskCompletionSource<object> ();
            asyncOp.completed += obj => { tcs.SetResult (null); };
            return ((Task) tcs.Task).GetAwaiter ();
        }
    }
}

//AddParameter (getBaseURL () + ":runQuery", "key", location.key);
// string AddParameter(string url, string field, string value) => (url.Contains ("?") ? url + "&" : url += "?") + field + "=" + value;
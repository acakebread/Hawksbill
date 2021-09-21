// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:03:22 by seancooper
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Hawksbill.Firebase.Firestore
{
    [Serializable, CreateAssetMenu (menuName = "Hawksbill/Firestore/Location")]
    public class Location : ScriptableObject
    {
        public bool invokeLogin;
        public string project, key, username, password;
        public Token token = new Token ();
        [TextArea (10, 100)] public string output;

        void OnValidate()
        {
            if (invokeLogin)
            {
                invokeLogin = false;
                login ();
            }
        }

        string getURL()
        {
            const string format = "https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={key}";
            return format.Replace ("{key}", key);
        }
        string getPayload()
        {
            const string format = "{\"email\":\"{email}\",\"password\":\"{password}\",\"returnSecureToken\":true}";
            return format.Replace ("{email}", username).Replace ("{password}", password);
        }

        public Task<Token> login() => loginAsync ();
        public async Task<Token> loginAsync()
        {
            if (token.expired)
            {
                token.clear ();
                var request = new UnityWebRequest (getURL (), UnityWebRequest.kHttpVerbPOST)
                {
                    uploadHandler = new UploadHandlerRaw (Encoding.UTF8.GetBytes (getPayload ())) { contentType = "application/json" },
                    downloadHandler = new DownloadHandlerBuffer (),
                };
                request.uploadHandler.contentType = "application/json";
                await request.SendWebRequest ();
                output = request.downloadHandler.text;
                if (request.isSuccessful (request.downloadHandler)) token.fromJson (request.downloadHandler.text);
            }
            return token;
        }

        [Serializable]
        public class Token
        {
            public string kind, localId, email, displayName, idToken, registered, refreshToken;
            public long expiresIn, timeExpires;

            public Token() => clear ();
            public void clear()
            {
                kind = localId = email = displayName = idToken = registered = refreshToken = "";
                timeExpires = expiresIn = 0;
            }
            public bool expired => idToken == null || idToken == "" ? true : (timeExpires - Conversion.UnityDateTime (DateTime.Now)) < 60;
            public void fromJson(string json)
            {
                Debug.Log (json);
                JsonUtility.FromJsonOverwrite (json, this);
                timeExpires = Conversion.UnityDateTime (DateTime.Now) + expiresIn;
                Debug.Log ("Successfully logged in! " + idToken);
            }
        }
    }
}

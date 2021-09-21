// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:03:59 by seancooper
using System;
using System.Linq;
using System.Threading.Tasks;
using Hawksbill.Serialization.Json;
using UnityEngine;
using System.Threading;
using System.Net;
using System.IO;

namespace Hawksbill.Network
{
    public static class Web
    {
        public static int PendingRequests { get; private set; }
        static CancellationTokenSource CancellationToken = new CancellationTokenSource ();

        public static void Send(Method method, string url, string requestBody, string token = null, Action<Response> complete = null, float timeout = 15, float retries = 0) =>
            ProcessRequest (method, url, requestBody, token, complete, timeout);

        async static void ProcessRequest(Method method, string url, string requestBody, string token, Action<Response> complete, float timeout)
        {
            PendingRequests++;
            Request request = new Request (url, requestBody, method, token, timeout);
            Task<Response> taskSend = request.send ();
            await taskSend;
            complete?.Invoke (taskSend.Result);
            PendingRequests--;
        }

        public class Request
        {
            readonly string url, token, requestBody;
            readonly Method method;
            readonly float timeout;
            bool state = Application.isPlaying;

            internal Request(string url, string requestBody, Method method, string token, float timeout)
            {
                this.url = url;
                this.token = token;
                this.timeout = timeout;
                this.requestBody = requestBody;
                this.method = method;
            }

            /// <summary>Send the Web Request!</summary>
            internal async Task<Response> send()
            {
                const int Retry = 1;
                HttpWebResponse webResponse = null;

                for (int i = 0; i < Retry && state == Application.isPlaying; i++)
                {
                    if (i > 0) await Task.Delay (500);

                    Pretty.Log (Pretty.Colors.Network, (i > 0 ? "RETRYING (" + i + ") " : "") + "[" + method + "] " + url);

                    WebRequest request = WebRequest.Create (url);

                    request.Method = method.ToString ();
                    request.ContentType = "application/json";

                    if (!String.IsNullOrEmpty (token))
                        request.Headers.Add ("Authorization", "Bearer " + token);

                    if (!String.IsNullOrEmpty (requestBody))
                    {
                        using (var streamWriter = new StreamWriter (request.GetRequestStream ()))
                            streamWriter.Write (requestBody);
                    }

                    // Get Response
                    for (int j = 0; j < Retry; j++)
                    {
                        try
                        {
                            webResponse = (HttpWebResponse) await request.GetResponseAsync ();
                            break;
                        }
                        catch (WebException ex)
                        {
                            webResponse = (HttpWebResponse) ex.Response;
                            Pretty.LogException (Pretty.Colors.Network, ex);
                            // if (webResponse != null)
                            // {
                            //     switch (webResponse.StatusCode)
                            //     {
                            //         case HttpStatusCode.NotFound: // 404                                        
                            //             break;
                            //     }
                            //     webResponse.Close ();
                            // }
                            continue;
                        }
                    }

                    if (webResponse != null)
                    {
                        switch (webResponse.StatusCode)
                        {
                            case HttpStatusCode.OK:
                            case HttpStatusCode.Created: // Andrew
                            case HttpStatusCode.Accepted: // Andrew
                                try
                                {
                                    using (Stream dataStream = webResponse.GetResponseStream ())
                                    {
                                        using (StreamReader reader = new StreamReader (dataStream))
                                        {
                                            string result = reader.ReadToEnd (); //await reader.ReadToEndAsync ();
                                            Response response = new Response (webResponse.StatusCode, result, url);
                                            webResponse.Close ();
                                            return response;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Pretty.LogException (Pretty.Colors.Network, ex);
                                    return new Response (HttpStatusCode.SeeOther, ex.Message, url);
                                }

                            case HttpStatusCode.NotFound: // 404
                                break;
                        }
                    }
                    else await Task.Delay (1000);
                }
                return new Response (webResponse == null ? HttpStatusCode.SeeOther : webResponse.StatusCode,
                    "Retried (" + Retry + ") and failed", url);
            }

            public static implicit operator bool(Request o) => o != null;
            public enum Status { Error, Success }
        }

        [Serializable]
        public class Response
        {
            public Status status;
            public HttpStatusCode httpStatusCode;
            public bool isSuccessful => status == Status.Success;
            public string text;

            public Response(HttpStatusCode statusCode, string result, string url)
            {
                text = result;
                this.httpStatusCode = statusCode;
                if (statusCode != HttpStatusCode.OK)
                {
                    text = statusCode + " " + text;
                    status = Status.Error;
                }
                else status = Status.Success;
                Pretty.Log (Pretty.Colors.Network, "WebRequest response: '" + statusCode + "' " + text + " on " + url);
            }

            public enum Status { Error, Success } //, Exception
            public static implicit operator bool(Response o) => o != null;
        }

        public enum Method { GET = 1, POST = 2, DELETE = 3, PUT = 4 }
    }
}


// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 05/05/2021 11:00:23 by acakebread

using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Hawksbill
{
    public static class Email
    {
		public static void SendEmail(string emailto = "support@massivehadron.com", string subject = "subject text", string body = "body text")
		{
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
			Debug.Log("sending email [windows]: " + emailto + " " + subject + " " + body);
			SendEmailWindows(emailto, subject, body);
#endif

#if UNITY_IOS && !UNITY_EDITOR
			Debug.Log("sending email [mobile]: " + emailto + " " + subject + " " + body);
			SendEmailMobile(emailto, subject, body);
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
			Debug.Log("sending email [mobile]: " + emailto + " " + subject + " " + body);
			SendEmailMobile(emailto, subject, body);
#endif
		}

		private static void SendEmailMobile(string emailto, string subject, string body)
		{
#if UNITY_ANDROID
			Application.OpenURL("mailto:" + emailto + "?subject=" + subject + "&body=" + body);
#endif

#if UNITY_IOS
			Application.OpenURL("mailto:" + emailto + "?subject=" + System.Uri.EscapeDataString(subject) + "&body=" + System.Uri.EscapeDataString(body));
#endif
		}

		private static void SendEmailWindows(string emailto, string subject, string body)
		{
			try
			{
				Application.OpenURL("mailto:" + emailto + "?subject=" + EscapeString(subject) + "&body=" + EscapeString(body));

				string EscapeString(string url) => UnityWebRequest.EscapeURL(url).Replace("+", "%20");
			}
			catch (Exception e)
			{
				Debug.LogError(e.GetBaseException());
				System.Console.WriteLine(e.GetBaseException());
				System.Console.ReadLine();
			}
		}
	}
}

//example call
//Email.SendEmail("acakebread@massivehadron.com","test title", "test body message");
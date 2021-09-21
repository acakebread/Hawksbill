// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 25/01/2021 16:59:30 by seantcooper
using System;
using System.Collections;
using UnityEngine;

namespace Hawksbill.Network
{
    ///<summary>Put text here to describe the Class</summary>
    public class Location : SingletonMonoBehaviour<Location>
    {
		public string status;
		public Info info;

		public static bool IsEnabledByUser => Input.location.isEnabledByUser;
		public static bool HasLocation => GetInstance().info.timestamp > 0;

		public bool getLongitudeLatitude(out float longitude, out float latitude)
		{
			longitude = info.longitude;
			latitude = info.latitude;
			return HasLocation;
		}
		public static bool GetLongitudeLatitude(out float longitude, out float latitude) =>
			GetInstance().getLongitudeLatitude(out longitude, out latitude);

		IEnumerator Start()
		{
			info = new Info();

			if (!Input.location.isEnabledByUser)
			{
				status = "Location is not enabled by user!";
				yield break;
			}

			Input.location.Start(10, 10);

			var startTime = Time.time;
			while (enabled)
			{
				status = Input.location.status.ToString();
				switch (Input.location.status)
				{
					case LocationServiceStatus.Initializing:
						if (Time.time - startTime > 20)
						{
							status = "Timed out";
							yield break;
						}
						break;
					case LocationServiceStatus.Failed: yield break;
					case LocationServiceStatus.Stopped: yield break;
					case LocationServiceStatus.Running:
						status = Input.location.status.ToString();
						info.latitude = Input.location.lastData.latitude;
						info.longitude = Input.location.lastData.longitude;
						info.timestamp = Input.location.lastData.timestamp;
						break;
				}
				yield return new WaitForSeconds(1f);
			}

			Input.location.Stop();
		}

		//void OnGUI()
		//{
		//	GUI.skin.label.fontSize = Screen.height / 32;
		//	GUILayout.Space(64);
		//	GUILayout.Label("Location: " + status + " " + info);
		//}

		[Serializable]
		public class Info
		{
			public float latitude;
			public float longitude;
			public double timestamp;
			public override string ToString()
			{
				return "latitude: " + latitude + " longitude: " + longitude + " timestamp: " + timestamp;
			}
		}
	}
}
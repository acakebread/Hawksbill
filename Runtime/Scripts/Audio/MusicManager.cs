// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 06/02/2021 11:40:30 by acakebread
using UnityEngine;
using Hawksbill;

namespace Hawksbill
{
	public class MusicManager : SingletonMonoBehaviour<MusicManager>
	{
		public AudioSource mainMusic;
		private static AudioSource _mainMusic { get { return GetInstance().mainMusic; } }

		public static bool PlayMainMusic
		{
			set
			{
				if (null == _mainMusic) return;
				_mainMusic.enabled = true;
				if (true == value)
				{
					_mainMusic.volume = true == PlayerPrefs.HasKey("MainMusicVolume") ? PlayerPrefs.GetFloat("MainMusicVolume") : 1;
					_mainMusic.Play();
				}
				else { _mainMusic.Stop(); }
			}
		}

		public static float MainMusicVolume
		{
			set
			{
				if (null == _mainMusic) return;
				_mainMusic.volume = value;
				PlayerPrefs.SetFloat("MainMusicVolume", value);
			}
		}
	}
}
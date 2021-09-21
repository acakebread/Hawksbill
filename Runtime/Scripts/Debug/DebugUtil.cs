// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 22/02/2021 18:19:41 by acakebread

using UnityEngine;

namespace Hawksbill
{
    public class DebugUtil : MonoBehaviour
	{
		public string title = "DEBUG MESSAGE";
		public string message = "";
		private Rect dlgRect = new Rect(Screen.width * 0.2f, Screen.height * 0.2f, Screen.width * 0.6f, Screen.height * 0.6f);

		private void WindowFunc(int windowID)
		{
			var originalSkin = GUI.skin;

			GUI.skin.window.normal.background = texture(new Color(0.9f, 0.1f, 0.1f, 0.8f));
			GUI.skin.window.onNormal.background = null;

			GUI.color = Color.white;
			float border = Screen.height * 0.01f;
			float btnSize = Screen.height * 0.04f;

			//title
			Rect titleRect = new Rect(border, border, dlgRect.width - border * 3 - btnSize, btnSize);
			DrawQuad(titleRect, new Color(0.5f, 0, 0, 0.2f));
			GUI.skin.label.fontSize = (int)(btnSize * 0.7f);
			GUI.Label(titleRect, title);

			//message body
			Rect msgRect = new Rect(border, border * 2 + btnSize, dlgRect.width - border * 2, dlgRect.height - (border * 3 + btnSize));
			DrawQuad(msgRect, new Color(0.5f, 0, 0, 0.2f));
			GUI.skin.label.fontSize = (int)(btnSize * 0.7f);
			GUI.Label(msgRect, message);

			//close button 
			GUI.skin.button.fontSize = (int)btnSize;
			GUI.backgroundColor = new Color(1, 0, 0, 0.4f);
			Rect btnRect = new Rect(dlgRect.width - btnSize - border, border, btnSize, btnSize);
			if (GUI.Button(btnRect, "X")) { Destroy(gameObject); }

			GUI.DragWindow(new Rect(0, 0, 10000, 10000));

			GUI.skin = originalSkin;

			//local functions
			void DrawQuad(Rect rect, Color color)
			{
				GUI.skin.box.normal.background = texture(color);
				GUI.Box(rect, GUIContent.none);
			}

			Texture2D texture(Color color)
			{
				Texture2D _texture = new Texture2D(1, 1);
				_texture.SetPixel(0, 0, color);
				_texture.Apply();
				return _texture;
			}
		}

		void OnGUI() => dlgRect = GUI.Window(0, dlgRect, WindowFunc, string.Empty);

		public static DebugUtil ShowMsg(string title = "title here", string msg = "msg here")
		{
			var debugUtil = ShowMsg(msg);
			debugUtil.title = title;
			return debugUtil;
		}

		public static DebugUtil ShowMsg(string msg = "msg here")
		{
			if (!Application.isPlaying) return null;

			var debugUtil = FindObjectOfType<DebugUtil> (true);
            if (!debugUtil) debugUtil = new GameObject ("DebugUtil").AddComponent<DebugUtil> ();
            debugUtil.message = (string.IsNullOrEmpty (debugUtil.message) ? "" : debugUtil.message + "\n") + msg;
			return debugUtil;
		}

        public static void CloseAll() => FindObjectsOfType<DebugUtil> (true).ForAll (d => DestroyImmediate (d));
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrettyEditorAttribute : MonoBehaviour
{
    public Type type = Type.Full;
    public Color color = new Color (1, 1, 1, 0.2f);
    public Color errorColor = new Color (1, 0, 0, 1);
    public bool error;

    public enum Type
    {
        Full = 0,
        Margin = 1,
    }
}

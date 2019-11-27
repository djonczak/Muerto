using System.Runtime.InteropServices;
using UnityEngine;

public class CursorOptions : MonoBehaviour
{
    public Texture2D cursorTexture;

#if UNITY_STANDALONE_WIN
    [DllImport("user32.dll")]
    static extern bool ClipCursor(ref RECT lpRect);
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }
#endif

    void Start()
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);

        RECT cursorLimits;
        cursorLimits.Left = 0;
        cursorLimits.Top = 0;
        cursorLimits.Right = Screen.width - 1;
        cursorLimits.Bottom = Screen.height - 1;
        ClipCursor(ref cursorLimits);
    }
}

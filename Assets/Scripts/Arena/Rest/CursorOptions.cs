using System.Runtime.InteropServices;
using UnityEngine;

namespace Game.VFX
{
    public class CursorOptions : MonoBehaviour
    {
        public Texture2D cursorTexture;

        private void Start()
        {
            Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        }
    }
}
﻿using System.Runtime.InteropServices;
using UnityEngine;

public class CursorOptions : MonoBehaviour
{
    public Texture2D cursorTexture;

    private void Start()
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }
}

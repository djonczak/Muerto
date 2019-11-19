using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickEvent : MonoBehaviour
{
    public delegate void ItemPickedHandler(int i);
    public static event ItemPickedHandler OnItemPick;

    public static void ItemPicked(int i)
    {
        OnItemPick?.Invoke(i);
    }
}

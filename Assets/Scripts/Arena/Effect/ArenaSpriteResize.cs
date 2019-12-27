using UnityEngine;

public class ArenaSpriteResize : MonoBehaviour
{
    public Transform arenaSprite;

    void OnEnable()
    {
        Vector3 maxScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        //Debug.Log(" X: " + maxScreenBounds.x + " Y: " +  maxScreenBounds.y);

        arenaSprite.transform.localScale = new Vector3(maxScreenBounds.x / 10f, arenaSprite.localScale.y, 1);
    }

}

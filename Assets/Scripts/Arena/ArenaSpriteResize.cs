using UnityEngine;

public class ArenaSpriteResize : MonoBehaviour
{
    public Transform arenaSprite;

    void OnEnable()
    {
        Vector3 maxScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
       // Debug.Log(" X: " + maxScreenBounds.x + " Y: " +  maxScreenBounds.y);

        if (maxScreenBounds.x > 2.24)
        {
            arenaSprite.transform.localScale = new Vector3(maxScreenBounds.x - 0.65f, arenaSprite.localScale.y, 0f);
        }
        else
        {
            arenaSprite.transform.localScale = new Vector3(maxScreenBounds.x - 0.5f, arenaSprite.localScale.y, 0f);
        }
    }

}

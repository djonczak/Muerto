using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArenaLoad : MonoBehaviour
{
    public Image blackScreen;
    [SerializeField] private float blackScreenDisperseTime = 1f;

    private bool canColor = true;
    private Color blackScreenMainColor;
    private Color alphaColor = new Color(0f, 0f, 0f, 0f);
    private float t;

    public void Start()
    {
        blackScreenMainColor = blackScreen.color;
        StartCoroutine("BlackScreenDisperse", blackScreenDisperseTime);
    }

    public void Update()
    {
        if (canColor)
        {
            t += Time.deltaTime / blackScreenDisperseTime;
            blackScreen.color = Color.Lerp(blackScreenMainColor, alphaColor, t);
        }
    }

    IEnumerator BlackScreenDisperse(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}

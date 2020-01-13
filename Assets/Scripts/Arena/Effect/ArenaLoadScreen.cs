using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ArenaLoadScreen : MonoBehaviour
{
    public Image blackScreen;
    [SerializeField] private float blackScreenDisperseTime = 1f;

    private bool canColor = true;
    private Color blackScreenMainColor;
    private Color alphaColor = new Color(0f, 0f, 0f, 0f);
    private float t;

    private void Start()
    {
        blackScreenMainColor = blackScreen.color;
        StartCoroutine("BlackScreenDisperse", blackScreenDisperseTime);
    }

    private void Update()
    {
        if (canColor)
        {
            t += Time.deltaTime / blackScreenDisperseTime;
            blackScreen.color = Color.Lerp(blackScreenMainColor, alphaColor, t);
        }
    }

    private IEnumerator BlackScreenDisperse(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}

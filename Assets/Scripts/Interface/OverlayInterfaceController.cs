using UnityEngine.UI;
using UnityEngine;

public class OverlayInterfaceController : MonoBehaviour
{
    public Image darkScreen;
    public float fadeSpeed = 2f;
    private bool fadingIn;
    private bool fadingOut;

    private void Start()
    {
        FadeIn();
    }

    private void Update()
    {
        if (fadingOut)
            darkScreen.color = new Color(darkScreen.color.r, darkScreen.color.g, darkScreen.color.b, Mathf.MoveTowards(darkScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
        if (fadingIn)
            darkScreen.color = new Color(darkScreen.color.r, darkScreen.color.g, darkScreen.color.b, Mathf.MoveTowards(darkScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
    }

    public void FadeOut()
    {
        fadingOut = true;
        fadingIn = false;
    }

    public void FadeIn()
    {
        fadingIn = true;
        fadingOut = false;
    }

}

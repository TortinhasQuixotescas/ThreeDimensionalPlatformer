using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour

{
    public static UIController uniqueInstance;
    private bool fadingIn;
    private bool fadingOut;
    public float fadeSpeed = 2f;
    public Image darkScreen;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void Awake()
    {
        uniqueInstance = this;
    }

    // Update is called once per frame
    void Update()
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

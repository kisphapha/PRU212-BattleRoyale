using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingSprite : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    private Color defaultColor;
    private Color fadedColor;
    // Start is called before the first frame update
    void Start()
    {
        defaultColor = spriteRenderer.color;
        fadedColor = defaultColor;
        fadedColor.a = 0f;
    }
    public void FadeOut()
    {
        spriteRenderer.color = fadedColor;
    }
    public void FadeIn()
    {
        spriteRenderer.color = defaultColor;
    }
    // Update is called once per frame
    void Update()
    {
       

    }
}

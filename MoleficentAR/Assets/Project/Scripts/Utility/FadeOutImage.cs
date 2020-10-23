using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOutImage : MonoBehaviour
{
    float Delay = 2f, FadeOutTime = 1f, Speed = 1f;
    float counter = 0f;
    bool fading = false;
    RawImage TextImage;
    Color originalColor;
    Vector3 StartingPosition;

    void Start()
    {
        TextImage = GetComponent<RawImage>();
        originalColor = TextImage.color;
        originalColor.a = 0.0f;
        TextImage.color = originalColor;
        StartingPosition = transform.position;

        Deactivate();

    }

    void Update()
    {
        if (fading && counter < FadeOutTime)
        {
            counter += Time.deltaTime;
            originalColor.a = 1 - counter / FadeOutTime;
            TextImage.color = originalColor;
            transform.position += transform.up * Speed * Time.deltaTime;

        }
    }

    public void Activate()
    {
        Color originalColor = TextImage.color;
        originalColor.a = 1f;
        TextImage.color = originalColor;
        CancelInvoke();

        Invoke("StartFadeout", Delay);
    }

    void StartFadeout()
    {
        fading = true;
    }

    public void Deactivate()
    {
        Color originalColor = TextImage.color;
        originalColor.a = 0f;
        TextImage.color = originalColor;
        transform.position = StartingPosition;
        fading = false;
        counter = 0f;
    }
}

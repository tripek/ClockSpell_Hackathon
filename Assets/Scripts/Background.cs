using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] AnimationCurve curveBlueSky;
    [SerializeField] AnimationCurve curveRedSky;
    [SerializeField] Transform sunTransform;
    [SerializeField] SpriteRenderer spriteRendererBlueSky;
    [SerializeField] SpriteRenderer spriteRendererRedSky;
    private float sunPercentage;

    private void Update()
    {
        sunPercentage = ((sunTransform.localPosition.y - 6) * -1) / 12;
        spriteRendererRedSky.color = new Color(spriteRendererRedSky.color.r, spriteRendererRedSky.color.g, spriteRendererRedSky.color.b, curveRedSky.Evaluate(sunPercentage));
    }
}

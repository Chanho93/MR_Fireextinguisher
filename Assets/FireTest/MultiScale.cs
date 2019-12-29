using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiScale : MonoBehaviour
{
    public enum ScalePolicy
    {
        FIXED_WIDTH,
        FIXED_HEIGHT
    }

    public float desiredWidth;
    public float desiredHeight;
    public float pixelsToUnits;
    private ScalePolicy scalePolicy;

    private void Awake()
    {
        float desiredRatio = desiredWidth / desiredHeight;
        float currentRatio = (float)Screen.width / (float)Screen.height;
        float differenceInSize = desiredRatio / currentRatio;
        float desiredOrthographicSize = desiredHeight / 2 / pixelsToUnits;
        float targetOrthographicSize = 0.0f;

        if (currentRatio <= desiredRatio)
        {
            scalePolicy = ScalePolicy.FIXED_WIDTH;
        }
        else
        {
            scalePolicy = ScalePolicy.FIXED_HEIGHT;
        }

        switch (scalePolicy)
        {
            case ScalePolicy.FIXED_WIDTH:
                targetOrthographicSize = desiredOrthographicSize * differenceInSize;
                break;

            case ScalePolicy.FIXED_HEIGHT:
                targetOrthographicSize = desiredOrthographicSize;
                break;
        }
        Camera.main.orthographicSize = targetOrthographicSize;
    }
}

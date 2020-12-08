﻿using UnityEngine;
using TMPro;

public class FrameRateCounter : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI display = default;

    public enum DisplayMode{ FPS, MS}

    [SerializeField]
    DisplayMode displayMode = DisplayMode.FPS;

    [SerializeField, Range(0.1f, 2f)]
    float sampleDuration = 1f;

    int frame;
    float duration, bestDuration = float.MaxValue, worstDuration;

    private void Update()
    {
        float frameDuration = Time.unscaledDeltaTime;
        frame += 1;
        duration += frameDuration;

        if (frameDuration < bestDuration)
            bestDuration = frameDuration;
        if (frameDuration > worstDuration)
            worstDuration = frameDuration;

        if (duration > sampleDuration)
        {
            if(displayMode == DisplayMode.FPS)
            {
                display.SetText("FPS\n{0:0}\n{1:0}\n{2:0}",
                1f / bestDuration, frame / duration, 1f / worstDuration);
            }
            else
            {
                display.SetText("MS\n{0:1}\n{1:1}\n{2:1}",
                1000f * bestDuration, 1000f * duration / frame,
                1000f * worstDuration);
            }
            
            frame = 0;
            duration = 0f;
            bestDuration = float.MaxValue;
            worstDuration = 0f;
        }
    }
}

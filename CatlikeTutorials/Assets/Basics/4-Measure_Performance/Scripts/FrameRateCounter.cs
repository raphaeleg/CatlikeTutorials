using UnityEngine;
using TMPro;
using UnityEditor.Rendering;

public class FrameRateCounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI display;
    public enum DisplayMode { FPS, MS }
    [SerializeField] DisplayMode displayMode = DisplayMode.FPS;

    int frames = 0;
    float duration = 0f;
    float bestDuration = float.MaxValue;
    float worstDuration = 0f;
    [SerializeField, Range(0.1f, 2f)] float sampleDuration = 1f;
    void Update()
    {
        float frameDuration = Time.unscaledDeltaTime;
        frames += 1;
        duration += frameDuration;
        if (frameDuration < bestDuration) { bestDuration = frameDuration; }
        if (frameDuration > worstDuration) {  worstDuration = frameDuration; }
       
        if (duration < sampleDuration) { return; }
        
        if (displayMode == DisplayMode.FPS) { DisplayFPS(); }
        else { DisplayMS(); }

        frames = 0;
        duration = 0f;
        bestDuration = float.MaxValue;
        worstDuration = 0f;
    }

    void DisplayFPS()
    {
        display.SetText("FPS\n{0:0}\n{1:0}\n{2:0}",
            1f / bestDuration,
            frames / duration,
            1f / worstDuration
        );
    }

    void DisplayMS()
    {
        display.SetText("MS\n{0:1}\n{1:1}\n{2:1}",
            1000f * bestDuration,
            1000f * duration/frames,
            1000f * worstDuration
        ) ;
    }
}
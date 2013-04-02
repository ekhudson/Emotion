using UnityEngine;
using System.Collections;

[System.Serializable]
public class AdjustableAudioClip
{
    public AudioClip Clip;
    public bool RandomPitch;
    public float PitchMin;
    public float PitchMax;

    public float Pitch
    {
        get
        {
            if (RandomPitch)
            {
                return Random.Range(PitchMin, PitchMax);
            }
            else
            {
                return 1;
            }
        }
    }

}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GrendelAudioOptions
{
    public int test;
    private List<GrendelAudioChannel> mAudioChannels = new List<GrendelAudioChannel>();
    private List<GrendelAudioBank> mAudioBanks = new List<GrendelAudioBank>();

    public List<GrendelAudioChannel> AudioChannels
    {
        get
        {
            return mAudioChannels;
        }
        set
        {
            mAudioChannels = value;
        }
    }

    public List<GrendelAudioBank> AudioBanks
    {
        get
        {
            return mAudioBanks;
        }
        set
        {
            mAudioBanks = value;
        }
    }
}

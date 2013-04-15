using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class ProgressionManager : ScriptableObject
{
    [System.Serializable]
    public class LevelData
    {
        public int Level;
        public int XPAmount;
        public string Descriptor;
    }

    public AnimationCurve LevellingCurve;
    [HideInInspector]public List<LevelData> Levels = new List<LevelData>();

    public static int XPForNextLevel(ProgressionManager manager, int currentLevel)
    {
        return manager.Levels[currentLevel + 1].XPAmount;
    }
}

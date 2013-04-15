using UnityEngine;

using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Information relevant to the Player (current XP, inventory, personalization, etc.)
/// </summary>
[System.Serializable]
public class PlayerData
{
    public string PlayerName = "NewPlayer";
    public Color PrimaryPlayerColor = Color.green;
    public Color SecondaryPlayerColor = Color.white;

    public InventoryData PlayerInventory;

    public ProgressionManager ProgressionManagerToUse;
    private int mCurrentLevel = 1;
    private int mCurrentXP = 0;

    public int CurrentLevel
    {
        get
        {
            return mCurrentLevel;
        }
    }

    public int CurrentXP
    {
        get
        {
            return mCurrentXP;
        }
    }

    public void GiveXP(int amount)
    {
        mCurrentXP += amount;
    }

}

using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class InventoryView : MonoBehaviour
{
    public ItemClass DebugItem;
    private InventoryData mInventoryData;
    private int mColumnAmt = 5;

    private const float kInventoryIconSize = 64f;
    private const float kWeightIndicatorWidth = 128f;


    private void Start()
    {
        if (mInventoryData == null)
        {
            mInventoryData = new InventoryData();
        }

        //mColumnAmt = Mathf.RoundToInt(Screen.width / kInventoryIconSize);
    }

    public void AssignInventoryData(InventoryData data)
    {
        mInventoryData = data;
    }

    private void Update ()
    {
    
    }

    public void OnGUI()
    {
        if (mInventoryData == null)
        {
            return;
        }

        GUILayout.BeginVertical();

        GUILayout.Box(string.Empty, GUI.skin.button, GUILayout.Width(kWeightIndicatorWidth));

        Rect rect = GUILayoutUtility.GetLastRect();
        rect.width = kWeightIndicatorWidth * (mInventoryData.CurrentWeight / mInventoryData.WeightCapacity);

        GUI.color = Color.green;
        GUI.Box(rect, string.Empty, GUI.skin.button);
        GUI.color = Color.white;

        rect.width = kWeightIndicatorWidth;

        GUI.Box(rect, string.Format("Weight: {0} / {1}", mInventoryData.CurrentWeight.ToString(), mInventoryData.WeightCapacity.ToString()), GUI.skin.label);

            GUILayout.BeginHorizontal();
    
                if(GUILayout.Button("Add"))
                {
    
                     ItemClass item = DebugItem;
//                    item.ShortName = "TestItem";
//                    item.MaxStackSize = 5;
//                    item.Weight = 1.75f;
                    mInventoryData.InsertItem(item);
                }

                foreach(InventoryData.InventorySlotData slot in mInventoryData.InventorySlotsData)
                {
                    GUILayout.BeginVertical();

                    if(GUILayout.Button(new GUIContent(string.Empty, slot.Item.InventoryIcon, slot.Item.Description), GUI.skin.label, new GUILayoutOption[]{ GUILayout.Width(kInventoryIconSize), GUILayout.Height(kInventoryIconSize)}))
                    {
                        mInventoryData.RemoveItem(slot.Item, slot);
                        return;
                    }

                    GUILayout.Label(slot.Item.ShortName + " x " + slot.Quantity, GUILayout.Width(kInventoryIconSize));

                    GUILayout.EndVertical();
                }

            GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }
}

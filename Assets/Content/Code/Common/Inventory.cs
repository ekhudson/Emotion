using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory
{
    public class InventorySlot
    {
        public ItemClass Item;
        public int Quantity;        
    }
    
    public float WeightCapacity = 100f;
        
    
    public List<InventorySlot> InventorySlots = new List<InventorySlot>();
    
    public InventorySlot ContainsItem(ItemClass item)
    {
        foreach(InventorySlot slot in InventorySlots)
        {
            if (slot.Item != item)
            {
                continue;
            }

            else return slot;
        }
    }
}

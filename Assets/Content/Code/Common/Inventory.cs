using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory
{
    public class InventorySlot
    {
        public ItemClass Item;
        public int Quantity;

        public void Inventory(ItemClass item, int quantity)
        {
            Item = item;
            Quantity = quantity;
        }
    }
    
    public float WeightCapacity = 100f;
    public List<InventorySlot> InventorySlots = new List<InventorySlot>();


    private float mCurrentWeight = 0f;

    public float CurrentWeight
    {
        get
        {
            return mCurrentWeight;
        }
    }

    public InventorySlot ContainsItem(ItemClass item)
    {
        foreach(InventorySlot slot in InventorySlots)
        {
            if (slot.Item != item)
            {
                continue;
            }
            else
            {
                return slot;
            }
        }

        return null;
    }

    public bool InsertItem(ItemClass item)
    {
        InventorySlot slot = ContainsItem(item);

        //Don't bother at all if it will put us over capacity
        if((mCurrentWeight + item.Weight) > WeightCapacity)
        {
            return false;
        }

        if (slot == null)
        {
            //no slot
           InventorySlots.Add(new InventorySlot(item, 1));

        }
        else if (slot.Item.MaxStackSize >= slot.Quantity)
        {
            //slot full
            InventorySlots.Add(new InventorySlot(item, 1));
        }
        else
        {
            
        }
    }

}

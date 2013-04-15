using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryData
{
    public class InventorySlotData
    {
        public ItemClass Item;
        public int Quantity;

        public InventorySlotData(ItemClass item, int quantity)
        {
            Item = item;
            Quantity = quantity;
        }
    }
    
    public float WeightCapacity = 100f;
    public List<InventorySlotData> InventorySlotsData = new List<InventorySlotData>();

    private float mCurrentWeight = 0f;

    public float CurrentWeight
    {
        get
        {
            return mCurrentWeight;
        }
    }

    public InventorySlotData ContainsItem(ItemClass item, bool skipFullStacks)
    {
        foreach(InventorySlotData slot in InventorySlotsData)
        {
            if (slot.Item != item)
            {
                continue;
            }
            else if (skipFullStacks && slot.Quantity >= item.MaxStackSize)
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

    public bool HasRoomForItem(ItemClass item)
    {
        if((mCurrentWeight + item.Weight) > WeightCapacity)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool InsertItem(ItemClass item)
    {
        InventorySlotData slot = ContainsItem(item, true);

        //Don't bother at all if it will put us over capacity
        if (!HasRoomForItem(item))
        {
            return false;
        }

        if (slot == null) //no slot for this item yet
        {
            InventorySlotsData.Add(new InventorySlotData(item, 1));
            mCurrentWeight += item.Weight;
            return true;
        }
        else if (slot.Quantity >= slot.Item.MaxStackSize) //current slot is full
        {
            InventorySlotsData.Add(new InventorySlotData(item, 1));
            mCurrentWeight += item.Weight;
            return true;
        }
        else if (slot.Quantity < slot.Item.MaxStackSize) //current slot has room
        {
            slot.Quantity++;
            mCurrentWeight += item.Weight;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool RemoveItem(ItemClass item)
    {
        return RemoveItem(item, null);
    }

    public bool RemoveItem(ItemClass item, InventorySlotData slotToRemoveFrom)
    {
        if (slotToRemoveFrom == null)
        {
            slotToRemoveFrom = ContainsItem(item, true);
        }

        if (item != slotToRemoveFrom.Item)
        {
            return false;
        }

        if (slotToRemoveFrom == null) //this item wasn't found in the inventory
        {
            return false;
        }
        else
        {
            slotToRemoveFrom.Quantity--;
            mCurrentWeight  -= slotToRemoveFrom.Item.Weight;

            if (slotToRemoveFrom.Quantity <= 0)
            {
                InventorySlotsData.Remove(slotToRemoveFrom);
            }

            return true;
        }
    }
}

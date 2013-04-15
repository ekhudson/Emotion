using UnityEngine;
using System.Collections;

public class ItemClass : MonoBehaviour
{
    public string LongName = string.Empty;
    public string ShortName = string.Empty;
    public string Description = string.Empty;
    public float Weight;
    public int MaxStackSize = 100;
    public GameObject InventoryModel;
    public GameObject WorldModel;
    public Texture2D InventoryIcon;
}

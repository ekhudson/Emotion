using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class LevelData : MonoBehaviour
{
    public string MapName = "TheZone";
    public TileManager.TileCoordinate PlayerStart = new TileManager.TileCoordinate(0,0);    
    
    [System.Serializable]
    public class WorldLayer
    {
        [SerializeField]public Texture2D[] Data;
        [SerializeField]public int Depth = 0;
        [SerializeField]public bool Impassable = false;
    }
    
    public WorldLayer TerrainLayer = new WorldLayer();
    public WorldLayer ImpassableLayer = new WorldLayer();
    public WorldLayer PassableLayer = new WorldLayer();
    public WorldLayer OverlayLayer = new WorldLayer();
    
    private WorldLayer[] mLayers;
    
    public WorldLayer[] Layers
    {
        get
        {
            WorldLayer[] tempLayerArray;

                tempLayerArray = new WorldLayer[4];
                tempLayerArray[0] = TerrainLayer;
                tempLayerArray[1] = ImpassableLayer;
                tempLayerArray[2] = PassableLayer;
                tempLayerArray[3] = OverlayLayer;
                    
            return tempLayerArray;

        }
    }
}

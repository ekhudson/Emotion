//#define DEBUG

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileManager : Singleton<TileManager>
{
    public GameObject TilePrefab;
    public LevelData WorldMap;
    public int TileSize = 128;
    public int TileLimit = 32768;
//    private int mHalfLimit;
    private int mBufferAmt = 3;
    private int mColumns;
    private int mRows;
    public static int WorldSize = 300;
	

#if DEBUG
    private UISprite[] mPathDebugSprites;
#endif

    private UISprite mHighlightTile;

    [System.Serializable]
    public class TileCoordinate
    {
        public int x;
        public int y;
        
        public TileCoordinate(int xpos, int ypos)
        {
            x = xpos;
            y = ypos;
        }
    } 
       
    private List<UISprite> CanvasList = new List<UISprite>();

    public UISprite HighlightTile
    {
        get
        {
            if (mHighlightTile == null)
            {
                mHighlightTile = NGUITools.AddChild(gameObject, TilePrefab).GetComponent<UISprite>();
                mHighlightTile.spriteName = "highlight";
                mHighlightTile.depth = 5;
                mHighlightTile.MakePixelPerfect();
            }

            return mHighlightTile;

        }
    }
    
    void Start()
    {        
        EventManager.Instance.AddHandler<CameraMoveEvent>(OnCameraMoveHandler);
        
        mColumns = (1920 / TileSize) + (mBufferAmt);
        mRows = (1080 / TileSize) + (mBufferAmt);
        
//        mHalfLimit = TileLimit / 2;
        
        for(int i = 0; i < mColumns; i++)
        {
            for(int j = 0; j < mRows; j++)
            {
                UISprite tile = NGUITools.AddChild(gameObject, TilePrefab).GetComponent<UISprite>();
                tile.transform.localPosition = new Vector3(i * TileSize, j * TileSize, 0);
                NGUITools.MakePixelPerfect(tile.transform);
                CanvasList.Add(tile);
            }
        }

        MainCamera.Instance.transform.localPosition = new Vector3(0,0,10);
        
        EventManager.Instance.Post(new CameraMoveEvent());
        
		Vector3 scale = transform.localScale;
		scale *= MainCamera.Instance.camera.orthographicSize;
		transform.localScale = scale;
		
		//NetworkManager.Instance.LocalPlayerAvatar.transform.localPosition = new Vector3(WorldMap.PlayerStart.x * TileSize, (0 - (WorldMap.PlayerStart.y)) * TileSize, 0);
    }
    
    public void OnCameraMoveHandler(object sender, CameraMoveEvent evt)
    {
        TileCoordinate position = new TileCoordinate((int)Mathf.Floor(transform.InverseTransformPoint(MainCamera.Instance.transform.position).x / TileSize) - (mColumns / 2),
                                        (int)Mathf.Floor(transform.InverseTransformPoint(MainCamera.Instance.transform.position).z / TileSize) - (mRows / 2));
		
		
		
        int colStart = position.x;
        int colEnd = position.x + mColumns;
        
        int rowStart = position.y ;
        int rowEnd = position.y+ mRows;
        
        TileCoordinate keyPosition = new TileCoordinate(colStart, rowStart);
        
        int canvasIndex = 0;
        
        int indexX = 0; 
        int indexY = 0;
        
        UISprite sprite;

        for(int i = colStart; i < colEnd; i++)
        {
            for (int j = rowStart; j < rowEnd; j++)
            {
                keyPosition = new TileCoordinate(i,j);
                
                indexX = keyPosition.x + (TileManager.WorldSize / 2);
                indexY = WorldSize - (keyPosition.y + (TileManager.WorldSize / 2));
                
                foreach(LevelData.WorldLayer layer in WorldMap.Layers)
                {
                    
                    if (layer.Data[indexX * (WorldSize) + indexY] == null)
                    {
                        continue;
                    }
                    
                    if (canvasIndex > (CanvasList.Count - 1))
                    {
                        UISprite tile = NGUITools.AddChild(gameObject, TilePrefab).GetComponent<UISprite>();
                        NGUITools.MakePixelPerfect(tile.transform);
                        CanvasList.Add(tile);
                        sprite = tile;
                    }
                    else
                    {
                        sprite = CanvasList[canvasIndex];
                    }
                    
                    sprite.transform.localPosition = new Vector3((i * TileSize), (j * TileSize), 0);
                    sprite.depth = layer.Depth;
                    sprite.spriteName = layer.Data[indexX * (WorldSize) + indexY].name;
                    sprite.collider.enabled = layer.Impassable;
                    NGUITools.MakePixelPerfect(sprite.transform);

                    canvasIndex++;
                }
            }
        }
    }

#if DEBUG
    public void UpdatePathDebug(TileCoordinate start, TileCoordinate end)
    {
//        Pathfinder.PathCoordinate[] path = Pathfinder.FindPath(start, end, null);
//
//        if (mPathDebugSprites != null && mPathDebugSprites.Length > 0)
//        {
//            for (int i = mPathDebugSprites.Length - 1; i >= 0; i--)
//            {
//                Destroy(mPathDebugSprites[i].gameObject);
//            }
//        }
//
//        mPathDebugSprites = new UISprite[path.Length];
//
//        for(int j = 0; j < mPathDebugSprites.Length; j++)
//        {
//            mPathDebugSprites[j] = NGUITools.AddChild(gameObject, TilePrefab).GetComponent<UISprite>();
//            mPathDebugSprites[j].name = "highlight";
//            mPathDebugSprites[j].spriteName = "highlight";
//            mPathDebugSprites[j].MakePixelPerfect();
//            mPathDebugSprites[j].color = Color.yellow;
//            mPathDebugSprites[j].depth = 100;
//            mPathDebugSprites[j].transform.localPosition = new Vector3((path[j].PathNode.x * TileSize), (path[j].PathNode.y * TileSize), 0);
//        }


    }
#endif

    public Vector3 GetTileCoordinates(Vector2 position)
    {
        TileCoordinate keyPosition = new TileCoordinate((int)Mathf.Floor(position.x / TileSize),
                                     (int)Mathf.Floor(position.y / TileSize));


        return new Vector3( keyPosition.x * TileSize , keyPosition.y * TileSize, 0);
    }

    public bool CheckPassability(Vector2 position)
    {
        TileCoordinate keyPosition = new TileCoordinate((int)Mathf.Floor(position.x / TileSize),
                                     (int)Mathf.Floor(position.y / TileSize));

        int indexX = keyPosition.x + (TileManager.WorldSize / 2);
        int indexY = WorldSize - (keyPosition.y + (TileManager.WorldSize / 2));


        return !WorldMap.ImpassableLayer.Data[indexX * (WorldSize) + indexY];
    }

    public bool CheckPassability(Vector3 position)
    {
        return CheckPassability(new Vector2(position.x, position.y));
    }

    public bool CheckPassability(TileCoordinate tileCoordinates)
    {
        return CheckPassability(new Vector2(tileCoordinates.x * TileSize, tileCoordinates.y * TileSize));
    }

    public bool CheckPassability(Bounds boundsToCheck)
    {
        TileCoordinate keyPosition = new TileCoordinate((int)Mathf.Floor(boundsToCheck.center.x / TileSize), (int)Mathf.Floor(boundsToCheck.center.y / TileSize));
        //int indexX = keyPosition.x + (TileManager.WorldSize / 2);
        //int indexY = WorldSize - (keyPosition.y + (TileManager.WorldSize / 2));

        Bounds tileBounds = new Bounds(new Vector3(keyPosition.x - TileSize, keyPosition.y - TileSize, 0), new Vector3(TileSize * 0.5f, TileSize * 0.5f));
        Vector3 originalCentre = tileBounds.center;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                tileBounds.center = originalCentre + new Vector3(i * TileSize, j * TileSize, 0);
                if (tileBounds.Intersects(boundsToCheck))
                {
                    return false;
                }
            }
        }

        return true;
    }
}
             
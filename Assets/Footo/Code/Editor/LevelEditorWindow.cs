using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;


public class LevelEditorWindow : EditorWindow
{
    [MenuItem("IWZ/Level Editor")]
    static void Init()
    {
        mWindowRef = EditorWindow.GetWindow(typeof(LevelEditorWindow));
        mWindowRef.minSize = new Vector2(1240, 780);
        mWindowRef.wantsMouseMove = true;
    }
    
    private int mTextureToolBarSelection = 0;
    private int mCurrentTextureIndex = 0;
    private int mCurrentLayerIndex = 0;
    private bool mShowAllLayers = false;
    private Vector2 mCanvasScrollPosition = Vector2.zero;
    private Vector2 mTextureScrollPosition = Vector2.zero;
    private static EditorWindow mWindowRef;    
    private TextureCollection mTerrainTextures = new TextureCollection();
    private TextureCollection mPropTextures = new TextureCollection();
    private List<TextureCollection> mTextureCollections = new List<TextureCollection>();
    private string[] mTextureFolders = new string[]{"Tiles", "Props"};
    private GUIStyle mTextureGridStyle;    
    private GameObject CurrentMap;
    private LevelData mLevelData;
    private GUIStyle mMapTileStyle = new GUIStyle();
    private GUIStyle mGridStyle = new GUIStyle();
    private Texture2D mTempMapTexture = new Texture2D(128,128);
    private int mColumns;
    private int mRows;
    private int TileSize = 128;
    private float mScrollSpeed = 500f;
    
    private float mDeltaTime = 0f;
    private double mCurrentTime = 0f;
    
    private string[] mPenSizeStrings  = new string[]
    {
        "1",
        "9",
        "25"        
    };
    
    private int[] mPenSizes = new int[]
    {
        0,
        1,
        2        
    };
    
    private int mPenSizeChoice = 0;
    
    //public static int WorldSize = 300;
    private Rect mWorldRect = new Rect();
    private Texture2D mEmptyTexture = new Texture2D(1,1);
    
    [System.Serializable]
    public class TextureCollection
    {
        public Texture2D[] TextureArray = new Texture2D[0];
    }
    
    public void OnEnable()
    {
        
        mColumns = 1920 / TileSize;
        mRows = 1080 / TileSize;
        
        mWorldRect = new Rect( -((TileManager.WorldSize / 2) * TileSize), -((TileManager.WorldSize / 2) *  TileSize), ((TileManager.WorldSize / 2) * TileSize), ((TileManager.WorldSize / 2) * TileSize));
        
        mTerrainTextures.TextureArray = FindTextures("Tiles");
        mPropTextures.TextureArray = FindTextures("Props");
        
        mTextureCollections.Add(mTerrainTextures);
        mTextureCollections.Add(mPropTextures);
        mEmptyTexture.SetPixel(1,1, Color.clear);
        mEmptyTexture.Apply();
        AssetDatabase.Refresh();
    }
    
    public void OnDestroy()
    {
        EditorUtility.SetDirty(mLevelData.gameObject);
        AssetDatabase.Refresh();
    }
    
    public void OnGUI()        
    {
        mMapTileStyle.fixedWidth = mMapTileStyle.fixedHeight = TileSize;
        
        if (mTextureGridStyle == null)
        {
            mTextureGridStyle = new GUIStyle(GUI.skin.button);
            mTextureGridStyle.fixedWidth = mTextureGridStyle.fixedHeight = 136;    
        }
        
        if (mGridStyle == null)
        {
            mGridStyle = new GUIStyle(GUI.skin.button);
        }
        
        if (Event.current.button == 1 && Event.current.type == EventType.mouseDrag)
        {
            if (mWindowRef.position.Contains(Event.current.mousePosition))
            {
                EditorGUIUtility.AddCursorRect(mWindowRef.position, MouseCursor.Pan);
                mCanvasScrollPosition -= Event.current.delta;
            }
        }
        else if (Event.current.type == EventType.mouseMove || Event.current.type == EventType.keyDown)
        {
            mWindowRef.Repaint();
        }    
        
        EditorGUILayout.BeginHorizontal();    
        
        CurrentMap = EditorGUILayout.ObjectField( "Map: ", CurrentMap, typeof(GameObject), false, GUILayout.Width(256)) as GameObject;
        
        if (mLevelData == null && CurrentMap != null)
        {
            mLevelData = CurrentMap.GetComponent<LevelData>();
            
            foreach(LevelData.WorldLayer layer in mLevelData.Layers)
            {
                if (layer.Data.Length < TileManager.WorldSize * TileManager.WorldSize)
                {
                    layer.Data = new Texture2D[TileManager.WorldSize * TileManager.WorldSize];
                }
            }
        }
        
        GUILayout.Label(mCanvasScrollPosition.ToString());
        
        GUILayout.FlexibleSpace();
        
        mPenSizeChoice = GUILayout.Toolbar(mPenSizeChoice, mPenSizeStrings);

        GUILayout.FlexibleSpace();

        if (mLevelData != null)
        {
            mLevelData.Layers[mCurrentLayerIndex].Depth = EditorGUILayout.IntField("Layer Depth: ", mLevelData.Layers[mCurrentLayerIndex].Depth);
        }
        
        GUILayout.FlexibleSpace();

        if (mLevelData != null)
        {
            mLevelData.Layers[mCurrentLayerIndex].Impassable = EditorGUILayout.Toggle("Impassable: ", mLevelData.Layers[mCurrentLayerIndex].Impassable);
        }
        
        GUILayout.FlexibleSpace();
        
        mShowAllLayers = GUILayout.Toggle(mShowAllLayers, "Show All", GUI.skin.button);
        
        EditorGUI.BeginChangeCheck();
        
        mCurrentLayerIndex = GUILayout.Toolbar(mCurrentLayerIndex, new string[]{"Terrain", "Impassable", "Passable", "Overlay"});
        
        GUILayout.FlexibleSpace();
        
        mTextureToolBarSelection = GUILayout.Toolbar(mTextureToolBarSelection, mTextureFolders);
        
        if (EditorGUI.EndChangeCheck())
        {
            mCurrentTextureIndex = 0;
        }
        
        GUILayout.Space(16);
        
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();                
        
            if (mWindowRef == null)
            {
                mWindowRef = EditorWindow.GetWindow<LevelEditorWindow>();
            }            
        
                mCanvasScrollPosition.y = GUILayout.VerticalScrollbar(mCanvasScrollPosition.y, mWindowRef.position.height, mWorldRect.y, mWorldRect.height, GUILayout.Height(mWindowRef.position.height - 24));
            
                EditorGUILayout.BeginVertical();
                GUILayout.BeginScrollView(mCanvasScrollPosition);                
                
                DrawCanvas();                
                
                GUILayout.EndScrollView();
        
                mCanvasScrollPosition.x = GUILayout.HorizontalScrollbar(mCanvasScrollPosition.x, mWindowRef.position.width - 160, mWorldRect.x, mWorldRect.width, GUILayout.Width(mWindowRef.position.width - 160));
                        
                EditorGUILayout.EndVertical();
        
            mTextureScrollPosition = EditorGUILayout.BeginScrollView(mTextureScrollPosition,true,false, GUILayout.Width(160));
                        
            if (mTextureCollections.Count > 0)
            {        
                mCurrentTextureIndex = GUILayout.SelectionGrid(mCurrentTextureIndex, mTextureCollections[mTextureToolBarSelection].TextureArray, 
                                                               1, mTextureGridStyle);
            }
            
            EditorGUILayout.EndScrollView();
        
        EditorGUILayout.EndHorizontal();
        
        if (Event.current.keyCode == KeyCode.W)
        {        
            mCanvasScrollPosition.y -= mScrollSpeed * mDeltaTime;            
        }
        
        if (Event.current.keyCode == KeyCode.S)
        {        
            mCanvasScrollPosition.y += mScrollSpeed * mDeltaTime;            
        }
        
        if (Event.current.keyCode == KeyCode.A)
        {        
            mCanvasScrollPosition.x -= mScrollSpeed * mDeltaTime;            
        }
        
        if (Event.current.keyCode == KeyCode.D)
        {        
            mCanvasScrollPosition.x += mScrollSpeed * mDeltaTime;            
        }
        
        mCanvasScrollPosition.x = Mathf.Clamp(mCanvasScrollPosition.x, mWorldRect.x, mWorldRect.width);
        mCanvasScrollPosition.y = Mathf.Clamp(mCanvasScrollPosition.y, mWorldRect.y, mWorldRect.height);
        
        mDeltaTime = (float)(EditorApplication.timeSinceStartup - mCurrentTime);
        mCurrentTime = EditorApplication.timeSinceStartup;
        
    }
    
    private void DrawCanvas()
    {

        if (CurrentMap == null)
        {
            GUILayout.Label("No Map Selected... ");
            return;
        }
        
        TileManager.TileCoordinate position = new TileManager.TileCoordinate((int)Mathf.Floor(mCanvasScrollPosition.x / TileSize),
                                        (int)Mathf.Floor(mCanvasScrollPosition.y / TileSize));
        
        int colStart = position.x;
        int colEnd = position.x + mColumns;
        
        int rowStart = position.y;
        int rowEnd = position.y + mRows;
        int indexX = 0;
        int indexY = 0;
        
        TileManager.TileCoordinate keyPosition = new TileManager.TileCoordinate(colStart, rowStart);    
        
        Rect mouseTilePos = new Rect();
        Rect tilePos = new Rect();
        //Debug.Log (string.Format("Camera: x: {0} y: {1}, Tiles: x: {2} y: {3}", MainCamera.Instance.transform.localPosition.x, MainCamera.Instance.transform.localPosition.y, keyPosition.x, keyPosition.y));
        
        for(int i = colStart; i < colEnd; i++)
        {
            for (int j = rowStart; j < rowEnd; j++)
            {
                keyPosition = new TileManager.TileCoordinate(i,j);

                tilePos = new Rect((keyPosition.x * 128) - mCanvasScrollPosition.x, (keyPosition.y * 128) - mCanvasScrollPosition.y, 128, 128);
                                    
                indexX = keyPosition.x + (TileManager.WorldSize / 2);
                indexY = keyPosition.y + (TileManager.WorldSize / 2);                
                
                if (tilePos.Contains(Event.current.mousePosition))
                {
                    mouseTilePos = tilePos;
                }
                
                foreach(LevelData.WorldLayer layer in mLevelData.Layers)
                {
                    int dataIndex = indexX * (TileManager.WorldSize) + indexY;
                    
                    if (dataIndex >= layer.Data.Length || layer.Data[dataIndex] == null)
                    {
                        mTempMapTexture = mEmptyTexture;
                    }
                    else
                    {
                        mTempMapTexture = layer.Data[indexX * (TileManager.WorldSize) + indexY];
                    }                    
                    
                    if (layer == mLevelData.Layers[mCurrentLayerIndex])
                    {
                        
                        if (Event.current.keyCode == KeyCode.E && tilePos.Contains(new Vector3(Event.current.mousePosition.x, Event.current.mousePosition.y, 0)))
                        {
                            layer.Data[dataIndex] = null;
                        }

                        if (Event.current.keyCode == KeyCode.P && tilePos.Contains(new Vector3(Event.current.mousePosition.x, Event.current.mousePosition.y, 0)))
                        {
                            mLevelData.PlayerStart = keyPosition;
                        }
                        
                        if (GUI.Button(tilePos, mTempMapTexture, mMapTileStyle))
                        {                            
                            int paintX = indexX - mPenSizes[mPenSizeChoice];
                            int paintY = indexY - mPenSizes[mPenSizeChoice];
                            int endX = indexX + mPenSizes[mPenSizeChoice] + 1;
                            int endY = indexY + mPenSizes[mPenSizeChoice] + 1;
                            
                            for (int x = paintX; x < endX; x++)
                            {
                                for (int y = paintY; y < endY; y++)
                                {                                    
                                    if ( (x * (TileManager.WorldSize) + y) >= layer.Data.Length || (x * (TileManager.WorldSize) + y) < 0)
                                    {
                                        continue;
                                    }
                                    
                                    layer.Data[x * (TileManager.WorldSize) + y] = mTextureCollections[mTextureToolBarSelection].TextureArray[mCurrentTextureIndex];
                                }                                
                            }
//

                        }
                        
                    }
                    else
                    {
                        GUI.color = mShowAllLayers ? Color.white : GrendelColor.CustomAlpha(Color.grey, 0.25f);
                        GUI.DrawTexture(tilePos, mTempMapTexture);
                    }

                    GUI.color = Color.white;
                }
            }
        }

        GUI.color = Color.blue;
        GUI.Box(new Rect((mLevelData.PlayerStart.x * TileSize) - mCanvasScrollPosition.x, (mLevelData.PlayerStart.y * TileSize) - mCanvasScrollPosition.y, TileSize, TileSize), "Player");
        GUI.color = Color.white;

        if (mouseTilePos != new Rect())
        {                            
            GUI.color = GrendelColor.CustomAlpha(Color.green, 0.6f);
            Rect mouseRect = new Rect( mouseTilePos.x - (TileSize * mPenSizes[mPenSizeChoice]), 
                                       mouseTilePos.y - (TileSize * mPenSizes[mPenSizeChoice]),
                                       (TileSize + (TileSize * ( mPenSizes[mPenSizeChoice] * 2) )),
                                       (TileSize + (TileSize * ( mPenSizes[mPenSizeChoice] * 2) )));
            GUI.Box(mouseRect, mTextureCollections[mTextureToolBarSelection].TextureArray[mCurrentTextureIndex]);
            GUI.color = Color.white;
        }

        SerializedObject obj = new SerializedObject(CurrentMap);
        EditorUtility.SetDirty(CurrentMap);
        obj.ApplyModifiedProperties();
        obj.Update();
    }
    
    private void Fill(TileManager.TileCoordinate origin, string textureName)
    {            
        
    }
    
    private Texture2D[] FindTextures(string directory)
    {        
        List<Texture2D> tempList = new List<Texture2D>();
        FileInfo[] ScenesFileInfo = (new DirectoryInfo(Application.dataPath + "/Textures/Environment/" + directory)).GetFiles("*.png", SearchOption.AllDirectories);
                    
        foreach (FileInfo fileInfo in ScenesFileInfo)
        {
            Texture2D tempTexture = AssetDatabase.LoadAssetAtPath("Assets/Textures/Environment/" + directory + "/" + fileInfo.Name, typeof(Texture2D) ) as Texture2D;            
            AssetDatabase.Refresh();            
            if (tempTexture != null)
            {
                    tempList.Add(tempTexture);        
            }
            else
            {
                Debug.LogWarning("Texture File " + fileInfo.ToString() + " not found.");
            }            
        }    
        
        return tempList.ToArray();
    
    }

}

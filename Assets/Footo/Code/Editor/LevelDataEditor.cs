using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(LevelData))]
public class LevelDataEditor : Editor
{	
//	private List<UISprite> CanvasList = new List<UISprite>();
//	private int TileSize = 128;
//	private int mColumns;
//	private int mRows;
//	private GameObject mTileManager;
//	private GameObject mTilePrefab;
//	
//	
//	public LevelData Target
//	{
//		get
//		{
//			return target as LevelData;
//		}
//	}
//	
//	public void OnEnable()
//	{
//		//nasty
//		TileSize = 128;
//		mColumns = (1920 / 128) + 3;
//		mRows = (1080 / 128) + 3;
//		mTileManager = GameObject.Find("Panel");
//		mTilePrefab = (GameObject)Resources.Load("Prefabs/Sprite (green)", typeof(GameObject));
//		
//		for(int i = 0; i < mColumns; i++)
//		{
//			for(int j = 0; j < mRows; j++)
//			{
//				UISprite tile = NGUITools.AddChild(mTileManager, mTilePrefab).GetComponent<UISprite>();
//				tile.transform.localPosition = new Vector3(i * TileSize, j * TileSize, 0);
//				NGUITools.MakePixelPerfect(tile.transform);
//				CanvasList.Add(tile);
//			}
//		}
//	}
//	
//	public void OnDisable()
//	{
//		for(int i = CanvasList.Count - 1; i >= 0; i--)
//		{
//			DestroyImmediate(CanvasList[i]);
//		}
//	}
//	
//	
//	public void OnSceneGUI()
//	{		
//		TileManager.TileCoordinate position = new TileManager.TileCoordinate((int)Mathf.Floor(SceneView.currentDrawingSceneView.camera.transform.position.x / TileSize) - (mColumns / 2), 
//									    (int)Mathf.Floor(SceneView.currentDrawingSceneView.camera.transform.position.y / TileSize) - (mRows / 2));
//		
//		int colStart = position.x;
//		int colEnd = position.x + mColumns;
//		
//		int rowStart = position.y;
//		int rowEnd = position.y + mRows;
//		
//		TileManager.TileCoordinate keyPosition = new TileManager.TileCoordinate(colStart, rowStart);
//		
//		int canvasIndex = 0;
//		
//		//Debug.Log (string.Format("Camera: x: {0} y: {1}, Tiles: x: {2} y: {3}", MainCamera.Instance.transform.localPosition.x, MainCamera.Instance.transform.localPosition.y, keyPosition.x, keyPosition.y));
//		
//		for(int i = colStart; i < colEnd; i++)
//		{
//			for (int j = rowStart; j < rowEnd; j++)
//			{
//				keyPosition = new TileManager.TileCoordinate(i,j);
//				
//				if (!Target.TerrainLayer.ContainsKey(keyPosition))
//				{
//					Target.TerrainLayer.Add(keyPosition, "grassAndDirt");
//				}
//				
//				UISprite sprite;
//					
//				if (canvasIndex > (CanvasList.Count - 1))
//				{
//					UISprite tile = NGUITools.AddChild(mTileManager, mTilePrefab).GetComponent<UISprite>();
//					NGUITools.MakePixelPerfect(tile.transform);
//					CanvasList.Add(tile);
//					sprite = tile;
//				}
//				else
//				{
//					sprite = CanvasList[canvasIndex];
//				}
//				
//				sprite.transform.localPosition = new Vector3((i * TileSize), (j * TileSize), 0);
//				sprite.spriteName = Target.TerrainLayer[keyPosition];
//				NGUITools.MakePixelPerfect(sprite.transform);
//				
//				canvasIndex++;
//			}
//		}
//	}	
	
}

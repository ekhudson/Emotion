using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace GrendelEditor.UI
{

	public static class CustomEditorGUI
	{		
		
		public class ColorGridState
		{
			public bool PickerOpen = false;
		}
		
		public static Color ColorGrid(Vector2 position, int rows, int columns, float gridSize, float gridBuffer, Color[] colors, Color selectedColor)
		{
			return ColorGrid(position, rows, columns, gridSize, gridBuffer, colors, selectedColor, false);
		}
			
		public static Color ColorGridLayout(int rows, int columns, float gridSize, float gridBuffer, Color[] colors, Color selectedColor)
		{
			return ColorGrid(Vector2.zero, rows, columns, gridSize, gridBuffer, colors, selectedColor, true);
		}
		
		private static Color ColorGrid(Vector2 position, int rows, int columns, float gridSize, float gridBuffer, Color[] colors, Color selectedColor, bool isLayout)
		{	
			Rect controlRect = new Rect(position.x, position.y, gridSize, gridSize);
			
			if (isLayout)
			{
				controlRect = GUILayoutUtility.GetRect(gridSize, gridSize, GUI.skin.button);
			}
			
			int id = GUIUtility.GetControlID(FocusType.Passive, controlRect);
			
			ColorGridState controlState = (ColorGridState)GUIUtility.GetStateObject(typeof(ColorGridState), id);				
			
			List<Color> colorList = new List<Color>(colors);			
			
			if (!colorList.Contains(selectedColor))
			{
				selectedColor = colors[0];
			}
			
			GUILayout.BeginVertical();
			
			GUI.color = selectedColor;
			
			if ( !controlState.PickerOpen )
			{
				if (isLayout)
				{
					controlState.PickerOpen = GUILayout.Toggle(controlState.PickerOpen, string.Empty, GUI.skin.button, new GUILayoutOption[]{GUILayout.Width(gridSize), GUILayout.Height(gridSize)});
					controlRect = GUILayoutUtility.GetLastRect();
				}
				else
				{
					controlState.PickerOpen = GUI.Toggle(new Rect(position.x, position.y, gridSize, gridSize), controlState.PickerOpen, string.Empty, GUI.skin.button);
				}			
			}
			else
			{
				if (isLayout)
				{
					GUILayout.Toggle(false, string.Empty, GUI.skin.button, new GUILayoutOption[]{GUILayout.Width(gridSize), GUILayout.Height(gridSize)});
					controlRect = GUILayoutUtility.GetLastRect();				
				}
				else
				{
					GUI.Toggle(new Rect(position.x, position.y, gridSize, gridSize), false, string.Empty, GUI.skin.button);
				}
			}		 
				
			if (controlState.PickerOpen)
			{
				
				GUI.color = Color.white;
				
				Rect pickerRect = new Rect(controlRect.x, controlRect.y + gridSize + gridBuffer, (gridSize * columns) + (gridBuffer * columns + 2), (gridSize * rows) + (gridBuffer * rows + 2));			
				
				GUI.Box(pickerRect, string.Empty);
				
				Event e = Event.current;						
				
				if (e.type == EventType.mouseDown && !pickerRect.Contains(e.mousePosition))
				{
					controlState.PickerOpen = false;
					GUILayout.EndVertical();
					return selectedColor;
				}				
				
				int colorCount = 0;
				
				for(int row = 0; row < rows; row++)
				{
					for (int col = 0; col < columns; col++)
					{
						Color color = Color.black;
							
						if (colorCount > (colors.Length - 1))
						{
							//do nothing, use black
						}
						else
						{
							color = colors[colorCount];		
						}
						
						GUI.color = color;
						GUIStyle style = new GUIStyle(GUI.skin.button);
						
						Rect colorRect = new Rect(pickerRect.x + gridBuffer + (gridSize * col) + (gridBuffer * col), 
							                   pickerRect.y + gridBuffer + (gridSize * row) + (gridBuffer * row), 
							                   gridSize, gridSize);						
						
						if (selectedColor == color)
						{							
							GUI.color = Color.Lerp(Color.white, selectedColor, 0.5f);
							GUI.Button(colorRect, string.Empty, style);
							colorRect = new Rect(colorRect.x + (gridSize * 0.125f), colorRect.y + (gridSize * 0.125f), colorRect.width - (gridSize * 0.25f), colorRect.height - (gridSize * 0.25f));
							GUI.color = color;
						}
						
						 
						
						if(GUI.Button(colorRect, "", style))
						{
							selectedColor = color;
							controlState.PickerOpen = false;
						}	
						
						colorCount++;											
					}				
				}				
			}
					
			GUILayout.EndVertical();		
			
			GUI.color = Color.white;		
			
			return selectedColor;			
		}	
	}
}

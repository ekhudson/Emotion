using UnityEngine;
using System.Collections;

public class MapCamera : BaseObject {

	public int[] CameraSizes; //the orthographic sizes this camera can cycle through
	public int CameraSizeIndex = 5;
	public int MaxSizeThreshold = 75;
	public Vector3 DefaultMaxPosition = new Vector3(0,100,0); //the default position this camera goes to when past max threshold
	
	private Camera _camera; //this camera
	private float _zoomTime = 0.05f; //zoom time between stages
	//private float _currentZoomTime = 0f;
	private Vector3 _followPosition; //position to follow
	private static MapCamera _instance;
	private float _currentSize;
	private bool _isZooming = false;
		
	static public MapCamera Instance
	{
		get {return _instance;}
	}
	
	protected override void Awake ()
	{
		base.Awake();
		_camera = GetComponent<Camera>();
		_instance = this;
	}
	
	// Use this for initialization
	protected override void Start () 
	{
		base.Start();
		
		_currentSize = _camera.orthographicSize = CameraSizes[CameraSizeIndex]; //set initial size
	}
	
	// Update is called once per frame
	public void LateUpdate () 
	{
		if (_currentSize < MaxSizeThreshold)
		{
			_followPosition = GrendelPlayer.Instance.BaseTransform.position;
			_followPosition.y = 100;
			_transform.position = _followPosition;	
		}
		else
		{
			_transform.position = DefaultMaxPosition;
		}
	}
	
	public void ZoomOut()
	{		
		if (!_isZooming)
		{
			StartCoroutine( Zoom(CameraSizes[ (CameraSizeIndex = Mathf.Clamp(++CameraSizeIndex, 0, CameraSizes.Length - 1)) ]) );
		}
	}
	
	public void ZoomIn()
	{			
		if (!_isZooming)
		{
			StartCoroutine ( Zoom(CameraSizes[ (CameraSizeIndex = Mathf.Clamp(--CameraSizeIndex, 0, CameraSizes.Length - 1)) ]) );
		}
	}
	
	public override void ToggleScript()
	{
		if (_camera.enabled) {_camera.enabled = false; Console.Instance.OutputToConsole("MiniMap Disabled", Console.Instance.Style_Admin); }
		else {_camera.enabled = true; Console.Instance.OutputToConsole("MiniMap Enabled", Console.Instance.Style_Admin);}
				
		base.ToggleScript();
	}
	
	IEnumerator Zoom(int target)
	{		
		float amt = (target - _currentSize) * (1 / _zoomTime);
		
		_isZooming = true;
		
		if (amt > 0)
		{		
			while (_currentSize < target)
			{				
				_currentSize += amt * Time.deltaTime;
				camera.orthographicSize = _currentSize;
				yield return new WaitForSeconds(Time.deltaTime);				
			}
		}
		else if (amt < 0)
		{
			while (_currentSize > target)
			{				
				_currentSize += amt * Time.deltaTime;
				camera.orthographicSize = _currentSize;
				yield return new WaitForSeconds(Time.deltaTime);				
			}			
		}
		
		_currentSize = target;
		camera.orthographicSize = _currentSize;
		
		_isZooming = false;		
	}
}

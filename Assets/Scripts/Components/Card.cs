using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour 
{	
	[SerializeField]
	private string _textureAssetBasePath = "assets/textures/cards/";

	[SerializeField]
	private string _textureAssetExt = "png";

	public string TexturePath { get; set; }
	
	public string SourceAssetBundlePath { get; set; }

	public Texture mTexture { get; set; }

	private void Awake()
	{
		this.tag = "Card";	
	}

	public Transform TargetTransform 
	{ 
		get 
		{ 
			if (_targetTransform == null)
			{
				GameObject gameObject = new GameObject(this.name + "Target");
				_targetTransform = gameObject.GetComponent<Transform>();
				_targetTransform.position = transform.position;
				_targetTransform.forward = transform.forward;				
			}
			return _targetTransform; 
		} 
	}		
	private Transform _targetTransform;	

	public CardSlot ParentCardSlot { get; set; }

	public string Description { get; set; }
	
	public string FaceValue 
	{ 
		get { return _faceValue; }
		set
		{
			_faceValue = value;
			TexturePath = _textureAssetBasePath + value + "." + _textureAssetExt;
		}
	}
	private string _faceValue;

	private float _positionDamp = .05f;

	private float _rotationDamp = .05f;   
	
	private void FixedUpdate()
	{
		SmoothToTargetPositionRotation();
	}
	
	public void SetDamp(float newPositionDamp, float newRotationDamp)
	{
		_positionDamp = newPositionDamp;
		_rotationDamp = newRotationDamp;
	}

	public void Toggle()
	{
		Quaternion newRotation = new Quaternion();
		Vector3 ang = TargetTransform.eulerAngles;
		Vector3 eulerAngles = new Vector3(ang.x + 180, ang.y, ang.z);
		newRotation.eulerAngles = eulerAngles;
		TargetTransform.rotation = newRotation;
	}

	private void SmoothToTargetPositionRotation()
	{
		if (TargetTransform.position != transform.position || TargetTransform.eulerAngles != transform.eulerAngles)
		{
			SmoothToPointAndDirection(TargetTransform.position, _positionDamp, TargetTransform.rotation, _rotationDamp);	
		}    	
	}	
    
	private void SmoothToPointAndDirection(Vector3 point, float moveSmooth, Quaternion rotation, float rotSmooth)
	{
		transform.position = Vector3.Lerp( transform.position, point, 1 - Mathf.Exp( -1 * Time.deltaTime )  );
		Quaternion newRotation;
		newRotation.x = Mathf.SmoothDamp(transform.rotation.x, rotation.x, ref _smoothRotationVelocity.x, rotSmooth); 
		newRotation.y = Mathf.SmoothDamp(transform.rotation.y, rotation.y, ref _smoothRotationVelocity.y, rotSmooth); 
		newRotation.z = Mathf.SmoothDamp(transform.rotation.z, rotation.z, ref _smoothRotationVelocity.z, rotSmooth); 		  
		newRotation.w = Mathf.SmoothDamp(transform.rotation.w, rotation.w, ref _smoothRotationVelocity.w, rotSmooth); 		  
		transform.rotation = newRotation;	
		TestVisibility();					     
	}	

	private Vector4 _smoothRotationVelocity = Vector4.zero;	
    
	private void TestVisibility()
	{
		float angle = Vector3.Angle(Camera.main.transform.forward, transform.forward);
		if (angle < 90)
		{
			FrontBecameVisible();
		}
		else
		{
			FrontBecameHidden();
		}
	}

	private void FrontBecameVisible()
	{
		AssetBundle cardBundle = BundleSingleton.Instance.LoadBundle(SourceAssetBundlePath);
		GetComponent<Renderer>().material.mainTexture = cardBundle.LoadAsset<Texture>(TexturePath);
	}
	
	private void FrontBecameHidden()
	{
		Resources.UnloadAsset(GetComponent<Renderer>().material.mainTexture);
		GetComponent<Renderer>().material.mainTexture = null;
	}	
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PokerButtonSlot : MonoBehaviour 
{
	[Range(0.05f, 0.95f)]
	[SerializeField]
	private float _rotationDamp = 0.5f;

	[Range(0.05f, 0.95f)]
	[SerializeField]
	private float _movementDamp = 0.5f;

	public PokerButton Button
	{
		get { return _button; }
		set
		{
			_button = value;

			if (_button.ParentSlot != null)
			{
				_button.ParentSlot.Button = null;
			}
			else
			{
				_button.transform.position = transform.position;
				_button.transform.rotation = transform.rotation;
				_button.SetVisible(true);
			}

			_button.ParentSlot = this;
		}
	}
	private PokerButton _button;

	private Vector4 _rotationVelocity;

	private Vector3 _movementVelocity;

	private void FixedUpdate()
	{
		if (_button != null)
		{
			float posX = _button.transform.position.x; 
			float posY = _button.transform.position.y; 
			float posZ = _button.transform.position.z;
			posX =  Mathf.SmoothDamp(posX, transform.position.x, ref _movementVelocity.x, _movementDamp);
			posY =  Mathf.SmoothDamp(posY, transform.position.y, ref _movementVelocity.y, _movementDamp);
			posZ =  Mathf.SmoothDamp(posZ, transform.position.z, ref _movementVelocity.z, _movementDamp);

			_button.transform.position = new Vector3(posX, posY, posZ);
		
			Quaternion newRotation;
			newRotation.x = Mathf.SmoothDamp(_button.transform.rotation.x, transform.rotation.x, ref _rotationVelocity.x, _rotationDamp); 
			newRotation.y = Mathf.SmoothDamp(_button.transform.rotation.y, transform.rotation.y, ref _rotationVelocity.y, _rotationDamp); 
			newRotation.z = Mathf.SmoothDamp(_button.transform.rotation.z, transform.rotation.z, ref _rotationVelocity.z, _rotationDamp); 		  
			newRotation.w = Mathf.SmoothDamp(_button.transform.rotation.w, transform.rotation.w, ref _rotationVelocity.w, _rotationDamp); 		  
			
			_button.transform.rotation = newRotation;
		}
	}
}

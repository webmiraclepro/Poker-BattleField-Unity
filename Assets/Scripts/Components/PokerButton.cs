using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokerButton : MonoBehaviour
{
    [Range(0.05f, 0.95f)]
    [SerializeField]
    private float _rotationDamp = 0.5f;

    [Range(0.05f, 0.95f)]
    [SerializeField]
    private float _movementDamp = 0.5f;

    public PokerButtonSlot ParentSlot
    { 
        get
        {
            return _parentSlot;
        } 
        set
        {
            _parentSlot = value;

            if (value != null)
            {
                SetVisible(true);
            }
        }
    }
    private PokerButtonSlot _parentSlot;

    private Vector4 _rotationVelocity;

    private Vector3 _movementVelocity;

    private void Awake()
    {
        SetVisible(false);
    }

    public void SetVisible(bool visibility)
    {
        foreach(MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>())
        {
            renderer.enabled = visibility;
        }
    }

    private void FixedUpdate()
    {
        if (_parentSlot != null)
        {
            float posX = transform.position.x; 
            float posY = transform.position.y; 
            float posZ = transform.position.z;
            posX =  Mathf.SmoothDamp(posX, _parentSlot.transform.position.x, ref _movementVelocity.x, _movementDamp);
            posY =  Mathf.SmoothDamp(posY, _parentSlot.transform.position.y, ref _movementVelocity.y, _movementDamp);
            posZ =  Mathf.SmoothDamp(posZ, _parentSlot.transform.position.z, ref _movementVelocity.z, _movementDamp);

            transform.position = new Vector3(posX, posY, posZ);
        
            Quaternion newRotation;
            newRotation.x = Mathf.SmoothDamp(transform.rotation.x, _parentSlot.transform.rotation.x, ref _rotationVelocity.x, _rotationDamp); 
            newRotation.y = Mathf.SmoothDamp(transform.rotation.y, _parentSlot.transform.rotation.y, ref _rotationVelocity.y, _rotationDamp); 
            newRotation.z = Mathf.SmoothDamp(transform.rotation.z, _parentSlot.transform.rotation.z, ref _rotationVelocity.z, _rotationDamp);         
            newRotation.w = Mathf.SmoothDamp(transform.rotation.w, _parentSlot.transform.rotation.w, ref _rotationVelocity.w, _rotationDamp);         
            
            transform.rotation = newRotation;
        }
    }
}

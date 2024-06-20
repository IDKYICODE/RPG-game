using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;//pinpoints the player
    public float smoothing;// how fast the camera moves
    public Vector2 maxPosition;
    public Vector2 minPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()//lateupdate ensures that the camera only tracks the player upon movement
    {
        if(transform.position != target.position)
        {
            Vector3 targetPosition = new Vector3(target.position.x,target.position.y,transform.position.z);
            targetPosition.x = Mathf.Clamp(targetPosition.x,minPosition.x,maxPosition.x);//Clamp function is used to set the max limit of the camera
            targetPosition.y = Mathf.Clamp(targetPosition.y,minPosition.y,maxPosition.y);
            transform.position = Vector3.Lerp(transform.position,targetPosition, smoothing);//Lerp takes current position and the final position and the amount of camera movement as parameters.

     }
    }
}

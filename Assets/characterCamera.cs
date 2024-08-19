using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterCamera : MonoBehaviour
{

public Transform playerCharacter;
 public Vector3 cameraOffset;
  

    // Update is called once per frame
    void Update()
    {
        //update character coordinates to character
         transform.position = new Vector3 (playerCharacter.position.x + cameraOffset.x, playerCharacter.position.y + cameraOffset.y, cameraOffset.z); // Camera follows the player with specified offset position
    }
}

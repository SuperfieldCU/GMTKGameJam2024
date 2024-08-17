using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerInput : MonoBehaviour
{
    [SerializeField] private characterMovement characterMovement;
        float horizontalMove;
        float verticalMove;
    // Start is called before the first frame update
    private void Awake()
    {
        characterMovement = GetComponent<characterMovement>();    
    } 
    

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        characterMovement.Move(horizontalMove, verticalMove, false);
    }
}

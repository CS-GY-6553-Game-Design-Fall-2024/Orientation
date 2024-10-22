using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepController : MonoBehaviour
{
    public GameObject footstep;
    [SerializeField] private FirstPersonMovement playerMovement;

    // Update is called once per frame
    void Update()
    {
        Debug.Log("move");
        bool isMoving = playerMovement.IsPlayerMoving();

        if (isMoving)
        {
            
            Footsteps();
        }
        else {
            StopFootsteps();
        }
    }

    void Footsteps() { 
        footstep.SetActive(true);
    }

    void StopFootsteps() {
        footstep.SetActive(false);
    }
}

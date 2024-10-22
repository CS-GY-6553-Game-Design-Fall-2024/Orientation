using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour
{
    [SerializeField] private Collider m_collider;
    [SerializeField] private GameManager m_gameManager;

    private void Awake() {
        m_collider.isTrigger = true;        
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Hooray!");
        m_gameManager.ShowWinMenu();
    }
}

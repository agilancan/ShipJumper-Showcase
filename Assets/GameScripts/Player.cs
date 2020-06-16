using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private NodeBehaviour nodeBehaviour;

    private void Start()
    {
        nodeBehaviour = GetComponent<NodeBehaviour>();
    }

    private void Update()
    {
        
    }
}

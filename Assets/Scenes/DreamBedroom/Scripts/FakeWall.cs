using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeWall : MonoBehaviour
{ 

    public void OnInteract()
    {
        Destroy(gameObject);
    }
}
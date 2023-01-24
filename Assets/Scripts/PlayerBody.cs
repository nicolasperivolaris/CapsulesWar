using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : MonoBehaviour
{    
    public void Touched(Shot shot)
    {
        Debug.Log("Touched");
    }

    public void Touched(GameObject o)
    {
        Debug.Log("Touched");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteMe : MonoBehaviour
{
    // Start is called before the first frame update

    public float deleteTime = 1.7f;

    void Start()
    {
        Destroy(this.gameObject, deleteTime); 
    }
}

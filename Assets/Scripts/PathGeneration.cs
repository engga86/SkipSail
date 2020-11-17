using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGeneration : MonoBehaviour
{
    public GameObject pathPiece;
    public Transform threshold;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z < threshold.position.z)
        {
            Instantiate(pathPiece, transform.position, transform.rotation);
            transform.position += new Vector3(0f, 0f, 36f);
        }
    }
}

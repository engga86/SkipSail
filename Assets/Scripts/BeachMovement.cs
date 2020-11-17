using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeachMovement : MonoBehaviour
{
    public Transform disappearPoint;

    public Collider theCol;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager._canMove)
        {
            transform.position -= new Vector3(0f, 0f, GameManager._worldSpeed * Time.deltaTime);
        }

        if(transform.position.z < disappearPoint.position.z)
        {
            transform.position += new Vector3(0f, 0f, theCol.bounds.size.z * 2);
        }

    }
}

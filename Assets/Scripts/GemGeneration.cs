using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemGeneration : MonoBehaviour
{

    public float gemGenertionTime;

    private float gemGenCounter;

    public GameObject[] gemGroups;

    public Transform topPos;

    // Start is called before the first frame update
    void Start()
    {
        gemGenCounter = gemGenertionTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager._canMove)
        {
            gemGenCounter -= Time.deltaTime;

            if (gemGenCounter <= 0)
            {
                bool goTop = Random.value > .5f;

                int pickedObstacles = Random.Range(0, gemGroups.Length);

                if (goTop)
                {
                    Instantiate(gemGroups[pickedObstacles], topPos.position, transform.rotation);
                }
                else
                {
                    Instantiate(gemGroups[pickedObstacles], transform.position, transform.rotation);

                }
                gemGenCounter = Random.Range(gemGenertionTime * 0.75f, gemGenertionTime * 1.25f);
            }
        }
    }
}

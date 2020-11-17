using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGeneration : MonoBehaviour
{
    public GameObject[] obstacles;

    public float timeBetweenObstacles;
    private float obstaclesGenCounter;

    public GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        obstaclesGenCounter = timeBetweenObstacles;
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.canMove)
        {
            obstaclesGenCounter -= Time.deltaTime;

            if (obstaclesGenCounter <= 0)
            {
                int pickedObstacles = Random.Range(0, obstacles.Length);
                Instantiate(obstacles[pickedObstacles], transform.position, Quaternion.Euler(0f, Random.Range(-45, 45f), 0f));

                obstaclesGenCounter = Random.Range(timeBetweenObstacles * 0.75f, timeBetweenObstacles * 1.25f);

                //increase difficulty
                obstaclesGenCounter = obstaclesGenCounter / gm.speedMultiplier;
            }
        }
    }
}

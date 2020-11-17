using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;


public class GameManager : MonoBehaviour
{
    public bool canMove;
    static public bool _canMove;

    public float worldSpeed;
    static public float _worldSpeed;

    public int gemCollected;

    private bool gameStarted;

    public float timeToIncreaseSpeed;
    private float increaseSpeedCounter;
    public float speedMultiplier;
    private float targetSpeedMultiplier;
    public float acceleration;
    private float accelerationStore;
    public float speedIncreaseAmount;
    private float worldSpeedStore;

    public GameObject tapMessage;
    public Text gemText;

    public Text disText;
    private float distanceMoved;

    public GameObject deathScreen;
    public Text deathScreenGems;
    public Text deathScreenDis;

    public float deathScreenDelay;

    public string mainMenu;

    public GameObject noGemsScreen;

    public PlayerControl player;

    public GameObject pauseScreen;

    public GameObject[] models;
    public GameObject defaultShip;

    public AudioManager audioM;

    public Text updateGemText;
    public Text updateGemText2;
    public GameObject adRewardPanel;
    public GameObject adRewardPanel2;
    public Text rewardText;
    public Text rewardText2;



    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("GemsCollected"))
        {
            gemCollected = PlayerPrefs.GetInt("GemsCollected");
        }

        increaseSpeedCounter = timeToIncreaseSpeed;

        targetSpeedMultiplier = speedMultiplier;
        worldSpeedStore = worldSpeed;
        accelerationStore = acceleration;

        gemText.text = "Gems: " + gemCollected;
        disText.text = distanceMoved + "m";

        //load correct model ship
        for(int i = 0; i < models.Length; i++)
        {
            if(models[i].name == PlayerPrefs.GetString("SelectedShip"))
            {
                GameObject clone = Instantiate(models[i], player.modleHolder.position, player.modleHolder.rotation);
                clone.transform.parent = player.modleHolder;
                defaultShip.SetActive(false);
            }
        }

        //PlayGamesPlatform.Activate();
    }

    // Update is called once per frame
    void Update()
    {
        _canMove = canMove;
        _worldSpeed = worldSpeed;

        if (!gameStarted && Input.GetMouseButtonDown(0))
        {
            canMove = true;
            _canMove = true;

            gameStarted = true;

            tapMessage.SetActive(false);

        }

        //increase speed over time.
        if (canMove)
        {
            increaseSpeedCounter -= Time.deltaTime;
            if(increaseSpeedCounter <= 0)
            {
                increaseSpeedCounter = timeToIncreaseSpeed;

                //worldSpeed = worldSpeed * speedMultiplier;

                targetSpeedMultiplier = targetSpeedMultiplier * speedIncreaseAmount;
                timeToIncreaseSpeed = timeToIncreaseSpeed * .97f;
            }
            acceleration = accelerationStore * speedMultiplier;

            speedMultiplier = Mathf.MoveTowards(speedMultiplier, targetSpeedMultiplier, acceleration * Time.deltaTime);
            worldSpeed = worldSpeedStore * speedMultiplier;

            //update UI distance
            distanceMoved += Time.deltaTime * worldSpeed;
            disText.text = Mathf.Floor(distanceMoved) + "m";

        }

        updateGemText.text = "Gems: " + gemCollected;
        updateGemText2.text = gemCollected + " Gems!";




    }

    public void HitObstacle()
    {
        canMove = false;
        _canMove = false;

        PlayerPrefs.SetInt("GemsCollected", gemCollected);

        
        deathScreenGems.text = gemCollected + " Gems!";
        deathScreenDis.text = Mathf.Floor(distanceMoved) + "m!";


        StartCoroutine("ShowDeathMenu");
    }

    public IEnumerator ShowDeathMenu()
    {
        audioM.StopMusic();

        yield return new WaitForSeconds(deathScreenDelay);
        deathScreen.SetActive(true);

        audioM.gameOverMusic.Play();
    }

    public void AddGem()
    {
        gemCollected ++;

        gemText.text = "Gems: " + gemCollected;
    }

    public void ContinueGame()
    {
        if (gemCollected >= 100)
        {
            gemCollected -= 100;

            canMove = true;
            _canMove = true;

            deathScreen.SetActive(false);

            player.RestPlayer();

            audioM.StopMusic();
            audioM.gameMusic.Play();
        }
        else
        {
            noGemsScreen.SetActive(true);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GetGems()
    {
        ShowRewardedVideo();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenu);

        Time.timeScale = 1f;
    }

    public void CloseWhenNoGems()
    {
        noGemsScreen.SetActive(false);
    }

    public void Resume()
    {
        pauseScreen.SetActive(false);

        Time.timeScale = 1f;
    }

    public void Pause()
    {
        if(Time.timeScale == 1f)
        {
            pauseScreen.SetActive(true);

            Time.timeScale = 0f;
        }
        else
        {
            pauseScreen.SetActive(false);

            Time.timeScale = 1f;
        }

    }


    void ShowRewardedVideo()
    {
        ShowOptions options = new ShowOptions();
        options.resultCallback = HandleShowResult;

        Advertisement.Show("rewardedVideo", options);
        noGemsScreen.SetActive(false);
    }

    void HandleShowResult(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            Debug.Log("Video completed - Offer a reward to the player");

            //reward
            gemCollected += 100;
            PlayerPrefs.SetInt("GemsCollected", gemCollected);

            rewardText.text = "Thanks for watching. 100 Gems has added.";
            rewardText2.text = "Thanks for watching. 100 Gems has added.";
        }

        else if (result == ShowResult.Skipped)
        {
            Debug.Log("Video was skipped- Do not reward the player");
            rewardText.text = "You skipped the ad. Watch another to get gems";
            rewardText2.text = "You skipped the ad. Watch another to get gems";

        }

        else if (result == ShowResult.Failed)
        {
            Debug.LogError("Video failed to show");
            rewardText.text = "Unable to load. Please try again.";
            rewardText2.text = "Unable to load. Please try again.";
        }

        adRewardPanel.SetActive(true);
        adRewardPanel2.SetActive(true);
    }

    public void CloseAdPanel()
    {
        adRewardPanel.SetActive(false);
        adRewardPanel2.SetActive(false);
    }

}

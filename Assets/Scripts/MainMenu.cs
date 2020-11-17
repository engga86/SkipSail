using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class MainMenu : MonoBehaviour
{

    public string levelToLoad;

    public GameObject mainScreen;
    public GameObject switchScreen;

    public Transform cam;

    public Transform shipSwitchHolder;

    private Vector3 camTargetPos;
    public float camSpeed;

    public GameObject[] shipNumber;
    public int currentShips;

    public GameObject switchPlayButton;
    public GameObject switchUnlockButton;
    public GameObject switchGetGemsButton;

    public GameObject lockImage;

    public int currentGems;

    public Text gemText;
    public GameObject adRewardPanel;
    public Text rewardText;

    // Start is called before the first frame update
    void Start()
    {
        camTargetPos = cam.position;

        if (!PlayerPrefs.HasKey(shipNumber[0].name))
        {
            PlayerPrefs.SetInt(shipNumber[0].name, 1);
        }


        if (PlayerPrefs.HasKey("GemsCollected"))
        {
            currentGems = PlayerPrefs.GetInt("GemsCollected");
        }
        else
        {
            PlayerPrefs.SetInt("GemsCollected", 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        cam.position = Vector3.Lerp(cam.position, camTargetPos, camSpeed * Time.deltaTime);

        gemText.text = "Gems: " + currentGems;

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.L))
        {
            for (int i = 1; i < shipNumber.Length; i++)
            {
                PlayerPrefs.SetInt(shipNumber[i].name, 0);
            }
        }

        UnlockedCheck();

#endif
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    public void ChooseShip()
    {
        mainScreen.SetActive(false);
        switchScreen.SetActive(true);

        camTargetPos = cam.position + new Vector3(0f, shipSwitchHolder.position.y, 0f);
        UnlockedCheck();
    }

    public void MoveLeft()
    {
        if(currentShips > 0)
        {
            camTargetPos += new Vector3(0f, 0f, 8f);

            currentShips--;
            UnlockedCheck();
        }
    }

    public void MoveRight()
    {
        if(currentShips <= shipNumber.Length - 2)
        {
            camTargetPos -= new Vector3(0f, 0f, 8f);

            currentShips++;
            UnlockedCheck();
        }
    }

    public void UnlockedCheck()
    {
        if (PlayerPrefs.HasKey(shipNumber[currentShips].name))
        {
            if(PlayerPrefs.GetInt(shipNumber[currentShips].name) == 0)
            {
                switchPlayButton.SetActive(false);

                lockImage.SetActive(true);

                if(currentGems < 1000)
                {
                    switchGetGemsButton.SetActive(true);
                    switchUnlockButton.SetActive(false);
                }
                else
                {
                    switchUnlockButton.SetActive(true);
                    switchGetGemsButton.SetActive(false);
                }
            }
            else
            {
                switchPlayButton.SetActive(true);

                lockImage.SetActive(false);

                switchGetGemsButton.SetActive(false);
                switchUnlockButton.SetActive(false);
            }
        }

        else
        {
            PlayerPrefs.SetInt(shipNumber[currentShips].name, 0);

            UnlockedCheck();
        }
    }

    public void UnlockShip()
    {
        currentGems -= 1000;

        PlayerPrefs.SetInt(shipNumber[currentShips].name, 1);
        PlayerPrefs.SetInt("GemsCollected", currentGems);

        UnlockedCheck();
    }

    public void SelectShip()
    {
        PlayerPrefs.SetString("SelectedShip", shipNumber[currentShips].name);

        PlayGame();
    }

    public void GetGems()
    {
        ShowRewardedVideo();
    }

    void ShowRewardedVideo()
    {
        ShowOptions options = new ShowOptions();
        options.resultCallback = HandleShowResult;

        Advertisement.Show("rewardedVideo", options);
    }

    void HandleShowResult(ShowResult result)
    {
        if(result == ShowResult.Finished)
        {
            Debug.Log("Video completed - Offer a reward to the player");

            //reward
            currentGems += 100;
            PlayerPrefs.SetInt("GemsCollected", currentGems);

            rewardText.text = "Thanks for watching. 100 Gems has added.";
        }

        else if(result == ShowResult.Skipped)
        {
            Debug.Log("Video was skipped- Do not reward the player");
            rewardText.text = "You skipped the ad. Watch another to get gems";

        }

        else if(result == ShowResult.Failed)
        {
            Debug.LogError("Video failed to show");
            rewardText.text = "Unable to load. Please try again.";
        }

        adRewardPanel.SetActive(true);
    }

    public void CloseAdPanel()
    {
        adRewardPanel.SetActive(false);
    }
}

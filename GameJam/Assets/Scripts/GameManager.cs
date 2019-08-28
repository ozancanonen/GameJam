using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject endOfLevelMenuUI;
    public GameObject Player1HasWonText;
    public GameObject Player2HasWonText;
    public GameObject fireCollider;
    public GameObject flameParticle;
    public GameObject layer2Light;
    public GameObject particleParentObject;

    public AudioManager audioManager;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public IEnumerator footStepSounds(string sound1, string sound2, string sound3,int numberOfRepetition)
    {
        string[] currentSound = { sound1 , sound2 , sound3 };

        for (int i = 0; i <2; i++)
        {
            //int a = Random.Range(0, 3);
            audioManager.Play(currentSound[i]);
            yield return new WaitForSeconds(0.5f);
        }
    }



    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        StartCoroutine(footStepSounds("Wood1", "Wood2"," Wood3", 3
            ));
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void PlayerIsDeath(string playerTag)
    {
        Time.timeScale = 0f;
        if (playerTag == "Player1")
        {
            Player2HasWonText.SetActive(true);
        }
        if (playerTag == "Player2")
        {
            Player1HasWonText.SetActive(true);
        }
        endOfLevelMenuUI.SetActive(true);
    }

    public IEnumerator DestroyThisAFter(GameObject thisObject, float destroyAfter)
    {
        yield return new WaitForSeconds(destroyAfter);
        Destroy(thisObject);
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<DataPlayer> dataPlayers;

    public List<Button> listButtonChoose;

    public TextMeshProUGUI tmp;

    public Player player1;

    public Player player2;

    public ChunkSpawner chunkSpawner;

    public GameObject uiChoosePlayer;

    public Text countdownText;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Start()
    {
        uiChoosePlayer.SetActive(true);

        listButtonChoose.ForEach(x =>
        {
            x.onClick.AddListener(() => OnChoose(x));
        });

        chunkSpawner.IsUpdateChunk = false;

        tmp.text = "Choose Character 1";

    }

    private int countChoose;  


    private void OnChoose(Button button)
    {
        countChoose++;

        if(countChoose == 1)
        {
            tmp.text = "Choose Character 2";

            int indexCharacter = int.Parse(button.name);

           
            player1.spriteCharacter.sprite = dataPlayers[indexCharacter].sprite;
            player1.animator.runtimeAnimatorController = dataPlayers[indexCharacter].animator;
        }
        else if(countChoose == 2)
        {
            int indexCharacter = int.Parse(button.name);

            player2.spriteCharacter.sprite = dataPlayers[indexCharacter].sprite;
            player2.animator.runtimeAnimatorController = dataPlayers[indexCharacter].animator;

            StartCoroutine(IEChooseCharacterCompleted());
        }

        Destroy(button.transform.parent.gameObject);
    }

    private IEnumerator IEChooseCharacterCompleted()
    {
        yield return new WaitForSeconds(0.1f);

        uiChoosePlayer.SetActive(false);

        countdownText.gameObject.SetActive(true);
        for (int i = 1; i > 0; i--)
        {
            yield return new WaitForSecondsRealtime(1f);
        }

        countdownText.text = "GO!";
        yield return new WaitForSecondsRealtime(1f);

        countdownText.gameObject.SetActive(false);

        player1.IsGameStarted = true;
        player2.IsGameStarted = true;
        chunkSpawner.IsUpdateChunk = true;
    }

    bool isFailed = false;
    public void RestartGame()
    {
        if (isFailed)
            return;

        isFailed = true;

        StartCoroutine(IEReStartGame());
    }

    private IEnumerator IEReStartGame()
    {

        player1.IsGameStarted = false;
        player2.IsGameStarted = false;
        chunkSpawner.IsUpdateChunk = false;

        countdownText.gameObject.SetActive(true);
        countdownText.text = "Failed";

        yield return new WaitForSecondsRealtime(2f);
        SceneManager.LoadScene("Map2Player");
    }
}


[Serializable]
public class DataPlayer
{
    public RuntimeAnimatorController animator;
    public Sprite sprite;
}

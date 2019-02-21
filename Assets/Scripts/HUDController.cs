using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUDController : MonoBehaviour
{
    public Image redHealth;
    public Image blueHealth;
    public Slider simulationSpeed;
    public Button newButton;
    public Button loadButton;
    public Button saveButton;

    public Image popUp;
    public Text winnerText;
    public Text simulationTime;
    public Button againButton;


    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        newButton.onClick.AddListener(StartNew);
        loadButton.onClick.AddListener(Load);
        saveButton.onClick.AddListener(Save);
        againButton.onClick.AddListener(StartNew);

        
    }

    private void Update()
    {
        if (gameManager.GetFirstTeamCount() + gameManager.GetSecondTeamCount() != 0)
        {
            redHealth.rectTransform.sizeDelta = new Vector2((Screen.width - 15) * gameManager.GetFirstTeamCount() / (gameManager.GetFirstTeamCount() + gameManager.GetSecondTeamCount()), 20);
            blueHealth.rectTransform.sizeDelta = new Vector2((Screen.width - 15) * gameManager.GetSecondTeamCount() / (gameManager.GetFirstTeamCount() + gameManager.GetSecondTeamCount()), 20);
        }
        if (simulationSpeed.enabled) Time.timeScale = simulationSpeed.value;
    }

    void StartNew()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Load()
    {
        gameManager.LoadState();
    }

    void Save()
    {
        gameManager.SaveState();
    }

    public void ShowEndingPopUp(Team winner)
    {
        simulationSpeed.enabled = false;
        popUp.gameObject.SetActive(true);
        simulationTime.text = "Simulation Time: " + gameManager.GetSimulationTime();
        winnerText.text = "Winner Team : " + winner.name;
        popUp.color = winner.color;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class GameManager : MonoBehaviour
{
    Transform areaTransform;
    GameObject circlePrefab;
    GameObject HUD;
    public List<Team> teams = new List<Team>();

    GameConfig gameConfig;
    SpriteRenderer areaSprite;
    [HideInInspector] public List<Circle> circlesList = new List<Circle>(); //list with all circle in game
    //place for spawning
    float spawnRangeX;
    float spawnRangeY;

    [HideInInspector] public int countFirstTeam = 0;
    [HideInInspector] public int countSecondTeam = 0;

    [HideInInspector]  public float tempTime;
    [HideInInspector]  public float time;
    [HideInInspector]  public int spawning = 1;

    SavedState saveData;
    void Awake()
    {
        //  Assets / Prefabs / Circle.prefab
        circlePrefab = Resources.Load("Prefabs/Circle") as GameObject;    
        areaTransform = GameObject.Find("Area").transform;
        HUD = GameObject.Find("HUD");

        SetConfig("data");

        //Set area and camera size 
        areaTransform.localScale = new Vector2(gameConfig.gameAreaWidth, gameConfig.gameAreaHeight);
        Camera.main.GetComponent<CameraController>().SetCameraSize(areaTransform);

        areaSprite = areaTransform.gameObject.GetComponent<SpriteRenderer>();
        spawnRangeX = areaSprite.bounds.extents.x - gameConfig.maxUnitRadius;
        spawnRangeY = areaSprite.bounds.extents.y - gameConfig.maxUnitRadius;
    }

    private void Update()
    {
        time += Time.deltaTime; // Common timer for simulation 

        SpawnCircles();
        StartSimulation();
        Ending();
    }

    //Spawning circles
    void SpawnCircles()
    {
        tempTime += Time.deltaTime * spawning;
        if (tempTime >= gameConfig.unitSpawnDelay)
        {
            tempTime = 0;
            circlesList.Add(SpawnObject(teams[Random.Range(0, teams.Capacity)]));
        }
    }

    //Start simulation
    private void StartSimulation()
    {
        if (countFirstTeam + countSecondTeam == gameConfig.numUnitsToSpawn)
        {
            spawning = 0;
            foreach (var circle in GameObject.FindGameObjectsWithTag("circle"))
            {
                circle.GetComponent<CircleController>().StartMoving();
            }
        }
    }

    //Ending
    void Ending()
    {
        if (time >= gameConfig.numUnitsToSpawn * gameConfig.unitSpawnDelay)
        {
            if (countFirstTeam.Equals(0))
            {
                HUD.GetComponent<HUDController>().ShowEndingPopUp(teams[1]);
            }
            if (countSecondTeam.Equals(0))
            {
                HUD.GetComponent<HUDController>().ShowEndingPopUp(teams[0]);
            }
            Time.timeScale = 0;
        }
    }

    //Load and set config
    private void SetConfig(string configResource)
    {
        TextAsset JSONConfig = (TextAsset)Resources.Load(configResource);
        gameConfig = JsonUtility.FromJson<GameConfig>(JSONConfig.ToString());
    }
    
    public void SaveState()
    {
        saveData = new SavedState();
        saveData.SetData (circlesList,  countFirstTeam,  countSecondTeam,  tempTime,  time,  spawning);
        PlayerPrefs.SetString("state", JsonUtility.ToJson(saveData));
    }

    public void LoadState()
    {
        SavedState loadedState = JsonUtility.FromJson<SavedState>(PlayerPrefs.GetString("state"));
        //Set GameManager state from savedata
        countFirstTeam = loadedState.countFirstTeam;
        countSecondTeam = loadedState.countSecondTeam;
        tempTime = loadedState.tempTime;
        time = loadedState.time;
        spawning = loadedState.spawning;
        //Delete all circle on area
        foreach (var tempCircle in GameObject.FindGameObjectsWithTag("circle"))
        {
            Destroy(tempCircle);
        }
        //Spawn circle from save
        circlesList = new List<Circle>();
        foreach (var savedCircle in loadedState.circlesList)
        {
            circlesList.Add(SpawnObject(savedCircle));
        }
    }
    //Standart spawning
    Circle SpawnObject(Team team)
    {
        switch (team.name)
        {
            case "red":
                countFirstTeam++;
                break;
            case "blue":
                countSecondTeam++;
                break;
        }
        GameObject circle = Instantiate(circlePrefab, 
                                        new Vector2(Random.Range(-spawnRangeX,spawnRangeX), Random.Range(-spawnRangeY, spawnRangeY)),
                                        Quaternion.identity) as GameObject;
        circle.GetComponent<CircleController>().InitCircle(team,
                                                            Random.Range(gameConfig.minUnitSpeed,gameConfig.maxUnitSpeed),
                                                            Random.insideUnitCircle.normalized, 
                                                            Random.Range(gameConfig.minUnitRadius,gameConfig.maxUnitRadius),
                                                            areaSprite,0f);
        return circle.GetComponent<CircleController>().circle;
    }
    //Spawn from saved data
    Circle SpawnObject(Circle savedCircle)
    {
        GameObject circle = Instantiate(circlePrefab,
                                        new Vector2(savedCircle.position.x, savedCircle.position.y),
                                        Quaternion.identity) as GameObject;
        circle.GetComponent<CircleController>().InitCircle(savedCircle.team,
                                                           savedCircle.baseSpeed,
                                                           savedCircle.direction,
                                                           savedCircle.scale.x,
                                                           areaSprite,savedCircle.currentSpeed);
        return circle.GetComponent<CircleController>().circle;
    }

    public List<Circle> GetCirclesList()
    {
        return circlesList;
    }

    public int GetFirstTeamCount()
    {
        return countFirstTeam;
    }

    public void KillFirstTeamCircle()
    {
        this.countFirstTeam--;
    }

    public int GetSecondTeamCount()
    {
        return countSecondTeam;
    }

    public void KillSecondTeamCircle()
    {
        this.countSecondTeam--;
    }

    public float GetSimulationTime()
    {
        return time;
    }
}

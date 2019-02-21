using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Simple class for storing saved data
[System.Serializable]
public class SavedState 
{
    [HideInInspector] public List<Circle> circlesList = new List<Circle>(); //list with all circle in game
    [HideInInspector] public int countFirstTeam = 0;
    [HideInInspector] public int countSecondTeam = 0;

    [HideInInspector] public float tempTime;
    [HideInInspector] public float time;
    [HideInInspector] public int spawning = 1;

    public void SetData(List<Circle> circlesList, int countFirstTeam, int countSecondTeam, float tempTime, float time, int spawning)
    {
        this.circlesList = circlesList;
        this.countFirstTeam = countFirstTeam;
        this.countSecondTeam = countSecondTeam;
        this.tempTime = tempTime;
        this.time = time;
        this.spawning = spawning;
    }
}

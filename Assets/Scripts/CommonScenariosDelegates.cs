using UnityEngine;

public class CommonScenariosDelegates : MonoBehaviour
{
    public delegate void Level_1Scene();

    public Level_1Scene bossStartStep;
    public Level_1Scene finalBatteryStep;

    
    private static ScenariosState nextScenariosState;
    private enum ScenariosState
    {
        NONE,
        START_LEVEL,
        BOSS_START,
        FINAL_BATTERY,
        END_LEVEL_1
    }

    private void Start()
    {
        nextScenariosState = ScenariosState.START_LEVEL;

        bossStartStep += NextScenarioStep;
        finalBatteryStep += NextScenarioStep;
    }

    private void NextScenarioStep()
    {
        nextScenariosState++;
    }
}

using UnityEngine;

public class CommonScenariosDelegates : MonoBehaviour
{
    public delegate void Level_1Scene();

    public Level_1Scene bossStartStep;
    public Level_1Scene firstShakeStartStep;
    public Level_1Scene firstShakeStopStep;
    public Level_1Scene finalBatteryStep;
    public Level_1Scene secondShakeStartStep;
    public Level_1Scene secondShakeStopStep;

    private static ScenariosState nextScenariosState;
    private enum ScenariosState
    {
        START_LEVEL,
        BOSS_START,
        FIRST_SHAKE_START,
        FIRST_SHAKE_STOP,
        FINAL_BATTERY,
        SECOND_SHAKE_START,
        SECOND_SHAKE_STOP,
        END_LEVEL_1
    }

    private void Start()
    {
        nextScenariosState = ScenariosState.START_LEVEL;

        bossStartStep += NextScenarioStep;
        finalBatteryStep += NextScenarioStep;
        secondShakeStartStep += NextScenarioStep;
    }

    private void NextScenarioStep()
    {
        nextScenariosState++;
    }
}

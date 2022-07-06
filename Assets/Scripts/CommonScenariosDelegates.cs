using UnityEngine;

public class CommonScenariosDelegates : MonoBehaviour
{
    delegate void FinalBattery();
    delegate void BossStart();
    delegate void EndLevel();

    FinalBattery finalBattery;

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
        finalBattery += NextScenarioStep;
    }

    private void NextScenarioStep()
    {
        nextScenariosState++;
    }
}

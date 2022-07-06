using UnityEngine;

public class CommonScenariosDelegates : MonoBehaviour
{
    public delegate void Level_1Scene();

    public Level_1Scene bossStartStep;
    public Level_1Scene firstShakeStartStep;
    public Level_1Scene firstShakeStopStep;
    
    public Level_1Scene secondShakeStartStep;
    public Level_1Scene secondShakeStopStep;
    public Level_1Scene finalBatteryStep;
    public Level_1Scene finalShotStep;
}

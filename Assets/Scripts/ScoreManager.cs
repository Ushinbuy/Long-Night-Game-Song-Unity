using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private Animator animator;

    private readonly float scoreScaleX = 1.4f;
    private readonly float scoreScaleMinY = -0.01f;
    private readonly float scoreScaleMaxY = 0.3f;
    private readonly float scoreScaleMaxRotationZ = 24f;
    private readonly float updateTime = 0.3f;
    private float startZposition;

    public static ScoreAnimatorState scoreAnimatorState;
    float scoreScaleUnitX, scoreScaleUnitY, scoreScaleUnitRotateZ;

    private static int score = 50;
    public static bool isDopelgangerWeen;
    public static bool wasDamagedOneTime;

    public enum ScoreAnimatorState
    {
        NONE,
        DECREASE,
        INCREASE
    }

    void Start()
    {
        wasDamagedOneTime = false;
        scoreScaleUnitX = scoreScaleX * 2 / 100;
        scoreScaleUnitY = (scoreScaleMaxY + scoreScaleMinY) * 2 / 100;
        scoreScaleUnitRotateZ = scoreScaleMaxRotationZ * 2 / 100;

        isDopelgangerWeen = false;
        startZposition = transform.localPosition.z;
        StartCoroutine(CheckScoreState());
        scoreAnimatorState = ScoreAnimatorState.NONE;
        CalcPositionsAndSetText();
    }

    private void Update()
    {
        if (scoreAnimatorState == ScoreAnimatorState.INCREASE)
        {
            animator.SetTrigger("increase");
            scoreAnimatorState = ScoreAnimatorState.NONE;
        }
        else if (scoreAnimatorState == ScoreAnimatorState.DECREASE)
        {
            animator.SetTrigger("decrease");
            scoreAnimatorState = ScoreAnimatorState.NONE;
        }
    }

    IEnumerator CheckScoreState()
    {
        while (true)
        {
            yield return new WaitForSeconds(updateTime);
            CalcPositionsAndSetText();
        }
    }

    private void CalcPositionsAndSetText()
    {
        float currentPositionX = scoreScaleX - (scoreScaleUnitX * score);
        float currentPositionY = scoreScaleMinY + (scoreScaleUnitY * Mathf.Abs(score - 50));
        float currentPositionRotateZ = scoreScaleMaxRotationZ - (scoreScaleUnitRotateZ * score);

        transform.localPosition = new Vector3(currentPositionX, currentPositionY, startZposition);
        transform.eulerAngles = new Vector3(0, 0, -currentPositionRotateZ);
        scoreText.text = score.ToString();
    }

    public static void IncreaseScore(int diff)
    {
        if (score < (100 - diff - 1))
        {
            score += diff;
        }
        else
        {
            score = 100;
        }
        scoreAnimatorState = ScoreAnimatorState.INCREASE;
    }
    public static void DecreaseScore(int diff)
    {
        if (score > diff - 1)
        {
            score -= diff;
        }
        else
        {
            score = 0;
        }
        scoreAnimatorState = ScoreAnimatorState.DECREASE;
    }

    public enum FinishState
    {
        BAD,
        MID,
        GOOD
    }

    public static FinishState GetFinishState()
    {
        if(score < 34)
        {
            return FinishState.BAD;
        }
        else if(score < 67)
        {
            return FinishState.MID;
        }
        else
        {
            return FinishState.GOOD;
        }
    }

    public static void ResetScores()
    {
        score = 50;
    }
}

public static class ScoreVolumes
{
    public static readonly int enemyHeat = 10;
    public static readonly int enemyOutOfDisplay = 3;
    public static readonly int enemyDestroy = 7;

    public static readonly int oilHeat = 10;
    public static readonly int oilOutOfDisplay = 3;
    public static readonly int oilDestroy = 7;

    public static readonly int bzzHeat = 10;
    public static readonly int bzzOutOfDisplay = 3;
    public static readonly int bzzDestroy = 7;

    public static readonly int octoppusHeat = 10;
    public static readonly int octoppusOutOfDisplay = 3;
    public static readonly int octoppusDestroy = 7;

    public static readonly int oilBulletHeat = 4;
    public static readonly int oilBulletOutOfDisplay = 0;
    public static readonly int oilBulletDestroy = 0;
    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    // if value of each monster is different from enemy, make shure
    // that you're redefine OnTriggerEnter method in child classes
}
using System.Collections;
using UnityEngine;

public class BatteryScore : MonoBehaviour
{
    [SerializeField] private Sprite zeroCharge, oneCharge, twoCharge, threeCharge, fourCharge;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Sprite[] spriteMassiv;
    private readonly float updateTime = 0.3f;
    //private static int score;
    private static bool batteryBlink;

    public static int Score { get; private set;}

    void Start()
    {
        spriteMassiv = new Sprite[] { zeroCharge, oneCharge, twoCharge, threeCharge, fourCharge };
        StartCoroutine(CheckScoreState());
        Score = 2;
        batteryBlink = false;
    }

    IEnumerator CheckScoreState()
    {
        while (true)
        {
            yield return new WaitForSeconds(updateTime);
            SetSprite();
            if(batteryBlink)
            {
                StartCoroutine(StartBlinkRed());
                batteryBlink = false;
            }
        }
    }

    public static void IncreaseScore()
    {
        if (Score == 0)
        {
            Hero.SetBlumpAccess(true);
        }
        if (Score < (4))
        {
            Score++;
        }
        else
        {
            Score = 4;
        }
    }
    public static void DecreaseScore()
    {
        if (Score > 0)
        {
            Score--;
        }
        else
        {
            batteryBlink = true;
        }
        if (Score == 0)
        {
            Hero.SetBlumpAccess(false);
        }
    }

    private void SetSprite()
    {
        spriteRenderer.sprite = spriteMassiv[Score];
    }

    IEnumerator StartBlinkRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        spriteRenderer.color = Color.white;
    }
}

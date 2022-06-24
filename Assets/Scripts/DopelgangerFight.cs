using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DopelgangerFight : MonoBehaviour
{
    public static DopelgangerState dopelgangerState;
    private float score;
    private float lightMaskMoveUnit;
    private Vector3 maskStartPosition;

    [SerializeField] GameObject maskForLaser;
    [SerializeField] GameObject startScreenAnimation;
    [SerializeField] SpriteRenderer lightMaskWhite, lightMaskBlack;
    [SerializeField] GameObject tapDopelganger;
    [SerializeField] CameraShake backgroundShake;
    [SerializeField] CameraShake cameraShake;

    private readonly float scoreIncrement = 1f;
    private readonly float scoreDecrement = 1f;
    private readonly float decrementUpdateTime = 0.15f;
    private readonly float maxScore = 100;

    private readonly float lighMaskAlphaMin = 0;
    private readonly float lighMaskAlphaMax = 0.47f;
    
    private readonly float maskMaxPosition = 5.35f;
    private readonly float maskMinPosition = 1.1f;
    

    public enum DopelgangerState
    {
        NONE,
        START_FIGHT,
        END_FIGHT
    }

    void Start()
    {
        
        lightMaskMoveUnit = (lighMaskAlphaMax - lighMaskAlphaMin) / maxScore;
        dopelgangerState = DopelgangerState.NONE;
        score = maxScore/2;
        StartCoroutine(DecrementScore());
        maskStartPosition = maskForLaser.transform.localPosition;
        startScreenAnimation.gameObject.SetActive(true);
        StartCoroutine(SpawnTapDopelganger());
    }

    IEnumerator SpawnTapDopelganger()
    {
        while(dopelgangerState != DopelgangerState.START_FIGHT)
        {
            yield return new WaitForSeconds(0.3f);
        }
        for(int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.4f);
            float xRandom, yRandom;
            xRandom = Random.Range(-1.9f, 1.9f);
            yRandom = Random.Range(-4.3f, 4.3f);
            Vector3 tapFuturePosition = new Vector3(xRandom, yRandom, tapDopelganger.transform.position.z);
            Instantiate(tapDopelganger, tapFuturePosition, tapDopelganger.transform.rotation);
        }
    }

    IEnumerator DecrementScore()
    {
        while(true)
        {
            yield return new WaitForSecondsRealtime(decrementUpdateTime);
            if (dopelgangerState == DopelgangerState.START_FIGHT)
            {
                score -= scoreDecrement;
            }
        }
    }

    void Update()
    {
        GetControllers();
        MoveMask();
        CheckLevelEndRule();
        backgroundShake.ShakerEnable = true;
    }

    private void MoveMask()
    {
        Vector3 maskNewPosition = maskStartPosition;
        maskNewPosition.y = maskMinPosition + (maskMaxPosition - maskMinPosition) / maxScore * score;
        maskForLaser.gameObject.transform.localPosition = maskNewPosition;

        float alphaWhite, alphaBlack;
        if((score - maxScore / 2) > 0)
        {
            alphaBlack = (score - maxScore / 2) * lightMaskMoveUnit;
            alphaWhite = 0;
        }
        else
        {
            alphaBlack = 0; 
            alphaWhite = -((score - maxScore / 2) * lightMaskMoveUnit * 4f);
        }
        lightMaskBlack.color = new Color(1f, 1f, 1f, alphaBlack);
        lightMaskWhite.color = new Color(0f, 0f, 0f, alphaWhite);
    }

    private void CheckLevelEndRule()
    {
        if (score > maxScore)
        {
            dopelgangerState = DopelgangerState.END_FIGHT;
            SceneManager.LoadScene("Finish");
        }
        else if (score < 0)
        {
            dopelgangerState = DopelgangerState.END_FIGHT;
            ScoreManager.isDopelgangerWeen = true;
            ScoreManager.wasDamagedOneTime = true;
            SceneManager.LoadScene("Finish");
        }
    }

    private void GetControllers()
    {
        if (dopelgangerState == DopelgangerState.START_FIGHT)
        {
            if (Input.touches.Length > 0)
            {
                Touch t = Input.GetTouch(0);
                if (t.phase == TouchPhase.Ended)
                {
                    score += scoreIncrement * 1.2f;
                }
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                score += scoreIncrement * 1.3f;
            }
            cameraShake.ShakerEnable = true;
        }
    }
}
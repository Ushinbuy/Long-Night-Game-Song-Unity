using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasMeneger : MonoBehaviour
{
    [SerializeField] Button bossButton, pauseButton, restartButton, letsGoButton, resetState;
    [SerializeField] Button ebashButton;
    [SerializeField] AudioSource mainTheme;
    [SerializeField] Sprite pauseSprite, resumeSprite;
    [SerializeField] GameObject rulesOnPause;
    [SerializeField] Image pauseButtonImage;
    [SerializeField] Animator animator;
    [SerializeField] AudioSource lampSource;
    public AudioClip lampMidClip, lampBadClip, lampGoodClip;

    [SerializeField] GameObject badLamp, midLamp;

    [SerializeField] CommonScenariosDelegates commonScenariosDelegates;
    private readonly float timeBossBegining = 105;
    private readonly float timeTrackTotal = 171;
    private static CanvasState canvasState;

    private bool gamePaused;

    public static bool isStartPassed;

    private enum CanvasState
    {
        NONE,
        ON_PAUSE,
        ENABLE_EBASH_BUTTON,
        LISTEN_SPACE_BUTTON
    }

    void Start()
    {
        // reset audio if restart
        mainTheme.Stop();
        mainTheme.Play();

        badLamp.SetActive(false);
        midLamp.SetActive(false);

        //gamePaused = false;
        if (bossButton == null || pauseButton == null)
        {
            return;
        }
        canvasState = CanvasState.NONE;

        if (isStartPassed == true)
        {
            rulesOnPause.SetActive(false);
        }
        else
        {
            rulesOnPause.SetActive(true);
            StartStateCanvas();
            
        }

        restartButton.onClick.AddListener(StartAgain);
        bossButton.onClick.AddListener(BossStart);
        pauseButton.onClick.AddListener(PauseMethod);
        ebashButton.onClick.AddListener(FinalShot);
        letsGoButton.onClick.AddListener(FirstStart);
        resetState.onClick.AddListener(ResetFinish);

        restartButton.gameObject.SetActive(false);
        bossButton.gameObject.SetActive(false);
        ebashButton.gameObject.SetActive(false);
        resetState.gameObject.SetActive(false);
        
        StartCoroutine(CoroutineBossBegining());

        commonScenariosDelegates.bossStartStep += BossStartInit;
    }

    private void ResetFinish()
    {
        ScoreManager.wasDamagedOneTime = false;
    }

    private void StartStateCanvas()
    {
        Time.timeScale = 0;
        gamePaused = true;
        rulesOnPause.SetActive(true);
        Hero.SetPause(true);
        mainTheme.Pause();
        pauseButton.gameObject.SetActive(false);
    }

    private void FirstStart()
    {
        Time.timeScale = 1;
        gamePaused = false;
        pauseButton.gameObject.SetActive(true);
        rulesOnPause.SetActive(false);
        mainTheme.Play();
        letsGoButton.gameObject.SetActive(false);
        StartCoroutine(PauseForHeroStart());
    }

    IEnumerator PauseForHeroStart()
    {
        yield return new WaitForSeconds(0.2f);
        Hero.SetPause(false);
    }

    private void PauseMethod()
    {
        if (gamePaused == false)
        {
            Time.timeScale = 0;
            gamePaused = true;
            //bossButton.gameObject.SetActive(true);
            //resetState.gameObject.SetActive(true);
            restartButton.gameObject.SetActive(true);
            rulesOnPause.SetActive(true);
            Hero.SetPause(true);
            mainTheme.Pause();
            pauseButtonImage.sprite = resumeSprite;
            animator.SetBool("usualyState", true);
        }
        else
        {
            Time.timeScale = 1;
            gamePaused = false;
            bossButton.gameObject.SetActive(false);
            resetState.gameObject.SetActive(false);
            restartButton.gameObject.SetActive(false);
            rulesOnPause.SetActive(false);
            mainTheme.Play();
            pauseButtonImage.sprite = pauseSprite;
            StartCoroutine(ResumePause());
        }
        
    }

    IEnumerator ResumePause()
    {
        yield return new WaitForSeconds(0.2f);
        Hero.SetPause(false);
    }

    private void BossStart()
    {
        commonScenariosDelegates.bossStartStep?.Invoke();
    }

    private void BossStartInit()
    {
        StopCoroutine(CoroutineBossBegining());
        StartCoroutine(PressButtonEbash());
        bossButton.enabled = false;
    }

    IEnumerator PressButtonEbash()
    {
        yield return new WaitForSeconds(timeTrackTotal - timeBossBegining);
        FinalShot();
    }

    public static void SetEbashVisible()
    {
        canvasState = CanvasState.ENABLE_EBASH_BUTTON;
    }

    IEnumerator CoroutineBossBegining()
    {
        yield return new WaitForSeconds(timeBossBegining);
        BossStart();
    }

    private void FinalShot()
    {
        StopCoroutine(PressButtonEbash());
        ebashButton.gameObject.SetActive(false);
        if(ScoreManager.GetFinishState() == ScoreManager.FinishState.GOOD)
        {
            BossManager.BossCollapse();
            lampSource.PlayOneShot(lampGoodClip);
        }
        else if(ScoreManager.GetFinishState() == ScoreManager.FinishState.MID)
        {
            midLamp.SetActive(true);
            StartCoroutine(MidLampDelay());
        }
        else if(ScoreManager.GetFinishState() == ScoreManager.FinishState.BAD)
        {
            badLamp.SetActive(true);
            lampSource.PlayOneShot(lampBadClip);
            StartCoroutine(SlowBatteryDischargeAnimation());
        }
        commonScenariosDelegates.finalShotStep?.Invoke();
    }

    IEnumerator MidLampDelay()
    {
        yield return new WaitForSeconds(1.2f);
        lampSource.PlayOneShot(lampMidClip);
    }

    IEnumerator SlowBatteryDischargeAnimation()
    {
        for (int i = 4; i > 0; i--)
        {
            yield return new WaitForSeconds(0.3f);
            BatteryScore.DecreaseScore();
        }
    }

    private void Update()
    {
        if (canvasState == CanvasState.NONE)
        {
            return;
        }
        else if (canvasState == CanvasState.ENABLE_EBASH_BUTTON)
        {
            pauseButton.interactable = false;
            canvasState = CanvasState.LISTEN_SPACE_BUTTON;
            StartCoroutine(SetEbashButtonWithDelay());
        }
        else if (canvasState == CanvasState.LISTEN_SPACE_BUTTON)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                FinalShot();
            }
        }
    }

    IEnumerator SetEbashButtonWithDelay()
    {
        yield return new WaitForSeconds(1f);
        ebashButton.gameObject.SetActive(true);
    }

    public void StartAgain()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneBuildIndex: 0);
    }
}
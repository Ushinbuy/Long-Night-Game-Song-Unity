using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FinishChouse : MonoBehaviour
{
    [SerializeField] private GameObject badFinish, midFinish, goodFinish, secretFinish,
                                        canvasLinks, endBackground, scoreScale;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Button passFinishButton;
    [SerializeField] private GameObject scoreText;

    private readonly float startVolume = 0;
    private readonly float endVolume = 0.99f;

    private GameObject currentFinish;
    private bool firstPassFinishButtonPress;

    public static FinishAnimationState finishAnimationState;

    public enum FinishAnimationState
    {
        START_PLAY,
        SET_SCORE,
        STOP_PLAY
    }

    void Start()
    {
        firstPassFinishButtonPress = true;
        finishAnimationState = FinishAnimationState.START_PLAY;
        canvasLinks.SetActive(false);
        endBackground.SetActive(false);

        if(ScoreManager.wasDamagedOneTime == false)
        {
            secretFinish.gameObject.SetActive(true);
            return;
        }

        audioSource.volume = startVolume;
        audioSource.Play();

        passFinishButton.onClick.AddListener(PassFinish);
        

        StartCoroutine(AudioFade());
        if(ScoreManager.GetFinishState() == ScoreManager.FinishState.BAD)
        {
            currentFinish = badFinish;
        }
        else if(ScoreManager.GetFinishState() == ScoreManager.FinishState.MID || ScoreManager.isDopelgangerWeen)
        {
            currentFinish = midFinish;
        }
        else if(ScoreManager.GetFinishState() == ScoreManager.FinishState.GOOD)
        {
            currentFinish = goodFinish;
        }
        currentFinish.SetActive(true);
    }

    IEnumerator AudioFade()
    {
        float increaseTime = 0.2f;
        while (audioSource.volume < endVolume)
        {
            yield return new WaitForSeconds(increaseTime);
            audioSource.volume += (0.01f);
        }
    }

    private void Update()
    {
        if(finishAnimationState == FinishAnimationState.SET_SCORE)
        {
            scoreText.SetActive(true);
            scoreScale.SetActive(true);
        }
        else if(finishAnimationState == FinishAnimationState.STOP_PLAY)
        {
            SetEndScreen();
        }
    }

    private void SetEndScreen()
    {
        canvasLinks.SetActive(true);
        endBackground.SetActive(true);

        passFinishButton.gameObject.SetActive(false);
        scoreScale.SetActive(false);
        badFinish.SetActive(false);
        midFinish.SetActive(false);
        goodFinish.SetActive(false);
        scoreText.SetActive(false);
    }

    public void PassFinish()
    {
        if(firstPassFinishButtonPress)
        {
            Animator animator = currentFinish.GetComponent<Animator>();
            //animator.speed = 0;
            animator.Play("Bad Finish", 0, (float)57 / 74);
            firstPassFinishButtonPress = false;
        }
        else
        {
            finishAnimationState = FinishAnimationState.STOP_PLAY;
        }
    }
}

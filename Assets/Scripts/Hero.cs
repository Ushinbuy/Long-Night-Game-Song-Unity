using System.Collections;
using UnityEngine;

public class Hero : MoveController
{
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private CanvasMeneger canvasMeneger;
    [SerializeField] private CameraShake cameraShake;

    [SerializeField] private CommonScenariosDelegates commonScenariosDelegates;

    public AudioClip swipeLeftAudio, swipeRightAudio, batteryAudio, noPowerAudio, atackAudio, heatPlayer;
    private Vector2 firstPressPos;
    private bool doesItRight = false;
    
    private TouchState touchState;
    private readonly int swipeRange = 30;

    private static bool blumpIsAccess;
    private static bool pause;
    private bool moveBlocked;
    private float startSpeed;

    private readonly float yMainPosition = -2.9f;

    private HeroState heroState;

    private bool GetDoesItRight() => doesItRight;
    private void SetDoesItRight(bool doesItRight) => this.doesItRight = doesItRight;
    public static void SetPause(bool inputValue) => pause = inputValue;

    public enum SwipeDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    private enum TouchState
    {
        NONE,
        BEGAN,
        MOVED,
        END
    }

    private enum HeroState
    {
        NONE,
        BOSS_START,
        SCALING,
        END_LEVEL
    }

    private void Start()
    {
        InitMoveController();
        startSpeed = 2.4f;
        MoveToY(yMainPosition, startSpeed);

        heroState = HeroState.NONE;
        firstPressPos = Vector2.zero;
        touchState = TouchState.NONE;

        StartCoroutine(StartDeceleration());
        SetPause(false);
        SetMoveBlocked(false);
        SetBlumpAccess(true);
        StartCoroutine(DisablePauseAfterStart());

        commonScenariosDelegates.bossStartStep += HeroBossStart;
        commonScenariosDelegates.finalBatteryStep += FinalBattery;
        commonScenariosDelegates.firstShakeStartStep += ShakeEnable;
        commonScenariosDelegates.firstShakeStopStep += ShakeDisable;
        commonScenariosDelegates.finalShotStep += FinalShot;
    }

    IEnumerator StartDeceleration()
    {
        yield return new WaitForSeconds(0.8f);
        for(int i = 4; i > 0; i--)
        {
            MoveToY(yMainPosition, startSpeed * i * 0.2f);
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator DisablePauseAfterStart()
    {
        yield return new WaitForSeconds(0.2f);
        SetPause(false);
    }

    void Update()
    {
        CheckSwipe();
        if ( !(moveBlocked || pause))
        {
            GetPcKey();
        }
        if(heroState == HeroState.BOSS_START)
        {
            StartCoroutine(TimeForFinishMoving());
            heroState = HeroState.NONE;
        }
        else if(heroState == HeroState.END_LEVEL)
        {
            StartCoroutine(TimeForAnotherFinish());
            heroState = HeroState.NONE;
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (heroState == HeroState.SCALING)
        {
            float decreaseSpeed = 0.001f;
            Vector3 decreaseUnit = new Vector3(decreaseSpeed, decreaseSpeed, 0f);
            transform.localScale -= decreaseUnit;
            if (transform.localScale.x < 0.8f)
            {
                heroState = HeroState.NONE;
            }
        }
    }

    private void SetMoveBlocked(bool value)
    {
        moveBlocked = value;
    }

    private void HeroBossStart()
    {
        heroState = HeroState.BOSS_START;
    }

    private void FinalBattery()
    {
        heroState = HeroState.END_LEVEL;
    }

    IEnumerator TimeForAnotherFinish()
    {
        yield return new WaitForEndOfFrame();
        SetPause(true);
        MoveToRoad(3 - 1);
    }

    IEnumerator TimeForFinishMoving()
    {
        yield return new WaitForSeconds(4f); // time for end all enemys
        SetPause(true);

        numPositions = 5;
        movementUnit = 2.5f / 2f;
        InitMoveController();
        MoveToY(-3.6f, 0.34f);
        heroState = HeroState.SCALING;
        SetPause(false);
    }

    private void GetPcKey()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveHero(SwipeDirection.Left);
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveHero(SwipeDirection.Right);
        }
        else if(Input.GetKeyDown(KeyCode.Space))
        {
            Shot();
        }
    }

    public void CheckSwipe()
    {
        if (Input.touches.Length > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                touchState = TouchState.BEGAN;
                firstPressPos = new Vector2(t.position.x, t.position.y);
            }
            if (t.phase == TouchPhase.Moved)
            {
                if (touchState == TouchState.END || touchState == TouchState.NONE)
                {
                    touchState = TouchState.BEGAN;
                    firstPressPos = new Vector2(t.position.x, t.position.y);
                }
            }
            if (t.phase == TouchPhase.Ended)
            {
                touchState = TouchState.END;
                Vector2 currentSwipe = new Vector2(t.position.x - firstPressPos.x, t.position.y - firstPressPos.y);
                if (!(!moveBlocked && !pause))
                {
                    firstPressPos = Vector2.zero;
                }
                else
                {
                    CheckMovement(currentSwipe);
                }
            }
        }
    }



    public static void SetBlumpAccess(bool access) => blumpIsAccess = access;

    private void Fuse()
    {
        SetMoveBlocked(true);
    }

    private void Unfuse()
    {
        SetMoveBlocked(false);
    }

    private void Damage()
    {
        animator.SetTrigger("damage");
    }

    private void CheckMovement(Vector2 currentSwipe)
    {
        if (currentSwipe.x < -swipeRange)
        {
            MoveHero(SwipeDirection.Left);
        }
        else if (currentSwipe.x > swipeRange)
        {
            MoveHero(SwipeDirection.Right);
        }
        else
        {
            Shot();
        }
        firstPressPos = Vector2.zero;
    } 

    private void ShakeEnable()
    {
        cameraShake.ShakerEnable = true;
    }

    private void ShakeDisable()
    {
        cameraShake.ShakerEnable = false;
    }

    public void Shot()
    {
        // this trigger callback functions StartBulb() and StopBulb() from animation
        if (blumpIsAccess)
        {
            audioSource.PlayOneShot(atackAudio, 0.2f);
            animator.SetTrigger("shot");
        }
        else
        {
            audioSource.PlayOneShot(noPowerAudio, 0.2f);
        }
        BatteryScore.DecreaseScore();
    }

    private void MoveHero(SwipeDirection swipeDirection)
    {
        if (swipeDirection == SwipeDirection.Left)
        {
            if (SetLeftMove() == true)
            {
                SetDoesItRight(false);
                AnimateSwipe();
                audioSource.PlayOneShot(swipeLeftAudio, 13f);
            }
        }
        else if (swipeDirection == SwipeDirection.Right)
        {
            
            if (SetRightMove() == true)
            {
                SetDoesItRight(true);
                AnimateSwipe();
                audioSource.PlayOneShot(swipeRightAudio, 13f);
            }
        }
        else { }
    }

    // unused in animator
    private void AnimateSwipe()
    {
        animator.SetBool("doesItRight", GetDoesItRight());
        animator.SetTrigger("swipe");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Bulb"))
        {
            return;
        }
        else if (other.tag.Equals("Enemy"))
        {
            ScoreManager.wasDamagedOneTime = true;
            Damage();
            audioSource.PlayOneShot(heatPlayer, 0.5f);
        }
        else if (other.tag.Equals("Battery"))
        {
            if(BatteryScore.Score < 4)
            {
                audioSource.PlayOneShot(batteryAudio, 0.2f);
            }
        }
        else if (other.tag.Equals("Boss Atack"))
        {
            ScoreManager.wasDamagedOneTime = true;
            Damage();
            ScoreManager.DecreaseScore(ScoreVolumes.enemyHeat / 2);
            audioSource.PlayOneShot(heatPlayer, 0.5f);
        }
        else if (other.tag.Equals("Super Battery"))
        {
            audioSource.PlayOneShot(batteryAudio, 0.2f);
            Destroy(other.gameObject);
            for (int i = 0; i < 4; i++)
            {
                BatteryScore.IncreaseScore();
            }
        }
        else { }
    }

    private void FinalShot()
    {
        audioSource.PlayOneShot(atackAudio, 0.2f);
        
        if (ScoreManager.GetFinishState() == ScoreManager.FinishState.GOOD)
        {
            animator.SetTrigger("boss destroy");
        }
        else if (ScoreManager.GetFinishState() == ScoreManager.FinishState.MID)
        {
            animator.SetTrigger("lamp mid");
        }
        else if (ScoreManager.GetFinishState() == ScoreManager.FinishState.BAD)
        {
            animator.SetTrigger("lamp bad");
        }
    }
}
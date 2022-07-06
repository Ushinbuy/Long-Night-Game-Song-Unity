using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossManager : MoveController
{
    [SerializeField] private GameObject cloudPrefab, batteryPrefab, destroyMask;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer bossSpriteRenderer, laserSpriteRenderer;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip bossBatteryCreate, bossMove, bossDamage, lauchClip,
                                        laserClip, snakeBegin, snakeBite, cloudCreate, bossCameClip;
    [SerializeField] private CommonScenariosDelegates commonScenariosDelegates;
    [SerializeField] private CameraShake bossShake;
    private GameObject hero, background;
    private CameraShake heroShake, backgroundShake;
    private readonly float yNormalPoision = 2.9f;

    private static BossState bossState;
    private enum BossState
    {
        NONE,
        START_MOVE,
        LASER_START,
        WORM,
        BATTERY,
        START_CLOUD_ATACK_MOVE,
        FINISH_CLOUD_ATACK_MOVE,
        BOSS_IS_DESTROYED
    }

    private void Start()
    {
        hero = GameObject.Find("Player");
        heroShake = hero.GetComponent<CameraShake>();
        background = GameObject.Find("Background");
        backgroundShake = background.GetComponent<CameraShake>();
        audioSource.PlayOneShot(bossCameClip, 0.9f);

        numPositions = 5;
        movementUnit = 2.5f / 2f;
        InitMoveController();

        bossState = BossState.START_MOVE;
        MoveToY(yNormalPoision, 1.1f);
        //audioSource.PlayOneShot(bossMove, 0.2f);
    }

    private void Update()
    {
        if((bossState == BossState.START_MOVE) &&
            yMoveState == MoveState.MOVING)
        {
            bossShake.ShakerEnable = true;
            heroShake.ShakerEnable = true;
            backgroundShake.ShakerEnable = true;
        }
        if((bossState == BossState.START_MOVE) &&
            (yMoveState == MoveState.DONE))
        {
            bossShake.ShakerEnable = false;
            heroShake.ShakerEnable = false;
            backgroundShake.ShakerEnable = false;
            yMoveState = MoveState.NONE;
            StartCoroutine(AtackScenario());
        }
        if ((bossState == BossState.START_CLOUD_ATACK_MOVE) &&
            (yMoveState == MoveState.DONE))
        {
            
            yMoveState = MoveState.NONE;
            StartCoroutine(StartAtackCloud());
        }
        if ((bossState == BossState.FINISH_CLOUD_ATACK_MOVE) &&
            (yMoveState == MoveState.DONE))
        {
            yMoveState = MoveState.NONE;
            StartCoroutine(BatteryScenario());
        }
        if(bossState == BossState.BOSS_IS_DESTROYED)
        {
            //Debug.Log("Boss manager Finish start");
            //boxCollider.enabled = false;
            if (ScoreManager.GetFinishState() == ScoreManager.FinishState.GOOD)
            {
                animator.SetTrigger("destroy");
                bossShake.ShakerEnable = true;
                backgroundShake.ShakerEnable = true;
            }
            else if(ScoreManager.GetFinishState() == ScoreManager.FinishState.MID)
            {
                animator.SetTrigger("boss mid");
                bossShake.ShakerEnable = true;
                backgroundShake.ShakerEnable = true;
            }
            else if(ScoreManager.GetFinishState() == ScoreManager.FinishState.BAD)
            {
                animator.SetTrigger("boss win");
            }
        }
    }

    IEnumerator AtackScenario()
    {
        float pauseBetweenAtacks = 0.5f;
        yield return StartCoroutine(SetAtackLaser());
        yield return new WaitForSeconds(pauseBetweenAtacks);
        SetBattery();
        yield return new WaitForSeconds(pauseBetweenAtacks);
        yield return StartCoroutine(SetAtackWorm());
        yield return new WaitForSeconds(0.1f);
        SetBattery();
        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(SetAtackCloud());
    }


    //----------------   LASER  --------------------------
    private void AudioLaserClip()
    {
        audioSource.PlayOneShot(laserClip, 0.9f);
    }

    private void AudioLauchClip()
    {
        audioSource.PlayOneShot(lauchClip, 1.2f);
    }

    IEnumerator SetAtackLaser()
    {
        yield return new WaitForSeconds(0.2f);
        int[] roads = new int[] { 1, 3, 5, 2, 3, 5, 3 };
        for (int i = 0; i < roads.Length - 1; i++)
        {
            MoveToRoad(roads[i] - 1);
            audioSource.PlayOneShot(bossMove, 0.6f);
            bossState = BossState.LASER_START;
            animator.SetTrigger("atack_laser");
            while (bossState == BossState.LASER_START)
            {
                yield return new WaitForSeconds(0.3f);
            }
        }
        MoveToRoad(3 - 1);
        audioSource.PlayOneShot(bossMove, 0.6f);
    }

    //----------------   WORM  --------------------------
    private void AudioSnakeBegin()
    {
        // this function call by animation atack_worm
        audioSource.PlayOneShot(snakeBegin, 0.8f);
    }

    private void AudioSnakeBite()
    {
        // this function call by animation atack_worm_right or atack_worm_left
        audioSource.PlayOneShot(snakeBite, 0.8f);
    }

    IEnumerator SetAtackWorm()
    {
        yield return new WaitForSeconds(0.2f);

        string firstWorm, secondWorm;
        int wormDir = Random.Range(0, 2);
        if(wormDir > 0)
        {
            animator.SetTrigger("atack worm start right");
            firstWorm = "atack_worm_left";
            secondWorm = "atack_worm_right";
        }
        else
        {
            animator.SetTrigger("atack worm start left");
            firstWorm = "atack_worm_right";
            secondWorm = "atack_worm_left";
        }
        bossState = BossState.WORM;

        while (bossState == BossState.WORM)
        {
            yield return new WaitForSeconds(0.3f);
        }

        animator.SetTrigger(firstWorm);
        bossState = BossState.WORM;
        while (bossState == BossState.WORM)
        {
            yield return new WaitForSeconds(0.3f);
        }
        
        animator.SetTrigger(secondWorm);
        bossState = BossState.WORM;
        while (bossState == BossState.WORM)
        {
            yield return new WaitForSeconds(0.3f);
        }
    }

    //----------------   CLOUD  --------------------------
    IEnumerator SetAtackCloud()
    {
        yield return new WaitForSeconds(0.2f);
        bossState = BossState.START_CLOUD_ATACK_MOVE;
        MoveToY(4.0f, 0.5f);
        audioSource.PlayOneShot(bossMove, 0.6f);
    }

    IEnumerator StartAtackCloud()
    {
        animator.SetTrigger("atack_cloud");
        yield return new WaitForSeconds(7f);
        bossState = BossState.FINISH_CLOUD_ATACK_MOVE;
        MoveToY(yNormalPoision, 1f);
        audioSource.PlayOneShot(bossMove, 0.6f);
    }

    private void SpawnCloud()
    {
        // this function callsed by animation "atack_cloud"
        Vector3 cloudCreatePosition = new Vector3(transform.position.x, transform.position.y - 1.4f, transform.position.z - 0.1f);
        audioSource.PlayOneShot(cloudCreate, 1.2f);
        Instantiate(cloudPrefab, cloudCreatePosition, cloudPrefab.transform.rotation);
    }

    //----------------   BATTERY  --------------------------

    IEnumerator BatteryScenario()
    {
        yield return new WaitForSeconds(6f);
        animator.SetTrigger("super battery");
    }

    private void AudioBattery()
    {
        audioSource.PlayOneShot(bossBatteryCreate, 1.2f);
    }

    void BetteryEnd()
    {
        // this function call by animation "battery"
        commonScenariosDelegates.finalBatteryStep.Invoke();
        commonScenariosDelegates.finalBatteryStep = null;
        StartCoroutine(TheEndCoroutine());
    }

    private void SetBattery()
    {
        animator.SetTrigger("battery");
    }

    private void SpawnBattery()
    {
        Instantiate(batteryPrefab, new Vector3(transform.position.x, transform.position.y - 1.2f, transform.position.z - 1),
                                    batteryPrefab.transform.rotation);
    }

    //----------------   COMMON  --------------------------

    IEnumerator TheEndCoroutine()
    {
        yield return new WaitForSeconds(1f);
        CanvasMeneger.SetEbashVisible();
    }
    

    private void DestroyBoss()
    {
        SceneManager.LoadScene("Dopelganger");
    }

    private void EndOfLevel()
    {
        SceneManager.LoadScene("Finish");
    }

    public static void BossCollapse()
    {
        bossState = BossState.BOSS_IS_DESTROYED;
    }

    private void AnimationIsFinish()
    {
        // this function call by animations
        bossState = BossState.NONE;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Enemy"))
        {
            return;
        }
        else if (other.tag.Equals("Bulb"))
        {
            ScoreManager.IncreaseScore(ScoreVolumes.enemyDestroy);
            StartCoroutine(HeatColorAnimate());
            audioSource.PlayOneShot(bossDamage, 0.63f);
        }
    }

    private void SetDestroyMask()
    {
        destroyMask.gameObject.SetActive(true);
    }

    IEnumerator HeatColorAnimate()
    {
        bossSpriteRenderer.color = Color.red;
        laserSpriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        bossSpriteRenderer.color = Color.white;
        laserSpriteRenderer.color = Color.white;
    }
}
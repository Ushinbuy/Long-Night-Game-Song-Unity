using System.Collections;
using UnityEngine;

public class BackGroundManager : MonoBehaviour
{
    [Range(0.1f, 10f)]
    public float scrollSpeedStart = 0.5f;
    public Sprite[] images;
    [SerializeField] private GameObject obj123, obj23;
    [SerializeField] private GameObject[] obj456;
    [SerializeField] private GameObject[] obj67;
    [SerializeField] private GameObject panelkaSky;

    [SerializeField] private CanvasMeneger canvasMeneger;

    private readonly float maxPosition = -17.5f;
    private readonly float timeLoopStop_3 = 50f - 12f;
    private readonly float timeLoopStop_5 = 96f - 15f;

    private float scrollSpeed;
    private static float scrollSpeedDecrement;
    private Vector3 startPosition;

    public SpriteRenderer currentPage, nextPage;
    private int imagesCounter;
    private GameObject[] currentLayerObjects;

    private State state;

    private enum State
    {
        NONE,
        LOOP_SPRITE_3,
        LOOP_SPRITE_5,
        BOSS_START
    }

    void Start()
    {
        state = State.NONE;
        scrollSpeedDecrement = 0f;
        scrollSpeed = scrollSpeedStart;
        if (images == null)
        {
            return;
        }
        LoadStartBackground();
        SwitchSpawnScenario(imagesCounter);
        StartCoroutine(WaitLoop3());
        StartCoroutine(WaitLoop5());
        StartCoroutine(SpawnObjectsCoroutine());

        canvasMeneger.bossStartingEvent += BackGroundManagerBossStart;
    }

    public static void StartLevel()
    {
        scrollSpeedDecrement = 0;
    }

    private void BackGroundManagerBossStart()
    {
        state = State.BOSS_START;
    }

    void Update()
    {
        if (images == null)
        {
            return;
        }
        if (scrollSpeed > 0)
        {
            MoveBackGround();
        }
        if (transform.position.y < maxPosition)
        {
            ChangeBackgroundSprite();
            ResetBackgroundPosition();
        }
        if(state == State.BOSS_START)
        {
            StopAllCoroutines();
            scrollSpeedDecrement = 0.005f;
            state = State.NONE;
        }
        
    }
    private void ChangeBackgroundSprite()
    {
        if ((imagesCounter + 1) < images.Length)
        {
            //Debug.Log("Sprite is " + images[imagesCounter].name + " Time " + Time.time);
            if (images[imagesCounter].name.Equals("3_loop")
                && state == State.LOOP_SPRITE_3)
            {
                currentPage.sprite = images[imagesCounter];
                nextPage.sprite = images[imagesCounter];
                return;
            }
            else if (images[imagesCounter].name.Equals("5_loop")
                && state == State.LOOP_SPRITE_5)
            {
                currentPage.sprite = images[imagesCounter];
                nextPage.sprite = images[imagesCounter];
                return;
            }
            currentPage.sprite = images[imagesCounter];
            nextPage.sprite = images[imagesCounter + 1];
            imagesCounter += 1;
            SwitchSpawnScenario(imagesCounter);
            if (currentPage.sprite.name.Equals("2"))
            {
                currentLayerObjects = new GameObject[] { obj123, obj23 };
            }
            else if (currentPage.sprite.name.Equals("4"))
            {
                currentLayerObjects = obj456;
            }
            else if (currentPage.sprite.name.Equals("6"))
            {
                currentLayerObjects = obj67;
            }
        }
        else
        {
            currentPage.sprite = images[imagesCounter];
            nextPage.sprite = images[imagesCounter];
        }
    }

    IEnumerator WaitLoop3()
    {
        yield return new WaitForSeconds(timeLoopStop_3);
        state = State.NONE;
    }
    IEnumerator WaitLoop5()
    {
        yield return new WaitForSeconds(timeLoopStop_5);
        state = State.NONE;
    }

    private void MoveBackGround()
    {
        transform.Translate(Vector2.down * scrollSpeed * Time.deltaTime);
        if(scrollSpeedDecrement > 0.002)
        {
            scrollSpeed -= scrollSpeedDecrement;
        }
    }
    private void ResetBackgroundPosition()
    {
        float currentPositionX = transform.position.x;
        transform.position = new Vector3(currentPositionX, startPosition.y, startPosition.z);
        if(panelkaSky != null)
        {
            panelkaSky.SetActive(false); // Destroy has error
        }
    }

    private void LoadStartBackground()
    {
        currentPage.sprite = images[0];
        nextPage.sprite = images[1];
        imagesCounter = 1;
        startPosition = transform.position;
        currentLayerObjects = new GameObject []{obj123};
    }

    IEnumerator SpawnObjectsCoroutine()
    {
        while (true)
        {
            float time = Random.Range(2f, 10f);
            yield return new WaitForSeconds(time);
            SpawnRandomObjects();
        }
    }

    private void SpawnRandomObjects()
    {
        if (this.currentLayerObjects == null)
        {
            return;
        }
        int objectsIndex;
        objectsIndex = Random.Range(0, this.currentLayerObjects.Length);
        int randomIndexX = Random.Range(-1, 1);
        if(randomIndexX == 0)
        {
            randomIndexX = 1;
        }
        float randomPostoinX = 4f * randomIndexX;
        float randomPostoinY = Random.Range(-4.9f, 5.5f);
        Vector3 spawnPos = new Vector3(randomPostoinX, randomPostoinY, -0.02f);
        Instantiate(this.currentLayerObjects[objectsIndex], spawnPos, this.currentLayerObjects[objectsIndex].transform.rotation);
    }

    private void SwitchSpawnScenario(int index)
    {
        switch(index)
        {
            case 1:
                ProbabilityMaster.SetValues(09f, 01f, 90f, 3f,      70f, 1.2f);  break;
            case 2:
                ProbabilityMaster.SetValues(10f, 10f, 80f, 2.8f,    75f, 1.2f);  break;
            case 3:
                ProbabilityMaster.SetValues(45f, 20f, 35f, 2.4f,    80f, 1f);    break;
            case 4:
                ProbabilityMaster.SetValues(45f, 25f, 35f, 2f,      85f, 1f);    break;
            case 5:
                ProbabilityMaster.SetValues(33f, 33f, 33f, 1.3f,    90f, 0.8f);  break;
            case 6:
                ProbabilityMaster.SetValues(30f, 40f, 30f, 1.8f,    95f, 0.8f);  break;
            default:
                break;
        }
    }
}

public static class ProbabilityMaster
{
    public static float oilP;
    public static float bzzP;
    public static float octoP;
    public static float timeEnemyStart;
    public static float batteryP;
    public static float timeBatteryStart;

    public static void SetValues(float oilP, float bzzP, float octoP, float timeEnemyStart,
                                float batteryP, float timeBatteryStart)
    {
        ProbabilityMaster.oilP = oilP;
        ProbabilityMaster.bzzP = bzzP;
        ProbabilityMaster.octoP = octoP;
        ProbabilityMaster.batteryP = batteryP;
        ProbabilityMaster.timeEnemyStart = timeEnemyStart;
        ProbabilityMaster.timeBatteryStart = timeBatteryStart;
    }
}
using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject oilEnemyPrefab;
    [SerializeField] private GameObject bzzEnemyPrefab;
    [SerializeField] private GameObject octoEnemyPrefab;
    [SerializeField] private GameObject bossGameObject;
    [SerializeField] private GameObject batteryPrefab;

    [SerializeField] private CanvasMeneger canvasMeneger;

    private readonly float spawnRangeX = 2.0f;
    private readonly float spawnRangeZ = -0.1f;

    private bool bossStart;

    void Start()
    {
        InitBoss();
        ProbabilityMaster.SetValues(9, 1, 90, 2f, 50, 1.5f);
        StartCoroutine(SpawnMonstersRandomly());
        StartCoroutine(SpawnBattery());

        canvasMeneger.bossStartingEvent += SpawnManagerBossStart;
    }

    private void InitBoss()
    {
        bossStart = false;
        bossGameObject.SetActive(false);
        // boss start position
        bossGameObject.transform.position = new Vector3(0, 8.3f, -0.1f);
    }

    IEnumerator SpawnMonstersRandomly()
    {
        yield return new WaitForSeconds(5.5f);
        float updateEnemyTime;
        int previousRoad = -10;
        while (true)
        {
            if (bossStart)
            {
                StopAllCoroutines();
                StartCoroutine(SpawnBoss());
                bossStart = false;
            }
            updateEnemyTime = Random.Range(ProbabilityMaster.timeEnemyStart, ProbabilityMaster.timeEnemyStart + 0.7f);
            yield return new WaitForSeconds(updateEnemyTime);
            int multplyForRoad = Random.Range(-1, 2);
            if(multplyForRoad == previousRoad)
            {
                // this for do difference in road of monster spawn
                if(multplyForRoad != 0)
                {
                    multplyForRoad = 0;
                }
                else
                {
                    int roadCorrection = Random.Range(-1, 1);
                    if(roadCorrection > -1)
                    {
                        roadCorrection = 1;
                    }
                    multplyForRoad += roadCorrection;
                }
            }
            else
            {
                previousRoad = multplyForRoad;
            }
            Vector3 spawnPos = new Vector3(multplyForRoad * spawnRangeX, 6.0f, spawnRangeZ);

            float summOfAllProbability = ProbabilityMaster.oilP + ProbabilityMaster.bzzP + ProbabilityMaster.octoP;
            float enemyProbability = Random.Range(0f, summOfAllProbability);
            if(enemyProbability < ProbabilityMaster.oilP)
            {
                Instantiate(oilEnemyPrefab, spawnPos, oilEnemyPrefab.transform.rotation);
            }
            else if(enemyProbability < (ProbabilityMaster.oilP + ProbabilityMaster.bzzP))
            {
                Instantiate(bzzEnemyPrefab, spawnPos, bzzEnemyPrefab.transform.rotation);
            }
            else
            {
                Instantiate(octoEnemyPrefab, spawnPos, octoEnemyPrefab.transform.rotation);
            }
        }
    }

    IEnumerator SpawnBattery()
    {
        float updateBatteryTime;
        while (true)
        {
            updateBatteryTime = Random.Range(ProbabilityMaster.timeBatteryStart, ProbabilityMaster.timeBatteryStart + 0.5f);
            yield return new WaitForSeconds(updateBatteryTime);
            float batteryProbability = Random.Range(0f, 100f);
            if(batteryProbability < ProbabilityMaster.batteryP)
            {
                int multplyForRoad = Random.Range(-1, 2);
                Vector3 spawnPos = new Vector3(multplyForRoad * spawnRangeX, 6.0f, batteryPrefab.transform.position.z);
                Instantiate(batteryPrefab, spawnPos, batteryPrefab.transform.rotation);
            }
        }
    }

    private void SpawnManagerBossStart()
    {
        bossStart = true;
    }

    IEnumerator SpawnBoss()
    {
        yield return new WaitForSeconds(4f);
        bossGameObject.SetActive(true);
    }
}
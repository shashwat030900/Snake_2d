using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.AI;

public class PowerUps : MonoBehaviour
{
    private MainPlayerController snakeController;
    public GameObject shieldPowerUpPrefab;
    private float shieldCooldown = 0f;
    public GameObject scoreBoostPowerUpPrefab;
    private float scoreBoostCooldown = 0f;
    public GameObject speedUpPowerUpPrefab;
    private float speedUpCooldown = 0f;

    public float powerUpSpawnTimeMin = 5f;
    public float powerUpSpawnTimeMax = 10f;

    void Start()
    {
        snakeController = FindObjectOfType<MainPlayerController>();
        SpawningPowerUps();
    }
    void Update()
    {
       
    }


    public void ActivateSheild()
    {
        if (shieldCooldown <= 0){
            snakeController.isShieldActive = true;
            shieldCooldown = 3f;
            StartCoroutine(ShieldCooldown());


        }
    }

    private IEnumerator ShieldCooldown()
    {
        while( shieldCooldown > 0f)
        {
            shieldCooldown -= Time.deltaTime;
            yield return null;
        }
    }


    public void ActivateScoreBoost()
    {
        if (scoreBoostCooldown <= 0f)
        {
            snakeController.isScoreBoostActive = true;
            scoreBoostCooldown = 3f;
            StartCoroutine(ScoreBoostCooldown());
        }
    }    
    private IEnumerator ScoreBoostCooldown()
    {
        while (scoreBoostCooldown > 0f)
        {
            scoreBoostCooldown -= Time.deltaTime;
            yield return null;
        }        
    }

    public void ActivateSpeedUp()
    {
        snakeController.isSpeedBoostActive = true;
        speedUpCooldown = 3f;
        StartCoroutine(SpeedUpCooldown());    
    }

    private IEnumerator SpeedUpCooldown()
    {
        while (speedUpCooldown > 0f)
        {
            speedUpCooldown -= Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator SpawningPowerUps()
    {
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
        while (true)
        {
        GameObject powerUpToSpawn = GetRandomPowerUp();
        Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(bottomLeft.x,topRight.x), UnityEngine.Random.Range(bottomLeft.y,topRight.y), 0f);
        Instantiate(powerUpToSpawn, spawnPosition, Quaternion.identity);

        float spwanTime = UnityEngine.Random.Range(powerUpSpawnTimeMin, powerUpSpawnTimeMax);
        yield return new WaitForSeconds(spwanTime);  
        }   
    }

    private GameObject GetRandomPowerUp()
    {
       int randomIndex = UnityEngine.Random.Range(0,3);

       switch ( randomIndex)
       {
            case 0:
                return shieldPowerUpPrefab;
            case 1:
                return scoreBoostPowerUpPrefab;
            case 2:
                return speedUpPowerUpPrefab;
            default:
                return null;        
       }
    }
}

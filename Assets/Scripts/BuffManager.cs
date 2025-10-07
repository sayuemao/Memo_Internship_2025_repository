using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BuffManager : MonoBehaviour
{
    public static BuffManager Instance { get; private set; }

    public Transform[] spawnBuffPoints;
    public GameObject[] buffPrefabs;

    public Transform[] spawnCoinPoints;
    public GameObject[] coinPrefabs;

    public Sprite speedUpSprite;
    public float speedUpAmount = 3f;
    public float speedUpDuration = 10f;
    
    public Sprite invincibleSprite;
    public float invincibleDuration = 10f;
    
    public Sprite flySprite;
    public float flyDuration = 10f;

    public Sprite healUpSprite;
    public float healUpDuration = 0f;
    
    public Sprite noneSprite;

    public float spawnBuffInterval = 5f;
    public float spawnBuffTimer = 0f;

    public float spawnCoinInterval = 3f;
    public float spawnCoinTimer = 0f;

    public float buffDuration = 10f;
    public float buffTimer = 0f;
    public enum BuffType
    {
        None,
        SpeedUp,
        Invincible,
        Fly,
        HealUp
    }

    public BuffType currentBuff = BuffType.None;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        currentBuff = BuffType.None;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.isGamePaused || GameManager.Instance.playerDead || GameManager.Instance.isPlayerWin)
        {           
            return;
        }
        spawnBuffTimer += Time.deltaTime;
        if (spawnBuffTimer > spawnBuffInterval)
        {
            spawnBuffTimer = 0f;
            int randomIndex = Random.Range(0, spawnBuffPoints.Length);
            int randomBuffIndex = Random.Range(0, buffPrefabs.Length);
            Instantiate(buffPrefabs[randomBuffIndex], spawnBuffPoints[randomIndex].position, spawnBuffPoints[randomIndex].rotation);
            AudioManager.Instance.PlaySFX(10);
        }

        spawnCoinTimer += Time.deltaTime;
        if (spawnCoinTimer > spawnCoinInterval)
        {
            spawnCoinTimer = 0f;
            int randomIndex = Random.Range(0, spawnCoinPoints.Length);
            int randomBuffIndex = Random.Range(0, coinPrefabs.Length);
            Instantiate(coinPrefabs[randomBuffIndex], spawnCoinPoints[randomIndex].position, spawnCoinPoints[randomIndex].rotation);
            AudioManager.Instance.PlaySFX(10);
        }

        switch (currentBuff)
        {
            case BuffType.None:
                buffTimer = 0f;
                break;
            case BuffType.SpeedUp:
                buffTimer += Time.deltaTime;
                PlayerController.Instance.moveSpeed += speedUpAmount;
                if (buffTimer>= buffDuration)
                {
                    currentBuff = BuffType.None;
                    buffTimer = 0f;
                    PlayerController.Instance.moveSpeed -= speedUpAmount;
                    GameManager.Instance.UpdateBuff(ChooseBuffSprite(currentBuff));
                }
                break;
            case BuffType.Invincible:
                buffTimer += Time.deltaTime;
                PlayerHealthController.Instance.isBuffInvincible = true;
                if (buffTimer >= buffDuration)
                {
                    currentBuff = BuffType.None;
                    buffTimer = 0f;
                    PlayerHealthController.Instance.isBuffInvincible = false;
                    GameManager.Instance.UpdateBuff(ChooseBuffSprite(currentBuff));
                }
                break;
            case BuffType.Fly:
                buffTimer += Time.deltaTime;
                PlayerController.Instance.canFly = true;
                if (buffTimer >= buffDuration)
                {
                    currentBuff = BuffType.None;
                    buffTimer = 0f;
                    PlayerController.Instance.canFly = false;
                    GameManager.Instance.UpdateBuff(ChooseBuffSprite(currentBuff));
                }
                break;
            case BuffType.HealUp:
                buffTimer += Time.deltaTime;
                PlayerHealthController.Instance.playerCurrentHealth = Mathf.Min(PlayerHealthController.Instance.playerCurrentHealth+1,PlayerHealthController.Instance.playerMaxHealth);
                GameManager.Instance.UpdateHealthDisplay(PlayerHealthController.Instance.playerCurrentHealth);
                if (buffTimer >= buffDuration)
                {
                    currentBuff = BuffType.None;
                    buffTimer = 0f;
                    GameManager.Instance.UpdateBuff(ChooseBuffSprite(currentBuff));
                }
                break;
            default:
                break;
        }
    }

    public void ApplyBuff(BuffType buff)
    {
        switch(currentBuff)
        {
            case BuffType.SpeedUp:
                PlayerController.Instance.moveSpeed -= speedUpAmount;
                break;
            case BuffType.Invincible:
                PlayerHealthController.Instance.isBuffInvincible = false;
                break;
            case BuffType.Fly:
                PlayerController.Instance.canFly = false;
                break;
            default:
                break;
        }

        currentBuff = buff;
        buffDuration = GetBuffDuration(buff);
        GameManager.Instance.UpdateBuff(ChooseBuffSprite(currentBuff));

    }
  
    private Sprite ChooseBuffSprite(BuffType buffType)
    {
        Sprite buffSprite = noneSprite;
        switch (buffType)
        {
            case BuffType.None:
                buffSprite = noneSprite;
                break;
            case BuffType.SpeedUp:
                buffSprite =  speedUpSprite;
                break;

            case BuffType.Invincible:
                buffSprite = invincibleSprite;
                break;
            case BuffType.Fly:
                buffSprite = flySprite;
                break;
            default:
                buffSprite = noneSprite;
                break;
        }
        return buffSprite;
    }

    private float GetBuffDuration(BuffType buffType)
    {
        float duration = 0f;
        switch (buffType)
        {
            case BuffType.None:
                duration = 0f;
                break;
            case BuffType.SpeedUp:
                duration = speedUpDuration;
                break;
            case BuffType.Invincible:
                duration = invincibleDuration;
                break;
            case BuffType.Fly:
                duration = flyDuration;
                break;
            case BuffType.HealUp:
                duration = healUpDuration;
                break;
            default:
                duration = 0f;
                break;
        }
        return duration;
    }
}

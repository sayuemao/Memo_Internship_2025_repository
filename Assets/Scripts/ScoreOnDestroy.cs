using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreOnDestroy : MonoBehaviour
{
    public int scoreValue = 100;

    private void OnDestroy()
    {
        GameManager.Instance.AddScore(scoreValue);
        switch(scoreValue)
        {
            case 50:
                AudioManager.Instance.PlaySFX(2);
                break;
            case 100:
                AudioManager.Instance.PlaySFX(1);
                break;
            case 150:
                AudioManager.Instance.PlaySFX(0);
                break;
            default:
                AudioManager.Instance.PlaySFX(2);
                break;
        }
    }
}

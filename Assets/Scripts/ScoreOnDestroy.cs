using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreOnDestroy : MonoBehaviour
{
    public int scoreValue = 100;

    private void OnDestroy()
    {
        GameManager.Instance.AddScore(scoreValue);
    }
}

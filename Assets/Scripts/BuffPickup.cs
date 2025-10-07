using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffPickup : MonoBehaviour
{
    public BuffManager.BuffType buffType = BuffManager.BuffType.None;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            BuffManager.Instance.ApplyBuff(buffType);
            //GameManager.Instance.UpdateBuff(buffType);
            AudioManager.Instance.PlaySFX(6);
            Destroy(gameObject);
        }
    }

}

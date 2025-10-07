using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDropItem
{
    public int ScoreValue { get; set; }
    public void DropItem(Transform dropPosition);
}

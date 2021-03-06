﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour {

    public bool hasFlower;
    public bool hasMask;

    public bool canPass;

    void OnEnable()
    {
        ItemPickEvent.OnItemPick += CheckWhichItem;
    }

    private void ChecksIfGathered()
    {
        if(hasFlower && hasMask )
        {
            canPass = true;
        }
        else
        {
            canPass = false;
        }
    }

    private void CheckWhichItem(int i)
    {
        if(i == 1)
        {
            AddFlowers();
        }
        if(i == 2)
        {
            AddMask();
        }
    }

    public void AddFlowers()
    {
        hasFlower = true;
        ChecksIfGathered();
    }

    public void AddMask()
    {
        hasMask = true;
        ChecksIfGathered();
    }

    private void OnDestroy()
    {
        ItemPickEvent.OnItemPick -= CheckWhichItem;
    }
}

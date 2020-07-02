﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance = null;

    public GameObject SameColorBomb;
    public GameObject SameLineBomb;
    public GameObject SameSideBomb;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void GenerateItem()
    {
        List<Tile> tileList = GameManager.Instance.TileList;

        int n = Random.Range(1,100);
        int idx;

        while (true)
        {
            idx = Random.Range(0, tileList.Count);
            if (tileList[idx].child == null)
                break;
        }

        GameObject bomb = null;
        CustomVariables.TILE type = CustomVariables.TILE.EMPTY;

        if (n <= 33)
        {
            bomb = Instantiate(SameColorBomb, tileList[idx].transform);
            type = CustomVariables.TILE.COLOR_BOMB;

        }
        else if (n <= 66)
        {
            bomb = Instantiate(SameLineBomb, tileList[idx].transform);
            type = CustomVariables.TILE.LINE_BOMB;
        }
        else if (n <= 99)
        {
            bomb = Instantiate(SameSideBomb, tileList[idx].transform);
            type = CustomVariables.TILE.SIDE_BOMB;
        }
        bomb.transform.localScale = new Vector3(0.8f, 100.0f, 0.8f);
        bomb.transform.position += bomb.transform.up * 0.5f;
        bomb.GetComponent<MeshRenderer>().material.color = tileList[idx].transform.GetComponent<MeshRenderer>().material.color;
        tileList[idx].SetChild(bomb, type);
    }

    public void Activate(Transform target, CustomVariables.TILE type)
    {
        Tile tile = GameManager.Instance.GetTile(target);

        if (type == CustomVariables.TILE.COLOR_BOMB)
        {
            StartCoroutine(BingoManager.Instance.BingoEventByColorBomb(tile.child.GetComponent<MeshRenderer>().material.color));
        }
        else if (type == CustomVariables.TILE.LINE_BOMB)
        {
            StartCoroutine(BingoManager.Instance.BingoEventByLineBomb(tile));
        }
        else if (type == CustomVariables.TILE.SIDE_BOMB)
        {
            StartCoroutine(BingoManager.Instance.BingoEventBySideBomb(tile));
        }

        tile.type = CustomVariables.TILE.EMPTY;
        Destroy(tile.child);
    }

}

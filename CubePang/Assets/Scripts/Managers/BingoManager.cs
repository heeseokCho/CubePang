using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BingoManager : MonoBehaviour
{
    public static BingoManager Instance = null;  // 싱글톤 인스턴스

    List<int> centerIndices = new List<int>() { 10, 23, 26, 27, 30, 43 };

    public int totalItemSummonCount;
    private float currentItemSummonCount;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public bool CheckLineEvent(Tile tile)
    {
        List<Tile> tileList = GameManager.Instance.TileList;

        List<Tile> sameSideTiles;
        sameSideTiles = new List<Tile>();
        List<Tile>[] sameLineTiles;
        sameLineTiles = new List<Tile>[6];
        List<Tile> bingoTiles;

        for (int i = 0; i < 6; ++i)
            sameLineTiles[i] = new List<Tile>();

        bingoTiles = new List<Tile>();

        int cubeCount = GameManager.Instance.CubeCount;
        float radius = CubeManager.Instance.Radius * 2;

        Vector3 upVector = tile.transform.up;
        // 빙고의 조건
        // - 같은 면에 있다 -> Up Vectr 가 같다
        for (int i = 0; i < cubeCount * cubeCount * 6; ++i)
        {
           // tileList[i].SaveCurTileColor();
            if (CustomVariables.IsSimillerVectorDir(upVector, tileList[i].transform.up))
                sameSideTiles.Add(tileList[i]);
        }
        // - 같은 줄에 있다 -> Up Vectr를 제외한 평면에서 한 좌표가 같다
        for (int i = -cubeCount / 2; i <= cubeCount / 2; ++i)
        {
            int idx = i + cubeCount / 2;
            for (int j = 0; j < sameSideTiles.Count; ++j)
            {
                
                if (0.5f< upVector.x ||  upVector.x < -0.5f)
                {
                    if (CustomVariables.IsSimillerValue(radius * i, sameSideTiles[j].transform.position.y))   // 가로
                        sameLineTiles[idx].Add(sameSideTiles[j]);
                    if (CustomVariables.IsSimillerValue(radius * i, sameSideTiles[j].transform.position.z))   // 세로
                        sameLineTiles[3 + idx].Add(sameSideTiles[j]);
                }
                if (0.5f < upVector.y || upVector.y < -0.5f)
                {
                    if (CustomVariables.IsSimillerValue(radius * i, sameSideTiles[j].transform.position.x))   // 가로
                        sameLineTiles[idx].Add(sameSideTiles[j]);
                    if (CustomVariables.IsSimillerValue(radius * i, sameSideTiles[j].transform.position.z))   // 세로
                        sameLineTiles[3 + idx].Add(sameSideTiles[j]);
                }
                if (0.5f < upVector.z || upVector.z < -0.5f)
                {
                    if (CustomVariables.IsSimillerValue(radius * i, sameSideTiles[j].transform.position.y))   // 가로
                        sameLineTiles[idx].Add(sameSideTiles[j]);
                    if (CustomVariables.IsSimillerValue(radius * i, sameSideTiles[j].transform.position.x))   // 세로
                        sameLineTiles[3 + idx].Add(sameSideTiles[j]);
                }

            }
        }

        for (int i = 0; i < 6; ++i)
        {
            if (sameLineTiles[i].Count != GameManager.Instance.CubeCount)
                continue;

            bool bingo = true;
            Color compareColor = sameLineTiles[i][0].GetComponent<MeshRenderer>().material.color;
            for(int j = 0; j < GameManager.Instance.CubeCount; ++j)
            {
                if (false == sameLineTiles[i][j].IsSameColor(compareColor))
                {
                    bingo = false;
                    break;
                }
            }

            if( true == bingo)
            {
                foreach (Tile t in sameLineTiles[i] )
                    bingoTiles.Add(t);
            }
        }

        if (bingoTiles.Count == 0)
            return false;

        for (int i = 0; i < bingoTiles.Count; ++i)
        {
            bingoTiles[i].TakeDamage();
        }

        CubeManager.Instance.OccurCubeEarthQuake(0.2f, 720.0f);
        StartCoroutine(CameraManager.Instance.ShakeCamera(0.2f, 0.08f));

        return true;
    }

    public bool CheckSameColorBombEvent(Color color)
    {
        List<Tile> tileList = GameManager.Instance.TileList;
        List<Tile> bingoTiles = new List<Tile>();

        foreach(Tile tile in tileList)
        {
            if (tile.IsSameColor(color))
                bingoTiles.Add(tile);
        }

        if (bingoTiles.Count == 0)
            return false;

        foreach (Tile tile in bingoTiles)
        {
            tile.TakeDamage();
        }

        return true;
    }

    public bool CheckSameLineBombEvent(Tile target)
    {
        List<Tile> tileList = GameManager.Instance.TileList;
        List<Tile> bingoTiles = new List<Tile>();

        Vector3 upVector = target.transform.up;
        if (0.5f < upVector.x || upVector.x < -0.5f)
        {
            foreach (Tile tile in tileList)
            {
                if (CustomVariables.IsSimillerVectorDir(upVector, tile.transform.up) &&
                    CustomVariables.IsSimillerValue(tile.transform.position.y, target.transform.position.y))
                    bingoTiles.Add(tile);

                if (CustomVariables.IsSimillerVectorDir(upVector, tile.transform.up) &&
                    CustomVariables.IsSimillerValue(tile.transform.position.z, target.transform.position.z))
                    bingoTiles.Add(tile);
            }
        }
        if (0.5f < upVector.y || upVector.y < -0.5f)
        {
            foreach (Tile tile in tileList)
            {

                if (CustomVariables.IsSimillerVectorDir(upVector, tile.transform.up) &&
                    CustomVariables.IsSimillerValue(tile.transform.position.x, target.transform.position.x))
                    bingoTiles.Add(tile);

                if (CustomVariables.IsSimillerVectorDir(upVector, tile.transform.up) &&
                    CustomVariables.IsSimillerValue(tile.transform.position.z, target.transform.position.z))
                    bingoTiles.Add(tile);
            }
        }
        if (0.5f < upVector.z || upVector.z < -0.5f)
        {
            foreach (Tile tile in tileList)
            {

                if (CustomVariables.IsSimillerVectorDir(upVector, tile.transform.up) &&
                    CustomVariables.IsSimillerValue(tile.transform.position.y, target.transform.position.y))
                    bingoTiles.Add(tile);

                if (CustomVariables.IsSimillerVectorDir(upVector, tile.transform.up) &&
                    CustomVariables.IsSimillerValue(tile.transform.position.x, target.transform.position.x))
                    bingoTiles.Add(tile);
            }
        }

        if (bingoTiles.Count == 0)
            return false;

        foreach (Tile tile in bingoTiles)
        {
            tile.TakeDamage();
        }

        return true;
    }
    
    public bool CheckSameSideBombEvent(Tile target)
    {
        List<Tile> tileList = GameManager.Instance.TileList;
        List<Tile> bingoTiles = new List<Tile>();

        Vector3 upVector = target.transform.up;
        foreach(Tile tile in tileList)
        {
            if (CustomVariables.IsSimillerVectorDir(upVector, tile.transform.up))
                bingoTiles.Add(tile);
        }

        if (bingoTiles.Count == 0)
            return false;

        foreach (Tile tile in bingoTiles)
        {
            tile.TakeDamage();
        }

        return true;
    }

    public IEnumerator BingoEvent(CustomVariables.TILE type, Tile tile = null)
    {
        yield return new WaitForFixedUpdate();
        if (type == CustomVariables.TILE.EMPTY)
        {
            for (int i = 0; i < centerIndices.Count; ++i)
            {
                while (true == CheckLineEvent(GameManager.Instance.TileList[centerIndices[i]]))
                    yield return new WaitForSeconds(0.4f);
            }
        }
    }

    public IEnumerator BingoEventByColorBomb(Color color)
    {
        yield return new WaitForFixedUpdate();

        if (CheckSameColorBombEvent(color))
            yield return new WaitForSeconds(0.4f);
    }

    public IEnumerator BingoEventByLineBomb(Tile tile)
    {
        yield return new WaitForFixedUpdate();
        if (CheckSameLineBombEvent(tile))
            yield return new WaitForSeconds(0.4f);
    }

    public IEnumerator BingoEventBySideBomb(Tile tile)
    {
        yield return new WaitForFixedUpdate();
        if (CheckSameSideBombEvent(tile))
            yield return new WaitForSeconds(0.4f);
    }


    public Tile GetMiddleTile()
    {
        if (GameManager.Instance.CubeCount != 3)
            return null;

        centerIndices.Shuffle();

        for (int i = 0; i < centerIndices.Count; ++i)
        {
            if (GameManager.Instance.TileList[centerIndices[i]].type == CustomVariables.TILE.EMPTY)
                return GameManager.Instance.TileList[centerIndices[i]];
        }

        return null;
    }
}

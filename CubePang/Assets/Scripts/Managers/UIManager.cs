using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance = null;

    int score;
    List<Tile> tileList;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        UpdateScore();
        UpdateColorCount();
    }

    public void UpdateScore()
    {
        // transform.GetChild(0) : Canvas
        score = GameManager.Instance.Score;
        transform.GetChild(0).Find("ScoreText").GetComponent<Text>().text = "Score " + score.ToString();
    }

    public void UpdateColorCount()
    {
        string text; //= transform.GetChild(0).Find("ColorCountText").GetComponent<Text>().text;
        text = "";
        tileList = GameManager.Instance.TileList;
        int[] colors = new int[GameManager.Instance.Level];
        foreach (Tile tile in tileList)
        {
            if (tile.IsSameColor(Color.red)) ++colors[0];
            else if (tile.IsSameColor(Color.green)) ++colors[1];
            else if (tile.IsSameColor(Color.blue)) ++colors[2];
            else if (tile.IsSameColor(Color.yellow)) ++colors[3];
            else if (tile.IsSameColor(Color.cyan)) ++colors[4];
            else if (tile.IsSameColor(Color.magenta)) ++colors[5];
        }
        for(int i = 0; i < GameManager.Instance.Level; ++i)
        {
            if (0 == i) text += "\n<color=#ff0000ff>Red : " + colors[i].ToString() + "</color>";
            if (1 == i) text += "\n<color=#00ff00ff>Green : " + colors[i].ToString() + "</color>";
            if (2 == i) text += "\n<color=#0000ffff>Blue : " + colors[i].ToString() + "</color>";
            if (3 == i) text += "\n<color=#ffff00ff>Yellow : " + colors[i].ToString() + "</color>";
            if (4 == i) text += "\n<color=#00ffffff>Cyan : " + colors[i].ToString() + "</color>";
            if (5 == i) text += "\n<color=#ff00ffff>Magenta : " + colors[i].ToString() + "</color>";
        }
        transform.GetChild(0).Find("ColorCountText").GetComponent<Text>().text = text;
    }
}

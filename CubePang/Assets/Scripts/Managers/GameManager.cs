using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    public LayerMask tileLayer;

    private Color originTileColor;


    public int CubeCount;        // 큐브의 규격 : 3X3 => 3,  4X4 => 4  
    public bool PlayerTurnEnd { get; set; }
    public int Level { get; set; }
    public List<Tile> TileList { get; set; }   // 타일을 관리할 리스트

    public bool IsCubeSelected { get; set; }
    public CustomVariables.TURN PlayerTurn { get; set; }
    public int currentTurn { get;set; }

    public int Score { get; set; }

    private int itemPercentage;

    private void Awake()
    {
        // 중복된 인스턴스 객체 생성을 방지합니다.
        if (Instance == null)
            Instance = this;

        Level = 4;
        itemPercentage = 20;

        PlayerTurn = CustomVariables.TURN.PLAYER_TURN;
        TileList = new List<Tile>();

        currentTurn = 0;
        IsCubeSelected = false;
        PlayerTurnEnd = false;
    }

    void Update()
    {
        ProcessInput();
        ProcessTurn();
    }

    private void ProcessInput()
    {
        if (false == IsCubeSelected)
            CameraManager.Instance.RotateInput();
        CameraManager.Instance.ProcessInput();
    }

    private void ProcessTurn()
    {
        if (PlayerTurn == CustomVariables.TURN.PLAYER_TURN)
        {       // 플레이어의 조작
            if (PlayerTurnEnd)
            {
                PlayerTurnEnd = false;
                NextTurn();
            }
            if ( false == CubeManager.Instance.IsWorldShaking)
                CubeManager.Instance.ProcessMouseInput();
        }
        else if (PlayerTurn == CustomVariables.TURN.EVENT_TURN)
        {       // 아이템 타일 파괴 이벤트
            NextTurn();
        }
        else if (PlayerTurn == CustomVariables.TURN.PREPARE_TURN)
        {
            if (200 < Score)
                Level = 6;
            else if (100 < Score)
                Level = 5;

            if (Random.Range(0, 100) < itemPercentage)
                ItemManager.Instance.GenerateItem();

            NextTurn();
        }
        else
        {
            Debug.LogError("잘못된 접근입니다.");
        }
       if( false == IsCubeSelected)
        ConfirmBingo();
        UIManager.Instance.UpdateScore();
        UIManager.Instance.UpdateColorCount();

    }

    public void NextTurn()
    {
        if (PlayerTurn == CustomVariables.TURN.PLAYER_TURN)
            PlayerTurn = CustomVariables.TURN.EVENT_TURN;
        else if (PlayerTurn == CustomVariables.TURN.EVENT_TURN)
            PlayerTurn = CustomVariables.TURN.PREPARE_TURN;
        else if (PlayerTurn == CustomVariables.TURN.PREPARE_TURN)
            PlayerTurn = CustomVariables.TURN.PLAYER_TURN;
        else
            Debug.LogError("잘못된 턴");
    }

    public void ConfirmBingo()
    {
        StartCoroutine(BingoManager.Instance.BingoEvent(CustomVariables.TILE.EMPTY));
    }

    public Tile GetTile(Transform target)
    {
        return TileList.Find(x => x.transform == target);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleStateManager : MonoBehaviour
{
    [Header("State")]
    BattleBaseState currentState;
    public BattleStartState startState = new BattleStartState();
    public BattlePlayerTurnState playerTurnState = new BattlePlayerTurnState();
    public BattleEnemyTurnState enemyTurnState = new BattleEnemyTurnState();
    public BattleWonState wonState = new BattleWonState();
    public BattleLostState lostState = new BattleLostState();

    [Header("Variable Data")]
    public GameObject playerPrefabs;
    public GameObject enemyPrefabs;
    public Transform playerBattleStation;
    public Transform enemyBattleStation;
    public Unit playerUnit;
    public Unit enemyUnit;
    public Text dialogueText;
    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;
    public string menang;
    public string kalah;

    // Start is called before the first frame update
    void Start()
    {
        currentState = startState;
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(BattleBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }
}

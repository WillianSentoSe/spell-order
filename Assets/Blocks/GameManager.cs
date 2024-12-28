using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager main;

    #region Properties

    public Player playerPrefab;

    protected Level currentLevel;
    protected Player player;
    protected GameActionsManager actionManager;

    #endregion

    #region Getters and Setters

    public Level CurrentLevel { get { return currentLevel; } }
    public GameActionsManager Actions { get { return actionManager; } }
    public Player Player { get { return GetPlayer(); } }

    #endregion

    #region Execution

    public void Awake()
    {
        if (main == null) main = this;
        else if (main != this) Destroy(gameObject);

        actionManager = new GameActionsManager();
    }
    protected void OnEnable()
    {
    }

    public void Start()
    {
        // Player.Actions.OnDeath += RestartGame;
    }

    #endregion

    #region Public Methods

    public void SetLevel(Level _level)
    {
        currentLevel = _level;
    }

    #endregion

    #region Gameplay

    public void StartGame()
    {

    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        string _scene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(_scene);
    }

    public void EndGame()
    {

    }

    public Player GetPlayer()
    {
        if (player == null) player = GameObject.FindObjectOfType<Player>();
        return player;
    }

    #endregion
}

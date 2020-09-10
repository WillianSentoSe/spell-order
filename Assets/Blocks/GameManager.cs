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

    #endregion

    #region Getters and Setters

    public Level CurrentLevel { get { return currentLevel; } }

    #endregion

    #region Execution

    public void Awake()
    {
        if (main == null) main = this;
        else if (main != this) Destroy(gameObject);
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
        string _scene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(_scene);
    }

    public void EndGame()
    {

    } 

    #endregion
}

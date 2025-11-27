using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : Singleton<GameStateManager>
{
    [SerializeField] private InputReader _input;
    private PlayerLife _playerLife;
    
    public override void Awake()
    {
        base.Awake();
        _playerLife = FindObjectOfType<PlayerLife>();
    }

    private void OnEnable()
    {
        _input.RestartPerformed += Input_OnRestartPerformed;
        _input.QuitPerformed += Input_OnQuitPerformed;
        _input.LoadCheckpointPerformed += Input_OnLoadCheckpointPerformed;
        _input.SkipLevelPerformed += Input_OnSkipLevelPerformed;
    }

    private void Input_OnSkipLevelPerformed()
    {
        int nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextLevel > 2)
        {
            nextLevel = 2;
            return;
        }
        
        SceneManager.LoadScene(nextLevel);
    }

    private void OnDisable()
    {
        _input.RestartPerformed -= Input_OnRestartPerformed;
        _input.QuitPerformed -= Input_OnQuitPerformed;
        _input.LoadCheckpointPerformed -= Input_OnLoadCheckpointPerformed;
        _input.SkipLevelPerformed -= Input_OnSkipLevelPerformed;
    }

    private void Input_OnRestartPerformed()
    {
        SceneManager.LoadScene(0);
    }
    
    private void Input_OnQuitPerformed()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
        
    private void Input_OnLoadCheckpointPerformed()
    {
        if (_playerLife != null)
        {
            _playerLife.Respawn();
        }    
    }

}

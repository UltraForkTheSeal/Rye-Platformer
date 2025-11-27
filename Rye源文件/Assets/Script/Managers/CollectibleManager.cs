using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectibleManager : Singleton<CollectibleManager>
{
    private int _currentSceneIndex;
    private int[] _totalCollectiblesInLevel;
    private int[] _currentCollectiblesInLevel;
    private RyeCollect[] _sceneRyeList;

    public override void Awake()
    {
        base.Awake();
        
        _sceneRyeList = new RyeCollect[10];
        _totalCollectiblesInLevel = new int[SceneManager.sceneCountInBuildSettings];
        _currentCollectiblesInLevel = new int[SceneManager.sceneCountInBuildSettings];
    }


    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneManager_OnSceneLoaded;
        SceneManager.sceneUnloaded += SceneManager_OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneManager_OnSceneLoaded;
        SceneManager.sceneUnloaded -= SceneManager_OnSceneUnloaded;
    }

    private void SceneManager_OnSceneLoaded(Scene loadedScene, LoadSceneMode arg1)
    {
        _currentSceneIndex = loadedScene.buildIndex;

        _sceneRyeList = FindObjectsOfType<RyeCollect>();
        foreach (var rye in _sceneRyeList)
        {
            rye.OnCollected += Rye_OnCollected;
        }


        // 进入关卡 更新关卡当前、所有收集品数量
        if (_sceneRyeList.Length > 0)
        {
            _currentCollectiblesInLevel[_currentSceneIndex] = 0;
            _totalCollectiblesInLevel[_currentSceneIndex] = _sceneRyeList.Length;
        }
    }
    
    private void SceneManager_OnSceneUnloaded(Scene arg0)
    {
        _sceneRyeList = null;
    }

    private void Rye_OnCollected()
    {
        _currentCollectiblesInLevel[_currentSceneIndex]++;
        if (_currentCollectiblesInLevel[_currentSceneIndex] >= _totalCollectiblesInLevel[_currentSceneIndex])
        {
            _currentCollectiblesInLevel[_currentSceneIndex] = _totalCollectiblesInLevel[_currentSceneIndex];
        }
    }

    public int GetTotalCollectiblesInLevel(int sceneIndex)
    {
        return _totalCollectiblesInLevel[sceneIndex];
    }

    public int GetCurrentCollectiblesInLevel(int sceneIndex)
    {
        return _currentCollectiblesInLevel[sceneIndex];
    }

}

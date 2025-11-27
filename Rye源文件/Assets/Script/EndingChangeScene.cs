using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EndingChangeScene : MonoBehaviour
{
    [SerializeField] private int _sceneIndex;
    [SerializeField] private float _changeSceneDelay;
    private WaitForSeconds _changeSceneWait;
    private SceneChangeFade _fade;
    [SerializeField] private UnityEvent _onFadeInStarted;
    [SerializeField] private UnityEvent _onSceneLoaded;

    private void Awake()
    {
        _changeSceneWait = new WaitForSeconds(_changeSceneDelay);
        _fade = FindObjectOfType<SceneChangeFade>();
    }

    
    private void OnEnable()
    {
        Change();
    }

    private void Change()
    {
        StartCoroutine(ChangeSceneCoroutine());
    }
    
    private IEnumerator ChangeSceneCoroutine()
    {
        _fade.TriggerFadeOut();
        _onFadeInStarted?.Invoke();
        yield return _changeSceneWait;
        _onSceneLoaded?.Invoke();
        SceneManager.LoadScene(_sceneIndex);
    }
}

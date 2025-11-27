using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathDetect : MonoBehaviour,IHazard
{
    
    public Action PlayerKilled { get; set; }
    private BoxCollider2D m_BoxCollider2D;
    
    void Start()
    {
        m_BoxCollider2D = GetComponent<BoxCollider2D>();
    }
    
	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.CompareTag("Player"))
        {
            //GameObject player = collision.gameObject;
            // Debug.Log("Player is dead");
            LevelEffectPerform();
            //player.GetComponent<CharacterControl1>().ResetPlayerCondition_Reborn();
            PlayerKilled?.Invoke();
		}
	}

    private void LevelEffectPerform()
    { 
        
    }

}

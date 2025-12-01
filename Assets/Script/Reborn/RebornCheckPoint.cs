using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RebornCheckPoint : MonoBehaviour
{
    public int CheckPointID;

	private void OnTriggerEnter2D(Collider2D collision)
	{
  //       if (collision.CompareTag("Player"))
  //       {
  //           GameObject player = collision.gameObject;
  //           player.GetComponent<CharacterControl1>().UpdateCheckPoint(this.CheckPointID, this.transform);
		// }

        if (collision.TryGetComponent(out PlayerLife playerLife))
        {
	        playerLife.UpdateCheckPoint(this.CheckPointID, this.transform);
        }
	}
}

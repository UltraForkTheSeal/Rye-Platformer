using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayCollectibles : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Text t = GetComponent<Text>();
        t.text = "";
        for (int i = 0; i < 3; i++)
        {
            t.text = t.text +CollectibleManager.Instance.GetCurrentCollectiblesInLevel(i).ToString() +
                 " / " + CollectibleManager.Instance.GetTotalCollectiblesInLevel(i).ToString() + "\n";
        }
        
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

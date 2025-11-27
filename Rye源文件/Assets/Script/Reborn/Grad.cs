using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Grad : MonoBehaviour
{

    Image image;
    public Color startColor;
	public Color endColor;
    public float duration;
    public float delay;
	public AnimationCurve curve;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }
	private void OnEnable()
	{
        timer = 0;
	}

    // Update is called once per frame
    void Update()
    {
        float rate = curve.Evaluate((timer - delay) / duration);
        if (rate > 0)
            if (rate < 1)
            {
                image.color = Color.Lerp(startColor, endColor, rate);
            }
            else
            {
                image.color = endColor;
                this.enabled = false;
            }
		timer += Time.deltaTime;
	}
}

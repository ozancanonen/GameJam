using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskAnimator : MonoBehaviour
{
    public Sprite[] frames;
    public float fps;
    public float hold1;
    public float hold2;
    private float fpsSin;

    void Start()
    {
        StartCoroutine(Animate());
    }
    private void Update()
    {
    }

    IEnumerator Animate()
    {
        while (gameObject.activeSelf)
        {
            for (int i = 0; i < frames.Length; i++)
            {
                gameObject.GetComponent<SpriteMask>().sprite = frames[i];
                yield return new WaitForSeconds(fps);
            }
                yield return new WaitForSeconds(hold1);
            for (int i = frames.Length-1; i > -1; i--)
            {
                gameObject.GetComponent<SpriteMask>().sprite = frames[i];
                yield return new WaitForSeconds(fps);
            }
                yield return new WaitForSeconds(hold2);
        }
    }
}

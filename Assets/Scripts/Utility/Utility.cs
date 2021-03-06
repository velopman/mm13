using System.Collections;
using UnityEngine;

public class Utility
{
    static public IEnumerator frameFreeze(float timeScale, float waitTime)
    {
        Time.timeScale = timeScale;

        yield return new WaitForSeconds(waitTime * timeScale);

        Time.timeScale = 1f;
    }
}

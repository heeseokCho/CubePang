using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public IEnumerator Shake(float strength)
    {
        if (transform.position == Vector3.zero)
            yield break;

        Vector3 startPos = transform.position;
        Vector3 direction= transform.position.normalized;
        float period = 0.0f;

        while (true)
        {
            period += Time.deltaTime *strength;
            transform.position = startPos + direction * Mathf.Sin( Mathf.PI/180* period);

            if (period > 180.0f)
            {
                transform.position = startPos;
                break;
            }
            yield return null;
        }
        if(true == CubeManager.instance.IsWorldShaking)
        {
            yield return new WaitForSeconds(0.1f);
            CubeManager.instance.IsWorldShaking = false;
        }

    }
}

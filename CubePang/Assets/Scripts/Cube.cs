using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public IEnumerator Shake(float magnitude = 1.0f, float speed = 360.0f)
    {
        if (transform.position == Vector3.zero)
            yield break;

        Vector3 startPos = transform.position;
        Vector3 direction= transform.position.normalized;
        float period = 0.0f;

        while (true)
        {
            period += Time.deltaTime * speed;
            transform.position = startPos + direction* magnitude * Mathf.Sin( Mathf.PI/180* period);

            if (period > 180.0f)
            {
                transform.position = startPos;
                break;
            }
            yield return null;
        }
        if(true == CubeManager.Instance.IsWorldShaking)
        {
            yield return new WaitForSeconds(0.1f);
            CubeManager.Instance.IsWorldShaking = false;
        }

    }
}

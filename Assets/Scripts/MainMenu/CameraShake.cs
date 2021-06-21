using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public IEnumerator CamShake()
    {
        Vector3 originalPos = transform.position;
        float posX = originalPos.x;
        float posY = originalPos.y;
        float posZ = originalPos.z;

        transform.position = new Vector3(posX - 0.15f, posY + 0.05f, posZ);
        yield return new WaitForSeconds(0.025f);
        transform.position = new Vector3(posX + 0.15f, posY - 0.05f, posZ);
        yield return new WaitForSeconds(0.025f);
        transform.position = new Vector3(posX - 0.15f, posY + 0.05f, posZ);
        yield return new WaitForSeconds(0.025f);
        transform.position = new Vector3(posX + 0.15f, posY - 0.05f, posZ);
        yield return new WaitForSeconds(0.025f);
        transform.position = originalPos;
    }
}

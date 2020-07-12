using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSlightly : MonoBehaviour
{
    public float rotationTime = 0.5f;
    public int rotationAmount = 1;

    private IEnumerator RotateSlightlyFunc()
    {
        float currentRotationZ = gameObject.transform.rotation.eulerAngles.z;

        while (true)
        {
            rotationAmount *= -1;
            transform.rotation = Quaternion.Euler(0, 0, currentRotationZ + rotationAmount);

            yield return new WaitForSeconds(rotationTime);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("RotateSlightlyFunc");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscRotator : MonoBehaviour
{
    [SerializeField] float rotateSpeed;
    Vector3 rotateVector;
    
    // Start is called before the first frame update
    void Start()
    {
        rotateVector = new Vector3(0, 0, rotateSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotateVector);
    }
}

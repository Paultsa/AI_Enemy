using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGun : MonoBehaviour
{

    public float currentAngle;
    public bool rotating;
    public float rotateDuration;
    public float counter;
    public float _xAngle;
    public float xAngle
    {
        get { return _xAngle;  }

        set
        {
            Debug.Log("Uusi kulma on: " + value);
            _xAngle = value;
            rotating = true;
            counter = 0;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        if(counter > rotateDuration && rotating)
        {
            rotating = false;
        }
        currentAngle = Mathf.LerpAngle(transform.localRotation.eulerAngles.x, xAngle, counter / rotateDuration);
        transform.localEulerAngles = new Vector3(currentAngle, 0, 0);
    }
}

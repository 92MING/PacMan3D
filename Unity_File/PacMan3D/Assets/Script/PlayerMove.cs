using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    private float forceAmount = 5f;

    private bool invisibleMode = false;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Forward();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            Left();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Backward();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Right();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            if (!invisibleMode)
            {
                InvisibleMode();
            }
        }
    }

    private void Forward()
    {
        Rigidbody rigidbody = transform.GetComponent<Rigidbody>();
        rigidbody.AddForce(Vector3.forward * forceAmount, ForceMode.Impulse);
    }

    private void Backward()
    {
        Rigidbody rigidbody = transform.GetComponent<Rigidbody>();
        rigidbody.AddForce(Vector3.back * forceAmount, ForceMode.Impulse);
    }

    private void Left()
    {
        Rigidbody rigidbody = transform.GetComponent<Rigidbody>();
        rigidbody.AddForce(Vector3.left * forceAmount, ForceMode.Impulse);
    }

    private void Right()
    {
        Rigidbody rigidbody = transform.GetComponent<Rigidbody>();
        rigidbody.AddForce(Vector3.right * forceAmount, ForceMode.Impulse);
    }

    private void Jump()
    {
        Rigidbody rigidbody = transform.GetComponent<Rigidbody>();
        rigidbody.AddForce(Vector3.up * forceAmount, ForceMode.Impulse);
    }

    private void InvisibleMode()
    {
        float invisibleTime = 5f;
        IEnumerator InvisibleModeCountDown()
        {
            invisibleMode = true;
            yield return new WaitForSeconds(invisibleTime);
            invisibleMode = false;
        }

        StartCoroutine(InvisibleModeCountDown());
    }
}

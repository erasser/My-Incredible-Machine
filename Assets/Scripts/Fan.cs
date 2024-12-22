using System;
using UnityEngine;
using static GameController;

public class Fan : MonoBehaviour
{
    public Transform fanRotatorTransform;
    public float fanRotatorSpeed = 1000;
    public float fanPower = 2000;
    [Range(0, 90)]
    public float fanConeAngle = 45; 
    Vector2 _fanForward;
    float _dotThreshold;

    void Start()
    {
        _fanForward = - fanRotatorTransform.forward;
        _dotThreshold = Mathf.Cos(fanConeAngle * Mathf.Deg2Rad / 2);
    }

    void Update()
    {
        fanRotatorTransform.Rotate(Vector3.back, Time.deltaTime * fanRotatorSpeed);
    }

    void FixedUpdate()
    {
        foreach (Rigidbody2D ballRb2D in BallsRigidbodies)
        {
            Vector2 toBallDirection = ballRb2D.transform.position - fanRotatorTransform.position;
            var dot = Vector2.Dot(_fanForward, toBallDirection.normalized);

            Debug.DrawRay(fanRotatorTransform.position, toBallDirection, Color.magenta);

            var dotCoefficient = Map(dot, _dotThreshold, 1, 0, 1);

            if (dotCoefficient > 0)
            {
                var force = fanPower * dotCoefficient / toBallDirection.sqrMagnitude * toBallDirection;
                ballRb2D.AddForce(force * Time.deltaTime, ForceMode2D.Force);
            }
        }
    }

    static float Map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s-a1)*(b2-b1)/(a2-a1);
    }
}

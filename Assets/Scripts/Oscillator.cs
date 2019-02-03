using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f;

    Vector3 startingPos;
    float movementFactor;

    private void Start()
    {
        startingPos = transform.position;
    }

    private void Update()
    {
        if(period <= 0f) { return; }

        float cycles = Time.time / period;
        const float TAU = Mathf.PI * 2;
        float rawSinWave = Mathf.Sin(cycles * TAU);

        movementFactor = rawSinWave / 2f + 0.5f;
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
    }
}
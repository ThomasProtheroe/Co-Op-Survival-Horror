using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Crosshair : MonoBehaviour
{
    private Vector3 mousePosition;
    [SerializeField]
    private float moveSpeed;

    // Update is called once per frame
    void Update()
    {
        mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.position = Vector2.Lerp(transform.position, mousePosition, moveSpeed);
    }
}

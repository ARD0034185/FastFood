using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Vector3 offSet;
    public float sensibilidad;
    private Transform target;
    [Range(0, 1)]public float lerpValue;
    void Start()
    {
        target = GameObject.Find("Player").transform;
    }

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + offSet, lerpValue);
        //Para que gire la camara en la direccion que mire el jugador
        offSet = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * sensibilidad, Vector3.up) * offSet;


        //Para que apunte siempre a nuestro jugador
        transform.LookAt(target);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushRigiBody : MonoBehaviour
{
    public float pushPower = 2.0f;
    private float targetMass;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        //En esta condicion evitamos empujar objetos que no tenga el rigibody con el iskinematic

        if (body == null  || body.isKinematic) 
        {
            return;
        }
        //Estando el personaje encima del objeto evitamos que este se mueve
        if (hit.moveDirection.y < -0.3)
        {
            return;
        }

        targetMass = body.mass;

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        body.velocity = pushDir * pushPower / targetMass;
    }
}

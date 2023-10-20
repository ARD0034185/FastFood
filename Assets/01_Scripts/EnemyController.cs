using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    [Header("Estadisticas")]
    public float gravity = 9.8f;
    public float enemySpeed;
    public int distanceTargetPlayer;
    public float fallVelocity;
    public float horizontalMove;
    public float verticalMove;
    public float slideVelocity;
    public float slopeForceDown;
    public bool isONSlope = false;

    [SerializeField] public float timer = 5;
    public float bulletSpeed;
    private float bulletTime;

    [Header("Referencias")]
    public CharacterController enemy;
    //Incorpora el horizontal y vertical MOve
    private Vector3 hitNormal;
    private Vector3 enemyInput;
    private Vector3 moveEnemy;
    public int routineEnemy;
    public float chronometer;
    public Quaternion angleEnemy;
    public float grade;
    private GameObject targetPlayer;

    public Transform player;
    public GameObject enemyBullet;
    public Transform spawnPoint;
 



    private void Start()
    {
        targetPlayer = GameObject.Find("Player");
    }

    

    void Update()
    {
        BehaviorEnemy();
        //ShootAtPlayer();
    }

    private void ShootAtPlayer()
    {
        bulletTime -= Time.deltaTime;
        if (bulletTime > 0) return;

        bulletTime = timer;

        Vector3 playerDirection = player.position - transform.position;
        GameObject bulletObject;
        bulletObject = Instantiate(enemyBullet, spawnPoint.position, spawnPoint.rotation);
        bulletObject.GetComponent<Rigidbody>().AddForce(playerDirection * bulletSpeed, ForceMode.Force);

        Rigidbody bulletRig = bulletObject.GetComponent<Rigidbody>();
        //bulletRig.AddForce(bulletRig.transform.forward * bulletSpeed, ForceMode.Force);
        
        //Destroy(bulletObject, 5f);
    }

    public void BehaviorEnemy()
    {
        if (Vector3.Distance(transform.position, targetPlayer.transform.position) > distanceTargetPlayer)
        {
            chronometer += 1 * Time.deltaTime;
            if (chronometer >= 4)
            {
                routineEnemy = Random.Range(0, 2);
                chronometer = 0;
            }

            switch (routineEnemy)
            {
                case 0:
                    horizontalMove = 0;
                    verticalMove = 0;

                    Movement(horizontalMove, verticalMove);

                    break;
                case 1:
                    //Direccion a desplazarse
                    grade = Random.Range(0, 360);
                    angleEnemy = Quaternion.Euler(0, grade, 0);
                    routineEnemy++;

                    break;
                case 2:
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, angleEnemy, 0.5f);

                    transform.Translate(Vector3.forward * 1 * Time.deltaTime);
                    //activar su movimiento
                    break;
            }
        }
        else
        {
            var lookPosPlayer = targetPlayer.transform.position - transform.position;
            lookPosPlayer.y = 0;
            var rotationEnemy = Quaternion.LookRotation(lookPosPlayer);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationEnemy, 2); ;
            horizontalMove = 0;
            verticalMove = 0;
            Movement(horizontalMove, verticalMove);
            ShootAtPlayer();
        }

    }

    private void Movement(float horizontalMove, float verticalMove)
    {
        SetGravity();
        enemyInput = new Vector3(horizontalMove, 0, verticalMove);
        enemyInput = Vector3.ClampMagnitude(enemyInput, 1);

        moveEnemy = moveEnemy * enemySpeed;

        enemy.Move(moveEnemy * enemySpeed * Time.deltaTime);
    }


    private void SetGravity()
    {
        //condicion para acelerar la caida
        if (enemy.isGrounded)
        {
            fallVelocity = -gravity * Time.deltaTime;
            moveEnemy.y = fallVelocity;
        }
        else
        {
            fallVelocity -= gravity * Time.deltaTime;
            moveEnemy.y = fallVelocity;
        }

        SlipeDown();
    }

    public void SlipeDown()
    {
        isONSlope = Vector3.Angle(Vector3.up, hitNormal) >= enemy.slopeLimit;

        if (isONSlope)
        {
            moveEnemy.x += ((1f - hitNormal.y) * hitNormal.x) * slideVelocity;
            moveEnemy.z += ((1f - hitNormal.y) * hitNormal.z) * slideVelocity;

            moveEnemy.y += slopeForceDown;
        }
    }
}

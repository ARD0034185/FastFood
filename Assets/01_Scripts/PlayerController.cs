using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Estadisticas")]
    public float gravity = 9.8f;
    public float jumpForce;
    public float playerSpeed;
    public float playerRunSpeed;
    public float energyPlayerA;
    public float energyPlayerM;
    [Range(0, 1)] public float slowEnergy;
    public float fallVelocity;
    public float horizontalMove;
    public float verticalMove;
    public float slideVelocity;
    public float slopeForceDown;
    public bool isONSlope = false;
    private bool isMoving = false;
    public float score = 0;
    public float pointsScore = 3;
    [Header("Referencias")]
    public CharacterController player;
    public Camera mainCamera;
    //Incorpora el horizontal y vertical MOve
    private Vector3 hitNormal;
    private Vector3 playerInput;
    private Vector3 camForward;
    private Vector3 camRight;
    private Vector3 movePlayer;
    [Header("UI")]
    public TextMeshPro scorePlayer;
    public Image barrLife;
    [Header("Sonidos")]
    public AudioClip walkSound;
    public AudioClip runSound;

   
    void Start()
    {
        player = GetComponent<CharacterController>();
        scorePlayer.text = score.ToString();
    }

    void Update()
    {
        score += pointsScore * Time.deltaTime;
        Movement();
        barrLife.fillAmount = energyPlayerA / energyPlayerM;
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitNormal = hit.normal;
    }

  

    public void SlipeDown()
    {
        isONSlope = Vector3.Angle(Vector3.up, hitNormal) >= player.slopeLimit;

        if (isONSlope)
        {
            movePlayer.x += ((1f - hitNormal.y) * hitNormal.x) * slideVelocity;
            movePlayer.z += ((1f - hitNormal.y) * hitNormal.z) * slideVelocity;

            movePlayer.y += slopeForceDown;
        }
    }

    public void ApplyEnergy(float energyFood)
    {
        energyPlayerA += energyFood;
    }

    
    private void Movement()
    {
        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");

        playerInput = new Vector3(horizontalMove, 0, verticalMove);
        playerInput = Vector3.ClampMagnitude(playerInput, 1f);

        camDirection();

        if (playerInput != Vector3.zero)
        {
            isMoving = true;

            movePlayer = playerInput.x * camRight + playerInput.z * camForward;
            player.transform.LookAt(player.transform.position + movePlayer);
            SetGravity();

            PlayerSkills();

            if (Input.GetButton("Sprint"))
            {
                AudioManager.instance.SongStop(walkSound);
                AudioManager.instance.SetMusicNotStop(runSound);
                movePlayer = movePlayer * playerRunSpeed;
                player.Move(movePlayer * playerRunSpeed * Time.deltaTime);
                energyPlayerA -= 0.01f;
                
            }
            else
            {
                AudioManager.instance.SongStop(runSound);
                AudioManager.instance.SetMusicNotStop(walkSound);
                movePlayer = movePlayer * playerSpeed;
                player.Move(movePlayer * playerSpeed * Time.deltaTime);
                
            }
        }
        else
        {
            isMoving = false;
        }


       


        if (isMoving)
        {
            energyPlayerA -= slowEnergy * Time.deltaTime;
            energyPlayerA = Math.Max(0f, energyPlayerA);
            barrLife.fillAmount = energyPlayerA;
            Debug.Log("Velocity Player: " + energyPlayerA);
        }

        if (energyPlayerA <= 0f)
        {
            //playerInput = new Vector3(0, 0, 0);
            Destroy(gameObject, 3);
            SceneManager.LoadScene(0);
        }
    }

    private void ReceiveDamage()
    {
        energyPlayerA -= slowEnergy;
        energyPlayerA = Math.Max(0f, energyPlayerA);
        barrLife.fillAmount = energyPlayerA;
        Debug.Log("Velocity Player: " + energyPlayerA);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "BulletEnemy")
        {
            ReceiveDamage();
        }
    }

    private void PlayerSkills()
    {
        if (player.isGrounded && Input.GetButtonDown("Jump"))
        {
            fallVelocity = jumpForce;
            movePlayer.y = fallVelocity;
        }
    }

    private void SetGravity()
    {
        //condicion para acelerar la caida
        if (player.isGrounded)
        {
            fallVelocity = -gravity * Time.deltaTime;
            movePlayer.y = fallVelocity;
        }
        else
        {
            fallVelocity -= gravity * Time.deltaTime;
            movePlayer.y = fallVelocity;
        }

        SlipeDown();
    }

    private void camDirection()
    {
        camForward = mainCamera.transform.forward;
        camRight = mainCamera.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward = camForward.normalized;
        camRight = camRight.normalized;
    }
}

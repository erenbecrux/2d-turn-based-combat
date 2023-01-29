using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    [SerializeField] float playerSpeed = 5f;
    Vector2 inputVector;
    Rigidbody2D rigidBody2D;
    Animator playerAnimator;
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }


    void Update()
    {
        PlayerMovement();
    }

    void OnMove(InputValue value)
    {
        inputVector = value.Get<Vector2>();
        
        playerAnimator.SetFloat("HorizontalMove", inputVector.x);
        playerAnimator.SetFloat("VerticalMove", inputVector.y);
    }

    void PlayerMovement()
    {
        rigidBody2D.velocity = inputVector * playerSpeed;
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "EnemyArea")
        {
            StartCoroutine(LoadBattleScene());
        }    
    }

    IEnumerator LoadBattleScene()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("BattleScene");
    }

}

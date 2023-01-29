using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    public BattleState state;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Transform playerPosition;
    [SerializeField] Transform enemyPosition;
    Unit playerUnit;
    Unit enemyUnit;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] BattleHUD playerHUD;
    [SerializeField] BattleHUD enemyHUD;
    [SerializeField] Button attackButton;
    [SerializeField] Button healButton;
    [SerializeField] ParticleSystem damageEffect;
    [SerializeField] ParticleSystem healEffect;
    
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(StartBattle());
    }

    IEnumerator StartBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerPosition.transform.position, Quaternion.identity);
        playerUnit = playerGO.GetComponent<Unit>();
        

        GameObject enemyGO = Instantiate(enemyPrefab, enemyPosition.transform.position, Quaternion.identity);
        enemyUnit = enemyGO.GetComponent<Unit>();
        

        dialogueText.text = "A wild " + enemyUnit.unitName + " is approaching!";
        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);
        
        
        yield return new WaitForSeconds(2);
        state = BattleState.PLAYERTURN;
        PlayerTurn();
        
    }

    void PlayerTurn()
    {
        dialogueText.text = "Select an action: ";
        attackButton.interactable = true;
        healButton.interactable = true;        
    }

    void EnemyTurn()
    {
        dialogueText.text = enemyUnit.unitName + " is choosing an action...";
        
        StartCoroutine(EnemyAttack());
    }

    public void OnAttackButton()
    {
        if(state != BattleState.PLAYERTURN)
        {
            return;
        }
        
        StartCoroutine(PlayerAttack());

        attackButton.interactable = false;
        healButton.interactable = false;
    }

    public void OnHealButton()
    {
        if(state != BattleState.PLAYERTURN)
        {
            return;
        }
        
        StartCoroutine(PlayerHeal());

        attackButton.interactable = false;
        healButton.interactable = false;
    }

    IEnumerator EnemyAttack()
    {

        yield return new WaitForSeconds(2);
        dialogueText.text = enemyUnit.unitName + " hit " + enemyUnit.unitDamage + " damage!";
        PlayDamageEffect(playerUnit.transform);
        playerUnit.unitCurrentHP -= enemyUnit.unitDamage;
        playerHUD.SetHP(playerUnit.unitCurrentHP);
        yield return new WaitForSeconds(2);

        if(playerUnit.unitCurrentHP < 1)
        {
            dialogueText.text = "You lost the battle!";
            state = BattleState.LOST;
            StartCoroutine(ExitBattleScene());
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    IEnumerator ExitBattleScene()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("CityScene");
    }

    void PlayDamageEffect(Transform unitTransform)
    {
        if(damageEffect != null)
        {
            ParticleSystem instance = Instantiate(damageEffect, unitTransform.position, Quaternion.identity);
            Destroy(instance.gameObject, instance.main.duration + instance.main.startLifetime.constantMax);
        }
    }

    void PlayHealEffect(Transform unitTransform)
    {
        if(healEffect != null)
        {
            ParticleSystem instance = Instantiate(healEffect, unitTransform.position, Quaternion.identity);
            Destroy(instance.gameObject, instance.main.duration + instance.main.startLifetime.constantMax);
        }
    }

    IEnumerator PlayerAttack()
    {
        dialogueText.text = "You hit " + playerUnit.unitDamage + " damage to "+ enemyUnit.unitName;
        PlayDamageEffect(enemyUnit.transform);
        enemyUnit.unitCurrentHP -= playerUnit.unitDamage;
        enemyHUD.SetHP(enemyUnit.unitCurrentHP);

        yield return new WaitForSeconds(2);

        if(enemyUnit.unitCurrentHP < 1)
        {
            dialogueText.text = "You won the battle!";
            state = BattleState.WON;
            StartCoroutine(ExitBattleScene());
        }
        else
        {
            state = BattleState.ENEMYTURN;
            EnemyTurn();
        }
    }

    IEnumerator PlayerHeal()
    {
        dialogueText.text = "You recovered " + playerUnit.unitHeal + " health!";
        PlayHealEffect(playerUnit.transform);
        playerUnit.unitCurrentHP += playerUnit.unitHeal;
        if(playerUnit.unitCurrentHP > playerUnit.unitMaxHP)
        {
            playerUnit.unitCurrentHP = playerUnit.unitMaxHP;
        }
        playerHUD.SetHP(playerUnit.unitCurrentHP);

        yield return new WaitForSeconds(2);

        state = BattleState.ENEMYTURN;
        EnemyTurn();

    }
    
}

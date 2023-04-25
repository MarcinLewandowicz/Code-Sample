using ML.Character;
using ML.Combat;
using ML.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FieldOfView))]
public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private Health playerHealthComponent;
    [SerializeField] private GameObject exclamationMark;
    protected Health enemyHealth;
    protected Mover enemyMover;
    public FieldOfView enemyFOV;
    public Health PlayerHealthComponent { get { return playerHealthComponent; } }

    public Health EnemyHealth { get { return enemyHealth; } }
    private void Awake()
    {
        playerHealthComponent = GameObject.FindWithTag("Player").GetComponent<Health>();
        enemyHealth = GetComponent<Health>();
        enemyMover = GetComponent<Mover>();
        enemyFOV = GetComponent<FieldOfView>();
    }

    protected virtual void OnEnable()
    {
        GetComponent<Health>().OnDeath += DeathBehaviour;
        RespawnBehaviour();
    }
    protected virtual void OnDisable()
    {
        GetComponent<Health>().OnDeath -= DeathBehaviour;
    }

    protected virtual void RespawnBehaviour()
    {
        GetComponent<Health>().Resurrect();
        transform.LookAt(Vector3.zero);
    }
    private void DeathBehaviour()
    {
        LevelType currentLevel = GameManager.Instance.CurrentLevelType;
        switch (currentLevel)
        {
            case LevelType.sandbox:
                InitializeRespawn();
                StartCoroutine(DisableEnemy());
                SetExclamationMarkStatus(false);
                break;
            case LevelType.game:
                SetExclamationMarkStatus(false);
                break;
            default:
                break;
        }
    }

    private void InitializeRespawn()
    {
        if (transform.parent != null)
        {
            AddToEnemyPool(transform.parent.gameObject);
            float respawnTime = Random.Range(3f, 5f);
            EnemySpawner.Instance.SpawnEnemiesWithDelay(respawnTime);
        }
    }

    IEnumerator DisableEnemy()
    {
        yield return new WaitForSeconds(2f);
        if (transform.parent != null)
        {
            transform.position = transform.parent.position;
            transform.parent.gameObject.SetActive(false);
        }
    }

    private void AddToEnemyPool(GameObject enemy)
    {
        EnemySpawner.Instance.AddEnemy(enemy);
    }

    public void Attack()
    {
        GetComponent<Fighter>().StartAttack(playerHealthComponent);
    }

    public void Move(Vector3 targetPosition) => enemyMover.StartMovement(targetPosition);

    public void SetMovementSpeed(float speed) => enemyMover.SetMovementSpeed(speed);
    public void StopMovement() => enemyMover.StopMovement();

    public void SetExclamationMarkStatus(bool state)
    {
        exclamationMark.SetActive(state);
    }
}

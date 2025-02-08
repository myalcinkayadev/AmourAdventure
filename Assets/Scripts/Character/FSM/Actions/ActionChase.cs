using UnityEngine;

public class ActionChase : FSMAction
{
    [Header("Config")]
    [SerializeField] private float chaseSpeed;

    private const float ChaseRange = 1.69f;

    private CharacterBrain characterBrain;

    public override void Act()
    {
        ChasePlayer();
    }

    private void Awake() {
        characterBrain = GetComponent<CharacterBrain>();

        if (characterBrain == null)
        {
            Debug.LogError($"ActionChase: No CharacterBrain found on {gameObject.name}");
            enabled = false;
        }
    }
    
    private void ChasePlayer() {
        if (characterBrain.Player == null) return;
        
        Vector3 directionToPlayer = characterBrain.Player.position - transform.position;
        if (directionToPlayer.magnitude >= ChaseRange) {
            transform.Translate(directionToPlayer.normalized * (chaseSpeed * Time.deltaTime));
        }
    }
}

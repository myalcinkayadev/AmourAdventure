using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyBrain : MonoBehaviour {

    [SerializeField] private string initState;
    [SerializeField] private FSMState[] states;

    public FSMState CurrentState { get; set; }

    public Transform Player { get; private set; }

    public void ChangeState(string newStateID) {
        FSMState newState = GetState(newStateID);
        if (newState == null) {
            Debug.LogError($"EnemyBrain: State '{newStateID}' not found for {gameObject.name}");
            return;
        }

        CurrentState = newState;
    }

    private void Start() {
        ChangeState(initState);
    }

    private void Update() {
        CurrentState?.UpdateState(this);
    }

    private FSMState GetState(string stateId) {
        return states?.FirstOrDefault(state => state.ID == stateId);
    }

    public void SetPlayer(Transform player)
    {
        Player = player;
    }
}

using System;

[Serializable]
public class FSMState {
    public string ID;
    public FSMAction[] Actions;
    public FSMTransition[] Transitions;

    public void UpdateState(EnemyBrain enemyBrain) {
        ExecuteActions();
        ExecuteTransitions(enemyBrain);
    }

    private void ExecuteActions() {
        if (Actions == null) return;

        foreach (var action in Actions) {
            action.Act();
        }
    }

    private void ExecuteTransitions(EnemyBrain enemyBrain) {
        if (Transitions == null) return;

        foreach (var transition in Transitions) {
            bool decisionResult = transition.Decision.Decide();
            string nextStateID = decisionResult ? transition.TrueState : transition.FalseState;
            enemyBrain.ChangeState(nextStateID);
        }
    }
}

using System;

[Serializable]
public class FSMState {
    public string ID;
    public FSMAction[] Actions;
    public FSMTransition[] Transitions;

    public void UpdateState(CharacterBrain characterBrain) {
        ExecuteActions();
        ExecuteTransitions(characterBrain);
    }

    private void ExecuteActions() {
        if (Actions == null || Actions.Length == 0) return;

        foreach (var action in Actions) {
            action.Act();
        }
    }

    private void ExecuteTransitions(CharacterBrain characterBrain) {
        if (Transitions == null) return;

        foreach (var transition in Transitions) {
            bool decisionResult = transition.Decision.Decide();
            string nextStateID = decisionResult ? transition.TrueState : transition.FalseState;
            characterBrain.ChangeState(nextStateID);
        }
    }
}
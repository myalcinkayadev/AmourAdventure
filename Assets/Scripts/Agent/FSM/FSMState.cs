
using System;

[Serializable]
public class FSMState {
    public string ID;
    public FSMAction[] Actions;
    public FSMTransition[] Transitions;

    public void UpdateState(AgentBrain agentBrain) {
        ExecuteActions();
        ExecuteTransitions(agentBrain);
    }

    private void ExecuteActions() {
        if (Actions == null || Actions.Length == 0) return;

        foreach (var action in Actions) {
            action.Act();
        }
    }

    private void ExecuteTransitions(AgentBrain agentBrain) {
        if (Transitions == null) return;

        foreach (var transition in Transitions) {
            bool decisionResult = transition.Decision.Decide();
            string nextStateID = decisionResult ? transition.TrueState : transition.FalseState;
            agentBrain.ChangeState(nextStateID);
        }
    }
}
using System;
using System.Collections.Generic;

namespace Platformer {
	public class StateMachine {
		private StateNode _current;
		private Dictionary<Type, StateNode> _nodes = new();
		private HashSet<ITransition> _transitions = new();

		public void Update() {
			var transition = GetTransition();
			if (transition is not null) {
				ChangeState(transition.To);
			}

			_current.State.Update();
		}

		public void FixedUpdate() {
			_current.State.FixedUpdate();
		}

		public void SetState(IState state) {
			_current = _nodes[state.GetType()];
			_current.State.OnEnter();
		}

		private void ChangeState(IState newState) {
			if (newState == _current.State) return;

			IState previousState = _current.State;
			IState nextState = _nodes[newState.GetType()].State;

			previousState.OnExit();
			nextState.OnEnter();
			_current = _nodes[newState.GetType()];
		}
		private ITransition GetTransition() {
			foreach (ITransition transition in _transitions) {
				if (transition.Condition.Evaluate())
					return transition;
			}
			foreach (ITransition transition in _current.Transitions) {
				if (transition.Condition.Evaluate())
					return transition;
			}
			return null;
		}
		public void AddTransition(IState from, IState to, IPredicate condition) {
			GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);
		}

		public void AddAnyTransition(IState to, IPredicate condition) {
			_transitions.Add(new Transition(to, condition));
		}

		private StateNode GetOrAddNode(IState state) {
			if (!_nodes.TryGetValue(state.GetType(), out StateNode node)) {
				node = new StateNode(state);
				_nodes.Add(state.GetType(), node);
			}
			return node;
		}

		private class StateNode {
			public IState State { get; }
			public HashSet<ITransition> Transitions { get; } = new HashSet<ITransition>();

			public StateNode(IState state) {
				State = state;
			}

			public void AddTransition(IState to, IPredicate condition) {
				Transitions.Add(new Transition(to, condition));
			}
		}
	}
}

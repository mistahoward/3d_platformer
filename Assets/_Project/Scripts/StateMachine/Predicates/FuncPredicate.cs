using System;

namespace Platformer {
	public class FuncPredicate : IPredicate {
		public FuncPredicate(Func<bool> func) {
			_func = func;
		}
		private readonly Func<bool> _func;
		public bool Evaluate() => _func.Invoke();
	}
}

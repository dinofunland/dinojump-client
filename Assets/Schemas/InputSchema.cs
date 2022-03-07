// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 2.0.4
// 

using Colyseus.Schema;
using Action = System.Action;

namespace Dinojump.Schemas {
	public partial class InputSchema : Schema {
		[Type(0, "boolean")]
		public bool left = default(bool);

		[Type(1, "boolean")]
		public bool right = default(bool);

		/*
		 * Support for individual property change callbacks below...
		 */

		protected event PropertyChangeHandler<bool> _leftChange;
		public Action OnLeftChange(PropertyChangeHandler<bool> handler) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(left));
			_leftChange += handler;
			return () => {
				__callbacks.RemovePropertyCallback(nameof(left));
				_leftChange -= handler;
			};
		}

		protected event PropertyChangeHandler<bool> _rightChange;
		public Action OnRightChange(PropertyChangeHandler<bool> handler) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(right));
			_rightChange += handler;
			return () => {
				__callbacks.RemovePropertyCallback(nameof(right));
				_rightChange -= handler;
			};
		}

		protected override void TriggerFieldChange(DataChange change) {
			switch (change.Field) {
				case nameof(left): _leftChange?.Invoke((bool) change.Value, (bool) change.PreviousValue); break;
				case nameof(right): _rightChange?.Invoke((bool) change.Value, (bool) change.PreviousValue); break;
				default: break;
			}
		}
	}
}

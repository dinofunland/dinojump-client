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
		[Type(0, "number")]
		public float horizontal = default(float);

		/*
		 * Support for individual property change callbacks below...
		 */

		protected event PropertyChangeHandler<float> _horizontalChange;
		public Action OnHorizontalChange(PropertyChangeHandler<float> handler) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(horizontal));
			_horizontalChange += handler;
			return () => {
				__callbacks.RemovePropertyCallback(nameof(horizontal));
				_horizontalChange -= handler;
			};
		}

		protected override void TriggerFieldChange(DataChange change) {
			switch (change.Field) {
				case nameof(horizontal): _horizontalChange?.Invoke((float) change.Value, (float) change.PreviousValue); break;
				default: break;
			}
		}
	}
}

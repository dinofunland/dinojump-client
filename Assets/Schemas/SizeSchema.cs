// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 2.0.4
// 

using Colyseus.Schema;
using Action = System.Action;

namespace Dinojump.Schemas {
	public partial class SizeSchema : Schema {
		[Type(0, "number")]
		public float width = default(float);

		[Type(1, "number")]
		public float height = default(float);

		/*
		 * Support for individual property change callbacks below...
		 */

		protected event PropertyChangeHandler<float> _widthChange;
		public Action OnWidthChange(PropertyChangeHandler<float> handler) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(width));
			_widthChange += handler;
			return () => {
				__callbacks.RemovePropertyCallback(nameof(width));
				_widthChange -= handler;
			};
		}

		protected event PropertyChangeHandler<float> _heightChange;
		public Action OnHeightChange(PropertyChangeHandler<float> handler) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(height));
			_heightChange += handler;
			return () => {
				__callbacks.RemovePropertyCallback(nameof(height));
				_heightChange -= handler;
			};
		}

		protected override void TriggerFieldChange(DataChange change) {
			switch (change.Field) {
				case nameof(width): _widthChange?.Invoke((float) change.Value, (float) change.PreviousValue); break;
				case nameof(height): _heightChange?.Invoke((float) change.Value, (float) change.PreviousValue); break;
				default: break;
			}
		}
	}
}

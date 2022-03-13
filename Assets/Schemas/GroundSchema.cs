// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 2.0.4
// 

using Colyseus.Schema;
using Action = System.Action;

namespace Dinojump.Schemas {
	public partial class GroundSchema : Schema {
		[Type(0, "ref", typeof(PositionSchema))]
		public PositionSchema position = new PositionSchema();

		[Type(1, "ref", typeof(SizeSchema))]
		public SizeSchema size = new SizeSchema();

		/*
		 * Support for individual property change callbacks below...
		 */

		protected event PropertyChangeHandler<PositionSchema> _positionChange;
		public Action OnPositionChange(PropertyChangeHandler<PositionSchema> handler) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(position));
			_positionChange += handler;
			return () => {
				__callbacks.RemovePropertyCallback(nameof(position));
				_positionChange -= handler;
			};
		}

		protected event PropertyChangeHandler<SizeSchema> _sizeChange;
		public Action OnSizeChange(PropertyChangeHandler<SizeSchema> handler) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(size));
			_sizeChange += handler;
			return () => {
				__callbacks.RemovePropertyCallback(nameof(size));
				_sizeChange -= handler;
			};
		}

		protected override void TriggerFieldChange(DataChange change) {
			switch (change.Field) {
				case nameof(position): _positionChange?.Invoke((PositionSchema) change.Value, (PositionSchema) change.PreviousValue); break;
				case nameof(size): _sizeChange?.Invoke((SizeSchema) change.Value, (SizeSchema) change.PreviousValue); break;
				default: break;
			}
		}
	}
}

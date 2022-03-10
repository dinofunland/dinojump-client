// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 2.0.4
// 

using Colyseus.Schema;
using Action = System.Action;

namespace Dinojump.Schemas {
	public partial class PlayerSchema : Schema {
		[Type(0, "string")]
		public string sessionId = default(string);

		[Type(1, "string")]
		public string username = default(string);

		[Type(2, "boolean")]
		public bool isReady = default(bool);

		[Type(3, "ref", typeof(PositionSchema))]
		public PositionSchema position = new PositionSchema();

		[Type(4, "ref", typeof(SizeSchema))]
		public SizeSchema size = new SizeSchema();

		[Type(5, "ref", typeof(InputSchema))]
		public InputSchema input = new InputSchema();

		[Type(6, "number")]
		public float skin = default(float);

		[Type(7, "number")]
		public float animation = default(float);

		/*
		 * Support for individual property change callbacks below...
		 */

		protected event PropertyChangeHandler<string> _sessionIdChange;
		public Action OnSessionIdChange(PropertyChangeHandler<string> handler) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(sessionId));
			_sessionIdChange += handler;
			return () => {
				__callbacks.RemovePropertyCallback(nameof(sessionId));
				_sessionIdChange -= handler;
			};
		}

		protected event PropertyChangeHandler<string> _usernameChange;
		public Action OnUsernameChange(PropertyChangeHandler<string> handler) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(username));
			_usernameChange += handler;
			return () => {
				__callbacks.RemovePropertyCallback(nameof(username));
				_usernameChange -= handler;
			};
		}

		protected event PropertyChangeHandler<bool> _isReadyChange;
		public Action OnIsReadyChange(PropertyChangeHandler<bool> handler) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(isReady));
			_isReadyChange += handler;
			return () => {
				__callbacks.RemovePropertyCallback(nameof(isReady));
				_isReadyChange -= handler;
			};
		}

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

		protected event PropertyChangeHandler<InputSchema> _inputChange;
		public Action OnInputChange(PropertyChangeHandler<InputSchema> handler) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(input));
			_inputChange += handler;
			return () => {
				__callbacks.RemovePropertyCallback(nameof(input));
				_inputChange -= handler;
			};
		}

		protected event PropertyChangeHandler<float> _skinChange;
		public Action OnSkinChange(PropertyChangeHandler<float> handler) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(skin));
			_skinChange += handler;
			return () => {
				__callbacks.RemovePropertyCallback(nameof(skin));
				_skinChange -= handler;
			};
		}

		protected event PropertyChangeHandler<float> _animationChange;
		public Action OnAnimationChange(PropertyChangeHandler<float> handler) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(animation));
			_animationChange += handler;
			return () => {
				__callbacks.RemovePropertyCallback(nameof(animation));
				_animationChange -= handler;
			};
		}

		protected override void TriggerFieldChange(DataChange change) {
			switch (change.Field) {
				case nameof(sessionId): _sessionIdChange?.Invoke((string) change.Value, (string) change.PreviousValue); break;
				case nameof(username): _usernameChange?.Invoke((string) change.Value, (string) change.PreviousValue); break;
				case nameof(isReady): _isReadyChange?.Invoke((bool) change.Value, (bool) change.PreviousValue); break;
				case nameof(position): _positionChange?.Invoke((PositionSchema) change.Value, (PositionSchema) change.PreviousValue); break;
				case nameof(size): _sizeChange?.Invoke((SizeSchema) change.Value, (SizeSchema) change.PreviousValue); break;
				case nameof(input): _inputChange?.Invoke((InputSchema) change.Value, (InputSchema) change.PreviousValue); break;
				case nameof(skin): _skinChange?.Invoke((float) change.Value, (float) change.PreviousValue); break;
				case nameof(animation): _animationChange?.Invoke((float) change.Value, (float) change.PreviousValue); break;
				default: break;
			}
		}
	}
}

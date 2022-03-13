// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 2.0.4
// 

using Colyseus.Schema;
using Action = System.Action;

namespace Dinojump.Schemas {
	public partial class GameSchema : Schema {
		[Type(0, "string")]
		public string gameStep = default(string);

		[Type(1, "number")]
		public float gameSpeed = default(float);

		[Type(2, "map", typeof(MapSchema<PlayerSchema>))]
		public MapSchema<PlayerSchema> players = new MapSchema<PlayerSchema>();

		[Type(3, "map", typeof(MapSchema<PlatformSchema>))]
		public MapSchema<PlatformSchema> platforms = new MapSchema<PlatformSchema>();

		[Type(4, "ref", typeof(FloorSchema))]
		public FloorSchema floor = new FloorSchema();

		[Type(5, "ref", typeof(GroundSchema))]
		public GroundSchema ground = new GroundSchema();

		/*
		 * Support for individual property change callbacks below...
		 */

		protected event PropertyChangeHandler<string> _gameStepChange;
		public Action OnGameStepChange(PropertyChangeHandler<string> handler) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(gameStep));
			_gameStepChange += handler;
			return () => {
				__callbacks.RemovePropertyCallback(nameof(gameStep));
				_gameStepChange -= handler;
			};
		}

		protected event PropertyChangeHandler<float> _gameSpeedChange;
		public Action OnGameSpeedChange(PropertyChangeHandler<float> handler) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(gameSpeed));
			_gameSpeedChange += handler;
			return () => {
				__callbacks.RemovePropertyCallback(nameof(gameSpeed));
				_gameSpeedChange -= handler;
			};
		}

		protected event PropertyChangeHandler<MapSchema<PlayerSchema>> _playersChange;
		public Action OnPlayersChange(PropertyChangeHandler<MapSchema<PlayerSchema>> handler) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(players));
			_playersChange += handler;
			return () => {
				__callbacks.RemovePropertyCallback(nameof(players));
				_playersChange -= handler;
			};
		}

		protected event PropertyChangeHandler<MapSchema<PlatformSchema>> _platformsChange;
		public Action OnPlatformsChange(PropertyChangeHandler<MapSchema<PlatformSchema>> handler) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(platforms));
			_platformsChange += handler;
			return () => {
				__callbacks.RemovePropertyCallback(nameof(platforms));
				_platformsChange -= handler;
			};
		}

		protected event PropertyChangeHandler<FloorSchema> _floorChange;
		public Action OnFloorChange(PropertyChangeHandler<FloorSchema> handler) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(floor));
			_floorChange += handler;
			return () => {
				__callbacks.RemovePropertyCallback(nameof(floor));
				_floorChange -= handler;
			};
		}

		protected event PropertyChangeHandler<GroundSchema> _groundChange;
		public Action OnGroundChange(PropertyChangeHandler<GroundSchema> handler) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(ground));
			_groundChange += handler;
			return () => {
				__callbacks.RemovePropertyCallback(nameof(ground));
				_groundChange -= handler;
			};
		}

		protected override void TriggerFieldChange(DataChange change) {
			switch (change.Field) {
				case nameof(gameStep): _gameStepChange?.Invoke((string) change.Value, (string) change.PreviousValue); break;
				case nameof(gameSpeed): _gameSpeedChange?.Invoke((float) change.Value, (float) change.PreviousValue); break;
				case nameof(players): _playersChange?.Invoke((MapSchema<PlayerSchema>) change.Value, (MapSchema<PlayerSchema>) change.PreviousValue); break;
				case nameof(platforms): _platformsChange?.Invoke((MapSchema<PlatformSchema>) change.Value, (MapSchema<PlatformSchema>) change.PreviousValue); break;
				case nameof(floor): _floorChange?.Invoke((FloorSchema) change.Value, (FloorSchema) change.PreviousValue); break;
				case nameof(ground): _groundChange?.Invoke((GroundSchema) change.Value, (GroundSchema) change.PreviousValue); break;
				default: break;
			}
		}
	}
}

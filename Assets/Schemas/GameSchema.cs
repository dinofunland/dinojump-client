// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 1.0.34
// 

using Colyseus.Schema;

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
	}
}

// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 1.0.34
// 

using Colyseus.Schema;

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
	}
}

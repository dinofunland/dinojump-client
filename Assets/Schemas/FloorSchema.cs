// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 1.0.34
// 

using Colyseus.Schema;

namespace Dinojump.Schemas {
	public partial class FloorSchema : Schema {
		[Type(0, "ref", typeof(PositionSchema))]
		public PositionSchema position = new PositionSchema();

		[Type(1, "ref", typeof(SizeSchema))]
		public SizeSchema size = new SizeSchema();
	}
}

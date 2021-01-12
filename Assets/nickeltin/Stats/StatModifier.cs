namespace Characters
{
	public class StatModifier
	{
		public enum Type
		{
			Flat = 100,
			PercentAdd = 200,
			PercentMult = 300,
		}
		
		public readonly float value;
		public readonly Type type;
		public readonly int order;
		public readonly object source;

		public StatModifier(float value, Type type, int order, object source)
		{
			this.value = value;
			this.type = type;
			this.order = order;
			this.source = source;
		}

		public StatModifier(float value, Type type) : this(value, type, (int)type, null) { }

		public StatModifier(float value, Type type, int order) : this(value, type, order, null) { }

		public StatModifier(float value, Type type, object source) : this(value, type, (int)type, source) { }
	}
}

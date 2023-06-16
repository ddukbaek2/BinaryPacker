using System.Collections.Generic;


namespace BinaryPacker
{
	/// <summary>
	/// 값 타입.
	/// </summary>
	public enum BPValueType : byte
	{
		None = 0,
		Boolean, // bool
		Number, // short, int, long, ushort, uint, ulong, float, double
		String, // string, etc
		Enum, // enum.
		Array, // List<T>, T[]
		Class, // class (not supported struct)
	}


	/// <summary>
	/// 오브젝트.
	/// </summary>
	public class BPObject
	{
		public string name = string.Empty;
		public BPValueType type = BPValueType.None;
		public string value = string.Empty;

		public string assemblyFullName = string.Empty;
		public string objectTypeFullName = string.Empty;
		public string elementTypeFullName = string.Empty;

		public List<BPObject> array = new List<BPObject>();
		public Dictionary<string, BPObject> members = new Dictionary<string, BPObject>();

		public bool IsLeaf => array.Count == 0 && members.Count == 0;

		public BPObject this[int _index] => array[_index];
		public BPObject this[string _name] => members[_name];

		public override string ToString()
		{
			return type.ToString();
		}
	}
}
using System.Collections.Generic;


namespace BinaryBuilder
{
	/// <summary>
	/// 값 타입.
	/// </summary>
	public enum BBValueType
	{
		None,
		Boolean,
		Number,
		String,
		Array,
		Object,
	}


	/// <summary>
	/// 오브젝트.
	/// </summary>
	public class BBObject
	{
		public string name = string.Empty;
		public BBValueType type = BBValueType.None;
		public string value = string.Empty;
		public List<BBObject> array = new List<BBObject>();
		public Dictionary<string, BBObject> members = new Dictionary<string, BBObject>();

		public string elementTypeFullName = string.Empty;
		public string objectTypeFullName = string.Empty;
		public string assemblyFullName = string.Empty;

		public bool IsLeaf => array.Count == 0 && members.Count == 0;
	}
}
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
		public string objectTypeName = string.Empty;
		public BBValueType valueType = BBValueType.None;
		public string value = string.Empty;
		public Dictionary<string, BBObject> children = new Dictionary<string, BBObject>();
	}
}
using System;
using System.Collections;
using System.Collections.Generic;


namespace BinaryPacker
{
	/// <summary>
	/// 유틸리티.
	/// </summary>
	public static class BPUtility
	{
		/// <summary>
		/// 값의 타입 반환.
		/// </summary>
		public static BPValueType GetValueType(object _object)
		{
			var objectType = _object.GetType();

			if (_object is char || _object is string)
			{
				return BPValueType.String;
			}
			else if (objectType.IsArray || _object is IEnumerable)
			{
				return BPValueType.Array;
			}
			else if (objectType.IsClass)
			{
				return BPValueType.Object;
			}
			else
			{
				switch (objectType.FullName)
				{
					case "System.Boolean":
						return BPValueType.Boolean;
					case "System.Int16":
					case "System.UInt16":
					case "System.Int32":
					case "System.UInt32":
					case "System.Int64":
					case "System.UInt64":
					case "System.Single":
					case "System.Double":
						return BPValueType.Number;
					default:
						return BPValueType.String;
				}
			}
		}
	}
}
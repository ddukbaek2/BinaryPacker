using System;
using System.Collections;
using System.Collections.Generic;


namespace BinaryBuilder
{
	/// <summary>
	/// 유틸리티.
	/// </summary>
	public static class BBUtility
	{
		/// <summary>
		/// 배열 타입.
		/// </summary>
		public static bool IsArrayType(object _object)
		{
			var objectType = _object.GetType();
			if (objectType.IsArray)
				return true;
			else if (_object is IEnumerable)
				return true;

			return false;
		}

		///// <summary>
		///// 문자열 타입.
		///// </summary>
		//public static bool IsStringType(object _object)
		//{
		//	var objectType = _object.GetType();
		//	if (objectType.IsArray)
		//		return true;
		//	else if (_object is IEnumerable)
		//		return true;

		//	return false;
		//}

		/// <summary>
		/// 값의 타입 반환.
		/// </summary>
		public static BBValueType GetValueType(object _object)
		{
			var objectType = _object.GetType();

			if (_object is char || _object is string)
			{
				return BBValueType.String;
			}
			else if (IsArrayType(_object))
			{
				return BBValueType.Array;
			}
			else if (objectType.IsClass)
			{
				return BBValueType.Object;
			}
			else
			{
				switch (objectType.FullName)
				{
					case "System.Boolean":
						return BBValueType.Boolean;
					case "System.Int16":
					case "System.UInt16":
					case "System.Int32":
					case "System.UInt32":
					case "System.Int64":
					case "System.UInt64":
					case "System.Single":
					case "System.Double":
						return BBValueType.Number;
					default:
						return BBValueType.String;
				}
			}
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace BinaryBuilder
{
	/// <summary>
	/// 유틸리티.
	/// </summary>
	public static class BBUtility
	{
		/// <summary>
		/// 값의 타입 반환.
		/// </summary>
		public static BBValueType GetValueType(Type _type)
		{
			if (_type.IsArray)
			{
				return BBValueType.Array;
			}
			else if (_type.IsClass)
			{
				return BBValueType.Object;
			}
			else
			{
				switch (_type.Name)
				{
					case "bool":
						return BBValueType.Boolean;
					case "short":
					case "int":
					case "long":
					case "ushort":
					case "uint":
					case "ulong":
					case "float":
					case "double":
						return BBValueType.Number;
					default:
						return BBValueType.String;
				}
			}
		}
	}
}
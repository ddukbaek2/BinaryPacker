using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Remoting.Messaging;

namespace BinaryBuilder
{
	public class BBClassAttribute : Attribute
	{
	}

	public class BBFieldAttribute : Attribute
	{
	}


	/// <summary>
	/// 변환기.
	/// </summary>
	public static class BBConvert
	{
		/// <summary>
		/// 역직렬화.
		/// </summary>
		public static object Deserialize(this BBObject _bbObject)
		{
			void Recursive(BBObject _bbChildObject)
			{
				
			}

			var instanceType = Type.GetType(_bbObject.objectTypeName);
			var instance = Activator.CreateInstance(instanceType);


			foreach (var bbChildObject in _bbObject.children.Values)
			{
				Recursive(bbChildObject);
			}

			return instance;
		}


		/// <summary>
		/// 직렬화.
		/// </summary>
		public static BBObject Serialize<T>(this T _instance) where T : class
		{
			BBObject Recursive(BBObject _bbParentObject, object _object, Type _objectType)
			{
				var bbObject = new BBObject();
				bbObject.name = nameof(_object);
				bbObject.objectTypeName = _objectType.Name;
				bbObject.valueType = BBUtility.GetValueType(_objectType);

				if (_bbParentObject != null)
					_bbParentObject.children.Add(bbObject.name, bbObject);

				switch (bbObject.valueType)
				{
					case BBValueType.Boolean:
						{
							bbObject.value = _instance.ToString().ToLower();
							break;
						}

					case BBValueType.Number:
						{
							bbObject.value = _instance.ToString();
							break;
						}

					case BBValueType.String:
						{
							bbObject.value = _instance.ToString();
							break;
						}

					case BBValueType.Array:
						{
							bbObject.value = String.Empty;
							bbObject.children.Clear();
							foreach (var childObject in (Array)_object)
							{
								Recursive(bbObject, childObject, childObject.GetType());
							}
							break;
						}

					case BBValueType.Object:
						{
							bbObject.value = String.Empty;
							var fieldInfos = _objectType.GetFields();
							foreach (var fieldInfo in fieldInfos)
							{
								var childObject = fieldInfo.GetValue(_object);
								Recursive(bbObject, childObject, childObject.GetType());
							}

							break;
						}

					default:
						{
							bbObject.value = String.Empty;
							break;
						}
				}

				return bbObject;
			}

			var objectType = typeof(T);
			var bbRootObject = Recursive(null, _instance, objectType);
			return bbRootObject;
		}
	}
}
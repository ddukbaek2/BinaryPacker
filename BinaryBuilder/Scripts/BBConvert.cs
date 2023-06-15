using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;


namespace BinaryBuilder
{
	/// <summary>
	/// 변환기.
	/// </summary>
	public static class BBConvert
	{
		/// <summary>
		/// 직렬화 재귀.
		/// </summary>
		private static BBObject SerializeRecursively(BBObject _bbParentObject, object _object, string _objectName)
		{
			var objectType = _object.GetType();
			BBTypes.AddType(objectType);

			var bbObject = new BBObject();
			bbObject.name = _objectName;
			bbObject.assemblyFullName = objectType.Assembly.FullName;
			bbObject.objectTypeFullName = objectType.FullName;
			bbObject.isGeneric = objectType.IsGenericType;
			bbObject.type = BBUtility.GetValueType(_object);
			bbObject.value = String.Empty;
			bbObject.array.Clear();
			bbObject.members.Clear();

			if (_bbParentObject != null)
			{
				switch (_bbParentObject.type)
				{
					case BBValueType.Array:
						_bbParentObject.array.Add(bbObject);
						break;
					case BBValueType.Object:
						_bbParentObject.members.Add(_objectName, bbObject);
						break;
				}
			}

			switch (bbObject.type)
			{
				case BBValueType.Boolean:
				case BBValueType.Number:
				case BBValueType.String:
					{
						bbObject.value = _object.ToString();
						break;
					}

				case BBValueType.Array:
					{
						var childObjects = ((IEnumerable)_object).GetEnumerator();
						var index = 0;
						while (childObjects.MoveNext())
						{
							var childObject = childObjects.Current;
							SerializeRecursively(bbObject, childObject, string.Empty);
							++index;
						}
						break;
					}

				case BBValueType.Object:
					{
						var fieldInfos = objectType.GetFields();
						foreach (var fieldInfo in fieldInfos)
						{
							var childObject = fieldInfo.GetValue(_object);
							SerializeRecursively(bbObject, childObject, fieldInfo.Name);
						}

						break;
					}
			}

			return bbObject;
		}

		/// <summary>
		/// 역직렬화 재귀.
		/// </summary>
		private static object DeserializeRecursively(BBObject _bbObject, object _parentObject)
		{
			var assemblyFullName = _bbObject.assemblyFullName;
			var assembly = BBTypes.GetOrAddAssembly(assemblyFullName);
			if (assembly == null)
			{
				Console.WriteLine($"[ERROR] assemblyName is null. ({assemblyFullName})");
				return null;
			}

			var types = assembly.GetTypes();
			foreach (var type in types)
			{
				BBTypes.AddType(type);
			}

			switch (_bbObject.type)
			{
				case BBValueType.Boolean:
				case BBValueType.Number:
				case BBValueType.String:
					{
						var parentObjectType = _parentObject.GetType();
						var fieldInfo = parentObjectType.GetField(_bbObject.name);

						switch (_bbObject.type)
						{
							case BBValueType.Boolean:
								{
									if (!bool.TryParse(_bbObject.value, out var boolValue))
										boolValue = false;

									fieldInfo.SetValue(_parentObject, boolValue);
									break;
								}
							case BBValueType.Number:
								{
									Console.Write(_bbObject.objectTypeFullName);
									//fieldInfo.SetValue(_parentObject, _bbObject.value);
									break;
								}
							case BBValueType.String:
								{
									fieldInfo.SetValue(_parentObject, _bbObject.value);
									break;
								}
						}

						return null;
					}

				case BBValueType.Array:
				case BBValueType.Object:
					{
						var objectType = BBTypes.GetType(_bbObject.objectTypeFullName);
						var @object = Activator.CreateInstance(objectType);

						foreach (var bbChildObject in _bbObject.members.Values)
						{
							DeserializeRecursively(bbChildObject, @object);
						}
				
						return @object;
					}

				default:
					{
						return null;
					}
			}			
		}

		/// <summary>
		/// 직렬화.
		/// </summary>
		public static BBObject Serialize(object _object)
		{
			if (_object == null)
				return new BBObject();

			var bbRootObject = SerializeRecursively(null, _object, string.Empty);
			return bbRootObject;
		}

		/// <summary>
		/// 역직렬화.
		/// </summary>
		public static object Deserialize(BBObject _bbObject)
		{
			if (_bbObject == null)
				return null;

			var @object = DeserializeRecursively(_bbObject, null);
			return @object;
		}

		///// <summary>
		///// 직렬화.
		///// </summary>
		//public static byte[] Serialize<T>(T _object) where T : class
		//{
		//	return null;
		//}

		///// <summary>
		///// 역직렬화.
		///// </summary>
		//public static T Deserialize<T>(byte[] _bytes) where T : class, new()
		//{
		//	return default;
		//}
	}
}
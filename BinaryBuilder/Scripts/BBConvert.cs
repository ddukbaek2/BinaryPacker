using System;
using System.Xml.Linq;


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
		private static BBObject SerializeRecursively(BBObject _bbParentObject, object _object)
		{
			var objectType = _object.GetType();
			var bbObject = new BBObject();
			bbObject.name = nameof(_object);
			bbObject.objectTypeName = objectType.Name;
			bbObject.valueType = BBUtility.GetValueType(objectType);
			bbObject.value = String.Empty;
			bbObject.children.Clear();

			if (_bbParentObject != null)
				_bbParentObject.children.Add(bbObject.name, bbObject);

			switch (bbObject.valueType)
			{
				case BBValueType.Boolean:
					{
						bbObject.value = _object.ToString().ToLower();
						break;
					}

				case BBValueType.Number:
					{
						bbObject.value = _object.ToString();
						break;
					}

				case BBValueType.String:
					{
						bbObject.value = _object.ToString();
						break;
					}

				case BBValueType.Array:
					{
						var childObjects = (Array)_object;
						foreach (var childObject in childObjects)
						{
							SerializeRecursively(bbObject, childObject);
						}
						break;
					}

				case BBValueType.Object:
					{
						var fieldInfos = objectType.GetFields();
						foreach (var fieldInfo in fieldInfos)
						{
							var childObject = fieldInfo.GetValue(_object);
							SerializeRecursively(bbObject, childObject);
						}

						break;
					}
			}

			return bbObject;
		}

		/// <summary>
		/// 직렬화.
		/// </summary>
		public static BBObject Serialize(object _object)
		{
			var bbRootObject = SerializeRecursively(null, _object);
			return bbRootObject;
		}

		/// <summary>
		/// 역직렬화.
		/// </summary>
		public static object Deserialize(BBObject _bbObject)
		{
			object DeserializeRecursively(BBObject _bbParentObject, object _object)
			{
				var objectType = Type.GetType(_bbObject.objectTypeName);
				switch (_bbObject.valueType)
				{
					case BBValueType.Boolean:
						{
							break;
						}

					case BBValueType.Number:
						{
							break;
						}

					case BBValueType.String:
						{
							var fieldInfo = objectType.GetField(_bbObject.name);
							fieldInfo.SetValue(_object, _bbObject.value);
							break;
						}

					case BBValueType.Array:
						{
							break;
						}

					case BBValueType.Object:
						{
							var childObject = Activator.CreateInstance(objectType);
							DeserializeRecursively(_bbObject, _object);
							return childObject;
						}
				}

				foreach (var bbObject in _bbParentObject.children.Values)
				{
				}
			}

			var rootObject = DeserializeRecursively(_bbObject, null);
			return rootObject;
		}

		/// <summary>
		/// 직렬화.
		/// </summary>
		public static byte[] Serialize<T>(T _object) where T : class
		{
			return null;
		}

		/// <summary>
		/// 역직렬화.
		/// </summary>
		public static T Deserialize<T>(byte[] _bytes) where T : class, new()
		{
			return default;
		}
	}
}
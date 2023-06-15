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
							var childObjectType = childObject.GetType();
							bbObject.elementTypeFullName = childObjectType.FullName;
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
							var childObjectType = childObject.GetType();
							bbObject.elementTypeFullName = childObjectType.FullName;
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
									switch (_bbObject.objectTypeFullName)
									{
										case "System.Int16":
											{
												if (!short.TryParse(_bbObject.value, out var shortValue))
													shortValue = 0;

												fieldInfo.SetValue(_parentObject, shortValue);
												break;
											}

										case "System.Int32":
											{
												if (!int.TryParse(_bbObject.value, out var intValue))
													intValue = 0;

												fieldInfo.SetValue(_parentObject, intValue);
												break;
											}

										case "System.Int64":
											{
												if (!long.TryParse(_bbObject.value, out var longValue))
													longValue = 0;

												fieldInfo.SetValue(_parentObject, longValue);
												break;
											}

										case "System.UInt16":
											{
												if (!ushort.TryParse(_bbObject.value, out var ushortValue))
													ushortValue = 0;

												fieldInfo.SetValue(_parentObject, ushortValue);
												break;
											}

										case "System.UInt32":
											{
												if (!uint.TryParse(_bbObject.value, out var uintValue))
													uintValue = 0;

												fieldInfo.SetValue(_parentObject, uintValue);
												break;
											}

										case "System.UInt64":
											{
												if (!ulong.TryParse(_bbObject.value, out var ulongValue))
													ulongValue = 0;

												fieldInfo.SetValue(_parentObject, ulongValue);
												break;
											}

										case "System.Single":
											{
												if (!float.TryParse(_bbObject.value, out var floatValue))
													floatValue = 0;

												fieldInfo.SetValue(_parentObject, floatValue);
												break;
											}

										case "System.Double":
											{
												if (!double.TryParse(_bbObject.value, out var doubleValue))
													doubleValue = 0;

												fieldInfo.SetValue(_parentObject, doubleValue);
												break;
											}
									}
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
					{
						var objectType = BBTypes.GetType(_bbObject.elementTypeFullName);
						var array = Array.CreateInstance(objectType, _bbObject.array.Count);

						var index = 0;
						foreach (var bbChildObject in _bbObject.array)
						{
							var childObject = DeserializeRecursively(bbChildObject, array);
							array.SetValue(childObject, index);
							++index;
						}

						return array;
					}

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
		public static BBObject Serialize<T>(this T _object) where T : class
		{
			if (_object == null)
				return null;

			var bbRootObject = SerializeRecursively(null, _object, string.Empty);
			return bbRootObject;
		}

		/// <summary>
		/// 역직렬화.
		/// </summary>
		public static T Deserialize<T>(this BBObject _bbObject) where T : class
		{
			if (_bbObject == null)
				return default(T);

			var rootObject = DeserializeRecursively(_bbObject, null) as T;
			return rootObject;
		}

		/// <summary>
		/// 오브젝트를 바이트배열로 변환.
		/// </summary>
		public static byte[] ToBytes(this BBObject _object)
		{
			if (_object == null)
				return byte[0];

			return null;
		}

		/// <summary>
		/// 바이트배열을 오브젝트로 변환.
		/// </summary>
		public static BBObject ToBBObject(this byte[] _bytes)
		{
			return default;
		}
	}
}
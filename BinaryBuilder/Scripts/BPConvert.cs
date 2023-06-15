using System;
using System.Collections;


namespace BinaryPacker
{
	/// <summary>
	/// 변환기.
	/// </summary>
	public static class BPConvert
	{
		/// <summary>
		/// 직렬화 재귀.
		/// </summary>
		private static BPObject SerializeRecursively(BPObject _bpParentObject, object _object, string _objectName)
		{
			var objectType = _object.GetType();
			BPTypes.AddType(objectType);

			var bpObject = new BPObject();
			bpObject.name = _objectName;
			bpObject.assemblyFullName = objectType.Assembly.FullName;
			bpObject.objectTypeFullName = objectType.FullName;
			bpObject.type = BPUtility.GetValueType(_object);
			bpObject.value = String.Empty;
			bpObject.array.Clear();
			bpObject.members.Clear();

			if (_bpParentObject != null)
			{
				switch (_bpParentObject.type)
				{
					case BPValueType.Array:
						_bpParentObject.array.Add(bpObject);
						break;
					case BPValueType.Object:
						_bpParentObject.members.Add(_objectName, bpObject);
						break;
				}
			}

			switch (bpObject.type)
			{
				case BPValueType.Boolean:
				case BPValueType.Number:
				case BPValueType.String:
					{
						bpObject.value = _object.ToString();
						break;
					}

				case BPValueType.Array:
					{
						var childObjects = ((IEnumerable)_object).GetEnumerator();
						var index = 0;
						while (childObjects.MoveNext())
						{
							var childObject = childObjects.Current;
							var childObjectType = childObject.GetType();
							bpObject.elementTypeFullName = childObjectType.FullName;
							SerializeRecursively(bpObject, childObject, string.Empty);
							++index;
						}
						break;
					}

				case BPValueType.Object:
					{
						var fieldInfos = objectType.GetFields();
						foreach (var fieldInfo in fieldInfos)
						{
							var childObject = fieldInfo.GetValue(_object);
							var childObjectType = childObject.GetType();
							bpObject.elementTypeFullName = childObjectType.FullName;
							SerializeRecursively(bpObject, childObject, fieldInfo.Name);
						}

						break;
					}
			}

			return bpObject;
		}

		/// <summary>
		/// 역직렬화 재귀.
		/// </summary>
		private static object DeserializeRecursively(BPObject _bpObject, object _parentObject)
		{
			var assemblyFullName = _bpObject.assemblyFullName;
			var assembly = BPTypes.GetOrAddAssembly(assemblyFullName);
			if (assembly == null)
			{
				Console.WriteLine($"[ERROR] assemblyName is null. ({assemblyFullName})");
				return null;
			}

			switch (_bpObject.type)
			{
				case BPValueType.Boolean:
				case BPValueType.Number:
				case BPValueType.String:
					{
						var parentObjectType = _parentObject.GetType();
						var fieldInfo = parentObjectType.GetField(_bpObject.name);

						switch (_bpObject.type)
						{
							case BPValueType.Boolean:
								{
									if (!bool.TryParse(_bpObject.value, out var boolValue))
										boolValue = false;

									fieldInfo.SetValue(_parentObject, boolValue);
									break;
								}
							case BPValueType.Number:
								{
									switch (_bpObject.objectTypeFullName)
									{
										case "System.Int16":
											{
												if (!short.TryParse(_bpObject.value, out var shortValue))
													shortValue = 0;

												fieldInfo.SetValue(_parentObject, shortValue);
												break;
											}

										case "System.Int32":
											{
												if (!int.TryParse(_bpObject.value, out var intValue))
													intValue = 0;

												fieldInfo.SetValue(_parentObject, intValue);
												break;
											}

										case "System.Int64":
											{
												if (!long.TryParse(_bpObject.value, out var longValue))
													longValue = 0;

												fieldInfo.SetValue(_parentObject, longValue);
												break;
											}

										case "System.UInt16":
											{
												if (!ushort.TryParse(_bpObject.value, out var ushortValue))
													ushortValue = 0;

												fieldInfo.SetValue(_parentObject, ushortValue);
												break;
											}

										case "System.UInt32":
											{
												if (!uint.TryParse(_bpObject.value, out var uintValue))
													uintValue = 0;

												fieldInfo.SetValue(_parentObject, uintValue);
												break;
											}

										case "System.UInt64":
											{
												if (!ulong.TryParse(_bpObject.value, out var ulongValue))
													ulongValue = 0;

												fieldInfo.SetValue(_parentObject, ulongValue);
												break;
											}

										case "System.Single":
											{
												if (!float.TryParse(_bpObject.value, out var floatValue))
													floatValue = 0;

												fieldInfo.SetValue(_parentObject, floatValue);
												break;
											}

										case "System.Double":
											{
												if (!double.TryParse(_bpObject.value, out var doubleValue))
													doubleValue = 0;

												fieldInfo.SetValue(_parentObject, doubleValue);
												break;
											}
									}
									break;
								}
							case BPValueType.String:
								{
									fieldInfo.SetValue(_parentObject, _bpObject.value);
									break;
								}
						}

						return null;
					}

				case BPValueType.Array:
					{
						var objectType = BPTypes.GetType(_bpObject.elementTypeFullName);
						var array = Array.CreateInstance(objectType, _bpObject.array.Count);

						var index = 0;
						foreach (var bpChildObject in _bpObject.array)
						{
							var childObject = DeserializeRecursively(bpChildObject, array);
							array.SetValue(childObject, index);
							++index;
						}

						return array;
					}

				case BPValueType.Object:
					{
						var objectType = BPTypes.GetType(_bpObject.objectTypeFullName);
						var @object = Activator.CreateInstance(objectType);

						foreach (var bpChildObject in _bpObject.members.Values)
						{
							DeserializeRecursively(bpChildObject, @object);
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
		public static BPObject Serialize<T>(this T _object) where T : class
		{
			if (_object == null)
				return null;

			var bpRootObject = SerializeRecursively(null, _object, string.Empty);
			return bpRootObject;
		}

		/// <summary>
		/// 역직렬화.
		/// </summary>
		public static T Deserialize<T>(this BPObject _bpObject) where T : class
		{
			if (_bpObject == null)
				return default(T);

			var rootObject = DeserializeRecursively(_bpObject, null) as T;
			return rootObject;
		}

		/// <summary>
		/// 오브젝트를 바이트배열로 변환.
		/// </summary>
		public static byte[] ObjectToBytes(this BPObject _bpObject)
		{
			if (_bpObject == null)
				return null;

			var bpBuffer = new BPBuffer();
			bpBuffer.Write(_bpObject);

			return bpBuffer.ByteArray;
		}

		/// <summary>
		/// 바이트배열을 오브젝트로 변환.
		/// </summary>
		public static BPObject BytesToObject(this byte[] _bytes)
		{
			if (_bytes == null || _bytes.Length == 0)
				return null;

			var bpBuffer = new BPBuffer(_bytes);
			if (!bpBuffer.Read(out BPObject bpObject))
				return null;

			return bpObject;
		}
	}
}
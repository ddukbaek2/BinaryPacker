using System;
using System.Collections.Generic;
using System.Reflection;


namespace BinaryPacker
{
	/// <summary>
	/// 타입.
	/// </summary>
	public static class BPTypes
	{
		/// <summary>
		/// 로드된 어셈블리목록.
		/// </summary>
		private static Dictionary<string, Assembly> loadedAssemblies = new Dictionary<string, Assembly>();

		/// <summary>
		/// 로드된 타입목록.
		/// </summary>
		private static Dictionary<string, Type> loadedTypes = new Dictionary<string, Type>();

		/// <summary>
		/// 어셈블리 로드.
		/// </summary>
		public static bool LoadAssembly(string _assemblyFullName)
		{
			var assembly = Assembly.Load(_assemblyFullName);
			return AddAssembly(assembly);
		}

		/// <summary>
		/// 어셈블리 추가.
		/// </summary>
		public static bool AddAssembly(Assembly _assembly)
		{
			if (_assembly == null)
				return false;

			if (loadedAssemblies.ContainsKey(_assembly.FullName))
				return false;

			loadedAssemblies.Add(_assembly.FullName, _assembly);

			foreach (var type in _assembly.GetTypes())
				BPTypes.AddType(type);

			return true;
		}

		/// <summary>
		/// 어셈블리 가져오거나 추가.
		/// </summary>
		public static Assembly GetOrAddAssembly(string _assemblyFullName)
		{
			if (!loadedAssemblies.TryGetValue(_assemblyFullName, out var assembly) || assembly == null)
			{
				assembly = Assembly.Load(_assemblyFullName);
				AddAssembly(assembly);
			}

			return assembly;
		}

		/// <summary>
		/// 타입 추가.
		/// </summary>
		public static bool AddType(Type _type)
		{
			if (_type == null)
				return false;

			if (loadedTypes.ContainsKey(_type.FullName))
				return false;

			loadedTypes.Add(_type.FullName, _type);
			return true;
		}

		/// <summary>
		/// 타입 반환.
		/// </summary>
		public static Type GetType(string _typeFullName)
		{
			if (string.IsNullOrEmpty(_typeFullName))
				return null;

			var type = Type.GetType(_typeFullName);
			if (type == null)
			{
				if (!loadedTypes.TryGetValue(_typeFullName, out type))
					return null;
			}

			return type;
		}
	}
}
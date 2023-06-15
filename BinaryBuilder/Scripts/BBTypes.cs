using System;
using System.Collections.Generic;
using System.Reflection;


namespace BinaryBuilder
{
	public static class BBTypes
	{
		private static Dictionary<string, Assembly> loadedAssemblies = new Dictionary<string, Assembly>();
		private static Dictionary<string, Type> loadedTypes = new Dictionary<string, Type>();

		public static bool LoadAssembly(string _assemblyFullName)
		{
			var assembly = Assembly.Load(_assemblyFullName);
			return AddAssembly(assembly);
		}

		public static bool AddAssembly(Assembly _assembly)
		{
			if (_assembly == null)
				return false;

			if (loadedAssemblies.ContainsKey(_assembly.FullName))
				return false;

			loadedAssemblies.Add(_assembly.FullName, _assembly);

			foreach (var type in _assembly.GetTypes())
				BBTypes.AddType(type);

			return true;
		}

		public static Assembly GetOrAddAssembly(string _assemblyFullName)
		{
			if (!loadedAssemblies.TryGetValue(_assemblyFullName, out var assembly) || assembly == null)
			{
				assembly = Assembly.Load(_assemblyFullName);
				AddAssembly(assembly);
			}

			return assembly;
		}

		public static bool AddType(Type _type)
		{
			if (_type == null)
				return false;

			if (loadedTypes.ContainsKey(_type.FullName))
				return false;

			loadedTypes.Add(_type.FullName, _type);
			return true;
		}

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
using System.Collections.Generic;


namespace BinaryBuilder
{
	public class BBNativeObject
	{
		public ulong nameSize = 0;
		public char[] name;

		public ulong type;

		public ulong valueSize = 0;
		public char[] value;

		public ulong arraySize = 0;
		public ulong arrayCount = 0;
		public BBNativeObject[] array;

		public ulong membersSize = 0;
		public ulong membersCount = 0;
		public BBNativeObject[] members;

		public ulong elementTypeFullNameSize = 0;
		public char[] elementTypeFullName;

		public ulong objectTypeFullNameSize = 0;
		public char[] objectTypeFullName;

		public ulong assemblyFullNameSize = 0;
		public char[] assemblyFullName;
	}
}
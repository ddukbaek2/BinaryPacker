namespace BinaryBuilder
{
	//public class BinaryFileData
	//{
	//	public ulong totalSize;
	//	public byte version;

	//	public ulong keySize;
	//	public string key;

	//	public ulong count;
	//	public LinkedList<BinaryClassData> data;
	//}

	//public class BinaryClassData
	//{
	//	public ulong size;
	//	public ulong count;
	//	public BinaryFieldData[] data;
	//}

	//public class BinaryFieldData
	//{
	//	public ulong totalSize;
	//	public BBObject name;
	//	public BBObject type;
	//	public BBObject data;
	//}

	//public class BBObject
	//{
	//	public ulong size;
	//	public ulong count;
	//	public byte[] data;
	//}

	public static class BinaryBuilder
    {
		public static byte[] Serialize<T>(T _object) where T : class
		{
			return null;
        }

        public static T Deserialize<T>(byte[] _bytes) where T : class, new()
        {
            return default;
        }
    }
}
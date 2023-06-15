//using System;
//using System.Text;


//namespace BinaryPacker
//{
//	public struct BBNativeData
//	{
//		public int size;
//		public int count;
//		public byte[] data;

//		public BBNativeData(int _value)
//		{
//			// 리틀엔디안으로 저장됨.
//			data = BitConverter.GetBytes(_value);
//			size = data.Length;
//			count = 1;
//		}

//		public BBNativeData(string _value)
//		{
//			data = Encoding.UTF8.GetBytes(_value);
//			size = data.Length;
//			count = _value.Length;
//		}
//	}

//	public class BBNativeObject
//	{
//		public ulong totalSize = 0;

//		public BBNativeData name;
//		public BBNativeData type = new BBNativeData();
//		public BBNativeData value = new BBNativeData();

//		public BBNativeData elementTypeFullName = new BBNativeData();
//		public BBNativeData objectTypeFullName = new BBNativeData();
//		public BBNativeData assemblyFullName = new BBNativeData();

//		public BBNativeData array = new BBNativeData();
//		public BBNativeData members = new BBNativeData();

//		public static BBNativeObject ToBBNativeObject(BPObject _bbObject)
//		{
//			var bbNativeObject = new BBNativeObject();
//			bbNativeObject.name = new BBNativeData(_bbObject.name);
//			bbNativeObject.type = new BBNativeData((int)_bbObject.type);
//			bbNativeObject.value = new BBNativeData(_bbObject.value);

//			bbNativeObject.elementTypeFullName = new BBNativeData(_bbObject.elementTypeFullName);
//			bbNativeObject.objectTypeFullName = new BBNativeData(_bbObject.objectTypeFullName);
//			bbNativeObject.assemblyFullName = new BBNativeData(_bbObject.assemblyFullName);

//			foreach (var bbChildObject in _bbObject.array)
//			{
//				ToBBNativeObject(bbChildObject);
//			}

//			return bbNativeObject;
//		}

//		public static BPObject ToBBObejct(BBNativeObject _bbNativeObject)
//		{
//			return null;
//		}
//	}
//}
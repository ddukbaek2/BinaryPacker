using System;
using System.Collections.Generic;
using System.Text;


namespace BinaryPacker
{
	/// <summary>
	/// 변수 버퍼.
	/// 호출 순서대로 바이트 쌓기.
	/// </summary>
	public class BPBuffer
	{
		public static StringBuilder stringBuilder = new StringBuilder();

		private List<byte> bytes;
		private byte[] byteArray;
		private int readOffset;

		/// <summary>
		/// 바이트 배열.
		/// </summary>
		public byte[] ByteArray
		{
			get
			{
				if (byteArray == null || byteArray.Length != bytes.Count)
					byteArray = bytes.ToArray();

				return byteArray;
			}
		}

		/// <summary>
		/// 읽는 시작 위치.
		/// </summary>
		public int ReadOffset
		{
			set
			{
				readOffset = Math.Min(Math.Max(value, 0), bytes.Count - 1);
			}
			get
			{
				return readOffset;
			}
		}

		/// <summary>
		/// 생성.
		/// </summary>
		public BPBuffer()
		{
			bytes = new List<byte>();
			Clear();
		}

		/// <summary>
		/// 생성.
		/// </summary>
		public BPBuffer(byte[] _bytes) : this()
		{
			bytes.AddRange(_bytes);
			byteArray = _bytes;
			readOffset = 0;
		}

		public void Clear()
		{
			bytes.Clear();
			byteArray = null;
			readOffset = 0;
		}

		public bool Read(out bool _value)
		{
			_value = BitConverter.ToBoolean(ByteArray, ReadOffset);
			ReadOffset += sizeof(bool);
			return true;
		}

		public bool Read(out byte _value)
		{
			_value = bytes[ReadOffset];
			ReadOffset += sizeof(byte);
			return true;
		}

		public bool Read(out ushort _value)
		{
			_value = BitConverter.ToUInt16(ByteArray, ReadOffset);
			ReadOffset += sizeof(ushort);
			return true;
		}

		public bool Read(out uint _value)
		{
			_value = BitConverter.ToUInt32(ByteArray, ReadOffset);
			ReadOffset += sizeof(uint);
			return true;
		}

		public bool Read(out ulong _value)
		{
			_value = BitConverter.ToUInt64(ByteArray, ReadOffset);
			ReadOffset += sizeof(ulong);
			return true;
		}

		public bool Read(out sbyte _value)
		{
			_value = (sbyte)bytes[ReadOffset];
			ReadOffset += sizeof(sbyte);
			return true;
		}

		public bool Read(out char _value)
		{
			_value = BitConverter.ToChar(ByteArray, ReadOffset);
			ReadOffset += sizeof(char);
			return true;
		}

		public bool Read(out short _value)
		{
			_value = BitConverter.ToInt16(ByteArray, ReadOffset);
			ReadOffset += sizeof(short);
			return true;
		}

		public bool Read(out int _value)
		{
			_value = BitConverter.ToInt32(ByteArray, ReadOffset);
			ReadOffset += sizeof(int);
			return true;
		}

		public bool Read(out long _value)
		{
			_value = BitConverter.ToInt64(ByteArray, ReadOffset);
			ReadOffset += sizeof(long);
			return true;
		}

		public bool Read(out float _value)
		{
			_value = BitConverter.ToSingle(ByteArray, ReadOffset);
			ReadOffset += sizeof(float);
			return true;
		}

		public bool Read(out double _value)
		{
			_value = BitConverter.ToDouble(ByteArray, ReadOffset);
			ReadOffset += sizeof(double);
			return true;
		}

		public bool Read(out string _value)
		{
			stringBuilder.Clear();
			_value = string.Empty;

			if (!Read(out int count))
				return false;

			for (int i = 0; i < count; ++i)
			{
				if (!Read(out char ch))
					return false;

				stringBuilder.Append(ch);
			}

			_value = stringBuilder.ToString();
			return true;
		}

		public bool Read(out DateTime _value)
		{
			_value = DateTime.MinValue;

			if (!Read(out long ticks))
				return false;

			_value = new DateTime(ticks);
			return true;
		}

		public void Write(bool _value)
		{
			var bytes = BitConverter.GetBytes(_value);
			this.bytes.AddRange(bytes);
		}

		public void Write(byte value)
		{
			bytes.Add(value);
		}

		public void Write(ushort value)
		{
			var bytes = BitConverter.GetBytes(value);
			this.bytes.AddRange(bytes);
		}

		public void Write(uint value)
		{
			var bytes = BitConverter.GetBytes(value);
			this.bytes.AddRange(bytes);
		}

		public void Write(ulong value)
		{
			var bytes = BitConverter.GetBytes(value);
			this.bytes.AddRange(bytes);
		}

		public void Write(sbyte value)
		{
			bytes.Add((byte)value);
		}

		public void Write(char value)
		{
			var bytes = BitConverter.GetBytes(value);
			this.bytes.AddRange(bytes);
		}

		public void Write(short value)
		{
			var bytes = BitConverter.GetBytes(value);
			this.bytes.AddRange(bytes);
		}

		public void Write(int value)
		{
			var bytes = BitConverter.GetBytes(value);
			this.bytes.AddRange(bytes);
		}

		public void Write(long value)
		{
			var bytes = BitConverter.GetBytes(value);
			this.bytes.AddRange(bytes);
		}

		public void Write(float value)
		{
			var bytes = BitConverter.GetBytes(value);
			this.bytes.AddRange(bytes);
		}

		public void Write(double value)
		{
			var bytes = BitConverter.GetBytes(value);
			this.bytes.AddRange(bytes);
		}

		public void Write(string value)
		{
			Write(value.Length);
			for (int i = 0; i < value.Length; ++i)
				Write(value[i]);
		}

		public void Write(DateTime value)
		{
			Write(value.Ticks);
		}

		public bool Read(out BPObject _bpObject)
		{
			_bpObject = new BPObject();

			if (!Read(out _bpObject.name)) return false;

			if (!Read(out byte type)) return false;
			_bpObject.type = (BPValueType)type;

			if (!Read(out _bpObject.value)) return false;

			if (!Read(out _bpObject.assemblyFullName)) return false;
			if (!Read(out _bpObject.objectTypeFullName)) return false;
			if (!Read(out _bpObject.elementTypeFullName)) return false;

			if (!Read(out int arrayCount)) return false;
			for (var i = 0; i < arrayCount; ++i)
			{
				if (!Read(out BPObject bpChildObject))
					return false;

				_bpObject.array.Add(bpChildObject);
			}

			if (!Read(out int membersCount)) return false;
			for (var i = 0; i < membersCount; ++i)
			{
				if (!Read(out BPObject bpChildObject))
					return false;

				_bpObject.members.Add(bpChildObject.name, bpChildObject);
			}

			return true;
		}

		public void Write(BPObject _bpObject)
		{
			Write(_bpObject.name);
			Write((byte)_bpObject.type);
			Write(_bpObject.value);

			Write(_bpObject.assemblyFullName);
			Write(_bpObject.objectTypeFullName);
			Write(_bpObject.elementTypeFullName);
					
			Write(_bpObject.array.Count);
			foreach (var bpChildObject in _bpObject.array)
				Write(bpChildObject);

			Write(_bpObject.members.Count);
			foreach (var bpChildObject in _bpObject.members.Values)
				Write(bpChildObject);
		}

		//public void Write(decimal[] value)
		//{
		//	Write(value.Length);
		//	for (int i = 0; i < value.Length; ++i)
		//		Write(value[i]);
		//}

		//public void Write(int[] value)
		//{
		//	Write(value.Length);
		//	for (int i = 0; i < value.Length; ++i)
		//		Write(value[i]);
		//}

		//public void Write(string[] value)
		//{
		//	Write(value.Length);
		//	for (int i = 0; i < value.Length; ++i)
		//		Write(value[i]);
		//}
	}
}
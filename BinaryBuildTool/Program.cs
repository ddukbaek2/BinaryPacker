using System;
using System.Collections.Generic;
using BinaryPacker;


namespace BinaryBuildTool
{
	public static class Program
	{
		public class TableData
		{
			public int id;
			public string value;
			public bool isFlag;
			public float position;
		}

		private static List<TableData> originalDatas;

		public static void Main(string[] args)
		{
			originalDatas = new List<TableData>();

			for (var i = 0; i < 100; ++i)
			{
				originalDatas.Add(new TableData { id = i + 1, value = i.ToString(), isFlag = i % 2 == 0, position = 15.5678f});
			}

			var startTime = default(DateTime);
			var endTime = default(DateTime);

			startTime = DateTime.Now;
			var bpObject = BPConvert.Serialize(originalDatas);
			endTime = DateTime.Now;
			Console.WriteLine("BBObject Serialize Time = {0:F4}s", (endTime - startTime).TotalSeconds);

			startTime = DateTime.Now;
			var newDatas = BPConvert.Deserialize<List<TableData>>(bpObject);
			endTime = DateTime.Now;
			Console.WriteLine("BBObject Deserialize Time = {0:F4}s", (endTime - startTime).TotalSeconds);

			startTime = DateTime.Now;
			var bytes = bpObject.ToBytes();
			endTime = DateTime.Now;
			Console.WriteLine("BBObject ToBytes Time = {0:F4}s", (endTime - startTime).TotalSeconds);

			startTime = DateTime.Now;
			var bbNewObject = bytes.ToBPObject();
			endTime = DateTime.Now;
			Console.WriteLine("BBObject ToBBObject Time = {0:F4}s", (endTime - startTime).TotalSeconds);

			Console.ReadKey();
		}
	}
}

using System;
using System.Collections.Generic;
using BinaryBuilder;


namespace BinaryBuildTool
{
	public static class Program
	{
		public class Data
		{
			public int id;
			public string value;
			public bool isFlag;
			public float position;
		}


		private static List<Data> originalDatas;

		public static void Main(string[] args)
		{
			originalDatas = new List<Data>();

			for (var i = 0; i < 100; ++i)
			{
				originalDatas.Add(new Data { id = i + 1, value = i.ToString(), isFlag = i % 2 == 0, position = 15.5678f});
			}

			var startTime = default(DateTime);
			var endTime = default(DateTime);

			startTime = DateTime.Now;
			var bbObject = BBConvert.Serialize(originalDatas);
			endTime = DateTime.Now;
			Console.WriteLine("BBObject Serialize Time = {0:F4}s", (endTime - startTime).TotalSeconds);

			startTime = DateTime.Now;
			var newDatas = BBConvert.Deserialize<List<Data>>(bbObject);
			endTime = DateTime.Now;
			Console.WriteLine("BBObject Deserialize Time = {0:F4}s", (endTime - startTime).TotalSeconds);

			startTime = DateTime.Now;
			var bytes = bbObject.ToBytes();
			endTime = DateTime.Now;
			Console.WriteLine("BBObject ToBytes Time = {0:F4}s", (endTime - startTime).TotalSeconds);

			startTime = DateTime.Now;
			var bbNewObject = bytes.ToBBObject();
			endTime = DateTime.Now;
			Console.WriteLine("BBObject ToBBObject Time = {0:F4}s", (endTime - startTime).TotalSeconds);

			Console.ReadKey();
		}
	}
}

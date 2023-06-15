using System;
using System.Collections.Generic;
using BinaryPacker;


namespace BinaryBuildTool
{
	/// <summary>
	/// 바이너리 빌드 도구.
	/// </summary>
	public static class Program
	{
		// 변환할 데이터 포맷.
		public class TableData
		{
			public int id;
			public string value;
			public bool isFlag;
			public float position;
		}


		public static void Main(string[] args)
		{
			var originalDatas = new List<TableData>();

			const int maxCount = 1000;
			for (var i = 0; i < maxCount; ++i)
			{
				var tableData = new TableData
				{
					id = i + 1,
					value = i.ToString(),
					isFlag = i % 2 == 0,
					position = 15.5678f,
				};

				originalDatas.Add(tableData);
			}

			var startTime = default(DateTime);
			var endTime = default(DateTime);

			//// BPObject로 변환.
			//startTime = DateTime.Now;
			//var bpObject = BPConvert.Serialize(originalDatas);
			//endTime = DateTime.Now;
			//Console.WriteLine("BPObject Serialize Time = {0:F4}s", (endTime - startTime).TotalSeconds);

			//// 원래의 데이터로 변환.
			//startTime = DateTime.Now;
			//var newDatas = BPConvert.Deserialize<List<TableData>>(bpObject);
			//endTime = DateTime.Now;
			//Console.WriteLine("BPObject Deserialize Time = {0:F4}s", (endTime - startTime).TotalSeconds);

			//// 바이트배열로 변환.
			//startTime = DateTime.Now;
			//var bytes = bpObject.ObjectToBytes();
			//endTime = DateTime.Now;
			//Console.WriteLine("BPObject Object To Bytes Time = {0:F4}s", (endTime - startTime).TotalSeconds);

			//// BPObject로 변환.
			//startTime = DateTime.Now;
			//var bpNewObject = bytes.BytesToObject();
			//endTime = DateTime.Now;
			//Console.WriteLine("BPObject Byte To Object Time = {0:F4}s", (endTime - startTime).TotalSeconds);
			
			// 데이터를 바이트로 변환.
			startTime = DateTime.Now;
			var bytes = BPConvert.Serialize(originalDatas).ObjectToBytes();
			endTime = DateTime.Now;
			Console.WriteLine("Data to Bytes Time = {0:F4}s", (endTime - startTime).TotalSeconds);

			// 바이트를 데이터로 변환.
			startTime = DateTime.Now;
			var newDatas = bytes.BytesToObject().Deserialize<List<TableData>>();
			endTime = DateTime.Now;
			Console.WriteLine("Bytes to Data Time = {0:F4}s", (endTime - startTime).TotalSeconds);

			// 완료 후 대기.
			Console.ReadKey();
		}
	}
}

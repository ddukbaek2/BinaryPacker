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
			//public string value;
			//public bool isFlag;
			//public float position;
		}


		/// <summary>
		/// 시작됨.
		/// </summary>
		public static void Main(string[] args)
		{
			var maxCount = 1000;
			var originalDatas = new List<TableData>(maxCount);
			for (var i = 0; i < maxCount; ++i)
			{
				var tableData = new TableData
				{
					id = i + 1,
					//value = i.ToString(),
					//isFlag = i % 2 == 0,
					//position = 15.5678f,
				};

				originalDatas.Add(tableData);
			}

			var startTime = default(DateTime);
			var endTime = default(DateTime);

			// 과정.
			// Serialize : Object ==> (BPObject ==> ByteArray)
			// Deserialize : (ByteArray ==> BPObject) ==> Object

			// 데이터를 바이트배열로 변환.
			startTime = DateTime.Now;
			var byteArray = BPConvert.Serialize<List<TableData>>(originalDatas);
			endTime = DateTime.Now;
			Console.WriteLine("Data to ByteArray Time = {0:F4}s", (endTime - startTime).TotalSeconds);

			// 바이트배열을 데이터로 변환.
			startTime = DateTime.Now;
			var newDatas = BPConvert.Deserialize<List<TableData>>(byteArray);
			endTime = DateTime.Now;
			Console.WriteLine("ByteArray to Data Time = {0:F4}s", (endTime - startTime).TotalSeconds);

			// 완료 후 대기.
			Console.ReadKey();
		}
	}
}

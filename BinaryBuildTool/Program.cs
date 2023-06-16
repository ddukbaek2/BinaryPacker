using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BinaryPacker;



namespace BinaryBuildTool
{
	/// <summary>
	/// 바이너리 빌드 도구.
	/// </summary>
	public static class Program
	{
		/// <summary>
		/// 열거체.
		/// </summary>
		public enum TableEnum
		{
			Case1 = 100000,
			Case2 = 200000,
			Case3 = 300000,
		}


		// 변환할 데이터 포맷.
		public class TableData
		{
			public int id;
			public TableEnum tableEnum;
			public string value;
		}


		/// <summary>
		/// 랜덤.
		/// </summary>
		private static readonly Random random = new Random();

		/// <summary>
		/// 시작됨.
		/// </summary>
		public static void Main(string[] _args)
		{
			var maxCount = 100000;
			var originalDatas = new List<TableData>(maxCount);
			for (var i = 0; i < maxCount; ++i)
			{
				var tableData = new TableData
				{
					id = i + 1,
					tableEnum = TableEnum.Case3,
					value = RandomText(random.Next(20)),
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
			Console.WriteLine("Serialize() : {0:F4}s", (endTime - startTime).TotalSeconds);

			// 바이트배열을 파일로 저장.
			startTime = DateTime.Now;
			File.WriteAllBytes("C:\\data.bytes", byteArray);
			endTime = DateTime.Now;
			Console.WriteLine("WriteFile() : {0:F4}s", (endTime - startTime).TotalSeconds);

			// 파일에서 바이트배열 불러오기.
			startTime = DateTime.Now;
			var result = File.ReadAllBytes("C:\\data.bytes");
			endTime = DateTime.Now;
			Console.WriteLine("ReadFile() : {0:F4}s", (endTime - startTime).TotalSeconds);

			// 바이트배열을 데이터로 변환.
			startTime = DateTime.Now;
			var newDatas = BPConvert.Deserialize<List<TableData>>(result);
			endTime = DateTime.Now;
			Console.WriteLine("Deserialize() : {0:F4}s", (endTime - startTime).TotalSeconds);

			// 완료 후 대기.
			Console.ReadKey();
		}

		/// <summary>
		/// 랜덤문자열 반환.
		/// </summary>
		public static string RandomText(int _count)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			return new string(Enumerable.Repeat(chars, _count).Select(s => s[random.Next(s.Length)]).ToArray());
		}
	}
}
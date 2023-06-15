using System;
using System.Collections.Generic;
using BinaryBuilder;


namespace BinaryBuildTool
{
	public class Data
	{
		//public int id;
		public string value;
	}

	public class Program
	{
		public static void Main(string[] args)
		{
			var datas = new List<Data>();
			for (var i = 0; i < 1; ++i)
			{
				//datas.Add(new Data { id = i + 1, value = i.ToString() });
				datas.Add(new Data { value = i.ToString() });
			}

			var startTime = default(DateTime);
			var endTime = default(DateTime);

			startTime = DateTime.Now;
			var bbObject = BBConvert.Serialize(datas);
			endTime = DateTime.Now;
			Console.WriteLine("BBObject Serialize Time = {0}", endTime - startTime);

			startTime = DateTime.Now;
			var newDatas = BBConvert.Deserialize(bbObject);
			endTime = DateTime.Now;
			Console.WriteLine("BBObject Deserialize Time = {0}", endTime - startTime);

			Console.WriteLine(datas);
			//Console.ReadKey();
		}
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharpTracer.Model
{
	public delegate void StoreDelegate(object data, FileStream fs);
	public delegate void LoadDelegate(object data, FileStream fs);

	public class DataManager
	{
		public static List<String> Path;

		public static StoreDelegate storeFunc;
		public static LoadDelegate loadFunc;

		public static bool StoreFile(StoreDelegate sd, String path, String fileName, object data)
		{
			string filePath = path + "/" + fileName;
			FileStream fs = File.OpenWrite(filePath);

			sd(data, fs);

			return true;
		}

		public static bool LoadFile(LoadDelegate ld, string path, string fileName, object data)
		{
			string filePath = path + "/" + fileName;
			FileStream fs = File.OpenRead(filePath);

			ld(data, fs);

			return true;
		}

		List<string> GetDir(string dir)
		{
			List<string> folders;

			folders = Directory.GetDirectories(dir).ToList();

			return folders;
		}

		public static List<string> GetFiles(string dir)
		{
			List<string> files = new List<string>();

			foreach (String d in Directory.GetFiles(dir))
			{
				if (File.GetAttributes(d).HasFlag(FileAttributes.Directory))
				{
					files.Add(d.Substring(d.LastIndexOf('/') + 1));
				}
			}

			return files;
		}

		void SetPath(int number, string path)
		{
			Path[number] = path;
		}

	}
}

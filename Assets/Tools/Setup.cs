using UnityEngine;
using static UnityEditor.AssetDatabase;
using static System.IO.Directory;
using static System.IO.Path;
using UnityEditor;

public static class Setup {
	[MenuItem("Tools/Setup/Create Default Folders")]
	public static void CreateDefaultFolders() {
		Folders.CreateDefault("_Project", "Animation", "Art", "Materials", "Prefabs", "ScriptableObjects", "Scripts", "Settings");
		Refresh();
	}
	private static class Folders {
		public static void CreateDefault(string root, params string[] folders) {
			string fullPath = Combine(Application.dataPath, root);
			foreach (string folder in folders) {
				string path = Combine(fullPath, folder);
				if (!Exists(path)) CreateDirectory(path);
			}
		}
	}
}

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

	[MenuItem("Tools/Setup/Import Assets")]
	public static void ImportAssets() {
		Assets.ImportAsset("DOTween HOTween v2.unitypackage", "Demigiant/Editor ExtensionsAnimation");
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
	public static class Assets {
		public static void ImportAsset(string asset, string subfolder, string folder = "C:/Users/Alex/AppData/Roaming/Unity/Asset Store-5.x") {
			ImportPackage(Combine(folder, subfolder, asset), false);
		}
	}
}

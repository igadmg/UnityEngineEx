using System.Collections;



namespace UnityEngineEx {
	public static class ListEx {
		public static L DestroyObjects<L>(this L list) where L : IEnumerable {
			foreach (UnityEngine.Object child in list) {
				UnityEngine.Object.Destroy(child);
			}

			return list;
		}
	}
}

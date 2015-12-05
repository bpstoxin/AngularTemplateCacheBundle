using System.Text;

namespace AngularTemplateCacheBundle {
	public static class UrlUtils {
		private static string HostnameForTesting;


		public static string Combine(params string[] urlParts) {
			if (urlParts == null) {
				return "";
			}
			var builder = new StringBuilder();
			foreach (var urlPart in urlParts) {
				if (urlPart == null) {
					continue;
				}
				if (builder.Length > 0 && builder[builder.Length - 1] != '/') {
					builder.Append('/');
				}
				if (builder.Length > 0 && urlPart.StartsWith("/")) {
					builder.Append(urlPart.Substring("/".Length));
				} else {
					builder.Append(urlPart);
				}
			}
			return builder.ToString();
		}
	}
}
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Optimization;

namespace AngularTemplateCacheBundle {
	public class TemplateCacheBundleItemTransform : IItemTransform {
		private const string AngularTemplateCacheTemplate = @"angular.module('{0}', []).run(['$templateCache', function($templateCache) {{
	$templateCache.put('{0}', 
{1});
}} ]);";
		public TemplateCacheBundle Bundle { get; private set; }
		public BundleContext Context { get; private set; }

		public TemplateCacheBundleItemTransform(BundleContext context, TemplateCacheBundle bundle) {
			Bundle = bundle;
			Context = context;
		}

		/// <summary>
		///     Encodes a string to be represented as a string literal. The format
		///     is essentially a JSON string.
		///     The string returned includes outer quotes
		///     Example Output: "Hello \"Rick\"!\r\nRock on"
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static string EncodeJsString(string s) {
			var sb = new StringBuilder();
			sb.Append("\"");
			foreach (var c in s) {
				switch (c) {
					case '\"':
						sb.Append("\\\"");
						break;
					case '\\':
						sb.Append("\\\\");
						break;
					case '\b':
						sb.Append("\\b");
						break;
					case '\f':
						sb.Append("\\f");
						break;
					case '\n':
						sb.Append("\\n");
						break;
					case '\r':
						sb.Append("\\r");
						break;
					case '\t':
						sb.Append("\\t");
						break;
					default:
						var i = (int) c;
						if (i < 32 || i > 127) {
							sb.AppendFormat("\\u{0:X04}", i);
						} else {
							sb.Append(c);
						}
						break;
				}
			}
			sb.Append("\"");

			return sb.ToString();
		}

		public static string Indent(int indentLevel, string value) {
			var indentString = "";
			for (var index = 0; index < indentLevel; index++) {
				indentString += "\t";
			}

			return Regex.Replace(value, "^", indentString, RegexOptions.Multiline);
		}

		public string Process(string includedVirtualPath, string input) {
			var newPath = includedVirtualPath.Replace("\\", "/");
			newPath = newPath.Replace("~/", "");
			newPath = UrlUtils.Combine(Context.HttpContext.Request.ApplicationPath, newPath);
			//			newPath = newPath.Replace(Bundle.BaseCachePath, );
			var jsHtml = EncodeJsString(input);

			jsHtml = jsHtml.Replace("\\r\\n", "\" + \r\n\"");
			jsHtml = Indent(2, jsHtml);

			var template = string.Format(AngularTemplateCacheTemplate, newPath, jsHtml);

			return template;
		}
	}
}
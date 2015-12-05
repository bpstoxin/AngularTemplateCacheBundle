using System.Collections.Generic;
using System.Text;
using System.Web.Optimization;

namespace AngularTemplateCacheBundle {
	public class TemplateCacheBundleTransform : IBundleTransform {
		public void Process(BundleContext context, BundleResponse response) {
			var bundle = (TemplateCacheBundle)context.BundleCollection.GetBundleFor(context.BundleVirtualPath);
			var files = bundle.EnumerateFiles(context);

			var dependencies = new StringBuilder();
			foreach (var file in files) {
				var cacheName = file.VirtualFile.VirtualPath;
				if (dependencies.Length > 0) {
					dependencies.Append(", ");
				}
				dependencies.AppendFormat("'{0}'", cacheName);
			}
			response.Files = new List<BundleFile>();
			response.Content = string.Format(@"angular.module('{0}', [{1}]);
{2}
", bundle.ModuleName, dependencies, response.Content);
		}
	}
}
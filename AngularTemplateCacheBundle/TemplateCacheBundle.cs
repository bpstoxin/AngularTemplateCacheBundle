using System.Collections.Generic;
using System.Web.Optimization;

namespace AngularTemplateCacheBundle {
	public class TemplateCacheBundle : Bundle {
		public string BaseCachePath { get; private set; }
		public string ModuleName { get; private set; }

		public TemplateCacheBundle(string virtualPath, string baseCachePath, string moduleName) : base(virtualPath) {
			BaseCachePath = baseCachePath;
			Transforms.Add(new TemplateCacheBundleTransform());
			Builder = new TemplateCacheBundleBuilder();
			ModuleName = moduleName;
		}
		public class TemplateCacheBundleBuilder : DefaultBundleBuilder, IBundleBuilder {
			string IBundleBuilder.BuildBundleContent(Bundle bundle, BundleContext context, IEnumerable<BundleFile> files) {
				foreach (var bundleFile in files) {
					bundleFile.Transforms.Add(new TemplateCacheBundleItemTransform(context, (TemplateCacheBundle)bundle));
				}
				return base.BuildBundleContent(bundle, context, files);
			}
		}
	}
}
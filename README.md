# AngularTemplateCacheBundle
ASP.NET Bundle to bundle all partial view templates into a single file

This is a pretty basic class that will bundle all the view templates in a specified directory
into a single file to be included into a project.  

NOTE - When optimizations are turned off, no file is generated causing angular to make a request
for the file itself.  This makes development much easier as the files downloaded are the source
files rather than the bundled up templates inlined into a single file.

# Usage
## BundleConfig.cs

Add the bundle to your bundles collection.

```C#
public class BundleConfig {
	public static void RegisterBundles(BundleCollection bundles) {
        ...
        // Uncomment the following line to turn optimizations on
		// BundleTable.EnableOptimizations = true;
        ...
        bundles.Add(new TemplateCacheBundle("~/path/to/bundle", "~/path/to/app", "MODULE_NAME.tpl")
        	.IncludeDirectory("~/path/to/app", "*.tpl.html", true)
        );
        ...
    }
}
```

## _Layout.cshtml

Add a reference to your _Layout.cshtml file:
```
@Scripts.Render("~/path/to/bundle")
```

## Define empty module for the templates
We must put the templates into their own module.  For this reason, we must define it prior to adding it
as a dependency to our app module.  When optimization is turned on, it will get redefined after the 
app module is defined.
```
angular.module('MODULE_NAME.tpl', []);
```

## Include templates as dependency in your angular module
```
angular.module('MODULE_NAME', [
	'MODULE_NAME.tpl'
])

```

## Example Usages
You can simply use the URL anywhere you would provide a URL to in angular.

- ng-include
```
<div ng-include="'/APP_NAME/path/to/template.tpl.html'"></div>
```
- Directive
```
angular.module('MODULE_NAME')
	.directive('DIRECTIVE_NAME', function () {
		return {
			templateUrl: '/APP_NAME/path/to/template.tpl.html',
		}
	})
;
```

# Best Practices
- Use multiple bundles to package different groups of templates together rather than bundle all
templates into one huge file.
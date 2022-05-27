:: --Build the project BundleMaker.Core--
:: Build core project
msbuild .\BundleMaker.Console\BundleMaker.Core.csproj


:: --Build project BundleMaker.Console--
:: Build whole project
msbuild .\BundleMaker.Console\BundleMaker.Console.csproj

:: Copy the bundle files into output folder.
msbuild .\BundleMaker.Console\BundleMaker.Console.csproj -t:CopyBundle
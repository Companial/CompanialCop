# BC CodeCop Analyzer

This code analyzer is meant to check AL code for all sorts of problems, the analyzer allows you to write more reliable, secure and reusable code.

## Contribution

If you have any rule on mind that would be nice to be covered, **please start a new [discussion](https://github.com/StefanMaron/BusinessCentral.LinterCop/discussions)!** then we can maybe sharpen the rule a bit if necessary. This way we can build value for all of us. If you want to write the rule yourself you can of course also submit a pull request ;)

## How to use

### Manual Compile

1. Take `BCCodeCopAnalyzer.dll` and place it into your AL Extension folder. For Example `%userprofile%\.vscode\extensions\ms-dynamics-smb.al-7.4.502459\bin\`
2. Run the AL Compiler `. %userprofile%\.vscode\extensions\ms-dynamics-smb.al-9.0.605172\bin\alc.exe /project:"<PathToYourAlProject>" /packagecachepath:"<PathToYour.alpackages>" /analyzer:"userprofile%\.vscode\extensions\ms-dynamics-smb.al-9.0.605172\bin\BCCodeCopAnalyzer.dll"`

### In VS Code

1. Add path to `BCCodeCopAnalyzer.dll` to the `"al.codeAnalyzers"` setting in either user, workspace or folder settings
2. Be aware that folder settings overwrite workspace and workspace overwrite user settings. If you have codecops defined in folder settings, the codecops defined in the user settings won't be applied.

### BcContainerHelper

For manual compile you can use the `Compile-AppInBcContainer` command and pass the path to the `BCCodeCopAnalyzer.dll` in via the parameter `-CustomCodeCops`.

If you are using `Run-ALPipeline` in your build pipelines you can also pass in the `BCCodeCopAnalyzer.dll` in via the parameter `-CustomCodeCops`. To have the correct compiler dependencies you should also load the latest compiler from the marketplace. Add `-vsixFile (Get-LatestAlLanguageExtensionUrl)` to do so.

Be aware though, the `BCCodeCopAnalyzer.dll` needs to be placed in a folder shared with the container.

## Rules

|Id| Title|Default Severity|
|---|---|---|
|BC0001|Variable name should not contain whitespace|Warning|
|BC0002|Variable name should not contain wildcard symbols such as % or &|Warning|
|BC0003|Procedure name should not contain whitespace|Warning|
|BC0004|Global variables section should be above triggers and procedures|Warning|
|BC0005|Objects which type is: Table, Page, XmlPort, Report, Query must have caption property|Warning|
|BC0006|Table Fields must have caption property|Warning|
|BC0007|Enum Value must have caption property|Warning|
|BC0008|Page parts: Request Page, Page Group, Page Part, Page Action, Page Action Group must have caption property|Warning|
|BC0009|First 19 Field IDs are reserved to primary key fields|Warning|
|BC0010|All fields in table extensions should be numbered in the dedicated extension or PTE range|Warning|
|BC0011|All values in enum extensions should be numbered in the dedicated extension or PTE range|Warning|
|BC0012|Empty Actions sections should be removed|Warning|
|BC0013|Empty OnRun triggers should be removed|Warning|
|BC0014|Hardcoding IP addresses is security-sensitive|Warning|
|BC0015|Email and Phone No must not be present in any part of the source code|Warning|

## How to disable certain rules?

Since the analyzer integrates with the AL compiler directly, you can use the custom rule sets like you are used to from the other code cops.
https://docs.microsoft.com/en-us/dynamics365/business-central/dev-itpro/developer/devenv-rule-set-syntax-for-code-analysis-tools

Of course you can also use pragmas for disabling a rule just for a certain place in code.
https://docs.microsoft.com/en-us/dynamics365/business-central/dev-itpro/developer/directives/devenv-directive-pragma-warning
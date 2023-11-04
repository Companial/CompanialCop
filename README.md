# BC CodeCop Analyzer

This code analyzer is meant to check AL code for all sorts of coding conventions, the analyzer allows you to write more reliable, secure and reusable code. 

The analyzer is meant to be an extension to the publicly available [BusinessCentral.LinterCop](https://github.com/StefanMaron/BusinessCentral.LinterCop). 

As rules are developed in this repository, discussions for moving the rules to the LinterCop are also opened and if there is enough interests, pull requests will be made to that repository.

## Contribution

If you have a rule on your mind that would be a good addition, **please start a new [discussion](https://github.com/StefanMaron/BusinessCentral.LinterCop/discussions) on the BusinessCentral.LinterCop repo.** The community is bigger there, and it's our goal to grow that repository instead of maintaining two repositories.

If you found a bug in one of the existing rules, open a new issue and try to give as many details how to reproduce the bug.

Pull requests are always welcome, but again, new rules should be added to the BusinessCentral.LinterCop instead.

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

|Id| Title|Default Severity|Enabled|
|---|---|---|---|
|CM0001|Name for primary key must be PK.|Warning|Yes|
|CM0002|Variable with suffix Tok must be locked and locked variables must have suffix Tok.|Warning|Yes|
|CM0003|Procedure name must not contain whitespaces.|Warning|Yes|
|CM0004|First 19 field IDs are reserved for primary key fields.|Warning|Yes|
|CM0005|The Enum identifier must be within the allowed range.|Warning|Yes|
|CM0006|IP address must not be present in any part of the source code.|Warning|Yes|
|CM0007|FlowFields must not be editable.|Warning|Yes|
|CM0008|Commit() must have a comment to justify its existence.|Warning|Yes|
|CM0009|Use of hardcoded object IDs in functions is not allowed.|Warning|Yes|
|CM0010|Caption must be specified.|Warning|Yes|
|CM0011|Procedure prototype must not end with semicolon.|Warning|Yes|
|CM0012|Procedure must be either local or internal.|Warning|Yes|
|CM0013|ToolTip must end with a dot.|Warning|Yes|
|CM0014|Position for global variables, triggers and methods must be correct.|Warning|Yes|
|CM0015|Msg and Err labels should end with a dot, Qst should end with a question mark.|Warning|Yes|
|CM0016|Internal Methods must be invoked with explicit parameters.|Warning|Yes|
|CM0017|Object should not have empty sections.|Warning|Yes|
|CM0018|Objects need to have the Access/Extensible properties defined.|Warning|Yes|
|CM0019|Local variable name should not contain whitespace/wildcard symbols.|Warning|Yes|
|CM0020|Global variable name should not contain whitespace/wildcard symbols.|Warning|Yes|
|CM0021|Parameter name should not contain whitespace/wildcard symbols.|Warning|Yes|
|CM0022|GridLayout property must not have value Rows|Warning|Yes|
|CM0023|The identifier must have at least one of the mandatory affixes.|Warning|Yes|
|CM0024|Empty captions should be locked.|Warning|Yes|
|CM0025|SetAutoCalcFields must not be invoked on Normal fields.|Warning|Yes|
|CM0026|Zero (0) Enum Value should be reserved for Empty Value.|Warning|Yes|
|CM0028|When using get the values provided must match the values required by table key.|Warning|Yes|
|CM0029|Option data type is not allowed.|Warning|Yes|
|CM0030|Method parameters are not used.|Warning|Yes|
|CM0031|Object is unused in project.|Warning|No|
|CM0032|Property already exists in object level with the same value.|Warning|Yes|
|CM0033|Editable property already exists in object level.|Warning|Yes|
|CM0035|Global procedure is unused in project.|Warning|No|

## How to disable certain rules?

Since the analyzer integrates with the AL compiler directly, you can use the custom rule sets like you are used to from the other code cops.
https://docs.microsoft.com/en-us/dynamics365/business-central/dev-itpro/developer/devenv-rule-set-syntax-for-code-analysis-tools

Of course you can also use pragmas for disabling a rule just for a certain place in code.
https://docs.microsoft.com/en-us/dynamics365/business-central/dev-itpro/developer/directives/devenv-directive-pragma-warning

## Warning
The released version of CompanialCop is always built against the latest major version of AL. If you try to use the last release with earlier versions of AL compiler, you will receive errors. If you need the analyzer to be compatible with an earlier version, clone the repository, override the references with references from the version you want to build against and build the project. As time goes on, you might have issues that earlier versions of references do not include the symbols that are required in some rules.
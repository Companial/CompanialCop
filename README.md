# BC CodeCop Analyzer

This code analyzer is meant to check AL code for all sorts of problems, the analyzer allows you to write more reliable, secure and reusable code.

## Contribution

If you have any rule on mind that would be nice to be covered or you found a bug in existing rule, **please create new Backlog Item or Bug in the [Backlog](https://dev.azure.com/1cfnav/INTERNAL-BC%20tools/_backlogs/backlog/INTERNAL-BC%20tools%20Team/Features/)** and link it with BC CodeCop Analyzer feature.

Of course you want to create the rule yourself you can also submit a pull request ;)

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
|CM0001|Name for primary key must be PK.|Warning|
|CM0002|Variable with suffix Tok must be locked and locked variables must have suffix Tok.|Warning|
|CM0003|Procedure name must not contain whitespaces.|Warning|
|CM0004|First 19 field IDs are reserved for primary key fields.|Warning|
|CM0005|The Enum identifier must be within the allowed range.|Warning|
|CM0006|IP address must not be present in any part of the source code.|Warning|
|CM0007|FlowFields must not be editable.|Warning|
|CM0008|Commit() must have a comment to justify its existence.|Warning|
|CM0009|Use of hardcoded object IDs in functions is not allowed.|Warning|
|CM0010|Caption must be specified.|Warning|
|CM0011|Procedure prototype must not end with semicolon.|Warning|
|CM0012|Procedure must be either local or internal.|Warning|
|CM0013|ToolTip must end with a dot.|Warning|
|CM0014|Position for global variables, triggers and methods must be correct.|Warning|
|CM0015|Msg and Err labels should end with a dot, Qst should end with a question mark.|Warning|
|CM0016|Internal Methods must be invoked with explicit parameters.|Warning|
|CM0017|Object should not have empty sections.|Warning|
|CM0018|Objects need to have the Access/Extensible properties defined.|Warning|
|CM0019|Local variable name should not contain whitespace/wildcard symbols.|Warning|
|CM0020|Global variable name should not contain whitespace/wildcard symbols.|Warning|
|CM0021|Parameter name should not contain whitespace/wildcard symbols.|Warning|
|CM0022|GridLayout property must not have value Rows|Warning|
|CM0023|The identifier must have at least one of the mandatory affixes.|Warning|
|CM0024|Empty captions should be locked.|Warning|
|CM0025|SetAutoCalcFields must not be invoked on Normal fields.|Error|
|CM0026|Zero (0) Enum Value should be reserved for Empty Value.|Warning|

## How to disable certain rules?

Since the analyzer integrates with the AL compiler directly, you can use the custom rule sets like you are used to from the other code cops.
https://docs.microsoft.com/en-us/dynamics365/business-central/dev-itpro/developer/devenv-rule-set-syntax-for-code-analysis-tools

Of course you can also use pragmas for disabling a rule just for a certain place in code.
https://docs.microsoft.com/en-us/dynamics365/business-central/dev-itpro/developer/directives/devenv-directive-pragma-warning
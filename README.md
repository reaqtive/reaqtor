![https://reaqtive.net](reaqtor-header.svg)

[![.NET Foundation](https://img.shields.io/badge/.NET%20Foundation-blueviolet.svg)](https://www.dotnetfoundation.org/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Build Status](https://dev.azure.com/dotnet/Reaqtor-private/_apis/build/status/reaqtive.reaqtor?branchName=main)](https://dev.azure.com/dotnet/Reaqtor-private/_build/latest?definitionId=155&branchName=main)
[![codecov](https://codecov.io/gh/reaqtive/reaqtor/branch/main/graph/badge.svg?token=AD59K911YN)](https://codecov.io/gh/reaqtive/reaqtor) [![NuGet](https://img.shields.io/badge/nuget-reaqtive-blue)](https://www.nuget.org/profiles/reaqtive)

# About Reaqtor

[Reaqtor](https://reaqtive.net) is a framework for reliable, stateful, distributed, and scalable event processing based on [Reactive Extensions (Rx)](https://github.com/dotnet/reactive).

Reaqtor has been in development for over 10 years. It is the evolution of the Reactive Extensions & powers services across Bing and M365.

## .NET Foundation

Reaqtor is part of the [.NET Foundation](https://www.dotnetfoundation.org/).

## License

Reaqtor is available under the [MIT open source license](https://opensource.org/licenses/MIT).

## Building

To build Reaqtor, you will need to install the [.NET SDK](https://dotnet.microsoft.com/download) ([version 5.0](https://dotnet.microsoft.com/download/dotnet/5.0) or later). Clone this repo, and in the root folder run this command:

```
dotnet build All.sln
```

Alternatively, if you use Visual Studio 2019, then as long as you have enabled the .NET SDK and .NET 5 runtimes in the Visual Studio installer, you can open that same `All.sln` file in Visual Studio and build it in the usual way.

### Running Example Notebooks

Many of the examples in the documentation in this repository take the form of notebooks, to make the examples interactively runnable. Since Reaqtor is a .NET technology, these notebooks contain C#, so you'll need a Jupyter notebook editor with support for a .NET kernel. For example, you use Visual Studio Code with the .NET Interactive Notebooks extension. You can install Visual Studio Code from https://code.visualstudio.com/ and then you can install the free [.NET Interactive Notebook](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.dotnet-interactive-vscode) extension from the VS Code marketplace.

With these installed, you will be able to open any of the `.ipynb` files in this repository. For example, in the [Reaqtor/Samples/IoT/Reaqtor.IoT](./Reaqtor/Samples/IoT/Reaqtor.IoT) folder, there are two such files. When you open one, VS Code might ask you whether you want to trust the file. You will need to indicate that you do if you want to run the code in a notebook. If you had already installed tools in Visual Studio Code for running Python notebooks, you may see a prompt asking which notebook kernel to use. Select `.NET Interactive` (and not a Python kernel). You will then be able to run the C# code in any of the code cells in the notebook. (You will need to have built Reaqtor first for this to work, because these various example notebooks load the libraries built by the source in this repository.)

## Contributor License Agreement

We appreciate community contributions to code repositories open sourced by the .NET Foundation. By signing a contributor license agreement, we ensure that the community is free to use your contributions.

### Review the CLA document
The [.NET Foundation Contributor License Agreement (CLA)](https://github.com/dotnet-foundation/foundation/blob/master/guidance/net-foundation-contribution-license-agreement.pdf) document is available for review as a PDF.

### Sign the CLA
When you contribute to a .NET Foundation open source project on GitHub with a new pull request, a bot will evaluate whether you have signed the CLA. If required, the bot will comment on the pull request, including a link to this system to accept the agreement.

## Code of Conduct

This project has adopted a code of conduct adapted from the [Contributor Covenant](http://contributor-covenant.org/) to clarify expected behavior in our community. This code of conduct has been [adopted by many other projects](http://contributor-covenant.org/adopters/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [&#104;&#101;&#108;&#108;&#111;&#064;&#101;&#110;&#100;&#106;&#105;&#110;&#046;&#099;&#111;&#109;](&#109;&#097;&#105;&#108;&#116;&#111;:&#104;&#101;&#108;&#108;&#111;&#064;&#101;&#110;&#100;&#106;&#105;&#110;&#046;&#099;&#111;&#109;) with any additional questions or comments.

## Acknowledgements 

A list of people who have contributed to the development of Reaqtor is featured in [A History of Reaqtor](https://reaqtive.net/history), by [Bart De Smet](https://github.com/bartdesmet).

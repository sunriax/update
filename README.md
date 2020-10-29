[![Version: 1.0 Release](https://img.shields.io/badge/Version-1.0%20Release-green.svg)](https://github.com/sunriax) [![Build Status](https://www.travis-ci.org/sunriax/update.svg?branch=master)](https://www.travis-ci.org/sunriax/update) [![codecov](https://codecov.io/gh/sunriax/update/branch/master/graph/badge.svg)](https://codecov.io/gh/sunriax/update) [![License: GPL v3](https://img.shields.io/badge/License-GPL%20v3-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)

# Update Tool

## Description:

With Update Tool updates from different sources (e.g. GitHub, WWW, FTP, ...) can be made. An example library how UpdateModels are created can be found [here](https://github.com/sunriax/update/tree/master/TemplateUpdateModelLib).

---

## Installation

To install Update Tool it is possible to download necessary libraries [[zip](https://github.com/sunriax/update/releases/latest/download/Update.zip) | [tar.gz](https://github.com/sunriax/update/releases/latest/download/Update.tar.gz)] or install the library via nuget.

```
PM> Install-Package RaGae.Update
```

After adding/installing the UpdateLib in a project it is necessary to place the user update models to a directory in your project.

[![Installed Models](https://raw.githubusercontent.com/sunriax/update/master/model.png)](https://github.com/sunriax/argument/tree/master/MakeUpdate)

To copy the user created models to output folder it is necessary to setup the *.csproj file.

*`*.csproj`*
``` xml
<Project Sdk="Microsoft.NET.Sdk">
  // ...

  <ItemGroup>
    <LibraryFiles Include="$(ProjectDir)Model\*" />
  </ItemGroup>

  <Target Name="PostBuild" AfterTargets="PostBuildEvent">
    <Copy SourceFiles="@(LibraryFiles)" DestinationFolder="$(TargetDir)Model" SkipUnchangedFiles="true" />
  </Target>
  
  // ...
</Project>
```

**Configuration with path or file after Installation**

*`UpdateLib.Path.json`*

``` yaml
{
  "ReflectionConfig": [
    {
      "ReflectionPath": "Model",
      "FileSpecifier": "*UpdateModelLib.dll"
    }
  ]
}
```

*`UpdateLib.File.json`*

``` yaml
{
  "ReflectionConfig": [
    {
      "Files": [
        "RaGae.UpdateLib.TemplateUpdateModelLib.dll",
        "RaGae.UpdateLib.??UpdateModelLib.dll",
        "..."
      ]
    }
  ]
}
```

An example project howto use the Update Tool can be found within this repository in the [MakeUpdate](https://github.com/sunriax/update/tree/master/MakeUpdate) project.

---

## Structure

### Initialisation

``` csharp
Update update = new Update("Arguments from command line");
```

*`UpdateLib.json`*

``` yaml
{
  "ReflectionConfig": [
    {
        "ReflectionPath": "Model",
        "FileSpecifier": "*UpdateModelLib.dll"
    }
  ]
}
```

*`UserGeneratedUpdateModelLib.json`*

``` yaml
{
  "UpdateConfig": {
    "Model": "Template",
    "SkipBeforeUpdate": false,
    "SkipUpdate": false,
    "SkipAfterUpdate": false
  },
  "ReflectionConfig": [
    {
      "ReflectionPath": "Marshaler",
      "FileSpecifier": "*MarshalerLib.dll"
    }
  ],
  "ArgumentConfig": {
    "Schema": [
      {
        "Argument": [
          "p",
          "parameter"
        ],
        "Marshaler": "*",
        "Required": true
      }
    ],
    "Delimiter": "-:/"
  }
}
```

### Arguments from CLI

The first argument contains the filename for configuration of UpdateLib and is not bypassed to the UpdateModel libraries. The subsequent arguments are passed to the update model.

``` bash
UserUpdate.exe UpdateLib.json UserGeneratedUpdateModelLib.json -p "Test"
# or
UserUpdate.exe UpdateLib.json UserGeneratedUpdateModelLib.json -parameter "Test"
```

``` csharp
string[] args = {
    "UpdateLib.json",
    "UserGeneratedUpdateModelLib.json"
    "-p",
    "Test"
};
```

``` csharp
Update update = new Update(args);
update.UpdateMessage += Console.WriteLine;  // Get messages from UpdateModel
update.ExecuteUpdate();
```

---

## Build your own Model

1. Create a new VisualStudio .NET Standard Classlibrary (**??UpdateModelLib**)
1. Link a new project reference to [RaGae.UpdateLib.UpdateModelLib.dll](https://github.com/sunriax/update/releases/latest/download/RaGae.UpdateLib.UpdateModelLib.dll) (in this repository) or install as nuget (see below)
1. Write Model (See example code below)
1. Copy the TestUpdateModelLib.dll to the Model directory in your executable project

```
PM> Install-Package RaGae.Update.Model
```

### Model with no arguments

*`UpdateLib.json`*

``` yaml
{
  "ReflectionConfig": [
    {
      "ReflectionPath": "Model",
      "FileSpecifier": "*UpdateModelLib.dll"
    }
  ]
}
```

*`TestUpdateModelLib.NoArguments.json`*

``` yaml
{
  "UpdateConfig": {
    "Model": "Test",
    "SkipBeforeUpdate": false,
    "SkipUpdate": false,
    "SkipAfterUpdate": false
  }
}
```

``` csharp
using System;
using RaGae.UpdateLib.UpdateModelLib;

namespace RaGae.UpdateLib.TestUpdateModelLib
{
    public class TestUpdateModel : UpdateModel
    {
        public override event WriteMessage UpdateMessage;

        private const string model = "Test";
        public override string Model { get => model.ToLower(); }

        // Necessary for reflector library, otherwise the class can not be found!
        // If the constructor is not used it can be private
        // If no arguments are passed to the model use this constructor
        private TestUpdateModel()
        {

        }

        // If no arguments are passed to the model the constructor can be removed!
        // public TestUpdateModel(IEnumerable<string> args) : base(args)
        // {
        //     // Initiate config
        //     this.config = new TestUpdateConfig()
        //     {
        //       // ...
        //     };
            
        // }

        public override void BeforeUpdate()
        {
            this.UpdateMessage?.Invoke("Things to do before update");
        }

        public override void Update()
        {
            this.UpdateMessage?.Invoke("Things to update");
        }

        public override void AfterUpdate()
        {
            this.UpdateMessage?.Invoke("Things to do after update");
        }
    }
}
```

**Application**

``` bash
UserUpdate.exe UpdateLib.json TestUpdateModelLib.NoArguments.json
```

``` csharp
string[] args = {
    "UpdateLib.json",
    "TestUpdateModelLib.NoArguments.json"
};
```

``` csharp
Update update = new Update(args);
update.UpdateMessage += Console.WriteLine;  // Get messages from UpdateModel
update.ExecuteUpdate();
```

### Model with arguments

> **Important**: If arguments are used it is necessary to install required marshalers in the application folder. A readme howto do this can be found [here](https://github.com/sunriax/argument).

*`UpdateLib.json`*

``` yaml
{
  "ReflectionConfig": [
    {
      "ReflectionPath": "Model",
      "FileSpecifier": "*UpdateModelLib.dll"
    }
  ]
}
```

*`TestUpdateModelLib.NoArguments.json`*

``` yaml
{
  "UpdateConfig": {
    "Model": "Test",
    "SkipBeforeUpdate": false,
    "SkipUpdate": false,
    "SkipAfterUpdate": false
  },
  "ReflectionConfig": [
    {
      "ReflectionPath": "Marshaler",
      "FileSpecifier": "*MarshalerLib.dll"
    }
  ],
  "ArgumentConfig": {
    "Schema": [
      {
        "Argument": [
          "p",
          "parameter"
        ],
        "Marshaler": "*",
        "Required": true
      }
    ],
    "Delimiter": "-:/"
  }
}
```

``` csharp
using System;
using RaGae.UpdateLib.UpdateModelLib;

namespace RaGae.UpdateLib.TestUpdateModelLib
{
    public class TestUpdateModel : UpdateModel
    {
        public override event WriteMessage UpdateMessage;

        private const string model = "Test";
        public override string Model { get => model.ToLower(); }

        private readonly TestUpdateConfig config;

        // Necessary for reflector library, otherwise the class can not be found!
        // If the constructor is not used it can be private
        // If no arguments are passed to the model use this constructor
        private TestUpdateModel()
        {

        }

        // If no arguments are passed to the model the constructor can be removed!
        public TestUpdateModel(IEnumerable<string> args) : base(args)
        {
            // Initiate config
            this.config = new TestUpdateConfig()
            {
              Parameter = base.argument.GetValue<string>("p")
              // or
              // Parameter = base.argument.GetValue<string>("parameter")
            };
            
        }

        public override void BeforeUpdate()
        {
            this.UpdateMessage?.Invoke("Things to do before update");
        }

        public override void Update()
        {
            this.UpdateMessage?.Invoke("Things to update");
            this.UpdateMessage?.Invoke($"Parameter {this.config.Parameter}");
        }

        public override void AfterUpdate()
        {
            this.UpdateMessage?.Invoke("Things to do after update");
        }

        internal class TestUpdateConfig
        {
          public string Parameter { get; set; }
        }
    }
}
```

**Application**

``` bash
UserUpdate.exe UpdateLib.json UserGeneratedUpdateModelLib.json -p "Test"
# or
UserUpdate.exe UpdateLib.json UserGeneratedUpdateModelLib.json -parameter "Test"
```

``` csharp
string[] args = {
    "UpdateLib.json",
    "TestUpdateModelLib.NoArguments.json"
    "-p",
    // or
    // "-parameter",
    "Test"
};
```

``` csharp
Update update = new Update(args);
update.UpdateMessage += Console.WriteLine;  // Get messages from UpdateModel
update.ExecuteUpdate();
```

---

R. Gächter

# _Employers Front Door_

<img src="https://avatars.githubusercontent.com/u/9841374?s=200&v=4" align="right" alt="UK Government logo">

![Build Status](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_apis/build/status/das-find-employment-schemes)
[![Trello Project](https://img.shields.io/badge/Trello-%23026AA7.svg?style=for-the-badge&logo=Trello&logoColor=white)](https://trello.com/b/7rko6qQ2/cx-employers-beta)
[![Confluence Project](https://img.shields.io/badge/Confluence-Project-blue)](https://skillsfundingagency.atlassian.net/wiki/spaces/NDL/pages/3507748939/CX+Employers)
[![License](https://img.shields.io/badge/license-MIT-lightgrey.svg?longCache=true&style=flat-square)](https://en.wikipedia.org/wiki/MIT_License)

Provides a portal for employers to see all training schemes available to them across government.

## How It Works

![Architecture](docs/Find%20Employer%20Schemes%20Architecture.drawio.png)

## 🚀 Installation

### Pre-Requisites

```
* A clone of this repository
* An Azure storage account (or emulator)
* A Contentful space (optional)
```

### Config

This utility uses the standard Apprenticeship Service configuration. All configuration can be found in the [das-employer-config repository](https://github.com/SkillsFundingAgency/das-employer-config/blob/master/das-find-employment-schemes/SFA.DAS.FindEmploymentSchemes.Web.json).

```
AppSettings.Development.json file
```json
{
    "Logging": {
      "LogLevel": {
        "Default": "Information",
        "Microsoft": "Warning",
        "Microsoft.Hosting.Lifetime": "Information"
      }
    },
    "ConfigurationStorageConnectionString": "UseDevelopmentStorage=true;",
    "ConfigNames": "SFA.DAS.Tools.Servicebus.Support",
    "EnvironmentName": "LOCAL",
    "Version": "1.0",
    "APPINSIGHTS_INSTRUMENTATIONKEY": ""
  }  
```

Azure Table Storage config

Row Key: SFA.DAS.FindEmploymentSchemes.Web_1.0

Partition Key: LOCAL

Data:

```json
{
  "ConnectionStrings": {
      "Redis": "localhost:6379"
  },
  "Endpoints": {
    "BaseURL": "https://localhost:44318/"
  }
  "ContentUpdates": {
      "Enabled": true,
      "CronSchedule": "*/30 * * * *"
  },
  "ContentfulOptions": {
    "DeliveryApiKey": "<Ask for key>",
    "ManagementApiKey": "",
    "PreviewApiKey": "<Ask for key>",
    "SpaceId": "082i50qdtar9",
    "UsePreviewApi": false,
    "MaxNumberOfRateLimitRetries": 0,
    "Environment", "at"
  }
}
```

## 🔗 External Dependencies

This project _can_ auto update content from a correctly configured Contentful space. An export of the required space is in the Contenful folder.

Alternatively, you can set ContentUpdates:Enabled to false, to run without requiring a Contentful space set up.

## Technologies

The key technologies used in the project are:

```
* .NetCore 3.1
* Contentful
* xUnit
* FakeItEasy
```

## User Guide

[User guide](docs/userguide.md)

## 🐛 Known Issues

N/A

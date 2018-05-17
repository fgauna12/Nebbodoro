# Nebbodoro Sample App (ASP.NET Core, Azure App Service, Event Grid) :tomato:

<a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Ffgauna12%2FNebbodoro%2Fmaster%2Fsrc%2FNebbodoro.ARM%2Fazuredeploy.json" target="_blank">
    <img src="http://azuredeploy.net/deploybutton.png"/>
</a>

Nebbodoro is a sample app based on the [Pomodoro Technique](https://en.wikipedia.org/wiki/Pomodoro_Technique).
It aims to show-case .NET Core development on Azure using Azure technologies. 

This app it's meant to be simple enough to be used during a workshop. It has minimal dependencies yet it's rich enough to provide a demo.
The front-end can be downloaded as a Angular Docker image so that developers can just focus on the ASP.NET Core API.

## Requirements

- Visual Studio 2017 1.5 or higher with these workloads:
  - Azure development
  - ASP.NET and web development
  - Node.js development
  - .NET Core cross-platform development

## Infrastructure as Code (ARM Template)

> Friend's don't let friend create Azure resources from the portal

We created an ARM template that lives along side the source code in the same Visual Studio solution.
Once the ARM template is deployed, you still have to deploy the API App Service.

**Note:** Today, this ARM template does not include the front-end. You can deploy this seperately using Azure Container Instance. _(See Below)_

<p align="center">
  <img alt="Azure Architecture" src="/assets/azure_architecture.png?raw=true">
</p>

## ASP.NET Core API  

An unauthenticated API using:
- ASP.NET Core 2.0 and .NET Core 2.0.
- ORM is Entity Framework Core 2.0.
- Application Insights for Telemetry
- Event Grid for events to be consumed by serverless components

### Publishing to Azure

We recommend that you avoid _right-click deploy_ from within Visual Studio. Instead, use CI/CD to publish from your own Git Repo.
We like to use [Visual Studio Team Services](https://www.visualstudio.com/team-services/)

## Angular Front-End (Optional)

This repo also contains an Angular front-end hosted with a Docker container and served with NodeJS. 
It uses the back-end API hosted anywhere and the API can be specified at _runtime_ when running the Docker container.

<p align="center">
  <img alt="Nebbodoro in action" src="/assets/nebbodoro_example.png?raw=true">
</p>

### Run from a Docker container

`docker run -p 4100:80 -e API_URL='https://some-api.azurewebsites.net/api/' fgauna12/nebbodoro.spa`

### Run from Azure Container Instance

`az container create --resource-group myResourceGroup -e API_URL=https://some-api.azurewebsites.net/api/ -n myapp --image fgauna12/nebbodoro.spa --dns-name-label myapp --ports 80`

## Extending with Azure Functions or Azure Logic Apps

Fork this repo and create your own Azure Function or Logic app that sends you an email or sends a text message **when a pomodoro is done.** :tada:

Your serverless component can be triggered from an [Event Grid](https://azure.microsoft.com/en-us/services/event-grid/) topic that the API raises:

```
Event Type: Nebbodoro.OnPomodoroDone
Subject: OnPomodoroDone
```

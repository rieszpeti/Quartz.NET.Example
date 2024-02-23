# Quartz.NET Example App

This example app demonstrates the usage of JobFactory, JobListener, EF Core in Quartz.NET.

## Overview

Quartz.NET is a feature-rich job scheduling library that can be integrated into your .NET applications. This example showcases how to use Quartz.NET to schedule and manage jobs, along with utilizing JobFactory and JobListener for custom job execution and monitoring.

Sometimes, I do not follow best practices such as storing the connection string in the `appsettings.json` or in environment variables to keep everything simple. This is not a design pattern course, so that is not the main focus.

## Features

- **Job Scheduling**: Demonstrates how to schedule jobs using Quartz.NET.
- **JobFactory**: Implements a custom JobFactory to instantiate job instances with dependencies injected.
- **JobListener**: Implements a custom JobListener to monitor job executions and scheduler linked jobs.

## Setup

Ensure that localdb is available in Visual Studio, but you can also use external databases like MSSQL in Docker, etc.

If you are using another provider, change the connection string in the `Startup` where the `ConnectionString` property is located.

Next, apply the `Update-Database` command in the `Tools -> NuGet Package Manager -> Package Manager Console`.

After this, you can run the program.

Initially, you will see the 'I am dummy.' message at the bottom of the console.

There are 3 + 1 types of jobs to demonstrate various mechanisms:

1. Dummy job is the basic one, it doesn't have any constructor.
2. The next one is SomeJob where I injected SomeService to show basic DI.
3. The most advanced one demonstrates the scenario when you need some data before initialization.
4. The last one is ErrorJob. If an initialization fails, most of the time Quartz can't handle it, so you have to add this error job or do nothing.

There are two part that is crucial one is the JobFactory that initialize the jobs that extends the IJob interface.

The other one is the JobListener where I start an other job, if that is comes from the parameters.

You can use JobDataMap most of this features, but sometimes that is not enough, because it is working like a Dictionary so that is why I wrote this example program.

If you notice any mistakes or have any suggestions, please feel free to share them with me.

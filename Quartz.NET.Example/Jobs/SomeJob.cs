using Quartz.NET.Example.JobHelpers;
using Quartz.NET.Example.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quartz.NET.Example.Jobs;

internal class SomeJob : IJob
{
    private readonly SomeService _someService;
    private readonly NextJobCreator? _nextJobCreator;

    public SomeJob(SomeService someService)
    {
        _someService = someService;

        Console.WriteLine("Doing something before calling the execute");
    }

    public SomeJob(SomeService someService, NextJobCreator nextJobCreator)
    {
        _someService = someService;
        _nextJobCreator = nextJobCreator;

        Console.WriteLine("Doing something before calling the execute");
    }

    public Task Execute(IJobExecutionContext context)
    {
        _someService.DoSomething();

        if (true)
        {

        }

        return Task.CompletedTask;
    }
}

using Quartz.NET.Example.JobHelpers;
using Quartz.NET.Example.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quartz.NET.Example.Jobs
{
    internal class SomeJobWithNextJob(NextJobCreator nextJobCreator) : IJob
    {
        private readonly NextJobCreator _nextJobCreator = nextJobCreator;

        public Task Execute(IJobExecutionContext context)
        {

            Console.WriteLine("Add next job.");

            if (_nextJobCreator.CheckNextJob())
            {
                _nextJobCreator.AddNextJob(context);
            }

            return Task.CompletedTask;
        }
    }
}

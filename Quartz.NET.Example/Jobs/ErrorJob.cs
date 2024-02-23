using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quartz.NET.Example.Jobs
{
    /// <summary>
    /// If the jobCreation fails then this will be instanciated
    /// </summary>
    internal class ErrorJob : IJob
    {
        public ErrorJob(Exception ex)
        {
            Console.WriteLine(ex.Message);
            // You can log here or in the execute if the jobfactory couldn't make the job
        }

        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"Couldn't create job with name: {context.JobDetail.Key.Name}");

            return Task.CompletedTask;
        }
    }
}

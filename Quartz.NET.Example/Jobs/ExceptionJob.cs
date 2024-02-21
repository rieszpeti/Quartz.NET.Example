using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quartz.NET.Example.Jobs
{
    internal class ExceptionJob : IJob
    {
        public ExceptionJob(Exception ex)
        {
            Console.WriteLine(ex.Message);
            // You can log here or in the execute if the jobfactory couldn't make the job
        }

        public Task Execute(IJobExecutionContext context)
        {
            return Task.CompletedTask;
        }
    }
}

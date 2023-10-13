using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public class TimerTriggerCSharp1
    {
        [FunctionName("TimerTriggerCSharp1")]
        public void Run([TimerTrigger("0 */20 * * * *")]TimerInfo myTimer, ExecutionContext context, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

           
            string currentDirectory = Environment.CurrentDirectory;
            string telegrafPath = "/home/site/wwwroot/bin/telegraf";

            if (File.Exists(telegrafPath))
            {
                try
                {
                    
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = telegrafPath,
                        Arguments = "--once",
                        WorkingDirectory = context.FunctionAppDirectory
                    });
                    log.LogInformation("telegraf executed successfully.");
                }
                catch (Exception ex)
                {
                    log.LogError($"Error executing telegraf: {ex.Message}");
                }
            }
            else
            {
                log.LogError($"telegraf not found at the specified path: {telegrafPath}");
            }
        }
    }
}

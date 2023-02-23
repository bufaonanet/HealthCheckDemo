using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BlazorDemo.HealthChecks
{
    public class ResponseTimeHealthCheck : IHealthCheck
    {
        private Random rnd = new Random();

        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            int responseTime = rnd.Next(1, 300);

            if (responseTime < 100)
            {
                return Task.FromResult(HealthCheckResult.Healthy($"The response time looks good ({responseTime})ms"));
            }
            else if (responseTime < 200)
            {
                return Task.FromResult(HealthCheckResult.Degraded($"The response time is a bit slow ({responseTime})ms"));
            }
            else
            {
                return Task.FromResult(HealthCheckResult.Unhealthy($"The response time is unacceptable ({responseTime})ms"));
            }
        }
    }
}
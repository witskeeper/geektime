using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prometheus;
using Metric = Prometheus.Metrics;
namespace GeekTime.HealthChecksHost.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ExportersController : ControllerBase
    {
        [HttpGet]
        public async Task GetMetricsAsync()
        {
            var r = Metric.NewCustomRegistry();
            MetricFactory f = Metric.WithCustomRegistry(r);
            r.AddBeforeCollectCallback(() =>
            {
                f.CreateCounter("counter_v1", "").Inc(100);
            });
            Response.ContentType = PrometheusConstants.ExporterContentType;
            Response.StatusCode = 200;
            await r.CollectAndExportAsTextAsync(Response.Body, HttpContext.RequestAborted);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Hangfire;
using SSH.Core.Infrastructure;
using SSH.Service;

namespace SSH.API.Infrastructure
{
    public class HangfireIntegeration : IHangfireIntegeration
    {
        public Task MarkJobsExpired()
        {
            // RecurringJob.AddOrUpdate<JobsService>("MarkJobsExpired", (T) => T.MarkJobsExpired(), "*/2 * * * *");
            return Task.FromResult(0);
        }

        public Task UpdateAverageAndTotal()
        {
            return Task.FromResult(0);
        }

        public Task UpdateRating(int deliveryPartnerId)
        {
            return Task.FromResult(0);
        }

        public Task ForceEndOfDayBasedOnCountry(string timeZone, string country, int countryId, string timeDifference)
        {
            return Task.FromResult(0);
        }

        public Task CreateFutureJobBasedOnCountry(string timeZone, string country, int countryId, string timeDifference)
        {
            return Task.FromResult(0);
        }

        public Task NotifyForDocumentExpiryBasedOnCountry(int deliveryPartnerId, string timeZone, string country, int countryId, string timeDifference)
        {
            return Task.FromResult(0);
        }
    }
}
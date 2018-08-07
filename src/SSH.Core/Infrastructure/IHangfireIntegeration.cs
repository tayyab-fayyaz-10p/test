using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSH.Core.Infrastructure
{
    public interface IHangfireIntegeration
    {
        Task MarkJobsExpired();

        Task UpdateAverageAndTotal();

        Task UpdateRating(int deliveryPartnerId);

        Task ForceEndOfDayBasedOnCountry(string timeZone, string country, int countryId, string timeDifference);

        Task CreateFutureJobBasedOnCountry(string timeZone, string country, int countryId, string timeDifference);

        Task NotifyForDocumentExpiryBasedOnCountry(int deliveryPartnerId, string timeZone, string country, int countryId, string timeDifference);
    }
}

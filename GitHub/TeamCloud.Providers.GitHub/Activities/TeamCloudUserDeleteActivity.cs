/**
 *  Copyright (c) Microsoft Corporation.
 *  Licensed under the MIT License.
 */

using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using TeamCloud.Model.Data;
using TeamCloud.Orchestration;
using TeamCloud.Serialization;
using TeamCloud.Providers.GitHub.Services;

namespace TeamCloud.Providers.GitHub.Activities
{
    public class TeamCloudUserDeleteActivity
    {
        private readonly GitHubService github;

        public TeamCloudUserDeleteActivity(GitHubService github)
        {
            this.github = github ?? throw new ArgumentNullException(nameof(github));
        }

        [FunctionName(nameof(TeamCloudUserDeleteActivity)), RetryOptions(10, FirstRetryInterval = "00:02:00")]
        public async Task RunActivity(
            [ActivityTrigger] User user,
            ILogger log)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user));

            // using (log.BeginProjectScope(project))
            // {
            try
            {
                await github
                    .DeleteAdminUserAsync(user)
                    .ConfigureAwait(false);

                // log.LogInformation($"Created GitHub resources for project.");
            }
            catch (Exception exc)
            {
                log.LogError(exc, $"{nameof(TeamCloudUserDeleteActivity)} failed: {exc.Message}");

                throw exc.AsSerializable();
            }
            // }
        }
    }
}

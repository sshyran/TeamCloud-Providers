﻿using System;
using System.Security.Cryptography;
using System.Text;
using TeamCloud.Model.Data;

namespace TeamCloud.Providers.Core.Clients
{
    public static class ProjectLinkExtensions
    {
        public static ProjectLink WithGeneratedId(this ProjectLink projectLink, string source)
        {
            if (projectLink is null)
                throw new ArgumentNullException(nameof(projectLink));

            if (string.IsNullOrWhiteSpace(source))
                throw new ArgumentException($"'{nameof(source)}' cannot be null or whitespace", nameof(source));

            if (!Guid.TryParse(source, out Guid projectLinkId))
            {
                using var algorithm = HashAlgorithm.Create("MD5");

                var buffer = Encoding.ASCII.GetBytes(source);
                var hash = algorithm.ComputeHash(buffer);

                projectLinkId = new Guid(hash);
            }

            projectLink.Id = projectLinkId.ToString();

            return projectLink;
        }
    }
}

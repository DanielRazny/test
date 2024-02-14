namespace UpdateService.Services.VersionChecker
{
    public class VersionCompareHandler: IVersionCompareHandler
    {
        /// <summary>
        /// compares versions 
        /// </summary>
        /// <param name="version1"></param>
        /// <param name="version2"></param>
        /// <returns>
        /// if <paramref name="version1"/> is smaller than <paramref name="version2"/> return -1
        /// if <paramref name="version1"/> is bigger than <paramref name="version2"/> return 1
        /// if <paramref name="version1"/> equals <paramref name="version2"/> return 0
        /// </returns>
        public int CompareVersions(string? version1, string? version2)
        {
            if (string.IsNullOrWhiteSpace(version1) && string.IsNullOrWhiteSpace(version2))
            {
                return 0;
            }

            if (string.IsNullOrWhiteSpace(version1))
            {
                return -1;
            }

            if (string.IsNullOrWhiteSpace(version2))
            {
                return 1;
            }

            var split1 = version1.Split('.');
            var split2 = version2.Split('.');

            var min = Math.Min(split1.Length, split2.Length);

            for (var i = 0; i < min; i++)
            {
                if (!int.TryParse(split1[i], out var val1))
                {
                    return -1;
                }

                if (!int.TryParse(split2[i], out var val2))
                {
                    return 1;
                }

                if (val1 == val2)
                {
                    continue;
                }

                if (val1 > val2)
                {
                    return 1;
                } 
                
                return -1;
            }

            if (split1.Length == split2.Length)
            {
                return 0;
            }

            if (split1.Length > split2.Length)
            {
                return 1;
            }

            return -1;
        }
    }
}

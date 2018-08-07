namespace SSH.Core.IService
{
    public interface IConfigurationService
    {
        string DeploymentURL { get; }

        string OldWebConnectionString { get; }
        
        string DataMigrationConnectionString { get; }

        string LdapDomain { get; }

        string LdapPort { get; }

        string LdapUserName { get; }

        string LdapPassword { get; }

        string LdapConnectionString { get; }

        string LdapRoles { get; }

        string LdapSearchType { get; }

        string MaxPasswordRetryCount { get; }

        string GoogleMapUrl { get; }

        string AnonymousApiToken { get; }

        string PortForMobile { get; }

        string PortForMobileFilePath { get; }

        string DefaultRadius { get; }

        string SearchDriverRadius { get; }

        string SecondAttemptDefaultRadius { get; }

        double SearchSubstationRadius { get; }

        int JobExpiryMinutes { get; }

        int CacheControlTime { get; }

        int RefreshTokenExpiryTimeInSeconds { get; }

        string SENDGRID_API_KEY { get; }

        string FromEmail { get; }

        string SENDGRID_API { get; }

        string PushNotificationServerId { get; }

        string SMSPassword { get; }

        string SMSUserName { get; }

        string SMSSender { get; }

        int OTPExpiry { get; }

        string RedisConnection { get; }

        string GoogleLocationApi { get; }

        string GoogleLocationApiKey { get; }

        string AzureBlobStorageName { get; }

        string AzureBlobStorageKey { get; }

        string AzureBlobStorageUrl { get; }

        string AzureBlobStorageContainer { get; }
    }
}

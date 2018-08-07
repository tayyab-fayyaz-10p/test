using System;
using System.Configuration;
using SSH.Core.IService;

namespace SSH.Service
{
    public class ConfigurationService : IConfigurationService
    {
        private static string deploymentURL = string.IsNullOrEmpty(ConfigurationManager.AppSettings["DeploymentURL"]) ? string.Empty : ConfigurationManager.AppSettings["DeploymentURL"].ToString();
        private static string oldWebConnectionString = ConfigurationManager.ConnectionStrings["OldWebConnectionString"] == null ? string.Empty : ConfigurationManager.ConnectionStrings["OldWebConnectionString"].ConnectionString.ToString();
        private static string dataMigrationConnectionString = ConfigurationManager.ConnectionStrings["dataMigrationConnectionString"] == null ? string.Empty : ConfigurationManager.ConnectionStrings["dataMigrationConnectionString"].ConnectionString.ToString();
        private static string ldapDomain = string.IsNullOrEmpty(ConfigurationManager.AppSettings["LdapDomain"]) ? string.Empty : ConfigurationManager.AppSettings["LdapDomain"].ToString();
        private static string ldapPort = string.IsNullOrEmpty(ConfigurationManager.AppSettings["LdapPort"]) ? string.Empty : ConfigurationManager.AppSettings["LdapPort"].ToString();
        private static string ldapUserName = string.IsNullOrEmpty(ConfigurationManager.AppSettings["LdapUserName"]) ? string.Empty : ConfigurationManager.AppSettings["LdapUserName"].ToString();
        private static string ldapPassword = string.IsNullOrEmpty(ConfigurationManager.AppSettings["LdapPassword"]) ? string.Empty : ConfigurationManager.AppSettings["LdapPassword"].ToString();
        private static string ldapConnectionString = string.IsNullOrEmpty(ConfigurationManager.AppSettings["LdapConnectionString"]) ? string.Empty : ConfigurationManager.AppSettings["LdapConnectionString"].ToString();
        private static string ldapRoles = string.IsNullOrEmpty(ConfigurationManager.AppSettings["LdapRole"]) ? string.Empty : ConfigurationManager.AppSettings["LdapRole"].ToString();
        private static string ldapSearchType = string.IsNullOrEmpty(ConfigurationManager.AppSettings["LdapSearchType"]) ? string.Empty : ConfigurationManager.AppSettings["LdapSearchType"].ToString();
        private static string maxPasswordRetryCount = string.IsNullOrEmpty(ConfigurationManager.AppSettings["MaxPasswordRetryCount"]) ? string.Empty : ConfigurationManager.AppSettings["MaxPasswordRetryCount"].ToString();
        private static string googlemapUrl = string.IsNullOrEmpty(ConfigurationManager.AppSettings["GoogleMapUrl"]) ? string.Empty : ConfigurationManager.AppSettings["GoogleMapUrl"].ToString();
        private static string anonymousApiToken = string.IsNullOrEmpty(ConfigurationManager.AppSettings["AnonymousApiToken"]) ? string.Empty : ConfigurationManager.AppSettings["AnonymousApiToken"].ToString();
        private static string portForMobile = string.IsNullOrEmpty(ConfigurationManager.AppSettings["PortForMobile"]) ? string.Empty : ConfigurationManager.AppSettings["PortForMobile"].ToString();
        private static string portForMobileFilePath = string.IsNullOrEmpty(ConfigurationManager.AppSettings["PortForMobileFilePath"]) ? string.Empty : ConfigurationManager.AppSettings["PortForMobileFilePath"].ToString();
        private static string defaultRadius = string.IsNullOrEmpty(ConfigurationManager.AppSettings["DefaultRadius"]) ? string.Empty : ConfigurationManager.AppSettings["DefaultRadius"].ToString();
        private static string searchDriverRadius = string.IsNullOrEmpty(ConfigurationManager.AppSettings["SearchDriverRadius"]) ? string.Empty : ConfigurationManager.AppSettings["SearchDriverRadius"].ToString();
        private static string secondAttemptDefaultRadius = string.IsNullOrEmpty(ConfigurationManager.AppSettings["SecondAttemptDefaultRadius"]) ? string.Empty : ConfigurationManager.AppSettings["SecondAttemptDefaultRadius"].ToString();
        private static int jobExpiryMinutes = string.IsNullOrEmpty(ConfigurationManager.AppSettings["JobExpiryMinutes"]) ? 0 : Convert.ToInt32(ConfigurationManager.AppSettings["JobExpiryMinutes"]);
        private static double searchSubstationRadius = string.IsNullOrEmpty(ConfigurationManager.AppSettings["SearchSubstationRadius"]) ? 0 : Convert.ToDouble(ConfigurationManager.AppSettings["SearchSubstationRadius"]);
        private static int cacheControlTime = string.IsNullOrEmpty(ConfigurationManager.AppSettings["CacheControlTime"]) ? 0 : Convert.ToInt32(ConfigurationManager.AppSettings["CacheControlTime"]);
        private static int refreshTokenExpiryTimeInSeconds = string.IsNullOrEmpty(ConfigurationManager.AppSettings["RefreshTokenExpiryTimeInSeconds"]) ? 0 : Convert.ToInt32(ConfigurationManager.AppSettings["RefreshTokenExpiryTimeInSeconds"]);
        private static string sendGridApiKey = string.IsNullOrEmpty(ConfigurationManager.AppSettings["SENDGRID_API_KEY"]) ? string.Empty : ConfigurationManager.AppSettings["SENDGRID_API_KEY"].ToString();
        private static string fromEmail = string.IsNullOrEmpty(ConfigurationManager.AppSettings["FromEmail"]) ? string.Empty : ConfigurationManager.AppSettings["FromEmail"].ToString();
        private static string sendGridApi = string.IsNullOrEmpty(ConfigurationManager.AppSettings["SENDGRID_API"]) ? string.Empty : ConfigurationManager.AppSettings["SENDGRID_API"].ToString();
        private static string pushNotificationServerId = string.IsNullOrEmpty(ConfigurationManager.AppSettings["PushNotificationServerId"]) ? string.Empty : ConfigurationManager.AppSettings["PushNotificationServerId"].ToString();
        private static string smsUserName = string.IsNullOrEmpty(ConfigurationManager.AppSettings["SMSUserName"]) ? string.Empty : ConfigurationManager.AppSettings["SMSUserName"].ToString();
        private static string smsPassword = string.IsNullOrEmpty(ConfigurationManager.AppSettings["SMSPassword"]) ? string.Empty : ConfigurationManager.AppSettings["SMSPassword"].ToString();
        private static string smsSender = string.IsNullOrEmpty(ConfigurationManager.AppSettings["SMSSender"]) ? string.Empty : ConfigurationManager.AppSettings["SMSSender"].ToString();
        private static int otpExpiry = string.IsNullOrEmpty(ConfigurationManager.AppSettings["OTPExpiry"]) ? 0 : Convert.ToInt32(ConfigurationManager.AppSettings["OTPExpiry"]);
        private static string redisConnection = string.IsNullOrEmpty(ConfigurationManager.AppSettings["RedisConnection"]) ? string.Empty : ConfigurationManager.AppSettings["RedisConnection"].ToString();
        private static string googleLocationApi = string.IsNullOrEmpty(ConfigurationManager.AppSettings["GoogleLocationApi"]) ? string.Empty : ConfigurationManager.AppSettings["GoogleLocationApi"].ToString();
        private static string googleLocationApiKey = string.IsNullOrEmpty(ConfigurationManager.AppSettings["GoogleLocationApiKey"]) ? string.Empty : ConfigurationManager.AppSettings["GoogleLocationApiKey"].ToString();
        private static string azureBlobStorageName = string.IsNullOrEmpty(ConfigurationManager.AppSettings["AzureBlobStorageName"]) ? string.Empty : ConfigurationManager.AppSettings["AzureBlobStorageName"].ToString();
        private static string azureBlobStorageKey = string.IsNullOrEmpty(ConfigurationManager.AppSettings["AzureBlobStorageKey"]) ? string.Empty : ConfigurationManager.AppSettings["AzureBlobStorageKey"].ToString();
        private static string azureBlobStorageUrl = string.IsNullOrEmpty(ConfigurationManager.AppSettings["AzureBlobStorageUrl"]) ? string.Empty : ConfigurationManager.AppSettings["AzureBlobStorageUrl"].ToString();
        private static string azureBlobStorageContainer = string.IsNullOrEmpty(ConfigurationManager.AppSettings["AzureBlobStorageContainer"]) ? string.Empty : ConfigurationManager.AppSettings["AzureBlobStorageContainer"].ToString();

        public string AzureBlobStorageName
        {
            get
            {
                return azureBlobStorageName;
            }
        }

        public string AzureBlobStorageKey
        {
            get
            {
                return azureBlobStorageKey;
            }
        }

        public string AzureBlobStorageUrl
        {
            get
            {
                return azureBlobStorageUrl;
            }
        }

        public string AzureBlobStorageContainer
        {
            get
            {
                return azureBlobStorageContainer;
            }
        }

        public string GoogleLocationApi
        {
            get
            {
                return googleLocationApi;
            }
        }

        public string GoogleLocationApiKey
        {
            get
            {
                return googleLocationApiKey;
            }
        }

        public string RedisConnection
        {
            get
            {
                return redisConnection;
            }
        }

        public int OTPExpiry
        {
            get
            {
                return otpExpiry;
            }
        }

        public string AnonymousApiToken
        {
            get
            {
                return anonymousApiToken;
            }
        }

        public string FromEmail
        {
            get
            {
                return fromEmail;
            }
        }

        public string PortForMobile
        {
            get
            {
                return portForMobile;
            }
        }

        public int JobExpiryMinutes
        {
            get
            {
                return jobExpiryMinutes;
            }
        }

        public string PortForMobileFilePath
        {
            get
            {
                return portForMobileFilePath;
            }
        }

        public string DefaultRadius
        {
            get
            {
                return defaultRadius;
            }
        }

        public string SearchDriverRadius
        {
            get
            {
                return searchDriverRadius;
            }
        }

        public string SecondAttemptDefaultRadius
        {
            get
            {
                return secondAttemptDefaultRadius;
            }
        }

        public double SearchSubstationRadius
        {
            get
            {
                return searchSubstationRadius;
            }
        }

        public string DeploymentURL
        {
            get { return deploymentURL; }
        }

        public string OldWebConnectionString
        {
            get
            {
                return oldWebConnectionString;
            }
        }
        
        public string DataMigrationConnectionString
        {
            get
            {
                return dataMigrationConnectionString;
            }
        }

        public string LdapDomain 
        {
            get 
            {
                return ldapDomain;
            }
        }

        public string LdapPort 
        {
            get 
            {
                return ldapPort;
            }
        }

        public string LdapUserName
        {
            get
            {
                return ldapUserName;
            }
        }

        public string LdapPassword
        {
            get
            {
                return ldapPassword;
            }
        }

        public string LdapConnectionString
        {
            get
            {
                return ldapConnectionString;
            }
        }

        public string LdapRoles
        {
            get
            {
                return ldapRoles;
            }
        }

        public string LdapSearchType
        {
            get
            {
                return ldapSearchType;
            }
        }

        public string MaxPasswordRetryCount
        {
            get 
            {
                return maxPasswordRetryCount;
            }
        }

        public string GoogleMapUrl
        {
            get
            {
                return googlemapUrl;
            }
        }

        public int CacheControlTime
        {
            get
            {
                return cacheControlTime;
            }
        }

        public int RefreshTokenExpiryTimeInSeconds
        {
            get
            {
                return refreshTokenExpiryTimeInSeconds;
            }
        }

        public string SENDGRID_API_KEY
        {
            get
            {
                return sendGridApiKey;
            }
        }

        public string SENDGRID_API
        {
            get
            {
                return sendGridApi;
            }
        }

        public string PushNotificationServerId
        {
            get
            {
                return pushNotificationServerId;
            }
        }

        public string SMSPassword
        {
            get
            {
                return smsPassword;
            }
        }

        public string SMSUserName
        {
            get
            {
                return smsUserName;
            }
        }

        public string SMSSender
        {
            get
            {
                return smsSender;
            }
        }
    }
}

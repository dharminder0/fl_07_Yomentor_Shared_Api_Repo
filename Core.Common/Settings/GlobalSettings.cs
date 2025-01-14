using Core.Common.Configuration;
using System.Collections.Specialized;

namespace Hub.Common.Settings
{
    public class GlobalSettings {
        private static NameValueCollection _appSettings = ConfigurationManager.AppSettings;

        public static string JobRockAccountsApiUrl => GetString("JobRock.Accounts.ApiUrl");
        public static string JobRockAccountsApiAuthorization => GetString("JobRock.Accounts.AuthorizationBearer");

        public static string SymmetricSecretKey => GetString("SymmetricSecretKey");
        public static bool ValueTweakerEnabled => GetBool("ValueTweakerEnabled");
        public static bool ApiLoggingFilterEnabled => GetBool("ApiLoggingFilterEnabled");

        public static string JobRockAppointmentApiUrl => GetString("JobRock.Appointment.ApiUrl");
        public static string JobRockAppointmentApiSecret => GetString("JobRock.Appointment.ApiSecretKey");

        public static string JobRockAutomationApiUrl => GetString("JobRock.Automation.ApiUrl");
        public static string JobRockAutomationApiSecret => GetString("JobRock.Automation.ApiSecretKey");
        public static string JobRockAutomationDomain => GetString("JobRock.Automation.Domain");

        public static string JobRockCampaignApiUrl => GetString("JobRock.Campaign.ApiUrl");
        public static string JobRockCampaignApiAuthorization => GetString("JobRock.Campaign.AuthorizationBearer");

        public static string JobRockLeadApiUrl => GetString("JobRock.Lead.ApiUrl");
        public static string JobRockHubUrl => GetString("JobRock.Hub.Url");
        public static string JobRockLeadApiAuthorization => GetString("JobRock.Lead.AuthorizationBearer");
        public static string CoreApiUrl => GetString("JobRock.Core.ApiUrl");
        public static string CoreApiAuthorization => GetString("JobRock.Core.AuthorizationBearer");
        public static string JobRockCanvasApiUrl => GetString("JobRock.Canvas.ApiUrl");
        public static string JobRockCanvasApiSecret => GetString("JobRock.Canvas.ApiSecretKey");
        public static string JobRockCommunicationApiUrl => GetString("JobRock.Communication.ApiUrl");
        public static string JobRockCommunicationApiAuthorization => GetString("JobRock.Communication.AuthorizationBearer");
        public static string DefaultDomainUrl => GetString("DefaultDomainUrl");
        public static string BlobStorageAccount => GetString("BlobStorageAccount");
        public static string BlobContainerName => GetString("BlobContainerName");
        public static bool PushAppointmentActionInQueue => GetBool("PushAppointmentActionInQueue");

        public static string FacebookPixelEventClients => GetString("FacebookPixelEventClients");
        public static string url => GetString("FacebookGraphUrl");
        public static string FacebookTestEventCode => GetString("FacebookTestEventCode");

        public static string VoltasAccApiUrl => GetString("JobRock.Voltas.Url");
        public static string VoltasAccApiAuthorization => GetString("JobRock.Voltas.Authorization");
        public static string VoltaSubmitStatus => GetString("JobRock.Volta.SubmitStatus");
        public static string VoltaExcludedoffices => GetString("JobRock.Volta.Excludedoffices");

        public static string FaradayAccApiUrl => GetString("JobRock.Faraday.Url");
        public static string FaradayAccApiAuthorization => GetString("JobRock.Faraday.Authorization");
        public static string FaradaySubmitStatus => GetString("JobRock.faraday.SubmitStatus");

        public static string RefApiUrl => GetString("JobRock.RefApi.url");
        public static string RefApiAuthorization => GetString("JobRock.RefApi.Authorization");
        public static string ExternalQueueActions => GetString("JobRock.External.ExternalQueueActions");
        public static string AuthorizationBearers => GetString("AuthorizationBearer");



        public static bool DisabledMySolutionSync => GetBool("DisabledMySolutionSync");
        public static string CalenderLink => GetString("JobRock.Appointment.Url");
        public static string AppointmentToken => GetString("JobRock.Appointment.Token");
        public static string MediaLink => GetString("Jobrock.Media.Url");
        public static string MediaAuthorization => GetString("Jobrock.Media.Authorization");
        public static string ConsentDefaultDomain => GetString("JobRock.Consent.Url");
        public static int DeleteConsentMinDays => GetInt("DiscardContactDayLimit");
        public static string DmpApiUrl => GetString("JobRock.Dmp.ApiUrl");
        public static string DmpApiAuthorization => GetString("JobRock.Dmp.AuthorizationBearer");
        public static string PrerenderUrl => GetString("JobRock.Pre-render.Url");
        public static string PrerenderAuthorization => GetString("JobRock.Pre-render.AuthorizationBearer");
        public static string PrerenderToken => GetString("JobRock.Pre-render.Token");
        public static bool EnableCommonErrorLog => GetBool("EnableCommonErrorLog");
        public static string JobRockAutomationUrl => GetString("JobRock.Automation.Url");
        public static bool IsEnableAutomationLog => GetBool("IsEnableAutomationLog");
        public static bool ElmahLogEnabled => GetBool("ElmahLogEnabled");
        public static bool ElmahInfoLogEnabled => GetBool("ElmahInfoLogEnabled");
        public static bool _isCacheEnabled => GetBool("AppLocalCacheEnabled");
        public static int _defaultCacheHours => GetInt("DefaultAppLocalCacheHours");
        public static string DefaultConnectionName => GetString("DefaultConnectionName");
        public static string LogConnectionName => GetString("LogConnectionName");
        public static string FileLogDirectory => GetString("FileLogDirectory");
        public static string TempLeadId => GetString("Temp.LeadId");
        public static string EnableWhatsApptempRedirection => GetString("EnableWhatsApptempRedirection");
        public static string ElmahBasicUserName => GetString("ElmahBasicAuth.UserName");
        public static string ElmahBasicUserPassword => GetString("ElmahBasicAuth.Password");
        public static string ElmahMvcRoute => GetString("elmah.mvc.route");
        public static bool AuthorizationEnabled => GetBool("AuthorizationEnabled");
        public static string AuthorizationBearer => GetString("AuthorizationBearer");
        public static string AllowedOrigins => GetString("AllowedOrigins");
        public static IEnumerable<string> LogLevels => GetListValues("LogLevels");
        public static IEnumerable<string> EnableLogObjects => GetListValues("EnableLogObjects");
        public static int DisabledBySettingInterval => GetInt("DisabledBySettingInterval");

        public static int DeleteLogInterval => GetInt("DeleteLogInterval");
        public static int DeleteLogIntervalWhatsapp => GetInt("DeleteLogIntervalWhatsapp");
        public static int DeleteApiUsageLogs => GetInt("DeleteApiUsageLogs");
        public static int DeleteOldClientFieldsLog => GetInt("DeleteOldClientFieldsLog");
        public static int DeleteOldMasterObjectMappingLog => GetInt("DeleteMasterObjectMappingLog");
        public static int DeleteOldUnknownContactQueue => GetInt("DeleteOldUnknownContactQueue");
        public static bool AzureFuncContactActionProcessEnabled => GetBool("AzureFunc.ContactActionProcessEnabled");
        public static string AzureFuncContactActionProcessName => GetString("AzureFunc.ContactActionProcessName");
        public static string AzureFuncContactActionProcessEnableClients => GetString("AzureFunc.ContactActionProcessEnableClients");
        public static string owcCompanySecret => GetString("OWCCompanySecret");
        public static bool isDeleteCacheListEnable => GetBool("IsDeleteCacheListEnable");
        public static bool IsDisabledSalesforceWhatsappV3 => GetBool("IsDisabledSalesforceWhatsappV3");
        public static bool IsEnabledMappingErrorEmailNotify => GetBool("IsEnabledMappingErrorEmailNotify");
        public static IEnumerable<string> EmailNotifyClientCode => GetListValues("EmailNotifyClientCode");
        public static IEnumerable<string> TraceActionList => GetListValues("TraceActionList");
        public static string CaptchaSecretkey => GetString("captchaSecretkey");
        public static string CaptchaUrl => GetString("captchaUrl");
        public static bool IsCaptchaEnabled => GetBool("isCaptchaEnabled");
        public static int ElasticVMDeleteLogInterval => GetInt("ElasticVM.DeleteLogInterval");
        public static bool IsEnabledElasticSearch => GetBool("IsEnabledElasticSearch");
        public static string ElasticUrl => GetString("ElasticUrl");
        public static IEnumerable<string> EnableElasticObject => GetListValues("EnableElasticObject");
        public static string ElasticNotificationEmail => GetString("ElasticNotificationEmail");

        public static string ElasticEnvironmentPrefix => GetString("ElasticEnvironmentPrefix");
        public static string ElasticIndexName => GetString("ElasticIndexName");
        public static string ElasticAuthorizationBearer => GetString("ElasticAuthorizationBearer");
        public static bool IsEnableLogCVData => GetBool("isEnableLogCVData");
        public static string HunterUrl => GetString("HunterUrl");

        //public static string accountsApiUrl => GetString("AccountsApiUrl");
        //  public static bool IsEnableWhatsappSingleSelect => GetBool("IsEnableWhatsappSingleSelect");
        // public static bool EnableContactObjectList => GetBool("EnableContactObjectList");
        public static bool IsEnabledCustomSetting => GetBool("IsEnabledCustomSetting");
        public static IEnumerable<string> LeadApiClientCode => GetListValues("LeadApiClientCode");

        public static string ElasticVMUrl => GetString("ElasticVM.Url");
        public static string ElasticVMUsername => GetString("ElasticVM.Username");
        public static string ElasticVMPassword => GetString("ElasticVM.Password");
        public static bool IsElasticVMHttpsCertificateRequired => GetBool("ElasticVM.HttpsCertificateRequired");
        public static string WhatsAppTempRedirectionForm => GetString("WhatsApp.TempRedirectFrom");
        public static IEnumerable<string> WhatsappTempRedirectionTo => GetListValues("Whatsapp.TempRedirectTo");
        public static bool IsMappingWithParentClientcode => GetBool("IsMappingWithParentClientcode");
        public static bool IsEnabledPickListLabel => GetBool("IsEnabledPickListLabel");

        public static bool IsUserTokenEncryptionEnabled => GetBool("UserTokenEncryptionEnabled");
        public static string BlobSymmetricSecretKey = GetString("BlobSymmetricSecretKey");
        public static string JwtIssuer { get; set; }
        public static string JwtAudience { get; set; }
        public static string BlobAssessment => GetString("BlobAssessment");
        public static string BlobAssignment => GetString("BlobAssignment");

        public static string ChatGPTKey => GetString("ChatGPTKey");
        public static string FilePath => GetPath("FilePath");
        public static string GetKeyValues(string key) {
            return !string.IsNullOrWhiteSpace(_appSettings[key]) ? _appSettings[key] : null;
        }

        public static string GetPath(string key)
        {
            return !string.IsNullOrWhiteSpace(_appSettings[key]) ? _appSettings[key] : null;
        }
        #region Utils
        private static string GetString(string key, string defaultVal = null)
        {
            var data = Environment.GetEnvironmentVariable(key) ?? _appSettings[key] ?? defaultVal;
            if (data == null)
            {
                var filepath = GlobalSettings.FilePath;
                if (File.Exists(filepath))
                {
                    var json = File.ReadAllText(filepath);
                    var jsonObj = JObject.Parse(json);
                    var appSettings = jsonObj["appSettings"];
                    if (appSettings != null && appSettings[key] != null)
                    {
                        data = appSettings[key]?.ToString();
                    }
                    else
                    {
                        Console.WriteLine("Key not found: " + key);
                    }
                }
            }
            return data;
        }


        private static int GetInt(string key, int defaultVal = 0)
        {
            var dataStr = Environment.GetEnvironmentVariable(key) ?? _appSettings[key];
            var data = dataStr != null ? Convert.ToInt32(dataStr) : defaultVal;
            if (dataStr == null)
            {
                var filepath = GlobalSettings.FilePath;
                if (File.Exists(filepath))
                {
                    var json = File.ReadAllText(filepath);
                    var jsonObj = JObject.Parse(json);
                    var appSettings = jsonObj["appSettings"];
                    if (appSettings != null && appSettings[key] != null)
                    {
                        data = Convert.ToInt32(appSettings[key]);
                    }
                    else
                    {
                        Console.WriteLine("Key not found: " + key);
                    }
                }
            }
            return data;
        }

        private static bool GetBool(string key, bool defaultVal = false)
        {
            var dataStr = Environment.GetEnvironmentVariable(key) ?? _appSettings[key];
            var data = dataStr != null ? Convert.ToBoolean(dataStr) : defaultVal;
            if (dataStr == null)
            {
                var filepath = GlobalSettings.FilePath ;
                if (File.Exists(filepath))
                {
                    var json = File.ReadAllText(filepath);
                    var jsonObj = JObject.Parse(json);
                    var appSettings = jsonObj["appSettings"];
                    if (appSettings != null && appSettings[key] != null)
                    {
                        data = Convert.ToBoolean(appSettings[key]);
                    }
                    else
                    {
                        Console.WriteLine("Key not found: " + key);
                    }
                }
            }
            return data;
        }

        private static IEnumerable<string> GetListValues(string key, IEnumerable<string> defaultVal = null)
        {
            var dataStr = Environment.GetEnvironmentVariable(key) ?? _appSettings[key];
            var data = dataStr != null ? dataStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList() : defaultVal?.ToList();
            if (data == null)
            {
                var filepath = GlobalSettings.FilePath;
                if (File.Exists(filepath))
                {
                    var json = File.ReadAllText(filepath);
                    var jsonObj = JObject.Parse(json);
                    var appSettings = jsonObj["appSettings"];
                    if (appSettings != null && appSettings[key] != null)
                    {
                        data = appSettings[key].ToString()?.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    }
                    else
                    {
                        Console.WriteLine("Key not found: " + key);
                    }
                }
            }
            return data;
        }


        #endregion
    }
}

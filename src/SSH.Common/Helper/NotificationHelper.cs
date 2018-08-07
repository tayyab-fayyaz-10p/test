using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace SSH.Common.Helper
{
    public class NotificationHelper
    {
        public static string SendNotification(SendNotificationDTO notificationObj)
        {
            return SendNotificationToFCM(notificationObj);
        }

        private static string SendNotificationToFCM(SendNotificationDTO sendNotification)
        {
            string result = "";
            string pushNotificationServerId = string.IsNullOrEmpty(ConfigurationManager.AppSettings["PushNotificationServerId"]) ? string.Empty : ConfigurationManager.AppSettings["PushNotificationServerId"].ToString();
            using (var client = new WebClient())
            {
                var serializer = new JavaScriptSerializer();
                var json = JSONHelper.ConvertObjectToJSON(sendNotification);

                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Headers[HttpRequestHeader.Authorization] = pushNotificationServerId;
                result = client.UploadString("https://fcm.googleapis.com/fcm/send", "POST", json);
            }
            return result;
        }
    }

    public enum NotificationType
    {
        Default,
        RoleAsDriver,
        DocumentExpiry,
        EODSettle,
        AdditionalDocument,
        EODMarked,
        DPArrival,
        DPReject,
        NewInvite,
        SODSettle,
        Training,
        Blocked,
        Payroll
    }

    public class SendNotificationDTO
    {
        public string To { get; set; }

        public string Priority { get; set; }

        public bool Content_available { get; set; }

        public Notification Notification { get; set; }

        public NotificationData Data { get; set; }
    }

    public class Notification
    {
        public string Body { get; set; }

        public string Title { get; set; }

        public string Icon { get; set; }

        public string Sound { get; set; }
    }

    public class NotificationData
    {
        public int NotificationType { get; set; }
        
        public int Badge { get; set; }

        public int Id { get; set; }

        public string trainingId { get; set; }

        public string DeliveryPartnerStatus { get; set; }

        public string Reason { get; set; }

        public string ReasonKey { get; set; }

        public string UserId { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }
    }
}

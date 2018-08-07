using Newtonsoft.Json.Linq;

namespace SSH.Core.DTO
{
    public class ShipmentDetailsDTO
    {
        public ShipmentDetailsDTO(string json)
        {
            var jobject = JObject.Parse(json);

            this.AwbNumber = jobject["awbNumber"].ToString();
            this.Reference = jobject["reference"].ToString();
            this.ShipperName = jobject["shipperName"].ToString();
            this.ShipperAddress1 = jobject["shipperAddress1"].ToString();
            this.ShipperAddress2 = jobject["shipperAddress2"].ToString();
            this.ShipperPhone = jobject["shipperPhone"].ToString();
            this.ReceiverName = jobject["receiverName"].ToString();
            this.ReceiverAddress1 = jobject["receiverAddress1"].ToString();
            this.ReceiverAddress2 = jobject["receiverAddress2"].ToString();
            this.ReceiverPhone = jobject["receiverPhone"].ToString();
            this.DeclaredValue = (double)jobject["declaredValue"];
            this.DeclaredValueCurrency = jobject["declaredValueCurrency"].ToString();
            this.CODAmount = (double)jobject["codAmount"];
            this.CommodityDesc = jobject["commodityDesc"].ToString();
            this.OriginCountry = jobject["originCountry"].ToString();
            this.OriginCity = jobject["originCity"].ToString();
            this.DestCountry = jobject["destCountry"].ToString();
            this.DestCity = jobject["destCity"].ToString();
            this.Weight = (double)jobject["weight"];
            this.ShipperLocation = jobject["location"].ToString();
            this.ReceiverLocation = jobject["location"].ToString();
            this.DeliverDate = jobject["deliverDate"].ToString();
            this.RetailID = jobject["retailID"].ToString();
            this.Parcels = (int)jobject["parcels"];
        }

        public string AwbNumber { get; set; }

        public string Reference { get; set; }

        public string ShipperName { get; set; }

        public string ShipperAddress1 { get; set; }

        public string ShipperAddress2 { get; set; }

        public string ShipperPhone { get; set; }

        public string ReceiverName { get; set; }

        public string ReceiverAddress1 { get; set; }

        public string ReceiverAddress2 { get; set; }

        public string ReceiverPhone { get; set; }

        public double DeclaredValue { get; set; }

        public string DeclaredValueCurrency { get; set; }

        public double CODAmount { get; set; }

        public string CommodityDesc { get; set; }

        public string OriginCountry { get; set; }

        public string OriginCity { get; set; }

        public string DestCountry { get; set; }

        public string DestCity { get; set; }

        public double Weight { get; set; }

        public string ShipperLocation { get; set; }

        public string ReceiverLocation { get; set; }

        public string DeliverDate { get; set; }

        public string RetailID { get; set; }

        public int Parcels { get; set; }
    }
}

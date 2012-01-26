namespace Spinit.Wpc.Synologen.Presentation.Intranet.Models.Orders
{
    public class CreateOrderConfirmationModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonalIdNumber { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
        public string Telephone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }

        public string Article { get; set; }

        public string DeliveryOption { get; set; }
        public string PaymentOption { get; set; }
        public string TaxedAmount { get; set; }
        public string TaxfreeAmount { get; set; }
        public string TotalWithdrawal { get; set; }
        public string Monthly { get; set; }
        public string SubscriptionTime { get; set; }

        public string LeftPower { get; set; }
        public string LeftBaseCurve { get; set; }
        public string LeftAddition { get; set; }
        public string LeftDiameter { get; set; }
        public string LeftCylinder { get; set; }
        public string LeftAxis { get; set; }
        public string RightPower { get; set; }
        public string RightBaseCurve { get; set; }
        public string RightAddition { get; set; }
        public string RightDiameter { get; set; }
        public string RightCylinder { get; set; }
        public string RightAxis { get; set; }
    }
}
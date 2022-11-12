namespace Shopping.Client.Models
{
    public class EditReceiptItemModel
    {
        private decimal _price;
        private decimal _amount;

        public Guid Id { get; init; }

        public decimal Price
        {
            get => _price; set
            {
                if (_price != value)
                {
                    IsSubmitted = false;
                }

                _price = value;
            }
        }

        public decimal Amount
        {
            get => _amount; set
            {
                if (_amount != value)
                {
                    IsSubmitted = false;
                }

                _amount = value;
            }
        }

        public decimal Cost => Price * Amount;

        public bool IsSubmitted { get; set; }

        public bool IsPriceInvalid => Price <= 0 && IsSubmitted;

        public bool IsAmountInvalid => Amount <= 0 && IsSubmitted;

        public virtual bool IsValid => !(IsPriceInvalid || IsAmountInvalid);
    }
}
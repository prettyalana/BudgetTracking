public class Transaction
    {
        // Properties
        public required string Description { get; set; }
        public decimal Amount { get; set; }
        public required Category CategoryName { get; set; }
        public DateTime Date { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
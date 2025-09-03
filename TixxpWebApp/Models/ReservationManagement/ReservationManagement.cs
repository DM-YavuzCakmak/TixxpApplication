namespace Tixxp.WebApp.Models.ReservationManagement
{
    public class ReservationManagement
    {
    }
    public class IdNameVm
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public class FilterLookups
    {
        public IEnumerable<IdNameVm> Channels { get; set; } = new List<IdNameVm>();
        public IEnumerable<IdNameVm> PaymentTypes { get; set; } = new List<IdNameVm>();
        public IEnumerable<IdNameVm> Statuses { get; set; } = new List<IdNameVm>();
        public IEnumerable<IdNameVm> CurrencyTypes { get; set; } = new List<IdNameVm>();
    }

    public class ReservationListFilter
    {
        public long? ChannelId { get; set; }
        public long? PaymentTypeId { get; set; }
        public long? StatusId { get; set; }
        public string Email { get; set; }
        public long? ReservationId { get; set; }
        public long? EventId { get; set; }
        public long? CurrencyTypeId { get; set; } // varsa zaten ekli

        public DateTime? StartDate { get; set; }           // dahil
        public DateTime? EndDate { get; set; }             // dahil
        public DateTime? EndDateExclusive { get; set; }    // internal: < EndExclusive

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 25;
    }

    public class ReservationDetailProductVm
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public int Piece { get; set; }
    }



    public class ReservationListItemVm
    {
        public long Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public long ChannelId { get; set; }
        public string ChannelName { get; set; }
        public string CurrencySymbol { get; set; }
        public decimal TotalPrice { get; set; }
        public long? PaymentTypeId { get; set; }
        public string PaymentTypeName { get; set; }
        public long StatusId { get; set; }
        public string StatusName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
    }

    public class ReservationListResultVm
    {
        public List<ReservationListItemVm> Items { get; set; } = new();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }

    // ===== Detay =====

    public class ReservationDetailVm
    {
        public long ReservationId { get; set; }
        public DateTime CreatedDate { get; set; }
        public long StatusId { get; set; }
        public string StatusName { get; set; }
        public long ChannelId { get; set; }
        public string ChannelName { get; set; }
        public decimal TotalPrice { get; set; }
        public string CurrencySymbol { get; set; } 
        public ReservationInvoiceMiniVm InvoiceInfo { get; set; }
        public List<ReservationDetailTicketVm> Tickets { get; set; } = new();
        public List<ReservationDetailProductVm> Products { get; set; } = new();
        // İstersen Product satırları da ekleyebilirsin.
    }

    public class ReservationInvoiceMiniVm
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public long? PaymentTypeId { get; set; }
        public string PaymentTypeName { get; set; }
    }

    public class ReservationDetailTicketVm
    {
        public long TicketTypeId { get; set; }
        public string TicketTypeName { get; set; } = "-";
        public string TicketSubTypeName { get; set; } = "-";
        public long TicketSubTypeId { get; set; }
        public int Piece { get; set; }
    }

    // ===== Cancel =====
    public class ReservationCancelRequest
    {
        public long ReservationId { get; set; }
        public string Reason { get; set; }
    }

    // ===== Change Payment Type =====
    public class ChangePaymentTypeVm
    {
        public long ReservationId { get; set; }
        public long? CurrentPaymentTypeId { get; set; }
        public List<IdNameVm> PaymentTypeOptions { get; set; } = new();
    }

    public class ChangePaymentTypeRequest
    {
        public long ReservationId { get; set; }
        public long PaymentTypeId { get; set; }
    }
}

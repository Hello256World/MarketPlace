using MarketPlace.DataLayer.DTOs.Paging;
using MarketPlace.DataLayer.Entities.Contacts;

namespace MarketPlace.DataLayer.DTOs.Contacts
{
    public class FilterTicketDTO : BasePaging
    {
        public string Title { get; set; }

        public long? UserId { get; set; }

        public FilterTicketState FilterTicketState { get; set; }

        public FilterTicketOrder OrderBy { get; set; }

        public TicketSection? TicketSection { get; set; }

        public TicketPriority? TicketPriority { get; set; }

        public List<Ticket> Tickets { get; set; }

        public FilterTicketDTO SetTickets(List<Ticket> tickets)
        {
            this.Tickets = tickets;
            return this;
        }

        public FilterTicketDTO SetPaging(BasePaging paging)
        {
            this.PageId = paging.PageId;
            this.PageCount = paging.PageCount;
            this.HowManyShowPageAfterAndBefore = paging.HowManyShowPageAfterAndBefore;
            this.EndPage = paging.EndPage;
            this.StartPage = paging.StartPage;
            this.SkipEntity = paging.SkipEntity;
            this.TakeEntity = paging.TakeEntity;
            this.AllEntitiesCount = paging.AllEntitiesCount;
            return this;
        }
    }

    public enum FilterTicketState
    {
        All,
        NotDeleted,
        Deleted
    }

   public enum FilterTicketOrder
    {
        CreateDate_DES,
        CreateDate_ASC
    }
}

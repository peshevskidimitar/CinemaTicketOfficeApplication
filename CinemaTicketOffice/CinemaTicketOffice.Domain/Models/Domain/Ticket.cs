using CinemaTicketOffice.Domain.Entity;
using CinemaTicketOffice.Domain.Relation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CinemaTicketOffice.Domain.Models.Domain
{
    public class Ticket : BaseEntity
    {
        [Required]
        [Display(Name = "Movie Name")]
        public string MovieName { get; set; }

        [Required]
        [Display(Name = "Movie Genre")]
        public string MovieGenre { get; set; }

        [Required]
        [Display(Name = "Movie Cover Image")]
        public string MovieCoverImage { get; set; }

        [Required]
        [Display(Name = "Movie Description")]
        public string MovieDescription { get; set; }

        [Required]
        [Display(Name = "Time")]
        public DateTime Time { get; set; }

        [Required]
        [Display(Name = "Price")]
        public double Price { get; set; }

        public virtual ICollection<TicketInShoppingCart> TicketInShoppingCartCollection { get; set; }
        public virtual ICollection<TicketInOrder> TicketInOrderCollection { get; set; }
    }
}

using Shopping.Client.Shared.ResourceFiles;
using System.ComponentModel.DataAnnotations;

namespace Shopping.Client.Models
{
    public class EditCarCostModel
    {
        [Required(ErrorMessageResourceName = nameof(Resource.Description_is_required_), ErrorMessageResourceType = typeof(Resource))]
        public string Description { get; set; }

        [Required(ErrorMessageResourceName = nameof(Resource.Price_is_required_), ErrorMessageResourceType = typeof(Resource))]
        [Range(0.01, 100000, ErrorMessageResourceName = nameof(Resource.Price_is_invalid_), ErrorMessageResourceType = typeof(Resource))]
        public decimal Price { get; set; }

        [Required(ErrorMessageResourceName = nameof(Resource.Amount_is_required_), ErrorMessageResourceType = typeof(Resource))]
        [Range(0.01, 100000, ErrorMessageResourceName = nameof(Resource.Amount_is_invalid_), ErrorMessageResourceType = typeof(Resource))]
        public decimal Amount { get; set; }

        public decimal Cost => Price * Amount;

        [Required(ErrorMessageResourceName = nameof(Resource.Date_is_required), ErrorMessageResourceType = typeof(Resource))]
        public DateTime Date { get; set; }
    }
}
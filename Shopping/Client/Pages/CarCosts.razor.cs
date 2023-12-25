using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;
using Shopping.Client.Shared.ResourceFiles;
using System.ComponentModel.DataAnnotations;

namespace Shopping.Client.Pages
{
    public partial class CarCosts : ComponentBase
    {
        [Inject]
        protected HttpClient HttpClient { get; set; }

        [Inject]
        protected IStringLocalizer<Resource> Localizer { get; set; }

        [Parameter]
        [SupplyParameterFromQuery]
        public int? month { get; set; }

        private int _listMonth;

        private bool _pending;
        private CarCostModel[]? _list;

        private ValidationMessageStore _messageStore;
        private EditContext _editContext;

        private EditCarCostModel _formModel;
        private Guid? _editingItemId;
        private bool _isNew = false;

        protected override async Task OnParametersSetAsync()
        {
            if (month.HasValue && month != _listMonth)
            {
                _listMonth = month.Value;
            }
            else if (_listMonth == 0)
            {
                _listMonth = DateTime.Now.Month;
            }
            else
            {
                return;
            }
            await Reload().ConfigureAwait(false);
            await base.OnParametersSetAsync();
        }

        private async Task Reload()
        {
            _pending = true;
            _list = await HttpClient.GetFromJsonAsync<CarCostModel[]>($"/api/car-costs?month={_listMonth}");
            _pending = false;
        }

        private async void Add()
        {
            _isNew = true;
            var carCost = new CarCostModel
            {
                Id = Guid.NewGuid(),
                Description = Resource.Description_Car_Cost_Default,
                Price = 0,
                Amount = 0,
                Date = DateTime.UtcNow,
            };

            _list = new[] { carCost }.Union(_list).ToArray();

            InitEditState(carCost);
        }

        private void StartEdit(CarCostModel carCost)
        {
            _isNew = false;
            InitEditState(carCost);
        }

        private void InitEditState(CarCostModel carCost)
        {
            _formModel = new EditCarCostModel { Description = carCost.Description, Price = carCost.Price, Amount = carCost.Amount, Date = carCost.Date };
            _editContext = new EditContext(_formModel);
            _messageStore = new ValidationMessageStore(_editContext);
            _editContext.OnValidationRequested += (s, e) => _messageStore?.Clear();
            _editContext.OnFieldChanged += (s, e) => _messageStore?.Clear(e.FieldIdentifier);

            _editingItemId = carCost.Id;
        }

        private void Cancel()
        {
            _editingItemId = null;

            _formModel = null;
            _editContext = null;
            _messageStore = null;
        }

        private async Task Save()
        {
            var validationResults = new List<ValidationResult>();
            if (Validator.TryValidateObject(_formModel, new ValidationContext(_formModel), validationResults, true))
            {
                if (_isNew)
                {
                    await HttpClient.PostAsJsonAsync("/api/car-costs", new AddCarCost
                    {
                        Id = _editingItemId.Value,
                        Description = _formModel.Description,
                        Price = _formModel.Price,
                        Amount = _formModel.Amount,
                        Date = _formModel.Date
                    });
                }
                else
                {
                    await HttpClient.PutAsJsonAsync($"/api/car-costs/{_editingItemId.Value}", new UpdateCarCost
                    {
                        Description = _formModel.Description,
                        Price = _formModel.Price,
                        Amount = _formModel.Amount,
                        Date = _formModel.Date
                    });

                    await Reload().ConfigureAwait(false);
                }

                await Reload().ConfigureAwait(false);

                _editingItemId = null;
                _formModel = null;
            }
            else if (_editContext is not null)
            {
                _messageStore.Clear();
                foreach (var err in validationResults)
                {
                    _messageStore?.Add(_editContext.Field(err.MemberNames.First()), err.ErrorMessage);
                }

                _editContext.NotifyValidationStateChanged();
            }
        }

        private async Task Delete(CarCostModel carCost)
        {
            await HttpClient.DeleteAsync($"/api/car-costs/{carCost.Id}");
            await Reload().ConfigureAwait(false);
        }
    }
}
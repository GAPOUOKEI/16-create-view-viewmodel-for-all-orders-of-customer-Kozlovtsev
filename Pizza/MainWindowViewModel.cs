using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Pizza.Models;
using Pizza.Services;
using Pizza.ViewModels;
using Unity;

namespace Pizza
{
    class MainWindowViewModel : BindableBase
    {
        private AddEditCustomerViewModel _addEditCustomerViewModel;
        private CustomerListViewModel _customerListViewModel;
        private OrderPerpViewModel _orderPrepViewModel;
        private OrderViewModer _orderViewModel;

        private ICustomerRepository _customerRepository = new CustomerRepository();
        private IOrderRepository _orderRepository = new OrderRepository();

        public MainWindowViewModel()
        {
            NavigationCommand = new RelayCommand<string>(OnNavigation);
            //_customerListViewModel = new CustomerListViewModel(new CustomerRepository()) ;
            //_addEditCustomerVewModel = new AddEditCustomerViewModel(new CustomerRepository()) ; 
            _customerListViewModel = RepoContainer.Container.Resolve<CustomerListViewModel>();
            _addEditCustomerViewModel = RepoContainer.Container.Resolve<AddEditCustomerViewModel>();
            _orderViewModel = RepoContainer.Container.Resolve<OrderViewModer>();

            _customerListViewModel.AddCustomerRequested += NavigationToAddCustomer;
            _customerListViewModel.EditCustomerRequested += NavigationToEditCustomer;

            _customerListViewModel.PlaceOrderRequested += NavigateToOrder;
            _customerListViewModel.CheckOrdersCustomerRequest += NavigationToOrdersCustomer;
            _orderViewModel = new OrderViewModer(_orderRepository);
        }

        private BindableBase _currentViewModel;
        public BindableBase CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        public RelayCommand<string> NavigationCommand { get; private set; }

        private void OnNavigation(string dest)
        {
            switch (dest)
            {
                case "orderPrep":
                    CurrentViewModel = _orderPrepViewModel; break;
                case "customers":
                default:
                    CurrentViewModel = _customerListViewModel; break;
            }
        }

        private void NavigationToEditCustomer(Customer customer)
        {
            _addEditCustomerViewModel.IsEditeMode = true;
            _addEditCustomerViewModel.SetCustomer(customer);
            CurrentViewModel = _addEditCustomerViewModel;
        }

        private void NavigationToAddCustomer()
        {
            _addEditCustomerViewModel.IsEditeMode = false;
            _addEditCustomerViewModel.SetCustomer(new Customer { Id = Guid.NewGuid() });
            CurrentViewModel = _addEditCustomerViewModel;
        }

        private void NavigateToOrder(Customer customer)
        {
            _orderViewModel.Id = customer.Id;
            CurrentViewModel = _orderViewModel;
        }

        private void NavigationToOrdersCustomer(Customer customer)
        {
            _orderViewModel.LoadOrdersCustomer(customer);
            CurrentViewModel = _orderViewModel;
        }
    }

}

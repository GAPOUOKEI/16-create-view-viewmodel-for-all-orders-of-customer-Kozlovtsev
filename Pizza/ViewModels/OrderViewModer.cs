using Pizza.Models;
using Pizza.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace Pizza.ViewModels
{
    class OrderViewModer : BindableBase
    {
        private Guid _id;
        public Guid Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private ObservableCollection<Order> _orders;
        private IOrderRepository _orderRepository;
        private Customer _customer;

        public OrderViewModer(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
            Orders = new ObservableCollection<Order>();
        }

        public ObservableCollection<Order> Orders
        {
            get => _orders;
            set => SetProperty(ref _orders, value);
        }

        public RelayCommand CloseCommand { get; private set; }

        public void SetCustomer(Customer customer)
        {
            _customer = customer;
            LoadOrders();
        }

        private async void LoadOrders()
        {
            if (_customer == null) return;

            var orders = await _orderRepository.GetOrdersByCustomerAsync(_customer.Id);
            Orders.Clear();
            foreach (var order in orders.OrderBy(o => o.OrderDate)) // Сортировка заказов по дате
            {
                Orders.Add(order);
            }
        }
    }
}
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

        private IOrderRepository _orderRepository;

        public OrderViewModer(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
            Orders = new ObservableCollection<Order>();
        }

        private ObservableCollection<Order>? _orders;
        public ObservableCollection<Order>? Orders
        {
            get => _orders;
            set => SetProperty(ref _orders, value);
        }

        private List<Order>? _orderList;

        public async void LoadOrdersCustomer(Customer customer)
        {
            _orderList = await _orderRepository.GetOrdersByCustomerAsync(customer.Id);
            Orders = new ObservableCollection<Order>(_orderList);
        }
    }
}
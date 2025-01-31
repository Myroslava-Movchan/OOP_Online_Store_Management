﻿using Online_Store_Management.Models;
using System.Collections;
using Online_Store_Management.Interfaces;
using Online_Store_Management.Extensions;
namespace Online_Store_Management.Services
{
    public class OrderInfoService(IRepository<OrderInfo> orderRepository) : IOrderInfo
    {
        private readonly ArrayList orders = new(100);
        private readonly HashSet<OrderInfo> orderTable = [];
        private readonly IRepository<OrderInfo> orderRepository = orderRepository;

        public Func<OrderInfo, decimal>? CalculateTotal { get; set; }
        private static readonly string[] Gifts =
        [
            "Pin", "Sticker",
            "Candy", "Bracelet",
            "Hairclip", "Socks"
        ];
        public DateTime ConvertJapaneseToUtc(DateTime japaneseTime)
        {
            var japaneseTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
            return TimeZoneInfo.ConvertTimeToUtc(japaneseTime, japaneseTimeZone);
        }

        public DateTime GetTimeTokyo()
        {
            var time = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, time);
        }

        public async Task<OrderInfo> PostAsync(Product product, DateTime time, CancellationToken cancellationToken)
        {
            await Task.Delay(50, cancellationToken);
            var gifts = Gifts[Random.Shared.Next(Gifts.Length)];
            var orderInfo = new OrderInfo()
            {
                OrderNumber = Random.Shared.Next(1, 250),
                Gift = gifts,
                Product = product,
                Status = "Processing",
                OrderDate = ConvertJapaneseToUtc(time)
            };

            object objOrder = orderInfo;
            orders.Add(objOrder);
            OrderInfo order = (OrderInfo)objOrder;
            return order;
        }

        public async Task<bool> CompareOrdersAsync(OrderInfo order, CancellationToken cancellationToken)
        {
            await Task.Delay(50, cancellationToken);
            bool compare = false;
            foreach (var existingOrder in orders)
            {
                if (order.Equals(existingOrder))
                {
                    compare = true;
                    break;
                }
            }
            return compare;
        }

        public async Task<bool> AddToTableAsync(OrderInfo order, CancellationToken cancellationToken)
        {
            await Task.Delay(50, cancellationToken);
            foreach (var existingOrder in orders)
            {
                if (existingOrder.Equals(order))
                {
                    return false;
                }
            }
            orderTable.Add(order);
            return true;
        }
        public async Task<int> EstimateDeliveryAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(50, cancellationToken);
            int delieverySum = 120;
            return delieverySum;
        }

        public decimal GetTotal(OrderInfo order)
        {
            if(CalculateTotal == null)
            {
                return 0;
            }
            return CalculateTotal(order);
        }

        public async Task<OrderInfo?> GetOrderByIdAsync(int orderNumber, CancellationToken cancellationToken)
        {
            var order = await orderRepository.GetByIdAsync(orderNumber, cancellationToken);
            if (order != null && order.Product == null)
            {
                order.Product = new Product();
            }
            return order;
        }

        public async Task AddOrderAsync(OrderInfo order, CancellationToken cancellationToken)
        {
            await orderRepository.AddAsync(order, cancellationToken);
        }

        public async Task UpdateAsync(OrderInfo order, CancellationToken cancellationToken)
        {
            await orderRepository.UpdateAsync(order, cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            await orderRepository.DeleteAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<OrderInfo>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await orderRepository.GetAllAsync(cancellationToken);
        }
    }
}

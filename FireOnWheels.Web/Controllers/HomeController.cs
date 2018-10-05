﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FireOnWheels.Messaging;
using FireOnWheels.Web.Models;

namespace FireOnWheels.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult RegisterOrder()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterOrder(OrderViewModel model)
        {
            //Send RegisterOrderCommand
            var bus = BusConfigurator.ConfigureBus();

            var sendToUri = new Uri($"{RabbitMqConstants.RabbitMqUri}{RabbitMqConstants.SagaQueue}");
            var endPoint = await bus.GetSendEndpoint(sendToUri);

            await endPoint.Send<IRegisterOrderCommand>(new
            {
                PickupName = model.PickupName,
                PickupAddress = model.PickupAddress,
                PickupCity = model.PickupCity,
                DeliverName = model.DeliverName,
                DeliverAddress = model.DeliverAddress,
                DeliverCity = model.DeliverCity,
                Weight = model.Weight,
                Fragile = model.Fragile,
                Oversized = model.Oversized
            });

            return View("Thanks");
        }
    }
}

using BLL.Interface;
//using BLL.Interface.Dto;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmlTestTask
{
    public class MessageHub : Hub
    {
        public async Task Send(string message)
        {
            await this.Clients.All.SendAsync("Send", message);
        }

        public async Task TaskAdded(string message)
        {
            await this.Clients.All.SendAsync("TaskAdded", message);
        }
    }

    public class CustomUserIdProvider : IUserIdProvider
    {
        IComplexProvider db;
        public CustomUserIdProvider(IComplexProvider unitOfWork)
        {
            db = unitOfWork;
            //db.UseOneService(typeof(PersonDto));
        }
        //public string GetUserId(IRequest request)
        //{
        //    // your logic to fetch a user identifier goes here.

        //    // for example:

        //    var userId = MyCustomUserClass.FindUserId(request.User.Identity.Name);
        //    return userId.ToString();
        //}

        string IUserIdProvider.GetUserId(HubConnectionContext connection)
        {
            throw new NotImplementedException();
        }
    }
}

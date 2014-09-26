using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace MyPlace.Web.GameHubs
{
    public class GameHub : Hub
    {
        public void send(object pme)
        {
            Clients.All.rcvByClient(pme);
        }
        public void sendConstant(string name, string d, int x)
        {
            Clients.Others.rcvConstant(name, d, x);
        }
        public void sendMessage(string msg)
        {
            Clients.All.rcvMessage(msg);
        }
        
    }

    public class chatHub : Hub
    {
        public void sendmessage(dynamic msg)
        {
            string sendto = msg.to;
            Clients.Client(sendto).rcvMessage(msg);
            Clients.Caller.rcvSelf(msg);
        }
        public void send()
        {
            Clients.All.rcvByClient(MyUsers.ToArray());
        }
        public void giveTalkIdto(string usertosend, string code)
        {
            Clients.Client(usertosend).callSuccess(code);
        }
        public void call(string connid,string myid,string name)
        {
         
            Clients.Client(connid).playCallSound(myid,name);
        }

        public class MyUserType
        {
            public string ConnectionId { get; set; }
            public string ConnectionName { get; set; }
            // Can have whatever you want here
        }

        // Your external procedure then has access to all users via MyHub.MyUsers
        public static ConcurrentDictionary<string, MyUserType> MyUsers = new ConcurrentDictionary<string, MyUserType>();

        public override Task OnConnected()
        {
            MyUsers.TryAdd(Context.ConnectionId, new MyUserType() { 
                ConnectionId = Context.ConnectionId,
                ConnectionName=Context.Request.User.Identity.Name
            });

            Clients.All.rcvByClient(MyUsers.ToArray());
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool how)
        {
            MyUserType garbage;

            MyUsers.TryRemove(Context.ConnectionId, out garbage);
            Clients.All.rcvByClient(MyUsers.ToArray());
            return base.OnDisconnected(how);
        }

       
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;

namespace Telemetry_app
{
    public class Connection
    {
        //firebase connection Settings
        public IFirebaseConfig fc = new FirebaseConfig()
        {
            AuthSecret = "hsGBAmW4WbRA698LB2cwHTFqvrImtzamg3mQYSDz",
            BasePath = "https://telemetry-b40fc-default-rtdb.europe-west1.firebasedatabase.app/"
        };

        public IFirebaseClient client;
        //Code to warn console if class cannot connect when called.
        public Connection()
        {
            try
            {
                client = new FirebaseClient(fc);
            }
            catch (Exception)
            {
                Console.WriteLine("Cannot connect to database!");
            }
        }

        public string GetRawData() {

            string ret = client.Get("inputstring").Body.ToString();
            return ret;
        }

        public string[] GetArrayData()
        {

            string ret = client.Get("inputstring").Body.ToString();
            return ret.Split(';');
        }
    }
}

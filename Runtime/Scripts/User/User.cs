// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 05/02/2021 13:51:44 by seantcooper
using UnityEngine;
using Hawksbill;
using System.Net.NetworkInformation;

namespace Hawksbill
{
    public static class User
    {
        public static string GetMacAddress()
        {
            string macAdress = "";
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces ();

            Debug.Log ("Network Interfaces:");
            foreach (var adapter in nics)
                Debug.Log ("Adapter: " + adapter.Name + " " + adapter.GetPhysicalAddress ());



            foreach (var adapter in nics)
            {
                PhysicalAddress address = adapter.GetPhysicalAddress ();
                if (address.ToString () != "")
                {
                    macAdress = address.ToString ();
                    return macAdress;
                }
            }
            return "error lectura mac address";
        }
    }
}
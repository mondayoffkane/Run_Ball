using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
namespace AmazonAds {
    public interface IFetchManager {
        void dispense ();

        void start ();

        void stop ();

        bool isEmpty ();

        AmazonAds.AdResponse peek ();
    }
}
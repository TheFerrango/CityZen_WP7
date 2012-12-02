using System.Collections.Generic;

namespace CityZen
{
    public class AddressClass
    {
        Dictionary<string, string> _address;

        public Dictionary<string, string> address
        {
            get { return _address; }
            set { _address = value; }
        }        
    }
}

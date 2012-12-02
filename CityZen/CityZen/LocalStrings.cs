
namespace CityZen
{
    public class LocalStrings
    {
        public LocalStrings()
        {
        }

        private static Languages.AppResources locStrings = new Languages.AppResources();

        public Languages.AppResources LocStrings
        {
            get { return LocalStrings.locStrings; }
            set { LocalStrings.locStrings = value; }
        }

    }
}

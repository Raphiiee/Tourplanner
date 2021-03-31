using System;

namespace Tourplanner.BusinessLayer
{
    public static class TourItemFactory
    {
        private static ITourItemFactory instance;

        public static ITourItemFactory GetInstance()
        {
            if (instance == null)
            {
                instance = new TourItemFactoryImpl();
            }

            return instance;
        }
    }
}

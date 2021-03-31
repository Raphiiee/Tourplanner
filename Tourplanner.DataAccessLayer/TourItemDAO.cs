using System.Collections.Generic;
using Tourplanner.Models;

namespace Tourplanner.DataAccessLayer
{
    public class TourItemDAO
    {
        private IDataAccess dataAccess;

        public TourItemDAO()
        {
            // Check which datasource to use
            // dataAccess = new Database();
             dataAccess = new FileSystem();
        }

        public List<TourItem> GetItems()
        {
            return dataAccess.GetItems();
        }
    }
}
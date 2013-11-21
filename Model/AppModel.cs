using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuierobesarteApp.Model
{
    public class AppModel
    {
        string _currentWeddingId;

        public string CurrentWeddingId
        {
            get { return _currentWeddingId; }
            set { _currentWeddingId = value; }
        }
    }
}

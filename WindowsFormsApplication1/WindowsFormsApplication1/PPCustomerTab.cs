using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    class PPCustomerTab
    {
        public String id {set;get;}
        public String status { set; get; }
        public String customerName { set; get; }
        public String photoUrl { set; get; }

        public String createDate { set; get; }
        public String updateDate { set; get; }
        public String expirationDate { set; get; }

        public String invoiceId { set; get; }
        public String customerId { set; get; }
    }
}

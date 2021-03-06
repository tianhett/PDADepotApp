﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace HomotorDepotMgr.Model
{
    public class CaseData
    {
        public string InvoiceBarcode { get; set; }
        public string LoginID { get; set; }
        public string siteCode { get; set; }
        public string ModType { get; set; }
        public string ModID { get; set; }
        public string DH { get; set; }
        public List<CaseNumberEntity> InvoiceCase { get; set; }
    }
}

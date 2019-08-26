using System;

using System.Collections.Generic;
using System.Text;

namespace HomotorDepotMgr.Model
{
    public class FromERPDetailJson
    {
        public string siteCode { get; set; }
        public string ModType { get; set; }
        public string ModID { get; set; }
        public string DH { get; set; }
        public string InvoiceID { get; set; }
        public string Model { get; set; }
        public string ProdName { get; set; }
        public decimal? Num { get; set; }
        public string Barcode { get; set; }
        public string BoxBarcode { get; set; }
        public decimal? NormNum { get; set; }
        public string ProdID { get; set; }
        public string Title { get; set; }
        public string bJianMaInBox { get; set; }
        public string AreaPlaceCode { get; set; }
    }
}

using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace HomotorDepotMgr.Model
{
    public class SysProduct
    {
        public string ID { get; set; }
        public Nullable<int> SeedID { get; set; }
        public string ClassID { get; set; }
        public string ProdCode { get; set; }
        public string Brands { get; set; }
        public string ProdName { get; set; }
        public string OrigCode { get; set; }
        public string Model { get; set; }
        public string Norm { get; set; }
        public string FullName { get; set; }
        public string CarModel { get; set; }
        public string Material { get; set; }
        public string Barcode { get; set; }
        public string Unit { get; set; }
        public string PlaceofOrigin { get; set; }
        public string GW { get; set; }
        public string Size { get; set; }
        public string Parameters { get; set; }
        public Nullable<System.DateTime> ReleaseDate { get; set; }
        public Nullable<decimal> GuidancePrice { get; set; }
        public Nullable<decimal> ParPrice { get; set; }
        public string FMSICode { get; set; }
        public Nullable<bool> bDiscontinued { get; set; }
        public Nullable<bool> bStopPur { get; set; }
        public Nullable<bool> bStopSale { get; set; }
        public Nullable<bool> bNotClientSale { get; set; }
        public Nullable<bool> Status { get; set; }
        public string Memo { get; set; }
        public string ClassCode { get; set; }
        public string SuitVehicles { get; set; }
        public string Remarks { get; set; }
        public string Remarks0 { get; set; }
        public Nullable<int> NormNum { get; set; }
        public string BoxBarcode { get; set; }
        public Nullable<bool> bJianMaInBox { get; set; }
        public string CreateID { get; set; }
        public string CreateName { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public string UpdateID { get; set; }
        public string UpdateName { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
        public string ClassName { get; set; }
    }
}

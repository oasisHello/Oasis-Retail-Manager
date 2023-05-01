using ORMDataManager.Library.Internal.DataAccess;
using ORMDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Configuration.Internal;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace ORMDataManager.Library.DataAccess
{
    public class SaleData
    {

        public void SaveSale(SaleModel sale, string userId)
        {
            SqlDataAccess sql = new SqlDataAccess();
            //1.start the sale detail model we will save to the dataBase;
            List<DBSaleDetailModel> dbSaleDetails = new List<DBSaleDetailModel>();
            ProductData productData = new ProductData();
            //2.Fill the sale detail database
            foreach (var detail in sale.SaleDetails)
            {
                //2.1 Fill avaliable information
                var dbSaleDetail = new DBSaleDetailModel()
                {
                    ProductId = detail.ProductId,
                    Quantity = detail.Quantity,

                };
                //2.2 Get this product info, fill the PurchasePrice.
                var productModel = productData.GetProductById(detail.ProductId);
                if (productModel == null)
                {
                    throw new Exception($"Can't find the product with Id={detail.ProductId}");
                }
                dbSaleDetail.PurchasePrice = productModel.RetailPrice * detail.Quantity;
                //2.3 fill the Tax
                if (productModel.IsTaxable)
                {
                    dbSaleDetail.Tax = dbSaleDetail.PurchasePrice * ConfigHelper.GetTaxRate() / 100;
                }
                //2.3 populate to sale detail database
                dbSaleDetails.Add(dbSaleDetail);
            }
            //3.Create the sale model
            DBSaleModel dbSaleModel = new DBSaleModel()
            {
                CashierId = userId,
                SubTotal = dbSaleDetails.Sum(d => d.PurchasePrice),
                Tax = dbSaleDetails.Sum(d => d.Tax),
                Total = dbSaleDetails.Sum(d => d.PurchasePrice) + dbSaleDetails.Sum(d => d.Tax)

            };
            //4.Save the sale model
            string sqlComm1 = "INSERT INTO [dbo].[Sale]([CashierId],[SaleDate],[SubTotal],[Tax],[Total]) VALUES  (@CashierId, @SaleDate,@SubTotal,@Tax,@Total)";

            sql.SaveData<DBSaleModel>(sqlComm1, dbSaleModel, "DefaultConnection", CommandType.Text);
            //5.Get the Id from sale model
            string sqlComm2 = " SELECT TOP 1 [Id] FROM [dbo].[Sale] ORDER BY [SaleDate] DESC";
            var saleModelId=sql.LoadData<int,dynamic>(sqlComm2, new { @CashierId = dbSaleModel.CashierId, @SaleDate = dbSaleModel.SaleDate }, "DefaultConnection", CommandType.Text).FirstOrDefault();
            //6.Finish filling the sale detail model;7.Save the sale detail model.
            string sqlComm3 = "INSERT INTO [dbo].[SaleDetail]([SaleId],[ProductId],[Quantity],[PurchasePrice],[Tax])VALUES(@SaleId,@ProductId,@Quantity,@PurchasePrice,@Tax)";
            foreach (var item in dbSaleDetails)
            {
                item.SaleId = saleModelId;
                sql.SaveData(sqlComm3, item, "DefaultConnection", CommandType.Text);
            }

        }
    }
}

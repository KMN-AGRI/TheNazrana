
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharedModel.Contexts;
using SharedModel.Extensions;
using SharedModel.Helpers;
using SharedModel.Servers;

var services = new ServiceCollection();

services.AddDbContext<MainContext>(
options =>
{
    options.UseSqlServer(Settings.databaseString);
});


var provider = services.BuildServiceProvider();

var db = provider.GetService<MainContext>();




using (var book = new XLWorkbook("products.xlsx"))
{
    var sheet = book.Worksheets.FirstOrDefault();
    var rows = sheet.RowsUsed().Skip(1);
    
    foreach (var row in rows)
    {
        int col = 1;
        var product = new Product()
        {
            Title = row.Cell(col++).Value.ToString(),
            SubCategory = row.Cell(col++).Value.ToString(),
            Category = row.Cell(col++).Value.ToString(),
            Color = row.Cell(col++).Value.ToString(),
            SKU = row.Cell(col++).Value.ToString(),
            Medias = new List<Mediafile>()
            {
                new Mediafile()
                {
                    ClientName=row.Cell(col).Value.ToString(),
                    ServerName="/"+row.Cell(col++).Value.ToString(),
                    ContentType="image/jpg",
                    Size=0,
                }
            },
            Description = row.Cell(col++).Value.ToString(),

            Price = int.Parse(row.Cell(col++).Value.ToString()),
            Brand = "The Nazrana",
            Stock = 10,
            Tags = "#Leather#Bags"
        };

        var random = new Random().NextInt64(10, 20)*.01;
        product.Mrp = (float)(product.Price + (random * product.Price));
        product.UId = String.Join("-", product.Title.Split(' ')) + "-" + UtilityExtension.generateId();
        db.Products.Add(product);
    }
    db.SaveChanges();
   
}
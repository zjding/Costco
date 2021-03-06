﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ScrapySharp.Core;
using ScrapySharp.Html.Parsing;
using ScrapySharp.Network;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using ScrapySharp.Html.Forms;
using System.Data.SqlClient;

namespace SampleScraperClient
{
    class Program
    {
        static void Main(string[] args)
        {


            // setup the browser
            ScrapingBrowser Browser = new ScrapingBrowser();
            Browser.AllowAutoRedirect = true; // Browser has many settings you can access in setup
            Browser.AllowMetaRedirect = true;
            //go to the home page
            WebPage PageResult = Browser.NavigateToPage(new Uri("http://www.costco.com/view-more.html"));

            List<String> Names = new List<string>();
            List<HtmlNode> columnNodes = PageResult.Html.CssSelect(".viewmore-column").ToList<HtmlNode>();

            List<String> categoryUrlArray = new List<string>();
            List<String> departmentUrlArray = new List<string>();
            List<String> productUrlArray = new List<string>();

            foreach (HtmlNode columnNode in columnNodes)
            {
                List<HtmlNode> ulNodes = columnNode.SelectNodes("ul").ToList<HtmlNode>();

                List<HtmlNode> liNodes = ulNodes[0].SelectNodes("li").ToList<HtmlNode>();

                foreach (var node1 in liNodes)
                {
                    if (node1.ChildNodes[0].Attributes[0].Name == "href")
                    {
                        categoryUrlArray.Add(node1.ChildNodes[0].Attributes[0].Value);
                    }
                }
            }

            foreach (var categoryUrl in categoryUrlArray)
            {
                string url;
                if (categoryUrl.Contains("http"))
                    url = categoryUrl;
                else
                    url = "http://www.costco.com" + categoryUrl;
                PageResult = Browser.NavigateToPage(new Uri(url));
                var mainContentWrapperNote = PageResult.Html.SelectSingleNode("//div[@id='main_content_wrapper']");
                if (mainContentWrapperNote == null)
                    continue;
                List<HtmlNode> categoryNodes = mainContentWrapperNote.CssSelect(".department_facets").ToList<HtmlNode>();

                if (categoryNodes.CssSelect(".departmentContainer").Count() == 0)
                {
                    departmentUrlArray.Add(url);
                }
                else
                {
                    List<HtmlNode> departmentNodes = categoryNodes.CssSelect(".departmentContainer").ToList<HtmlNode>();
                    foreach (HtmlNode departmentNode in departmentNodes)
                    {
                        HtmlNode node = departmentNode.Descendants("a").First();
                        string departmentUrl = node.Attributes["href"].Value;
                        departmentUrlArray.Add(departmentUrl);
                    }
                }
            }

            foreach (string url in departmentUrlArray)
            {
                PageResult = Browser.NavigateToPage(new Uri(url));
                var mainContentWrapperNote = PageResult.Html.SelectSingleNode("//div[@id='main_content_wrapper']");
                List<HtmlNode> categoryNodes = mainContentWrapperNote.CssSelect(".department_facets").ToList<HtmlNode>();

                if (categoryNodes.CssSelect(".departmentContainer").Count() == 0)
                {
                    List<HtmlNode> gridNodes = PageResult.Html.CssSelect(".grid-4col").ToList<HtmlNode>();
                    if (gridNodes.Count() > 0)
                    {
                        List<HtmlNode> productNodes = gridNodes.CssSelect(".product-tile").ToList<HtmlNode>();

                        foreach (HtmlNode productNode in productNodes)
                        {
                            HtmlNode product = productNode.CssSelect(".product-tile-image-container ").First();
                            if (((product.SelectNodes("a")).First().Attributes).First().Name == "href")
                            {
                                productUrlArray.Add(((product.SelectNodes("a")).First().Attributes).First().Value);
                            }
                        }
                    }
                }
            }

            //string connectionString = "Server=tcp:zjding.database.windows.net,1433;Database=Costco;User ID=zjding@zjding;Password=G4indigo;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            string connectionString = "Data Source=DESKTOP-ABEPKAT;Initial Catalog=Costco;Integrated Security=False;User ID=sa;Password=G4indigo;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            string sqlString;

            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;

            cn.Open();

            sqlString = "TRUNCATE TABLE ProductUrls";
            cmd.CommandText = sqlString;
            cmd.ExecuteNonQuery();

            foreach (string url in productUrlArray)
            {
                sqlString = "INSERT INTO ProductUrls (ProductUrl) VALUES ('" + url + "')";
                cmd.CommandText = sqlString;
                cmd.ExecuteNonQuery();
            }

            productUrlArray.Clear();
            sqlString = "SELECT ProductUrl FROM ProductUrls";
            cmd.CommandText = sqlString;
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    productUrlArray.Add(reader.GetString(0));
                }
            }
            reader.Close();

            sqlString = "TRUNCATE TABLE ProductInfo";
            cmd.CommandText = sqlString;
            cmd.ExecuteNonQuery();

            foreach (string productUrl in productUrlArray)
            {
                try
                {

                    //string a = "http://www.costco.com/Generac-15%2c000W-Portable-Generator-with-Electric-Start-and-20&#039;-Cord.product.100278979.html";

                    PageResult = Browser.NavigateToPage(new Uri(productUrl));

                    if (PageResult.Html.InnerText.Contains("Product Not Found"))
                        continue;

                    HtmlNode productInfo = PageResult.Html.CssSelect(".product-info").ToList<HtmlNode>().First();

                    List<HtmlNode> topReviewPanelNode = productInfo.CssSelect(".top_review_panel").ToList<HtmlNode>();
                    string productName = ((topReviewPanelNode[0]).SelectNodes("h1"))[0].InnerText;

                    List<HtmlNode> col1Node = productInfo.CssSelect(".col1").ToList<HtmlNode>();
                    string itemNumber = (col1Node[0].SelectNodes("p")[0]).InnerText;
                    if (itemNumber.Length > 6)
                        itemNumber = itemNumber.Substring(6);
                    else
                        itemNumber = "";

                    string price;
                    List<HtmlNode> yourPriceNode = col1Node.CssSelect(".your-price").ToList<HtmlNode>();
                    if (yourPriceNode.Count > 0)
                    {
                        List<HtmlNode> priceNode = yourPriceNode[0].CssSelect(".currency").ToList<HtmlNode>();
                        price = priceNode[0].InnerText;
                        price = price.Replace("$", "");
                        price = price.Replace(",", "");

                        if (price == "- -")
                            price = "-2";
                    }
                    else
                    {
                        price = "-1";
                    }

                    string shipping;
                    List<HtmlNode> productsNode = col1Node.CssSelect(".products").ToList<HtmlNode>();
                    if (productsNode.Count > 0)
                        shipping = productsNode[0].SelectSingleNode("li").SelectSingleNode("p") == null ? "-1" : productsNode[0].SelectSingleNode("li").SelectSingleNode("p").InnerText;
                    else
                        shipping = "-1";

                    HtmlNode productDetailTabsNode = PageResult.Html.CssSelect(".product-detail-tabs").ToList<HtmlNode>().First();

                    var productDetailTab1Node = productDetailTabsNode.SelectSingleNode("//div[@id='product-tab1']");

                    var productDescriptionNode = productDetailTab1Node.SelectSingleNode("//p[@itemprop='description']");

                    string description = productDescriptionNode.InnerText;
                    description = description.Replace("'", "\"");

                    var productDetailTab2Node = productDetailTabsNode.SelectSingleNode("//div[@id='product-tab2']");

                    string specification = "";
                    if (productDetailTab2Node != null)
                    {
                        specification = productDetailTab2Node.InnerText;
                        specification = specification.Replace("'", "\"");
                    }

                    HtmlNode imageColumnNode = PageResult.Html.CssSelect(".image-column").ToList<HtmlNode>().First();

                    HtmlNode imageNode = imageColumnNode.SelectSingleNode("//img[@itemprop='image']");

                    string imageUrl = (imageNode.Attributes["src"]).Value;

                    sqlString = "INSERT INTO ProductInfo (Name, ItemNumber, Price, Shipping, Details, Specification, ImageLink, Url) VALUES ('" + productName + "','" + itemNumber + "'," + price + "," + 0 + "," + "'" + description + "','" + specification + "','" + imageUrl + "','" + productUrl + "')";
                    cmd.CommandText = sqlString;
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    continue;
                }

            }


            cn.Close();


            //}

            //foreach (var row in nodes.CssSelect(".viewmore-list")// .SelectNodes("tbody/tr"))
            //{
            //    foreach (var cell in row.SelectNodes("td[1]"))
            //    {
            //        Names.Add(cell.InnerText);
            //    }
            //}
            // find a form and send back data
            //PageWebForm form = PageResult.FindFormById("dataForm");
            //// assign values to the form fields
            //form["UserName"] = "AJSON";
            //form["Gender"] = "M";
            //form.Method = HttpVerb.Post;
            //WebPage resultsPage = form.Submit();

        }
    }
}

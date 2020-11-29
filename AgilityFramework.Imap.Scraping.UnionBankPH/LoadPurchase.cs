using System;
using System.Globalization;
using System.Linq;
using HtmlAgilityPack;

namespace AgilityFramework.Imap.Scraping.UnionBankPH
{
    public class LoadPurchase
    {
        public LoadPurchase() { }

        public string ReferenceNo { get; set; }

        public DateTime TransactionDate { get; set; }

        public string Recipient { get; set; }

        public decimal Amount { get; set; }

        public static LoadPurchase Scrape(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var root = doc.DocumentNode;
            var body = root.SelectSingleNode("//body");
            var table = body.Descendants("table").ElementAt(0);
            var tableHead = table.Element("thead");
            var tableBody = table.Element("tbody");
            var headCells = tableHead.Descendants("td");
            var bodyRows = tableBody.Elements("tr");

            // Source groups...
            var referenceGroup = headCells.ElementAt(0);
            var transDateGroup = headCells.ElementAt(1);
            var toGroup = bodyRows.ElementAt(1);
            var amountGroup = bodyRows.ElementAt(2);

            var referenceNo = referenceGroup.Descendants("span").ElementAt(1).InnerText;
            var transDate = transDateGroup.Descendants("span").ElementAt(1).InnerText;
            var recipient = toGroup.Descendants("span").ElementAt(0).InnerText;
            var strAmount = amountGroup.Descendants("span").ElementAt(1).InnerText;

            // Sanitation...
            referenceNo = referenceNo.Trim();
            transDate = transDate.Trim();
            recipient = recipient.Trim()
                                 .Replace("=", string.Empty);
            strAmount = strAmount.Trim()
                                 .Replace(",", string.Empty);

            // Parsing...
            var transactionDate = DateTime.ParseExact(transDate, "MMMM dd, yyyy", CultureInfo.CurrentCulture);
            var amount = decimal.Parse(strAmount);

            return new LoadPurchase
            {
                ReferenceNo = referenceNo,
                TransactionDate = transactionDate,
                Recipient = recipient,
                Amount = amount,
            };
        }
    }
}

using System;
using System.Globalization;
using System.Linq;
using HtmlAgilityPack;

namespace AgilityFramework.Imap.Scraping.UnionBankPH
{
    public class FundTransfer
    {
        public FundTransfer() { }

        public FundTransfer(string referenceNo, DateTime transactionDate, decimal amount)
        {
            ReferenceNo = referenceNo;
            TransactionDate = transactionDate;
            Amount = amount;
        }

        public string ReferenceNo { get; set; }

        public DateTime TransactionDate { get; set; }

        public string Recipient { get; set; }

        public string RecipientBank { get; set; }

        public string RecipientAccount { get; set; }

        public decimal Amount { get; set; }

        public string Message { get; set; }

        public static FundTransfer Scrape(string html)
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
            var messageGroup = bodyRows.Count() == 4 ? bodyRows.ElementAt(3) : null;

            var referenceNo = referenceGroup.Descendants("span").ElementAt(1).InnerText;
            var transDate = transDateGroup.Descendants("span").ElementAt(1).InnerText;
            var recipient = toGroup.Descendants("div").ElementAt(0).InnerText;
            var recipientBank = toGroup.Descendants("div").ElementAt(1).InnerText;
            var recipientAccount = toGroup.Descendants("div").ElementAt(2).InnerText;
            var strAmount = amountGroup.Descendants("span").ElementAt(1).InnerText;
            var message = messageGroup?.Descendants("span").ElementAt(0).InnerText ?? "";

            // Sanitation...
            referenceNo = referenceNo.Trim();
            transDate = transDate.Trim();
            recipient = recipient.Trim()
                                 .Replace("=\n", string.Empty)
                                 .Replace("=", string.Empty);
            recipientBank = recipientBank.Trim()
                                         .Replace("=\n", string.Empty)
                                         .Replace("=", string.Empty);
            recipientAccount = recipientAccount.Trim();
            strAmount = strAmount.Trim()
                                 .Replace(",", string.Empty);
            message = message.Trim()
                             .Replace("=\n", string.Empty)
                             .Replace("=", string.Empty);

            // Patch: Some markups has no ending div </div> and causes scraper to concat all three details...
            recipient = recipient.Replace(recipientBank, string.Empty)
                                 .Replace(recipientAccount, string.Empty)
                                 .Replace("\n", string.Empty);

            // Parsing...
            var transactionDate = DateTime.ParseExact(transDate, "MMMM dd, yyyy", CultureInfo.CurrentCulture);
            var amount = decimal.Parse(strAmount);

            return new FundTransfer
            {
                ReferenceNo = referenceNo,
                TransactionDate = transactionDate,
                Recipient = recipient,
                RecipientBank = recipientBank,
                RecipientAccount = recipientAccount,
                Amount = amount,
                Message = message
            };
        }
    }
}

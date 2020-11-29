### Introduction

Scrape email notifications from UnionBank PH into .NET objects.

### Downloads

You can get the latest package on [Nuget](https://www.nuget.org/packages/AgilityFramework.Imap/).

### Usage & Examples

```csharp
var html = ""; // HTML from Imap using AgilityFramework.Imap
var fundTransfer = FundTransfer.Scrape(html);

Console.WriteLine(fundTransfer.ReferenceNo);
Console.WriteLine(fundTransfer.TransactionDate);
Console.WriteLine(fundTransfer.Amount);
```

### Features

+ Supports Fund Transfer confirmations.

### Credits

Copyright © 2020 Gary Antier.

### License

This library is released under the [MIT license](https://github.com/sagemodeninja/AgilityFramework.Imap.Scraping.UnionBankPH/blob/master/License.md).

### Bug reports

Please send your bug reports to [contact@garyantier.com](mailto:contact@garyantier.com).

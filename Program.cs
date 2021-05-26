using System;
using System.Collections.Generic;
using System.Linq;
using Gw2Sharp;
using MathNet.Numerics;
using System.Threading.Tasks;

namespace GW2TPRegression
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var connection = new Connection();
            using var client = new Gw2Client(connection);
            var webApiClient = client.WebApi.V2;
            /*var allIds = await webApiClient.Items.IdsAsync();
            var allIdsList = allIds.Where(a => a > 19000).ToList();
            foreach(var id in allIdsList)
            {
                var item = webApiClient.Items.GetAsync(id).Result;
                try
                {
                    var commerces = webApiClient.Commerce.Listings.GetAsync(id).Result;
                    var buyPrices = commerces.Buys.Select(anon => (double)(anon.UnitPrice)).ToArray();
                    var buyQuantities = new List<double>();
                    foreach (var buyQ in commerces.Buys.Select(anon => anon.Quantity))
                    {
                        var addNumber = (buyQuantities.Count > 0) ? buyQuantities[^1] + ((double)(buyQ)) : (double)buyQ;
                        buyQuantities.Add(addNumber);
                    }
                    var median = (buyPrices.Length % 2 == 0) ? buyPrices[(int)(buyPrices.Length / 2)] : buyPrices[(int)((buyPrices.Length - 1) / 2)];
                    var overEstimates = buyPrices.Where(anon => anon < median / 1.5).Count();

                    var buyRegression = Fit.Polynomial(buyQuantities.ToArray(), buyPrices.ToArray(), 3);
                    var buyRS2 = GoodnessOfFit.RSquared(buyQuantities.Select(x => buyRegression[0] + buyRegression[1] * x + buyRegression[2] * x * x + buyRegression[3] * x * x * x), buyPrices);

                    var sellPrices = commerces.Sells.Select(anon => (double)(anon.UnitPrice)).ToArray();
                    var sellQuantities = new List<double>();
                    foreach (var sellQ in commerces.Sells.Select(anon => anon.Quantity))
                    {
                        var addNumber = (sellQuantities.Count > 0) ? sellQuantities[^1] + ((double)(sellQ)) : (double)sellQ;
                        sellQuantities.Add(addNumber);
                    }
                    median = (sellPrices.Length % 2 == 0) ? sellPrices[(int)(sellPrices.Length / 2)] : sellPrices[(int)((sellPrices.Length - 1) / 2)];
                    overEstimates = sellPrices.Where(anon => anon > median * 1.5).Count();

                    var sellRegression = Fit.Polynomial(sellQuantities.Take(sellPrices.Length - overEstimates).ToArray(), sellPrices.Take(sellPrices.Length - overEstimates).ToArray(), 3);
                    var sellRS2 = GoodnessOfFit.RSquared(sellQuantities.Take(sellPrices.Length - overEstimates).Select(x => sellRegression[0] + sellRegression[1] * x + sellRegression[2] * x * x + sellRegression[3] * x * x * x), sellPrices.Take(sellPrices.Length - overEstimates).ToArray());

                    var allResults = FindRoots.Cubic(sellRegression[0] - buyRegression[0], sellRegression[1] - buyRegression[1], sellRegression[2] - buyRegression[2], sellRegression[3] - buyRegression[3]);

                    var realResults = new List<double>();
                    if (allResults.Item1.Imaginary.Equals(0))
                    {
                        realResults.Add(allResults.Item1.Real);
                    }
                    if (allResults.Item2.Imaginary.Equals(0))
                    {
                        realResults.Add(allResults.Item2.Real);
                    }
                    if (allResults.Item3.Imaginary.Equals(0))
                    {
                        realResults.Add(allResults.Item3.Real);
                    }

                    var optimalResults = realResults
                        .Select(x => new
                        {
                            OptimalQuantity = x,
                            OptimalPrice = buyRegression[0] + buyRegression[1] * x + buyRegression[2] * x * x + buyRegression[3] * x * x * x
                        })
                        .ToList();

                    var allTaxedResults = FindRoots.Cubic(0.85 * sellRegression[0] - buyRegression[0], 0.85 * sellRegression[1] - buyRegression[1], 0.85 * sellRegression[2] - buyRegression[2], 0.85 * sellRegression[3] - buyRegression[3]);

                    var realTaxedResults = new List<double>();
                    if (allTaxedResults.Item1.Imaginary.Equals(0))
                    {
                        realTaxedResults.Add(allTaxedResults.Item1.Real);
                    }
                    if (allTaxedResults.Item2.Imaginary.Equals(0))
                    {
                        realTaxedResults.Add(allTaxedResults.Item2.Real);
                    }
                    if (allTaxedResults.Item3.Imaginary.Equals(0))
                    {
                        realTaxedResults.Add(allTaxedResults.Item3.Real);
                    }

                    var taxedResults = realTaxedResults
                        .Select(x => new
                        {
                            TaxedQuantity = x,
                            TaxedPriceBuyer = buyRegression[0] + buyRegression[1] * x + buyRegression[2] * x * x + buyRegression[3] * x * x * x,
                            TaxedPriceSeller = sellRegression[0] + sellRegression[1] * x + sellRegression[2] * x * x + sellRegression[3] * x * x * x
                        })
                        .ToList();
                    var accuracy = buyRS2 * sellRS2 * 100;
                    if (accuracy >= 95)
                    {
                        Console.WriteLine($"{id} - {item.Name}");
                    }
                }
                catch
                {
                    // nothing
                }
            }
            return;*/
            while (true)
            {
                Console.Write("Item Id: ");
                var input = Console.ReadLine();
                if (input == "q")
                {
                    break;
                }
                else if (input.Equals("wg"))
                {
                    input = "77604";
                }
                else if (input.Equals("mc"))
                {
                    input = "19976";
                }
                else if (input.Equals("ecto"))
                {
                    input = "19721";
                }
                if (!Int32.TryParse(input, out int id))
                {
                    Console.WriteLine("Not a number!");
                    continue;
                }

                var item = webApiClient.Items.GetAsync(id).Result;
                Console.WriteLine($"{item.Name}");
                var commerces = webApiClient.Commerce.Listings.GetAsync(id).Result;
                Console.WriteLine();
                var buyPrices = commerces.Buys.Select(anon => (double)(anon.UnitPrice)).ToArray();
                var buyQuantities = new List<double>();
                foreach (var buyQ in commerces.Buys.Select(anon => anon.Quantity))
                {
                    var addNumber = (buyQuantities.Count > 0) ? buyQuantities[^1] + ((double)(buyQ)) : (double)buyQ;
                    buyQuantities.Add(addNumber);
                }
                var median = (buyPrices.Length % 2 == 0) ? buyPrices[(int)(buyPrices.Length / 2)] : buyPrices[(int)((buyPrices.Length - 1) / 2)];
                //var overEstimates = buyPrices.Where(anon => anon < median / 1.5).Count();
                var overEstimates = 0;

                var buyRegression = Fit.Polynomial(buyQuantities.ToArray(), buyPrices.ToArray(), 3);
                var buyRS2 = GoodnessOfFit.RSquared(buyQuantities.Select(x => buyRegression[0] + buyRegression[1]*x + buyRegression[2]*x*x + buyRegression[3]*x*x*x), buyPrices);

                var sellPrices = commerces.Sells.Select(anon => (double)(anon.UnitPrice)).ToArray();
                var sellQuantities = new List<double>();
                foreach (var sellQ in commerces.Sells.Select(anon => anon.Quantity))
                {
                    var addNumber = (sellQuantities.Count > 0) ? sellQuantities[^1] + ((double)(sellQ)) : (double)sellQ;
                    sellQuantities.Add(addNumber);
                }
                median = (sellPrices.Length % 2 == 0) ? sellPrices[(int)(sellPrices.Length / 2)] : sellPrices[(int)((sellPrices.Length - 1) / 2)];
                overEstimates = sellPrices.Where(anon => anon > median * 1.5).Count();

                var sellRegression = Fit.Polynomial(sellQuantities.Take(sellPrices.Length - overEstimates).ToArray(), sellPrices.Take(sellPrices.Length - overEstimates).ToArray(), 3);
                var sellRS2 = GoodnessOfFit.RSquared(sellQuantities.Take(sellPrices.Length - overEstimates).Select(x => sellRegression[0] + sellRegression[1]*x + sellRegression[2]*x*x + sellRegression[3]*x*x*x), sellPrices.Take(sellPrices.Length - overEstimates).ToArray());

                var allResults = FindRoots.Cubic(sellRegression[0] - buyRegression[0], sellRegression[1] - buyRegression[1], sellRegression[2] - buyRegression[2], sellRegression[3] - buyRegression[3]);

                var realResults = new List<double>();
                if (allResults.Item1.Imaginary.Equals(0))
                {
                    realResults.Add(allResults.Item1.Real);
                }
                if (allResults.Item2.Imaginary.Equals(0))
                {
                    realResults.Add(allResults.Item2.Real);
                }
                if (allResults.Item3.Imaginary.Equals(0))
                {
                    realResults.Add(allResults.Item3.Real);
                }

                var optimalResults = realResults
                    .Select(x => new
                    {
                        OptimalQuantity = x,
                        OptimalPrice = buyRegression[0] + buyRegression[1]*x + buyRegression[2]*x*x + buyRegression[3]*x*x*x
                    })
                    .ToList();

                var allTaxedResults = FindRoots.Cubic(0.85 * sellRegression[0] - buyRegression[0], 0.85 * sellRegression[1] - buyRegression[1], 0.85 * sellRegression[2] - buyRegression[2], 0.85 * sellRegression[3] - buyRegression[3]);

                var realTaxedResults = new List<double>();
                if (allTaxedResults.Item1.Imaginary.Equals(0))
                {
                    realTaxedResults.Add(allTaxedResults.Item1.Real);
                }
                if (allTaxedResults.Item2.Imaginary.Equals(0))
                {
                    realTaxedResults.Add(allTaxedResults.Item2.Real);
                }
                if (allTaxedResults.Item3.Imaginary.Equals(0))
                {
                    realTaxedResults.Add(allTaxedResults.Item3.Real);
                }

                var taxedResults = realTaxedResults
                    .Select(x => new
                    {
                        TaxedQuantity = x,
                        TaxedPriceBuyer = buyRegression[0] + buyRegression[1]*x + buyRegression[2]*x*x + buyRegression[3]*x*x*x,
                        TaxedPriceSeller = sellRegression[0] + sellRegression[1]*x + sellRegression[2]*x*x + sellRegression[3]*x*x*x
                    })
                    .ToList();

                Console.WriteLine($"Buy order accuracy: {buyRS2 * 100}%");
                Console.WriteLine($"Sell order accuracy: {sellRS2 * 100}%");
                var accuracy = buyRS2 * sellRS2 * 100;
                Console.WriteLine($"Total data accuracy: {accuracy}%");
                Console.WriteLine($"p-value for data: {1 - buyRS2 * sellRS2}");
                Console.WriteLine();
                Console.WriteLine("Found optimals:");
                foreach(var result in optimalResults)
                {
                    Console.WriteLine($"- q*: {result.OptimalQuantity}");
                    Console.WriteLine($"- p*: {result.OptimalPrice}");
                    Console.WriteLine();
                }

                Console.WriteLine("Found taxed solutions:");
                foreach (var result in taxedResults)
                {
                    Console.WriteLine($"- taxed q: {result.TaxedQuantity}");
                    Console.WriteLine($"- taxed Pb: {result.TaxedPriceBuyer}");
                    Console.WriteLine($"- taxed Ps: {result.TaxedPriceSeller}");
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }
    }
}

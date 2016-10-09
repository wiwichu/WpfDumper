using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Reactive.Testing;
using WpfDumper.Model;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using System.Collections.Concurrent;
using System.Reactive.Concurrency;

namespace WpfDumper.Services
{
    public static class TickerService
    {
        private static IObservable<Tick> ticks = null;
        private static Random rand = new Random();
        private static ConcurrentDictionary<string, decimal> lastTicks = new ConcurrentDictionary<string, decimal>();
        static TickerService()
        {
            CreateTickService();
        }

        private static void CreateTickService()
        {
            var tick = GetNextTick(null);
            ticks = Observable.Generate(tick,
                                            _ => true,
                                            GetNextTick,
                                            p => p,
                                            _ => GetNextPriceDelay(),
                                            NewThreadScheduler.Default
                                            );
        }
        static private TimeSpan GetNextPriceDelay()
        {
            //return TimeSpan.FromSeconds(rand.Next(100) / 20.0);
            return new TimeSpan(0,0,0,0,10);
        }
        static private Tick GetNextTick(Tick currentTick)
        {
            decimal newPrice = 0m;
            string symbol = $"SYM{rand.Next(100)}";
            if (!lastTicks.TryGetValue(symbol,out newPrice))
            {
                newPrice = GetNextPrice(100m);
                lastTicks.TryAdd(symbol, newPrice);
            }
            else
            {
                lastTicks.TryUpdate(symbol, newPrice, newPrice);
            }
            Tick newTick = new Tick
            {
                Symbol = symbol,
                TickValue = GetNextPrice(newPrice),
                TimeStamp = DateTime.UtcNow
            };
            return newTick;
        }
        static private decimal GetNextPrice(decimal currentPrice)
        {
            // generate a percentage between -3% and +3%
            var percentage = (decimal)rand.NextDouble() * 6 - 3;
            var newPrice = currentPrice * (1 + percentage / 100);
            return Decimal.Round(newPrice, 2);
        }

        public static IObservable<Tick> Ticks
        {
            get { return ticks; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reactive.Linq;
using System.Reactive.Disposables;

namespace Stats
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = Observable.Create<double>(observer =>
            {
                while (true)
                {
                    var line = Console.ReadLine();
                    double value;
                    if (double.TryParse(line, out value))
                    {
                        observer.OnNext(value);
                    }
                }

                return Disposable.Empty;
            });

            data.Subscribe(x => Console.WriteLine("value: {0}", x));

            var totals = data.Scan(0.0, (total, x) => total + x);
            totals.Subscribe(total => Console.WriteLine("total: {0}", total));
        }
    }
}

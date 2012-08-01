using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reactive.Linq;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Reactive.Concurrency;

namespace Stats
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = ConsoleToObservable();

            data.Subscribe(x => Console.WriteLine("value: {0}", x));

            var totals = data.Scan(0.0, (total, x) => total + x);
            totals.Subscribe(total => Console.WriteLine("total: {0}", total));

            data.Connect();
        }

        static IConnectableObservable<double> ConsoleToObservable()
        {
            return Observable.Create<double>(observer =>
            {
                return Scheduler.Immediate.Schedule(self =>
                {
                    var currentValue = Console.ReadLine();
                    double value;

                    if (double.TryParse(currentValue, out value))
                    {
                        observer.OnNext(value);
                    }

                    self();
                });
            }).Publish();
        }
    }
}

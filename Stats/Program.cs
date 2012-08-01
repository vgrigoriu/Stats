using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reactive.Linq;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;

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

        static Task task;

        static IConnectableObservable<double> ConsoleToObservable()
        {
            return Observable.Create<double>(s =>
            {
                var cts = new CancellationTokenSource();
                task = Task.Factory.StartNew(obj =>
                {
                    var ct = (CancellationToken)obj;
                    while (true)
                    {
                        ct.ThrowIfCancellationRequested();

                        var input = Console.ReadLine();
                        double value;
                        if (double.TryParse(input, out value))
                        {
                            s.OnNext(value);
                        }
                    }
                }, cts.Token);

                return () => cts.Cancel();
            }).Publish();
        }
    }
}

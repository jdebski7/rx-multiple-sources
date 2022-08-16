// See https://aka.ms/new-console-template for more information

using System.Reactive.Linq;
using System.Reactive.Subjects;

var subjectA = new Subject<string>();
var subjectB = new Subject<string>();
var subjectC = new Subject<string>();

var sourceA = subjectA;
var sourceB = sourceA.Select(_ => Observable.Timer(TimeSpan.FromSeconds(2)).Select(_ => "up").StartWith("down")).Switch()
    .Select(semaphore => semaphore == "up" ? subjectB.AsObservable() : Observable.Empty<string?>()).Switch();
var sourceC = sourceA.Merge(sourceB).Select(_ => Observable.Timer(TimeSpan.FromSeconds(5)).Select(_ => "up").StartWith("down")).Switch()
    .Select(semaphore => semaphore == "up" ? subjectC.AsObservable() : Observable.Empty<string?>()).Switch();

sourceA.Merge(sourceB).Merge(sourceC).Subscribe(Console.WriteLine);

var taskA = Task.Run(async () =>
{
    subjectA.OnNext("a1"); 
    
    await Task.Delay(TimeSpan.FromSeconds(1));
    subjectA.OnNext("a2");
    
    await Task.Delay(TimeSpan.FromSeconds(5));
    subjectA.OnNext("a3");
    
    await Task.Delay(TimeSpan.FromSeconds(3));
    subjectA.OnNext("a4");
    
    await Task.Delay(TimeSpan.FromSeconds(1));
    subjectA.OnNext("a5");
    
    await Task.Delay(TimeSpan.FromSeconds(1));
    subjectA.OnNext("a6");
    
    await Task.Delay(TimeSpan.FromSeconds(1));
    subjectA.OnNext("a7");
});

var taskB = Task.Run(async () =>
{
    await Task.Delay(TimeSpan.FromMilliseconds(100));
    subjectB.OnNext("b1");
    
    await Task.Delay(TimeSpan.FromSeconds(2));
    subjectB.OnNext("b2");
    
    await Task.Delay(TimeSpan.FromSeconds(2));
    subjectB.OnNext("b3");
    
    await Task.Delay(TimeSpan.FromSeconds(2));
    subjectB.OnNext("b4");
    
    await Task.Delay(TimeSpan.FromSeconds(2));
    subjectB.OnNext("b5");
    
    await Task.Delay(TimeSpan.FromSeconds(2));
    subjectB.OnNext("b6");
    
    await Task.Delay(TimeSpan.FromSeconds(2));
    subjectB.OnNext("b7");
    
    await Task.Delay(TimeSpan.FromSeconds(2));
    subjectB.OnNext("b8");
});

var taskC = Task.Run(async () =>
{
    while (true)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(100));
        subjectC.OnNext("c");
    }
});

await Task.Delay(TimeSpan.FromSeconds(30));
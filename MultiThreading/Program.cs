using System.Threading;
using System.Threading.Tasks;
using System.Globalization;

class Program
{
    // Uncomment the function you want to run
    static Mutex _mutex = new Mutex();
    static Semaphore _semaphore = new Semaphore(2,2);
    static  void Main(string[] args)
    {
        // BasicThreading();
        // IsAliveAndJoin();
        // Protecting shared resources from concurrent access in multithreading by locking 
        // ValidLocking(); //using locks, exclusive locking
        // UseMutexLocking(); // using mutex to achive a exclusive lock
        // UseSemaphoreLocking(); // using semaphore to achive a shared lock
        // ThreadPoolExample(); // create a thread pool to resolve a memory issues 
        // Async Programming
        // Need to learn about task based Async patterns
        //UseTasks(); // Using tasks to achieve multithreading
        //ReturnAValueFromTasks(); // How to return a value from a task
        //ReturnComplexValuesFromTasks(); // How to retrun complex values
        // Async and Await; best practices and when to use
        
        // Parallel Programming using TPL

        // Data Prallelism and Task Prallelism
        //Data -> operation is applied to each element of the collection which means the process does the same work
        // with unique and indpendent pices of data ;  Parallel For and parallel foreach
        // Task -> independent computations are executed in parallel; each process performs a different function or executes different 
        // code sections that are independent, ; Parallel.invoke

        //ParallelForLoop(); // same syntax can be used with coollections for a parallel foreach loop
        //PLINQ(); // Parallel LINQ

    }

    static void PLINQ()
    {
        var source = Enumerable.Range(100, 500);

        var evenNum = from num in source.AsParallel() 
                        where num % 2 == 0
                        select num;
        Console.WriteLine($"We have total {evenNum.Count()} even numbers out of {source.Count()} ");
    }

    static void ParallelForLoop()
    {
        int length = 10;

        for(int i =0; i<10; i++)
        {
            Console.WriteLine($"The current value is {i} and the Current Thread is {Thread.CurrentThread.ManagedThreadId}");
        }

        Console.WriteLine("Now executing the Prallel for");

        ParallelOptions _options = new ParallelOptions{
            MaxDegreeOfParallelism = 3, //  after passing the options we make sure that only 3 threads at max use it
        };

        Parallel.For(0,length, _options,count => {
            Console.WriteLine($"The current value is {count} and the Current Thread is {Thread.CurrentThread.ManagedThreadId}");
        });
    }

    static void ReturnComplexValuesFromTasks()
    {
        Task<Student> t1 = Task.Run(() =>
        {
           Student studentstudent = new Student()
           {
            Id = 1,
            Name = "ABC"
           };
           return studentstudent;
        }
        );
        Console.WriteLine(t1.Result.Id + " " + t1.Result.Name); 
        t1.Wait();
    }

    class Student
    {
        public int Id { get; set;}
        public string Name { get; set;}
    }
    static void ReturnAValueFromTasks()
    {
        Task<int> t1 = Task.Run(() =>
        {
            return GetTotal(5);
        }
        );
        Console.WriteLine(t1.Result); 
        t1.Wait();
    }

    static int GetTotal(int max)
    {
        return max ;
    }
    static  void UseTasks()
    {
        Task t1 = new Task(Method1);
        t1.Start();
        Task t2 = Task.Run(() => { Method1(); }); // No need to call start; easy way
        t1.Wait(); // make sures that the main thread waits till this child task is complete
        

    }
    static void ThreadPoolExample()
    {
        Console.WriteLine("ThreadPool func started");
        for(int i = 0; i<3 ; i++)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadPoolWorker));
        }
        Console.WriteLine("ThreadPool func ended");
    }

    static void ThreadPoolWorker(object state)
    {
        Thread t = Thread.CurrentThread;
        string message  = $"isBackground : {t.IsBackground} is from ThreadPool : {t.IsThreadPoolThread}";
        Console.WriteLine(message);
    }
    static void UseSemaphoreLocking()
    {
        for (int i = 0; i <= 5; i++)
        {
            new Thread(WriteSemaphore).Start();
        }
    }
    static void WriteSemaphore()
    {
      Console.WriteLine(Thread.CurrentThread.ManagedThreadId + "Write Thread Waiting");
        _semaphore.WaitOne();
        Console.WriteLine(Thread.CurrentThread.ManagedThreadId + "Write Thread Writing");
        Thread.Sleep(5000);
        _semaphore.Release();   
    }

    static void UseMutexLocking()
    {
        for (int i = 0; i <= 5; i++)
        {
            new Thread(Write).Start();
        }

    }

    static void Write()
    {
        Console.WriteLine(Thread.CurrentThread.ManagedThreadId + "Write Thread Waiting");
        _mutex.WaitOne();
        Console.WriteLine(Thread.CurrentThread.ManagedThreadId + "Write Thread Writing");
        Thread.Sleep(5000);
        _mutex.ReleaseMutex();
    }


    public static int sum=0;
    static void ValidLocking()
    {

        Thread t1 = new Thread(Addnumbers);
        Thread t2 = new Thread(Addnumbers);
        Thread t3 = new Thread(Addnumbers);
        // All threads access a static variable
        t1.Start();
        t2.Start();
        t3.Start();

        t1.Join();
        t2.Join();
        t3.Join();

        Console.WriteLine($"The total sum is : {sum}");
    }

    public static Object _lock = new Object();
    static void Addnumbers()
    {
        for (int i = 0; i < 5; i++)
        {
            lock (_lock){
                sum+=i;
                Console.WriteLine(sum);
            }
            
        }
    }

    static void IsAliveAndJoin()
    {      
        //IsAlive and Join in Thread
        /*
        Thread t1 = new Thread(Method1);
        t1.Start();
        t1.Join();
        // To make sure that the main thread does not finish before t1 or t2, we need to use join
        // Here the main thread waits
        // To make sure the main thread does not gets blocked infinitly we pass a number in join
        // t1.Join(2000) it will wait for 2000 milli second and then let the main thread continue, does not matter if t1 completes or not

        Thread t2 = new Thread(Method2);
        t2.Start();
        t2.Join();

        */
    }

    static void BasicThreading()
    {
        // Checking for main thread 
        /*
        Thread thread1 = Thread.CurrentThread;
        thread1.Name = "Main thread";
        Console.WriteLine("the name of thread1 is " + thread1.Name);
        Console.WriteLine("Name of current thread is " + Thread.CurrentThread.Name);
        */

        // Initialise a function in a thread
       // Thread t1 = new Thread(ShowNumbers);
       // t1.Start();

    }
    static void Method1()
    {
        Console.WriteLine("Method 1 is executing");
    }

    static void Method2()
    {
        Console.WriteLine("Methods 2 is executing");
    }
}
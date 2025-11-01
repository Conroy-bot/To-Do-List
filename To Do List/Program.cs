using Microsoft.VisualBasic.FileIO;
using System;
using System.Diagnostics;
using System.Security.AccessControl;
using System.Transactions;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Security.Cryptography;
using Microsoft.VisualBasic;


namespace To_Do_List
{
    internal class Program
    {
        
        //static bool completedUsedLast = false;
        //static bool deletedUsedLast = false;
        

        static void Main(string[] args)
        {
            Queue<string> tasks = new();
            Queue<string> completedTask = new();
            Stack<string> stack = new();
            Stack<char> trackStack = new();


            string task1 = "Clean Bathroom";
            string task2 = "Clean Kitchen";
            string task3 = "Wash Clothes";
            tasks.Enqueue(task1);
            tasks.Enqueue(task2);
            tasks.Enqueue(task3);

            while (true)
            {

                Console.WriteLine("To Do List");
                Console.WriteLine("""
                    1. Add Task
                    2. View tasks
                    3. Exit
                    """);

                Console.Write("Choose an option: ");
                string? raw = Console.ReadLine();                      
                if (string.IsNullOrWhiteSpace(raw))
                {
                    Console.WriteLine("No input entered. Please choose an option.");
                    continue; // show menu again
                }

                if (!int.TryParse(raw, out int option))
                {
                    Console.WriteLine("Invalid input. Enter a number for the option.");
                    continue; // show menu again
                }

                try
                {
                    switch (option)
                    {
                        case 1:
                            AddTask(tasks);
                            break;

                        case 2:
                            ViewTasks(tasks,completedTask, stack,trackStack);
                            
                            break;

                        case 3:
                            Exit();
                            break;

                        default:
                            Console.WriteLine("Unknown option. Please choose 1, 2 or 3.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    Console.WriteLine("An error occurred: " + ex.Message);
                }

                
            }
        }

        static void Exit()
        {
            Console.WriteLine("Thank you for using the to do list\nNow exiting program");
            Environment.Exit(0);
        }

        
        static void AddTask(Queue<string> task)
        {
            Console.Write("Please input a task you need to do: ");
            string? toDo = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(toDo))
            {
                Console.WriteLine("No task entered.");
                Thread.Sleep(1000);
                Console.Clear();
                return;
            }
            Thread.Sleep(300);
            Console.Clear();
            Console.WriteLine("Added task: " + toDo);
            task.Enqueue(toDo);
        }

        static void ViewTasks(Queue<string> task , Queue<string> completedTask, Stack<string> stack, Stack<char> trackStack)
        {
            Console.Clear();
      
            while (true)
            {
                if (task.Count == 0 && completedTask.Count == 0 && stack.Count==0)
                {
                    Console.WriteLine("No tasks in the list.");
                    Thread.Sleep(1000);
                    Console.Clear();
                    return;
                }

                Thread.Sleep(500);
                

                Console.WriteLine("____________");
                Console.WriteLine("Pending tasks:");

                int i = 1;

                foreach (var t in task)
                {
                    Console.WriteLine($"{i++}. {t}");
                }

                int y = 1;

                Console.WriteLine("____________");
                Console.WriteLine("Completed Tasks");
                foreach (var c in completedTask)
                {
                    Console.WriteLine($"{y++}. {c}");
                }
                Console.WriteLine("____________");
                Console.WriteLine($"Tasks in stack: {stack.Count}");
                Console.WriteLine("____________");



                Console.WriteLine("""
                    1. Complete Task
                    2. Delete Task
                    3. Undo
                    4. Clear Stack
                    5. Back
                    """);

                try
                {
                    int option = Convert.ToInt32(Console.ReadLine());

                    switch (option)
                    {
                        case 1:
                            CompleteTask(task, completedTask, stack,trackStack);
                            Thread.Sleep(300);
                            
                            break;
                        case 2:
                            DeleteTask(task, stack,trackStack);
                            Thread.Sleep(700);
                            
                            break;
                        case 3:
                            UndoTask(task, completedTask, stack,trackStack);
                            Thread.Sleep(700);

                            break;

                        case 4:
                            Console.Clear();
                            ClearStack(stack);
                            Thread.Sleep(600);
                            
                            break;


                        case 5:
                            Thread.Sleep(300);
                            Console.Clear();
                            return;


                        default:
                            Console.WriteLine("Invalid option");
                            break;

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

            }
        }

        static void CompleteTask(Queue<string> task, Queue<string> completedTasks, Stack<string> stack, Stack<char> trackStack)
        {
            if (task.Count == 0 )
            {
                Console.WriteLine("No tasks to complete!");
                return;
            }
            string completedTask = task.Dequeue();
            stack.Push(completedTask);
            trackStack.Push('C');
            completedTasks.Enqueue(completedTask);


            Console.Clear();
            Console.WriteLine("Completed task: " + completedTask);

        }
        static void DeleteTask(Queue<string> task, Stack<string> stack, Stack<char> trackStack)
        {
           Queue<string> newQ = new Queue<string>();
            while (task.Count > 1)
            {
                var newTask=task.Dequeue();
                newQ.Enqueue(newTask);
            }

            var lastTask=task.Dequeue();
            stack.Push(lastTask);
            trackStack.Push('D');

            foreach (var item in newQ)
            {
                task.Enqueue(item);
            }

            // update shared flags
            deletedUsedLast = true;
            completedUsedLast = false;

            Console.Clear();
            Console.WriteLine("Task: " + lastTask + " removed");


        }

        static void UndoTask(Queue<string> task,Queue <string> completedtask, Stack<string> stack, Stack<char> trackStack)
        {
            if (stack.Count > 0)
            {

                if (trackStack.Peek() =='C')
                {
                    string undoneTask = stack.Peek();

                    if (completedtask.Count > 0)
                    {
                        completedtask.Dequeue();
                    }

                    var newQueue = new Queue<string>();
                    newQueue.Enqueue(undoneTask);

                    foreach (string item in task)
                    {
                        newQueue.Enqueue(item);
                    }


                    task.Clear();

                    foreach (string item in newQueue)
                    {
                        task.Enqueue(item);
                    }

                    stack.Pop();
                    trackStack.Pop();
                    Console.Clear();
                    Console.WriteLine("Undid task " + undoneTask);

                    
                
                }

                else if(trackStack.Peek() == 'D')
                
                {
                    string undoneTask = stack.Peek();         

                    var newQueue = new Queue<string>();

                    foreach (string item in task)
                    {
                        newQueue.Enqueue(item);
                    }

                    newQueue.Enqueue(undoneTask);

                    task.Clear();
                    foreach (string item in newQueue)
                    {
                        task.Enqueue(item);
                    }

                    stack.Pop();
                    trackStack.Pop();
                    Console.Clear();
                    Console.WriteLine("Undid task " + undoneTask);

                 

                }
            }
            else
            {
                Console.Clear() ;
                Console.WriteLine("No tasks in stack found.");
            }
            
        }

        static void ClearStack(Stack<string> stack)
        {
            if (stack.Count > 0)
            {
                Console.WriteLine("""
                Are you sure you want to clear the stack? 
                The stack contains the following tasks:
                """);

                foreach (string item in stack)
                {
                    Console.WriteLine(item);
                }

                Console.WriteLine("""
                1: Yes
                2 :No (Cancel)
                """);

                try
                {

                    int option = Convert.ToInt32(Console.ReadLine());

                    switch (option)
                    {
                        case 1:
                            stack.Clear();
                            Console.Clear();
                            Console.WriteLine("Stack Cleared.");

                            break;
                        case 2:
                            return;


                        default:
                            Console.WriteLine("Invalid option");
                            break;


                    }
                }
                catch (FormatException ex)

                {
                    Console.WriteLine(ex.ToString());
                }
            }
            else
            {
                Console.WriteLine("There are no items in the stack.");
                Thread.Sleep(400);
            }
        }
    }
}

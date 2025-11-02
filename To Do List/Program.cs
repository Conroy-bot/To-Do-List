using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml;


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

            Console.Clear();

            while (true)
            {
                if (tasks.Count == 0 && completedTask.Count == 0 && stack.Count == 0)
                {
                    Console.WriteLine("No tasks in the list.");
                    Thread.Sleep(1000);
                    Console.Clear();
                    return;
                }

                Thread.Sleep(500);

                Console.WriteLine("To Do List:");
                Console.WriteLine("____________");
                Console.WriteLine("Pending tasks:");

                int i = 1;

                foreach (var t in tasks)
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
                Console.WriteLine($"Undo's: {stack.Count}");
                Console.WriteLine("____________");



                Console.WriteLine("""
                    1. AddTask
                    2. Complete Task
                    3. Delete Task
                    4. Undo
                    5. Clear Undos
                    6. Back
                    Input choice here:
                    """);

                try
                {
                    int option = Convert.ToInt32(Console.ReadLine());

                    switch (option)
                    {
                        case 1:
                            Console.Clear();
                            AddTask(tasks, stack,trackStack);
                            break;
                        case 2:
                            CompleteTask(tasks, completedTask, stack, trackStack);
                            Thread.Sleep(300);

                            break;
                        case 3:
                            DeleteTask(tasks, stack, trackStack);
                            Thread.Sleep(700);

                            break;
                        case 4:
                            UndoTask(tasks, completedTask, stack, trackStack);
                            Thread.Sleep(700);

                            break;

                        case 5:
                            Console.Clear();
                            ClearStack(stack, trackStack);
                            Thread.Sleep(600);

                            break;


                        case 6:
                            Thread.Sleep(300);
                            Console.Clear();
                            Exit();
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

            static void Exit()
            {
                Console.WriteLine("Thank you for using the to do list\nNow exiting program");
                Environment.Exit(0);
            }


            static void AddTask(Queue<string> task, Stack<string> stack, Stack< char > trackStack)
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
                Console.WriteLine("Added task: " + toDo + "\n");
                stack.Push(toDo);
                trackStack.Push('A');
                task.Enqueue(toDo);
            }


            static void CompleteTask(Queue<string> task, Queue<string> completedTasks, Stack<string> stack, Stack<char> trackStack)
            {
                if (task.Count == 0)
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
                    var newTask = task.Dequeue();
                    newQ.Enqueue(newTask);
                }

                var lastTask = task.Dequeue();
                stack.Push(lastTask);
                trackStack.Push('D');

                foreach (var item in newQ)
                {
                    task.Enqueue(item);
                }



                Console.Clear();
                Console.WriteLine("Task: " + lastTask + " removed");


            }

            static void UndoTask(Queue<string> task, Queue<string> completedtask, Stack<string> stack, Stack<char> trackStack)
            {
                if (stack.Count > 0)
                {
                    switch (trackStack.Peek())
                    {
                        case 'C':
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
                            break;

                        case 'D':

                            undoneTask = stack.Peek();

                            newQueue = new Queue<string>();

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
                            break;


                        case 'A':
                            string addedTask = stack.Peek();

                            List<string> added = new();

                            newQueue = new Queue<string>();

                           foreach(string item in task)
                            {
                                added.Add(item);
                            }

                           added.RemoveAt(added.Count-1);

                            task.Clear();

                            foreach (string item in added)
                            {
                                task.Enqueue(item);
                            }

                            stack.Pop();
                            trackStack.Pop();
                            Console.Clear();
                            Console.WriteLine("Undid task " + addedTask);
                            break;



                        default:
                            Console.Clear();
                            Console.WriteLine("No tasks to undo.");
                            break;
                    }



                }
            }
        }

             static void ClearStack(Stack<string> stack, Stack<char> trackStack)
            {
                if (stack.Count > 0)
                {
                    Console.WriteLine("""
                Are you sure you want to clear your undos? 
                The stack contains the following tasks:
                """);

                List<char> characters = [.. trackStack];
                List<string> listOfTasks = [.. stack];
                Stack<string> shownTasks = new();

                Dictionary<char, string> dictionary = new Dictionary<char, string>();
                for (int i = 0;i<listOfTasks.Count;i++)
                {
                   
                    dictionary[characters[i]] = listOfTasks[i];
                    
                    if (dictionary.ContainsKey('A'))
                    {
                        string holder = dictionary[characters[i]].ToString();
                        
                        shownTasks.Push(holder+"- Added");
                    }
                    else if (dictionary.ContainsKey('D'))
                    {
                        string holder = dictionary[characters[i]].ToString();
                        
                        shownTasks.Push(holder + "- Deleted");
                    }

                    else if (dictionary.ContainsKey('C'))
                    {
                        string holder = dictionary[characters[i]].ToString();
                        
                        shownTasks.Push(holder + "- Completed");
                    }


                }



                int j = 1;

                foreach (string item in shownTasks)
                    {
                        Console.WriteLine($"{j++}. {item}");
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


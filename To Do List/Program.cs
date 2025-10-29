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
        static void Main(string[] args)
        {
            Queue<string> tasks = new Queue<string>();
            Queue<string> completedTask = new Queue<string>();
            Stack<string> stack = new Stack<string>();
            Console.WriteLine("To Do List");

            while (true)
            {
                
                
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
                            ViewTasks(tasks,completedTask, stack);
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
                return;
            }

            Console.WriteLine("Adding task: " + toDo);
            task.Enqueue(toDo);
        }

        static void ViewTasks(Queue<string> task , Queue<string> completedTask, Stack<string> stack)
        {
            if (task.Count == 0)
            {
                Console.WriteLine("No tasks in the list.");
                return;
            }

            Console.WriteLine("____________");
            Console.WriteLine("Pending tasks:");
            int i = 1;
            foreach (var t in task)
            {
                Console.WriteLine($"{i++}. {t}");
            }

            Console.WriteLine("____________");
            Console.WriteLine("Completed Tasks");
            foreach (var c in completedTask)
            {
                Console.WriteLine($"{i++}. {c}");
            }
            Console.WriteLine("____________");

            while (true)
            {
                Console.WriteLine("""
                    1. Complete Task
                    2. Delete Task?
                    3. Back
                    """);
                int option = Convert.ToInt32(Console.ReadLine());

                switch(option) 
                {
                    case 1:
                        CompleteTask(task,completedTask);
                        Thread.Sleep(1000);
                        Console.Clear();
                        break; 
                    case 2: 
                        DeleteTask(task, stack);
                        Thread.Sleep(1000);
                        Console.Clear();
                        break; 
                    case 3:
                        return;

                    default:
                        Console.WriteLine("Invalid option");
                        break;

                        
                }

            }
        }

        static void CompleteTask(Queue<string> task, Queue<string> completedTasks)
        {
            if (task.Count == 0)
            {
                Console.WriteLine("No tasks to complete!");
                return;
            }
            string completedTask = task.Dequeue();
            completedTasks.Enqueue(completedTask);
            Console.WriteLine("Completed task: " + completedTask);
        }
        static void DeleteTask(Queue<string> task, Stack<string> stack )
        {
            if (task.Count == 0)
            {
                Console.WriteLine("No tasks to delete!");
                return;
            }
            string completedTask = task.Dequeue();
            stack.Push(completedTask);
            Console.WriteLine("Deleted Task: " + completedTask);

        }

        public void UndoTask(Queue<string> task, Stack<string> stack)
        {
            // left unmodified — implement if you want an "Undo" option later
        }
    }
}

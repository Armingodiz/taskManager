﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Data.SqlClient;
using taskManager.DB;

namespace taskManager
{
	class TaskManager
	{
		public Dictionary<Task, Dictionary<int, User>> assignments;
		public Dictionary<int, User> users;
		public Dictionary<string, Task> tasks;
		private Model1 db;
		private int idCounter;
		public TaskManager() 
		{
			this.assignments = new Dictionary<Task, Dictionary<int, User>>();
			this.users = new Dictionary<int, User>();
			this.tasks = new Dictionary<string, Task>();
			this.db = new Model1();
			this.idCounter = this.db.users.Count() +1 ;
		}
		public void showAssignments() 
		{
			foreach (var key in assignments)
			{
				Console.WriteLine("Task ==> " + key.Key.getInfo());
				Console.WriteLine("USERS WORKING ON IT : ");
				foreach (var user in assignments[key.Key]) 
				{
					Console.WriteLine("USER == > " + user.Value.getInfo());
				}
				Console.WriteLine("######################################################");
			}
		}
		public void AddTask()
		{
			Console.WriteLine("Enter task name :");
			string name = Console.ReadLine();
			Task task2 = new Task(name);
			tasks.Add(task2.name , task2);
			Dictionary<int, User> newUsers = new Dictionary<int, User>();
			assignments.Add(task2, newUsers);
			task new_task = new task();
			new_task.name = task2.name;
			new_task.date = task2.date;
			this.db.tasks.Add(new_task);
			this.db.SaveChanges();
		}

		public void AddUser()
		{
			Console.WriteLine("Enter user name :");
			string name = Console.ReadLine();
			User user2 = new User(name, idCounter);
			idCounter += 1;
			users.Add(user2.id, user2);
			user new_user = new user();
			new_user.name = user2.name;
			new_user.ID = user2.id;
			this.db.users.Add(new_user);
			this.db.SaveChanges();
		}
		public User ChoseUser() 
		{
			User choosenUser;
			Console.WriteLine("USERS : ");
			int counter = 0;
			foreach (var key in users) { Console.WriteLine(counter + " ) " + key.Value.name + "  " + key.Value.id);counter++; }
			Console.WriteLine("Enter user id :");
			int userId = Convert.ToInt32(Console.ReadLine());
			bool check = users.TryGetValue(userId,out choosenUser);
			if (!check)
            {
				Console.WriteLine("no user with this id , try again !");
				return this.ChoseUser();
            }
			return choosenUser;
		}
		public Task ChoseTask()
		{
			Task choosenTask = null;
			Console.WriteLine("Tasks : ");
			int counter = 0;
			foreach (var key in tasks) { Console.WriteLine(counter + " ) " + key.Value.name + "  " + key.Value.date);counter++; }
			Console.WriteLine("Enter Task number :");
			int taskNumber = Convert.ToInt32(Console.ReadLine());
			int counter2 = 0;
			foreach (var key in tasks) 
			{
				if(counter2 == taskNumber)
                {
					choosenTask = key.Value;
					break;
                }
				counter2++;
			}
			if (choosenTask == null)
			{
				Console.WriteLine("no task with this number , try again !");
				return this.ChoseTask();
			}
			return choosenTask;
		}

		public void Assign()
		{
			User user = this.ChoseUser();
			Task task = this.ChoseTask();
			bool check = assignments[task].TryGetValue(user.id,out User user1);
            if (check)
            {
				Console.WriteLine("task already assigned to this user !");
            }else
            {
				assignments[task].Add(user.id, user);
				assignment new_assignment = new assignment();
				new_assignment.taskName = task.name;
				new_assignment.userId = user.id;
				this.db.assignments.Add(new_assignment);
				this.db.SaveChanges();
			}
		}

		public void removeUserFromTask() 
		{
			this.showAssignments();
			User user = this.ChoseUser();
			Task task = this.ChoseTask();
			bool check = assignments[task].TryGetValue(user.id, out User user1);
            if (check)
            {
				assignments[task].Remove(user.id);
				assignment new_assignment = new assignment();
				foreach (assignment tmp in this.db.assignments)
				{
					if (tmp.taskName.Replace(" ", "") == task.name && tmp.userId == user.id)
						new_assignment = tmp;
				}
				this.db.assignments.Remove(new_assignment);
				this.db.SaveChanges();
            }
            else
            {
				Console.WriteLine("there is no assignment with this information !!");
            }

		}

		public void showInfo()
		{
			Console.WriteLine("USERS : ");
			foreach (var key in users) { key.Value.printUser(); }
			Console.WriteLine("#####################");
			Console.WriteLine("TASKS : ");
			foreach (var key in tasks) { key.Value.printTask(); Console.WriteLine("@"+key.Value.name+"@"); }

			Console.WriteLine("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
		}

		public void loadInfo() 
		{
			List<user> dbUsers;
			dbUsers = this.db.users.ToList();
            foreach (user tmp in dbUsers) 
			{
	            User new_user = new User(tmp.name.Replace(" ",""),tmp.ID);
				users.Add(tmp.ID, new_user);
			}
			List<task> dbTasks;
			dbTasks = this.db.tasks.ToList();
			foreach(task tmp in dbTasks) 
			{
				string taskName = tmp.name.Replace(" ", "");
				Task new_task = new Task(taskName);
				new_task.date = tmp.date;
				tasks.Add(new_task.name, new_task);
				Dictionary<int, User> newUsers = new Dictionary<int, User>();
				assignments.Add(new_task, newUsers);
			}
			List<assignment> dbAsssignments;
			dbAsssignments = this.db.assignments.ToList();
			foreach (assignment tmp in dbAsssignments)
			{
				string taskName = tmp.taskName.Replace(" ", "");
				int userId = tmp.userId;
				assignments[tasks[taskName]].Add(userId, users[userId]);
			}
        }

		public void start()
		{
			Console.WriteLine(" 1 ) Add Task \t 2 ) Add User \t 3 ) Assign Task to user \t 4 ) Remove User from task \t 5 ) show assignments \t 6 ) show users and tasks \t 7 ) save and Exit !");
			int input = Convert.ToInt32(Console.ReadLine());
			switch (input) 
			{
				case 1:
					AddTask();
					start();
					break;
				case 2:
					AddUser();
					start();
					break;
				case 3:
					Assign();
					start();
					break;
				case 4:
					removeUserFromTask();
					start();
					break;
				case 5:
					showAssignments();
					start();
					break;
				case 6:
					showInfo();
					start();
					break;
				case 7:
					// todo
					break;
				default:
					Console.WriteLine("Invalid input !!");
					start();
					break;
			}
		}
		static void Main(string[] args)
		{
			TaskManager manager = new TaskManager();
			manager.loadInfo();
			manager.start();
		}
	}
	public class Task
	{
		public string name;
		public string date;
		public Task(string name) {
			this.name = name;
			this.date = DateTime.Now.ToString();
		}
		public void printTask() {
			Console.WriteLine("name : " + this.name + " \t" + "date : " + this.date);
		}
		public string getInfo() 
		{
			return "name : " + this.name + " \t" + "date : " + this.date;
		}

	}
	public class User
	{
		public string name;
		public int id;
		public User(string name , int id)
		{
			this.name = name;
			this.id = id;
		}
		public void printUser()
		{
			Console.WriteLine("name : " + this.name + " \t" + "id : " + this.id);
		}
		public string getInfo()
		{
			return "name : " + this.name + " \t" + "id : " + this.id;
		}
	}
}

using System;
using System.Collections.Generic;



namespace SerpentsLabyrinth
{

	
	public class Game
	{
		private bool isRunning, restartGame;
		private int score;
		
		// ⭐ Moved to class-level fields so they don't disappear after setup
		private Grid? grid; 
		private string[] players = new string[4];

		public Game(){
			isRunning = false;
			restartGame = false;
			score = 0;
		}

		public void Start(){	
			isRunning = true;

			// 🚀 Trigger the setup sequence when the game starts
			AddPlayers();
			CreateGrid(); 

			// Next steps would go here (e.g., RunGameLoop();)
		}

		public void Restart(){
			restartGame = true;
			isRunning = false;
			score = 0;
			Start(); // Restarting should launch setup again
		}

		private void AddPlayers(){
			Console.WriteLine("--- Setup Players ---");
			Console.WriteLine("Between 2-4 players:");

			for(int i = 0; i < players.Length; i++){
				Console.WriteLine($"Enter player {i + 1} name (or leave blank to skip):");
				
				// Added '?' to clear the CS8600 null warning
				string? playerName = Console.ReadLine();

				if(!string.IsNullOrEmpty(playerName)){
					players[i] = playerName;
				}
			}
		}

		// Removed 'int size' parameter since the method asks the user for the size anyway
		private void CreateGrid(){

			Console.WriteLine("\nWelcome to Serpent's Labyrinth!");
			
			Console.WriteLine("1. 9x9");
			Console.WriteLine("2. 10x10");
			Console.WriteLine("3. 11x11");
			Console.WriteLine("4. 12x12");

			int option = Convert.ToInt32(Console.ReadLine());

			switch(option){
				case 1:
					grid = new Grid(9);
					break;
				case 2:
					grid = new Grid(10);
					break;
				case 3:
					grid = new Grid(11);
					break;
				case 4:
					grid = new Grid(12);
					break;
				default:
					Console.WriteLine("Invalid option. Please choose a valid grid size.");
					CreateGrid(); // Ask again if they input a bad number
					break;
			}

			Console.WriteLine("Grid initialized successfully!");
		}

		public static void Main(){
    		Game game = new Game();
    		game.Start();
		}
	}
}
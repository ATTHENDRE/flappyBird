using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace flappyBird
{
	public partial class MainWindow : Window
	{
		DispatcherTimer timer = new DispatcherTimer();

		int gravity = 8;
		int score = 0;
		bool gameOver = false;

		Random rnd = new Random();

		public MainWindow()
		{
			InitializeComponent();

			timer.Interval = TimeSpan.FromMilliseconds(20);
			timer.Tick += GameLoop;

			ShowMenu(); 
		}

		

		void ShowMenu()
		{
			MenuScene.Visibility = Visibility.Visible;
			Canv.Visibility = Visibility.Hidden;


			timer.Stop();
		}

		void ShowGame()
		{
			MenuScene.Visibility = Visibility.Hidden;
			Canv.Visibility = Visibility.Visible;
			

			Canv.Focus();
			StartGame();
		}

		void ShowCredits()
		{
			MenuScene.Visibility = Visibility.Hidden;
			Canv.Visibility = Visibility.Hidden;
			

			timer.Stop();
		}

		

		private void GameLoop(object sender, EventArgs e)
		{
			scoreLabel.Content = "Score: " + score;

			Canvas.SetTop(bird, Canvas.GetTop(bird) + gravity);

			Rect birdHitBox = new Rect(
				Canvas.GetLeft(bird),
				Canvas.GetTop(bird),
				bird.Width,
				bird.Height
			);

			if (Canvas.GetTop(bird) < -10 || Canvas.GetTop(bird) > 440)
			{
				EndGame();
			}

			foreach (var obj in Canv.Children)
			{
				Image img = obj as Image;
				if (img == null) continue;

				// csövek
				if ((string)img.Tag == "pipe")
				{
					Canvas.SetLeft(img, Canvas.GetLeft(img) - 5);

					if (Canvas.GetLeft(img) < -80)
					{
						Canvas.SetLeft(img, 800);
						score++;

						if (Canvas.GetTop(img) < 0)
							Canvas.SetTop(img, -rnd.Next(200, 350));
						else
							Canvas.SetTop(img, rnd.Next(200, 350));
					}

					Rect pipeHitBox = new Rect(
						Canvas.GetLeft(img),
						Canvas.GetTop(img),
						img.Width,
						img.Height
					);

					if (birdHitBox.IntersectsWith(pipeHitBox))
					{
						EndGame();
					}
				}

				// felhők
				if ((string)img.Tag == "cloud")
				{
					Canvas.SetLeft(img, Canvas.GetLeft(img) - 1);

					if (Canvas.GetLeft(img) < -250)
					{
						Canvas.SetLeft(img, 550);
					}
				}
			}
		}

		

		private void KeyIsDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Space && !gameOver)
			{
				gravity = -8;
				bird.RenderTransform = new RotateTransform(-20, bird.Width / 2, bird.Height / 2);
			}

			if (e.Key == Key.Enter && gameOver)
			{
				ShowMenu(); 
			}
		}

		private void KeyIsUp(object sender, KeyEventArgs e)
		{
			gravity = 8;
			bird.RenderTransform = new RotateTransform(5, bird.Width / 2, bird.Height / 2);
		}

		

		private void StartGame()
		{
			score = 0;
			gameOver = false;
			gravity = 8;

			Canvas.SetTop(bird, 190);

			int x = 400;

			foreach (var obj in Canv.Children)
			{
				Image img = obj as Image;
				if (img != null && (string)img.Tag == "pipe")
				{
					Canvas.SetLeft(img, x);
					x += 300;
				}
			}

			scoreLabel.Content = "Score: 0";
			timer.Start();
		}

		private void EndGame()
		{
			timer.Stop();
			gameOver = true;
			scoreLabel.Content = "Score: " + score + "  Játék vége! (ENTER)";
		}

		

		private void StartGame_Click(object sender, RoutedEventArgs e)
		{
			ShowGame();
		}

		private void Credits_Click(object sender, RoutedEventArgs e)
		{
			ShowCredits();
		}

		private void BackToMenu_Click(object sender, RoutedEventArgs e)
		{
			ShowMenu();
		}

		private void Exit_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}
	}
}

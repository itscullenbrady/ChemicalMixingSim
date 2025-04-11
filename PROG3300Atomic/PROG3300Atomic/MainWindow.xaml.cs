using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace PROG3300Atomic
{
    public partial class MainWindow : Window
    {
        private List<string> chemicals = new List<string>();
        private string dbPath = "Data Source=sqlite.db";
        private string OpenBabelPath;
        private FileSystemWatcher watcher;

        public MainWindow()
        {
            InitializeComponent();
          
            OpenBabelPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "OpenBabel-3.1.1", "obabel.exe");
          
            if (!File.Exists(OpenBabelPath))
            {
                MessageBox.Show($"Open Babel not found at: {OpenBabelPath}\nPlease install Open Babel in the application directory.",
                               "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            LoadChemicals();
            PopulateComboBox();
            comboBox1.SelectionChanged += ComboBox_SelectionChanged;
            comboBox2.SelectionChanged += ComboBox_SelectionChanged;
            Console.WriteLine($"Application directory: {AppDomain.CurrentDomain.BaseDirectory}");
            Console.WriteLine($"Open Babel path: {OpenBabelPath}");

            string molDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mol");
            Directory.CreateDirectory(molDirectory);

            watcher = new FileSystemWatcher(molDirectory);
            watcher.Created += (s, e) => Console.WriteLine($"File created: {e.FullPath}");
            watcher.EnableRaisingEvents = true;
        }

        

        private void LoadChemicals()
        {
            try
            {
                using (var connection = new SQLiteConnection(dbPath))
                {
                    connection.Open();
                    string query = "SELECT Name FROM Chemicals";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string chemicalName = reader["Name"].ToString();
                                chemicals.Add(chemicalName);
                                Console.WriteLine($"Loaded chemical: {chemicalName}");
                            }
                        }
                    }
                }

                if (chemicals.Count == 0)
                {
                    Console.WriteLine("No chemicals found in the database.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading chemicals: {ex.Message}");
            }
        }

        private void PopulateComboBox()
        {
            foreach (var chemical in chemicals)
            {
                comboBox1.Items.Add(new ComboBoxItem { Content = chemical });
                comboBox2.Items.Add(new ComboBoxItem { Content = chemical });
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                string chemical1 = ((ComboBoxItem)comboBox1.SelectedItem).Content.ToString();
                DisplayDescription(chemical1, textBlockDescription1);
                DisplayMoleculeImage(chemical1, imageLeft);
            }

            if (comboBox2.SelectedItem != null)
            {
                string chemical2 = ((ComboBoxItem)comboBox2.SelectedItem).Content.ToString();
                DisplayDescription(chemical2, textBlockDescription2);
                DisplayMoleculeImage(chemical2, imageRight);
            }

            if (comboBox1.SelectedItem != null && comboBox2.SelectedItem != null)
            {
                string chemical1 = ((ComboBoxItem)comboBox1.SelectedItem).Content.ToString();
                string chemical2 = ((ComboBoxItem)comboBox2.SelectedItem).Content.ToString();
                DisplayReaction(chemical1, chemical2);
            }
        }

        private void DisplayDescription(string chemical, TextBlock textBlock)
        {
            try
            {
                using (var connection = new SQLiteConnection(dbPath))
                {
                    connection.Open();
                    string query = "SELECT Description FROM Chemicals WHERE Name = @chemical";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@chemical", chemical);
                        var result = command.ExecuteScalar();
                        textBlock.Text = result?.ToString() ?? "Description not found.";
                    }
                }
            }
            catch (Exception ex)
            {
                textBlock.Text = $"Error loading description: {ex.Message}";
            }
        }

        private (bool success, string message) GenerateMoleculeImage(string smiles, string imagePath)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(imagePath));

                if (!File.Exists(OpenBabelPath))
                {
                    return (false, $"Open Babel not found at: {OpenBabelPath}");
                }

                var startInfo = new ProcessStartInfo
                {
                    FileName = OpenBabelPath,
                    Arguments = $"-:\"{smiles}\" -O \"{imagePath}\" --gen2D -xu 500",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    WorkingDirectory = Path.GetDirectoryName(OpenBabelPath),
                    StandardOutputEncoding = System.Text.Encoding.UTF8,
                    StandardErrorEncoding = System.Text.Encoding.UTF8
                };

                using (var process = new Process())
                {
                    process.StartInfo = startInfo;

                    // Set up output handlers
                    StringBuilder output = new StringBuilder();
                    StringBuilder error = new StringBuilder();

                    process.OutputDataReceived += (sender, args) => output.AppendLine(args.Data);
                    process.ErrorDataReceived += (sender, args) => error.AppendLine(args.Data);

                    process.Start();

                    // Begin async reading
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    process.WaitForExit();

                    string result = output.ToString();
                    string err = error.ToString();

                    if (process.ExitCode != 0 || !File.Exists(imagePath))
                    {
                        return (false, $"Open Babel failed (Exit {process.ExitCode}): {err}");
                    }

                    return (true, "Success");
                }
            }
            catch (Exception ex)
            {
                return (false, $"Error: {ex.Message}");
            }
        }

        private void DisplayMoleculeImage(string chemicalName, Image imageControl)
        {
            try
            {
                string smiles = GetSmilesFromDatabase(chemicalName);
                if (string.IsNullOrEmpty(smiles))
                {
                    Console.WriteLine($"No SMILES found for chemical: {chemicalName}");
                    SetPlaceholderImage(imageControl);
                    return;
                }

                string molDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mol");
                string imagePath = Path.Combine(molDirectory, $"{chemicalName}.png");

                Console.WriteLine($"Attempting to generate image for {chemicalName}...");
                Console.WriteLine($"SMILES: {smiles}");
                Console.WriteLine($"Target path: {imagePath}");

                // Clear previous image
                imageControl.Dispatcher.Invoke(() => imageControl.Source = null);

                var (success, message) = GenerateMoleculeImage(smiles, imagePath);
                Console.WriteLine(message);

                if (success)
                {
                    Console.WriteLine($"Image generated successfully at: {imagePath}");
                    Console.WriteLine($"File exists: {File.Exists(imagePath)}");
                    Console.WriteLine($"File size: {new FileInfo(imagePath).Length} bytes");

                    LoadImageWithRetry(imagePath, imageControl);
                }
                else
                {
                    Console.WriteLine($"Failed to generate image: {message}");
                    SetPlaceholderImage(imageControl);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error displaying molecule: {ex.Message}");
                SetPlaceholderImage(imageControl);
            }
        }

        private void LoadImageWithRetry(string imagePath, Image imageControl, int retryCount = 3, int delayMs = 100)
        {
            for (int i = 0; i < retryCount; i++)
            {
                try
                {
                    using (var stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                    {
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.StreamSource = stream;
                        bitmap.EndInit();
                        bitmap.Freeze();

                        imageControl.Dispatcher.Invoke(() => imageControl.Source = bitmap);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading image (attempt {i + 1}): {ex.Message}");
                    if (i < retryCount - 1)
                    {
                        System.Threading.Thread.Sleep(delayMs);
                    }
                }
            }
            SetPlaceholderImage(imageControl);
        }

        private void SetPlaceholderImage(Image imageControl)
        {
            try
            {
                string placeholderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mol", "placeholder.png");

                if (File.Exists(placeholderPath))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.UriSource = new Uri(placeholderPath);
                    bitmap.EndInit();
                    bitmap.Freeze();

                    imageControl.Dispatcher.Invoke(() => imageControl.Source = bitmap);
                }
                else
                {
                    Console.WriteLine($"Placeholder not found at: {placeholderPath}");
                    imageControl.Dispatcher.Invoke(() => imageControl.Source = null);

                    // Create a blank placeholder if missing
                    var blankBitmap = new BitmapImage();
                    blankBitmap.BeginInit();
                    blankBitmap.UriSource = new Uri("pack://application:,,,/PROG3300Atomic;component/blank.png");
                    blankBitmap.EndInit();
                    imageControl.Dispatcher.Invoke(() => imageControl.Source = blankBitmap);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading placeholder: {ex.Message}");
                imageControl.Dispatcher.Invoke(() => imageControl.Source = null);
            }
        }

        private string GetSmilesFromDatabase(string chemicalName)
        {
            try
            {
                using (var connection = new SQLiteConnection(dbPath))
                {
                    connection.Open();
                    string query = "SELECT Smiles FROM Chemicals WHERE Name = @name";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@name", chemicalName);
                        return command.ExecuteScalar()?.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting SMILES: {ex.Message}");
                return null;
            }
        }

        private void DisplayReaction(string chemical1, string chemical2)
        {
            try
            {
                using (var connection = new SQLiteConnection(dbPath))
                {
                    connection.Open();
                    string query = @"SELECT Products, Description, Synopsis, products_smiles FROM Reactions 
                             WHERE Reactant1ID = (SELECT ChemicalID FROM Chemicals WHERE Name = @chemical1) 
                             AND Reactant2ID = (SELECT ChemicalID FROM Chemicals WHERE Name = @chemical2)";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@chemical1", chemical1);
                        command.Parameters.AddWithValue("@chemical2", chemical2);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                textBlockReaction.Text = $"Products: {reader["Products"]}\nDescription: {reader["Description"]}\nSynopsis: {reader["Synopsis"]}";

                                string productsSmiles = reader["products_smiles"].ToString();
                                DisplayMoleculeImageFromSmiles(productsSmiles, imageBottom);
                            }
                            else
                            {
                                textBlockReaction.Text = "No reaction found.";
                                imageBottom.Source = null; // Clear the image if no reaction is found
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                textBlockReaction.Text = $"Error loading reaction: {ex.Message}";
                imageBottom.Source = null; // Clear the image in case of error
            }
        }

        private void DisplayMoleculeImageFromSmiles(string smiles, Image imageControl)
        {
            try
            {
                string molDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mol");
                string imagePath = Path.Combine(molDirectory, $"reaction.png");

                Console.WriteLine($"Attempting to generate image for reaction...");
                Console.WriteLine($"SMILES: {smiles}");
                Console.WriteLine($"Target path: {imagePath}");

                // Clear previous image
                imageControl.Dispatcher.Invoke(() => imageControl.Source = null);

                var (success, message) = GenerateMoleculeImage(smiles, imagePath);
                Console.WriteLine(message);

                if (success)
                {
                    Console.WriteLine($"Image generated successfully at: {imagePath}");
                    Console.WriteLine($"File exists: {File.Exists(imagePath)}");
                    Console.WriteLine($"File size: {new FileInfo(imagePath).Length} bytes");

                    LoadImageWithRetry(imagePath, imageControl);
                }
                else
                {
                    Console.WriteLine($"Failed to generate image: {message}");
                    SetPlaceholderImage(imageControl);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error displaying molecule: {ex.Message}");
                SetPlaceholderImage(imageControl);
            }
        }

    }
}

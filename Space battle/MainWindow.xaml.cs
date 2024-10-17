using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Space_battle
{
    // DispatcherTimer: Controla o intervalo do game loop, que é o ciclo onde o estado do jogo é atualizado continuamente. 

    // Canvas: Área de desenho onde os elementos visuais (nave, inimigos, tiros) são desenhados.

    // Rect: Representa uma área de colisão usada para detectar interações(colisões) entre elementos como a nave, os inimigos e os tiros.

    // Movimentação do jogador: Controlada pelas teclas de seta para mover à esquerda e à direita, enquanto a barra de espaço dispara tiros.

    // Colisões: Verificadas usando Rect.IntersectsWith(), que determina se dois objetos se sobrepõem, resultando na remoção de tiros e inimigos quando atingidos.

    // Gerador de inimigos: Utiliza o método MakeEnemies() para criar inimigos em posições aleatórias com sprites variados, aumentando a dificuldade do jogo conforme o tempo passa.

    public partial class MainWindow : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer();  // Timer do jogo que controla a taxa de atualização do game loop

        bool moveLeft, moveRight;   // Flags para controle do movimento do jogador

        List<Rectangle> itemRemover = new List<Rectangle>();    // Lista que armazena itens (inimigos e balas) a serem removidos do canvas

        Random rand = new Random(); // Gerador de números aleatórios para posicionamento dos inimigos

        // Contadores e variáveis de controle do jogo
        int enemySpriteCounter = 0;
        int enemyCounter = 100;
        int playerSpeed = 10;
        int score = 0;
        int limit = 50;
        int damage = 0;
        int enemySpeed = 8;

        Rect playerHitBox;  // Área de colisão da nave do jogador

        public MainWindow()
        {
            InitializeComponent();

            // Configura o timer do jogo para disparar a cada 20 milissegundos
            gameTimer.Interval = TimeSpan.FromMilliseconds(20); 
            gameTimer.Tick += GameLoop;
            gameTimer.Start();

            MyCanvas.Focus(); // Define o foco para capturar eventos de teclado

            // Configura o plano de fundo do jogo com uma imagem
            ImageBrush bg = new ImageBrush();
            bg.ImageSource = new BitmapImage(new Uri("pack://application:,,,/imagens/fundo.png"));
            bg.TileMode = TileMode.Tile;    // Modo de repetição do fundo
            bg.Viewport = new Rect(0,0, 0.15, 0.15);     // Ajusta o tamanho do tile
            bg.ViewportUnits = BrushMappingMode.RelativeToBoundingBox;
            MyCanvas.Background = bg;    // Define o fundo no canvas do jogo

            // Configura a imagem da nave do jogador
            ImageBrush playerImage = new ImageBrush();
            playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/imagens/Nave principal.png"));
            player.Fill = playerImage;   // Define a imagem da nave

        }

        // Função que representa o loop principal do jogo (executada a cada tick do gameTimer)
        private void GameLoop(object sender, EventArgs e)
        {
            // Atualiza a área de colisão do jogador com base na posição atual da nave
            playerHitBox = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width, player.Height);

            enemyCounter -= 1;  // Reduz o contador de inimigos

            // Atualiza a pontuação e o dano na interface
            scoreText.Content = "Score: " + score;
            damageText.Content = "Damage: " + damage;

            // Gera novos inimigos se o contador chegar a zero
            if (enemyCounter < 0)
            {
                MakeEnemies();
                enemyCounter = limit;   // Reinicia o contador
            }

            // Movimenta a nave para a esquerda, se permitido
            else if (moveLeft == true && Canvas.GetLeft(player) > 0)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) - playerSpeed);
            }

            // Movimenta a nave para a direita, se permitido
            else if (moveRight == true && Canvas.GetLeft(player) + 90 < Application.Current.MainWindow.Width)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) + playerSpeed);
            }

            // Verifica todos os retângulos no canvas (balas e inimigos)
            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
                // Processa os tiros do jogador (balas)
                if (x is Rectangle && (string)x.Tag == "bullet")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) - 20);    // Movimenta a bala para cima   

                    Rect bulletHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    // Remove a bala se ela sair da tela
                    if (Canvas.GetTop(x) < 10)
                    {
                        itemRemover.Add(x);
                    }

                    // Verifica colisão entre a bala e os inimigos
                    foreach (var y in MyCanvas.Children.OfType<Rectangle>())
                    {
                        if(y is Rectangle &&  (string)y.Tag == "enemy")
                        {
                            Rect enemyHit = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);

                            // Se a bala atingir o inimigo, ambos são removidos
                            if (bulletHitBox.IntersectsWith(enemyHit))
                            {
                                itemRemover.Add(x);
                                itemRemover.Add(y);
                                score++;
                            }
                        }
                    }

                }

                // Processa os inimigos
                else if (x is Rectangle && (string)x.Tag == "enemy")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) + enemySpeed);    // Movimenta o inimigo para baixo

                    // Se o inimigo sair da tela, aumenta o dano e o remove
                    if (Canvas.GetTop(x) > 750)
                    {
                        itemRemover.Add(x);
                        damage += 10;
                    }

                    Rect enemyHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    // Verifica colisão entre o jogador e os inimigos
                    if (playerHitBox.IntersectsWith(enemyHitBox))
                    {
                        itemRemover.Add(x);
                        damage += 5;
                    }

                }
            }

            // Remove todos os itens marcados para remoção
            foreach (Rectangle i in itemRemover)
            {
                MyCanvas.Children.Remove(i);
            }

            // Aumenta a dificuldade se o jogador ultrapassar 5 pontos
            if (score > 5)
            {
                limit = 20;
                enemySpeed = 13;
            }

            // Fim do jogo se o dano ultrapassar 99
            if (damage > 99)
            {
                gameTimer.Stop();    // Para o jogo
                damageText.Content = "Damage: 100";
                damageText.Foreground = Brushes.Red;
                MessageBox.Show("Captain you have destroyed " + score + " Aliens Ships" + Environment.NewLine + "Press Ok to Play Again", "Game Over: ");

                // Reinicia o jogo
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();
            }

        }

        // Evento de tecla pressionada (controla o movimento da nave)
        private void OnKeyDomn(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                moveLeft = true;     // Ativa movimento para a esquerda
            }
            else if (e.Key == Key.Right)
            {
                moveRight = true;    // Ativa movimento para a direita
            }
        }

        // Evento de tecla liberada (interrompe o movimento da nave)
        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                moveLeft = false;   // Para o movimento para a esquerda
            }
            else if (e.Key == Key.Right)
            {
                moveRight = false;  // Para o movimento para a direita
            }
            else if (e.Key == Key.Space)
            {
                // Cria uma nova bala e adiciona ao canvas, acionada com o espaço
                Rectangle newBullet = new Rectangle()
                {
                    Tag = "bullet",
                    Height = 20,
                    Width = 5,
                    Fill = Brushes.White,
                    Stroke = Brushes.Red,
                };

                // Define a posição inicial da bala
                Canvas.SetLeft(newBullet, Canvas.GetLeft(player) + player.Width / 2);
                Canvas.SetTop(newBullet, Canvas.GetTop(player) - newBullet.Height);

                MyCanvas.Children.Add(newBullet);   // Adiciona a bala ao canvas

            }

        }

        // Função que gera novos inimigos de forma aleatória
        private void MakeEnemies()
        {
            ImageBrush enemySprite = new ImageBrush(); // Define o sprite do inimigo

            enemySpriteCounter = rand.Next(1, 4);    // Escolhe aleatoriamente entre 4 tipos de inimigos

            switch (enemySpriteCounter)
            {
                case 1:
                    enemySprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/imagens/Nave inimiga 1.png"));
                    break;
                case 2:
                    enemySprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/imagens/Nave inimiga 2.png"));
                    break;
                case 3:
                    enemySprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/imagens/Nave inimiga 3.png"));
                    break;
                case 4:
                    enemySprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/imagens/Nave boss.png"));
                    break;
            }

            // Cria um novo retângulo que representa o inimigo
            Rectangle newEnemy = new Rectangle()
            {
                Tag = "enemy",
                Height = 50,
                Width = 56,
                Fill = enemySprite
            };

            // Define a posição inicial do inimigo fora da tela, no topo
            Canvas.SetTop(newEnemy, -100);

            // Define uma posição horizontal aleatória dentro do canvas
            Canvas.SetLeft(newEnemy, rand.Next(30, 430));

            // Adiciona o inimigo ao canvas
            MyCanvas.Children.Add(newEnemy);

        }
    }
}

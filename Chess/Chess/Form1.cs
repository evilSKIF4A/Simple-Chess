using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


// Create by SKIF4A -> https://github.com/evilSKIF4A


namespace Chess
{
    public partial class Form1 : Form
    {
        public Image Sprite;
        public int Player = 1;
        public Button[,] butts = new Button[8, 8]; //массив кнопок, чтобы их использовать как фигуры
        public Form1()
        {
            InitializeComponent();
            // загрузка спрайтов
            // @"E:\\Основы программирования\\Курсовая №1\\Sprites\\chess.png"
            Sprite = new Bitmap(@"Sprites\\chess.png");
            Image figure = new Bitmap(50, 50);
            Graphics gr = Graphics.FromImage(figure);
            Start();
        }
        public void Start()
        {
            map = new int[8, 8] // координаты фигур
            {
                {25,24,23,22,21,23,24,25 },
                {26,26,26,26,26,26,26,26 },
                {0,0,0,0,0,0,0,0 },
                {0,0,0,0,0,0,0,0 },
                {0,0,0,0,0,0,0,0 },
                {0,0,0,0,0,0,0,0 },
                {16,16,16,16,16,16,16,16 },
                {15,14,13,12,11,13,14,15 },
            };
            Player = 1;
            Creatmap();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // Не нужно
        }
        public int[,] map = new int[8, 8] // координаты фигур
        {
                {25,24,23,22,21,23,24,25 },
                {26,26,26,26,26,26,26,26 },
                {0,0,0,0,0,0,0,0 },
                {0,0,0,0,0,0,0,0 },
                {0,0,0,0,0,0,0,0 },
                {0,0,0,0,0,0,0,0 },
                {16,16,16,16,16,16,16,16 },
                {15,14,13,12,11,13,14,15 },
        };

        public void Creatmap()
        {
            map = new int[8, 8] // координаты фигур
            {
                {25,24,23,22,21,23,24,25 },
                {26,26,26,26,26,26,26,26 },
                {0,0,0,0,0,0,0,0 },
                {0,0,0,0,0,0,0,0 },
                {0,0,0,0,0,0,0,0 },
                {0,0,0,0,0,0,0,0 },
                {16,16,16,16,16,16,16,16 },
                {15,14,13,12,11,13,14,15 },
            };

            // создание поля
            int fl = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    butts[i, j] = new Button();
                    Button cell = new Button();
                    cell.Size = new Size(50, 50);
                    cell.Location = new Point(j * 50, i * 50);
                    if (fl == 0)
                    {
                        cell.BackColor = Color.White;
                        if (j != 7)
                        {
                            fl = 1;
                        }
                    }
                    else
                    {
                        cell.BackColor = Color.Brown;
                        if (j != 7)
                        {
                            fl = 0;
                        }
                    }

                    switch (map[i, j] / 10)
                    {
                        case 1:
                            Image figure = new Bitmap(50, 50);
                            Graphics gr = Graphics.FromImage(figure);
                            gr.DrawImage(Sprite, new Rectangle(0, 0, 50, 50), 0 + 150 * (map[i, j] % 10 - 1), 0, 150, 150, GraphicsUnit.Pixel);
                            cell.BackgroundImage = figure;
                            break;
                        case 2:
                            Image figure1 = new Bitmap(50, 50);
                            Graphics gr1 = Graphics.FromImage(figure1);
                            gr1.DrawImage(Sprite, new Rectangle(0, 0, 50, 50), 0 + 150 * (map[i, j] % 10 - 1), 150, 150, 150, GraphicsUnit.Pixel);
                            cell.BackgroundImage = figure1;
                            break;
                    }
                    // при нажатии на клетку происходит событие PressFigure
                    cell.Click += new EventHandler(PressFigure);
                    this.Controls.Add(cell);

                    // запоминаем созданную кнопку, чтобы использовать в будущем
                    butts[i, j] = cell;
                }
            }

        }
        public bool move = false; // ход
        public Button prevFigure; // прошлая фигура
        public Button Fig;
        public void PressFigure(object sender, EventArgs e)
        {
            // RestartCell(); 
            Button Fig = sender as Button;
            Load_Image_Button_Switch(map[Fig.Location.Y / 50, Fig.Location.X / 50]);
            if (map[Fig.Location.Y / 50, Fig.Location.X / 50] != 0 && map[Fig.Location.Y / 50, Fig.Location.X / 50] / 10 == Player)
            {
                Fig.BackColor = Color.Red;
                OffButtons();
                Fig.Enabled = true;
                Steps(Fig.Location.Y / 50, Fig.Location.X / 50, map[Fig.Location.Y / 50, Fig.Location.X / 50]);

                // если передумали ходить выбранной фигурой
                if (move)
                {
                    RestartCell();
                    OnButtons();
                    move = false;
                }
                else
                {
                    move = true;
                }

            }
            else
            {
                if (move)
                {
                    int temp = map[Fig.Location.Y / 50, Fig.Location.X / 50];
                    map[Fig.Location.Y / 50, Fig.Location.X / 50] = map[prevFigure.Location.Y / 50, prevFigure.Location.X / 50];
                    map[prevFigure.Location.Y / 50, prevFigure.Location.X / 50] = temp;
                    Fig.BackgroundImage = prevFigure.BackgroundImage;
                    map[prevFigure.Location.Y / 50, prevFigure.Location.X / 50] = 0;
                    prevFigure.BackgroundImage = null;
                    move = false;
                    RestartCell();
                    OnButtons();
                    Switch_Figure(map[Fig.Location.Y / 50, Fig.Location.X / 50], Fig.Location.Y / 50, Fig.Location.X / 50, Fig, Player);
                    switchPlayer();
                    OnButtons();

                }
            }


            prevFigure = Fig;
        }

        public void Steps(int X, int Y, int Figure)
        {
            // используется для показа шагов для пешки. Смотря еще какой цвет фигуры
            int ss;
            if (Player == 1)
            {
                ss = -1; // первый игрок. Ходы вверх
            }
            else
            {
                ss = 1; // второй игрок. Ходы вниз
            }
            switch (Figure % 10)
            {
                // пешка
                case 6:
                    if (Sized(X + 1 * ss, Y))
                    {
                        if (map[X + 1 * ss, Y] == 0)
                        {
                            butts[X + 1 * ss, Y].BackColor = Color.YellowGreen;
                            butts[X + 1 * ss, Y].Enabled = true;
                        }
                        if ((X == 6 && ss == -1) || (X == 1 && ss == 1))
                        {
                            // условие проверки если пешка находится на 7(6) или 2(1) горизонтали. И еще смотря какая фигура
                            butts[X + 2 * ss, Y].BackColor = Color.YellowGreen;
                            butts[X + 2 * ss, Y].Enabled = true;
                        }
                    }
                    // проверяю есть ли рядом фигура
                    if (Sized(X + 1 * ss, Y + 1))
                    {
                        if (map[X + 1 * ss, Y + 1] != 0 && map[X + 1 * ss, Y + 1] / 10 != Player)
                        {
                            butts[X + 1 * ss, Y + 1].BackColor = Color.YellowGreen;
                            butts[X + 1 * ss, Y + 1].Enabled = true;
                        }
                    }
                    if (Sized(X + 1 * ss, Y - 1))
                    {
                        if (map[X + 1 * ss, Y - 1] != 0 && map[X + 1 * ss, Y - 1] / 10 != Player)
                        {
                            butts[X + 1 * ss, Y - 1].BackColor = Color.YellowGreen;
                            butts[X + 1 * ss, Y - 1].Enabled = true;
                        }
                    }
                    break;
                case 5: // ладья
                    VG(X, Y);
                    break;
                case 2: // ферзь
                    VG(X, Y);
                    Diagonal(X, Y);
                    break;
                case 3: // слон
                    Diagonal(X, Y);
                    break;
                case 4: // конь
                    if (Sized(X + 2, Y + 1))
                    {
                        CheckStep(X + 2, Y + 1);
                    }
                    if (Sized(X + 2, Y - 1))
                    {
                        CheckStep(X + 2, Y - 1);
                    }
                    if (Sized(X - 2, Y + 1))
                    {
                        CheckStep(X - 2, Y + 1);
                    }
                    if (Sized(X - 2, Y - 1))
                    {
                        CheckStep(X - 2, Y - 1);
                    }
                    if (Sized(X + 2, Y + 1))
                    {
                        CheckStep(X + 2, Y + 1);
                    }
                    if (Sized(X + 1, Y + 2))
                    {
                        CheckStep(X + 1, Y + 2);
                    }
                    if (Sized(X + 1, Y - 2))
                    {
                        CheckStep(X + 1, Y - 2);
                    }
                    if (Sized(X - 1, Y + 2))
                    {
                        CheckStep(X - 1, Y + 2);
                    }
                    if (Sized(X - 1, Y - 2))
                    {
                        CheckStep(X - 1, Y - 2);
                    }
                    break;
                case 1: // король
                    VG(X, Y, true);
                    Diagonal(X, Y, true);
                    break;
            }

        }
        // ходы вертикаль и горизонталь. X u Y - ясно для чего. step - чтобы показать сколько нужно шагов. false чтобы показать на все поле
        public void VG(int X, int Y, bool step = false)
        {
            // все шаги вверх
            for (int i = X - 1; i >= 0; i--)
            {
                if (Sized(i, Y))
                {
                    if (!CheckStep(i, Y))
                    {
                        break;
                    }
                }
                if (step)
                {
                    break;
                }
            }
            // все шаги вниз
            for (int i = X + 1; i < 8; i++)
            {
                if (Sized(i, Y))
                {
                    if (!CheckStep(i, Y))
                    {
                        break;
                    }
                }
                if (step)
                {
                    break;
                }
            }
            // все шаги вправо
            for (int j = Y + 1; j < 8; j++)
            {
                if (Sized(X, j))
                {
                    if (!CheckStep(X, j))
                    {
                        break;
                    }
                }
                if (step)
                {
                    break;
                }
            }
            // все шаги влево
            for (int j = Y - 1; j >= 0; j--)
            {
                if (Sized(X, j))
                {
                    if (!CheckStep(X, j))
                    {
                        break;
                    }
                }
                if (step)
                {
                    break;
                }
            }
        }

        // ходы по диагонали
        public void Diagonal(int X, int Y, bool step = false)
        {
            // вверх вправо
            int j = Y + 1;
            for (int i = X - 1; i >= 0; i--)
            {
                if (Sized(i, j))
                {
                    if (!CheckStep(i, j))
                    {
                        break;
                    }
                }
                if (j < 7)
                {
                    j++;
                }
                else break;
                if (step) break;
            }
            // вверх влево
            j = Y - 1;
            for (int i = X - 1; i >= 0; i--)
            {
                if (Sized(i, j))
                {
                    if (!CheckStep(i, j))
                    {
                        break;
                    }
                }
                if (j > 0)
                {
                    j--;
                }
                else break;
                if (step) break;
            }
            // внил влево
            j = Y - 1;
            for (int i = X + 1; i < 8; i++)
            {
                if (Sized(i, j))
                {
                    if (!CheckStep(i, j))
                    {
                        break;
                    }
                }
                if (j > 0)
                {
                    j--;
                }
                else break;
                if (step) break;
            }
            // вниз вправо
            j = Y + 1;
            for (int i = X + 1; i < 8; i++)
            {
                if (Sized(i, j))
                {
                    if (!CheckStep(i, j))
                    {
                        break;
                    }
                }
                if (j < 7)
                {
                    j++;
                }
                else break;
                if (step) break;
            }
        }

        // проверка на возможность хода
        public bool CheckStep(int X, int Y)
        {
            if (map[X, Y] == 0)
            {
                butts[X, Y].BackColor = Color.YellowGreen; // показывает куда я могу сходить
                butts[X, Y].Enabled = true; // это для пустой клетки
            }
            else
            {
                if (map[X, Y] / 10 != Player)
                {
                    butts[X, Y].BackColor = Color.YellowGreen;
                    butts[X, Y].Enabled = true; // если противник стоит на этой клетке
                }
                return false;
            }
            return true;
        }

        // включить фигуры(кнопки)
        public void OnButtons()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    butts[i, j].Enabled = true;
                }
            }
        }

        // отлючить фигуры(кнопки)
        public void OffButtons()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    butts[i, j].Enabled = false;
                }
            }
        }

        // проверяю границы
        public bool Sized(int i, int j)
        {
            if (i < 0 || j < 0 || i >= 8 || j >= 8)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // смена хода у игроков
        public void switchPlayer()
        {
            if (Player == 1)
            {
                Player = 2;
                label14.Text = "★ Черные";
                label15.Text = "Белые";
                lb14.Text = "★ Черные";
                lb15.Text = "Белые";
            }
            else
            {
                Player = 1;
                label14.Text = "Черные";
                label15.Text = "★ Белые";
                lb14.Text = "Черные";
                lb15.Text = "★ Белые";
            }
        }
        // вернуть все клетки
        public void RestartCell()
        {
            int fl = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (fl == 0)
                    {
                        butts[i, j].BackColor = Color.White;
                        if (j != 7)
                        {
                            fl = 1;
                        }
                    }
                    else
                    {
                        butts[i, j].BackColor = Color.Brown;
                        if (j != 7)
                        {
                            fl = 0;
                        }
                    }
                }

            }
        }
        // начать все заново
        // динамические кнопки, которые на форме
        public Button bt1 = new Button();
        public Button bt2 = new Button();
        public Button bt3 = new Button();
        public TextBox tb1 = new TextBox();
        public TextBox tb2 = new TextBox();
        public TextBox tb3 = new TextBox();
        public Label lb1 = new Label();
        public Label lb2 = new Label();
        public Label lb3 = new Label();
        public Label lb4 = new Label();
        public Label lb5 = new Label();
        public Label lb6 = new Label();
        public Label lb7 = new Label();
        public Label lb8 = new Label();
        public Label lb9 = new Label();
        public Label lb10 = new Label();
        public Label lb11 = new Label();
        public Label lb12 = new Label();
        public Label lb13 = new Label();
        public Label lb14 = new Label();
        public Label lb15 = new Label();
        public Label lb16 = new Label();
        public Label lb17 = new Label();
        public Label lb18 = new Label();
        public Label lb19 = new Label();
        public Label lb20 = new Label();
        public Label lb21 = new Label();
        public Label lb22 = new Label();
        public Label lb23 = new Label();
        public Label lb24 = new Label();
        public Label lb25 = new Label();
        public Label lb26 = new Label();
        public Label lb27 = new Label();
        public Label lb28 = new Label();
        public Label lb29 = new Label();
        public Label lb30 = new Label();
        public Label lb31 = new Label();
        public int Check_DEN_Button = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            // Заново
            this.Controls.Clear();
            timer1.Enabled = false;
            timer2.Enabled = false;
            Start();
            // сделать динамические кнопки (такие же)
            bt1.Text = button1.Text;
            bt1.Location = button1.Location;
            bt1.Size = button1.Size;
            bt1.Click += button1_Click;
            this.Controls.Add(bt1);

            bt2.Text = button2.Text;
            bt2.Location = button2.Location;
            bt2.Size = button2.Size;
            bt2.Click += Read_Time_TextBox;
            this.Controls.Add(bt2);

            bt3.Text = button3.Text;
            bt3.Location = button3.Location;
            bt3.Size = button3.Size;
            bt3.Click += button3_Click;
            this.Controls.Add(bt3);
           
            tb1.Text = textBox1.Text;
            tb1.Location = textBox1.Location;
            tb1.Size = textBox1.Size;
            tb1.Font = textBox1.Font;
            this.Controls.Add(tb1);

            tb2.Text = textBox2.Text;
            tb2.Location = textBox2.Location;
            tb2.Size = textBox2.Size;
            tb2.Font = textBox2.Font;
            this.Controls.Add(tb2);

            tb3.Text = textBox3.Text;
            tb3.Location = textBox3.Location;
            tb3.Size = textBox3.Size;
            tb3.Font = textBox3.Font;
            this.Controls.Add(tb3);

            lb1.Text = label1.Text;
            lb1.Location = label1.Location;
            lb1.Size = label1.Size;
            lb1.Font = label1.Font;
            lb1.AutoSize = label1.AutoSize;
            this.Controls.Add(lb1);

            lb2.Text = label2.Text;
            lb2.Location = label2.Location;
            lb2.Size = label2.Size;
            lb2.Font = label2.Font;
            lb2.AutoSize = label2.AutoSize;
            this.Controls.Add(lb2);

            lb3.Text = label3.Text;
            lb3.Location = label3.Location;
            lb3.Size = label3.Size;
            lb3.Font = label3.Font;
            lb3.AutoSize = label3.AutoSize;
            this.Controls.Add(lb3);

            lb4.Text = label4.Text;
            lb4.Location = label4.Location;
            lb4.Size = label4.Size;
            lb4.Font = label4.Font;
            lb4.AutoSize = label4.AutoSize;
            this.Controls.Add(lb4);

            lb5.Text = label5.Text;
            lb5.Location = label5.Location;
            lb5.Size = label5.Size;
            lb5.Font = label5.Font;
            lb5.AutoSize = label5.AutoSize;
            this.Controls.Add(lb5);

            lb6.Text = label6.Text;
            lb6.Location = label6.Location;
            lb6.Size = label6.Size;
            lb6.Font = label6.Font;
            lb6.AutoSize = label6.AutoSize;
            this.Controls.Add(lb6);

            lb7.Text = label7.Text;
            lb7.Location = label7.Location;
            lb7.Size = label7.Size;
            lb7.Font = label7.Font;
            lb7.AutoSize = label7.AutoSize;
            this.Controls.Add(lb7);

            lb8.Text = label8.Text;
            lb8.Location = label8.Location;
            lb8.Size = label8.Size;
            lb8.Font = label8.Font;
            lb8.AutoSize = label8.AutoSize;
            this.Controls.Add(lb8);

            lb9.Text = label9.Text;
            lb9.Location = label9.Location;
            lb9.Size = label9.Size;
            lb9.Font = label9.Font;
            lb9.AutoSize = label9.AutoSize;
            this.Controls.Add(lb9);

            lb10.Text = label10.Text;
            lb10.Location = label10.Location;
            lb10.Size = label10.Size;
            lb10.Font = label10.Font;
            lb10.AutoSize = label10.AutoSize;
            this.Controls.Add(lb10);

            lb11.Text = label11.Text;
            lb11.Location = label11.Location;
            lb11.Size = label11.Size;
            lb11.Font = label11.Font;
            lb11.AutoSize = label11.AutoSize;
            this.Controls.Add(lb11);

            lb12.Text = label12.Text;
            lb12.Location = label12.Location;
            lb12.Size = label12.Size;
            lb12.Font = label12.Font;
            lb12.AutoSize = label12.AutoSize;
            this.Controls.Add(lb12);

            lb13.Text = label13.Text;
            lb13.Location = label13.Location;
            lb13.Size = label13.Size;
            lb13.Font = label13.Font;
            lb13.AutoSize = label13.AutoSize;
            this.Controls.Add(lb13);

            lb14.Text = label14.Text;
            lb14.Location = label14.Location;
            lb14.Size = label14.Size;
            lb14.Font = label14.Font;
            lb14.AutoSize = label14.AutoSize;
            this.Controls.Add(lb14);

            lb15.Text = label15.Text;
            lb15.Location = label15.Location;
            lb15.Size = label15.Size;
            lb15.Font = label15.Font;
            lb15.AutoSize = label15.AutoSize;
            this.Controls.Add(lb15);

            Queen.BackgroundImage = button4.BackgroundImage;
            Queen.Location = button4.Location;
            Queen.Size = button4.Size;
            Queen.AutoSize = button4.AutoSize;
            Queen.Enabled = true;
            Queen.Click += Switch_Figure_Click_Button1;
            this.Controls.Add(Queen);

            Rook.BackgroundImage = button5.BackgroundImage;
            Rook.Location = button5.Location;
            Rook.Size = button5.Size;
            Rook.AutoSize = button5.AutoSize;
            Rook.Enabled = true;
            Rook.Click += Switch_Figure_Click_Button2;
            this.Controls.Add(Rook);

            Elephant.BackgroundImage = button6.BackgroundImage;
            Elephant.Location = button6.Location;
            Elephant.Size = button6.Size;
            Elephant.AutoSize = button6.AutoSize;
            Elephant.Enabled = true;
            Elephant.Click += Switch_Figure_Click_Button3;
            this.Controls.Add(Elephant);

            Horse.BackgroundImage = button7.BackgroundImage;
            Horse.Location = button7.Location;
            Horse.Size = button7.Size;
            Horse.AutoSize = button7.AutoSize;
            Horse.Enabled = true;
            Horse.Click += Switch_Figure_Click_Button4;
            this.Controls.Add(Horse);

            lb16.Text = label16.Text;
            lb16.Location = label16.Location;
            lb16.Size = label16.Size;
            lb16.Font = label16.Font;
            lb16.AutoSize = label16.AutoSize;
            this.Controls.Add(lb16);

            lb17.Text = label17.Text;
            lb17.Location = label17.Location;
            lb17.Size = label17.Size;
            lb17.Font = label17.Font;
            lb17.AutoSize = label17.AutoSize;
            this.Controls.Add(lb17);

            lb18.Text = label18.Text;
            lb18.Location = label18.Location;
            lb18.Size = label18.Size;
            lb18.Font = label18.Font;
            lb18.AutoSize = label18.AutoSize;
            this.Controls.Add(lb18);

            lb19.Text = label19.Text;
            lb19.Location = label19.Location;
            lb19.Size = label19.Size;
            lb19.Font = label19.Font;
            lb19.AutoSize = label19.AutoSize;
            this.Controls.Add(lb19);

            lb20.Text = label20.Text;
            lb20.Location = label20.Location;
            lb20.Size = label20.Size;
            lb20.Font = label20.Font;
            lb20.AutoSize = label20.AutoSize;
            this.Controls.Add(lb20);

            lb21.Text = label21.Text;
            lb21.Location = label21.Location;
            lb21.Size = label21.Size;
            lb21.Font = label21.Font;
            lb21.AutoSize = label21.AutoSize;
            this.Controls.Add(lb21);

            lb22.Text = label22.Text;
            lb22.Location = label22.Location;
            lb22.Size = label22.Size;
            lb22.Font = label22.Font;
            lb22.AutoSize = label22.AutoSize;
            this.Controls.Add(lb22);

            lb23.Text = label23.Text;
            lb23.Location = label23.Location;
            lb23.Size = label23.Size;
            lb23.Font = label23.Font;
            lb23.AutoSize = label23.AutoSize;
            this.Controls.Add(lb23);

            lb24.Text = label24.Text;
            lb24.Location = label24.Location;
            lb24.Size = label24.Size;
            lb24.Font = label24.Font;
            lb24.AutoSize = label24.AutoSize;
            this.Controls.Add(lb24);

            lb25.Text = label25.Text;
            lb25.Location = label25.Location;
            lb25.Size = label25.Size;
            lb25.Font = label25.Font;
            lb25.AutoSize = label25.AutoSize;
            this.Controls.Add(lb25);

            lb26.Text = label26.Text;
            lb26.Location = label26.Location;
            lb26.Size = label26.Size;
            lb26.Font = label26.Font;
            lb26.AutoSize = label26.AutoSize;
            this.Controls.Add(lb26);

            lb27.Text = label27.Text;
            lb27.Location = label27.Location;
            lb27.Size = label27.Size;
            lb27.Font = label27.Font;
            lb27.AutoSize = label27.AutoSize;
            this.Controls.Add(lb27);

            lb28.Text = label28.Text;
            lb28.Location = label28.Location;
            lb28.Size = label28.Size;
            lb28.Font = label28.Font;
            lb28.AutoSize = label28.AutoSize;
            this.Controls.Add(lb28);

            lb29.Text = label29.Text;
            lb29.Location = label29.Location;
            lb29.Size = label29.Size;
            lb29.Font = label29.Font;
            lb29.AutoSize = label29.AutoSize;
            this.Controls.Add(lb29);

            lb30.Text = label30.Text;
            lb30.Location = label30.Location;
            lb30.Size = label30.Size;
            lb30.Font = label30.Font;
            lb30.AutoSize = label30.AutoSize;
            this.Controls.Add(lb30);

            lb31.Text = label31.Text;
            lb31.Location = label31.Location;
            lb31.Size = label31.Size;
            lb31.Font = label31.Font;
            lb31.AutoSize = label31.AutoSize;
            this.Controls.Add(lb31);

            Check_DEN_Button = 1;
            flll = 0;
            lb14.Text = "Черные";
            lb15.Text = "★ Белые";
        }

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            // не нужно
        }       

        // Часы ------------------------------------------------------------------

        public int s1 = -11, s2 = -11; // секунды
        public int m1 = -11, m2 = -11; // минуты
        public int h1 = -11, h2 = -11; // часы

        private void Read_Time_TextBox(object sender, EventArgs e)
        {
            s1 = int.Parse(tb1.Text);
            s2 = int.Parse(tb1.Text);
            m1 = int.Parse(tb2.Text);
            m2 = int.Parse(tb2.Text);
            h1 = int.Parse(tb3.Text);
            h2 = int.Parse(tb3.Text);
            lb1.Text = s1.ToString();
            lb2.Text = m1.ToString();
            lb3.Text = h1.ToString();
            lb4.Text = s2.ToString();
            lb5.Text = m2.ToString();
            lb6.Text = h2.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // выбор времени
            s1 = int.Parse(textBox1.Text);
            s2 = int.Parse(textBox1.Text);
            m1 = int.Parse(textBox2.Text);
            m2 = int.Parse(textBox2.Text);
            h1 = int.Parse(textBox3.Text);
            h2 = int.Parse(textBox3.Text);
            label1.Text = s1.ToString();
            label2.Text = m1.ToString();
            label3.Text = h1.ToString();
            label4.Text = s2.ToString();
            label5.Text = m2.ToString();
            label6.Text = h2.ToString();           
        }

        private void label2_Click(object sender, EventArgs e)
        {
           // не нужно
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // старт игры(запускает таймер)
            timer1.Enabled = !timer1.Enabled;
            //timer2.Enabled = !timer2.Enabled;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // не нужно
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // таймер 1 игрока
            CheckTimeWin();
            if (Player == 1 && flll == 0)
            {
                timer1.Enabled = true;
                timer2.Enabled = false;
                if(Check_DEN_Button == 0)
                {
                    s1 = int.Parse(label1.Text);
                    m1 = int.Parse(label2.Text);
                    h1 = int.Parse(label3.Text);
                }
                else
                {
                    s1 = int.Parse(lb1.Text);
                    m1 = int.Parse(lb2.Text);
                    h1 = int.Parse(lb3.Text);
                }
                s1--;
                if (s1 == -1)
                {
                    if (m1 != 0)
                    {
                        m1--;
                        s1 = 59;
                    }

                }
                if (m1 == 0 && s1 == -1)
                {
                    if (h1 != 0)
                    {
                        h1--;
                        m1 = 59;
                        s1 = 59;
                    }
                }
                label1.Text = s1.ToString();
                label2.Text = m1.ToString();
                label3.Text = h1.ToString();
                lb1.Text = s1.ToString();
                lb2.Text = m1.ToString();
                lb3.Text = h1.ToString();
            }
            else
            {
                CheckTimeWin();
                if(flll == 0)
                {                  
                    timer1.Enabled = false;
                    timer2.Enabled = true;
                }
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            // таймер 2 игрока
            CheckTimeWin();
            if(Player == 2 && flll == 0)
            {

                timer1.Enabled = false;
                timer2.Enabled = true;

                if (Check_DEN_Button == 0)
                {
                    s2 = int.Parse(label4.Text);
                    m2 = int.Parse(label5.Text);
                    h2 = int.Parse(label6.Text);
                }
                else
                {
                    s2 = int.Parse(lb4.Text);
                    m2 = int.Parse(lb5.Text);
                    h2 = int.Parse(lb6.Text);
                }
                s2--;
                if (s2 == -1)
                {
                    if (m2 != 0)
                    {
                        m2--;
                        s2 = 59;
                    }

                }
                if (m2 == 0 && s2 == -1)
                {
                    if (h2 != 0)
                    {
                        h2--;
                        m2 = 59;
                        s2 = 59;
                    }
                }
                label4.Text = s2.ToString();
                label5.Text = m2.ToString();
                label6.Text = h2.ToString();
                lb4.Text = s2.ToString();
                lb5.Text = m2.ToString();
                lb6.Text = h2.ToString();
            }
            else
            {
                CheckTimeWin();
                if(flll == 0)
                {
                    timer1.Enabled = true;
                    timer2.Enabled = false;
                }
            }
        }
        public int flll = 0;
        public int ss1, ss2, mm1, mm2, hh1, hh2;
        public void CheckTimeWin()
        {

            ss1 = s1; ss2 = s2;
            mm1 = m1; mm2 = m2;
            hh1 = h1; hh2 = h2;
            //------------------------

            if (ss1 == 0 && mm1 == 0 && hh1 == 0 && flll == 0)
            {
                s1 = -1; m1 = -1; h1 = -1;
                timer1.Enabled = false;
                timer2.Enabled = false;
                flll = 1;
                OffButtons();
                MessageBox.Show("Черные выиграли!");
            }
            if(ss2 == 0 && mm2 == 0 && hh2 == 0 && flll == 0)
            {
                s2 = -1; m2 = -1; h2 = -1;
                timer1.Enabled = false;
                timer2.Enabled = false;
                flll = 1;
                OffButtons();
                MessageBox.Show("Белые выиграли!");
            }
        }

        // -------------------------------------------------------------------------------

        public void Switch_Figure_Click_Button1(object sender, EventArgs e)
        {
            if (Press_Switch_Figure1 == false)
            {
                Press_Switch_Figure1 = true;
                button5.Enabled = false;
                button6.Enabled = false;
                button7.Enabled = false;
                Rook.Enabled = false;
                Elephant.Enabled = false;
                Horse.Enabled = false;
            }
            else
            {
                button5.Enabled = true;
                button6.Enabled = true;
                button7.Enabled = true;
                Rook.Enabled = true;
                Elephant.Enabled = true;
                Horse.Enabled = true;
                Press_Switch_Figure1 = false;
            }
        }
        public void Switch_Figure_Click_Button2(object sender, EventArgs e)
        {
            if (Press_Switch_Figure2 == false)
            {
                Press_Switch_Figure2 = true;
                button4.Enabled = false;
                button6.Enabled = false;
                button7.Enabled = false;
                Queen.Enabled = false;
                Elephant.Enabled = false;
                Horse.Enabled = false;
            }
            else
            {
                button4.Enabled = true;
                button6.Enabled = true;
                button7.Enabled = true;
                Queen.Enabled = true;
                Elephant.Enabled = true;
                Horse.Enabled = true;
                Press_Switch_Figure2 = false;
            }
        }
        public void Switch_Figure_Click_Button3(object sender, EventArgs e)
        {
            if (Press_Switch_Figure3 == false)
            {
                Press_Switch_Figure3 = true;
                button4.Enabled = false;
                button5.Enabled = false;
                button7.Enabled = false;
                Queen.Enabled = false;
                Rook.Enabled = false;
                Horse.Enabled = false;
            }
            else
            {
                button4.Enabled = true;
                button5.Enabled = true;
                button7.Enabled = true;
                Queen.Enabled = true;
                Rook.Enabled = true;
                Horse.Enabled = true;
                Press_Switch_Figure3 = false;

            }
        }
        public void Switch_Figure_Click_Button4(object sender, EventArgs e)
        {
            if (Press_Switch_Figure4 == false)
            {
                Press_Switch_Figure4 = true;
                button4.Enabled = false;
                button5.Enabled = false;
                button6.Enabled = false;
                Queen.Enabled = false;
                Rook.Enabled = false;
                Elephant.Enabled = false;
            }
            else
            {
                button4.Enabled = true;
                button5.Enabled = true;
                button6.Enabled = true;
                Queen.Enabled = true;
                Rook.Enabled = true;
                Elephant.Enabled = true;
                Press_Switch_Figure4 = false;

            }
        }
        public void Switch_Figure_Click_Button(object sender, EventArgs e)
        { 
            // Не нужно
        }

        // проверка если пешка дошла до конца поля
        public bool Press_Switch_Figure1 = false;
        public bool Press_Switch_Figure2 = false;
        public bool Press_Switch_Figure3 = false;
        public bool Press_Switch_Figure4 = false;
        public Button Queen = new Button(); // королева
        public Button Rook = new Button(); // ладья
        public Button Elephant = new Button(); // слон
        public Button Horse = new Button(); // конь
        public void Switch_Figure(int Figure, int Position_Figure_X, int Position_Figure_Y, Button Fig, int Check_Player)
        {
            Image figure = new Bitmap(50, 50);
            Graphics gr = Graphics.FromImage(figure);
            // int Check_Player = Player; 
            // 1. Проверка если горизонталь 1(0) или 8(7). Соответственно белая или черная фигура
            if(Figure % 10 == 6 && (Position_Figure_X == 0 || Position_Figure_X == 7))
            {
                if(Figure / 10 == 1)
                {
                    figure = new Bitmap(50, 50);
                    gr = Graphics.FromImage(figure);
                    gr.DrawImage(Sprite, new Rectangle(0, 0, 50, 50), 0 + 150 * 1, 0, 150, 150, GraphicsUnit.Pixel);
                    button4.BackgroundImage = figure;
                    Queen.BackgroundImage = figure;

                    figure = new Bitmap(50, 50);
                    gr = Graphics.FromImage(figure);
                    gr.DrawImage(Sprite, new Rectangle(0, 0, 50, 50), 0 + 150 * 4, 0, 150, 150, GraphicsUnit.Pixel);
                    button5.BackgroundImage = figure;
                    Rook.BackgroundImage = figure;

                    figure = new Bitmap(50, 50);
                    gr = Graphics.FromImage(figure);
                    gr.DrawImage(Sprite, new Rectangle(0, 0, 50, 50), 0 + 150 * 2, 0, 150, 150, GraphicsUnit.Pixel);
                    button6.BackgroundImage = figure;
                    Elephant.BackgroundImage = figure;

                    figure = new Bitmap(50, 50);
                    gr = Graphics.FromImage(figure);
                    gr.DrawImage(Sprite, new Rectangle(0, 0, 50, 50), 0 + 150 * 3, 0, 150, 150, GraphicsUnit.Pixel);
                    button7.BackgroundImage = figure;
                    Horse.BackgroundImage = figure;
                }
                else
                {
                    figure = new Bitmap(50, 50);
                    gr = Graphics.FromImage(figure);
                    gr.DrawImage(Sprite, new Rectangle(0, 0, 50, 50), 0 + 150 * 1, 150, 150, 150, GraphicsUnit.Pixel);
                    button4.BackgroundImage = figure;
                    Queen.BackgroundImage = figure;

                    figure = new Bitmap(50, 50);
                    gr = Graphics.FromImage(figure);
                    gr.DrawImage(Sprite, new Rectangle(0, 0, 50, 50), 0 + 150 * 4, 150, 150, 150, GraphicsUnit.Pixel);
                    button5.BackgroundImage = figure;
                    Rook.BackgroundImage = figure;

                    figure = new Bitmap(50, 50);
                    gr = Graphics.FromImage(figure);
                    gr.DrawImage(Sprite, new Rectangle(0, 0, 50, 50), 0 + 150 * 2, 150, 150, 150, GraphicsUnit.Pixel);
                    button6.BackgroundImage = figure;
                    Elephant.BackgroundImage = figure;

                    figure = new Bitmap(50, 50);
                    gr = Graphics.FromImage(figure);
                    gr.DrawImage(Sprite, new Rectangle(0, 0, 50, 50), 0 + 150 * 3, 150, 150, 150, GraphicsUnit.Pixel);
                    button7.BackgroundImage = figure;
                    Horse.BackgroundImage = figure;
                }

                OffButtons();

                if (Press_Switch_Figure1 == true)
                {
                    Fig.BackgroundImage = button4.BackgroundImage;
                    Fig.BackgroundImage = Queen.BackgroundImage;
                    map[Position_Figure_X, Position_Figure_Y] = Check_Player * 10 + 2;
                    button4.Enabled = true;
                    Queen.Enabled = true;
                    Press_Switch_Figure1 = false;
                    button5.Enabled = true;
                    Rook.Enabled = true;
                    Press_Switch_Figure2 = false;
                    button6.Enabled = true;
                    Elephant.Enabled = true;
                    Press_Switch_Figure3 = false;
                    button7.Enabled = true;
                    Horse.Enabled = true;
                    Press_Switch_Figure4 = false;
                    OnButtons();
                }
                else if (Press_Switch_Figure2 == true)
                {
                    Fig.BackgroundImage = button5.BackgroundImage;
                    Fig.BackgroundImage = Rook.BackgroundImage;
                    map[Position_Figure_X, Position_Figure_Y] = Check_Player * 10 + 5;
                    button4.Enabled = true;
                    Queen.Enabled = true;
                    Press_Switch_Figure1 = false;
                    button5.Enabled = true;
                    Rook.Enabled = true;
                    Press_Switch_Figure2 = false;
                    button6.Enabled = true;
                    Elephant.Enabled = true;
                    Press_Switch_Figure3 = false;
                    button7.Enabled = true;
                    Horse.Enabled = true;
                    Press_Switch_Figure4 = false;
                    OnButtons();
                }
                else if (Press_Switch_Figure3 == true)
                {
                    Fig.BackgroundImage = button6.BackgroundImage;
                    Fig.BackgroundImage = Elephant.BackgroundImage;
                    map[Position_Figure_X, Position_Figure_Y] = Check_Player * 10 + 3;
                    button4.Enabled = true;
                    Queen.Enabled = true;
                    Press_Switch_Figure1 = false;
                    button5.Enabled = true;
                    Rook.Enabled = true;
                    Press_Switch_Figure2 = false;
                    button6.Enabled = true;
                    Elephant.Enabled = true;
                    Press_Switch_Figure3 = false;
                    button7.Enabled = true;
                    Horse.Enabled = true;
                    Press_Switch_Figure4 = false;
                    OnButtons();
                }
                else if (Press_Switch_Figure4 == true)
                {
                    Fig.BackgroundImage = button7.BackgroundImage;
                    Fig.BackgroundImage = Horse.BackgroundImage;
                    map[Position_Figure_X, Position_Figure_Y] = Check_Player * 10 + 4;
                    button4.Enabled = true;
                    Queen.Enabled = true;
                    Press_Switch_Figure1 = false;
                    button5.Enabled = true;
                    Rook.Enabled = true;
                    Press_Switch_Figure2 = false;
                    button6.Enabled = true;
                    Elephant.Enabled = true;
                    Press_Switch_Figure3 = false;
                    button7.Enabled = true;
                    Horse.Enabled = true;
                    Press_Switch_Figure4 = false;
                    OnButtons();
                }
            }

        }

        // функция загружает в 4 кнопки картинки. Нужна чтобы выбирать на какую фигуру менять, когда пешка дойдет до конца.
        public void Load_Image_Button_Switch(int Figure)
        {
            Image figure = new Bitmap(50, 50);
            Graphics gr = Graphics.FromImage(figure);
            if (Figure / 10 == 1)
            {
                figure = new Bitmap(50, 50);
                gr = Graphics.FromImage(figure);
                gr.DrawImage(Sprite, new Rectangle(0, 0, 50, 50), 0 + 150 * 1, 0, 150, 150, GraphicsUnit.Pixel);
                button4.BackgroundImage = figure;
                Queen.BackgroundImage = figure;

                figure = new Bitmap(50, 50);
                gr = Graphics.FromImage(figure);
                gr.DrawImage(Sprite, new Rectangle(0, 0, 50, 50), 0 + 150 * 4, 0, 150, 150, GraphicsUnit.Pixel);
                button5.BackgroundImage = figure;
                Rook.BackgroundImage = figure;

                figure = new Bitmap(50, 50);
                gr = Graphics.FromImage(figure);
                gr.DrawImage(Sprite, new Rectangle(0, 0, 50, 50), 0 + 150 * 2, 0, 150, 150, GraphicsUnit.Pixel);
                button6.BackgroundImage = figure;
                Elephant.BackgroundImage = figure;

                figure = new Bitmap(50, 50);
                gr = Graphics.FromImage(figure);
                gr.DrawImage(Sprite, new Rectangle(0, 0, 50, 50), 0 + 150 * 3, 0, 150, 150, GraphicsUnit.Pixel);
                button7.BackgroundImage = figure;
                Horse.BackgroundImage = figure;
            }
            else
            {
                figure = new Bitmap(50, 50);
                gr = Graphics.FromImage(figure);
                gr.DrawImage(Sprite, new Rectangle(0, 0, 50, 50), 0 + 150 * 1, 150, 150, 150, GraphicsUnit.Pixel);
                button4.BackgroundImage = figure;
                Queen.BackgroundImage = figure;

                figure = new Bitmap(50, 50);
                gr = Graphics.FromImage(figure);
                gr.DrawImage(Sprite, new Rectangle(0, 0, 50, 50), 0 + 150 * 4, 150, 150, 150, GraphicsUnit.Pixel);
                button5.BackgroundImage = figure;
                Rook.BackgroundImage = figure;

                figure = new Bitmap(50, 50);
                gr = Graphics.FromImage(figure);
                gr.DrawImage(Sprite, new Rectangle(0, 0, 50, 50), 0 + 150 * 2, 150, 150, 150, GraphicsUnit.Pixel);
                button6.BackgroundImage = figure;
                Elephant.BackgroundImage = figure;

                figure = new Bitmap(50, 50);
                gr = Graphics.FromImage(figure);
                gr.DrawImage(Sprite, new Rectangle(0, 0, 50, 50), 0 + 150 * 3, 150, 150, 150, GraphicsUnit.Pixel);
                button7.BackgroundImage = figure;
                Horse.BackgroundImage = figure;
            }
        }
    }
}
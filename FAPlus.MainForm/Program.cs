using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace FAPlus.MainForm
{
    internal static class Program
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool flagMutex;

            Mutex mutex = new Mutex(true, "TestFrm", out flagMutex);
            if (flagMutex)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                // 실행할 Form 클래스
                Application.Run(new Form1());
                mutex.ReleaseMutex();
            }
            else
            {
                // 여러개 실행시켰을 때 띄울 메시지
                MessageBox.Show("프로그램이 이미 실행 중입니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

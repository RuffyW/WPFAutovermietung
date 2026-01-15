using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WPFAutovermietung.Model
{

    public class Metronom
    {

        private readonly int interval;
        private bool running;
        private Thread thread;

        public event Action Tick;  // <-- Delegates + Events!

        public Metronom(int intervalMs)
        {
            interval = intervalMs;
        }

        public void Start()
        {
            if (running) return;

            running = true;
            thread = new Thread(Run);
            thread.IsBackground = true;  // kein Blockieren beim Schließen
            thread.Start();
        }

        public void Stop()
        {
            running = false;
        }

        private void Run()
        {
            while (running)
            {
                Thread.Sleep(interval);
                Tick?.Invoke(); // Event feuern
                // verkürzte Schreibweise für : Tick != null

            }
        }

    }
}

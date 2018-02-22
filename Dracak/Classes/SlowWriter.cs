using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Dracak
{
    public class SlowWriter
    {
        private DispatcherTimer timer = new DispatcherTimer();
        private Random randInt = new Random();

        public int MsTimeMin { get; set; } = 5;
        public int MsTimeMax { get; set; } = 10;
        public string StoryFull { get; set; }
        public TextBlock Target { get; set; }

        private int Letter { get; set; } = 0;
        private bool Writing { get; set; } = true;
        private string StoryTemp { get; set; }

        public SlowWriter()
        {
            InitTimer();
        }
        public SlowWriter(TextBlock target, string storyFull)
        {
            this.Target = target;
            this.StoryFull = storyFull;
            InitTimer();
        }

        private void InitTimer()
        {
            timer.Interval = new TimeSpan(0, 0, 0, 0, 20);
            timer.Tick += (sender, args) => { WriteIt(); };
        }

        private void WriteIt()
        {
            timer.Interval = new TimeSpan(0, 0, 0, 0, randInt.Next(MsTimeMin, MsTimeMax));

            if (Letter >= StoryFull.Length)
            {
                StopWriting();
            }
            else
            {
                StoryTemp = StoryTemp + StoryFull[Letter];
                Target.Text = StoryTemp;
                Letter++;
            }

        }
        public void StopWriting()
        {
            Target.Text = StoryFull;
            timer.Stop();
            Writing = false;
            StoryTemp = "";
            Letter = 0;
        }
        public void StartWriting()
        {
            timer.Start();
            Writing = true;
        }
        public bool IsWriting()
        {
            return Writing;
        }
    }
}

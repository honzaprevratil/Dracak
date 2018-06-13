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
        public TextBlock Target { get; set; }
        public string StoryFull
        {
            get
            {
                return storyFull;
            }
            set
            {
                StopWriting(false);
                storyFull = value;
                StartWriting();
            }
        }

        private int Letter { get; set; } = 0;
        private bool Writing { get; set; } = true;
        private string StoryTemp { get; set; }
        private string storyFull { get; set; }

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
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            timer.Tick += (sender, args) => { WriteStory(); };
        }

        private void WriteStory()
        {
            //timer.Interval = new TimeSpan(0, 0, 0, 0, randInt.Next(MsTimeMin, MsTimeMax));
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1);

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
        public void StopWriting(bool SetFullStory = true)
        {
            timer.Stop();
            Writing = false;

            Target.Text = SetFullStory ? StoryFull : "";
            ResetValues();
        }
        public void ResetValues()
        {
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

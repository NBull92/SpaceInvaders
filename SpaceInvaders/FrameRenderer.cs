using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;

namespace SpaceInvaders
{
    public class FrameRenderer
    {
        private readonly DispatcherTimer _renderTimer;
        public event EventHandler RenderUpdate = delegate { };
       
        public FrameRenderer()
        {
            _renderTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1.0f / 30.0f)
            };
            _renderTimer.Tick += InvokeUpdate;
        }

        public void Start()
        {
            _renderTimer.Start();
        }

        private void InvokeUpdate(object sender, EventArgs args)
        {
            //Create list of exception
            var exceptions = new List<Exception>();

            //Invoke RenderUpdate Action by iterating on all subscribers event handlers
            foreach (var handler in RenderUpdate.GetInvocationList())
            {
                try
                {
                    //pass sender object and eventArgs
                    handler.DynamicInvoke(this, EventArgs.Empty);
                }
                catch (Exception e)
                {
                    //Add exception in exception list if occured any
                    exceptions.Add(e);
                }
            }

            //Check if any exception occured while 
            //invoking the subscribers event handlers
            if (exceptions.Any())
            {
                //Throw aggregate exception of all exceptions 
                //occured while invoking subscribers event handlers
                throw new AggregateException(exceptions);
            }
        }
    }
}